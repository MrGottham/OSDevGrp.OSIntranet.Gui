namespace OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring
{
    /// <summary>
    /// Interface til en model for en budgetkonto.
    /// </summary>
    public interface IBudgetkontoModel : IKontoModelBase
    {
        /// <summary>
        /// Budgetterede indtægter.
        /// </summary>
        decimal Indtægter
        {
            get; 
            set; 
        }

        /// <summary>
        /// Budgetterede udgifter.
        /// </summary>
        decimal Udgifter
        {
            get; 
            set;
        }

        /// <summary>
        /// Budgetteret beløb.
        /// </summary>
        decimal Budget
        {
            get;
        }

        /// <summary>
        /// Bogført beløb.
        /// </summary>
        decimal Bogført
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
