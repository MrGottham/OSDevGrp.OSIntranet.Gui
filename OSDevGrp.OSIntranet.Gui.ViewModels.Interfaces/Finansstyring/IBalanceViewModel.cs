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
        /// Samlet kreditbeløb for balancelinjen.
        /// </summary>
        decimal Kredit
        {
            get;
        }

        /// <summary>
        /// Tekstangivelse af samlet kreditbeløb for balancelinjen.
        /// </summary>
        string KreditAsText
        {
            get;
        }

        /// <summary>
        /// Label til samlet kreditbeløb for balancelinjen.
        /// </summary>
        string KreditLabel
        {
            get;
        }

        /// <summary>
        /// Samlet saldo for balancelinjen.
        /// </summary>
        decimal Saldo
        {
            get;
        }

        /// <summary>
        /// Tekstangivelse af samlet saldo for balancelinjen.
        /// </summary>
        string SaldoAsText
        {
            get;
        }

        /// <summary>
        /// Label til samlet saldo for balancelinjen.
        /// </summary>
        string SaldoLabel
        {
            get;
        }

        /// <summary>
        /// Samlet disponibel beløb for balancelinjen.
        /// </summary>
        decimal Disponibel
        {
            get;
        }

        /// <summary>
        /// Tekstangivelse af samlet disponibel beløb for balancelinjen.
        /// </summary>
        string DisponibelAsText
        {
            get;
        }

        /// <summary>
        /// Label til samlet disponibel beløb for balancelinjen.
        /// </summary>
        string DisponibelLabel
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
