﻿using System;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;

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
                throw new NotImplementedException();
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
                throw new NotImplementedException();
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
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Disponibel beløb.
        /// </summary>
        public virtual decimal Disponibel
        {
            get
            {
                if (Budget < 0M)
                {
                    return Math.Abs(Budget) - Math.Abs(Bogført);
                }
                return 0M;
            }
        }

        #endregion
    }
}
