// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dolittle.Build;
using Dolittle.Lifecycle;
using Microsoft.Extensions.DependencyModel;
using Mono.Cecil;

namespace Dolittle.Interaction.WebAssembly.Build
{
    /// <summary>
    /// Represents a system that knows how to discover assemblies.
    /// </summary>
    [Singleton]
    public class Assemblies
    {
        readonly Configuration _configuration;
        readonly AssemblyPaths _assemblyPaths;
        readonly HashSet<SkippedImport> _skippedImports = new HashSet<SkippedImport>();
        readonly BuildTarget _buildTarget;
        IEnumerable<string> _rootAssemblyPaths;
        IEnumerable<string> _allImportedAssemblyPaths;
        IEnumerable<string> _allImportedAssemblyDebugSymbolPaths;
        IEnumerable<Library> _libraries;

        /// <summary>
        /// Initializes a new instance of the <see cref="Assemblies"/> class.
        /// </summary>
        /// <param name="configuration">Current <see cref="Configuration"/>.</param>
        /// <param name="buildTarget">Current <see cref="BuildTarget"/>.</param>
        /// <param name="assemblyPaths">Paths for assemblies.</param>
        public Assemblies(
            Configuration configuration,
            BuildTarget buildTarget,
            AssemblyPaths assemblyPaths)
        {
            _configuration = configuration;
            _assemblyPaths = assemblyPaths;
            _buildTarget = buildTarget;
            PopulateRootAssemblies();
            ImportAllAssemblies();
        }

        /// <summary>
        /// Gets all the skipped imports.
        /// </summary>
        public IEnumerable<SkippedImport> SkippedImports => _skippedImports;

        /// <summary>
        /// Gets all assemblies represented as <see cref="Library"/>.
        /// </summary>
        public IEnumerable<Library> Libraries => _libraries;

        /// <summary>
        /// Gets all the paths to all the root assemblies.
        /// </summary>
        public IEnumerable<string> RootAssemblyPaths => _rootAssemblyPaths;

        /// <summary>
        /// Gets the paths to all the imported assemblies (recursively imported).
        /// </summary>
        public IEnumerable<string> AllImportedAssemblyPaths => _allImportedAssemblyPaths;

        /// <summary>
        /// Gets the paths to all the imported assemblies debug symbols (recursively imported).
        /// </summary>
        public IEnumerable<string> AllImportedAssemblyDebugSymbolPaths => _allImportedAssemblyDebugSymbolPaths;

        void ImportAllAssemblies()
        {
            var importedFiles = new HashSet<string>();
            foreach (var rootAssemblyPath in _rootAssemblyPaths)
            {
                Import(rootAssemblyPath, importedFiles);
            }

            Import("WebAssembly.Bindings.dll", importedFiles);
            Import("netstandard.dll", importedFiles);

            _allImportedAssemblyPaths = importedFiles.Where(_ => Path.GetExtension(_).Equals(".dll", StringComparison.InvariantCultureIgnoreCase)).Distinct();
            _allImportedAssemblyDebugSymbolPaths = importedFiles.Where(_ => Path.GetExtension(_).Equals(".pdb", StringComparison.InvariantCultureIgnoreCase)).Distinct();
        }

        void Import(string file, HashSet<string> files)
        {
            var bestMatchedFile = _assemblyPaths.FindBestMatchFor(file);
            try
            {
                var filesToImport = GetFilesFor(_assemblyPaths, bestMatchedFile);
                foreach (var fileToAdd in filesToImport)
                {
                    if (!files.Add(fileToAdd)) return;
                }

                var module = ModuleDefinition.ReadModule(bestMatchedFile);
                foreach (var assemblyReference in module.AssemblyReferences)
                {
                    var referenceFile = $"{assemblyReference.Name}.dll";
                    referenceFile = _rootAssemblyPaths
                                        .SingleOrDefault(_ =>
                                            Path.GetFileName(_).Equals(referenceFile, StringComparison.InvariantCultureIgnoreCase)) ?? referenceFile;

                    Import(referenceFile, files);
                }
            }
            catch (Exception ex)
            {
                _skippedImports.Add(new SkippedImport(file, ex.Message));
            }
        }

        IEnumerable<string> GetFilesFor(AssemblyPaths paths, string path)
        {
            var files = new List<string>();
            path = paths.FindBestMatchFor(path);
            if (File.Exists(path))
            {
                files.Add(path);

                var pdbFile = Path.ChangeExtension(path, ".pdb");
                if (File.Exists(pdbFile)) files.Add(pdbFile);
            }

            return files;
        }

        void PopulateRootAssemblies()
        {
            var libraries = new List<Library>
            {
                new Library(
                "Project",
                _buildTarget.AssemblyName.Name,
                "1.0.0",
                string.Empty,
                Array.Empty<Dependency>(),
                false)
            };

            libraries.AddRange(_buildTarget.AssemblyContext.GetReferencedLibraries());
            _libraries = libraries;

            var paths = new List<string>();
            libraries.ForEach(library =>
            {
                var assetsPaths = _buildTarget.AssemblyContext.GetAssemblyPathsFor(library);
                var bestMatchedPaths = assetsPaths.Select(_ => _assemblyPaths.FindBestMatchFor(_));
                paths.AddRange(bestMatchedPaths);
            });
            _rootAssemblyPaths = paths;
        }
    }
}