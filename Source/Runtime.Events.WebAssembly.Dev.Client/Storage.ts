// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

const COMMITS_OBJECT_STORE = 'commits';
const EVENT_PROCESSOR_OFFSETS_OBJECT_STORE = 'eventProcessorOffsets';

/**
 * Represents the underlying storage for the event store.
 */
class Storage {
    private _database: IDBDatabase | undefined;
    private _promise: Promise<IDBDatabase>;

    constructor() {
        this._promise = new Promise<IDBDatabase>((resolve, reject) => {
            const request = indexedDB.open('EventStore', 1);

            request.onupgradeneeded = e => {
                const database = (e.target as any).result as IDBDatabase;
                database.createObjectStore(COMMITS_OBJECT_STORE, { keyPath: 'purpose' });
                database.createObjectStore(EVENT_PROCESSOR_OFFSETS_OBJECT_STORE, { keyPath: 'eventProcessorId' });
            };

            request.onsuccess = e => {
                console.log('Database preloaded');
                this._database = (e.target as any).result as IDBDatabase;
                resolve(this._database);
            };

            request.onerror = e => {
                console.log(`Error during initializing IndexedDB - ${e}`);
                reject(e);
            };
        });
    }

    preload() {
        return this._promise;
    }

    get database() {
        return this._database;
    }

    get commits() {
        return this.getObjectStore(COMMITS_OBJECT_STORE);
    }

    get eventProcessorOffsets() {
        return this.getObjectStore(EVENT_PROCESSOR_OFFSETS_OBJECT_STORE);
    }

    private getObjectStore(store: string): IDBObjectStore | undefined {
        console.log('Asking for transaction', this._database);
        const transaction = this._database?.transaction(store, 'readwrite');
        return transaction?.objectStore(store);
    }
}

const storage = new Storage();
export { storage };
