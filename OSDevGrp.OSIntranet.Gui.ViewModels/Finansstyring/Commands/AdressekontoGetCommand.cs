using System;
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
    public class AdressekontoGetCommand : ViewModelCommandBase<IAdressekontoViewModel>, ITaskableCommand
    {
        #region Private variables

        private bool _isBusy;
        private readonly IFinansstyringRepository _finansstyringRepository;

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
        /// <param name="viewModel">ViewModel for adressekontoen, der skal hentes og opdateres.</param>
        protected override void Execute(IAdressekontoViewModel viewModel)
        {
            _isBusy = true;
            var task = _finansstyringRepository.AdressekontoGetAsync(viewModel.Regnskab.Nummer, viewModel.Nummer, viewModel.StatusDato);
            ExecuteTask = task.ContinueWith(t =>
                {
                    try
                    {
                        HandleResultFromTask(t, viewModel, new object(), HandleResult);
                    }
                    finally
                    {
                        _isBusy = false;
                    }
                });
        }

        /// <summary>
        /// Opdaterer ViewModel for adressekontoen.
        /// </summary>
        /// <param name="adressekontoViewModel">ViewModel for adressenkontoen, der skal opdateres.</param>
        /// <param name="adressekontoModel">Model for adressekontoen, som ViewModel for adressekontoen skal opdateres med.</param>
        /// <param name="argument">Argument, der benyttes til opdatering af ViewModel for adressekontoen.</param>
        private static void HandleResult(IAdressekontoViewModel adressekontoViewModel, IAdressekontoModel adressekontoModel, object argument)
        {
            if (adressekontoViewModel == null)
            {
                throw new ArgumentNullException("adressekontoViewModel");
            }
            if (adressekontoModel == null)
            {
                throw new ArgumentNullException("adressekontoModel");
            }
            if (argument == null)
            {
                throw new ArgumentNullException("argument");
            }
            adressekontoViewModel.Navn = adressekontoModel.Navn;
            adressekontoViewModel.PrimærTelefon = adressekontoModel.PrimærTelefon;
            adressekontoViewModel.SekundærTelefon = adressekontoModel.SekundærTelefon;
            adressekontoViewModel.StatusDato = adressekontoModel.StatusDato;
            adressekontoViewModel.Saldo = adressekontoModel.Saldo;
        }

        #endregion
    }
}
