using GlasgowAstro.GuideAlert.Helpers;
using GlasgowAstro.GuideAlert.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Sockets;

namespace GlasgowAstro.GuideAlert
{
    class Program
    {
        private const string Host = "localhost";
        private const int Port = 4400;

        static void Main(string[] args)
        {
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
                ConsoleHelper.TestAlertNotify();

                SlackClient slackClient = new SlackClient(webhookUrl);         
                var canSendAlerts = slackClient.ConnectAndConfirm();

                if (!canSendAlerts)
                {
                    ConsoleHelper.TestAlertFailure();
                    ConsoleHelper.ProgramTerminated();
                    return;
                }

                ConsoleHelper.TestAlertSuccess();
                ConsoleHelper.ConnectingToPhd();

                TcpClient client = new TcpClient(Host, Port);
                StreamReader streamReader = new StreamReader(client.GetStream());
                var starLossCount = 0;

                while (true)
                {
                    var eventJson = streamReader.ReadLine();

                    if (!string.IsNullOrEmpty(eventJson))
                    {
                        Console.WriteLine(eventJson);

                        try
                        {
                            var phdEvent = JsonConvert.DeserializeObject<PhdEvent>(eventJson);

                            if (phdEvent != null && phdEvent.Event.Equals("StarLost"))
                            {
                                starLossCount++;
                                Console.WriteLine("STAR LOST! Sending notification...");
                                // TODO: Fire off notification...
                                //(rate limit, wait for X seconds of star loss? - user chooses duration)
                            }
                        }
                        catch (Exception e)
                        {
                            // TODO: Logging
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // TODO: Logging
            }
        }
    }
}
