using System;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core.Options
{
    public class OnlineOptions
    {
        public Uri ApiEndpoint { get; set; } = new Uri("https://localhost");
    }
}