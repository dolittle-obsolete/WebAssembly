/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { BootloaderResult } from './BootloaderResult';

/**
 * Represents the initial bootloader for a Dolittle WebAssembly based system
 */
export class Bootloader {
    
    /**
     * Start the bootloader
     * @param {string} vfsPrefix 
     * @param {string} deployPrefix 
     * @param {string} enableDebugging
     * @param {string[]} files  
     * @returns {Promise} - Resolve will be called with {BootloaderResult}
     */
    start(vfsPrefix, deployPrefix, enableDebugging, files) {
        let promise = new Promise((resolve, reject) => {

            MONO.mono_load_runtime_and_bcl(
                vfsPrefix, 
                deployPrefix,
                enableDebugging,
                files,
                () => {
                    var result = new BootloaderResult();
                    Module.mono_bindings_init("[WebAssembly.Bindings]WebAssembly.Runtime"); 
                    resolve(result);
                }
            )
        });
        return promise;
    }
}