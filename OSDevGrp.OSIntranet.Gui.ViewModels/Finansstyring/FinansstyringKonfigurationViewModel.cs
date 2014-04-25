using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Repositories.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core.Validators;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring
{
    /// <summary>
    /// ViewModel indeholdende konfiguration til finansstyring.
    /// </summary>
    public class FinansstyringKonfigurationViewModel : ValidateableViewModelBase, IFinansstyringKonfigurationViewModel
    {
        #region Private variables

        private readonly IFinansstyringKonfigurationRepository _finansstyringKonfigurationRepository;
        private readonly IExceptionHandlerViewModel _exceptionHandlerViewModel;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner en ViewModel indeholdende konfiguration til finansstyring.
        /// </summary>
        /// <param name="finansstyringKonfigurationRepository">Implementering af konfigurationsrepository til finansstyring.</param>
        /// <param name="exceptionHandlerViewModel">Implementering af ViewMdoel til en exceptionhandler.</param>
        public FinansstyringKonfigurationViewModel(IFinansstyringKonfigurationRepository finansstyringKonfigurationRepository, IExceptionHandlerViewModel exceptionHandlerViewModel)
        {
            if (finansstyringKonfigurationRepository == null)
            {
                throw new ArgumentNullException("finansstyringKonfigurationRepository");
            }
            if (exceptionHandlerViewModel == null)
            {
                throw new ArgumentNullException("exceptionHandlerViewModel");
            }
            _finansstyringKonfigurationRepository = finansstyringKonfigurationRepository;
            _exceptionHandlerViewModel = exceptionHandlerViewModel;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Unikt navn for konfigurationen.
        /// </summary>
        public virtual string Configuration
        {
            get
            {
                return "OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.FinansstyringKonfigurationViewModel";
            }
        }

        /// <summary>
        /// Collection indeholdende navne for de enkelte konfigurationsværdier.
        /// </summary>
        public virtual IEnumerable<string> Keys
        {
            get
            {
                return FinansstyringKonfigurationRepository.Keys;
            }
        }

        /// <summary>
        /// Navn for ViewModel, som kan benyttes til visning i brugergrænsefladen.
        /// </summary>
        public override string DisplayName
        {
            get
            {
                return Resource.GetText(Text.Configuration);
            }
        }

        /// <summary>
        /// Label til uri for servicen, der supporterer finansstyring.
        /// </summary>
        public virtual string FinansstyringServiceUriLabel
        {
            get
            {
                return Resource.GetText(Text.SupportingServiceUri);
            }
        }

        /// <summary>
        /// Uri til servicen, der supporterer finansstyring.
        /// </summary>
        [CustomValidation(typeof(FinansstyringKonfigurationViewModel), "ValidateFinansstyringServiceUri")]
        public virtual string FinansstyringServiceUri
        {
            get
            {
                try
                {
                    return _finansstyringKonfigurationRepository.FinansstyringServiceUri.ToString();
                }
                catch (IntranetGuiExceptionBase ex)
                {
                    _exceptionHandlerViewModel.HandleException(ex);
                    return string.Empty;
                }
                catch (Exception ex)
                {
                    _exceptionHandlerViewModel.HandleException(new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileGettingPropertyValue, "FinansstyringServiceUri", ex.Message), ex));
                    return string.Empty;
                }
            }
            set
            {
                try
                {
                    var validationResult = ValidateFinansstyringServiceUri(value);
                    if (validationResult != ValidationResult.Success)
                    {
                        throw new IntranetGuiValidationException(validationResult.ErrorMessage, this, "FinansstyringServiceUri", value);
                    }
                    FinansstyringServiceUriValidationError = null;
                    var uri = new Uri(value);
                    if (_finansstyringKonfigurationRepository.FinansstyringServiceUri == uri)
                    {
                        return;
                    }
                    _finansstyringKonfigurationRepository.KonfigurationerAdd(new Dictionary<string, object> {{"FinansstyringServiceUri", uri}});
                    RaisePropertyChanged("FinansstyringServiceUri");
                }
                catch (IntranetGuiValidationException ex)
                {
                    FinansstyringServiceUriValidationError = ex.Message;
                    _exceptionHandlerViewModel.HandleException(ex);
                }
                catch (IntranetGuiExceptionBase ex)
                {
                    _exceptionHandlerViewModel.HandleException(ex);
                }
                catch (Exception ex)
                {
                    _exceptionHandlerViewModel.HandleException(new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingPropertyValue, "FinansstyringServiceUri", ex.Message), ex));
                }
            }
        }

        /// <summary>
        /// Valideringsfejl ved angivelse af uri til servicen, der supporterer finansstyring.
        /// </summary>
        public virtual string FinansstyringServiceUriValidationError
        {
            get
            {
                return GetValidationError("FinansstyringServiceUri");
            }
            private set
            {
                SetValidationError("FinansstyringServiceUri", value, "FinansstyringServiceUriValidationError");
            }
        }

        /// <summary>
        /// Label til antal bogføringslinjer, der skal hentes.
        /// </summary>
        public virtual string AntalBogføringslinjerLabel
        {
            get
            {
                return Resource.GetText(Text.NumberOfAccountingLinesToGet);
            }
        }

        /// <summary>
        /// Antal bogføringslinjer, der skal hentes.
        /// </summary>
        [CustomValidation(typeof(FinansstyringKonfigurationViewModel), "ValidateAntalBogføringslinjer")]
        public virtual int AntalBogføringslinjer
        {
            get
            {
                try
                {
                    return _finansstyringKonfigurationRepository.AntalBogføringslinjer;
                }
                catch (IntranetGuiExceptionBase ex)
                {
                    _exceptionHandlerViewModel.HandleException(ex);
                    return 0;
                }
                catch (Exception ex)
                {
                    _exceptionHandlerViewModel.HandleException(new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileGettingPropertyValue, "AntalBogføringslinjer", ex.Message), ex));
                    return 0;
                }
            }
            set
            {
                try
                {
                    var validationResult = ValidateAntalBogføringslinjer(value);
                    if (validationResult != ValidationResult.Success)
                    {
                        throw new IntranetGuiValidationException(validationResult.ErrorMessage, this, "AntalBogføringslinjer", value);
                    }
                    AntalBogføringslinjerValidationError = null;
                    if (_finansstyringKonfigurationRepository.AntalBogføringslinjer == value)
                    {
                        return;
                    }
                    _finansstyringKonfigurationRepository.KonfigurationerAdd(new Dictionary<string, object> {{"AntalBogføringslinjer", value}});
                    RaisePropertyChanged("AntalBogføringslinjer");
                }
                catch (IntranetGuiValidationException ex)
                {
                    AntalBogføringslinjerValidationError = ex.Message;
                    _exceptionHandlerViewModel.HandleException(ex);
                }
                catch (IntranetGuiExceptionBase ex)
                {
                    _exceptionHandlerViewModel.HandleException(ex);
                }
                catch (Exception ex)
                {
                    _exceptionHandlerViewModel.HandleException(new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingPropertyValue, "AntalBogføringslinjer", ex.Message), ex));
                }
            }
        }

        /// <summary>
        /// Valideringsfejl ved angivelse af antal bogføringslinjer, der skal hentes.
        /// </summary>
        public virtual string AntalBogføringslinjerValidationError
        {
            get
            {
                return GetValidationError("AntalBogføringslinjer");
            }
            private set
            {
                SetValidationError("AntalBogføringslinjer", value, "AntalBogføringslinjerValidationError");
            }
        }

        /// <summary>
        /// Label til antal dage, som nyheder er gældende.
        /// </summary>
        public virtual string DageForNyhederLabel
        {
            get
            {
                return Resource.GetText(Text.DaysForNews);
            }
        }

        /// <summary>
        /// Antal dage, som nyheder er gældende.
        /// </summary>
        [CustomValidation(typeof(FinansstyringKonfigurationViewModel), "ValidateDageForNyheder")]
        public virtual int DageForNyheder
        {
            get
            {
                try
                {
                    return _finansstyringKonfigurationRepository.DageForNyheder;
                }
                catch (IntranetGuiExceptionBase ex)
                {
                    _exceptionHandlerViewModel.HandleException(ex);
                    return 0;
                }
                catch (Exception ex)
                {
                    _exceptionHandlerViewModel.HandleException(new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileGettingPropertyValue, "DageForNyheder", ex.Message), ex));
                    return 0;
                }
            }
            set
            {
                try
                {
                    var validationResult = ValidateDageForNyheder(value);
                    if (validationResult != ValidationResult.Success)
                    {
                        throw new IntranetGuiValidationException(validationResult.ErrorMessage, this, "DageForNyheder", value);
                    }
                    DageForNyhederValidationError = null;
                    if (_finansstyringKonfigurationRepository.DageForNyheder == value)
                    {
                        return;
                    }
                    _finansstyringKonfigurationRepository.KonfigurationerAdd(new Dictionary<string, object> {{"DageForNyheder", value}});
                    RaisePropertyChanged("DageForNyheder");
                }
                catch (IntranetGuiValidationException ex)
                {
                    DageForNyhederValidationError = ex.Message;
                    _exceptionHandlerViewModel.HandleException(ex);
                }
                catch (IntranetGuiExceptionBase ex)
                {
                    _exceptionHandlerViewModel.HandleException(ex);
                }
                catch (Exception ex)
                {
                    _exceptionHandlerViewModel.HandleException(new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingPropertyValue, "DageForNyheder", ex.Message), ex));
                }
            }
        }

        /// <summary>
        /// Valideringsfejl ved angivelse af antal dage, som nyheder er gældende.
        /// </summary>
        public virtual string DageForNyhederValidationError
        {
            get
            {
                return GetValidationError("DageForNyheder");
            }
            private set
            {
                SetValidationError("DageForNyheder", value, "DageForNyhederValidationError");
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Nulstiller alle valideringsfejl.
        /// </summary>
        public override void ClearValidationErrors()
        {
            FinansstyringServiceUriValidationError = null;
            AntalBogføringslinjerValidationError = null;
            DageForNyhederValidationError = null;
        }

        /// <summary>
        /// Validerer værdien for uri til servicen, der supporterer finansstyring.
        /// </summary>
        /// <param name="value">Værdi, der skal valideres.</param>
        /// <returns>Valideringsresultat.</returns>
        public static ValidationResult ValidateFinansstyringServiceUri(string value)
        {
            return Validation.ValidateUri(value);
        }

        /// <summary>
        /// Validerer værdien for antal bogføringslinjer, der skal hentes.
        /// </summary>
        /// <param name="value">Værdi, der skal valideres.</param>
        /// <returns>Valideringsresultat.</returns>
        public static ValidationResult ValidateAntalBogføringslinjer(int value)
        {
            return Validation.ValidateInterval(value, 10, 250);
        }

        /// <summary>
        /// Validerer værdien for antal dage, som nyheder er gældende.
        /// </summary>
        /// <param name="value">Værdi, der skal valideres.</param>
        /// <returns>Valideringsresultat.</returns>
        public static ValidationResult ValidateDageForNyheder(int value)
        {
            return Validation.ValidateInterval(value, 0, 30);
        }

        #endregion
    }
}
