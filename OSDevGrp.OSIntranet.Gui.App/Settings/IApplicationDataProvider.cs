using System.Xml;

namespace OSDevGrp.OSIntranet.Gui.App.Settings
{
    public interface IApplicationDataProvider : IDisposable
    {
        FileInfo OfflineDataFile { get; }

        Task<Uri> GetApiEndpointAsync();

        Task<string> GetClientIdAsync();

        Task<string> GetClientSecretAsync();

        Task<bool> GetShouldOpenSettingsOnStartupAsync();

        Task<XmlDocument> GetOfflineDataDocumentAsync();

        Task SetApiEndpointAsync(Uri apiEndpoint);

        Task SetClientIdAsync(string clientId);

        Task SetClientSecretAsync(string clientSecret);

        Task SetShouldOpenSettingsOnStartupAsync(bool shouldOpenSettingsOnStartup);
    }
}