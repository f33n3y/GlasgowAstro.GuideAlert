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
        private readonly GuideAlertSettings _guideAlertSettings;
        private readonly ILogger<GuideAlertApp> _logger;
        private readonly ISlackClient _slackClient;
        private readonly IPhdClient _phdClient;

        public GuideAlertApp(GuideAlertSettings guideAlertSettings, ILogger<GuideAlertApp> logger, 
            ISlackClient slackClient, IPhdClient phdClient)
        {
            _guideAlertSettings = guideAlertSettings;
            _logger = logger;
            _slackClient = slackClient;
            _phdClient = phdClient;
        }

        public async Task StartAsync()
        {
            ConsoleHelper.DisplayWelcomeMessage();
            _logger.LogInformation("Guide alert app started.");

            try
            {
                ConsoleHelper.ConnectingToPhd();
                var phdTestSuccess = _phdClient.ConnectAndTestAsync();
                ConsoleHelper.TestAlertNotify();
                var alertTestSuccess = _slackClient.ConnectAndTestAsync();

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
                _logger.LogCritical(e, "Startup tests failed.");
                ConsoleHelper.ProgramTerminated();
                return;
            }

            // Tests successful so start monitoring event messages
            ConsoleHelper.PhdConnectionSuccess();
            ConsoleHelper.TestAlertSuccess();
            ConsoleHelper.MonitoringPhdEvents();

            if (await _phdClient.WatchForStarLossEvents())  //TODO Exception handling
            {
                var alertResponse = await _slackClient.SendAlert(new SlackWebhookRequest
                {
                    Text = _guideAlertSettings?.AlertMessage,
                    IsTest = false
                });
                //TODO Check response
            }
            //TODO Option to star watching events again
        }
    }    
}
