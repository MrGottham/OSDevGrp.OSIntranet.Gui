using System;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Resources;

namespace OSDevGrp.OSIntranet.Gui.Models.Finansstyring
{
    /// <summary>
    /// Model indeholdende grundlæggende kontooplysninger.
    /// </summary>
    public abstract class KontoModelBase : ModelBase, IKontoModelBase
    {
        #region Private variables

        private readonly int _regnskabsnummer;
        private readonly string _kontonummer;
        private string _kontonavn;
        private string _beskrivelse;
        private string _notat;
        private int _kontogruppe;
        private DateTime _statusDato;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner en model indeholdende grundlæggende kontooplysninger.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, som kontoen er tilknyttet.</param>
        /// <param name="kontonummer">Kontonummer.</param>
        /// <param name="kontonavn">Kontonavn.</param>
        /// <param name="kontogruppe">Unik identifikation af kontogruppen.</param>
        /// <param name="statusDato">Statusdato for opgørelse af kontoen.</param>
        protected KontoModelBase(int regnskabsnummer, string kontonummer, string kontonavn, int kontogruppe, DateTime statusDato)
        {
            if (regnskabsnummer <= 0)
            {
                throw new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "regnskabsnummer", regnskabsnummer), "regnskabsnummer");
            }
            if (string.IsNullOrEmpty(kontonummer))
            {
                throw new ArgumentNullException("kontonummer");
            }
            if (string.IsNullOrEmpty(kontonavn))
            {
                throw new ArgumentNullException("kontonavn");
            }
            if (kontogruppe <= 0)
            {
                throw new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "kontogruppe", kontogruppe), "kontogruppe");
            }
            _regnskabsnummer = regnskabsnummer;
            _kontonummer = kontonummer;
            _kontonavn = kontonavn;
            _kontogruppe = kontogruppe;
            _statusDato = statusDato;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Regnskabsnummer, som kontoen er tilknyttet.
        /// </summary>
        public virtual int Regnskabsnummer
        {
            get
            {
                return _regnskabsnummer;
            }
        }

        /// <summary>
        /// Kontonummer.
        /// </summary>
        public virtual string Kontonummer
        {
            get
            {
                return _kontonummer;
            }
        }

        /// <summary>
        /// Kontonavn.
        /// </summary>
        public virtual string Kontonavn
        {
            get
            {
                return _kontonavn;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("value");
                }
                if (_kontonavn == value)
                {
                    return;
                }
                _kontonavn = value;
                RaisePropertyChanged("Kontonavn");
            }
        }

        /// <summary>
        /// Beskrivelse.
        /// </summary>
        public virtual string Beskrivelse
        {
            get
            {
                return _beskrivelse;
            }
            set
            {
                _beskrivelse = value;
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Notat.
        /// </summary>
        public virtual string Notat
        {
            get
            {
                return _notat;
            }
            set
            {
                _notat = value;
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Unik identifikation af kontogruppen.
        /// </summary>
        public virtual int Kontogruppe
        {
            get
            {
                return _kontogruppe;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Statusdato for opgørelse af kontoen.
        /// </summary>
        public virtual DateTime StatusDato
        {
            get
            {
                return _statusDato;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
