using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core.Commands;
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

        private ICommand _bogførCommand;
        private IKontoViewModel _kontoViewModel;
        private Task<IKontoViewModel> _kontoReaderTask;
        private IBudgetkontoViewModel _budgetkontoViewModel;
        private Task<IBudgetkontoViewModel> _budgetkontoReaderTask;
        private IAdressekontoViewModel _adressekontoViewModel;
        private Task<IEnumerable<IAdressekontoViewModel>> _adressekontoReaderTask;
        private Task _bogføringTask;
        private readonly IFinansstyringRepository _finansstyringRepository;
        private readonly IExceptionHandlerViewModel _exceptionHandlerViewModel;
        private readonly ObservableCollection<IAdressekontoViewModel> _adressekontoViewModelCollection = new ObservableCollection<IAdressekontoViewModel>();

        #endregion

        #region Constructors

        /// <summary>
        /// Danner en ViewModel, hvorfra der kan bogføres.
        /// </summary>
        /// <param name="regnskabViewModel">ViewModel for regnskabet, hvorpå der skal bogføres.</param>
        /// <param name="bogføringslinjeModel">Model til en ny bogføringslinje, der kan tilrettes og bogføres.</param>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="exceptionHandlerViewModel">Implementering af ViewModel til en exceptionhandler.</param>
        public BogføringViewModel(IRegnskabViewModel regnskabViewModel, IBogføringslinjeModel bogføringslinjeModel, IFinansstyringRepository finansstyringRepository, IExceptionHandlerViewModel exceptionHandlerViewModel)
            : this(regnskabViewModel, bogføringslinjeModel, finansstyringRepository, exceptionHandlerViewModel, false)
        {
        }

        /// <summary>
        /// Danner en ViewModel, hvorfra der kan bogføres.
        /// </summary>
        /// <param name="regnskabViewModel">ViewModel for regnskabet, hvorpå der skal bogføres.</param>
        /// <param name="bogføringslinjeModel">Model til en ny bogføringslinje, der kan tilrettes og bogføres.</param>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="exceptionHandlerViewModel">Implementering af ViewModel til en exceptionhandler.</param>
        /// <param name="runRefreshTasks">Angivelse af, om de Tasks, der udfører refresh, skal køres ved initiering af ViewModel, hvorfra der kan bogføres.</param>
        public BogføringViewModel(IRegnskabViewModel regnskabViewModel, IBogføringslinjeModel bogføringslinjeModel, IFinansstyringRepository finansstyringRepository, IExceptionHandlerViewModel exceptionHandlerViewModel, bool runRefreshTasks)
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
            _adressekontoViewModelCollection.CollectionChanged += CollectionChangedOnAdressekontoViewModelCollectionEventHandler;
            if (!runRefreshTasks)
            {
                return;
            }
            KontoViewModelRefresh();
            BudgetkontoViewModelRefresh();
            AdressekontoViewModelCollectionRefresh();
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
                return KontoReaderTaskIsActive || BudgetkontoReaderTaskIsActive || AdressekontoReaderTaskIsActive || ErBogført;
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
        /// Tekstangivelse af debitbeløb.
        /// </summary>
        [CustomValidation(typeof (BogføringViewModel), "ValidateCurrency")]
        public new virtual string DebitAsText
        {
            get
            {
                return base.DebitAsText;
            }
            set
            {
                try
                {
                    var result = ValidateCurrency(value);
                    if (result != ValidationResult.Success)
                    {
                        throw new IntranetGuiValidationException(result.ErrorMessage, this, "DebitAsText", value);
                    }
                    try
                    {
                        if (string.IsNullOrWhiteSpace(value))
                        {
                            Model.Debit = 0M;
                            return;
                        }
                        Model.Debit = decimal.Parse(value.Trim(), NumberStyles.Any, CultureInfo.CurrentUICulture);
                    }
                    catch (ArgumentNullException ex)
                    {
                        throw new IntranetGuiValidationException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingDebit), this, "DebitAsText", value, ex);
                    }
                    catch (ArgumentException ex)
                    {
                        throw new IntranetGuiValidationException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingDebit), this, "DebitAsText", value, ex);
                    }
                }
                catch (IntranetGuiExceptionBase ex)
                {
                    _exceptionHandlerViewModel.HandleException(ex);
                }
                catch (Exception ex)
                {
                    _exceptionHandlerViewModel.HandleException(new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingPropertyValue, "DebitAsText", ex.Message), ex));
                }
            }
        }

        /// <summary>
        /// Angivelse af den maksimale tekstlængde for debitbeløbet.
        /// </summary>
        public virtual int DebitMaxLength
        {
            get
            {
                return FieldInformations.DebitFieldLength;
            }
        }

        /// <summary>
        /// Angivelse af, om debitbeløbet kan redigeres.
        /// </summary>
        public virtual bool DebitIsReadOnly
        {
            get
            {
                return ErBogført;
            }
        }

        /// <summary>
        /// Label til debitbeløbet.
        /// </summary>
        public virtual string DebitLabel
        {
            get
            {
                return Resource.GetText(Text.Debit);
            }
        }

        /// <summary>
        /// Tekstangivelse af kreditbeløb.
        /// </summary>
        [CustomValidation(typeof (BogføringViewModel), "ValidateCurrency")]
        public new virtual string KreditAsText
        {
            get
            {
                return base.KreditAsText;
            }
            set
            {
                try
                {
                    var result = ValidateCurrency(value);
                    if (result != ValidationResult.Success)
                    {
                        throw new IntranetGuiValidationException(result.ErrorMessage, this, "KreditAsText", value);
                    }
                    try
                    {
                        if (string.IsNullOrWhiteSpace(value))
                        {
                            Model.Kredit = 0M;
                            return;
                        }
                        Model.Kredit = decimal.Parse(value.Trim(), NumberStyles.Any, CultureInfo.CurrentUICulture);
                    }
                    catch (ArgumentNullException ex)
                    {
                        throw new IntranetGuiValidationException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingCredit), this, "KreditAsText", value, ex);
                    }
                    catch (ArgumentException ex)
                    {
                        throw new IntranetGuiValidationException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingCredit), this, "KreditAsText", value, ex);
                    }
                }
                catch (IntranetGuiExceptionBase ex)
                {
                    _exceptionHandlerViewModel.HandleException(ex);
                }
                catch (Exception ex)
                {
                    _exceptionHandlerViewModel.HandleException(new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingPropertyValue, "KreditAsText", ex.Message), ex));
                }
            }
        }

        /// <summary>
        /// Angivelse af den maksimale tekstlængde for kreditbeløbet.
        /// </summary>
        public virtual int KreditMaxLength
        {
            get
            {
                return FieldInformations.KreditFieldLength;
            }
        }

        /// <summary>
        /// Angivelse af, om kreditbeløbet kan redigeres.
        /// </summary>
        public virtual bool KreditIsReadOnly
        {
            get
            {
                return ErBogført;
            }
        }

        /// <summary>
        /// Label til kreditbeløbet.
        /// </summary>
        public virtual string KreditLabel
        {
            get
            {
                return Resource.GetText(Text.Credit);
            }
        }

        /// <summary>
        /// Unik identifikation af adressekonto, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        [CustomValidation(typeof (BogføringViewModel), "ValidateAdressekonto")]
        public new virtual int Adressekonto
        {
            get
            {
                return base.Adressekonto;
            }
            set
            {
                try
                {
                    var result = ValidateAdressekonto(value);
                    if (result != ValidationResult.Success)
                    {
                        throw new IntranetGuiValidationException(result.ErrorMessage, this, "Adressekonto", value);
                    }
                    try
                    {
                        Model.Adressekonto = value;
                    }
                    catch (ArgumentNullException ex)
                    {
                        throw new IntranetGuiValidationException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingAddressAccount), this, "Adressekonto", value, ex);
                    }
                    catch (ArgumentException ex)
                    {
                        throw new IntranetGuiValidationException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingAddressAccount), this, "Adressekonto", value, ex);
                    }
                }
                catch (IntranetGuiExceptionBase ex)
                {
                    _exceptionHandlerViewModel.HandleException(ex);
                }
                catch (Exception ex)
                {
                    _exceptionHandlerViewModel.HandleException(new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingPropertyValue, "Adressekonto", ex.Message), ex));
                }
            }
        }

        /// <summary>
        /// Angivelse af, om den unikke identifikation af adressekonto, hvortil bogføringslinjen er tilknyttet, kan redigeres.
        /// </summary>
        public virtual bool AdressekontoIsReadOnly
        {
            get
            {
                return AdressekontoReaderTaskIsActive || ErBogført;
            }
        }

        /// <summary>
        /// Label til den unikke identifikation af adressekonto, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual string AdressekontoLabel
        {
            get
            {
                return Resource.GetText(Text.AddressAccount);
            }
        }

        /// <summary>
        /// Navn på adressekontoen.
        /// </summary>
        public virtual string AdressekontoNavn
        {
            get
            {
                return AdressekontoViewModel == null ? string.Empty : AdressekontoViewModel.Navn;
            }
        }

        /// <summary>
        /// Label til navnet på adressekontoen.
        /// </summary>
        public virtual string AdressekontoNavnLabel
        {
            get
            {
                return Resource.GetText(Text.Name);
            }
        }

        /// <summary>
        /// Saldo på adressekontoen.
        /// </summary>
        public virtual decimal AdressekontoSaldo
        {
            get
            {
                return AdressekontoViewModel == null ? 0M : AdressekontoViewModel.Saldo;
            }
        }

        /// <summary>
        /// Tekstangivelse af saldo på adressekontoen.
        /// </summary>
        public virtual string AdressekontoSaldoAsText
        {
            get
            {
                return AdressekontoViewModel == null ? string.Empty : AdressekontoViewModel.SaldoAsText;
            }
        }

        /// <summary>
        /// Label til saldoen på adressekontoen.
        /// </summary>
        public virtual string AdressekontoSaldoLabel
        {
            get
            {
                return Resource.GetText(Text.Balance);
            }
        }

        /// <summary>
        /// Adressekonti, der kan tilknyttes bogføringslinjen.
        /// </summary>
        public virtual IEnumerable<IAdressekontoViewModel> Adressekonti
        {
            get
            {
                return _adressekontoViewModelCollection.OrderBy(m => m.Navn);
            }
        }

        /// <summary>
        /// Label til adressekonti, der kan tilknyttes bogføringslinjen.
        /// </summary>
        public virtual string AdressekontiLabel
        {
            get
            {
                return Resource.GetText(Text.AddressAccounts);
            }
        }

        /// <summary>
        /// Tasks, der udføres asynkront.
        /// </summary>
        public virtual IEnumerable<Task> Tasks
        {
            get
            {
                var tasks = new ObservableCollection<Task>();
                if (KontoReaderTask != null)
                {
                    tasks.Add(KontoReaderTask);
                }
                if (BudgetkontoReaderTask != null)
                {
                    tasks.Add(BudgetkontoReaderTask);
                }
                if (AdressekontoReaderTask != null)
                {
                    tasks.Add(AdressekontoReaderTask);
                }
                if (BogføringTask != null)
                {
                    tasks.Add(BogføringTask);
                }
                return tasks;
            }
        }

        /// <summary>
        /// Angivelse af, om asynkront arbejde er igangværende for bogføringslinjen.
        /// </summary>
        public virtual bool IsWorking
        {
            get
            {
                return KontoReaderTaskIsActive || BudgetkontoReaderTaskIsActive || AdressekontoReaderTaskIsActive || BogføringTaskIsActive;
            }
        }

        /// <summary>
        /// Kommando, der kan foretage bogføring af bogføringslinjen.
        /// </summary>
        public virtual ICommand BogførCommand
        {
            get
            {
                if (_bogførCommand != null)
                {
                    return _bogførCommand;
                }

                // TODO: BogførCommand skal bruge Validate metoder til validering.
                // TODO: BogførCommand skal kunne kalde tilbage, efter end bogføring.
                ITaskableCommand cmd = null;
                var executeCommands = new Collection<ICommand>
                    {
                        //cmd,
                        new RelayCommand(obj => BogføringTask = cmd.ExecuteTask)
                    };
                _bogførCommand = new CommandCollectionExecuterCommand(executeCommands);
                return _bogførCommand;
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
                return _kontoReaderTask;
            }
            private set
            {
                _kontoReaderTask = value;
                RaisePropertyChanged("DatoAsTextIsReadOnly");
                RaisePropertyChanged("KontonummerIsReadOnly");
                RaisePropertyChanged("Tasks");
                RaisePropertyChanged("IsWorking");
            }
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
                return _budgetkontoReaderTask;
            }
            private set
            {
                _budgetkontoReaderTask = value;
                RaisePropertyChanged("DatoAsTextIsReadOnly");
                RaisePropertyChanged("BudgetkontonummerIsReadOnly");
                RaisePropertyChanged("Tasks");
                RaisePropertyChanged("IsWorking");
            }
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
        /// ViewModel for adressekontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        protected virtual IAdressekontoViewModel AdressekontoViewModel
        {
            get
            {
                return _adressekontoViewModel;
            }
            private set 
            { 
                _adressekontoViewModel = value;
                RaisePropertyChanged("AdressekontoNavn");
                RaisePropertyChanged("AdressekontoSaldo");
                RaisePropertyChanged("AdressekontoSaldoAsText");
            }
        }

        /// <summary>
        /// Task, der indlæser og opdaterer adressekontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        protected virtual Task<IEnumerable<IAdressekontoViewModel>> AdressekontoReaderTask
        {
            get
            {
                return _adressekontoReaderTask;
            }
            private set
            {
                _adressekontoReaderTask = value;
                RaisePropertyChanged("DatoAsTextIsReadOnly");
                RaisePropertyChanged("AdressekontoIsReadOnly");
                RaisePropertyChanged("Tasks");
                RaisePropertyChanged("IsWorking");
            }
        }

        /// <summary>
        /// Angivelse af, om den Task, der indlæser og opdaterer adressekontoen, hvortil bogføringslinjen er tilknyttet, er igangværende.
        /// </summary>
        protected virtual bool AdressekontoReaderTaskIsActive
        {
            get
            {
                return AdressekontoReaderTask != null && AdressekontoReaderTask.IsCompleted == false && AdressekontoReaderTask.IsCanceled == false && AdressekontoReaderTask.IsFaulted == false;
            }
        }

        /// <summary>
        /// Task, der udfører den asynkrone bogføring.
        /// </summary>
        protected virtual Task BogføringTask
        {
            get
            {
                return _bogføringTask;
            }
            private set
            {
                _bogføringTask = value;
                RaisePropertyChanged("DatoAsTextIsReadOnly");
                RaisePropertyChanged("BilagIsReadOnly");
                RaisePropertyChanged("KontonummerIsReadOnly");
                RaisePropertyChanged("TekstIsReadOnly");
                RaisePropertyChanged("BudgetkontonummerIsReadOnly");
                RaisePropertyChanged("DebitIsReadOnly");
                RaisePropertyChanged("KreditIsReadOnly");
                RaisePropertyChanged("AdressekontoIsReadOnly");
                RaisePropertyChanged("Tasks");
                RaisePropertyChanged("IsWorking");
            }
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

        /// <summary>
        /// Angivelse af, om den Task, der udfører den asynkrone bogføring, er igangværende.
        /// </summary>
        protected virtual bool BogføringTaskIsActive
        {
            get
            {
                return BogføringTask != null && BogføringTask.IsCompleted == false && BogføringTask.IsCanceled == false && BogføringTask.IsFaulted == false;
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
                    AdressekontoViewModelCollectionRefresh();
                    break;

                case "Kontonummer":
                    KontoViewModelRefresh();
                    break;

                case "Budgetkontonummer":
                    BudgetkontoViewModelRefresh();
                    break;

                case "Adressekonto":
                    AdressekontoViewModelCollectionRefresh();
                    break;
            }
        }

        /// <summary>
        /// Genindlæser ViewModel for kontoen, som bogføringslinjen er tilknyttet.
        /// </summary>
        private async void KontoViewModelRefresh()
        {
            KontoViewModel = null;
            if (string.IsNullOrEmpty(Kontonummer))
            {
                return;
            }
            try
            {
                KontoViewModel = await CreateKontoReaderTask();
            }
            catch (IntranetGuiExceptionBase ex)
            {
                _exceptionHandlerViewModel.HandleException(ex);
            }
            catch (Exception ex)
            {
                _exceptionHandlerViewModel.HandleException(new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.MethodError, "KontoViewModelRefresh", ex.Message), ex));
            }
            finally
            {
                KontoReaderTask = null;
            }
        }

        /// <summary>
        /// Danner Task, der indlæser og opdaterer kontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        /// <returns>Task, der indlæser og opdaterer kontoen, hvortil bogføringslinjen er tilknyttet.</returns>
        private Task<IKontoViewModel> CreateKontoReaderTask()
        {
            Func<Task<IKontoViewModel>> kontoViewModelGetter = async () =>
                {
                    try
                    {
                        var kontogruppeModelCollection = await _finansstyringRepository.KontogruppelisteGetAsync();
                        var kontoModel = await _finansstyringRepository.KontoGetAsync(Regnskab.Nummer, Kontonummer, Dato);
                        var kontogruppeViewModel = new KontogruppeViewModel(kontogruppeModelCollection.FirstOrDefault(m => m.Nummer == kontoModel.Kontogruppe), _exceptionHandlerViewModel);
                        return new KontoViewModel(Regnskab, kontoModel, kontogruppeViewModel, _finansstyringRepository, _exceptionHandlerViewModel);
                    }
                    catch
                    {
                        return null;
                    }
                };
            KontoReaderTask = Task.Run(kontoViewModelGetter);
            return KontoReaderTask;
        }

        /// <summary>
        /// Genindlæser ViewModel for budgetkontoen, som bogføringslinjen er tilknyttet.
        /// </summary>
        private async void BudgetkontoViewModelRefresh()
        {
            BudgetkontoViewModel = null;
            if (string.IsNullOrEmpty(Budgetkontonummer))
            {
                return;
            }
            try
            {
                BudgetkontoViewModel = await CreateBudgetkontoReaderTask();
            }
            catch (IntranetGuiExceptionBase ex)
            {
                _exceptionHandlerViewModel.HandleException(ex);
            }
            catch (Exception ex)
            {
                _exceptionHandlerViewModel.HandleException(new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.MethodError, "BudgetkontoViewModelRefresh", ex.Message), ex));
            }
            finally
            {
                BudgetkontoReaderTask = null;
            }
        }

        /// <summary>
        /// Danner Task, der indlæser og opdaterer budgetkontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        /// <returns>Task, der indlæser og opdaterer budgetkontoen, hvortil bogføringslinjen er tilknyttet.</returns>
        private Task<IBudgetkontoViewModel> CreateBudgetkontoReaderTask()
        {
            Func<Task<IBudgetkontoViewModel>> budgetkontoViewModelGetter = async () =>
                {
                    try
                    {
                        var budgetkontogruppeModelCollection = await _finansstyringRepository.BudgetkontogruppelisteGetAsync();
                        var budgetkontoModel = await _finansstyringRepository.BudgetkontoGetAsync(Regnskab.Nummer, Budgetkontonummer, Dato);
                        var budgetkontogruppeViewModel = new BudgetkontogruppeViewModel(budgetkontogruppeModelCollection.FirstOrDefault(m => m.Nummer == budgetkontoModel.Kontogruppe), _exceptionHandlerViewModel);
                        return new BudgetkontoViewModel(Regnskab, budgetkontoModel, budgetkontogruppeViewModel, _finansstyringRepository, _exceptionHandlerViewModel);
                    }
                    catch
                    {
                        return null;
                    }
                };
            BudgetkontoReaderTask = Task.Run(budgetkontoViewModelGetter);
            return BudgetkontoReaderTask;
        }

        /// <summary>
        /// Genindlæser ViewModels for adressekonti og dermed ViewModel for adressekontoen, som bogføringslinjen er tilknyttet.
        /// </summary>
        private async void AdressekontoViewModelCollectionRefresh()
        {
            AdressekontoViewModel = null;
            try
            {
                foreach (var adressekontoViewModel in await CreateAdressekontoReaderTask())
                {
                    var viewModel = _adressekontoViewModelCollection.FirstOrDefault(m => m.Nummer == adressekontoViewModel.Nummer);
                    if (viewModel == null)
                    {
                        adressekontoViewModel.PropertyChanged += PropertyChangedOnAdressekontoViewModelEventHander;
                        _adressekontoViewModelCollection.Add(adressekontoViewModel);
                        continue;
                    }
                    viewModel.Navn = adressekontoViewModel.Navn;
                    viewModel.PrimærTelefon = adressekontoViewModel.PrimærTelefon;
                    viewModel.SekundærTelefon = adressekontoViewModel.SekundærTelefon;
                    viewModel.StatusDato = adressekontoViewModel.StatusDato;
                    viewModel.Saldo = adressekontoViewModel.Saldo;
                }
                if (Adressekonto == 0)
                {
                    return;
                }
                AdressekontoViewModel = _adressekontoViewModelCollection.FirstOrDefault(m => m.Nummer == Adressekonto);
            }
            catch (IntranetGuiExceptionBase ex)
            {
                _exceptionHandlerViewModel.HandleException(ex);
            }
            catch (Exception ex)
            {
                _exceptionHandlerViewModel.HandleException(new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.MethodError, "BudgetkontoViewModelRefresh", ex.Message), ex));
            }
            finally
            {
                AdressekontoReaderTask = null;
            }
        }

        /// <summary>
        /// Danner Task, der indlæser og opdaterer adressekontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        /// <returns>Task, der indlæser og opdaterer adressekontoen, hvortil bogføringslinjen er tilknyttet.</returns>
        private Task<IEnumerable<IAdressekontoViewModel>> CreateAdressekontoReaderTask()
        {
            Func<Task<IEnumerable<IAdressekontoViewModel>>> adressekontoViewModelCollectionGetter = async () =>
                {
                    try
                    {
                        var adressekontoModelCollection = await _finansstyringRepository.AdressekontolisteGetAsync(Regnskab.Nummer, Dato);
                        return adressekontoModelCollection.Select(m => new AdressekontoViewModel(Regnskab, m, Resource.GetText(Text.AddressAccount), Resource.GetEmbeddedResource("Images.Adressekonto.png"), _finansstyringRepository, _exceptionHandlerViewModel));
                    }
                    catch
                    {
                        return new List<IAdressekontoViewModel>(0);
                    }
                };
            AdressekontoReaderTask = Task.Run(adressekontoViewModelCollectionGetter);
            return AdressekontoReaderTask;
        }

        /// <summary>
        /// Eventhandler, der kaldes, når en property ændres på en ViewModel for en adressekonto, der kan tilknyttes regnskabet.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        private void PropertyChangedOnAdressekontoViewModelEventHander(object sender, PropertyChangedEventArgs eventArgs)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            if (eventArgs == null)
            {
                throw new ArgumentNullException("eventArgs");
            }
            var viewModel = sender as AdressekontoViewModel;
            if (viewModel == null)
            {
                return;
            }
            switch (eventArgs.PropertyName)
            {
                case "Navn":
                    RaisePropertyChanged("Adressekonti");
                    if (AdressekontoViewModel != null && AdressekontoViewModel.Nummer == viewModel.Nummer)
                    {
                        RaisePropertyChanged("AdressekontoNavn");
                    }
                    break;

                case "Saldo":
                    if (AdressekontoViewModel != null && AdressekontoViewModel.Nummer == viewModel.Nummer)
                    {
                        RaisePropertyChanged("AdressekontoSaldo");
                    }
                    break;

                case "SaldoAsText":
                    if (AdressekontoViewModel != null && AdressekontoViewModel.Nummer == viewModel.Nummer)
                    {
                        RaisePropertyChanged("AdressekontoSaldoAsText");
                    }
                    break;
            }
        }

        /// <summary>
        /// Eventhandler, der kaldes, når collection af adressekonti, som kan tilknyttes bogføringslinjen, ændres.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        private void CollectionChangedOnAdressekontoViewModelCollectionEventHandler(object sender, NotifyCollectionChangedEventArgs eventArgs)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            if (eventArgs == null)
            {
                throw new ArgumentNullException("eventArgs");
            }
            switch (eventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    RaisePropertyChanged("Adressekonti");
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
            return result != ValidationResult.Success
                       ? result
                       : Validation.ValidateDateLowerOrEqualTo(value, DateTime.Now);
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

        /// <summary>
        /// Validerer værdien for et bogføringsbeløb.
        /// </summary>
        /// <param name="value">Værdi, der skal valideres.</param>
        /// <returns>Valideringsresultat.</returns>
        public static ValidationResult ValidateCurrency(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return ValidationResult.Success;
            }
            var result = Validation.ValidateDecimal(value);
            return result != ValidationResult.Success ? result : Validation.ValidateDecimalGreaterOrEqualTo(value, 0M);
        }

        /// <summary>
        /// Validerer værdien for en unik identifikation af adressekontoen, som bogføringslinjen skal tilknyttes.
        /// </summary>
        /// <param name="value">Værdi, der skal valideres.</param>
        /// <returns>Valideringsresultat.</returns>
        public static ValidationResult ValidateAdressekonto(int value)
        {
            return ValidationResult.Success;
        }

        #endregion
    }
}