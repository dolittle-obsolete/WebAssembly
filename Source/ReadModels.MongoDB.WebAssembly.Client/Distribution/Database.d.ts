/**
 * Represents a wrapper for the MiniMongo database
 */
export declare class Database {
    private _database;
    /**
     * Initializes a new instance of {Database}
     * @param {string} databaseName Name of the database
     */
    constructor(databaseName: string);
    /**
     * Add a collection
     * @param {string} name Name of the collection
     */
    addCollection(name: string): void;
}
