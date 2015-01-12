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

        private readonly IFinansstyringKonfigurationRepository _finansstyringKonfigurationRepository;
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
                throw new ArgumentNullException("finansstyringKonfigurationRepository");
            }
            if (localeDataStorage == null)
            {
                throw new ArgumentNullException("localeDataStorage");
            }
            _finansstyringKonfigurationRepository = finansstyringKonfigurationRepository;
            _localeDataStorage = localeDataStorage;
            lock (SyncRoot)
            {
                _localeDataStorage.PrepareLocaleData += PrepareLocaleDataEventHandler;
                if (_localeDataStorage.HasLocaleData)
                {
                    return;
                }
                var currentDate = DateTime.Today;
                _localeDataStorage.StoreLocaleData((IRegnskabModel) new RegnskabModel(1, Resource.GetText(Text.PrivateAccounting)));
                _localeDataStorage.StoreLocaleData((IKontogruppeModel) new KontogruppeModel(1, Resource.GetText(Text.Cash), Balancetype.Aktiver));
                _localeDataStorage.StoreLocaleData((IKontogruppeModel) new KontogruppeModel(2, Resource.GetText(Text.BankAccounts), Balancetype.Aktiver));
                _localeDataStorage.StoreLocaleData((IKontogruppeModel) new KontogruppeModel(3, Resource.GetText(Text.Loans), Balancetype.Passiver));
                _localeDataStorage.StoreLocaleData((IBudgetkontogruppeModel) new BudgetkontogruppeModel(1, Resource.GetText(Text.Income)));
                _localeDataStorage.StoreLocaleData((IBudgetkontogruppeModel) new BudgetkontogruppeModel(2, Resource.GetText(Text.Expenses)));
                _localeDataStorage.StoreLocaleData((IKontoModel) new KontoModel(1, "10110", Resource.GetText(Text.CachAccount, "10110"), 1, currentDate));
                _localeDataStorage.StoreLocaleData((IKontoModel) new KontoModel(1, "10120", Resource.GetText(Text.CachAccount, "10120"), 1, currentDate));
                _localeDataStorage.StoreLocaleData((IKontoModel) new KontoModel(1, "10130", Resource.GetText(Text.CachAccount, "10130"), 1, currentDate));
                _localeDataStorage.StoreLocaleData((IKontoModel) new KontoModel(1, "10210", Resource.GetText(Text.BankAccount, "10210"), 2, currentDate));
                _localeDataStorage.StoreLocaleData((IKontoModel) new KontoModel(1, "10220", Resource.GetText(Text.BankAccount, "10220"), 2, currentDate));
                _localeDataStorage.StoreLocaleData((IKontoModel) new KontoModel(1, "10230", Resource.GetText(Text.BankAccount, "10230"), 2, currentDate));
                _localeDataStorage.StoreLocaleData((IKontoModel) new KontoModel(1, "10240", Resource.GetText(Text.BankAccount, "10240"), 2, currentDate));
                _localeDataStorage.StoreLocaleData((IKontoModel) new KontoModel(1, "10250", Resource.GetText(Text.BankAccount, "10250"), 2, currentDate));
                _localeDataStorage.StoreLocaleData((IKontoModel) new KontoModel(1, "20110", Resource.GetText(Text.LoanAccount, "20110"), 3, currentDate));
                _localeDataStorage.StoreLocaleData((IKontoModel) new KontoModel(1, "20120", Resource.GetText(Text.LoanAccount, "20120"), 3, currentDate));
                _localeDataStorage.StoreLocaleData((IKontoModel) new KontoModel(1, "20130", Resource.GetText(Text.LoanAccount, "20130"), 3, currentDate));
                _localeDataStorage.StoreLocaleData((IKontoModel) new KontoModel(1, "20140", Resource.GetText(Text.LoanAccount, "20140"), 3, currentDate));
                _localeDataStorage.StoreLocaleData((IKontoModel) new KontoModel(1, "20150", Resource.GetText(Text.LoanAccount, "20150"), 3, currentDate));
                _localeDataStorage.StoreLocaleData((IBudgetkontoModel) new BudgetkontoModel(1, "1010", Resource.GetText(Text.Wages), 1, currentDate));
                _localeDataStorage.StoreLocaleData((IBudgetkontoModel) new BudgetkontoModel(1, "1020", Resource.GetText(Text.Other), 1, currentDate));
                _localeDataStorage.StoreLocaleData((IBudgetkontoModel) new BudgetkontoModel(1, "2010", Resource.GetText(Text.Household), 2, currentDate));
                _localeDataStorage.StoreLocaleData((IBudgetkontoModel) new BudgetkontoModel(1, "2020", Resource.GetText(Text.HousingCost), 2, currentDate));
                _localeDataStorage.StoreLocaleData((IBudgetkontoModel) new BudgetkontoModel(1, "2030", Resource.GetText(Text.Insurances), 2, currentDate));
                _localeDataStorage.StoreLocaleData((IBudgetkontoModel) new BudgetkontoModel(1, "2040", Resource.GetText(Text.Vehicles), 2, currentDate));
                _localeDataStorage.StoreLocaleData((IBudgetkontoModel) new BudgetkontoModel(1, "2050", Resource.GetText(Text.Vacations), 2, currentDate));
                _localeDataStorage.StoreLocaleData((IBudgetkontoModel) new BudgetkontoModel(1, "2060", Resource.GetText(Text.Experiences), 2, currentDate));
                _localeDataStorage.StoreLocaleData((IBudgetkontoModel) new BudgetkontoModel(1, "2070", Resource.GetText(Text.Other), 2, currentDate));
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returnerer konfigurationsrepositoryet, der supporterer finansstyring.
        /// </summary>
        public virtual IFinansstyringKonfigurationRepository Konfiguration
        {
            get
            {
                return _finansstyringKonfigurationRepository;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Henter en liste af regnskaber.
        /// </summary>
        /// <returns>Liste af regnskaber.</returns>
        public virtual Task<IEnumerable<IRegnskabModel>> RegnskabslisteGetAsync()
        {
            Func<IEnumerable<IRegnskabModel>> func = RegnskabslisteGet;
            return Task.Run(func);
        }

        /// <summary>
        /// Henter kontoplanen til et givent regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, hvortil kontoplanen skal hentes.</param>
        /// <param name="statusDato">Statusdato for kontoplanen.</param>
        /// <returns>Kontoplan.</returns>
        public virtual Task<IEnumerable<IKontoModel>> KontoplanGetAsync(int regnskabsnummer, DateTime statusDato)
        {
            Func<IEnumerable<IKontoModel>> func = () => KontoplanGet(regnskabsnummer, statusDato);
            return Task.Run(func);
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
            if (string.IsNullOrEmpty(kontonummer))
            {
                throw new ArgumentNullException("kontonummer");
            }
            Func<IKontoModel> func = () => KontoGet(regnskabsnummer, kontonummer, statusDato);
            return Task.Run(func);
        }

        /// <summary>
        /// Henter budgetkontoplanen til et givent regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, hvortil budgetkontoplanen skal hentes.</param>
        /// <param name="statusDato">Statusdato for budgetkontoplanen.</param>
        /// <returns>Budgetkontoplan.</returns>
        public virtual Task<IEnumerable<IBudgetkontoModel>> BudgetkontoplanGetAsync(int regnskabsnummer, DateTime statusDato)
        {
            Func<IEnumerable<IBudgetkontoModel>> func = () => BudgetkontoplanGet(regnskabsnummer, statusDato);
            return Task.Run(func);
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
            if (string.IsNullOrEmpty(budgetkontonummer))
            {
                throw new ArgumentNullException("budgetkontonummer");
            }
            Func<IBudgetkontoModel> func = () => BudgetkontoGet(regnskabsnummer, budgetkontonummer, statusDato);
            return Task.Run(func);
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
            Func<IEnumerable<IBogføringslinjeModel>> func = () => BogføringslinjerGet(regnskabsnummer, statusDato, antalBogføringslinjer);
            return Task.Run(func);
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
            Func<IBogføringslinjeModel> func = () =>
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
            };
            return Task.Run(func);
        }

        /// <summary>
        /// Bogfører værdier i et givent regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, hvor værdier skal bogføres.</param>
        /// <param name="dato">Bogføringsdato.</param>
        /// <param name="bilag">Bilagsnummer.</param>
        /// <param name="kontonummer">Kontonummer, hvorpå værdier skal bogføres.</param>
        /// <param name="tekst">Tekst.</param>
        /// <param name="budgetkontonummer">Budgetkontonummer, hvorpå værdier skal bogføres.</param>
        /// <param name="debit">Debitbeløb.</param>
        /// <param name="kredit">Kreditbeløb.</param>
        /// <param name="adressekonto">Unik identifikation af adressekontoen, hvorpå værdier skal bogføres.</param>
        /// <returns>Bogføringsresultat.</returns>
        public virtual Task<IBogføringsresultatModel> BogførAsync(int regnskabsnummer, DateTime dato, string bilag, string kontonummer, string tekst, string budgetkontonummer, decimal debit, decimal kredit, int adressekonto)
        {
            if (regnskabsnummer <= 0)
            {
                throw new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "regnskabsnummer", regnskabsnummer), "regnskabsnummer");
            }
            if (dato > DateTime.Now)
            {
                throw new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "dato", dato), "dato");
            }
            if (string.IsNullOrEmpty(kontonummer))
            {
                throw new ArgumentNullException("kontonummer");
            }
            if (string.IsNullOrEmpty(tekst))
            {
                throw new ArgumentNullException("tekst");
            }
            if (debit < 0M)
            {
                throw new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "debit", debit), "debit");
            }
            if (kredit < 0M)
            {
                throw new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "kredit", kredit), "kredit");
            }
            Func<IBogføringsresultatModel> func = () => Bogfør(regnskabsnummer, dato, bilag, kontonummer, tekst, budgetkontonummer, debit, kredit, adressekonto);
            return Task.Run(func);
        }

        /// <summary>
        /// Henter listen af debitorer til et regnskab.
        /// </summary>
        /// <param name="regnskabsummer">Regnskabsnummer, hvortil listen af debitorer skal hentes.</param>
        /// <param name="statusDato">Statusdato for listen af debitorer.</param>
        /// <returns>Debitorer i regnskabet.</returns>
        public virtual Task<IEnumerable<IAdressekontoModel>> DebitorlisteGetAsync(int regnskabsummer, DateTime statusDato)
        {
            Func<IEnumerable<IAdressekontoModel>> func = () => DebitorlisteGet(regnskabsummer, statusDato);
            return Task.Run(func);
        }

        /// <summary>
        /// Henter listen af kreditorer til et regnskab.
        /// </summary>
        /// <param name="regnskabsummer">Regnskabsnummer, hvortil listen af kreditorer skal hentes.</param>
        /// <param name="statusDato">Statusdato for listen af kreditorer.</param>
        /// <returns>Kreditorer i regnskabet.</returns>
        public virtual Task<IEnumerable<IAdressekontoModel>> KreditorlisteGetAsync(int regnskabsummer, DateTime statusDato)
        {
            Func<IEnumerable<IAdressekontoModel>> func = () => KreditorlisteGet(regnskabsummer, statusDato);
            return Task.Run(func);
        }

        /// <summary>
        /// Henter listen af adressekonti til en regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, hvortil listen af adressekonti skal hentes.</param>
        /// <param name="statusDato">Statusdato for listen af adressekonti.</param>
        /// <returns>Adressekonti til regnskabet.</returns>
        public virtual Task<IEnumerable<IAdressekontoModel>> AdressekontolisteGetAsync(int regnskabsnummer, DateTime statusDato)
        {
            Func<IEnumerable<IAdressekontoModel>> func = () => AdressekontolisteGet(regnskabsnummer, statusDato);
            return Task.Run(func);
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
            Func<IAdressekontoModel> func = () => AdressekontoGet(regnskabsnummer, nummer, statusDato);
            return Task.Run(func);
        }

        /// <summary>
        /// Henter listen af kontogrupper.
        /// </summary>
        /// <returns>Kontogrupper.</returns>
        public virtual Task<IEnumerable<IKontogruppeModel>> KontogruppelisteGetAsync()
        {
            Func<IEnumerable<IKontogruppeModel>> func = KontogruppelisteGet;
            return Task.Run(func);
        }

        /// <summary>
        /// Henter listen af grupper til budgetkonti.
        /// </summary>
        /// <returns>Grupper til budgetkonti.</returns>
        public virtual Task<IEnumerable<IBudgetkontogruppeModel>> BudgetkontogruppelisteGetAsync()
        {
            Func<IEnumerable<IBudgetkontogruppeModel>> func = BudgetkontogruppelisteGet;
            return Task.Run(func);
        }

        /// <summary>
        /// Henter en liste af regnskaber.
        /// </summary>
        /// <returns>Liste af regnskaber.</returns>
        private IEnumerable<IRegnskabModel> RegnskabslisteGet()
        {
            try
            {
                var regnskaber = new List<IRegnskabModel>();
                var localeDataDocument = _localeDataStorage.GetLocaleData();
                foreach (var regnskabElement in localeDataDocument.Root.Elements(XName.Get("Regnskab", Namespace)))
                {
                    try
                    {
                        var regnskabModel = new RegnskabModel(int.Parse(regnskabElement.Attribute(XName.Get("nummer", string.Empty)).Value, NumberStyles.Integer, CultureInfo.InvariantCulture), regnskabElement.Attribute(XName.Get("navn", string.Empty)).Value);
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
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "RegnskabslisteGet", ex.Message), ex);
            }
        }

        /// <summary>
        /// Henter kontoplanen til et givent regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, hvortil kontoplanen skal hentes.</param>
        /// <param name="statusDato">Statusdato for kontoplanen.</param>
        /// <returns>Kontoplan.</returns>
        private IEnumerable<IKontoModel> KontoplanGet(int regnskabsnummer, DateTime statusDato)
        {
            try
            {
                var localeDataDocument = _localeDataStorage.GetLocaleData();
                var regnskabElement = localeDataDocument.GetRegnskabElement(regnskabsnummer);
                if (regnskabElement == null)
                {
                    return new List<IKontoModel>(0);
                }
                var kontogrupper = KontogruppelisteGet().ToList();
                var kontoplan = new List<IKontoModel>();
                foreach (var kontoElement in regnskabElement.Elements(XName.Get("Konto", Namespace)))
                {
                    try
                    {
                        var kontogruppeAttribute = kontoElement.Attribute(XName.Get("kontogruppe", string.Empty));
                        if (kontogruppeAttribute == null || kontogrupper.Select(m => m.Nummer).Contains(int.Parse(kontogruppeAttribute.Value, NumberStyles.Integer, CultureInfo.InvariantCulture)) == false) 
                        {
                            continue;
                        }
                        var kontoModel = new KontoModel(int.Parse(regnskabElement.Attribute(XName.Get("nummer", string.Empty)).Value, NumberStyles.Integer, CultureInfo.InvariantCulture), kontoElement.Attribute(XName.Get("kontonummer", string.Empty)).Value, kontoElement.Attribute(XName.Get("kontonavn")).Value, int.Parse(kontogruppeAttribute.Value, NumberStyles.Integer, CultureInfo.InvariantCulture), statusDato);
                        if (kontoElement.Attribute(XName.Get("beskrivelse", string.Empty)) != null)
                        {
                            kontoModel.Beskrivelse = kontoElement.Attribute(XName.Get("beskrivelse", string.Empty)).Value;
                        }
                        if (kontoElement.Attribute(XName.Get("note", string.Empty)) != null)
                        {
                            kontoModel.Notat = kontoElement.Attribute(XName.Get("note", string.Empty)).Value;
                        }
                        var historikElement = localeDataDocument.GetHistorikElements(kontoModel).FirstOrDefault();
                        if (historikElement != null)
                        {
                            kontoModel.Kredit = decimal.Parse(historikElement.Attribute(XName.Get("kredit", string.Empty)).Value, NumberStyles.Any, CultureInfo.InvariantCulture);
                            kontoModel.Saldo = decimal.Parse(historikElement.Attribute(XName.Get("saldo", string.Empty)).Value, NumberStyles.Any, CultureInfo.InvariantCulture);
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
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "KontoplanGet", ex.Message), ex);
            }
        }

        /// <summary>
        /// Henter en given konto i et givent regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, hvorfra kontoen skal hentes.</param>
        /// <param name="kontonummer">Kontonummer på kontoen, der skal hentes.</param>
        /// <param name="statusDato">Statusdato for kontoen.</param>
        /// <returns>Konto.</returns>
        private IKontoModel KontoGet(int regnskabsnummer, string kontonummer, DateTime statusDato)
        {
            try
            {
                var kontoplan = KontoplanGet(regnskabsnummer, statusDato).ToList();
                try
                {
                    return kontoplan.Single(m => string.Compare(m.Kontonummer, kontonummer, StringComparison.Ordinal) == 0);
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
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "KontoGet", ex.Message), ex);
            }
        }

        /// <summary>
        /// Henter budgetkontoplanen til et givent regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, hvortil budgetkontoplanen skal hentes.</param>
        /// <param name="statusDato">Statusdato for budgetkontoplanen.</param>
        /// <returns>Budgetkontoplan.</returns>
        private IEnumerable<IBudgetkontoModel> BudgetkontoplanGet(int regnskabsnummer, DateTime statusDato)
        {
            try
            {
                var localeDataDocument = _localeDataStorage.GetLocaleData();
                var regnskabElement = localeDataDocument.GetRegnskabElement(regnskabsnummer);
                if (regnskabElement == null)
                {
                    return new List<IBudgetkontoModel>(0);
                }
                var budgetkontogrupper = BudgetkontogruppelisteGet().ToList();
                var budgetkontoplan = new List<IBudgetkontoModel>();
                foreach (var budgetkontoElement in regnskabElement.Elements(XName.Get("Budgetkonto", Namespace)))
                {
                    try
                    {
                        var kontogruppeAttribute = budgetkontoElement.Attribute(XName.Get("kontogruppe", string.Empty));
                        if (kontogruppeAttribute == null || budgetkontogrupper.Select(m => m.Nummer).Contains(int.Parse(kontogruppeAttribute.Value, NumberStyles.Integer, CultureInfo.InvariantCulture)) == false)
                        {
                            continue;
                        }
                        var budgetkontoModel = new BudgetkontoModel(int.Parse(regnskabElement.Attribute(XName.Get("nummer", string.Empty)).Value, NumberStyles.Integer, CultureInfo.InvariantCulture), budgetkontoElement.Attribute(XName.Get("kontonummer", string.Empty)).Value, budgetkontoElement.Attribute(XName.Get("kontonavn", string.Empty)).Value, int.Parse(kontogruppeAttribute.Value, NumberStyles.Integer, CultureInfo.InvariantCulture), statusDato);
                        if (budgetkontoElement.Attribute(XName.Get("beskrivelse", string.Empty)) != null)
                        {
                            budgetkontoModel.Beskrivelse = budgetkontoElement.Attribute(XName.Get("beskrivelse", string.Empty)).Value;
                        }
                        if (budgetkontoElement.Attribute(XName.Get("note", string.Empty)) != null)
                        {
                            budgetkontoModel.Notat = budgetkontoElement.Attribute(XName.Get("note", string.Empty)).Value;
                        }
                        var historikElementArray = localeDataDocument.GetHistorikElements(budgetkontoModel).ToArray();
                        var historikElementForStatusDato = historikElementArray.FirstOrDefault();
                        if (historikElementForStatusDato != null)
                        {
                            budgetkontoModel.Indtægter = decimal.Parse(historikElementForStatusDato.Attribute(XName.Get("indtaegter", string.Empty)).Value, NumberStyles.Any, CultureInfo.InvariantCulture);
                            budgetkontoModel.Udgifter = decimal.Parse(historikElementForStatusDato.Attribute(XName.Get("udgifter", string.Empty)).Value, NumberStyles.Any, CultureInfo.InvariantCulture);
                            budgetkontoModel.Bogført = decimal.Parse(historikElementForStatusDato.Attribute(XName.Get("bogfoert", string.Empty)).Value, NumberStyles.Any, CultureInfo.InvariantCulture);
                        }
                        var historikDato = new DateTime(budgetkontoModel.StatusDato.AddMonths(-1).Year, budgetkontoModel.StatusDato.AddMonths(-1).Month, DateTime.DaysInMonth(budgetkontoModel.StatusDato.AddMonths(-1).Year, budgetkontoModel.StatusDato.AddMonths(-1).Month));
                        var historikElementForDato = historikElementArray.FirstOrDefault(m => string.Compare(m.Attribute(XName.Get("dato", string.Empty)).Value, ModelExtentions.GetHistorikDato(historikDato), StringComparison.Ordinal) <= 0);
                        if (historikElementForDato != null)
                        {
                            budgetkontoModel.BudgetSidsteMåned += decimal.Parse(historikElementForDato.Attribute(XName.Get("indtaegter", string.Empty)).Value, NumberStyles.Any, CultureInfo.InvariantCulture);
                            budgetkontoModel.BudgetSidsteMåned -= decimal.Parse(historikElementForDato.Attribute(XName.Get("udgifter", string.Empty)).Value, NumberStyles.Any, CultureInfo.InvariantCulture);
                            budgetkontoModel.BogførtSidsteMåned = decimal.Parse(historikElementForDato.Attribute(XName.Get("bogfoert", string.Empty)).Value, NumberStyles.Any, CultureInfo.InvariantCulture);
                        }
                        budgetkontoModel.BudgetÅrTilDato = budgetkontoModel.Budget;
                        budgetkontoModel.BogførtÅrTilDato = budgetkontoModel.Bogført;
                        for (var month = budgetkontoModel.StatusDato.Month - 1; month > 0; month--)
                        {
                            historikDato = new DateTime(budgetkontoModel.StatusDato.Year, month, DateTime.DaysInMonth(budgetkontoModel.StatusDato.Year, month));
                            historikElementForDato = historikElementArray.FirstOrDefault(m => string.Compare(m.Attribute(XName.Get("dato", string.Empty)).Value, ModelExtentions.GetHistorikDato(historikDato), StringComparison.Ordinal) <= 0);
                            if (historikElementForDato != null)
                            {
                                budgetkontoModel.BudgetÅrTilDato += decimal.Parse(historikElementForDato.Attribute(XName.Get("indtaegter", string.Empty)).Value, NumberStyles.Any, CultureInfo.InvariantCulture);
                                budgetkontoModel.BudgetÅrTilDato -= decimal.Parse(historikElementForDato.Attribute(XName.Get("udgifter", string.Empty)).Value, NumberStyles.Any, CultureInfo.InvariantCulture);
                                budgetkontoModel.BogførtÅrTilDato += decimal.Parse(historikElementForDato.Attribute(XName.Get("bogfoert", string.Empty)).Value, NumberStyles.Any, CultureInfo.InvariantCulture);
                            }
                        }
                        budgetkontoModel.BudgetSidsteÅr = 0M;
                        budgetkontoModel.BogførtSidsteÅr = 0M;
                        for (var month = 12; month > 0; month--)
                        {
                            historikDato = new DateTime(budgetkontoModel.StatusDato.AddYears(-1).Year, month, DateTime.DaysInMonth(budgetkontoModel.StatusDato.AddYears(-1).Year, month));
                            historikElementForDato = historikElementArray.FirstOrDefault(m => string.Compare(m.Attribute(XName.Get("dato", string.Empty)).Value, ModelExtentions.GetHistorikDato(historikDato), StringComparison.Ordinal) <= 0);
                            if (historikElementForDato != null)
                            {
                                budgetkontoModel.BudgetSidsteÅr += decimal.Parse(historikElementForDato.Attribute(XName.Get("indtaegter", string.Empty)).Value, NumberStyles.Any, CultureInfo.InvariantCulture);
                                budgetkontoModel.BudgetSidsteÅr -= decimal.Parse(historikElementForDato.Attribute(XName.Get("udgifter", string.Empty)).Value, NumberStyles.Any, CultureInfo.InvariantCulture);
                                budgetkontoModel.BogførtSidsteÅr += decimal.Parse(historikElementForDato.Attribute(XName.Get("bogfoert", string.Empty)).Value, NumberStyles.Any, CultureInfo.InvariantCulture);
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
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "BudgetkontoplanGet", ex.Message), ex);
            }
        }

        /// <summary>
        /// Henter en given budgetkonto i et givent regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, hvorfra budgetkontoen skal hentes.</param>
        /// <param name="budgetkontonummer">Kontonummer på budgetkontoen, der skal hentes.</param>
        /// <param name="statusDato">Statusdato for budgetkontoen.</param>
        /// <returns>Budgetkonto.</returns>
        private IBudgetkontoModel BudgetkontoGet(int regnskabsnummer, string budgetkontonummer, DateTime statusDato)
        {
            try
            {
                var budgetkontoplan = BudgetkontoplanGet(regnskabsnummer, statusDato).ToList();
                try
                {
                    return budgetkontoplan.Single(m => string.Compare(m.Kontonummer, budgetkontonummer, StringComparison.Ordinal) == 0);
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
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "BudgetkontoGet", ex.Message), ex);
            }
        }

        /// <summary>
        /// Henter et givent antal bogføringslinjer til et regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, hvortil bogføringslinjer skal hentes.</param>
        /// <param name="statusDato">Dato, hvorfra bogføringslinjer skal hentes.</param>
        /// <param name="antalBogføringslinjer">Antal bogføringslinjer, der skal hentes.</param>
        /// <returns>Bogføringslinjer til regnskabet.</returns>
        private IEnumerable<IBogføringslinjeModel> BogføringslinjerGet(int regnskabsnummer, DateTime statusDato, int antalBogføringslinjer)
        {
            try
            {
                var localeDataDocument = _localeDataStorage.GetLocaleData();
                var regnskabElement = localeDataDocument.GetRegnskabElement(regnskabsnummer);
                if (regnskabElement == null)
                {
                    return new List<IBogføringslinjeModel>(0);
                }
                var kontoplan = KontoplanGet(regnskabsnummer, statusDato).ToList();
                var budgetkontoplan = BudgetkontoplanGet(regnskabsnummer, statusDato).ToList();
                var adressekontoliste = AdressekontolisteGet(regnskabsnummer, statusDato).ToList();
                var bogføringslinjer = new List<IBogføringslinjeModel>();
                foreach (var bogføringslinjeElement in regnskabElement.Elements(XName.Get("Bogfoeringslinje", Namespace)))
                {
                    try
                    {
                        var kontonummerAttribute = bogføringslinjeElement.Attribute(XName.Get("kontonummer", string.Empty));
                        if (kontonummerAttribute == null || kontoplan.Select(m => m.Kontonummer).Contains(kontonummerAttribute.Value) == false)
                        {
                            continue;
                        }
                        var debitAttribute = bogføringslinjeElement.Attribute(XName.Get("debit", string.Empty));
                        var kreditAttribute = bogføringslinjeElement.Attribute(XName.Get("kredit", string.Empty));
                        var bogføringslinjeModel = new BogføringslinjeModel(int.Parse(regnskabElement.Attribute(XName.Get("nummer", string.Empty)).Value, NumberStyles.Integer, CultureInfo.InvariantCulture), int.Parse(bogføringslinjeElement.Attribute(XName.Get("loebenummer", string.Empty)).Value, NumberStyles.Integer, CultureInfo.InvariantCulture), DateTime.ParseExact(bogføringslinjeElement.Attribute(XName.Get("dato", string.Empty)).Value, "yyyyMMdd", CultureInfo.InvariantCulture), kontonummerAttribute.Value, bogføringslinjeElement.Attribute(XName.Get("tekst", string.Empty)).Value, debitAttribute == null ? 0M : decimal.Parse(debitAttribute.Value, NumberStyles.Any, CultureInfo.InvariantCulture), kreditAttribute == null ? 0M : decimal.Parse(kreditAttribute.Value, NumberStyles.Any, CultureInfo.InvariantCulture));
                        if (bogføringslinjeElement.Attribute(XName.Get("bilag", string.Empty)) != null)
                        {
                            bogføringslinjeModel.Bilag = bogføringslinjeElement.Attribute(XName.Get("bilag", string.Empty)).Value;
                        }
                        if (bogføringslinjeElement.Attribute(XName.Get("budgetkontonummer", string.Empty)) != null && budgetkontoplan.Select(m => m.Kontonummer).Contains(bogføringslinjeElement.Attribute(XName.Get("budgetkontonummer", string.Empty)).Value))
                        {
                            bogføringslinjeModel.Budgetkontonummer = bogføringslinjeElement.Attribute(XName.Get("budgetkontonummer", string.Empty)).Value;
                        }
                        if (bogføringslinjeElement.Attribute(XName.Get("adressekonto", string.Empty)) != null && adressekontoliste.Select(m => m.Nummer).Contains(int.Parse(bogføringslinjeElement.Attribute(XName.Get("adressekonto", string.Empty)).Value, NumberStyles.Integer, CultureInfo.InvariantCulture)))
                        {
                            bogføringslinjeModel.Adressekonto = int.Parse(bogføringslinjeElement.Attribute(XName.Get("adressekonto", string.Empty)).Value, NumberStyles.Integer, CultureInfo.InvariantCulture);
                        }
                        bogføringslinjer.Add(bogføringslinjeModel);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
                return bogføringslinjer.Where(m => m.Dato.Date <= statusDato.Date).OrderByDescending(m => m.Dato).ThenByDescending(m => m.Løbenummer).Take(antalBogføringslinjer).ToList();
            }
            catch (IntranetGuiRepositoryException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "BogføringslinjerGet", ex.Message), ex);
            }
        }

        /// <summary>
        /// Bogfører værdier i et givent regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, hvor værdier skal bogføres.</param>
        /// <param name="dato">Bogføringsdato.</param>
        /// <param name="bilag">Bilagsnummer.</param>
        /// <param name="kontonummer">Kontonummer, hvorpå værdier skal bogføres.</param>
        /// <param name="tekst">Tekst.</param>
        /// <param name="budgetkontonummer">Budgetkontonummer, hvorpå værdier skal bogføres.</param>
        /// <param name="debit">Debitbeløb.</param>
        /// <param name="kredit">Kreditbeløb.</param>
        /// <param name="adressekonto">Unik identifikation af adressekontoen, hvorpå værdier skal bogføres.</param>
        /// <returns>Bogføringsresultat.</returns>
        private IBogføringsresultatModel Bogfør(int regnskabsnummer, DateTime dato, string bilag, string kontonummer, string tekst, string budgetkontonummer, decimal debit, decimal kredit, int adressekonto)
        {
            try
            {
                lock (SyncRoot)
                {
                    var kontoModel = KontoGet(regnskabsnummer, kontonummer, dato);
                    var budgetkontoModel = string.IsNullOrWhiteSpace(budgetkontonummer) ? null : BudgetkontoGet(regnskabsnummer, budgetkontonummer, dato);
                    var adressekontoModel = adressekonto == 0 ? null : AdressekontoGet(regnskabsnummer, adressekonto, dato);

                    var localeDataDocument = _localeDataStorage.GetLocaleData();
                    var regnskabElement = localeDataDocument.GetRegnskabElement(regnskabsnummer);
                    if (regnskabElement == null)
                    {
                        throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.AccountingNotFound, kontoModel.Regnskabsnummer));
                    }

                    var nextLøbenummer = 1;
                    foreach (var bogføringslinjeElement in regnskabElement.Elements(XName.Get("Bogfoeringslinje", Namespace)))
                    {
                        var value = int.Parse(bogføringslinjeElement.Attribute(XName.Get("loebenummer", string.Empty)).Value, NumberStyles.Integer, CultureInfo.InvariantCulture);
                        if (value >= nextLøbenummer)
                        {
                            nextLøbenummer = value + 1;
                        }
                    }
                    IBogføringslinjeModel bogføringslinjeModel = new BogføringslinjeModel(kontoModel.Regnskabsnummer, nextLøbenummer, dato, kontoModel.Kontonummer, tekst, debit, kredit)
                    {
                        Bilag = string.IsNullOrWhiteSpace(bilag) ? null : bilag,
                        Budgetkontonummer = budgetkontoModel == null ? null : budgetkontoModel.Kontonummer,
                        Adressekonto = adressekontoModel == null ? 0 : adressekontoModel.Nummer
                    };
                    bogføringslinjeModel.StoreInDocument(localeDataDocument, false);

                    kontoModel.Saldo += bogføringslinjeModel.Bogført;
                    kontoModel.StoreInDocument(localeDataDocument);
                    foreach (var kontoHistorikElement in regnskabElement.Elements(XName.Get("KontoHistorik", regnskabElement.Name.NamespaceName)).Where(m => m.Attribute(XName.Get("kontonummer", string.Empty)) != null && string.Compare(m.Attribute(XName.Get("kontonummer", string.Empty)).Value, kontoModel.Kontonummer, StringComparison.Ordinal) == 0 && m.Attribute(XName.Get("dato", string.Empty)) != null && string.Compare(m.Attribute(XName.Get("dato", string.Empty)).Value, ModelExtentions.GetHistorikDato(bogføringslinjeModel.Dato), StringComparison.Ordinal) > 0))
                    {
                        var saldoAttribute = kontoHistorikElement.Attribute(XName.Get("saldo", string.Empty));
                        if (saldoAttribute == null)
                        {
                            continue;
                        }
                        var newSaldo = decimal.Parse(saldoAttribute.Value, NumberStyles.Any, CultureInfo.InvariantCulture) + bogføringslinjeModel.Bogført;
                        kontoHistorikElement.UpdateAttribute(saldoAttribute.Name, newSaldo.ToString("#.00", CultureInfo.InvariantCulture));
                    }

                    if (budgetkontoModel != null)
                    {
                        budgetkontoModel.Bogført += bogføringslinjeModel.Bogført;
                        budgetkontoModel.StoreInDocument(localeDataDocument);
                        foreach (var budgetkontoHistorikElement in regnskabElement.Elements(XName.Get("BudgetkontoHistorik", regnskabElement.Name.NamespaceName)).Where(m => m.Attribute(XName.Get("kontonummer", string.Empty)) != null && string.Compare(m.Attribute(XName.Get("kontonummer", string.Empty)).Value, budgetkontoModel.Kontonummer, StringComparison.Ordinal) == 0 && m.Attribute(XName.Get("dato", string.Empty)) != null && string.Compare(m.Attribute(XName.Get("dato", string.Empty)).Value, ModelExtentions.GetHistorikDato(bogføringslinjeModel.Dato), StringComparison.Ordinal) > 0))
                        {
                            var bogførtAttribute = budgetkontoHistorikElement.Attribute(XName.Get("bogfoert", string.Empty));
                            if (bogførtAttribute == null)
                            {
                                continue;
                            }
                            var newBogført = decimal.Parse(bogførtAttribute.Value, NumberStyles.Any, CultureInfo.InvariantCulture) + bogføringslinjeModel.Bogført;
                            budgetkontoHistorikElement.UpdateAttribute(bogførtAttribute.Name, newBogført.ToString("#.00", CultureInfo.InvariantCulture));
                        }
                    }

                    if (adressekontoModel != null)
                    {
                        adressekontoModel.Saldo += bogføringslinjeModel.Bogført;
                        adressekontoModel.StoreInDocument(localeDataDocument);
                        foreach (var adressekontoHistorikElement in regnskabElement.Elements(XName.Get("AdressekontoHistorik", regnskabElement.Name.NamespaceName)).Where(m => m.Attribute(XName.Get("nummer", string.Empty)) != null && string.Compare(m.Attribute(XName.Get("nummer", string.Empty)).Value, adressekontoModel.Nummer.ToString(CultureInfo.InvariantCulture), StringComparison.Ordinal) == 0 && m.Attribute(XName.Get("dato", string.Empty)) != null && string.Compare(m.Attribute(XName.Get("dato", string.Empty)).Value, ModelExtentions.GetHistorikDato(bogføringslinjeModel.Dato), StringComparison.Ordinal) > 0))
                        {
                            var saldoAttribute = adressekontoHistorikElement.Attribute(XName.Get("saldo", string.Empty));
                            if (saldoAttribute == null)
                            {
                                continue;
                            }
                            var newSaldo = decimal.Parse(saldoAttribute.Value, NumberStyles.Any, CultureInfo.InvariantCulture) + bogføringslinjeModel.Bogført;
                            adressekontoHistorikElement.UpdateAttribute(saldoAttribute.Name, newSaldo.ToString("#.00", CultureInfo.InvariantCulture));
                        }
                    }

                    _localeDataStorage.StoreLocaleDocument(localeDataDocument);
                    return new BogføringsresultatModel(bogføringslinjeModel, new List<BogføringsadvarselModel>(0));
                }
            }
            catch (IntranetGuiRepositoryException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "Bogfør", ex.Message), ex);
            }
        }

        /// <summary>
        /// Henter listen af debitorer til et regnskab.
        /// </summary>
        /// <param name="regnskabsummer">Regnskabsnummer, hvortil listen af debitorer skal hentes.</param>
        /// <param name="statusDato">Statusdato for listen af debitorer.</param>
        /// <returns>Debitorer i regnskabet.</returns>
        private IEnumerable<IAdressekontoModel> DebitorlisteGet(int regnskabsummer, DateTime statusDato)
        {
            try
            {
                return AdressekontolisteGet(regnskabsummer, statusDato)
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
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "DebitorlisteGet", ex.Message), ex);
            }
        }

        /// <summary>
        /// Henter listen af kreditorer til et regnskab.
        /// </summary>
        /// <param name="regnskabsummer">Regnskabsnummer, hvortil listen af kreditorer skal hentes.</param>
        /// <param name="statusDato">Statusdato for listen af kreditorer.</param>
        /// <returns>Kreditorer i regnskabet.</returns>
        private IEnumerable<IAdressekontoModel> KreditorlisteGet(int regnskabsummer, DateTime statusDato)
        {
            try
            {
                return AdressekontolisteGet(regnskabsummer, statusDato)
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
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "KreditorlisteGet", ex.Message), ex);
            }
        }

        /// <summary>
        /// Henter listen af adressekonti til en regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, hvortil listen af adressekonti skal hentes.</param>
        /// <param name="statusDato">Statusdato for listen af adressekonti.</param>
        /// <returns>Adressekonti til regnskabet.</returns>
        private IEnumerable<IAdressekontoModel> AdressekontolisteGet(int regnskabsnummer, DateTime statusDato)
        {
            try
            {
                var localeDataDocument = _localeDataStorage.GetLocaleData();
                var regnskabElement = localeDataDocument.GetRegnskabElement(regnskabsnummer);
                if (regnskabElement == null)
                {
                    return new List<IAdressekontoModel>(0);
                }
                var adressekonti = new List<IAdressekontoModel>();
                foreach (var adressekontoElement in regnskabElement.Elements(XName.Get("Adressekonto", Namespace)))
                {
                    try
                    {
                        var adressekontoModel = new AdressekontoModel(int.Parse(regnskabElement.Attribute(XName.Get("nummer", string.Empty)).Value, NumberStyles.Integer, CultureInfo.InvariantCulture), int.Parse(adressekontoElement.Attribute(XName.Get("nummer", string.Empty)).Value, NumberStyles.Integer, CultureInfo.InvariantCulture), adressekontoElement.Attribute(XName.Get("navn", string.Empty)).Value, statusDato, 0M);
                        if (adressekontoElement.Attribute(XName.Get("primaerTelefon", string.Empty)) != null)
                        {
                            adressekontoModel.PrimærTelefon = adressekontoElement.Attribute(XName.Get("primaerTelefon", string.Empty)).Value;
                        }
                        if (adressekontoElement.Attribute(XName.Get("sekundaerTelefon", string.Empty)) != null)
                        {
                            adressekontoModel.PrimærTelefon = adressekontoElement.Attribute(XName.Get("sekundaerTelefon", string.Empty)).Value;
                        }
                        var historikElement = localeDataDocument.GetHistorikElements(adressekontoModel).FirstOrDefault();
                        if (historikElement != null)
                        {
                            adressekontoModel.Saldo = decimal.Parse(historikElement.Attribute(XName.Get("saldo", string.Empty)).Value, NumberStyles.Any, CultureInfo.InvariantCulture);
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
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "AdressekontolisteGet", ex.Message), ex);
            }
        }

        /// <summary>
        /// Henter en given adressekonto til et regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, hvortil adressekontoen skal hentes.</param>
        /// <param name="nummer">Bogføringsdato, som den nye bogføringslinje skal initieres med.</param>
        /// <param name="statusDato">Statusdato, hvorpå adressekontoen skal hentes.</param>
        /// <returns>Ny bogføringslinje, der efterfølgende kan bogføres.</returns>
        private IAdressekontoModel AdressekontoGet(int regnskabsnummer, int nummer, DateTime statusDato)
        {
            try
            {
                var adressekontoliste = AdressekontolisteGet(regnskabsnummer, statusDato).ToList();
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
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "AdressekontoGet", ex.Message), ex);
            }
        }

        /// <summary>
        /// Henter listen af kontogrupper.
        /// </summary>
        /// <returns>Kontogrupper.</returns>
        private IEnumerable<IKontogruppeModel> KontogruppelisteGet()
        {
            try
            {
                var kontogrupper = new List<IKontogruppeModel>();
                var localeDataDocument = _localeDataStorage.GetLocaleData();
                foreach (var kontogruppeElement in localeDataDocument.Root.Elements(XName.Get("Kontogruppe", Namespace)))
                {
                    try
                    {
                        var kontogruppe = new KontogruppeModel(int.Parse(kontogruppeElement.Attribute(XName.Get("nummer", string.Empty)).Value, NumberStyles.Integer, CultureInfo.InvariantCulture), kontogruppeElement.Attribute(XName.Get("tekst", string.Empty)).Value, (Balancetype) Enum.Parse(typeof (Balancetype), kontogruppeElement.Attribute(XName.Get("balanceType")).Value));
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
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "KontogruppelisteGet", ex.Message), ex);
            }
        }

        /// <summary>
        /// Henter listen af grupper til budgetkonti.
        /// </summary>
        /// <returns>Grupper til budgetkonti.</returns>
        private IEnumerable<IBudgetkontogruppeModel> BudgetkontogruppelisteGet()
        {
            try
            {
                var budgetkontogrupper = new List<IBudgetkontogruppeModel>();
                var localeDataDocument = _localeDataStorage.GetLocaleData();
                foreach (var budgetkontogruppeElement in localeDataDocument.Root.Elements(XName.Get("Budgetkontogruppe", Namespace)))
                {
                    try
                    {
                        var budgetkontogruppe = new BudgetkontogruppeModel(int.Parse(budgetkontogruppeElement.Attribute(XName.Get("nummer", string.Empty)).Value, NumberStyles.Integer, CultureInfo.InvariantCulture), budgetkontogruppeElement.Attribute(XName.Get("tekst", string.Empty)).Value);
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
        }

        /// <summary>
        /// Eventhandler, der rejses, når data i det lokale datalager skal forberedes til læsning eller skrivning.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private static void PrepareLocaleDataEventHandler(object sender, IPrepareLocaleDataEventArgs eventArgs)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            if (eventArgs == null)
            {
                throw new ArgumentNullException("eventArgs");
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
