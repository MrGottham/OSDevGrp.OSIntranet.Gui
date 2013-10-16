using System;
using System.Linq;
using System.Threading;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.Commands
{
    /// <summary>
    /// Kommando, der kan hente bogføringslinjer til et regnskab.
    /// </summary>
    public class BogføringslinjerGetCommand : ViewModelCommandBase<IRegnskabViewModel>
    {
        #region Private constants

        private const int AntalBogføringslinjer = 50;
        private const int DageForNyheder = 7;

        #endregion

        #region Private variables

        private bool _isBusy;
        private readonly IFinansstyringRepository _finansstyringRepository;
        private readonly SynchronizationContext _synchronizationContext;

        #endregion
        
        #region Constructor

        /// <summary>
        /// Danner kommando, der kan hente bogføringslinjer til et regnskab.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="exceptionHandlerViewModel">Implementering af en ViewModel til en exceptionhandler.</param>
        public BogføringslinjerGetCommand(IFinansstyringRepository finansstyringRepository, IExceptionHandlerViewModel exceptionHandlerViewModel)
            : base(exceptionHandlerViewModel)
        {
            if (finansstyringRepository == null)
            {
                throw new ArgumentNullException("finansstyringRepository");
            }
            _finansstyringRepository = finansstyringRepository;
            _synchronizationContext = SynchronizationContext.Current;
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
        /// Henter og indsætter bogføringslinjer til regnskabet.
        /// </summary>
        /// <param name="regnskabViewModel">ViewModel for et regnskabet, hvortil bogføringslinjer skal hentes og indsætter.</param>
        protected override void Execute(IRegnskabViewModel regnskabViewModel)
        {
            _isBusy = true;
            var task = _finansstyringRepository.BogføringslinjerGetAsync(regnskabViewModel.Nummer, regnskabViewModel.StatusDato, AntalBogføringslinjer);
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
                        foreach (var bogføringslinjeModel in t.Result.OrderBy(m => m.Dato))
                        {
                            HandleBogføringslinjeModel(regnskabViewModel, bogføringslinjeModel, _synchronizationContext);
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
        /// Håndtering af hentet model for en bogføringslinje.
        /// </summary>
        /// <param name="regnskabViewModel">ViewModel for regnskabet, der skal håndtere den hentede bogføringslinje.</param>
        /// <param name="bogføringslinjeModel">Model for den hentede bogføringslinje.</param>
        /// <param name="synchronizationContext">Synkroniseringskontekst.</param>
        private static void HandleBogføringslinjeModel(IRegnskabViewModel regnskabViewModel, IBogføringslinjeModel bogføringslinjeModel, SynchronizationContext synchronizationContext)
        {
            if (regnskabViewModel == null)
            {
                throw new ArgumentNullException("regnskabViewModel");
            }
            if (bogføringslinjeModel == null)
            {
                throw new ArgumentNullException("bogføringslinjeModel");
            }
            if (synchronizationContext == null)
            {
                if (regnskabViewModel.Bogføringslinjer.Any(m => m.Løbenummer == bogføringslinjeModel.Løbenummer))
                {
                    return;
                }
                var bogføringslinjeViewModel = new BogføringslinjeViewModel(regnskabViewModel, bogføringslinjeModel);
                regnskabViewModel.BogføringslinjeAdd(bogføringslinjeViewModel);
                if (bogføringslinjeModel.Dato.Date.CompareTo(regnskabViewModel.StatusDato.AddDays(DageForNyheder*-1).Date) >= 0 && bogføringslinjeModel.Dato.Date.CompareTo(regnskabViewModel.StatusDato.Date) <= 0)
                {
                    regnskabViewModel.NyhedAdd(new NyhedViewModel(bogføringslinjeModel, bogføringslinjeViewModel.Image));
                }
                return;
            }
            var arguments = new Tuple<IRegnskabViewModel, IBogføringslinjeModel>(regnskabViewModel, bogføringslinjeModel);
            synchronizationContext.Post(obj =>
                {
                    var tuple = (Tuple<IRegnskabViewModel, IBogføringslinjeModel>) obj;
                    HandleBogføringslinjeModel(tuple.Item1, tuple.Item2, null);
                }, arguments);
        }

        #endregion
    }
}
