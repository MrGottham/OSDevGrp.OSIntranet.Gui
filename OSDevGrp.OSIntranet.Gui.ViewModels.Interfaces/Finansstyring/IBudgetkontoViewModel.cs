namespace OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring
{
    /// <summary>
    /// Interface til en ViewModel for en budgetkonto.
    /// </summary>
    public interface IBudgetkontoViewModel : IKontoViewModelBase<IBudgetkontogruppeViewModel>
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
        /// Tekstangivelse af budgetterede indtægter.
        /// </summary>
        string IndtægterAsText
        {
            get;
        }

        /// <summary>
        /// Label til budgetterede indtægter.
        /// </summary>
        string IndtægterLabel
        {
            get;
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
        /// Tekstangivelse af budgetterede udgifter.
        /// </summary>
        string UdgifterAsText
        { 
            get;
        }

        /// <summary>
        /// Label til budgetterede udgifter.
        /// </summary>
        string UdgifterLabel
        {
            get;
        }

        /// <summary>
        /// Budgetteret beløb.
        /// </summary>
        decimal Budget
        {
            get;
        }

        /// <summary>
        /// Tekstangivelse af budgetteret beløb.
        /// </summary>
        string BudgetAsText
        {
            get;
        }

        /// <summary>
        /// Label til budgetteret beløb.
        /// </summary>
        string BudgetLabel
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
        /// Tekstangivelse af budgetteret beløb for sidste måned.
        /// </summary>
        string BudgetSidsteMånedAsText
        {
            get;
        }

        /// <summary>
        /// Label af budgetteret beløb for sidste måned.
        /// </summary>
        string BudgetSidsteMånedLabel
        {
            get;
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
        /// Tekstangivelse af budgetteret beløb for år til dato.
        /// </summary>
        string BudgetÅrTilDatoAsText
        {
            get;
        }

        /// <summary>
        /// Label af budgetteret beløb for år til dato.
        /// </summary>
        string BudgetÅrTilDatoLabel
        {
            get;
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
        /// Tekstangivelse af budgetteret beløb for sidste år.
        /// </summary>
        string BudgetSidsteÅrAsText
        {
            get;
        }

        /// <summary>
        /// Label af budgetteret beløb for sidste år.
        /// </summary>
        string BudgetSidsteÅrLabel
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
        /// Tekstangivelse af bogført beløb.
        /// </summary>
        string BogførtAsText
        {
            get; 
        }

        /// <summary>
        /// Label til bogført beløb.
        /// </summary>
        string BogførtLabel
        {
            get;
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
        /// Tekstangivelse af bogført beløb for sidste måned.
        /// </summary>
        string BogførtSidsteMånedAsText
        {
            get;
        }

        /// <summary>
        /// Label af bogført beløb for sidste måned.
        /// </summary>
        string BogførtSidsteMånedLabel
        {
            get;
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
        /// Tekstangivelse af bogført beløb for år til dato.
        /// </summary>
        string BogførtÅrTilDatoAsText
        {
            get;
        }

        /// <summary>
        /// Label af bogført beløb for år til dato.
        /// </summary>
        string BogførtÅrTilDatoLabel
        {
            get;
        }

        /// <summary>
        /// Bogført beløb for sidste år.
        /// </summary>
        decimal BogførtSidsteÅr
        {
            get;
            set;
        }

        /// <summary>
        /// Tekstangivelse af bogført beløb for sidste år.
        /// </summary>
        string BogførtSidsteÅrAsText
        {
            get;
        }

        /// <summary>
        /// Label af bogført beløb for sidste år.
        /// </summary>
        string BogførtSidsteÅrLabel
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
