using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
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
        /// <param name="value">Værdi, der skal valideres.</param>
        /// <param name="min">Minimum for intervallet, hvori værdien skal være.</param>
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

        /// <summary>
        /// Validerer, om værdien er en dato.
        /// </summary>
        /// <param name="value">Værdi, der skal valideres.</param>
        /// <returns>Valideringsresultat.</returns>
        public static ValidationResult ValidateDate(string value)
        {
            return ValidateDateLowerOrEqualTo(value, DateTime.MaxValue);
        }

        /// <summary>
        /// Validerer, om værdien er en dato, der er mindre end eller lig en anden dato.
        /// </summary>
        /// <param name="value">Værdi, der skal valideres.</param>
        /// <param name="maxDateTime">Datoen, som værdien skal være mindre end eller lig med.</param>
        /// <returns>Valideringsresutlat.</returns>
        public static ValidationResult ValidateDateLowerOrEqualTo(string value, DateTime maxDateTime)
        {
            DateTime dateTime;
            if (DateTime.TryParse(value, CultureInfo.CurrentUICulture, DateTimeStyles.AssumeLocal, out dateTime) == false)
            {
                return new ValidationResult(Resource.GetText(Text.ValueIsNotDate));
            }
            return dateTime.Date.CompareTo(maxDateTime.Date) > 0
                       ? new ValidationResult(Resource.GetText(Text.DateGreaterThan, maxDateTime.ToString("D", CultureInfo.CurrentUICulture)))
                       : ValidationResult.Success;
        }

        /// <summary>
        /// Validaterer, om værdien er et decimaltal.
        /// </summary>
        /// <param name="value">Værdi, der skal valideres.</param>
        /// <returns>Valideringsresultat.</returns>
        public static ValidationResult ValidateDecimal(string value)
        {
            return ValidateDecimalGreaterOrEqualTo(value, decimal.MinValue);
        }

        /// <summary>
        /// Validerer, om værdien er et decimaltal, der er større end eller lig et andet decimaltal.
        /// </summary>
        /// <param name="value">Værdi, der skal valideres.</param>
        /// <param name="minValue">Minimumsværdi, som værdien skal være større end eller lig med.</param>
        /// <returns>Valideringsresultat.</returns>
        public static ValidationResult ValidateDecimalGreaterOrEqualTo(string value, decimal minValue)
        {
            decimal valueAsDecimal;
            if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.CurrentUICulture, out valueAsDecimal) == false)
            {
                return new ValidationResult(Resource.GetText(Text.ValueIsNotDecimal));
            }
            return valueAsDecimal < minValue
                       ? new ValidationResult(Resource.GetText(Text.DecimalLowerThan, minValue.ToString("G", CultureInfo.CurrentUICulture)))
                       : ValidationResult.Success;
        }
    }
}
