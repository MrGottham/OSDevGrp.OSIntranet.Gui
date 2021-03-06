﻿using System;
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
            get
            {
                return Model.Indtægter;
            }
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
        public virtual string IndtægterAsText
        {
            get
            {
                return Indtægter.ToString("C");
            }
        }

        /// <summary>
        /// Label til budgetterede indtægter.
        /// </summary>
        public virtual string IndtægterLabel
        {
            get
            {
                return Resource.GetText(Text.Income);
            }
        }

        /// <summary>
        /// Budgetterede udgifter.
        /// </summary>
        public virtual decimal Udgifter
        {
            get
            {
                return Model.Udgifter;
            }
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
        public virtual string UdgifterAsText
        {
            get
            {
                return Udgifter.ToString("C");
            }
        }

        /// <summary>
        /// Label til budgetterede udgifter.
        /// </summary>
        public virtual string UdgifterLabel
        {
            get
            {
                return Resource.GetText(Text.Expenses);
            }
        }

        /// <summary>
        /// Budgetteret beløb.
        /// </summary>
        public virtual decimal Budget
        {
            get
            {
                return Model.Budget;
            }
        }

        /// <summary>
        /// Tekstangivelse af budgetteret beløb.
        /// </summary>
        public virtual string BudgetAsText
        {
            get
            {
                return Budget.ToString("C");
            }
        }

        /// <summary>
        /// Label til budgetteret beløb.
        /// </summary>
        public virtual string BudgetLabel
        {
            get
            {
                return Resource.GetText(Text.Budget);
            }
        }

        /// <summary>
        /// Budgetteret beløb for sidste måned.
        /// </summary>
        public virtual decimal BudgetSidsteMåned
        {
            get
            {
                return Model.BudgetSidsteMåned;
            }
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
        public virtual string BudgetSidsteMånedAsText
        {
            get
            {
                return BudgetSidsteMåned.ToString("C");
            }
        }

        /// <summary>
        /// Label af budgetteret beløb for sidste måned.
        /// </summary>
        public virtual string BudgetSidsteMånedLabel
        {
            get
            {
                return Resource.GetText(Text.BudgetLastMonth);
            }
        }

        /// <summary>
        /// Budgetteret beløb for år til dato.
        /// </summary>
        public virtual decimal BudgetÅrTilDato
        {
            get
            {
                return Model.BudgetÅrTilDato;
            }
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
        public virtual string BudgetÅrTilDatoAsText
        {
            get
            {
                return BudgetÅrTilDato.ToString("C");
            }
        }

        /// <summary>
        /// Label af budgetteret beløb for år til dato.
        /// </summary>
        public virtual string BudgetÅrTilDatoLabel
        {
            get
            {
                return Resource.GetText(Text.BudgetYearToDate);
            }
        }

        /// <summary>
        /// Budgetteret beløb for sidste år.
        /// </summary>
        public virtual decimal BudgetSidsteÅr
        {
            get
            {
                return Model.BudgetSidsteÅr;
            }
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
        public virtual string BudgetSidsteÅrAsText
        {
            get
            {
                return BudgetSidsteÅr.ToString("C");
            }
        }

        /// <summary>
        /// Label af budgetteret beløb for sidste år.
        /// </summary>
        public virtual string BudgetSidsteÅrLabel
        {
            get
            {
                return Resource.GetText(Text.BudgetLastYear);
            }
        }

        /// <summary>
        /// Bogført beløb.
        /// </summary>
        public virtual decimal Bogført
        {
            get
            {
                return Model.Bogført;
            }
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
        public virtual string BogførtAsText
        {
            get
            {
                return Bogført.ToString("C");
            }
        }

        /// <summary>
        /// Label til bogført beløb.
        /// </summary>
        public virtual string BogførtLabel
        {
            get
            {
                return Resource.GetText(Text.Bookkeeped);
            }
        }

        /// <summary>
        /// Bogført beløb for sidste måned.
        /// </summary>
        public virtual decimal BogførtSidsteMåned
        {
            get
            {
                return Model.BogførtSidsteMåned;
            }
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
        public virtual string BogførtSidsteMånedAsText
        {
            get
            {
                return BogførtSidsteMåned.ToString("C");
            }
        }

        /// <summary>
        /// Label af bogført beløb for sidste måned.
        /// </summary>
        public virtual string BogførtSidsteMånedLabel
        {
            get
            {
                return Resource.GetText(Text.BookkeepedLastMonth);
            }
        }

        /// <summary>
        /// Bogført beløb for år til dato.
        /// </summary>
        public virtual decimal BogførtÅrTilDato
        {
            get
            {
                return Model.BogførtÅrTilDato;
            }
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
        public virtual string BogførtÅrTilDatoAsText
        {
            get
            {
                return BogførtÅrTilDato.ToString("C");
            }
        }

        /// <summary>
        /// Label af bogført beløb for år til dato.
        /// </summary>
        public virtual string BogførtÅrTilDatoLabel
        {
            get
            {
                return Resource.GetText(Text.BookkeepedYearToDate);
            }
        }

        /// <summary>
        /// Bogført beløb for sidste år.
        /// </summary>
        public virtual decimal BogførtSidsteÅr
        {
            get
            {
                return Model.BogførtSidsteÅr;
            }
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
        public virtual string BogførtSidsteÅrAsText
        {
            get
            {
                return BogførtSidsteÅr.ToString("C");
            }
        }

        /// <summary>
        /// Label af bogført beløb for sidste år.
        /// </summary>
        public virtual string BogførtSidsteÅrLabel
        {
            get
            {
                return Resource.GetText(Text.BookkeepedLastYear);
            }
        }

        /// <summary>
        /// Disponibel beløb.
        /// </summary>
        public virtual decimal Disponibel
        {
            get
            {
                return Model.Disponibel;
            }
        }

        /// <summary>
        /// Tekstangivelse af disponibel beløb.
        /// </summary>
        public virtual string DisponibelAsText
        {
            get
            {
                return Disponibel.ToString("C");
            }
        }

        /// <summary>
        /// Label til disponibel beløb.
        /// </summary>
        public virtual string DisponibelLabel
        {
            get
            {
                return Resource.GetText(Text.Available);
            }
        }

        /// <summary>
        /// Kontoens værdi pr. opgørelsestidspunktet.
        /// </summary>
        public override decimal Kontoværdi
        {
            get
            {
                return Math.Abs(Bogført);
            }
        }

        /// <summary>
        /// Kommando til genindlæsning og opdatering af budgetkontoen.
        /// </summary>
        public override ICommand RefreshCommand
        {
            get
            {
                return _refreshCommand ?? (_refreshCommand = new BudgetkontoGetCommand(new BudgetkontogrupperGetCommand(FinansstyringRepository, ExceptionHandler), FinansstyringRepository, ExceptionHandler));
            }
        }

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
                case "Indtægter":
                    RaisePropertyChanged("IndtægterAsText");
                    break;

                case "Udgifter":
                    RaisePropertyChanged("UdgifterAsText");
                    break;

                case "Budget":
                    RaisePropertyChanged("BudgetAsText");
                    break;

                case "BudgetSidsteMåned":
                    RaisePropertyChanged("BudgetSidsteMånedAsText");
                    break;

                case "BudgetÅrTilDato":
                    RaisePropertyChanged("BudgetÅrTilDatoAsText");
                    break;

                case "BudgetSidsteÅr":
                    RaisePropertyChanged("BudgetSidsteÅrAsText");
                    break;

                case "Bogført":
                    RaisePropertyChanged("BogførtAsText");
                    RaisePropertyChanged("Kontoværdi");
                    break;

                case "BogførtSidsteMåned":
                    RaisePropertyChanged("BogførtSidsteMånedAsText");
                    break;

                case "BogførtÅrTilDato":
                    RaisePropertyChanged("BogførtÅrTilDatoAsText");
                    break;

                case "BogførtSidsteÅr":
                    RaisePropertyChanged("BogførtSidsteÅrAsText");
                    break;

                case "Disponibel":
                    RaisePropertyChanged("DisponibelAsText");
                    break;
            }
        }

        #endregion
    }
}
