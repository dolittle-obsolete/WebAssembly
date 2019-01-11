using System;
using System.Collections.Generic;
using System.Linq;
using WebAssembly;
using Dolittle.ReadModels;
using Dolittle.Queries;
using System.Threading;
using System.Threading.Tasks;
using Dolittle.Interaction.WebAssembly.Interop;

namespace Basic.MyFeature
{
    public class MyQuery : IQueryFor<Animal>
    {
        readonly IJSRuntime _jsRuntime;

        /*readonly IReadModelRepositoryFor<Animal> _repository;

public MyQuery(IReadModelRepositoryFor<Animal> repository)
{
_repository = repository;
}*/

        public MyQuery(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }


        public IQueryable<Animal> Query //=> _repository.Query;
        {
            get
            {
                var task = _jsRuntime.Invoke<IEnumerable<Animal>>("window.mongoDb.getAllAnimals");
                task.Wait();

                Console.WriteLine("Continuing");
                var result = task.Result;

                Console.WriteLine("Result : "+result);

                /*
                var window = (JSObject) WebAssembly.Runtime.GetGlobalObject("window");
                var mongoDb = (JSObject)window.GetObjectProperty("mongoDb");

                var completion = new TaskCompletionSource<object>();
                var resultCallback = new Action<object>((results) => {
                    Console.WriteLine("RESULTS : "+results);

                    var animals = Program._serializer.FromJson<IEnumerable<Animal>>(results.ToString());
                    completion.SetResult(animals);
                });

                mongoDb.Invoke("getAllAnimals", resultCallback);*/

                //completion.Task.ConfigureAwait(false);

                //completion.Task.Wait();
                //var result = completion.Task.Result as IEnumerable<Animal>;
                return result.AsQueryable();
            }

        }

    }
}