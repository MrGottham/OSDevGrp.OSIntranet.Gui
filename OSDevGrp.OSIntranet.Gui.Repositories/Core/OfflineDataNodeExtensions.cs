using OSDevGrp.OSIntranet.Core;
using System.Xml;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Core
{
    internal static class OfflineDataNodeExtensions
    {
        #region Methods

        internal static XmlNodeList? SelectAccountingNodes(this XmlNode offlineDataNode, XmlNamespaceManager namespaceManager)
        {
            NullGuard.NotNull(offlineDataNode, nameof(offlineDataNode))
                .NotNull(namespaceManager, nameof(namespaceManager));

            return offlineDataNode.SelectNodes($"ns:Accounting", namespaceManager);
        }

        internal static XmlNode? SelectAccountingNode(this XmlNode offlineDataNode, int accountingIdentification, XmlNamespaceManager namespaceManager)
        {
            NullGuard.NotNull(offlineDataNode, nameof(offlineDataNode))
                .NotNull(namespaceManager, nameof(namespaceManager));

            return offlineDataNode.SelectSingleNode($"ns:Accounting[@number='{accountingIdentification}']", namespaceManager);
        }

        internal static XmlNode? SelectLetterHeadNode(this XmlNode offlineDataNode, int letterHeadIdentification, XmlNamespaceManager namespaceManager)
        {
            NullGuard.NotNull(offlineDataNode, nameof(offlineDataNode))
                .NotNull(namespaceManager, nameof(namespaceManager));

            return offlineDataNode.SelectSingleNode($"ns:LetterHead[@number='{letterHeadIdentification}']", namespaceManager);
        }

        #endregion
    }
}