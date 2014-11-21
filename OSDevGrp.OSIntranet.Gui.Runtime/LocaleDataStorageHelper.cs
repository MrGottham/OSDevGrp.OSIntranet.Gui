using System;
using System.IO;
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
        public static void HasLocaleDataEventHandler(object sender, object eventArgs)
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
                var storageFile = GetLocaleDataFile(localeDataStorage.LocaleDataFileName, localeDataStorage.SyncDataFileName);
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
        public static void CreateReaderStreamEventHandler(object sender, object eventArgs)
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
            var storageFile = GetLocaleDataFile(localeDataStorage.LocaleDataFileName, localeDataStorage.SyncDataFileName);
            try
            {
                var openTask = storageFile.OpenAsync(FileAccessMode.Read).AsTask();
                openTask.Wait();
                handleStreamCreationEventArgs.Result = openTask.Result.AsStream();
            }
            catch (AggregateException ex)
            {
                if (ex.InnerException == null)
                {
                    throw;
                }
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// Eventhandler, der danner en skrivestream til det lokale datalager.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        public static void CreateWriterStreamEventHandler(object sender, object eventArgs)
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
            if (Convert.ToBoolean(handleStreamCreationEventArgs.CreationContext))
            {
                try
                {
                    storageFile = GetStorageFile(localeDataStorage.SyncDataFileName);
                }
                catch (FileNotFoundException)
                {
                    try
                    {
                        var createFileTask = GetStorageFolder().CreateFileAsync(localeDataStorage.SyncDataFileName).AsTask();
                        createFileTask.Wait();
                        storageFile = createFileTask.Result;
                    }
                    catch (AggregateException ex)
                    {
                        if (ex.InnerException == null)
                        {
                            throw;
                        }
                        throw ex.InnerException;
                    }
                }
            }
            else
            {
                try
                {
                    storageFile = GetLocaleDataFile(localeDataStorage.LocaleDataFileName, localeDataStorage.SyncDataFileName);
                }
                catch (FileNotFoundException)
                {
                    try
                    {
                        var createFileTask = GetStorageFolder().CreateFileAsync(localeDataStorage.LocaleDataFileName).AsTask();
                        createFileTask.Wait();
                        storageFile = createFileTask.Result;
                    }
                    catch (AggregateException ex)
                    {
                        if (ex.InnerException == null)
                        {
                            throw;
                        }
                        throw ex.InnerException;
                    }
                }
            }
            try
            {
                var openFileTask = storageFile.OpenAsync(FileAccessMode.ReadWrite).AsTask();
                openFileTask.Wait();
                handleStreamCreationEventArgs.Result = openFileTask.Result.AsStream();
            }
            catch (AggregateException ex)
            {
                if (ex.InnerException == null)
                {
                    throw;
                }
                throw ex.InnerException;
            }
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
        private static StorageFile GetStorageFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName");
            }
            try
            {
                var getFileTask = GetStorageFolder().GetFileAsync(fileName).AsTask();
                getFileTask.Wait();
                return getFileTask.Result;
            }
            catch (AggregateException ex)
            {
                if (ex.InnerException == null)
                {
                    throw;
                }
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// Returnerer filen indeholdende data i det lokale datalager.
        /// </summary>
        /// <param name="localeDataFileName">Navnet på filen, som indeholde data i det lokale datalager.</param>
        /// <param name="syncDataFileName">Navnet på filen, som indeholder synkroniseringsdata i det lokale datalager.</param>
        /// <returns>Fil indeholdende data i det lokale datalager.</returns>
        private static StorageFile GetLocaleDataFile(string localeDataFileName, string syncDataFileName)
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
                return GetStorageFile(syncDataFileName);
            }
            catch (FileNotFoundException)
            {
                return GetStorageFile(localeDataFileName);
            }
        }
    }
}
