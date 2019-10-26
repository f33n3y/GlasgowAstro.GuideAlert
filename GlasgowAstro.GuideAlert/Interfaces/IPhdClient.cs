using System.Threading.Tasks;

namespace GlasgowAstro.GuideAlert.Interfaces
{
    public interface IPhdClient
    {
        Task<bool> ConnectAndTestAsync();

        Task<bool> WatchForStarLossEvents();
    }
}
