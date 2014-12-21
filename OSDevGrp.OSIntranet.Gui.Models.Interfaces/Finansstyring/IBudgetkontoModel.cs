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
        /// Budgetteret beløb for sidste måned.
        /// </summary>
        decimal BudgetSidsteMåned
        {
            get; 
            set;
        }

        /// <summary>
        /// Budgetteret beløb for år til dato.
        /// </summary>
        decimal BudgetÅrTilDato
        {
            get;
            set;
        }

        /// <summary>
        /// Budgetteret beløb for sidste år.
        /// </summary>
        decimal BudgetSidsteÅr
        {
            get;
            set;
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
        /// Bogført beløb for sidste måned.
        /// </summary>
        decimal BogførtSidsteMåned
        {
            get;
            set;
        }

        /// <summary>
        /// Bogført beløb for år til dato.
        /// </summary>
        decimal BogførtÅrTilDato
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
