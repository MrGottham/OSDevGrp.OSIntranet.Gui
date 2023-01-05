using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventSource;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Gui.App.Features;
using OSDevGrp.OSIntranet.Gui.App.Settings;
using System.Collections.Concurrent;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.Text;

namespace OSDevGrp.OSIntranet.Gui.App.Core
{
    internal sealed class EventListenerFeature : EventListener, IBackgroundFeature
    {
        #region Private variables

        private readonly IApplicationDataProvider _applicationDataProvider;
        private readonly string _loggerCategory;
        private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

        #endregion

        #region Constructors

        public EventListenerFeature(IApplicationDataProvider applicationDataProvider)
            : this(applicationDataProvider, typeof(MauiApp).Namespace)
        {
        }

        private EventListenerFeature(IApplicationDataProvider applicationDataProvider, string loggerCategory)
        {
            NullGuard.NotNull(applicationDataProvider, nameof(applicationDataProvider))
                .NotNullOrWhiteSpace(loggerCategory, nameof(loggerCategory));

            _applicationDataProvider = applicationDataProvider;
            _loggerCategory = loggerCategory;
        }

        #endregion

        #region Methods

        public Task StartAsync() => Task.CompletedTask;

        public Task StopAsync() => Task.CompletedTask;

        public override void Dispose()
        {
            base.Dispose();

            _semaphoreSlim.Dispose();
        }

        protected override void OnEventSourceCreated(EventSource eventSource)
        {
            NullGuard.NotNull(eventSource, nameof(eventSource));

            if (string.Compare(eventSource.Name, "Microsoft-Extensions-Logging", StringComparison.InvariantCulture) != 0)
            {
                return;
            }

            IDictionary<string, string> arguments = new ConcurrentDictionary<string, string>();
            arguments.Add("FilterSpecs", $"{_loggerCategory};{_loggerCategory}*");

            EnableEvents(eventSource, EventLevel.Informational, LoggingEventSource.Keywords.FormattedMessage, arguments);
        }

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            NullGuard.NotNull(eventData, nameof(eventData));

            string logMessage = BuildLogMessage(eventData);
            if (string.IsNullOrWhiteSpace(logMessage))
            {
                return;
            }

            WriteToLogFile(logMessage);
        }

        private void WriteToLogFile(string logMessage)
        {
            NullGuard.NotNullOrWhiteSpace(logMessage, nameof(logMessage));

            _semaphoreSlim.Wait();
            try
            {
                FileInfo logFile = _applicationDataProvider.LogFile;

                using FileStream fileStream = logFile.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
                using StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8);

                fileStream.Seek(0, SeekOrigin.End);

                streamWriter.WriteLine(logMessage);
                streamWriter.Flush();

                streamWriter.Close();
                fileStream.Close();
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        private static string BuildLogMessage(EventWrittenEventArgs eventData)
        {
            NullGuard.NotNull(eventData, nameof(eventData));

            if (string.Compare(eventData.EventName, "FormattedMessage", StringComparison.InvariantCulture) != 0 || eventData.Payload == null || eventData.Payload.Count < 6)
            {
                return null;
            }

            return $"{eventData.TimeStamp.ToString("O", CultureInfo.InvariantCulture)} {eventData.Payload[2]} ({ToLogLevel(eventData.Payload[0])}): {eventData.Payload[5]}";
        }

        private static LogLevel ToLogLevel(object value)
        {
            if (value == null)
            {
                return LogLevel.None;
            }

            return (LogLevel)Enum.ToObject(typeof(LogLevel), Convert.ToInt32(value));
        }

        #endregion
    }
}