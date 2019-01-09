/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { CommandCoordinator } from '@dolittle/commands';
import { CommandCoordinator as WASMCommandCoordinator } from '@dolittle/webassembly/commands';
import { QueryCoordinator } from '@dolittle/queries';
import { QueryCoordinator as WASMQueryCoordinator } from '@dolittle/webassembly/queries';

export function configure(aurelia) {
    aurelia.container.registerInstance(CommandCoordinator, new WASMCommandCoordinator());
    aurelia.container.registerInstance(QueryCoordinator, new WASMQueryCoordinator());
}