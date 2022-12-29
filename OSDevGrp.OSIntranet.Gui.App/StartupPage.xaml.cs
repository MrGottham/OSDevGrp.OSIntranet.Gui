using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Gui.App.Core;

namespace OSDevGrp.OSIntranet.Gui.App;

public partial class StartupPage
{
    #region Constructor

    public StartupPage(StartupViewModel startupViewModel)
    {
        NullGuard.NotNull(startupViewModel, nameof(startupViewModel));

        BindingContext = startupViewModel;

		InitializeComponent();
    }

    #endregion

    #region Methods

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await ((StartupViewModel)BindingContext).InitializeAsync();
    }

    #endregion
}