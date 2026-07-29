using System;

namespace CPlugin.Net;

/// <summary>
/// Specifies that the current plugin depends on another plugin.
/// </summary>
/// <remarks>
/// Example:
/// <para>
/// If <c>GunGame</c> depends on <c>Weapons</c>, add this line before the namespace declaration:
/// </para>
/// <c>[assembly: DependsOn("Weapons")]</c>
/// <para>
/// During plugin loading, the specified plugin must also be present in the plugin
/// configuration; otherwise, a <c>PluginDependencyException</c> is thrown.
/// </para>
/// </remarks>
[AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = true)]
public class DependsOnAttribute : Attribute
{
    /// <summary>
    /// Gets the name of the required plugin.
    /// </summary>
    public string PluginName { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DependsOnAttribute"/> class.
    /// </summary>
    /// <param name="pluginName">
    /// The name of the plugin that the current plugin depends on.
    /// The <c>.dll</c> extension is optional.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <c>pluginName</c> is <c>null</c>.
    /// </exception>
    public DependsOnAttribute(string pluginName)
    {
        if (pluginName is null)
        {
            throw new ArgumentNullException(nameof(pluginName));
        }

        PluginName = pluginName;
    }
}
