using System;
using Windows.UI.Xaml.Data;

namespace OSDevGrp.OSIntranet.Gui.Finansstyring.Core.Converters
{
    /// <summary>
    /// Converter, der kan konvertere en streng til en streng af store bogstaver.
    /// </summary>
    public class ToUpperCaseConverter : IValueConverter
    {
        #region Methods

        /// <summary>
        /// Konverterer en en streng til en streng af store bogstaver.
        /// </summary>
        /// <param name="value">Værdi, der skal konverteres.</param>
        /// <param name="targetType">Typen, der skal konverteres til.</param>
        /// <param name="parameter">Parameter til konverteringen.</param>
        /// <param name="language">Sprog.</param>
        /// <returns>Konverteret værdi.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var valueAsString = value as string;
            return string.IsNullOrEmpty(valueAsString) ? string.Empty : valueAsString.ToUpper();
        }

        /// <summary>
        /// Konverterer en en streng til en streng af store bogstaver.
        /// </summary>
        /// <param name="value">Værdi, der skal konverteres.</param>
        /// <param name="targetType">Typen, der skal konverteres til.</param>
        /// <param name="parameter">Parameter til konverteringen.</param>
        /// <param name="language">Sprog.</param>
        /// <returns>Konverteret værdi.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return Convert(value, targetType, parameter, language);
        }

        #endregion
    }
}
