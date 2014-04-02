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

        #endregion
    }
}
