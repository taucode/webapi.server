namespace TauCode.WebApi.Server.Tests.AppHost;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        throw new NotImplementedException();
        //return Host.CreateDefaultBuilder(args)
        //    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
        //    .ConfigureWebHostDefaults(webBuilder =>
        //    {
        //        webBuilder.UseStartup<Startup>();
        //    });
    }
}