// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { IndexedDb } from 'minimongo';
import { Collection } from './Collection';

/**
 * Represents a wrapper for the MiniMongo database
 */
export class Database {
    private _database: IndexedDb;

    /**
     * Initializes a new instance of {Database}
     * @param {string} databaseName Name of the database
     */
    constructor(databaseName: string) {
        this._database = new IndexedDb({
            namespace: databaseName
        }, () => {
            console.log(`Database ${databaseName} is ready`);
        }, () => {
            console.log(`Database ${databaseName} failed to start - ${arguments}`);
        });
    }

    /**
     * Add a collection
     * @param {string} name Name of the collection
     */
    addCollection(name: string) {
        console.log(`Add collection ${name}`);
        (this as any)[name] = new Collection(name, this._database);

        this._database.addCollection(name, () => {
            console.log(`Collection ${name} added`);
        });
    }
}
