using System;
using OSDevGrp.OSIntranet.Gui.Finansstyring.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces;
using Windows.ApplicationModel.Background;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace OSDevGrp.OSIntranet.Gui.Finansstyring
{
    /// <summary>
    /// MainPage til Finansstyring.
    /// </summary>
    public sealed partial class MainPage
    {
        #region Private variables

        private static readonly DependencyProperty MainViewModelProperty = DependencyProperty.Register("MainViewModel", typeof (IMainViewModel), typeof (MainPage), new PropertyMetadata(null));

        #endregion

        #region Constructor

        /// <summary>
        /// Danner MainPage til Finansstyring.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            SuspensionManager.RegisterFrame(FinansstyringFrame, "FinansstyringAppSubFrame");

            MainViewModel = (IMainViewModel) Application.Current.Resources["MainViewModel"];
            DataContext = MainViewModel;

            RegisterBackgroundTasks();
        }

        #endregion

        #region Properties

        /// <summary>
        /// MainViewModel, der skal benyttes til Finansstyring.
        /// </summary>
        public IMainViewModel MainViewModel
        {
            get
            {
                return GetValue(MainViewModelProperty) as IMainViewModel;
            }
            private set
            {
                SetValue(MainViewModelProperty, value);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var contentPage = FinansstyringFrame.Content as Page;
            if (contentPage != null)
            {
                return;
            }
            FinansstyringFrame.Navigate(typeof (RegnskabslistePage), null);
        }

        /// <summary>
        /// Eventhandler, der håndterer åbning af applikationens TopAppBar.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        private void TopAppBarOpenedEventHandler(object sender, object eventArgs)
        {
            if (TopAppBar == null)
            {
                return;
            }
            TopAppBar.IsOpen = MainViewModel.ExceptionHandler.ShowLast;
        }

        /// <summary>
        /// Registrering af diverse baggrundstråde.
        /// </summary>
        private static async void RegisterBackgroundTasks()
        {
            var backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();
            if (backgroundAccessStatus != BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity && backgroundAccessStatus != BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity)
            {
                return;
            }
            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == "OSDevGrp.OSIntranet.Gui.Runtime.FinansstyringsnyhederLoaderBackgroundTask")
                {
                    task.Value.Unregister(true);
                }
            }

            var backgoundTaskBuilder = new BackgroundTaskBuilder
                {
                    Name = "OSDevGrp.OSIntranet.Gui.Runtime.FinansstyringsnyhederLoaderBackgroundTask",
                    TaskEntryPoint = "OSDevGrp.OSIntranet.Gui.Runtime.FinansstyringsnyhederLoaderBackgroundTask"
                };
            backgoundTaskBuilder.SetTrigger(new TimeTrigger(15, false));
            backgoundTaskBuilder.Register();
        }

        #endregion
    }
}
