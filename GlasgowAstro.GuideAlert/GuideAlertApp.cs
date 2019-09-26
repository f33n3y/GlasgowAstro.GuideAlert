using GlasgowAstro.GuideAlert.Helpers;
using GlasgowAstro.GuideAlert.Interfaces;
using Microsoft.Extensions.Logging;

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
            logger.LogInformation("Guide alert app started.");
            ConsoleHelper.SetConsoleColours();
            ConsoleHelper.DisplayWelcomeMessages();

            // Try send test alert to Slack.
            ConsoleHelper.TestAlertNotify();
            if (!slackClient.ConnectAndTest())
            {
                ConsoleHelper.TestAlertFailure();
                ConsoleHelper.ProgramTerminated();
                return;
            }

            // Test alert success. Now try get data from Phd stream.
            ConsoleHelper.TestAlertSuccess();
            ConsoleHelper.ConnectingToPhd();

            if (!phdClient.ConnectAndTest())
            {
                ConsoleHelper.PhdConnectionFailure();
                ConsoleHelper.ProgramTerminated();
                return;
            }

            // Phd connection success. Start monitoring event messages.
            ConsoleHelper.PhdConnectionSuccess();
            //phdClient.WatchForStarLossEvents();
        }
    }
}
