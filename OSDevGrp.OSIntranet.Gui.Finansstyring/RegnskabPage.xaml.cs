using System;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace OSDevGrp.OSIntranet.Gui.Finansstyring
{
    /// <summary>
    /// Page til et regnskab.
    /// </summary>
    public sealed partial class RegnskabPage
    {
        #region Private variables

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

            var mainViewModel = (IMainViewModel) Application.Current.Resources["MainViewModel"];
            Regnskabsliste = mainViewModel.Regnskabsliste;

            SizeChanged += PageSizeChangedEventHandler;

            // TODO: http://www.c-sharpcorner.com/UploadFile/c25b6d/perform-grouping-in-gridview-using-windows-store-apps/
            // TODO: http://msdn.microsoft.com/en-us/library/windows/apps/hh780627.aspx
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
            var regnskabsnummer = (int) e.Parameter;
            Regnskab = await Regnskabsliste.RegnskabGetAsync(regnskabsnummer);
            try
            {
                if (Regnskab == null)
                {
                    return;
                }
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

        #endregion
    }
}
