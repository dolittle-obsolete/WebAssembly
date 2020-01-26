"use strict";
// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
Object.defineProperty(exports, "__esModule", { value: true });
const minimongo_1 = require("minimongo");
const Collection_1 = require("./Collection");
/**
 * Represents a wrapper for the MiniMongo database
 */
class Database {
    /**
     * Initializes a new instance of {Database}
     * @param {string} databaseName Name of the database
     */
    constructor(databaseName) {
        this._database = new minimongo_1.IndexedDb({
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
    addCollection(name) {
        console.log(`Add collection ${name}`);
        this[name] = new Collection_1.Collection(name, this._database);
        this._database.addCollection(name, () => {
            console.log(`Collection ${name} added`);
        });
    }
}
exports.Database = Database;

//# sourceMappingURL=data:application/json;charset=utf8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIkRhdGFiYXNlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7QUFBQSwrQ0FBK0M7QUFDL0MscUdBQXFHOztBQUVyRyx5Q0FBc0M7QUFDdEMsNkNBQTBDO0FBRTFDOztHQUVHO0FBQ0gsTUFBYSxRQUFRO0lBR2pCOzs7T0FHRztJQUNILFlBQVksWUFBb0I7UUFDNUIsSUFBSSxDQUFDLFNBQVMsR0FBRyxJQUFJLHFCQUFTLENBQUM7WUFDM0IsU0FBUyxFQUFFLFlBQVk7U0FDMUIsRUFBRSxHQUFHLEVBQUU7WUFDSixPQUFPLENBQUMsR0FBRyxDQUFDLFlBQVksWUFBWSxXQUFXLENBQUMsQ0FBQztRQUNyRCxDQUFDLEVBQUUsR0FBRyxFQUFFO1lBQ0osT0FBTyxDQUFDLEdBQUcsQ0FBQyxZQUFZLFlBQVksc0JBQXNCLFNBQVMsRUFBRSxDQUFDLENBQUM7UUFDM0UsQ0FBQyxDQUFDLENBQUM7SUFDUCxDQUFDO0lBRUQ7OztPQUdHO0lBQ0gsYUFBYSxDQUFDLElBQVk7UUFDdEIsT0FBTyxDQUFDLEdBQUcsQ0FBQyxrQkFBa0IsSUFBSSxFQUFFLENBQUMsQ0FBQztRQUNyQyxJQUFZLENBQUMsSUFBSSxDQUFDLEdBQUcsSUFBSSx1QkFBVSxDQUFDLElBQUksRUFBRSxJQUFJLENBQUMsU0FBUyxDQUFDLENBQUM7UUFFM0QsSUFBSSxDQUFDLFNBQVMsQ0FBQyxhQUFhLENBQUMsSUFBSSxFQUFFLEdBQUcsRUFBRTtZQUNwQyxPQUFPLENBQUMsR0FBRyxDQUFDLGNBQWMsSUFBSSxRQUFRLENBQUMsQ0FBQztRQUM1QyxDQUFDLENBQUMsQ0FBQztJQUNQLENBQUM7Q0FDSjtBQTdCRCw0QkE2QkMiLCJmaWxlIjoiRGF0YWJhc2UuanMiLCJzb3VyY2VzQ29udGVudCI6WyIvLyBDb3B5cmlnaHQgKGMpIERvbGl0dGxlLiBBbGwgcmlnaHRzIHJlc2VydmVkLlxuLy8gTGljZW5zZWQgdW5kZXIgdGhlIE1JVCBsaWNlbnNlLiBTZWUgTElDRU5TRSBmaWxlIGluIHRoZSBwcm9qZWN0IHJvb3QgZm9yIGZ1bGwgbGljZW5zZSBpbmZvcm1hdGlvbi5cblxuaW1wb3J0IHsgSW5kZXhlZERiIH0gZnJvbSAnbWluaW1vbmdvJztcbmltcG9ydCB7IENvbGxlY3Rpb24gfSBmcm9tICcuL0NvbGxlY3Rpb24nO1xuXG4vKipcbiAqIFJlcHJlc2VudHMgYSB3cmFwcGVyIGZvciB0aGUgTWluaU1vbmdvIGRhdGFiYXNlXG4gKi9cbmV4cG9ydCBjbGFzcyBEYXRhYmFzZSB7XG4gICAgcHJpdmF0ZSBfZGF0YWJhc2U6IEluZGV4ZWREYjtcblxuICAgIC8qKlxuICAgICAqIEluaXRpYWxpemVzIGEgbmV3IGluc3RhbmNlIG9mIHtEYXRhYmFzZX1cbiAgICAgKiBAcGFyYW0ge3N0cmluZ30gZGF0YWJhc2VOYW1lIE5hbWUgb2YgdGhlIGRhdGFiYXNlXG4gICAgICovXG4gICAgY29uc3RydWN0b3IoZGF0YWJhc2VOYW1lOiBzdHJpbmcpIHtcbiAgICAgICAgdGhpcy5fZGF0YWJhc2UgPSBuZXcgSW5kZXhlZERiKHtcbiAgICAgICAgICAgIG5hbWVzcGFjZTogZGF0YWJhc2VOYW1lXG4gICAgICAgIH0sICgpID0+IHtcbiAgICAgICAgICAgIGNvbnNvbGUubG9nKGBEYXRhYmFzZSAke2RhdGFiYXNlTmFtZX0gaXMgcmVhZHlgKTtcbiAgICAgICAgfSwgKCkgPT4ge1xuICAgICAgICAgICAgY29uc29sZS5sb2coYERhdGFiYXNlICR7ZGF0YWJhc2VOYW1lfSBmYWlsZWQgdG8gc3RhcnQgLSAke2FyZ3VtZW50c31gKTtcbiAgICAgICAgfSk7XG4gICAgfVxuXG4gICAgLyoqXG4gICAgICogQWRkIGEgY29sbGVjdGlvblxuICAgICAqIEBwYXJhbSB7c3RyaW5nfSBuYW1lIE5hbWUgb2YgdGhlIGNvbGxlY3Rpb25cbiAgICAgKi9cbiAgICBhZGRDb2xsZWN0aW9uKG5hbWU6IHN0cmluZykge1xuICAgICAgICBjb25zb2xlLmxvZyhgQWRkIGNvbGxlY3Rpb24gJHtuYW1lfWApO1xuICAgICAgICAodGhpcyBhcyBhbnkpW25hbWVdID0gbmV3IENvbGxlY3Rpb24obmFtZSwgdGhpcy5fZGF0YWJhc2UpO1xuXG4gICAgICAgIHRoaXMuX2RhdGFiYXNlLmFkZENvbGxlY3Rpb24obmFtZSwgKCkgPT4ge1xuICAgICAgICAgICAgY29uc29sZS5sb2coYENvbGxlY3Rpb24gJHtuYW1lfSBhZGRlZGApO1xuICAgICAgICB9KTtcbiAgICB9XG59XG4iXX0=
