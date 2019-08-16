/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { DotNetRuntime } from './DotNetRuntime';

 /**
  * Represents the JavaScript runtime callable from .NET WebAssembly
  */
export class JSRuntime {
    #invoker;

    /**
     * Initializes a new instance of {JSRuntime}
     */
    constructor() {
        this.#invoker = new DotNetRuntime('Dolittle.Interaction.WebAssembly.Interop.IJSRuntime');
    }

    /**
     * Perform invocation on JavaScript side
     * @param {string} identifier Fully qualified identifier for the call to make
     * @param {string} argumentsAsJson Arguments as serialized JSON array
     */
    invoke(identifier, argumentsAsJson) {
        //console.log(`Invocation '${identifier}' with '${argumentsAsJson}'`)
        let object = window;
        const path = identifier.split('.');
        const method = path.pop();
        path.forEach(_ => {
            object = object[_];
        });
        return object[method].apply(object, JSON.parse(argumentsAsJson));
    }

    /**
     * Begin invocation on JavaScript side
     * @param {string} invocationId The unique identifier for the invocation
     * @param {string} identifier Fully qualified identifier for the call to make
     * @param {string} argumentsAsJson Arguments as serialized JSON array
     */
    beginInvoke(invocationId, identifier, argumentsAsJson) {
        const result = this.invoke(identifier, argumentsAsJson);

        if( result instanceof Promise ) {
            result.then(e => {
                this.#invoker.invoke("Succeeded",[invocationId, JSON.stringify(e)]);
            }).catch(e => {
                this.#invoker.invoke("Failed",[invocationId, JSON.stringify(e)]);
            });
        } else {
            this.#invoker.invoke("Succeeded",[invocationId, JSON.stringify(result)]);
        }
    }
}

window._jsRuntime = new JSRuntime();