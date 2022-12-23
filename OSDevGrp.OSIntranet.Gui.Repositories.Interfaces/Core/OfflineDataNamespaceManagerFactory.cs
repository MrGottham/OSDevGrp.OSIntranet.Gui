using System.Xml;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core
{
    public static class OfflineDataNamespaceManagerFactory
    {
        #region Constants

        private const string TargetNamespace = "urn:osdevgrp:osintranet:gui:offline:data";

        #endregion

        #region Methods

        public static XmlNamespaceManager Build()
        {
            NameTable nameTable = new NameTable();
            nameTable.Add("OfflineData");
            nameTable.Add("Accounting");
            nameTable.Add("LetterHead");

            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(nameTable);
            namespaceManager.AddNamespace(string.Empty, TargetNamespace);
            namespaceManager.AddNamespace("ns", TargetNamespace);

            return namespaceManager;
        }

        #endregion
    }
}