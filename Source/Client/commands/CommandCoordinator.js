/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { Command, CommandRequest } from '@dolittle/commands';
import { DotNetRuntime } from '../interop/DotNetRuntime';


/**
* Represents the coordinator of a {Command} for in-process WebAssembly usage
*/
export class CommandCoordinator {
    #interop;

    /**
     * Initializes a new instance of {CommandCoordinator}
     */
    constructor() {
        this.#interop = new DotNetRuntime("Dolittle.Interaction.WebAssembly.Commands.CommandCoordinator");
    }
 
    /**
     * Handle a {Command}
     * @param {Command} command 
     * @returns {Promise} Promise with eventually the result of the command
     */
    handle(command) {
        var promise = new Promise((resolve, reject) => {
            let request = CommandRequest.createFrom(command);
            let result = this.#interop.invoke("Handle", [request]);
            resolve(result);
        });
        return promise;
    }
}