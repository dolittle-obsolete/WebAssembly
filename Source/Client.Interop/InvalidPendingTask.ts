// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/**
 * Exception that gets thrown when a given task with id is not a valid pending task
 */
export class InvalidPendingTask extends Error {

    /**
     * Initializes a new instance of {InvalidPendingTask}
     * @param {string} invocationId - The unique identifier for the invocation
     */
    constructor(invocationId: string) {
        super(`Invocation with id '${invocationId}' is not a pending task`);
    }
}
