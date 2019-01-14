/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { IndexedDb } from 'minimongo';

/**
 * Represents a wrapper for the MiniMongo collection
 */
export class Collection {
    #database;

    /**
     * Initializes a new instance of {Collection}
     * @param {IndexedDb} database The underlying database
     */
    constructor(database) {
        this.#database = database;
    }

    /**
     * Upsert a document - this will update or insert
     * @param {Any} document 
     */
    upsert(document) {
        let promise = new Promise((resolve, reject) => {
            this.#database.upsert(document, () => {
                resolve();
            });
        });
        return promise;
    }


}