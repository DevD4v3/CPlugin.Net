using CPlugin.Net;
using TestProject.Contracts;
using TestProject.GunGamePlugin;

[assembly: Plugin(typeof(GunGameMode))]
[assembly: DependsOn("TestProject.WeaponsPlugin")]

namespace TestProject.GunGamePlugin;

internal class GunGameMode(IEnumerable<IWeapon> weapons) : IGameMode
{
    public string Name => nameof(GunGameMode);

    public string ExecuteAction()
    {
        return string.Join(", ", weapons.Select(w => w.Name));
    }
}
