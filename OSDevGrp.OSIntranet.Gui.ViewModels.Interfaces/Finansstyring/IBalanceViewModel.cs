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
    }
}
