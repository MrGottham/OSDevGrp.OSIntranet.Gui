using Microsoft.Extensions.Logging;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Gui.App.Core;

namespace OSDevGrp.OSIntranet.Gui.App;

public partial class StartupPage
{
    #region Private variables

    private readonly ILogger<StartupPage> _logger;

    #endregion

    #region Constructor

    public StartupPage(StartupViewModel startupViewModel, ILogger<StartupPage> logger)
    {
        NullGuard.NotNull(startupViewModel, nameof(startupViewModel))
            .NotNull(logger, nameof(logger));

        _logger = logger;

        BindingContext = startupViewModel;

        InitializeComponent();
    }

    #endregion

    #region Methods

    protected override async void OnAppearing()
    {
        try
        {
            base.OnAppearing();

            await ((StartupViewModel)BindingContext).InitializeAsync();
        }
        catch (Exception ex)
        {
            await ex.HandleAsync(_logger, this);
        }
    }

    #endregion
}