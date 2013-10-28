using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring
{
    /// <summary>
    /// Interface til en ViewModel indeholdende konfiguration til finansstyring.
    /// </summary>
    public interface IFinansstyringKonfigurationViewModel : IConfigurationViewModel
    {
        /// <summary>
        /// Uri til servicen, der supporterer finansstyring.
        /// </summary>
        string FinansstyringServiceUri
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Antal bogføringslinjer, der skal hentes.
        /// </summary>
        int AntalBogføringslinjer
        {
            get; 
            set; 
        }

        /// <summary>
        /// Antal dage, som nyheder er gældende.
        /// </summary>
        int DageForNyheder
        {
            get; 
            set; 
        }
    }
}
