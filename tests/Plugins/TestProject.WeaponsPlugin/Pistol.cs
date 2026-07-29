using TestProject.Contracts;

namespace TestProject.WeaponsPlugin;

internal class Pistol : IWeapon
{
    public string Id => "pistol";
    public string Name => "Pistol";
    public int Slot => 1;
}
