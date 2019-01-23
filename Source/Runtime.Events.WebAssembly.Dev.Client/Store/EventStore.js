/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import * as storage from '../Storage';

/**
 * Represents the storage for our event store
 */
export class EventStore {
    #storage;

    /**
     * Initializes a new instance of {EventStore}
     */
    constructor() {

        this.#storage = storage;

    }

    /**
     * Load the event store 
     */
    load() {

        return "{}";
    }

    /**
     * 
     * @param {string} json 
     */
    save(json) {
        console.log(`Save Events : {json}`);

    }
}