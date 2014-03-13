using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring
{
    /// <summary>
    /// ViewModel, der indeholder de grundlæggende oplysninger for en kontogruppe.
    /// </summary>
    /// <typeparam name="TKontogruppeModel">Typen på modellen, der indeholder de grundlæggende oplysninger for en kontogruppe.</typeparam>
    public abstract class KontogruppeViewModelBase<TKontogruppeModel> : TabelViewModelBase<TKontogruppeModel>, IKontogruppeViewModelBase where TKontogruppeModel : IKontogruppeModelBase
    {
        #region Constructor

        /// <summary>
        /// Danner en ViewModel, der indeholder de grundlæggende oplysninger for en kontogruppe.
        /// </summary>
        /// <param name="kontogruppeModel">Modellen, der indeholder de grundlæggende oplysninger for en kontogruppe.</param>
        /// <param name="exceptionHandlerViewModel">Implementering af ViewModel for en exceptionhandler.</param>
        protected KontogruppeViewModelBase(TKontogruppeModel kontogruppeModel, IExceptionHandlerViewModel exceptionHandlerViewModel)
            : base(kontogruppeModel, exceptionHandlerViewModel)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Unik identifikation af kontogruppen.
        /// </summary>
        public virtual int Nummer
        {
            get
            {
                return Model.Nummer;
            }
        }

        #endregion
    }
}
