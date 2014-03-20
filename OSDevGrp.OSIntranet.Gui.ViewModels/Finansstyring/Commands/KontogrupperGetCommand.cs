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
    /// Kommando, der kan hente kontogrupper til et regnskab.
    /// </summary>
    public class KontogrupperGetCommand : ViewModelCommandBase<IRegnskabViewModel>, ITaskableCommand
    {
        #region Private variables

        private bool _isBusy;
        private readonly IFinansstyringRepository _finansstyringRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner en kommando, der kan hente kontogrupper til et regnskab.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="exceptionHandlerViewModel">Implementering af ViewModel til en exceptionhandler.</param>
        public KontogrupperGetCommand(IFinansstyringRepository finansstyringRepository, IExceptionHandlerViewModel exceptionHandlerViewModel)
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
        /// Henter og opdaterer regnskabet med kontogrupper.
        /// </summary>
        /// <param name="regnskabViewModel">ViewModel for regnskabet, hvorpå kontogrupper skal opdateres.</param>
        protected override void Execute(IRegnskabViewModel regnskabViewModel)
        {
            var kontogrupperViewModels = new List<IKontogruppeViewModel>(regnskabViewModel.Kontogrupper);
            if (kontogrupperViewModels.Any())
            {
                return;
            }
            _isBusy = true;
            var task = _finansstyringRepository.KontogruppelisteGetAsync();
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
        /// Indsætter ViewModels for kontogrupper i regnskabet.
        /// </summary>
        /// <param name="regnskabViewModel">ViewModel for regnskabet, hvorpå kontogrupper skal indsættes.</param>
        /// <param name="kontogruppeModels">Kontogrupper, der skal indsættes i regnskabet.</param>
        /// <param name="argument">Argument til indsættelse af ViewModels for kontogrupper i regnskabet.</param>
        private void HandleResult(IRegnskabViewModel regnskabViewModel, IEnumerable<IKontogruppeModel> kontogruppeModels, object argument)
        {
            if (regnskabViewModel == null)
            {
                throw new ArgumentNullException("regnskabViewModel");
            }
            if (kontogruppeModels == null)
            {
                throw new ArgumentNullException("kontogruppeModels");
            }
            if (argument == null)
            {
                throw new ArgumentNullException("argument");
            }
            foreach (var kontogruppeModel in kontogruppeModels)
            {
                regnskabViewModel.KontogruppeAdd(new KontogruppeViewModel(kontogruppeModel, ExceptionHandler));
            }
        }

        #endregion
    }
}
