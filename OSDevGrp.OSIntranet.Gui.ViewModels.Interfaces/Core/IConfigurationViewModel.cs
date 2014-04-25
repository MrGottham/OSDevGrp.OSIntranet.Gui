using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core
{
    /// <summary>
    /// Interface til en ViewModel indeholdende konfiguration.
    /// </summary>
    public interface IConfigurationViewModel : IValidateableViewModel
    {
        /// <summary>
        /// Unikt navn for konfigurationen.
        /// </summary>
        string Configuration
        {
            get;
        }

        /// <summary>
        /// Collection indeholdende navne for de enkelte konfigurationsværdier.
        /// </summary>
        IEnumerable<string> Keys
        {
            get;
        }
    }
}
