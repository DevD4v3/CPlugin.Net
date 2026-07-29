using System;

namespace CPlugin.Net;

/// <summary>
/// The exception that is thrown when a plugin dependency cannot be resolved.
/// </summary>
public class PluginDependencyException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PluginDependencyException"/> class.
    /// </summary>
    /// <param name="message">
    /// The message that describes the error.
    /// </param>
    public PluginDependencyException(string message) : base(message)
    {
    }
}
