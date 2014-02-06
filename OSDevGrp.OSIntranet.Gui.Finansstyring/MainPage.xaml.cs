using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Gui.Runtime;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
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

        private readonly IExceptionHandlerViewModel _exceptionHandlerViewModel;
        private readonly IRegnskabslisteViewModel _regnskabslisteViewModel;
        
        #endregion

        #region Constructor

        /// <summary>
        /// Danner MainPage til Finansstyring.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();

            var mainViewModel = (IMainViewModel) Resources["MainViewModel"];
            mainViewModel.ApplyConfiguration(ConfigurationProvider.Instance.Settings);

            _exceptionHandlerViewModel = (IExceptionHandlerViewModel) ExceptionHandlerAppBar.DataContext;
            _regnskabslisteViewModel = (IRegnskabslisteViewModel) ((Grid) Content).DataContext;
            
            Application.Current.UnhandledException += UnhandledExceptionEventHandler;
            TaskScheduler.UnobservedTaskException += UnobservedTaskExceptionEventHandler;

            SizeChanged += PageSizeChangedEventHandler;

            RegisterBackgroundTasks();
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
            if (_regnskabslisteViewModel.Regnskaber.Any())
            {
                return;
            }
            var refreshCommand = _regnskabslisteViewModel.RefreshCommand;
            if (refreshCommand.CanExecute(_regnskabslisteViewModel))
            {
                refreshCommand.Execute(_regnskabslisteViewModel);
            }
        }

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
            TopAppBar.IsOpen = _exceptionHandlerViewModel.ShowLast;
        }

        /// <summary>
        /// Eventhandler, der håndterer uhåndterede exceptions fra applikationen.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        private void UnhandledExceptionEventHandler(object sender, UnhandledExceptionEventArgs eventArgs)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            if (eventArgs == null)
            {
                throw new ArgumentNullException("eventArgs");
            }
            try
            {
                HandleException(eventArgs.Exception);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("UnhandledExceptionEventHandler: {0}", ex.Message);
            }
            finally
            {
                eventArgs.Handled = true;
            }
        }

        /// <summary>
        /// Eventhandler, der håndterer uhåndterede exceptions fra tasks.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        private void UnobservedTaskExceptionEventHandler(object sender, UnobservedTaskExceptionEventArgs eventArgs)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            if (eventArgs == null)
            {
                throw new ArgumentNullException("eventArgs");
            }
            try
            {
                HandleException(eventArgs.Exception.InnerException ?? eventArgs.Exception);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("UnobservedTaskExceptionEventHandler: {0}", ex.Message);
            }
            finally
            {
                eventArgs.SetObserved();
            }
        }

        /// <summary>
        /// Eventhandler, der håndterer ændring af størrelse på siden.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        private void PageSizeChangedEventHandler(object sender, SizeChangedEventArgs eventArgs)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            if (eventArgs == null)
            {
                throw new ArgumentNullException("eventArgs");
            }
            if (eventArgs.NewSize.Width < 500)
            {
                VisualStateManager.GoToState(this, "MinimalLayout", true);
                return;
            }
            VisualStateManager.GoToState(this, "DefaultLayout", true);
        }

        /// <summary>
        /// Eventhandler, der håndterer aktivering/visning af et regnskab.
        /// </summary>
        /// <param name="sender">Object, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        private void RegnskabButtonClickEventHandler(object sender, RoutedEventArgs eventArgs)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            if (eventArgs == null)
            {
                throw new ArgumentNullException("eventArgs");
            }
            var hyperlinkButton = sender as HyperlinkButton;
            if (hyperlinkButton == null)
            {
                return;
            }
            var regnskabViewModel = hyperlinkButton.Tag as IRegnskabViewModel;
            if (regnskabViewModel == null)
            {
                return;
            }
            Frame.Navigate(typeof (RegnskabPage), new Tuple<IRegnskabViewModel, IExceptionHandlerViewModel>(regnskabViewModel, _exceptionHandlerViewModel));
        }

        /// <summary>
        /// Håndtering af exceptions.
        /// </summary>
        /// <param name="exception">Exception, der skal håndteres.</param>
        private void HandleException(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }
            try
            {
                _exceptionHandlerViewModel.HandleException(exception);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("HandleException: {0}", ex.Message);
            }
        }

        #endregion
    }
}
