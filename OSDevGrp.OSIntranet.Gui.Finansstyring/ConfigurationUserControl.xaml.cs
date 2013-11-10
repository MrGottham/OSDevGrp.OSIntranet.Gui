using System;
using System.ComponentModel;
using OSDevGrp.OSIntranet.Gui.Finansstyring.Core;
using OSDevGrp.OSIntranet.Gui.Runtime;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
using Windows.UI.Xaml.Controls;

namespace OSDevGrp.OSIntranet.Gui.Finansstyring
{
    /// <summary>
    /// User control til konfiguration.
    /// </summary>
    public sealed partial class ConfigurationUserControl : IDisposable
    {
        #region Private variables

        private bool _disposed;
        private readonly ConfigurationProvider _configurationProvider;
        private readonly IMainViewModel _mainViewModel;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner user control til konfiguration.
        /// </summary>
        public ConfigurationUserControl()
        {
            InitializeComponent();

            _configurationProvider = ConfigurationProvider.Instance;

            _mainViewModel = (IMainViewModel) Resources["MainViewModel"];
            _mainViewModel.ApplyConfiguration(_configurationProvider.Settings);

            // TODO: Subscripe...

            var finansstyringKonfiguration = (IFinansstyringKonfigurationViewModel) ((Grid) Content).DataContext;
            finansstyringKonfiguration.PropertyChanged += FinansstyringKonfigurationPropertyChangedEventHandler;
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
            if (disposing)
            {
            }
            _disposed = true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Eventhandler, der kaldes, når properties ændres på ViewModel for konfiguration til finansstyring.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        public void FinansstyringKonfigurationPropertyChangedEventHandler(object sender, PropertyChangedEventArgs eventArgs)
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
                    _configurationProvider.SetValue("FinansstyringServiceUri", finansstyringKonfiguration.FinansstyringServiceUri);
                    break;
            }
        }

        #endregion

    }
}
