/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Dolittle.Assemblies;
using Dolittle.Collections;
using Microsoft.Extensions.DependencyModel;
using Newtonsoft.Json;

namespace Dolittle.Interaction.WebAssembly
{
    /// <summary>
    /// Represents a <see cref="ICanProvideAssemblies">assembly provider</see> that will provide assemblies based on
    /// an embedded JSON file called 'assemblies.json' that holds an array of strings
    /// </summary>
    public class EmbeddedResourceAssembliesAssemblyProvider : ICanProvideAssemblies
    {
        /// <summary>
        /// Initializes a new instance of <see cref="EmbeddedResourceAssembliesAssemblyProvider"/>
        /// </summary>
        public EmbeddedResourceAssembliesAssemblyProvider(Assembly assembly)
        {
            var name = $"{assembly.GetName().Name}.assemblies.json";
            var manifestResourceNames = assembly.GetManifestResourceNames();
            System.Console.WriteLine($"Looking for {name} - there are {manifestResourceNames.Length} resources");
            manifestResourceNames.ForEach(System.Console.WriteLine);

            if( !manifestResourceNames.Any(_ => _.Trim() == name) ) 
            {
                System.Console.WriteLine("No assemblies found");
                Libraries = new Library[0];
                return;
            }

            var resource = assembly.GetManifestResourceStream(name);
            using( var reader = new StreamReader(resource) ) 
            {
                var json = reader.ReadToEnd();

                var assemblies = JsonConvert.DeserializeObject<string[]>(json);
                Libraries = assemblies
                    .Select(_ => new Library("Package", _, "1.0.0", string.Empty, new Dependency[0], false)).Distinct().ToArray();
            }
        }

        /// <inheritdoc/>
        public IEnumerable<Library> Libraries { get; }


        /// <inheritdoc/>
        public Assembly GetFrom(Library library)
        {
            return Assembly.Load(library.Name);
        }
    }
}