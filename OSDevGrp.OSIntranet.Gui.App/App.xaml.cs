using Microsoft.Extensions.Logging;
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
    private readonly ILogger<App> _logger;

    #endregion

    #region Constructor

    public App(StartupViewModel startupViewModel, Func<StartupPage> startupPageGetter, Func<AppShell> appShellGetter, Func<SettingsPage> settingsPageGetter, Func<SettingsViewModel> settingsViewModelGetter, ILogger<App> logger)
    {
        NullGuard.NotNull(startupViewModel, nameof(startupViewModel))
            .NotNull(startupPageGetter, nameof(startupPageGetter))
            .NotNull(appShellGetter, nameof(appShellGetter))
            .NotNull(settingsPageGetter, nameof(settingsPageGetter))
            .NotNull(settingsViewModelGetter, nameof(settingsViewModelGetter))
            .NotNull(logger, nameof(logger));

        try
        {
            startupViewModel.PropertyChanged += StartupViewModelChanged;

            _appShellGetter = appShellGetter;
            _settingsPageGetter = settingsPageGetter;
            _settingsViewModelGetter = settingsViewModelGetter;
            _logger = logger;

            InitializeComponent();

            MainPage = startupPageGetter();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.ToString());
            throw;
        }
    }

    #endregion

    #region Methods

    private async void StartupViewModelChanged(object sender, PropertyChangedEventArgs eventArgs)
    {
        NullGuard.NotNull(sender, nameof(sender))
            .NotNull(eventArgs, nameof(eventArgs));

        try
        {
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
        catch (Exception ex)
        {
            await ex.HandleAsync(_logger);
        }
    }

    private async void SettingsViewModelChanged(object sender, PropertyChangedEventArgs eventArgs)
    {
        NullGuard.NotNull(sender, nameof(sender))
            .NotNull(eventArgs, nameof(eventArgs));

        try
        {
            switch (eventArgs.PropertyName)
            {
                case "AllSettingsCommitted":
                    SettingsViewModel settingsViewModel = (SettingsViewModel)sender;
                    settingsViewModel.PropertyChanged -= SettingsViewModelChanged;

                    MainPage = _appShellGetter.Invoke();
                    break;
            }
        }
        catch (Exception ex)
        {
            await ex.HandleAsync(_logger);
        }
    }

    #endregion
}