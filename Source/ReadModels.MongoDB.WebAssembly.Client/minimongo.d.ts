// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
import { IndexedDb } from 'minimongo';
declare module 'minimongo' {
    class IndexedDb {
        constructor(options: any, success?: Function, error?: Function);

        addCollection(name: string, success?: Function, error?: Function): void;
        removeCollection(name: string, success?: Function, error?: Function): void;

        getCollectionNames(): string[];
    }
}
