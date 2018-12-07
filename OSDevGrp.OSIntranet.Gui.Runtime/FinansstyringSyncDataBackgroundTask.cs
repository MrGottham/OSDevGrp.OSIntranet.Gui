using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Events;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using Windows.ApplicationModel.Background;

namespace OSDevGrp.OSIntranet.Gui.Runtime
{
    /// <summary>
    /// Baggrundstask, der kan synkronisere finansstyringsdata til og fra det lokale datalager.
    /// </summary>
    public sealed class FinansstyringSyncDataBackgroundTask : IBackgroundTask
    {
        #region Private variables

        private static bool _isSynchronizing;
        private static readonly object SyncRoot = new object();

        #endregion

        /// <summary>
        /// Sarter baggrundstask, der kan synkronisere finansstyringsdata til og fra det lokale datalager.
        /// </summary>
        /// <param name="taskInstance">Instans af baggrundstasken.</param>
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            lock (SyncRoot)
            {
                if (_isSynchronizing)
                {
                    return;
                }
                _isSynchronizing = true;
            }

            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            try
            {

                WindowsRuntimeResourceManager.PatchResourceManagers();

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
                localeDataStorage.PrepareLocaleData += PrepareLocaleDataEventHandler;

                await SyncData(finansstyringRepository, finansstyringKonfigurationRepository, localeDataStorage);
            }
            catch (AggregateException aggregateException)
            {
                aggregateException.Handle(ex =>
                {
                    if (ex is IntranetGuiOfflineRepositoryException)
                    {
                        return true;
                    }
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
                    _isSynchronizing = false;
                }
                deferral.Complete();
            }
        }

        /// <summary>
        /// Synkroniserer finansstyringsdata til og fra det lokale datalager.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af finansstyringsrepositoryet, hvorfra data skal synkroniseres til og fra det lokale datalager.</param>
        /// <param name="finansstyringKonfigurationRepository">Implementering af konfiguration til finansstyringsrepositoryet.</param>
        /// <param name="localeDataStorage">Implementering af det lokale datalager.</param>
        private static async Task SyncData(IFinansstyringRepository finansstyringRepository, IFinansstyringKonfigurationRepository finansstyringKonfigurationRepository, ILocaleDataStorage localeDataStorage)
        {
            if (finansstyringRepository == null)
            {
                throw new ArgumentNullException("finansstyringRepository");
            }
            if (finansstyringKonfigurationRepository == null)
            {
                throw new ArgumentNullException("finansstyringKonfigurationRepository");
            }
            if (localeDataStorage == null)
            {
                throw new ArgumentNullException("localeDataStorage");
            }
            try
            {
                XDocument syncDataDocument = null;

                var kontogruppeliste = await finansstyringRepository.KontogruppelisteGetAsync();
                foreach (var kontogruppe in kontogruppeliste)
                {
                    lock (SyncRoot)
                    {
                        if (syncDataDocument == null)
                        {
                            localeDataStorage.StoreSyncData(kontogruppe);
                            syncDataDocument = localeDataStorage.GetLocaleData();
                        }
                        else
                        {
                            kontogruppe.StoreInDocument(syncDataDocument);
                        }
                    }
                }

                var budgetkontogruppeliste = await finansstyringRepository.BudgetkontogruppelisteGetAsync();
                foreach (var budgetkontogruppe in budgetkontogruppeliste)
                {
                    lock (SyncRoot)
                    {
                        if (syncDataDocument == null)
                        {
                            localeDataStorage.StoreSyncData(budgetkontogruppe);
                            syncDataDocument = localeDataStorage.GetLocaleData();
                        }
                        else
                        {
                            budgetkontogruppe.StoreInDocument(syncDataDocument);
                        }
                    }
                }

                var makeFullSync = true;
                lock (SyncRoot)
                {
                    if (syncDataDocument != null)
                    {
                        var lastFullSync = syncDataDocument.GetSidsteFuldeSynkronisering();
                        makeFullSync = lastFullSync.HasValue == false || lastFullSync.Value.Date < DateTime.Now.AddDays(-30).Date;
                    }
                }

                var regnskabSyncTasks = new List<Task>();
                var regnskabsliste = await finansstyringRepository.RegnskabslisteGetAsync();
                foreach (var regnskab in regnskabsliste)
                {
                    lock (SyncRoot)
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
                    regnskabSyncTasks.Add(SyncData(finansstyringRepository, finansstyringKonfigurationRepository, localeDataStorage, syncDataDocument, regnskab, makeFullSync));
                }
                Task.WaitAll(regnskabSyncTasks.ToArray());

                lock (SyncRoot)
                {
                    if (syncDataDocument == null)
                    {
                        return;
                    }
                    if (makeFullSync)
                    {
                        syncDataDocument.StoreSidsteFuldeSynkroniseringInDocument(DateTime.Now);
                    }
                    localeDataStorage.StoreSyncDocument(syncDataDocument);
                }
            }
            catch (IntranetGuiOfflineRepositoryException)
            {
                // We are currently offline.
                // Don't rethrow the exception.
            }
        }

        /// <summary>
        /// Synkroniserer finansstyringsdata til og fra det lokale datalager.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af finansstyringsrepositoryet, hvorfra data skal synkroniseres til og fra det lokale datalager.</param>
        /// <param name="finansstyringKonfigurationRepository">Implementering af konfiguration til finansstyringsrepositoryet.</param>
        /// <param name="localeDataStorage">Implementering af det lokale datalager.</param>
        /// <param name="syncDataDocument">XML dokument indeholdende de synkroniserede data.</param>
        /// <param name="regnskabModel">Model for regnskabet, hvor data skal synkroniseres til og fra.</param>
        /// <param name="makeFullSync">Angivelse af, om der skal foretages en fuld synkronisering.</param>
        private static async Task SyncData(IFinansstyringRepository finansstyringRepository, IFinansstyringKonfigurationRepository finansstyringKonfigurationRepository, ILocaleDataStorage localeDataStorage, XDocument syncDataDocument, IRegnskabModel regnskabModel, bool makeFullSync)
        {
            if (finansstyringRepository == null)
            {
                throw new ArgumentNullException("finansstyringRepository");
            }
            if (finansstyringKonfigurationRepository == null)
            {
                throw new ArgumentNullException("finansstyringKonfigurationRepository");
            }
            if (localeDataStorage == null)
            {
                throw new ArgumentNullException("localeDataStorage");
            }
            if (syncDataDocument == null)
            {
                throw new ArgumentNullException("syncDataDocument");
            }
            if (regnskabModel == null)
            {
                throw new ArgumentNullException("regnskabModel");
            }
            try
            {
                // Synkronisér regnskabsdata fra det lokale datalager til det online finansstyringsrepository.
                await SyncLocaleData(finansstyringRepository, finansstyringKonfigurationRepository, localeDataStorage, syncDataDocument, regnskabModel);
                // Synkronisér regnskabsdata fra det online finansstyringsrepository til det lokale datalager.
                var currentDate = DateTime.Now;
                foreach (var kontoModel in await finansstyringRepository.KontoplanGetAsync(regnskabModel.Nummer, currentDate))
                {
                    lock (SyncRoot)
                    {
                        kontoModel.StoreInDocument(syncDataDocument);
                    }
                }
                foreach (var budgetkontoModel in await finansstyringRepository.BudgetkontoplanGetAsync(regnskabModel.Nummer, currentDate))
                {
                    lock (SyncRoot)
                    {
                        budgetkontoModel.StoreInDocument(syncDataDocument);
                    }
                }
                if (makeFullSync)
                {
                    var historicStatusDate = currentDate.AddMonths(-2);
                    while (historicStatusDate.Year >= currentDate.AddYears(-1).Year)
                    {
                        historicStatusDate = new DateTime(historicStatusDate.Year, historicStatusDate.Month, DateTime.DaysInMonth(historicStatusDate.Year, historicStatusDate.Month), 23, 59, 59);
                        foreach (var budgetkontoModel in await finansstyringRepository.BudgetkontoplanGetAsync(regnskabModel.Nummer, historicStatusDate))
                        {
                            lock (SyncRoot)
                            {
                                budgetkontoModel.StoreInDocument(syncDataDocument);
                            }
                        }
                        historicStatusDate = historicStatusDate.AddMonths(-2);
                    }
                }
                foreach (var adressekontoModel in await finansstyringRepository.AdressekontolisteGetAsync(regnskabModel.Nummer, currentDate))
                {
                    lock (SyncRoot)
                    {
                        adressekontoModel.StoreInDocument(syncDataDocument);
                    }
                }
                foreach (var bogføringslinjeModel in await finansstyringRepository.BogføringslinjerGetAsync(regnskabModel.Nummer, currentDate, finansstyringKonfigurationRepository.AntalBogføringslinjer))
                {
                    lock (SyncRoot)
                    {
                        bogføringslinjeModel.StoreInDocument(syncDataDocument, true);
                    }
                }
                lock (SyncRoot)
                {
                    localeDataStorage.StoreSyncDocument(syncDataDocument);
                }
            }
            catch (IntranetGuiOfflineRepositoryException)
            {
                // We are currently offline.
                // Don't rethrow the exception.
            }
        }

        /// <summary>
        /// Synkroniserer finansstyringsdata fra det lokale datalager.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af finansstyringsrepositoryet, hvorfra data skal synkroniseres til og fra det lokale datalager.</param>
        /// <param name="finansstyringKonfigurationRepository">Implementering af konfiguration til finansstyringsrepositoryet.</param>
        /// <param name="localeDataStorage">Implementering af det lokale datalager.</param>
        /// <param name="syncDataDocument">XML dokument indeholdende de synkroniserede data.</param>
        /// <param name="regnskabModel">Model for regnskabet, hvor data skal synkroniseres til og fra.</param>
        private static async Task SyncLocaleData(IFinansstyringRepository finansstyringRepository, IFinansstyringKonfigurationRepository finansstyringKonfigurationRepository, ILocaleDataStorage localeDataStorage, XDocument syncDataDocument, IRegnskabModel regnskabModel)
        {
            if (finansstyringRepository == null)
            {
                throw new ArgumentNullException("finansstyringRepository");
            }
            if (finansstyringKonfigurationRepository == null)
            {
                throw new ArgumentNullException("finansstyringKonfigurationRepository");
            }
            if (localeDataStorage == null)
            {
                throw new ArgumentNullException("localeDataStorage");
            }
            if (syncDataDocument == null)
            {
                throw new ArgumentNullException("syncDataDocument");
            }
            if (regnskabModel == null)
            {
                throw new ArgumentNullException("regnskabModel");
            }
            try
            {
                var rootElement = syncDataDocument.Root;
                var regnskabElement = rootElement.Elements(XName.Get("Regnskab", rootElement.Name.NamespaceName)).SingleOrDefault(m => m.Attribute(XName.Get("nummer", string.Empty)) != null && string.Compare(m.Attribute(XName.Get("nummer", string.Empty)).Value, regnskabModel.Nummer.ToString(CultureInfo.InvariantCulture), StringComparison.Ordinal) == 0);
                if (regnskabElement == null)
                {
                    return;
                }

                XElement bogføringslinjeElementToSync;
                lock (SyncRoot)
                {
                    bogføringslinjeElementToSync = GetBogføringslinjeElementToSync(rootElement, regnskabElement);
                }
                while (bogføringslinjeElementToSync != null)
                {
                    var dato = DateTime.ParseExact(bogføringslinjeElementToSync.Attribute(XName.Get("dato", string.Empty)).Value, "yyyyMMdd", CultureInfo.InvariantCulture);
                    var bilag = bogføringslinjeElementToSync.Attribute(XName.Get("bilag", string.Empty)) == null ? null : bogføringslinjeElementToSync.Attribute(XName.Get("bilag", string.Empty)).Value;
                    var kontonummer = bogføringslinjeElementToSync.Attribute(XName.Get("kontonummer", string.Empty)).Value;
                    var tekst = bogføringslinjeElementToSync.Attribute(XName.Get("tekst", string.Empty)).Value;
                    var budgetkontonummer = bogføringslinjeElementToSync.Attribute(XName.Get("budgetkontonummer", string.Empty)) == null ? null : bogføringslinjeElementToSync.Attribute(XName.Get("budgetkontonummer", string.Empty)).Value;
                    var debit = bogføringslinjeElementToSync.Attribute(XName.Get("debit", string.Empty)) == null ? 0M : decimal.Parse(bogføringslinjeElementToSync.Attribute(XName.Get("debit", string.Empty)).Value, NumberStyles.Any, CultureInfo.InvariantCulture);
                    var kredit = bogføringslinjeElementToSync.Attribute(XName.Get("kredit", string.Empty)) == null ? 0M : decimal.Parse(bogføringslinjeElementToSync.Attribute(XName.Get("kredit", string.Empty)).Value, NumberStyles.Any, CultureInfo.InvariantCulture);
                    var adressekonto = bogføringslinjeElementToSync.Attribute(XName.Get("adressekonto", string.Empty)) == null ? 0 : int.Parse(bogføringslinjeElementToSync.Attribute(XName.Get("adressekonto", string.Empty)).Value, NumberStyles.Integer, CultureInfo.InvariantCulture);
                    await finansstyringRepository.BogførAsync(regnskabModel.Nummer, dato, bilag, kontonummer, tekst, budgetkontonummer, debit, kredit, adressekonto);

                    lock (SyncRoot)
                    {
                        bogføringslinjeElementToSync.Remove();
                        localeDataStorage.StoreSyncDocument(syncDataDocument);

                        bogføringslinjeElementToSync = GetBogføringslinjeElementToSync(rootElement, regnskabElement);
                    }
                }
            }
            catch (IntranetGuiOfflineRepositoryException)
            {
                // We are currently offline.
                // Don't rethrow the exception.
            }
        }

        /// <summary>
        /// Finder en XML node, som indeholder en usynkroniseret bogføringslinje.
        /// </summary>
        /// <param name="rootElement">XML rod elementet.</param>
        /// <param name="regnskabElement">XML element indeholde synkroniserede data for et regnskab.</param>
        /// <returns>XML node, som indeholder en usynkroniiseret bogføringslinje.</returns>
        private static XElement GetBogføringslinjeElementToSync(XElement rootElement, XElement regnskabElement)
        {
            if (rootElement == null)
            {
                throw new ArgumentNullException("rootElement");
            }
            if (regnskabElement == null)
            {
                throw new ArgumentNullException("regnskabElement");
            }
            return regnskabElement.Elements(XName.Get("Bogfoeringslinje", rootElement.Name.NamespaceName))
                .Where(m => m.Attribute(XName.Get("loebenummer", string.Empty)) != null && m.Attribute(XName.Get("dato", string.Empty)) != null && m.Attribute(XName.Get("synkroniseret", string.Empty)) != null && Convert.ToBoolean(m.Attribute(XName.Get("synkroniseret", string.Empty)).Value) == false)
                .OrderBy(m => DateTime.ParseExact(m.Attribute(XName.Get("dato", string.Empty)).Value, "yyyyMMdd", CultureInfo.InvariantCulture))
                .ThenBy(m => int.Parse(m.Attribute(XName.Get("loebenummer")).Value, NumberStyles.Integer, CultureInfo.InvariantCulture))
                .FirstOrDefault();
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
            lock (SyncRoot)
            {
                eventArgs.LocaleDataDocument.StoreVersionNumberInDocument(FinansstyringRepositoryLocale.RepositoryVersion);
            }
        }
    }
}
