using Microsoft.Extensions.DependencyInjection;

namespace Example.Contracts;

public interface IPlugin
{
    void ConfigureServices(IServiceCollection services);
}
