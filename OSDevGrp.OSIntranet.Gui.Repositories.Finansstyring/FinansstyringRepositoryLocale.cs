using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Events;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Models.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.Resources;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Finansstyring
{
    /// <summary>
    /// Repository, der supporterer lokale data til finansstyring.
    /// </summary>
    public class FinansstyringRepositoryLocale : IFinansstyringRepository
    {
        #region Constants

        public const decimal RepositoryVersion = 1.0M;
        public const string XmlSchema = "Schemas.FinansstyringRepositoryLocale.xsd";
        public const string Namespace = "urn:osdevgrp:osintranet:finansstyringrepository:locale:1.0.0";

        #endregion

        #region Private variables

        private readonly ILocaleDataStorage _localeDataStorage;
        private static readonly object SyncRoot = new object();

        #endregion

        #region Constructor

        /// <summary>
        /// Danner repository, der supporterer lokale data til finansstyring.
        /// </summary>
        /// <param name="finansstyringKonfigurationRepository">Implementering af konfigurationsrepository, der supporterer finansstyring.</param>
        /// <param name="localeDataStorage">Implementering af et lokalt datalager.</param>
        public FinansstyringRepositoryLocale(IFinansstyringKonfigurationRepository finansstyringKonfigurationRepository, ILocaleDataStorage localeDataStorage)
        {
            if (finansstyringKonfigurationRepository == null)
            {
                throw new ArgumentNullException(nameof(finansstyringKonfigurationRepository));
            }

            if (localeDataStorage == null)
            {
                throw new ArgumentNullException(nameof(localeDataStorage));
            }

            Konfiguration = finansstyringKonfigurationRepository;
            _localeDataStorage = localeDataStorage;

            lock (SyncRoot)
            {
                _localeDataStorage.PrepareLocaleData += PrepareLocaleDataEventHandler;
                if (_localeDataStorage.HasLocaleData)
                {
                    return;
                }

                DateTime currentDate = DateTime.Today;
                _localeDataStorage.StoreLocaleData((IRegnskabModel)new RegnskabModel(1, Resource.GetText(Text.PrivateAccounting)));
                _localeDataStorage.StoreLocaleData((IKontogruppeModel)new KontogruppeModel(1, Resource.GetText(Text.Cash), Balancetype.Aktiver));
                _localeDataStorage.StoreLocaleData((IKontogruppeModel)new KontogruppeModel(2, Resource.GetText(Text.BankAccounts), Balancetype.Aktiver));
                _localeDataStorage.StoreLocaleData((IKontogruppeModel)new KontogruppeModel(3, Resource.GetText(Text.Loans), Balancetype.Passiver));
                _localeDataStorage.StoreLocaleData((IBudgetkontogruppeModel)new BudgetkontogruppeModel(1, Resource.GetText(Text.Income)));
                _localeDataStorage.StoreLocaleData((IBudgetkontogruppeModel)new BudgetkontogruppeModel(2, Resource.GetText(Text.Expenses)));
                _localeDataStorage.StoreLocaleData((IKontoModel)new KontoModel(1, "10110", Resource.GetText(Text.CashAccount, "10110"), 1, currentDate));
                _localeDataStorage.StoreLocaleData((IKontoModel)new KontoModel(1, "10120", Resource.GetText(Text.CashAccount, "10120"), 1, currentDate));
                _localeDataStorage.StoreLocaleData((IKontoModel)new KontoModel(1, "10130", Resource.GetText(Text.CashAccount, "10130"), 1, currentDate));
                _localeDataStorage.StoreLocaleData((IKontoModel)new KontoModel(1, "10210", Resource.GetText(Text.BankAccount, "10210"), 2, currentDate));
                _localeDataStorage.StoreLocaleData((IKontoModel)new KontoModel(1, "10220", Resource.GetText(Text.BankAccount, "10220"), 2, currentDate));
                _localeDataStorage.StoreLocaleData((IKontoModel)new KontoModel(1, "10230", Resource.GetText(Text.BankAccount, "10230"), 2, currentDate));
                _localeDataStorage.StoreLocaleData((IKontoModel)new KontoModel(1, "10240", Resource.GetText(Text.BankAccount, "10240"), 2, currentDate));
                _localeDataStorage.StoreLocaleData((IKontoModel)new KontoModel(1, "10250", Resource.GetText(Text.BankAccount, "10250"), 2, currentDate));
                _localeDataStorage.StoreLocaleData((IKontoModel)new KontoModel(1, "20110", Resource.GetText(Text.LoanAccount, "20110"), 3, currentDate));
                _localeDataStorage.StoreLocaleData((IKontoModel)new KontoModel(1, "20120", Resource.GetText(Text.LoanAccount, "20120"), 3, currentDate));
                _localeDataStorage.StoreLocaleData((IKontoModel)new KontoModel(1, "20130", Resource.GetText(Text.LoanAccount, "20130"), 3, currentDate));
                _localeDataStorage.StoreLocaleData((IKontoModel)new KontoModel(1, "20140", Resource.GetText(Text.LoanAccount, "20140"), 3, currentDate));
                _localeDataStorage.StoreLocaleData((IKontoModel)new KontoModel(1, "20150", Resource.GetText(Text.LoanAccount, "20150"), 3, currentDate));
                _localeDataStorage.StoreLocaleData((IBudgetkontoModel)new BudgetkontoModel(1, "1010", Resource.GetText(Text.Wages), 1, currentDate));
                _localeDataStorage.StoreLocaleData((IBudgetkontoModel)new BudgetkontoModel(1, "1020", Resource.GetText(Text.Other), 1, currentDate));
                _localeDataStorage.StoreLocaleData((IBudgetkontoModel)new BudgetkontoModel(1, "2010", Resource.GetText(Text.Household), 2, currentDate));
                _localeDataStorage.StoreLocaleData((IBudgetkontoModel)new BudgetkontoModel(1, "2020", Resource.GetText(Text.HousingCost), 2, currentDate));
                _localeDataStorage.StoreLocaleData((IBudgetkontoModel)new BudgetkontoModel(1, "2030", Resource.GetText(Text.Insurances), 2, currentDate));
                _localeDataStorage.StoreLocaleData((IBudgetkontoModel)new BudgetkontoModel(1, "2040", Resource.GetText(Text.Vehicles), 2, currentDate));
                _localeDataStorage.StoreLocaleData((IBudgetkontoModel)new BudgetkontoModel(1, "2050", Resource.GetText(Text.Vacations), 2, currentDate));
                _localeDataStorage.StoreLocaleData((IBudgetkontoModel)new BudgetkontoModel(1, "2060", Resource.GetText(Text.Expenses), 2, currentDate));
                _localeDataStorage.StoreLocaleData((IBudgetkontoModel)new BudgetkontoModel(1, "2070", Resource.GetText(Text.Other), 2, currentDate));
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returnerer konfigurationsrepositoryet, der supporterer finansstyring.
        /// </summary>
        public virtual IFinansstyringKonfigurationRepository Konfiguration { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Henter en liste af regnskaber.
        /// </summary>
        /// <returns>Liste af regnskaber.</returns>
        public virtual Task<IEnumerable<IRegnskabModel>> RegnskabslisteGetAsync()
        {
            return Task.Run<IEnumerable<IRegnskabModel>>(() =>
            {
                try
                {
                    XDocument localeDataDocument = _localeDataStorage.GetLocaleData();
                    if (localeDataDocument.Root == null)
                    {
                        return new List<IRegnskabModel>(0);
                    }

                    IList<IRegnskabModel> regnskaber = new List<IRegnskabModel>();
                    foreach (XElement regnskabElement in localeDataDocument.Root.Elements(XName.Get("Regnskab", Namespace)))
                    {
                        try
                        {
                            IRegnskabModel regnskabModel = new RegnskabModel(
                                int.Parse(regnskabElement.GetRequiredAttributeValue("nummer", string.Empty), NumberStyles.Integer, CultureInfo.InvariantCulture),
                                regnskabElement.GetRequiredAttributeValue("navn", string.Empty));
                            regnskaber.Add(regnskabModel);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                        }
                    }

                    return regnskaber.OrderBy(m => m.Nummer).ToList();
                }
                catch (IntranetGuiRepositoryException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "RegnskabslisteGetAsync", ex.Message), ex);
                }
            });
        }

        /// <summary>
        /// Henter kontoplanen til et givent regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, hvortil kontoplanen skal hentes.</param>
        /// <param name="statusDato">Statusdato for kontoplanen.</param>
        /// <returns>Kontoplan.</returns>
        public virtual Task<IEnumerable<IKontoModel>> KontoplanGetAsync(int regnskabsnummer, DateTime statusDato)
        {
            return Task.Run<IEnumerable<IKontoModel>>(async () =>
            {
                try
                {
                    XDocument localeDataDocument = _localeDataStorage.GetLocaleData();
                    XElement regnskabElement = localeDataDocument.GetRegnskabElement(regnskabsnummer);
                    if (regnskabElement == null)
                    {
                        return new List<IKontoModel>(0);
                    }

                    IKontogruppeModel[] kontogrupper = (await KontogruppelisteGetAsync()).ToArray();
                    IList<IKontoModel> kontoplan = new List<IKontoModel>();
                    foreach (XElement kontoElement in regnskabElement.Elements(XName.Get("Konto", Namespace)))
                    {
                        try
                        {
                            int kontogruppe = int.Parse(kontoElement.GetRequiredAttributeValue("kontogruppe", string.Empty), NumberStyles.Integer, CultureInfo.InvariantCulture);
                            if (kontogrupper.Select(m => m.Nummer).Contains(kontogruppe) == false)
                            {
                                continue;
                            }

                            IKontoModel kontoModel = new KontoModel(
                                int.Parse(regnskabElement.GetRequiredAttributeValue("nummer", string.Empty), NumberStyles.Integer, CultureInfo.InvariantCulture),
                                kontoElement.GetRequiredAttributeValue("kontonummer", string.Empty),
                                kontoElement.GetRequiredAttributeValue("kontonavn", string.Empty),
                                kontogruppe,
                                statusDato);
                            kontoModel.Beskrivelse = kontoElement.GetNonRequiredAttributeValue("beskrivelse", string.Empty);
                            kontoModel.Notat = kontoElement.GetNonRequiredAttributeValue("note", string.Empty);

                            XElement historikElement = localeDataDocument.GetHistorikElements(kontoModel).FirstOrDefault(m => string.CompareOrdinal(m.GetRequiredAttributeValue("dato", string.Empty), ModelExtensions.GetHistorikDato(statusDato)) <= 0);
                            if (historikElement != null)
                            {
                                kontoModel.Kredit = decimal.Parse(historikElement.GetRequiredAttributeValue("kredit", string.Empty), NumberStyles.Any, CultureInfo.InvariantCulture);
                                kontoModel.Saldo = decimal.Parse(historikElement.GetRequiredAttributeValue("saldo", string.Empty), NumberStyles.Any, CultureInfo.InvariantCulture);
                            }

                            kontoplan.Add(kontoModel);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                        }
                    }

                    return kontoplan.OrderBy(m => m.Kontonummer).ToList();
                }
                catch (IntranetGuiRepositoryException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "KontoplanGetAsync", ex.Message), ex);
                }
            });
        }

        /// <summary>
        /// Henter en given konto i et givent regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, hvorfra kontoen skal hentes.</param>
        /// <param name="kontonummer">Kontonummer på kontoen, der skal hentes.</param>
        /// <param name="statusDato">Statusdato for kontoen.</param>
        /// <returns>Konto.</returns>
        public virtual Task<IKontoModel> KontoGetAsync(int regnskabsnummer, string kontonummer, DateTime statusDato)
        {
            if (string.IsNullOrWhiteSpace(kontonummer))
            {
                throw new ArgumentNullException(nameof(kontonummer));
            }

            return Task.Run(async () =>
            {
                try
                {
                    IEnumerable<IKontoModel> kontoplan = await KontoplanGetAsync(regnskabsnummer, statusDato);
                    try
                    {
                        return kontoplan.Single(m => string.CompareOrdinal(m.Kontonummer, kontonummer) == 0);
                    }
                    catch (InvalidOperationException ex)
                    {
                        throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.AccountNotFound, kontonummer), ex);
                    }
                }
                catch (IntranetGuiRepositoryException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "KontoGetAsync", ex.Message), ex);
                }
            });
        }

        /// <summary>
        /// Henter budgetkontoplanen til et givent regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, hvortil budgetkontoplanen skal hentes.</param>
        /// <param name="statusDato">Statusdato for budgetkontoplanen.</param>
        /// <returns>Budgetkontoplan.</returns>
        public virtual Task<IEnumerable<IBudgetkontoModel>> BudgetkontoplanGetAsync(int regnskabsnummer, DateTime statusDato)
        {
            return Task.Run<IEnumerable<IBudgetkontoModel>>(async () =>
            {
                try
                {
                    XDocument localeDataDocument = _localeDataStorage.GetLocaleData();
                    XElement regnskabElement = localeDataDocument.GetRegnskabElement(regnskabsnummer);
                    if (regnskabElement == null)
                    {
                        return new List<IBudgetkontoModel>(0);
                    }

                    IBudgetkontogruppeModel[] budgetkontogrupper = (await BudgetkontogruppelisteGetAsync()).ToArray();
                    IList<IBudgetkontoModel> budgetkontoplan = new List<IBudgetkontoModel>();
                    foreach (var budgetkontoElement in regnskabElement.Elements(XName.Get("Budgetkonto", Namespace)))
                    {
                        try
                        {
                            int kontogruppe = int.Parse(budgetkontoElement.GetRequiredAttributeValue("kontogruppe", string.Empty), NumberStyles.Integer, CultureInfo.InvariantCulture);
                            if (budgetkontogrupper.Select(m => m.Nummer).Contains(kontogruppe) == false)
                            {
                                continue;
                            }

                            IBudgetkontoModel budgetkontoModel = new BudgetkontoModel(
                                int.Parse(regnskabElement.GetRequiredAttributeValue("nummer", string.Empty), NumberStyles.Integer, CultureInfo.InvariantCulture),
                                budgetkontoElement.GetRequiredAttributeValue("kontonummer", string.Empty),
                                budgetkontoElement.GetRequiredAttributeValue("kontonavn", string.Empty),
                                kontogruppe,
                                statusDato);
                            budgetkontoModel.Beskrivelse = budgetkontoElement.GetNonRequiredAttributeValue("beskrivelse", string.Empty);
                            budgetkontoModel.Notat = budgetkontoElement.GetNonRequiredAttributeValue("note", string.Empty);

                            XElement[] historikElements = localeDataDocument.GetHistorikElements(budgetkontoModel).ToArray();
                            XElement historikElementForStatusDato = historikElements.FirstOrDefault(m => string.CompareOrdinal(m.GetRequiredAttributeValue("dato", string.Empty), ModelExtensions.GetHistorikDato(statusDato)) <= 0);
                            if (historikElementForStatusDato != null)
                            {
                                DateTime datoForHistorikElement = DateTime.ParseExact(historikElementForStatusDato.GetRequiredAttributeValue("dato", string.Empty), "yyyyMMdd", CultureInfo.InvariantCulture);
                                budgetkontoModel.Indtægter = decimal.Parse(historikElementForStatusDato.GetRequiredAttributeValue("indtaegter", string.Empty), NumberStyles.Any, CultureInfo.InvariantCulture);
                                budgetkontoModel.Udgifter = decimal.Parse(historikElementForStatusDato.GetRequiredAttributeValue("udgifter", string.Empty), NumberStyles.Any, CultureInfo.InvariantCulture);
                                if (datoForHistorikElement.Year == budgetkontoModel.StatusDato.Year && datoForHistorikElement.Month == budgetkontoModel.StatusDato.Month)
                                {
                                    budgetkontoModel.Bogført = decimal.Parse(historikElementForStatusDato.GetRequiredAttributeValue("bogfoert", string.Empty), NumberStyles.Any, CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    budgetkontoModel.Bogført = 0M;
                                }
                            }

                            DateTime historikDato = new DateTime(budgetkontoModel.StatusDato.AddMonths(-1).Year, budgetkontoModel.StatusDato.AddMonths(-1).Month, DateTime.DaysInMonth(budgetkontoModel.StatusDato.AddMonths(-1).Year, budgetkontoModel.StatusDato.AddMonths(-1).Month));
                            DateTime compareDate1 = historikDato;
                            XElement historikElementForDato = historikElements.FirstOrDefault(m => string.CompareOrdinal(m.GetRequiredAttributeValue("dato", string.Empty), ModelExtensions.GetHistorikDato(compareDate1)) <= 0);
                            if (historikElementForDato != null)
                            {
                                budgetkontoModel.BudgetSidsteMåned += decimal.Parse(historikElementForDato.GetRequiredAttributeValue("indtaegter", string.Empty), NumberStyles.Any, CultureInfo.InvariantCulture);
                                budgetkontoModel.BudgetSidsteMåned -= decimal.Parse(historikElementForDato.GetRequiredAttributeValue("udgifter", string.Empty), NumberStyles.Any, CultureInfo.InvariantCulture);
                                budgetkontoModel.BogførtSidsteMåned = decimal.Parse(historikElementForDato.GetRequiredAttributeValue("bogfoert", string.Empty), NumberStyles.Any, CultureInfo.InvariantCulture);
                            }

                            budgetkontoModel.BudgetÅrTilDato = budgetkontoModel.Budget;
                            budgetkontoModel.BogførtÅrTilDato = budgetkontoModel.Bogført;
                            for (int month = budgetkontoModel.StatusDato.Month - 1; month > 0; month--)
                            {
                                historikDato = new DateTime(budgetkontoModel.StatusDato.Year, month, DateTime.DaysInMonth(budgetkontoModel.StatusDato.Year, month));
                                DateTime compareDate2 = historikDato;
                                historikElementForDato = historikElements.FirstOrDefault(m => string.CompareOrdinal(m.GetRequiredAttributeValue("dato", string.Empty), ModelExtensions.GetHistorikDato(compareDate2)) <= 0);
                                if (historikElementForDato != null)
                                {
                                    budgetkontoModel.BudgetÅrTilDato += decimal.Parse(historikElementForDato.GetRequiredAttributeValue("indtaegter", string.Empty), NumberStyles.Any, CultureInfo.InvariantCulture);
                                    budgetkontoModel.BudgetÅrTilDato -= decimal.Parse(historikElementForDato.GetRequiredAttributeValue("udgifter", string.Empty), NumberStyles.Any, CultureInfo.InvariantCulture);
                                    budgetkontoModel.BogførtÅrTilDato += decimal.Parse(historikElementForDato.GetRequiredAttributeValue("bogfoert", string.Empty), NumberStyles.Any, CultureInfo.InvariantCulture);
                                }
                            }

                            budgetkontoModel.BudgetSidsteÅr = 0M;
                            budgetkontoModel.BogførtSidsteÅr = 0M;
                            for (var month = 12; month > 0; month--)
                            {
                                historikDato = new DateTime(budgetkontoModel.StatusDato.AddYears(-1).Year, month, DateTime.DaysInMonth(budgetkontoModel.StatusDato.AddYears(-1).Year, month));
                                DateTime compareDate3 = historikDato;
                                historikElementForDato = historikElements.FirstOrDefault(m => string.CompareOrdinal(m.GetRequiredAttributeValue("dato", string.Empty), ModelExtensions.GetHistorikDato(compareDate3)) <= 0);
                                if (historikElementForDato != null)
                                {
                                    budgetkontoModel.BudgetSidsteÅr += decimal.Parse(historikElementForDato.GetRequiredAttributeValue("indtaegter", string.Empty), NumberStyles.Any, CultureInfo.InvariantCulture);
                                    budgetkontoModel.BudgetSidsteÅr -= decimal.Parse(historikElementForDato.GetRequiredAttributeValue("udgifter", string.Empty), NumberStyles.Any, CultureInfo.InvariantCulture);
                                    budgetkontoModel.BogførtSidsteÅr += decimal.Parse(historikElementForDato.GetRequiredAttributeValue("bogfoert", string.Empty), NumberStyles.Any, CultureInfo.InvariantCulture);
                                }
                            }

                            budgetkontoplan.Add(budgetkontoModel);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                        }
                    }

                    return budgetkontoplan.OrderBy(m => m.Kontonummer).ToList();
                }
                catch (IntranetGuiRepositoryException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "BudgetkontoplanGetAsync", ex.Message), ex);
                }
            });
        }

        /// <summary>
        /// Henter en given budgetkonto i et givent regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, hvorfra budgetkontoen skal hentes.</param>
        /// <param name="budgetkontonummer">Kontonummer på budgetkontoen, der skal hentes.</param>
        /// <param name="statusDato">Statusdato for budgetkontoen.</param>
        /// <returns>Budgetkonto.</returns>
        public virtual Task<IBudgetkontoModel> BudgetkontoGetAsync(int regnskabsnummer, string budgetkontonummer, DateTime statusDato)
        {
            if (string.IsNullOrWhiteSpace(budgetkontonummer))
            {
                throw new ArgumentNullException(nameof(budgetkontonummer));
            }

            return Task.Run(async () =>
            {
                try
                {
                    IEnumerable<IBudgetkontoModel> budgetkontoplan = await BudgetkontoplanGetAsync(regnskabsnummer, statusDato);
                    try
                    {
                        return budgetkontoplan.Single(m => string.CompareOrdinal(m.Kontonummer, budgetkontonummer) == 0);
                    }
                    catch (InvalidOperationException ex)
                    {
                        throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.BudgetAccountNotFound, budgetkontonummer), ex);
                    }
                }
                catch (IntranetGuiRepositoryException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "BudgetkontoGetAsync", ex.Message), ex);
                }
            });
        }

        /// <summary>
        /// Henter et givent antal bogføringslinjer til et regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, hvortil bogføringslinjer skal hentes.</param>
        /// <param name="statusDato">Dato, hvorfra bogføringslinjer skal hentes.</param>
        /// <param name="antalBogføringslinjer">Antal bogføringslinjer, der skal hentes.</param>
        /// <returns>Bogføringslinjer til regnskabet.</returns>
        public virtual Task<IEnumerable<IBogføringslinjeModel>> BogføringslinjerGetAsync(int regnskabsnummer, DateTime statusDato, int antalBogføringslinjer)
        {
            return Task.Run<IEnumerable<IBogføringslinjeModel>>(async () =>
            {
                try
                {
                    XDocument localeDataDocument = _localeDataStorage.GetLocaleData();
                    XElement regnskabElement = localeDataDocument.GetRegnskabElement(regnskabsnummer);
                    if (regnskabElement == null)
                    {
                        return new List<IBogføringslinjeModel>(0);
                    }

                    IKontoModel[] kontoplan = (await KontoplanGetAsync(regnskabsnummer, statusDato)).ToArray();
                    IBudgetkontoModel[] budgetkontoplan = (await BudgetkontoplanGetAsync(regnskabsnummer, statusDato)).ToArray();
                    IAdressekontoModel[] adressekontoliste = (await AdressekontolisteGetAsync(regnskabsnummer, statusDato)).ToArray();

                    IList<IBogføringslinjeModel> bogføringslinjer = new List<IBogføringslinjeModel>();
                    foreach (XElement bogføringslinjeElement in regnskabElement.Elements(XName.Get("Bogfoeringslinje", Namespace)))
                    {
                        try
                        {
                            string kontonummer = bogføringslinjeElement.GetRequiredAttributeValue("kontonummer", string.Empty);
                            if (kontoplan.Select(m => m.Kontonummer).Contains(kontonummer) == false)
                            {
                                continue;
                            }

                            string debitValue = bogføringslinjeElement.GetNonRequiredAttributeValue("debit", string.Empty);
                            string kreditValue= bogføringslinjeElement.GetNonRequiredAttributeValue("kredit", string.Empty);
                            IBogføringslinjeModel bogføringslinjeModel = new BogføringslinjeModel(
                                int.Parse(regnskabElement.GetRequiredAttributeValue("nummer", string.Empty), NumberStyles.Integer, CultureInfo.InvariantCulture),
                                int.Parse(bogføringslinjeElement.GetRequiredAttributeValue("loebenummer", string.Empty), NumberStyles.Integer, CultureInfo.InvariantCulture),
                                DateTime.ParseExact(bogføringslinjeElement.GetRequiredAttributeValue("dato", string.Empty), "yyyyMMdd", CultureInfo.InvariantCulture),
                                kontonummer,
                                bogføringslinjeElement.GetRequiredAttributeValue("tekst", string.Empty),
                                string.IsNullOrWhiteSpace(debitValue) ? 0M : decimal.Parse(debitValue, NumberStyles.Any, CultureInfo.InvariantCulture),
                                string.IsNullOrWhiteSpace(kreditValue) ? 0M : decimal.Parse(kreditValue, NumberStyles.Any, CultureInfo.InvariantCulture));
                            bogføringslinjeModel.Bilag = bogføringslinjeElement.GetNonRequiredAttributeValue("bilag", string.Empty);

                            string attributeValue = bogføringslinjeElement.GetNonRequiredAttributeValue("budgetkontonummer", string.Empty);
                            if (string.IsNullOrWhiteSpace(attributeValue) == false && budgetkontoplan.Select(m => m.Kontonummer).Contains(attributeValue))
                            {
                                bogføringslinjeModel.Budgetkontonummer = attributeValue;
                            }

                            attributeValue = bogføringslinjeElement.GetNonRequiredAttributeValue("adressekonto", string.Empty);
                            if (string.IsNullOrWhiteSpace(attributeValue) == false && adressekontoliste.Select(m => m.Nummer).Contains(int.Parse(attributeValue, NumberStyles.Integer, CultureInfo.InvariantCulture)))
                            {
                                bogføringslinjeModel.Adressekonto = int.Parse(attributeValue, NumberStyles.Integer, CultureInfo.InvariantCulture);
                            }

                            bogføringslinjer.Add(bogføringslinjeModel);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                        }
                    }

                    return bogføringslinjer
                        .Where(m => m.Dato.Date <= statusDato.Date)
                        .OrderByDescending(m => m.Dato)
                        .ThenByDescending(m => m.Løbenummer)
                        .Take(antalBogføringslinjer)
                        .ToList();
                }
                catch (IntranetGuiRepositoryException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "BogføringslinjerGetAsync", ex.Message), ex);
                }
            });
        }

        /// <summary>
        /// Danner og returnerer en ny bogføringslinje, der efterfølgende kan bogføres.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, som den nye bogføringslinje skal være tilknyttet.</param>
        /// <param name="dato">Bogføringsdato, som den nye bogføringslinje skal initieres med.</param>
        /// <param name="kontonummer">Kontonummer, som den nye bogføringslinje skal initieres med.</param>
        /// <returns>Ny bogføringslinje, der efterfølgende kan bogføres.</returns>
        public virtual Task<IBogføringslinjeModel> BogføringslinjeCreateNewAsync(int regnskabsnummer, DateTime dato, string kontonummer)
        {
            return Task.Run(() =>
            {
                try
                {
                    return BogføringslinjeModel.CreateNew(regnskabsnummer, dato, kontonummer);
                }
                catch (ArgumentNullException)
                {
                    throw;
                }
                catch (ArgumentException)
                {
                    throw;
                }
                catch (IntranetGuiRepositoryException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "BogføringslinjeCreateNewAsync", ex.Message), ex);
                }
            });
        }

        /// <summary>
        /// Bogfører én til flere bogføringslinjer.
        /// </summary>
        /// <param name="bogføringslinjer">De bogføringslinjer, der skal bogføres.</param>
        /// <returns>Bogføringsresultater for de enkelte bogførte bogføringslinjer.</returns>
        public virtual Task<IEnumerable<IBogføringsresultatModel>> BogførAsync(params IBogføringslinjeModel[] bogføringslinjer)
        {
            if (bogføringslinjer == null)
            {
                throw new ArgumentNullException(nameof(bogføringslinjer));
            }

            if (bogføringslinjer.Any(m => m.Regnskabsnummer <= 0))
            {
                throw new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "Regnskabsnummer", bogføringslinjer.First(m => m.Regnskabsnummer <= 0).Regnskabsnummer), nameof(bogføringslinjer));
            }

            if (bogføringslinjer.Any(m => m.Dato > DateTime.Now))
            {
                throw new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "Dato", bogføringslinjer.First(m => m.Dato > DateTime.Now).Dato), nameof(bogføringslinjer));
            }

            if (bogføringslinjer.Any(m => string.IsNullOrWhiteSpace(m.Kontonummer)))
            {
                throw new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "Kontonummer", bogføringslinjer.First(m => string.IsNullOrWhiteSpace(m.Kontonummer)).Kontonummer), nameof(bogføringslinjer));
            }

            if (bogføringslinjer.Any(m => string.IsNullOrWhiteSpace(m.Tekst)))
            {
                throw new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "Tekst", bogføringslinjer.First(m => string.IsNullOrWhiteSpace(m.Tekst)).Tekst), nameof(bogføringslinjer));
            }

            if (bogføringslinjer.Any(m => m.Debit < 0M))
            {
                throw new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "Debit", bogføringslinjer.First(m => m.Debit < 0M).Debit), nameof(bogføringslinjer));
            }

            if (bogføringslinjer.Any(m => m.Kredit < 0M))
            {
                throw new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "Kredit", bogføringslinjer.First(m => m.Kredit < 0M).Kredit), nameof(bogføringslinjer));
            }

            return Task.Run<IEnumerable<IBogføringsresultatModel>>(() =>
            {
                try
                {
                    lock (SyncRoot)
                    {
                        IList<IBogføringsresultatModel> bogføringsresultater = new List<IBogføringsresultatModel>();
                        foreach (IBogføringslinjeModel bogføringslinje in bogføringslinjer)
                        {
                            IKontoModel kontoModel = KontoGetAsync(bogføringslinje.Regnskabsnummer, bogføringslinje.Kontonummer, bogføringslinje.Dato)
                                .GetAwaiter()
                                .GetResult();
                            IBudgetkontoModel budgetkontoModel = string.IsNullOrWhiteSpace(bogføringslinje.Budgetkontonummer)
                                ? null
                                : BudgetkontoGetAsync(bogføringslinje.Regnskabsnummer, bogføringslinje.Budgetkontonummer, bogføringslinje.Dato)
                                    .GetAwaiter()
                                    .GetResult();
                            IAdressekontoModel adressekontoModel = bogføringslinje.Adressekonto == 0
                                ? null
                                : AdressekontoGetAsync(bogføringslinje.Regnskabsnummer, bogføringslinje.Adressekonto, bogføringslinje.Dato)
                                    .GetAwaiter()
                                    .GetResult();

                            XDocument localeDataDocument = _localeDataStorage.GetLocaleData();
                            XElement regnskabElement = localeDataDocument.GetRegnskabElement(bogføringslinje.Regnskabsnummer);
                            if (regnskabElement == null)
                            {
                                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.AccountingNotFound, kontoModel.Regnskabsnummer));
                            }

                            int nextLøbenummer = 1;
                            foreach (XElement bogføringslinjeElement in regnskabElement.Elements(XName.Get("Bogfoeringslinje", Namespace)))
                            {
                                int value = int.Parse(bogføringslinjeElement.GetRequiredAttributeValue("loebenummer", string.Empty), NumberStyles.Integer, CultureInfo.InvariantCulture);
                                if (value >= nextLøbenummer)
                                {
                                    nextLøbenummer = value + 1;
                                }
                            }

                            IBogføringslinjeModel storedBogføringslinje = new BogføringslinjeModel(kontoModel.Regnskabsnummer, nextLøbenummer, bogføringslinje.Dato, kontoModel.Kontonummer, bogføringslinje.Tekst, bogføringslinje.Debit, bogføringslinje.Kredit)
                            {
                                Bilag = string.IsNullOrWhiteSpace(bogføringslinje.Bilag)
                                    ? null
                                    : bogføringslinje.Bilag,
                                Budgetkontonummer = budgetkontoModel?.Kontonummer,
                                Adressekonto = adressekontoModel?.Nummer ?? 0
                            };
                            storedBogføringslinje.StoreInDocument(localeDataDocument, false);

                            kontoModel.Saldo += storedBogføringslinje.Bogført;
                            kontoModel.StoreInDocument(localeDataDocument);
                            foreach (XElement kontoHistorikElement in localeDataDocument.GetHistorikElements(kontoModel).Where(m => string.CompareOrdinal(m.GetRequiredAttributeValue("dato", string.Empty), ModelExtensions.GetHistorikDato(storedBogføringslinje.Dato)) > 0))
                            {
                                decimal newSaldo = decimal.Parse(kontoHistorikElement.GetRequiredAttributeValue("saldo", string.Empty), NumberStyles.Any, CultureInfo.InvariantCulture) + storedBogføringslinje.Bogført;
                                kontoHistorikElement.UpdateAttribute(XName.Get("saldo", string.Empty), newSaldo.ToString("#.00", CultureInfo.InvariantCulture));
                            }

                            if (budgetkontoModel != null)
                            {
                                budgetkontoModel.Bogført += storedBogføringslinje.Bogført;
                                budgetkontoModel.StoreInDocument(localeDataDocument);
                                foreach (XElement budgetkontoHistorikElement in localeDataDocument.GetHistorikElements(budgetkontoModel).Where(m => string.CompareOrdinal(m.GetRequiredAttributeValue("dato", string.Empty), ModelExtensions.GetHistorikDato(storedBogføringslinje.Dato)) > 0))
                                {
                                    if (string.CompareOrdinal(budgetkontoHistorikElement.GetRequiredAttributeValue("dato", string.Empty), ModelExtensions.GetHistorikDato(new DateTime(storedBogføringslinje.Dato.Year, storedBogføringslinje.Dato.Month, DateTime.DaysInMonth(storedBogføringslinje.Dato.Year, storedBogføringslinje.Dato.Month)))) > 0)
                                    {
                                        continue;
                                    }

                                    decimal newBogført = decimal.Parse(budgetkontoHistorikElement.GetRequiredAttributeValue("bogfoert", string.Empty), NumberStyles.Any, CultureInfo.InvariantCulture) + storedBogføringslinje.Bogført;
                                    budgetkontoHistorikElement.UpdateAttribute(XName.Get("bogfoert", string.Empty), newBogført.ToString("#.00", CultureInfo.InvariantCulture));
                                }
                            }

                            if (adressekontoModel != null)
                            {
                                adressekontoModel.Saldo += storedBogføringslinje.Bogført;
                                adressekontoModel.StoreInDocument(localeDataDocument);
                                foreach (XElement adressekontoHistorikElement in localeDataDocument.GetHistorikElements(adressekontoModel).Where(m => string.CompareOrdinal(m.GetRequiredAttributeValue("dato", string.Empty), ModelExtensions.GetHistorikDato(storedBogføringslinje.Dato)) > 0))
                                {
                                    decimal newSaldo = decimal.Parse(adressekontoHistorikElement.GetRequiredAttributeValue("saldo", string.Empty), NumberStyles.Any, CultureInfo.InvariantCulture) + storedBogføringslinje.Bogført;
                                    adressekontoHistorikElement.UpdateAttribute(XName.Get("saldo", string.Empty), newSaldo.ToString("#.00", CultureInfo.InvariantCulture));
                                }
                            }

                            _localeDataStorage.StoreLocaleDocument(localeDataDocument);

                            bogføringsresultater.Add(new BogføringsresultatModel(storedBogføringslinje, Array.Empty<IBogføringsadvarselModel>()));
                        }
                        return bogføringsresultater;
                    }
                }
                catch (IntranetGuiRepositoryException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "BogførAsync", ex.Message), ex);
                }
            });
        }

        /// <summary>
        /// Henter listen af debitorer til et regnskab.
        /// </summary>
        /// <param name="regnskabsummer">Regnskabsnummer, hvortil listen af debitorer skal hentes.</param>
        /// <param name="statusDato">Statusdato for listen af debitorer.</param>
        /// <returns>Debitorer i regnskabet.</returns>
        public virtual Task<IEnumerable<IAdressekontoModel>> DebitorlisteGetAsync(int regnskabsummer, DateTime statusDato)
        {
            return Task.Run<IEnumerable<IAdressekontoModel>>(async () =>
            {
                try
                {
                    return (await AdressekontolisteGetAsync(regnskabsummer, statusDato))
                        .Where(m => m.Saldo > 0M)
                        .OrderBy(m => m.Nummer)
                        .ToList();
                }
                catch (IntranetGuiRepositoryException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "DebitorlisteGetAsync", ex.Message), ex);
                }
            });
        }

        /// <summary>
        /// Henter listen af kreditorer til et regnskab.
        /// </summary>
        /// <param name="regnskabsummer">Regnskabsnummer, hvortil listen af kreditorer skal hentes.</param>
        /// <param name="statusDato">Statusdato for listen af kreditorer.</param>
        /// <returns>Kreditorer i regnskabet.</returns>
        public virtual Task<IEnumerable<IAdressekontoModel>> KreditorlisteGetAsync(int regnskabsummer, DateTime statusDato)
        {
            return Task.Run<IEnumerable<IAdressekontoModel>>(async () =>
            {
                try
                {
                    return (await AdressekontolisteGetAsync(regnskabsummer, statusDato))
                        .Where(m => m.Saldo < 0M)
                        .OrderBy(m => m.Nummer)
                        .ToList();
                }
                catch (IntranetGuiRepositoryException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "KreditorlisteGetAsync", ex.Message), ex);
                }
            });
        }

        /// <summary>
        /// Henter listen af adressekonti til en regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, hvortil listen af adressekonti skal hentes.</param>
        /// <param name="statusDato">Statusdato for listen af adressekonti.</param>
        /// <returns>Adressekonti til regnskabet.</returns>
        public virtual Task<IEnumerable<IAdressekontoModel>> AdressekontolisteGetAsync(int regnskabsnummer, DateTime statusDato)
        {
            return Task.Run<IEnumerable<IAdressekontoModel>>(() =>
            {
                try
                {
                    XDocument localeDataDocument = _localeDataStorage.GetLocaleData();
                    XElement regnskabElement = localeDataDocument.GetRegnskabElement(regnskabsnummer);
                    if (regnskabElement == null)
                    {
                        return new List<IAdressekontoModel>(0);
                    }

                    IList<IAdressekontoModel> adressekonti = new List<IAdressekontoModel>();
                    foreach (XElement adressekontoElement in regnskabElement.Elements(XName.Get("Adressekonto", Namespace)))
                    {
                        try
                        {
                            IAdressekontoModel adressekontoModel = new AdressekontoModel(
                                int.Parse(regnskabElement.GetRequiredAttributeValue("nummer", string.Empty), NumberStyles.Integer, CultureInfo.InvariantCulture),
                                int.Parse(adressekontoElement.GetRequiredAttributeValue("nummer", string.Empty), NumberStyles.Integer, CultureInfo.InvariantCulture),
                                adressekontoElement.GetRequiredAttributeValue("navn", string.Empty),
                                statusDato,
                                0M);
                            adressekontoModel.PrimærTelefon = adressekontoElement.GetNonRequiredAttributeValue("primaerTelefon", string.Empty);
                            adressekontoModel.SekundærTelefon = adressekontoElement.GetNonRequiredAttributeValue("sekundaerTelefon", string.Empty);

                            XElement historikElement = localeDataDocument.GetHistorikElements(adressekontoModel).FirstOrDefault(m => string.CompareOrdinal(m.GetRequiredAttributeValue("dato", string.Empty), ModelExtensions.GetHistorikDato(statusDato)) <= 0);
                            if (historikElement != null)
                            {
                                adressekontoModel.Saldo = decimal.Parse(historikElement.GetRequiredAttributeValue("saldo", string.Empty), NumberStyles.Any, CultureInfo.InvariantCulture);
                            }

                            adressekonti.Add(adressekontoModel);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                        }
                    }

                    return adressekonti.OrderBy(m => m.Nummer).ToList();
                }
                catch (IntranetGuiRepositoryException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "AdressekontolisteGetAsync", ex.Message), ex);
                }
            });
        }

        /// <summary>
        /// Henter en given adressekonto til et regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, hvortil adressekontoen skal hentes.</param>
        /// <param name="nummer">Unik identifikation af adressekontoen.</param>
        /// <param name="statusDato">Statusdato, hvorpå adressekontoen skal hentes.</param>
        /// <returns>Adressekonto.</returns>
        public virtual Task<IAdressekontoModel> AdressekontoGetAsync(int regnskabsnummer, int nummer, DateTime statusDato)
        {
            return Task.Run(async () =>
            {
                try
                {
                    IEnumerable<IAdressekontoModel> adressekontoliste = await AdressekontolisteGetAsync(regnskabsnummer, statusDato);
                    try
                    {
                        return adressekontoliste.Single(m => m.Nummer == nummer);
                    }
                    catch (InvalidOperationException ex)
                    {
                        throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.AddressAccountNotFound, nummer), ex);
                    }
                }
                catch (IntranetGuiRepositoryException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "AdressekontoGetAsync", ex.Message), ex);
                }
            });
        }

        /// <summary>
        /// Henter listen af kontogrupper.
        /// </summary>
        /// <returns>Kontogrupper.</returns>
        public virtual Task<IEnumerable<IKontogruppeModel>> KontogruppelisteGetAsync()
        {
            return Task.Run<IEnumerable<IKontogruppeModel>>(() =>
            {
                try
                {
                    XDocument localeDataDocument = _localeDataStorage.GetLocaleData();
                    if (localeDataDocument.Root == null)
                    {
                        return new List<IKontogruppeModel>(0);
                    }

                    IList<IKontogruppeModel> kontogrupper = new List<IKontogruppeModel>();
                    foreach (XElement kontogruppeElement in localeDataDocument.Root.Elements(XName.Get("Kontogruppe", Namespace)))
                    {
                        try
                        {
                            IKontogruppeModel kontogruppe = new KontogruppeModel(
                                int.Parse(kontogruppeElement.GetRequiredAttributeValue("nummer", string.Empty), NumberStyles.Integer, CultureInfo.InvariantCulture),
                                kontogruppeElement.GetRequiredAttributeValue("tekst", string.Empty),
                                (Balancetype)Enum.Parse(typeof(Balancetype), kontogruppeElement.GetRequiredAttributeValue("balanceType", string.Empty)));
                            kontogrupper.Add(kontogruppe);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                        }
                    }

                    return kontogrupper.OrderBy(m => m.Nummer).ToList();
                }
                catch (IntranetGuiRepositoryException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "KontogruppelisteGetAsync", ex.Message), ex);
                }
            });
        }

        /// <summary>
        /// Henter listen af grupper til budgetkonti.
        /// </summary>
        /// <returns>Grupper til budgetkonti.</returns>
        public virtual Task<IEnumerable<IBudgetkontogruppeModel>> BudgetkontogruppelisteGetAsync()
        {
            return Task.Run<IEnumerable<IBudgetkontogruppeModel>>(() =>
            {
                try
                {
                    XDocument localeDataDocument = _localeDataStorage.GetLocaleData();
                    if (localeDataDocument.Root == null)
                    {
                        return new List<IBudgetkontogruppeModel>(0);
                    }

                    IList<IBudgetkontogruppeModel> budgetkontogrupper = new List<IBudgetkontogruppeModel>();
                    foreach (XElement budgetkontogruppeElement in localeDataDocument.Root.Elements(XName.Get("Budgetkontogruppe", Namespace)))
                    {
                        try
                        {
                            IBudgetkontogruppeModel budgetkontogruppe = new BudgetkontogruppeModel(
                                int.Parse(budgetkontogruppeElement.GetRequiredAttributeValue("nummer", string.Empty), NumberStyles.Integer, CultureInfo.InvariantCulture),
                                budgetkontogruppeElement.GetRequiredAttributeValue("tekst", string.Empty));
                            budgetkontogrupper.Add(budgetkontogruppe);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                        }
                    }

                    return budgetkontogrupper.OrderBy(m => m.Nummer).ToList();
                }
                catch (IntranetGuiRepositoryException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "BudgetkontogruppelisteGet", ex.Message), ex);
                }
            });
        }

        /// <summary>
        /// Eventhandler, der rejses, når data i det lokale datalager skal forberedes til læsning eller skrivning.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        private static void PrepareLocaleDataEventHandler(object sender, IPrepareLocaleDataEventArgs eventArgs)
        {
            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            if (eventArgs == null)
            {
                throw new ArgumentNullException(nameof(eventArgs));
            }

            if (eventArgs.ReadingContext == false && eventArgs.WritingContext == false)
            {
                return;
            }

            eventArgs.LocaleDataDocument.StoreVersionNumberInDocument(RepositoryVersion);
        }

        #endregion
    }
}