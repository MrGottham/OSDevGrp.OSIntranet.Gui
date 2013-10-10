﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

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

        #region Methods

        /// <summary>
        /// Danner MainPage til Finansstyring.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();

            _exceptionHandlerViewModel = (IExceptionHandlerViewModel) ExceptionHandlerAppBar.DataContext;
            _regnskabslisteViewModel = (IRegnskabslisteViewModel) ((Grid) Content).DataContext;
            
            Application.Current.UnhandledException += UnhandledExceptionEventHandler;
            TaskScheduler.UnobservedTaskException += UnobservedTaskExceptionEventHandler;
        }

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
                if (eventArgs.Observed)
                {
                    HandleException(eventArgs.Exception);
                    return;
                }
                HandleException(eventArgs.Exception.InnerException ?? eventArgs.Exception);
                eventArgs.SetObserved();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("UnobservedTaskExceptionEventHandler: {0}", ex.Message);
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

        #endregion
    }
}
