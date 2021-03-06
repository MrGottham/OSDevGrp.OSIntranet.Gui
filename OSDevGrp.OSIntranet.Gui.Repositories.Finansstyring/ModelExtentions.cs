﻿using System;
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
        #region Private constants

        private const string CurrencyFormat = "#0.00";
        private const string VersionFormat = "#0.0";

        #endregion

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
                kontoHistorikElement.UpdateAttribute(XName.Get("kredit", string.Empty), kontoModel.Kredit.ToString(CurrencyFormat, CultureInfo.InvariantCulture));
                kontoHistorikElement.UpdateAttribute(XName.Get("saldo", string.Empty), kontoModel.Saldo.ToString(CurrencyFormat, CultureInfo.InvariantCulture));
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
                var makeHistoric = true;
                kontoHistorikElement = localeDataDocument.GetHistorikElements(kontoModel).FirstOrDefault(m => string.Compare(m.Attribute(XName.Get("dato", string.Empty)).Value, kontoHistorikDato, StringComparison.Ordinal) < 0);
                if (kontoHistorikElement != null)
                {
                    var prevStatusDato = DateTime.ParseExact(kontoHistorikElement.Attribute(XName.Get("dato", string.Empty)).Value, "yyyyMMdd", CultureInfo.InvariantCulture);
                    var prevKredit = decimal.Parse(kontoHistorikElement.Attribute(XName.Get("kredit", string.Empty)).Value, NumberStyles.Any, CultureInfo.InvariantCulture);
                    var prevSaldo = decimal.Parse(kontoHistorikElement.Attribute(XName.Get("saldo", string.Empty)).Value, NumberStyles.Any, CultureInfo.InvariantCulture);
                    makeHistoric = (kontoModel.StatusDato.Year != prevStatusDato.Year) ||
                                   (kontoModel.StatusDato.Year == prevStatusDato.Year && kontoModel.StatusDato.Month != prevStatusDato.Month) ||
                                   (kontoModel.Kredit != prevKredit) ||
                                   (kontoModel.Saldo != prevSaldo);
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
        public static void StoreInDocument(this IBudgetkontoModel budgetkontoModel, XDocument localeDataDocument)
        {
            if (budgetkontoModel== null)
            {
                throw new ArgumentNullException("budgetkontoModel");
            }
            if (localeDataDocument == null)
            {
                throw new ArgumentNullException("localeDataDocument");
            }
            var regnskabElement = localeDataDocument.GetRegnskabElement(budgetkontoModel.Regnskabsnummer);
            if (regnskabElement == null)
            {
                return;
            }
            var sidsteMånedStatusDato = new DateTime(budgetkontoModel.StatusDato.AddMonths(-1).Year, budgetkontoModel.StatusDato.AddMonths(-1).Month, DateTime.DaysInMonth(budgetkontoModel.StatusDato.AddMonths(-1).Year, budgetkontoModel.StatusDato.AddMonths(-1).Month));
            var sidsteMånedIndtægter = Math.Abs(budgetkontoModel.BudgetSidsteMåned > 0 ? budgetkontoModel.BudgetSidsteMåned : 0M);
            var sidsteMånedUdgifter = Math.Abs(budgetkontoModel.BudgetSidsteMåned < 0 ? budgetkontoModel.BudgetSidsteMåned : 0M);
            XElement budgetkontoHistorikElement;
            var budgetkontoElement = regnskabElement.Elements(XName.Get("Budgetkonto", regnskabElement.Name.NamespaceName)).SingleOrDefault(m => m.Attribute(XName.Get("kontonummer", string.Empty)) != null && string.Compare(m.Attribute(XName.Get("kontonummer", string.Empty)).Value, budgetkontoModel.Kontonummer, StringComparison.Ordinal) == 0);
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

            for (var iteration = 0; iteration < 2; iteration++)
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
                budgetkontoHistorikElement = localeDataDocument.GetHistorikElements(budgetkontoModel).FirstOrDefault(m => string.Compare(m.Attribute(XName.Get("dato", string.Empty)).Value, budgetkontoHistorikDato, StringComparison.Ordinal) == 0);
                if (budgetkontoHistorikElement == null)
                {
                    var makeHistoric = true;
                    budgetkontoHistorikElement = localeDataDocument.GetHistorikElements(budgetkontoModel).FirstOrDefault(m => string.Compare(m.Attribute(XName.Get("dato", string.Empty)).Value, budgetkontoHistorikDato, StringComparison.Ordinal) < 0);
                    if (budgetkontoHistorikElement != null)
                    {
                        var prevStatusDato = DateTime.ParseExact(budgetkontoHistorikElement.Attribute(XName.Get("dato", string.Empty)).Value, "yyyyMMdd", CultureInfo.InvariantCulture);
                        var prevIndtægter = decimal.Parse(budgetkontoHistorikElement.Attribute(XName.Get("indtaegter", string.Empty)).Value, NumberStyles.Any, CultureInfo.InvariantCulture);
                        var prevUdgifter = decimal.Parse(budgetkontoHistorikElement.Attribute(XName.Get("udgifter", string.Empty)).Value, NumberStyles.Any, CultureInfo.InvariantCulture);
                        var prevBogført = decimal.Parse(budgetkontoHistorikElement.Attribute(XName.Get("bogfoert", string.Empty)).Value, NumberStyles.Any, CultureInfo.InvariantCulture);
                        switch (iteration)
                        {
                            case 0:
                                makeHistoric = (budgetkontoModel.StatusDato.Year != prevStatusDato.Year) ||
                                               (budgetkontoModel.StatusDato.Year == prevStatusDato.Year && budgetkontoModel.StatusDato.Month != prevStatusDato.Month) ||
                                               (budgetkontoModel.Indtægter != prevIndtægter) ||
                                               (budgetkontoModel.Udgifter != prevUdgifter) ||
                                               (budgetkontoModel.Bogført != prevBogført);
                                break;

                            default:
                                makeHistoric = (budgetkontoModel.StatusDato.Year != prevStatusDato.Year) ||
                                               (budgetkontoModel.StatusDato.Year == prevStatusDato.Year && budgetkontoModel.StatusDato.Month != prevStatusDato.Month) ||
                                               (sidsteMånedIndtægter != prevIndtægter) ||
                                               (sidsteMånedUdgifter != prevUdgifter) ||
                                               (budgetkontoModel.BogførtSidsteMåned != prevBogført);
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
        public static void StoreInDocument(this IAdressekontoModel adressekontoModel, XDocument localeDataDocument)
        {
            if (adressekontoModel == null)
            {
                throw new ArgumentNullException("adressekontoModel");
            }
            if (localeDataDocument == null)
            {
                throw new ArgumentNullException("localeDataDocument");
            }
            var regnskabElement = localeDataDocument.GetRegnskabElement(adressekontoModel.Regnskabsnummer);
            if (regnskabElement == null)
            {
                return;
            }
            XElement adressekontoHistorikElement;
            var adressekontoElement = regnskabElement.Elements(XName.Get("Adressekonto", regnskabElement.Name.NamespaceName)).SingleOrDefault(m => m.Attribute(XName.Get("nummer", string.Empty)) != null && string.Compare(m.Attribute(XName.Get("nummer", string.Empty)).Value, adressekontoModel.Nummer.ToString(CultureInfo.InvariantCulture), StringComparison.Ordinal) == 0);
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

            var adressekontoHistorikDato = GetHistorikDato(adressekontoModel.StatusDato);
            adressekontoHistorikElement = localeDataDocument.GetHistorikElements(adressekontoModel).FirstOrDefault(m => string.Compare(m.Attribute(XName.Get("dato", string.Empty)).Value, adressekontoHistorikDato, StringComparison.Ordinal) == 0);
            if (adressekontoHistorikElement== null)
            {
                var makeHistoric = true;
                adressekontoHistorikElement = localeDataDocument.GetHistorikElements(adressekontoModel).FirstOrDefault(m => string.Compare(m.Attribute(XName.Get("dato", string.Empty)).Value, adressekontoHistorikDato, StringComparison.Ordinal) < 0);
                if (adressekontoHistorikElement != null)
                {
                    var prevSaldo = decimal.Parse(adressekontoHistorikElement.Attribute(XName.Get("saldo", string.Empty)).Value, NumberStyles.Any, CultureInfo.InvariantCulture);
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
        public static void StoreInDocument(this IBogføringslinjeModel bogføringslinjeModel, XDocument localeDataDocument, bool isStoringSynchronizedData)
        {
            if (bogføringslinjeModel == null)
            {
                throw new ArgumentNullException("bogføringslinjeModel");
            }
            if (localeDataDocument == null)
            {
                throw new ArgumentNullException("localeDataDocument");
            }

            XElement accountingElement = localeDataDocument.GetRegnskabElement(bogføringslinjeModel.Regnskabsnummer);
            if (accountingElement == null)
            {
                return;
            }

            XElement postingLineElement = accountingElement.Elements(XName.Get("Bogfoeringslinje", accountingElement.Name.NamespaceName)).SingleOrDefault(m => m.Attribute(XName.Get("loebenummer", string.Empty)) != null && string.Compare(m.Attribute(XName.Get("loebenummer", string.Empty)).Value, bogføringslinjeModel.Løbenummer.ToString(CultureInfo.InvariantCulture), StringComparison.Ordinal) == 0);
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
        /// Gemmer versionsnummeret for repositoryet i det lokale datalager.
        /// </summary>
        /// <param name="localeDataDocument">XML dokument, hvori versionsnummeret for repositoryet skal gemmes.</param>
        /// <param name="versionNumber">Versionsummer.</param>
        public static void StoreVersionNumberInDocument(this XDocument localeDataDocument, decimal versionNumber)
        {
            if (localeDataDocument == null)
            {
                throw new ArgumentNullException("localeDataDocument");
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
        public static void StoreSidsteFuldeSynkroniseringInDocument(this XDocument localeDataDocument, DateTime sidsteFuldeSynkronisering)
        {
            if (localeDataDocument == null)
            {
                throw new ArgumentNullException("localeDataDocument");
            }
            if (localeDataDocument.Root == null)
            {
                return;
            }
            localeDataDocument.Root.UpdateAttribute("sidsteFuldeSynkronisering", GetHistorikDato(sidsteFuldeSynkronisering), false);
        }

        /// <summary>
        /// Finder og returnerer datoen for sidste fulde synkronisering.
        /// </summary>
        /// <param name="localeDataDocument">XML dokument, hvorfra datoen for sidste fulde synkronisering skal returneres.</param>
        /// <returns>Datoen for sidste fulde synkronisering.</returns>
        public static DateTime? GetSidsteFuldeSynkronisering(this XDocument localeDataDocument)
        {
            if (localeDataDocument == null)
            {
                throw new ArgumentNullException("localeDataDocument");
            }
            try
            {
                if (localeDataDocument.Root == null || localeDataDocument.Root.Attribute(XName.Get("sidsteFuldeSynkronisering", string.Empty)) == null)
                {
                    return null;
                }
                return DateTime.ParseExact(localeDataDocument.Root.Attribute(XName.Get("sidsteFuldeSynkronisering", string.Empty)).Value, "yyyyMMdd", CultureInfo.InvariantCulture);
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
        public static void StorePendingPostingLineInDocument(this XElement postingLineElement, bool pending = true)
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
            var removeElementUntil = DateTime.Today.AddYears(-2).AddMonths(-1);
            removeElementUntil = removeElementUntil.AddDays((removeElementUntil.Day)*-1);
            var elementToDelete = historikElements.FirstOrDefault(m => string.Compare(m.Attribute(XName.Get("dato", string.Empty)).Value, GetHistorikDato(removeElementUntil), StringComparison.Ordinal) <= 0);
            while (elementToDelete != null)
            {
                elementToDelete.Remove();
                historikElements.Remove(elementToDelete);
                elementToDelete = historikElements.FirstOrDefault(m => string.Compare(m.Attribute(XName.Get("dato", string.Empty)).Value, GetHistorikDato(removeElementUntil), StringComparison.Ordinal) <= 0);
            }
            return historikElements.OrderByDescending(m => m.Attribute(XName.Get("dato", string.Empty)).Value).ToList();
        }

        /// <summary>
        /// Finder og returnerer elementer med historiske data for en given budgetkonto.
        /// </summary>
        /// <param name="localeDataDocument">XML dokument, hvorfra elementer skal returneres.</param>
        /// <param name="budgetkontoModel">Model for budgetkontoen, hvortil historiske data skal returneres.</param>
        /// <returns>Elementer indeholdende historiske data til den givne budgetkonto.</returns>
        public static IEnumerable<XElement> GetHistorikElements(this XDocument localeDataDocument, IBudgetkontoModel budgetkontoModel)
        {
            if (localeDataDocument == null)
            {
                throw new ArgumentNullException("localeDataDocument");
            }
            if (budgetkontoModel == null)
            {
                throw new ArgumentNullException("budgetkontoModel");
            }
            var regnskabElement = localeDataDocument.GetRegnskabElement(budgetkontoModel.Regnskabsnummer);
            if (regnskabElement == null)
            {
                return new List<XElement>(0);
            }
            var historikElements = regnskabElement.Elements(XName.Get("BudgetkontoHistorik", regnskabElement.Name.NamespaceName)).Where(m => m.Attribute(XName.Get("kontonummer", string.Empty)) != null && string.Compare(m.Attribute(XName.Get("kontonummer", string.Empty)).Value, budgetkontoModel.Kontonummer, StringComparison.Ordinal) == 0 && m.Attribute(XName.Get("dato", string.Empty)) != null && string.Compare(m.Attribute(XName.Get("dato", string.Empty)).Value, GetHistorikDato(budgetkontoModel.StatusDato), StringComparison.Ordinal) <= 0).ToList();
            var removeElementUntil = DateTime.Today.AddYears(-2).AddMonths(-1);
            removeElementUntil = removeElementUntil.AddDays((removeElementUntil.Day)*-1);
            var elementToDelete = historikElements.FirstOrDefault(m => string.Compare(m.Attribute(XName.Get("dato", string.Empty)).Value, GetHistorikDato(removeElementUntil), StringComparison.Ordinal) <= 0);
            while (elementToDelete != null)
            {
                elementToDelete.Remove();
                historikElements.Remove(elementToDelete);
                elementToDelete = historikElements.FirstOrDefault(m => string.Compare(m.Attribute(XName.Get("dato", string.Empty)).Value, GetHistorikDato(removeElementUntil), StringComparison.Ordinal) <= 0);
            }
            return historikElements.OrderByDescending(m => m.Attribute(XName.Get("dato", string.Empty)).Value).ToList();
        }

        /// <summary>
        /// Finder og returnerer elementer med historiske data for en given adressekonto.
        /// </summary>
        /// <param name="localeDataDocument">XML dokument, hvorfra elementer skal returneres.</param>
        /// <param name="adressekontoModel">Model for adressekontoen, hvortil historiske data skal returneres.</param>
        /// <returns>Elementer indeholdende historiske data til den givne adressekonto.</returns>
        public static IEnumerable<XElement> GetHistorikElements(this XDocument localeDataDocument, IAdressekontoModel adressekontoModel)
        {
            if (localeDataDocument == null)
            {
                throw new ArgumentNullException("localeDataDocument");
            }
            if (adressekontoModel == null)
            {
                throw new ArgumentNullException("adressekontoModel");
            }
            var regnskabElement = localeDataDocument.GetRegnskabElement(adressekontoModel.Regnskabsnummer);
            if (regnskabElement == null)
            {
                return new List<XElement>(0);
            }
            var historikElements = regnskabElement.Elements(XName.Get("AdressekontoHistorik", regnskabElement.Name.NamespaceName)).Where(m => m.Attribute(XName.Get("nummer", string.Empty)) != null && string.Compare(m.Attribute(XName.Get("nummer", string.Empty)).Value, adressekontoModel.Nummer.ToString(CultureInfo.InvariantCulture), StringComparison.Ordinal) == 0 && m.Attribute(XName.Get("dato", string.Empty)) != null && string.Compare(m.Attribute(XName.Get("dato", string.Empty)).Value, GetHistorikDato(adressekontoModel.StatusDato), StringComparison.Ordinal) <= 0).ToList();
            var removeElementUntil = DateTime.Today.AddYears(-2).AddMonths(-1);
            removeElementUntil = removeElementUntil.AddDays((removeElementUntil.Day)*-1);
            var elementToDelete = historikElements.FirstOrDefault(m => string.Compare(m.Attribute(XName.Get("dato", string.Empty)).Value, GetHistorikDato(removeElementUntil), StringComparison.Ordinal) <= 0);
            while (elementToDelete != null)
            {
                elementToDelete.Remove();
                historikElements.Remove(elementToDelete);
                elementToDelete = historikElements.FirstOrDefault(m => string.Compare(m.Attribute(XName.Get("dato", string.Empty)).Value, GetHistorikDato(removeElementUntil), StringComparison.Ordinal) <= 0);
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
        public static void UpdateAttribute(this XElement localeDataElement, XName attributeName, string attributeValue, bool required = true)
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
        public static string GetHistorikDato(DateTime statusDato)
        {
            return statusDato.ToString("yyyyMMdd");
        }
    }
}
