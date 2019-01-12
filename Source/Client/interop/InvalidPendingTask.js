/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { Exception } from '@dolittle/exceptions';

/**
 * Exception that gets thrown when a given task with id is not a valid pending task
 */
export class InvalidPendingTask extends Exception {

    /**
     * Inializes a new instance of {InvalidPendingTask}
     * @param {string} invocationId 
     */
    constructor(invocationId) {
        super(`Invocation with id '${invocationId}' is not a pending task`)
    }
}