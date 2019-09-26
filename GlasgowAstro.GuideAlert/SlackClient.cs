using GlasgowAstro.GuideAlert.Interfaces;
using GlasgowAstro.GuideAlert.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text;

namespace GlasgowAstro.GuideAlert
{
    /// <summary>
    /// Client object used to send requests to a Slack webhook
    /// </summary>
    public class SlackClient : ISlackClient
    {
        private readonly GuideAlertSettings guideAlertSettings;
        private readonly ILogger<SlackClient> logger;
        private readonly IHttpClientFactory httpClientFactory;

        public SlackClient(GuideAlertSettings guideAlertSettings, ILogger<SlackClient> logger, 
            IHttpClientFactory httpClientFactory)
        {
            this.guideAlertSettings = guideAlertSettings;
            this.logger = logger;
            this.httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Sends POST request to Slack webhook url and checks for
        /// successful response (200).
        /// </summary>
        /// <returns>Boolean indicating successful response</returns>
        public bool ConnectAndTest()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(guideAlertSettings.SlackWebhookUrl))
                {
                    logger.LogInformation("No webhook URL found in config.");
                    return false;
                }

                var httpClient = httpClientFactory.CreateClient();
                httpClient.BaseAddress = new Uri(guideAlertSettings.SlackWebhookUrl);

                HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Post, "");
                var content = new StringContent("{\"text\":\"Test alert\"}", Encoding.UTF8, "application/json");

                var result = httpClient.PostAsync("", content)?.Result;
                return result?.StatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed to send test Slack notification.");            
            }
            return false;
        }

        public bool SendAlert(string alertMessage)
        {
            // TODO
            logger.LogInformation("Sending star lost alert.");


            return true;
        }
    }
}
