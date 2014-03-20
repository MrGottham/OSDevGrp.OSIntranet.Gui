using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.Commands
{
    /// <summary>
    /// Kommando, der kan hente og opdatere en kontoen.
    /// </summary>
    public class KontoGetCommand : ViewModelCommandBase<IKontoViewModel>, ITaskableCommand
    {
        #region Private variables

        private bool _isBusy;
        private readonly ITaskableCommand _dependencyCommand;
        private readonly IFinansstyringRepository _finansstyringRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner en kommando, der kan hente og opdatere en kontoen.
        /// </summary>
        /// <param name="dependencyCommand">Implementering af kommando, som denne kommando er afhængig af.</param>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="exceptionHandlerViewModel">Implementering af ViewModel til en exceptionhandler.</param>
        public KontoGetCommand(ITaskableCommand dependencyCommand, IFinansstyringRepository finansstyringRepository, IExceptionHandlerViewModel exceptionHandlerViewModel)
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
        /// <param name="kontoViewModel">ViewModel, hvorpå kommandoen skal udføres.</param>
        /// <returns>Angivelse af, om kommandoen kan udføres på den givne ViewModel.</returns>
        protected override bool CanExecute(IKontoViewModel kontoViewModel)
        {
            return _isBusy == false;
        }

        /// <summary>
        /// Henter og opdaterer kontoen.
        /// </summary>
        /// <param name="kontoViewModel">ViewModel for kontoen, der skal hentes og opdateres.</param>
        protected override void Execute(IKontoViewModel kontoViewModel)
        {
            var regnskabViewModel = kontoViewModel.Regnskab;
            Task dependencyCommandTask = null;
            if (_dependencyCommand.CanExecute(regnskabViewModel))
            {
                _dependencyCommand.Execute(regnskabViewModel);
                dependencyCommandTask = _dependencyCommand.ExecuteTask;
            }
            _isBusy = true;
            var task = _finansstyringRepository.KontoGetAsync(regnskabViewModel.Nummer, kontoViewModel.Kontonummer, kontoViewModel.StatusDato);
            ExecuteTask = task.ContinueWith(t =>
                {
                    try
                    {
                        if (dependencyCommandTask != null)
                        {
                            dependencyCommandTask.Wait();
                        }
                        HandleResultFromTask(t, kontoViewModel, new List<IKontogruppeViewModel>(regnskabViewModel.Kontogrupper), HandleResult);
                    }
                    finally
                    {
                        _isBusy = false;
                    }
                });
        }

        /// <summary>
        /// Opdaterer ViewModel for kontoen.
        /// </summary>
        /// <param name="kontoViewModel">ViewModel for kontoen, som skal opdateres.</param>
        /// <param name="kontoModel">Model for kontoen, som ViewModel for kontoen skal opdateres med.</param>
        /// <param name="kontogruppeViewModels">ViewModels for kontogrupper.</param>
        private static void HandleResult(IKontoViewModel kontoViewModel, IKontoModel kontoModel, IEnumerable<IKontogruppeViewModel> kontogruppeViewModels)
        {
            if (kontoViewModel == null)
            {
                throw new ArgumentNullException("kontoViewModel");
            }
            if (kontoModel == null)
            {
                throw new ArgumentNullException("kontoModel");
            }
            if (kontogruppeViewModels == null)
            {
                throw new ArgumentNullException("kontogruppeViewModels");
            }
            var kontogruppeViewModel = kontogruppeViewModels.SingleOrDefault(m => m.Nummer == kontoModel.Kontogruppe);
            if (kontogruppeViewModel == null)
            {
                throw new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.AccountGroupNotFound, kontoModel.Kontogruppe));
            }
            kontoViewModel.Kontonavn = kontoModel.Kontonavn;
            kontoViewModel.Beskrivelse = kontoModel.Beskrivelse;
            kontoViewModel.Notat = kontoModel.Notat;
            kontoViewModel.Kontogruppe = kontogruppeViewModel;
            kontoViewModel.StatusDato = kontoModel.StatusDato;
            kontoViewModel.Kredit = kontoModel.Kredit;
            kontoViewModel.Saldo = kontoModel.Saldo;
        }

        #endregion
    }
}
