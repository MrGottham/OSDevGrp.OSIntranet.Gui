using System;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Resources;

namespace OSDevGrp.OSIntranet.Gui.Models.Finansstyring
{
    /// <summary>
    /// Model for en budgetkonto.
    /// </summary>
    public class BudgetkontoModel : KontoModelBase, IBudgetkontoModel
    {
        #region Private variables

        private decimal _indtægter;
        private decimal _udgifter;
        private decimal _budgetSidsteMåned;
        private decimal _budgetÅrTilDato;
        private decimal _budgetSidsteÅr;
        private decimal _bogført;
        private decimal _bogførtSidsteMåned;
        private decimal _bogførtÅrTilDato;
        private decimal _bogførtSidsteÅr;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner model til en budgetkonto.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, som kontoen er tilknyttet.</param>
        /// <param name="kontonummer">Kontonummer.</param>
        /// <param name="kontonavn">Kontonavn.</param>
        /// <param name="kontogruppe">Unik identifikation af kontogruppen.</param>
        /// <param name="statusDato">Statusdato for opgørelse af kontoen.</param>
        /// <param name="budget">Budgetteret beløb.</param>
        /// <param name="bogført">Bogført beløb.</param>
        public BudgetkontoModel(int regnskabsnummer, string kontonummer, string kontonavn, int kontogruppe, DateTime statusDato, decimal budget = 0M, decimal bogført = 0M)
            : base(regnskabsnummer, kontonummer, kontonavn, kontogruppe, statusDato)
        {
            if (budget > 0M)
            {
                _indtægter = Math.Abs(budget);
            }
            else if (budget <= 0M)
            {
                _udgifter = Math.Abs(budget);
            }
            _bogført = bogført;
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
                return _indtægter;
            }
            set
            {
                if (value < 0M)
                {
                    throw new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "value", value), "value");
                }
                if (_indtægter == value)
                {
                    return;
                }
                _indtægter = value;
                RaisePropertyChanged("Indtægter");
                RaisePropertyChanged("Budget");
                RaisePropertyChanged("Disponibel");
            }
        }

        /// <summary>
        /// Budgetterede udgifter.
        /// </summary>
        public virtual decimal Udgifter
        {
            get
            {
                return _udgifter;
            }
            set
            {
                if (value < 0M)
                {
                    throw new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "value", value), "value");
                }
                if (_udgifter == value)
                {
                    return;
                }
                _udgifter = value;
                RaisePropertyChanged("Udgifter");
                RaisePropertyChanged("Budget");
                RaisePropertyChanged("Disponibel");
            }
        }

        /// <summary>
        /// Budgetteret beløb.
        /// </summary>
        public virtual decimal Budget
        {
            get
            {
                return Indtægter - Udgifter;
            }
        }

        /// <summary>
        /// Budgetteret beløb for sidste måned.
        /// </summary>
        public virtual decimal BudgetSidsteMåned
        {
            get
            {
                return _budgetSidsteMåned;
            }
            set
            {
                if (_budgetSidsteMåned == value)
                {
                    return;
                }
                _budgetSidsteMåned = value;
                RaisePropertyChanged("BudgetSidsteMåned");
            }
        }

        /// <summary>
        /// Budgetteret beløb for år til dato.
        /// </summary>
        public virtual decimal BudgetÅrTilDato
        {
            get
            {
                return _budgetÅrTilDato;
            }
            set
            {
                if (_budgetÅrTilDato == value)
                {
                    return;
                }
                _budgetÅrTilDato = value;
                RaisePropertyChanged("BudgetÅrTilDato");
            }
        }

        /// <summary>
        /// Budgetteret beløb for sidste år.
        /// </summary>
        public virtual decimal BudgetSidsteÅr
        {
            get
            {
                return _budgetSidsteÅr;
            }
            set
            {
                if (_budgetSidsteÅr == value)
                {
                    return;
                }
                _budgetSidsteÅr = value;
                RaisePropertyChanged("BudgetSidsteÅr");
            }
        }

        /// <summary>
        /// Bogført beløb.
        /// </summary>
        public virtual decimal Bogført
        {
            get
            {
                return _bogført;
            }
            set
            {
                if (_bogført == value)
                {
                    return;
                }
                _bogført = value;
                RaisePropertyChanged("Bogført");
                RaisePropertyChanged("Disponibel");
            }
        }

        /// <summary>
        /// Bogført beløb for sidste måned.
        /// </summary>
        public virtual decimal BogførtSidsteMåned
        {
            get
            {
                return _bogførtSidsteMåned;
            }
            set
            {
                if (_bogførtSidsteMåned == value)
                {
                    return;
                }
                _bogførtSidsteMåned = value;
                RaisePropertyChanged("BogførtSidsteMåned");
            }
        }

        /// <summary>
        /// Bogført beløb for år til dato.
        /// </summary>
        public virtual decimal BogførtÅrTilDato
        {
            get
            {
                return _bogførtÅrTilDato;
            }
            set
            {
                if (_bogførtÅrTilDato == value)
                {
                    return;
                }
                _bogførtÅrTilDato = value;
                RaisePropertyChanged("BogførtÅrTilDato");
            }
        }

        /// <summary>
        /// Bogført beløb for sidste år.
        /// </summary>
        public virtual decimal BogførtSidsteÅr
        {
            get
            {
                return _bogførtSidsteÅr;
            }
            set
            {
                if (_bogførtSidsteÅr == value)
                {
                    return;
                }
                _bogførtSidsteÅr = value;
                RaisePropertyChanged("BogførtSidsteÅr");
            }
        }

        /// <summary>
        /// Disponibel beløb.
        /// </summary>
        public virtual decimal Disponibel
        {
            get
            {
                if (Budget <= 0M)
                {
                    var disponibel = Math.Abs(Budget) - Math.Abs(Bogført);
                    return disponibel < 0M ? 0M : disponibel;
                }
                return 0M;
            }
        }

        #endregion
    }
}
