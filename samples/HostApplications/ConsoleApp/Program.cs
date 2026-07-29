// See https://aka.ms/new-console-template for more information

new EnvLoader().Load();
Console.WriteLine("PLUGINS=" + EnvReader.Instance["PLUGINS"]);
Console.WriteLine();

var envConfiguration = new CPluginEnvConfiguration();
// Loads the plugins from the .env file.
PluginLoader.Load(envConfiguration);

var services = new ServiceCollection();
var plugins = TypeFinder.FindSubtypesOf<IPlugin>();
foreach (IPlugin plugin in plugins)
    plugin.ConfigureServices(services);

services.AddSubtypesOf<ICommand>(ServiceLifetime.Transient);

var serviceProvider = services.BuildServiceProvider();
var commands = serviceProvider.GetServices<ICommand>();
foreach (ICommand command in commands)
    command.Execute();
