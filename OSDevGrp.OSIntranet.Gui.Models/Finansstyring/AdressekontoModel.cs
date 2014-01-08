using System;
using System.Text;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Resources;

namespace OSDevGrp.OSIntranet.Gui.Models.Finansstyring
{
    /// <summary>
    /// Model for en adressekonto. 
    /// </summary>
    public class AdressekontoModel : ModelBase, IAdressekontoModel
    {
        #region Private variables

        private readonly int _regnskabsnummer;
        private readonly int _nummer;
        private string _navn;
        private string _primærTelefon;
        private string _sekundærTelefon;
        private DateTime _statusDato;
        private decimal _saldo;
        private Nyhedsaktualitet _nyhedsaktualitet = Nyhedsaktualitet.Medium;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner model for en adressekonto. 
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, som adressekontoen er tilknyttet.</param>
        /// <param name="nummer">Unik identifikation af adressekontoen.</param>
        /// <param name="navn">Navn for adressekontoen.</param>
        /// <param name="statusDato">Statusdato for opgørelsen.</param>
        /// <param name="saldo">Saldo pr. statusdato.</param>
        public AdressekontoModel(int regnskabsnummer, int nummer, string navn, DateTime statusDato, decimal saldo)
        {
            if (regnskabsnummer <= 0)
            {
                throw new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "regnskabsnummer", regnskabsnummer), "regnskabsnummer");
            }
            if (nummer <= 0)
            {
                throw new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "nummer", nummer), "nummer");
            }
            if (string.IsNullOrEmpty(navn))
            {
                throw new ArgumentNullException("navn");
            }
            _regnskabsnummer = regnskabsnummer;
            _nummer = nummer;
            _navn = navn;
            _statusDato = statusDato;
            _saldo = saldo;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Regnskabsnummer, som adressekontoen er tilknyttet.
        /// </summary>
        public virtual int Regnskabsnummer
        {
            get
            {
                return _regnskabsnummer;
            }
        }

        /// <summary>
        /// Unik identifikation af adressekontoen.
        /// </summary>
        public virtual int Nummer
        {
            get
            {
                return _nummer;
            }
        }

        /// <summary>
        /// Navn for adressekontoen.
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
                RaisePropertyChanged("Nyhedsinformation");
            }
        }

        /// <summary>
        /// Primær telefonnummer.
        /// </summary>
        public virtual string PrimærTelefon
        {
            get
            {
                return _primærTelefon;
            }
            set
            {
                if (_primærTelefon == value)
                {
                    return;
                }
                _primærTelefon = value;
                RaisePropertyChanged("PrimærTelefon");
            }
        }

        /// <summary>
        /// Sekundær telefonnummer.
        /// </summary>
        public virtual string SekundærTelefon
        {
            get
            {
                return _sekundærTelefon;
            }
            set
            {
                if (_sekundærTelefon == value)
                {
                    return;
                }
                _sekundærTelefon = value;
                RaisePropertyChanged("SekundærTelefon");
            }
        }

        /// <summary>
        /// Statusdato for opgørelsen.
        /// </summary>
        public virtual DateTime StatusDato
        {
            get
            {
                return _statusDato;
            }
            set
            {
                if (_statusDato == value)
                {
                    return;
                }
                _statusDato = value;
                RaisePropertyChanged("StatusDato");
                RaisePropertyChanged("Nyhedsudgivelsestidspunkt");
                RaisePropertyChanged("Nyhedsinformation");
            }
        }

        /// <summary>
        /// Saldo pr. statusdato.
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
                RaisePropertyChanged("Nyhedsinformation");
            }
        }

        /// <summary>
        /// Nyhedsaktualitet.
        /// </summary>
        public virtual Nyhedsaktualitet Nyhedsaktualitet
        {
            get
            {
                return _nyhedsaktualitet;
            }
            private set
            {
                if (_nyhedsaktualitet == value)
                {
                    return;
                }
                _nyhedsaktualitet = value;
                RaisePropertyChanged("Nyhedsaktualitet");
            }
        }

        /// <summary>
        /// Udgivelsestidspunkt for nyheden.
        /// </summary>
        public virtual DateTime Nyhedsudgivelsestidspunkt
        {
            get
            {
                return StatusDato;
            }
        }

        /// <summary>
        /// Detaljeret nyhedsinformation.
        /// </summary>
        public virtual string Nyhedsinformation
        {
            get
            {
                var nyhedsinformationBuilder = new StringBuilder();
                nyhedsinformationBuilder.AppendFormat("{0} {1}", StatusDato.ToString("d"), Navn);
                nyhedsinformationBuilder.AppendLine();
                nyhedsinformationBuilder.AppendFormat("{0} {1}", Resource.GetText(Text.Balance), Saldo.ToString("C"));
                return nyhedsinformationBuilder.ToString();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Opdaterer nyhedsaktualiteten for adressekontoen.
        /// </summary>
        /// <param name="nyhedsaktualitet">Nyhedsaktualitet, som adressekontoen skal opdateres med.</param>
        public void SetNyhedsaktualitet(Nyhedsaktualitet nyhedsaktualitet)
        {
            Nyhedsaktualitet = nyhedsaktualitet;
        }

        #endregion
    }
}
