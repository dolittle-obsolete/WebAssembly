/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

const COMMITS_OBJECT_STORE = "commits";
const EVENT_PROCESSOR_OFFSETS_OBJECT_STORE = "eventProcessorOffsets";

/**
 * 
 */
class Storage {
    #database;
    #commits;
    #eventProcessorOffsets;

    constructor() {
        let request = indexedDB.open('EventStore', 1);

        request.onupgradeneeded = e => {
            let database = e.target.result;
            database.createObjectStore(COMMITS_OBJECT_STORE, { keyPath: 'purpose' });
            database.createObjectStore(EVENT_PROCESSOR_OFFSETS_OBJECT_STORE, { keyPath: 'eventProcessorId' });
        }

        request.onsuccess = e => {
            this.#database = e.target.result;

        }

        request.onerror = e => {
            console.log(`Error during initializing IndexedDB - ${e}`);
        };
    }

    get database() {
        return this.#database;
    }

    get commits() {
        return this.#getObjectStore(COMMITS_OBJECT_STORE);
    }

    get eventProcessorOffsets() {
        return this.#getObjectStore(EVENT_PROCESSOR_OFFSETS_OBJECT_STORE);
    }

    #getObjectStore(store) {
        let transaction = this.#database.transaction(store, 'readwrite');
        return transaction.objectStore(store);
    }
}

let storage = new Storage();
export { storage };