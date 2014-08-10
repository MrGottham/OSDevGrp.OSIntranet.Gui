using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Events;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.Resources;

namespace OSDevGrp.OSIntranet.Gui.Runtime
{
    /// <summary>
    /// Hjælpeklasse til det lokale datalager.
    /// </summary>
    public static class LocaleDataStorageHelper
    {
        /// <summary>
        /// Eventhandler, der kan afgøre, om der findes et lokalt datalager indeholdende data.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        public static async void HasLocaleDataEventHandler(object sender, object eventArgs)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            if (eventArgs == null)
            {
                throw new ArgumentNullException("eventArgs");
            }
            var localeDataStorage = sender as ILocaleDataStorage;
            if (localeDataStorage == null)
            {
                throw new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "sender", sender.GetType()));
            }
            var handleEvaluationEventArgs = eventArgs as IHandleEvaluationEventArgs;
            if (handleEvaluationEventArgs == null)
            {
                throw new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "eventArgs", eventArgs.GetType()));
            }
            try
            {
                var storageFile = await GetLocaleDataFile(localeDataStorage.LocaleDataFileName, localeDataStorage.SyncDataFileName);
                handleEvaluationEventArgs.Result = storageFile != null;
            }
            catch (FileNotFoundException)
            {
                handleEvaluationEventArgs.Result = false;
            }
        }

        /// <summary>
        /// Eventhandler, der danner en læsestream til det lokale datalager.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        public static async void CreateReaderStreamEventHandler(object sender, object eventArgs)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            if (eventArgs == null)
            {
                throw new ArgumentNullException("eventArgs");
            }
            var localeDataStorage = sender as ILocaleDataStorage;
            if (localeDataStorage == null)
            {
                throw new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "sender", sender.GetType()));
            }
            var handleStreamCreationEventArgs = eventArgs as IHandleStreamCreationEventArgs;
            if (handleStreamCreationEventArgs == null)
            {
                throw new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "eventArgs", eventArgs.GetType()));
            }
            var storageFile = await GetLocaleDataFile(localeDataStorage.LocaleDataFileName, localeDataStorage.SyncDataFileName);
            var storageFileStream = await storageFile.OpenAsync(FileAccessMode.Read).AsTask();
            handleStreamCreationEventArgs.Result = storageFileStream.AsStream();
        }

        /// <summary>
        /// Eventhandler, der danner en skrivestream til det lokale datalager.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        public static async void CreateWriterStreamEventHandler(object sender, object eventArgs)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            if (eventArgs == null)
            {
                throw new ArgumentNullException("eventArgs");
            }
            var localeDataStorage = sender as ILocaleDataStorage;
            if (localeDataStorage == null)
            {
                throw new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "sender", sender.GetType()));
            }
            var handleStreamCreationEventArgs = eventArgs as IHandleStreamCreationEventArgs;
            if (handleStreamCreationEventArgs == null)
            {
                throw new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "eventArgs", eventArgs.GetType()));
            }
            StorageFile storageFile;
            try
            {
                storageFile = await GetLocaleDataFile(localeDataStorage.LocaleDataFileName, localeDataStorage.SyncDataFileName);
            }
            catch (FileNotFoundException)
            {
                storageFile = GetStorageFolder().CreateFileAsync(localeDataStorage.LocaleDataFileName).AsTask().Result;
            }
            var storageFileStream = await storageFile.OpenAsync(FileAccessMode.ReadWrite).AsTask();
            handleStreamCreationEventArgs.Result = storageFileStream.AsStream();
        }

        /// <summary>
        /// Returnerer mappen for det lokale datalager.
        /// </summary>
        /// <returns>Mappen til det lokale datalager.</returns>
        private static StorageFolder GetStorageFolder()
        {
            return ApplicationData.Current.RoamingFolder;
        }

        /// <summary>
        /// Returnerer en fil indeholdende data i det lokale datalager.
        /// </summary>
        /// <param name="fileName">Navnet på filen, som indeholder data i det lokale datalager.</param>
        /// <returns>Fil indeholdende data i det lokale datalager.</returns>
        private static Task<StorageFile> GetStorageFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName");
            }
            return GetStorageFolder().GetFileAsync(fileName).AsTask();
        }

        /// <summary>
        /// Returnerer filen indeholdende data i det lokale datalager.
        /// </summary>
        /// <param name="localeDataFileName">Navnet på filen, som indeholde data i det lokale datalager.</param>
        /// <param name="syncDataFileName">Navnet på filen, som indeholder synkroniseringsdata i det lokale datalager.</param>
        /// <returns>Fil indeholdende data i det lokale datalager.</returns>
        private static async Task<StorageFile> GetLocaleDataFile(string localeDataFileName, string syncDataFileName)
        {
            if (string.IsNullOrEmpty(localeDataFileName))
            {
                throw new ArgumentNullException("localeDataFileName");
            }
            if (string.IsNullOrEmpty(syncDataFileName))
            {
                throw new ArgumentNullException("syncDataFileName");
            }
            try
            {
                return await GetStorageFile(syncDataFileName);
            }
            catch (FileNotFoundException)
            {
                return GetStorageFile(localeDataFileName).Result;
            }
        }
    }
}
