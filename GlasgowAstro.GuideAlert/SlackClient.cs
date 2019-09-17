using GlasgowAstro.GuideAlert.Interfaces;
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
        private readonly IHttpClientFactory httpClientFactory;
        private const string ContentType = "application/json";

        public SlackClient(IHttpClientFactory httpClientFactory)
        {
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
                var httpClient = httpClientFactory.CreateClient();
                httpClient.BaseAddress = new Uri("");
                HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Post, "");
                var content = new StringContent("{\"text\":\"Test alert\"}", Encoding.UTF8, ContentType);

                var result = httpClient.PostAsync("", content)?.Result;
                return result?.StatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                // TODO: Logging              
            }
            return false;
        }

        public bool SendAlert(string alertMessage)
        {
            // TODO
            return true;
        }
    }
}
