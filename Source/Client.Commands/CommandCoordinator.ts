// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ICommand, CommandRequest } from '@dolittle/commands';
import { DotNetRuntime } from '@dolittle/webassembly.interop/DotNetRuntime';

/**
 * Represents the coordinator of a {Command} for in-process WebAssembly usage
 */
export class CommandCoordinator {
    private _interop: DotNetRuntime;

    /**
     * Initializes a new instance of {CommandCoordinator}
     */
    constructor() {
        this._interop = new DotNetRuntime('Dolittle.Interaction.WebAssembly.Commands.CommandCoordinator');
    }

    /**
     * Handle an {ICommand}
     * @param {ICommand} command - Command to handle
     * @returns {Promise} Promise with eventually the result of the command
     */
    handle(command: ICommand) {
        const promise = new Promise((resolve, reject) => {
            const request = CommandRequest.createFrom(command);
            const result = this._interop.invoke('Handle', [request]);
            resolve(result);
        });
        return promise;
    }
}
