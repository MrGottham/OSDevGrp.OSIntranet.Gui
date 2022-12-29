using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core;
using System.Globalization;
using System.Xml;

namespace OSDevGrp.OSIntranet.Gui.App.Settings
{
    internal class ApplicationDataProvider : IApplicationDataProvider
    {
        #region Constants

        private const string ApiEndpointKey = "ApiEndpoint";
        private const string SecurityClientIdKey = "Security:ClientId";
        private const string SecurityClientSecretKey = "Security:ClientSecret";
        private const string ShouldOpenSettingsOnStartupKey = "ShouldOpenSettingsOnStartup";

        private const string ApiEndpointDefault = "https://api.osdevgrp.local";
        private const string SecurityClientIdDefault = "[TBD]";
        private const string SecurityClientSecretDefault = "[TBD]";

        #endregion

        #region Private variables

        private FileInfo _offlineDataFile;
        private readonly Func<ISecureStorage> _secureStorageGetter;
        private readonly Func<IFileSystem> _fileSystemGetter;
        private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

        #endregion

        #region Constructor

        public ApplicationDataProvider(Func<ISecureStorage> secureStorageGetter, Func<IFileSystem> fileSystemGetter)
        {
            NullGuard.NotNull(secureStorageGetter, nameof(secureStorageGetter))
                .NotNull(fileSystemGetter, nameof(fileSystemGetter));

            _secureStorageGetter = secureStorageGetter;
            _fileSystemGetter = fileSystemGetter;
        }

        #endregion

        #region Properties

        public FileInfo OfflineDataFile
        {
            get
            {
                _semaphoreSlim.Wait();
                try
                {
                    return _offlineDataFile ??= new FileInfo(Path.Combine(ResolveFileSystem().AppDataDirectory, "OfflineData.xml"));
                }
                finally
                {
                    _semaphoreSlim.Release();
                }
            }
        }

        #endregion

        #region Methods

        public void Dispose()
        {
            _semaphoreSlim.Dispose();
        }

        public async Task<Uri> GetApiEndpointAsync()
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                string value = await ResolveSecureStorage().GetAsync(ApiEndpointKey);
                if (string.IsNullOrWhiteSpace(value))
                {
                    return new Uri(ApiEndpointDefault);
                }

                if (Uri.TryCreate(value, UriKind.Absolute, out Uri apiEndpoint) == false)
                {
                    return new Uri(ApiEndpointDefault);
                }

                return apiEndpoint;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        public async Task<string> GetClientIdAsync()
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                string value = await ResolveSecureStorage().GetAsync(SecurityClientIdKey);

                return string.IsNullOrWhiteSpace(value) ? SecurityClientIdDefault : value;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        public async Task<string> GetClientSecretAsync()
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                string value = await ResolveSecureStorage().GetAsync(SecurityClientSecretKey);

                return string.IsNullOrWhiteSpace(value) ? SecurityClientSecretDefault : value;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        public async Task<bool> GetShouldOpenSettingsOnStartupAsync()
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                string value = await ResolveSecureStorage().GetAsync(ShouldOpenSettingsOnStartupKey);
                if (string.IsNullOrWhiteSpace(value))
                {
                    return true;
                }

                if (bool.TryParse(value, out bool shouldOpenSettingsOnStartup))
                {
                    return shouldOpenSettingsOnStartup;
                }

                return true;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        public async Task<XmlDocument> GetOfflineDataDocumentAsync()
        {
            FileInfo offlineDataFile = OfflineDataFile;

            await _semaphoreSlim.WaitAsync();
            try
            {
                offlineDataFile.Refresh();
                if (offlineDataFile.Exists == false)
                {
                    return OfflineDataDocumentFactory.Build();
                }

                XmlNamespaceManager offlineDataNamespaceManager = OfflineDataNamespaceManagerFactory.Build();
                XmlDocument offlineDataDocument = new XmlDocument(offlineDataNamespaceManager.NameTable ?? new NameTable());

                await using FileStream fileStream = offlineDataFile.Open(FileMode.Open, FileAccess.Read, FileShare.Read);

                XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
                using XmlReader xmlReader = XmlReader.Create(fileStream, xmlReaderSettings);

                offlineDataDocument.Load(xmlReader);

                xmlReader.Close();
                fileStream.Close();

                return offlineDataDocument;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        public async Task SetApiEndpointAsync(Uri apiEndpoint)
        {
            NullGuard.NotNull(apiEndpoint, nameof(apiEndpoint));

            await _semaphoreSlim.WaitAsync();
            try
            {
                await ResolveSecureStorage().SetAsync(ApiEndpointKey, apiEndpoint.AbsoluteUri);
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        public async Task SetClientIdAsync(string clientId)
        {
            NullGuard.NotNullOrWhiteSpace(clientId, nameof(clientId));

            await _semaphoreSlim.WaitAsync();
            try
            {
                await ResolveSecureStorage().SetAsync(SecurityClientIdKey, clientId);
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        public async Task SetClientSecretAsync(string clientSecret)
        {
            NullGuard.NotNullOrWhiteSpace(clientSecret, nameof(clientSecret));

            await _semaphoreSlim.WaitAsync();
            try
            {
                await ResolveSecureStorage().SetAsync(SecurityClientSecretKey, clientSecret);
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        public async Task SetShouldOpenSettingsOnStartupAsync(bool shouldOpenSettingsOnStartup)
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                await ResolveSecureStorage().SetAsync(ShouldOpenSettingsOnStartupKey, shouldOpenSettingsOnStartup.ToString(CultureInfo.InvariantCulture));
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        private ISecureStorage ResolveSecureStorage()
        {
            return _secureStorageGetter.Invoke();
        }

        private IFileSystem ResolveFileSystem()
        {
            return _fileSystemGetter.Invoke();
        }

        internal static IApplicationDataProvider Create(Func<ISecureStorage> secureStorageGetter, Func<IFileSystem> fileSystemGetter)
        {
            NullGuard.NotNull(secureStorageGetter, nameof(secureStorageGetter))
                .NotNull(fileSystemGetter, nameof(fileSystemGetter));

            return new ApplicationDataProvider(secureStorageGetter, fileSystemGetter);
        }

        #endregion
    }
}