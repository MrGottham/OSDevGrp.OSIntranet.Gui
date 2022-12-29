using Microsoft.Extensions.Options;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Gui.App.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core.Options;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security.Options;
using System.Windows.Input;

namespace OSDevGrp.OSIntranet.Gui.App.Settings
{
    public class SettingsViewModel : ViewModelBase
    {
        #region Private variables

        private string _apiEndpoint;
        private string _clientId;
        private string _clientSecret;
        private readonly IOptions<OnlineOptions> _onlineOptions;
        private readonly IOptions<SecurityOptions> _securityOptions;
        private readonly IOptions<AppOptions> _appOptions;
        private readonly IApplicationDataProvider _applicationDataProvider;

        #endregion

        #region Constructor

        public SettingsViewModel(IOptions<OnlineOptions> onlineOptions, IOptions<SecurityOptions> securityOptions, IOptions<AppOptions> appOptions, IApplicationDataProvider applicationDataProvider)
        {
            NullGuard.NotNull(onlineOptions, nameof(onlineOptions))
                .NotNull(securityOptions, nameof(securityOptions))
                .NotNull(appOptions, nameof(appOptions))
                .NotNull(applicationDataProvider, nameof(applicationDataProvider));

            _onlineOptions = onlineOptions;
            _securityOptions = securityOptions;
            _appOptions = appOptions;
            _applicationDataProvider = applicationDataProvider;

            _apiEndpoint = _onlineOptions.Value.ApiEndpoint.AbsoluteUri;
            _clientId = securityOptions.Value.ClientId;
            _clientSecret = securityOptions.Value.ClientSecret;

            CommitSettings = new Command(CommitAll, () => CanCommitSettings);
        }

        #endregion

        #region Properties

        public string ApiEndpoint
        {
            get => _apiEndpoint;
            set
            {
                NullGuard.NotNull(value, nameof(value));

                if (_apiEndpoint == value)
                {
                    return;
                }

                _apiEndpoint = value;

                RaisePropertyChanged();
            }
        }

        public string ClientId
        {
            get => _clientId;
            set
            {
                NullGuard.NotNull(value, nameof(value));

                if (_clientId == value)
                {
                    return;
                }

                _clientId = value;

                RaisePropertyChanged();
            }
        }

        public string ClientSecret
        {
            get => _clientSecret;
            set
            {
                NullGuard.NotNull(value, nameof(value));

                if (_clientSecret == value)
                {
                    return;
                }

                _clientSecret = value;

                RaisePropertyChanged();
            }
        }

        public string OfflineDataFile => _appOptions.Value.OfflineDataFile.FullName;

        public bool CanCommitSettings => _appOptions.Value.ShouldOpenSettingsOnStartup;

        public bool SettingsCommitted => CanCommitSettings == false;

        public ICommand CommitSettings { get; }

        #endregion

        #region Methods

        public async Task CommitApiEndpointAsync()
        {
            if (string.IsNullOrWhiteSpace(ApiEndpoint) || Uri.TryCreate(ApiEndpoint, UriKind.Absolute, out Uri apiEndpoint) == false)
            {
                return;
            }

            _onlineOptions.Value.ApiEndpoint = apiEndpoint;
            _securityOptions.Value.ApiEndpoint = apiEndpoint;

            await _applicationDataProvider.SetApiEndpointAsync(apiEndpoint);
        }

        public async Task CommitClientIdAsync()
        {
            if (string.IsNullOrWhiteSpace(ClientId))
            {
                return;
            }

            _securityOptions.Value.ClientId = ClientId;

            await _applicationDataProvider.SetClientIdAsync(ClientId);
        }

        public async Task CommitClientSecretAsync()
        {
            if (string.IsNullOrWhiteSpace(ClientSecret))
            {
                return;
            }

            _securityOptions.Value.ClientSecret = ClientSecret;

            await _applicationDataProvider.SetClientSecretAsync(ClientSecret);
        }

        private async void CommitAll()
        {
            await CommitApiEndpointAsync();
            await CommitClientIdAsync();
            await CommitClientSecretAsync();
            await CommitShouldOpenSettingsOnStartupAsync(false);
        }

        private async Task CommitShouldOpenSettingsOnStartupAsync(bool shouldOpenSettingsOnStartup)
        {
            _appOptions.Value.ShouldOpenSettingsOnStartup = shouldOpenSettingsOnStartup;

            await _applicationDataProvider.SetShouldOpenSettingsOnStartupAsync(shouldOpenSettingsOnStartup);

            RaisePropertyChanged(nameof(SettingsCommitted));
        }

        #endregion
    }
}