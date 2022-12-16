using System;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security.Options
{
    public class SecurityOptions
    {
        public Uri ApiEndpoint { get; set; } = new Uri("https://localhost");

        public string ClientId { get; set; } = string.Empty;

        public string ClientSecret { get; set; } = string.Empty;
    }
}