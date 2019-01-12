using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Dolittle.Interaction.WebAssembly.Interop;
using Dolittle.Queries;
using Dolittle.ReadModels;
using WebAssembly;

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

        public Task<IQueryable<Animal>> Query //=> _repository.Query;
        {
            get
            {
                var tcs = new TaskCompletionSource<IQueryable<Animal>>();
                _jsRuntime.Invoke<IEnumerable<Animal>>("window.mongoDb.getAllAnimals").ContinueWith(result =>
                {
                    Console.WriteLine("Continuing");
                    Console.WriteLine("Result : " + result.Result);

                    tcs.SetResult(result.Result.AsQueryable());
                });

                return tcs.Task;

                //task.Wait();

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
                //return new Animal[0].AsQueryable(); // result.AsQueryable();
            }

        }

    }
}