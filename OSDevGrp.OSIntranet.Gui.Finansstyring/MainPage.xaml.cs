using System;
using System.Diagnostics;
using OSDevGrp.OSIntranet.Gui.Finansstyring.Core;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Events;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
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
    public sealed partial class MainPage : IDisposable, IEventSubscriber<IHandleExceptionEventArgs>
    {
        #region Private variables

        private bool _disposed;
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
            MainViewModel.Subscribe(this);
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

        #region IDisposable members

        /// <summary>
        /// Destructor.
        /// </summary>
        ~MainPage()
        {
            Dispose(false);
        }

        /// <summary>
        /// Frigørelse af ressourcer.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Frigørelse af ressourcer.
        /// </summary>
        /// <param name="disposing">Angivelse af, om managed ressourcer også skal frigøres.</param>
        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            MainViewModel.Unsubscribe(this);
            if (disposing)
            {
            }
            _disposed = true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Eventhandler, der rejses, når eventet til håndtering af exceptions publiceres.
        /// </summary>
        /// <param name="eventArgs">Argumenter fra eventet, der publiceres.</param>
        public void OnEvent(IHandleExceptionEventArgs eventArgs)
        {
            if (eventArgs == null)
            {
                throw new ArgumentNullException("eventArgs");
            }

            var validationException = eventArgs.Error as IntranetGuiValidationException;
            if (validationException != null)
            {
                eventArgs.IsHandled = false;
                return;
            }

            var businessException = eventArgs.Error as IntranetGuiBusinessException;
            if (businessException != null)
            {
                eventArgs.IsHandled = false;
                return;
            }

            var repositoryException = eventArgs.Error as IntranetGuiRepositoryException;
            if (repositoryException != null)
            {
                Debug.WriteLine("{0}: {1}", repositoryException.GetType().Name, repositoryException.Message);
                eventArgs.IsHandled = true;
                return;
            }

            var systemException = eventArgs.Error as IntranetGuiSystemException;
            if (systemException != null)
            {
                Debug.WriteLine("{0}: {1}", systemException.GetType().Name, systemException.Message);
                eventArgs.IsHandled = true;
                return;
            }

            try
            {
                var exception = eventArgs.Error;
                if (exception == null)
                {
                    return;
                }
                Debug.WriteLine("{0}: {1}", exception.GetType().Name, exception.Message);
            }
            finally
            {
                eventArgs.IsHandled = true;
            }
        }

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

        /// <summary>
        /// Henter og returnerer valideringsfejl, der er sat på en given dependency property.
        /// </summary>
        /// <param name="dependencyObject">Objekt, som ejer dependency property.</param>
        /// <param name="dependencyProperty">Dependency property, hvorfra valideringsfejl skal hentes.</param>
        /// <returns>Valideringsfejl.</returns>
        public static string GetValidationErrorFromDependencyProperty(DependencyObject dependencyObject, DependencyProperty dependencyProperty)
        {
            if (dependencyObject == null)
            {
                throw new ArgumentNullException("dependencyObject");
            }
            if (dependencyProperty == null)
            {
                throw new ArgumentNullException("dependencyProperty");
            }
            return (string) dependencyObject.GetValue(dependencyProperty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dependencyObject"></param>
        /// <param name="dependencyProperty"></param>
        /// <param name="validationError"></param>
        public static void SetValidationErrorOnDependencyProperty(DependencyObject dependencyObject, DependencyProperty dependencyProperty, string validationError)
        {
            if (dependencyObject == null)
            {
                throw new ArgumentNullException("dependencyObject");
            }
            if (dependencyProperty == null)
            {
                throw new ArgumentNullException("dependencyProperty");
            }
            var newValue = validationError;
            if (string.IsNullOrWhiteSpace(newValue))
            {
                newValue = string.Empty;
            }
            if (string.Compare(newValue, GetValidationErrorFromDependencyProperty(dependencyObject, dependencyProperty), StringComparison.Ordinal) == 0)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(newValue))
            {
                dependencyObject.ClearValue(dependencyProperty);
                return;
            }
            dependencyObject.SetValue(dependencyProperty, newValue);
        }

        #endregion
    }
}
