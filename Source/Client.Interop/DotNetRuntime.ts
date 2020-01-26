// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Guid } from '@dolittle/rudiments';
import { InvalidPendingTask } from './InvalidPendingTask';

const invokeMethod = '[Dolittle.Interaction.WebAssembly.Interop] Dolittle.Interaction.WebAssembly.Interop.DotNetRuntime:Invoke';
const beginInvokeMethod = '[Dolittle.Interaction.WebAssembly.Interop] Dolittle.Interaction.WebAssembly.Interop.DotNetRuntime:BeginInvoke';

let initialized = false;
let invokeMethodBinding: Function;
let beginInvokeMethodBinding: Function;

declare var Module: any;


function initializeIfNotInitialized() {
    if (!initialized) {
        invokeMethodBinding = Module.mono_bind_static_method(invokeMethod);
        beginInvokeMethodBinding = Module.mono_bind_static_method(beginInvokeMethod);
        initialized = true;
    }
}

const pendingTasks: any = {};

function addInvocation(resolve: Function, reject: Function) {
    const invocationId = Guid.create().toString();

    pendingTasks[invocationId] = {
        resolve: resolve,
        reject: reject
    };

    return invocationId;
}

function throwIfInvalidPendingTask(invocationId: string) {
    if (!pendingTasks.hasOwnProperty(invocationId)) {
        throw new InvalidPendingTask(invocationId);
    }
}
(window as any)._dotNetRuntime = {};
(window as any)._dotNetRuntime.succeeded = (invocationId: string, resultAsJson: string) => {
    throwIfInvalidPendingTask(invocationId);
    pendingTasks[invocationId].resolve(resultAsJson);
};

(window as any)._dotNetRuntime.failed = (invocationId: string, exception: any) => {
    throwIfInvalidPendingTask(invocationId);
    pendingTasks[invocationId].reject(exception);
};


/**
 * Represents interop object for a .net CLR type
 */
export class DotNetRuntime {
    /**
     * Fully qualified name of the type the interop represents
     * @param {string} type - Type the interop represents.
     */
    constructor(private _type: string) {
    }

    /**
     * Invoke a method on the CLR type the interop represents
     * @param {string} methodName Name of the CLR method
     * @param {any[]} args Arguments passed to the CLR method
     * @returns {Any} Result from the CLR method
     */
    invoke(methodName: string, args: any[]): any {
        //console.log(`C#Invoke '${this.#type}'.'${methodName}' with '${args}'`);
        initializeIfNotInitialized();

        const argsAsJson = this.serializeArguments(args);
        const resultAsJson = invokeMethodBinding(this._type, methodName, argsAsJson);
        const result = this.deserializeResult(resultAsJson);
        return result;
    }

    /**
     * Invoke a method on the CLR type the interop represents
     * @param {string} methodName Name of the CLR method
     * @param {Any[]} args Arguments passed to the CLR method
     * @returns {Promise} Promise for continuation
     */
    beginInvoke(methodName: string, args: any[]): Promise<any> {
        //console.log(`C#BeginInvoke '${this.#type}'.'${methodName}' with '${args}'`);
        initializeIfNotInitialized();

        const argsAsJson = this.serializeArguments(args);
        const promise = new Promise((resolve, reject) => {
            const continuationId = addInvocation((resultAsJson: string) => {
                const result = this.deserializeResult(resultAsJson);
                resolve(result);
            }, (exception: any) => {
                reject(exception);
            });
            beginInvokeMethodBinding(continuationId, this._type, methodName, argsAsJson);
        });
        return promise;
    }

    private serializeArguments(args: any[]) {
        return JSON.stringify(args);
    }

    private deserializeResult(resultAsJson: string) {
        return JSON.parse(resultAsJson);
    }
}
