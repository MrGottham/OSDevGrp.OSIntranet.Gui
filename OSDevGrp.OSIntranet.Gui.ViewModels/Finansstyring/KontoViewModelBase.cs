using System;
using System.Windows.Input;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring
{
    /// <summary>
    /// ViewModel indeholdende grundlæggende kontooplysninger.
    /// </summary>
    /// <typeparam name="TKontoModel">Typen på modellen, der indeholdende grundlæggende kontooplysninger.</typeparam>
    /// <typeparam name="TKontogruppeViewModel">Typen på den ViewModel, der indeholder kontogruppen.</typeparam>
    public abstract class KontoViewModelBase<TKontoModel, TKontogruppeViewModel> : ViewModelBase, IKontoViewModelBase<TKontogruppeViewModel> where TKontoModel : IKontoModelBase where TKontogruppeViewModel : IKontogruppeViewModelBase
    {
        #region Private variables

        private readonly IRegnskabViewModel _regnskabViewModel;
        private readonly TKontoModel _kontoModel;
        private TKontogruppeViewModel _kontogruppeViewModel;
        private readonly string _displayName;
        private readonly byte[] _image;
        private readonly IFinansstyringRepository _finansstyringRepository;
        private readonly IExceptionHandlerViewModel _exceptionHandlerViewModel;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner en ViewModel indeholdende grundlæggende kontooplysninger.
        /// </summary>
        /// <param name="regnskabViewModel">ViewModel for regnskabet, som kontoen er tilknyttet.</param>
        /// <param name="kontoModel">Model indeholdende grundlæggende kontooplysninger.</param>
        /// <param name="kontogruppeViewModel">ViewModel for kontogruppen.</param>
        /// <param name="displayName">Navn for ViewModel, som kan benyttes til visning i brugergrænsefladen.</param>
        /// <param name="image">Billede, der illustrerer en kontoen.</param>
        /// <param name="finansstyringRepository">Implementering af repositoryet til finansstyring.</param>
        /// <param name="exceptionHandlerViewModel">Implementering af ViewModel for exceptionhandleren.</param>
        protected KontoViewModelBase(IRegnskabViewModel regnskabViewModel, TKontoModel kontoModel, TKontogruppeViewModel kontogruppeViewModel, string displayName, byte[] image, IFinansstyringRepository finansstyringRepository, IExceptionHandlerViewModel exceptionHandlerViewModel)
        {
            if (regnskabViewModel == null)
            {
                throw new ArgumentNullException("regnskabViewModel");
            }
            if (Equals(kontoModel, null))
            {
                throw new ArgumentNullException("kontoModel");
            }
            if (Equals(kontogruppeViewModel, null))
            {
                throw new ArgumentNullException("kontogruppeViewModel");
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
            _kontoModel = kontoModel;
            _kontogruppeViewModel = kontogruppeViewModel;
            _displayName = displayName;
            _image = image;
            _finansstyringRepository = finansstyringRepository;
            _exceptionHandlerViewModel = exceptionHandlerViewModel;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Regnskabet, som kontoen er tilknyttet.
        /// </summary>
        public virtual IRegnskabViewModel Regnskab
        {
            get
            {
                return _regnskabViewModel;
            }
        }

        /// <summary>
        /// Kontonummer.
        /// </summary>
        public virtual string Kontonummer
        {
            get
            {
                return Model.Kontonummer;
            }
        }

        /// <summary>
        /// Kontonavn.
        /// </summary>
        public virtual string Kontonavn
        {
            get
            {
                return Model.Kontonavn;
            }
            set
            {
                try
                {
                    Model.Kontonavn = value;
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex);
                }
            }
        }

        /// <summary>
        /// Beskrivelse.
        /// </summary>
        public virtual string Beskrivelse
        {
            get
            {
                return Model.Beskrivelse;
            }
            set
            {
                try
                {
                    Model.Beskrivelse = value;
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex);
                }
            }
        }

        /// <summary>
        /// Notat.
        /// </summary>
        public virtual string Notat
        {
            get
            {
                return Model.Notat;
            }
            set
            {
                try
                {
                    Model.Notat = value;
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex);
                }
            }
        }

        /// <summary>
        /// Kontogruppe.
        /// </summary>
        public virtual TKontogruppeViewModel Kontogruppe
        {
            get
            {
                return _kontogruppeViewModel;
            }
            set
            {
                try
                {
                    if (Equals(value, null))
                    {
                        throw new ArgumentNullException("value");
                    }
                    if (Equals(_kontogruppeViewModel, value))
                    {
                        return;
                    }
                    throw new NotImplementedException();
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex);
                }
            }
        }

        /// <summary>
        /// Statusdato for opgørelse af kontoen.
        /// </summary>
        public virtual DateTime StatusDato
        {
            get
            {
                return Model.StatusDato;
            }
            set
            {
                try
                {
                    Model.StatusDato = value;
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex);
                }
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
        /// Billede, der illustrerer en kontoen.
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
        public abstract ICommand RefreshCommand
        {
            get;
        }

        /// <summary>
        /// Modellen, der indeholdende grundlæggende kontooplysninger.
        /// </summary>
        protected virtual TKontoModel Model
        {
            get
            {
                return _kontoModel;
            }
        }

        /// <summary>
        /// Repository til finansstyring.
        /// </summary>
        protected virtual IFinansstyringRepository FinansstyringRepository
        {
            get
            {
                return _finansstyringRepository;
            }
        }

        /// <summary>
        /// ViewModel for en exceptionhandler.
        /// </summary>
        protected virtual IExceptionHandlerViewModel ExceptionHandler
        {
            get
            {
                return _exceptionHandlerViewModel;
            }
        }

        #endregion
    }
}
