using FunctionApp.IsolatedDemo.Api;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        var configuration = new ConfigurationBuilder()
                 .SetBasePath(Environment.CurrentDirectory)
                 .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                 .AddEnvironmentVariables()
                 .Build();

        services.AddApiServices(configuration);
    })
    .Build();

await host.RunAsync();

/// <summary>
/// For the integration Tests Project
/// </summary>
public partial class Program { }