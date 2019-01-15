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


    constructor(commandCoordinator, queryCoordinator) {
        this.#commandCoordinator = commandCoordinator;
        this.#queryCoordinator = queryCoordinator;
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