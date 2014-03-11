using System;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Core
{
    /// <summary>
    /// ViewModel, der indeholder grundlæggende tabeloplysninger, såsom typer, grupper m.m.
    /// </summary>
    /// <typeparam name="TTabelModel">Typen på modellen, der indeholder grundlæggende tabeloplysninger, såsom typer, grupper m.m.</typeparam>
    public abstract class TabelViewModelBase<TTabelModel> : ViewModelBase, ITabelViewModelBase where TTabelModel : ITabelModelBase
    {
        #region Private variables

        private readonly TTabelModel _tabelModel;
        private readonly IExceptionHandlerViewModel _exceptionHandlerViewModel;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner en ViewModel, der indeholder grundlæggende tabeloplysninger, såsom typer, grupper m.m.
        /// </summary>
        /// <param name="tabelModel">Modellen, der indeholder grundlæggende tabeloplysninger, såsom typer, grupper m.m.</param>
        /// <param name="exceptionHandlerViewModel">Implementering af ViewModel for en exceptionhandler.</param>
        protected TabelViewModelBase(TTabelModel tabelModel, IExceptionHandlerViewModel exceptionHandlerViewModel)
        {
            if (Equals(tabelModel, null))
            {
                throw new ArgumentNullException("tabelModel");
            }
            if (exceptionHandlerViewModel == null)
            {
                throw new ArgumentNullException("exceptionHandlerViewModel");
            }
            _tabelModel = tabelModel;
            _exceptionHandlerViewModel = exceptionHandlerViewModel;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Unik identifikation af de grundlæggende tabeloplysningerne i denne model.
        /// </summary>
        public virtual string Id
        {
            get
            {
                return Model.Id;
            }
        }

        /// <summary>
        /// Teksten der beskriver de grundlæggende tabeloplysninger i denne model.
        /// </summary>
        public virtual string Tekst
        {
            get
            {
                return Model.Tekst;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Navn for ViewModel, som kan benyttes til visning i brugergrænsefladen.
        /// </summary>
        public override string DisplayName
        {
            get
            {
                return Tekst;
            }
        }

        /// <summary>
        /// Modellen, der indeholder grundlæggende tabeloplysninger, såsom typer, grupper m.m.
        /// </summary>
        protected virtual TTabelModel Model
        {
            get
            {
                return _tabelModel;
            }
        }

        /// <summary>
        /// ViewModel for exceptionhandleren.
        /// </summary>
        protected virtual IExceptionHandlerViewModel ExceptionHandler
        {
            get
            {
                return _exceptionHandlerViewModel;
            }
        }

        #endregion
    }
}
