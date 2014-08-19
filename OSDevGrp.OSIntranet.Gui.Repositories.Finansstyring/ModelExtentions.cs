using System;
using System.Collections.Generic;
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
                regnskabElement.UpdateAttribute(XName.Get("nummer", string.Empty), regnskabModel.Nummer.ToString(CultureInfo.InvariantCulture));
                regnskabElement.UpdateAttribute(XName.Get("navn", string.Empty), regnskabModel.Navn);
                rootElement.Add(regnskabElement);
                return;
            }
            regnskabElement.UpdateAttribute(XName.Get("navn", string.Empty), regnskabModel.Navn);
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
            XElement kontoHistorikElement;
            var kontoElement = regnskabElement.Elements(XName.Get("Konto", regnskabElement.Name.NamespaceName)).SingleOrDefault(m => m.Attribute(XName.Get("kontonummer", string.Empty)) != null && string.Compare(m.Attribute(XName.Get("kontonummer", string.Empty)).Value, kontoModel.Kontonummer, StringComparison.Ordinal) == 0);
            if (kontoElement == null)
            {
                kontoElement = new XElement(XName.Get("Konto", regnskabElement.Name.NamespaceName));
                kontoElement.UpdateAttribute(XName.Get("kontonummer", string.Empty), kontoModel.Kontonummer);
                kontoElement.UpdateAttribute(XName.Get("kontonavn", string.Empty), kontoModel.Kontonavn);
                kontoElement.UpdateAttribute(XName.Get("kontogruppe", string.Empty), kontoModel.Kontogruppe.ToString(CultureInfo.InvariantCulture));
                kontoElement.UpdateAttribute(XName.Get("beskrivelse", string.Empty), kontoModel.Beskrivelse, false);
                kontoElement.UpdateAttribute(XName.Get("note", string.Empty), kontoModel.Notat, false);
                regnskabElement.Add(kontoElement);
                kontoHistorikElement = new XElement(XName.Get("KontoHistorik", regnskabElement.Name.NamespaceName));
                kontoHistorikElement.UpdateAttribute(XName.Get("kontonummer", string.Empty), kontoModel.Kontonummer);
                kontoHistorikElement.UpdateAttribute(XName.Get("dato", string.Empty), kontoModel.StatusDato.ToString("yyyyMMdd"));
                kontoHistorikElement.UpdateAttribute(XName.Get("kredit", string.Empty), kontoModel.Kredit.ToString("N", CultureInfo.InvariantCulture));
                kontoHistorikElement.UpdateAttribute(XName.Get("saldo", string.Empty), kontoModel.Saldo.ToString("N", CultureInfo.InvariantCulture));
                regnskabElement.Add(kontoHistorikElement);
                return;
            }
            kontoElement.UpdateAttribute(XName.Get("kontonavn", string.Empty), kontoModel.Kontonavn);
            kontoElement.UpdateAttribute(XName.Get("kontogruppe", string.Empty), kontoModel.Kontogruppe.ToString(CultureInfo.InvariantCulture));
            kontoElement.UpdateAttribute(XName.Get("beskrivelse", string.Empty), kontoModel.Beskrivelse, false);
            kontoElement.UpdateAttribute(XName.Get("note", string.Empty), kontoModel.Notat, false);

            var kontoHistorikElements = localeDataDocument.GetHistorikElements(kontoModel).ToList();
            try
            {
                kontoHistorikElement = kontoHistorikElements.FirstOrDefault(m => string.Compare(m.Attribute(XName.Get("dato", string.Empty)).Value, kontoModel.StatusDato.ToString("yyyyMMdd"), StringComparison.Ordinal) == 0);
                if (kontoHistorikElement == null)
                {
                    kontoHistorikElement = new XElement(XName.Get("KontoHistorik", regnskabElement.Name.NamespaceName));
                    kontoHistorikElement.UpdateAttribute(XName.Get("kontonummer", string.Empty), kontoModel.Kontonummer);
                    kontoHistorikElement.UpdateAttribute(XName.Get("dato", string.Empty), kontoModel.StatusDato.ToString("yyyyMMdd"));
                    kontoHistorikElement.UpdateAttribute(XName.Get("kredit", string.Empty), kontoModel.Kredit.ToString("N", CultureInfo.InvariantCulture));
                    kontoHistorikElement.UpdateAttribute(XName.Get("saldo", string.Empty), kontoModel.Saldo.ToString("N", CultureInfo.InvariantCulture));
                    regnskabElement.Add(kontoHistorikElement);
                    return;
                }
                kontoHistorikElement.UpdateAttribute(XName.Get("kredit", string.Empty), kontoModel.Kredit.ToString("N", CultureInfo.InvariantCulture));
                kontoHistorikElement.UpdateAttribute(XName.Get("saldo", string.Empty), kontoModel.Saldo.ToString("N", CultureInfo.InvariantCulture));
            }
            finally
            {
                var elementsToDelete = kontoHistorikElements.Where(m => string.Compare(m.Attribute(XName.Get("dato", string.Empty)).Value, kontoModel.StatusDato.AddYears(-1).ToString("yyyyMMdd"), StringComparison.Ordinal) <= 0).ToList();
                while (elementsToDelete.Count > 0)
                {
                    var elementToDelete = elementsToDelete.First();
                    elementToDelete.Remove();
                    elementsToDelete.Remove(elementToDelete);
                }
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
                kontogruppeElement.UpdateAttribute(XName.Get("nummer", string.Empty), kontogruppeModel.Nummer.ToString(CultureInfo.InvariantCulture));
                kontogruppeElement.UpdateAttribute(XName.Get("tekst", string.Empty), kontogruppeModel.Tekst);
                kontogruppeElement.UpdateAttribute(XName.Get("balanceType", string.Empty), kontogruppeModel.Balancetype.ToString());
                rootElement.Add(kontogruppeElement);
                return;
            }
            kontogruppeElement.UpdateAttribute(XName.Get("tekst", string.Empty), kontogruppeModel.Tekst);
            kontogruppeElement.UpdateAttribute(XName.Get("balanceType", string.Empty), kontogruppeModel.Balancetype.ToString());
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
        /// Finder og returnerer elementer med historiske data for en given konto.
        /// </summary>
        /// <param name="localeDataDocument">XML dokument, hvorfra elementer skal returneres.</param>
        /// <param name="kontoModel">Model for kontoen, hvortil historiske data skal returneres.</param>
        /// <returns>Elementer indeholdende historiske data til den givne konto.</returns>
        public static IEnumerable<XElement> GetHistorikElements(this XDocument localeDataDocument, IKontoModel kontoModel)
        {
            if (localeDataDocument == null)
            {
                throw new ArgumentNullException("localeDataDocument");
            }
            if (kontoModel == null)
            {
                throw new ArgumentNullException("kontoModel");
            }
            var regnskabElement = localeDataDocument.GetRegnskabElement(kontoModel.Regnskabsnummer);
            if (regnskabElement == null)
            {
                return new List<XElement>(0);
            }
            return regnskabElement.Elements(XName.Get("KontoHistorik", regnskabElement.Name.NamespaceName)).Where(m => m.Attribute(XName.Get("kontonummer", string.Empty)) != null && string.Compare(m.Attribute(XName.Get("kontonummer", string.Empty)).Value, kontoModel.Kontonummer, StringComparison.Ordinal) == 0 && m.Attribute(XName.Get("dato", string.Empty)) != null && string.Compare(m.Attribute(XName.Get("dato", string.Empty)).Value, kontoModel.StatusDato.ToString("yyyyMMdd"), StringComparison.Ordinal) <= 0).OrderByDescending(m => m.Attribute(XName.Get("dato", string.Empty)).Value).ToList();
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

        /// <summary>
        /// Opdaterer værdien af en given XML attribute.
        /// </summary>
        /// <param name="localeDataElement">XML element, hvorpå attributværdi skal opdateres.</param>
        /// <param name="attributeName">Navn på XML attributten, der skal opdateres.</param>
        /// <param name="attributeValue">Værdi, som XML attributten skal opdateres med.</param>
        /// <param name="required">Angivelse af, om XML attributten er requried.</param>
        private static void UpdateAttribute(this XElement localeDataElement, XName attributeName, string attributeValue, bool required = true)
        {
            if (localeDataElement == null)
            {
                throw new ArgumentNullException("localeDataElement");
            }
            if (attributeName == null)
            {
                throw new ArgumentNullException("attributeName");
            }
            if (string.IsNullOrEmpty(attributeValue) && required)
            {
                throw new ArgumentNullException("attributeValue");
            }
            var attribute = localeDataElement.Attribute(attributeName);
            if (attribute == null)
            {
                if (string.IsNullOrEmpty(attributeValue) && required == false)
                {
                    return;
                }
                localeDataElement.Add(new XAttribute(attributeName, attributeValue));
                return;
            }
            if (string.IsNullOrEmpty(attributeValue) && required == false)
            {
                attribute.Remove();
                return;
            }
            attribute.Value = attributeValue;
        }
    }
}
