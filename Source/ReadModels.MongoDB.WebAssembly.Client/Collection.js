/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { IndexedDb } from 'minimongo';

window.CSUUID = function(input) {
    return input;
};


/**
 * Represents a wrapper for the MiniMongo collection
 */
export class Collection {
    #name;
    #database;

    /**
     * Initializes a new instance of {Collection}
     * @param {string} name The name of the collection
     * @param {IndexedDb} database The underlying database
     */
    constructor(name, database) {
        this.#name = name;
        this.#database = database;
    }

    /**
     * Upsert a document - this will update or insert
     * @param {Any} document 
     */
    upsert(document) {

        let promise = new Promise((resolve, reject) => {
            console.log(`Upsert ${document}`);
            if( document.id ) document._id = document.id;

            try {
                this.#database[this.#name].upsert(document, () => {
                    console.log(`Upserted`);
                    resolve();
                });
            } catch (ex) {
                console.log(`Exception ${ex}`);
                reject(ex);
            }
        });
        return promise;
    }


    /**
     * Find documents
     * @param {*} selector Selector to use
     * @param {*} options Options to use
     */
    find(selector, options) {
        let actualSelector = eval(`actualSelector = ${selector}`);
        
        let promise = new Promise((resolve, reject) => {
            this.#database[this.#name].find(actualSelector, options)
                .fetch(result => {
                    resolve(result);
                }, error => {
                    console.log(`Error ${error}`);
                    reject(error);
                });
        });
        return promise;
    }

    /**
     * Delete one document based on a selector criteria
     * @param {*} selector 
     */
    deleteOne(selector) {
        let actualSelector = eval(`actualSelector = ${selector}`);
        let promise = new Promise((resolve, reject) => {
            let collection = this.#database[this.#name];
            collection.findOne(actualSelector, {}, result => {
                collection.remove(result._id, () => {
                    collection.resolveRemove(result._id, () => {
                        resolve();
                    }, () => reject());                       
                }, () => reject());
            }, () => reject());
        });
        return promise;
    }
}