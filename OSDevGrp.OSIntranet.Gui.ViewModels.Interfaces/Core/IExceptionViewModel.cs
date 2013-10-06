namespace OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core
{
    /// <summary>
    /// Interface til en ViewModel for en præsenterbar exception.
    /// </summary>
    public interface IExceptionViewModel : IViewModel
    {
        /// <summary>
        /// Fejlbesked.
        /// </summary>
        string Message
        {
            get;
        }

        /// <summary>
        /// Detaljeret fejlbesked.
        /// </summary>
        string Details
        {
            get;
        }
    }
}
