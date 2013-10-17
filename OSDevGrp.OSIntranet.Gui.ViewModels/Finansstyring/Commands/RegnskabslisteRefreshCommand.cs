using System;
using System.Linq;
using System.Threading;
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
    public class RegnskabslisteRefreshCommand : ViewModelCommandBase<IRegnskabslisteViewModel>
    {
        #region Private variables

        private bool _isBusy;
        private readonly IFinansstyringRepository _finansstyringRepository;
        private readonly SynchronizationContext _synchronizationContext;
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
            _synchronizationContext = SynchronizationContext.Current;
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
            task.ContinueWith(t =>
                {
                    try
                    {
                        if (t.IsCanceled || t.IsFaulted)
                        {
                            if (t.Exception != null)
                            {
                                t.Exception.Handle(exception =>
                                    {
                                        HandleException(exception);
                                        return true;
                                    });
                            }
                            return;
                        }
                        UpdateRegnskabslisteViewModel(regnskabslisteViewModel, _synchronizationContext);
                        foreach (var regnskabModel in t.Result)
                        {
                            HandleRegnskabModel(regnskabslisteViewModel, regnskabModel, _finansstyringRepository, ExceptionHandler, _synchronizationContext);
                        }
                        if (_onFinish != null)
                        {
                            InvokeOnFinish(regnskabslisteViewModel, _onFinish, _synchronizationContext);
                        }
                    }
                    catch (Exception ex)
                    {
                        HandleException(ex);
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
        /// <param name="regnskabslisteViewModel">ViewModel for regnskabslisten.</param>
        /// <param name="synchronizationContext">Synkroniseringskontekst.</param>
        private static void UpdateRegnskabslisteViewModel(IRegnskabslisteViewModel regnskabslisteViewModel, SynchronizationContext synchronizationContext)
        {
            if (regnskabslisteViewModel == null)
            {
                throw new ArgumentNullException("regnskabslisteViewModel");
            }
            if (synchronizationContext == null)
            {
                regnskabslisteViewModel.StatusDato = DateTime.Now;
                return;
            }
            using (var waitEvent = new AutoResetEvent(false))
            {
                var we = waitEvent;
                synchronizationContext.Post(obj =>
                    {
                        try
                        {
                            UpdateRegnskabslisteViewModel(obj as IRegnskabslisteViewModel, null);
                        }
                        finally
                        {
                            we.Set();
                        }
                    }, regnskabslisteViewModel);
                waitEvent.WaitOne();
            }
        }

        /// <summary>
        /// Håndtering af modeller for hentede regnskaber.
        /// </summary>
        /// <param name="regnskabslisteViewModel">ViewModel for regnskabslisten, hvorpå hentede modeller skal håndteres.</param>
        /// <param name="regnskabModel">Model for det hentede regnskab, der skal håndteres.</param>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="exceptionHandlerViewModel">Implementering af en ViewModel til en exceptionhandler.</param>
        /// <param name="synchronizationContext">Synkroniseringskontekst.</param>
        private static void HandleRegnskabModel(IRegnskabslisteViewModel regnskabslisteViewModel, IRegnskabModel regnskabModel, IFinansstyringRepository finansstyringRepository, IExceptionHandlerViewModel exceptionHandlerViewModel, SynchronizationContext synchronizationContext)
        {
            if (regnskabslisteViewModel == null)
            {
                throw new ArgumentNullException("regnskabslisteViewModel");
            }
            if (regnskabModel == null)
            {
                throw new ArgumentNullException("regnskabModel");
            }
            if (finansstyringRepository == null)
            {
                throw new ArgumentNullException("finansstyringRepository");
            }
            if (exceptionHandlerViewModel == null)
            {
                throw new ArgumentNullException("exceptionHandlerViewModel");
            }
            if (synchronizationContext == null)
            {
                var regnskabViewModel = regnskabslisteViewModel.Regnskaber.SingleOrDefault(m => m.Nummer == regnskabModel.Nummer);
                if (regnskabViewModel != null)
                {
                    regnskabViewModel.Navn = regnskabModel.Navn;
                    regnskabViewModel.StatusDato = regnskabslisteViewModel.StatusDato;
                    return;
                }
                regnskabslisteViewModel.RegnskabAdd(new RegnskabViewModel(regnskabModel, regnskabslisteViewModel.StatusDato, finansstyringRepository, exceptionHandlerViewModel));
                return;
            }
            using (var waitEvent = new AutoResetEvent(false))
            {
                var we = waitEvent;
                var arguments = new Tuple<IRegnskabslisteViewModel, IRegnskabModel, IFinansstyringRepository, IExceptionHandlerViewModel>(regnskabslisteViewModel, regnskabModel, finansstyringRepository, exceptionHandlerViewModel);
                synchronizationContext.Post(obj =>
                    {
                        try
                        {
                            var tuple = (Tuple<IRegnskabslisteViewModel, IRegnskabModel, IFinansstyringRepository, IExceptionHandlerViewModel>) obj;
                            HandleRegnskabModel(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, null);
                        }
                        finally
                        {
                            we.Set();
                        }
                    }, arguments);
                waitEvent.WaitOne();
            }
        }

        /// <summary>
        /// Invoker callbackmetode, der udføres, når kommandoen er udført fejlfrit.
        /// </summary>
        /// <param name="regnskabslisteViewModel">ViewModel for regnskabslisten, hvormed callbackmetoden skal invokes.</param>
        /// <param name="onFinish">Callbackmetode, der udføres, når kommandoen er udført fejlfrit.</param>
        /// <param name="synchronizationContext">Synkroniseringskontekst.</param>
        private static void InvokeOnFinish(IRegnskabslisteViewModel regnskabslisteViewModel, Action<IRegnskabslisteViewModel> onFinish, SynchronizationContext synchronizationContext)
        {
            if (regnskabslisteViewModel == null)
            {
                throw new ArgumentNullException("regnskabslisteViewModel");
            }
            if (onFinish == null)
            {
                throw new ArgumentNullException("onFinish");
            }
            if (synchronizationContext == null)
            {
                onFinish.Invoke(regnskabslisteViewModel);
                return;
            }
            var arguments = new Tuple<IRegnskabslisteViewModel, Action<IRegnskabslisteViewModel>>(regnskabslisteViewModel, onFinish);
            synchronizationContext.Post(obj =>
                {
                    var tuple = (Tuple<IRegnskabslisteViewModel, Action<IRegnskabslisteViewModel>>) obj;
                    InvokeOnFinish(tuple.Item1, tuple.Item2, null);
                }, arguments);
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
