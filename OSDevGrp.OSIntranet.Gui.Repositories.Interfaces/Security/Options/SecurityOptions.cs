using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core.Options;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security.Options
{
    public class SecurityOptions : OnlineOptions
    {
        public string ClientId { get; set; } = string.Empty;

        public string ClientSecret { get; set; } = string.Empty;
    }
}