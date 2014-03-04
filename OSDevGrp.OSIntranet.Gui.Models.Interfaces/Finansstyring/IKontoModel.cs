namespace OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring
{
    /// <summary>
    /// Interface til en model for en konto.
    /// </summary>
    public interface IKontoModel : IKontoModelBase
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
        /// Saldo.
        /// </summary>
        decimal Saldo
        {
            get; 
            set; 
        }

        /// <summary>
        /// Disponibel beløb.
        /// </summary>
        decimal Disponibel
        {
            get;
        }
    }
}
