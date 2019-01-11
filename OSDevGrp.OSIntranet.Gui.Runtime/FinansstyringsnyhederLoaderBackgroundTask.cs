using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace OSDevGrp.OSIntranet.Gui.Runtime
{
    /// <summary>
    /// Baggrundstask, der kan loaded nyheder til finansstyring.
    /// </summary>
    public sealed class FinansstyringsnyhederLoaderBackgroundTask : IBackgroundTask
    {
        #region Private variables

        private static bool _isLoading;
        private static readonly object SyncRoot = new object();

        #endregion

        #region Methods

        /// <summary>
        /// Starter baggrundstask, der kan loaded nyheder til finansstyring.
        /// </summary>
        /// <param name="taskInstance">Instans af baggrundstasken.</param>
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            lock (SyncRoot)
            {
                if (_isLoading)
                {
                    return;
                }
                _isLoading = true;
            }

            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            try
            {
                WindowsRuntimeResourceManager.PatchResourceManagers();

                ConfigurationProvider configurationProvider = new ConfigurationProvider();

                IDictionary<string, object> finansstyringConfiguration = configurationProvider.Settings
                    .Where(m => FinansstyringKonfigurationRepository.Keys.Contains(m.Key))
                    .ToDictionary(m => m.Key, m => m.Value);
                IFinansstyringKonfigurationRepository finansstyringKonfigurationRepository = new FinansstyringKonfigurationRepository();
                finansstyringKonfigurationRepository.KonfigurationerAdd(finansstyringConfiguration);

                IEnumerable<INyhedModel> finansstyringsnyheder;
                try
                {
                    IFinansstyringRepository finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepository);
                    finansstyringsnyheder = await FinansstyringsnyhederGetAsync(finansstyringRepository);
                }
                catch (IntranetGuiOfflineRepositoryException)
                {
                    ILocaleDataStorage localeDataStorage = new LocaleDataStorage(finansstyringKonfigurationRepository.LokalDataFil, finansstyringKonfigurationRepository.SynkroniseringDataFil, FinansstyringRepositoryLocale.XmlSchema);
                    localeDataStorage.OnHasLocaleData += LocaleDataStorageHelper.HasLocaleDataEventHandler;
                    localeDataStorage.OnCreateReaderStream += LocaleDataStorageHelper.CreateReaderStreamEventHandler;
                    localeDataStorage.OnCreateWriterStream += LocaleDataStorageHelper.CreateWriterStreamEventHandler;

                    IFinansstyringRepository finansstyringRepository = new FinansstyringRepositoryLocale(finansstyringKonfigurationRepository, localeDataStorage);

                    Task<IEnumerable<INyhedModel>> finansstyringsnyhederGetTask = FinansstyringsnyhederGetAsync(finansstyringRepository);
                    finansstyringsnyhederGetTask.Wait();
                    finansstyringsnyheder = finansstyringsnyhederGetTask.Result;
                }

                UpdateTile(finansstyringsnyheder);
            }
            catch (AggregateException aggregateException)
            {
                aggregateException.Handle(ex =>
                {
                    Logger.LogError(ex);
                    return true;
                });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
            finally
            {
                lock (SyncRoot)
                {
                    _isLoading = false;
                }
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
                throw new ArgumentNullException(nameof(finansstyringRepository));
            }

            List<INyhedModel> nyheder = new List<INyhedModel>();
            try
            {
                DateTime statusDato = DateTime.Now;

                IFinansstyringKonfigurationRepository konfiguration = finansstyringRepository.Konfiguration;
                DateTime nyhederFromDate = statusDato.AddDays(konfiguration.DageForNyheder * -1);
                DateTime nyhederToDate = statusDato;

                foreach (IRegnskabModel regnskab in await finansstyringRepository.RegnskabslisteGetAsync())
                {
                    nyheder.AddRange((await finansstyringRepository.BogføringslinjerGetAsync(regnskab.Nummer, statusDato, konfiguration.AntalBogføringslinjer)).Where(m => m.Nyhedsudgivelsestidspunkt.Date.CompareTo(nyhederFromDate.Date) >= 0 && m.Nyhedsudgivelsestidspunkt.Date.CompareTo(nyhederToDate.Date) <= 0).ToList());
                    nyheder.AddRange((await finansstyringRepository.DebitorlisteGetAsync(regnskab.Nummer, statusDato)).Where(m => m.Nyhedsudgivelsestidspunkt.Date.CompareTo(nyhederFromDate.Date) >= 0 && m.Nyhedsudgivelsestidspunkt.Date.CompareTo(nyhederToDate.Date) <= 0).ToList());
                    nyheder.AddRange((await finansstyringRepository.KreditorlisteGetAsync(regnskab.Nummer, statusDato)).Where(m => m.Nyhedsudgivelsestidspunkt.Date.CompareTo(nyhederFromDate.Date) >= 0 && m.Nyhedsudgivelsestidspunkt.Date.CompareTo(nyhederToDate.Date) <= 0).ToList());
                }
                return nyheder;
            }
            catch (IntranetGuiOfflineRepositoryException)
            {
                throw;
            }
            catch (IntranetGuiRepositoryException)
            {
                return nyheder;
            }
        }

        /// <summary>
        /// Opdaterer applikationstile med nyheder til finansstyring.
        /// </summary>
        /// <param name="nyheder">Nyheder til finansstyring.</param>
        private static void UpdateTile(IEnumerable<INyhedModel> nyheder)
        {
            if (nyheder == null)
            {
                throw new ArgumentNullException(nameof(nyheder));
            }

            TileUpdater tileUpdater = TileUpdateManager.CreateTileUpdaterForApplication();
            tileUpdater.EnableNotificationQueue(true);
            tileUpdater.Clear();
            foreach (IGrouping<DateTime, INyhedModel> nyhedsgruppe in nyheder.Where(m => string.IsNullOrEmpty(m.Nyhedsinformation) == false).OrderByDescending(m => m.Nyhedsudgivelsestidspunkt.Date).Take(5).GroupBy(m => m.Nyhedsudgivelsestidspunkt.Date))
            {
                foreach (INyhedModel nyhed in nyhedsgruppe.OrderByDescending(m => m.Nyhedsaktualitet))
                {
                    tileUpdater.Update(UpdateTile(nyhed, TileTemplateType.TileSquare150x150Text04));
                    tileUpdater.Update(UpdateTile(nyhed, TileTemplateType.TileWide310x150BlockAndText02));
                }
            }
        }

        /// <summary>
        /// Opdaterer applikationstile med en given nyhed på en given templatetype.
        /// </summary>
        /// <param name="nyhed">Nyhed, som applikationstile skal opdateres med.</param>
        /// <param name="type">Templatetypen, som skal præsentere nyheden.</param>
        /// <returns>Notifikation, som applikationstile skal benytte.</returns>
        private static TileNotification UpdateTile(INyhedModel nyhed, TileTemplateType type)
        {
            if (nyhed == null)
            {
                throw new ArgumentNullException(nameof(nyhed));
            }

            XmlDocument tileXml = TileUpdateManager.GetTemplateContent(type);
            tileXml.GetElementsByTagName("text").First().InnerText = nyhed.Nyhedsinformation;
            return new TileNotification(tileXml);
        }

        #endregion
    }
}
