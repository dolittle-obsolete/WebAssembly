/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Dolittle.Assemblies;
using Mono.Cecil;
using Mono.Options;

namespace Dolittle.WebAssembly.Packager
{
    class Driver
    {
        static bool enable_debug, enable_linker;
        static string app_prefix, framework_prefix, bcl_prefix, bcl_facades_prefix, out_prefix;
        static HashSet<string> asm_map = new HashSet<string>();
        static List<string> file_list = new List<string>();

        const string BINDINGS_ASM_NAME = "WebAssembly.Bindings";
        const string BINDINGS_RUNTIME_CLASS_NAME = "WebAssembly.Runtime";

        static List<AssemblyDetails> assemblies = new List<AssemblyDetails>();

        static IEnumerable<Assembly> _assemblies;

        void Run(string[] args)
        {
            /*
            var root_assemblies = new List<string>();
            enable_debug = false;
            string builddir = null;
            string sdkdir = null;
            string emscripten_sdkdir = null;
            out_prefix = Environment.CurrentDirectory;
            app_prefix = Environment.CurrentDirectory;
            var deploy_prefix = "managed";
            var vfs_prefix = "managed";
            var use_release_runtime = true;
            var enable_aot = false;
            var enable_dedup = true;
            var print_usage = false;
            var assets = new List<string>();
            var profilers = new List<string>();
            var copyTypeParm = "default";
            var copyType = CopyType.Default;

            var new_args = p.Parse(args).ToArray();
            var rootAssemblies = new List<Assembly>();
            foreach (var a in new_args)
            {
                root_assemblies.Add(a);

                var loader = AssemblyContext.From(a);
                rootAssemblies.AddRange(loader.GetReferencedAssemblies());
            }
            _assemblies = rootAssemblies.Distinct().ToArray();

            if (print_usage)
            {
                Usage();
                return;
            }

            if (!Enum.TryParse(copyTypeParm, true, out copyType))
            {
                Console.WriteLine("Invalid copy value");
                Usage();
                return;
            }

            if (enable_aot)
                enable_linker = true;

            var tool_prefix = Path.GetDirectoryName(typeof(Driver).Assembly.Location);

            //are we working from the tree?
            if (sdkdir != null)
            {
                framework_prefix = tool_prefix; //all framework assemblies are currently side built to packager.exe
                bcl_prefix = Path.Combine(sdkdir, "wasm-bcl/wasm");
            }
            else if (Directory.Exists(Path.Combine(tool_prefix, "../out/wasm-bcl/wasm")))
            {
                framework_prefix = tool_prefix; //all framework assemblies are currently side built to packager.exe
                bcl_prefix = Path.Combine(tool_prefix, "../out/wasm-bcl/wasm");
                sdkdir = Path.Combine(tool_prefix, "../out");
            }
            else
            {
                framework_prefix = Path.Combine(tool_prefix, "framework");
                bcl_prefix = Path.Combine(tool_prefix, "wasm-bcl/wasm");
                sdkdir = tool_prefix;
            }
            bcl_facades_prefix = Path.Combine(bcl_prefix, "Facades");

            foreach (var ra in root_assemblies)
            {
                AssemblyKind kind;
                var resolved = Resolve(ra, out kind);
                Import(resolved, kind);
            }

            Import(ResolveFramework(BINDINGS_ASM_NAME + ".dll"), AssemblyKind.Framework);

            if (builddir != null)
            {
                emit_ninja = true;
                if (!Directory.Exists(builddir))
                    Directory.CreateDirectory(builddir);
            }

            if (!Directory.Exists(out_prefix))
                Directory.CreateDirectory(out_prefix);
            var bcl_dir = Path.Combine(out_prefix, deploy_prefix);
            if (Directory.Exists(bcl_dir))
                Directory.Delete(bcl_dir, true);
            Directory.CreateDirectory(bcl_dir);
            foreach (var f in file_list)
            {
                CopyFile(f, Path.Combine(bcl_dir, Path.GetFileName(f)), copyType);
            }
        }

        if (deploy_prefix.EndsWith("/"))
            deploy_prefix = deploy_prefix.Substring(0, deploy_prefix.Length - 1);
        if (vfs_prefix.EndsWith("/"))
            vfs_prefix = vfs_prefix.Substring(0, vfs_prefix.Length - 1);

        var file_list_str = string.Join(",", file_list.Select(f => $"\"{Path.GetFileName (f)}\"").Distinct());

        if (File.Exists(runtime_js) && (File.Exists(runtimeTemplate)))
        {
            CopyFile(runtimeTemplate, runtime_js, CopyType.IfNewer, $"runtime template <{runtimeTemplate}> ");
        }
        else
        {
            if (File.Exists(runtimeTemplate))
                CopyFile(runtimeTemplate, runtime_js, CopyType.IfNewer, $"runtime template <{runtimeTemplate}> ");
            else
            {
                var runtime_gen = "\nvar Module = {\n\tonRuntimeInitialized: function () {\n\t\tMONO.mono_load_runtime_and_bcl (\n\t\tconfig.vfs_prefix,\n\t\tconfig.deploy_prefix,\n\t\tconfig.enable_debugging,\n\t\tconfig.file_list,\n\t\tfunction () {\n\t\t\tconfig.add_bindings ();\n\t\t\tApp.init ();\n\t\t}\n\t)\n\t},\n};";
                File.Delete(runtime_js);
                File.WriteAllText(runtime_js, runtime_gen);
            }
        }

        

        string runtime_dir = Path.Combine(tool_prefix, use_release_runtime ? "release" : "debug");
        File.Delete(Path.Combine(out_prefix, "mono.js"));
        File.Delete(Path.Combine(out_prefix, "mono.wasm"));

        var source = Path.Combine(runtime_dir, "mono.js");
        var destination = Path.Combine(out_prefix, "mono.js");

        File.Copy (
                    Path.Combine (runtime_dir, "mono.js"),
                    Path.Combine (out_prefix, "mono.js"));
        File.Copy(
            Path.Combine(runtime_dir, "mono.wasm"),
            Path.Combine(out_prefix, "mono.wasm"));

        foreach (var asset in assets)
        {
            CopyFile(asset,
                Path.Combine(out_prefix, asset), copyType, "Asset: ");
        }
        */

        }
    }
}