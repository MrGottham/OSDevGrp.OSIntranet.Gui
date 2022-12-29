using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Gui.App.Core;
using OSDevGrp.OSIntranet.Gui.App.Settings;
using System.ComponentModel;

namespace OSDevGrp.OSIntranet.Gui.App;

public partial class App
{
    #region Private variabels

    private readonly Func<AppShell> _appShellGetter;
    private readonly Func<SettingsPage> _settingsPageGetter;
    private readonly Func<SettingsViewModel> _settingsViewModelGetter;

    #endregion

    #region Constructor

    public App(StartupViewModel startupViewModel, Func<StartupPage> startupPageGetter, Func<AppShell> appShellGetter, Func<SettingsPage> settingsPageGetter, Func<SettingsViewModel> settingsViewModelGetter)
    {
        NullGuard.NotNull(startupViewModel, nameof(startupViewModel))
            .NotNull(startupPageGetter, nameof(startupPageGetter))
            .NotNull(appShellGetter, nameof(appShellGetter))
            .NotNull(settingsPageGetter, nameof(settingsPageGetter))
            .NotNull(settingsViewModelGetter, nameof(settingsViewModelGetter));

        startupViewModel.PropertyChanged += StartupViewModelChanged;

        _appShellGetter = appShellGetter;
        _settingsPageGetter = settingsPageGetter;
        _settingsViewModelGetter = settingsViewModelGetter;

		InitializeComponent();

        MainPage = startupPageGetter();
    }

    #endregion

    #region Methods

    private void StartupViewModelChanged(object sender, PropertyChangedEventArgs eventArgs)
    {
        NullGuard.NotNull(sender, nameof(sender))
            .NotNull(eventArgs, nameof(eventArgs));

        switch (eventArgs.PropertyName)
        {
            case "Initialized":
                StartupViewModel startupViewModel = (StartupViewModel)sender;
                startupViewModel.PropertyChanged -= StartupViewModelChanged;

                AppOptions appOptions = startupViewModel.AppOptions;
                if (appOptions.ShouldOpenSettingsOnStartup)
                {
                    _settingsViewModelGetter.Invoke().PropertyChanged += SettingsViewModelChanged;

                    MainPage = _settingsPageGetter.Invoke();
                    break;
                }

                MainPage = _appShellGetter.Invoke();
                break;
        }
    }

    private void SettingsViewModelChanged(object sender, PropertyChangedEventArgs eventArgs)
    {
        NullGuard.NotNull(sender, nameof(sender))
            .NotNull(eventArgs, nameof(eventArgs));

        switch (eventArgs.PropertyName)
        {
            case "SettingsCommitted":
                SettingsViewModel settingsViewModel = (SettingsViewModel)sender;
                settingsViewModel.PropertyChanged -= SettingsViewModelChanged;

                MainPage = _appShellGetter.Invoke();
                break;
        }
    }

    #endregion
}