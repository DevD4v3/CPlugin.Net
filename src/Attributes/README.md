# CPlugin.Net.Attributes

A simple library that includes the attributes used by CPlugin.Net plugins.

- [`PluginAttribute`](https://DevD4v3.github.io/CPlugin.Net/api/CPlugin.Net.PluginAttribute.html)
- [`DependsOnAttribute`](https://DevD4v3.github.io/CPlugin.Net/api/CPlugin.Net.DependsOnAttribute.html)

## Plugin

`IPluginStartup` represents the contract and can reside in its own project called `MyApp.Contracts`.

Each plugin must implement the contract in this way:

```cs
internal class Startup : IPluginStartup
{
}
```

And then add this line before the namespace declaration:

```cs
[assembly: Plugin(typeof(Startup))]
```

**Complete example:**

```cs
using Project.MyPlugin1;
using MyApp.Contracts;
using CPlugin.Net;

[assembly: Plugin(typeof(Startup))]

namespace Project.MyPlugin1;

internal class Startup : IPluginStartup
{
}
```

## DependsOn

If a plugin depends on another plugin, declare the dependency before the namespace declaration.

```cs
[assembly: DependsOn("WeaponsPlugin")]
```

Multiple dependencies can be declared by applying the attribute multiple times.

```cs
[assembly: DependsOn("WeaponsPlugin")]
[assembly: DependsOn("EconomyPlugin")]
```

When the plugin is loaded, all declared dependencies must also be present in the plugin configuration; otherwise, `PluginDependencyException` is thrown.