using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring
{
    /// <summary>
    /// ViewModel for en linje i opgørelsen.
    /// </summary>
    public class OpgørelseViewModel : BudgetkontogruppeViewModel, IOpgørelseViewModel
    {
        #region Private variables

        private readonly IRegnskabViewModel _regnskabViewModel;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner en ViewModel til en linje i opgørelsen.
        /// </summary>
        /// <param name="regnskabViewModel">ViewModel for regnskabet, som opgørelseslinjen er tilknyttet.</param>
        /// <param name="budgetkontogruppeModel">Model for gruppen af budgetkonti, som opgørelseslinjen baserer sig på.</param>
        /// <param name="exceptionHandlerViewModel">Implementering af ViewModel for exceptionhandleren.</param>
        public OpgørelseViewModel(IRegnskabViewModel regnskabViewModel, IBudgetkontogruppeModel budgetkontogruppeModel, IExceptionHandlerViewModel exceptionHandlerViewModel)
            : base(budgetkontogruppeModel, exceptionHandlerViewModel)
        {
            if (regnskabViewModel == null)
            {
                throw new ArgumentNullException("regnskabViewModel");
            }
            _regnskabViewModel = regnskabViewModel;
            _regnskabViewModel.PropertyChanged += PropertyChangedOnRegnskabViewModelEventHandler;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Registrerede budgetkonti, som indgår i opgørelseslinjen.
        /// </summary>
        public virtual IEnumerable<IBudgetkontoViewModel> Budgetkonti
        {
            get
            {
                return _regnskabViewModel.Budgetkonti
                    .Where(m => m.Kontogruppe != null && m.Kontogruppe.Nummer == Nummer && m.ErRegistreret)
                    .OrderBy(m => m.Kontonummer);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Registrerer en budgetkonto til at indgå i opgørelseslinjen.
        /// </summary>
        /// <param name="budgetkontoViewModel">ViewModel for budgetkontoen, der skal indgå i opgørelseslinjen.</param>
        public virtual void Register(IBudgetkontoViewModel budgetkontoViewModel)
        {
            if (budgetkontoViewModel == null)
            {
                throw new ArgumentNullException("budgetkontoViewModel");
            }
            if (_regnskabViewModel.Budgetkonti.Contains(budgetkontoViewModel) == false || budgetkontoViewModel.Kontogruppe == null || budgetkontoViewModel.Kontogruppe.Nummer != Nummer || budgetkontoViewModel.ErRegistreret)
            {
                return;
            }
            budgetkontoViewModel.ErRegistreret = true;
            budgetkontoViewModel.PropertyChanged += PropertyChangedOnBudgetkontoViewModelEventHandler;
        }

        /// <summary>
        /// Event, der rejses, når en property ændres på ViewModel for regnskabet, som balancelinjen er tilknyttet.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        private void PropertyChangedOnRegnskabViewModelEventHandler(object sender, PropertyChangedEventArgs eventArgs)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            if (eventArgs == null)
            {
                throw new ArgumentNullException("eventArgs");
            }
            switch (eventArgs.PropertyName)
            {
                case "Budgetkonti":
                    RaisePropertyChanged("Budgetkonti");
                    break;
            }
        }

        /// <summary>
        /// Event, der rejses, når en property ændres på ViewModel for en konto, der er registreret til brug i denne balancelinje.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        private void PropertyChangedOnBudgetkontoViewModelEventHandler(object sender, PropertyChangedEventArgs eventArgs)
        {
            /*
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            if (eventArgs == null)
            {
                throw new ArgumentNullException("eventArgs");
            }
             */
            throw new NotImplementedException();
        }

        #endregion
    }
}
