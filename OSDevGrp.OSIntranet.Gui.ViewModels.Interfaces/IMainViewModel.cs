using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces
{
    /// <summary>
    /// Interface for ViewModel til binding mod Views.
    /// </summary>
    public interface IMainViewModel : IViewModel
    {
        /// <summary>
        /// ViewModel for en liste af regnskaber.
        /// </summary>
        IRegnskabslisteViewModel Regnskabsliste
        {
            get;
        }

        /// <summary>
        /// ViewModel for en exceptionhandler.
        /// </summary>
        IExceptionHandlerViewModel ExceptionHandler
        {
            get;
        }
    }
}
