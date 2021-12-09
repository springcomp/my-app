using SpringComp.Functions.Extensions.StaticWebApps;

[assembly: FunctionsStartup(typeof(Startup))]

public class Startup : FunctionsStartup
{
  public override void Configure(IFunctionsHostBuilder builder)
  {
    ConfigureServices(builder.Services);
  }

  private void ConfigureServices(IServiceCollection services)
  {
    // retrieve IConfiguration instance if necessary
    // to configure the Function App

    var configuration = services.GetConfiguration();

    services.AddHttpClient("client", (provider, client) =>
    {
      client.DefaultRequestHeaders.Add("Accept", "application/json");
      client.DefaultRequestHeaders.Add("User-Agent", "dotnet-core/3.1");
    });

    services.AddStaticWebApps();
  }
}

public static class IServiceCollectionConfigurationExtensions
{
  public static IConfiguration GetConfiguration(this IServiceCollection services)
  {
    return services.BuildServiceProvider().GetService<IConfiguration>();
  }
}