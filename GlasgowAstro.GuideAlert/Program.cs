using System;
using System.IO;
using GlasgowAstro.GuideAlert.Interfaces;
using GlasgowAstro.GuideAlert.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;

namespace GlasgowAstro.GuideAlert
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Configure services
            IServiceProvider serviceProvider = ConfigureServices();

            // Lift off!
            IGuideAlertApp app = serviceProvider.GetRequiredService<IGuideAlertApp>();
            app.Start();
        }

        private static IServiceProvider ConfigureServices()
        {
            IServiceCollection serviceCollection = new ServiceCollection(); 

            // Build config and bind to GuideAlertSettings
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .Build();

            serviceCollection.Configure<GuideAlertSettings>(config.GetSection("GuideAlertSettings"));
            serviceCollection.AddSingleton(resolver => resolver.GetRequiredService<IOptions<GuideAlertSettings>>().Value);

            // Configure logging
            Log.Logger = new LoggerConfiguration()
                 .ReadFrom.Configuration(config)
                 .CreateLogger();
            
            serviceCollection.AddLogging(configure => configure.AddSerilog());

            // Add other services
            serviceCollection.AddHttpClient();   
            serviceCollection.AddSingleton<ISlackClient, SlackClient>();
            serviceCollection.AddSingleton<IPhdClient, PhdClient>();
            serviceCollection.AddSingleton<IGuideAlertApp, GuideAlertApp>();            

            return serviceCollection.BuildServiceProvider();
        }
    }
}
