/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Dolittle.Lifecycle;
using Dolittle.Logging;
using Dolittle.Serialization.Json;
using WebAssembly;

namespace Dolittle.Interaction.WebAssembly.Interop
{
    /// <summary>
    /// Represents an implementation of <see cref="IJSRuntime"/>
    /// </summary>
    /// <remarks>
    /// Inspired by https://github.com/dotnet/jsinterop
    /// </remarks>
    [Singleton]
    public class JSRuntime : IJSRuntime
    {
        readonly ConcurrentDictionary<Guid, TaskCompletionSourceWrapper> _pendingTasks = new ConcurrentDictionary<Guid, TaskCompletionSourceWrapper>();
        readonly ISerializer _serializer;
        readonly ILogger _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="logger"></param>
        public JSRuntime(ISerializer serializer, ILogger logger)
        {
            _serializer = serializer;
            _logger = logger;
        }


        /// <inheritdoc/>
        public Task<T> Invoke<T>(string identifier, params object[] arguments)
        {
            var invocationId = Guid.NewGuid();
            var taskCompletionSource = new TaskCompletionSource<T>();
            var taskCompletionSourceWrapper = new TaskCompletionSourceWrapper(typeof(T), taskCompletionSource);
            _pendingTasks[invocationId] = taskCompletionSourceWrapper;

            try
            {
                var serializedArguments = _serializer.ToJson(arguments, SerializationOptions.CamelCase);
                var window = (JSObject)global::WebAssembly.Runtime.GetGlobalObject("window");
                var jsRuntime = (JSObject)window.GetObjectProperty("_jsRuntime");
                jsRuntime.Invoke("beginInvoke", invocationId.ToString(), identifier, serializedArguments);               
            }
            catch( Exception ex )
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
            TaskCompletionSourceWrapper taskCompletionSourceWrapper;
            if(!_pendingTasks.TryRemove(invocationId, out taskCompletionSourceWrapper) ) throw new InvalidPendingTask(invocationId);

            var result = _serializer.FromJson(taskCompletionSourceWrapper.Type, resultAsJson);
            taskCompletionSourceWrapper.SetResult(result);
        }

        /// <inheritdoc/>
        public void Failed(Guid invocationId, string exception)
        {
            TaskCompletionSourceWrapper taskCompletionSourceWrapper;
            if(!_pendingTasks.TryRemove(invocationId, out taskCompletionSourceWrapper) ) throw new InvalidPendingTask(invocationId);
            taskCompletionSourceWrapper.SetException(new JSException(exception));
        }
    }
}