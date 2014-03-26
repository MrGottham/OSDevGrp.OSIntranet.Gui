using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Models.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Finansstyring.Service;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.Resources;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Finansstyring
{
    /// <summary>
    /// Repository, der supporterer finansstyring.
    /// </summary>
    public class FinansstyringRepository : IFinansstyringRepository
    {
        #region Private variables

        private readonly IFinansstyringKonfigurationRepository _finansstyringKonfigurationRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner repository, der supporterer finansstyring.
        /// </summary>
        /// <param name="finansstyringKonfigurationRepository">Implementering af konfigurationsrepository, der supporterer finansstyring.</param>
        public FinansstyringRepository(IFinansstyringKonfigurationRepository finansstyringKonfigurationRepository)
        {
            if (finansstyringKonfigurationRepository == null)
            {
                throw new ArgumentNullException("finansstyringKonfigurationRepository");
            }
            _finansstyringKonfigurationRepository = finansstyringKonfigurationRepository;
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
                var result = new List<IRegnskabModel>();
                var binding = GetBasicHttpBinding();
                var endpointAddress = new EndpointAddress(Konfiguration.FinansstyringServiceUri);
                var client = new FinansstyringServiceClient(binding, endpointAddress);
                try
                {
                    var query = new RegnskabslisteGetQuery();
                    
                    var serviceInterface = (FinansstyringService) client;
                    var asyncResult = serviceInterface.BeginRegnskabslisteGet(query, null, null);
                    result.AddRange(serviceInterface.EndRegnskabslisteGet(asyncResult).Select(m => new RegnskabModel(m.Nummer, m.Navn)));
                    
                    client.CloseAsync();
                }
                catch
                {
                    client.Abort();
                    throw;
                }
                return result;
            }
            catch (IntranetGuiRepositoryException)
            {
                throw;
            }
            catch (FaultException ex)
            {
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "RegnskabslisteGet", ex.Message), ex);
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
                var result = new List<IKontoModel>();
                var binding = GetBasicHttpBinding();
                var endpointAddress = new EndpointAddress(Konfiguration.FinansstyringServiceUri);
                var client = new FinansstyringServiceClient(binding, endpointAddress);
                try
                {
                    var query = new KontoplanGetQuery
                        {
                            Regnskabsnummer = regnskabsnummer,
                            StatusDato = statusDato
                        };

                    var serviceInterface = (FinansstyringService) client;
                    var asyncResult = serviceInterface.BeginKontoplanGet(query, null, null);
                    foreach (var kontoView in serviceInterface.EndKontoplanGet(asyncResult))
                    {
                        try
                        {
                            var kontoModel = new KontoModel(regnskabsnummer, kontoView.Kontonummer, kontoView.Kontonavn, kontoView.Kontogruppe.Nummer, statusDato, kontoView.Saldo, kontoView.Kredit);
                            if (string.IsNullOrEmpty(kontoView.Beskrivelse) == false)
                            {
                                kontoModel.Beskrivelse = kontoView.Beskrivelse;
                            }
                            if (string.IsNullOrEmpty(kontoView.Notat) == false)
                            {
                                kontoModel.Notat = kontoView.Notat;
                            }
                            result.Add(kontoModel);
                        }
                        catch (ArgumentNullException)
                        {
                        }
                        catch (ArgumentException)
                        {
                        }
                    }

                    client.CloseAsync();
                }
                catch
                {
                    client.Abort();
                    throw;
                }
                return result;
            }
            catch (IntranetGuiRepositoryException)
            {
                throw;
            }
            catch (FaultException ex)
            {
                throw new IntranetGuiRepositoryException(
                    Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "KontoplanGet", ex.Message), ex);
            }
            catch (Exception ex)
            {
                throw new IntranetGuiRepositoryException(
                    Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "KontoplanGet", ex.Message), ex);
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
            if (string.IsNullOrEmpty(kontonummer))
            {
                throw new ArgumentNullException("kontonummer");
            }
            try
            {
                IKontoModel kontoModel;
                var binding = GetBasicHttpBinding();
                var endpointAddress = new EndpointAddress(Konfiguration.FinansstyringServiceUri);
                var client = new FinansstyringServiceClient(binding, endpointAddress);
                try
                {
                    var query = new KontoGetQuery
                        {
                            Regnskabsnummer = regnskabsnummer,
                            Kontonummer = kontonummer,
                            StatusDato = statusDato
                        };

                    var serviceInterface = (FinansstyringService) client;
                    var asyncResult = serviceInterface.BeginKontoGet(query, null, null);
                    var kontoView = serviceInterface.EndKontoGet(asyncResult);

                    kontoModel = new KontoModel(regnskabsnummer, kontoView.Kontonummer, kontoView.Kontonavn, kontoView.Kontogruppe.Nummer, statusDato, kontoView.Saldo, kontoView.Kredit);
                    if (string.IsNullOrEmpty(kontoView.Beskrivelse) == false)
                    {
                        kontoModel.Beskrivelse = kontoView.Beskrivelse;
                    }
                    if (string.IsNullOrEmpty(kontoView.Notat) == false)
                    {
                        kontoModel.Notat = kontoView.Notat;
                    }

                    client.CloseAsync();
                }
                catch
                {
                    client.Abort();
                    throw;
                }
                return kontoModel;
            }
            catch (IntranetGuiRepositoryException)
            {
                throw;
            }
            catch (FaultException ex)
            {
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "KontoGet", ex.Message), ex);
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
                var result = new List<IBudgetkontoModel>();
                var binding = GetBasicHttpBinding();
                var endpointAddress = new EndpointAddress(Konfiguration.FinansstyringServiceUri);
                var client = new FinansstyringServiceClient(binding, endpointAddress);
                try
                {
                    var query = new BudgetkontoplanGetQuery
                        {
                            Regnskabsnummer = regnskabsnummer,
                            StatusDato = statusDato
                        };

                    var serviceInterface = (FinansstyringService) client;
                    var asyncResult = serviceInterface.BeginBudgetkontoplanGet(query, null, null);
                    foreach (var budgetkontoView in serviceInterface.EndBudgetkontoplanGet(asyncResult))
                    {
                        try
                        {
                            var budgetkontoModel = new BudgetkontoModel(regnskabsnummer, budgetkontoView.Kontonummer, budgetkontoView.Kontonavn, budgetkontoView.Budgetkontogruppe.Nummer, statusDato, budgetkontoView.Budget, budgetkontoView.Bogført);
                            if (string.IsNullOrEmpty(budgetkontoView.Beskrivelse) == false)
                            {
                                budgetkontoModel.Beskrivelse = budgetkontoView.Beskrivelse;
                            }
                            if (string.IsNullOrEmpty(budgetkontoView.Notat) == false)
                            {
                                budgetkontoModel.Notat = budgetkontoView.Notat;
                            }
                            result.Add(budgetkontoModel);
                        }
                        catch (ArgumentNullException)
                        {
                        }
                        catch (ArgumentException)
                        {
                        }
                    }

                    client.CloseAsync();
                }
                catch (Exception)
                {
                    client.Abort();
                    throw;
                }
                return result;
            }
            catch (IntranetGuiRepositoryException)
            {
                throw;
            }
            catch (FaultException ex)
            {
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "BudgetkontoplanGet", ex.Message), ex);
            }
            catch (Exception ex)
            {
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "BudgetkontoplanGet", ex.Message), ex);
            }
        }

        /// <summary>
        /// Henter en given budgetkonto i et givet regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, hvorfra budgetkontoen skal hentes.</param>
        /// <param name="budgetkontonummer">Kontonummer på budgetkontoen, der skal hentes.</param>
        /// <param name="statusDato">Statusdato for budgetkontoen.</param>
        /// <returns>Budgetkonto.</returns>
        private IBudgetkontoModel BudgetkontoGet(int regnskabsnummer, string budgetkontonummer, DateTime statusDato)
        {
            if (string.IsNullOrEmpty(budgetkontonummer))
            {
                throw new ArgumentNullException("budgetkontonummer");
            }
            try
            {
                IBudgetkontoModel budgetkontoModel;
                var binding = GetBasicHttpBinding();
                var endpointAddress = new EndpointAddress(Konfiguration.FinansstyringServiceUri);
                var client = new FinansstyringServiceClient(binding, endpointAddress);
                try
                {
                    var query = new BudgetkontoGetQuery
                        {
                            Regnskabsnummer = regnskabsnummer,
                            Kontonummer = budgetkontonummer,
                            StatusDato = statusDato
                        };

                    var serviceInterface = (FinansstyringService) client;
                    var asyncResult = serviceInterface.BeginBudgetkontoGet(query, null, null);
                    var budgetkontoView = serviceInterface.EndBudgetkontoGet(asyncResult);

                    budgetkontoModel = new BudgetkontoModel(regnskabsnummer, budgetkontoView.Kontonummer, budgetkontoView.Kontonavn, budgetkontoView.Budgetkontogruppe.Nummer, statusDato, budgetkontoView.Budget, budgetkontoView.Bogført);
                    if (string.IsNullOrEmpty(budgetkontoView.Beskrivelse) == false)
                    {
                        budgetkontoModel.Beskrivelse = budgetkontoView.Beskrivelse;
                    }
                    if (string.IsNullOrEmpty(budgetkontoView.Notat) == false)
                    {
                        budgetkontoModel.Notat = budgetkontoView.Notat;
                    }

                    client.CloseAsync();
                }
                catch (Exception)
                {
                    client.Abort();
                    throw;
                }
                return budgetkontoModel;
            }
            catch (IntranetGuiRepositoryException)
            {
                throw;
            }
            catch (FaultException ex)
            {
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "BudgetkontoGet", ex.Message), ex);
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
                var result = new List<IBogføringslinjeModel>(antalBogføringslinjer);
                var binding = GetBasicHttpBinding();
                var endpointAddress = new EndpointAddress(Konfiguration.FinansstyringServiceUri);
                var client = new FinansstyringServiceClient(binding, endpointAddress);
                try
                {
                    var query = new BogføringerGetQuery
                        {
                            Regnskabsnummer = regnskabsnummer,
                            StatusDato = statusDato,
                            Linjer = antalBogføringslinjer
                        };

                    var serviceInterface = (FinansstyringService) client;
                    var asyncResult = serviceInterface.BeginBogføringerGet(query, null, null);
                    foreach (var bogføringslinjeView in serviceInterface.EndBogføringerGet(asyncResult))
                    {
                        try
                        {
                            if (bogføringslinjeView.Konto == null)
                            {
                                continue;
                            }
                            var bogføringslinjeModel = new BogføringslinjeModel(regnskabsnummer, bogføringslinjeView.Løbenr, bogføringslinjeView.Dato, bogføringslinjeView.Konto.Kontonummer, bogføringslinjeView.Tekst, bogføringslinjeView.Debit, bogføringslinjeView.Kredit);
                            if (string.IsNullOrEmpty(bogføringslinjeView.Bilag) == false)
                            {
                                bogføringslinjeModel.Bilag = bogføringslinjeView.Bilag;
                            }
                            if (bogføringslinjeView.Budgetkonto != null)
                            {
                                bogføringslinjeModel.Budgetkontonummer = bogføringslinjeView.Budgetkonto.Kontonummer;
                            }
                            if (bogføringslinjeView.Adressekonto != null)
                            {
                                bogføringslinjeModel.Adressekonto = bogføringslinjeView.Adressekonto.Nummer;
                            }
                            result.Add(bogføringslinjeModel);
                        }
                        catch (ArgumentNullException)
                        {
                        }
                        catch (ArgumentException)
                        {
                        }
                    }

                    client.CloseAsync();
                }
                catch
                {
                    client.Abort();
                    throw;
                }
                return result;
            }
            catch (IntranetGuiRepositoryException)
            {
                throw;
            }
            catch (FaultException ex)
            {
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "BogføringslinjerGet", ex.Message), ex);
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
                IBogføringsresultatModel bogføringsresultatModel;
                var binding = GetBasicHttpBinding();
                var endpointAddress = new EndpointAddress(Konfiguration.FinansstyringServiceUri);
                var client = new FinansstyringServiceClient(binding, endpointAddress);
                try
                {
                    var command = new BogføringslinjeOpretCommand
                        {
                            Regnskabsnummer = regnskabsnummer,
                            Dato = dato,
                            Bilag = string.IsNullOrEmpty(bilag) ? null : bilag,
                            Kontonummer = kontonummer,
                            Tekst = tekst,
                            Budgetkontonummer = string.IsNullOrEmpty(budgetkontonummer) ? null : budgetkontonummer,
                            Debit = debit,
                            Kredit = kredit,
                            Adressekonto = adressekonto
                        };

                    var serviceInterface = (FinansstyringService) client;
                    var asyncResult = serviceInterface.BeginBogføringslinjeOpret(command, null, null);
                    var response = serviceInterface.EndBogføringslinjeOpret(asyncResult);

                    var bogføringslinjeModel = new BogføringslinjeModel(regnskabsnummer, response.Løbenr, response.Dato, response.Konto.Kontonummer, response.Tekst, response.Debit, response.Kredit)
                        {
                            Bilag = string.IsNullOrEmpty(response.Bilag) ? null : response.Bilag,
                            Budgetkontonummer = response.Budgetkonto == null ? null : string.IsNullOrEmpty(response.Budgetkonto.Kontonummer) ? null : response.Budgetkonto.Kontonummer,
                            Adressekonto = response.Adressekonto == null ? 0 : response.Adressekonto.Nummer
                        };
                    var bogføringsadvarselModelCollection = new List<IBogføringsadvarselModel>();
                    if (response.Advarsler != null)
                    {
                        bogføringsadvarselModelCollection.AddRange(response.Advarsler.Where(m => string.IsNullOrEmpty(m.Advarsel) == false && m.Konto != null && string.IsNullOrEmpty(m.Konto.Kontonummer) == false && string.IsNullOrEmpty(m.Konto.Kontonavn) == false).Select(m => new BogføringsadvarselModel(m.Advarsel, m.Konto.Kontonummer, m.Konto.Kontonavn, m.Beløb)));
                    }
                    bogføringsresultatModel = new BogføringsresultatModel(bogføringslinjeModel, bogføringsadvarselModelCollection);

                    client.CloseAsync();
                }
                catch (Exception)
                {
                    client.Abort();
                    throw;
                }
                return bogføringsresultatModel;
            }
            catch (IntranetGuiRepositoryException)
            {
                throw;
            }
            catch (FaultException ex)
            {
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "Bogfør", ex.Message), ex);
            }
            catch (Exception ex)
            {
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "Bogfør", ex.Message), ex);
            }
        }

        /// <summary>
        /// Henter listen af debitorer til et regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, hvortil listen af debitorer skal hentes.</param>
        /// <param name="statusDato">Statusdato for listen af debitorer.</param>
        /// <returns>Debitorer i regnskabet.</returns>
        private IEnumerable<IAdressekontoModel> DebitorlisteGet(int regnskabsnummer, DateTime statusDato)
        {
            try
            {
                var debitorer = new List<IAdressekontoModel>();
                var binding = GetBasicHttpBinding();
                var endpointAddress = new EndpointAddress(Konfiguration.FinansstyringServiceUri);
                var client = new FinansstyringServiceClient(binding, endpointAddress);
                try
                {
                    var query = new DebitorlisteGetQuery
                        {
                            Regnskabsnummer = regnskabsnummer,
                            StatusDato = statusDato
                        };

                    var serviceInterface = (FinansstyringService) client;
                    var asyncResult = serviceInterface.BeginDebitorlisteGet(query, null, null);
                    foreach (var debitorView in serviceInterface.EndDebitorlisteGet(asyncResult))
                    {
                        try
                        {
                            var adressekontoModel = new AdressekontoModel(regnskabsnummer, debitorView.Nummer, debitorView.Navn, statusDato, debitorView.Saldo);
                            if (string.IsNullOrEmpty(debitorView.PrimærTelefon) == false)
                            {
                                adressekontoModel.PrimærTelefon = debitorView.PrimærTelefon.Trim();
                                if (string.IsNullOrEmpty(debitorView.SekundærTelefon) == false && string.Compare(adressekontoModel.PrimærTelefon, debitorView.SekundærTelefon.Trim(), StringComparison.OrdinalIgnoreCase) != 0)
                                {
                                    adressekontoModel.SekundærTelefon = debitorView.SekundærTelefon.Trim();
                                }
                            }
                            else if (string.IsNullOrEmpty(debitorView.SekundærTelefon) == false)
                            {
                                adressekontoModel.PrimærTelefon = debitorView.PrimærTelefon.Trim();
                            }
                            adressekontoModel.SetNyhedsaktualitet(Nyhedsaktualitet.Low);
                            debitorer.Add(adressekontoModel);
                        }
                        catch (ArgumentNullException)
                        {
                        }
                        catch (ArgumentException)
                        {
                        }
                    }

                    client.CloseAsync();
                }
                catch (Exception)
                {
                    client.Abort();
                    throw;
                }
                return debitorer;
            }
            catch (IntranetGuiRepositoryException)
            {
                throw;
            }
            catch (FaultException ex)
            {
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "DebitorlisteGet", ex.Message), ex);
            }
            catch (Exception ex)
            {
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "DebitorlisteGet", ex.Message), ex);
            }
        }

        /// <summary>
        /// Henter listen af kreditorer til et regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, hvortil listen af kreditorer skal hentes.</param>
        /// <param name="statusDato">Statusdato for listen af kreditorer.</param>
        /// <returns>Kreditorer i regnskabet.</returns>
        private IEnumerable<IAdressekontoModel> KreditorlisteGet(int regnskabsnummer, DateTime statusDato)
        {
            try
            {
                var kreditorer = new List<IAdressekontoModel>();
                var binding = GetBasicHttpBinding();
                var endpointAddress = new EndpointAddress(Konfiguration.FinansstyringServiceUri);
                var client = new FinansstyringServiceClient(binding, endpointAddress);
                try
                {
                    var query = new KreditorlisteGetQuery
                        {
                            Regnskabsnummer = regnskabsnummer,
                            StatusDato = statusDato
                        };

                    var serviceInterface = (FinansstyringService) client;
                    var asyncResult = serviceInterface.BeginKreditorlisteGet(query, null, null);
                    foreach (var kreditorView in serviceInterface.EndKreditorlisteGet(asyncResult))
                    {
                        try
                        {
                            var adressekontoModel = new AdressekontoModel(regnskabsnummer, kreditorView.Nummer, kreditorView.Navn, statusDato, kreditorView.Saldo);
                            if (string.IsNullOrEmpty(kreditorView.PrimærTelefon) == false)
                            {
                                adressekontoModel.PrimærTelefon = kreditorView.PrimærTelefon.Trim();
                                if (string.IsNullOrEmpty(kreditorView.SekundærTelefon) == false && string.Compare(adressekontoModel.PrimærTelefon, kreditorView.SekundærTelefon.Trim(), StringComparison.OrdinalIgnoreCase) != 0)
                                {
                                    adressekontoModel.SekundærTelefon = kreditorView.SekundærTelefon.Trim();
                                }
                            }
                            else if (string.IsNullOrEmpty(kreditorView.SekundærTelefon) == false)
                            {
                                adressekontoModel.PrimærTelefon = kreditorView.PrimærTelefon.Trim();
                            }
                            adressekontoModel.SetNyhedsaktualitet(Nyhedsaktualitet.High);
                            kreditorer.Add(adressekontoModel);
                        }
                        catch (ArgumentNullException)
                        {
                        }
                        catch (ArgumentException)
                        {
                        }
                    }

                    client.CloseAsync();
                }
                catch (Exception)
                {
                    client.Abort();
                    throw;
                }
                return kreditorer;
            }
            catch (IntranetGuiRepositoryException)
            {
                throw;
            }
            catch (FaultException ex)
            {
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "KreditorlisteGet", ex.Message), ex);
            }
            catch (Exception ex)
            {
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "KreditorlisteGet", ex.Message), ex);
            }
        }

        /// <summary>
        /// Henter en given adressekonto til et regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, hvortil listen af kreditorer skal hentes.</param>
        /// <param name="nummer">Unik identifikation af adressekontoen.</param>
        /// <param name="statusDato">Statusdato for listen af kreditorer.</param>
        /// <returns>Adressekonto.</returns>
        private IAdressekontoModel AdressekontoGet(int regnskabsnummer, int nummer, DateTime statusDato)
        {
            try
            {
                IAdressekontoModel adressekontoModel;
                var binding = GetBasicHttpBinding();
                var endpointAddress = new EndpointAddress(Konfiguration.FinansstyringServiceUri);
                var client = new FinansstyringServiceClient(binding, endpointAddress);
                try
                {
                    var query = new AdressekontoGetQuery
                        {
                            Regnskabsnummer = regnskabsnummer,
                            Nummer = nummer,
                            StatusDato = statusDato
                        };

                    var serviceInterface = (FinansstyringService) client;
                    var asyncResult = serviceInterface.BeginAdressekontoGet(query, null, null);
                    var adressekontoView = serviceInterface.EndAdressekontoGet(asyncResult);

                    adressekontoModel = new AdressekontoModel(regnskabsnummer, adressekontoView.Nummer, adressekontoView.Navn, statusDato, adressekontoView.Saldo);
                    if (string.IsNullOrEmpty(adressekontoView.PrimærTelefon) == false)
                    {
                        adressekontoModel.PrimærTelefon = adressekontoView.PrimærTelefon.Trim();
                        if (string.IsNullOrEmpty(adressekontoView.SekundærTelefon) == false && string.Compare(adressekontoModel.PrimærTelefon, adressekontoView.SekundærTelefon.Trim(), StringComparison.OrdinalIgnoreCase) != 0)
                        {
                            adressekontoModel.SekundærTelefon = adressekontoView.SekundærTelefon.Trim();
                        }
                    }
                    else if (string.IsNullOrEmpty(adressekontoView.SekundærTelefon) == false)
                    {
                        adressekontoModel.PrimærTelefon = adressekontoView.PrimærTelefon.Trim();
                    }

                    client.CloseAsync();
                }
                catch (Exception)
                {
                    client.Abort();
                    throw;
                }
                return adressekontoModel;
            }
            catch (IntranetGuiRepositoryException)
            {
                throw;
            }
            catch (FaultException ex)
            {
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "AdressekontoGet", ex.Message), ex);
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
                var binding = GetBasicHttpBinding();
                var endpointAddress = new EndpointAddress(Konfiguration.FinansstyringServiceUri);
                var client = new FinansstyringServiceClient(binding, endpointAddress);
                try
                {
                    var query = new KontogrupperGetQuery();

                    var serviceInterface = (FinansstyringService) client;
                    var asyncResult = serviceInterface.BeginKontogrupperGet(query, null, null);
                    kontogrupper.AddRange(serviceInterface.EndKontogrupperGet(asyncResult).Select(m => new KontogruppeModel(m.Nummer, m.Navn, m.ErAktiver ? Balancetype.Aktiver : Balancetype.Passiver)));

                    client.CloseAsync();
                }
                catch (Exception)
                {
                    client.Abort();
                    throw;
                }
                return kontogrupper;
            }
            catch (IntranetGuiRepositoryException)
            {
                throw;
            }
            catch (FaultException ex)
            {
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "KontogruppelisteGet", ex.Message), ex);
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
                var binding = GetBasicHttpBinding();
                var endpointAddress = new EndpointAddress(Konfiguration.FinansstyringServiceUri);
                var client = new FinansstyringServiceClient(binding, endpointAddress);
                try
                {
                    var query = new BudgetkontogrupperGetQuery();

                    var serviceInterface = (FinansstyringService) client;
                    var asyncResult = serviceInterface.BeginBudgetkontogrupperGet(query, null, null);
                    budgetkontogrupper.AddRange(serviceInterface.EndBudgetkontogrupperGet(asyncResult).Select(m => new BudgetkontogruppeModel(m.Nummer, m.Navn)));

                    client.CloseAsync();
                }
                catch (Exception)
                {
                    client.Abort();
                    throw;
                }
                return budgetkontogrupper;
            }
            catch (IntranetGuiRepositoryException)
            {
                throw;
            }
            catch (FaultException ex)
            {
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "BudgetkontogruppelisteGet", ex.Message), ex);
            }
            catch (Exception ex)
            {
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "BudgetkontogruppelisteGet", ex.Message), ex);
            }
        }

        /// <summary>
        /// Returnerer den HTTP binding, der skal benyttes til kommunikation.
        /// </summary>
        /// <returns>HTTP binding, der skal benyttes til kommunikation.</returns>
        private static Binding GetBasicHttpBinding()
        {
            return new BasicHttpBinding(BasicHttpSecurityMode.None)
                {
                    SendTimeout = new TimeSpan(0, 0, 3, 0),
                    ReceiveTimeout = new TimeSpan(0, 0, 3, 0),
                    MaxReceivedMessageSize = 4194304
                };
        }

        #endregion
    }
}
