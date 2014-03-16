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
    }
}
