/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { storage } from '../Storage';

const COMMITS_KEY = 'commits';

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
        let promise = new Promise((resolve, reject) => {
            let store = this.#storage.commits;
            let request = store.get(COMMITS_KEY);
            request.onsuccess = e => {
                resolve(e.target.result ? e.target.result.content : []);
            }
        });
        return promise;
    }

    /**
     * 
     * @param {string} json 
     */
    save(json) {
        let commits = JSON.parse(json);
        let obj = {
            purpose: COMMITS_KEY,
            content: commits
        };
        let store = this.#storage.commits;
        store.put(obj);
    }

    getSequenceNumber() {

    }

    setSequenceNumber(sequenceNumber) {

    }

}