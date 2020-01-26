// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { IndexedDb } from 'minimongo';

(window as any).CSUUID = function (input: string) {
    return input;
};


/**
 * Represents a wrapper for the MiniMongo collection
 */
export class Collection {

    /**
     * Initializes a new instance of {Collection}
     * @param {string} name The name of the collection
     * @param {IndexedDb} database The underlying database
     */
    constructor(private _name: string, private _database: IndexedDb) {
    }

    /**
     * Upsert a document - this will update or insert
     * @param {any} document - Document to upsert.
     */
    upsert(document: any): Promise<any> {

        const promise = new Promise((resolve, reject) => {
            console.log(`Upsert ${document}`);
            if (document.id) document._id = document.id;

            try {
                (this._database as any)[this._name].upsert(document, () => {
                    console.log('Upserted');
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
    find(selector: any, options: any): Promise<any> {
        const actualSelector = eval(`actualSelector = ${selector}`);

        const promise = new Promise((resolve, reject) => {
            (this._database as any)[this._name].find(actualSelector, options)
                .fetch((result: any) => {
                    resolve(result);
                }, (error: any) => {
                    console.log(`Error ${error}`);
                    reject(error);
                });
        });
        return promise;
    }

    /**
     * Delete one document based on a selector criteria
     * @param {*} selector - Selector to use
     */
    deleteOne(selector: any): Promise<any> {
        const actualSelector = eval(`actualSelector = ${selector}`);
        const promise = new Promise((resolve, reject) => {
            const collection = (this._database as any)[this._name];
            collection.findOne(actualSelector, {}, (result: any) => {
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
