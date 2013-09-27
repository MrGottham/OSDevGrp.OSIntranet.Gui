using System;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.Models.Finansstyring
{
    /// <summary>
    /// Modellen for et regnskab.
    /// </summary>
    public class RegnskabModel : ModelBase, IRegnskabModel
    {
        #region Private variables

        private readonly int _nummer;
        private string _navn;

        #endregion
        
        #region Constructor

        /// <summary>
        /// Danner model for et regnskab.
        /// </summary>
        /// <param name="nummer">Regnskabsnummer.</param>
        /// <param name="navn">Navnet på regnskabet.</param>
        public RegnskabModel(int nummer, string navn)
        {
            if (string.IsNullOrEmpty(navn))
            {
                throw new ArgumentNullException("navn");
            }
            _nummer = nummer;
            _navn = navn;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Regnskabsnummer.
        /// </summary>
        public virtual int Nummer
        {
            get
            {
                return _nummer;
            }
        }

        /// <summary>
        /// Navnet på regnskabet.
        /// </summary>
        public virtual string Navn
        {
            get
            {
                return _navn;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("value");
                }
                if (_navn == value)
                {
                    return;
                }
                _navn = value;
                RaisePropertyChanged("Navn");
            }
        }
        
        #endregion
    }
}
