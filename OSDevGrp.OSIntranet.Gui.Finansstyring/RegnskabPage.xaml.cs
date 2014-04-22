using System;
using System.ComponentModel;
using System.Linq;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Events;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace OSDevGrp.OSIntranet.Gui.Finansstyring
{
    /// <summary>
    /// Page til et regnskab.
    /// </summary>
    public sealed partial class RegnskabPage : IDisposable, IEventSubscriber<IHandleExceptionEventArgs>
    {
        #region Private variables

        private bool _regnskabPropertyChangedIsSet;
        private bool _bogføringPropertyChangedIsSet;
        private IBogføringViewModel _lastBogføringViewModel;
        private bool _disposed;
        private static readonly DependencyProperty MainViewModelProperty = DependencyProperty.Register("MainViewModel", typeof (IMainViewModel), typeof (RegnskabPage), new PropertyMetadata(null));
        private static readonly DependencyProperty RegnskabProperty = DependencyProperty.Register("Regnskab", typeof (IRegnskabViewModel), typeof (RegnskabPage), new PropertyMetadata(null));
        private static readonly DependencyProperty RegnskabslisteProperty = DependencyProperty.Register("Regnskabsliste", typeof (IRegnskabslisteViewModel), typeof (RegnskabPage), new PropertyMetadata(null));
        private static readonly DependencyProperty DatoValidationErrorProperty = DependencyProperty.Register("DatoValidationError", typeof (string), typeof (RegnskabPage), new PropertyMetadata(string.Empty));
        private static readonly DependencyProperty BilagValidationErrorProperty = DependencyProperty.Register("BilagValidationError", typeof (string), typeof (RegnskabPage), new PropertyMetadata(string.Empty));
        private static readonly DependencyProperty KontonummerValidationErrorProperty = DependencyProperty.Register("KontonummerValidationError", typeof (string), typeof (RegnskabPage), new PropertyMetadata(string.Empty));
        private static readonly DependencyProperty TekstValidationErrorProperty = DependencyProperty.Register("TekstValidationError", typeof (string), typeof (RegnskabPage), new PropertyMetadata(string.Empty));
        private static readonly DependencyProperty BudgetkontonummerValidationErrorProperty = DependencyProperty.Register("BudgetkontonummerValidationError", typeof (string), typeof (RegnskabPage), new PropertyMetadata(string.Empty));
        private static readonly DependencyProperty DebitValidationErrorProperty = DependencyProperty.Register("DebitValidationError", typeof (string), typeof (RegnskabPage), new PropertyMetadata(string.Empty));
        private static readonly DependencyProperty KreditValidationErrorProperty = DependencyProperty.Register("KreditValidationError", typeof (string), typeof (RegnskabPage), new PropertyMetadata(string.Empty));
        private static readonly DependencyProperty AdressekontoValidationErrorProperty = DependencyProperty.Register("AdressekontoValidationError", typeof(string), typeof(RegnskabPage), new PropertyMetadata(string.Empty));

        #endregion

        #region Constructor

        /// <summary>
        /// Danner en Page til et regnskab.
        /// </summary>
        public RegnskabPage()
        {
            InitializeComponent();

            MainViewModel = (IMainViewModel) Application.Current.Resources["MainViewModel"];
            MainViewModel.Subscribe(this);

            Regnskabsliste = MainViewModel.Regnskabsliste;

            SizeChanged += PageSizeChangedEventHandler;
        }

        #endregion

        #region Properties

        /// <summary>
        /// MainViewModel, der skal benyttes til konfiguration.
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
                if (Regnskab != null)
                {
                    if (Regnskab.Bogføring != null && _bogføringPropertyChangedIsSet)
                    {
                        Regnskab.Bogføring.PropertyChanged -= PropertyChangedOnBogføringViewModelEventHandler;
                        _bogføringPropertyChangedIsSet = false;
                    }
                    if (_regnskabPropertyChangedIsSet)
                    {
                        Regnskab.PropertyChanged -= PropertyChangedOnRegnskabViewModelEventHandler;
                        _regnskabPropertyChangedIsSet = false;
                    }
                }
                DatoValidationError = string.Empty;
                BilagValidationError = string.Empty;
                KontonummerValidationError = string.Empty;
                TekstValidationError = string.Empty;
                BudgetkontonummerValidationError = string.Empty;
                DebitValidationError = string.Empty;
                KreditValidationError = string.Empty;
                AdressekontoValidationError = string.Empty;
                SetValue(RegnskabProperty, value);
                if (Regnskab == null)
                {
                    return;
                }
                if (Regnskab.Bogføring != null && _bogføringPropertyChangedIsSet == false)
                {
                    Regnskab.Bogføring.PropertyChanged += PropertyChangedOnBogføringViewModelEventHandler;
                    _bogføringPropertyChangedIsSet = true;
                }
                if (_regnskabPropertyChangedIsSet)
                {
                    return;
                }
                Regnskab.PropertyChanged += PropertyChangedOnRegnskabViewModelEventHandler;
                _regnskabPropertyChangedIsSet = true;
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

        /// <summary>
        /// Valideringsfejl for bogføringsdato.
        /// </summary>
        public string DatoValidationError
        {
            get
            {
                return MainPage.GetValidationErrorFromDependencyProperty(this, DatoValidationErrorProperty);
            }
            private set
            {
                MainPage.SetValidationErrorOnDependencyProperty(this, DatoValidationErrorProperty, value);
            }
        }

        /// <summary>
        /// Valideringsfejl for bilagsnummer.
        /// </summary>
        public string BilagValidationError
        {
            get
            {
                return MainPage.GetValidationErrorFromDependencyProperty(this, BilagValidationErrorProperty);
            }
            private set
            {
                MainPage.SetValidationErrorOnDependencyProperty(this, BilagValidationErrorProperty, value);
            }
        }

        /// <summary>
        /// Valideringsfejl for kontonummer.
        /// </summary>
        public string KontonummerValidationError
        {
            get
            {
                return MainPage.GetValidationErrorFromDependencyProperty(this, KontonummerValidationErrorProperty);
            }
            private set
            {
                MainPage.SetValidationErrorOnDependencyProperty(this, KontonummerValidationErrorProperty, value);
            }
        }

        /// <summary>
        /// Valideringsfejl for teksten til bogføringslinjen.
        /// </summary>
        public string TekstValidationError
        {
            get
            {
                return MainPage.GetValidationErrorFromDependencyProperty(this, TekstValidationErrorProperty);
            }
            private set
            {
                MainPage.SetValidationErrorOnDependencyProperty(this, TekstValidationErrorProperty, value);
            }
        }

        /// <summary>
        /// Valideringsfejl for kontonummer på budgetkonto.
        /// </summary>
        public string BudgetkontonummerValidationError
        {
            get
            {
                return MainPage.GetValidationErrorFromDependencyProperty(this, BudgetkontonummerValidationErrorProperty);
            }
            private set
            {
                MainPage.SetValidationErrorOnDependencyProperty(this, BudgetkontonummerValidationErrorProperty, value);
            }
        }

        /// <summary>
        /// Valideringsfejl for debitbeløb.
        /// </summary>
        public string DebitValidationError
        {
            get
            {
                return MainPage.GetValidationErrorFromDependencyProperty(this, DebitValidationErrorProperty);
            }
            private set
            {
                MainPage.SetValidationErrorOnDependencyProperty(this, DebitValidationErrorProperty, value);
            }
        }

        /// <summary>
        /// Valideringsfejl for kreditbeløb.
        /// </summary>
        public string KreditValidationError
        {
            get
            {
                return MainPage.GetValidationErrorFromDependencyProperty(this, KreditValidationErrorProperty);
            }
            private set
            {
                MainPage.SetValidationErrorOnDependencyProperty(this, KreditValidationErrorProperty, value);
            }
        }

        /// <summary>
        /// Valideringsfejl for adressekonto.
        /// </summary>
        public string AdressekontoValidationError
        {
            get
            {
                return MainPage.GetValidationErrorFromDependencyProperty(this, AdressekontoValidationErrorProperty);
            }
            private set
            {
                MainPage.SetValidationErrorOnDependencyProperty(this, AdressekontoValidationErrorProperty, value);
            }
        }

        #endregion

        #region IDisposable members

        /// <summary>
        /// Destructor.
        /// </summary>
        ~RegnskabPage()
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
            if (Regnskab != null)
            {
                if (Regnskab.Bogføring != null && _bogføringPropertyChangedIsSet)
                {
                    Regnskab.Bogføring.PropertyChanged -= PropertyChangedOnBogføringViewModelEventHandler;
                    _bogføringPropertyChangedIsSet = false;
                }
                if (_regnskabPropertyChangedIsSet)
                {
                    Regnskab.PropertyChanged -= PropertyChangedOnRegnskabViewModelEventHandler;
                    _regnskabPropertyChangedIsSet = false;
                }
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
        /// <param name="handleExceptionEventArgs">Argumenter fra eventet, der publiceres.</param>
        public void OnEvent(IHandleExceptionEventArgs handleExceptionEventArgs)
        {
            if (handleExceptionEventArgs == null)
            {
                throw new ArgumentNullException("handleExceptionEventArgs");
            }
            // Håndtering af kommandoexception.
            var commandException = handleExceptionEventArgs.Error as IntranetGuiCommandException;
            if (commandException != null)
            {
                var commandContext = commandException.CommandContext as BogføringAddCommand;
                if (commandContext == null)
                {
                    return;
                }
                try
                {
                    return;
                }
                finally
                {
                    handleExceptionEventArgs.IsHandled = true;
                }
            }
            // Håndtering af valideringsexception.
            var validationException = handleExceptionEventArgs.Error as IntranetGuiValidationException;
            if (validationException == null)
            {
                return;
            }
            var validationContext = validationException.ValidationContext as IBogføringViewModel;
            if (validationContext == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(validationException.PropertyName) || string.IsNullOrEmpty(validationException.Message))
            {
                return;
            }
            try
            {
                switch (validationException.PropertyName)
                {
                    case "Dato":
                    case "DatoAsText":
                        if (string.IsNullOrWhiteSpace(DatoValidationError) == false)
                        {
                            DatoValidationError = string.Empty;
                        }
                        DatoValidationError = validationException.Message;
                        break;

                    case "Bilag":
                        if (string.IsNullOrWhiteSpace(BilagValidationError) == false)
                        {
                            BilagValidationError = string.Empty;
                        }
                        BilagValidationError = validationException.Message;
                        break;

                    case "Kontonummer":
                        if (string.IsNullOrWhiteSpace(KontonummerValidationError) == false)
                        {
                            KontonummerValidationError = string.Empty;
                        }
                        KontonummerValidationError = validationException.Message;
                        break;

                    case "Tekst":
                        if (string.IsNullOrWhiteSpace(TekstValidationError) == false)
                        {
                            TekstValidationError = string.Empty;
                        }
                        TekstValidationError = validationException.Message;
                        break;

                    case "Budgetkontonummer":
                        if (string.IsNullOrWhiteSpace(BudgetkontonummerValidationError) == false)
                        {
                            BudgetkontonummerValidationError = string.Empty;
                        }
                        BudgetkontonummerValidationError = validationException.Message;
                        break;

                    case "Debit":
                    case "DebitAsText":
                        if (string.IsNullOrWhiteSpace(DebitValidationError) == false)
                        {
                            DebitValidationError = string.Empty;
                        }
                        DebitValidationError = validationException.Message;
                        break;

                    case "Kredit":
                    case "KreditAsText":
                        if (string.IsNullOrWhiteSpace(KreditValidationError) == false)
                        {
                            KreditValidationError = string.Empty;
                        }
                        KreditValidationError = validationException.Message;
                        break;

                    case "Adressekonto":
                        if (string.IsNullOrWhiteSpace(AdressekontoValidationError) == false)
                        {
                            AdressekontoValidationError = string.Empty;
                        }
                        AdressekontoValidationError = validationException.Message;
                        break;
                }
            }
            finally
            {
                handleExceptionEventArgs.IsHandled = true;
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
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
            Frame.Navigate(typeof (RegnskabslistePage), null);
        }

        /// <summary>
        /// Eventhandler, der håndterer aktivering/visning af en kontoplan til regnskabet.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        private void KontoplanButtonClickEventHandler(object sender, RoutedEventArgs eventArgs)
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
            Frame.Navigate(typeof (KontoplanPage), regnskabViewModel.Nummer);
        }

        /// <summary>
        /// Eventhandler, der håndterer aktivering/visning af en budgetkontoplan til regnskabet.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        private void BudgetkontoplanButtonClickEventHandler(object sender, RoutedEventArgs eventArgs)
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
            Frame.Navigate(typeof (BudgetkontoplanPage), regnskabViewModel.Nummer);
        }

        /// <summary>
        /// Eventhandler, der rejses, når en property ændres på ViewModel for regnskabet.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        private void PropertyChangedOnRegnskabViewModelEventHandler(object sender, PropertyChangedEventArgs eventArgs)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            if (eventArgs == null)
            {
                throw new ArgumentNullException();
            }
            var regnskabViewModel = sender as IRegnskabViewModel;
            if (regnskabViewModel == null)
            {
                return;
            }
            switch (eventArgs.PropertyName)
            {
                case "Bogføring":
                    if (_lastBogføringViewModel != null && _lastBogføringViewModel != regnskabViewModel.Bogføring && _bogføringPropertyChangedIsSet)
                    {
                        _lastBogføringViewModel.PropertyChanged -= PropertyChangedOnBogføringViewModelEventHandler;
                        _bogføringPropertyChangedIsSet = false;
                    }
                    _lastBogføringViewModel = regnskabViewModel.Bogføring;
                    if (regnskabViewModel.Bogføring != null && _bogføringPropertyChangedIsSet == false)
                    {
                        regnskabViewModel.Bogføring.PropertyChanged += PropertyChangedOnBogføringViewModelEventHandler;
                        _bogføringPropertyChangedIsSet = true;
                    }
                    break;
            }
        }

        /// <summary>
        /// Eventhandler, der rejses, når en property ændres på ViewModel til bogføring.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        private void PropertyChangedOnBogføringViewModelEventHandler(object sender, PropertyChangedEventArgs eventArgs)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            if (eventArgs == null)
            {
                throw new ArgumentNullException("eventArgs");
            }
            switch (eventArgs.PropertyName)
            {
                case "Dato":
                case "DatoAsText":
                    if (string.IsNullOrWhiteSpace(""))
                    {
                        break;
                    }
                    DatoValidationError = string.Empty;
                    break;

                case "Bilag":
                    BilagValidationError = string.Empty;
                    break;

                case "Kontonummer":
                    KontonummerValidationError = string.Empty;
                    break;

                case "Tekst":
                    TekstValidationError = string.Empty;
                    break;

                case "Budgetkontonummer":
                    BudgetkontonummerValidationError = string.Empty;
                    break;

                case "Debit":
                case "DebitAsText":
                    DebitValidationError = string.Empty;
                    break;

                case "Kredit":
                case "KreditAsText":
                    KreditValidationError = string.Empty;
                    break;

                case "Adressekonto":
                    AdressekontoValidationError = string.Empty;
                    break;
            }
        }

        #endregion
    }
}
