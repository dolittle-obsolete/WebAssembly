/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Runtime.Serialization;

namespace Dolittle.Interaction.WebAssembly.Interop
{
    /// <summary>
    /// Exception that gets thrown when a given task with id is not a valid pending task
    /// </summary>
    public class InvalidPendingTask : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="InvalidPendingTask"/>
        /// </summary>
        /// <param name="taskId">Id of task that is invalid</param>
        public InvalidPendingTask(Guid taskId) : base($"Task with id '{taskId}' is not a pending task")
        {
        }
    }
}