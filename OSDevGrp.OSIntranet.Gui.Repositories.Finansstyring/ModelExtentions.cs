using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Finansstyring
{
    /// <summary>
    /// Extentions til modeller.
    /// </summary>
    public static class ModelExtentions
    {
        /// <summary>
        /// Gemmer data for et regnskab i det lokale datalager.
        /// </summary>
        /// <param name="regnskabModel">Model for et regnskab.</param>
        /// <param name="localeDataDocument">XML dokument, hvori data skal gemmes.</param>
        public static void StoreInDocument(this IRegnskabModel regnskabModel, XDocument localeDataDocument)
        {
            if (regnskabModel == null)
            {
                throw new ArgumentNullException("regnskabModel");
            }
            if (localeDataDocument == null)
            {
                throw new ArgumentNullException("localeDataDocument");
            }
            var rootElement = localeDataDocument.Root;
            var regnskabElement = rootElement.Elements(XName.Get("Regnskab", rootElement.Name.NamespaceName)).SingleOrDefault(m => m.Attribute(XName.Get("nummer", string.Empty)) == null || string.Compare(m.Attribute(XName.Get("nummer", string.Empty)).Value, regnskabModel.Nummer.ToString(CultureInfo.InvariantCulture), StringComparison.Ordinal) == 0);
            if (regnskabElement == null)
            {
                regnskabElement = new XElement(XName.Get("Regnskab", rootElement.Name.NamespaceName));
                regnskabElement.Add(new XAttribute(XName.Get("nummer", string.Empty), regnskabModel.Nummer.ToString(CultureInfo.InvariantCulture)));
                regnskabElement.Add(new XAttribute(XName.Get("navn", string.Empty), regnskabModel.Navn));
                rootElement.Add(regnskabElement);
                return;
            }
            var navnAttribute = regnskabElement.Attribute(XName.Get("navn", string.Empty));
            if (navnAttribute == null)
            {
                regnskabElement.Add(new XAttribute(XName.Get("navn", string.Empty), regnskabModel.Navn));
                return;
            }
            navnAttribute.Value = regnskabModel.Navn;
        }

        /// <summary>
        /// Gemmer data for en kontogruppe i det lokale datalager.
        /// </summary>
        /// <param name="kontogruppeModel">Model for en kontogruppe.</param>
        /// <param name="localeDataDocument">XML dokument, hvori data skal gemmes.</param>
        public static void StoreInDocument(this IKontogruppeModel kontogruppeModel, XDocument localeDataDocument)
        {
            if (kontogruppeModel == null)
            {
                throw new ArgumentNullException("kontogruppeModel");
            }
            if (localeDataDocument == null)
            {
                throw new ArgumentNullException("localeDataDocument");
            }
            var rootElement = localeDataDocument.Root;
            var kontogruppeElement = rootElement.Elements(XName.Get("Kontogruppe", rootElement.Name.NamespaceName)).SingleOrDefault();

//            var regnskabElement = rootElement.Elements(XName.Get("Regnskab", rootElement.Name.NamespaceName)).SingleOrDefault(m => m.Attribute(XName.Get("nummer", string.Empty)) == null || string.Compare(m.Attribute(XName.Get("nummer", string.Empty)).Value, regnskabModel.Nummer.ToString(CultureInfo.InvariantCulture), StringComparison.Ordinal) == 0);
        }
    }
}
