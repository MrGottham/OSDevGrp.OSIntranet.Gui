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
        private decimal _bogført;

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
