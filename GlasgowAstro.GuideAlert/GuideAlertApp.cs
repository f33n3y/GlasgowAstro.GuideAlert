using GlasgowAstro.GuideAlert.Helpers;
using GlasgowAstro.GuideAlert.Interfaces;
using GlasgowAstro.GuideAlert.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace GlasgowAstro.GuideAlert
{
    public class GuideAlertApp : IGuideAlertApp
    {
        private readonly GuideAlertSettings guideAlertSettings;
        private readonly ILogger<GuideAlertApp> logger;
        private readonly ISlackClient slackClient;
        private readonly IPhdClient phdClient;

        public GuideAlertApp(GuideAlertSettings guideAlertSettings, ILogger<GuideAlertApp> logger, 
            ISlackClient slackClient, IPhdClient phdClient)
        {
            this.guideAlertSettings = guideAlertSettings;
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

            if (await phdClient.WatchForStarLossEvents())  //TODO Exception handling
            {
                var alertResponse = await slackClient.SendAlert(new SlackWebhookRequest
                {
                    Text = guideAlertSettings?.AlertMessage,
                    IsTest = false
                });
                //TODO Check response
            }
            //TODO Option to star watching events again
        }
    }    
}
