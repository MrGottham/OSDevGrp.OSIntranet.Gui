using System;
using System.Threading;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.Commands
{
    /// <summary>
    /// Kommando, der kan hente og opdaterer en adressekonto.
    /// </summary>
    public class AdressekontoGetCommand : ViewModelCommandBase<IAdressekontoViewModel>
    {
        #region Private variables

        private bool _isBusy;
        private readonly IFinansstyringRepository _finansstyringRepository;
        private readonly SynchronizationContext _synchronizationContext;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner en kommando, der kan hente og opdaterer en adressekonto.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="exceptionHandlerViewModel">Implementering af en ViewModel til en exceptionhandler.</param>
        public AdressekontoGetCommand(IFinansstyringRepository finansstyringRepository, IExceptionHandlerViewModel exceptionHandlerViewModel)
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
        protected override bool CanExecute(IAdressekontoViewModel viewModel)
        {
            return _isBusy == false;
        }

        /// <summary>
        /// Henter og opdaterer adressekontoen.
        /// </summary>
        /// <param name="viewModel">ViewModel for kontoen, der skal hentes og opdateres.</param>
        protected override void Execute(IAdressekontoViewModel viewModel)
        {
            _isBusy = true;
            var task = _finansstyringRepository.AdressekontoGetAsync(viewModel.Regnskab.Nummer, viewModel.Nummer, viewModel.StatusDato);
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
                        HandleAdressekontoModel(viewModel, t.Result, _synchronizationContext);
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
        /// Håndterer en hentet model for en adressekonto.
        /// </summary>
        /// <param name="adressekontoViewModel">ViewModel for adressekontoen, der skal opdateres.</param>
        /// <param name="adressekontoModel">Model for den hentede adressekonto.</param>
        /// <param name="synchronizationContext">Synkroniseringskontekst.</param>
        private static void HandleAdressekontoModel(IAdressekontoViewModel adressekontoViewModel, IAdressekontoModel adressekontoModel, SynchronizationContext synchronizationContext)
        {
            if (adressekontoViewModel == null)
            {
                throw new ArgumentNullException("adressekontoViewModel");
            }
            if (adressekontoModel == null)
            {
                throw new ArgumentNullException("adressekontoModel");
            }
            if (synchronizationContext == null)
            {
                adressekontoViewModel.Navn = adressekontoModel.Navn;
                adressekontoViewModel.PrimærTelefon = adressekontoModel.PrimærTelefon;
                adressekontoViewModel.SekundærTelefon = adressekontoModel.SekundærTelefon;
                adressekontoViewModel.StatusDato = adressekontoModel.StatusDato;
                adressekontoViewModel.Saldo = adressekontoModel.Saldo;
                return;
            }
            var arguments = new Tuple<IAdressekontoViewModel, IAdressekontoModel>(adressekontoViewModel, adressekontoModel);
            synchronizationContext.Post(obj =>
                {
                    var tuple = (Tuple<IAdressekontoViewModel, IAdressekontoModel>) obj;
                    HandleAdressekontoModel(tuple.Item1, tuple.Item2, null);
                }, arguments);
        }

        #endregion
    }
}
