
/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
const invokeMethod = '[Dolittle.Interaction.WebAssembly.Interop] Dolittle.Interaction.WebAssembly.Interop.MethodInvoker:Invoke';
let invokeMethodBinding = null;

/**
 * Represents interop object for a .net CLR type
 */
export class MethodInvoker {
    
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
        if( invokeMethodBinding == null ) {
            invokeMethodBinding = Module.mono_bind_static_method(invokeMethod);
        }
        let serializedArgs = [];
        args.forEach(_ => serializedArgs.push(JSON.stringify(_)));
        let argsAsJson = JSON.stringify(serializedArgs);
        let result = JSON.parse(invokeMethodBinding(this.#type,methodName, argsAsJson));
        if( outputType ) {
            let instance = new outputType();
            for( var property in instance ) {
                if( result.hasOwnProperty(property) ) {
                    instance[property] = result[property];
                }
            }
            return instance;
        }
        
        return result;
    }
}