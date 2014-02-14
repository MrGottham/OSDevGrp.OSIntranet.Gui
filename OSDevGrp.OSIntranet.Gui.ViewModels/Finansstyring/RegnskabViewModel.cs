using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core.Comparers;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.Comparers;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring
{
    /// <summary>
    /// ViewModel for et regnskab.
    /// </summary>
    public class RegnskabViewModel : ViewModelBase, IRegnskabViewModel
    {
        #region Private variables

        private DateTime _statusDato;
        private ICommand _refreshCommand;
        private readonly IRegnskabModel _regnskabModel;
        private readonly IFinansstyringRepository _finansstyringRepository;
        private readonly IExceptionHandlerViewModel _exceptionHandlerViewModel;
        private readonly ObservableCollection<IReadOnlyBogføringslinjeViewModel> _bogføringslinjeViewModels = new ObservableCollection<IReadOnlyBogføringslinjeViewModel>();
        private readonly ObservableCollection<IAdressekontoViewModel> _debitorerViewModels = new ObservableCollection<IAdressekontoViewModel>();
        private readonly ObservableCollection<IAdressekontoViewModel> _kreditorerViewModels = new ObservableCollection<IAdressekontoViewModel>();
        private readonly ObservableCollection<INyhedViewModel> _nyhedViewModels = new ObservableCollection<INyhedViewModel>(); 

        #endregion

        #region Constructor
        
        /// <summary>
        /// Danner ViewModel for et regnskab.
        /// </summary>
        /// <param name="regnskabModel">Model for et regnskab.</param>
        /// <param name="statusDato">Statusdato for regnskabet.</param>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="exceptionHandlerViewModel">Implementering af ViewModel for en exceptionhandler.</param>
        public RegnskabViewModel(IRegnskabModel regnskabModel, DateTime statusDato, IFinansstyringRepository finansstyringRepository, IExceptionHandlerViewModel exceptionHandlerViewModel)
        {
            if (regnskabModel == null)
            {
                throw new ArgumentNullException("regnskabModel");
            }
            if (finansstyringRepository == null)
            {
                throw new ArgumentNullException("finansstyringRepository");
            }
            if (exceptionHandlerViewModel == null)
            {
                throw new ArgumentNullException("exceptionHandlerViewModel");
            }
            _regnskabModel = regnskabModel;
            _regnskabModel.PropertyChanged += PropertyChangedOnRegnskabModelEventHandler;
            _statusDato = statusDato;
            _finansstyringRepository = finansstyringRepository;
            _exceptionHandlerViewModel = exceptionHandlerViewModel;
            _bogføringslinjeViewModels.CollectionChanged += CollectionChangedOnBogføringslinjeViewModelsEventHandler;
            _debitorerViewModels.CollectionChanged += CollectionChangedOnDebitorViewModelsEventHandler;
            _kreditorerViewModels.CollectionChanged += CollectionChangedOnKreditorerViewModelsEventHandler;
            _nyhedViewModels.CollectionChanged += CollectionChangedOnNyhedViewModelsEventHandler;
        }
        
        #endregion

        #region Properties

        /// <summary>
        /// Regnskabsnummer.
        /// </summary>
        public virtual int Nummer
        {
            get
            {
                return _regnskabModel.Nummer;
            }
        }

        /// <summary>
        /// Navn.
        /// </summary>
        public virtual string Navn
        {
            get
            {
                return _regnskabModel.Navn;
            }
            set
            {
                _regnskabModel.Navn = value;
            }
        }

        /// <summary>
        /// Navn for ViewModel, som kan benyttes til visning i brugergrænsefladen.
        /// </summary>
        public override string DisplayName
        {
            get
            {
                return Navn;
            }
        }

        /// <summary>
        /// Statusdato for regnskabet.
        /// </summary>
        public virtual DateTime StatusDato
        {
            get
            {
                return _statusDato;
            }
            set
            {
                if (_statusDato.CompareTo(value) > 0)
                {
                    throw new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "value", value), "value");
                }
                if (_statusDato.CompareTo(value) == 0)
                {
                    return;
                }
                _statusDato = value;
                RaisePropertyChanged("StatusDato");
            }
        }

        /// <summary>
        /// Bogføringslinjer.
        /// </summary>
        public virtual IEnumerable<IReadOnlyBogføringslinjeViewModel> Bogføringslinjer
        {
            get
            {
                var comparer = new BogføringslinjeViewModelComparer();
                return _bogføringslinjeViewModels.OrderBy(m => m, comparer);
            }
        }

        /// <summary>
        /// Overskrift til bogføringslinjer.
        /// </summary>
        public virtual string BogføringslinjerHeader
        {
            get
            {
                return Resource.GetText(Text.Bookkeeping);
            }
        }

        /// <summary>
        /// Debitorer.
        /// </summary>
        public virtual IEnumerable<IAdressekontoViewModel> Debitorer
        {
            get
            {
                var comparer = new AdressekontoViewModelComparer();
                return _debitorerViewModels.OrderBy(m => m, comparer);
            }
        }

        /// <summary>
        /// Overskrift til debitorer.
        /// </summary>
        public virtual string DebitorerHeader
        {
            get
            {
                return Resource.GetText(Text.Debtors);
            }
        }

        /// <summary>
        /// Kreditorer.
        /// </summary>
        public virtual IEnumerable<IAdressekontoViewModel> Kreditorer
        {
            get
            {
                var comparer = new AdressekontoViewModelComparer();
                return _kreditorerViewModels.OrderBy(m => m, comparer);
            }
        }

        /// <summary>
        /// Overskrift til kreditorer.
        /// </summary>
        public virtual string KreditorerHeader
        {
            get
            {
                return Resource.GetText(Text.Creditors);
            }
        }

        /// <summary>
        /// Nyheder.
        /// </summary>
        public virtual IEnumerable<INyhedViewModel> Nyheder
        {
            get
            {
                var comparer = new NyhedViewModelComparer();
                return _nyhedViewModels.OrderBy(m => m, comparer);
            }
        }

        /// <summary>
        /// Overskrift til nyheder.
        /// </summary>
        public virtual string NyhederHeader
        {
            get
            {
                return Resource.GetText(Text.NewsMultiple);
            }
        }

        /// <summary>
        /// Kommando til genindlæsning og opdatering.
        /// </summary>
        public virtual ICommand RefreshCommand
        {
            get
            {
                if (_refreshCommand != null)
                {
                    return _refreshCommand;
                }
                var executeCommands = new Collection<ICommand>
                    {
                        new BogføringslinjerGetCommand(_finansstyringRepository, _exceptionHandlerViewModel),
                        new DebitorlisteGetCommand(_finansstyringRepository, _exceptionHandlerViewModel),
                        new KreditorlisteGetCommand(_finansstyringRepository, _exceptionHandlerViewModel)
                    };
                _refreshCommand = new CommandCollectionExecuterCommand(executeCommands);
                return _refreshCommand;
            }
        }

        #endregion
        
        #region Methods

        /// <summary>
        /// Tilføjer en bogføringslinje til regnskabet.
        /// </summary>
        /// <param name="bogføringslinjeViewModel">ViewModel for bogføringslinjen, der skal tilføjes regnskabet.</param>
        public virtual void BogføringslinjeAdd(IReadOnlyBogføringslinjeViewModel bogføringslinjeViewModel)
        {
            if (bogføringslinjeViewModel == null)
            {
                throw new ArgumentNullException("bogføringslinjeViewModel");
            }
            bogføringslinjeViewModel.PropertyChanged += PropertyChangedOnBogføringslinjeViewModelEventHandler;
            _bogføringslinjeViewModels.Add(bogføringslinjeViewModel);
        }

        /// <summary>
        /// Tilføjerer en debitor til regnskabet.
        /// </summary>
        /// <param name="adressekontoViewModel">ViewModel for adressekontoen, der skal tilføjes som debitor.</param>
        public virtual void DebitorAdd(IAdressekontoViewModel adressekontoViewModel)
        {
            if (adressekontoViewModel == null)
            {
                throw new ArgumentNullException("adressekontoViewModel");
            }
            adressekontoViewModel.PropertyChanged += PropertyChangedOnAdressekontoViewModelForDebitorEventHandler;
            _debitorerViewModels.Add(adressekontoViewModel);
        }

        /// <summary>
        /// Tilføjerer en kreditor til regnskabet.
        /// </summary>
        /// <param name="adressekontoViewModel">ViewModel for adressekontoen, der skal tilføjes som kreditor.</param>
        public virtual void KreditorAdd(IAdressekontoViewModel adressekontoViewModel)
        {
            if (adressekontoViewModel == null)
            {
                throw new ArgumentNullException("adressekontoViewModel");
            }
            adressekontoViewModel.PropertyChanged += PropertyChangedOnAdressekontoViewModelForKreditorEventHandler;
            _kreditorerViewModels.Add(adressekontoViewModel);
        }

        /// <summary>
        /// Tilføjer en nyhed til regnskabet.
        /// </summary>
        /// <param name="nyhedViewModel">ViewModel for nyheden, er skal tilføjes regnskabet.</param>
        public virtual void NyhedAdd(INyhedViewModel nyhedViewModel)
        {
            if (nyhedViewModel == null)
            {
                throw new ArgumentNullException("nyhedViewModel");
            }
            nyhedViewModel.PropertyChanged += PropertyChangedOnNyhedViewModelEventHandler;
            _nyhedViewModels.Add(nyhedViewModel);
        }

        /// <summary>
        /// Eventhandler, der kaldes, når en property ændres på modellen for regnskabet.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="e">Argumenter til eventet.</param>
        private void PropertyChangedOnRegnskabModelEventHandler(object sender, PropertyChangedEventArgs e)
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
                case "Navn":
                    RaisePropertyChanged(e.PropertyName);
                    RaisePropertyChanged("DisplayName");
                    break;

                default:
                    RaisePropertyChanged(e.PropertyName);
                    break;
            }
        }

        /// <summary>
        /// Eventhandler, der kaldes, når en property ændres på en ViewModel for en bogføringslinje.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="e">Argumenter til eventet.</param>
        private static void PropertyChangedOnBogføringslinjeViewModelEventHandler(object sender, PropertyChangedEventArgs e)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }
        }

        /// <summary>
        /// Eventhandler, der kaldes, når en property ændres på en ViewModel for adressekontoen til en debitor.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="e">Argumenter til eventet.</param>
        private static void PropertyChangedOnAdressekontoViewModelForDebitorEventHandler(object sender, PropertyChangedEventArgs e)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }
        }

        /// <summary>
        /// Eventhandler, der kaldes, når en property ændres på en ViewModel for adressekontoen til en kreditor.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="e">Argumenter til eventet.</param>
        private static void PropertyChangedOnAdressekontoViewModelForKreditorEventHandler(object sender, PropertyChangedEventArgs e)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }
        }

        /// <summary>
        /// Eventhandler, der kaldes, når en property ændres på en ViewModel for en nyhed.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="e">Argumenter til eventet.</param>
        private static void PropertyChangedOnNyhedViewModelEventHandler(object sender, PropertyChangedEventArgs e)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }
        }

        /// <summary>
        /// Eventhandler, der kaldes, når collection af ViewModels for bogføringslinjer ændres.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="e">Argumenter til eventet.</param>
        private void CollectionChangedOnBogføringslinjeViewModelsEventHandler(object sender, NotifyCollectionChangedEventArgs e)
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
                    RaisePropertyChanged("Bogføringslinjer");
                    break;
            }
        }

        /// <summary>
        /// Eventhandler, der kaldes, når collection af ViewModels for debitorer ændres.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="e">Argumenter til eventet.</param>
        private void CollectionChangedOnDebitorViewModelsEventHandler(object sender, NotifyCollectionChangedEventArgs e)
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
                    RaisePropertyChanged("Debitorer");
                    break;
            }
        }

        /// <summary>
        /// Eventhandler, der kaldes, når collection af ViewModels for kreditorer ændres.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="e">Argumenter til eventet.</param>
        private void CollectionChangedOnKreditorerViewModelsEventHandler(object sender, NotifyCollectionChangedEventArgs e)
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
                    RaisePropertyChanged("Kreditorer");
                    break;
            }
        }

        /// <summary>
        /// Eventhandler, der kaldes, når collection af ViewModels for nyheder ændres.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="e">Argumenter til eventet.</param>
        private void CollectionChangedOnNyhedViewModelsEventHandler(object sender, NotifyCollectionChangedEventArgs e)
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
                    RaisePropertyChanged("Nyheder");
                    break;
            }
        }

        #endregion
    }
}
