// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { storage } from '../Storage';

const COMMITS_KEY = 'commits';

let commits: any[] = [];

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
     * Load the event store.
     */
    load() {
        return commits;
    }

    static preload() {
        const promise = new Promise((resolve, reject) => {
            const store = storage.commits;
            const request = store?.get(COMMITS_KEY);
            if (request) {
                request.onsuccess = e => {
                    commits = (e.target as any).result ? (e.target as any).result.content : [];
                    resolve(commits);
                };
            }
        });
        return promise;
    }

    /**
     * Save event.
     * @param {string} json - Event as JSON
     */
    save(json: string) {
        commits = JSON.parse(json);
        const obj = {
            purpose: COMMITS_KEY,
            content: commits
        };
        const store = storage.commits;
        store?.put(obj);
    }
}
