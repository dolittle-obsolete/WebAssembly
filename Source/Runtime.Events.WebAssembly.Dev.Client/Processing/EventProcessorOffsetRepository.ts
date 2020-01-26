// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { storage } from '../Storage';

const OFFSETS_KEY = 'offsets';

let offsets: any[] = [];

/**
 * Represents the storage of event processor offsets.
 */
export class EventProcessorOffsetRepository {

    constructor() {
    }

    static preload() {
        const promise = new Promise((resolve, reject) => {
            const store = storage.eventProcessorOffsets;
            const request = store?.getAll();
            if (request) {
                request.onsuccess = (e: any) => {
                    offsets = e.target.result ? e.target.result : [];
                    resolve(offsets);
                };
            }
        });
        return promise;
    }

    /**
     * Load
     */
    load() {
        return offsets;
    }

    /**
     * Save the committed event version for an event processor with a given id.
     */
    save(eventProcessorId: string, committedEventVersion: any) {
        const store = storage.eventProcessorOffsets;
        let found = false;
        offsets.forEach(_ => {
            if (_.eventProcessorId === eventProcessorId) {
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
        store?.put({
            eventProcessorId: eventProcessorId,
            content: committedEventVersion,
        });
    }
}
