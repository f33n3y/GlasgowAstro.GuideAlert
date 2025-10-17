using GlasgowAstro.GuideAlert.Interfaces;
using GlasgowAstro.GuideAlert.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GlasgowAstro.GuideAlert
{
    /// <summary>
    /// Client object used to send requests to a Slack webhook
    /// </summary>
    public class SlackClient : ISlackClient, IAlertable
    {
        private readonly GuideAlertSettings _guideAlertSettings;
        private readonly ILogger<SlackClient> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public SlackClient(GuideAlertSettings guideAlertSettings, ILogger<SlackClient> logger,
            IHttpClientFactory httpClientFactory)
        {
            _guideAlertSettings = guideAlertSettings;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> ConnectAndTestAsync()
        {
            if (string.IsNullOrWhiteSpace(_guideAlertSettings?.SlackWebhookUrl))
            {
                _logger.LogCritical("No webhook URL found in config. This is required to send alerts.");
                return false;
            }

            var alertResponse = await SendAlert(new SlackWebhookRequest
            {
                Text = _guideAlertSettings.TestAlertMessage,
                IsTest = true
            });

            return alertResponse?.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// Sends POST request to Slack webhook URL.
        /// </summary>
        /// <param name="webhookRequest"></param>
        /// <returns>A HTTP response message</returns>
        public async Task<HttpResponseMessage> SendAlert(SlackWebhookRequest webhookRequest)
        {
            _logger.LogInformation("Sending alert.");

            var httpClient = _httpClientFactory.CreateClient();                                  
            httpClient.BaseAddress = new Uri(_guideAlertSettings.SlackWebhookUrl);

            var webhookJson = JsonConvert.SerializeObject(webhookRequest);

            var content = new StringContent(webhookJson, Encoding.UTF8, "application/json");

            return await httpClient.PostAsync(string.Empty, content);
        }
    }
}
