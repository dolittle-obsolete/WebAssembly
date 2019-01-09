/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Reflection;
using Dolittle.Booting.Stages;
using Dolittle.Interaction.WebAssembly;

namespace Dolittle.Booting
{
    /// <summary>
    /// Extensions for building <see cref="DiscoverySettings"/> 
    /// </summary>
    public static class DiscoveryBootBuilderExtensions
    {
        /// <summary>
        /// With a set of known <see cref="AssemblyName">assemblies</see>
        /// </summary>
        /// <param name="bootBuilder"><see cref="BootBuilder"/> to build</param>
        /// <param name="assembly"><see cref="Assembly"/> that holds the embedded resource with assemblies in it</param>
        /// <returns>Chained <see cref="BootBuilder"/></returns>
        public static IBootBuilder WithAssembliesSpecifiedIn(this IBootBuilder bootBuilder, Assembly assembly)
        {
            bootBuilder.Set<DiscoverySettings>(_ => _.AssemblyProvider, new EmbeddedResourceAssembliesAssemblyProvider(assembly));
            return bootBuilder;
        }       
    }
}