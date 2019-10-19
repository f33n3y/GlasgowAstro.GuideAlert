using GlasgowAstro.GuideAlert.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace GlasgowAstro.GuideAlert.Interfaces
{
    public interface ISlackClient
    {
        Task<bool> ConnectAndTestAsync();

        Task<HttpResponseMessage> SendAlert(SlackWebhookRequest webhookRequest);
    }
}
