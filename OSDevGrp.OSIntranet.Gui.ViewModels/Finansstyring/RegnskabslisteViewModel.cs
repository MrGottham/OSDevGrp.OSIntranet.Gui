using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring
{
    /// <summary>
    /// ViewModel for en liste indeholdende regnskaber.
    /// </summary>
    public class RegnskabslisteViewModel : ViewModelBase, IRegnskabslisteViewModel
    {
        #region Private variables

        private DateTime _statusDato = DateTime.Now;
        private ICommand _refreshCommand;
        private readonly IFinansstyringRepository _finansstyringRepository;
        private readonly IExceptionHandlerViewModel _exceptionHandlerViewModel;
        private readonly ObservableCollection<IRegnskabViewModel> _regnskaber = new ObservableCollection<IRegnskabViewModel>(); 

        #endregion
        
        #region Constructor

        /// <summary>
        /// Danner ViewModel for en liste indeholdende regnskaber.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="exceptionHandlerViewModel">Implementering af ViewModel for en exceptionhandler.</param>
        public RegnskabslisteViewModel(IFinansstyringRepository finansstyringRepository, IExceptionHandlerViewModel exceptionHandlerViewModel)
        {
            if (finansstyringRepository == null)
            {
                throw new ArgumentNullException("finansstyringRepository");
            }
            if (exceptionHandlerViewModel == null)
            {
                throw new ArgumentNullException("exceptionHandlerViewModel");
            }
            _finansstyringRepository = finansstyringRepository;
            _exceptionHandlerViewModel = exceptionHandlerViewModel;
            _regnskaber.CollectionChanged += RegnskaberCollectionChangedEventHandler;
        }
        
        #endregion

        #region Properties

        /// <summary>
        /// Navn for ViewModel, som kan benyttes til visning i brugergrænsefladen.
        /// </summary>
        public override string DisplayName
        {
            get
            {
                return Resource.GetText(Text.Accountings);
            }
        }

        /// <summary>
        /// Statusdato for listen af regnskaber.
        /// </summary>
        public virtual DateTime StatusDato
        {
            get
            {
                return _statusDato;
            }
            set
            {
                if (_statusDato == value)
                {
                    return;
                }
                _statusDato = value;
                RaisePropertyChanged("StatusDato");
            }
        }

        /// <summary>
        /// Regnskaber.
        /// </summary>
        public virtual IEnumerable<IRegnskabViewModel> Regnskaber
        {
            get
            {
                return _regnskaber.OrderBy(m => m.Nummer);
            }
        }

        /// <summary>
        /// Kommando til genindlæsning og opdatering.
        /// </summary>
        public virtual ICommand RefreshCommand
        {
            get
            {
                return _refreshCommand ?? (_refreshCommand = new RegnskabslisteRefreshCommand(_finansstyringRepository, _exceptionHandlerViewModel, RegnskabslisteRefreshCommand.ExecuteRefreshCommandOnRegnskabViewModels));
            }
        }

        #endregion
        
        #region Methods
        
        /// <summary>
        /// Tilføjer et regnskab til listen af regnskaber.
        /// </summary>
        /// <param name="regnskab">ViewModel for regnskabet, der skal tilføjes listen af regnskaber.</param>
        public virtual void RegnskabAdd(IRegnskabViewModel regnskab)
        {
            if (regnskab == null)
            {
                throw new ArgumentNullException("regnskab");
            }
            regnskab.PropertyChanged += PropertyChangedOnRegnskabViewModelEventHandler;
            _regnskaber.Add(regnskab);
        }

        /// <summary>
        /// Henter og returnerer en ViewModel til et givent regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummeret, hvortil ViewModel for regnskabet skal returneres.</param>
        /// <returns>ViewModel for det givne regnskab.</returns>
        public async virtual Task<IRegnskabViewModel> RegnskabGetAsync(int regnskabsnummer)
        {
            try
            {
                var regnskabViewModel = Regnskaber.FirstOrDefault(m => m.Nummer == regnskabsnummer);
                if (regnskabViewModel != null)
                {
                    return regnskabViewModel;
                }
                var statusDato = DateTime.Now;
                var regnskabModelCollection = await _finansstyringRepository.RegnskabslisteGetAsync();
                var regnskabModel = regnskabModelCollection.FirstOrDefault(m => m.Nummer == regnskabsnummer);
                return regnskabModel == null ? null : new RegnskabViewModel(regnskabModel, statusDato, _finansstyringRepository, _exceptionHandlerViewModel);
            }
            catch (IntranetGuiExceptionBase ex)
            {
                _exceptionHandlerViewModel.HandleException(ex);
                return null;
            }
            catch (Exception ex)
            {
                _exceptionHandlerViewModel.HandleException(new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.MethodError, "RegnskabGetAsync", ex.Message)));
                return null;
            }
        }

        /// <summary>
        /// Eventhandler, der rejses, når en ViewModel for et regnskab opdateres.
        /// </summary>
        /// <param name="sender">Object, der rejser eventet.</param>
        /// <param name="e">Argumenter til eventet.</param>
        private void PropertyChangedOnRegnskabViewModelEventHandler(object sender, PropertyChangedEventArgs e)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }
            switch (e.PropertyName)
            {
                case "Nummer":
                    RaisePropertyChanged("Regnskaber");
                    break;
            }
        }

        /// <summary>
        /// Eventhandler, der rejses, når listen af regnskaber ændres.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventer.</param>
        /// <param name="e">Argumenter til eventet.</param>
        private void RegnskaberCollectionChangedEventHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    RaisePropertyChanged("Regnskaber");
                    break;
            }
        }

        #endregion
    }
}
