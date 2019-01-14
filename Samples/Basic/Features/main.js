/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import environment from './environment';
import { PLATFORM } from 'aurelia-pal';
import * as Bluebird from 'bluebird';

import * as assemblies from '../publish/assemblies.json';

// remove out if you don't want a Promise polyfill (remove also from webpack.config.js)
Bluebird.config({ warnings: { wForgottenReturn: false } });

export function configure(aurelia) {
    aurelia.use
        .standardConfiguration()
        .plugin(PLATFORM.moduleName('@dolittle/webassembly.aurelia'), {
            entryPoint: "[Basic] Basic.Program:Main",
            assemblies: assemblies.default,
            offline: false
        });

    if (environment.debug) {
        aurelia.use.developmentLogging();
    }
    aurelia.start().then(() => aurelia.setRoot(PLATFORM.moduleName('app')));
}


//import SQLiteWasm from './sqlitenet';
//import { Bootloader, BootProcedures, BootProcedure } from '@dolittle/booting';
//import { Bootloader, BootloaderResult } from '@dolittle/booting';

    // https://github.com/Microsoft/TypeScript/wiki/JavaScript-Language-Service-in-Visual-Studio#Rich
    // https://code.visualstudio.com/docs/languages/javascript#_javascript-projects-jsconfigjson
    // https://github.com/Microsoft/TypeScript/wiki/JsDoc-support-in-JavaScript

    // export * from ...  does not really work with intellisense
    // Semicolon confusion - function should be closed, but gets put after a JSDoc declaration for the next function
    // _proto - Don't do intermediate step : https://github.com/babel/babel/issues/4840
    // Don't include JavaScript other than the dist folder - it gets confused
    // JSDoc for properties - should not be on the defineProperty level, but on the actual functions
    // Types on JSDoc are referenced wrong - e.g. BootProcedure in BootProcedures should be _BootProcedure.BootProcedure
    // Properties are screwed up - not coming out as proper Object.defineProperty, but as (0, _createClass...)

    // 1. Strip JSDoc comments - associate them with class, function and property
    // 2. Rewrite proto aliasing
    // 3. Rewrite property assignments
    // 4. Put JSDoc comments back into the right place - property get/set - param/returns
    // 4a. Process JSDoc comments to link to correct type - potentially use the @typedef option - inserted at the top
    // 5. Generate index.js - merge with existing - remove duplicates
    // 6. extract @callback / @type / @typedef definition JSDoc and put in where its appropriate
