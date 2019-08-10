using System;
using System.Net.Http;
using System.Text;

namespace GlasgowAstro.GuideAlert
{
    public class SlackClient
    {
        private readonly HttpClient httpClient = new HttpClient();
        private readonly string webhookRequestUri;
        private const string ContentType = "application/json";

        public SlackClient(string webhookRequestUri)
        {
            this.webhookRequestUri = webhookRequestUri;
        }

        public bool ConnectAndConfirm()
        {
            try
            {
                HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Post, webhookRequestUri);

                var content = new StringContent("{\"text\":\"Test alert\"}", Encoding.UTF8, ContentType);
                var result = httpClient.PostAsync(webhookRequestUri, content)?.Result;
                var success = result?.StatusCode == System.Net.HttpStatusCode.OK;
                return success;
            }
            catch (Exception e)
            {
                // TODO: Logging              
            }
            return false;
        }

        public bool SendNotification(string message)
        {
            // TODO
            return true;
        }
    }
}
