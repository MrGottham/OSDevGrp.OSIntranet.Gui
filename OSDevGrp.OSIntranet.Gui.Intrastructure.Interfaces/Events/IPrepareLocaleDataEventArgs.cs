using System.Xml.Linq;

namespace OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Events
{
    /// <summary>
    /// Interface for argumenter til et event, der forbereder data i det lokale datalager for læsning og skrivning.
    /// </summary>
    public interface IPrepareLocaleDataEventArgs : IIntranetGuiEventArgs
    {
        /// <summary>
        /// XML dokument indeholdende data i det lokale datalager.
        /// </summary>
        XDocument LocaleDataDocument
        {
            get;
        }

        /// <summary>
        /// Angivelse af, om data skal forberedes for læsning.
        /// </summary>
        bool ReadingContext
        {
            get;
        }

        /// <summary>
        /// Angivelse af, om data skal forberedes for skrivning.
        /// </summary>
        bool WritingContext
        {
            get;
        }

        /// <summary>
        /// Angivelse af, om data skal forberedes for synkronisering.
        /// </summary>
        bool SynchronizationContext
        {
            get;
        }
    }
}
