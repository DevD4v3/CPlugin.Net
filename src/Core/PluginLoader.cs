using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CPlugin.Net;

/// <summary>
/// Represents the loader of plug-in assemblies.
/// </summary>
public static class PluginLoader
{
    private readonly static ConcurrentDictionary<string, Assembly> s_assemblies = new();

    /// <summary>
    /// Gets the plugin assemblies.
    /// </summary>
    public static IEnumerable<Assembly> Assemblies => s_assemblies.Values;

    /// <summary>
    /// Loads plugins from a configuration source specified by the <c>configuration</c> parameter.
    /// This means that the plugin names can be obtained from a json or .env file.
    /// </summary>
    /// <param name="configuration">A configuration source to get the plugin files.</param>
    /// <remarks>
    /// This method is idempotent, so if this method is called N times, 
    /// it will not reload assemblies that have already been loaded.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <c>configuration</c> is <c>null</c>.
    /// </exception>
    /// <exception cref="PluginDependencyException">
    /// A plugin declares a dependency that cannot be resolved.
    /// </exception>
    public static void Load(CPluginConfigurationBase configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        string[] pluginFiles = [.. configuration.GetPluginFiles()];
        HashSet<string> configuredPluginNames = pluginFiles
            .Select(Path.GetFileName)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (string pluginFile in pluginFiles)
        {
            Assembly assembly = FindAssembly(pluginFile);

            if (assembly is not null)
                continue;

            assembly = LoadAssembly(pluginFile);
            DependsOnAttribute[] dependencies = [.. assembly.GetCustomAttributes<DependsOnAttribute>()];

            foreach (DependsOnAttribute dependency in dependencies)
            {
                var normalizedPluginName = PluginNameNormalizer.Normalize(dependency.PluginName);
                if (!configuredPluginNames.Contains(normalizedPluginName))
                {
                    var message = 
                        $"The plugin '{assembly.GetName().Name}' depends on '{normalizedPluginName}', " +
                        $"but '{normalizedPluginName}' was not found in the plugin configuration.";

                    throw new PluginDependencyException(message);
                }
            }
        }
    }

    private static Assembly LoadAssembly(string assemblyFile)
    {
        var loadContext = new PluginLoadContext(assemblyFile);
        var assemblyName = AssemblyName.GetAssemblyName(assemblyFile);
        var currentAssembly = loadContext.LoadFromAssemblyName(assemblyName);
        s_assemblies.TryAdd(assemblyFile, currentAssembly);
        PluginLogger.DefaultLogInformation(currentAssembly.GetName().Name, currentAssembly.FullName);
        return currentAssembly;
    }

    private static Assembly FindAssembly(string assemblyFile)
    {
        s_assemblies.TryGetValue(assemblyFile, out Assembly assembly);
        return assembly;
    }
}
