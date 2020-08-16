using System;
using System.IO;
using System.Reflection;

namespace Bannerlord.ButterLib.Assemblies
{
    /// <summary>
    /// Proxy for calling <see cref="Assembly.LoadFile(string)"/> of a non-default <see cref="AppDomain"/>.
    /// </summary>
    internal sealed class AssemblyLoaderProxy : MarshalByRefObject, IAssemblyLoader
    {
        public AssemblyLoaderProxy()
        {
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
        }
        ~AssemblyLoaderProxy()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= OnAssemblyResolve;
        }

        private static Assembly? OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.FullName == args.Name)
                {
                    return assembly;
                }
            }
            return null;
        }

        public void LoadFile(string path)
        {
            ValidatePath(path);

            Assembly.LoadFile(path);
        }

        public bool LoadFileAndTest(string path)
        {
            ValidatePath(path);

            var assembly = Assembly.LoadFile(path);
            try
            {
                assembly.GetTypes();
                return true;
            }
            catch (Exception e) when (e is ReflectionTypeLoadException)
            {
                return false;
            }
        }

        private static void ValidatePath(string path)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            if (!File.Exists(path)) throw new ArgumentException($"path \"{path}\" does not exist");
        }
    }
}