using GlasgowAstro.GuideAlert.Helpers;
using System;

namespace GlasgowAstro.GuideAlert
{
    class Program
    {
        private const string Host = "localhost"; // TODO: Read from config
        private const int Port = 4400; // TODO: Read from config

        static void Main(string[] args)
        {
            // Prompt user for webhook url
            ConsoleHelper.DisplayWelcomeMessages();
            ConsoleHelper.PromptUserForWebhookUrl();
            var webhookUrl = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(webhookUrl))
            {
                ConsoleHelper.InvalidWebhookUrl();
                ConsoleHelper.ProgramTerminated();
                return;
            }

            try
            {
                // Try send test alert to Slack
                ConsoleHelper.TestAlertNotify();
                SlackClient slackClient = new SlackClient(webhookUrl);
                var canSendAlerts = slackClient.ConnectAndTest();

                if (!canSendAlerts) // Test alert fail
                {
                    ConsoleHelper.TestAlertFailure();
                    ConsoleHelper.ProgramTerminated();
                    return;
                }

                // Test alert success. Try get data from PHD stream
                ConsoleHelper.TestAlertSuccess();
                ConsoleHelper.ConnectingToPhd();
                PhdClient phdClient = new PhdClient(Host, Port);
                var canReadPhdEvents = phdClient.ConnectAndTest();

                if (!canReadPhdEvents) // PHD connection fail
                {
                    ConsoleHelper.PhdConnectionFailure();
                    ConsoleHelper.ProgramTerminated();
                }

                // PHD connection success. Start monitoring event messages
                ConsoleHelper.PhdConnectionSuccess();
                phdClient.WatchForStarLossEvents();

                // TODO: ...
            }
            catch (Exception e)
            {
                // TODO: Logging
            }
        }
    }
}
