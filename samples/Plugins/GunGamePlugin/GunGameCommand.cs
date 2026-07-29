[assembly: Plugin(typeof(GunGameCommand))]
[assembly: DependsOn("Example.WeaponsPlugin")]

namespace Example.GunGamePlugin;

internal class GunGameCommand(IEnumerable<IWeapon> weapons) : ICommand
{
    public string Name => "gungame";
    public string Description => "Lists the available weapons.";
    public string Version => "1.0.0";

    public int Execute()
    {
        Console.WriteLine();
        Console.WriteLine("=== GunGame Plugin ===");
        Console.WriteLine($"Command     : {Name}");
        Console.WriteLine($"Description : {Description}");
        Console.WriteLine($"Version     : {Version}");
        Console.WriteLine();
        Console.WriteLine("Registered weapons:");

        foreach (IWeapon weapon in weapons.OrderBy(w => w.Slot))
        {
            Console.WriteLine($"  [{weapon.Slot}] {weapon.Name} (Id: {weapon.Id})");
        }

        Console.WriteLine();
        return 0;
    }
}
