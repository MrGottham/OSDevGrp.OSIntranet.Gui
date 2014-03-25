using System;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.Models.Finansstyring
{
    /// <summary>
    /// Model, der indeholder en advarsel ved en bogføring.
    /// </summary>
    public class BogføringsadvarselModel : ModelBase, IBogføringsadvarselModel
    {
        #region Private variables

        private readonly string _advarsel;
        private readonly string _kontonummer;
        private readonly string _kontonavn;
        private readonly decimal _beløb;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner en model, der indeholder en advarsel ved en bogføring.
        /// </summary>
        /// <param name="advarsel">Tekstangivelse af advarslen, som er opstået ved bogføring.</param>
        /// <param name="kontonummer">Kontonummer på kontoen, hvorpå advarslen er opstået ved bogføring.</param>
        /// <param name="kontonavn">Kontonavnet på kontoen, hvorpå advarslen er opstået ved bogføring.</param>
        /// <param name="beløb">Beløbet, der har medført advarslen.</param>
        public BogføringsadvarselModel(string advarsel, string kontonummer, string kontonavn, decimal beløb)
        {
            if (string.IsNullOrEmpty(advarsel))
            {
                throw new ArgumentNullException("advarsel");
            }
            if (string.IsNullOrEmpty(kontonummer))
            {
                throw new ArgumentNullException("kontonummer");
            }
            if (string.IsNullOrEmpty(kontonavn))
            {
                throw new ArgumentNullException("kontonavn");
            }
            _advarsel = advarsel;
            _kontonummer = kontonummer;
            _kontonavn = kontonavn;
            _beløb = beløb;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Tekstangivelse af advarslen, som er opstået ved bogføring.
        /// </summary>
        public virtual string Advarsel
        {
            get
            {
                return _advarsel;
            }
        }

        /// <summary>
        /// Kontonummer på kontoen, hvorpå advarslen er opstået ved bogføring.
        /// </summary>
        public virtual string Kontonummer
        {
            get
            {
                return _kontonummer;
            }
        }

        /// <summary>
        /// Kontonavnet på kontoen, hvorpå advarslen er opstået ved bogføring.
        /// </summary>
        public virtual string Kontonavn
        {
            get
            {
                return _kontonavn;
            }
        }

        /// <summary>
        /// Beløbet, der har medført advarslen.
        /// </summary>
        public virtual decimal Beløb
        {
            get
            {
                return _beløb;
            }
        }

        #endregion
    }
}
