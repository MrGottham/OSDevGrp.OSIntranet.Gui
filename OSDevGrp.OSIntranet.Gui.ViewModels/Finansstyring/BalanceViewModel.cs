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
    /// ViewModel for en linje i balancen.
    /// </summary>
    public class BalanceViewModel : KontogruppeViewModel, IBalanceViewModel
    {
        #region Private variables

        private readonly IRegnskabViewModel _regnskabViewModel;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner en ViewModel til en linje i balancen.
        /// </summary>
        /// <param name="regnskabViewModel">ViewModel for regnskabet, som balancelinjen er tilknyttet.</param>
        /// <param name="kontogruppeModel">Model for gruppen af konti, som balancelinjen baserer sig på.</param>
        /// <param name="exceptionHandlerViewModel">Implementering af ViewModel for exceptionhandleren.</param>
        public BalanceViewModel(IRegnskabViewModel regnskabViewModel, IKontogruppeModel kontogruppeModel, IExceptionHandlerViewModel exceptionHandlerViewModel)
            : base(kontogruppeModel, exceptionHandlerViewModel)
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
        /// Registrerede konti, som indgår i balancelinjen.
        /// </summary>
        public virtual IEnumerable<IKontoViewModel> Konti
        {
            get
            {
                return _regnskabViewModel.Konti
                    .Where(m => m.Kontogruppe != null && m.Kontogruppe.Nummer == Nummer && m.ErRegistreret)
                    .OrderBy(m => m.Kontonummer);
            }
        }
        
        #endregion

        #region Methods

        /// <summary>
        /// Registrerer en konto til at indgå i balancelinjen.
        /// </summary>
        /// <param name="kontoViewModel">ViewModel for kontoen, der skal indgå i balancelinjen.</param>
        public virtual void Register(IKontoViewModel kontoViewModel)
        {
            if (kontoViewModel == null)
            {
                throw new ArgumentNullException("kontoViewModel");
            }
            if (_regnskabViewModel.Konti.Contains(kontoViewModel) == false || kontoViewModel.Kontogruppe == null || kontoViewModel.Kontogruppe.Nummer != Nummer || kontoViewModel.ErRegistreret)
            {
                return;
            }
            kontoViewModel.ErRegistreret = true;
            kontoViewModel.PropertyChanged += PropertyChangedOnKontoViewModelEventHandler;
        }

        /// <summary>
        /// Metode, der kaldes, når den underlæggende model ændres.
        /// </summary>
        /// <param name="propertyName">Navn på property, som er blevet ændret.</param>
        protected override void ModelChanged(string propertyName)
        {
            base.ModelChanged(propertyName);
            switch (propertyName)
            {
                case "Nummer":
                    RaisePropertyChanged("Konti");
                    break;
            }
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
                case "Konti":
                    RaisePropertyChanged("Konti");
                    break;
            }
        }

        /// <summary>
        /// Event, der rejses, når en property ændres på ViewModel for en konto, der er registreret til brug i denne balancelinje.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        private void PropertyChangedOnKontoViewModelEventHandler(object sender, PropertyChangedEventArgs eventArgs)
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
                case "Kontonummer":
                    RaisePropertyChanged("Konti");
                    break;

                case "Kontogruppe":
                    RaisePropertyChanged("Konti");
                    break;

                case "ErRegistreret":
                    RaisePropertyChanged("Konti");
                    break;
            }

        }

        #endregion
    }
}
