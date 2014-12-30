using System;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring
{
    /// <summary>
    /// ViewModel til en kontogruppe for budgetkonti.
    /// </summary>
    public class BudgetkontogruppeViewModel : KontogruppeViewModelBase<IBudgetkontogruppeModel>, IBudgetkontogruppeViewModel
    {
        #region Constructor

        /// <summary>
        /// Danner en ViewModel til en kontogruppe for budgetkonti.
        /// </summary>
        /// <param name="budgetkontogruppeModel">Modellen for kontogruppen til budgetkonti.</param>
        /// <param name="exceptionHandlerViewModel">Implementering af ViewModel for en exceptionhandler.</param>
        public BudgetkontogruppeViewModel(IBudgetkontogruppeModel budgetkontogruppeModel, IExceptionHandlerViewModel exceptionHandlerViewModel)
            : base(budgetkontogruppeModel, exceptionHandlerViewModel)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Danner en ViewModel for en linje, der kan indgå i årsopgørelsen i et givent regnskab og som er baseret på kontogruppen til budgetkonti.
        /// </summary>
        /// <param name="regnskabViewModel">ViewModel for regnskabet, hvori linjen skal indgå i årsopgørelsen.</param>
        /// <returns>ViewModel for en linje, der kan indgå i årsopgørelsen i det givne regnskab.</returns>
        public virtual IOpgørelseViewModel CreateOpgørelseslinje(IRegnskabViewModel regnskabViewModel)
        {
            if (regnskabViewModel == null)
            {
                throw new ArgumentNullException("regnskabViewModel");
            }
            return new OpgørelseViewModel(regnskabViewModel, Model, ExceptionHandler);
        }

        #endregion
    }
}
