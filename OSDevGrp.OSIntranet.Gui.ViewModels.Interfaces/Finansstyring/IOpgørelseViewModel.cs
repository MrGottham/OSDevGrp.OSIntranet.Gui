using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring
{
    /// <summary>
    /// Interface til en ViewModel for en linje i opgørelsen.
    /// </summary>
    public interface IOpgørelseViewModel : IBudgetkontogruppeViewModel
    {
        /// <summary>
        /// Regnskabet, som opgørelseslinjen er tilkyttet.
        /// </summary>
        IRegnskabViewModel Regnskab
        {
            get;
        }

        /// <summary>
        /// Registrerede budgetkonti, som indgår i opgørelseslinjen.
        /// </summary>
        IEnumerable<IBudgetkontoViewModel> Budgetkonti
        {
            get;
        }

        /// <summary>
        /// Samlet budgetteret beløb for opgørelseslinjen.
        /// </summary>
        decimal Budget
        {
            get;
        }

        /// <summary>
        /// Tekstangivelse af samlet budgetteret beløb for opgørelseslinjen.
        /// </summary>
        string BudgetAsText
        {
            get;
        }

        /// <summary>
        /// Label til samlet budgetteret beløb for opgørelseslinjen.
        /// </summary>
        string BudgetLabel
        {
            get;
        }

        /// <summary>
        /// Samlet budgetteret beløb for sidste måned til opgørelseslinjen.
        /// </summary>
        decimal BudgetSidsteMåned
        {
            get;
        }

        /// <summary>
        /// Tekstangivelse af samlet budgetteret beløb for sidste måned til opgørelseslinjen.
        /// </summary>
        string BudgetSidsteMånedAsText
        {
            get;
        }

        /// <summary>
        /// Label til samlet budgetteret beløb for sidste måned til opgørelseslinjen.
        /// </summary>
        string BudgetSidsteMånedLabel
        {
            get;
        }

        /// <summary>
        /// Samlet budgetteret beløb for år til dato til opgørelseslinjen.
        /// </summary>
        decimal BudgetÅrTilDato
        {
            get;
        }

        /// <summary>
        /// Tekstangivelse af samlet budgetteret beløb for år til dato til opgørelseslinjen.
        /// </summary>
        string BudgetÅrTilDatoAsText
        {
            get;
        }

        /// <summary>
        /// Label til samlet budgetteret beløb for år til dato til opgørelseslinjen.
        /// </summary>
        string BudgetÅrTilDatoLabel
        {
            get;
        }

        /// <summary>
        /// Samlet budgetteret beløb for sidste år til opgørelseslinjen.
        /// </summary>
        decimal BudgetSidsteÅr
        {
            get;
        }

        /// <summary>
        /// Tekstangivelse af samlet budgetteret beløb for sidste år til opgørelseslinjen.
        /// </summary>
        string BudgetSidsteÅrAsText
        {
            get;
        }

        /// <summary>
        /// Label til samlet budgetteret beløb for sidste år til opgørelseslinjen.
        /// </summary>
        string BudgetSidsteÅrLabel
        {
            get;
        }

        /// <summary>
        /// Samlet bogført beløb til opgørelseslinjen.
        /// </summary>
        decimal Bogført
        {
            get;
        }

        /// <summary>
        /// Tekstangivelse af samlet bogført beløb til opgørelseslinjen.
        /// </summary>
        string BogførtAsText
        {
            get;
        }

        /// <summary>
        /// Label til samlet bogført beløb til opgørelseslinjen.
        /// </summary>
        string BogførtLabel
        {
            get;
        }

        /// <summary>
        /// Samlet bogført beløb for sidste måned til opgørelseslinjen.
        /// </summary>
        decimal BogførtSidsteMåned
        {
            get;
        }

        /// <summary>
        /// Tekstangivelse af samlet bogført beløb for sidste måned til opgørelseslinjen.
        /// </summary>
        string BogførtSidsteMånedAsText
        {
            get;
        }

        /// <summary>
        /// Label til samlet bogført beløb for sidste måned til opgørelseslinjen.
        /// </summary>
        string BogførtSidsteMånedLabel
        {
            get;
        }

        /// <summary>
        /// Samlet bogført beløb for år til dato til opgørelseslinjen.
        /// </summary>
        decimal BogførtÅrTilDato
        {
            get;
        }

        /// <summary>
        /// Tekstangivelse af samlet bogført beløb for år til dato til opgørelseslinjen.
        /// </summary>
        string BogførtÅrTilDatoAsText
        {
            get;
        }

        /// <summary>
        /// Label til samlet bogført beløb for år til dato til opgørelseslinjen.
        /// </summary>
        string BogførtÅrTilDatoLabel
        {
            get;
        }

        /// <summary>
        /// Samlet bogført beløb for sidste år til opgørelseslinjen.
        /// </summary>
        decimal BogførtSidsteÅr
        {
            get;
        }

        /// <summary>
        /// Tekstangivelse af samlet bogført beløb for sidste år til opgørelseslinjen.
        /// </summary>
        string BogførtSidsteÅrAsText
        {
            get;
        }

        /// <summary>
        /// Label til samlet bogført beløb for sidste år til opgørelseslinjen.
        /// </summary>
        string BogførtSidsteÅrLabel
        {
            get;
        }

        /// <summary>
        /// Samlet disponibel beløb til opgørelseslinjen.
        /// </summary>
        decimal Disponibel
        {
            get;
        }

        /// <summary>
        /// Tekstangivelse af samlet disponibel beløb til opgørelseslinjen.
        /// </summary>
        string DisponibelAsText
        {
            get;
        }

        /// <summary>
        /// Label til samlet disponibel beløb til opgørelseslinjen.
        /// </summary>
        string DisponibelLabel
        {
            get;
        }

        /// <summary>
        /// Registrerer en budgetkonto til at indgå i opgørelseslinjen.
        /// </summary>
        /// <param name="budgetkontoViewModel">ViewModel for budgetkontoen, der skal indgå i opgørelseslinjen.</param>
        void Register(IBudgetkontoViewModel budgetkontoViewModel);
    }
}
