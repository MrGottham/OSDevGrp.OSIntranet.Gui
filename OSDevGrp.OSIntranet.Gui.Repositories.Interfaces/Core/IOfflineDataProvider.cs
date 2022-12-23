using System.Threading.Tasks;
using System.Xml;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core
{
    public interface IOfflineDataProvider
    {
        Task<XmlDocument> GetOfflineDataDocumentAsync();

        object GetSyncRoot();
    }
}