using System.IO;

namespace OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Events
{
    /// <summary>
    /// Interface for argumenter til et event, der håndterer oprettelse af en stream.
    /// </summary>
    public interface IHandleStreamCreationEventArgs : IIntranetGuiEventArgs
    {
        /// <summary>
        /// Kontekst, hvorfra stream skal oprettes.
        /// </summary>
        object CreationContext
        {
            get;
        }

        /// <summary>
        /// Stream, der er blevet oprettet.
        /// </summary>
        Stream Result
        {
            get; 
            set;
        }
    }
}
