using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring
{
    /// <summary>
    /// ViewModel for en linje i opgørelsen.
    /// </summary>
    public class OpgørelseViewModel : BudgetkontogruppeViewModel, IOpgørelseViewModel
    {
        #region Private variables

        private readonly IRegnskabViewModel _regnskabViewModel;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner en ViewModel til en linje i opgørelsen.
        /// </summary>
        /// <param name="regnskabViewModel">ViewModel for regnskabet, som opgørelseslinjen er tilknyttet.</param>
        /// <param name="budgetkontogruppeModel">Model for gruppen af budgetkonti, som opgørelseslinjen baserer sig på.</param>
        /// <param name="exceptionHandlerViewModel">Implementering af ViewModel for exceptionhandleren.</param>
        public OpgørelseViewModel(IRegnskabViewModel regnskabViewModel, IBudgetkontogruppeModel budgetkontogruppeModel, IExceptionHandlerViewModel exceptionHandlerViewModel)
            : base(budgetkontogruppeModel, exceptionHandlerViewModel)
        {
            if (regnskabViewModel == null)
            {
                throw new ArgumentNullException(nameof(regnskabViewModel));
            }

            _regnskabViewModel = regnskabViewModel;
            _regnskabViewModel.PropertyChanged += PropertyChangedOnRegnskabViewModelEventHandler;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Regnskabet, som opgørelseslinjen er tilkyttet.
        /// </summary>
        public virtual IRegnskabViewModel Regnskab => _regnskabViewModel;

        /// <summary>
        /// Registrerede budgetkonti, som indgår i opgørelseslinjen.
        /// </summary>
        public virtual IEnumerable<IBudgetkontoViewModel> Budgetkonti
        {
            get
            {
                return _regnskabViewModel.Budgetkonti
                    .Where(m => m.Kontogruppe != null && m.Kontogruppe.Nummer == Nummer && m.ErRegistreret)
                    .OrderBy(m => m.Kontonummer);
            }
        }

        /// <summary>
        /// Samlet budgetteret beløb for opgørelseslinjen.
        /// </summary>
        public virtual decimal Budget
        {
            get
            {
                return Budgetkonti.Sum(m => m.Budget);
            }
        }

        /// <summary>
        /// Tekstangivelse af samlet budgetteret beløb for opgørelseslinjen.
        /// </summary>
        public virtual string BudgetAsText => Budget.ToString("C");

        /// <summary>
        /// Label til samlet budgetteret beløb for opgørelseslinjen.
        /// </summary>
        public virtual string BudgetLabel => Resource.GetText(Text.Budget);

        /// <summary>
        /// Samlet budgetteret beløb for sidste måned til opgørelseslinjen.
        /// </summary>
        public virtual decimal BudgetSidsteMåned
        {
            get
            {
                return Budgetkonti.Sum(m => m.BudgetSidsteMåned);
            }
        }

        /// <summary>
        /// Tekstangivelse af samlet budgetteret beløb for sidste måned til opgørelseslinjen.
        /// </summary>
        public virtual string BudgetSidsteMånedAsText => BudgetSidsteMåned.ToString("C");

        /// <summary>
        /// Label til samlet budgetteret beløb for sidste måned til opgørelseslinjen.
        /// </summary>
        public virtual string BudgetSidsteMånedLabel => Resource.GetText(Text.BudgetLastMonth);

        /// <summary>
        /// Samlet budgetteret beløb for år til dato til opgørelseslinjen.
        /// </summary>
        public virtual decimal BudgetÅrTilDato
        {
            get
            {
                return Budgetkonti.Sum(m => m.BudgetÅrTilDato);
            }
        }

        /// <summary>
        /// Tekstangivelse af samlet budgetteret beløb for år til dato til opgørelseslinjen.
        /// </summary>
        public virtual string BudgetÅrTilDatoAsText => BudgetÅrTilDato.ToString("C");

        /// <summary>
        /// Label til samlet budgetteret beløb for år til dato til opgørelseslinjen.
        /// </summary>
        public virtual string BudgetÅrTilDatoLabel => Resource.GetText(Text.BudgetYearToDate);

        /// <summary>
        /// Samlet budgetteret beløb for sidste år til opgørelseslinjen.
        /// </summary>
        public virtual decimal BudgetSidsteÅr
        {
            get
            {
                return Budgetkonti.Sum(m => m.BudgetSidsteÅr);
            }
        }

        /// <summary>
        /// Tekstangivelse af samlet budgetteret beløb for sidste år til opgørelseslinjen.
        /// </summary>
        public virtual string BudgetSidsteÅrAsText => BudgetSidsteÅr.ToString("C");

        /// <summary>
        /// Label til samlet budgetteret beløb for sidste år til opgørelseslinjen..
        /// </summary>
        public virtual string BudgetSidsteÅrLabel => Resource.GetText(Text.BudgetLastYear);

        /// <summary>
        /// Samlet bogført beløb til opgørelseslinjen.
        /// </summary>
        public virtual decimal Bogført
        {
            get
            {
                return Budgetkonti.Sum(m => m.Bogført);
            }
        }

        /// <summary>
        /// Tekstangivelse af samlet bogført beløb til opgørelseslinjen.
        /// </summary>
        public virtual string BogførtAsText => Bogført.ToString("C");

        /// <summary>
        /// Label til samlet bogført beløb til opgørelseslinjen.
        /// </summary>
        public virtual string BogførtLabel => Resource.GetText(Text.Posted);

        /// <summary>
        /// Samlet bogført beløb for sidste måned til opgørelseslinjen.
        /// </summary>
        public virtual decimal BogførtSidsteMåned
        {
            get
            {
                return Budgetkonti.Sum(m => m.BogførtSidsteMåned);
            }
        }

        /// <summary>
        /// Tekstangivelse af samlet bogført beløb for sidste måned til opgørelseslinjen.
        /// </summary>
        public virtual string BogførtSidsteMånedAsText => BogførtSidsteMåned.ToString("C");

        /// <summary>
        /// Label til samlet bogført beløb for sidste måned til opgørelseslinjen.
        /// </summary>
        public virtual string BogførtSidsteMånedLabel => Resource.GetText(Text.PostedLastMonth);

        /// <summary>
        /// Samlet bogført beløb for år til dato til opgørelseslinjen.
        /// </summary>
        public virtual decimal BogførtÅrTilDato
        {
            get
            {
                return Budgetkonti.Sum(m => m.BogførtÅrTilDato);
            }
        }

        /// <summary>
        /// Tekstangivelse af samlet bogført beløb for år til dato til opgørelseslinjen.
        /// </summary>
        public virtual string BogførtÅrTilDatoAsText => BogførtÅrTilDato.ToString("C");

        /// <summary>
        /// Label til samlet bogført beløb for år til dato til opgørelseslinjen.
        /// </summary>
        public virtual string BogførtÅrTilDatoLabel => Resource.GetText(Text.PostedYearToDate);

        /// <summary>
        /// Samlet bogført beløb for sidste år til opgørelseslinjen.
        /// </summary>
        public virtual decimal BogførtSidsteÅr
        {
            get
            {
                return Budgetkonti.Sum(m => m.BogførtSidsteÅr);
            }
        }

        /// <summary>
        /// Tekstangivelse af samlet bogført beløb for sidste år til opgørelseslinjen.
        /// </summary>
        public virtual string BogførtSidsteÅrAsText => BogførtSidsteÅr.ToString("C");

        /// <summary>
        /// Label til samlet bogført beløb for sidste år til opgørelseslinjen.
        /// </summary>
        public virtual string BogførtSidsteÅrLabel
        {
            get
            {
                return Resource.GetText(Text.PostedLastYear);
            }
        }

        /// <summary>
        /// Samlet disponibel beløb til opgørelseslinjen.
        /// </summary>
        public virtual decimal Disponibel
        {
            get
            {
                return Budgetkonti.Sum(m => m.Disponibel);
            }
        }

        /// <summary>
        /// Tekstangivelse af samlet disponibel beløb til opgørelseslinjen.
        /// </summary>
        public virtual string DisponibelAsText => Disponibel.ToString("C");

        /// <summary>
        /// Label til samlet disponibel beløb til opgørelseslinjen.
        /// </summary>
        public virtual string DisponibelLabel => Resource.GetText(Text.Available);

        #endregion

        #region Methods

        /// <summary>
        /// Registrerer en budgetkonto til at indgå i opgørelseslinjen.
        /// </summary>
        /// <param name="budgetkontoViewModel">ViewModel for budgetkontoen, der skal indgå i opgørelseslinjen.</param>
        public virtual void Register(IBudgetkontoViewModel budgetkontoViewModel)
        {
            if (budgetkontoViewModel == null)
            {
                throw new ArgumentNullException(nameof(budgetkontoViewModel));
            }

            if (_regnskabViewModel.Budgetkonti.Contains(budgetkontoViewModel) == false || budgetkontoViewModel.Kontogruppe == null || budgetkontoViewModel.Kontogruppe.Nummer != Nummer || budgetkontoViewModel.ErRegistreret)
            {
                return;
            }

            budgetkontoViewModel.ErRegistreret = true;
            budgetkontoViewModel.PropertyChanged += PropertyChangedOnBudgetkontoViewModelEventHandler;
            RaisePropertyChanged(nameof(Budgetkonti));
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
            RaisePropertyChanged(nameof(Disponibel));
            RaisePropertyChanged(nameof(DisponibelAsText));
        }

        /// <summary>
        /// Metode, der kaldes, når den underlæggende model ændres.
        /// </summary>
        /// <param name="propertyName">Navn på property, som er blevet ændret.</param>
        protected override void ModelChanged(string propertyName)
        {
            base.ModelChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Nummer):
                    foreach (IBudgetkontoViewModel budgetkontoViewModel in _regnskabViewModel.Budgetkonti.Where(m => m.Kontogruppe != null && m.Kontogruppe.Nummer == Nummer && m.ErRegistreret))
                    {
                        budgetkontoViewModel.ErRegistreret = false;
                    }

                    foreach (IBudgetkontoViewModel budgetkontoViewModel in _regnskabViewModel.Budgetkonti.Where(m => m.Kontogruppe != null && m.Kontogruppe.Nummer == Nummer && m.ErRegistreret == false))
                    {
                        Register(budgetkontoViewModel);
                    }

                    RaisePropertyChanged(nameof(Budgetkonti));
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
                    RaisePropertyChanged(nameof(Disponibel));
                    RaisePropertyChanged(nameof(DisponibelAsText));
                    break;
            }
        }

        /// <summary>
        /// Event, der rejses, når en property ændres på ViewModel for regnskabet, som balancelinjen er tilknyttet.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        private void PropertyChangedOnRegnskabViewModelEventHandler(object sender, PropertyChangedEventArgs eventArgs)
        {
            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            if (eventArgs == null)
            {
                throw new ArgumentNullException(nameof(eventArgs));
            }

            switch (eventArgs.PropertyName)
            {
                case nameof(Budgetkonti):
                    RaisePropertyChanged(nameof(Budgetkonti));
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
                    RaisePropertyChanged(nameof(Disponibel));
                    RaisePropertyChanged(nameof(DisponibelAsText));
                    break;
            }
        }

        /// <summary>
        /// Event, der rejses, når en property ændres på ViewModel for en konto, der er registreret til brug i denne balancelinje.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        private void PropertyChangedOnBudgetkontoViewModelEventHandler(object sender, PropertyChangedEventArgs eventArgs)
        {
            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            if (eventArgs == null)
            {
                throw new ArgumentNullException(nameof(eventArgs));
            }

            IBudgetkontoViewModel budgetkontoViewModel = sender as IBudgetkontoViewModel;
            if (budgetkontoViewModel == null)
            {
                throw new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, nameof(sender), sender.GetType().Name));
            }

            switch (eventArgs.PropertyName)
            {
                case "Kontonummer":
                    RaisePropertyChanged(nameof(Budgetkonti));
                    break;

                case "Kontogruppe":
                    if ((budgetkontoViewModel.Kontogruppe == null || budgetkontoViewModel.Kontogruppe.Nummer != Nummer) && budgetkontoViewModel.ErRegistreret)
                    {
                        budgetkontoViewModel.PropertyChanged -= PropertyChangedOnBudgetkontoViewModelEventHandler;
                        budgetkontoViewModel.ErRegistreret = false;
                    }

                    RaisePropertyChanged(nameof(Budgetkonti));
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
                    RaisePropertyChanged(nameof(Disponibel));
                    RaisePropertyChanged(nameof(DisponibelAsText));
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

                case nameof(Disponibel):
                    RaisePropertyChanged(nameof(Disponibel));
                    RaisePropertyChanged(nameof(DisponibelAsText));
                    break;

                case "ErRegistreret":
                    if (budgetkontoViewModel.ErRegistreret)
                    {
                        budgetkontoViewModel.PropertyChanged -= PropertyChangedOnBudgetkontoViewModelEventHandler;
                        budgetkontoViewModel.ErRegistreret = false;
                    }

                    RaisePropertyChanged(nameof(Budgetkonti));
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
                    RaisePropertyChanged(nameof(Disponibel));
                    RaisePropertyChanged(nameof(DisponibelAsText));
                    break;
            }
        }

        #endregion
    }
}