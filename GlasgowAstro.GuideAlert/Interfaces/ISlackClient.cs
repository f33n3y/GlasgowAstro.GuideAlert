namespace GlasgowAstro.GuideAlert.Interfaces
{
    public interface ISlackClient
    {
        bool ConnectAndTest();
        bool SendAlert(string alertMessage);
    }
}
