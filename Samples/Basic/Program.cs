using System;
using Dolittle.Booting;
using Dolittle.Interaction.WebAssembly.Interop;
using Dolittle.Logging;
using Dolittle.Execution;
using Dolittle.Tenancy;
using Dolittle.Types;
using Dolittle.ResourceTypes;
using Dolittle.Collections;

namespace Basic
{
    class Program
    {
        static void Main(string[] args)
        {
            var before = DateTime.Now;
            Console.WriteLine("Start Dolittle");

            var bootResult = Bootloader.Configure(_ => _
                .WithEntryAssembly(typeof(Program).Assembly)
                .WithAssembliesSpecifiedIn(typeof(Program).Assembly)
                .SynchronousScheduling()
                .NoLogging()
                //.UseLogAppender(new CustomLogAppender())
            ).Start();

            var container = bootResult.Container;
            var logger = container.Get<ILogger>();
            var after = DateTime.Now;
            var delta = after.Subtract(before);

            Console.WriteLine($"We're running - took {delta.TotalSeconds}");

            var executionContextManager = container.Get<IExecutionContextManager>();
            executionContextManager.CurrentFor(TenantId.Development);

            var interop = container.Get<IJSRuntime>();
            interop.Invoke("window._dolittleLoaded");
        }
    }
}