using JimengSDK;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Samples;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        services.AddJimengClient();
        // services.AddJiMengAIClient(options =>
        // {
        //     options.AccessKeyId = "AKxxxxx";
        //     options.AccessKeySecret = "xxxxx";
        // });
        services.AddTransient<JimengAIDemoService>();
    })
    .Build();

using var scope = host.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var demoService = services.GetRequiredService<JimengAIDemoService>();
    await demoService.RunDemoAsync();
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during demo execution.");
}