using System;
using System.Linq;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.Commands
{
    /// <summary>
    /// Kommando, der på et regnskab kan initerer en ny ViewModel til bogføring.
    /// </summary>
    public class BogføringSetCommand : ViewModelCommandBase<IRegnskabViewModel>, ITaskableCommand
    {
        #region Private variables

        private bool _isBusy;
        private readonly IFinansstyringRepository _finansstyringRepository;
        private readonly bool _runRefreshTasks;

        #endregion

        #region Constructors

        /// <summary>
        /// Danner en kommando, der på et regnskab kan initerer en ny ViewModel til bogføring.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="exceptionHandlerViewModel">Implementering af ViewModel til exceptionhandleren.</param>
        public BogføringSetCommand(IFinansstyringRepository finansstyringRepository, IExceptionHandlerViewModel exceptionHandlerViewModel)
            : this(finansstyringRepository, exceptionHandlerViewModel, false)
        {
        }

        /// <summary>
        /// Danner en kommando, der på et regnskab kan initerer en ny ViewModel til bogføring.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="exceptionHandlerViewModel">Implementering af ViewModel til exceptionhandleren.</param>
        /// <param name="runRefreshTasks">Angivelse af, om de Tasks, der udfører refresh, skal køres ved initiering af ViewModel, hvorfra der kan bogføres.</param>
        public BogføringSetCommand(IFinansstyringRepository finansstyringRepository, IExceptionHandlerViewModel exceptionHandlerViewModel, bool runRefreshTasks)
            : base(exceptionHandlerViewModel)
        {
            if (finansstyringRepository == null)
            {
                throw new ArgumentNullException("finansstyringRepository");
            }
            _finansstyringRepository = finansstyringRepository;
            _runRefreshTasks = runRefreshTasks;
        }

        #endregion

        /// <summary>
        /// Returnerer angivelse af, om kommandoen kan udføres på den givne ViewModel.
        /// </summary>
        /// <param name="regnskabViewModel">ViewModel, hvorpå kommandoen skal udføres.</param>
        /// <returns>Angivelse af, om kommandoen kan udføres på den givne ViewModel.</returns>
        protected override bool CanExecute(IRegnskabViewModel regnskabViewModel)
        {
            return _isBusy == false && (regnskabViewModel.Bogføringslinjer.Any() || regnskabViewModel.Konti.Any());
        }

        /// <summary>
        /// Initierer en ny ViewModel til bogføring på et givent regnskab.
        /// </summary>
        /// <param name="regnskabViewModel">ViewModel for regnskabet, hvor en ny ViewModel til bogføring skal initieres.</param>
        protected override void Execute(IRegnskabViewModel regnskabViewModel)
        {
            _isBusy = true;
            var kontonummer = regnskabViewModel.Bogføringslinjer.Any() ? regnskabViewModel.Bogføringslinjer.ElementAt(0).Kontonummer : regnskabViewModel.Konti.ElementAt(0).Kontonummer;
            var task = _finansstyringRepository.BogføringslinjeCreateNewAsync(regnskabViewModel.Nummer, DateTime.Now, kontonummer);
            ExecuteTask = task.ContinueWith(t =>
                {
                    try
                    {
                        HandleResultFromTask(t, regnskabViewModel, new object(), HandleResult);
                    }
                    finally
                    {
                        _isBusy = false;
                    }
                });
        }

        /// <summary>
        /// Initierer en ny ViewModel til bogføring på et givent regnskab.
        /// </summary>
        /// <param name="regnskabViewModel">ViewModel for regnskabet, hvor en ny ViewModel til bogføring skal initieres.</param>
        /// <param name="bogføringslinjeModel">Model for en ny bogføringslinje, der efterfølgende kan bogføres.</param>
        /// <param name="argument">Argument til initiering af en ny ViewModel til bogføring.</param>
        private void HandleResult(IRegnskabViewModel regnskabViewModel, IBogføringslinjeModel bogføringslinjeModel, object argument)
        {
            if (regnskabViewModel == null)
            {
                throw new ArgumentNullException("regnskabViewModel");
            }
            if (bogføringslinjeModel == null)
            {
                throw new ArgumentNullException("bogføringslinjeModel");
            }
            if (argument == null)
            {
                throw new ArgumentNullException("argument");
            }
            regnskabViewModel.BogføringSet(new BogføringViewModel(regnskabViewModel, bogføringslinjeModel, _finansstyringRepository, ExceptionHandler, _runRefreshTasks));
        }
    }
}
