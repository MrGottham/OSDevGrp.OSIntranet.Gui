using Microsoft.Extensions.Logging;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Gui.App.Core;
using System.Collections.Concurrent;
using System.ComponentModel;

namespace OSDevGrp.OSIntranet.Gui.App.Settings;

public partial class SettingsPage
{
    #region Private variables

    private readonly ILogger<SettingsPage> _logger;
    private readonly IDictionary<string, Func<SettingsViewModel, Task>> _settingsCommits = new ConcurrentDictionary<string, Func<SettingsViewModel, Task>>();

    #endregion

    #region Constructor

    public SettingsPage(SettingsViewModel settingsViewModel, ILogger<SettingsPage> logger)
    {
        NullGuard.NotNull(settingsViewModel, nameof(settingsViewModel))
            .NotNull(logger, nameof(logger));

        _logger = logger;

        _settingsCommits.Add("ApiEndpoint", viewModel => viewModel.CommitApiEndpointAsync());
        _settingsCommits.Add("ClientId", viewModel => viewModel.CommitClientIdAsync());
        _settingsCommits.Add("ClientSecret", viewModel => viewModel.CommitClientSecretAsync());

        settingsViewModel.PropertyChanged += SettingsViewModelChanged;

        BindingContext = settingsViewModel;

		InitializeComponent();
    }

    #endregion

    #region Methods

    private async void SettingsViewModelChanged(object sender, PropertyChangedEventArgs eventArgs)
    {
        NullGuard.NotNull(sender, nameof(sender))
            .NotNull(eventArgs, nameof(eventArgs));

        try
        {
            SettingsViewModel settingsViewModel = (SettingsViewModel)sender;
            switch (eventArgs.PropertyName)
            {
                case "CommandError":
                    await settingsViewModel.CommandError.HandleAsync(_logger, this);
                    return;
            }

            if (eventArgs.PropertyName == null || _settingsCommits.ContainsKey(eventArgs.PropertyName) == false)
            {
                return;
            }

            await _settingsCommits[eventArgs.PropertyName].Invoke(settingsViewModel);
        }
        catch (Exception ex)
        {
            await ex.HandleAsync(_logger, this);
        }
    }

    #endregion
}