import { Database } from './Database';
/**
 * Represents a wrapper for working with MiniMongo
 */
export declare class MongoDB {
    private _database;
    /**
     * Initializes MongoDB in the browser
     * @param {string} databaseName Name of the database
     */
    initialize(databaseName: string): void;
    get database(): Database | undefined;
}
