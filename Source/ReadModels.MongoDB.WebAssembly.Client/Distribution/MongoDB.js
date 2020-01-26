"use strict";
// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
Object.defineProperty(exports, "__esModule", { value: true });
const Database_1 = require("./Database");
/**
 * Represents a wrapper for working with MiniMongo
 */
class MongoDB {
    /**
     * Initializes MongoDB in the browser
     * @param {string} databaseName Name of the database
     */
    initialize(databaseName) {
        console.log(`Initialize MongoDB with database '${databaseName}'`);
        this._database = new Database_1.Database(databaseName);
    }
    get database() {
        return this._database;
    }
}
exports.MongoDB = MongoDB;

//# sourceMappingURL=data:application/json;charset=utf8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIk1vbmdvREIudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IjtBQUFBLCtDQUErQztBQUMvQyxxR0FBcUc7O0FBRXJHLHlDQUFzQztBQUV0Qzs7R0FFRztBQUNILE1BQWEsT0FBTztJQUdoQjs7O09BR0c7SUFDSCxVQUFVLENBQUMsWUFBb0I7UUFDM0IsT0FBTyxDQUFDLEdBQUcsQ0FBQyxxQ0FBcUMsWUFBWSxHQUFHLENBQUMsQ0FBQztRQUNsRSxJQUFJLENBQUMsU0FBUyxHQUFHLElBQUksbUJBQVEsQ0FBQyxZQUFZLENBQUMsQ0FBQztJQUNoRCxDQUFDO0lBRUQsSUFBSSxRQUFRO1FBQ1IsT0FBTyxJQUFJLENBQUMsU0FBUyxDQUFDO0lBQzFCLENBQUM7Q0FDSjtBQWZELDBCQWVDIiwiZmlsZSI6Ik1vbmdvREIuanMiLCJzb3VyY2VzQ29udGVudCI6WyIvLyBDb3B5cmlnaHQgKGMpIERvbGl0dGxlLiBBbGwgcmlnaHRzIHJlc2VydmVkLlxuLy8gTGljZW5zZWQgdW5kZXIgdGhlIE1JVCBsaWNlbnNlLiBTZWUgTElDRU5TRSBmaWxlIGluIHRoZSBwcm9qZWN0IHJvb3QgZm9yIGZ1bGwgbGljZW5zZSBpbmZvcm1hdGlvbi5cblxuaW1wb3J0IHsgRGF0YWJhc2UgfSBmcm9tICcuL0RhdGFiYXNlJztcblxuLyoqXG4gKiBSZXByZXNlbnRzIGEgd3JhcHBlciBmb3Igd29ya2luZyB3aXRoIE1pbmlNb25nb1xuICovXG5leHBvcnQgY2xhc3MgTW9uZ29EQiB7XG4gICAgcHJpdmF0ZSBfZGF0YWJhc2U6IERhdGFiYXNlIHzCoHVuZGVmaW5lZDtcblxuICAgIC8qKlxuICAgICAqIEluaXRpYWxpemVzIE1vbmdvREIgaW4gdGhlIGJyb3dzZXJcbiAgICAgKiBAcGFyYW0ge3N0cmluZ30gZGF0YWJhc2VOYW1lIE5hbWUgb2YgdGhlIGRhdGFiYXNlXG4gICAgICovXG4gICAgaW5pdGlhbGl6ZShkYXRhYmFzZU5hbWU6IHN0cmluZykge1xuICAgICAgICBjb25zb2xlLmxvZyhgSW5pdGlhbGl6ZSBNb25nb0RCIHdpdGggZGF0YWJhc2UgJyR7ZGF0YWJhc2VOYW1lfSdgKTtcbiAgICAgICAgdGhpcy5fZGF0YWJhc2UgPSBuZXcgRGF0YWJhc2UoZGF0YWJhc2VOYW1lKTtcbiAgICB9XG5cbiAgICBnZXQgZGF0YWJhc2UoKSB7XG4gICAgICAgIHJldHVybiB0aGlzLl9kYXRhYmFzZTtcbiAgICB9XG59XG4iXX0=
