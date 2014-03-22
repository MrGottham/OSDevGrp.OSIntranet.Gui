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
        private readonly ObservableCollection<IKontoViewModel> _kontoViewModels = new ObservableCollection<IKontoViewModel>();
        private readonly ObservableCollection<IBudgetkontoViewModel> _budgetkontoViewModels = new ObservableCollection<IBudgetkontoViewModel>();
        private readonly ObservableCollection<IReadOnlyBogføringslinjeViewModel> _bogføringslinjeViewModels = new ObservableCollection<IReadOnlyBogføringslinjeViewModel>();
        private readonly ObservableCollection<IAdressekontoViewModel> _debitorerViewModels = new ObservableCollection<IAdressekontoViewModel>();
        private readonly ObservableCollection<IAdressekontoViewModel> _kreditorerViewModels = new ObservableCollection<IAdressekontoViewModel>();
        private readonly ObservableCollection<INyhedViewModel> _nyhedViewModels = new ObservableCollection<INyhedViewModel>();

        private static ObservableCollection<string> _bogføringslinjeColumns;
        private static readonly ObservableCollection<IKontogruppeViewModel> KontogruppeViewModels = new ObservableCollection<IKontogruppeViewModel>();
        private static readonly ObservableCollection<IBudgetkontogruppeViewModel> BudgetkontogruppeViewModels = new ObservableCollection<IBudgetkontogruppeViewModel>(); 
        private static readonly object SyncRoot = new object();

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
            _kontoViewModels.CollectionChanged += CollectionChangedOnKontoViewModelsEventHandler;
            _budgetkontoViewModels.CollectionChanged += CollectionChangedOnBudgetkontoViewModelsEventHandler;
            _bogføringslinjeViewModels.CollectionChanged += CollectionChangedOnBogføringslinjeViewModelsEventHandler;
            _debitorerViewModels.CollectionChanged += CollectionChangedOnDebitorViewModelsEventHandler;
            _kreditorerViewModels.CollectionChanged += CollectionChangedOnKreditorerViewModelsEventHandler;
            _nyhedViewModels.CollectionChanged += CollectionChangedOnNyhedViewModelsEventHandler;
            KontogruppeViewModels.CollectionChanged += CollectionChangedOnKontogruppeViewModelsEventHandler;
            BudgetkontogruppeViewModels.CollectionChanged += CollectionChangedOnBudgetkontogruppeViewModelsEventHandler;
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
        /// Konti.
        /// </summary>
        public virtual IEnumerable<IKontoViewModel> Konti
        {
            get
            {
                var comparer = new KontoViewModelBaseComparer<IKontoViewModel, IKontogruppeViewModel>(false);
                return _kontoViewModels.OrderBy(m => m, comparer);
            }
        }

        /// <summary>
        /// Konti fordelt på kontogrupper.
        /// </summary>
        public virtual IEnumerable<KeyValuePair<IKontogruppeViewModel, IEnumerable<IKontoViewModel>>> KontiGrouped
        {
            get
            {
                return GenerateKontoViewModelBaseGroups(Konti, new List<IKontogruppeViewModel>(Kontogrupper));
            }
        }

        /// <summary>
        /// Topbenyttede konti.
        /// </summary>
        public virtual IEnumerable<IKontoViewModel> KontiTop
        {
            get
            {
                var comparer = new KontoViewModelBaseComparer<IKontoViewModel, IKontogruppeViewModel>(true);
                return Konti.Where(m => m.Kontoværdi != 0M).OrderBy(m => m, comparer).Take(25);
            }
        }

        /// <summary>
        /// Topbenyttede konti fordelt på kontogrupper.
        /// </summary>
        public virtual IEnumerable<KeyValuePair<IKontogruppeViewModel, IEnumerable<IKontoViewModel>>> KontiTopGrouped
        {
            get
            {
                return GenerateKontoViewModelBaseGroups(KontiTop, new List<IKontogruppeViewModel>(Kontogrupper));
            }
        }

        /// <summary>
        /// Overskrift til konti.
        /// </summary>
        public virtual string KontiHeader
        {
            get
            {
                return Resource.GetText(Text.Accounts);
            }
        }

        /// <summary>
        /// Budgetkonti.
        /// </summary>
        public virtual IEnumerable<IBudgetkontoViewModel> Budgetkonti
        {
            get
            {
                var comparer = new KontoViewModelBaseComparer<IBudgetkontoViewModel, IBudgetkontogruppeViewModel>(false);
                return _budgetkontoViewModels.OrderBy(m => m, comparer);
            }
        }

        /// <summary>
        /// Budgetkonti fordelt på kontogrupper til budetkonti.
        /// </summary>
        public virtual IEnumerable<KeyValuePair<IBudgetkontogruppeViewModel, IEnumerable<IBudgetkontoViewModel>>> BudgetkontiGrouped
        {
            get
            {
                return GenerateKontoViewModelBaseGroups(Budgetkonti, new List<IBudgetkontogruppeViewModel>(Budgetkontogrupper));
            }
        }

        /// <summary>
        /// Topbenyttede budgetkonti.
        /// </summary>
        public virtual IEnumerable<IBudgetkontoViewModel> BudgetkontiTop
        {
            get
            {
                var comparer = new KontoViewModelBaseComparer<IBudgetkontoViewModel, IBudgetkontogruppeViewModel>(true);
                return Budgetkonti.Where(m => m.Kontoværdi != 0M).OrderBy(m => m, comparer).Take(25);
            }
        }

        /// <summary>
        /// Topbenyttede budgetkonti fordelt på kontogrupper til budetkonti.
        /// </summary>
        public virtual IEnumerable<KeyValuePair<IBudgetkontogruppeViewModel, IEnumerable<IBudgetkontoViewModel>>> BudgetkontiTopGrouped
        {
            get
            {
                return GenerateKontoViewModelBaseGroups(BudgetkontiTop, new List<IBudgetkontogruppeViewModel>(Budgetkontogrupper));
            }
        }

        /// <summary>
        /// Overskrift til budgetkonti.
        /// </summary>
        public virtual string BudgetkontiHeader
        {
            get
            {
                return Resource.GetText(Text.BudgetAccounts);
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
        /// Kolonneoverskrifter til bogføringslinjer.
        /// </summary>
        public virtual IEnumerable<string> BogføringslinjerColumns
        {
            get
            {
                lock (SyncRoot)
                {
                    return _bogføringslinjeColumns ?? (_bogføringslinjeColumns = new ObservableCollection<string>(new Collection<string>(new List<string> {Resource.GetText(Text.Date), Resource.GetText(Text.Annex), Resource.GetText(Text.Account), Resource.GetText(Text.Text), Resource.GetText(Text.BudgetAccount), Resource.GetText(Text.Debit), Resource.GetText(Text.Credit)})));
                }
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
        /// Kontogrupper.
        /// </summary>
        public virtual IEnumerable<IKontogruppeViewModel> Kontogrupper
        {
            get
            {
                lock (SyncRoot)
                {
                    return KontogruppeViewModels.OrderBy(m => m.Nummer);
                }
            }
        }

        /// <summary>
        /// Kontogrupper til budgetkonti.
        /// </summary>
        public virtual IEnumerable<IBudgetkontogruppeViewModel> Budgetkontogrupper
        {
            get
            {
                lock (SyncRoot)
                {
                    return BudgetkontogruppeViewModels.OrderBy(m => m.Nummer);
                }
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
                        new RelayCommand(obj => StatusDato = DateTime.Now),
                        new KontoplanGetCommand(new KontogrupperGetCommand(_finansstyringRepository, _exceptionHandlerViewModel), _finansstyringRepository, _exceptionHandlerViewModel),
                        new BudgetkontoplanGetCommand(new BudgetkontogrupperGetCommand(_finansstyringRepository, _exceptionHandlerViewModel), _finansstyringRepository, _exceptionHandlerViewModel),
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
        /// Tilføjer en konto til regnskabet.
        /// </summary>
        /// <param name="kontoViewModel">ViewModel for kontoen, der skal tilføjes regnskabet.</param>
        public virtual void KontoAdd(IKontoViewModel kontoViewModel)
        {
            if (kontoViewModel == null)
            {
                throw new ArgumentNullException("kontoViewModel");
            }
            kontoViewModel.PropertyChanged += PropertyChangedOnKontoViewModelEventHandler;
            _kontoViewModels.Add(kontoViewModel);
        }

        /// <summary>
        /// Tilføjer en budgetkonto til regnskabet.
        /// </summary>
        /// <param name="budgetkontoViewModel">ViewModel for budgetkontoen, der skal tilføjes regnskabet.</param>
        public virtual void BudgetkontoAdd(IBudgetkontoViewModel budgetkontoViewModel)
        {
            if (budgetkontoViewModel == null)
            {
                throw new ArgumentNullException("budgetkontoViewModel");
            }
            budgetkontoViewModel.PropertyChanged += PropertyChangedOnBudgetkontoViewModelEventHandler;
            _budgetkontoViewModels.Add(budgetkontoViewModel);
        }

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
        /// <param name="nyhedViewModel">ViewModel for nyheden, der skal tilføjes regnskabet.</param>
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
        /// Tilføjer en kontogruppe til regnskabet.
        /// </summary>
        /// <param name="kontogruppeViewModel">ViewModel for kontogruppen, der skal tilføjes.</param>
        public virtual void KontogruppeAdd(IKontogruppeViewModel kontogruppeViewModel)
        {
            if (kontogruppeViewModel == null)
            {
                throw new ArgumentNullException("kontogruppeViewModel");
            }
            lock (SyncRoot)
            {
                if (KontogruppeViewModels.Any(m => m.Nummer == kontogruppeViewModel.Nummer))
                {
                    return;
                }
                kontogruppeViewModel.PropertyChanged += PropertyChangedOnKontogruppeViewModelEventHandler;
                KontogruppeViewModels.Add(kontogruppeViewModel);
            }
        }

        /// <summary>
        /// Tilføjer en kontogruppe for budgetkonti til regnskabet.
        /// </summary>
        /// <param name="budgetkontogruppeViewModel">ViewModel for budgetkontogruppen, der skal tilføjes.</param>
        public virtual void BudgetkontogruppeAdd(IBudgetkontogruppeViewModel budgetkontogruppeViewModel)
        {
            if (budgetkontogruppeViewModel == null)
            {
                throw new ArgumentNullException("budgetkontogruppeViewModel");
            }
            lock (SyncRoot)
            {
                if (BudgetkontogruppeViewModels.Any(m => m.Nummer == budgetkontogruppeViewModel.Nummer))
                {
                    return;
                }
                budgetkontogruppeViewModel.PropertyChanged += PropertyChangedOnBudgetkontogruppeViewModelEventHandler;
                BudgetkontogruppeViewModels.Add(budgetkontogruppeViewModel);
            }
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
        /// Eventhandler, der kaldes, når en property ændres på en ViewModel for en konto.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="e">Argumenter til eventet.</param>
        private void PropertyChangedOnKontoViewModelEventHandler(object sender, PropertyChangedEventArgs e)
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
                case "Kontonummer":
                    RaisePropertyChanged("Konti");
                    RaisePropertyChanged("KontiGrouped");
                    RaisePropertyChanged("KontiTop");
                    RaisePropertyChanged("KontiTopGrouped");
                    break;

                case "Kontogruppe":
                    RaisePropertyChanged("Konti");
                    RaisePropertyChanged("KontiGrouped");
                    RaisePropertyChanged("KontiTop");
                    RaisePropertyChanged("KontiTopGrouped");
                    break;

                case "Kontoværdi":
                    RaisePropertyChanged("KontiTop");
                    RaisePropertyChanged("KontiTopGrouped");
                    break;
            }
        }

        /// <summary>
        /// Eventhandler, der kaldes, når en property ændres på en ViewModel for en  budgetkonto.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="e">Argumenter til eventet.</param>
        private void PropertyChangedOnBudgetkontoViewModelEventHandler(object sender, PropertyChangedEventArgs e)
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
                case "Kontonummer":
                    RaisePropertyChanged("Budgetkonti");
                    RaisePropertyChanged("BudgetkontiGrouped");
                    RaisePropertyChanged("BudgetkontiTop");
                    RaisePropertyChanged("BudgetkontiTopGrouped");
                    break;

                case "Kontogruppe":
                    RaisePropertyChanged("Budgetkonti");
                    RaisePropertyChanged("BudgetkontiGrouped");
                    RaisePropertyChanged("BudgetkontiTop");
                    RaisePropertyChanged("BudgetkontiTopGrouped");
                    break;

                case "Kontoværdi":
                    RaisePropertyChanged("BudgetkontiTop");
                    RaisePropertyChanged("BudgetkontiTopGrouped");
                    break;
            }
        }

        /// <summary>
        /// Eventhandler, der kaldes, når en property ændres på en ViewModel for en bogføringslinje.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="e">Argumenter til eventet.</param>
        private void PropertyChangedOnBogføringslinjeViewModelEventHandler(object sender, PropertyChangedEventArgs e)
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
                case "Løbenummer":
                case "Dato":
                    RaisePropertyChanged("Bogføringslinjer");
                    break;
            }
        }

        /// <summary>
        /// Eventhandler, der kaldes, når en property ændres på en ViewModel for adressekontoen til en debitor.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="e">Argumenter til eventet.</param>
        private void PropertyChangedOnAdressekontoViewModelForDebitorEventHandler(object sender, PropertyChangedEventArgs e)
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
                case "StatusDato":
                case "Saldo":
                    RaisePropertyChanged("Debitorer");
                    break;
            }
        }

        /// <summary>
        /// Eventhandler, der kaldes, når en property ændres på en ViewModel for adressekontoen til en kreditor.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="e">Argumenter til eventet.</param>
        private void PropertyChangedOnAdressekontoViewModelForKreditorEventHandler(object sender, PropertyChangedEventArgs e)
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
                case "StatusDato":
                case "Saldo":
                    RaisePropertyChanged("Kreditorer");
                    break;
            }
        }

        /// <summary>
        /// Eventhandler, der kaldes, når en property ændres på en ViewModel for en nyhed.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="e">Argumenter til eventet.</param>
        private void PropertyChangedOnNyhedViewModelEventHandler(object sender, PropertyChangedEventArgs e)
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
                case "Nyhedsudgivelsestidspunkt":
                case "Nyhedsaktualitet":
                    RaisePropertyChanged("Nyheder");
                    break;
            }
        }

        /// <summary>
        /// Eventhandler, der kaldes, når en property ændres på en ViewModel for en kontogruppe.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="e">Argumenter til eventet.</param>
        private void PropertyChangedOnKontogruppeViewModelEventHandler(object sender, PropertyChangedEventArgs e)
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
                    RaisePropertyChanged("Kontogrupper");
                    break;
            }
        }

        /// <summary>
        /// Eventhandler, der kaldes, når en property ændres på en ViewModel for en kontogruppe til budgetkonti.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="e">Argumenter til eventet.</param>
        private void PropertyChangedOnBudgetkontogruppeViewModelEventHandler(object sender, PropertyChangedEventArgs e)
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
                    RaisePropertyChanged("Budgetkontogrupper");
                    break;
            }
        }

        /// <summary>
        /// Eventhandler, der kaldes, når collection af ViewModels for konti ændres.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="e">Argumenter til eventet.</param>
        private void CollectionChangedOnKontoViewModelsEventHandler(object sender, NotifyCollectionChangedEventArgs e)
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
                    RaisePropertyChanged("Konti");
                    RaisePropertyChanged("KontiGrouped");
                    RaisePropertyChanged("KontiTop");
                    RaisePropertyChanged("KontiTopGrouped");
                    break;
            }
        }

        /// <summary>
        /// Eventhandler, der kaldes, når collection af ViewModels for budgetkonti ændres.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="e">Argumenter til eventet.</param>
        private void CollectionChangedOnBudgetkontoViewModelsEventHandler(object sender, NotifyCollectionChangedEventArgs e)
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
                    RaisePropertyChanged("Budgetkonti");
                    RaisePropertyChanged("BudgetkontiGrouped");
                    RaisePropertyChanged("BudgetkontiTop");
                    RaisePropertyChanged("BudgetkontiTopGrouped");
                    break;
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

        /// <summary>
        /// Eventhandler, der kaldes, når collection af ViewModels for kontogrupper ændres.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="e">Argumenter til eventet.</param>
        private void CollectionChangedOnKontogruppeViewModelsEventHandler(object sender, NotifyCollectionChangedEventArgs e)
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
                    RaisePropertyChanged("Kontogrupper");
                    break;
            }
        }

        /// <summary>
        /// Eventhandler, der kaldes, når collection af ViewModels for kontogrupper til budgetkonti ændres.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="e">Argumenter til eventet.</param>
        private void CollectionChangedOnBudgetkontogruppeViewModelsEventHandler(object sender, NotifyCollectionChangedEventArgs e)
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
                    RaisePropertyChanged("Budgetkontogrupper");
                    break;
            }
        }

        /// <summary>
        /// Grupperer en liste af konti på kontogrupper.
        /// </summary>
        /// <typeparam name="TKontogruppeViewModel">Typen på kontogrupper, der skal grupperes efter.</typeparam>
        /// <typeparam name="TKontoViewModel">Typen på konti, der skal grupperes.</typeparam>
        /// <param name="kontoViewModels">Konti, der skal grupperes.</param>
        /// <param name="kontogruppeViewModels">Kontogrupper, der kan grupperes på.</param>
        /// <returns>Konti grupperet efter kontogrupper.</returns>
        private static IEnumerable<KeyValuePair<TKontogruppeViewModel, IEnumerable<TKontoViewModel>>> GenerateKontoViewModelBaseGroups<TKontogruppeViewModel, TKontoViewModel>(IEnumerable<TKontoViewModel> kontoViewModels, IEnumerable<TKontogruppeViewModel> kontogruppeViewModels) where TKontogruppeViewModel : IKontogruppeViewModelBase where TKontoViewModel : IKontoViewModelBase<TKontogruppeViewModel>
        {
            if (kontoViewModels == null)
            {
                throw new ArgumentNullException("kontoViewModels");
            }
            if (kontogruppeViewModels == null)
            {
                throw new ArgumentNullException("kontogruppeViewModels");
            }
            var kontogrupper = kontogruppeViewModels.ToArray();

            var dictionary = new Dictionary<TKontogruppeViewModel, IEnumerable<TKontoViewModel>>();
            foreach (var kontoViewModel in kontoViewModels)
            {
                var kontogruppeViewModel = kontogrupper.SingleOrDefault(m => m.Nummer == kontoViewModel.Kontogruppe.Nummer);
                if (Equals(kontogruppeViewModel, null))
                {
                    continue;
                }
                var key = dictionary.Keys.SingleOrDefault(m => m.Nummer == kontogruppeViewModel.Nummer);
                while (Equals(key, null))
                {
                    dictionary.Add(kontogruppeViewModel, new List<TKontoViewModel>());
                    key = dictionary.Keys.SingleOrDefault(m => m.Nummer == kontogruppeViewModel.Nummer);
                }
            }
            return dictionary;
        }


        #endregion
    }
}
