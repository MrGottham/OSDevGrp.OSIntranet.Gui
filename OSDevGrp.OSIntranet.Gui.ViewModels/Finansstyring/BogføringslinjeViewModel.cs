using System;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring
{
    /// <summary>
    /// ViewModel for en bogføringslinje.
    /// </summary>
    public class BogføringslinjeViewModel : ViewModelBase, IReadOnlyBogføringslinjeViewModel
    {
        #region Properties

        /// <summary>
        /// Regnskabet, som bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual IRegnskabViewModel Regnskab
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Unik identifikation af bogføringslinjen inden for regnskabet.
        /// </summary>
        public virtual int Løbenummer
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Bogføringstidspunkt.
        /// </summary>
        public virtual DateTime Dato
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Kontonummer, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual string Kontonummer
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Tekst.
        /// </summary>
        public virtual string Tekst
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Budgetkontonummer, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual string Budgetkontonummer
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Debitbeløb.
        /// </summary>
        public virtual decimal Debit
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Kreditbeløb.
        /// </summary>
        public virtual decimal Kredit
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Bogføringsbeløb.
        /// </summary>
        public virtual decimal Bogført
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Adressekonto, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual int Adressekonto
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Billede, der illustrerer en bogføringslinje.
        /// </summary>
        public virtual byte[] Image
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Navn for ViewModel, som kan benyttes til visning i brugergrænsefladen.
        /// </summary>
        public override string DisplayName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
