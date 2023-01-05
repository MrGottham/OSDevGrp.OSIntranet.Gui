using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security.Models;
using System.Xml;

namespace OSDevGrp.OSIntranet.Gui.App.Settings
{
    public interface IApplicationDataProvider : IDisposable
    {
        FileInfo OfflineDataFile { get; }

        FileInfo LogFile { get; }

        IReadOnlyCollection<FileInfo> TemporaryFiles { get; }

        Task<Uri> GetApiEndpointAsync();

        Task<string> GetClientIdAsync();

        Task<string> GetClientSecretAsync();

        Task<IAccessTokenModel> GetAccessTokenAsync();

        Task<bool> GetShouldOpenSettingsOnStartupAsync();

        Task<XmlDocument> GetOfflineDataDocumentAsync();

        Task SetApiEndpointAsync(Uri apiEndpoint);

        Task SetClientIdAsync(string clientId);

        Task SetClientSecretAsync(string clientSecret);

        Task SetAccessTokenAsync(IAccessTokenModel accessTokenModel);

        Task SetShouldOpenSettingsOnStartupAsync(bool shouldOpenSettingsOnStartup);

        Task SetOfflineDataDocumentAsync(XmlDocument offlineDataDocument);
    }
}