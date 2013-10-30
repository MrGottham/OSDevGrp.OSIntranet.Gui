using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring
{
    /// <summary>
    /// Interface til en ViewModel indeholdende konfiguration til finansstyring.
    /// </summary>
    public interface IFinansstyringKonfigurationViewModel : IConfigurationViewModel
    {
        /// <summary>
        /// Label til uri for servicen, der supporterer finansstyring.
        /// </summary>
        string FinansstyringServiceUriLabel
        {
            get;
        }

        /// <summary>
        /// Uri til servicen, der supporterer finansstyring.
        /// </summary>
        string FinansstyringServiceUri
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Label til antal bogføringslinjer, der skal hentes.
        /// </summary>
        string AntalBogføringslinjerLabel
        {
            get;
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
