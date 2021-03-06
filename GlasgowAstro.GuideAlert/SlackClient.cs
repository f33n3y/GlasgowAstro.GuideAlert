﻿using GlasgowAstro.GuideAlert.Interfaces;
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

        public async Task<bool> ConnectAndTestAsync()
        {
            if (string.IsNullOrWhiteSpace(guideAlertSettings?.SlackWebhookUrl))
            {
                logger.LogCritical("No webhook URL found in config. This is required to send alerts.");
                return false;
            }

            var alertResponse = await SendAlert(new SlackWebhookRequest
            {
                Text = guideAlertSettings.TestAlertMessage,
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
            logger.LogInformation("Sending alert.");

            var httpClient = httpClientFactory.CreateClient();                                  
            httpClient.BaseAddress = new Uri(guideAlertSettings.SlackWebhookUrl);

            var webhookJson = JsonConvert.SerializeObject(webhookRequest);

            var content = new StringContent(webhookJson, Encoding.UTF8, "application/json");

            return await httpClient.PostAsync(string.Empty, content);
        }
    }
}
