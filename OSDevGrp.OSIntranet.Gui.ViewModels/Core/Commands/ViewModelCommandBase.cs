using System;
using System.Threading;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Core.Commands
{
    /// <summary>
    /// Basisfunktionalitet til en kommando, der skal udføres på en ViewModel.
    /// </summary>
    /// <typeparam name="TViewModel">Typen på den ViewModel, hvorpå kommandoen skal udføres.</typeparam>
    public abstract class ViewModelCommandBase<TViewModel> : CommandBase where TViewModel : IViewModel
    {
        #region Constructor

        /// <summary>
        /// Danner basisfunktionalitet til en kommando, der skal udføres på en ViewModel.
        /// </summary>
        /// <param name="exceptionHandlerViewModel">Implementering af ViewModel til en exceptionhandler.</param>
        protected ViewModelCommandBase(IExceptionHandlerViewModel exceptionHandlerViewModel)
        {
            if (exceptionHandlerViewModel == null)
            {
                throw new ArgumentNullException(nameof(exceptionHandlerViewModel));
            }

            ExceptionHandler = exceptionHandlerViewModel;
            SynchronizationContext = SynchronizationContext.Current;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Task, som kommandoen gør brug af.
        /// </summary>
        public virtual Task ExecuteTask { get; protected set; }

        /// <summary>
        /// ViewModel for den exceptionhandler, der skal håndtere exceptions.
        /// </summary>
        protected IExceptionHandlerViewModel ExceptionHandler { get; }

        /// <summary>
        /// Synkroniseringskontekst.
        /// </summary>
        protected SynchronizationContext SynchronizationContext { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Returnerer angivelse af, om kommandoen kan udføres.
        /// </summary>
        /// <param name="parameter">Parameter til kommandoen.</param>
        /// <returns>Angivelse af, om kommandoen kan udføres.</returns>
        public override bool CanExecute(object parameter)
        {
            return parameter is TViewModel && CanExecute((TViewModel)parameter);
        }

        /// <summary>
        /// Udfører kommandoen.
        /// </summary>
        /// <param name="parameter">Parameter til kommandoen.</param>
        public override void Execute(object parameter)
        {
            try
            {
                Execute((TViewModel)parameter);
            }
            catch (Exception ex)
            {
                HandleException(ex, parameter);
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
        /// <param name="parameter">Parameter, som kommandoen er blevet kaldt med.</param>
        protected virtual void HandleException(Exception exception, object parameter)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            if (exception is IntranetGuiRepositoryException)
            {
                ExceptionHandler.HandleException(exception);
                return;
            }

            if (exception is IntranetGuiBusinessException)
            {
                ExceptionHandler.HandleException(exception);
                return;
            }

            if (exception is IntranetGuiSystemException)
            {
                ExceptionHandler.HandleException(exception);
                return;
            }

            ExceptionHandler.HandleException(new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.CommandError, GetType().Name, exception.Message), exception));
        }

        /// <summary>
        /// Håndterer resultatet fra udførelsen af en given task.
        /// </summary>
        /// <typeparam name="TTaskResult">Typen af resultatet, som task'en medfører.</typeparam>
        /// <typeparam name="TArgument">Typen på argumentet, som skal benyttes ved håndtering af resultatet.</typeparam>
        /// <param name="task">Task, hvorfra resultatet skal håndteres.</param>
        /// <param name="viewModel">Implementering af den ViewModel, hvorpå resultatet skal benyttes.</param>
        /// <param name="argument">Argument, som skal benyttes ved håndtering af resultatet.</param>
        /// <param name="onHandleTaskResult">Callback metode, der udføres, når resultatet skal håndteres.</param>
        protected void HandleResultFromTask<TTaskResult, TArgument>(Task<TTaskResult> task, TViewModel viewModel, TArgument argument, Action<TViewModel, TTaskResult, TArgument> onHandleTaskResult)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            if (Equals(viewModel, null))
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            if (onHandleTaskResult == null)
            {
                throw new ArgumentNullException(nameof(onHandleTaskResult));
            }

            try
            {
                if (task.IsCanceled || task.IsFaulted)
                {
                    task.Exception?.Handle(exception =>
                    {
                        HandleException(exception, viewModel);
                        return true;
                    });

                    return;
                }

                TTaskResult result = task.GetAwaiter().GetResult();
                if (Equals(result, null))
                {
                    return;
                }

                HandleResultFromTask(result, viewModel, argument, onHandleTaskResult, SynchronizationContext);
            }
            catch (Exception ex)
            {
                HandleException(ex, viewModel);
            }
        }

        /// <summary>
        /// Håndterer resultatet fra udførelsen af en given task.
        /// </summary>
        /// <typeparam name="TTaskResult">Typen af resultatet, som task'en medfører.</typeparam>
        /// <typeparam name="TArgument">Typen på argumentet, som skal benyttes ved håndtering af resultatet.</typeparam>
        /// <param name="taskResult">Resultat fra udførelsen af det givne task.</param>
        /// <param name="viewModel">Implementering af den ViewModel, hvorpå resultatet skal benyttes.</param>
        /// <param name="argument">Argument, som skal benyttes ved håndtering af resultatet.</param>
        /// <param name="onHandleTaskResult">Callback metode, der udføres, når resultatet skal håndteres.</param>
        /// <param name="synchronizationContext">Synkroniseringskontekst.</param>
        private void HandleResultFromTask<TTaskResult, TArgument>(TTaskResult taskResult, TViewModel viewModel, TArgument argument, Action<TViewModel, TTaskResult, TArgument> onHandleTaskResult, SynchronizationContext synchronizationContext)
        {
            if (Equals(viewModel, null))
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            if (onHandleTaskResult == null)
            {
                throw new ArgumentNullException(nameof(onHandleTaskResult));
            }

            if (synchronizationContext == null)
            {
                try
                {
                    onHandleTaskResult(viewModel, taskResult, argument);
                }
                catch (Exception ex)
                {
                    HandleException(ex, viewModel);
                }

                return;
            }

            Tuple<TTaskResult, TViewModel, TArgument, Action<TViewModel, TTaskResult, TArgument>> arguments = new Tuple<TTaskResult, TViewModel, TArgument, Action<TViewModel, TTaskResult, TArgument>>(taskResult, viewModel, argument, onHandleTaskResult);
            synchronizationContext.Post(obj =>
            {
                Tuple<TTaskResult, TViewModel, TArgument, Action<TViewModel, TTaskResult, TArgument>> tuple = (Tuple<TTaskResult, TViewModel, TArgument, Action<TViewModel, TTaskResult, TArgument>>)obj;
                HandleResultFromTask(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, null);
            }, arguments);
        }

        #endregion
    }
}