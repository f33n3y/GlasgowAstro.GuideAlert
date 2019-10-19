using GlasgowAstro.GuideAlert.Helpers;
using GlasgowAstro.GuideAlert.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GlasgowAstro.GuideAlert
{
    public class GuideAlertApp : IGuideAlertApp
    {
        private readonly ILogger<GuideAlertApp> logger;
        private readonly ISlackClient slackClient;
        private readonly IPhdClient phdClient;

        public GuideAlertApp(ILogger<GuideAlertApp> logger, ISlackClient slackClient, IPhdClient phdClient)
        {
            this.logger = logger;
            this.slackClient = slackClient;
            this.phdClient = phdClient;
        }

        public async Task StartAsync()
        {
            logger.LogInformation("Guide alert app started.");
            ConsoleHelper.SetConsoleColours();
            ConsoleHelper.DisplayWelcomeMessages();

            try
            {
                // Test Slack webhook and connection to PHD stream
                var slackTest = slackClient.ConnectAndTestAsync();
                var phdTest = phdClient.ConnectAndTestAsync();

                await Task.WhenAll(slackTest, phdTest);
            }
            catch (AggregateException aggEx)
            {
                foreach (var ex in aggEx.InnerExceptions)
                {
                    if (ex.Source.Equals(slackClient))
                    {
                        ConsoleHelper.TestAlertFailure();
                        logger.LogCritical(ex, "Slack test alert failed. Check logs for more info.");
                    }
                    if (ex.Source.Equals(phdClient))
                    {
                        ConsoleHelper.PhdConnectionFailure();
                        logger.LogCritical(ex, "Failed to connect to PHD stream. Check logs for more info.");
                    }
                }

                ConsoleHelper.ProgramTerminated();
                return;
            }
         
            // Tests successful so start monitoring event messages...       

            //ConsoleHelper.ConnectingToPhd();
            //phdClient.WatchForStarLossEvents();              
        }
    }
}
