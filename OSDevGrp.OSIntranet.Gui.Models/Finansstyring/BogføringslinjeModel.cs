using System;
using System.Text;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Resources;

namespace OSDevGrp.OSIntranet.Gui.Models.Finansstyring
{
    /// <summary>
    /// Model for en bogføringslinje.
    /// </summary>
    public class BogføringslinjeModel : ModelBase, IBogføringslinjeModel
    {
        #region Private variables

        private readonly int _regnskabsnummer;
        private readonly int _løbenummer;
        private DateTime _dato;
        private string _bilag;
        private string _kontonummer;
        private string _tekst;
        private string _budgetkontonummer;
        private decimal _debit;
        private decimal _kredit;
        private int _adressekonto;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner model for en bogføringslinje.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, som bogføringslinjen er tilknyttet.</param>
        /// <param name="løbenummer">Unik identifikation af bogføringslinjen inden for regnskabet.</param>
        /// <param name="dato">Bogføringstidspunkt.</param>
        /// <param name="kontonummer">Kontonummer, hvortil bogføringslinjen er tilknyttet.</param>
        /// <param name="tekst">Tekst.</param>
        /// <param name="debit">Debitbeløb.</param>
        /// <param name="kredit">Kreditbeløb.</param>
        public BogføringslinjeModel(int regnskabsnummer, int løbenummer, DateTime dato, string kontonummer, string tekst, decimal debit, decimal kredit)
        {
            if (regnskabsnummer <= 0)
            {
                throw new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "regnskabsnummer", regnskabsnummer), "regnskabsnummer");
            }
            if (løbenummer <= 0)
            {
                throw new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "løbenummer", løbenummer), "løbenummer");
            }
            if (dato > DateTime.Now)
            {
                throw new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "dato", dato), "dato");
            }
            if (string.IsNullOrEmpty(kontonummer))
            {
                throw new ArgumentNullException("kontonummer");
            }
            if (string.IsNullOrEmpty(tekst))
            {
                throw new ArgumentNullException("tekst");
            }
            if (debit < 0M)
            {
                throw new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "debit", debit), "debit");
            }
            if (kredit < 0M)
            {
                throw new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "kredit", kredit), "kredit");
            }
            _regnskabsnummer = regnskabsnummer;
            _løbenummer = løbenummer;
            _dato = dato;
            _kontonummer = kontonummer;
            _tekst = tekst;
            _debit = debit;
            _kredit = kredit;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Regnskabsnummer, som bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual int Regnskabsnummer
        {
            get
            {
                return _regnskabsnummer;
            }
        }

        /// <summary>
        /// Unik identifikation af bogføringslinjen inden for regnskabet.
        /// </summary>
        public virtual int Løbenummer
        {
            get
            {
                return _løbenummer;
            }
        }

        /// <summary>
        /// Bogføringstidspunkt.
        /// </summary>
        public virtual DateTime Dato
        {
            get
            {
                return _dato;
            }
            set
            {
                if (value > DateTime.Now)
                {
                    throw new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "value", value), "value");
                }
                if (_dato == value)
                {
                    return;
                }
                _dato = value;
                RaisePropertyChanged("Dato");
                RaisePropertyChanged("Nyhedsudgivelsestidspunkt");
                RaisePropertyChanged("Nyhedsinformation");
            }
        }

        /// <summary>
        /// Bilag.
        /// </summary>
        public virtual string Bilag
        {
            get
            {
                return _bilag;
            }
            set
            {
                if (_bilag == value)
                {
                    return;
                }
                _bilag = value;
                RaisePropertyChanged("Bilag");
            }
        }

        /// <summary>
        /// Kontonummer, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual string Kontonummer
        {
            get
            {
                return _kontonummer;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("value");
                }
                if (_kontonummer == value)
                {
                    return;
                }
                _kontonummer = value;
                RaisePropertyChanged("Kontonummer");
                RaisePropertyChanged("Nyhedsinformation");
            }
        }

        /// <summary>
        /// Tekst.
        /// </summary>
        public virtual string Tekst
        {
            get
            {
                return _tekst;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("value");
                }
                if (_tekst == value)
                {
                    return;
                }
                _tekst = value;
                RaisePropertyChanged("Tekst");
                RaisePropertyChanged("Nyhedsinformation");
            }
        }

        /// <summary>
        /// Budgetkontonummer, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual string Budgetkontonummer
        {
            get
            {
                return _budgetkontonummer;
            }
            set
            {
                if (_budgetkontonummer == value)
                {
                    return;
                }
                _budgetkontonummer = value;
                RaisePropertyChanged("Budgetkontonummer");
            }
        }

        /// <summary>
        /// Debitbeløb.
        /// </summary>
        public virtual decimal Debit
        {
            get
            {
                return _debit;
            }
            set
            {
                if (value < 0M)
                {
                    throw new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "value", value), "value");
                }
                if (_debit == value)
                {
                    return;
                }
                _debit = value;
                RaisePropertyChanged("Debit");
                RaisePropertyChanged("Bogført");
                RaisePropertyChanged("Nyhedsinformation");
            }
        }

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
                if (value < 0M)
                {
                    throw new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "value", value), "value");
                }
                if (_kredit == value)
                {
                    return;
                }
                _kredit = value;
                RaisePropertyChanged("Kredit");
                RaisePropertyChanged("Bogført");
                RaisePropertyChanged("Nyhedsinformation");
            }
        }

        /// <summary>
        /// Bogføringsbeløb.
        /// </summary>
        public virtual decimal Bogført
        {
            get
            {
                return Debit - Kredit;
            }
        }

        /// <summary>
        /// Adressekonto, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual int Adressekonto
        {
            get
            {
                return _adressekonto;
            }
            set
            {
                if (_adressekonto == value)
                {
                    return;
                }
                _adressekonto = value;
                RaisePropertyChanged("Adressekonto");
            }
        }

        /// <summary>
        /// Nyhedsaktualitet.
        /// </summary>
        public virtual Nyhedsaktualitet Nyhedsaktualitet
        {
            get
            {
                return Nyhedsaktualitet.Medium;
            }
        }

        /// <summary>
        /// Udgivelsestidspunkt for nyheden.
        /// </summary>
        public virtual DateTime Nyhedsudgivelsestidspunkt
        {
            get
            {
                return Dato;
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
                nyhedsinformationBuilder.AppendFormat("{0} {1}", Dato.ToString("d"), Kontonummer);
                nyhedsinformationBuilder.AppendLine();
                nyhedsinformationBuilder.AppendFormat("{0} {1}", Tekst, Bogført.ToString("C"));
                return nyhedsinformationBuilder.ToString();
            }
        }

        #endregion
    }
}
