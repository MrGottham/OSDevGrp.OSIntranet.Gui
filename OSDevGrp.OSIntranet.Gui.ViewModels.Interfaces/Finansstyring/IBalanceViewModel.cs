using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring
{
    /// <summary>
    /// Interface til en ViewModel for en linje i balancen.
    /// </summary>
    public interface IBalanceViewModel : IKontogruppeViewModel
    {
        /// <summary>
        /// Registrerede konti, som indgår i balancelinjen.
        /// </summary>
        IEnumerable<IKontoViewModel> Konti
        {
            get;
        }

        /// <summary>
        /// Registrerer en konto til at indgå i balancelinjen.
        /// </summary>
        /// <param name="kontoViewModel">ViewModel for kontoen, der skal indgå i balancelinjen.</param>
        void Register(IKontoViewModel kontoViewModel);
    }
}
