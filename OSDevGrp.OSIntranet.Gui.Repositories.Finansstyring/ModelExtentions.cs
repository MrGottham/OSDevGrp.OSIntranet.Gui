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
            var regnskabElement = localeDataDocument.GetRegnskabElement(regnskabModel.Nummer);
            if (regnskabElement == null)
            {
                var rootElement = localeDataDocument.Root;
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
        /// Gemmer data for en konto i det lokale datalager.
        /// </summary>
        /// <param name="kontoModel">Model for en konto.</param>
        /// <param name="localeDataDocument">XML dokument, hvori data skal gemmes.</param>
        public static void StoreInDocument(this IKontoModel kontoModel, XDocument localeDataDocument)
        {
            if (kontoModel == null)
            {
                throw new ArgumentNullException("kontoModel");
            }
            if (localeDataDocument == null)
            {
                throw new ArgumentNullException("localeDataDocument");
            }
            var regnskabElement = localeDataDocument.GetRegnskabElement(kontoModel.Regnskabsnummer);
            if (regnskabElement == null)
            {
                return;
            }
            var kontoElement = regnskabElement.Elements(XName.Get("Konto", regnskabElement.Name.NamespaceName)).SingleOrDefault(m => m.Attribute(XName.Get("kontonummer", string.Empty)) != null && string.Compare(m.Attribute(XName.Get("kontonummer", string.Empty)).Value, kontoModel.Kontonummer, StringComparison.Ordinal) == 0);
            if (kontoElement == null)
            {
                kontoElement = new XElement(XName.Get("Konto", regnskabElement.Name.NamespaceName));
                kontoElement.Add(new XAttribute(XName.Get("kontonummer", string.Empty), kontoModel.Kontonummer));
                kontoElement.Add(new XAttribute(XName.Get("kontonavn", string.Empty), kontoModel.Kontonavn));
                kontoElement.Add(new XAttribute(XName.Get("kontogruppe", string.Empty), kontoModel.Kontogruppe.ToString(CultureInfo.InvariantCulture)));
                // TODO: Continue....
                regnskabElement.Add(kontoElement);
                return;
            }
            var kontonavnAttribute = kontoElement.Attribute(XName.Get("kontonavn", string.Empty));
            if (kontonavnAttribute == null)
            {
                kontoElement.Add(new XAttribute(XName.Get("kontonavn", string.Empty), kontoModel.Kontonavn));
            }
            else
            {
                kontonavnAttribute.Value = kontoModel.Kontonavn;
            }
            var kontogruppeAttribute = kontoElement.Attribute(XName.Get("kontogruppe", string.Empty));
            if (kontogruppeAttribute == null)
            {
                kontoElement.Add(new XAttribute(XName.Get("kontogruppe", string.Empty), kontoModel.Kontogruppe.ToString(CultureInfo.InvariantCulture)));
            }
            else
            {
                kontogruppeAttribute.Value = kontoModel.Kontogruppe.ToString(CultureInfo.InvariantCulture);
            }
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
            var kontogruppeElement = localeDataDocument.GetKontogruppeElement(kontogruppeModel.Nummer);
            if (kontogruppeElement == null)
            {
                var rootElement = localeDataDocument.Root;
                kontogruppeElement = new XElement(XName.Get("Kontogruppe", rootElement.Name.NamespaceName));
                kontogruppeElement.Add(new XAttribute(XName.Get("nummer", string.Empty), kontogruppeModel.Nummer.ToString(CultureInfo.InvariantCulture)));
                kontogruppeElement.Add(new XAttribute(XName.Get("tekst", string.Empty), kontogruppeModel.Tekst));
                kontogruppeElement.Add(new XAttribute(XName.Get("balanceType", string.Empty), kontogruppeModel.Balancetype.ToString()));
                rootElement.Add(kontogruppeElement);
                return;
            }
            var tekstAttribute = kontogruppeElement.Attribute(XName.Get("tekst", string.Empty));
            if (tekstAttribute == null)
            {
                kontogruppeElement.Add(new XAttribute(XName.Get("tekst", string.Empty), kontogruppeModel.Tekst));
            }
            else
            {
                tekstAttribute.Value = kontogruppeModel.Tekst;
            }
            var balanceTypeNode = kontogruppeElement.Attribute(XName.Get("balanceType", string.Empty));
            if (balanceTypeNode == null)
            {
                kontogruppeElement.Add(new XAttribute(XName.Get("balanceType", string.Empty), kontogruppeModel.Balancetype.ToString()));
                return;
            }
            balanceTypeNode.Value = kontogruppeModel.Balancetype.ToString();
        }

        /// <summary>
        /// Finder og returnerer elementet til et givent regnskab.
        /// </summary>
        /// <param name="localeDataDocument">XML dokument, hvorfra elementet skal returneres.</param>
        /// <param name="nummer">Unik identifikation af regnskabet.</param>
        /// <returns>Elementet til det givne regnskab.</returns>
        public static XElement GetRegnskabElement(this XDocument localeDataDocument, int nummer)
        {
            if (localeDataDocument == null)
            {
                throw new ArgumentNullException("localeDataDocument");
            }
            var rootElement = localeDataDocument.Root;
            return rootElement.Elements(XName.Get("Regnskab", rootElement.Name.NamespaceName)).SingleOrDefault(m => m.Attribute(XName.Get("nummer", string.Empty)) != null && string.Compare(m.Attribute(XName.Get("nummer", string.Empty)).Value, nummer.ToString(CultureInfo.InvariantCulture), StringComparison.Ordinal) == 0);
        }

        /// <summary>
        /// Finder og returnerer elementet til en given kontogruppe.
        /// </summary>
        /// <param name="localeDataDocument">XML dokument, hvorfra elementet skal returneres.</param>
        /// <param name="nummer">Unik identifikation af kontogruppen.</param>
        /// <returns>Elementet til den givne kontogruppe.</returns>
        public static XElement GetKontogruppeElement(this XDocument localeDataDocument, int nummer)
        {
            if (localeDataDocument == null)
            {
                throw new ArgumentNullException("localeDataDocument");
            }
            var rootElement = localeDataDocument.Root;
            return rootElement.Elements(XName.Get("Kontogruppe", rootElement.Name.NamespaceName)).SingleOrDefault(m => m.Attribute(XName.Get("nummer", string.Empty)) != null && string.Compare(m.Attribute(XName.Get("nummer", string.Empty)).Value, nummer.ToString(CultureInfo.InvariantCulture), StringComparison.Ordinal) == 0);
        }
    }
}
