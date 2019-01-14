/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import * as minimongo from 'minimongo';

/**
 * Represents a wrapper for working with MiniMongo
 */
export class MongoDB {

    /**
     * Initializes MongoDB in the browser
     * @param {string} databaseName Name of the database
     */
    initialize(databaseName) {
        console.log(`Initialize MongoDB with database '${databaseName}'`);
        let IndexedDb = minimongo.IndexedDb;
        this.database = new IndexedDb({
            namespace: databaseName
        });
    }
}