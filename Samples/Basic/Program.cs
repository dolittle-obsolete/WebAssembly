using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Dolittle.Assemblies;
using Dolittle.Bootstrapping;
using Dolittle.Collections;
using Dolittle.DependencyInversion;
using Microsoft.Extensions.DependencyModel;
using WebAssembly;

namespace Basic
{
    class Program
    {

        static AssemblyName CreateAssemblyNameFor(string name, string version)
        {
            var assemblyName = new AssemblyName(name);
            assemblyName.Version = new Version(version);
            return assemblyName;
        }

        static void Main(string[] args)
        {
            var before = DateTime.Now;
            var window = (JSObject) WebAssembly.Runtime.GetGlobalObject("window");
            //var location = (JSObject)window.GetObjectProperty("location");

             var bootResult = Bootloader.Configure()
                .WithEntryAssembly(typeof(Program).Assembly)
                .WithAssemblies(new[] {
                    CreateAssemblyNameFor("Dolittle.Assemblies", "2.0.0"),
                    CreateAssemblyNameFor("Dolittle.Collections", "2.0.0"),
                    CreateAssemblyNameFor("Dolittle.DependencyInversion", "2.0.0"),
                    CreateAssemblyNameFor("Dolittle.DependencyInversion.Bootstrap", "2.0.0"),
                    CreateAssemblyNameFor("Dolittle.DependencyInversion.Conventions", "2.0.0"),
                    CreateAssemblyNameFor("Dolittle.DependencyInversion.Autofac", "2.0.0"),
                    CreateAssemblyNameFor("Dolittle.Execution", "2.0.0"),
                    CreateAssemblyNameFor("Dolittle.Lifecycle", "2.0.0"),
                    CreateAssemblyNameFor("Dolittle.Logging", "2.0.0"),
                    CreateAssemblyNameFor("Dolittle.Reflection", "2.0.0"),
                    CreateAssemblyNameFor("Dolittle.Scheduling", "2.0.0"),
                    CreateAssemblyNameFor("Dolittle.Specifications", "2.0.0"),
                    CreateAssemblyNameFor("Dolittle.Types", "2.0.0")
                })
                .UseLogAppender(new CustomLogAppender())
                .Start();

            var container = bootResult.Container;

            Console.WriteLine("Go get Foo");
            var foo = container.Get<IFoo>();
            foo.DoStuff();

            var after = DateTime.Now;
            var delta = after.Subtract(before);

            window.Invoke("alert","Initialization took : "+delta.TotalMilliseconds);

            var i = 0;
            i++;

            if (i == 5)
            {
                Dolittle.DependencyInversion.Autofac.Container c = new Dolittle.DependencyInversion.Autofac.Container(null);
                Dolittle.Scheduling.IScheduler s = new Dolittle.Scheduling.SyncScheduler();
            }

            //Console.WriteLine("Location : "+location);
            //var hostBuilder = new HostBuilder();
            //hostBuilder.Build(entryAssembly:typeof(Program).Assembly);
        }
    }
}