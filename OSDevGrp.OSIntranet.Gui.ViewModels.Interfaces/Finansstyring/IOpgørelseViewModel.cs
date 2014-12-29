using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring
{
    /// <summary>
    /// Interface til en ViewModel for en linje i opgørelsen.
    /// </summary>
    public interface IOpgørelseViewModel : IBudgetkontogruppeViewModel
    {
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
        /// Label af samlet budgetteret beløb for sidste måned til opgørelseslinjen.
        /// </summary>
        string BudgetSidsteMånedLabel
        {
            get;
        }

        /// <summary>
        /// Samlet Budgetteret beløb for år til dato til opgørelseslinjen.
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
        /// Label af samlet budgetteret beløb for år til dato til opgørelseslinjen.
        /// </summary>
        string BudgetÅrTilDatoLabel
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
