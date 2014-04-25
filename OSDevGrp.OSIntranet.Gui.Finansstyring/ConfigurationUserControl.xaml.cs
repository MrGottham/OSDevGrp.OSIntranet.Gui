using System;
using System.ComponentModel;
using OSDevGrp.OSIntranet.Gui.Finansstyring.Core;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Events;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Runtime;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace OSDevGrp.OSIntranet.Gui.Finansstyring
{
    /// <summary>
    /// User control til konfiguration.
    /// </summary>
    public sealed partial class ConfigurationUserControl : IDisposable, IEventSubscriber<IHandleExceptionEventArgs>
    {
        #region Private variables

        private bool _finansstyringKonfigurationPropertyChangedIsSet;
        private bool _disposed;
        private static readonly DependencyProperty ConfigurationProviderProperty = DependencyProperty.Register("ConfigurationProvider", typeof (ConfigurationProvider), typeof (ConfigurationUserControl), new PropertyMetadata(null));
        private static readonly DependencyProperty MainViewModelProperty = DependencyProperty.Register("MainViewModel", typeof (IMainViewModel), typeof (ConfigurationUserControl), new PropertyMetadata(null));

        #endregion

        #region Constructor

        /// <summary>
        /// Danner user control til konfiguration.
        /// </summary>
        public ConfigurationUserControl()
        {
            InitializeComponent();

            ConfigurationProvider = ConfigurationProvider.Instance;

            MainViewModel = (IMainViewModel) Application.Current.Resources["MainViewModel"];
            MainViewModel.Subscribe(this);

            DataContext = MainViewModel;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Leverandør af konfigurationsværdier.
        /// </summary>
        public ConfigurationProvider ConfigurationProvider
        {
            get
            {
                return GetValue(ConfigurationProviderProperty) as ConfigurationProvider;
            }
            private set
            {
                SetValue(ConfigurationProviderProperty, value);
            }
        }

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
                    if (MainViewModel.FinansstyringKonfiguration != null && _finansstyringKonfigurationPropertyChangedIsSet)
                    {
                        MainViewModel.FinansstyringKonfiguration.PropertyChanged -= FinansstyringKonfigurationPropertyChangedEventHandler;
                        _finansstyringKonfigurationPropertyChangedIsSet = false;
                    }
                }
                SetValue(MainViewModelProperty, value);
                if (MainViewModel == null || MainViewModel.FinansstyringKonfiguration == null)
                {
                    return;
                }
                MainViewModel.FinansstyringKonfiguration.ClearValidationErrors();
                if (_finansstyringKonfigurationPropertyChangedIsSet)
                {
                    return;
                }
                MainViewModel.FinansstyringKonfiguration.PropertyChanged += FinansstyringKonfigurationPropertyChangedEventHandler;
                _finansstyringKonfigurationPropertyChangedIsSet = true;
            }
        }

        #endregion

        #region IDisposable members

        /// <summary>
        /// Destructor.
        /// </summary>
        ~ConfigurationUserControl()
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
            if (MainViewModel.FinansstyringKonfiguration != null && _finansstyringKonfigurationPropertyChangedIsSet)
            {
                MainViewModel.FinansstyringKonfiguration.PropertyChanged -= FinansstyringKonfigurationPropertyChangedEventHandler;
                _finansstyringKonfigurationPropertyChangedIsSet = false;
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
        public async void OnEvent(IHandleExceptionEventArgs handleExceptionEventArgs)
        {
            if (handleExceptionEventArgs == null)
            {
                throw new ArgumentNullException("handleExceptionEventArgs");
            }
            var validationException = handleExceptionEventArgs.Error as IntranetGuiValidationException;
            if (validationException == null)
            {
                return;
            }
            var validationContext = validationException.ValidationContext as IFinansstyringKonfigurationViewModel;
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
                    case "FinansstyringServiceUri":
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
        /// Eventhandler, der kaldes, når properties ændres på ViewModel for konfiguration til finansstyring.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        private void FinansstyringKonfigurationPropertyChangedEventHandler(object sender, PropertyChangedEventArgs eventArgs)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            if (eventArgs == null)
            {
                throw new ArgumentNullException("sender");
            }
            var finansstyringKonfiguration = sender as IFinansstyringKonfigurationViewModel;
            if (finansstyringKonfiguration == null)
            {
                return;
            }
            if (!ViewModelValidator.Validate(finansstyringKonfiguration, eventArgs.PropertyName))
            {
                return;
            }
            switch (eventArgs.PropertyName)
            {
                case "FinansstyringServiceUri":
                    ConfigurationProvider.SetValue("FinansstyringServiceUri", finansstyringKonfiguration.FinansstyringServiceUri);
                    break;

                case "AntalBogføringslinjer":
                    ConfigurationProvider.SetValue("AntalBogføringslinjer", finansstyringKonfiguration.AntalBogføringslinjer);
                    break;

                case "DageForNyheder":
                    ConfigurationProvider.SetValue("DageForNyheder", finansstyringKonfiguration.DageForNyheder);
                    break;
            }
        }

        #endregion
    }
}
