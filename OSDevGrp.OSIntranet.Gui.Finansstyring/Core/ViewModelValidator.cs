using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces;

namespace OSDevGrp.OSIntranet.Gui.Finansstyring.Core
{
    /// <summary>
    /// Funktionalitet til validering af ViewModels.
    /// </summary>
    public static class ViewModelValidator
    {
        /// <summary>
        /// Validere en given property på en given ViewModel.
        /// </summary>
        /// <param name="viewModel">ViewModel, hvorpå property skal valideres.</param>
        /// <param name="propertyName">Navn på property, der skal valideres.</param>
        public static bool Validate(IViewModel viewModel, string propertyName)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException("viewModel");
            }
            if (propertyName == null)
            {
                throw new ArgumentNullException("propertyName");
            }
            var propertyInfo = viewModel.GetType().GetRuntimeProperty(propertyName);
            if (propertyInfo == null)
            {
                return true;
            }
            var validationAttributes = propertyInfo.GetCustomAttributes<ValidationAttribute>().ToList();
            if (validationAttributes == null || validationAttributes.Count == 0)
            {
                return true;
            }
            var validationContext = new ValidationContext(viewModel)
                {
                    MemberName = propertyName
                };
            var propertyValue = propertyInfo.GetValue(viewModel);
            foreach (var validationAttribute in validationAttributes)
            {
                var result = validationAttribute.GetValidationResult(propertyValue, validationContext);
                if (result == ValidationResult.Success)
                {
                    continue;
                }
                return false;
            }
            return true;
        }
    }
}
