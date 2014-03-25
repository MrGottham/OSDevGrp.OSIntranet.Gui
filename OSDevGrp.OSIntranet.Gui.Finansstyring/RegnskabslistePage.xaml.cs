using System;
using System.Linq;
using System.Threading;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace OSDevGrp.OSIntranet.Gui.Finansstyring
{
    /// <summary>
    /// Page til en regnskabsliste.
    /// </summary>
    public sealed partial class RegnskabslistePage
    {
        #region Private variables

        private readonly SynchronizationContext _synchronizationContext;
        private static readonly DependencyProperty RegnskabslisteProperty = DependencyProperty.Register("Regnskabsliste", typeof (IRegnskabslisteViewModel), typeof (RegnskabslistePage), new PropertyMetadata(null));

        #endregion

        #region Constructor

        /// <summary>
        /// Danner Page til en regnskabsliste..
        /// </summary>
        public RegnskabslistePage()
        {
            InitializeComponent();
            _synchronizationContext = SynchronizationContext.Current;

            var mainViewModel = (IMainViewModel) Application.Current.Resources["MainViewModel"];
            Regnskabsliste = mainViewModel.Regnskabsliste;
            DataContext = Regnskabsliste;
            CommandAppBar.DataContext = Regnskabsliste;

            SizeChanged += PageSizeChangedEventHandler;
        }

        #endregion

        #region Properties

        /// <summary>
        /// ViewModel for listen af regnskaber.
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
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (Regnskabsliste.Regnskaber.Any())
            {
                ShowContent();
                return;
            }
            var refreshCommand = Regnskabsliste.RefreshCommand as ITaskableCommand;
            if (refreshCommand == null || refreshCommand.CanExecute(Regnskabsliste) == false)
            {
                return;
            }
            refreshCommand.Execute(Regnskabsliste);
            var executeTask = refreshCommand.ExecuteTask;
            if (executeTask == null)
            {
                return;
            }
            executeTask.ContinueWith(t =>
                {
                    if (_synchronizationContext == null)
                    {
                        ShowContent();
                        return;
                    }
                    _synchronizationContext.Post(obj => ShowContent(), null);
                });
        }

        /// <summary>
        /// Viser indhold på siden.
        /// </summary>
        private void ShowContent()
        {
            try
            {
                DefaultNavigationContent.Visibility = Visibility.Visible;
                DefaultNavigationProgressBar.Visibility = Visibility.Collapsed;
                MinimalNavigationContent.Visibility = Visibility.Visible;
                MinimalNavigationProgressBar.Visibility = Visibility.Collapsed;
                CommandAppBar.Visibility = Visibility.Visible;
            }
            finally
            {
                MinimalNavigationProgressBar.IsEnabled = false;
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
            Frame.Navigate(typeof (RegnskabPage), regnskabViewModel.Nummer);
        }

        #endregion
    }
}
