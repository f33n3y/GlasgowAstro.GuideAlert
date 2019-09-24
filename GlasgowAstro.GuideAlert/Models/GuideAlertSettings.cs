namespace GlasgowAstro.GuideAlert.Models
{
    public class GuideAlertSettings
    {
        public string SlackWebhookUrl { get; set; }

        public string AlertMessage { get; set; }

        public string LossCountThreshold { get; set; }
    }
}
