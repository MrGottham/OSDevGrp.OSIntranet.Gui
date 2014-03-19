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
    /// Kommando til genindlæsning af regnskabslisten.
    /// </summary>
    public class RegnskabslisteRefreshCommand : ViewModelCommandBase<IRegnskabslisteViewModel>, ITaskableCommand
    {
        #region Private variables

        private bool _isBusy;
        private readonly IFinansstyringRepository _finansstyringRepository;
        private readonly Action<IRegnskabslisteViewModel> _onFinish;
        
        #endregion

        #region Constructors

        /// <summary>
        /// Danner kommando til genindlæsning af regnskabslisten.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="exceptionHandlerViewModel">Implementering af en ViewModel til en exceptionhandler.</param>
        public RegnskabslisteRefreshCommand(IFinansstyringRepository finansstyringRepository, IExceptionHandlerViewModel exceptionHandlerViewModel)
            : this(finansstyringRepository, exceptionHandlerViewModel, null)
        {
        }

        /// <summary>
        /// Danner kommando til genindlæsning af regnskabslisten.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="exceptionHandlerViewModel">Implementering af en ViewModel til en exceptionhandler.</param>
        /// <param name="onFinish">Callbackmetode, der udføres, når kommandoen er udført fejlfrit.</param>
        public RegnskabslisteRefreshCommand(IFinansstyringRepository finansstyringRepository, IExceptionHandlerViewModel exceptionHandlerViewModel, Action<IRegnskabslisteViewModel> onFinish)
            : base(exceptionHandlerViewModel)
        {
            if (finansstyringRepository == null)
            {
                throw new ArgumentNullException("finansstyringRepository");
            }
            _finansstyringRepository = finansstyringRepository;
            _onFinish = onFinish;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returnerer angivelse af, om kommandoen kan udføres på den givne ViewModel.
        /// </summary>
        /// <param name="regnskabslisteViewModel">ViewModel, hvorpå kommandoen skal udføres.</param>
        /// <returns>Angivelse af, om kommandoen kan udføres på den givne ViewModel.</returns>
        protected override bool CanExecute(IRegnskabslisteViewModel regnskabslisteViewModel)
        {
            return _isBusy == false;
        }

        /// <summary>
        /// Genindlæser regnskaber i regnskabslisten.
        /// </summary>
        /// <param name="regnskabslisteViewModel">ViewModel for en liste af regnskaber.</param>
        protected override void Execute(IRegnskabslisteViewModel regnskabslisteViewModel)
        {
            _isBusy = true;
            var task = _finansstyringRepository.RegnskabslisteGetAsync();
            ExecuteTask = task.ContinueWith(t =>
                {
                    try
                    {
                        var statusDato = DateTime.Now;
                        HandleResultFromTask(t, regnskabslisteViewModel, statusDato, HandleResult);
                    }
                    finally
                    {
                        _isBusy = false;
                    }
                });
        }

        /// <summary>
        /// Opdaterer ViewModel for regnskabslisten.
        /// </summary>
        /// <param name="regnskabslisteViewModel">ViewModel for regnskabslisten, der skal opdateres.</param>
        /// <param name="regnskabModels">Regnskaber, som ViewModel for regnskabslisten skal opdateres med.</param>
        /// <param name="statusDato">Statusdato, som ViewModel for regnskabslisten skal opdateres med.</param>
        private void HandleResult(IRegnskabslisteViewModel regnskabslisteViewModel, IEnumerable<IRegnskabModel> regnskabModels, DateTime statusDato)
        {
            if (regnskabslisteViewModel == null)
            {
                throw new ArgumentNullException("regnskabslisteViewModel");
            }
            if (regnskabModels == null)
            {
                throw new ArgumentNullException("regnskabModels");
            }
            regnskabslisteViewModel.StatusDato = statusDato;
            foreach (var regnskabModel in regnskabModels)
            {
                var regnskabViewModel = regnskabslisteViewModel.Regnskaber.SingleOrDefault(m => m.Nummer == regnskabModel.Nummer);
                if (regnskabViewModel != null)
                {
                    regnskabViewModel.Navn = regnskabModel.Navn;
                    regnskabViewModel.StatusDato = regnskabslisteViewModel.StatusDato;
                    continue;
                }
                regnskabslisteViewModel.RegnskabAdd(new RegnskabViewModel(regnskabModel, regnskabslisteViewModel.StatusDato, _finansstyringRepository, ExceptionHandler));
            }
            if (_onFinish == null)
            {
                return;
            }
            _onFinish.Invoke(regnskabslisteViewModel);
        }

        /// <summary>
        /// Udfører RefreshCommand på alle ViewModels for regnskaber i listen af regnskaber.
        /// </summary>
        /// <param name="regnskabslisteViewModel">ViewModel for en liste af regnskaber.</param>
        public static void ExecuteRefreshCommandOnRegnskabViewModels(IRegnskabslisteViewModel regnskabslisteViewModel)
        {
            if (regnskabslisteViewModel == null)
            {
                throw new ArgumentNullException("regnskabslisteViewModel");
            }
            foreach (var regnskabViewModel in regnskabslisteViewModel.Regnskaber)
            {
                var refreshCommand = regnskabViewModel.RefreshCommand;
                if (refreshCommand.CanExecute(regnskabViewModel))
                {
                    refreshCommand.Execute(regnskabViewModel);
                }
            }
        }

        #endregion
    }
}
