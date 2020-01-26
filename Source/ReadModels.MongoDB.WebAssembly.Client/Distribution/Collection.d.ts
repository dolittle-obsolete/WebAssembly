import { IndexedDb } from 'minimongo';
/**
 * Represents a wrapper for the MiniMongo collection
 */
export declare class Collection {
    private _name;
    private _database;
    /**
     * Initializes a new instance of {Collection}
     * @param {string} name The name of the collection
     * @param {IndexedDb} database The underlying database
     */
    constructor(_name: string, _database: IndexedDb);
    /**
     * Upsert a document - this will update or insert
     * @param {any} document - Document to upsert.
     */
    upsert(document: any): Promise<any>;
    /**
     * Find documents
     * @param {*} selector Selector to use
     * @param {*} options Options to use
     */
    find(selector: any, options: any): Promise<any>;
    /**
     * Delete one document based on a selector criteria
     * @param {*} selector - Selector to use
     */
    deleteOne(selector: any): Promise<any>;
}
