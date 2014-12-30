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

        /// <summary>
        /// Danner en ViewModel for en linje, der kan indgå i balancen til et givent regnskab og som er baseret på kontogruppen.
        /// </summary>
        /// <param name="regnskabViewModel">ViewModel for regnskabet, hvori linjen skal indgå i balancen.</param>
        /// <returns>ViewModel for en linje, der kan indgå i balancen i det givne regnskab.</returns>
        IBalanceViewModel CreateBalancelinje(IRegnskabViewModel regnskabViewModel);
    }
}
