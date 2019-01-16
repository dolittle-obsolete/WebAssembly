import * as minimongo from 'minimongo';
import { inject } from 'aurelia-framework'
import { CommandCoordinator } from '@dolittle/commands';
import { QueryCoordinator } from '@dolittle/queries';

import { MyCommand } from './MyFeature/MyCommand';
import { MyQuery } from './MyFeature/MyQuery';

import { Guid } from '@dolittle/core';



@inject(CommandCoordinator, QueryCoordinator)
export class index {
    #commandCoordinator;
    #queryCoordinator;

    results = [];
    animals = [];
    doc = { Species: '', Name: '' };

    loading = true;

    constructor(commandCoordinator, queryCoordinator) {
        this.#commandCoordinator = commandCoordinator;
        this.#queryCoordinator = queryCoordinator;

        window._dolittleLoaded = () => {
            this.loading = false;¯
        };

        let IndexedDb = minimongo.IndexedDb;
        window.mongoDb = {};        
        window.mongoDb.database = new IndexedDb({ namespace: "mydb" }, function () {
            //console.log("Hello : " + window.mongoDb.database);
            window.mongoDb.database.addCollection("animals", () => {
                window.mongoDb.collection = window.mongoDb.database.animals;

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

        window.mongoDb.insert = (document) => {
            let obj = JSON.parse(document);
            //console.log(document);

            window.mongoDb.database.animals.upsert(obj, () => {
                console.log("upserted");

            });

        };

        window.mongoDb.getAllAnimals = () => {
            let promise = new Promise(resolve => {
                window.mongoDb.collection.find({}).fetch(results => {
                    resolve(JSON.stringify(results));
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
        }        
    }


    perform() {
        let command = new MyCommand();
        command.something = 42;
        this.#commandCoordinator.handle(command).then(result => {
            this.results.push(result);
        });
    }

    populate() {
        this.animals = [];

        let query = new MyQuery();
        this.#queryCoordinator.execute(query).then(result => {
            this.animals = result.items;
        });
    }


    getData() {

        /*
        window.mongoDb.database.animals.findOne({ Species: "Dog" }, {}, (res) => {
            this.doc = res;
            //console.log("Dog's name is: " + res.Name);
        });*/
    }

    goOffline() {

        let link = document.createElement("link");
        link.rel = 'manifest';
        link.href = '/manifest.json';
        document.head.appendChild(link);

        navigator.serviceWorker.register('service-worker.js');

    }

    clearCache() {
        caches.keys().then(names => {
            for (let name of names)
                caches.delete(name);
        });

        navigator.serviceWorker.getRegistrations().then(registrations => {

            for (let registration of registrations) {
                registration.unregister();
            }
        });
    }
}