using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Web.Builders.Interfaces
{


    public interface IServiceSetup
    {
        void InstallService(IServiceCollection serviceCollection, IConfiguration configuration);
    }
}
