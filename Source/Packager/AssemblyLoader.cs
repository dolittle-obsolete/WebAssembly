using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.DependencyModel.Resolution;
using Mono.Cecil;

namespace Packager
{

    /// <summary>
    /// Represents a <see cref="ICompilationAssemblyResolver"/> that tries to resolve from the package runtime store
    /// </summary>
    /// <remarks>
    /// Read more here : https://docs.microsoft.com/en-us/dotnet/core/deploying/runtime-store
    /// Linux / macOS : /usr/local/share/dotnet/store/{CPU}/{targetFramework e.g. netcoreapp2.0}/{package path}
    /// Windows       : C:/Program Files/dotnet/store/{CPU}/{targetFramework e.g. netcoreapp2.0}/{package path} 
    /// </remarks>
    public class PackageRuntimeStoreAssemblyResolver : ICompilationAssemblyResolver
    {
        /// <inheritdoc/>
        public bool TryResolveAssemblyPaths(CompilationLibrary library, List<string> assemblies)
        {

            var basePath = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)?
                            @"c:\Program Files\dotnet\store":
                            "/usr/local/share/dotnet/store";

            var cpuBasePath = Path.Combine(basePath,RuntimeInformation.ProcessArchitecture.ToString().ToLowerInvariant());
            var found = false;

            foreach( var targetFrameworkBasePath in Directory.GetDirectories(cpuBasePath))
            {
                var libraryBasePath = Path.Combine(targetFrameworkBasePath,library.Path);
                foreach( var assembly in library.Assemblies )
                {
                    var assemblyPath = Path.Combine(libraryBasePath, assembly);
                    if( File.Exists(assemblyPath))
                    {
                        assemblies.Add(assemblyPath);
                        found = true;
                    }
                }
            }

            return found;
        }
    }    

    /// <inheritdoc/>
    public class AssemblyLoader
    {
        readonly ICompilationAssemblyResolver _assemblyResolver;

        /// <summary>
        /// Initializes a new instance of <see cref="AssemblyLoader"/>
        /// </summary>
        /// <param name="path">Path to the <see cref="Assembly"/> to load</param>
        public AssemblyLoader(string path)
        {
            Assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(path);
            AssemblyLoadContext = AssemblyLoadContext.GetLoadContext(Assembly);
            AssemblyLoadContext.Resolving += OnResolving;

            DependencyContext = DependencyContext.Load(Assembly);

            var basePath = Path.GetDirectoryName(path);

            _assemblyResolver = new CompositeCompilationAssemblyResolver(new ICompilationAssemblyResolver[]
            {
                new AppBaseCompilationAssemblyResolver(basePath),
                    new ReferenceAssemblyPathResolver(),
                    new PackageCompilationAssemblyResolver(),
                    new PackageRuntimeStoreAssemblyResolver()
            });
        }

        /// <inheritdoc/>
        public Assembly Assembly { get; }

        /// <inheritdoc/>
        public DependencyContext DependencyContext {  get; }

        /// <inheritdoc/>
        public AssemblyLoadContext AssemblyLoadContext {  get; }

        /// <inheritdoc/>
        public IEnumerable<Assembly> GetReferencedAssemblies()
        {
            var libraries = DependencyContext.RuntimeLibraries.Cast<RuntimeLibrary>()
                .Where(_ => _.RuntimeAssemblyGroups.Count() > 0 && !_.Name.StartsWith("runtime"));
            //(_.Type.ToLowerInvariant() == "project" || _.Type.ToLowerInvariant() == "reference"));
            return libraries
                .Select(_ => Assembly.Load(_.Name))
                .ToArray();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            AssemblyLoadContext.Resolving -= OnResolving;
        }

        Assembly OnResolving(AssemblyLoadContext context, AssemblyName name)
        {
            bool NamesMatch(Library runtime)
            {
                return string.Equals(runtime.Name, name.Name, StringComparison.OrdinalIgnoreCase);
            }

            var library = DependencyContext.RuntimeLibraries.FirstOrDefault(NamesMatch);
            if (library != null)
            {
                var compileLibrary = new CompilationLibrary(
                    library.Type,
                    library.Name,
                    library.Version,
                    library.Hash,
                    library.RuntimeAssemblyGroups.SelectMany(g => g.AssetPaths),
                    library.Dependencies,
                    library.Serviceable,
                    library.Path,
                    library.HashPath);

                var assemblies = new List<string>();
                _assemblyResolver.TryResolveAssemblyPaths(compileLibrary, assemblies);
                if (assemblies.Count > 0)
                {
                    return AssemblyLoadContext.LoadFromAssemblyPath(assemblies[0]);
                }
            }

            return null;
        }
    }
}