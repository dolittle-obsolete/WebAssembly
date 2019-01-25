/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { storage } from '../Storage';

const COMMITS_KEY = 'commits';

let commits = [];

/**
 * Represents the storage for our event store
 */
export class EventStore {

    /**
     * Initializes a new instance of {EventStore}
     */
    constructor() {
    }

    /**
     * Load the event store 
     */
    load() {
        return commits;
    }

    static preload() {       
        let promise = new Promise((resolve, reject) => {
            let store = storage.commits;
            let request = store.get(COMMITS_KEY);
            request.onsuccess = e => {
                commits = e.target.result ? e.target.result.content : [];
                resolve(commits);
            }
        });
        return promise;
    }

    /**
     * 
     * @param {string} json 
     */
    save(json) {
        commits = JSON.parse(json);
        let obj = {
            purpose: COMMITS_KEY,
            content: commits
        };
        let store = storage.commits;
        store.put(obj);
    }

}