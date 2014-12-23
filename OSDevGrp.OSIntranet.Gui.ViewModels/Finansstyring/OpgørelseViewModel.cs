using System;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring
{
    /// <summary>
    /// ViewModel for en linje i opgørelsen.
    /// </summary>
    public class OpgørelseViewModel : BudgetkontogruppeViewModel, IOpgørelseViewModel
    {
        #region Private variables

        private readonly IRegnskabViewModel _regnskabViewModel;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner en ViewModel til en linje i opgørelsen.
        /// </summary>
        /// <param name="regnskabViewModel">ViewModel for regnskabet, som opgørelseslinjen er tilknyttet.</param>
        /// <param name="budgetkontogruppeModel">Model for gruppen af budgetkonti, som opgørelseslinjen baserer sig på.</param>
        /// <param name="exceptionHandlerViewModel">Implementering af ViewModel for exceptionhandleren.</param>
        public OpgørelseViewModel(IRegnskabViewModel regnskabViewModel, IBudgetkontogruppeModel budgetkontogruppeModel, IExceptionHandlerViewModel exceptionHandlerViewModel)
            : base(budgetkontogruppeModel, exceptionHandlerViewModel)
        {
            if (regnskabViewModel == null)
            {
                throw new ArgumentNullException("regnskabViewModel");
            }
            _regnskabViewModel = regnskabViewModel;
        }

        #endregion
    }
}
