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
    /// Kommando, der kan hente og opdaterer kontoplanen til et givent regnskab.
    /// </summary>
    public class KontoplanGetCommand : ViewModelCommandBase<IRegnskabViewModel>, ITaskableCommand
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
        public KontoplanGetCommand(ITaskableCommand dependencyCommand, IFinansstyringRepository finansstyringRepository, IExceptionHandlerViewModel exceptionHandlerViewModel)
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
        /// Henter og opdaterer kontoplanen til regnskabet.
        /// </summary>
        /// <param name="regnskabViewModel">ViewModel for regnskabet, hvortil kontoplanen skal hentes og opdateres.</param>
        protected override void Execute(IRegnskabViewModel regnskabViewModel)
        {
            Task dependencyCommandTask = null;
            if (_dependencyCommand.CanExecute(regnskabViewModel))
            {
                _dependencyCommand.Execute(regnskabViewModel);
                dependencyCommandTask = _dependencyCommand.ExecuteTask;
            }
            _isBusy = true;
            var task = _finansstyringRepository.KontoplanGetAsync(regnskabViewModel.Nummer, regnskabViewModel.StatusDato);
            ExecuteTask = task.ContinueWith(t =>
                {
                    try
                    {
                        if (dependencyCommandTask != null)
                        {
                            dependencyCommandTask.Wait();
                        }
                        HandleResultFromTask(t, regnskabViewModel, new List<IKontogruppeViewModel>(regnskabViewModel.Kontogrupper), HandleResult);
                    }
                    finally
                    {
                        _isBusy = false;
                    }
                });
        }

        /// <summary>
        /// Opdaterer kontoplanen på regnskabet.
        /// </summary>
        /// <param name="regnskabViewModel">ViewModel for regnskabet, hvor kontoplanen skal opdateres.</param>
        /// <param name="kontoModels">Modeller for konti, som kontoplanen skal opdateres med.</param>
        /// <param name="kontogruppeViewModels">ViewModels for kontogrupper.</param>
        private void HandleResult(IRegnskabViewModel regnskabViewModel, IEnumerable<IKontoModel> kontoModels, IEnumerable<IKontogruppeViewModel> kontogruppeViewModels)
        {
            if (regnskabViewModel == null)
            {
                throw new ArgumentNullException("regnskabViewModel");
            }
            if (kontoModels == null)
            {
                throw new ArgumentNullException("kontoModels");
            }
            if (kontogruppeViewModels == null)
            {
                throw new ArgumentNullException("kontogruppeViewModels");
            }
            var kontogruppeViewModelCollection = kontogruppeViewModels.ToArray();
            var kontoViewModelCollection = new List<IKontoViewModel>(regnskabViewModel.Konti);
            foreach (var kontoModel in kontoModels)
            {
                try
                {
                    var kontogruppeViewModel = kontogruppeViewModelCollection.SingleOrDefault(m => m.Nummer == kontoModel.Kontogruppe);
                    if (kontogruppeViewModel == null)
                    {
                        continue;
                    }
                    var kontoViewModel = regnskabViewModel.Konti.FirstOrDefault(m => string.Compare(m.Kontonummer, kontoModel.Kontonummer, StringComparison.Ordinal) == 0);
                    if (kontoViewModel == null)
                    {
                        regnskabViewModel.KontoAdd(new KontoViewModel(regnskabViewModel, kontoModel, kontogruppeViewModel, _finansstyringRepository, ExceptionHandler));
                        continue;
                    }
                    kontoViewModel.Kontonavn = kontoModel.Kontonavn;
                    kontoViewModel.Beskrivelse = kontoModel.Beskrivelse;
                    kontoViewModel.Notat = kontoModel.Notat;
                    kontoViewModel.Kontogruppe = kontogruppeViewModel;
                    kontoViewModel.StatusDato = kontoModel.StatusDato;
                    kontoViewModel.Kredit = kontoModel.Kredit;
                    kontoViewModel.Saldo = kontoModel.Saldo;
                }
                finally
                {
                    var kontoViewModel = kontoViewModelCollection.FirstOrDefault(m => string.Compare(m.Kontonummer, kontoModel.Kontonummer, StringComparison.Ordinal) == 0);
                    while (kontoViewModel != null)
                    {
                        kontoViewModelCollection.Remove(kontoViewModel);
                        kontoViewModel = kontoViewModelCollection.FirstOrDefault(m => string.Compare(m.Kontonummer, kontoModel.Kontonummer, StringComparison.Ordinal) == 0);
                    }
                }
            }
            foreach (var unreadedKontoViewModel in kontoViewModelCollection)
            {
                var refreshCommand = unreadedKontoViewModel.RefreshCommand;
                if (refreshCommand.CanExecute(unreadedKontoViewModel))
                {
                    refreshCommand.Execute(unreadedKontoViewModel);
                }
            }
        }

        #endregion
    }
}
