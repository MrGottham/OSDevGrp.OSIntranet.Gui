using System;
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
    /// ViewModel til en budgetkonto.
    /// </summary>
    public class BudgetkontoViewModel : KontoViewModelBase<IBudgetkontoModel, IBudgetkontogruppeViewModel>, IBudgetkontoViewModel
    {
        #region Private variables

        private ICommand _refreshCommand;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner en ViewModel til en budgetkonto.
        /// </summary>
        /// <param name="regnskabViewModel">ViewModel for regnskabet, som budgetkontoen er tilknyttet.</param>
        /// <param name="budgetkontoModel">Model for budgetkontoen.</param>
        /// <param name="budgetkontogruppeViewModel">ViewModel for kontogruppen til budgetkontoen.</param>
        /// <param name="finansstyringRepository">Implementering af repositoryet til finansstyring.</param>
        /// <param name="exceptionHandlerViewModel">Implementering af ViewModel for exceptionhandleren.</param>
        public BudgetkontoViewModel(IRegnskabViewModel regnskabViewModel, IBudgetkontoModel budgetkontoModel, IBudgetkontogruppeViewModel budgetkontogruppeViewModel, IFinansstyringRepository finansstyringRepository, IExceptionHandlerViewModel exceptionHandlerViewModel)
            : base(regnskabViewModel, budgetkontoModel, budgetkontogruppeViewModel, Resource.GetText(Text.BudgetAccount), Resource.GetEmbeddedResource("Images.Budgetkonto.png"), finansstyringRepository, exceptionHandlerViewModel)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Budgetterede indtægter.
        /// </summary>
        public virtual decimal Indtægter
        {
            get { return Model.Indtægter; }
            set
            {
                try
                {
                    Model.Indtægter = value;
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex);
                }
            }
        }

        /// <summary>
        /// Tekstangivelse af budgetterede indtægter.
        /// </summary>
        public virtual string IndtægterAsText => Indtægter.ToString("C");

        /// <summary>
        /// Label til budgetterede indtægter.
        /// </summary>
        public virtual string IndtægterLabel => Resource.GetText(Text.Income);

        /// <summary>
        /// Budgetterede udgifter.
        /// </summary>
        public virtual decimal Udgifter
        {
            get { return Model.Udgifter; }
            set
            {
                try
                {
                    Model.Udgifter = value;
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex);
                }
            }
        }

        /// <summary>
        /// Tekstangivelse af budgetterede udgifter.
        /// </summary>
        public virtual string UdgifterAsText => Udgifter.ToString("C");

        /// <summary>
        /// Label til budgetterede udgifter.
        /// </summary>
        public virtual string UdgifterLabel => Resource.GetText(Text.Expenses);

        /// <summary>
        /// Budgetteret beløb.
        /// </summary>
        public virtual decimal Budget => Model.Budget;

        /// <summary>
        /// Tekstangivelse af budgetteret beløb.
        /// </summary>
        public virtual string BudgetAsText => Budget.ToString("C");

        /// <summary>
        /// Label til budgetteret beløb.
        /// </summary>
        public virtual string BudgetLabel => Resource.GetText(Text.Budget);

        /// <summary>
        /// Budgetteret beløb for sidste måned.
        /// </summary>
        public virtual decimal BudgetSidsteMåned
        {
            get { return Model.BudgetSidsteMåned; }
            set
            {
                try
                {
                    Model.BudgetSidsteMåned = value;
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex);
                }
            }
        }

        /// <summary>
        /// Tekstangivelse af budgetteret beløb for sidste måned.
        /// </summary>
        public virtual string BudgetSidsteMånedAsText => BudgetSidsteMåned.ToString("C");

        /// <summary>
        /// Label af budgetteret beløb for sidste måned.
        /// </summary>
        public virtual string BudgetSidsteMånedLabel => Resource.GetText(Text.BudgetLastMonth);

        /// <summary>
        /// Budgetteret beløb for år til dato.
        /// </summary>
        public virtual decimal BudgetÅrTilDato
        {
            get { return Model.BudgetÅrTilDato; }
            set
            {
                try
                {
                    Model.BudgetÅrTilDato = value;
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex);
                }
            }
        }

        /// <summary>
        /// Tekstangivelse af budgetteret beløb for år til dato.
        /// </summary>
        public virtual string BudgetÅrTilDatoAsText => BudgetÅrTilDato.ToString("C");

        /// <summary>
        /// Label af budgetteret beløb for år til dato.
        /// </summary>
        public virtual string BudgetÅrTilDatoLabel => Resource.GetText(Text.BudgetYearToDate);

        /// <summary>
        /// Budgetteret beløb for sidste år.
        /// </summary>
        public virtual decimal BudgetSidsteÅr
        {
            get { return Model.BudgetSidsteÅr; }
            set
            {
                try
                {
                    Model.BudgetSidsteÅr = value;
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex);
                }
            }
        }

        /// <summary>
        /// Tekstangivelse af budgetteret beløb for sidste år.
        /// </summary>
        public virtual string BudgetSidsteÅrAsText => BudgetSidsteÅr.ToString("C");

        /// <summary>
        /// Label af budgetteret beløb for sidste år.
        /// </summary>
        public virtual string BudgetSidsteÅrLabel => Resource.GetText(Text.BudgetLastYear);

        /// <summary>
        /// Bogført beløb.
        /// </summary>
        public virtual decimal Bogført
        {
            get { return Model.Bogført; }
            set
            {
                try
                {
                    Model.Bogført = value;
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex);
                }
            }
        }

        /// <summary>
        /// Tekstangivelse af bogført beløb.
        /// </summary>
        public virtual string BogførtAsText => Bogført.ToString("C");

        /// <summary>
        /// Label til bogført beløb.
        /// </summary>
        public virtual string BogførtLabel => Resource.GetText(Text.Posted);

        /// <summary>
        /// Bogført beløb for sidste måned.
        /// </summary>
        public virtual decimal BogførtSidsteMåned
        {
            get { return Model.BogførtSidsteMåned; }
            set
            {
                try
                {
                    Model.BogførtSidsteMåned = value;
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex);
                }
            }
        }

        /// <summary>
        /// Tekstangivelse af bogført beløb for sidste måned.
        /// </summary>
        public virtual string BogførtSidsteMånedAsText => BogførtSidsteMåned.ToString("C");

        /// <summary>
        /// Label af bogført beløb for sidste måned.
        /// </summary>
        public virtual string BogførtSidsteMånedLabel => Resource.GetText(Text.PostedLastMonth);

        /// <summary>
        /// Bogført beløb for år til dato.
        /// </summary>
        public virtual decimal BogførtÅrTilDato
        {
            get { return Model.BogførtÅrTilDato; }
            set
            {
                try
                {
                    Model.BogførtÅrTilDato = value;
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex);
                }
            }
        }

        /// <summary>
        /// Tekstangivelse af bogført beløb for år til dato.
        /// </summary>
        public virtual string BogførtÅrTilDatoAsText => BogførtÅrTilDato.ToString("C");

        /// <summary>
        /// Label af bogført beløb for år til dato.
        /// </summary>
        public virtual string BogførtÅrTilDatoLabel => Resource.GetText(Text.PostedYearToDate);

        /// <summary>
        /// Bogført beløb for sidste år.
        /// </summary>
        public virtual decimal BogførtSidsteÅr
        {
            get { return Model.BogførtSidsteÅr; }
            set
            {
                try
                {
                    Model.BogførtSidsteÅr = value;
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex);
                }
            }
        }

        /// <summary>
        /// Tekstangivelse af bogført beløb for sidste år.
        /// </summary>
        public virtual string BogførtSidsteÅrAsText => BogførtSidsteÅr.ToString("C");

        /// <summary>
        /// Label af bogført beløb for sidste år.
        /// </summary>
        public virtual string BogførtSidsteÅrLabel => Resource.GetText(Text.PostedLastYear);

        /// <summary>
        /// Disponibel beløb.
        /// </summary>
        public virtual decimal Disponibel => Model.Disponibel;

        /// <summary>
        /// Tekstangivelse af disponibel beløb.
        /// </summary>
        public virtual string DisponibelAsText => Disponibel.ToString("C");

        /// <summary>
        /// Label til disponibel beløb.
        /// </summary>
        public virtual string DisponibelLabel => Resource.GetText(Text.Available);

        /// <summary>
        /// Kontoens værdi pr. opgørelsestidspunktet.
        /// </summary>
        public override decimal Kontoværdi => Math.Abs(Bogført);

        /// <summary>
        /// Kommando til genindlæsning og opdatering af budgetkontoen.
        /// </summary>
        public override ICommand RefreshCommand => _refreshCommand ?? (_refreshCommand = new BudgetkontoGetCommand(new BudgetkontogrupperGetCommand(FinansstyringRepository, ExceptionHandler), FinansstyringRepository, ExceptionHandler));

        #endregion

        #region Methods

        /// <summary>
        /// Metode, der kaldes, når en property på modellen for kontoen ændres.
        /// </summary>
        /// <param name="propertyName">Navn på den ændrede property.</param>
        protected override void ModelChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Indtægter):
                    RaisePropertyChanged(nameof(IndtægterAsText));
                    break;

                case nameof(Udgifter):
                    RaisePropertyChanged(nameof(UdgifterAsText));
                    break;

                case nameof(Budget):
                    RaisePropertyChanged(nameof(BudgetAsText));
                    break;

                case nameof(BudgetSidsteMåned):
                    RaisePropertyChanged(nameof(BudgetSidsteMånedAsText));
                    break;

                case nameof(BudgetÅrTilDato):
                    RaisePropertyChanged(nameof(BudgetÅrTilDatoAsText));
                    break;

                case nameof(BudgetSidsteÅr):
                    RaisePropertyChanged(nameof(BudgetSidsteÅrAsText));
                    break;

                case nameof(Bogført):
                    RaisePropertyChanged(nameof(BogførtAsText));
                    RaisePropertyChanged(nameof(Kontoværdi));
                    break;

                case nameof(BogførtSidsteMåned):
                    RaisePropertyChanged(nameof(BogførtSidsteMånedAsText));
                    break;

                case nameof(BogførtÅrTilDato):
                    RaisePropertyChanged(nameof(BogførtÅrTilDatoAsText));
                    break;

                case nameof(BogførtSidsteÅr):
                    RaisePropertyChanged(nameof(BogførtSidsteÅrAsText));
                    break;

                case nameof(Disponibel):
                    RaisePropertyChanged(nameof(DisponibelAsText));
                    break;
            }
        }

        #endregion
    }
}