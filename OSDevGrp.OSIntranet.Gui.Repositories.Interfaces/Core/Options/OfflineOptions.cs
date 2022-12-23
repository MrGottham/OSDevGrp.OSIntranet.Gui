using System.Xml;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core.Options
{
    public class OfflineOptions
    {
        public XmlDocument OfflineDataDocument { get; set; } = OfflineDataDocumentFactory.Build();
    }
}