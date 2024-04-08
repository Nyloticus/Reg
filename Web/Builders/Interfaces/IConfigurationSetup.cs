using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Web.Builders.Interfaces
{

    public interface IConfigurationSetup
    {
        void SetupConfiguration(IApplicationBuilder app, IWebHostEnvironment env = default);
    }
}