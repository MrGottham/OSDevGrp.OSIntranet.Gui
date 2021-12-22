using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Resources;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Finansstyring
{
    /// <summary>
    /// Extentions til modeller.
    /// </summary>
    internal static class ModelExtensions
    {
        #region Private constants

        private const string CurrencyFormat = "#0.00";
        private const string VersionFormat = "#0.0";

        #endregion

        /// <summary>
        /// Gemmer data for et regnskab i det lokale datalager.
        /// </summary>
        /// <param name="regnskabModel">Model for et regnskab.</param>
        /// <param name="localeDataDocument">XML dokument, hvori data skal gemmes.</param>
        internal static void StoreInDocument(this IRegnskabModel regnskabModel, XDocument localeDataDocument)
        {
            if (regnskabModel == null)
            {
                throw new ArgumentNullException(nameof(regnskabModel));
            }

            if (localeDataDocument == null)
            {
                throw new ArgumentNullException(nameof(localeDataDocument));
            }

            XElement regnskabElement = localeDataDocument.GetRegnskabElement(regnskabModel.Nummer);
            if (regnskabElement == null)
            {
                var rootElement = localeDataDocument.Root;
                if (rootElement == null)
                {
                    return;
                }

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
        internal static void StoreInDocument(this IKontoModel kontoModel, XDocument localeDataDocument)
        {
            if (kontoModel == null)
            {
                throw new ArgumentNullException(nameof(kontoModel));
            }

            if (localeDataDocument == null)
            {
                throw new ArgumentNullException(nameof(localeDataDocument));
            }

            XElement regnskabElement = localeDataDocument.GetRegnskabElement(kontoModel.Regnskabsnummer);
            if (regnskabElement == null)
            {
                return;
            }

            XElement kontoHistorikElement;
            XElement kontoElement = regnskabElement
                .Elements(XName.Get("Konto", regnskabElement.Name.NamespaceName))
                .SingleOrDefault(m => string.CompareOrdinal(m.GetRequiredAttributeValue("kontonummer", string.Empty), kontoModel.Kontonummer) == 0);
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
                kontoHistorikElement.UpdateAttribute(XName.Get("kredit", string.Empty), kontoModel.Kredit.ToString(CurrencyFormat, CultureInfo.InvariantCulture));
                kontoHistorikElement.UpdateAttribute(XName.Get("saldo", string.Empty), kontoModel.Saldo.ToString(CurrencyFormat, CultureInfo.InvariantCulture));
                regnskabElement.Add(kontoHistorikElement);
                return;
            }

            kontoElement.UpdateAttribute(XName.Get("kontonavn", string.Empty), kontoModel.Kontonavn);
            kontoElement.UpdateAttribute(XName.Get("kontogruppe", string.Empty), kontoModel.Kontogruppe.ToString(CultureInfo.InvariantCulture));
            kontoElement.UpdateAttribute(XName.Get("beskrivelse", string.Empty), kontoModel.Beskrivelse, false);
            kontoElement.UpdateAttribute(XName.Get("note", string.Empty), kontoModel.Notat, false);

            string kontoHistorikDato = GetHistorikDato(kontoModel.StatusDato);
            kontoHistorikElement = localeDataDocument
                .GetHistorikElements(kontoModel)
                .SingleOrDefault(m => string.CompareOrdinal(m.GetRequiredAttributeValue("dato", string.Empty), kontoHistorikDato) == 0);
            if (kontoHistorikElement == null)
            {
                bool makeHistoric = true;
                kontoHistorikElement = localeDataDocument
                    .GetHistorikElements(kontoModel)
                    .FirstOrDefault(m => string.CompareOrdinal(m.GetRequiredAttributeValue("dato", string.Empty), kontoHistorikDato) < 0);
                if (kontoHistorikElement != null)
                {
                    DateTime prevStatusDato = DateTime.ParseExact(kontoHistorikElement.GetRequiredAttributeValue("dato", string.Empty), "yyyyMMdd", CultureInfo.InvariantCulture);
                    decimal prevKredit = decimal.Parse(kontoHistorikElement.GetRequiredAttributeValue("kredit", string.Empty), NumberStyles.Any, CultureInfo.InvariantCulture);
                    decimal prevSaldo = decimal.Parse(kontoHistorikElement.GetRequiredAttributeValue("saldo", string.Empty), NumberStyles.Any, CultureInfo.InvariantCulture);
                    makeHistoric = kontoModel.StatusDato.Year != prevStatusDato.Year ||
                                   kontoModel.StatusDato.Year == prevStatusDato.Year && kontoModel.StatusDato.Month != prevStatusDato.Month ||
                                   kontoModel.Kredit != prevKredit ||
                                   kontoModel.Saldo != prevSaldo;
                }

                if (!makeHistoric)
                {
                    return;
                }

                kontoHistorikElement = new XElement(XName.Get("KontoHistorik", regnskabElement.Name.NamespaceName));
                kontoHistorikElement.UpdateAttribute(XName.Get("kontonummer", string.Empty), kontoModel.Kontonummer);
                kontoHistorikElement.UpdateAttribute(XName.Get("dato", string.Empty), kontoHistorikDato);
                kontoHistorikElement.UpdateAttribute(XName.Get("kredit", string.Empty), kontoModel.Kredit.ToString(CurrencyFormat, CultureInfo.InvariantCulture));
                kontoHistorikElement.UpdateAttribute(XName.Get("saldo", string.Empty), kontoModel.Saldo.ToString(CurrencyFormat, CultureInfo.InvariantCulture));
                regnskabElement.Add(kontoHistorikElement);
                return;
            }

            kontoHistorikElement.UpdateAttribute(XName.Get("kredit", string.Empty), kontoModel.Kredit.ToString(CurrencyFormat, CultureInfo.InvariantCulture));
            kontoHistorikElement.UpdateAttribute(XName.Get("saldo", string.Empty), kontoModel.Saldo.ToString(CurrencyFormat, CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Gemmer data for en budgetkonto i det lokale datalager.
        /// </summary>
        /// <param name="budgetkontoModel">Model for en budgetkonto.</param>
        /// <param name="localeDataDocument">XML dokument, hvori data skal gemmes.</param>
        internal static void StoreInDocument(this IBudgetkontoModel budgetkontoModel, XDocument localeDataDocument)
        {
            if (budgetkontoModel == null)
            {
                throw new ArgumentNullException(nameof(budgetkontoModel));
            }

            if (localeDataDocument == null)
            {
                throw new ArgumentNullException(nameof(localeDataDocument));
            }

            XElement regnskabElement = localeDataDocument.GetRegnskabElement(budgetkontoModel.Regnskabsnummer);
            if (regnskabElement == null)
            {
                return;
            }

            DateTime sidsteMånedStatusDato = new DateTime(budgetkontoModel.StatusDato.AddMonths(-1).Year, budgetkontoModel.StatusDato.AddMonths(-1).Month, DateTime.DaysInMonth(budgetkontoModel.StatusDato.AddMonths(-1).Year, budgetkontoModel.StatusDato.AddMonths(-1).Month));
            decimal sidsteMånedIndtægter = Math.Abs(budgetkontoModel.BudgetSidsteMåned > 0 ? budgetkontoModel.BudgetSidsteMåned : 0M);
            decimal sidsteMånedUdgifter = Math.Abs(budgetkontoModel.BudgetSidsteMåned < 0 ? budgetkontoModel.BudgetSidsteMåned : 0M);
            XElement budgetkontoHistorikElement;
            XElement budgetkontoElement = regnskabElement
                .Elements(XName.Get("Budgetkonto", regnskabElement.Name.NamespaceName))
                .SingleOrDefault(m => string.CompareOrdinal(m.GetRequiredAttributeValue("kontonummer", string.Empty), budgetkontoModel.Kontonummer) == 0);
            if (budgetkontoElement == null)
            {
                budgetkontoElement = new XElement(XName.Get("Budgetkonto", regnskabElement.Name.NamespaceName));
                budgetkontoElement.UpdateAttribute(XName.Get("kontonummer", string.Empty), budgetkontoModel.Kontonummer);
                budgetkontoElement.UpdateAttribute(XName.Get("kontonavn", string.Empty), budgetkontoModel.Kontonavn);
                budgetkontoElement.UpdateAttribute(XName.Get("kontogruppe", string.Empty), budgetkontoModel.Kontogruppe.ToString(CultureInfo.InvariantCulture));
                budgetkontoElement.UpdateAttribute(XName.Get("beskrivelse", string.Empty), budgetkontoModel.Beskrivelse, false);
                budgetkontoElement.UpdateAttribute(XName.Get("note", string.Empty), budgetkontoModel.Notat, false);
                regnskabElement.Add(budgetkontoElement);

                budgetkontoHistorikElement = new XElement(XName.Get("BudgetkontoHistorik", regnskabElement.Name.NamespaceName));
                budgetkontoHistorikElement.UpdateAttribute(XName.Get("kontonummer", string.Empty), budgetkontoModel.Kontonummer);
                budgetkontoHistorikElement.UpdateAttribute(XName.Get("dato", string.Empty), GetHistorikDato(budgetkontoModel.StatusDato));
                budgetkontoHistorikElement.UpdateAttribute(XName.Get("indtaegter", string.Empty), budgetkontoModel.Indtægter.ToString(CurrencyFormat, CultureInfo.InvariantCulture));
                budgetkontoHistorikElement.UpdateAttribute(XName.Get("udgifter", string.Empty), budgetkontoModel.Udgifter.ToString(CurrencyFormat, CultureInfo.InvariantCulture));
                budgetkontoHistorikElement.UpdateAttribute(XName.Get("bogfoert", string.Empty), budgetkontoModel.Bogført.ToString(CurrencyFormat, CultureInfo.InvariantCulture));
                regnskabElement.Add(budgetkontoHistorikElement);

                budgetkontoHistorikElement = new XElement(XName.Get("BudgetkontoHistorik", regnskabElement.Name.NamespaceName));
                budgetkontoHistorikElement.UpdateAttribute(XName.Get("kontonummer", string.Empty), budgetkontoModel.Kontonummer);
                budgetkontoHistorikElement.UpdateAttribute(XName.Get("dato", string.Empty), GetHistorikDato(sidsteMånedStatusDato));
                budgetkontoHistorikElement.UpdateAttribute(XName.Get("indtaegter", string.Empty), sidsteMånedIndtægter.ToString(CurrencyFormat, CultureInfo.InvariantCulture));
                budgetkontoHistorikElement.UpdateAttribute(XName.Get("udgifter", string.Empty), sidsteMånedUdgifter.ToString(CurrencyFormat, CultureInfo.InvariantCulture));
                budgetkontoHistorikElement.UpdateAttribute(XName.Get("bogfoert", string.Empty), budgetkontoModel.BogførtSidsteMåned.ToString(CurrencyFormat, CultureInfo.InvariantCulture));
                regnskabElement.Add(budgetkontoHistorikElement);
                return;
            }

            budgetkontoElement.UpdateAttribute(XName.Get("kontonavn", string.Empty), budgetkontoModel.Kontonavn);
            budgetkontoElement.UpdateAttribute(XName.Get("kontogruppe", string.Empty), budgetkontoModel.Kontogruppe.ToString(CultureInfo.InvariantCulture));
            budgetkontoElement.UpdateAttribute(XName.Get("beskrivelse", string.Empty), budgetkontoModel.Beskrivelse, false);
            budgetkontoElement.UpdateAttribute(XName.Get("note", string.Empty), budgetkontoModel.Notat, false);

            for (int iteration = 0; iteration < 2; iteration++)
            {
                string budgetkontoHistorikDato;
                switch (iteration)
                {
                    case 0:
                        budgetkontoHistorikDato = GetHistorikDato(budgetkontoModel.StatusDato);
                        break;

                    default:
                        budgetkontoHistorikDato = GetHistorikDato(sidsteMånedStatusDato);
                        break;
                }

                budgetkontoHistorikElement = localeDataDocument
                    .GetHistorikElements(budgetkontoModel)
                    .SingleOrDefault(m => string.CompareOrdinal(m.GetRequiredAttributeValue("dato", string.Empty), budgetkontoHistorikDato) == 0);
                if (budgetkontoHistorikElement == null)
                {
                    bool makeHistoric = true;
                    budgetkontoHistorikElement = localeDataDocument
                        .GetHistorikElements(budgetkontoModel)
                        .FirstOrDefault(m => string.CompareOrdinal(m.GetRequiredAttributeValue("dato", string.Empty), budgetkontoHistorikDato) < 0);
                    if (budgetkontoHistorikElement != null)
                    {
                        DateTime prevStatusDato = DateTime.ParseExact(budgetkontoHistorikElement.GetRequiredAttributeValue("dato", string.Empty), "yyyyMMdd", CultureInfo.InvariantCulture);
                        decimal prevIndtægter = decimal.Parse(budgetkontoHistorikElement.GetRequiredAttributeValue("indtaegter", string.Empty), NumberStyles.Any, CultureInfo.InvariantCulture);
                        decimal prevUdgifter = decimal.Parse(budgetkontoHistorikElement.GetRequiredAttributeValue("udgifter", string.Empty), NumberStyles.Any, CultureInfo.InvariantCulture);
                        decimal prevBogført = decimal.Parse(budgetkontoHistorikElement.GetRequiredAttributeValue("bogfoert", string.Empty), NumberStyles.Any, CultureInfo.InvariantCulture);
                        switch (iteration)
                        {
                            case 0:
                                makeHistoric = budgetkontoModel.StatusDato.Year != prevStatusDato.Year ||
                                               budgetkontoModel.StatusDato.Year == prevStatusDato.Year && budgetkontoModel.StatusDato.Month != prevStatusDato.Month ||
                                               budgetkontoModel.Indtægter != prevIndtægter ||
                                               budgetkontoModel.Udgifter != prevUdgifter ||
                                               budgetkontoModel.Bogført != prevBogført;
                                break;

                            default:
                                makeHistoric = budgetkontoModel.StatusDato.Year != prevStatusDato.Year ||
                                               budgetkontoModel.StatusDato.Year == prevStatusDato.Year && budgetkontoModel.StatusDato.Month != prevStatusDato.Month ||
                                               sidsteMånedIndtægter != prevIndtægter ||
                                               sidsteMånedUdgifter != prevUdgifter ||
                                               budgetkontoModel.BogførtSidsteMåned != prevBogført;
                                break;
                        }
                    }

                    if (!makeHistoric)
                    {
                        continue;
                    }

                    budgetkontoHistorikElement = new XElement(XName.Get("BudgetkontoHistorik", regnskabElement.Name.NamespaceName));
                    budgetkontoHistorikElement.UpdateAttribute(XName.Get("kontonummer", string.Empty), budgetkontoModel.Kontonummer);
                    budgetkontoHistorikElement.UpdateAttribute(XName.Get("dato", string.Empty), budgetkontoHistorikDato);
                    switch (iteration)
                    {
                        case 0:
                            budgetkontoHistorikElement.UpdateAttribute(XName.Get("indtaegter", string.Empty), budgetkontoModel.Indtægter.ToString(CurrencyFormat, CultureInfo.InvariantCulture));
                            budgetkontoHistorikElement.UpdateAttribute(XName.Get("udgifter", string.Empty), budgetkontoModel.Udgifter.ToString(CurrencyFormat, CultureInfo.InvariantCulture));
                            budgetkontoHistorikElement.UpdateAttribute(XName.Get("bogfoert", string.Empty), budgetkontoModel.Bogført.ToString(CurrencyFormat, CultureInfo.InvariantCulture));
                            regnskabElement.Add(budgetkontoHistorikElement);
                            break;

                        default:
                            budgetkontoHistorikElement.UpdateAttribute(XName.Get("indtaegter", string.Empty), sidsteMånedIndtægter.ToString(CurrencyFormat, CultureInfo.InvariantCulture));
                            budgetkontoHistorikElement.UpdateAttribute(XName.Get("udgifter", string.Empty), sidsteMånedUdgifter.ToString(CurrencyFormat, CultureInfo.InvariantCulture));
                            budgetkontoHistorikElement.UpdateAttribute(XName.Get("bogfoert", string.Empty), budgetkontoModel.BogførtSidsteMåned.ToString(CurrencyFormat, CultureInfo.InvariantCulture));
                            regnskabElement.Add(budgetkontoHistorikElement);
                            break;
                    }

                    continue;
                }

                switch (iteration)
                {
                    case 0:
                        budgetkontoHistorikElement.UpdateAttribute(XName.Get("indtaegter", string.Empty), budgetkontoModel.Indtægter.ToString(CurrencyFormat, CultureInfo.InvariantCulture));
                        budgetkontoHistorikElement.UpdateAttribute(XName.Get("udgifter", string.Empty), budgetkontoModel.Udgifter.ToString(CurrencyFormat, CultureInfo.InvariantCulture));
                        budgetkontoHistorikElement.UpdateAttribute(XName.Get("bogfoert", string.Empty), budgetkontoModel.Bogført.ToString(CurrencyFormat, CultureInfo.InvariantCulture));
                        break;

                    default:
                        budgetkontoHistorikElement.UpdateAttribute(XName.Get("indtaegter", string.Empty), sidsteMånedIndtægter.ToString(CurrencyFormat, CultureInfo.InvariantCulture));
                        budgetkontoHistorikElement.UpdateAttribute(XName.Get("udgifter", string.Empty), sidsteMånedUdgifter.ToString(CurrencyFormat, CultureInfo.InvariantCulture));
                        budgetkontoHistorikElement.UpdateAttribute(XName.Get("bogfoert", string.Empty), budgetkontoModel.BogførtSidsteMåned.ToString(CurrencyFormat, CultureInfo.InvariantCulture));
                        break;
                }
            }
        }

        /// <summary>
        /// Gemmer data for en adressekonto i det lokale datalager.
        /// </summary>
        /// <param name="adressekontoModel">Model for adressekontoen.</param>
        /// <param name="localeDataDocument">XML dokument, hvori data skal gemmes.</param>
        internal static void StoreInDocument(this IAdressekontoModel adressekontoModel, XDocument localeDataDocument)
        {
            if (adressekontoModel == null)
            {
                throw new ArgumentNullException(nameof(adressekontoModel));
            }

            if (localeDataDocument == null)
            {
                throw new ArgumentNullException(nameof(localeDataDocument));
            }

            XElement regnskabElement = localeDataDocument.GetRegnskabElement(adressekontoModel.Regnskabsnummer);
            if (regnskabElement == null)
            {
                return;
            }

            XElement adressekontoHistorikElement;
            XElement adressekontoElement = regnskabElement
                .Elements(XName.Get("Adressekonto", regnskabElement.Name.NamespaceName))
                .SingleOrDefault(m => string.CompareOrdinal(m.GetRequiredAttributeValue("nummer", string.Empty), adressekontoModel.Nummer.ToString(CultureInfo.InvariantCulture)) == 0);
            if (adressekontoElement == null)
            {
                adressekontoElement = new XElement(XName.Get("Adressekonto", regnskabElement.Name.NamespaceName));
                adressekontoElement.UpdateAttribute(XName.Get("nummer", string.Empty), adressekontoModel.Nummer.ToString(CultureInfo.InvariantCulture));
                adressekontoElement.UpdateAttribute(XName.Get("navn", string.Empty), adressekontoModel.Navn);
                adressekontoElement.UpdateAttribute(XName.Get("primaerTelefon", string.Empty), adressekontoModel.PrimærTelefon, false);
                adressekontoElement.UpdateAttribute(XName.Get("sekundaerTelefon", string.Empty), adressekontoModel.SekundærTelefon, false);
                regnskabElement.Add(adressekontoElement);

                adressekontoHistorikElement = new XElement(XName.Get("AdressekontoHistorik", regnskabElement.Name.NamespaceName));
                adressekontoHistorikElement.UpdateAttribute(XName.Get("nummer", string.Empty), adressekontoModel.Nummer.ToString(CultureInfo.InvariantCulture));
                adressekontoHistorikElement.UpdateAttribute(XName.Get("dato", string.Empty), GetHistorikDato(adressekontoModel.StatusDato));
                adressekontoHistorikElement.UpdateAttribute(XName.Get("saldo", string.Empty), adressekontoModel.Saldo.ToString(CurrencyFormat, CultureInfo.InvariantCulture));
                regnskabElement.Add(adressekontoHistorikElement);
                return;
            }

            adressekontoElement.UpdateAttribute(XName.Get("navn", string.Empty), adressekontoModel.Navn);
            adressekontoElement.UpdateAttribute(XName.Get("primaerTelefon", string.Empty), adressekontoModel.PrimærTelefon, false);
            adressekontoElement.UpdateAttribute(XName.Get("sekundaerTelefon", string.Empty), adressekontoModel.SekundærTelefon, false);

            string adressekontoHistorikDato = GetHistorikDato(adressekontoModel.StatusDato);
            adressekontoHistorikElement = localeDataDocument
                .GetHistorikElements(adressekontoModel)
                .SingleOrDefault(m => string.CompareOrdinal(m.GetRequiredAttributeValue("dato", string.Empty), adressekontoHistorikDato) == 0);
            if (adressekontoHistorikElement == null)
            {
                bool makeHistoric = true;
                adressekontoHistorikElement = localeDataDocument
                    .GetHistorikElements(adressekontoModel)
                    .FirstOrDefault(m => string.CompareOrdinal(m.GetRequiredAttributeValue("dato", string.Empty), adressekontoHistorikDato) < 0);
                if (adressekontoHistorikElement != null)
                {
                    decimal prevSaldo = decimal.Parse(adressekontoHistorikElement.GetRequiredAttributeValue("saldo", string.Empty), NumberStyles.Any, CultureInfo.InvariantCulture);
                    makeHistoric = adressekontoModel.Saldo != prevSaldo;
                }

                if (!makeHistoric)
                {
                    return;
                }

                adressekontoHistorikElement = new XElement(XName.Get("AdressekontoHistorik", regnskabElement.Name.NamespaceName));
                adressekontoHistorikElement.UpdateAttribute(XName.Get("nummer", string.Empty), adressekontoModel.Nummer.ToString(CultureInfo.InvariantCulture));
                adressekontoHistorikElement.UpdateAttribute(XName.Get("dato", string.Empty), adressekontoHistorikDato);
                adressekontoHistorikElement.UpdateAttribute(XName.Get("saldo", string.Empty), adressekontoModel.Saldo.ToString(CurrencyFormat, CultureInfo.InvariantCulture));
                regnskabElement.Add(adressekontoHistorikElement);
                return;
            }

            adressekontoHistorikElement.UpdateAttribute(XName.Get("saldo", string.Empty), adressekontoModel.Saldo.ToString(CurrencyFormat, CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Gemmer data for en bogføringslinje i det lokale datalager.
        /// </summary>
        /// <param name="bogføringslinjeModel">Model for bogføringslinjen.</param>
        /// <param name="localeDataDocument">XML dokument, hvori data skal gemmes.</param>
        /// <param name="isStoringSynchronizedData">Angivelse af, om det er synkroniserede data, der gemmes.</param>
        internal static void StoreInDocument(this IBogføringslinjeModel bogføringslinjeModel, XDocument localeDataDocument, bool isStoringSynchronizedData)
        {
            if (bogføringslinjeModel == null)
            {
                throw new ArgumentNullException(nameof(bogføringslinjeModel));
            }

            if (localeDataDocument == null)
            {
                throw new ArgumentNullException(nameof(localeDataDocument));
            }

            XElement accountingElement = localeDataDocument.GetRegnskabElement(bogføringslinjeModel.Regnskabsnummer);
            if (accountingElement == null)
            {
                return;
            }

            XElement postingLineElement = accountingElement
                .Elements(XName.Get("Bogfoeringslinje", accountingElement.Name.NamespaceName))
                .SingleOrDefault(m => string.CompareOrdinal(m.GetRequiredAttributeValue("loebenummer", string.Empty), bogføringslinjeModel.Løbenummer.ToString(CultureInfo.InvariantCulture)) == 0);
            if (postingLineElement == null)
            {
                postingLineElement = new XElement(XName.Get("Bogfoeringslinje", accountingElement.Name.NamespaceName));
                postingLineElement.UpdateAttribute(XName.Get("loebenummer", string.Empty), bogføringslinjeModel.Løbenummer.ToString(CultureInfo.InvariantCulture));
                postingLineElement.UpdateAttribute(XName.Get("dato", string.Empty), GetHistorikDato(bogføringslinjeModel.Dato));
                postingLineElement.UpdateAttribute(XName.Get("bilag", string.Empty), string.IsNullOrWhiteSpace(bogføringslinjeModel.Bilag) ? null : bogføringslinjeModel.Bilag, false);
                postingLineElement.UpdateAttribute(XName.Get("kontonummer", string.Empty), bogføringslinjeModel.Kontonummer);
                postingLineElement.UpdateAttribute(XName.Get("tekst", string.Empty), bogføringslinjeModel.Tekst);
                postingLineElement.UpdateAttribute(XName.Get("budgetkontonummer", string.Empty), string.IsNullOrWhiteSpace(bogføringslinjeModel.Budgetkontonummer) ? null : bogføringslinjeModel.Budgetkontonummer, false);
                postingLineElement.UpdateAttribute(XName.Get("debit", string.Empty), bogføringslinjeModel.Debit == 0M ? null : bogføringslinjeModel.Debit.ToString(CurrencyFormat, CultureInfo.InvariantCulture), false);
                postingLineElement.UpdateAttribute(XName.Get("kredit", string.Empty), bogføringslinjeModel.Kredit == 0M ? null : bogføringslinjeModel.Kredit.ToString(CurrencyFormat, CultureInfo.InvariantCulture), false);
                postingLineElement.UpdateAttribute(XName.Get("adressekonto", string.Empty), bogføringslinjeModel.Adressekonto == 0 ? null : bogføringslinjeModel.Adressekonto.ToString(CultureInfo.InvariantCulture), false);
                postingLineElement.UpdateAttribute(XName.Get("synkroniseret", string.Empty), isStoringSynchronizedData ? "true" : "false");
                accountingElement.Add(postingLineElement);
                return;
            }

            XAttribute pendingAttribute = postingLineElement.Attribute(XName.Get("verserende", string.Empty));
            if (pendingAttribute != null && string.IsNullOrWhiteSpace(pendingAttribute.Value) == false && Convert.ToBoolean(pendingAttribute.Value))
            {
                return;
            }

            postingLineElement.UpdateAttribute(XName.Get("dato", string.Empty), GetHistorikDato(bogføringslinjeModel.Dato));
            postingLineElement.UpdateAttribute(XName.Get("bilag", string.Empty), string.IsNullOrWhiteSpace(bogføringslinjeModel.Bilag) ? null : bogføringslinjeModel.Bilag, false);
            postingLineElement.UpdateAttribute(XName.Get("kontonummer", string.Empty), bogføringslinjeModel.Kontonummer);
            postingLineElement.UpdateAttribute(XName.Get("tekst", string.Empty), bogføringslinjeModel.Tekst);
            postingLineElement.UpdateAttribute(XName.Get("budgetkontonummer", string.Empty), string.IsNullOrWhiteSpace(bogføringslinjeModel.Budgetkontonummer) ? null : bogføringslinjeModel.Budgetkontonummer, false);
            postingLineElement.UpdateAttribute(XName.Get("debit", string.Empty), bogføringslinjeModel.Debit == 0M ? null : bogføringslinjeModel.Debit.ToString(CurrencyFormat, CultureInfo.InvariantCulture), false);
            postingLineElement.UpdateAttribute(XName.Get("kredit", string.Empty), bogføringslinjeModel.Kredit == 0M ? null : bogføringslinjeModel.Kredit.ToString(CurrencyFormat, CultureInfo.InvariantCulture), false);
            postingLineElement.UpdateAttribute(XName.Get("adressekonto", string.Empty), bogføringslinjeModel.Adressekonto == 0 ? null : bogføringslinjeModel.Adressekonto.ToString(CultureInfo.InvariantCulture), false);
            postingLineElement.UpdateAttribute(XName.Get("synkroniseret", string.Empty), isStoringSynchronizedData ? "true" : "false");
        }

        /// <summary>
        /// Gemmer data for en kontogruppe i det lokale datalager.
        /// </summary>
        /// <param name="kontogruppeModel">Model for en kontogruppe.</param>
        /// <param name="localeDataDocument">XML dokument, hvori data skal gemmes.</param>
        internal static void StoreInDocument(this IKontogruppeModel kontogruppeModel, XDocument localeDataDocument)
        {
            if (kontogruppeModel == null)
            {
                throw new ArgumentNullException(nameof(kontogruppeModel));
            }

            if (localeDataDocument == null)
            {
                throw new ArgumentNullException(nameof(localeDataDocument));
            }

            XElement kontogruppeElement = localeDataDocument.GetKontogruppeElement(kontogruppeModel.Nummer);
            if (kontogruppeElement == null)
            {
                XElement rootElement = localeDataDocument.Root;
                if (rootElement == null)
                {
                    return;
                }

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
        internal static void StoreInDocument(this IBudgetkontogruppeModel budgetkontogruppeModel, XDocument localeDataDocument)
        {
            if (budgetkontogruppeModel == null)
            {
                throw new ArgumentNullException(nameof(budgetkontogruppeModel));
            }

            if (localeDataDocument == null)
            {
                throw new ArgumentNullException(nameof(localeDataDocument));
            }

            XElement budgetkontogruppeElement = localeDataDocument.GetBudgetkontogruppeElement(budgetkontogruppeModel.Nummer);
            if (budgetkontogruppeElement == null)
            {
                XElement rootElement = localeDataDocument.Root;
                if (rootElement == null)
                {
                    return;
                }

                budgetkontogruppeElement = new XElement(XName.Get("Budgetkontogruppe", rootElement.Name.NamespaceName));
                budgetkontogruppeElement.UpdateAttribute(XName.Get("nummer", string.Empty), budgetkontogruppeModel.Nummer.ToString(CultureInfo.InvariantCulture));
                budgetkontogruppeElement.UpdateAttribute(XName.Get("tekst", string.Empty), budgetkontogruppeModel.Tekst);
                rootElement.Add(budgetkontogruppeElement);
                return;
            }

            budgetkontogruppeElement.UpdateAttribute(XName.Get("tekst", string.Empty), budgetkontogruppeModel.Tekst);
        }

        /// <summary>
        /// Gemmer versionsnummeret for repositoryet i det lokale datalager.
        /// </summary>
        /// <param name="localeDataDocument">XML dokument, hvori versionsnummeret for repositoryet skal gemmes.</param>
        /// <param name="versionNumber">Versionsummer.</param>
        internal static void StoreVersionNumberInDocument(this XDocument localeDataDocument, decimal versionNumber)
        {
            if (localeDataDocument == null)
            {
                throw new ArgumentNullException(nameof(localeDataDocument));
            }

            if (localeDataDocument.Root == null || localeDataDocument.Root.Attribute(XName.Get("version", string.Empty)) != null)
            {
                return;
            }

            localeDataDocument.Root.Add(new XAttribute(XName.Get("version", string.Empty), versionNumber.ToString(VersionFormat, CultureInfo.InvariantCulture)));
        }

        /// <summary>
        /// Gemmer datoen for sidste fulde synkronisering i det lokale datalager.
        /// </summary>
        /// <param name="localeDataDocument">XML dokument, hvori datoen for sidste fulde synkronisering skal gemmes.</param>
        /// <param name="sidsteFuldeSynkronisering">Datoen for sidste fulde synkronisering.</param>
        internal static void StoreSidsteFuldeSynkroniseringInDocument(this XDocument localeDataDocument, DateTime sidsteFuldeSynkronisering)
        {
            if (localeDataDocument == null)
            {
                throw new ArgumentNullException(nameof(localeDataDocument));
            }

            localeDataDocument.Root?.UpdateAttribute("sidsteFuldeSynkronisering", GetHistorikDato(sidsteFuldeSynkronisering), false);
        }

        /// <summary>
        /// Finder og returnerer datoen for sidste fulde synkronisering.
        /// </summary>
        /// <param name="localeDataDocument">XML dokument, hvorfra datoen for sidste fulde synkronisering skal returneres.</param>
        /// <returns>Datoen for sidste fulde synkronisering.</returns>
        internal static DateTime? GetSidsteFuldeSynkronisering(this XDocument localeDataDocument)
        {
            if (localeDataDocument == null)
            {
                throw new ArgumentNullException(nameof(localeDataDocument));
            }

            try
            {
                string attributeValue = localeDataDocument.Root?.GetNonRequiredAttributeValue("sidsteFuldeSynkronisering", string.Empty);
                if (string.IsNullOrWhiteSpace(attributeValue))
                {
                    return null;
                }

                return DateTime.ParseExact(attributeValue, "yyyyMMdd", CultureInfo.InvariantCulture);
            }
            catch (ArgumentNullException)
            {
                return null;
            }
            catch (FormatException)
            {
                return null;
            }
        }

        /// <summary>
        /// Stores a pending posting line in the <see cref="XDocument"/> containing the locale accounting data.
        /// </summary>
        /// <param name="postingLineElement">The <see cref="XElement"/> for the pending posting line.</param>
        /// <param name="pending">True when the posting line is pending otherwise false.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="postingLineElement"/> is null.</exception>
        internal static void StorePendingPostingLineInDocument(this XElement postingLineElement, bool pending = true)
        {
            if (postingLineElement == null)
            {
                throw new ArgumentNullException(nameof(postingLineElement));
            }

            postingLineElement.UpdateAttribute("verserende", pending ? "true" : "false", false);
        }

        /// <summary>
        /// Finder og returnerer elementet til et givent regnskab.
        /// </summary>
        /// <param name="localeDataDocument">XML dokument, hvorfra elementet skal returneres.</param>
        /// <param name="nummer">Unik identifikation af regnskabet.</param>
        /// <returns>Elementet til det givne regnskab.</returns>
        internal static XElement GetRegnskabElement(this XDocument localeDataDocument, int nummer)
        {
            if (localeDataDocument == null)
            {
                throw new ArgumentNullException(nameof(localeDataDocument));
            }

            XElement rootElement = localeDataDocument.Root;
            return rootElement?
                .Elements(XName.Get("Regnskab", rootElement.Name.NamespaceName))
                .SingleOrDefault(m =>string.CompareOrdinal(m.GetRequiredAttributeValue("nummer", string.Empty), nummer.ToString(CultureInfo.InvariantCulture)) == 0);
        }

        /// <summary>
        /// Finder og returnerer elementer med historiske data for en given konto.
        /// </summary>
        /// <param name="localeDataDocument">XML dokument, hvorfra elementer skal returneres.</param>
        /// <param name="kontoModel">Model for kontoen, hvortil historiske data skal returneres.</param>
        /// <returns>Elementer indeholdende historiske data til den givne konto.</returns>
        internal static IEnumerable<XElement> GetHistorikElements(this XDocument localeDataDocument, IKontoModel kontoModel)
        {
            if (localeDataDocument == null)
            {
                throw new ArgumentNullException(nameof(localeDataDocument));
            }

            if (kontoModel == null)
            {
                throw new ArgumentNullException(nameof(kontoModel));
            }

            XElement regnskabElement = localeDataDocument.GetRegnskabElement(kontoModel.Regnskabsnummer);
            if (regnskabElement == null)
            {
                return new List<XElement>(0);
            }

            IList<XElement> historikElements = regnskabElement
                .Elements(XName.Get("KontoHistorik", regnskabElement.Name.NamespaceName))
                .Where(m =>
                    string.CompareOrdinal(m.GetRequiredAttributeValue("kontonummer", string.Empty), kontoModel.Kontonummer) == 0 &&
                    string.CompareOrdinal(m.GetRequiredAttributeValue("dato", string.Empty), GetHistorikDato(kontoModel.StatusDato)) <= 0)
                .ToList();

            DateTime removeElementUntil = DateTime.Today.AddYears(-2).AddMonths(-1);
            removeElementUntil = removeElementUntil.AddDays(removeElementUntil.Day * -1);

            return historikElements.RemoveHistoricalElements(removeElementUntil, m => m.GetRequiredAttributeValue("dato", string.Empty))
                .OrderByDescending(m => m.GetRequiredAttributeValue("dato", string.Empty))
                .ToList();
        }

        /// <summary>
        /// Finder og returnerer elementer med historiske data for en given budgetkonto.
        /// </summary>
        /// <param name="localeDataDocument">XML dokument, hvorfra elementer skal returneres.</param>
        /// <param name="budgetkontoModel">Model for budgetkontoen, hvortil historiske data skal returneres.</param>
        /// <returns>Elementer indeholdende historiske data til den givne budgetkonto.</returns>
        internal static IEnumerable<XElement> GetHistorikElements(this XDocument localeDataDocument, IBudgetkontoModel budgetkontoModel)
        {
            if (localeDataDocument == null)
            {
                throw new ArgumentNullException(nameof(localeDataDocument));
            }

            if (budgetkontoModel == null)
            {
                throw new ArgumentNullException(nameof(budgetkontoModel));
            }

            XElement regnskabElement = localeDataDocument.GetRegnskabElement(budgetkontoModel.Regnskabsnummer);
            if (regnskabElement == null)
            {
                return new List<XElement>(0);
            }

            IList<XElement> historikElements = regnskabElement
                .Elements(XName.Get("BudgetkontoHistorik", regnskabElement.Name.NamespaceName))
                .Where(m =>
                    string.CompareOrdinal(m.GetRequiredAttributeValue("kontonummer", string.Empty), budgetkontoModel.Kontonummer) == 0 &&
                    string.CompareOrdinal(m.GetRequiredAttributeValue("dato", string.Empty), GetHistorikDato(budgetkontoModel.StatusDato)) <= 0)
                .ToList();

            DateTime removeElementUntil = DateTime.Today.AddYears(-2).AddMonths(-1);
            removeElementUntil = removeElementUntil.AddDays(removeElementUntil.Day * -1);

            return historikElements.RemoveHistoricalElements(removeElementUntil, m => m.GetRequiredAttributeValue("dato", string.Empty))
                .OrderByDescending(m => m.GetRequiredAttributeValue("dato", string.Empty))
                .ToList();
        }

        /// <summary>
        /// Finder og returnerer elementer med historiske data for en given adressekonto.
        /// </summary>
        /// <param name="localeDataDocument">XML dokument, hvorfra elementer skal returneres.</param>
        /// <param name="adressekontoModel">Model for adressekontoen, hvortil historiske data skal returneres.</param>
        /// <returns>Elementer indeholdende historiske data til den givne adressekonto.</returns>
        internal static IEnumerable<XElement> GetHistorikElements(this XDocument localeDataDocument, IAdressekontoModel adressekontoModel)
        {
            if (localeDataDocument == null)
            {
                throw new ArgumentNullException(nameof(localeDataDocument));
            }

            if (adressekontoModel == null)
            {
                throw new ArgumentNullException(nameof(adressekontoModel));
            }

            XElement regnskabElement = localeDataDocument.GetRegnskabElement(adressekontoModel.Regnskabsnummer);
            if (regnskabElement == null)
            {
                return new List<XElement>(0);
            }

            IList<XElement> historikElements = regnskabElement
                .Elements(XName.Get("AdressekontoHistorik", regnskabElement.Name.NamespaceName))
                .Where(m =>
                    string.CompareOrdinal(m.GetRequiredAttributeValue("nummer", string.Empty), adressekontoModel.Nummer.ToString(CultureInfo.InvariantCulture)) == 0 &&
                    string.CompareOrdinal(m.GetRequiredAttributeValue("dato", string.Empty), GetHistorikDato(adressekontoModel.StatusDato)) <= 0)
                .ToList();

            DateTime removeElementUntil = DateTime.Today.AddYears(-2).AddMonths(-1);
            removeElementUntil = removeElementUntil.AddDays(removeElementUntil.Day * -1);

            return historikElements.RemoveHistoricalElements(removeElementUntil, m => m.GetRequiredAttributeValue("dato", string.Empty))
                .OrderByDescending(m => m.GetRequiredAttributeValue("dato", string.Empty))
                .ToList();
        }

        /// <summary>
        /// Finder og returnerer elementet til en given kontogruppe.
        /// </summary>
        /// <param name="localeDataDocument">XML dokument, hvorfra elementet skal returneres.</param>
        /// <param name="nummer">Unik identifikation af kontogruppen.</param>
        /// <returns>Elementet til den givne kontogruppe.</returns>
        internal static XElement GetKontogruppeElement(this XDocument localeDataDocument, int nummer)
        {
            if (localeDataDocument == null)
            {
                throw new ArgumentNullException(nameof(localeDataDocument));
            }

            XElement rootElement = localeDataDocument.Root;
            return rootElement?
                .Elements(XName.Get("Kontogruppe", rootElement.Name.NamespaceName))
                .SingleOrDefault(m => string.CompareOrdinal(m.GetRequiredAttributeValue("nummer", string.Empty), nummer.ToString(CultureInfo.InvariantCulture)) == 0);
        }

        /// <summary>
        /// Finder og returnerer elementet til en given kontogruppe til budgetkonti.
        /// </summary>
        /// <param name="localeDataDocument">XML dokument, hvorfra elementet skal returneres.</param>
        /// <param name="nummer">Unik identifikation af kontogruppen til budgetkonti.</param>
        /// <returns>Elementet til den givne kontogruppe for budgetkonti.</returns>
        internal static XElement GetBudgetkontogruppeElement(this XDocument localeDataDocument, int nummer)
        {
            if (localeDataDocument == null)
            {
                throw new ArgumentNullException(nameof(localeDataDocument));
            }

            var rootElement = localeDataDocument.Root;
            return rootElement?
                .Elements(XName.Get("Budgetkontogruppe", rootElement.Name.NamespaceName))
                .SingleOrDefault(m => string.CompareOrdinal(m.GetRequiredAttributeValue("nummer", string.Empty), nummer.ToString(CultureInfo.InvariantCulture)) == 0);
        }

        /// <summary>
        /// Opdaterer værdien af en given XML attribute.
        /// </summary>
        /// <param name="localeDataElement">XML element, hvorpå attributværdi skal opdateres.</param>
        /// <param name="attributeName">Navn på XML attributten, der skal opdateres.</param>
        /// <param name="attributeValue">Værdi, som XML attributten skal opdateres med.</param>
        /// <param name="required">Angivelse af, om XML attributten er requried.</param>
        internal static void UpdateAttribute(this XElement localeDataElement, XName attributeName, string attributeValue, bool required = true)
        {
            if (localeDataElement == null)
            {
                throw new ArgumentNullException(nameof(localeDataElement));
            }

            if (attributeName == null)
            {
                throw new ArgumentNullException(nameof(attributeName));
            }

            if (string.IsNullOrWhiteSpace(attributeValue) && required)
            {
                throw new ArgumentNullException(nameof(attributeValue));
            }

            XAttribute attribute = localeDataElement.Attribute(attributeName);
            if (attribute == null)
            {
                if (string.IsNullOrWhiteSpace(attributeValue) && required == false)
                {
                    return;
                }

                localeDataElement.Add(new XAttribute(attributeName, attributeValue));
                return;
            }

            if (string.IsNullOrWhiteSpace(attributeValue) && required == false)
            {
                attribute.Remove();
                return;
            }

            attribute.Value = attributeValue;
        }

        internal static string GetRequiredAttributeValue(this XElement localeDataElement, string attributeName, string attributeNamespace)
        {
            if (localeDataElement == null)
            {
                throw new ArgumentNullException(nameof(localeDataElement));
            }

            if (string.IsNullOrWhiteSpace(attributeName))
            {
                throw new ArgumentNullException(nameof(attributeName));
            }

            string attributeValue = GetNonRequiredAttributeValue(localeDataElement, attributeName, attributeNamespace);
            if (string.IsNullOrWhiteSpace(attributeValue) == false)
            {
                return attributeValue;
            }

            throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.MissingRequiredAttributeValue, attributeName));
        }

        internal static string GetNonRequiredAttributeValue(this XElement localeDataElement, string attributeName, string attributeNamespace)
        {
            if (localeDataElement == null)
            {
                throw new ArgumentNullException(nameof(localeDataElement));
            }

            if (string.IsNullOrWhiteSpace(attributeName))
            {
                throw new ArgumentNullException(nameof(attributeName));
            }

            string attributeValue = localeDataElement.Attribute(XName.Get(attributeName, attributeNamespace))?.Value;
            return string.IsNullOrWhiteSpace(attributeValue) == false ? attributeValue : null;
        }

        /// <summary>
        /// Konverterer en statudato til formatet, der benyttes til en historisk dato i XML dokumentet.
        /// </summary>
        /// <param name="statusDato">Statusdato.</param>
        /// <returns>Historisk dato, som kan benyttes i XML dokumentet.</returns>
        internal static string GetHistorikDato(DateTime statusDato)
        {
            return statusDato.ToString("yyyyMMdd");
        }

        internal static IEnumerable<XElement> RemoveHistoricalElements(this IList<XElement> historicalElementCollection, DateTime removeElementUntil, Func<XElement, string> historicalDateValueGetter)
        {
            if (historicalElementCollection == null)
            {
                throw new ArgumentNullException(nameof(historicalElementCollection));
            }

            if (historicalDateValueGetter == null)
            {
                throw new ArgumentNullException(nameof(historicalDateValueGetter));
            }

            XElement elementToDelete = historicalElementCollection.FirstOrDefault(m => string.CompareOrdinal(historicalDateValueGetter(m), GetHistorikDato(removeElementUntil)) <= 0);
            while (elementToDelete != null)
            {
                elementToDelete.Remove();
                historicalElementCollection.Remove(elementToDelete);

                elementToDelete = historicalElementCollection.FirstOrDefault(m => string.CompareOrdinal(historicalDateValueGetter(m), GetHistorikDato(removeElementUntil)) <= 0);
            }

            return historicalElementCollection;
        }
    }
}