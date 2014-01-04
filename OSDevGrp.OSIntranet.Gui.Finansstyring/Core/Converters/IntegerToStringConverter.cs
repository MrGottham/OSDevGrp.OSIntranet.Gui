using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace OSDevGrp.OSIntranet.Gui.Finansstyring.Core.Converters
{
    /// <summary>
    /// Converter, der kan konvertere en integer til en streng.
    /// </summary>
    public class IntegerToStringConverter : IValueConverter
    {
        #region Methods

        /// <summary>
        /// Konverterer en integer til en streng.
        /// </summary>
        /// <param name="value">Værdi, der skal konverteres.</param>
        /// <param name="targetType">Typen, der skal konverteres til.</param>
        /// <param name="parameter">Parameter til konverteringen.</param>
        /// <param name="language">Sprog.</param>
        /// <returns>Konverteret værdi.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                var valueAsInteger = System.Convert.ToInt32(value);
                return valueAsInteger.ToString(new CultureInfo(language));
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Konverterer en stregn til en integer.
        /// </summary>
        /// <param name="value">Værdi, der skal konverteres.</param>
        /// <param name="targetType">Typen, der skal konverteres til.</param>
        /// <param name="parameter">Parameter til konverteringen.</param>
        /// <param name="language">Sprog.</param>
        /// <returns>Konverteret værdi.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            try
            {
                var valueAsString = value as string;
                return string.IsNullOrEmpty(valueAsString) ? 0 : System.Convert.ToInt32(valueAsString);
            }
            catch
            {
                return 0;
            }
        }

        #endregion
    }
}
