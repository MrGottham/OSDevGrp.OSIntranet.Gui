using System;
using System.IO;
using Windows.Storage;

namespace OSDevGrp.OSIntranet.Gui.Runtime
{
    /// <summary>
    /// Implementation of a logger.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Log an error.
        /// </summary>
        /// <param name="exception">Exception which should be logged as an error.</param>
        public static void LogError(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException();
            }
            try
            {
                var logFileName = string.Format("Error.{0}.txt", DateTime.Now.ToString("yyyyMMdd"));
                var logFile = GetLogFile(logFileName);

                var openStreamForReadTask = logFile.OpenStreamForWriteAsync();
                openStreamForReadTask.Wait();

                using (var streamWriter = new StreamWriter(openStreamForReadTask.Result))
                {
                    streamWriter.BaseStream.Seek(0, SeekOrigin.End);

                    var writeLineTask = streamWriter.WriteLineAsync(string.Format("Error ({1}) at {0} - {2}", DateTime.Now.ToString("T"), exception.GetType().Name, exception.Message));
                    writeLineTask.Wait();

                    if (string.IsNullOrEmpty(exception.StackTrace) == false)
                    {
                        writeLineTask = streamWriter.WriteLineAsync(exception.StackTrace);
                        writeLineTask.Wait();
                    }

                    var flushTask = streamWriter.FlushAsync();
                    flushTask.Wait();
                }
            }
            catch (AggregateException aggregateException)
            {
                // Don't throw anything when logging fails.
                aggregateException.Handle(e => true);
            }
            catch (Exception)
            {
                // Don't throw anything when logging fails.
            }
        }

        /// <summary>
        /// Gets a storage file for the log.
        /// </summary>
        /// <param name="logFileName">File name for the log.</param>
        /// <returns>Storage file for the log.</returns>
        private static StorageFile GetLogFile(string logFileName)
        {
            if (string.IsNullOrEmpty(logFileName))
            {
                throw new ArgumentNullException("logFileName");
            }
            try
            {
                var getFileTask = ApplicationData.Current.LocalFolder.GetFileAsync(logFileName).AsTask();
                getFileTask.Wait();

                return getFileTask.Result;
            }
            catch (AggregateException aggregateException)
            {
                var logFileNotFound = false;
                aggregateException.Handle(exception =>
                {
                    logFileNotFound = exception is FileNotFoundException;
                    return true;
                });

                if (logFileNotFound == false)
                {
                    throw;
                }

                var createFileTask = ApplicationData.Current.LocalFolder.CreateFileAsync(logFileName).AsTask();
                createFileTask.Wait();

                return createFileTask.Result;
            }
        }
    }
}
