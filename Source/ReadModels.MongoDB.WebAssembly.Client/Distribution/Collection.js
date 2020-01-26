"use strict";
// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
Object.defineProperty(exports, "__esModule", { value: true });
window.CSUUID = function (input) {
    return input;
};
/**
 * Represents a wrapper for the MiniMongo collection
 */
class Collection {
    /**
     * Initializes a new instance of {Collection}
     * @param {string} name The name of the collection
     * @param {IndexedDb} database The underlying database
     */
    constructor(_name, _database) {
        this._name = _name;
        this._database = _database;
    }
    /**
     * Upsert a document - this will update or insert
     * @param {any} document - Document to upsert.
     */
    upsert(document) {
        const promise = new Promise((resolve, reject) => {
            console.log(`Upsert ${document}`);
            if (document.id)
                document._id = document.id;
            try {
                this._database[this._name].upsert(document, () => {
                    console.log('Upserted');
                    resolve();
                });
            }
            catch (ex) {
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
        const actualSelector = eval(`actualSelector = ${selector}`);
        const promise = new Promise((resolve, reject) => {
            this._database[this._name].find(actualSelector, options)
                .fetch((result) => {
                resolve(result);
            }, (error) => {
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
    deleteOne(selector) {
        const actualSelector = eval(`actualSelector = ${selector}`);
        const promise = new Promise((resolve, reject) => {
            const collection = this._database[this._name];
            collection.findOne(actualSelector, {}, (result) => {
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
exports.Collection = Collection;

//# sourceMappingURL=data:application/json;charset=utf8;base64,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LEVBQUUsR0FBRyxFQUFFLENBQUMsTUFBTSxFQUFFLENBQUMsQ0FBQztnQkFDdkIsQ0FBQyxFQUFFLEdBQUcsRUFBRSxDQUFDLE1BQU0sRUFBRSxDQUFDLENBQUM7WUFDdkIsQ0FBQyxFQUFFLEdBQUcsRUFBRSxDQUFDLE1BQU0sRUFBRSxDQUFDLENBQUM7UUFDdkIsQ0FBQyxDQUFDLENBQUM7UUFDSCxPQUFPLE9BQU8sQ0FBQztJQUNuQixDQUFDO0NBQ0o7QUF4RUQsZ0NBd0VDIiwiZmlsZSI6IkNvbGxlY3Rpb24uanMiLCJzb3VyY2VzQ29udGVudCI6WyIvLyBDb3B5cmlnaHQgKGMpIERvbGl0dGxlLiBBbGwgcmlnaHRzIHJlc2VydmVkLlxuLy8gTGljZW5zZWQgdW5kZXIgdGhlIE1JVCBsaWNlbnNlLiBTZWUgTElDRU5TRSBmaWxlIGluIHRoZSBwcm9qZWN0IHJvb3QgZm9yIGZ1bGwgbGljZW5zZSBpbmZvcm1hdGlvbi5cblxuaW1wb3J0IHsgSW5kZXhlZERiIH0gZnJvbSAnbWluaW1vbmdvJztcblxuKHdpbmRvdyBhcyBhbnkpLkNTVVVJRCA9IGZ1bmN0aW9uIChpbnB1dDogc3RyaW5nKSB7XG4gICAgcmV0dXJuIGlucHV0O1xufTtcblxuXG4vKipcbiAqIFJlcHJlc2VudHMgYSB3cmFwcGVyIGZvciB0aGUgTWluaU1vbmdvIGNvbGxlY3Rpb25cbiAqL1xuZXhwb3J0IGNsYXNzIENvbGxlY3Rpb24ge1xuXG4gICAgLyoqXG4gICAgICogSW5pdGlhbGl6ZXMgYSBuZXcgaW5zdGFuY2Ugb2Yge0NvbGxlY3Rpb259XG4gICAgICogQHBhcmFtIHtzdHJpbmd9IG5hbWUgVGhlIG5hbWUgb2YgdGhlIGNvbGxlY3Rpb25cbiAgICAgKiBAcGFyYW0ge0luZGV4ZWREYn0gZGF0YWJhc2UgVGhlIHVuZGVybHlpbmcgZGF0YWJhc2VcbiAgICAgKi9cbiAgICBjb25zdHJ1Y3Rvcihwcml2YXRlIF9uYW1lOiBzdHJpbmcsIHByaXZhdGUgX2RhdGFiYXNlOiBJbmRleGVkRGIpIHtcbiAgICB9XG5cbiAgICAvKipcbiAgICAgKiBVcHNlcnQgYSBkb2N1bWVudCAtIHRoaXMgd2lsbCB1cGRhdGUgb3IgaW5zZXJ0XG4gICAgICogQHBhcmFtIHthbnl9IGRvY3VtZW50IC0gRG9jdW1lbnQgdG8gdXBzZXJ0LlxuICAgICAqL1xuICAgIHVwc2VydChkb2N1bWVudDogYW55KTogUHJvbWlzZTxhbnk+IHtcblxuICAgICAgICBjb25zdCBwcm9taXNlID0gbmV3IFByb21pc2UoKHJlc29sdmUsIHJlamVjdCkgPT4ge1xuICAgICAgICAgICAgY29uc29sZS5sb2coYFVwc2VydCAke2RvY3VtZW50fWApO1xuICAgICAgICAgICAgaWYgKGRvY3VtZW50LmlkKSBkb2N1bWVudC5faWQgPSBkb2N1bWVudC5pZDtcblxuICAgICAgICAgICAgdHJ5IHtcbiAgICAgICAgICAgICAgICAodGhpcy5fZGF0YWJhc2UgYXMgYW55KVt0aGlzLl9uYW1lXS51cHNlcnQoZG9jdW1lbnQsICgpID0+IHtcbiAgICAgICAgICAgICAgICAgICAgY29uc29sZS5sb2coJ1Vwc2VydGVkJyk7XG4gICAgICAgICAgICAgICAgICAgIHJlc29sdmUoKTtcbiAgICAgICAgICAgICAgICB9KTtcbiAgICAgICAgICAgIH0gY2F0Y2ggKGV4KSB7XG4gICAgICAgICAgICAgICAgY29uc29sZS5sb2coYEV4Y2VwdGlvbiAke2V4fWApO1xuICAgICAgICAgICAgICAgIHJlamVjdChleCk7XG4gICAgICAgICAgICB9XG4gICAgICAgIH0pO1xuICAgICAgICByZXR1cm4gcHJvbWlzZTtcbiAgICB9XG5cblxuICAgIC8qKlxuICAgICAqIEZpbmQgZG9jdW1lbnRzXG4gICAgICogQHBhcmFtIHsqfSBzZWxlY3RvciBTZWxlY3RvciB0byB1c2VcbiAgICAgKiBAcGFyYW0geyp9IG9wdGlvbnMgT3B0aW9ucyB0byB1c2VcbiAgICAgKi9cbiAgICBmaW5kKHNlbGVjdG9yOiBhbnksIG9wdGlvbnM6IGFueSk6IFByb21pc2U8YW55PiB7XG4gICAgICAgIGNvbnN0IGFjdHVhbFNlbGVjdG9yID0gZXZhbChgYWN0dWFsU2VsZWN0b3IgPSAke3NlbGVjdG9yfWApO1xuXG4gICAgICAgIGNvbnN0IHByb21pc2UgPSBuZXcgUHJvbWlzZSgocmVzb2x2ZSwgcmVqZWN0KSA9PiB7XG4gICAgICAgICAgICAodGhpcy5fZGF0YWJhc2UgYXMgYW55KVt0aGlzLl9uYW1lXS5maW5kKGFjdHVhbFNlbGVjdG9yLCBvcHRpb25zKVxuICAgICAgICAgICAgICAgIC5mZXRjaCgocmVzdWx0OiBhbnkpID0+IHtcbiAgICAgICAgICAgICAgICAgICAgcmVzb2x2ZShyZXN1bHQpO1xuICAgICAgICAgICAgICAgIH0sIChlcnJvcjogYW55KSA9PiB7XG4gICAgICAgICAgICAgICAgICAgIGNvbnNvbGUubG9nKGBFcnJvciAke2Vycm9yfWApO1xuICAgICAgICAgICAgICAgICAgICByZWplY3QoZXJyb3IpO1xuICAgICAgICAgICAgICAgIH0pO1xuICAgICAgICB9KTtcbiAgICAgICAgcmV0dXJuIHByb21pc2U7XG4gICAgfVxuXG4gICAgLyoqXG4gICAgICogRGVsZXRlIG9uZSBkb2N1bWVudCBiYXNlZCBvbiBhIHNlbGVjdG9yIGNyaXRlcmlhXG4gICAgICogQHBhcmFtIHsqfSBzZWxlY3RvciAtIFNlbGVjdG9yIHRvIHVzZVxuICAgICAqL1xuICAgIGRlbGV0ZU9uZShzZWxlY3RvcjogYW55KTogUHJvbWlzZTxhbnk+IHtcbiAgICAgICAgY29uc3QgYWN0dWFsU2VsZWN0b3IgPSBldmFsKGBhY3R1YWxTZWxlY3RvciA9ICR7c2VsZWN0b3J9YCk7XG4gICAgICAgIGNvbnN0IHByb21pc2UgPSBuZXcgUHJvbWlzZSgocmVzb2x2ZSwgcmVqZWN0KSA9PiB7XG4gICAgICAgICAgICBjb25zdCBjb2xsZWN0aW9uID0gKHRoaXMuX2RhdGFiYXNlIGFzIGFueSlbdGhpcy5fbmFtZV07XG4gICAgICAgICAgICBjb2xsZWN0aW9uLmZpbmRPbmUoYWN0dWFsU2VsZWN0b3IsIHt9LCAocmVzdWx0OiBhbnkpID0+IHtcbiAgICAgICAgICAgICAgICBjb2xsZWN0aW9uLnJlbW92ZShyZXN1bHQuX2lkLCAoKSA9PiB7XG4gICAgICAgICAgICAgICAgICAgIGNvbGxlY3Rpb24ucmVzb2x2ZVJlbW92ZShyZXN1bHQuX2lkLCAoKSA9PiB7XG4gICAgICAgICAgICAgICAgICAgICAgICByZXNvbHZlKCk7XG4gICAgICAgICAgICAgICAgICAgIH0sICgpID0+IHJlamVjdCgpKTtcbiAgICAgICAgICAgICAgICB9LCAoKSA9PiByZWplY3QoKSk7XG4gICAgICAgICAgICB9LCAoKSA9PiByZWplY3QoKSk7XG4gICAgICAgIH0pO1xuICAgICAgICByZXR1cm4gcHJvbWlzZTtcbiAgICB9XG59XG4iXX0=
