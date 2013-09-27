using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.Commands;
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
        private readonly ObservableCollection<IRegnskabViewModel> _regnskaber = new ObservableCollection<IRegnskabViewModel>(); 

        #endregion
        
        #region Constructor

        /// <summary>
        /// Danner ViewModel for en liste indeholdende regnskaber.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        public RegnskabslisteViewModel(IFinansstyringRepository finansstyringRepository)
        {
            if (finansstyringRepository == null)
            {
                throw new ArgumentNullException("finansstyringRepository");
            }
            _finansstyringRepository = finansstyringRepository;
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
                return _refreshCommand ?? (_refreshCommand = new RegnskabslisteRefreshCommand(_finansstyringRepository));
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
            _regnskaber.Add(regnskab);
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
