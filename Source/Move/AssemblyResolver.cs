/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

namespace Dolittle.WebAssembly.Packager
{
    /// <summary>
    /// 
    /// </summary>
    public class AssemblyResolver
    {

        static string FindFrameworkAssembly(string asm)
        {
            return asm;
        }

        static bool Try(string prefix, string name, out string out_res)
        {
            out_res = null;

            string res = (Path.Combine(prefix, name));
            if (File.Exists(res))
            {
                out_res = Path.GetFullPath(res);
                return true;
            }
            return false;
        }

        static string ResolveWithExtension(string prefix, string name)
        {
            string res = null;

            if (Try(prefix, name, out res))
                return res;
            if (Try(prefix, name + ".dll", out res))
                return res;
            if (Try(prefix, name + ".exe", out res))
                return res;
            return null;
        }

        static string ResolveUser(string asm_name)
        {
            return ResolveWithExtension(app_prefix, asm_name);
        }

        static string ResolveFramework(string asm_name)
        {
            return ResolveWithExtension(framework_prefix, asm_name);
        }

        static string ResolveBcl(string asm_name)
        {
            return ResolveWithExtension(bcl_prefix, asm_name);
        }

        static string ResolveBclFacade(string asm_name)
        {
            return ResolveWithExtension(bcl_facades_prefix, asm_name);
        }

        static string Resolve(string asm_name, out AssemblyKind kind)
        {
            kind = AssemblyKind.User;
            var asm = ResolveUser(asm_name);
            if (asm != null)
                return asm;

            kind = AssemblyKind.Framework;
            asm = ResolveFramework(asm_name);
            if (asm != null)
                return asm;

            kind = AssemblyKind.Bcl;
            asm = ResolveBcl(asm_name);
            if (asm == null)
                asm = ResolveBclFacade(asm_name);
            if (asm != null)
                return asm;

            kind = AssemblyKind.User;
            var assembly = _assemblies.SingleOrDefault(_ => _.GetName().Name == asm_name);
            if (assembly != null)
                return new Uri(assembly.CodeBase).AbsolutePath;

            kind = AssemblyKind.None;
            return string.Empty;
        }

        static void Import(string ra, AssemblyKind kind)
        {
            if (string.IsNullOrEmpty(ra)) return;

            if (!asm_map.Add(ra))
                return;
            ReaderParameters rp = new ReaderParameters();
            bool add_pdb = enable_debug && File.Exists(Path.ChangeExtension(ra, "pdb"));
            if (add_pdb)
            {
                rp.ReadSymbols = true;
            }

            rp.InMemory = true;
            var image = ModuleDefinition.ReadModule(ra, rp);
            file_list.Add(ra);
            Debug($"Processing {ra} debug {add_pdb}");

            var data = new AssemblyDetails() { Name = image.Assembly.Name.Name, SourcePath = ra };
            assemblies.Add(data);

            if (add_pdb && kind == AssemblyKind.User)
                file_list.Add(Path.ChangeExtension(ra, "pdb"));

            foreach (var ar in image.AssemblyReferences)
            {
                var resolve = Resolve(ar.Name, out kind);
                if (kind != AssemblyKind.None) Import(resolve, kind);
            }
        }

    }
}