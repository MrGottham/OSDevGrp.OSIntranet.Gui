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
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.Commands;
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
                throw new ArgumentNullException(nameof(finansstyringRepository));
            }

            if (exceptionHandlerViewModel == null)
            {
                throw new ArgumentNullException(nameof(exceptionHandlerViewModel));
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
        [CustomValidation(typeof(BogføringViewModel), "ValidateDatoAsText")]
        public virtual string DatoAsText
        {
            get { return Dato.ToString("d"); }
            set
            {
                try
                {
                    var validationResult = ValidateDatoAsText(value);
                    if (validationResult != ValidationResult.Success)
                    {
                        throw new IntranetGuiValidationException(validationResult.ErrorMessage, this, nameof(DatoAsText), value);
                    }

                    try
                    {
                        Model.Dato = DateTime.Parse(value, CultureInfo.CurrentUICulture, DateTimeStyles.AssumeLocal);
                    }
                    catch (ArgumentNullException ex)
                    {
                        throw new IntranetGuiValidationException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingPostingDate), this, nameof(DatoAsText), value, ex);
                    }
                    catch (ArgumentException ex)
                    {
                        throw new IntranetGuiValidationException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingPostingDate), this, nameof(DatoAsText), value, ex);
                    }
                }
                catch (IntranetGuiValidationException ex)
                {
                    DatoValidationError = ex.Message;
                    _exceptionHandlerViewModel.HandleException(ex);
                }
                catch (IntranetGuiExceptionBase ex)
                {
                    _exceptionHandlerViewModel.HandleException(ex);
                }
                catch (Exception ex)
                {
                    _exceptionHandlerViewModel.HandleException(new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingPropertyValue, nameof(DatoAsText), ex.Message), ex));
                }
            }
        }

        /// <summary>
        /// Valideringsfejl ved angivelse af bogføringstidspunkt.
        /// </summary>
        public virtual string DatoValidationError
        {
            get { return GetValidationError(nameof(Dato)); }
            private set { SetValidationError(nameof(Dato), value, nameof(DatoValidationError)); }
        }

        /// <summary>
        /// Angivelse af, om tekstangivelsen for bogføringstidspunktet kan redigeres.
        /// </summary>
        public virtual bool DatoAsTextIsReadOnly => KontoReaderTaskIsActive || BudgetkontoReaderTaskIsActive || AdressekontoReaderTaskIsActive || ErBogført;

        /// <summary>
        /// Label til bogføringstidspunkt.
        /// </summary>
        public virtual string DatoLabel => Resource.GetText(Text.Date);

        /// <summary>
        /// Bilagsnummer.
        /// </summary>
        [CustomValidation(typeof(BogføringViewModel), "ValidateBilag")]
        public new virtual string Bilag
        {
            get { return base.Bilag; }
            set
            {
                try
                {
                    var validationResult = ValidateBilag(value);
                    if (validationResult != ValidationResult.Success)
                    {
                        throw new IntranetGuiValidationException(validationResult.ErrorMessage, this, nameof(Bilag), value);
                    }

                    try
                    {
                        Model.Bilag = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
                    }
                    catch (ArgumentNullException ex)
                    {
                        throw new IntranetGuiValidationException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingReference), this, nameof(Bilag), value, ex);
                    }
                    catch (ArgumentException ex)
                    {
                        throw new IntranetGuiValidationException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingReference), this, nameof(Bilag), value, ex);
                    }
                }
                catch (IntranetGuiValidationException ex)
                {
                    BilagValidationError = ex.Message;
                    _exceptionHandlerViewModel.HandleException(ex);
                }
                catch (IntranetGuiExceptionBase ex)
                {
                    _exceptionHandlerViewModel.HandleException(ex);
                }
                catch (Exception ex)
                {
                    _exceptionHandlerViewModel.HandleException(new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingPropertyValue, nameof(Bilag), ex.Message), ex));
                }
            }
        }

        /// <summary>
        /// Valideringsfejl ved angivelse af bilagsnummer.
        /// </summary>
        public virtual string BilagValidationError
        {
            get { return GetValidationError(nameof(Bilag)); }
            private set { SetValidationError(nameof(Bilag), value, nameof(BilagValidationError)); }
        }

        /// <summary>
        /// Angivelse af den maksimale tekstlængde for bilagsnummeret.
        /// </summary>
        public virtual int BilagMaxLength => FieldInformations.BilagFieldLength;

        /// <summary>
        /// Angivelse af, om bilagsnummeret kan redigeres.
        /// </summary>
        public virtual bool BilagIsReadOnly => ErBogført;

        /// <summary>
        /// Label til bilagsnummer.
        /// </summary>
        public virtual string BilagLabel => Resource.GetText(Text.Reference);

        /// <summary>
        /// Kontonummer, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        [CustomValidation(typeof(BogføringViewModel), "ValidateKontonummer")]
        public new virtual string Kontonummer
        {
            get { return base.Kontonummer; }
            set
            {
                try
                {
                    var validationResult = ValidateKontonummer(value);
                    if (validationResult != ValidationResult.Success)
                    {
                        throw new IntranetGuiValidationException(validationResult.ErrorMessage, this, nameof(Kontonummer), value);
                    }

                    try
                    {
                        Model.Kontonummer = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
                    }
                    catch (ArgumentNullException ex)
                    {
                        throw new IntranetGuiValidationException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingAccountNumber), this, nameof(Kontonummer), value, ex);
                    }
                    catch (ArgumentException ex)
                    {
                        throw new IntranetGuiValidationException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingAccountNumber), this, nameof(Kontonummer), value, ex);
                    }
                }
                catch (IntranetGuiValidationException ex)
                {
                    KontonummerValidationError = ex.Message;
                    _exceptionHandlerViewModel.HandleException(ex);
                }
                catch (IntranetGuiExceptionBase ex)
                {
                    _exceptionHandlerViewModel.HandleException(ex);
                }
                catch (Exception ex)
                {
                    _exceptionHandlerViewModel.HandleException(new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingPropertyValue, nameof(Kontonummer), ex.Message), ex));
                }
            }
        }

        /// <summary>
        /// Valideringsfejl ved angivelse af kontonummer, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual string KontonummerValidationError
        {
            get { return GetValidationError(nameof(Kontonummer)); }
            private set { SetValidationError(nameof(Kontonummer), value, nameof(KontonummerValidationError)); }
        }

        /// <summary>
        /// Angivelse af den maksimale tekstlængde for kontonummeret.
        /// </summary>
        public virtual int KontonummerMaxLength => FieldInformations.KontonummerFieldLength;

        /// <summary>
        /// Angivelse af, om kontonummeret kan redigeres.
        /// </summary>
        public virtual bool KontonummerIsReadOnly => KontoReaderTaskIsActive || ErBogført;

        /// <summary>
        /// Label til kontonummeret, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual string KontonummerLabel => Resource.GetText(Text.Account);

        /// <summary>
        /// Navn på kontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual string Kontonavn => KontoViewModel == null ? string.Empty : KontoViewModel.Kontonavn;

        /// <summary>
        /// Label til navnet på kontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual string KontonavnLabel => Resource.GetText(Text.Account);

        /// <summary>
        /// Saldo på kontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual decimal KontoSaldo => KontoViewModel == null ? 0M : KontoViewModel.Saldo;

        /// <summary>
        /// Tekstangivelse af saldo på kontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual string KontoSaldoAsText => KontoViewModel == null ? string.Empty : KontoViewModel.SaldoAsText;

        /// <summary>
        /// Label til saldoen på kontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual string KontoSaldoLabel => Resource.GetText(Text.Balance);

        /// <summary>
        /// Disponibel beløb på kontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual decimal KontoDisponibel => KontoViewModel == null ? 0M : KontoViewModel.Disponibel;

        /// <summary>
        /// Tekstangivelse af disponibel beløb på kontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual string KontoDisponibelAsText => KontoViewModel == null ? string.Empty : KontoViewModel.DisponibelAsText;

        /// <summary>
        /// Label til disponibel beløb på kontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual string KontoDisponibelLabel => Resource.GetText(Text.Available);

        /// <summary>
        /// Tekst.
        /// </summary>
        [CustomValidation(typeof(BogføringViewModel), "ValidateTekst")]
        public new virtual string Tekst
        {
            get { return base.Tekst; }
            set
            {
                try
                {
                    var validationResult = ValidateTekst(value);
                    if (validationResult != ValidationResult.Success)
                    {
                        throw new IntranetGuiValidationException(validationResult.ErrorMessage, this, nameof(Tekst), value);
                    }

                    try
                    {
                        Model.Tekst = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
                    }
                    catch (ArgumentNullException ex)
                    {
                        throw new IntranetGuiValidationException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingPostingText), this, nameof(Tekst), value, ex);
                    }
                    catch (ArgumentException ex)
                    {
                        throw new IntranetGuiValidationException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingPostingText), this, nameof(Tekst), value, ex);
                    }
                }
                catch (IntranetGuiValidationException ex)
                {
                    TekstValidationError = ex.Message;
                    _exceptionHandlerViewModel.HandleException(ex);
                }
                catch (IntranetGuiExceptionBase ex)
                {
                    _exceptionHandlerViewModel.HandleException(ex);
                }
                catch (Exception ex)
                {
                    _exceptionHandlerViewModel.HandleException(new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingPropertyValue, nameof(Tekst), ex.Message), ex));
                }
            }
        }

        /// <summary>
        /// Valideringsfejl ved angivelse af tekst.
        /// </summary>
        public virtual string TekstValidationError
        {
            get { return GetValidationError(nameof(Tekst)); }
            private set { SetValidationError(nameof(Tekst), value, nameof(TekstValidationError)); }
        }

        /// <summary>
        /// Angivelse af den maksimale tekstlængde for teksten til bogføringslinjen.
        /// </summary>
        public virtual int TekstMaxLength => FieldInformations.BogføringstekstFieldLength;

        /// <summary>
        /// Angivelse af, om teksten til bogføringslinjen kan redigeres.
        /// </summary>
        public virtual bool TekstIsReadOnly => ErBogført;

        /// <summary>
        /// Label til teksten på bogføringslinjen.
        /// </summary>
        public virtual string TekstLabel => Resource.GetText(Text.Text);

        /// <summary>
        /// Kontonummer på budgetkontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        [CustomValidation(typeof(BogføringViewModel), "ValidateBudgetkontonummer")]
        public new virtual string Budgetkontonummer
        {
            get { return base.Budgetkontonummer; }
            set
            {
                try
                {
                    var validationResult = ValidateBudgetkontonummer(value);
                    if (validationResult != ValidationResult.Success)
                    {
                        throw new IntranetGuiValidationException(validationResult.ErrorMessage, this, nameof(Budgetkontonummer), value);
                    }

                    try
                    {
                        Model.Budgetkontonummer = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
                    }
                    catch (ArgumentNullException ex)
                    {
                        throw new IntranetGuiValidationException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingBudgetAccountNumber), this, nameof(Budgetkontonummer), value, ex);
                    }
                    catch (ArgumentException ex)
                    {
                        throw new IntranetGuiValidationException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingBudgetAccountNumber), this, nameof(Budgetkontonummer), value, ex);
                    }
                }
                catch (IntranetGuiValidationException ex)
                {
                    BudgetkontonummerValidationError = ex.Message;
                    _exceptionHandlerViewModel.HandleException(ex);
                }
                catch (IntranetGuiExceptionBase ex)
                {
                    _exceptionHandlerViewModel.HandleException(ex);
                }
                catch (Exception ex)
                {
                    _exceptionHandlerViewModel.HandleException(new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingPropertyValue, nameof(Budgetkontonummer), ex.Message), ex));
                }
            }
        }

        /// <summary>
        /// Valideringsfejl ved angivelse af kontonummer på budgetkontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual string BudgetkontonummerValidationError
        {
            get { return GetValidationError(nameof(Budgetkontonummer)); }
            private set { SetValidationError(nameof(Budgetkontonummer), value, nameof(BudgetkontonummerValidationError)); }
        }

        /// <summary>
        /// Angivelse af den maksimale tekstlængde for kontonummeret på budgetkontoen.
        /// </summary>
        public virtual int BudgetkontonummerMaxLength => FieldInformations.KontonummerFieldLength;

        /// <summary>
        /// Angivelse af, om kontonummeret på budgetkontoen kan redigeres.
        /// </summary>
        public virtual bool BudgetkontonummerIsReadOnly => BudgetkontoReaderTaskIsActive || ErBogført;

        /// <summary>
        /// Label til kontonummeret på budgetkontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual string BudgetkontonummerLabel => Resource.GetText(Text.BudgetAccount);

        /// <summary>
        /// Navn på budgetkontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual string Budgetkontonavn => BudgetkontoViewModel == null ? string.Empty : BudgetkontoViewModel.Kontonavn;

        /// <summary>
        /// Label til navnet på budgetkontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual string BudgetkontonavnLabel => Resource.GetText(Text.BudgetAccount);

        /// <summary>
        /// Bogført beløb på budgetkontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual decimal BudgetkontoBogført => BudgetkontoViewModel == null ? 0M : BudgetkontoViewModel.Bogført;

        /// <summary>
        /// Tekstangivelse af bogført beløb på budgetkontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual string BudgetkontoBogførtAsText => BudgetkontoViewModel == null ? string.Empty : BudgetkontoViewModel.BogførtAsText;

        /// <summary>
        /// Label til bogført beløb på budgetkontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual string BudgetkontoBogførtLabel => Resource.GetText(Text.Posted);

        /// <summary>
        /// Disponibel beløb på budgetkontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual decimal BudgetkontoDisponibel => BudgetkontoViewModel == null ? 0M : BudgetkontoViewModel.Disponibel;

        /// <summary>
        /// Tekstangivelse af disponibel beløb på budgetkontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual string BudgetkontoDisponibelAsText => BudgetkontoViewModel == null ? string.Empty : BudgetkontoViewModel.DisponibelAsText;

        /// <summary>
        /// Label til disponibel beløb på budgetkontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual string BudgetkontoDisponibelLabel => Resource.GetText(Text.Available);

        /// <summary>
        /// Tekstangivelse af debitbeløb.
        /// </summary>
        [CustomValidation(typeof(BogføringViewModel), "ValidateCurrency")]
        public new virtual string DebitAsText
        {
            get { return base.DebitAsText; }
            set
            {
                try
                {
                    var result = ValidateCurrency(value);
                    if (result != ValidationResult.Success)
                    {
                        throw new IntranetGuiValidationException(result.ErrorMessage, this, nameof(DebitAsText), value);
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
                        throw new IntranetGuiValidationException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingDebit), this, nameof(DebitAsText), value, ex);
                    }
                    catch (ArgumentException ex)
                    {
                        throw new IntranetGuiValidationException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingDebit), this, nameof(DebitAsText), value, ex);
                    }
                }
                catch (IntranetGuiValidationException ex)
                {
                    DebitValidationError = ex.Message;
                    _exceptionHandlerViewModel.HandleException(ex);
                }
                catch (IntranetGuiExceptionBase ex)
                {
                    _exceptionHandlerViewModel.HandleException(ex);
                }
                catch (Exception ex)
                {
                    _exceptionHandlerViewModel.HandleException(new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingPropertyValue, nameof(DebitAsText), ex.Message), ex));
                }
            }
        }

        /// <summary>
        /// Valideringsfejl ved angivelse af debitbeløb.
        /// </summary>
        public virtual string DebitValidationError
        {
            get { return GetValidationError(nameof(Debit)); }
            private set { SetValidationError(nameof(Debit), value, nameof(DebitValidationError)); }
        }

        /// <summary>
        /// Angivelse af den maksimale tekstlængde for debitbeløbet.
        /// </summary>
        public virtual int DebitMaxLength => FieldInformations.DebitFieldLength;

        /// <summary>
        /// Angivelse af, om debitbeløbet kan redigeres.
        /// </summary>
        public virtual bool DebitIsReadOnly => ErBogført;

        /// <summary>
        /// Label til debitbeløbet.
        /// </summary>
        public virtual string DebitLabel => Resource.GetText(Text.Debit);

        /// <summary>
        /// Tekstangivelse af kreditbeløb.
        /// </summary>
        [CustomValidation(typeof(BogføringViewModel), "ValidateCurrency")]
        public new virtual string KreditAsText
        {
            get { return base.KreditAsText; }
            set
            {
                try
                {
                    var result = ValidateCurrency(value);
                    if (result != ValidationResult.Success)
                    {
                        throw new IntranetGuiValidationException(result.ErrorMessage, this, nameof(KreditAsText), value);
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
                        throw new IntranetGuiValidationException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingCredit), this, nameof(KreditAsText), value, ex);
                    }
                    catch (ArgumentException ex)
                    {
                        throw new IntranetGuiValidationException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingCredit), this, nameof(KreditAsText), value, ex);
                    }
                }
                catch (IntranetGuiValidationException ex)
                {
                    KreditValidationError = ex.Message;
                    _exceptionHandlerViewModel.HandleException(ex);
                }
                catch (IntranetGuiExceptionBase ex)
                {
                    _exceptionHandlerViewModel.HandleException(ex);
                }
                catch (Exception ex)
                {
                    _exceptionHandlerViewModel.HandleException(new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingPropertyValue, nameof(KreditAsText), ex.Message), ex));
                }
            }
        }

        /// <summary>
        /// Valideringsfejl ved angivelse af kreditbeløb.
        /// </summary>
        public virtual string KreditValidationError
        {
            get { return GetValidationError(nameof(Kredit)); }
            private set { SetValidationError(nameof(Kredit), value, nameof(KreditValidationError)); }
        }

        /// <summary>
        /// Angivelse af den maksimale tekstlængde for kreditbeløbet.
        /// </summary>
        public virtual int KreditMaxLength => FieldInformations.KreditFieldLength;

        /// <summary>
        /// Angivelse af, om kreditbeløbet kan redigeres.
        /// </summary>
        public virtual bool KreditIsReadOnly => ErBogført;

        /// <summary>
        /// Label til kreditbeløbet.
        /// </summary>
        public virtual string KreditLabel => Resource.GetText(Text.Credit);

        /// <summary>
        /// Unik identifikation af adressekonto, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        [CustomValidation(typeof(BogføringViewModel), "ValidateAdressekonto")]
        public new virtual int Adressekonto
        {
            get { return base.Adressekonto; }
            set
            {
                try
                {
                    var result = ValidateAdressekonto(value);
                    if (result != ValidationResult.Success)
                    {
                        throw new IntranetGuiValidationException(result.ErrorMessage, this, nameof(Adressekonto), value);
                    }

                    try
                    {
                        Model.Adressekonto = value;
                    }
                    catch (ArgumentNullException ex)
                    {
                        throw new IntranetGuiValidationException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingAddressAccount), this, nameof(Adressekonto), value, ex);
                    }
                    catch (ArgumentException ex)
                    {
                        throw new IntranetGuiValidationException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingAddressAccount), this, nameof(Adressekonto), value, ex);
                    }
                }
                catch (IntranetGuiValidationException ex)
                {
                    AdressekontoValidationError = ex.Message;
                    _exceptionHandlerViewModel.HandleException(ex);
                }
                catch (IntranetGuiExceptionBase ex)
                {
                    _exceptionHandlerViewModel.HandleException(ex);
                }
                catch (Exception ex)
                {
                    _exceptionHandlerViewModel.HandleException(new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingPropertyValue, nameof(Adressekonto), ex.Message), ex));
                }
            }
        }

        /// <summary>
        /// Valideringsfejl ved angivelse af den unikke identifikation af adressekonto, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual string AdressekontoValidationError
        {
            get { return GetValidationError(nameof(Adressekonto)); }
            private set { SetValidationError(nameof(Adressekonto), value, nameof(AdressekontoValidationError)); }
        }

        /// <summary>
        /// Angivelse af, om den unikke identifikation af adressekonto, hvortil bogføringslinjen er tilknyttet, kan redigeres.
        /// </summary>
        public virtual bool AdressekontoIsReadOnly => AdressekontoReaderTaskIsActive || ErBogført;

        /// <summary>
        /// Label til den unikke identifikation af adressekonto, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        public virtual string AdressekontoLabel => Resource.GetText(Text.AddressAccount);

        /// <summary>
        /// Navn på adressekontoen.
        /// </summary>
        public virtual string AdressekontoNavn => AdressekontoViewModel == null ? string.Empty : AdressekontoViewModel.Navn;

        /// <summary>
        /// Label til navnet på adressekontoen.
        /// </summary>
        public virtual string AdressekontoNavnLabel => Resource.GetText(Text.Name);

        /// <summary>
        /// Saldo på adressekontoen.
        /// </summary>
        public virtual decimal AdressekontoSaldo => AdressekontoViewModel?.Saldo ?? 0M;

        /// <summary>
        /// Tekstangivelse af saldo på adressekontoen.
        /// </summary>
        public virtual string AdressekontoSaldoAsText => AdressekontoViewModel == null ? string.Empty : AdressekontoViewModel.SaldoAsText;

        /// <summary>
        /// Label til saldoen på adressekontoen.
        /// </summary>
        public virtual string AdressekontoSaldoLabel => Resource.GetText(Text.Balance);

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
        public virtual string AdressekontiLabel => Resource.GetText(Text.AddressAccounts);

        /// <summary>
        /// Tasks, der udføres asynkront.
        /// </summary>
        public virtual IEnumerable<Task> Tasks
        {
            get
            {
                IList<Task> tasks = new List<Task>();
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
        public virtual bool IsWorking => KontoReaderTaskIsActive || BudgetkontoReaderTaskIsActive || AdressekontoReaderTaskIsActive || BogføringTaskIsActive;

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

                BogføringAddCommand bogføringAddCommand = new BogføringAddCommand(_finansstyringRepository, _exceptionHandlerViewModel);
                bogføringAddCommand.OnError += (s, e) => BogføringTask = null;

                Collection<ICommand> executeCommands = new Collection<ICommand>
                {
                    bogføringAddCommand,
                    new RelayCommand(obj => BogføringTask = bogføringAddCommand.ExecuteTask)
                };
                _bogførCommand = new CommandCollectionExecuterCommand(executeCommands);

                return _bogførCommand;
            }
        }

        /// <summary>
        /// Label til kommandoen, der kan foretage bogføring af bogføringslinjen.
        /// </summary>
        public virtual string BogførCommandLabel => Resource.GetText(Text.AddPostingLine);

        /// <summary>
        /// ViewModel for kontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        protected virtual IKontoViewModel KontoViewModel
        {
            get { return _kontoViewModel; }
            private set
            {
                _kontoViewModel = value;
                RaisePropertyChanged(nameof(Kontonavn));
                RaisePropertyChanged(nameof(KontoSaldo));
                RaisePropertyChanged(nameof(KontoSaldoAsText));
                RaisePropertyChanged(nameof(KontoDisponibel));
                RaisePropertyChanged(nameof(KontoDisponibelAsText));
            }
        }

        /// <summary>
        /// Task, der indlæser og opdaterer kontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        protected virtual Task<IKontoViewModel> KontoReaderTask
        {
            get { return _kontoReaderTask; }
            private set
            {
                _kontoReaderTask = value;
                RaisePropertyChanged(nameof(DatoAsTextIsReadOnly));
                RaisePropertyChanged(nameof(KontonummerIsReadOnly));
                RaisePropertyChanged(nameof(Tasks));
                RaisePropertyChanged(nameof(IsWorking));
            }
        }

        /// <summary>
        /// Angivelse af, om den Task, der indlæser og opdaterer kontoen, hvortil bogføringslinjen er tilknyttet, er igangværende.
        /// </summary>
        protected virtual bool KontoReaderTaskIsActive => KontoReaderTask != null && KontoReaderTask.IsCompleted == false && KontoReaderTask.IsCanceled == false && KontoReaderTask.IsFaulted == false;

        /// <summary>
        /// ViewModel for budgetkontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        protected virtual IBudgetkontoViewModel BudgetkontoViewModel
        {
            get { return _budgetkontoViewModel; }
            private set
            {
                _budgetkontoViewModel = value;
                RaisePropertyChanged(nameof(Budgetkontonavn));
                RaisePropertyChanged(nameof(BudgetkontoBogført));
                RaisePropertyChanged(nameof(BudgetkontoBogførtAsText));
                RaisePropertyChanged(nameof(BudgetkontoDisponibel));
                RaisePropertyChanged(nameof(BudgetkontoDisponibelAsText));
            }
        }

        /// <summary>
        /// Task, der indlæser og opdaterer budgetkontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        protected virtual Task<IBudgetkontoViewModel> BudgetkontoReaderTask
        {
            get { return _budgetkontoReaderTask; }
            private set
            {
                _budgetkontoReaderTask = value;
                RaisePropertyChanged(nameof(DatoAsTextIsReadOnly));
                RaisePropertyChanged(nameof(BudgetkontonummerIsReadOnly));
                RaisePropertyChanged(nameof(Tasks));
                RaisePropertyChanged(nameof(IsWorking));
            }
        }

        /// <summary>
        /// Angivelse af, om den Task, der indlæser og opdaterer budgetkontoen, hvortil bogføringslinjen er tilknyttet, er igangværende.
        /// </summary>
        protected virtual bool BudgetkontoReaderTaskIsActive => BudgetkontoReaderTask != null && BudgetkontoReaderTask.IsCompleted == false && BudgetkontoReaderTask.IsCanceled == false && BudgetkontoReaderTask.IsFaulted == false;

        /// <summary>
        /// ViewModel for adressekontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        protected virtual IAdressekontoViewModel AdressekontoViewModel
        {
            get { return _adressekontoViewModel; }
            private set
            {
                _adressekontoViewModel = value;
                RaisePropertyChanged(nameof(AdressekontoNavn));
                RaisePropertyChanged(nameof(AdressekontoSaldo));
                RaisePropertyChanged(nameof(AdressekontoSaldoAsText));
            }
        }

        /// <summary>
        /// Task, der indlæser og opdaterer adressekontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        protected virtual Task<IEnumerable<IAdressekontoViewModel>> AdressekontoReaderTask
        {
            get { return _adressekontoReaderTask; }
            private set
            {
                _adressekontoReaderTask = value;
                RaisePropertyChanged(nameof(DatoAsTextIsReadOnly));
                RaisePropertyChanged(nameof(AdressekontoIsReadOnly));
                RaisePropertyChanged(nameof(Tasks));
                RaisePropertyChanged(nameof(IsWorking));
            }
        }

        /// <summary>
        /// Angivelse af, om den Task, der indlæser og opdaterer adressekontoen, hvortil bogføringslinjen er tilknyttet, er igangværende.
        /// </summary>
        protected virtual bool AdressekontoReaderTaskIsActive => AdressekontoReaderTask != null && AdressekontoReaderTask.IsCompleted == false && AdressekontoReaderTask.IsCanceled == false && AdressekontoReaderTask.IsFaulted == false;

        /// <summary>
        /// Task, der udfører den asynkrone bogføring.
        /// </summary>
        protected virtual Task BogføringTask
        {
            get { return _bogføringTask; }
            private set
            {
                _bogføringTask = value;
                RaisePropertyChanged(nameof(DatoAsTextIsReadOnly));
                RaisePropertyChanged(nameof(BilagIsReadOnly));
                RaisePropertyChanged(nameof(KontonummerIsReadOnly));
                RaisePropertyChanged(nameof(TekstIsReadOnly));
                RaisePropertyChanged(nameof(BudgetkontonummerIsReadOnly));
                RaisePropertyChanged(nameof(DebitIsReadOnly));
                RaisePropertyChanged(nameof(KreditIsReadOnly));
                RaisePropertyChanged(nameof(AdressekontoIsReadOnly));
                RaisePropertyChanged(nameof(Tasks));
                RaisePropertyChanged(nameof(IsWorking));
            }
        }

        /// <summary>
        /// Angivelse af, om bogføringslinjen er ved eller er blevet bogført.
        /// </summary>
        protected virtual bool ErBogført => BogføringTask != null;

        /// <summary>
        /// Angivelse af, om den Task, der udfører den asynkrone bogføring, er igangværende.
        /// </summary>
        protected virtual bool BogføringTaskIsActive => BogføringTask != null && BogføringTask.IsCompleted == false && BogføringTask.IsCanceled == false && BogføringTask.IsFaulted == false;

        #endregion

        #region Methods

        public IBogføringslinjeModel ToModel()
        {
            return Model;
        }

        /// <summary>
        /// Nulstiller alle valideringsfejl.
        /// </summary>
        public override void ClearValidationErrors()
        {
            DatoValidationError = null;
            BilagValidationError = null;
            KontonummerValidationError = null;
            TekstValidationError = null;
            BudgetkontonummerValidationError = null;
            DebitValidationError = null;
            KreditValidationError = null;
            AdressekontoValidationError = null;
        }

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
                case nameof(Dato):
                    DatoValidationError = null;
                    RaisePropertyChanged(nameof(DatoAsText));
                    KontoViewModelRefresh();
                    BudgetkontoViewModelRefresh();
                    AdressekontoViewModelCollectionRefresh();
                    break;

                case nameof(Bilag):
                    BilagValidationError = null;
                    break;

                case nameof(Kontonummer):
                    KontonummerValidationError = null;
                    KontoViewModelRefresh();
                    break;

                case nameof(Tekst):
                    TekstValidationError = null;
                    break;

                case nameof(Budgetkontonummer):
                    BudgetkontonummerValidationError = null;
                    BudgetkontoViewModelRefresh();
                    break;

                case nameof(Debit):
                    DebitValidationError = null;
                    break;

                case nameof(Kredit):
                    KreditValidationError = null;
                    break;

                case nameof(Adressekonto):
                    AdressekontoValidationError = null;
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
            if (string.IsNullOrWhiteSpace(Kontonummer))
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
        private async Task<IKontoViewModel> CreateKontoReaderTask()
        {
            try
            {
                IEnumerable<IKontogruppeModel> kontogruppeModelCollection = await _finansstyringRepository.KontogruppelisteGetAsync();
                IKontoModel kontoModel = await _finansstyringRepository.KontoGetAsync(Regnskab.Nummer, Kontonummer, Dato);
                IKontogruppeViewModel kontogruppeViewModel = new KontogruppeViewModel(kontogruppeModelCollection.FirstOrDefault(m => m.Nummer == kontoModel.Kontogruppe), _exceptionHandlerViewModel);
                return new KontoViewModel(Regnskab, kontoModel, kontogruppeViewModel, _finansstyringRepository, _exceptionHandlerViewModel);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Genindlæser ViewModel for budgetkontoen, som bogføringslinjen er tilknyttet.
        /// </summary>
        private async void BudgetkontoViewModelRefresh()
        {
            BudgetkontoViewModel = null;
            if (string.IsNullOrWhiteSpace(Budgetkontonummer))
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
        private async Task<IBudgetkontoViewModel> CreateBudgetkontoReaderTask()
        {
            try
            {
                IEnumerable<IBudgetkontogruppeModel> budgetkontogruppeModelCollection = await _finansstyringRepository.BudgetkontogruppelisteGetAsync();
                IBudgetkontoModel budgetkontoModel = await _finansstyringRepository.BudgetkontoGetAsync(Regnskab.Nummer, Budgetkontonummer, Dato);
                IBudgetkontogruppeViewModel budgetkontogruppeViewModel = new BudgetkontogruppeViewModel(budgetkontogruppeModelCollection.FirstOrDefault(m => m.Nummer == budgetkontoModel.Kontogruppe), _exceptionHandlerViewModel);
                return new BudgetkontoViewModel(Regnskab, budgetkontoModel, budgetkontogruppeViewModel, _finansstyringRepository, _exceptionHandlerViewModel);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Genindlæser ViewModels for adressekonti og dermed ViewModel for adressekontoen, som bogføringslinjen er tilknyttet.
        /// </summary>
        private async void AdressekontoViewModelCollectionRefresh()
        {
            AdressekontoViewModel = null;
            try
            {
                foreach (IAdressekontoViewModel adressekontoViewModel in await CreateAdressekontoReaderTask())
                {
                    IAdressekontoViewModel viewModel = _adressekontoViewModelCollection.SingleOrDefault(m => m.Nummer == adressekontoViewModel.Nummer);
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

                AdressekontoViewModel = _adressekontoViewModelCollection.SingleOrDefault(m => m.Nummer == Adressekonto);
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
        private async Task<IEnumerable<IAdressekontoViewModel>> CreateAdressekontoReaderTask()
        {
            try
            {
                IEnumerable<IAdressekontoModel> adressekontoModelCollection = await _finansstyringRepository.AdressekontolisteGetAsync(Regnskab.Nummer, Dato);
                return adressekontoModelCollection.Select(m => new AdressekontoViewModel(Regnskab, m, Resource.GetText(Text.AddressAccount), Resource.GetEmbeddedResource("Images.Adressekonto.png"), _finansstyringRepository, _exceptionHandlerViewModel));
            }
            catch
            {
                return new List<IAdressekontoViewModel>(0);
            }
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
                throw new ArgumentNullException(nameof(sender));
            }

            if (eventArgs == null)
            {
                throw new ArgumentNullException(nameof(eventArgs));
            }

            AdressekontoViewModel viewModel = sender as AdressekontoViewModel;
            if (viewModel == null)
            {
                return;
            }

            switch (eventArgs.PropertyName)
            {
                case "Navn":
                    RaisePropertyChanged(nameof(Adressekonti));
                    if (AdressekontoViewModel != null && AdressekontoViewModel.Nummer == viewModel.Nummer)
                    {
                        RaisePropertyChanged(nameof(AdressekontoNavn));
                    }
                    break;

                case "Saldo":
                    if (AdressekontoViewModel != null && AdressekontoViewModel.Nummer == viewModel.Nummer)
                    {
                        RaisePropertyChanged(nameof(AdressekontoSaldo));
                    }
                    break;

                case "SaldoAsText":
                    if (AdressekontoViewModel != null && AdressekontoViewModel.Nummer == viewModel.Nummer)
                    {
                        RaisePropertyChanged(nameof(AdressekontoSaldoAsText));
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
                throw new ArgumentNullException(nameof(sender));
            }

            if (eventArgs == null)
            {
                throw new ArgumentNullException(nameof(eventArgs));
            }

            switch (eventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    RaisePropertyChanged(nameof(Adressekonti));
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
            ValidationResult result = Core.Validators.Validation.ValidateRequiredValue(value);
            if (result != ValidationResult.Success)
            {
                return result;
            }

            result = Core.Validators.Validation.ValidateDate(value);
            return result != ValidationResult.Success
                ? result
                : Core.Validators.Validation.ValidateDateLowerOrEqualTo(value, DateTime.Now);
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
            return Core.Validators.Validation.ValidateRequiredValue(value);
        }

        /// <summary>
        /// Validerer værdien for teksten til bogføringslinjen.
        /// </summary>
        /// <param name="value">Værdi, der skal valideres.</param>
        /// <returns>Valideringsresultat.</returns>
        public static ValidationResult ValidateTekst(string value)
        {
            return Core.Validators.Validation.ValidateRequiredValue(value);
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

            ValidationResult result = Core.Validators.Validation.ValidateDecimal(value);
            return result != ValidationResult.Success ? result : Core.Validators.Validation.ValidateDecimalGreaterOrEqualTo(value, 0M);
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