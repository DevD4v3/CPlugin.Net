namespace CPlugin.Net.Tests.Core;

public class PluginLoaderTests
{
    private class Info
    {
        public string Version { get; set; }
    }

    [Test]
    public void Load_WhenPluginsAreFound_ShouldBeLoadedIntoMemory()
    {
        // Arrange
        var value =
        """
        TestProject.OldJsonPlugin.dll
        TestProject.JsonPlugin.dll
        """;
        Environment.SetEnvironmentVariable("PLUGINS", value);
        var envConfiguration = new CPluginEnvConfiguration();
        int expectedCommands = 2;
        var expectedVersions = new[]
        {
            "Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed",
            "Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed"
        };

        // Act
        PluginLoader.Load(envConfiguration);
        var commands = TypeFinder.FindSubtypesOf<ICommand>();
        var versions = commands.Select(command =>
        {
            var json = command.Execute();
            return JsonConvert.DeserializeObject<Info>(json).Version;
        }).ToArray();

        // Asserts
        commands.Should().HaveCount(expectedCommands);
        versions.Should().BeEquivalentTo(expectedVersions);
    }

    [Test]
    public void Load_WhenArgumentIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        CPluginEnvConfiguration configuration = default;

        // Act
        Action act = () => PluginLoader.Load(configuration);

        // Assert
        act.Should()
           .Throw<ArgumentNullException>()
           .WithParameterName(nameof(configuration));
    }

    [Test]
    public void Load_WhenMethodIsCalledMultipleTimes_ShouldNotLoadSamePluginsIntoMemory()
    {
        // Arrange
        var value = "TestProject.HelloPlugin.dll";
        Environment.SetEnvironmentVariable("PLUGINS", value);
        var envConfiguration = new CPluginEnvConfiguration();
        int expectedAssemblies = 1;

        // Act
        for (int i = 0; i < 10; i++) 
        {
            // This operation is idempotent.
            PluginLoader.Load(envConfiguration);
        }

        // Assert
        AppDomain
            .CurrentDomain
            .GetAssemblies()
            .Where(assembly => assembly.GetName().Name == "TestProject.HelloPlugin")
            .Count()
            .Should()
            .Be(expectedAssemblies);
    }

    [Test]
    public void Load_WhenPluginDependencyIsNotConfigured_ShouldThrowPluginDependencyException()
    {
        // Arrange
        var value = "TestProject.GunGamePlugin.dll";
        Environment.SetEnvironmentVariable("PLUGINS", value);
        var configuration = new CPluginEnvConfiguration();

        // Act
        Action act = () => PluginLoader.Load(configuration);

        // Assert
        act.Should()
           .Throw<PluginDependencyException>()
           .WithMessage("*TestProject.WeaponsPlugin.dll*");
    }

    [Test]
    public void Load_WhenPluginDependencyIsResolved_ShouldLoadPluginsSuccessfully()
    {
        // Arrange
        var value =
        """
        TestProject.WeaponsPlugin.dll
        TestProject.GunGamePlugin.dll
        """;

        Environment.SetEnvironmentVariable("PLUGINS", value);
        var configuration = new CPluginEnvConfiguration();
        var services = new ServiceCollection();

        // Act
        PluginLoader.Load(configuration);

        var weaponPlugins = TypeFinder.FindSubtypesOf<IWeaponPlugin>();
        foreach (IWeaponPlugin weaponPlugin in weaponPlugins)
        {
            weaponPlugin.ConfigureServices(services);
        }

        services.AddSubtypesOf<IGameMode>(ServiceLifetime.Transient);

        using ServiceProvider serviceProvider = services.BuildServiceProvider();
        var gameModes = serviceProvider.GetServices<IGameMode>().ToArray();

        // Assert
        string weapons = gameModes[0].ExecuteAction();
        weapons.Should().Be("Pistol, AK-47");
    }

    [Test]
    public void Load_WhenPluginIsNotFound_ShouldThrowPluginNotFoundException()
    {
        // Arrange
        var value = "Example.EconomyPlugin.dll";
        Environment.SetEnvironmentVariable("PLUGINS", value);
        var configuration = new CPluginEnvConfiguration();

        // Act
        Action act = () => PluginLoader.Load(configuration);

        // Assert
        act.Should()
           .Throw<PluginNotFoundException>()
           .WithMessage("*Example.EconomyPlugin.dll*");
    }
}
