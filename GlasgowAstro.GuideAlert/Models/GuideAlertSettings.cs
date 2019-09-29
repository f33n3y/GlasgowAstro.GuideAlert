namespace GlasgowAstro.GuideAlert.Models
{
    public class GuideAlertSettings
    {
        public string SlackWebhookUrl { get; set; }

        public string AlertMessage { get; set; }

        public int LossCountThreshold { get; set; }

        public string PhdHost { get; set; }

        public ushort PhdPort { get; set; }
    }
}
