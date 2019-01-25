
/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { Guid } from '@dolittle/core';
import { InvalidPendingTask } from './InvalidPendingTask';

const invokeMethod = '[Dolittle.Interaction.WebAssembly.Interop] Dolittle.Interaction.WebAssembly.Interop.DotNetRuntime:Invoke';
const beginInvokeMethod = '[Dolittle.Interaction.WebAssembly.Interop] Dolittle.Interaction.WebAssembly.Interop.DotNetRuntime:BeginInvoke';

let initialized = false;
let invokeMethodBinding = null;
let beginInvokeMethodBinding = null;


function initializeIfNotInitialized() {
    if (!initialized) {
        invokeMethodBinding = Module.mono_bind_static_method(invokeMethod);
        beginInvokeMethodBinding = Module.mono_bind_static_method(beginInvokeMethod);
        initialized = true;
    }
}

const pendingTasks = {};

function addInvocation(resolve, reject) {
    let invocationId = Guid.create();

    pendingTasks[invocationId] = {
        resolve: resolve,
        reject: reject
    }
    return invocationId;
}

function throwIfInvalidPendingTask(invocationId) {
    if (!pendingTasks.hasOwnProperty(invocationId)) {
        InvalidPendingTask.throw(invocationId);
    }
}
window._dotNetRuntime = {};
window._dotNetRuntime.succeeded = (invocationId, resultAsJson) => {
    throwIfInvalidPendingTask(invocationId);
    pendingTasks[invocationId].resolve(resultAsJson);
};

window._dotNetRuntime.failed = (invocationId, exception) => {
    throwIfInvalidPendingTask(invocationId);
    pendingTasks[invocationId].reject(exception);
};


/**
 * Represents interop object for a .net CLR type
 */
export class DotNetRuntime {
    #type;


    /**
     * Fully qualified name of the type the interop represents
     * @param {string} type 
     */
    constructor(type) {
        this.#type = type;
    }

    /**
     * Invoke a method on the CLR type the interop represents
     * @param {string} methodName Name of the CLR method
     * @param {Any[]} args Arguments passed to the CLR method
     * @param {Class|Function} [outputType] - Type of the output type - Optional
     * @returns {Any} Result from the CLR method
     */
    invoke(methodName, args, outputType) {
        //console.log(`C#Invoke '${this.#type}'.'${methodName}' with '${args}'`);
        initializeIfNotInitialized();

        let argsAsJson = this.#serializeArguments(args);
        let resultAsJson = invokeMethodBinding(this.#type, methodName, argsAsJson);
        let result = this.#deserializeResult(resultAsJson, outputType);
        return result;
    }

    /**
    * Invoke a method on the CLR type the interop represents
    * @param {string} methodName Name of the CLR method
    * @param {Any[]} args Arguments passed to the CLR method
    * @param {Class|Function} [outputType] - Type of the output type - Optional
    * @returns {Promise} Promise for continuation
    */
    beginInvoke(methodName, args, outputType) {
        //console.log(`C#BeginInvoke '${this.#type}'.'${methodName}' with '${args}'`);
        initializeIfNotInitialized();

        let argsAsJson = this.#serializeArguments(args);
        let promise = new Promise((resolve, reject) => {
            let continuationId = addInvocation((resultAsJson) => {
                let result = this.#deserializeResult(resultAsJson, outputType);
                resolve(result);
            }, (exception) => {
                reject(exception);
            });
            beginInvokeMethodBinding(continuationId, this.#type, methodName, argsAsJson);
        });
        return promise;
    }

    #serializeArguments(args) {
        return JSON.stringify(args);
    }

    #deserializeResult(resultAsJson, outputType) {
        return JSON.parse(resultAsJson);
    }
}