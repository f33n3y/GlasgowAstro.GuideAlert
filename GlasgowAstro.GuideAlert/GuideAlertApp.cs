using GlasgowAstro.GuideAlert.Helpers;
using GlasgowAstro.GuideAlert.Interfaces;
using Microsoft.Extensions.Logging;
using System;
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
            ConsoleHelper.DisplayWelcomeMessage();
            logger.LogInformation("Guide alert app started.");

            try
            {
                ConsoleHelper.ConnectingToPhd();
                var phdTestSuccess = phdClient.ConnectAndTestAsync();
                ConsoleHelper.TestAlertNotify();
                var alertTestSuccess = slackClient.ConnectAndTestAsync();
               
                if (!await phdTestSuccess)  // TODO Change to waitall or waitany ?? 
                {
                    ConsoleHelper.PhdConnectionFailure();
                    ConsoleHelper.ProgramTerminated();
                    return;
                }

                if (!await alertTestSuccess)
                {
                    ConsoleHelper.TestAlertFailure();
                    ConsoleHelper.ProgramTerminated();
                    return;
                }
            }
            catch (Exception e)
            {
                logger.LogCritical(e, "Startup tests failed.");
                ConsoleHelper.ProgramTerminated();
                return;
            }

            // Tests successful so start monitoring event messages
            ConsoleHelper.PhdConnectionSuccess();
            ConsoleHelper.TestAlertSuccess();
            ConsoleHelper.MonitoringPhdEvents();
            //phdClient.WatchForStarLossEvents(); // TODO
        }
    }
}
