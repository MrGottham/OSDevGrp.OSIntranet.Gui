using Microsoft.Extensions.Options;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Gui.App.Settings;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core.Options;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security.Models;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security.Options;

namespace OSDevGrp.OSIntranet.Gui.App.Core
{
    public class StartupViewModel : ViewModelBase
    {
        #region Private variables

        private bool _initializing;
        private bool _initialized;
        private readonly IOptions<OnlineOptions> _onlineOptions;
        private readonly IOptions<OfflineOptions> _offlineOptions;
        private readonly IOptions<SecurityOptions> _securityOptions;
        private readonly IOptions<AppOptions> _appOptions;
        private readonly IApplicationDataProvider _applicationDataProvider;
        private readonly IAccessTokenSetter _accessTokenSetter;

        #endregion

        #region Constructor

        public StartupViewModel(IOptions<OnlineOptions> onlineOptions, IOptions<OfflineOptions> offlineOptions, IOptions<SecurityOptions> securityOptions, IOptions<AppOptions> appOptions, IApplicationDataProvider applicationDataProvider, IAccessTokenSetter accessTokenSetter)
        {
            NullGuard.NotNull(onlineOptions, nameof(onlineOptions))
                .NotNull(offlineOptions, nameof(offlineOptions))
                .NotNull(securityOptions, nameof(securityOptions))
                .NotNull(appOptions, nameof(appOptions))
                .NotNull(applicationDataProvider, nameof(applicationDataProvider))
                .NotNull(accessTokenSetter, nameof(accessTokenSetter));

            _onlineOptions = onlineOptions;
            _offlineOptions = offlineOptions;
            _securityOptions = securityOptions;
            _appOptions = appOptions;
            _applicationDataProvider = applicationDataProvider;
            _accessTokenSetter = accessTokenSetter;
        }

        #endregion

        #region Properties

        public bool Initializing
        {
            get => _initializing;
            set
            {
                if (_initializing == value)
                {
                    return;
                }

                _initializing = value;

                RaisePropertyChanged();
            }
        }

        public bool Initialized
        {
            get => _initialized;
            set
            {
                if (_initialized == value)
                {
                    return;
                }

                _initialized = value;

                RaisePropertyChanged();
            }
        }

        public AppOptions AppOptions => _appOptions.Value;

        #endregion

        #region Methods

        internal async Task InitializeAsync()
        {
            Initializing = true;
            try
            {
                await InitializeAsync(_onlineOptions.Value);
                await InitializeAsync(_offlineOptions.Value);
                await InitializeAsync(_securityOptions.Value);
                await InitializeAsync(_appOptions.Value);

                IAccessTokenModel accessTokenModel = await _applicationDataProvider.GetAccessTokenAsync();
                if (accessTokenModel != null)
                {
                    await _accessTokenSetter.SetAccessTokenAsync(accessTokenModel);
                }

                Initialized = true;
            }
            finally
            {
                Initializing = false;
            }
        }

        private async Task InitializeAsync(OnlineOptions onlineOptions)
        {
            NullGuard.NotNull(onlineOptions, nameof(onlineOptions));

            onlineOptions.ApiEndpoint = await _applicationDataProvider.GetApiEndpointAsync();
        }

        private async Task InitializeAsync(OfflineOptions offlineOptions)
        {
            NullGuard.NotNull(offlineOptions, nameof(offlineOptions));

            offlineOptions.OfflineDataDocument = await _applicationDataProvider.GetOfflineDataDocumentAsync();
        }

        private async Task InitializeAsync(SecurityOptions securityOptions)
        {
            NullGuard.NotNull(securityOptions, nameof(securityOptions));

            securityOptions.ApiEndpoint = await _applicationDataProvider.GetApiEndpointAsync();
            securityOptions.ClientId = await _applicationDataProvider.GetClientIdAsync();
            securityOptions.ClientSecret = await _applicationDataProvider.GetClientSecretAsync();
        }

        private async Task InitializeAsync(AppOptions appOptions)
        {
            NullGuard.NotNull(appOptions, nameof(appOptions));

            appOptions.ShouldOpenSettingsOnStartup = await _applicationDataProvider.GetShouldOpenSettingsOnStartupAsync();
        }

        #endregion
    }
}