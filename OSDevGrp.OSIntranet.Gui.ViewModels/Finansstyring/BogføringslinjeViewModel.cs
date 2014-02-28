using System;
using System.ComponentModel;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring
{
    /// <summary>
    /// ViewModel for en bogføringslinje.
    /// </summary>
    public class BogføringslinjeViewModel : ViewModelBase, IReadOnlyBogføringslinjeViewModel
    {
        #region Private variables

        private readonly IRegnskabViewModel _regnskabViewModel;
        private readonly IBogføringslinjeModel _bogføringslinjeModel;

        #endregion

        #region Constructor
        
        /// <summary>
        /// Danner ViewModel for en bogføringslinje.
        /// </summary>
        /// <param name="regnskabViewModel">ViewModel for regnskabet, som bogføringslinjen skal være tilknyttet.</param>
        /// <param name="bogføringslinjeModel">Model for bogføringslinjen.</param>
        public BogføringslinjeViewModel(IRegnskabViewModel regnskabViewModel, IBogføringslinjeModel bogføringslinjeModel)
        {
            if (regnskabViewModel == null)
            {
                throw new ArgumentNullException("regnskabViewModel");
            }
            if (bogføringslinjeModel == null)
            {
                throw new ArgumentNullException("bogføringslinjeModel");
            }
            _regnskabViewModel = regnskabViewModel;
            _bogføringslinjeModel = bogføringslinjeModel;
            _bogføringslinjeModel.PropertyChanged += PropertyChangedOnBogføringslinjeModelEventHandler;
        }
        
        #endregion

        #region Properties

        /// <summary>
        /// Regnskabet, som bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual IRegnskabViewModel Regnskab
        {
            get
            {
                return _regnskabViewModel;
            }
        }

        /// <summary>
        /// Unik identifikation af bogføringslinjen inden for regnskabet.
        /// </summary>
        public virtual int Løbenummer
        {
            get
            {
                return _bogføringslinjeModel.Løbenummer;
            }
        }

        /// <summary>
        /// Bogføringstidspunkt.
        /// </summary>
        public virtual DateTime Dato
        {
            get
            {
                return _bogføringslinjeModel.Dato;
            }
        }

        /// <summary>
        /// Bilagsnummer.
        /// </summary>
        public virtual string Bilag
        {
            get
            {
                return _bogføringslinjeModel.Bilag;
            }
        }

        /// <summary>
        /// Kontonummer, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual string Kontonummer
        {
            get
            {
                return _bogføringslinjeModel.Kontonummer;
            }
        }

        /// <summary>
        /// Tekst.
        /// </summary>
        public virtual string Tekst
        {
            get
            {
                return _bogføringslinjeModel.Tekst;
            }
        }

        /// <summary>
        /// Budgetkontonummer, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual string Budgetkontonummer
        {
            get
            {
                return _bogføringslinjeModel.Budgetkontonummer;
            }
        }

        /// <summary>
        /// Debitbeløb.
        /// </summary>
        public virtual decimal Debit
        {
            get
            {
                return _bogføringslinjeModel.Debit;
            }
        }

        /// <summary>
        /// Kreditbeløb.
        /// </summary>
        public virtual decimal Kredit
        {
            get
            {
                return _bogføringslinjeModel.Kredit;
            }
        }

        /// <summary>
        /// Bogføringsbeløb.
        /// </summary>
        public virtual decimal Bogført
        {
            get
            {
                return _bogføringslinjeModel.Bogført;
            }
        }

        /// <summary>
        /// Adressekonto, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual int Adressekonto
        {
            get
            {
                return _bogføringslinjeModel.Adressekonto;
            }
        }

        /// <summary>
        /// Billede, der illustrerer en bogføringslinje.
        /// </summary>
        public virtual byte[] Image
        {
            get
            {
                return Resource.GetEmbeddedResource("Images.Bogføringslinje.png");
            }
        }

        /// <summary>
        /// Navn for ViewModel, som kan benyttes til visning i brugergrænsefladen.
        /// </summary>
        public override string DisplayName
        {
            get
            {
                return _bogføringslinjeModel.Nyhedsinformation;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Eventhandler, der kaldes, når en property ændres på modellen for bogføringslinjen.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        private void PropertyChangedOnBogføringslinjeModelEventHandler(object sender, PropertyChangedEventArgs eventArgs)
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
                case "Nyhedsinformation":
                    RaisePropertyChanged(eventArgs.PropertyName);
                    RaisePropertyChanged("DisplayName");
                    break;

                default:
                    RaisePropertyChanged(eventArgs.PropertyName);
                    break;
            }
        }

        #endregion
    }
}
