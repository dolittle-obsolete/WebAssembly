/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { storage } from '../Storage';

/**
 * 
 */
export class EventProcessorOffsetRepository {
    #storage;

    constructor() {

        this.#storage = storage;

    }

    get storage() {
        return this.#storage;
    }

}