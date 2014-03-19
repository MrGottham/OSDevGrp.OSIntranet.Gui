using System;
using System.Threading;
using System.Windows.Input;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.Commands
{
    /// <summary>
    /// Kommando, der kan hente og opdatere en budgetkontoen.
    /// </summary>
    public class BudgetkontoGetCommand : ViewModelCommandBase<IBudgetkontoViewModel>
    {
        #region Private variables

        private bool _isBusy = false;
        private readonly ICommand _dependencyCommand;
        private readonly IFinansstyringRepository _finansstyringRepository;
        private readonly SynchronizationContext _synchronizationContext;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner en kommando, der kan hente og opdatere en budgetkontoen.
        /// </summary>
        /// <param name="dependencyCommand">Implementering af kommando, som denne kommando er afhængig af.</param>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="exceptionHandlerViewModel">Implementering af ViewModel til en exceptionhandler.</param>
        public BudgetkontoGetCommand(ICommand dependencyCommand, IFinansstyringRepository finansstyringRepository, IExceptionHandlerViewModel exceptionHandlerViewModel)
            : base(exceptionHandlerViewModel)
        {
            // TODO: Null check on dependencyCommand.
            if (finansstyringRepository == null)
            {
                throw new ArgumentNullException("finansstyringRepository");
            }
            _dependencyCommand = dependencyCommand;
            _finansstyringRepository = finansstyringRepository;
            _synchronizationContext = SynchronizationContext.Current;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returnerer angivelse af, om kommandoen kan udføres på den givne ViewModel.
        /// </summary>
        /// <param name="budgetkontoViewModel">ViewModel, hvorpå kommandoen skal udføres.</param>
        /// <returns>Angivelse af, om kommandoen kan udføres på den givne ViewModel.</returns>
        protected override bool CanExecute(IBudgetkontoViewModel budgetkontoViewModel)
        {
            return _isBusy == false;
        }

        /// <summary>
        /// Henter og opdaterer budgetkontoen.
        /// </summary>
        /// <param name="budgetkontoViewModel">ViewModel for budgetkontoen, der skal hentes og opdateres.</param>
        protected override void Execute(IBudgetkontoViewModel budgetkontoViewModel)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
