import * as minimongo from 'minimongo';

export class index {
    results = [];
    animals = [];
    doc = {Species:'', Name:''};

    constructor() {

        let IndexedDb = minimongo.IndexedDb;
        window.mongoDb = {};

        window.mongoDb.database = new IndexedDb({namespace: "mydb"}, function() {
            console.log("Hello : "+window.mongoDb.database);
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


                console.log("Hello : "+window.mongoDb.collection);
            });           
        });

        window.mongoDb.insert = (document) => {
            let obj = JSON.parse(document);
            //console.log(document);

            window.mongoDb.database.animals.upsert(obj, () => {
                console.log("upserted");
    
            });

        };

        window.mongoDb.getAllAnimals = (callback) => {
            window.mongoDb.collection.find({}).fetch(results => {
                callback(JSON.stringify(results));
            });
        }

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


    perform() {
        //alert("HELLO");
        let result = JSON.parse(BINDING.call_static_method("[Basic] Basic.Program:HandleCommand",{ something: 42, somehingElse: 'someString'}));
        console.log(result);

        this.results.push(result);
    }

    populate() {
        let result = JSON.parse(BINDING.call_static_method("[Basic] Basic.Program:GetAnimals", []));
    }


    getData() {
        window.mongoDb.database.animals.findOne({ Species:"Dog" }, {}, (res) => {
            this.doc = res;
            //console.log("Dog's name is: " + res.Name);
        });
    }

}