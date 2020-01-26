// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Threading.Tasks;

namespace Dolittle.Interaction.WebAssembly.Interop
{
    /// <summary>
    /// Represents a wrapper for <see cref="TaskCompletionSource{T}"/>.
    /// </summary>
    public class TaskCompletionSourceWrapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskCompletionSourceWrapper"/> class.
        /// </summary>
        /// <param name="type">Type argument for the original.</param>
        /// <param name="instance">The original instance.</param>
        public TaskCompletionSourceWrapper(Type type, object instance)
        {
            Type = type;
            Instance = instance;
        }

        /// <summary>
        /// Gets the generic type argument for the original <see cref="TaskCompletionSource{T}"/>.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Gets the original <see cref="TaskCompletionSource{T}"/>.
        /// </summary>
        public object Instance { get; }

        /// <summary>
        /// Set exception on <see cref="TaskCompletionSource{T}"/> instance.
        /// </summary>
        /// <param name="exception"><see cref="Exception"/> to set.</param>
        public void SetException(Exception exception)
        {
            var setExceptionMethods = typeof(TaskCompletionSource<>).MakeGenericType(Type).GetMethods().Where(_ => _.Name == "SetException");
            var method = setExceptionMethods.SingleOrDefault(_ => _.GetParameters()[0].ParameterType == typeof(Exception));
            method?.Invoke(Instance, new[] { exception });
        }

        /// <summary>
        /// Set result on <see cref="TaskCompletionSource{T}"/> instance.
        /// </summary>
        /// <param name="result">The result to set.</param>
        public void SetResult(object result)
        {
            typeof(TaskCompletionSource<>).MakeGenericType(Type).GetMethod("SetResult").Invoke(Instance, new[] { result });
        }

        /// <summary>
        /// Set cancelled on <see cref="TaskCompletionSource{T}"/> instance.
        /// </summary>
        public void SetCancelled()
        {
            typeof(TaskCompletionSource<>).MakeGenericType(Type).GetMethod("SetCancelled").Invoke(Instance, Array.Empty<object>());
        }
    }
}