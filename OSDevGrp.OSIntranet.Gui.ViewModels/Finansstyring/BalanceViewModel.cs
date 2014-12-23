using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring
{
    /// <summary>
    /// ViewModel for en linje i balancen.
    /// </summary>
    public class BalanceViewModel : KontogruppeViewModel, IBalanceViewModel
    {
        #region Private variables

        private readonly IRegnskabViewModel _regnskabViewModel;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner en ViewModel til en linje i balancen.
        /// </summary>
        /// <param name="regnskabViewModel">ViewModel for regnskabet, som balancelinjen er tilknyttet.</param>
        /// <param name="kontogruppeModel">Model for gruppen af konti, som balancelinjen baserer sig på.</param>
        /// <param name="exceptionHandlerViewModel">Implementering af ViewModel for exceptionhandleren.</param>
        public BalanceViewModel(IRegnskabViewModel regnskabViewModel, IKontogruppeModel kontogruppeModel, IExceptionHandlerViewModel exceptionHandlerViewModel)
            : base(kontogruppeModel, exceptionHandlerViewModel)
        {
            if (regnskabViewModel == null)
            {
                throw new ArgumentNullException("regnskabViewModel");
            }
            _regnskabViewModel = regnskabViewModel;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Registrerede konti, som indgår i balancelinjen.
        /// </summary>
        public virtual IEnumerable<IKontoViewModel> Konti
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        
        #endregion
    }
}
