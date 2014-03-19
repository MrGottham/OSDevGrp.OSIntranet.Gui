using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.Commands
{
    /// <summary>
    /// Kommando, der kan hente og opdatere debitorlisten til et regnskab.
    /// </summary>
    public class DebitorlisteGetCommand : ViewModelCommandBase<IRegnskabViewModel>, ITaskableCommand
    {
        #region Private variables

        private bool _isBusy;
        private readonly IFinansstyringRepository _finansstyringRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner en kommando, der kan hente og opdatere debitorlisten til et regnskab.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="exceptionHandlerViewModel">Implementering af en ViewModel til en exceptionhandler.</param>
        public DebitorlisteGetCommand(IFinansstyringRepository finansstyringRepository, IExceptionHandlerViewModel exceptionHandlerViewModel)
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
        protected override bool CanExecute(IRegnskabViewModel viewModel)
        {
            return _isBusy == false;
        }

        /// <summary>
        /// Henter og opdaterer debitorlisten til et givent regnskab.
        /// </summary>
        /// <param name="viewModel">ViewModel for regnskabet, hvortil debitorlisten skal hentes og opdateres.</param>
        protected override void Execute(IRegnskabViewModel viewModel)
        {
            _isBusy = true;
            var konfiguration = _finansstyringRepository.Konfiguration;
            var task = _finansstyringRepository.DebitorlisteGetAsync(viewModel.Nummer, viewModel.StatusDato);
            ExecuteTask = task.ContinueWith(t =>
                {
                    try
                    {
                        var dageForNyheder = konfiguration.DageForNyheder;
                        HandleResultFromTask(t, viewModel, dageForNyheder, HandleResult);
                    }
                    finally
                    {
                        _isBusy = false;
                    }
                });
        }

        /// <summary>
        /// Opdaterer ViewModel for regnskabet med debitorer.
        /// </summary>
        /// <param name="regnskabViewModel">ViewModel for regnskabet, der skal opdateres.</param>
        /// <param name="adressekontoModels">Debitorer, som regnskabet skal opdateres med.</param>
        /// <param name="dageForNyheder">Antallet af dage, som nyheder er gældende.</param>
        private void HandleResult(IRegnskabViewModel regnskabViewModel, IEnumerable<IAdressekontoModel> adressekontoModels, int dageForNyheder)
        {
            if (regnskabViewModel == null)
            {
                throw new ArgumentNullException("regnskabViewModel");
            }
            if (adressekontoModels == null)
            {
                throw new ArgumentNullException("adressekontoModels");
            }
            var adressekontoViewModelCollection = new List<IAdressekontoViewModel>(regnskabViewModel.Debitorer);
            foreach (var adressekontoModel in adressekontoModels.OrderBy(m => m.Nummer))
            {
                try
                {
                    var adressekontoViewModel = regnskabViewModel.Debitorer.FirstOrDefault(m => m.Nummer == adressekontoModel.Nummer);
                    if (adressekontoViewModel == null)
                    {
                        adressekontoViewModel = new AdressekontoViewModel(regnskabViewModel, adressekontoModel, Resource.GetText(Text.Debtor), Resource.GetEmbeddedResource("Images.Adressekonto.png"), _finansstyringRepository, ExceptionHandler);
                        regnskabViewModel.DebitorAdd(adressekontoViewModel);
                        if (adressekontoModel.StatusDato.Date.CompareTo(regnskabViewModel.StatusDato.Date.AddDays(dageForNyheder*-1)) >= 0 && adressekontoModel.StatusDato.Date.CompareTo(regnskabViewModel.StatusDato.Date) <= 0)
                        {
                            adressekontoModel.SetNyhedsaktualitet(Nyhedsaktualitet.Low);
                            regnskabViewModel.NyhedAdd(new NyhedViewModel(adressekontoModel, adressekontoViewModel.Image));
                        }
                        continue;
                    }
                    adressekontoViewModel.Navn = adressekontoModel.Navn;
                    adressekontoViewModel.PrimærTelefon = adressekontoModel.PrimærTelefon;
                    adressekontoViewModel.SekundærTelefon = adressekontoModel.SekundærTelefon;
                    adressekontoViewModel.StatusDato = adressekontoModel.StatusDato;
                    adressekontoViewModel.Saldo = adressekontoModel.Saldo;
                }
                finally
                {
                    var adressekontoViewModel = adressekontoViewModelCollection.FirstOrDefault(m => m.Nummer == adressekontoModel.Nummer);
                    while (adressekontoViewModel != null)
                    {
                        adressekontoViewModelCollection.Remove(adressekontoViewModel);
                        adressekontoViewModel = adressekontoViewModelCollection.FirstOrDefault(m => m.Nummer == adressekontoModel.Nummer);
                    }
                }
            }
            foreach (var adressekontoViewModel in adressekontoViewModelCollection)
            {
                var refreshCommand = adressekontoViewModel.RefreshCommand;
                if (refreshCommand.CanExecute(adressekontoViewModel))
                {
                    refreshCommand.Execute(adressekontoViewModel);
                }
            }
        }

        #endregion
    }
}
