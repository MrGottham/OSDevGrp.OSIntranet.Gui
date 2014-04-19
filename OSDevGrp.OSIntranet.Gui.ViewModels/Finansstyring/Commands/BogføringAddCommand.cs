using System;
using System.ComponentModel.DataAnnotations;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Events;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.Commands
{
    /// <summary>
    /// Kommando, der kan tilføje en bogføringslinje til et regnskab.
    /// </summary>
    public class BogføringAddCommand : ViewModelCommandBase<IBogføringViewModel>, ITaskableCommand
    {
        #region Private variables

        private bool _isBusy;
        private readonly IFinansstyringRepository _finansstyringRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner en kommando, der kan tilføje en bogføringslinje til et regnskab.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="exceptionHandlerViewModel">Implementering af ViewModel til exceptionhandleren.</param>
        public BogføringAddCommand(IFinansstyringRepository finansstyringRepository, IExceptionHandlerViewModel exceptionHandlerViewModel)
            : base(exceptionHandlerViewModel)
        {
            if (finansstyringRepository == null)
            {
                throw new ArgumentNullException("finansstyringRepository");
            }
            _finansstyringRepository = finansstyringRepository;
        }

        #endregion

        #region Events

        /// <summary>
        /// Event, der rejses efter endt bogføring af en bogføringslinje.
        /// </summary>
        public virtual event IntranetGuiEventHandler<IEmptyEventArgs> OnBogført;

        /// <summary>
        /// Event, der rejses, når der opstår en fejl ved bogføring af en bogføringslinje.
        /// </summary>
        public virtual event IntranetGuiEventHandler<IHandleExceptionEventArgs> OnError; 

        #endregion

        #region Methods

        /// <summary>
        /// Returnerer angivelse af, om kommandoen kan udføres på den givne ViewModel.
        /// </summary>
        /// <param name="viewModel">ViewModel, hvorpå kommandoen skal udføres.</param>
        /// <returns>Angivelse af, om kommandoen kan udføres på den givne ViewModel.</returns>
        protected override bool CanExecute(IBogføringViewModel viewModel)
        {
            try
            {
                if (BogføringViewModel.ValidateDatoAsText(viewModel.DatoAsText) != ValidationResult.Success)
                {
                    return false;
                }
                if (BogføringViewModel.ValidateBilag(viewModel.Bilag) != ValidationResult.Success)
                {
                    return false;
                }
                if (BogføringViewModel.ValidateKontonummer(viewModel.Kontonummer) != ValidationResult.Success)
                {
                    return false;
                }
                if (BogføringViewModel.ValidateTekst(viewModel.Tekst) != ValidationResult.Success)
                {
                    return false;
                }
                if (BogføringViewModel.ValidateBudgetkontonummer(viewModel.Budgetkontonummer) != ValidationResult.Success)
                {
                    return false;
                }
                if (BogføringViewModel.ValidateCurrency(viewModel.DebitAsText) != ValidationResult.Success)
                {
                    return false;
                }
                if (BogføringViewModel.ValidateCurrency(viewModel.KreditAsText) != ValidationResult.Success)
                {
                    return false;
                }
                if (BogføringViewModel.ValidateAdressekonto(viewModel.Adressekonto) != ValidationResult.Success)
                {
                    return false;
                }
                if (Math.Abs(viewModel.Debit) + Math.Abs(viewModel.Kredit) <= 0M)
                {
                    return false;
                }
                if (viewModel.IsWorking)
                {
                    return false;
                }
                return _isBusy == false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Tilføjer en bogføringslinje til regnskabet på baggrund af ViewModel til bogføring.
        /// </summary>
        /// <param name="viewModel">ViewModel, som er grundlaget for bogføringslinjen, der skal tilføjes.</param>
        protected override void Execute(IBogføringViewModel viewModel)
        {
            _isBusy = true;
            var task = _finansstyringRepository.BogførAsync(viewModel.Regnskab.Nummer, viewModel.Dato, viewModel.Bilag, viewModel.Kontonummer, viewModel.Tekst, viewModel.Budgetkontonummer, viewModel.Debit, viewModel.Kredit, viewModel.Adressekonto);
            ExecuteTask = task.ContinueWith(t =>
                {
                    try
                    {
                        HandleResultFromTask(t, viewModel, new object(), HandleResult);
                    }
                    finally
                    {
                        _isBusy = false;
                    }
                });
        }

        /// <summary>
        /// Håndtering af exception ved udførelse af en kommando.
        /// </summary>
        /// <param name="exception">Exception.</param>
        /// <param name="parameter">Parameter, som kommanoden er blevet kaldt med.</param>
        protected override void HandleException(Exception exception, object parameter)
        {
            IntranetGuiCommandException commandException = null;
            IntranetGuiSystemException systemException = null;
            try
            {
                if (exception is IntranetGuiCommandException)
                {
                    base.HandleException(exception, parameter);
                    return;
                }
                if (exception is IntranetGuiValidationException)
                {
                    commandException = new IntranetGuiCommandException(Resource.GetExceptionMessage(ExceptionMessage.ErrorPostingAccountingLine), exception.Message, this, parameter, exception);
                    base.HandleException(commandException, parameter);
                    return;
                }
                if (exception is IntranetGuiBusinessException)
                {
                    commandException = new IntranetGuiCommandException(Resource.GetExceptionMessage(ExceptionMessage.ErrorPostingAccountingLine), exception.Message, this, parameter, exception);
                    base.HandleException(commandException, parameter);
                    return;
                }
                if (exception is IntranetGuiRepositoryException)
                {
                    commandException = new IntranetGuiCommandException(Resource.GetExceptionMessage(ExceptionMessage.ErrorPostingAccountingLine), Resource.GetExceptionMessage(ExceptionMessage.ErrorUpdateFinansstyringRepository), this, _finansstyringRepository, exception);
                    base.HandleException(commandException, parameter);
                    return;
                }
                if (exception is IntranetGuiSystemException)
                {
                    base.HandleException(exception, parameter);
                    return;
                }
                systemException = new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.CommandError, "BogføringAddCommand", exception.Message), exception);
                base.HandleException(systemException, parameter);
            }
            finally
            {
                if (OnError != null)
                {
                    OnError.Invoke(this, new HandleExceptionEventArgs(commandException ?? systemException ?? exception));
                }
            }
        }

        /// <summary>
        /// Tilføjer en bogføringslinje til regnskabet på baggrund af ViewModel til bogføring.
        /// </summary>
        /// <param name="bogføringViewModel">ViewModel, som er grundlaget for bogføringslinjen, der skal tilføjes.</param>
        /// <param name="bogføringsresultatModel">Model indeholdende resultatet af endt bogføring.</param>
        /// <param name="arguments">Argument til tilføjelse af en bogføringslinjen til regnskabet.</param>
        private void HandleResult(IBogføringViewModel bogføringViewModel, IBogføringsresultatModel bogføringsresultatModel, object arguments)
        {
            if (bogføringViewModel == null)
            {
                throw new ArgumentNullException("bogføringViewModel");
            }
            if (bogføringsresultatModel == null)
            {
                throw new ArgumentNullException("bogføringsresultatModel");
            }
            if (arguments == null)
            {
                throw new ArgumentNullException("arguments");
            }
            var regnskabViewModel = bogføringViewModel.Regnskab;
            if (regnskabViewModel == null)
            {
                return;
            }
            try
            {
                var bogføringslinjeViewModel = new BogføringViewModel(regnskabViewModel, bogføringsresultatModel.Bogføringslinje, _finansstyringRepository, ExceptionHandler);
                regnskabViewModel.BogføringslinjeAdd(bogføringslinjeViewModel);
                regnskabViewModel.NyhedAdd(new NyhedViewModel(bogføringsresultatModel.Bogføringslinje, bogføringslinjeViewModel.Image));
                foreach (var bogføringsadvarselModel in bogføringsresultatModel.Bogføringsadvarsler)
                {
                    regnskabViewModel.BogføringsadvarselAdd(new BogføringsadvarselViewModel(regnskabViewModel, bogføringslinjeViewModel, bogføringsadvarselModel, DateTime.Now));
                }
                if (OnBogført != null)
                {
                    OnBogført.Invoke(this, new EmptyEventArgs());
                }
            }
            catch (Exception ex)
            {
                throw new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.CommandError, "BogføringAddCommand", ex.Message), ex);
            }
            finally
            {
                var bogføringSetCommand = regnskabViewModel.BogføringSetCommand;
                if (bogføringSetCommand != null && bogføringSetCommand.CanExecute(regnskabViewModel))
                {
                    bogføringSetCommand.Execute(regnskabViewModel);
                }
            }
        }

        #endregion
    }
}
