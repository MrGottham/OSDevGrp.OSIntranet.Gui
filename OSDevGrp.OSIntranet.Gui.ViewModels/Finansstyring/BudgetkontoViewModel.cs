using System;
using System.Windows.Input;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring
{
    /// <summary>
    /// ViewModel til en budgetkonto.
    /// </summary>
    public class BudgetkontoViewModel : KontoViewModelBase<IBudgetkontoModel, IBudgetkontogruppeViewModel>, IBudgetkontoViewModel
    {
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
                throw new NotImplementedException();
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
                throw new NotImplementedException();
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
                throw new NotImplementedException();
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
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
