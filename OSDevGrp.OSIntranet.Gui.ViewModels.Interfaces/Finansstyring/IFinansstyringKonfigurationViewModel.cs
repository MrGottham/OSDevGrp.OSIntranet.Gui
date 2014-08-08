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
        /// Valideringsfejl ved angivelse af uri til servicen, der supporterer finansstyring.
        /// </summary>
        string FinansstyringServiceUriValidationError
        {
            get;
        }

        /// <summary>
        /// Label til filnavnet for det lokale datalager.
        /// </summary>
        string LokalDataFilLabel
        {
            get;
        }

        /// <summary>
        /// Filnavn til det lokale datalager.
        /// </summary>
        string LokalDataFil
        {
            get;
        }

        /// <summary>
        /// Label til filnavnet for det lokale synkroniseringslager.
        /// </summary>
        string SynkroniseringDataFilLabel
        {
            get;
        }

        /// <summary>
        /// Filnavn til det lokale synkroniseringslager.
        /// </summary>
        string SynkroniseringDataFil
        {
            get;
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
        /// Valideringsfejl ved angivelse af antal bogføringslinjer, der skal hentes.
        /// </summary>
        string AntalBogføringslinjerValidationError
        {
            get;
        }

        /// <summary>
        /// Label til antal dage, som nyheder er gældende.
        /// </summary>
        string DageForNyhederLabel
        {
            get;
        }

        /// <summary>
        /// Antal dage, som nyheder er gældende.
        /// </summary>
        int DageForNyheder
        {
            get; 
            set; 
        }

        /// <summary>
        /// Valideringsfejl ved angivelse af antal dage, som nyheder er gældende.
        /// </summary>
        string DageForNyhederValidationError
        {
            get;
        }
    }
}
