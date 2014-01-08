﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
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
                    using (var waitEvent = new AutoResetEvent(false))
                    {
                        var we = waitEvent;
                        Exception error = null;
                        client.RegnskabslisteGetCompleted += (s, e) =>
                            {
                                try
                                {
                                    if (e.Error != null)
                                    {
                                        error = e.Error;
                                        return;
                                    }
                                    result.AddRange(e.Result.Select(m => new RegnskabModel(m.Nummer, m.Navn)));
                                }
                                finally
                                {
                                    we.Set();
                                }
                            };
                        client.RegnskabslisteGetAsync(query);
                        waitEvent.WaitOne();
                        if (error != null)
                        {
                            throw error;
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
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "RegnskabslisteGet", ex.Message), ex);
            }
            catch (Exception ex)
            {
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "RegnskabslisteGet", ex.Message), ex);
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
                    using (var waitEvent = new AutoResetEvent(false))
                    {
                        var we = waitEvent;
                        Exception error = null;
                        client.BogføringerGetCompleted += (s, e) =>
                            {
                                try
                                {
                                    if (e.Error != null)
                                    {
                                        error = e.Error;
                                        return;
                                    }
                                    foreach (var bogføringslinjeView in e.Result)
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
                                }
                                finally
                                {
                                    we.Set();
                                }
                            };
                        client.BogføringerGetAsync(query);
                        waitEvent.WaitOne();
                        if (error != null)
                        {
                            throw error;
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
                    using (var waitEvent = new AutoResetEvent(false))
                    {
                        var we = waitEvent;
                        Exception error = null;
                        client.DebitorlisteGetCompleted += (s, e) =>
                            {
                                try
                                {
                                    if (e.Error != null)
                                    {
                                        error = e.Error;
                                        return;
                                    }
                                    foreach (var debitorView in e.Result)
                                    {
                                        try
                                        {
                                            var adressekontoModel = new AdressekontoModel(regnskabsnummer, debitorView.Nummer, debitorView.Navn, statusDato, debitorView.Saldo)
                                                {
                                                    PrimærTelefon = debitorView.PrimærTelefon,
                                                    SekundærTelefon = debitorView.SekundærTelefon
                                                };
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
                                }
                                finally
                                {
                                    we.Set();
                                }
                            };
                        client.DebitorlisteGetAsync(query);
                        waitEvent.WaitOne();
                        if (error != null)
                        {
                            throw error;
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
                    using (var waitEvent = new AutoResetEvent(false))
                    {
                        var we = waitEvent;
                        Exception error = null;
                        client.KreditorlisteGetCompleted += (s, e) =>
                            {
                                try
                                {
                                    if (e.Error != null)
                                    {
                                        error = e.Error;
                                        return;
                                    }
                                    foreach (var kreditorView in e.Result)
                                    {
                                        try
                                        {
                                            var adressekontoModel = new AdressekontoModel(regnskabsnummer, kreditorView.Nummer, kreditorView.Navn, statusDato, kreditorView.Saldo)
                                                {
                                                    PrimærTelefon = kreditorView.PrimærTelefon,
                                                    SekundærTelefon = kreditorView.SekundærTelefon
                                                };
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
                                }
                                finally
                                {
                                    we.Set();
                                }
                            };
                        client.KreditorlisteGetAsync(query);
                        waitEvent.WaitOne();
                        if (error != null)
                        {
                            throw error;
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
        /// Returnerer den HTTP binding, der skal benyttes til kommunikation.
        /// </summary>
        /// <returns>HTTP binding, der skal benyttes til kommunikation.</returns>
        private static Binding GetBasicHttpBinding()
        {
            return new BasicHttpBinding(BasicHttpSecurityMode.None)
                {
                    MaxReceivedMessageSize = 4194304
                };
        }

        #endregion
    }
}
