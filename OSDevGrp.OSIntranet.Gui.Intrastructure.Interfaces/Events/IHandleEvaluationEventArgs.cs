namespace OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Events
{
    /// <summary>
    /// Interface for argumenter til et event, der håndterer en validering.
    /// </summary>
    public interface IHandleEvaluationEventArgs : IIntranetGuiEventArgs
    {
        /// <summary>
        /// Valideringskontekst.
        /// </summary>
        object ValidationContext
        {
            get;
        }

        /// <summary>
        /// Resultat.
        /// </summary>
        bool Result
        {
            get; 
            set; 
        }
    }
}
