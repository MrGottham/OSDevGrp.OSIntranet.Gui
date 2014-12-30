using System;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring
{
    /// <summary>
    /// ViewModel til en kontogruppe.
    /// </summary>
    public class KontogruppeViewModel : KontogruppeViewModelBase<IKontogruppeModel>, IKontogruppeViewModel
    {
        #region Constructor

        /// <summary>
        /// Danner en ViewModel til en kontogruppe.
        /// </summary>
        /// <param name="kontogruppeModel">Modellen for kontogruppen.</param>
        /// <param name="exceptionHandlerViewModel">Implementering af ViewModel for en exceptionhandler.</param>
        public KontogruppeViewModel(IKontogruppeModel kontogruppeModel, IExceptionHandlerViewModel exceptionHandlerViewModel)
            : base(kontogruppeModel, exceptionHandlerViewModel)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Angivelse af siden i balancen, hvor kontogruppen er placeret.
        /// </summary>
        public virtual Balancetype Balancetype
        {
            get
            {
                return Model.Balancetype;
            }
            set
            {
                try
                {
                    Model.Balancetype = value;
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex);
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Danner en ViewModel for en linje, der kan indgå i balancen til et givent regnskab og som er baseret på kontogruppen.
        /// </summary>
        /// <param name="regnskabViewModel">ViewModel for regnskabet, hvori linjen skal indgå i balancen.</param>
        /// <returns>ViewModel for en linje, der kan indgå i balancen i det givne regnskab.</returns>
        public virtual IBalanceViewModel CreateBalancelinje(IRegnskabViewModel regnskabViewModel)
        {
            if (regnskabViewModel == null)
            {
                throw new ArgumentNullException("regnskabViewModel");
            }
            return new BalanceViewModel(regnskabViewModel, Model, ExceptionHandler);
        }

        #endregion
    }
}
