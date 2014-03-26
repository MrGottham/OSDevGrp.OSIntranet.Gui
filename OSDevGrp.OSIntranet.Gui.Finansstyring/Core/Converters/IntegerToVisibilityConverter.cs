using System;

namespace OSDevGrp.OSIntranet.Gui.Finansstyring.Core.Converters
{
    /// <summary>
    /// Konverterer en integer til Visibility. 
    /// </summary>
    public class IntegerToVisibilityConverter : BooleanToVisibilityConverter
    {
        #region Methods

        /// <summary>
        /// Konverterer en integer til Visibility.
        /// </summary>
        /// <param name="value">Værdi, der skal konverteres.</param>
        /// <param name="targetType">Typen, der skal konverteres til.</param>
        /// <param name="parameter">Parameter til konverteringen.</param>
        /// <param name="language">Sprog.</param>
        /// <returns>Konverteret værdi.</returns>
        public override object Convert(object value, Type targetType, object parameter, string language)
        {
            var valueAsInteger = System.Convert.ToInt32(value);
            return base.Convert(valueAsInteger > 0, targetType, parameter, language);
        }

        /// <summary>
        /// Konverterer Visibility til en integer.
        /// </summary>
        /// <param name="value">Værdi, der skal konverteres.</param>
        /// <param name="targetType">Typen, der skal konverteres til.</param>
        /// <param name="parameter">Parameter til konverteringen.</param>
        /// <param name="language">Sprog.</param>
        /// <returns>Konverteret værdi.</returns>
        public override object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
