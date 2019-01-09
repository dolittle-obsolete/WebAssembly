/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { Query, QueryRequest } from '@dolittle/queries';
import { Interop } from '../Interop';


/**
* Represents the coordinator of a {Command} for in-process WebAssembly usage
*/
export class QueryCoordinator {
    #interop;

    /**
     * Initializes a new instance of {CommandCoordinator}
     */
    constructor() {
        this.#interop = new Interop("Dolittle.Interaction.WebAssembly.Queries.QueryCoordinator");
    }
 
    /**
     * Execute a query
     * @param {Query} query 
     * @returns {Promise} Promise with eventually the result of executing the query
     */
    execute(query) {
        var promise = new Promise((resolve, reject) => {
            let request = QueryRequest.createFrom(query);
            let result = this.#interop.invoke("Execute", [request]);
            resolve(result);
        });
        return promise;
    }
}