using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Callisto.Controls;
using OSDevGrp.OSIntranet.Gui.Finansstyring.Core;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.Runtime;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace OSDevGrp.OSIntranet.Gui.Finansstyring
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App
    {
        #region Private variables

        private IMainViewModel _mainViewModel;
        private IExceptionHandlerViewModel _exceptionHandlerViewModel;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;

            // TODO: Can go back.
            // TODO: Panes.
            // TODO: Search.
        }

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            var rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();
                // Associate the frame with a SuspensionManager key
                SuspensionManager.RegisterFrame(rootFrame, "FinansstyringAppRootFrame");

                _mainViewModel = (IMainViewModel)Resources["MainViewModel"];
                _mainViewModel.ApplyConfiguration(ConfigurationProvider.Instance.Settings);

                _exceptionHandlerViewModel = _mainViewModel.ExceptionHandler;

                Current.UnhandledException += UnhandledExceptionEventHandler;
                TaskScheduler.UnobservedTaskException += UnobservedTaskExceptionEventHandler;

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // Restore the saved session state only when appropritate
                    try
                    {
                        await SuspensionManager.RestoreAsync();
                    }
                    catch (SuspensionManagerException)
                    {
                        //Something went wrong restoring state.
                        //Assume there is no state and continue
                    }
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(MainPage), args.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when the application creates a window.
        /// </summary>
        /// <param name="args">Event data for the event.</param>
        protected override void OnWindowCreated(WindowCreatedEventArgs args)
        {
            SettingsPane.GetForCurrentView().CommandsRequested += SettingsCommandsRequestedEventHandler;
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

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private static async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }

        /// <summary>
        /// Occurs when the user opens the settings pane.
        /// Listening for this event lets the app initialize the setting commands and pause its UI until the user closes the pane.
        /// </summary>
        /// <param name="settingsPane">The event source.</param>
        /// <param name="eventArgs">The event data. If there is no event data, this parameter will be null.</param>
        private static void SettingsCommandsRequestedEventHandler(SettingsPane settingsPane, SettingsPaneCommandsRequestedEventArgs eventArgs)
        {
            if (settingsPane == null)
            {
                throw new ArgumentNullException("settingsPane");
            }
            if (eventArgs == null)
            {
                throw new ArgumentNullException("eventArgs");
            }
            var configurationCommand = new SettingsCommand("configuration", Resource.GetText(Text.Configuration), eventHandler =>
                {
                    var settingsFlyout = new SettingsFlyout
                        {
                            FlyoutWidth = SettingsFlyout.SettingsFlyoutWidth.Narrow,
                            HeaderText = Resource.GetText(Text.Configuration), 
                            HeaderBrush = new SolidColorBrush(Colors.DarkBlue),
                            Content = new ConfigurationUserControl(),
                            IsOpen = true
                        };
                    settingsFlyout.IsOpen = true;
                });
            eventArgs.Request.ApplicationCommands.Add(configurationCommand);
        }

        #endregion
    }
}
