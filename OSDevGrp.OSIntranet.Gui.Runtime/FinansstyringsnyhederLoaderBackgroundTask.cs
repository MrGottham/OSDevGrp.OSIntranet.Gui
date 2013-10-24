using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using Windows.ApplicationModel.Background;

namespace OSDevGrp.OSIntranet.Gui.Runtime
{
    /// <summary>
    /// Baggrundstask, der kan loaded nyheder til finansstyring.
    /// </summary>
    public sealed class FinansstyringsnyhederLoaderBackgroundTask : IBackgroundTask
    {
        #region Methods

        /// <summary>
        /// Starter baggrundstask, der kan loaded nyheder til finansstyring.
        /// </summary>
        /// <param name="taskInstance">Instans af baggrundstasken.</param>
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            try
            {
                var configurationProvider = new ConfigurationProvider();

                var finansstyringKonfigurationRepository = new FinansstyringKonfigurationRepository();
                finansstyringKonfigurationRepository.KonfigurationerAdd(configurationProvider.Settings);

                var finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepository);

                var finansstyringsnyheder = await FinansstyringsnyhederGetAsync(finansstyringRepository);
                finansstyringsnyheder.ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                deferral.Complete();
            }
        }

        /// <summary>
        /// Henter alle nyheder til finansstyring.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <returns>Nyheder til finansstyring.</returns>
        private static async Task<IEnumerable<INyhedModel>> FinansstyringsnyhederGetAsync(IFinansstyringRepository finansstyringRepository)
        {
            if (finansstyringRepository == null)
            {
                throw new ArgumentNullException("finansstyringRepository");
            }
            var nyheder = new List<INyhedModel>();
            try
            {
                var statusDato = DateTime.Now;
                var konfiguration = finansstyringRepository.Konfiguration;
                foreach (var regnskab in await finansstyringRepository.RegnskabslisteGetAsync())
                {
                    nyheder.AddRange((await finansstyringRepository.BogføringslinjerGetAsync(regnskab.Nummer, statusDato, konfiguration.AntalBogføringslinjer)).Where(m => m.Nyhedsudgivelsestidspunkt.Date.CompareTo(statusDato.AddDays(konfiguration.DageForNyheder*-1).Date) >= 0 && m.Nyhedsudgivelsestidspunkt.Date.CompareTo(statusDato.Date) <= 0).ToList());
                }
                return nyheder.OrderByDescending(m => m.Nyhedsudgivelsestidspunkt).ToList();
            }
            catch (IntranetGuiRepositoryException)
            {
                return nyheder.OrderByDescending(m => m.Nyhedsudgivelsestidspunkt).ToList();
            }
        }

        #endregion
    }
}
