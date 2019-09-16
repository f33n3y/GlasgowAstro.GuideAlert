using GlasgowAstro.GuideAlert.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.IO;

namespace GlasgowAstro.GuideAlert
{
    class Program
    {

        static void Main(string[] args)
        {
            // Configure services
            var serviceProvider = ConfigureServices();

            // Lift off!
            var app = serviceProvider.GetRequiredService<IGuideAlertApp>();       
            app.Start();
        }

        private static IServiceProvider ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();

            // Build config
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .Build();
           
            // Configure logging
            Log.Logger = new LoggerConfiguration()
                 .ReadFrom.Configuration(config)
                 .CreateLogger();
            serviceCollection.AddLogging(loggingBuilder => loggingBuilder.AddSerilog());

            serviceCollection.AddSingleton<ISlackClient, SlackClient>();
            serviceCollection.AddSingleton<IPhdClient, PhdClient>();
            serviceCollection.AddSingleton<IGuideAlertApp, GuideAlertApp>();

            return serviceCollection.BuildServiceProvider();
        }
    }
}
