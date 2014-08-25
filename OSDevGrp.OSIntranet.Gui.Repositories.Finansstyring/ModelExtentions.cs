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
                kontoHistorikElement.UpdateAttribute(XName.Get("dato", string.Empty), GetHistorikDato(kontoModel.StatusDato));
                kontoHistorikElement.UpdateAttribute(XName.Get("kredit", string.Empty), kontoModel.Kredit.ToString("N", CultureInfo.InvariantCulture));
                kontoHistorikElement.UpdateAttribute(XName.Get("saldo", string.Empty), kontoModel.Saldo.ToString("N", CultureInfo.InvariantCulture));
                regnskabElement.Add(kontoHistorikElement);
                return;
            }
            kontoElement.UpdateAttribute(XName.Get("kontonavn", string.Empty), kontoModel.Kontonavn);
            kontoElement.UpdateAttribute(XName.Get("kontogruppe", string.Empty), kontoModel.Kontogruppe.ToString(CultureInfo.InvariantCulture));
            kontoElement.UpdateAttribute(XName.Get("beskrivelse", string.Empty), kontoModel.Beskrivelse, false);
            kontoElement.UpdateAttribute(XName.Get("note", string.Empty), kontoModel.Notat, false);

            var kontoHistorikDato = GetHistorikDato(kontoModel.StatusDato);
            kontoHistorikElement = localeDataDocument.GetHistorikElements(kontoModel).FirstOrDefault(m => string.Compare(m.Attribute(XName.Get("dato", string.Empty)).Value, kontoHistorikDato, StringComparison.Ordinal) == 0);
            if (kontoHistorikElement == null)
            {
                kontoHistorikElement = new XElement(XName.Get("KontoHistorik", regnskabElement.Name.NamespaceName));
                kontoHistorikElement.UpdateAttribute(XName.Get("kontonummer", string.Empty), kontoModel.Kontonummer);
                kontoHistorikElement.UpdateAttribute(XName.Get("dato", string.Empty), kontoHistorikDato);
                kontoHistorikElement.UpdateAttribute(XName.Get("kredit", string.Empty), kontoModel.Kredit.ToString("N", CultureInfo.InvariantCulture));
                kontoHistorikElement.UpdateAttribute(XName.Get("saldo", string.Empty), kontoModel.Saldo.ToString("N", CultureInfo.InvariantCulture));
                regnskabElement.Add(kontoHistorikElement);
                return;
            }
            kontoHistorikElement.UpdateAttribute(XName.Get("kredit", string.Empty), kontoModel.Kredit.ToString("N", CultureInfo.InvariantCulture));
            kontoHistorikElement.UpdateAttribute(XName.Get("saldo", string.Empty), kontoModel.Saldo.ToString("N", CultureInfo.InvariantCulture));
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
        /// Gemmer data for en budgetkontogruppe i det lokale datalager.
        /// </summary>
        /// <param name="budgetkontogruppeModel">Model for en kontogruppe til budgetkonti.</param>
        /// <param name="localeDataDocument">XML dokument, hvori data skal gemmes.</param>
        public static void StoreInDocument(this IBudgetkontogruppeModel budgetkontogruppeModel, XDocument localeDataDocument)
        {
            if (budgetkontogruppeModel == null)
            {
                throw new ArgumentNullException("budgetkontogruppeModel");
            }
            if (localeDataDocument == null)
            {
                throw new ArgumentNullException("localeDataDocument");
            }
            var budgetkontogruppeElement = localeDataDocument.GetBudgetkontogruppeElement(budgetkontogruppeModel.Nummer);
            if (budgetkontogruppeElement == null)
            {
                var rootElement = localeDataDocument.Root;
                budgetkontogruppeElement = new XElement(XName.Get("Budgetkontogruppe", rootElement.Name.NamespaceName));
                budgetkontogruppeElement.UpdateAttribute(XName.Get("nummer", string.Empty), budgetkontogruppeModel.Nummer.ToString(CultureInfo.InvariantCulture));
                budgetkontogruppeElement.UpdateAttribute(XName.Get("tekst", string.Empty), budgetkontogruppeModel.Tekst);
                rootElement.Add(budgetkontogruppeElement);
                return;
            }
            budgetkontogruppeElement.UpdateAttribute(XName.Get("tekst", string.Empty), budgetkontogruppeModel.Tekst);
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
            var historikElements = regnskabElement.Elements(XName.Get("KontoHistorik", regnskabElement.Name.NamespaceName)).Where(m => m.Attribute(XName.Get("kontonummer", string.Empty)) != null && string.Compare(m.Attribute(XName.Get("kontonummer", string.Empty)).Value, kontoModel.Kontonummer, StringComparison.Ordinal) == 0 && m.Attribute(XName.Get("dato", string.Empty)) != null && string.Compare(m.Attribute(XName.Get("dato", string.Empty)).Value, GetHistorikDato(kontoModel.StatusDato), StringComparison.Ordinal) <= 0).ToList();
            var remoteElementUntil = DateTime.Now.AddYears(-1);
            var elementToDelete = historikElements.FirstOrDefault(m => string.Compare(m.Attribute(XName.Get("dato", string.Empty)).Value, GetHistorikDato(remoteElementUntil), StringComparison.Ordinal) <= 0);
            while (elementToDelete != null)
            {
                elementToDelete.Remove();
                historikElements.Remove(elementToDelete);
                elementToDelete = historikElements.FirstOrDefault(m => string.Compare(m.Attribute(XName.Get("dato", string.Empty)).Value, GetHistorikDato(remoteElementUntil), StringComparison.Ordinal) <= 0);
            }
            return historikElements.OrderByDescending(m => m.Attribute(XName.Get("dato", string.Empty)).Value).ToList();
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
        /// Finder og returnerer elementet til en given kontogruppe til budgetkonti.
        /// </summary>
        /// <param name="localeDataDocument">XML dokument, hvorfra elementet skal returneres.</param>
        /// <param name="nummer">Unik identifikation af kontogruppen til budgetkonti.</param>
        /// <returns>Elementet til den givne kontogruppe for budgetkonti.</returns>
        public static XElement GetBudgetkontogruppeElement(this XDocument localeDataDocument, int nummer)
        {
            if (localeDataDocument == null)
            {
                throw new ArgumentNullException("localeDataDocument");
            }
            var rootElement = localeDataDocument.Root;
            return rootElement.Elements(XName.Get("Budgetkontogruppe", rootElement.Name.NamespaceName)).SingleOrDefault(m => m.Attribute(XName.Get("nummer", string.Empty)) != null && string.Compare(m.Attribute(XName.Get("nummer", string.Empty)).Value, nummer.ToString(CultureInfo.InvariantCulture), StringComparison.Ordinal) == 0);
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

        /// <summary>
        /// Konverterer en statudato til formatet, der benyttes til en historisk dato i XML dokumentet.
        /// </summary>
        /// <param name="statusDato">Statusdato.</param>
        /// <returns>Historisk dato, som kan benyttes i XML dokumentet.</returns>
        private static string GetHistorikDato(DateTime statusDato)
        {
            return statusDato.ToString("yyyyMMdd");
        }
    }
}
