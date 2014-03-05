using System;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.Models.Finansstyring
{
    /// <summary>
    /// Model for en konto.
    /// </summary>
    public class KontoModel : KontoModelBase, IKontoModel
    {
        #region Private variables

        private decimal _saldo;
        private decimal _kredit;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner model til en konto.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, som kontoen er tilknyttet.</param>
        /// <param name="kontonummer">Kontonummer.</param>
        /// <param name="kontonavn">Kontonavn.</param>
        /// <param name="kontogruppe">Unik identifikation af kontogruppen.</param>
        /// <param name="statusDato">Statusdato for opgørelse af kontoen.</param>
        /// <param name="saldo">Saldo.</param>
        /// <param name="kredit">Kreditbeløb.</param>
        public KontoModel(int regnskabsnummer, string kontonummer, string kontonavn, int kontogruppe, DateTime statusDato, decimal saldo = 0M, decimal kredit = 0M)
            : base(regnskabsnummer, kontonummer, kontonavn, kontogruppe, statusDato)
        {
            _saldo = saldo;
            _kredit = kredit;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Kreditbeløb.
        /// </summary>
        public virtual decimal Kredit
        {
            get
            {
                return _kredit;
            }
            set
            {
                if (_kredit == value)
                {
                    return;
                }
                _kredit = value;
                RaisePropertyChanged("Kredit");
                RaisePropertyChanged("Disponibel");
            }
        }

        /// <summary>
        /// Saldo.
        /// </summary>
        public virtual decimal Saldo
        {
            get
            {
                return _saldo;
            }
            set
            {
                if (_saldo == value)
                {
                    return;
                }
                _saldo = value;
                RaisePropertyChanged("Saldo");
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
                return Kredit + Saldo;
            }
        }

        #endregion
    }
}
