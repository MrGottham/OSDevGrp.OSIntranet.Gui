using System;
using System.ComponentModel.DataAnnotations;
using OSDevGrp.OSIntranet.Gui.Resources;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Core.Validators
{
    /// <summary>
    /// Basisfunktionalitet til validering.
    /// </summary>
    public static class Validation
    {
        /// <summary>
        /// Validerer, om der er angivet en værdi.
        /// </summary>
        /// <param name="value">Værdi, der skal være angivet.</param>
        /// <returns>Valideringsresultat.</returns>
        public static ValidationResult ValidateRequiredValue(string value)
        {
            return string.IsNullOrWhiteSpace(value) == false
                       ? ValidationResult.Success
                       : new ValidationResult(Resource.GetText(Text.ValueIsRequiered));
        }

        /// <summary>
        /// Validerer, om værdien er en URI adresse.
        /// </summary>
        /// <param name="value">Værdi, der skal valideres.</param>
        /// <returns>Valideringsresultat.</returns>
        public static ValidationResult ValidateUri(string value)
        {
            Uri uri;
            return Uri.TryCreate(value, UriKind.Absolute, out uri)
                       ? ValidationResult.Success
                       : new ValidationResult(Resource.GetText(Text.InvalidValueForUri, value));
        }

        /// <summary>
        /// Validerer, om værdien er i et givent interval.
        /// </summary>
        /// <param name="value">Værdien, der skal valideres.</param>
        /// <param name="min">Minimun for intervallet, hvori værdien skal være.</param>
        /// <param name="max">Maksimum for intervallet, hvori værdien skal være.</param>
        /// <returns>Valideringsresultat.</returns>
        public static ValidationResult ValidateInterval(int value, int min, int max)
        {
            if (value >= min && value <= max)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(Resource.GetText(Text.ValueOutsideInterval, min, max));
        }
    }
}
