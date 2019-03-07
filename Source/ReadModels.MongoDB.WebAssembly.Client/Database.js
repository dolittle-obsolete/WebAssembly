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
        }, () => {
            console.log(`Database ${databaseName} is ready`);
        });
    }

    /**
     * Add a collection
     * @param {string} name Name of the collection
     */
    addCollection(name) {
        console.log(`Add collection ${name}`);
        this[name] = new Collection(name, this.#database);

        this.#database.addCollection(name, () => {
            console.log(`Collection ${name} added`)
        });
    }
}