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
        /// Registrerer en budgetkonto til at indgå i opgørelseslinjen.
        /// </summary>
        /// <param name="budgetkontoViewModel">ViewModel for budgetkontoen, der skal indgå i opgørelseslinjen.</param>
        void Register(IBudgetkontoViewModel budgetkontoViewModel);
    }
}
