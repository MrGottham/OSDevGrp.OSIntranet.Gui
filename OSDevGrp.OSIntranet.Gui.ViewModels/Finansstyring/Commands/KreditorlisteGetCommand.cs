using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.Commands
{
    /// <summary>
    /// Kommando, der kan hente og opdatere kreditorlisten til et regnskab.
    /// </summary>
    public class KreditorlisteGetCommand : ViewModelCommandBase<IRegnskabViewModel>
    {
        #region Private variables

        private bool _isBusy;
        private readonly IFinansstyringRepository _finansstyringRepository;
        private readonly SynchronizationContext _synchronizationContext;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner en kommando, der kan hente og opdatere kreditorlisten til et regnskab.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="exceptionHandlerViewModel">Implementering af en ViewModel til en exceptionhandler.</param>
        public KreditorlisteGetCommand(IFinansstyringRepository finansstyringRepository, IExceptionHandlerViewModel exceptionHandlerViewModel)
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
        /// <param name="viewModel">ViewModel, hvorpå kommandoen skal udføres.</param>
        /// <returns>Angivelse af, om kommandoen kan udføres på den givne ViewModel.</returns>
        protected override bool CanExecute(IRegnskabViewModel viewModel)
        {
            return _isBusy == false;
        }

        /// <summary>
        /// Henter og opdaterer kreditorlisten til et givent regnskab.
        /// </summary>
        /// <param name="viewModel">ViewModel for regnskabet, hvortil kreditorlisten skal hentes og opdateres.</param>
        protected override void Execute(IRegnskabViewModel viewModel)
        {
            _isBusy = true;
            var konfiguration = _finansstyringRepository.Konfiguration;
            var task = _finansstyringRepository.KreditorlisteGetAsync(viewModel.Nummer, viewModel.StatusDato);
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
                        var dageForNyheder = konfiguration.DageForNyheder;
                        var adressekontoViewModelCollection = new List<IAdressekontoViewModel>(viewModel.Kreditorer);
                        foreach (var adressekontoModel in t.Result.OrderBy(m => m.Nummer))
                        {
                            HandleAdressekontoModel(viewModel, adressekontoModel, dageForNyheder, _synchronizationContext);
                            var adressekontoViewModel = adressekontoViewModelCollection.FirstOrDefault(m => m.Nummer == adressekontoModel.Nummer);
                            while (adressekontoViewModel != null)
                            {
                                adressekontoViewModelCollection.Remove(adressekontoViewModel);
                                adressekontoViewModel = adressekontoViewModelCollection.FirstOrDefault(m => m.Nummer == adressekontoModel.Nummer);
                            }
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
        /// Håndtering af hentet adressekonto for en kreditor.
        /// </summary>
        /// <param name="regnskabViewModel">ViewModel for regnskabet, hvorpå den hentede adressekonto skal håndteres.</param>
        /// <param name="adressekontoModel">Model for den hentede adressekonto.</param>
        /// <param name="dageForNyheder">Antal dage, som nyheder er gældende.</param>
        /// <param name="synchronizationContext">Synkroniseringskontekst.</param>
        private void HandleAdressekontoModel(IRegnskabViewModel regnskabViewModel, IAdressekontoModel adressekontoModel, int dageForNyheder, SynchronizationContext synchronizationContext)
        {
            if (regnskabViewModel == null)
            {
                throw new ArgumentNullException("regnskabViewModel");
            }
            if (adressekontoModel == null)
            {
                throw new ArgumentNullException("adressekontoModel");
            }
            if (synchronizationContext == null)
            {
                var adressekontoViewModel = regnskabViewModel.Kreditorer.FirstOrDefault(m => m.Nummer == adressekontoModel.Nummer);
                if (adressekontoViewModel == null)
                {
                    regnskabViewModel.KreditorAdd(new AdressekontoViewModel(regnskabViewModel, adressekontoModel, Resource.GetText(Text.Creditor), Resource.GetEmbeddedResource("Images.Adressekonto.png"), _finansstyringRepository, ExceptionHandler));
                    return;
                }
                return;
            }
            var arguments = new Tuple<IRegnskabViewModel, IAdressekontoModel, int>(regnskabViewModel, adressekontoModel, dageForNyheder);
            synchronizationContext.Post(obj =>
                {
                    var tuple = (Tuple<IRegnskabViewModel, IAdressekontoModel, int>) obj;
                    HandleAdressekontoModel(tuple.Item1, tuple.Item2, tuple.Item3, null);
                }, arguments);
        }

        #endregion
    }
}
