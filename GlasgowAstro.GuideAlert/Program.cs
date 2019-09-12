using GlasgowAstro.GuideAlert.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Settings.Configuration;
using Serilog;
using System;
using GlasgowAstro.GuideAlert.Interfaces;

namespace GlasgowAstro.GuideAlert
{
    class Program
    {
        private const string Host = "localhost"; // TODO: Read from config
        private const int Port = 4400; // TODO: Read from config

        static void Main(string[] args)
        {
            // Configure services
            var serviceProvider = ConfigureServices();

            // Lift off!...
            var app = serviceProvider.GetRequiredService<IGuideAlertApp>();
            app.Start();
           
        }

        private static IServiceProvider ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<ISlackClient, SlackClient>();
            serviceCollection.AddTransient<IPhdClient, PhdClient>();
            serviceCollection.AddSingleton<IGuideAlertApp, GuideAlertApp>();

            return serviceCollection.BuildServiceProvider();
            
            // TODO: Configure logger
        }

        private static IConfiguration BuildConfiguration()
        {
            throw new NotImplementedException();
            //return new ConfigurationBuilder()
            //    .AddYamlFile("appsettings.yaml")
            //    .Build());
        }

        public static ILogger BuildLogger(IServiceProvider provider)
        {
            throw new NotImplementedException();
            //return new LoggerConfiguration()
            //    .ReadFrom.Configuration(provider.GetRequiredService<IConfiguration>())
            //    .CreateLogger();
        }
    }
}
