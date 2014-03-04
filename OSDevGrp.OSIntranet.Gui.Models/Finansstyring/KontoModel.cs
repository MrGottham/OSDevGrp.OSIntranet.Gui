using System;

namespace OSDevGrp.OSIntranet.Gui.Models.Finansstyring
{
    /// <summary>
    /// Model for en konto.
    /// </summary>
    public class KontoModel : KontoModelBase
    {
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
        }

        #endregion
    }
}
