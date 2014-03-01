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
        #region Constructor

        /// <summary>
        /// Danner en model indeholdende grundlæggende kontooplysninger.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, som kontoen er tilknyttet.</param>
        /// <param name="kontonummer">Kontonummer.</param>
        /// <param name="kontonavn">Kontonavn.</param>
        /// <param name="statusDato">Statusdato for opgørelse af kontoen.</param>
        protected KontoModelBase(int regnskabsnummer, string kontonummer, string kontonavn, DateTime statusDato)
        {
            if (regnskabsnummer <= 0)
            {
                throw new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "regnskabsnummer", regnskabsnummer), "regnskabsnummer");
            }
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
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Kontonummer.
        /// </summary>
        public virtual string Kontonummer
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Kontonavn.
        /// </summary>
        public virtual string Kontonavn
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Beskrivelse.
        /// </summary>
        public virtual string Beskrivelse
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
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
                throw new NotImplementedException();
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
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
