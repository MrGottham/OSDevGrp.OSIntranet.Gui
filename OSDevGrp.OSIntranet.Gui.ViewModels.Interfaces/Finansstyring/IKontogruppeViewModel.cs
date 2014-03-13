using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring
{
    /// <summary>
    /// Interface til en ViewModel for en kontogruppe.
    /// </summary>
    public interface IKontogruppeViewModel : IKontogruppeViewModelBase
    {
        /// <summary>
        /// Angivelse af siden i balancen, hvor kontogruppen er placeret.
        /// </summary>
        Balancetype Balancetype
        {
            get; 
            set;
        }
    }
}
