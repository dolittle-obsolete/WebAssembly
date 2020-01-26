// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Dolittle.Lifecycle;
using Dolittle.Logging;
using Dolittle.Serialization.Json;
using WebAssembly;

namespace Dolittle.Interaction.WebAssembly.Interop
{
    /// <summary>
    /// Represents an implementation of <see cref="IJSRuntime"/>.
    /// </summary>
    /// <remarks>
    /// Inspired by https://github.com/dotnet/jsinterop.
    /// </remarks>
    [Singleton]
    public class JSRuntime : IJSRuntime
    {
        readonly ConcurrentDictionary<Guid, TaskCompletionSourceWrapper> _pendingTasks = new ConcurrentDictionary<Guid, TaskCompletionSourceWrapper>();
        readonly ISerializer _serializer;
        readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="JSRuntime"/> class.
        /// </summary>
        /// <param name="serializer">JSON <see cref="ISerializer"/> for serialization.</param>
        /// <param name="logger"><see cref="ILogger"/> for logging.</param>
        public JSRuntime(ISerializer serializer, ILogger logger)
        {
            _serializer = serializer;
            _logger = logger;
        }

        /// <inheritdoc/>
        public void Invoke(string identifier, params object[] arguments)
        {
            var serializedArguments = _serializer.ToJson(arguments, SerializationOptions.CamelCase);
            var jsRuntime = (JSObject)global::WebAssembly.Runtime.GetGlobalObject("_jsRuntime");
            jsRuntime.Invoke("invoke", identifier, serializedArguments);
        }

        /// <inheritdoc/>
        public Task<T> Invoke<T>(string identifier, params object[] arguments)
        {
            var invocationId = Guid.NewGuid();
            var taskCompletionSource = new TaskCompletionSource<T>();
            _pendingTasks[invocationId] = new TaskCompletionSourceWrapper(typeof(T), taskCompletionSource);

            try
            {
                var serializedArguments = _serializer.ToJson(arguments, SerializationOptions.CamelCase);
                _logger.Information($"BeginInvoke '{identifier}' with '{serializedArguments}");
                var jsRuntime = (JSObject)global::WebAssembly.Runtime.GetGlobalObject("_jsRuntime");
                jsRuntime.Invoke("beginInvoke", invocationId.ToString(), identifier, serializedArguments);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error invoking {identifier}");
                _pendingTasks.TryRemove(invocationId, out _);
                taskCompletionSource.SetException(ex);
            }

            return taskCompletionSource.Task;
        }

        /// <inheritdoc/>
        public void Succeeded(Guid invocationId, string resultAsJson)
        {
            if (!_pendingTasks.TryRemove(invocationId, out TaskCompletionSourceWrapper taskCompletionSourceWrapper)) throw new InvalidPendingTask(invocationId);

            if (string.IsNullOrEmpty(resultAsJson))
            {
                taskCompletionSourceWrapper.SetResult(null);
            }
            else
            {
                object result = _serializer.FromJson(taskCompletionSourceWrapper.Type, resultAsJson);
                taskCompletionSourceWrapper.SetResult(result);
            }
        }

        /// <inheritdoc/>
        public void Failed(Guid invocationId, string exception)
        {
            if (!_pendingTasks.TryRemove(invocationId, out TaskCompletionSourceWrapper taskCompletionSourceWrapper)) throw new InvalidPendingTask(invocationId);
            taskCompletionSourceWrapper.SetException(new JSException(exception));
        }
    }
}