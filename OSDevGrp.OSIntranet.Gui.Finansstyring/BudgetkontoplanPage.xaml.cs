﻿using System;
using System.Linq;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace OSDevGrp.OSIntranet.Gui.Finansstyring
{
    /// <summary>
    /// Page til en budgetkontoplan for et givent regnskab.
    /// </summary>
    public sealed partial class BudgetkontoplanPage
    {
        #region Private variables

        private static readonly DependencyProperty RegnskabProperty = DependencyProperty.Register("Regnskab", typeof(IRegnskabViewModel), typeof(BudgetkontoplanPage), new PropertyMetadata(null));
        private static readonly DependencyProperty RegnskabslisteProperty = DependencyProperty.Register("Regnskabsliste", typeof(IRegnskabslisteViewModel), typeof(BudgetkontoplanPage), new PropertyMetadata(null));

        #endregion

        #region Constructor

        /// <summary>
        /// Danner en page til en budgetkontoplan for et givent regnskab.
        /// </summary>
        public BudgetkontoplanPage()
        {
            InitializeComponent();

            var mainViewModel = (IMainViewModel) Application.Current.Resources["MainViewModel"];
            Regnskabsliste = mainViewModel.Regnskabsliste;

            SizeChanged += PageSizeChangedEventHandler;
        }

        #endregion

        #region Properties

        /// <summary>
        /// ViewModel for regnskabet.
        /// </summary>
        public IRegnskabViewModel Regnskab
        {
            get
            {
                return GetValue(RegnskabProperty) as IRegnskabViewModel;
            }
            private set
            {
                SetValue(RegnskabProperty, value);
            }
        }

        /// <summary>
        /// ViewModel for regnskabslisten.
        /// </summary>
        public IRegnskabslisteViewModel Regnskabsliste
        {
            get
            {
                return GetValue(RegnskabslisteProperty) as IRegnskabslisteViewModel;
            }
            private set
            {
                SetValue(RegnskabslisteProperty, value);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }
            if (e.Parameter == null)
            {
                return;
            }

            DataContext = null;
            DefaultNavigationContent.Visibility = Visibility.Collapsed;
            DefaultNavigationProgressBar.IsIndeterminate = true;
            DefaultNavigationProgressBar.Visibility = Visibility.Visible;
            MinimalNavigationContent.Visibility = Visibility.Collapsed;
            MinimalNavigationProgressBar.IsIndeterminate = true;
            MinimalNavigationProgressBar.Visibility = Visibility.Visible;

            CommandAppBar.DataContext = null;
            CommandAppBar.Visibility = Visibility.Collapsed;

            var regnskabsnummer = (int) e.Parameter;
            Regnskab = await Regnskabsliste.RegnskabGetAsync(regnskabsnummer);
            try
            {
                if (Regnskab == null)
                {
                    return;
                }
                if (Regnskabsliste.Regnskaber.Any(m => m.Nummer == Regnskab.Nummer) == false)
                {
                    Regnskabsliste.RegnskabAdd(Regnskab);
                }

                DefaultNavigationContent.Visibility = Visibility.Visible;
                MinimalNavigationContent.Visibility = Visibility.Visible;
                CommandAppBar.Visibility = Visibility.Visible;

                var refreshCommand = Regnskab.RefreshCommand;
                if (refreshCommand == null)
                {
                    return;
                }
                refreshCommand.Execute(Regnskab);
            }
            finally
            {
                DataContext = Regnskab;
                CommandAppBar.DataContext = Regnskab;

                DefaultNavigationProgressBar.IsIndeterminate = false;
                DefaultNavigationProgressBar.Visibility = Visibility.Collapsed;
                MinimalNavigationProgressBar.IsIndeterminate = false;
                MinimalNavigationProgressBar.Visibility = Visibility.Collapsed;
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
        /// Eventhandler, der håndterer aktivering af tilbageknappen.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        private void BackButtonClickEventHandler(object sender, RoutedEventArgs eventArgs)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            if (eventArgs == null)
            {
                throw new ArgumentNullException("eventArgs");
            }
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
                return;
            }
            Frame.Navigate(typeof(RegnskabslistePage), null);
        }

        #endregion
    }
}
