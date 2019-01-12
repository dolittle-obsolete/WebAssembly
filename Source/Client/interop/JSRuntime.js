import { invoker } from "aurelia-framework";

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
     * Begin invocation on JavaScript side
     * @param {string} invocationId The unique identifier for the invocation
     * @param {string} identifier Fully qualified identifier for the call to make
     * @param {string} argumentsAsJson Arguments as serialized JSON array
     */
    beginInvoke(invocationId, identifier, argumentsAsJson) {
        let call = `${identifier}(${argumentsAsJson.substr(1,argumentsAsJson.length-2)})`;
        let result = eval(call);
        
        if( result instanceof Promise ) {
            result.then(e => {
                this.#invoker.invoke("Succeeded",[invocationId, e]);
            }).catch(e => {
                this.#invoker.invoke("Failed",[invocationId, e]);
            });
        } else {
            this.#invoker.invoke("Succeeded",[invocationId, result]);
        }
    }
}

window._jsRuntime = new JSRuntime();