using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.Commands
{
    /// <summary>
    /// Kommando, der kan hente og opdatere en budgetkontoen.
    /// </summary>
    public class BudgetkontoGetCommand : ViewModelCommandBase<IBudgetkontoViewModel>, ITaskableCommand
    {
        #region Private variables

        private bool _isBusy;
        private readonly ITaskableCommand _dependencyCommand;
        private readonly IFinansstyringRepository _finansstyringRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner en kommando, der kan hente og opdatere en budgetkontoen.
        /// </summary>
        /// <param name="dependencyCommand">Implementering af kommando, som denne kommando er afhængig af.</param>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="exceptionHandlerViewModel">Implementering af ViewModel til en exceptionhandler.</param>
        public BudgetkontoGetCommand(ITaskableCommand dependencyCommand, IFinansstyringRepository finansstyringRepository, IExceptionHandlerViewModel exceptionHandlerViewModel)
            : base(exceptionHandlerViewModel)
        {
            if (dependencyCommand == null)
            {
                throw new ArgumentNullException("dependencyCommand");
            }
            if (finansstyringRepository == null)
            {
                throw new ArgumentNullException("finansstyringRepository");
            }
            _dependencyCommand = dependencyCommand;
            _finansstyringRepository = finansstyringRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returnerer angivelse af, om kommandoen kan udføres på den givne ViewModel.
        /// </summary>
        /// <param name="budgetkontoViewModel">ViewModel, hvorpå kommandoen skal udføres.</param>
        /// <returns>Angivelse af, om kommandoen kan udføres på den givne ViewModel.</returns>
        protected override bool CanExecute(IBudgetkontoViewModel budgetkontoViewModel)
        {
            return _isBusy == false;
        }

        /// <summary>
        /// Henter og opdaterer budgetkontoen.
        /// </summary>
        /// <param name="budgetkontoViewModel">ViewModel for budgetkontoen, der skal hentes og opdateres.</param>
        protected override void Execute(IBudgetkontoViewModel budgetkontoViewModel)
        {
            var regnskabViewModel = budgetkontoViewModel.Regnskab;
            Task dependencyCommandTask = null;
            if (_dependencyCommand.CanExecute(regnskabViewModel))
            {
                _dependencyCommand.Execute(regnskabViewModel);
                dependencyCommandTask = _dependencyCommand.ExecuteTask;
            }
            _isBusy = true;
            var task = _finansstyringRepository.BudgetkontoGetAsync(regnskabViewModel.Nummer, budgetkontoViewModel.Kontonummer, budgetkontoViewModel.StatusDato);
            ExecuteTask = task.ContinueWith(t =>
                {
                    try
                    {
                        if (dependencyCommandTask != null)
                        {
                            dependencyCommandTask.Wait();
                        }
                        HandleResultFromTask(t, budgetkontoViewModel, new List<IBudgetkontogruppeViewModel>(regnskabViewModel.Budgetkontogrupper), HandleResult);
                    }
                    finally
                    {
                        _isBusy = false;
                    }
                });
        }

        /// <summary>
        /// Opdaterer ViewModel for budgetkontoen.
        /// </summary>
        /// <param name="budgetkontoViewModel">ViewModel for budgetkontoen, som skal opdateres.</param>
        /// <param name="budgetkontoModel">Model for budgetkontoen, som ViewModel for budgetkontoen skal opdateres med.</param>
        /// <param name="budgetkontogruppeViewModels">ViewModels for kontogrupper til budgetkonti..</param>
        private static void HandleResult(IBudgetkontoViewModel budgetkontoViewModel, IBudgetkontoModel budgetkontoModel, IEnumerable<IBudgetkontogruppeViewModel> budgetkontogruppeViewModels)
        {
            if (budgetkontoViewModel == null)
            {
                throw new ArgumentNullException("budgetkontoViewModel");
            }
            if (budgetkontoModel == null)
            {
                throw new ArgumentNullException("budgetkontoModel");
            }
            if (budgetkontogruppeViewModels == null)
            {
                throw new ArgumentNullException("budgetkontogruppeViewModels");
            }
            var budgetkontogruppeViewModel = budgetkontogruppeViewModels.SingleOrDefault(m => m.Nummer == budgetkontoModel.Kontogruppe);
            if (budgetkontogruppeViewModel == null)
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
