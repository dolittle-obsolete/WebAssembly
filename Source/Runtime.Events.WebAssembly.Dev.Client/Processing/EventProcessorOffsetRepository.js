/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { storage } from '../Storage';

const OFFSETS_KEY = 'offsets';

let offsets = [];

/**
 * 
 */
export class EventProcessorOffsetRepository {

    constructor() {
    }

    static preload() {
        let promise = new Promise((resolve, reject) => {
            let store = storage.eventProcessorOffsets;
            let request = store.getAll();
            request.onsuccess = e => {
                offsets = e.target.result ? e.target.result : [];
                resolve(offsets);
            }
        });
        return promise;
    }

    /**
     * 
     */
    load() {
        return offsets;
    }

    /**
     * 
     * @param {string} json 
     */
    save(eventProcessorId, committedEventVersion) {
        let store = storage.eventProcessorOffsets;
        let found = false;
        offsets.forEach(_ => {
            if (_.eventProcessorId == eventProcessorId) {
                _.content = committedEventVersion;
                found = true;
            }
        });
        if (!found) {
            offsets.push({
                eventProcessorId: eventProcessorId,
                content: committedEventVersion,
            });
        }
        store.put({
            eventProcessorId: eventProcessorId,
            content: committedEventVersion,
        });
    }
}