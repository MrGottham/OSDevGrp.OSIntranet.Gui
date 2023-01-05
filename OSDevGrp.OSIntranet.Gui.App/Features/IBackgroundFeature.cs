namespace OSDevGrp.OSIntranet.Gui.App.Features
{
    public interface IBackgroundFeature : IDisposable
    {
        Task StartAsync();

        Task StopAsync();
    }
}