/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { IndexedDb } from 'minimongo';
import { Collection } from './Collection';

/**
 * Represents a wrapper for the MiniMongo database
 */
export class Database {
    #database;

    /**
     * Initializes a new instance of {Database}
     * @param {string} databaseName Name of the database
     */
    constructor(databaseName) {
        this.#database = new IndexedDb({
            namespace: databaseName
        });
    }

    /**
     * Add a collection
     * @param {string} name Name of the collection
     */
    addCollection(name) {
        this[name] = new Collection(this.#database);

        this.#database.addCollection(name, () => {

        });
    }
}