using System.Windows.Input;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces
{
    /// <summary>
    /// Interface, der kan angive, at en ViewModel er genindlæsbar.
    /// </summary>
    public interface IRefreshable
    {
        /// <summary>
        /// Kommando til genindlæsning og opdatering.
        /// </summary>
        ICommand RefreshCommand
        {
            get;
        }
    }
}
