namespace OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring
{
    /// <summary>
    /// Interface til en ViewModel for en kontogruppe til budgetkonti.
    /// </summary>
    public interface IBudgetkontogruppeViewModel : IKontogruppeViewModelBase
    {
        /// <summary>
        /// Danner en ViewModel for en linje, der kan indgå i årsopgørelsen i et givent regnskab og som er baseret på kontogruppen til budgetkonti.
        /// </summary>
        /// <param name="regnskabViewModel">ViewModel for regnskabet, hvori linjen skal indgå i årsopgørelsen.</param>
        /// <returns>ViewModel for en linje, der kan indgå i årsopgørelsen i det givne regnskab.</returns>
        IOpgørelseViewModel CreateOpgørelseslinje(IRegnskabViewModel regnskabViewModel);
    }
}
