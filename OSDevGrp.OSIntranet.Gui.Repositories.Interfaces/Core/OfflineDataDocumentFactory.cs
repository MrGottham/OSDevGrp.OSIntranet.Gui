using System.Text;
using System.Xml;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core
{
    public static class OfflineDataDocumentFactory
    {
        #region Methods

        public static XmlDocument Build()
        {
            XmlNamespaceManager namespaceManager = OfflineDataNamespaceManagerFactory.Build();

            XmlDocument offlineDataDocument = new XmlDocument(namespaceManager.NameTable);
            offlineDataDocument.AppendChild(offlineDataDocument.CreateXmlDeclaration("1.0", Encoding.UTF8.WebName, null));

            XmlAttribute schemaLocationAttribute = offlineDataDocument.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
            schemaLocationAttribute.Value = namespaceManager.DefaultNamespace;

            XmlElement offlineDataElement = offlineDataDocument.CreateElement("OfflineData", namespaceManager.DefaultNamespace);
            offlineDataElement.Attributes.Append(schemaLocationAttribute);

            offlineDataDocument.AppendChild(offlineDataElement);

            return offlineDataDocument;
        }

        #endregion
    }
}