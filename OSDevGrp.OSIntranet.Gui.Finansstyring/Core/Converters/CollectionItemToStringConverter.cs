using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Data;

namespace OSDevGrp.OSIntranet.Gui.Finansstyring.Core.Converters
{
    /// <summary>
    /// Converter, der kan konvertere et nummereret object i en collection til en streng.
    /// </summary>
    public class CollectionItemToStringConverter : IValueConverter
    {
        #region Methods

        /// <summary>
        /// Konverterer et nummereret object i en collection til en streng..
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
                var collection = value as IEnumerable<object>;
                if (collection == null)
                {
                    return "FEJL";
                }
                return "OK";
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Konverterer en streng til et nummereret object i en collection.
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
