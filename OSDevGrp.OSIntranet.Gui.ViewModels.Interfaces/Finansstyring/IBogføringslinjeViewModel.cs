namespace OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring
{
    /// <summary>
    /// Interface til en ViewModel for en bogføringslinje.
    /// </summary>
    public interface IBogføringslinjeViewModel : IViewModel
    {
        /// <summary>
        /// Regnskabet, som bogføringslinjen er tilknyttet.
        /// </summary>
        IRegnskabViewModel Regnskab
        {
            get;
        }

        /// <summary>
        /// Unik identifikation af bogføringslinjen inden for regnskabet.
        /// </summary>
        int Løbenummer
        {
            get;
        }

        /// <summary>
        /// Billede, der illustrerer en bogføringslinje.
        /// </summary>
        byte[] Image
        {
            get;
        }
    }
}
