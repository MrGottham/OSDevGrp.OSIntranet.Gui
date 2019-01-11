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
        private static bool _cancelRequested;
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
                _cancelRequested = false;
            }

            taskInstance.Canceled += CanceledEventHandler;

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

                IFinansstyringRepository finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepository);

                ILocaleDataStorage localeDataStorage = new LocaleDataStorage(finansstyringKonfigurationRepository.LokalDataFil, finansstyringKonfigurationRepository.SynkroniseringDataFil, FinansstyringRepositoryLocale.XmlSchema);
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
                throw new ArgumentNullException(nameof(finansstyringRepository));
            }
            if (finansstyringKonfigurationRepository == null)
            {
                throw new ArgumentNullException(nameof(finansstyringKonfigurationRepository));
            }
            if (localeDataStorage == null)
            {
                throw new ArgumentNullException(nameof(localeDataStorage));
            }

            try
            {
                XDocument syncDataDocument = null;

                IEnumerable<IKontogruppeModel> kontogruppeliste = await finansstyringRepository.KontogruppelisteGetAsync();
                foreach (IKontogruppeModel kontogruppe in kontogruppeliste)
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

                IEnumerable<IBudgetkontogruppeModel> budgetkontogruppeliste = await finansstyringRepository.BudgetkontogruppelisteGetAsync();
                foreach (IBudgetkontogruppeModel budgetkontogruppe in budgetkontogruppeliste)
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

                bool makeFullSync = true;
                lock (SyncRoot)
                {
                    if (syncDataDocument != null)
                    {
                        DateTime? lastFullSync = syncDataDocument.GetSidsteFuldeSynkronisering();
                        makeFullSync = lastFullSync.HasValue == false || lastFullSync.Value.Date < DateTime.Now.AddDays(-30).Date;
                    }
                }

                IRegnskabModel[] regnskabsliste = (await finansstyringRepository.RegnskabslisteGetAsync()).ToArray();
                await SyncLocaleData(finansstyringRepository, localeDataStorage, syncDataDocument, regnskabsliste);
                if (_cancelRequested)
                {
                    return;
                }

                IList<Task> regnskabSyncTasks = new List<Task>();
                foreach (IRegnskabModel regnskab in regnskabsliste)
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
        /// Synchronize the locale accounting data for a collection of <see cref="IRegnskabModel"/> to the online accounting repository.
        /// </summary>
        /// <param name="accountingRepository">The <see cref="IFinansstyringRepository"/> for the online accounting repository.</param>
        /// <param name="localeDataStorage">The <see cref="ILocaleDataStorage"/> for storing locale data.</param>>
        /// <param name="syncDataDocument">The <see cref="XDocument"/> containing the locale accounting data.</param>
        /// <param name="accountingCollection">The collection of <see cref="IRegnskabModel"/> on which to synchronize locale accounting data to the online repository.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="accountingRepository"/>, <paramref name="localeDataStorage"/> or <paramref name="accountingCollection"/> is null.</exception>
        private static async Task SyncLocaleData(IFinansstyringRepository accountingRepository, ILocaleDataStorage localeDataStorage, XDocument syncDataDocument, IEnumerable<IRegnskabModel> accountingCollection)
        {
            if (accountingRepository == null)
            {
                throw new ArgumentNullException(nameof(accountingRepository));
            }
            if (localeDataStorage == null)
            {
                throw new ArgumentNullException(nameof(localeDataStorage));
            }
            if (accountingCollection == null)
            {
                throw new ArgumentNullException(nameof(accountingCollection));
            }

            try
            {
                lock (SyncRoot)
                {
                    if (syncDataDocument == null)
                    {
                        return;
                    }
                }

                foreach (IRegnskabModel accountingModel in accountingCollection)
                {
                    await SyncLocaleData(accountingRepository, localeDataStorage, syncDataDocument, accountingModel);
                    if (_cancelRequested)
                    {
                        return;
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
        /// Synchronize the locale accounting data for a given <see cref="IRegnskabModel"/> to the online accounting repository.
        /// </summary>
        /// <param name="accountingRepository">The <see cref="IFinansstyringRepository"/> for the online accounting repository.</param>
        /// <param name="localeDataStorage">The <see cref="ILocaleDataStorage"/> for storing locale data.</param>>
        /// <param name="syncDataDocument">The <see cref="XDocument"/> containing the locale accounting data.</param>
        /// <param name="accountingModel">The given <see cref="IRegnskabModel"/> on which to synchronize locale accounting data to the online repository.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="accountingRepository"/>, <paramref name="localeDataStorage"/>, <paramref name="syncDataDocument"/> or <paramref name="accountingModel"/> is null.</exception>
        private static async Task SyncLocaleData(IFinansstyringRepository accountingRepository, ILocaleDataStorage localeDataStorage, XDocument syncDataDocument, IRegnskabModel accountingModel)
        {
            if (accountingRepository == null)
            {
                throw new ArgumentNullException(nameof(accountingRepository));
            }
            if (localeDataStorage == null)
            {
                throw new ArgumentNullException(nameof(localeDataStorage));
            }
            if (syncDataDocument == null)
            {
                throw new ArgumentNullException(nameof(syncDataDocument));
            }
            if (accountingModel == null)
            {
                throw new ArgumentNullException(nameof(accountingModel));
            }

            try
            {
                XElement rootElement = syncDataDocument.Root;
                XElement accountingElement = rootElement?.Elements(XName.Get("Regnskab", rootElement.Name.NamespaceName)).SingleOrDefault(element => GetAttributeValue(element, "nummer") != null && string.Compare(GetAttributeValue(element, "nummer"), accountingModel.Nummer.ToString(CultureInfo.InvariantCulture), StringComparison.Ordinal) == 0);
                if (accountingElement == null)
                {
                    return;
                }

                XElement postingLineToSync;
                lock (SyncRoot)
                {
                    postingLineToSync = GetPostingLineToSync(rootElement, accountingElement);
                    if (postingLineToSync != null && _cancelRequested == false)
                    {
                        postingLineToSync.StorePendingPostingLineInDocument();
                        localeDataStorage.StoreSyncDocument(syncDataDocument);
                    }
                }
                while (postingLineToSync != null && _cancelRequested == false)
                {
                    DateTime postingDate = DateTime.ParseExact(GetAttributeValue(postingLineToSync, "dato"), "yyyyMMdd", CultureInfo.InvariantCulture);
                    string voucherNo = GetAttributeValue(postingLineToSync, "bilag");
                    string accountNumber = GetAttributeValue(postingLineToSync, "kontonummer");
                    string text = GetAttributeValue(postingLineToSync, "tekst");
                    string budgetAccountNumber = GetAttributeValue(postingLineToSync, "budgetkontonummer");
                    decimal debit = GetAttributeValue(postingLineToSync, "debit") == null ? 0M : decimal.Parse(GetAttributeValue(postingLineToSync, "debit"), NumberStyles.Any, CultureInfo.InvariantCulture);
                    decimal credit = GetAttributeValue(postingLineToSync, "kredit") == null ? 0M : decimal.Parse(GetAttributeValue(postingLineToSync, "kredit"), NumberStyles.Any, CultureInfo.InvariantCulture);
                    int addressAccountNumber = GetAttributeValue(postingLineToSync, "adressekonto") == null ? 0 : int.Parse(GetAttributeValue(postingLineToSync, "adressekonto"), NumberStyles.Integer, CultureInfo.InvariantCulture);
                    await accountingRepository.BogførAsync(accountingModel.Nummer, postingDate, voucherNo, accountNumber, text, budgetAccountNumber, debit, credit, addressAccountNumber);

                    lock (SyncRoot)
                    {
                        postingLineToSync.Remove();
                        localeDataStorage.StoreSyncDocument(syncDataDocument);

                        postingLineToSync = GetPostingLineToSync(rootElement, accountingElement);
                        if (postingLineToSync != null && _cancelRequested == false)
                        {
                            postingLineToSync.StorePendingPostingLineInDocument();
                            localeDataStorage.StoreSyncDocument(syncDataDocument);
                        }
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
                throw new ArgumentNullException(nameof(finansstyringRepository));
            }
            if (finansstyringKonfigurationRepository == null)
            {
                throw new ArgumentNullException(nameof(finansstyringKonfigurationRepository));
            }
            if (localeDataStorage == null)
            {
                throw new ArgumentNullException(nameof(localeDataStorage));
            }
            if (syncDataDocument == null)
            {
                throw new ArgumentNullException(nameof(syncDataDocument));
            }
            if (regnskabModel == null)
            {
                throw new ArgumentNullException(nameof(regnskabModel));
            }

            try
            {
                // Synkronisér regnskabsdata fra det online finansstyringsrepository til det lokale datalager.
                DateTime currentDate = DateTime.Now;
                foreach (IKontoModel kontoModel in await finansstyringRepository.KontoplanGetAsync(regnskabModel.Nummer, currentDate))
                {
                    lock (SyncRoot)
                    {
                        kontoModel.StoreInDocument(syncDataDocument);
                    }
                }
                foreach (IBudgetkontoModel budgetkontoModel in await finansstyringRepository.BudgetkontoplanGetAsync(regnskabModel.Nummer, currentDate))
                {
                    lock (SyncRoot)
                    {
                        budgetkontoModel.StoreInDocument(syncDataDocument);
                    }
                }
                if (makeFullSync)
                {
                    DateTime historicStatusDate = currentDate.AddMonths(-2);
                    while (historicStatusDate.Year >= currentDate.AddYears(-1).Year)
                    {
                        historicStatusDate = new DateTime(historicStatusDate.Year, historicStatusDate.Month, DateTime.DaysInMonth(historicStatusDate.Year, historicStatusDate.Month), 23, 59, 59);
                        foreach (IBudgetkontoModel budgetkontoModel in await finansstyringRepository.BudgetkontoplanGetAsync(regnskabModel.Nummer, historicStatusDate))
                        {
                            lock (SyncRoot)
                            {
                                budgetkontoModel.StoreInDocument(syncDataDocument);
                            }
                        }
                        historicStatusDate = historicStatusDate.AddMonths(-2);
                    }
                }
                foreach (IAdressekontoModel adressekontoModel in await finansstyringRepository.AdressekontolisteGetAsync(regnskabModel.Nummer, currentDate))
                {
                    lock (SyncRoot)
                    {
                        adressekontoModel.StoreInDocument(syncDataDocument);
                    }
                }
                foreach (IBogføringslinjeModel bogføringslinjeModel in await finansstyringRepository.BogføringslinjerGetAsync(regnskabModel.Nummer, currentDate, finansstyringKonfigurationRepository.AntalBogføringslinjer))
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

            lock (SyncRoot)
            {
                eventArgs.LocaleDataDocument.StoreVersionNumberInDocument(FinansstyringRepositoryLocale.RepositoryVersion);
            }
        }

        /// <summary>
        /// Event handler which handles a canceled event.
        /// </summary>
        /// <param name="sender">The <see cref="IBackgroundTaskInstance"/> which send the canceled event.</param>
        /// <param name="reason">The <see cref="BackgroundTaskCancellationReason"/>.</param>
        private static void CanceledEventHandler(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            lock (SyncRoot)
            {
                _cancelRequested = true;
            }
        }

        /// <summary>
        /// Gets the first posting line which needs to be synchronized to the online accounting repository.
        /// </summary>
        /// <param name="rootElement">The root <see cref="XElement"/> within the <see cref="XDocument"/> containing the locale accounting data.</param>
        /// <param name="accountingElement">The accounting <see cref="XElement"/> within the <see cref="XDocument"/> containing the local accounting data.</param>
        /// <returns>The first posting line which need to be synchronized to the online accounting repository or null when no posting lines need to be synchronized.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="rootElement"/> or <paramref name="accountingElement"/> is null.</exception>
        private static XElement GetPostingLineToSync(XElement rootElement, XElement accountingElement)
        {
            if (rootElement == null)
            {
                throw new ArgumentNullException(nameof(rootElement));
            }
            if (accountingElement == null)
            {
                throw new ArgumentNullException(nameof(accountingElement));
            }

            return accountingElement.Elements(XName.Get("Bogfoeringslinje", rootElement.Name.NamespaceName))
                .Where(m => GetAttributeValue(m, "loebenummer") != null &&
                            GetAttributeValue(m, "dato") != null &&
                            GetAttributeValue(m, "synkroniseret") != null && Convert.ToBoolean(GetAttributeValue(m, "synkroniseret")) == false &&
                            (GetAttributeValue(m, "verserende") == null || Convert.ToBoolean(GetAttributeValue(m, "verserende")) == false))
                .OrderBy(m => DateTime.ParseExact(GetAttributeValue(m, "dato"), "yyyyMMdd", CultureInfo.InvariantCulture))
                .ThenBy(m => int.Parse(GetAttributeValue(m, "loebenummer"), NumberStyles.Integer, CultureInfo.InvariantCulture))
                .FirstOrDefault();
        }

        /// <summary>
        /// Gets the value for a given attribute.
        /// </summary>
        /// <param name="element">The <see cref="XElement"/> on which to get the attribute.</param>
        /// <param name="attributeName">The name of the attribute.</param>
        /// <returns>The value for the given attribute or null when the attribute does not exists.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="element"/> is null or when <paramref name="attributeName"/> is null, empty or white space.</exception>
        private static string GetAttributeValue(XElement element, string attributeName)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }
            if (string.IsNullOrWhiteSpace(attributeName))
            {
                throw new ArgumentNullException(nameof(attributeName));
            }

            XAttribute attribute = element.Attribute(XName.Get(attributeName, string.Empty));
            if (attribute == null)
            {
                return null;
            }

            return string.IsNullOrWhiteSpace(attribute.Value) ? null : attribute.Value;
        }
    }
}
