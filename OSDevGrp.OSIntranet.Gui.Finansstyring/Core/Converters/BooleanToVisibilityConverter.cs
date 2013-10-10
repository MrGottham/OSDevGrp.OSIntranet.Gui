using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace OSDevGrp.OSIntranet.Gui.Finansstyring.Core.Converters
{
    /// <summary>
    /// Converter, der kan konvertere en boolean til Visibility.
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        #region Methods

        /// <summary>
        /// Konverterer en boolean til Visibility.
        /// </summary>
        /// <param name="value">Værdi, der skal konverteres.</param>
        /// <param name="targetType">Typen, der skal konverteres til.</param>
        /// <param name="parameter">Parameter til konverteringen.</param>
        /// <param name="language">Sprog.</param>
        /// <returns>Konverteret værdi.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var valueAsBoolean = System.Convert.ToBoolean(value);
            return valueAsBoolean ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Konverterer Visibility til en boolean.
        /// </summary>
        /// <param name="value">Værdi, der skal konverteres.</param>
        /// <param name="targetType">Typen, der skal konverteres til.</param>
        /// <param name="parameter">Parameter til konverteringen.</param>
        /// <param name="language">Sprog.</param>
        /// <returns>Konverteret værdi.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var visibility = (Visibility) Enum.Parse(typeof (Visibility), value.ToString());
            switch (visibility)
            {
                case Visibility.Visible:
                    return true;

                case Visibility.Collapsed:
                    return false;

                default:
                    return false;
            }
        }

        #endregion
    }
}
