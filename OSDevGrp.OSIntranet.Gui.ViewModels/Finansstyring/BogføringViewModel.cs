using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Threading.Tasks;
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

        private IKontoViewModel _kontoViewModel;
        private IBudgetkontoViewModel _budgetkontoViewModel;
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
                        throw new IntranetGuiValidationException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingBookkeepingDate), this, "DatoAsText", value, ex);
                    }
                    catch (ArgumentException ex)
                    {
                        throw new IntranetGuiValidationException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingBookkeepingDate), this, "DatoAsText", value, ex);
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
        /// Angivelse af, om tekstangivelsen for bogføringstidspunktet kan redigeres.
        /// </summary>
        public virtual bool DatoAsTextIsReadOnly
        {
            get
            {
                return KontoReaderTaskIsActive || BudgetkontoReaderTaskIsActive || ErBogført;
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
                        Model.Bilag = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
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
        /// Angivelse af den maksimale tekstlængde for bilagsnummeret.
        /// </summary>
        public virtual int BilagMaxLength
        {
            get
            {
                return FieldInformations.BilagFieldLength;
            }
        }

        /// <summary>
        /// Angivelse af, om bilagsnummeret kan redigeres.
        /// </summary>
        public virtual bool BilagIsReadOnly
        {
            get
            {
                return ErBogført;
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
                try
                {
                    var validationResult = ValidateKontonummer(value);
                    if (validationResult != ValidationResult.Success)
                    {
                        throw new IntranetGuiValidationException(validationResult.ErrorMessage, this, "Kontonummer", value);
                    }
                    try
                    {
                        Model.Kontonummer = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
                    }
                    catch (ArgumentNullException ex)
                    {
                        throw new IntranetGuiValidationException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingAccountNumber), this, "Kontonummer", value, ex);
                    }
                    catch (ArgumentException ex)
                    {
                        throw new IntranetGuiValidationException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingAccountNumber), this, "Kontonummer", value, ex);
                    }
                }
                catch (IntranetGuiExceptionBase ex)
                {
                    _exceptionHandlerViewModel.HandleException(ex);
                }
                catch (Exception ex)
                {
                    _exceptionHandlerViewModel.HandleException(new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingPropertyValue, "Kontonummer", ex.Message), ex));
                }
            }
        }

        /// <summary>
        /// Angivelse af den maksimale tekstlængde for kontonummeret.
        /// </summary>
        public virtual int KontonummerMaxLength
        {
            get
            {
                return FieldInformations.KontonummerFieldLength;
            }
        }

        /// <summary>
        /// Angivelse af, om kontonummeret kan redigeres.
        /// </summary>
        public virtual bool KontonummerIsReadOnly
        {
            get
            {
                return KontoReaderTaskIsActive || ErBogført;
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
        /// Tekst.
        /// </summary>
        [CustomValidation(typeof (BogføringViewModel), "ValidateTekst")]
        public new virtual string Tekst
        {
            get
            {
                return base.Tekst;
            }
            set
            {
                try
                {
                    var validationResult = ValidateTekst(value);
                    if (validationResult != ValidationResult.Success)
                    {
                        throw new IntranetGuiValidationException(validationResult.ErrorMessage, this, "Tekst", value);
                    }
                    try
                    {
                        Model.Tekst = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
                    }
                    catch (ArgumentNullException ex)
                    {
                        throw new IntranetGuiValidationException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingBookkeepingText), this, "Tekst", value, ex);
                    }
                    catch (ArgumentException ex)
                    {
                        throw new IntranetGuiValidationException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingBookkeepingText), this, "Tekst", value, ex);
                    }
                }
                catch (IntranetGuiExceptionBase ex)
                {
                    _exceptionHandlerViewModel.HandleException(ex);
                }
                catch (Exception ex)
                {
                    _exceptionHandlerViewModel.HandleException(new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingPropertyValue, "Tekst", ex.Message), ex));
                }
            }
        }

        /// <summary>
        /// Angivelse af den maksimale tekstlængde for teksten til bogføringslinjen.
        /// </summary>
        public virtual int TekstMaxLength
        {
            get
            {
                return FieldInformations.BogføringstekstFieldLength;
            }
        }

        /// <summary>
        /// Angivelse af, om teksten til bogføringslinjen kan redigeres.
        /// </summary>
        public virtual bool TekstIsReadOnly
        {
            get
            {
                return ErBogført;
            }
        }

        /// <summary>
        /// Label til teksten på bogføringslinjen.
        /// </summary>
        public virtual string TekstLabel
        {
            get
            {
                return Resource.GetText(Text.Text);
            }
        }

        /// <summary>
        /// Kontonummer på budgetkontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        [CustomValidation(typeof (BogføringViewModel), "ValidateBudgetkontonummer")]
        public new virtual string Budgetkontonummer
        {
            get
            {
                return base.Budgetkontonummer;
            }
            set
            {
                try
                {
                    var validationResult = ValidateBudgetkontonummer(value);
                    if (validationResult != ValidationResult.Success)
                    {
                        throw new IntranetGuiValidationException(validationResult.ErrorMessage, this, "Budgetkontonummer", value);
                    }
                    try
                    {
                        Model.Budgetkontonummer = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
                    }
                    catch (ArgumentNullException ex)
                    {
                        throw new IntranetGuiValidationException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingBudgetAccountNumber), this, "Budgetkontonummer", value, ex);
                    }
                    catch (ArgumentException ex)
                    {
                        throw new IntranetGuiValidationException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingBudgetAccountNumber), this, "Budgetkontonummer", value, ex);
                    }
                }
                catch (IntranetGuiExceptionBase ex)
                {
                    _exceptionHandlerViewModel.HandleException(ex);
                }
                catch (Exception ex)
                {
                    _exceptionHandlerViewModel.HandleException(new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingPropertyValue, "Budgetkontonummer", ex.Message), ex));
                }
            }
        }

        /// <summary>
        /// Angivelse af den maksimale tekstlængde for kontonummeret på budgetkontoen.
        /// </summary>
        public virtual int BudgetkontonummerMaxLength
        {
            get
            {
                return FieldInformations.KontonummerFieldLength;
            }
        }

        /// <summary>
        /// Angivelse af, om kontonummeret på budgetkontoen kan redigeres.
        /// </summary>
        public virtual bool BudgetkontonummerIsReadOnly
        {
            get
            {
                return BudgetkontoReaderTaskIsActive || ErBogført;
            }
        }

        /// <summary>
        /// Label til kontonummeret på budgetkontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual string BudgetkontonummerLabel
        {
            get
            {
                return Resource.GetText(Text.BudgetAccount);
            }
        }

        /// <summary>
        /// Navn på budgetkontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual string Budgetkontonavn
        {
            get
            {
                return BudgetkontoViewModel == null ? string.Empty : BudgetkontoViewModel.Kontonavn;
            }
        }

        /// <summary>
        /// Label til navnet på budgetkontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual string BudgetkontonavnLabel
        {
            get
            {
                return Resource.GetText(Text.BudgetAccount);
            }
        }

        /// <summary>
        /// Bogført beløb på budgetkontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual decimal BudgetkontoBogført
        {
            get
            {
                return BudgetkontoViewModel == null ? 0M : BudgetkontoViewModel.Bogført;
            }
        }

        /// <summary>
        /// Tekstangivelse af bogført beløb på budgetkontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual string BudgetkontoBogførtAsText
        {
            get
            {
                return BudgetkontoViewModel == null ? string.Empty : BudgetkontoViewModel.BogførtAsText;
            }
        }

        /// <summary>
        /// Label til bogført beløb på budgetkontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual string BudgetkontoBogførtLabel
        {
            get
            {
                return Resource.GetText(Text.Bookkeeped);
            }
        }

        /// <summary>
        /// Disponibel beløb på budgetkontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual decimal BudgetkontoDisponibel
        {
            get
            {
                return BudgetkontoViewModel == null ? 0M : BudgetkontoViewModel.Disponibel;
            }
        }

        /// <summary>
        /// Tekstangivelse af disponibel beløb på budgetkontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual string BudgetkontoDisponibelAsText
        {
            get
            {
                return BudgetkontoViewModel == null ? string.Empty : BudgetkontoViewModel.DisponibelAsText;
            }
        }

        /// <summary>
        /// Label til disponibel beløb på budgetkontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual string BudgetkontoDisponibelLabel
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
            private set
            {
                _kontoViewModel = value;
                RaisePropertyChanged("Kontonavn");
                RaisePropertyChanged("KontoSaldo");
                RaisePropertyChanged("KontoSaldoAsText");
                RaisePropertyChanged("KontoDisponibel");
                RaisePropertyChanged("KontoDisponibelAsText");
            }
        }

        /// <summary>
        /// Task, der indlæser og opdaterer kontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        protected virtual Task<IKontoViewModel> KontoReaderTask
        {
            get
            {
                return null;
            }
            // TODO: RaisePropertyChanged("DatoAsTextIsReadOnly");
            // TODO: RaisePropertyChanged("KontonummerIsReadOnly");
        }

        /// <summary>
        /// Angivelse af, om den Task, der indlæser og opdaterer kontoen, hvortil bogføringslinjen er tilknyttet, er igangværende.
        /// </summary>
        protected virtual bool KontoReaderTaskIsActive
        {
            get
            {
                return KontoReaderTask != null && KontoReaderTask.IsCompleted == false && KontoReaderTask.IsCanceled == false && KontoReaderTask.IsFaulted == false;
            }
        }

        /// <summary>
        /// ViewModel for budgetkontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        protected virtual IBudgetkontoViewModel BudgetkontoViewModel
        {
            get
            {
                return _budgetkontoViewModel;
            }
            private set
            {
                _budgetkontoViewModel = value;
                RaisePropertyChanged("Budgetkontonavn");
                RaisePropertyChanged("BudgetkontoBogført");
                RaisePropertyChanged("BudgetkontoBogførtAsText");
                RaisePropertyChanged("BudgetkontoDisponibel");
                RaisePropertyChanged("BudgetkontoDisponibelAsText");
            }
        }

        /// <summary>
        /// Task, der indlæser og opdaterer budgetkontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        protected virtual Task<IBudgetkontoViewModel> BudgetkontoReaderTask
        {
            get
            {
                return null;
            }
            // TODO: RaisePropertyChanged("DatoAsTextIsReadOnly");
            // TODO: RaisePropertyChanged("BudgetkontonummerIsReadOnly");
        }

        /// <summary>
        /// Angivelse af, om den Task, der indlæser og opdaterer budgetkontoen, hvortil bogføringslinjen er tilknyttet, er igangværende.
        /// </summary>
        protected virtual bool BudgetkontoReaderTaskIsActive
        {
            get
            {
                return BudgetkontoReaderTask != null && BudgetkontoReaderTask.IsCompleted == false && BudgetkontoReaderTask.IsCanceled == false && BudgetkontoReaderTask.IsFaulted == false;
            }
        }

        /// <summary>
        /// Task, der udfører den asynkrone bogføring.
        /// </summary>
        protected virtual Task BogføringTask
        {
            get
            {
                return null;
            }
            // TODO: RaisePropertyChanged("DatoAsTextIsReadOnly");
            // TODO: RaisePropertyChanged("BilagIsReadOnly");
            // TODO: RaisePropertyChanged("KontonummerIsReadOnly");
            // TODO: RaisePropertyChanged("TekstIsReadOnly");
            // TODO: RaisePropertyChanged("BudgetkontonummerIsReadOnly");
        }

        /// <summary>
        /// Angivelse af, om bogføringslinjen er ved eller er blevet bogført.
        /// </summary>
        protected virtual bool ErBogført
        {
            get
            {
                return BogføringTask != null;
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
                    KontoViewModelRefresh();
                    BudgetkontoViewModelRefresh();
                    break;

                case "Kontonummer":
                    KontoViewModelRefresh();
                    break;

                case "Budgetkontonummer":
                    BudgetkontoViewModelRefresh();
                    break;
            }
        }

        /// <summary>
        /// Genindlæser ViewModel for kontoen, som bogføringslinjen er tilknyttet.
        /// </summary>
        private void KontoViewModelRefresh()
        {
            KontoViewModel = null;
            if (string.IsNullOrEmpty(Model.Kontonummer))
            {
                return;
            }
            // TODO: Reload konto.
        }

        /// <summary>
        /// Genindlæser ViewModel for budgetkontoen, som bogføringslinjen er tilknyttet.
        /// </summary>
        private void BudgetkontoViewModelRefresh()
        {
            BudgetkontoViewModel = null;
            if (string.IsNullOrEmpty(Model.Budgetkontonummer))
            {
                return;
            }
            // TODO: Reload budgetkonto.
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

        /// <summary>
        /// Validerer værdien for teksten til bogføringslinjen.
        /// </summary>
        /// <param name="value">Værdi, der skal valideres.</param>
        /// <returns>Valideringsresultat.</returns>
        public static ValidationResult ValidateTekst(string value)
        {
            return Validation.ValidateRequiredValue(value);
        }

        /// <summary>
        /// Validerer værdien for kontonummer til budgetkontoen.
        /// </summary>
        /// <param name="value">Værdi, der skal valideres.</param>
        /// <returns>Valideringsresultat.</returns>
        public static ValidationResult ValidateBudgetkontonummer(string value)
        {
            return ValidationResult.Success;
        }

        #endregion
    }
}
