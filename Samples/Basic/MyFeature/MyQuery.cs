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
        readonly IAsyncReadModelRepositoryFor<Animal> _repository;
        readonly IJSRuntime _jsRuntime;

        public MyQuery(IJSRuntime jsRuntime, IAsyncReadModelRepositoryFor<Animal> repository)
        {
            _jsRuntime = jsRuntime;
            _repository = repository;
        }

        public Task<IQueryable<Animal>> Query
        {
            get
            {   
                Console.WriteLine("QUERY");
                var tcs = new TaskCompletionSource<IQueryable<Animal>>();
                _jsRuntime.Invoke<IEnumerable<Animal>>("window.mongoDb.getAllAnimals").ContinueWith(result =>
                {
                    Console.WriteLine("Result");
                    tcs.SetResult(result.Result.AsQueryable());
                });

                return tcs.Task;
            }
        }
    }
}