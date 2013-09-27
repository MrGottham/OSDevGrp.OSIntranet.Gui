using System;
using System.Linq;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Events;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core.Commands;
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
        
        #endregion

        #region Constructor

        /// <summary>
        /// Danner kommando til genindlæsning af regnskabslisten.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        public RegnskabslisteRefreshCommand(IFinansstyringRepository finansstyringRepository)
        {
            if (finansstyringRepository == null)
            {
                throw new ArgumentNullException("finansstyringRepository");
            }
            _finansstyringRepository = finansstyringRepository;
        }

        #endregion

        #region Events

        /// <summary>
        /// Event, der rejses ved exception.
        /// </summary>
        public virtual event IntranetGuiEventHandler<IHandleExceptionEventArgs> OnException;
        
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
                                HandleException(t.Exception);
                            }
                            return;
                        }
                        foreach (var regnskabModel in t.Result)
                        {
                            var regnskabViewModel = regnskabslisteViewModel.Regnskaber.SingleOrDefault(m => m.Nummer == regnskabModel.Nummer);
                            if (regnskabViewModel != null)
                            {
                                regnskabViewModel.Navn = regnskabModel.Navn;
                                continue;
                            }
                            regnskabslisteViewModel.RegnskabAdd(new RegnskabViewModel(regnskabModel));
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
                }, TaskContinuationOptions.AttachedToParent);
        }

        /// <summary>
        /// Håndtering af exception ved udførelse af en kommando.
        /// </summary>
        /// <param name="exception">Exception.</param>
        protected override void HandleException(Exception exception)
        {
            var error = exception;
            if (error is AggregateException)
            {
                if (error.InnerException != null)
                {
                    error = error.InnerException;
                }
            }
            if (OnException != null)
            {
                OnException.Invoke(this, new HandleExceptionEventArgs(error));
                return;
            }
            base.HandleException(error);
        }

        #endregion
    }
}
