using System;

namespace GlasgowAstro.GuideAlert.Helpers
{
    public static class ConsoleHelper
    {
        public static void SetConsoleColours()
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }

        public static void DisplayWelcomeMessages()
        {
            Console.WriteLine("*** Guide Alert by GlasgowAstro *** \n");
            Console.WriteLine("Make sure PHD is looping exposures and server is enabled before continuing");
            Console.WriteLine("Press any key to continue...\n");
            Console.ReadKey(true);
        }

        public static void TestAlertNotify() => Console.WriteLine("Sending test alert");

        public static void TestAlertSuccess() => Console.WriteLine("Test alert successful");

        public static void TestAlertFailure() => Console.WriteLine("Test alert failed");

        public static void ConnectingToPhd() => Console.WriteLine("Connecting to PHD server");

        public static void PhdConnectionSuccess() => Console.WriteLine("Successfully connected to PHD server");

        public static void PhdConnectionFailure() => Console.WriteLine("Failed to connect to PHD server");

        public static void StarLostWarning() => Console.WriteLine("*** WARNING: STAR LOST ***");

        public static void ProgramTerminated()
        {
            Console.WriteLine("Guide Alert terminated. Check logs for more information.");
            Console.ReadKey(true);
        }

    }
}
