using System;
using System.ComponentModel;
using System.Windows.Input;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring
{
    /// <summary>
    /// ViewModel for en adressekonto.
    /// </summary>
    public class AdressekontoViewModel : ViewModelBase, IAdressekontoViewModel
    {
        #region Private variables

        private ICommand _refreshCommand;
        private readonly IRegnskabViewModel _regnskabViewModel;
        private readonly IAdressekontoModel _adressekontoModel;
        private readonly string _displayName;
        private readonly byte[] _image;
        private readonly IFinansstyringRepository _finansstyringRepository;
        private readonly IExceptionHandlerViewModel _exceptionHandlerViewModel;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner ViewModel for en adressekonto.
        /// </summary>
        /// <param name="regnskabViewModel">ViewModel for regnskabet, som adressekontoen skal være tilknyttet.</param>
        /// <param name="adressekontoModel">Model for adressekontoen.</param>
        /// <param name="displayName">Navn for ViewModel, som kan benyttes til visning i brugergrænsefladen.</param>
        /// <param name="image">Billede, der illustrerer en adressekontoen.</param>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="exceptionHandlerViewModel">Implementering af ViewModel for en exceptionhandler.</param>
        public AdressekontoViewModel(IRegnskabViewModel regnskabViewModel, IAdressekontoModel adressekontoModel, string displayName, byte[] image, IFinansstyringRepository finansstyringRepository, IExceptionHandlerViewModel exceptionHandlerViewModel)
        {
            if (regnskabViewModel == null)
            {
                throw new ArgumentNullException("regnskabViewModel");
            }
            if (adressekontoModel == null)
            {
                throw new ArgumentNullException("adressekontoModel");
            }
            if (string.IsNullOrEmpty(displayName))
            {
                throw new ArgumentNullException("displayName");
            }
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }
            if (finansstyringRepository == null)
            {
                throw new ArgumentNullException("finansstyringRepository");
            }
            if (exceptionHandlerViewModel == null)
            {
                throw new ArgumentNullException("exceptionHandlerViewModel");
            }
            _regnskabViewModel = regnskabViewModel;
            _adressekontoModel = adressekontoModel;
            _adressekontoModel.PropertyChanged += PropertyChangedOnAdressekontoModelEventHandler;
            _displayName = displayName;
            _image = image;
            _finansstyringRepository = finansstyringRepository;
            _exceptionHandlerViewModel = exceptionHandlerViewModel;
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
                try
                {
                    _adressekontoModel.Navn = value;
                }
                catch (Exception ex)
                {
                    _exceptionHandlerViewModel.HandleException(ex);
                }
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
                try
                {
                    _adressekontoModel.PrimærTelefon = value;
                }
                catch (Exception ex)
                {
                    _exceptionHandlerViewModel.HandleException(ex);
                }
            }
        }

        /// <summary>
        /// Label til primær telefonnummer.
        /// </summary>
        public virtual string PrimærTelefonLabel
        {
            get
            {
                return Resource.GetText(Text.Phone);
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
                try
                {
                    _adressekontoModel.SekundærTelefon = value;
                }
                catch (Exception ex)
                {
                    _exceptionHandlerViewModel.HandleException(ex);
                }
            }
        }

        /// <summary>
        /// Label til sekundær telefonummer.
        /// </summary>
        public virtual string SekundærTelefonLabel
        {
            get
            {
                return Resource.GetText(Text.Phone);
            }
        }

        /// <summary>
        /// Statusdato for opgørelsen.
        /// </summary>
        public virtual DateTime StatusDato
        {
            get
            {
                return _adressekontoModel.StatusDato;
            }
            set
            {
                try
                {
                    _adressekontoModel.StatusDato = value;
                }
                catch (Exception ex)
                {
                    _exceptionHandlerViewModel.HandleException(ex);
                }
            }
        }

        /// <summary>
        /// Saldo pr. statusdato.
        /// </summary>
        public virtual decimal Saldo
        {
            get
            {
                return _adressekontoModel.Saldo;
            }
            set
            {
                try
                {
                    _adressekontoModel.Saldo = value;
                }
                catch (Exception ex)
                {
                    _exceptionHandlerViewModel.HandleException(ex);
                }
            }
        }

        /// <summary>
        /// Tekstangivelse af saldo pr. statusdato
        /// </summary>
        public virtual string SaldoAsText
        {
            get
            {
                return Saldo.ToString("C");
            }
        }

        /// <summary>
        /// Label til saldo pr. statusdato.
        /// </summary>
        public virtual string SaldoLabel
        {
            get
            {
                return Resource.GetText(Text.Balance);
            }
        }

        /// <summary>
        /// Navn for ViewModel, som kan benyttes til visning i brugergrænsefladen.
        /// </summary>
        public override string DisplayName
        {
            get
            {
                return _displayName;
            }
        }

        /// <summary>
        /// Billede, der illustrerer en adressekontoen.
        /// </summary>
        public virtual byte[] Image
        {
            get
            {
                return _image;
            }
        }

        /// <summary>
        /// Kommando til genindlæsning og opdatering.
        /// </summary>
        public virtual ICommand RefreshCommand
        {
            get
            {
                return _refreshCommand ?? (_refreshCommand = new AdressekontoGetCommand(_finansstyringRepository, _exceptionHandlerViewModel));
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Eventhandler, der kaldes, når en property ændres på modellen for adressekontoen.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        private void PropertyChangedOnAdressekontoModelEventHandler(object sender, PropertyChangedEventArgs eventArgs)
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
                case "Saldo":
                    RaisePropertyChanged(eventArgs.PropertyName);
                    RaisePropertyChanged("SaldoAsText");
                    break;

                default:
                    RaisePropertyChanged(eventArgs.PropertyName);
                    break;
            }
        }

        #endregion
    }
}
