using Microsoft.Extensions.DependencyInjection;

namespace TestProject.Contracts;

public interface IWeaponPlugin
{
    void ConfigureServices(IServiceCollection services);
}
