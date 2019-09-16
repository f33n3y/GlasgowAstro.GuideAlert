using GlasgowAstro.GuideAlert.Interfaces;
using Microsoft.Extensions.Logging;
using System;

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

        public void Start()
        {
            Console.WriteLine("Starting...");
            Console.ReadLine();

            logger.LogInformation("Logging test");

            #region TODO
            //// Prompt user for webhook url
            //ConsoleHelper.DisplayWelcomeMessages();
            //ConsoleHelper.PromptUserForWebhookUrl();

            //if (string.IsNullOrWhiteSpace(webhookUrl))
            //{
            //    ConsoleHelper.InvalidWebhookUrl();
            //    ConsoleHelper.ProgramTerminated();
            //    return;
            //}

            //try
            //{
            //    // Try send test alert to Slack
            //    ConsoleHelper.TestAlertNotify();
            //    SlackClient slackClient = new SlackClient(webhookUrl);
                //var canSendAlerts = slackClient.ConnectAndTest();

            //    if (!canSendAlerts) // Test alert fail
            //    {
            //        ConsoleHelper.TestAlertFailure();
            //        ConsoleHelper.ProgramTerminated();
            //        return;
            //    }

            //    // Test alert success. Try get data from PHD stream
            //    ConsoleHelper.TestAlertSuccess();
            //    ConsoleHelper.ConnectingToPhd();
            //    PhdClient phdClient = new PhdClient(Host, Port);
            //    var canReadPhdEvents = phdClient.ConnectAndTest();

            //    if (!canReadPhdEvents) // PHD connection fail
            //    {
            //        ConsoleHelper.PhdConnectionFailure();
            //        ConsoleHelper.ProgramTerminated();
            //    }

            //    // PHD connection success. Start monitoring event messages
            //    ConsoleHelper.PhdConnectionSuccess();
            //    phdClient.WatchForStarLossEvents();

            //    // TODO: ...
            //}
            //catch (Exception e)
            //{
            //    // TODO: Logging
            //}
            #endregion
        }
    }
}
