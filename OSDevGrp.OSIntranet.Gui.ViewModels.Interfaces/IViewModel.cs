using System.ComponentModel;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces
{
    /// <summary>
    /// Interface til en ViewModel i OS Intrranet.
    /// </summary>
    public interface IViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Navn for ViewModel, som kan benyttes til visning i brugergrænsefladen.
        /// </summary>
        string DisplayName
        {
            get;
        }
    }
}
