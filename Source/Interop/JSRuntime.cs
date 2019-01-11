/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Dolittle.Lifecycle;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serializer"></param>
        public JSRuntime(ISerializer serializer)
        {
            _serializer = serializer;
        }

        /// <inheritdoc/>
        public Task<T> Invoke<T>(string identifier, params object[] arguments)
        {
            var taskId = Guid.NewGuid();
            var taskCompletionSource = new TaskCompletionSource<T>();
            var taskCompletionSourceWrapper = new TaskCompletionSourceWrapper(typeof(T), taskCompletionSource);
            _pendingTasks[taskId] = taskCompletionSourceWrapper;

            try
            {
                var serializedArguments = _serializer.ToJson(arguments, SerializationOptions.CamelCase);
                var window = (JSObject)global::WebAssembly.Runtime.GetGlobalObject("window");
                var jsRuntime = (JSObject)window.GetObjectProperty("_jsRuntime");
                jsRuntime.Invoke("beginInvoke", taskId.ToString(), identifier, serializedArguments);
                return taskCompletionSource.Task;
            }
            catch
            {
                _pendingTasks.TryRemove(taskId, out _);
                throw;
            }
        }


        /// <inheritdoc/>
        public void EndInvoke(Guid taskId, bool success, string resultOrException)
        {
            TaskCompletionSourceWrapper taskCompletionSourceWrapper;
            if(!_pendingTasks.TryRemove(taskId, out taskCompletionSourceWrapper) ) throw new InvalidPendingTask(taskId);

            if( success )
            {
                var result = _serializer.FromJson(taskCompletionSourceWrapper.Type, resultOrException);
                taskCompletionSourceWrapper.SetResult(result);
            } else
            {
                taskCompletionSourceWrapper.SetException(new JSException(resultOrException));
            }
        }
    }
}