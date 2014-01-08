using System;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring
{
    /// <summary>
    /// ViewModel for en adressekonto.
    /// </summary>
    public class AdressekontoViewModel : ViewModelBase, IAdressekontoViewModel
    {
        #region Properties

        /// <summary>
        /// Regnskabet, som adressekontoen er tilknyttet.
        /// </summary>
        public virtual IRegnskabViewModel Regnskabsnummer
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Unik identifikation af adressekontoen.
        /// </summary>
        public virtual int Nummer
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Navn for adressekontoen.
        /// </summary>
        public virtual string Navn
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Primær telefonnummer.
        /// </summary>
        public virtual string PrimærTelefon
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Sekundær telefonnummer.
        /// </summary>
        public virtual string SekundærTelefon
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Statusdato for opgørelsen.
        /// </summary>
        public virtual DateTime StatusDato
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Saldo pr. statusdato.
        /// </summary>
        public virtual decimal Saldo
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Billede, der illustrerer en adressekontoen.
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
