using System;
using System.ComponentModel;
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
            _tabelModel.PropertyChanged += PropertyChangedOnTabelModelEventHandler;
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
                try
                {
                    Model.Tekst = value;
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex);
                }
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

        #region Methods

        /// <summary>
        /// Metode, der kaldes, når en property på modellen, der indeholder grundlæggende tabeloplysninger, såsom typer, grupper m.m., ændres.
        /// </summary>
        /// <param name="propertyName">Navn på propertyen, der er blevet ændret.</param>
        protected virtual void ModelChanged(string propertyName)
        {
        }

        /// <summary>
        /// Eventhandler, der kaldes, når en property ændres på modellen, der indeholder grundlæggende tabeloplysninger, såsom typer, grupper m.m.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        private void PropertyChangedOnTabelModelEventHandler(object sender, PropertyChangedEventArgs eventArgs)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            if (eventArgs == null)
            {
                throw new ArgumentNullException("eventArgs");
            }
            switch (eventArgs.PropertyName)
            {
                case "Tekst":
                    RaisePropertyChanged(eventArgs.PropertyName);
                    RaisePropertyChanged("DisplayName");
                    break;

                default:
                    RaisePropertyChanged(eventArgs.PropertyName);
                    break;
            }
            try
            {
                ModelChanged(eventArgs.PropertyName);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }
        
        #endregion
    }
}
