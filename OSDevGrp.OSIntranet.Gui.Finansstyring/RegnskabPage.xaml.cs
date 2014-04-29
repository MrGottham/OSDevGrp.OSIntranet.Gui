using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Events;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Navigation;

namespace OSDevGrp.OSIntranet.Gui.Finansstyring
{
    /// <summary>
    /// Page til et regnskab.
    /// </summary>
    public sealed partial class RegnskabPage : IDisposable, IEventSubscriber<IHandleExceptionEventArgs>
    {
        #region Private variables

        private bool _disposed;
        private IBogføringViewModel _lastBogføringViewModel;
        private static readonly DependencyProperty MainViewModelProperty = DependencyProperty.Register("MainViewModel", typeof (IMainViewModel), typeof (RegnskabPage), new PropertyMetadata(null));
        private static readonly DependencyProperty RegnskabProperty = DependencyProperty.Register("Regnskab", typeof (IRegnskabViewModel), typeof (RegnskabPage), new PropertyMetadata(null));
        private static readonly DependencyProperty RegnskabslisteProperty = DependencyProperty.Register("Regnskabsliste", typeof (IRegnskabslisteViewModel), typeof (RegnskabPage), new PropertyMetadata(null));

        #endregion

        #region Constructor

        /// <summary>
        /// Danner en Page til et regnskab.
        /// </summary>
        public RegnskabPage()
        {
            InitializeComponent();

            MainViewModel = (IMainViewModel) Application.Current.Resources["MainViewModel"];

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
                if (MainViewModel != null)
                {
                    MainViewModel.Unsubscribe(this);
                }
                SetValue(MainViewModelProperty, value);
                if (MainViewModel == null)
                {
                    return;
                }
                MainViewModel.Subscribe(this);
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
                    if (Regnskab.Bogføring != null)
                    {
                        Regnskab.Bogføring.PropertyChanged -= PropertyChangedOnBogføringViewModelEventHandler;
                    }
                    Regnskab.PropertyChanged -= PropertyChangedOnRegnskabViewModelEventHandler;
                }
                SetValue(RegnskabProperty, value);
                if (Regnskab == null)
                {
                    return;
                }
                var adressekontoViewModelCollection = new ObservableCollection<IAdressekontoViewModel>
                    {
                        null
                    };
                Regnskab.PropertyChanged += PropertyChangedOnRegnskabViewModelEventHandler;
                try
                {
                    if (Regnskab.Bogføring == null)
                    {
                        return;
                    }
                    foreach (var adresseKontoViewModel in Regnskab.Bogføring.Adressekonti.Where(m => adressekontoViewModelCollection.Contains(m) == false))
                    {
                        adressekontoViewModelCollection.Add(adresseKontoViewModel);
                    }
                    Regnskab.Bogføring.ClearValidationErrors();
                    Regnskab.Bogføring.PropertyChanged += PropertyChangedOnBogføringViewModelEventHandler;
                }
                finally
                {
                    AdressekontiCollectionViewSource.Source = adressekontoViewModelCollection;
                    _lastBogføringViewModel = Regnskab.Bogføring;
                }
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
        /// Finder og returnerer CollectionViewSource til adressekonti.
        /// </summary>
        private CollectionViewSource AdressekontiCollectionViewSource
        {
            get
            {
                var mainGrid = (Grid) Content;
                return (CollectionViewSource) mainGrid.Resources["AdressekontiCollectionViewSource"];
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
                if (Regnskab.Bogføring != null)
                {
                    Regnskab.Bogføring.PropertyChanged -= PropertyChangedOnBogføringViewModelEventHandler;
                }
                Regnskab.PropertyChanged -= PropertyChangedOnRegnskabViewModelEventHandler;
            }
            if (MainViewModel != null)
            {
                MainViewModel.Unsubscribe(this);
            }
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
        public async void OnEvent(IHandleExceptionEventArgs handleExceptionEventArgs)
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
                    case "Bilag":
                    case "Kontonummer":
                    case "Tekst":
                    case "Budgetkontonummer":
                    case "Debit":
                    case "DebitAsText":
                    case "Kredit":
                    case "KreditAsText":
                    case "Adressekonto":
                        // Usernotification gives via behaviors.
                        break;

                    default:
                        var messageDialog = new MessageDialog(validationException.Message);
                        await messageDialog.ShowAsync();
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
        /// Eventhandler, der rejses, når en property på ViewModel for regnskabet ændres.
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
                throw new ArgumentNullException("eventArgs");
            }
            var regnskabViewModel = sender as IRegnskabViewModel;
            if (regnskabViewModel == null)
            {
                return;
            }
            switch (eventArgs.PropertyName)
            {
                case "Bogføring":
                    if (_lastBogføringViewModel != null)
                    {
                        _lastBogføringViewModel.PropertyChanged -= PropertyChangedOnBogføringViewModelEventHandler;
                    }
                    try
                    {
                        if (regnskabViewModel.Bogføring == null)
                        {
                            return;
                        }
                        var adressekontoViewModelCollection = (ObservableCollection<IAdressekontoViewModel>) AdressekontiCollectionViewSource.Source;
                        foreach (var adressekontoViewModel in regnskabViewModel.Bogføring.Adressekonti.Where(m => adressekontoViewModelCollection.Contains(m) == false))
                        {
                            adressekontoViewModelCollection.Add(adressekontoViewModel);
                        }
                        regnskabViewModel.Bogføring.ClearValidationErrors();
                        regnskabViewModel.Bogføring.PropertyChanged += PropertyChangedOnBogføringViewModelEventHandler;
                    }
                    finally
                    {
                        _lastBogføringViewModel = regnskabViewModel.Bogføring;
                    }
                    break;
            }
        }

        /// <summary>
        /// Eventhandler, der rejses, når en property på ViewModel til bogføring ændres.
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
            var bogføringViewModel = sender as IBogføringViewModel;
            if (bogføringViewModel == null)
            {
                return;
            }
            switch (eventArgs.PropertyName)
            {
                case "Adressekonti":
                    var adressekontoViewModelCollection = (ObservableCollection<IAdressekontoViewModel>) AdressekontiCollectionViewSource.Source;
                    foreach (var adressekontoViewModel in bogføringViewModel.Adressekonti.Where(m => adressekontoViewModelCollection.Contains(m) == false))
                    {
                        adressekontoViewModelCollection.Add(adressekontoViewModel);
                    }
                    break;
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
        /// Eventhandler, der håndterer, at tekst i en tekstboks er uppercase.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        private void UpperCaseTextBoxTextChangedEventHandler(object sender, TextChangedEventArgs eventArgs)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            if (eventArgs == null)
            {
                throw new ArgumentNullException("eventArgs");
            }
            var textBox = sender as TextBox;
            if (textBox == null)
            {
                return;
            }
            var value = textBox.Text;
            if (string.IsNullOrEmpty(value))
            {
                textBox.SelectionStart = 0;
                return;
            }
            textBox.Text = value.ToUpper();
            textBox.SelectionStart = value.Length;
        }

        /// <summary>
        /// Eventhandler, der håndterer, at tekst i tekstboksen til fremsøgning af adressekontoen ændres.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        private void AdressekontoSearchTextBoxTextChangedEventHandler(object sender, TextChangedEventArgs eventArgs)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            if (eventArgs == null)
            {
                throw new ArgumentNullException("eventArgs");
            }
            if (Regnskab.Bogføring == null)
            {
                return;
            }
            var textBox = sender as TextBox;
            if (textBox == null)
            {
                return;
            }
            var value = textBox.Text;
            var adressekontoViewModelCollection = (ObservableCollection<IAdressekontoViewModel>) AdressekontiCollectionViewSource.Source;
            if (string.IsNullOrWhiteSpace(value))
            {
                foreach (var adressekontoViewModel in Regnskab.Bogføring.Adressekonti.Where(m => adressekontoViewModelCollection.Contains(m) == false))
                {
                    adressekontoViewModelCollection.Add(adressekontoViewModel);
                }
                AdressekontiCollectionViewSource.View.MoveCurrentTo(null);
                return;
            }
            if (Regnskab.Bogføring.Adressekonto != 0)
            {
                AdressekontiCollectionViewSource.View.MoveCurrentTo(adressekontoViewModelCollection.FirstOrDefault(m => m != null && m.Nummer == Regnskab.Bogføring.Adressekonto));
                return;
            }
            foreach (var adressekontoViewModel in Regnskab.Bogføring.Adressekonti)
            {
                if (string.IsNullOrEmpty(adressekontoViewModel.Navn))
                {
                    if (adressekontoViewModelCollection.Contains(adressekontoViewModel))
                    {
                        adressekontoViewModelCollection.Remove(adressekontoViewModel);
                    }
                    continue;
                }
                if (adressekontoViewModel.Navn.Length < value.Length)
                {
                    if (adressekontoViewModelCollection.Contains(adressekontoViewModel))
                    {
                        adressekontoViewModelCollection.Remove(adressekontoViewModel);
                    }
                    continue;
                }
                if (string.Compare(adressekontoViewModel.Navn.Substring(0, value.Length), value, StringComparison.OrdinalIgnoreCase) != 0)
                {
                    if (adressekontoViewModelCollection.Contains(adressekontoViewModel))
                    {
                        adressekontoViewModelCollection.Remove(adressekontoViewModel);
                    }
                    continue;
                }
                if (adressekontoViewModelCollection.Contains(adressekontoViewModel))
                {
                    continue;
                }
                adressekontoViewModelCollection.Add(adressekontoViewModel);
            }



            //var value = textBox.Text;
            //var adressekonti = new List<IAdressekontoViewModel>
            //                {
            //                    null
            //                };
            //adressekonti.AddRange(Regnskab.Bogføring.Adressekonti);
            //if (string.IsNullOrWhiteSpace(value))
            //{
            //    AdressekontiCollectionViewSource.Source = adressekonti;
            //    AdressekontiCollectionViewSource.View.MoveCurrentTo(null);
            //    return;
            //}
            //if (Regnskab.Bogføring.Adressekonto != 0)
            //{
            //    AdressekontiCollectionViewSource.Source = adressekonti;
            //    AdressekontiCollectionViewSource.View.MoveCurrentTo(adressekonti.FirstOrDefault(m => m != null && m.Nummer == Regnskab.Bogføring.Adressekonto));
            //    return;
            //}
            //var filteredAdressekonti = adressekonti
            //    .Where(m =>
            //        {
            //            if (m == null)
            //            {
            //                return true;
            //            }
            //            if (string.IsNullOrEmpty(m.Navn))
            //            {
            //                return false;
            //            }
            //            if (m.Navn.Length < value.Length)
            //            {
            //                return false;
            //            }
            //            return string.Compare(m.Navn.Substring(0, value.Length), value, StringComparison.OrdinalIgnoreCase) == 0;
            //        })
            //    .ToList();
            //AdressekontiCollectionViewSource.Source = filteredAdressekonti;
            //if (filteredAdressekonti.Count(m => m != null) == 1)
            //{
            //    AdressekontiCollectionViewSource.View.MoveCurrentTo(filteredAdressekonti.First(m => m != null));
            //}
        }

        /// <summary>
        /// Eventhandler, der rejses, når den valgte værdi i kombiboksen til valg af adressekonto ændres.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        private void AdressekontoComboBoxSelectionChangedEventHandler(object sender, SelectionChangedEventArgs eventArgs)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            if (eventArgs == null)
            {
                throw new ArgumentNullException("eventArgs");
            }
            if (Regnskab.Bogføring == null)
            {
                return;
            }
            var comboBox = sender as ComboBox;
            if (comboBox == null)
            {
                return;
            }
            var selectedAdressekontoViewModel = comboBox.SelectedItem as IAdressekontoViewModel;
            if (selectedAdressekontoViewModel == null)
            {
                if (Regnskab.Bogføring.Adressekonto == 0)
                {
                    return;
                }
                Regnskab.Bogføring.Adressekonto = 0;
                return;
            }
            if (selectedAdressekontoViewModel.Nummer == Regnskab.Bogføring.Adressekonto)
            {
                return;
            }
            Regnskab.Bogføring.Adressekonto = selectedAdressekontoViewModel.Nummer;
        }

        #endregion
    }
}
