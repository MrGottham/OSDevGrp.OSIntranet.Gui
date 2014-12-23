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
    /// Kommando, der kan hente og opdatere budgetkontoplanen til et givent regnskab.
    /// </summary>
    public class BudgetkontoplanGetCommand : ViewModelCommandBase<IRegnskabViewModel>, ITaskableCommand
    {
        #region Private variables

        private bool _isBusy;
        private readonly ITaskableCommand _dependencyCommand;
        private readonly IFinansstyringRepository _finansstyringRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner en kommando, der kan hente og opdaterer kontoplanen til et givent regnskab.
        /// </summary>
        /// <param name="dependencyCommand">Implementering af kommando, som denne kommando er afhængig af.</param>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="exceptionHandlerViewModel">Implementering af ViewModel til en exceptionhandler.</param>
        public BudgetkontoplanGetCommand(ITaskableCommand dependencyCommand, IFinansstyringRepository finansstyringRepository, IExceptionHandlerViewModel exceptionHandlerViewModel)
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
        /// <param name="regnskabViewModel">ViewModel, hvorpå kommandoen skal udføres.</param>
        /// <returns>Angivelse af, om kommandoen kan udføres på den givne ViewModel.</returns>
        protected override bool CanExecute(IRegnskabViewModel regnskabViewModel)
        {
            return _isBusy == false;
        }

        /// <summary>
        /// Henter og opdaterer budgetkontoplanen til regnskabet.
        /// </summary>
        /// <param name="regnskabViewModel">ViewModel for regnskabet, hvortil budgetkontoplanen skal hentes og opdateres.</param>
        protected override void Execute(IRegnskabViewModel regnskabViewModel)
        {
            Task dependencyCommandTask = null;
            if (_dependencyCommand.CanExecute(regnskabViewModel))
            {
                _dependencyCommand.Execute(regnskabViewModel);
                dependencyCommandTask = _dependencyCommand.ExecuteTask;
            }
            _isBusy = true;
            var task = _finansstyringRepository.BudgetkontoplanGetAsync(regnskabViewModel.Nummer, regnskabViewModel.StatusDato);
            ExecuteTask = task.ContinueWith(t =>
                {
                    try
                    {
                        if (dependencyCommandTask != null)
                        {
                            dependencyCommandTask.Wait();
                        }
                        HandleResultFromTask(t, regnskabViewModel, new List<IBudgetkontogruppeViewModel>(regnskabViewModel.Budgetkontogrupper), HandleResult);
                    }
                    finally
                    {
                        _isBusy = false;
                    }
                });
        }

        /// <summary>
        /// Opdaterer budgetkontoplanen på regnskabet.
        /// </summary>
        /// <param name="regnskabViewModel">ViewModel for regnskabet, hvor budgetkontoplanen skal opdateres.</param>
        /// <param name="budgetkontoModels">Modeller for budgetkonti, som budgetkontoplanen skal opdateres med.</param>
        /// <param name="budgetkontogruppeViewModels">ViewModels for kontogrupper til budgetkonti.</param>
        private void HandleResult(IRegnskabViewModel regnskabViewModel, IEnumerable<IBudgetkontoModel> budgetkontoModels, IEnumerable<IBudgetkontogruppeViewModel> budgetkontogruppeViewModels)
        {
            if (regnskabViewModel == null)
            {
                throw new ArgumentNullException("regnskabViewModel");
            }
            if (budgetkontoModels == null)
            {
                throw new ArgumentNullException("budgetkontoModels");
            }
            if (budgetkontogruppeViewModels == null)
            {
                throw new ArgumentNullException("budgetkontogruppeViewModels");
            }
            var budgetkontogruppeViewModelCollection = budgetkontogruppeViewModels.ToArray();
            var budgetkontoViewModelCollection = new List<IBudgetkontoViewModel>(regnskabViewModel.Budgetkonti);
            foreach (var budgetkontoModel in budgetkontoModels)
            {
                try
                {
                    var budgetkontogruppeViewModel = budgetkontogruppeViewModelCollection.SingleOrDefault(m => m.Nummer == budgetkontoModel.Kontogruppe);
                    if (budgetkontogruppeViewModel == null)
                    {
                        continue;
                    }
                    var budgetkontoViewModel = regnskabViewModel.Budgetkonti.FirstOrDefault(m => string.Compare(m.Kontonummer, budgetkontoModel.Kontonummer, StringComparison.Ordinal) == 0);
                    if (budgetkontoViewModel == null)
                    {
                        regnskabViewModel.BudgetkontoAdd(new BudgetkontoViewModel(regnskabViewModel, budgetkontoModel, budgetkontogruppeViewModel, _finansstyringRepository, ExceptionHandler));
                        continue;
                    }
                    budgetkontoViewModel.Kontonavn = budgetkontoModel.Kontonavn;
                    budgetkontoViewModel.Beskrivelse = budgetkontoModel.Beskrivelse;
                    budgetkontoViewModel.Notat = budgetkontoModel.Notat;
                    budgetkontoViewModel.Kontogruppe = budgetkontogruppeViewModel;
                    budgetkontoViewModel.StatusDato = budgetkontoModel.StatusDato;
                    budgetkontoViewModel.Indtægter = budgetkontoModel.Indtægter;
                    budgetkontoViewModel.Udgifter = budgetkontoModel.Udgifter;
                    budgetkontoViewModel.BudgetSidsteMåned = budgetkontoModel.BudgetSidsteMåned;
                    budgetkontoViewModel.BudgetÅrTilDato = budgetkontoModel.BudgetÅrTilDato;
                    budgetkontoViewModel.BudgetSidsteÅr = budgetkontoModel.BudgetSidsteÅr;
                    budgetkontoViewModel.Bogført = budgetkontoModel.Bogført;
                    budgetkontoViewModel.BogførtSidsteMåned = budgetkontoModel.BogførtSidsteMåned;
                    budgetkontoViewModel.BogførtÅrTilDato = budgetkontoModel.BogførtÅrTilDato;
                    budgetkontoViewModel.BogførtSidsteÅr = budgetkontoModel.BogførtSidsteÅr;
                }
                finally
                {
                    var budgetkontoViewModel = budgetkontoViewModelCollection.FirstOrDefault(m => string.Compare(m.Kontonummer, budgetkontoModel.Kontonummer, StringComparison.Ordinal) == 0);
                    while (budgetkontoViewModel != null)
                    {
                        budgetkontoViewModelCollection.Remove(budgetkontoViewModel);
                        budgetkontoViewModel = budgetkontoViewModelCollection.FirstOrDefault(m => string.Compare(m.Kontonummer, budgetkontoModel.Kontonummer, StringComparison.Ordinal) == 0);
                    }
                }
            }
            foreach (var unreadedBudgetkontoViewModel in budgetkontoViewModelCollection)
            {
                var refreshCommand = unreadedBudgetkontoViewModel.RefreshCommand;
                if (refreshCommand.CanExecute(unreadedBudgetkontoViewModel))
                {
                    refreshCommand.Execute(unreadedBudgetkontoViewModel);
                }
            }
        }

        #endregion
    }
}
