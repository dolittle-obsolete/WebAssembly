/*
Things to remember

Min Req: Windows 7 

Edge 17
Chrome 40
FF 44

Can I Use - WebAssembly - Check
Can I Use - IndexedDb - Check

- ASM JS - Backwards compatible for now

- Developer Story - ability to view data simply

- Security Aspect - data is now in the browser

Issues to register

- Documentation for the Configuration System

- Configuration System should be able to do Key/Value - Dictionary type of ConfigurationObjects directly.

- Assembly filtering is to eager to filter away - it looks at actual usage of types. All Dolittle packages should just be included, as well as project references

- Provide assemblies needed to be discovered from the list generated in JavaScript

- Resource system - can use Boot container - e.g. for instances of IRepresentAResourceType

- Tenant needs to be set

- Tenant Map needs to be using Configuration System

- Server.json needs to be using Configuration System

- Event-horizons.json needs to be using Configuration System

- Tenants file should be just a key / value of tenant and configuration, not a "tenants" property

- Use of IImplementationsOf<> instead of ITypeFinder in ResourceConfiguration

- Fix XML comments throughout Resource system

- Clean up specs in ResourceTypes.Configuration -> Given statements with "all_dependencies" aren't constants, but should have actual dependencies

- Clean up any coupling between specs in ResourceTypes.Configuration due to reuse of constants and given statements

- Allow dependencies for resource type representations using the boot container - ResourceConfiguration

- Change from File.* to IFileSystem throughout

- Resource system - exception strategies - let the exceptions have their own messages

- Loading events - know when runtime is loading

- Remember ApplicationBindings - return actual instances

- Remember ResourceConfiguration - disabled only once configuration

- BootProcedure for Events.Processing - it has a massive number of dependencies - this feels wrong

- Time / SystemClock - events - how do we do this - synchronize with the server somehow at install time... ??  

- ConfigureResourceTypes on ResourceConfiguration seems wrong along with the IsConfigured

- ResourceConfiguration needs to be a singleton - it is now instantiated twice - [Singleton] not respected somehow - or wrong ordering

- Merging of bindings BindingCollection - who wins?  existing or overwrite with new binding

- Specs for startup of dependency inversion - its a bit of a mess there

- Change from Information to Trace on a lot of logging messages

- Specs for BootContainer

- Serializer should honor default values for constructor parameters

- Don't lock ourselves to JSON in the File/Manifest provider - they assume .json for filename right now

- Consider removing Container support from the JSON Serializer - it is kinda fundamentally wrong that it can do this

- Improve error messages from Json Serializer when not matching parameters for constructor with properties. 
  Validate by checking if JSON property is a C# property or a constructor parameter. If not we should probably not resolve it

- Default values for constructor parameters for configuration objects

- Configuration system should enforce recursive immutability on any child properties

- Immutability checks should check property type if it is immutable - a List<> for instance is mutable and should be allowed

- Build tool - namespace Artifact -> Artifacts

- APPLICATION AND BOUNDEDCONTEXT IS WRONG EVEN AFTER STARTUP!!!!!!!!!!

- Remove : ResourceConfigurationAlreadyConfigured - if duplicate of same type of resource, throw exception instead

- WE NEED A CONFIGURATION SYSTEM WITH SECTIONS - Automatically hooked up to the .dolittle folder and loaded from individual files

*/

using System;
using System.Collections.Generic;
using System.Reflection;
using Dolittle.Artifacts;
using Dolittle.Bootstrapping;
using Dolittle.Collections;
using Dolittle.Logging;
using Dolittle.PropertyBags;
using Dolittle.Runtime.Commands.Coordination;
using Dolittle.Serialization.Json;
using Dolittle.Runtime.Commands;
using Dolittle.Execution;
using Dolittle.Domain;
using Dolittle.Runtime.Events.Store;
using Dolittle.ResourceTypes.Configuration;
using Dolittle.Tenancy;
using Dolittle.ResourceTypes;
using Dolittle.Runtime.Tenancy;
using Dolittle.Queries;
using Dolittle.Queries.Coordination;

using Basic.MyFeature;
using Dolittle.Applications.Configuration;

namespace Basic
{

    public class MyBootProcedure : ICanPerformBootProcedure
    {
        readonly IResourceConfiguration _resourceConfiguration;
        readonly ICanProvideResourceConfigurationsByTenant _resourceConfigurations;
        private readonly ILogger _logger;

        public MyBootProcedure(
            IResourceConfiguration resourceConfiguration,
            ICanProvideResourceConfigurationsByTenant resourceConfigurations,
            ILogger logger)
        {
            _resourceConfiguration = resourceConfiguration;
            _resourceConfigurations = resourceConfigurations;
            _logger = logger;
        }

        public bool CanPerform() => _resourceConfiguration.IsConfigured;

        public void Perform()
        {
            // Missing configuration - manually setting it up           
            _resourceConfiguration.ConfigureResourceTypes(new Dictionary<ResourceType,ResourceTypeImplementation>{
                { "eventStore", "IndexedDb" }
            });
            _resourceConfigurations.AddConfigurationFor(TenantId.Development,"eventStore",new EventStoreConfiguration());
        }
    }

    class Program
    {
        public static ISerializer _serializer;
        static ICommandCoordinator _commandCoordinator;
        static IQueryCoordinator _queryCoordinator;

        static IExecutionContextManager _executionContextManager;
        
        public static string HandleCommand(object command)
        {
            _executionContextManager.CurrentFor(TenantId.Development);

            var request = new CommandRequest(
                CorrelationId.New(),
                (ArtifactId)Guid.Parse("0889884d-ae6b-46a1-adc0-7bdcb11c19d3"),
                1,
                new Dictionary<string,object>()
            );

            var result = _commandCoordinator.Handle(request);
            var jsonResult = _serializer.ToJson(result);
            return jsonResult;
        }

        public static string GetAnimals()
        {
            _executionContextManager.CurrentFor(TenantId.Development);

            var query = new MyQuery();
            var result = _queryCoordinator.Execute(query, new PagingInfo());
            return _serializer.ToJson(result);
        }

        static void Main(string[] args)
        {           
            var before = DateTime.Now;

             var bootResult = Bootloader.Configure()
                .WithEntryAssembly(typeof(Program).Assembly)
                .WithAssemblies(assemblies)
                .SynchronousScheduling()
                .UseLogAppender(new CustomLogAppender())
                //.SkipBootprocedures()
                .Start();

            var container = bootResult.Container;

            var logger = container.Get<ILogger>();
            logger.Information("We're running");

            var after = DateTime.Now;
            var delta = after.Subtract(before);

            _serializer = container.Get<ISerializer>();
            _commandCoordinator = container.Get<ICommandCoordinator>();
            _queryCoordinator = container.Get<IQueryCoordinator>();

            _executionContextManager = container.Get<IExecutionContextManager>();
            _executionContextManager.CurrentFor(TenantId.Development);

            var i = 0;
            i++;

            if (i == 5)
            {

                Dolittle.DependencyInversion.Autofac.Container c = new Dolittle.DependencyInversion.Autofac.Container(null);
                Dolittle.Scheduling.IScheduler s = new Dolittle.Scheduling.SyncScheduler();
                Dolittle.ResourceTypes.ResourceType r = new Dolittle.ResourceTypes.ResourceType();
                var a = Artifact.New();

                var propertyBag = new PropertyBag(new NullFreeDictionary<string, object> {});
                var sc = new Dolittle.Time.SystemClock();

                var ls = new Dolittle.Globalization.LocalizationScope(System.Globalization.CultureInfo.InvariantCulture,System.Globalization.CultureInfo.InvariantCulture);

                var cr = new Dolittle.Runtime.Commands.CommandResult();

                var cb = new Dolittle.Runtime.Events.CausedBy();

                var eb = new Dolittle.Events.EventsBindings();

                var ad = new Dolittle.Artifacts.Configuration.ArtifactDefinition();

                var cc = new Dolittle.ResourceTypes.Configuration.InvalidResourceTypeFound("");

                var ac = new Dolittle.Applications.Configuration.Area();

                var bl = new Dolittle.Applications.Configuration.MissingBoundedContextConfiguration();

                var ea = new Dolittle.Events.Processing.EventProcessorAttribute("");

                var fe = new Dolittle.Queries.Security.Fetching();

                var va = new Dolittle.Queries.Validation.QueryArgument();

                var rr = new Dolittle.Rules.RuleContext();

                var bc = new Dolittle.Configuration.Bindings(null,null,null);

                var cs = new Dolittle.Concepts.Serialization.Json.ConceptConverter();
            }
        }

        static AssemblyName CreateAssemblyNameFor(string name, string version)
        {
            var assemblyName = new AssemblyName(name);
            assemblyName.Version = new Version(version);
            return assemblyName;
        }

        static IEnumerable<AssemblyName> assemblies = new[] {
            CreateAssemblyNameFor("Basic", "2.0.0"),
            CreateAssemblyNameFor("Dolittle.Artifacts", "2.0.0"),
            //CreateAssemblyNameFor("Dolittle.Dynamic", "2.0.0"),
            CreateAssemblyNameFor("Dolittle.ResourceTypes", "2.0.0"),
            CreateAssemblyNameFor("Dolittle.Globalization", "2.0.0"),
            //CreateAssemblyNameFor("Dolittle.Immutability", "2.0.0"),
            CreateAssemblyNameFor("Dolittle.PropertyBags", "2.0.0"),
            CreateAssemblyNameFor("Dolittle.Rules", "2.0.0"),
            CreateAssemblyNameFor("Dolittle.Time", "2.0.0"),

            CreateAssemblyNameFor("Dolittle.Applications", "2.0.0"),
            
            CreateAssemblyNameFor("Dolittle.Assemblies", "2.0.0"),
            CreateAssemblyNameFor("Dolittle.Bootstrapping", "2.0.0"),
            CreateAssemblyNameFor("Dolittle.Collections", "2.0.0"),
            CreateAssemblyNameFor("Dolittle.Configuration", "2.0.0"),
            CreateAssemblyNameFor("Dolittle.Concepts", "2.0.0"),
            CreateAssemblyNameFor("Dolittle.Concepts.Serialization.Json", "2.0.0"),
            CreateAssemblyNameFor("Dolittle.DependencyInversion", "2.0.0"),
            CreateAssemblyNameFor("Dolittle.DependencyInversion.Bootstrap", "2.0.0"),
            CreateAssemblyNameFor("Dolittle.DependencyInversion.Conventions", "2.0.0"),
            CreateAssemblyNameFor("Dolittle.DependencyInversion.Autofac", "2.0.0"),
            
            CreateAssemblyNameFor("Dolittle.Execution", "2.0.0"),

            CreateAssemblyNameFor("Dolittle.SDK.Applications.Configuration", "2.0.0"),
            CreateAssemblyNameFor("Dolittle.SDK.Events", "2.0.0"),
            CreateAssemblyNameFor("Dolittle.SDK.Events.Processing", "2.0.0"),
            CreateAssemblyNameFor("Dolittle.SDK.Artifacts", "2.0.0"),
            CreateAssemblyNameFor("Dolittle.SDK.Artifacts.Configuration", "2.0.0"),
            CreateAssemblyNameFor("Dolittle.SDK.Commands.Handling", "2.0.0"),
            CreateAssemblyNameFor("Dolittle.SDK.Domain", "2.0.0"),
            
            CreateAssemblyNameFor("Dolittle.Lifecycle", "2.0.0"),
            CreateAssemblyNameFor("Dolittle.Logging", "2.0.0"),
            
            CreateAssemblyNameFor("Dolittle.Reflection", "2.0.0"),
            CreateAssemblyNameFor("Dolittle.ResourceTypes.Configuration", "2.0.0"),
            
            CreateAssemblyNameFor("Dolittle.Runtime.Applications.Configuration", "2.0.0"),
            CreateAssemblyNameFor("Dolittle.Runtime.Commands", "2.0.0"),
            CreateAssemblyNameFor("Dolittle.Runtime.Commands.Coordination", "2.0.0"),
            CreateAssemblyNameFor("Dolittle.Runtime.Commands.Handling", "2.0.0"),
            CreateAssemblyNameFor("Dolittle.Runtime.Commands.Security", "2.0.0"),
            CreateAssemblyNameFor("Dolittle.Runtime.Commands.Validation", "2.0.0"),
            CreateAssemblyNameFor("Dolittle.Runtime.Queries", "2.0.0"),
            CreateAssemblyNameFor("Dolittle.Runtime.Queries.Coordination", "2.0.0"),
            CreateAssemblyNameFor("Dolittle.Runtime.Queries.Security", "2.0.0"),
            CreateAssemblyNameFor("Dolittle.Runtime.Queries.Validation", "2.0.0"),

            CreateAssemblyNameFor("Dolittle.Runtime.Events", "2.0.0"),

            //CreateAssemblyNameFor("Dolittle.Runtime.Server", "2.0.0"),
            CreateAssemblyNameFor("Dolittle.Runtime.Tenancy", "2.0.0"),
            CreateAssemblyNameFor("Dolittle.Runtime.Transactions", "2.0.0"),
            CreateAssemblyNameFor("Dolittle.Runtime.Validation", "2.0.0"),

            CreateAssemblyNameFor("Dolittle.Scheduling", "2.0.0"),
            CreateAssemblyNameFor("Dolittle.Security", "2.0.0"),
            CreateAssemblyNameFor("Dolittle.Serialization.Json", "2.0.0"),
            CreateAssemblyNameFor("Dolittle.Serialization.Protobuf", "2.0.0"),
            CreateAssemblyNameFor("Dolittle.Specifications", "2.0.0"),
            CreateAssemblyNameFor("Dolittle.Strings", "2.0.0"),
            CreateAssemblyNameFor("Dolittle.Tenancy", "2.0.0"),

            CreateAssemblyNameFor("Dolittle.Types", "2.0.0")
        };
    }
}