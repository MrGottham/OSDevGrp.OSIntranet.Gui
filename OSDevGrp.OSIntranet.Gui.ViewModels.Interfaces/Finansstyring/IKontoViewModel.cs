namespace OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring
{
    /// <summary>
    /// Interface til en ViewModel for en konto.
    /// </summary>
    public interface IKontoViewModel : IKontoViewModelBase<IKontogruppeViewModel>
    {
        /// <summary>
        /// Kreditbeløb.
        /// </summary>
        decimal Kredit
        {
            get; 
            set; 
        }

        /// <summary>
        /// Tekstangivelse af kreditbeløb.
        /// </summary>
        string KreditAsText
        {
            get;
        }

        /// <summary>
        /// Label til kreditbeløb.
        /// </summary>
        string KreditLabel
        {
            get;
        }

        /// <summary>
        /// Saldo.
        /// </summary>
        decimal Saldo
        {
            get; 
            set; 
        }

        /// <summary>
        /// Tekstangivelse af saldo.
        /// </summary>
        string SaldoAsText
        {
            get;
        }

        /// <summary>
        /// Label til saldo.
        /// </summary>
        string SaldoLabel
        {
            get;
        }

        /// <summary>
        /// Disponibel beløb.
        /// </summary>
        decimal Disponibel
        {
            get;
        }

        /// <summary>
        /// Tekstangivelse af disponibel beløb.
        /// </summary>
        string DisponibelAsText
        {
            get;
        }

        /// <summary>
        /// Label til disponibel beløb.
        /// </summary>
        string DisponibelLabel
        {
            get;
        }
    }
}
