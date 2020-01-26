// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Dolittle.Interaction.WebAssembly.Interop
{
    /// <summary>
    /// Exception that gets thrown when a given task with id is not a valid pending task.
    /// </summary>
    public class InvalidPendingTask : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPendingTask"/> class.
        /// </summary>
        /// <param name="invocationId">Id of task that is invalid.</param>
        public InvalidPendingTask(Guid invocationId)
            : base($"Invocation with id '{invocationId}' is not a pending task")
        {
        }
    }
}