/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { storage } from '../Storage';

const OFFSETS_KEY = 'offsets';

/**
 * 
 */
export class EventProcessorOffsetRepository {
    #storage;

    constructor() {
        this.#storage = storage;
    }

    /**
     * 
     */
    load() {
        let promise = new Promise((resolve, reject) => {
            let store = this.#storage.eventProcessorOffsets;
            let request = store.getAll();
            request.onsuccess = e => {
                resolve(e.target.result ? e.target.result : []);
            }
        });
        return promise;
    }

    /**
     * 
     * @param {string} json 
     */
    save(eventProcessorId, committedEventVersion) {
        let store = this.#storage.eventProcessorOffsets;
        store.put({
            eventProcessorId: eventProcessorId,
            content: committedEventVersion,
        });
    }
}