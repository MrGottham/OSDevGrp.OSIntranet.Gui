using System;
using System.ComponentModel;
using OSDevGrp.OSIntranet.Gui.Runtime;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
using Windows.UI.Xaml.Controls;

namespace OSDevGrp.OSIntranet.Gui.Finansstyring
{
    /// <summary>
    /// User control til konfiguration.
    /// </summary>
    public sealed partial class ConfigurationUserControl
    {
        #region Private variables

        private readonly ConfigurationProvider _configurationProvider;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner user control til konfiguration.
        /// </summary>
        public ConfigurationUserControl()
        {
            InitializeComponent();

            _configurationProvider = ConfigurationProvider.Instance;

            var mainViewModel = (IMainViewModel) Resources["MainViewModel"];
            mainViewModel.ApplyConfiguration(_configurationProvider.Settings);

            var finansstyringKonfiguration = (IFinansstyringKonfigurationViewModel) ((Grid) Content).DataContext;
            finansstyringKonfiguration.PropertyChanged += FinansstyringKonfigurationPropertyChangedEventHandler;
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
