/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Threading.Tasks;

namespace Dolittle.Interaction.WebAssembly.Interop
{
    /// <summary>
    /// Represents the JavaScript runtime
    /// </summary>
    public interface IJSRuntime
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="arguments"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T> Invoke<T>(string identifier, params object[] arguments);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="success"></param>
        /// <param name="resultOrException"></param>
        void EndInvoke(Guid taskId, bool success, string resultOrException);
    }
}