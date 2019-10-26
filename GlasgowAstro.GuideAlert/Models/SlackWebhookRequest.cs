using Newtonsoft.Json;

namespace GlasgowAstro.GuideAlert.Models
{
    public class SlackWebhookRequest
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonIgnore]
        public bool IsTest { get; set; }
    }
}
