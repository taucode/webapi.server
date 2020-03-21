using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using TauCode.WebApi.Server.Tests.AppHost;
using MicrosoftHost = Microsoft.Extensions.Hosting.Host;

namespace TauCode.WebApi.Server.Tests
{
    public class TestFactory : WebApplicationFactory<Startup>
    {
        //protected override IWebHostBuilder CreateWebHostBuilder()
        //{
        //    return WebHost
        //        .CreateDefaultBuilder()
        //        .UseStartup<TestStartup>();
        //}

        protected override IHostBuilder CreateHostBuilder() =>
            MicrosoftHost.CreateDefaultBuilder()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
