using CPlugin.Net;
using Microsoft.Extensions.DependencyInjection;
using TestProject.Contracts;
using TestProject.WeaponsPlugin;

[assembly: Plugin(typeof(WeaponsPlugin))]

namespace TestProject.WeaponsPlugin;

internal class WeaponsPlugin : IWeaponPlugin
{
    public string Name => nameof(WeaponsPlugin);

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IWeapon, Pistol>();
        services.AddSingleton<IWeapon, Ak47>();
    }
}
