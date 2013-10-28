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
            if (configurationViewModels == null)
            {
                throw new ArgumentNullException("configurationViewModels");
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Unikt navn for konfigurationen.
        /// </summary>
        public virtual string Configuration
        {
            get
            {
                return "OSDevGrp.OSIntranet.Gui.ViewModels.Core.ConfigurationViewModel";
            }
        }

        /// <summary>
        /// Collection indeholdende navne for de enkelte konfigurationsværdier.
        /// </summary>
        public virtual IEnumerable<string> Keys
        {
            get
            {
                return new List<string>(0);
            }
        }

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

        #region Methods

        public 

        #endregion
    }
}
