using System;
using System.Reflection;
using System.Runtime.Loader;

namespace CPlugin.Net;

internal class PluginLoadContext : AssemblyLoadContext
{
    private readonly AssemblyDependencyResolver _resolver;

    public PluginLoadContext(string pluginPath)
    {
        if (!File.Exists(pluginPath))
        {
            var message =
            $"""
             The plugin '{Path.GetFileName(pluginPath)}' was not found at:

             {pluginPath}

             Ensure the plugin project has been built and the assembly exists in the plugins directory.
             """;

            throw new PluginNotFoundException(message);
        }

        _resolver = new AssemblyDependencyResolver(pluginPath);
    }

    protected override Assembly Load(AssemblyName assemblyName)
    {
        var assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
        return assemblyPath is null ? default : LoadFromAssemblyPath(assemblyPath);
    }

    protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
    {
        var libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
        return libraryPath is null ? IntPtr.Zero : LoadUnmanagedDllFromPath(libraryPath);
    }
}
