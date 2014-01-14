using System;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring
{
    /// <summary>
    /// ViewModel for en adressekonto.
    /// </summary>
    public class AdressekontoViewModel : ViewModelBase, IAdressekontoViewModel
    {
        #region Private variables

        private readonly IRegnskabViewModel _regnskabViewModel;
        private readonly IAdressekontoModel _adressekontoModel;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner ViewModel for en adressekonto.
        /// </summary>
        /// <param name="regnskabViewModel">ViewModel for regnskabet, som adressekontoen skal være tilknyttet.</param>
        /// <param name="adressekontoModel">Model for adressekontoen.</param>
        /// <param name="displayName">Navn for ViewModel, som kan benyttes til visning i brugergrænsefladen.</param>
        /// <param name="image">Billede, der illustrerer en adressekontoen.</param>
        public AdressekontoViewModel(IRegnskabViewModel regnskabViewModel, IAdressekontoModel adressekontoModel, string displayName, byte[] image)
        {
            _regnskabViewModel = regnskabViewModel;
            _adressekontoModel = adressekontoModel;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Regnskabet, som adressekontoen er tilknyttet.
        /// </summary>
        public virtual IRegnskabViewModel Regnskab
        {
            get
            {
                return _regnskabViewModel;
            }
        }

        /// <summary>
        /// Unik identifikation af adressekontoen.
        /// </summary>
        public virtual int Nummer
        {
            get
            {
                return _adressekontoModel.Nummer;
            }
        }

        /// <summary>
        /// Navn for adressekontoen.
        /// </summary>
        public virtual string Navn
        {
            get
            {
                return _adressekontoModel.Navn;
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
                return _adressekontoModel.PrimærTelefon;
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
                return _adressekontoModel.SekundærTelefon;
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
        /// Navn for ViewModel, som kan benyttes til visning i brugergrænsefladen.
        /// </summary>
        public override string DisplayName
        {
            get
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

        #endregion
    }
}
