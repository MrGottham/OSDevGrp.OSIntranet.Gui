namespace OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core
{
    /// <summary>
    /// Interface til en ViewModel indeholdende konfiguration.
    /// </summary>
    public interface IConfigurationViewModel : IViewModel
    {
        /// <summary>
        /// Unikt navn for konfigurationen.
        /// </summary>
        string Configuration
        {
            get;
        }
    }
}
