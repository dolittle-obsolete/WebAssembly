/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { CommandCoordinator } from '@dolittle/commands';
import { CommandCoordinator as WASMCommandCoordinator } from '@dolittle/webassembly/commands';
import { QueryCoordinator } from '@dolittle/queries';
import { QueryCoordinator as WASMQueryCoordinator } from '@dolittle/webassembly/queries';


export function configure(aurelia, config) {
    aurelia.container.registerInstance(CommandCoordinator, new WASMCommandCoordinator());
    aurelia.container.registerInstance(QueryCoordinator, new WASMQueryCoordinator());

    config = config || {};
    config.entryPoint = config.entryPoint || '';
    config.assemblies = config.assemblies || [];
    config.monoScript = config.monoScript || 'mono.js';
    config.offline = config.offline || false;

    if (config.entryPoint == '') throw "Missing entrypoint in Dolittle WebAssembly plugin configuration";
    if (config.assemblies.length == 0) throw "Missing assemblies in Dolittle WebAssembly plugin configuration";

    console.log(`Using '${config.entryPoint}' as entrypoint for WebAssembly `)

    window.Module = {};

    window.Module.onRuntimeInitialized = () => {
        MONO.mono_load_runtime_and_bcl(
            'managed',
            'managed',
            1,
            config.assemblies,
            () => {
                Module.mono_bindings_init("[WebAssembly.Bindings]WebAssembly.Runtime");
                BINDING.call_static_method(config.entryPoint, []);
            }
        );
    };

    let monoScript = document.createElement('script');
    monoScript.async = true;
    monoScript.src = config.monoScript;
    document.body.appendChild(monoScript);

    if (config.offline === true) {
        navigator.serviceWorker.register('service-worker.js');
    }
}