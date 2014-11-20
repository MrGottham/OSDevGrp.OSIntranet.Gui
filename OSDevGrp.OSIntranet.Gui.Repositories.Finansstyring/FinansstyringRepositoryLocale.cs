using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
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

        public const string XmlSchema = "Schemas.FinansstyringRepositoryLocale.xsd";
        public const string Namespace = "urn:osdevgrp:osintranet:finansstyringrepository:locale:1.0.0";

        #endregion

        #region Private variables

        private readonly IFinansstyringKonfigurationRepository _finansstyringKonfigurationRepository;
        private readonly ILocaleDataStorage _localeDataStorage;

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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
                        var historikElement = localeDataDocument.GetHistorikElements(budgetkontoModel).FirstOrDefault();
                        if (historikElement != null)
                        {
                            budgetkontoModel.Indtægter = decimal.Parse(historikElement.Attribute(XName.Get("indtaegter", string.Empty)).Value, NumberStyles.Any, CultureInfo.InvariantCulture);
                            budgetkontoModel.Indtægter = decimal.Parse(historikElement.Attribute(XName.Get("udgifter", string.Empty)).Value, NumberStyles.Any, CultureInfo.InvariantCulture);
                            budgetkontoModel.Indtægter = decimal.Parse(historikElement.Attribute(XName.Get("bogfoert", string.Empty)).Value, NumberStyles.Any, CultureInfo.InvariantCulture);
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

        #endregion
    }
}
