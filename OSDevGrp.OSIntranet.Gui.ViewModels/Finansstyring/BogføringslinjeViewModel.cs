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
    public class BogføringslinjeViewModel : ValidateableViewModelBase, IReadOnlyBogføringslinjeViewModel
    {
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
                throw new ArgumentNullException(nameof(regnskabViewModel));
            }

            if (bogføringslinjeModel == null)
            {
                throw new ArgumentNullException(nameof(bogføringslinjeModel));
            }

            Regnskab = regnskabViewModel;
            Model = bogføringslinjeModel;
            Model.PropertyChanged += PropertyChangedOnBogføringslinjeModelEventHandler;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Regnskabet, som bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual IRegnskabViewModel Regnskab { get; }

        /// <summary>
        /// Unik identifikation af bogføringslinjen inden for regnskabet.
        /// </summary>
        public virtual int Løbenummer => Model.Løbenummer;

        /// <summary>
        /// Bogføringstidspunkt.
        /// </summary>
        public virtual DateTime Dato => Model.Dato;

        /// <summary>
        /// Bilagsnummer.
        /// </summary>
        public virtual string Bilag => Model.Bilag;

        /// <summary>
        /// Kontonummer, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual string Kontonummer => Model.Kontonummer;

        /// <summary>
        /// Tekst.
        /// </summary>
        public virtual string Tekst => Model.Tekst;

        /// <summary>
        /// Budgetkontonummer, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual string Budgetkontonummer => Model.Budgetkontonummer;

        /// <summary>
        /// Debitbeløb.
        /// </summary>
        public virtual decimal Debit => Model.Debit;

        /// <summary>
        /// Tekstangivelse af debitbeløb.
        /// </summary>
        public virtual string DebitAsText
        {
            get
            {
                decimal result = Debit;
                return result == 0M ? string.Empty : result.ToString("C");
            }
        }

        /// <summary>
        /// Kreditbeløb.
        /// </summary>
        public virtual decimal Kredit => Model.Kredit;

        /// <summary>
        /// Tekstangivelse af kreditbeløb.
        /// </summary>
        public virtual string KreditAsText
        {
            get
            {
                decimal result = Kredit;
                return result == 0M ? string.Empty : result.ToString("C");
            }
        }

        /// <summary>
        /// Bogføringsbeløb.
        /// </summary>
        public virtual decimal Bogført => Model.Bogført;

        /// <summary>
        /// Tekstangivelse af bogføringsbeløb.
        /// </summary>
        public virtual string BogførtAsText
        {
            get
            {
                decimal result = Bogført;
                return result == 0M ? string.Empty : result.ToString("C");
            }
        }

        /// <summary>
        /// Adressekonto, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual int Adressekonto => Model.Adressekonto;

        /// <summary>
        /// Billede, der illustrerer en bogføringslinje.
        /// </summary>
        public virtual byte[] Image => Resource.GetEmbeddedResource("Images.Bogføringslinje.png");

        /// <summary>
        /// Navn for ViewModel, som kan benyttes til visning i brugergrænsefladen.
        /// </summary>
        public override string DisplayName => Model.Nyhedsinformation;

        /// <summary>
        /// Model for bogføringslinjen.
        /// </summary>
        protected IBogføringslinjeModel Model { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Eventhandler, der kaldes, når en property ændres på modellen for bogføringslinjen.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        protected virtual void PropertyChangedOnBogføringslinjeModelEventHandler(object sender, PropertyChangedEventArgs eventArgs)
        {
            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            if (eventArgs == null)
            {
                throw new ArgumentNullException(nameof(eventArgs));
            }

            switch (eventArgs.PropertyName)
            {
                case nameof(Debit):
                    RaisePropertyChanged(eventArgs.PropertyName);
                    RaisePropertyChanged(nameof(DebitAsText));
                    break;

                case nameof(Kredit):
                    RaisePropertyChanged(eventArgs.PropertyName);
                    RaisePropertyChanged(nameof(KreditAsText));
                    break;

                case nameof(Bogført):
                    RaisePropertyChanged(eventArgs.PropertyName);
                    RaisePropertyChanged(nameof(BogførtAsText));
                    break;

                case "Nyhedsinformation":
                    RaisePropertyChanged(eventArgs.PropertyName);
                    RaisePropertyChanged(nameof(DisplayName));
                    break;

                default:
                    RaisePropertyChanged(eventArgs.PropertyName);
                    break;
            }
        }

        #endregion
    }
}