using System;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Core.Commands
{
    /// <summary>
    /// Basisfunktionalitet til en kommando, der skal udføres på en ViewModel.
    /// </summary>
    /// <typeparam name="TViewModel">Typen på den ViewModel, hvorpå kommandoen skal udføres.</typeparam>
    public abstract class ViewModelCommandBase<TViewModel> : CommandBase where TViewModel : IViewModel
    {
        #region Methods

        /// <summary>
        /// Returnerer angivelse af, om kommandoen kan udføres.
        /// </summary>
        /// <param name="parameter">Parameter til kommandoen.</param>
        /// <returns>Angivelse af, om kommandoen kan udføres.</returns>
        public override bool CanExecute(object parameter)
        {
            return parameter is TViewModel && CanExecute((TViewModel) parameter);
        }

        /// <summary>
        /// Udfører kommandoen.
        /// </summary>
        /// <param name="parameter">Parameter til kommandoen.</param>
        public override void Execute(object parameter)
        {
            try
            {
                Execute((TViewModel) parameter);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Returnerer angivelse af, om kommandoen kan udføres på den givne ViewModel.
        /// </summary>
        /// <param name="viewModel">ViewModel, hvorpå kommandoen skal udføres.</param>
        /// <returns>Angivelse af, om kommandoen kan udføres på den givne ViewModel.</returns>
        protected virtual bool CanExecute(TViewModel viewModel)
        {
            return true;
        }

        /// <summary>
        /// Udfører kommandoen på den givne ViewModel.
        /// </summary>
        /// <param name="viewModel">ViewModel, hvorpå kommandoen skal udføres.</param>
        protected abstract void Execute(TViewModel viewModel);

        /// <summary>
        /// Håndtering af exception ved udførelse af en kommando.
        /// </summary>
        /// <param name="exception">Exception.</param>
        protected virtual void HandleException(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }
            if (exception is IntranetGuiRepositoryException)
            {
                throw exception;
            }
            if (exception is IntranetGuiBusinessException)
            {
                throw exception;
            }
            if (exception is IntranetGuiSystemException)
            {
                throw exception;
            }
            throw new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.CommandError, GetType().Name, exception.Message), exception);
        }

        #endregion
    }
}
