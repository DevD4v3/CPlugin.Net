namespace CPlugin.Net;

internal class PluginNameNormalizer
{
    public static string Normalize(string pluginName)
        => pluginName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase)
            ? pluginName
            : $"{pluginName}.dll";
}
