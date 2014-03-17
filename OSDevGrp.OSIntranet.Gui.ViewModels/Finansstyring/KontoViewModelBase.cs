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
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Kontonummer.
        /// </summary>
        public virtual string Kontonummer
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Kontonavn.
        /// </summary>
        public virtual string Kontonavn
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
        /// Beskrivelse.
        /// </summary>
        public virtual string Beskrivelse
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
        /// Notat.
        /// </summary>
        public virtual string Notat
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
        /// Kontogruppe.
        /// </summary>
        public virtual TKontogruppeViewModel Kontogruppe
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
        /// Statusdato for opgørelse af kontoen.
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
        /// Billede, der illustrerer en kontoen.
        /// </summary>
        public virtual byte[] Image
        {
            get
            {
                throw new NotImplementedException();
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
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Repository til finansstyring.
        /// </summary>
        protected virtual IFinansstyringRepository FinansstyringRepository
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// ViewModel for en exceptionhandler.
        /// </summary>
        protected virtual IExceptionHandlerViewModel ExceptionHandler
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
