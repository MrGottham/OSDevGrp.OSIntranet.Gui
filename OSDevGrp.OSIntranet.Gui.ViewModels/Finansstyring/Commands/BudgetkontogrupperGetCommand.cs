using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.Commands
{
    /// <summary>
    /// Kommando, der kan hente kontogrupper for budgetkonti til et regnskab.
    /// </summary>
    public class BudgetkontogrupperGetCommand : ViewModelCommandBase<IRegnskabViewModel>, ITaskableCommand
    {
        #region Private variables

        private bool _isBusy;
        private readonly IFinansstyringRepository _finansstyringRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner en kommando, der kan hente kontogrupper for budgetkonti til et regnskab.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="exceptionHandlerViewModel">Implementering af ViewModel til en exceptionhandler.</param>
        public BudgetkontogrupperGetCommand(IFinansstyringRepository finansstyringRepository, IExceptionHandlerViewModel exceptionHandlerViewModel)
            : base(exceptionHandlerViewModel)
        {
            if (finansstyringRepository == null)
            {
                throw new ArgumentNullException("finansstyringRepository");
            }
            _finansstyringRepository = finansstyringRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returnerer angivelse af, om kommandoen kan udføres på den givne ViewModel.
        /// </summary>
        /// <param name="regnskabViewModel">ViewModel, hvorpå kommandoen skal udføres.</param>
        /// <returns>Angivelse af, om kommandoen kan udføres på den givne ViewModel.</returns>
        protected override bool CanExecute(IRegnskabViewModel regnskabViewModel)
        {
            return _isBusy == false;
        }

        /// <summary>
        /// Henter og opdaterer regnskabet med kontogrupper til budgetkonti.
        /// </summary>
        /// <param name="regnskabViewModel">ViewModel for regnskabet, hvorpå kontogrupper til budgetkonti skal opdateres.</param>
        protected override void Execute(IRegnskabViewModel regnskabViewModel)
        {
            var budgetkontogrupperViewModels = new List<IBudgetkontogruppeViewModel>(regnskabViewModel.Budgetkontogrupper);
            if (budgetkontogrupperViewModels.Any())
            {
                return;
            }
            _isBusy = true;
            var task = _finansstyringRepository.BudgetkontogruppelisteGetAsync();
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
        /// Indsætter ViewModels for kontogrupper til budgetkonti i regnskabet.
        /// </summary>
        /// <param name="regnskabViewModel">ViewModel for regnskabet, hvorpå kontogrupper til budgetkonti skal indsættes.</param>
        /// <param name="budgetkontogruppeModels">Kontogrupper til budgetkonti, der skal indsættes i regnskabet.</param>
        /// <param name="argument">Argument til indsættelse af ViewModels for kontogrupper til budgetkonti i regnskabet.</param>
        private void HandleResult(IRegnskabViewModel regnskabViewModel, IEnumerable<IBudgetkontogruppeModel> budgetkontogruppeModels, object argument)
        {
            if (regnskabViewModel == null)
            {
                throw new ArgumentNullException("regnskabViewModel");
            }
            if (budgetkontogruppeModels == null)
            {
                throw new ArgumentNullException("budgetkontogruppeModels");
            }
            if (argument == null)
            {
                throw new ArgumentNullException("argument");
            }
            foreach (var budgetkontogruppeModel in budgetkontogruppeModels)
            {
                regnskabViewModel.BudgetkontogruppeAdd(new BudgetkontogruppeViewModel(budgetkontogruppeModel, ExceptionHandler));
            }
        }

        #endregion
    }
}
