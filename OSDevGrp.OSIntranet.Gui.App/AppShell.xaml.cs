using Microsoft.Extensions.Logging;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Gui.App.Core;
using OSDevGrp.OSIntranet.Gui.App.Features;

namespace OSDevGrp.OSIntranet.Gui.App;

public partial class AppShell : IDisposable
{
	#region Private variables

    private bool _disposed;
    private readonly IEnumerable<IBackgroundFeature> _backgroundFeatures;
    private readonly ILogger<AppShell> _logger;

    #endregion

	#region Constructor

	public AppShell(AppShellViewModel appShellViewModel, IEnumerable<IBackgroundFeature> backgroundFeatures, ILogger<AppShell> logger)
    {
        NullGuard.NotNull(appShellViewModel, nameof(appShellViewModel))
            .NotNull(backgroundFeatures, nameof(backgroundFeatures))
            .NotNull(logger, nameof(logger));

        _backgroundFeatures = backgroundFeatures;
        _logger = logger;

        BindingContext = appShellViewModel;

        InitializeComponent();
    }

    #endregion

    #region Methods

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected override async void OnAppearing()
    {
        try
        {
            base.OnAppearing();

            await Task.WhenAll(_backgroundFeatures.Select(backgroundFeature => backgroundFeature.StartAsync()).ToArray());
        }
        catch (Exception ex)
        {
            await ex.HandleAsync(_logger, this);
        }
    }

    protected override async void OnDisappearing()
    {
        try
        {
            base.OnDisappearing();

            await Task.WhenAll(_backgroundFeatures.Select(backgroundFeature => backgroundFeature.StopAsync()).ToArray());
        }
        catch (Exception ex)
        {
            await ex.HandleAsync(_logger, this);
        }
    }

    private void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            foreach (IBackgroundFeature backgroundFeature in _backgroundFeatures)
            {
                backgroundFeature.Dispose();
            }
        }

        _disposed = true;
    }

    #endregion
}