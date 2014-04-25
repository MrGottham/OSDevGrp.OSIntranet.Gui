namespace OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces
{
    /// <summary>
    /// Interface til en validérbar ViewModel i OS Intranet.
    /// </summary>
    public interface IValidateableViewModel : IViewModel
    {
        /// <summary>
        /// Nulstiller alle valideringsfejl.
        /// </summary>
        void ClearValidationErrors();
    }
}
