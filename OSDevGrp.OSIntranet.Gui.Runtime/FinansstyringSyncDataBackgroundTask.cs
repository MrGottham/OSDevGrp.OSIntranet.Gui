using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.ApplicationModel.Background;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Repositories.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Gui.Runtime
{
    /// <summary>
    /// Baggrundstask, der kan synkronisere finansstyringsdata til og fra det lokale datalager.
    /// </summary>
    public sealed class FinansstyringSyncDataBackgroundTask : IBackgroundTask
    {
        /// <summary>
        /// Sarter baggrundstask, der kan synkronisere finansstyringsdata til og fra det lokale datalager.
        /// </summary>
        /// <param name="taskInstance">Instans af baggrundstasken.</param>
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            try
            {
                var configurationProvider = new ConfigurationProvider();

                var finansstyringConfiguration = configurationProvider.Settings
                    .Where(m => FinansstyringKonfigurationRepository.Keys.Contains(m.Key))
                    .ToDictionary(m => m.Key, m => m.Value);
                var finansstyringKonfigurationRepository = new FinansstyringKonfigurationRepository();
                finansstyringKonfigurationRepository.KonfigurationerAdd(finansstyringConfiguration);

                var finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepository);

                var localeDataStorage = new LocaleDataStorage(finansstyringKonfigurationRepository.LokalDataFil, finansstyringKonfigurationRepository.SynkroniseringDataFil, FinansstyringRepositoryLocale.XmlSchema);
                localeDataStorage.OnHasLocaleData += LocaleDataStorageHelper.HasLocaleDataEventHandler;
                localeDataStorage.OnCreateReaderStream += LocaleDataStorageHelper.CreateReaderStreamEventHandler;
                localeDataStorage.OnCreateWriterStream += LocaleDataStorageHelper.CreateWriterStreamEventHandler;

                await SyncData(finansstyringRepository, localeDataStorage);
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
        /// Synkroniserer finansstyringsdata til og fra det lokale datalager.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af finansstyringsrepositoryet, hvorfra data skal synkroniseres til og fra det lokale datalager.</param>
        /// <param name="localeDataStorage">Implementering af det lokale datalger.</param>
        private static Task SyncData(IFinansstyringRepository finansstyringRepository, ILocaleDataStorage localeDataStorage)
        {
            if (finansstyringRepository == null)
            {
                throw new ArgumentNullException("finansstyringRepository");
            }
            if (localeDataStorage == null)
            {
                throw new ArgumentNullException("localeDataStorage");
            }
            Action func = async () =>
            {
                try
                {
                    XDocument syncDataDocument = null;

                    var regnskabsliste = await finansstyringRepository.RegnskabslisteGetAsync();
                    var syncDataTasks = new List<Task>();
                    foreach (var regnskab in regnskabsliste)
                    {
                        if (syncDataDocument == null)
                        {
                            localeDataStorage.StoreSyncData(regnskab);
                            syncDataDocument = localeDataStorage.GetLocaleData();
                        }
                        else
                        {
                            regnskab.StoreInDocument(syncDataDocument);
                        }
                    }
                    Task.WaitAll(syncDataTasks.ToArray());

                    if (syncDataDocument == null)
                    {
                        return;
                    }
                    localeDataStorage.StoreSyncDocument(syncDataDocument);
                }
                catch (IntranetGuiOfflineRepositoryException)
                {
                    // We are currently offline.
                    // Don't rethrow the exception.
                }
            };
            return Task.Run(func);
        }
    }
}
