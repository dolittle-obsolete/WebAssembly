import * as minimongo from 'minimongo';
import { autoinject } from 'aurelia-framework';
import { CommandCoordinator } from '@dolittle/commands';
import { QueryCoordinator } from '@dolittle/queries';

import { MyCommand } from './MyFeature/MyCommand';
import { MyQuery } from './MyFeature/MyQuery';

import { DeleteAnimal } from './MyFeature/DeleteAnimal';

import { Guid } from '@dolittle/core';



@autoinject
export class index {
    results = [];
    animals = [];
    doc = { Species: '', Name: '' };

    loading = true;

    isOffline = false;

    totalLoadTime = 0;

    constructor(private _commandCoordinator: CommandCoordinator, private _queryCoordinator: CommandCoordinator) {
        const before = new Date();

        (window as any)._dolittleLoaded = () => {
            this.loading = false;
            const after = new Date();
            //console.log(`Total time : ${after - before}`);
            this.totalLoadTime = after - before;
        };

        navigator.serviceWorker.getRegistration().then((serviceWorker) => {
            this.isOffline = serviceWorker ? true : false;
        });

        const IndexedDb = minimongo.IndexedDb;
        (window as any).mongoDb = {};
        (window as any).mongoDb.database = new IndexedDb({ namespace: 'read_model_database_for_Basic' }, function () {
            //console.log("Hello : " + window.mongoDb.database);
            (window as any).mongoDb.database.addCollection('Animal', () => {
                (window as any).mongoDb.collection = (window as any).mongoDb.database.Animal;

                /*
                let doc = { species: "dog", name: "Bingo" };

                // Always use upsert for both inserts and modifies
                window.mongoDb.database.animals.upsert(doc, function() {
                    // Success:

                    // Query dog (with no query options beyond a selector)
                    window.mongoDb.database.animals.findOne({ species:"dog" }, {}, function(res) {
                        console.log("Dog's name is: " + res.name);
                    });
                });
                */


                //console.log("Hello : " + window.mongoDb.collection);
            });
        });

        (window as any).mongoDb.insert = (document) => {
            const obj = JSON.parse(document);
            //console.log(document);

            (window as any).mongoDb.database.Animal.upsert(obj, () => {
                console.log('upserted');
            });
        };

        (window as any).mongoDb.getAllAnimals = () => {
            const promise = new Promise(resolve => {
                (window as any).mongoDb.collection.find({}).fetch(results => {
                    resolve(results);
                });
            });
            return promise;

            /*
            // Create IndexedDb
            let db = new IndexedDb({namespace: "mydb"}, function() {
                // Add a collection to the database
                db.addCollection("animals", function() {
                    let doc = { species: "dog", name: "Bingo" };

                    // Always use upsert for both inserts and modifies
                    db.animals.upsert(doc, function() {
                        // Success:

                        // Query dog (with no query options beyond a selector)
                        db.animals.findOne({ species:"dog" }, {}, function(res) {
                            console.log("Dog's name is: " + res.name);
                        });
                    });
                });
            }, function() { alert("some error!"); });
            */
        };
    }


    perform() {
        const command = new MyCommand();
        command.something = 42;
        this._commandCoordinator.handle(command).then((result: any) => {
            this.results.push(result);
        });
    }

    populate() {
        this.animals = [];
        const query = new MyQuery();
        this._queryCoordinator.execute(query).then((result: any) => {
            this.animals = result.items;
        });

        window.mongoDb.getAllAnimals().then((result: any) => {
            this.animals = result;

        });
    }

    delete(animal) {
        const command = new DeleteAnimal();
        command.animal = animal._id;
        this._commandCoordinator.handle(command).then((result: any) => {
            this.results.push(result);
        });
    }


    getData() {

        (window as any).mongoDb.database.Animal.findOne({ species: 'Dog' }, {}, (res) => {
            this.doc = res;
            //console.log("Dog's name is: " + res.Name);
        });
    }

    goOffline() {

        const link = document.createElement('link');
        link.rel = 'manifest';
        link.href = '/manifest.json';
        document.head.appendChild(link);

        navigator.serviceWorker.register('service-worker.js').then(() => {
            this.isOffline = true;
        });
    }

    clearCache() {
        caches.keys().then(names => {
            for (const name of names)
                caches.delete(name);
        });

        navigator.serviceWorker.getRegistrations().then(registrations => {

            for (const registration of registrations) {
                registration.unregister();
            }

            this.isOffline = false;
        });
    }
}
