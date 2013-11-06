using OSDevGrp.OSIntranet.Gui.Runtime;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces;

namespace OSDevGrp.OSIntranet.Gui.Finansstyring
{
    /// <summary>
    /// User control til konfiguration.
    /// </summary>
    public sealed partial class ConfigurationUserControl
    {
        /// <summary>
        /// Danner user control til konfiguration.
        /// </summary>
        public ConfigurationUserControl()
        {
            InitializeComponent();

            var configurationProvider = new ConfigurationProvider();

            var mainViewModel = (IMainViewModel)Resources["MainViewModel"];
            mainViewModel.ApplyConfiguration(configurationProvider.Settings);
        }
    }
}
