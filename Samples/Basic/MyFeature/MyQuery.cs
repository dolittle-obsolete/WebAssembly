using System;

using System.Collections.Generic;
using WebAssembly;
using Dolittle.Queries;
using System.Linq;

namespace Basic.MyFeature
{
    public class MyQuery : IQueryFor<Animal>
    {

        public IQueryable<Animal> Query 
        {
            get
            {
                var window = (JSObject) WebAssembly.Runtime.GetGlobalObject("window");
                var mongoDb = (JSObject)window.GetObjectProperty("mongoDb");

                
                IEnumerable<Animal> animals = null;

                var resultCallback = new Action<object>((results) => {
                    Console.WriteLine("RESULTS : "+results);

                    animals = Program._serializer.FromJson<IEnumerable<Animal>>(results.ToString());
                    
                });

                mongoDb.Invoke("getAllAnimals", resultCallback);

                //while( animals == null) Thread.Sleep(10);

                return animals.AsQueryable();
            }

        }

    }
}