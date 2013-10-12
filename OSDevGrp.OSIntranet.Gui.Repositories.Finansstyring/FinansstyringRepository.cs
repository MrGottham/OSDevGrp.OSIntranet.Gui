using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Models.Finansstyring;
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
        #region Private constants

        private const string FinansstyringServiceUri = "http://mother/osintranet/finansstyringservice.svc/mobile";

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
        /// Henter en liste af regnskaber.
        /// </summary>
        /// <returns>Liste af regnskaber.</returns>
        private static IEnumerable<IRegnskabModel> RegnskabslisteGet()
        {
            try
            {
                var result = new List<IRegnskabModel>();
                var binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
                var endpointAddress = new EndpointAddress(FinansstyringServiceUri);
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
        private static IEnumerable<IBogføringslinjeModel> BogføringslinjerGet(int regnskabsnummer, DateTime statusDato, int antalBogføringslinjer)
        {
            try
            {
                var result = new List<IBogføringslinjeModel>(antalBogføringslinjer);
                var binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
                var endpointAddress = new EndpointAddress(FinansstyringServiceUri);
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
                }
                catch
                {
                    client.Abort();
                    throw;
                }
                return result;
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

        #endregion
    }
}
