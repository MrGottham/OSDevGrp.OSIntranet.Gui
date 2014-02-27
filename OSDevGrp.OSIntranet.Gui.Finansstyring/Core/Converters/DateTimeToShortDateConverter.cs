using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace OSDevGrp.OSIntranet.Gui.Finansstyring.Core.Converters
{
    /// <summary>
    /// Converter, der kan konvertere en DateTime til en dato i kort format.
    /// </summary>
    public class DateTimeToShortDateConverter : IValueConverter
    {
        #region Methods

        /// <summary>
        /// Konverterer en DateTime til en dato i kort format.
        /// </summary>
        /// <param name="value">Værdi, der skal konverteres.</param>
        /// <param name="targetType">Typen, der skal konverteres til.</param>
        /// <param name="parameter">Parameter til konverteringen.</param>
        /// <param name="language">Sprog.</param>
        /// <returns>Konverteret værdi.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return string.Empty;
            }
            try
            {
                var cultureInfo = string.IsNullOrEmpty(language) ? new CultureInfo(Windows.Globalization.Language.CurrentInputMethodLanguageTag) : new CultureInfo(language);
                var dateTime = System.Convert.ToDateTime(value);
                return dateTime.ToString("d", cultureInfo);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Konverterer en dato i kort format til DateTime.
        /// </summary>
        /// <param name="value">Værdi, der skal konverteres.</param>
        /// <param name="targetType">Typen, der skal konverteres til.</param>
        /// <param name="parameter">Parameter til konverteringen.</param>
        /// <param name="language">Sprog.</param>
        /// <returns>Konverteret værdi.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
