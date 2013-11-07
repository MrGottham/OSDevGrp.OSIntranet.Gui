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
    }
}
