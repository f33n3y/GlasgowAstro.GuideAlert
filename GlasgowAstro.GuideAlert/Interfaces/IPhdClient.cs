namespace GlasgowAstro.GuideAlert.Interfaces
{
    public interface IPhdClient
    {
        bool ConnectAndTest();

        bool WatchForStarLossEvents();
    }
}
