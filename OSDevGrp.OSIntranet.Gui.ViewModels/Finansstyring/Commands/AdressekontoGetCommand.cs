using System;
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
        #region Constructor

        /// <summary>
        /// Danner en kommando, der kan hente og opdaterer en adressekonto.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="exceptionHandlerViewModel">Implementering af en ViewModel til en exceptionhandler.</param>
        public AdressekontoGetCommand(IFinansstyringRepository finansstyringRepository, IExceptionHandlerViewModel exceptionHandlerViewModel)
            : base(exceptionHandlerViewModel)
        {
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Henter og opdaterer adressekontoen.
        /// </summary>
        /// <param name="viewModel">ViewModel for kontoen, der skal hentes og opdateres.</param>
        protected override void Execute(IAdressekontoViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
