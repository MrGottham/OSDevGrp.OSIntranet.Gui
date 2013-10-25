using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Core
{
    /// <summary>
    /// ViewModel indeholdende konfiguration.
    /// </summary>
    public class ConfigurationViewModel : ViewModelBase, IConfigurationViewModel
    {
        #region Constructor

        /// <summary>
        /// Danner en ViewModel indeholdende konfiguration.
        /// </summary>
        /// <param name="configurationViewModels">Collection af ViewModels indeholdende forskellige konfigurationer.</param>
        public ConfigurationViewModel(IEnumerable<IConfigurationViewModel> configurationViewModels)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Navn for ViewModel, som kan benyttes til visning i brugergrænsefladen. 
        /// </summary>
        public override string DisplayName
        {
            get
            {
                return Resource.GetText(Text.Configuration);
            }
        }

        #endregion
    }
}
