using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring
{
    /// <summary>
    /// Interface til en ViewModel, der indeholder de grundlæggende oplysninger for en kontogruppe.
    /// </summary>
    public interface IKontogruppeViewModelBase : ITabelViewModelBase
    {
        /// <summary>
        /// Unik identifikation af kontogruppen.
        /// </summary>
        int Nummer
        {
            get;
        }
    }
}
