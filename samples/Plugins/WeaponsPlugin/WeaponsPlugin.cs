[assembly: Plugin(typeof(WeaponsPlugin))]

namespace Example.WeaponsPlugin;

internal class WeaponsPlugin : IPlugin
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IWeapon, Pistol>();
        services.AddSingleton<IWeapon, Ak47>();
    }
}
