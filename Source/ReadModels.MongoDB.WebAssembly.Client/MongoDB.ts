// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Database } from './Database';

/**
 * Represents a wrapper for working with MiniMongo
 */
export class MongoDB {
    private _database: Database | undefined;

    /**
     * Initializes MongoDB in the browser
     * @param {string} databaseName Name of the database
     */
    initialize(databaseName: string) {
        console.log(`Initialize MongoDB with database '${databaseName}'`);
        this._database = new Database(databaseName);
    }

    get database() {
        return this._database;
    }
}
