using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
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
        private IBogføringViewModel _bogføringViewModel;
        private ITaskableCommand _bogføringSetCommand;
        private ICommand _refreshCommand;
        private readonly IRegnskabModel _regnskabModel;
        private readonly IFinansstyringRepository _finansstyringRepository;
        private readonly IExceptionHandlerViewModel _exceptionHandlerViewModel;
        private readonly ObservableCollection<IKontoViewModel> _kontoViewModels = new ObservableCollection<IKontoViewModel>();
        private readonly ObservableCollection<IBudgetkontoViewModel> _budgetkontoViewModels = new ObservableCollection<IBudgetkontoViewModel>();
        private readonly ObservableCollection<IReadOnlyBogføringslinjeViewModel> _bogføringslinjeViewModels = new ObservableCollection<IReadOnlyBogføringslinjeViewModel>();
        private readonly ObservableCollection<IBogføringsadvarselViewModel> _bogføringsadvarselViewModels = new ObservableCollection<IBogføringsadvarselViewModel>(); 
        private readonly ObservableCollection<IOpgørelseViewModel> _opgørelseViewModels = new ObservableCollection<IOpgørelseViewModel>();
        private readonly ObservableCollection<IBalanceViewModel> _balanceViewModels = new ObservableCollection<IBalanceViewModel>(); 
        private readonly ObservableCollection<IAdressekontoViewModel> _debitorerViewModels = new ObservableCollection<IAdressekontoViewModel>();
        private readonly ObservableCollection<IAdressekontoViewModel> _kreditorerViewModels = new ObservableCollection<IAdressekontoViewModel>();
        private readonly ObservableCollection<INyhedViewModel> _nyhedViewModels = new ObservableCollection<INyhedViewModel>();

        private static ObservableCollection<string> _kontoColumns;
        private static ObservableCollection<string> _budgetkontoColumns;
        private static ObservableCollection<string> _bogføringslinjeColumns;
        private static ObservableCollection<string> _opgørelseHeaders; 
        private static ObservableCollection<string> _opgørelseColumns;
        private static ObservableCollection<string> _balanceColumns;
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
                throw new ArgumentNullException(nameof(regnskabModel));
            }

            if (finansstyringRepository == null)
            {
                throw new ArgumentNullException(nameof(finansstyringRepository));
            }

            if (exceptionHandlerViewModel == null)
            {
                throw new ArgumentNullException(nameof(exceptionHandlerViewModel));
            }

            _regnskabModel = regnskabModel;
            _regnskabModel.PropertyChanged += PropertyChangedOnRegnskabModelEventHandler;
            _statusDato = statusDato;
            _finansstyringRepository = finansstyringRepository;
            _exceptionHandlerViewModel = exceptionHandlerViewModel;
            _kontoViewModels.CollectionChanged += CollectionChangedOnKontoViewModelsEventHandler;
            _budgetkontoViewModels.CollectionChanged += CollectionChangedOnBudgetkontoViewModelsEventHandler;
            _bogføringslinjeViewModels.CollectionChanged += CollectionChangedOnBogføringslinjeViewModelsEventHandler;
            _bogføringsadvarselViewModels.CollectionChanged += CollectionChangedOnBogføringsadvarselViewModelsEventHandler;
            _opgørelseViewModels.CollectionChanged += CollectionChangedOnOpgørelseViewModelsEventHandler;
            _balanceViewModels.CollectionChanged += CollectionChangedOnBalanceViewModelsEventHandler;
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
        public virtual int Nummer => _regnskabModel.Nummer;

        /// <summary>
        /// Navn.
        /// </summary>
        public virtual string Navn
        {
            get { return _regnskabModel.Navn; }
            set { _regnskabModel.Navn = value; }
        }

        /// <summary>
        /// Navn for ViewModel, som kan benyttes til visning i brugergrænsefladen.
        /// </summary>
        public override string DisplayName => Navn;

        /// <summary>
        /// Statusdato for regnskabet.
        /// </summary>
        public virtual DateTime StatusDato
        {
            get { return _statusDato; }
            set
            {
                if (_statusDato.CompareTo(value) > 0)
                {
                    throw new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, nameof(value), value), nameof(value));
                }

                if (_statusDato.CompareTo(value) == 0)
                {
                    return;
                }

                _statusDato = value;
                RaisePropertyChanged(nameof(StatusDato));
                RaisePropertyChanged(nameof(StatusDatoAsMonthText));
                RaisePropertyChanged(nameof(StatusDatoAsLastMonthText));
                RaisePropertyChanged(nameof(StatusDatoAsYearToDateText));
                RaisePropertyChanged(nameof(StatusDatoAsLastYearText));
            }
        }

        /// <summary>
        ///  Månedstekst for statusdatoen.
        /// </summary>
        public virtual string StatusDatoAsMonthText => GetMonthTextForStatusDato(StatusDato);

        /// <summary>
        /// Månedstekst for forrige måned i forhold til statusdato.
        /// </summary>
        public virtual string StatusDatoAsLastMonthText
        {
            get
            {
                DateTime lastMonth = new DateTime(StatusDato.AddMonths(-1).Year, StatusDato.AddMonths(-1).Month, DateTime.DaysInMonth(StatusDato.AddMonths(-1).Year, StatusDato.AddMonths(-1).Month));
                return GetMonthTextForStatusDato(lastMonth);
            }
        }

        /// <summary>
        /// Tekst for år til dato i forhold til statusdato.
        /// </summary>
        public virtual string StatusDatoAsYearToDateText => Resource.GetText(Text.YearToDate, StatusDato.Year);

        /// <summary>
        /// Tekst for sidste år i forhold til statusdato.
        /// </summary>
        public virtual string StatusDatoAsLastYearText
        {
            get
            {
                DateTime lastYear = new DateTime(StatusDato.AddYears(-1).Year, 12, DateTime.DaysInMonth(StatusDato.AddYears(-1).Year, 12));
                return Resource.GetText(Text.LastYear, lastYear.Year);
            }
        }

        /// <summary>
        /// Konti.
        /// </summary>
        public virtual IEnumerable<IKontoViewModel> Konti
        {
            get
            {
                IComparer<IKontoViewModel> comparer = new KontoViewModelBaseComparer<IKontoViewModel, IKontogruppeViewModel>(false);
                return _kontoViewModels.OrderBy(m => m, comparer);
            }
        }

        /// <summary>
        /// Konti fordelt på kontogrupper.
        /// </summary>
        public virtual IEnumerable<KeyValuePair<IKontogruppeViewModel, IEnumerable<IKontoViewModel>>> KontiGrouped => GenerateKontoViewModelBaseGroups(Konti, new List<IKontogruppeViewModel>(Kontogrupper));

        /// <summary>
        /// Topbenyttede konti.
        /// </summary>
        public virtual IEnumerable<IKontoViewModel> KontiTop
        {
            get
            {
                IComparer<IKontoViewModel> comparer = new KontoViewModelBaseComparer<IKontoViewModel, IKontogruppeViewModel>(true);
                return Konti.Where(m => m.Kontoværdi != 0M).OrderBy(m => m.Kontoværdi).Take(25).OrderBy(m => m, comparer);
            }
        }

        /// <summary>
        /// Topbenyttede konti fordelt på kontogrupper.
        /// </summary>
        public virtual IEnumerable<KeyValuePair<IKontogruppeViewModel, IEnumerable<IKontoViewModel>>> KontiTopGrouped => GenerateKontoViewModelBaseGroups(KontiTop, new List<IKontogruppeViewModel>(Kontogrupper));

        /// <summary>
        /// Overskrift til konti.
        /// </summary>
        public virtual string KontiHeader => Resource.GetText(Text.Accounts);

        /// <summary>
        /// Kolonneoverskrifter til konti.
        /// </summary>
        public virtual IEnumerable<string> KontiColumns
        {
            get
            {
                lock (SyncRoot)
                {
                    return _kontoColumns ?? (_kontoColumns = new ObservableCollection<string>(new Collection<string>(new List<string> { Resource.GetText(Text.AccountNumber), Resource.GetText(Text.AccountName), Resource.GetText(Text.Credit), Resource.GetText(Text.Balance), Resource.GetText(Text.Available) })));
                }
            }
        }

        /// <summary>
        /// Budgetkonti.
        /// </summary>
        public virtual IEnumerable<IBudgetkontoViewModel> Budgetkonti
        {
            get
            {
                IComparer<IBudgetkontoViewModel> comparer = new KontoViewModelBaseComparer<IBudgetkontoViewModel, IBudgetkontogruppeViewModel>(false);
                return _budgetkontoViewModels.OrderBy(m => m, comparer);
            }
        }

        /// <summary>
        /// Budgetkonti fordelt på kontogrupper til budetkonti.
        /// </summary>
        public virtual IEnumerable<KeyValuePair<IBudgetkontogruppeViewModel, IEnumerable<IBudgetkontoViewModel>>> BudgetkontiGrouped => GenerateKontoViewModelBaseGroups(Budgetkonti, new List<IBudgetkontogruppeViewModel>(Budgetkontogrupper));

        /// <summary>
        /// Topbenyttede budgetkonti.
        /// </summary>
        public virtual IEnumerable<IBudgetkontoViewModel> BudgetkontiTop
        {
            get
            {
                IComparer<IBudgetkontoViewModel> comparer = new KontoViewModelBaseComparer<IBudgetkontoViewModel, IBudgetkontogruppeViewModel>(true);
                return Budgetkonti.Where(m => m.Kontoværdi != 0M).OrderBy(m => m.Kontoværdi).Take(25).OrderBy(m => m, comparer);
            }
        }

        /// <summary>
        /// Topbenyttede budgetkonti fordelt på kontogrupper til budetkonti.
        /// </summary>
        public virtual IEnumerable<KeyValuePair<IBudgetkontogruppeViewModel, IEnumerable<IBudgetkontoViewModel>>> BudgetkontiTopGrouped => GenerateKontoViewModelBaseGroups(BudgetkontiTop, new List<IBudgetkontogruppeViewModel>(Budgetkontogrupper));

        /// <summary>
        /// Overskrift til budgetkonti.
        /// </summary>
        public virtual string BudgetkontiHeader => Resource.GetText(Text.BudgetAccounts);

        /// <summary>
        /// Kolonneoverskrifter til budgetkonti.
        /// </summary>
        public virtual IEnumerable<string> BudgetkontiColumns
        {
            get
            {
                lock (SyncRoot)
                {
                    return _budgetkontoColumns ?? (_budgetkontoColumns = new ObservableCollection<string>(new Collection<string>(new List<string> { Resource.GetText(Text.AccountNumber), Resource.GetText(Text.AccountName), Resource.GetText(Text.Budget), Resource.GetText(Text.Posted) })));
                }
            }
        }

        /// <summary>
        /// Bogføringslinjer.
        /// </summary>
        public virtual IEnumerable<IReadOnlyBogføringslinjeViewModel> Bogføringslinjer
        {
            get
            {
                IComparer<IReadOnlyBogføringslinjeViewModel> comparer = new BogføringslinjeViewModelComparer();
                return _bogføringslinjeViewModels.OrderBy(m => m, comparer);
            }
        }

        /// <summary>
        /// Overskrift til bogføringslinjer.
        /// </summary>
        public virtual string BogføringslinjerHeader => Resource.GetText(Text.Bookkeeping);

        /// <summary>
        /// Kolonneoverskrifter til bogføringslinjer.
        /// </summary>
        public virtual IEnumerable<string> BogføringslinjerColumns
        {
            get
            {
                lock (SyncRoot)
                {
                    return _bogføringslinjeColumns ?? (_bogføringslinjeColumns = new ObservableCollection<string>(new Collection<string>(new List<string> { Resource.GetText(Text.Date), Resource.GetText(Text.Reference), Resource.GetText(Text.Account), Resource.GetText(Text.Text), Resource.GetText(Text.BudgetAccount), Resource.GetText(Text.Debit), Resource.GetText(Text.Credit) })));
                }
            }
        }

        /// <summary>
        /// ViewModel, hvorfra der kan bogføres.
        /// </summary>
        public virtual IBogføringViewModel Bogføring
        {
            get
            {
                lock (SyncRoot)
                {
                    return _bogføringViewModel;
                }
            }
            private set
            {
                lock (SyncRoot)
                {
                    if (_bogføringViewModel == value)
                    {
                        return;
                    }

                    _bogføringViewModel = value;
                    RaisePropertyChanged(nameof(Bogføring));
                }
            }
        }

        /// <summary>
        /// Overskrift til en ViewModel, hvorfra der kan bogføres.
        /// </summary>
        public virtual string BogføringHeader => Resource.GetText(Text.Bookkeeping);

        /// <summary>
        /// Kommando, der kan sætte en ny ViewModel, hvorfra der kan bogføres.
        /// </summary>
        public virtual ITaskableCommand BogføringSetCommand => _bogføringSetCommand ??  (_bogføringSetCommand = new BogføringSetCommand(_finansstyringRepository, _exceptionHandlerViewModel, true));

        /// <summary>
        /// Bogføringsadvarsler.
        /// </summary>
        public virtual IEnumerable<IBogføringsadvarselViewModel> Bogføringsadvarsler
        {
            get
            {
                return _bogføringsadvarselViewModels.OrderByDescending(m => m.Tidspunkt);
            }
        }

        /// <summary>
        /// Overskrift til bogføringsadvarsler.
        /// </summary>
        public virtual string BogføringsadvarslerHeader => Resource.GetText(Text.PostingWarnings);

        /// <summary>
        /// Linjer, der indgår i årsopgørelsen.
        /// </summary>
        public virtual IEnumerable<IOpgørelseViewModel> Opgørelseslinjer
        {
            get
            {
                return _opgørelseViewModels.OrderBy(m => m.Nummer);
            }
        }

        /// <summary>
        /// Overskrifter til linjer, der indgår i årsopgørelsen.
        /// </summary>
        public virtual IEnumerable<string> OpgørelseslinjerHeaders
        {
            get
            {
                lock (SyncRoot)
                {
                    return _opgørelseHeaders ?? (_opgørelseHeaders = new ObservableCollection<string>(new Collection<string>(new List<string> {Resource.GetText(Text.MonthlyStatement), Resource.GetText(Text.AnnualStatement)})));
                }
            }
        }

        /// <summary>
        /// Kolonneoverskrifter til linjer, der indgår i årsopgørelsen.
        /// </summary>
        public virtual IEnumerable<string> OpgørelseslinjerColumns
        {
            get
            {
                lock (SyncRoot)
                {
                    return _opgørelseColumns ?? (_opgørelseColumns = new ObservableCollection<string>(new Collection<string>(new List<string> { string.Empty, Resource.GetText(Text.Budget), Resource.GetText(Text.Posted) })));
                }
            }
        }

        /// <summary>
        /// Budgetteret beløb fra årsopgørelsen.
        /// </summary>
        public virtual decimal Budget
        {
            get
            {
                return Opgørelseslinjer.Sum(m => m.Budget);
            }
        }

        /// <summary>
        /// Tekstangivelse af budgetteret beløb fra årsopgørelsen.
        /// </summary>
        public virtual string BudgetAsText => Budget.ToString("C");

        /// <summary>
        /// Label til budgetteret beløb fra årsopgørelsen.
        /// </summary>
        public virtual string BudgetLabel => Resource.GetText(Text.Budget);

        /// <summary>
        /// Budgetteret beløb for sidste måned fra årsopgørelsen.
        /// </summary>
        public virtual decimal BudgetSidsteMåned
        {
            get
            {
                return Opgørelseslinjer.Sum(m => m.BudgetSidsteMåned);
            }
        }

        /// <summary>
        /// Tekstangivelse af budgetteret beløb for sidste måned fra årsopgørelsen.
        /// </summary>
        public virtual string BudgetSidsteMånedAsText => BudgetSidsteMåned.ToString("C");

        /// <summary>
        /// Label til budgetteret beløb for sidste måned fra årsopgørelsen.
        /// </summary>
        public virtual string BudgetSidsteMånedLabel => Resource.GetText(Text.BudgetLastMonth);

        /// <summary>
        /// Budgetteret beløb for år til dato fra årsopgørelsen.
        /// </summary>
        public virtual decimal BudgetÅrTilDato
        {
            get
            {
                return Opgørelseslinjer.Sum(m => m.BudgetÅrTilDato);
            }
        }

        /// <summary>
        /// Tekstangivelse af budgetteret beløb for år til dato fra årsopgørelsen.
        /// </summary>
        public virtual string BudgetÅrTilDatoAsText => BudgetÅrTilDato.ToString("C");

        /// <summary>
        /// Label til budgetteret beløb for år til dato fra årsopgørelsen.
        /// </summary>
        public virtual string BudgetÅrTilDatoLabel => Resource.GetText(Text.BudgetYearToDate);

        /// <summary>
        /// Budgetteret beløb for sidste år fra årsopgørelsen.
        /// </summary>
        public virtual decimal BudgetSidsteÅr
        {
            get
            {
                return Opgørelseslinjer.Sum(m => m.BudgetSidsteÅr);
            }
        }

        /// <summary>
        /// Tekstangivelse af budgetteret beløb for sidste år fra årsopgørelsen.
        /// </summary>
        public virtual string BudgetSidsteÅrAsText => BudgetSidsteÅr.ToString("C");

        /// <summary>
        /// Label til budgetteret beløb for sidste år fra årsopgørelsen.
        /// </summary>
        public virtual string BudgetSidsteÅrLabel => Resource.GetText(Text.BudgetLastYear);

        /// <summary>
        /// Bogført beløb fra årsopgørelsen.
        /// </summary>
        public virtual decimal Bogført
        {
            get
            {
                return Opgørelseslinjer.Sum(m => m.Bogført);
            }
        }

        /// <summary>
        /// Tekstangivelse af bogført beløb fra årsopgørelsen.
        ///  </summary>
        public virtual string BogførtAsText => Bogført.ToString("C");

        /// <summary>
        /// Label til bogført beløb fra årsopgørelsen.
        /// </summary>
        public virtual string BogførtLabel => Resource.GetText(Text.Posted);

        /// <summary>
        /// Bogført beløb for sidste måned fra årsopgørelsen.
        /// </summary>
        public virtual decimal BogførtSidsteMåned
        {
            get
            {
                return Opgørelseslinjer.Sum(m => m.BogførtSidsteMåned);
            }
        }

        /// <summary>
        /// Tekstangivelse af bogført beløb for sidste måned fra årsopgørelsen.
        /// </summary>
        public virtual string BogførtSidsteMånedAsText => BogførtSidsteMåned.ToString("C");

        /// <summary>
        /// Label til bogført beløb for sidste måned fra årsopgørelsen.
        /// </summary>
        public virtual string BogførtSidsteMånedLabel => Resource.GetText(Text.PostedLastMonth);

        /// <summary>
        /// Bogført beløb for år til dato til fra årsopgørelsen.
        /// </summary>
        public virtual decimal BogførtÅrTilDato
        {
            get
            {
                return Opgørelseslinjer.Sum(m => m.BogførtÅrTilDato);
            }
        }

        /// <summary>
        /// Tekstangivelse af bogført beløb for år til dato til fra årsopgørelsen.
        /// </summary>
        public virtual string BogførtÅrTilDatoAsText => BogførtÅrTilDato.ToString("C");

        /// <summary>
        /// Label til bogført beløb for år til dato til fra årsopgørelsen.
        /// </summary>
        public virtual string BogførtÅrTilDatoLabel => Resource.GetText(Text.PostedYearToDate);

        /// <summary>
        /// Bogført beløb for sidste år fra årsopgørelsen.
        /// </summary>
        public virtual decimal BogførtSidsteÅr
        {
            get
            {
                return Opgørelseslinjer.Sum(m => m.BogførtSidsteÅr);
            }
        }

        /// <summary>
        /// Tekstangivelse af bogført beløb for sidste år fra årsopgørelsen.
        /// </summary>
        public virtual string BogførtSidsteÅrAsText => BogførtSidsteÅr.ToString("C");

        /// <summary>
        /// Label til bogført beløb for sidste år fra årsopgørelsen.
        /// </summary>
        public virtual string BogførtSidsteÅrLabel => Resource.GetText(Text.PostedLastYear);

        /// <summary>
        /// Overskrift til balancen.
        /// </summary>
        public virtual string BalanceHeader => Resource.GetText(Text.AccountingBalance);

        /// <summary>
        /// Linjer, der indgår i balancens aktiver.
        /// </summary>
        public virtual IEnumerable<IBalanceViewModel> Aktiver
        {
            get
            {
                return _balanceViewModels.Where(m => m.Balancetype == Balancetype.Aktiver).OrderBy(m => m.Nummer);
            }
        }

        /// <summary>
        /// Overskrift til linjer, der indgår i balancens aktiver.
        /// </summary>
        public virtual string AktiverHeader => Resource.GetText(Text.Asserts);

        /// <summary>
        /// Kolonneoverskrifter til linjer, der indgår i balancens aktiver.
        /// </summary>
        public virtual IEnumerable<string> AktiverColumns
        {
            get
            {
                lock (SyncRoot)
                {
                    return _balanceColumns ?? (_balanceColumns = new ObservableCollection<string>(new Collection<string>(new List<string> { string.Empty, Resource.GetText(Text.Balance) })));
                }
            }
        }

        /// <summary>
        /// Aktiver i alt.
        /// </summary>
        public virtual decimal AktiverIAlt
        {
            get
            {
                return Aktiver.Sum(m => m.Saldo);
            }
        }

        /// <summary>
        /// Tekstangivelse af aktiver i alt.
        /// </summary>
        public virtual string AktiverIAltAsText => AktiverIAlt.ToString("C");

        /// <summary>
        /// Label til aktiver i alt.
        /// </summary>
        public virtual string AktiverIAltLabel => Resource.GetText(Text.AssertsTotal);

        /// <summary>
        /// Linjer, der indgår i balancens passiver.
        /// </summary>
        public virtual IEnumerable<IBalanceViewModel> Passiver
        {
            get
            {
                return _balanceViewModels.Where(m => m.Balancetype == Balancetype.Passiver).OrderBy(m => m.Nummer);
            }
        }

        /// <summary>
        /// Overskrift til linjer, der indgår i balancens passiver.
        /// </summary>
        public virtual string PassiverHeader => Resource.GetText(Text.Liabilities);

        /// <summary>
        /// Kolonneoverskrifter til linjer, der indgår i balancens aktiver.
        /// </summary>
        public virtual IEnumerable<string> PassiverColumns => AktiverColumns;

        /// <summary>
        /// Passiver i alt.
        /// </summary>
        public virtual decimal PassiverIAlt
        {
            get
            {
                return Passiver.Sum(m => m.Saldo);
            }
        }

        /// <summary>
        /// Tekstangivelse af passiver i alt.
        /// </summary>
        public virtual string PassiverIAltAsText => PassiverIAlt.ToString("C");

        /// <summary>
        /// Label til passiver i alt.
        /// </summary>
        public virtual string PassiverIAltLabel => Resource.GetText(Text.LiabilitiesTotal);

        /// <summary>
        /// Debitorer.
        /// </summary>
        public virtual IEnumerable<IAdressekontoViewModel> Debitorer
        {
            get
            {
                IComparer<IAdressekontoViewModel> comparer = new AdressekontoViewModelComparer();
                return _debitorerViewModels.OrderBy(m => m, comparer);
            }
        }

        /// <summary>
        /// Overskrift til debitorer.
        /// </summary>
        public virtual string DebitorerHeader => Resource.GetText(Text.Debtors);

        /// <summary>
        /// Kreditorer.
        /// </summary>
        public virtual IEnumerable<IAdressekontoViewModel> Kreditorer
        {
            get
            {
                IComparer<IAdressekontoViewModel> comparer = new AdressekontoViewModelComparer();
                return _kreditorerViewModels.OrderBy(m => m, comparer);
            }
        }

        /// <summary>
        /// Overskrift til kreditorer.
        /// </summary>
        public virtual string KreditorerHeader => Resource.GetText(Text.Creditors);

        /// <summary>
        /// Nyheder.
        /// </summary>
        public virtual IEnumerable<INyhedViewModel> Nyheder
        {
            get
            {
                IComparer<INyhedViewModel> comparer = new NyhedViewModelComparer();
                return _nyhedViewModels.OrderBy(m => m, comparer);
            }
        }

        /// <summary>
        /// Overskrift til nyheder.
        /// </summary>
        public virtual string NyhederHeader => Resource.GetText(Text.NewsMultiple);

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

                ICollection<ICommand> executeCommands = new Collection<ICommand>
                {
                    new RelayCommand(obj => StatusDato = DateTime.Now),
                    new KontoplanGetCommand(new KontogrupperGetCommand(_finansstyringRepository, _exceptionHandlerViewModel), _finansstyringRepository, _exceptionHandlerViewModel),
                    new BudgetkontoplanGetCommand(new BudgetkontogrupperGetCommand(_finansstyringRepository, _exceptionHandlerViewModel), _finansstyringRepository, _exceptionHandlerViewModel),
                    new BogføringslinjerGetCommand(_finansstyringRepository, _exceptionHandlerViewModel), new DebitorlisteGetCommand(_finansstyringRepository, _exceptionHandlerViewModel),
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
                throw new ArgumentNullException(nameof(kontoViewModel));
            }

            // Tilføj kontoen til regnskabet.
            kontoViewModel.PropertyChanged += PropertyChangedOnKontoViewModelEventHandler;
            _kontoViewModels.Add(kontoViewModel);
            
            // Registrér kontoen til at indgå i balancen.
            IBalanceViewModel balanceViewModel = _balanceViewModels.SingleOrDefault(m => m.Nummer == kontoViewModel.Kontogruppe.Nummer);
            if (balanceViewModel == null)
            {
                balanceViewModel = kontoViewModel.Kontogruppe.CreateBalancelinje(this);
                balanceViewModel.PropertyChanged += PropertyChangedOnBalanceViewModelEventHandler;
                _balanceViewModels.Add(balanceViewModel);
            }

            balanceViewModel.Register(kontoViewModel);
        }

        /// <summary>
        /// Tilføjer en budgetkonto til regnskabet.
        /// </summary>
        /// <param name="budgetkontoViewModel">ViewModel for budgetkontoen, der skal tilføjes regnskabet.</param>
        public virtual void BudgetkontoAdd(IBudgetkontoViewModel budgetkontoViewModel)
        {
            if (budgetkontoViewModel == null)
            {
                throw new ArgumentNullException(nameof(budgetkontoViewModel));
            }

            // Tilføj budgetkontoen til regnskabet.
            budgetkontoViewModel.PropertyChanged += PropertyChangedOnBudgetkontoViewModelEventHandler;
            _budgetkontoViewModels.Add(budgetkontoViewModel);
            
            // Registrér budgetkontoen til at indgå i årsopgørelsen.
            IOpgørelseViewModel opgørelseViewModel = _opgørelseViewModels.SingleOrDefault(m => m.Nummer == budgetkontoViewModel.Kontogruppe.Nummer);
            if (opgørelseViewModel == null)
            {
                opgørelseViewModel = budgetkontoViewModel.Kontogruppe.CreateOpgørelseslinje(this);
                opgørelseViewModel.PropertyChanged += PropertyChangedOnOpgørelseViewModelEventHandler;
                _opgørelseViewModels.Add(opgørelseViewModel);
            }

            opgørelseViewModel.Register(budgetkontoViewModel);
        }

        /// <summary>
        /// Tilføjer en bogføringslinje til regnskabet.
        /// </summary>
        /// <param name="bogføringslinjeViewModel">ViewModel for bogføringslinjen, der skal tilføjes regnskabet.</param>
        public virtual void BogføringslinjeAdd(IReadOnlyBogføringslinjeViewModel bogføringslinjeViewModel)
        {
            if (bogføringslinjeViewModel == null)
            {
                throw new ArgumentNullException(nameof(bogføringslinjeViewModel));
            }

            bogføringslinjeViewModel.PropertyChanged += PropertyChangedOnBogføringslinjeViewModelEventHandler;
            _bogføringslinjeViewModels.Add(bogføringslinjeViewModel);
        }

        /// <summary>
        /// Sætter en ny ViewModel, hvorfra der kan bogføres.
        /// </summary>
        /// <param name="bogføringViewModel">Ny ViewModel, hvorfra der kan bogføres.</param>
        public virtual void BogføringSet(IBogføringViewModel bogføringViewModel)
        {
            if (bogføringViewModel == null)
            {
                throw new ArgumentNullException(nameof(bogføringViewModel));
            }

            Bogføring = bogføringViewModel;
        }

        /// <summary>
        /// Tilføjer en bogføringsadvarsel til regnskabet.
        /// </summary>
        /// <param name="bogføringsadvarselViewModel">ViewModel for bogføringsadvarslen, der skal tilføjes regnskabet.</param>
        public virtual void BogføringsadvarselAdd(IBogføringsadvarselViewModel bogføringsadvarselViewModel)
        {
            if (bogføringsadvarselViewModel == null)
            {
                throw new ArgumentNullException(nameof(bogføringsadvarselViewModel));
            }

            bogføringsadvarselViewModel.PropertyChanged += PropertyChangedOnBogføringsadvarselViewModelEventHandler;
            _bogføringsadvarselViewModels.Add(bogføringsadvarselViewModel);
        }

        /// <summary>
        /// Fjerner en bogføringsadvarsel fra regnskabet.
        /// </summary>
        /// <param name="bogføringsadvarselViewModel">ViewModel for bogføringsadvarslen, der skal fjernes fra regnskabet.</param>
        public virtual void BogføringsadvarselRemove(IBogføringsadvarselViewModel bogføringsadvarselViewModel)
        {
            if (bogføringsadvarselViewModel == null)
            {
                throw new ArgumentNullException(nameof(bogføringsadvarselViewModel));
            }

            while (_bogføringsadvarselViewModels.Contains(bogføringsadvarselViewModel))
            {
                _bogføringsadvarselViewModels.Remove(bogføringsadvarselViewModel);
                bogføringsadvarselViewModel.PropertyChanged -= PropertyChangedOnBogføringsadvarselViewModelEventHandler;
            }
        }

        /// <summary>
        /// Tilføjerer en debitor til regnskabet.
        /// </summary>
        /// <param name="adressekontoViewModel">ViewModel for adressekontoen, der skal tilføjes som debitor.</param>
        public virtual void DebitorAdd(IAdressekontoViewModel adressekontoViewModel)
        {
            if (adressekontoViewModel == null)
            {
                throw new ArgumentNullException(nameof(adressekontoViewModel));
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
                throw new ArgumentNullException(nameof(adressekontoViewModel));
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
                throw new ArgumentNullException(nameof(nyhedViewModel));
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
                throw new ArgumentNullException(nameof(kontogruppeViewModel));
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
                throw new ArgumentNullException(nameof(budgetkontogruppeViewModel));
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
        /// Danner og returnerer månedsteksten til en given statusdato.
        /// </summary>
        /// <param name="statusDato">Statusdato.</param>
        /// <returns>Månedsteksten til den givne statusdato.</returns>
        private string GetMonthTextForStatusDato(DateTime statusDato)
        {
            string monthText = statusDato.ToString("MMMM yyyy");
            return $"{monthText.Substring(0, 1).ToUpper()}{monthText.Substring(1).ToLower()}";
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
                throw new ArgumentNullException(nameof(sender));
            }

            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            switch (e.PropertyName)
            {
                case nameof(Navn):
                    RaisePropertyChanged(e.PropertyName);
                    RaisePropertyChanged(nameof(DisplayName));
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
                throw new ArgumentNullException(nameof(sender));
            }

            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            switch (e.PropertyName)
            {
                case "Kontonummer":
                    RaisePropertyChanged(nameof(Konti));
                    RaisePropertyChanged(nameof(KontiGrouped));
                    RaisePropertyChanged(nameof(KontiTop));
                    RaisePropertyChanged(nameof(KontiTopGrouped));
                    break;

                case "Kontogruppe":
                    RaisePropertyChanged(nameof(Konti));
                    RaisePropertyChanged(nameof(KontiGrouped));
                    RaisePropertyChanged(nameof(KontiTop));
                    RaisePropertyChanged(nameof(KontiTopGrouped));
                    break;

                case "Kontoværdi":
                    RaisePropertyChanged(nameof(KontiTop));
                    RaisePropertyChanged(nameof(KontiTopGrouped));
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
                throw new ArgumentNullException(nameof(sender));
            }

            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            switch (e.PropertyName)
            {
                case "Kontonummer":
                    RaisePropertyChanged(nameof(Budgetkonti));
                    RaisePropertyChanged(nameof(BudgetkontiGrouped));
                    RaisePropertyChanged(nameof(BudgetkontiTop));
                    RaisePropertyChanged(nameof(BudgetkontiTopGrouped));
                    break;

                case "Kontogruppe":
                    RaisePropertyChanged(nameof(Budgetkonti));
                    RaisePropertyChanged(nameof(BudgetkontiGrouped));
                    RaisePropertyChanged(nameof(BudgetkontiTop));
                    RaisePropertyChanged(nameof(BudgetkontiTopGrouped));
                    break;

                case "Kontoværdi":
                    RaisePropertyChanged(nameof(BudgetkontiTop));
                    RaisePropertyChanged(nameof(BudgetkontiTopGrouped));
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
                throw new ArgumentNullException(nameof(sender));
            }

            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            switch (e.PropertyName)
            {
                case "Løbenummer":
                case "Dato":
                    RaisePropertyChanged(nameof(Bogføringslinjer));
                    break;
            }
        }

        /// <summary>
        /// Eventhandler, der kaldes, når en property ændres på en ViewModel for en bogføringsadvarsel.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="e">Argumenter til eventet.</param>
        private void PropertyChangedOnBogføringsadvarselViewModelEventHandler(object sender, PropertyChangedEventArgs e)
        {
            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            switch (e.PropertyName)
            {
                case "Tidspunkt":
                    RaisePropertyChanged(nameof(Bogføringsadvarsler));
                    break;
            }
        }

        /// <summary>
        /// Eventhandler, der kaldes, når en property ændres på en ViewModel til en linje, der indgår i årsopgørelsen.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="e">Argumenter til eventet.</param>
        private void PropertyChangedOnOpgørelseViewModelEventHandler(object sender, PropertyChangedEventArgs e)
        {
            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            switch (e.PropertyName)
            {
                case nameof(Nummer):
                    RaisePropertyChanged(nameof(Opgørelseslinjer));
                    break;

                case nameof(Budget):
                    RaisePropertyChanged(nameof(Budget));
                    RaisePropertyChanged(nameof(BudgetAsText));
                    break;

                case nameof(BudgetSidsteMåned):
                    RaisePropertyChanged(nameof(BudgetSidsteMåned));
                    RaisePropertyChanged(nameof(BudgetSidsteMånedAsText));
                    break;

                case nameof(BudgetÅrTilDato):
                    RaisePropertyChanged(nameof(BudgetÅrTilDato));
                    RaisePropertyChanged(nameof(BudgetÅrTilDatoAsText));
                    break;

                case nameof(BudgetSidsteÅr):
                    RaisePropertyChanged(nameof(BudgetSidsteÅr));
                    RaisePropertyChanged(nameof(BudgetSidsteÅrAsText));
                    break;

                case nameof(Bogført):
                    RaisePropertyChanged(nameof(Bogført));
                    RaisePropertyChanged(nameof(BogførtAsText));
                    break;

                case nameof(BogførtSidsteMåned):
                    RaisePropertyChanged(nameof(BogførtSidsteMåned));
                    RaisePropertyChanged(nameof(BogførtSidsteMånedAsText));
                    break;

                case nameof(BogførtÅrTilDato):
                    RaisePropertyChanged(nameof(BogførtÅrTilDato));
                    RaisePropertyChanged(nameof(BogførtÅrTilDatoAsText));
                    break;

                case nameof(BogførtSidsteÅr):
                    RaisePropertyChanged(nameof(BogførtSidsteÅr));
                    RaisePropertyChanged(nameof(BogførtSidsteÅrAsText));
                    break;
            }
        }

        /// <summary>
        /// Eventhandler, der kaldes, når en property ændres på en ViewModel til en linje, der indgår i balancen.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="e">Argumenter til eventet.</param>
        private void PropertyChangedOnBalanceViewModelEventHandler(object sender, PropertyChangedEventArgs e)
        {
            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            IBalanceViewModel balanceViewModel = sender as IBalanceViewModel;
            if (balanceViewModel == null)
            {
                throw new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, nameof(sender), sender.GetType().Name));
            }

            switch (e.PropertyName)
            {
                case nameof(Nummer):
                    if (balanceViewModel.Balancetype == Balancetype.Aktiver)
                    {
                        RaisePropertyChanged(nameof(Aktiver));
                        break;
                    }

                    RaisePropertyChanged(nameof(Passiver));
                    break;

                case "Balancetype":
                    RaisePropertyChanged(nameof(Aktiver));
                    RaisePropertyChanged(nameof(AktiverIAlt));
                    RaisePropertyChanged(nameof(AktiverIAltAsText));
                    RaisePropertyChanged(nameof(Passiver));
                    RaisePropertyChanged(nameof(PassiverIAlt));
                    RaisePropertyChanged(nameof(PassiverIAltAsText));
                    break;

                case "Saldo":
                    if (balanceViewModel.Balancetype == Balancetype.Aktiver)
                    {
                        RaisePropertyChanged(nameof(AktiverIAlt));
                        RaisePropertyChanged(nameof(AktiverIAltAsText));
                        break;
                    }

                    RaisePropertyChanged(nameof(PassiverIAlt));
                    RaisePropertyChanged(nameof(PassiverIAltAsText));
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
                throw new ArgumentNullException(nameof(sender));
            }

            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            switch (e.PropertyName)
            {
                case nameof(Navn):
                case nameof(StatusDato):
                case "Saldo":
                    RaisePropertyChanged(nameof(Debitorer));
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
                throw new ArgumentNullException(nameof(sender));
            }

            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            switch (e.PropertyName)
            {
                case nameof(Navn):
                case nameof(StatusDato):
                case "Saldo":
                    RaisePropertyChanged(nameof(Kreditorer));
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
                throw new ArgumentNullException(nameof(sender));
            }

            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            switch (e.PropertyName)
            {
                case "Nyhedsudgivelsestidspunkt":
                case "Nyhedsaktualitet":
                    RaisePropertyChanged(nameof(Nyheder));
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
                throw new ArgumentNullException(nameof(sender));
            }

            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            switch (e.PropertyName)
            {
                case nameof(Nummer):
                    RaisePropertyChanged(nameof(Kontogrupper));
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
                throw new ArgumentNullException(nameof(sender));
            }

            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            switch (e.PropertyName)
            {
                case nameof(Nummer):
                    RaisePropertyChanged(nameof(Budgetkontogrupper));
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
                throw new ArgumentNullException(nameof(sender));
            }

            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    RaisePropertyChanged(nameof(Konti));
                    RaisePropertyChanged(nameof(KontiGrouped));
                    RaisePropertyChanged(nameof(KontiTop));
                    RaisePropertyChanged(nameof(KontiTopGrouped));
                    if (Bogføring != null)
                    {
                        break;
                    }

                    lock (SyncRoot)
                    {
                        if (BogføringSetCommand.CanExecute(this) == false)
                        {
                            break;
                        }

                        BogføringSetCommand.Execute(this);
                    }

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
                throw new ArgumentNullException(nameof(sender));
            }

            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    RaisePropertyChanged(nameof(Budgetkonti));
                    RaisePropertyChanged(nameof(BudgetkontiGrouped));
                    RaisePropertyChanged(nameof(BudgetkontiTop));
                    RaisePropertyChanged(nameof(BudgetkontiTopGrouped));
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
                throw new ArgumentNullException(nameof(sender));
            }

            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    RaisePropertyChanged(nameof(Bogføringslinjer));
                    if (Bogføring != null)
                    {
                        break;
                    }

                    lock (SyncRoot)
                    {
                        if (BogføringSetCommand.CanExecute(this) == false)
                        {
                            break;
                        }

                        BogføringSetCommand.Execute(this);
                    }

                    break;
            }
        }

        /// <summary>
        /// Eventhandler, der kaldes, når collection af ViewModels for bogføringsadvarsler ændres.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="e">Argumenter til eventet.</param>
        private void CollectionChangedOnBogføringsadvarselViewModelsEventHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    RaisePropertyChanged(nameof(Bogføringsadvarsler));
                    break;

                case NotifyCollectionChangedAction.Remove:
                    RaisePropertyChanged(nameof(Bogføringsadvarsler));
                    break;
            }
        }

        /// <summary>
        /// Eventhandler, der kaldes, når collection af ViewModels, der indgår i årsopgørelsen, ændres.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="e">Argumenter til eventet.</param>
        private void CollectionChangedOnOpgørelseViewModelsEventHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    RaisePropertyChanged(nameof(Opgørelseslinjer));
                    RaisePropertyChanged(nameof(Budget));
                    RaisePropertyChanged(nameof(BudgetAsText));
                    RaisePropertyChanged(nameof(BudgetSidsteMåned));
                    RaisePropertyChanged(nameof(BudgetSidsteMånedAsText));
                    RaisePropertyChanged(nameof(BudgetÅrTilDato));
                    RaisePropertyChanged(nameof(BudgetÅrTilDatoAsText));
                    RaisePropertyChanged(nameof(BudgetSidsteÅr));
                    RaisePropertyChanged(nameof(BudgetSidsteÅrAsText));
                    RaisePropertyChanged(nameof(Bogført));
                    RaisePropertyChanged(nameof(BogførtAsText));
                    RaisePropertyChanged(nameof(BogførtSidsteMåned));
                    RaisePropertyChanged(nameof(BogførtSidsteMånedAsText));
                    RaisePropertyChanged(nameof(BogførtÅrTilDato));
                    RaisePropertyChanged(nameof(BogførtÅrTilDatoAsText));
                    RaisePropertyChanged(nameof(BogførtSidsteÅr));
                    RaisePropertyChanged(nameof(BogførtSidsteÅrAsText));
                    break;
            }
        }

        /// <summary>
        /// Eventhandler, der kaldes, når collection af ViewModels, der indgår i balancen, ændres.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="e">Argumenter til eventet.</param>
        private void CollectionChangedOnBalanceViewModelsEventHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems == null)
                    {
                        break;
                    }

                    if (e.NewItems.OfType<IBalanceViewModel>().Any(m => m.Balancetype == Balancetype.Aktiver))
                    {
                        RaisePropertyChanged(nameof(Aktiver));
                        RaisePropertyChanged(nameof(AktiverIAlt));
                        RaisePropertyChanged(nameof(AktiverIAltAsText));
                    }

                    if (e.NewItems.OfType<IBalanceViewModel>().Any(m => m.Balancetype == Balancetype.Passiver))
                    {
                        RaisePropertyChanged(nameof(Passiver));
                        RaisePropertyChanged(nameof(PassiverIAlt));
                        RaisePropertyChanged(nameof(PassiverIAltAsText));
                    }

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
                throw new ArgumentNullException(nameof(sender));
            }

            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    RaisePropertyChanged(nameof(Debitorer));
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
                throw new ArgumentNullException(nameof(sender));
            }

            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    RaisePropertyChanged(nameof(Kreditorer));
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
                throw new ArgumentNullException(nameof(sender));
            }

            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    RaisePropertyChanged(nameof(Nyheder));
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
                throw new ArgumentNullException(nameof(sender));
            }

            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    RaisePropertyChanged(nameof(Kontogrupper));
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
                throw new ArgumentNullException(nameof(sender));
            }

            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    RaisePropertyChanged(nameof(Budgetkontogrupper));
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
                throw new ArgumentNullException(nameof(kontoViewModels));
            }

            if (kontogruppeViewModels == null)
            {
                throw new ArgumentNullException(nameof(kontogruppeViewModels));
            }

            TKontogruppeViewModel[] kontogrupper = kontogruppeViewModels.ToArray();

            IDictionary<TKontogruppeViewModel, IEnumerable<TKontoViewModel>> dictionary = new Dictionary<TKontogruppeViewModel, IEnumerable<TKontoViewModel>>();
            foreach (TKontoViewModel kontoViewModel in kontoViewModels)
            {
                TKontogruppeViewModel kontogruppeViewModel = kontogrupper.SingleOrDefault(m => m.Nummer == kontoViewModel.Kontogruppe.Nummer);
                if (Equals(kontogruppeViewModel, null))
                {
                    continue;
                }

                TKontogruppeViewModel key = dictionary.Keys.SingleOrDefault(m => m.Nummer == kontogruppeViewModel.Nummer);
                while (Equals(key, null))
                {
                    dictionary.Add(kontogruppeViewModel, new List<TKontoViewModel>());
                    key = dictionary.Keys.SingleOrDefault(m => m.Nummer == kontogruppeViewModel.Nummer);
                }

                ((IList<TKontoViewModel>)dictionary[key]).Add(kontoViewModel);
            }

            return dictionary;
        }

        #endregion
    }
}