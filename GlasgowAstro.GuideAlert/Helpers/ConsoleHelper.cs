﻿using System;

namespace GlasgowAstro.GuideAlert.Helpers
{
    public static class ConsoleHelper
    {
        public static void DisplayWelcomeMessages()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("*** Guide Alert by GlasgowAstro *** \n");
            Console.WriteLine("Make sure PHD2 is looping exposures and server is enabled before continuing");
            Console.WriteLine("Press any key to continue...\n");
            Console.ReadKey(true);
        }

        public static void PromptUserForWebhookUrl() => Console.WriteLine("Enter your Slack webhook URL: \n");

        public static void InvalidWebhookUrl() => Console.WriteLine("Invalid webhook URL");

        public static void TestAlertNotify() => Console.WriteLine("Sending test alert");

        public static void TestAlertSuccess() => Console.WriteLine("Test alert successful");

        public static void TestAlertFailure() => Console.WriteLine("Test alert failed");

        public static void ConnectingToPhd() => Console.WriteLine("Connecting to PHD2 server");

        public static void ProgramTerminated()
        {
            Console.WriteLine("Guide Alert terminated");
            Console.ReadKey(true);
        }

    }
}