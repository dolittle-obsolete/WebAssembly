// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { IQuery, QueryRequest } from '@dolittle/queries';
import { DotNetRuntime } from '@dolittle/webassembly.interop/DotNetRuntime';

/**
 * Represents the coordinator of a {Command} for in-process WebAssembly usage
 */
export class QueryCoordinator {
    private _interop: DotNetRuntime;

    /**
     * Initializes a new instance of {CommandCoordinator}
     */
    constructor() {
        this._interop = new DotNetRuntime('Dolittle.Interaction.WebAssembly.Queries.QueryCoordinator');
    }

    /**
     * Execute a query
     * @param {IQuery} query - The actual query to execute.
     * @returns {Promise<any>} Promise with eventually the result of executing the query
     */
    execute(query: IQuery): Promise<any> {
        const promise = new Promise((resolve, reject) => {
            const request = QueryRequest.createFrom(query);
            this._interop.beginInvoke('Execute', [request]).then(result => {
                resolve(result);
            }).catch(exception => reject(exception));
        });
        return promise;
    }
}
