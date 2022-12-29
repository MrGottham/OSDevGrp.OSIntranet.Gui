using OSDevGrp.OSIntranet.Core;
using System.Collections.Concurrent;
using System.ComponentModel;

namespace OSDevGrp.OSIntranet.Gui.App.Settings;

public partial class SettingsPage
{
    #region Private variables

    private readonly IDictionary<string, Func<SettingsViewModel, Task>> _settingsCommits = new ConcurrentDictionary<string, Func<SettingsViewModel, Task>>();

    #endregion

    #region Constructor

    public SettingsPage(SettingsViewModel settingsViewModel)
    {
        NullGuard.NotNull(settingsViewModel, nameof(settingsViewModel));

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

        if (eventArgs.PropertyName == null || _settingsCommits.ContainsKey(eventArgs.PropertyName) == false)
        {
            return;
        }

        await _settingsCommits[eventArgs.PropertyName].Invoke((SettingsViewModel)sender);
    }

    #endregion
}