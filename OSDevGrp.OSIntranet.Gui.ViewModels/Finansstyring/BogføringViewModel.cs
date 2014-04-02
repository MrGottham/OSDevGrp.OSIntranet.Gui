using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core.Validators;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring
{
    /// <summary>
    /// ViewModel, hvorfra der kan bogføres.
    /// </summary>
    public class BogføringViewModel : BogføringslinjeViewModel, IBogføringViewModel
    {
        #region Private variables

        private readonly IKontoViewModel _kontoViewModel = null;
        private readonly IFinansstyringRepository _finansstyringRepository;
        private readonly IExceptionHandlerViewModel _exceptionHandlerViewModel;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner en ViewModel, hvorfra der kan bogføres.
        /// </summary>
        /// <param name="regnskabViewModel">ViewModel for regnskabet, hvorpå der skal bogføres.</param>
        /// <param name="bogføringslinjeModel">Model til en ny bogføringslinje, der kan tilrettes og bogføres.</param>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="exceptionHandlerViewModel">Implementering af ViewModel til en exceptionhandler.</param>
        public BogføringViewModel(IRegnskabViewModel regnskabViewModel, IBogføringslinjeModel bogføringslinjeModel, IFinansstyringRepository finansstyringRepository, IExceptionHandlerViewModel exceptionHandlerViewModel)
            : base(regnskabViewModel, bogføringslinjeModel)
        {
            if (finansstyringRepository == null)
            {
                throw new ArgumentNullException("finansstyringRepository");
            }
            if (exceptionHandlerViewModel == null)
            {
                throw new ArgumentNullException("exceptionHandlerViewModel");
            }
            _finansstyringRepository = finansstyringRepository;
            _exceptionHandlerViewModel = exceptionHandlerViewModel;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Tekstangivelse af bogføringstidspunkt.
        /// </summary>
        [CustomValidation(typeof (BogføringViewModel), "ValidateDatoAsText")]
        public virtual string DatoAsText
        {
            get
            {
                return Dato.ToString("d");
            }
            set
            {
                try
                {
                    var validationResult = ValidateDatoAsText(value);
                    if (validationResult != ValidationResult.Success)
                    {
                        throw new IntranetGuiValidationException(validationResult.ErrorMessage, this, "DatoAsText", value);
                    }
                    try
                    {
                        Model.Dato = DateTime.Parse(value, CultureInfo.CurrentUICulture, DateTimeStyles.AssumeLocal);
                    }
                    catch (ArgumentNullException ex)
                    {
                        throw new IntranetGuiValidationException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingBookeepingDate), this, "DatoAsText", value, ex);
                    }
                    catch (ArgumentException ex)
                    {
                        throw new IntranetGuiValidationException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingBookeepingDate), this, "DatoAsText", value, ex);
                    }
                }
                catch (IntranetGuiExceptionBase ex)
                {
                    _exceptionHandlerViewModel.HandleException(ex);
                }
                catch (Exception ex)
                {
                    _exceptionHandlerViewModel.HandleException(new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingPropertyValue, "DatoAsText", ex.Message), ex));
                }
            }
        }

        /// <summary>
        /// Label til bogføringstidspunkt.
        /// </summary>
        public virtual string DatoLabel
        {
            get
            {
                return Resource.GetText(Text.Date);
            }
        }

        /// <summary>
        /// Bilagsnummer.
        /// </summary>
        [CustomValidation(typeof (BogføringViewModel), "ValidateBilag")]
        public new virtual string Bilag
        {
            get
            {
                return base.Bilag;
            }
            set
            {
                try
                {
                    var validationResult = ValidateBilag(value);
                    if (validationResult != ValidationResult.Success)
                    {
                        throw new IntranetGuiValidationException(validationResult.ErrorMessage, this, "Bilag", value);
                    }
                    try
                    {
                        Model.Bilag = value;
                    }
                    catch (ArgumentNullException ex)
                    {
                        throw new IntranetGuiValidationException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingAnnex), this, "Bilag", value, ex);
                    }
                    catch (ArgumentException ex)
                    {
                        throw new IntranetGuiValidationException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingAnnex), this, "Bilag", value, ex);
                    }
                }
                catch (IntranetGuiExceptionBase ex)
                {
                    _exceptionHandlerViewModel.HandleException(ex);
                }
                catch (Exception ex)
                {
                    _exceptionHandlerViewModel.HandleException(new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingPropertyValue, "Bilag", ex.Message), ex));
                }
            }
        }

        /// <summary>
        /// Label til bilagsnummer.
        /// </summary>
        public virtual string BilagLabel
        {
            get
            {
                return Resource.GetText(Text.Annex);
            }
        }

        /// <summary>
        /// Kontonummer, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        [CustomValidation(typeof (BogføringViewModel), "ValidateKontonummer")]
        public new virtual string Kontonummer
        {
            get
            {
                return base.Kontonummer;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Label til kontonummeret, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual string KontonummerLabel
        {
            get
            {
                return Resource.GetText(Text.Account);
            }
        }

        /// <summary>
        /// Navn på kontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual string Kontonavn
        {
            get
            {
                return KontoViewModel == null ? string.Empty : KontoViewModel.Kontonavn;
            }
        }

        /// <summary>
        /// Label til navnet på kontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual string KontonavnLabel
        {
            get
            {
                return Resource.GetText(Text.Account);
            }
        }

        /// <summary>
        /// Saldo på kontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual decimal KontoSaldo
        {
            get
            {
                return KontoViewModel == null ? 0M : KontoViewModel.Saldo;
            }
        }

        /// <summary>
        /// Tekstangivelse af saldo på kontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual string KontoSaldoAsText
        {
            get
            {
                return KontoViewModel == null ? string.Empty : KontoViewModel.SaldoAsText;
            }
        }

        /// <summary>
        /// Label til saldoen på kontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual string KontoSaldoLabel
        {
            get
            {
                return Resource.GetText(Text.Balance);
            }
        }

        /// <summary>
        /// Disponibel beløb på kontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual decimal KontoDisponibel
        {
            get
            {
                return KontoViewModel == null ? 0M : KontoViewModel.Disponibel;
            }
        }

        /// <summary>
        /// Tekstangivelse af disponibel beløb på kontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual string KontoDisponibelAsText
        {
            get
            {
                return KontoViewModel == null ? string.Empty : KontoViewModel.DisponibelAsText;
            }
        }

        /// <summary>
        /// Label til disponibel beløb på kontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual string KontoDisponibelLabel
        {
            get
            {
                return Resource.GetText(Text.Available);
            }
        }

        /// <summary>
        /// ViewModel for kontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        protected virtual IKontoViewModel KontoViewModel
        {
            get
            {
                return _kontoViewModel;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Eventhandler, der kaldes, når en property ændres på modellen for bogføringslinjen.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        protected override void PropertyChangedOnBogføringslinjeModelEventHandler(object sender, PropertyChangedEventArgs eventArgs)
        {
            base.PropertyChangedOnBogføringslinjeModelEventHandler(sender, eventArgs);
            switch (eventArgs.PropertyName)
            {
                case "Dato":
                    RaisePropertyChanged("DatoAsText");
                    break;
            }
        }

        /// <summary>
        /// Validerer værdien for bogføringstidspunkt.
        /// </summary>
        /// <param name="value">Værdi, der skal valideres.</param>
        /// <returns>Valideringsresultat.</returns>
        public static ValidationResult ValidateDatoAsText(string value)
        {
            var result = Validation.ValidateRequiredValue(value);
            if (result != ValidationResult.Success)
            {
                return result;
            }
            result = Validation.ValidateDate(value);
            if (result != ValidationResult.Success)
            {
                return result;
            }
            return Validation.ValidateDateLowerOrEqualTo(value, DateTime.Now);
        }

        /// <summary>
        /// Validerer værdien for bilagsnummer.
        /// </summary>
        /// <param name="value">Værdi, der skal valideres.</param>
        /// <returns>Valideringsresultat.</returns>
        public static ValidationResult ValidateBilag(string value)
        {
            return ValidationResult.Success;
        }

        /// <summary>
        /// Validerer værdien for kontonummer.
        /// </summary>
        /// <param name="value">Værdi, der skal valideres.</param>
        /// <returns>Valideringsresultat.</returns>
        public static ValidationResult ValidateKontonummer(string value)
        {
            return Validation.ValidateRequiredValue(value);
        }

        #endregion
    }
}
