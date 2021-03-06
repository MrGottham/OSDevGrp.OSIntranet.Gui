﻿using System;
using System.Collections;

namespace OSDevGrp.OSIntranet.Gui.Finansstyring.Core.Converters
{
    /// <summary>
    /// Konverterer et objekt til Visibility. 
    /// </summary>
    public class ObjectToVisibilityConverter : BooleanToVisibilityConverter
    {
        #region Methods

        /// <summary>
        /// Konverterer et objekt til Visibility.
        /// </summary>
        /// <param name="value">Værdi, der skal konverteres.</param>
        /// <param name="targetType">Typen, der skal konverteres til.</param>
        /// <param name="parameter">Parameter til konverteringen.</param>
        /// <param name="language">Sprog.</param>
        /// <returns>Konverteret værdi.</returns>
        public override object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return base.Convert(false, targetType, parameter, language);
            }
            var enumerable = value as IEnumerable;
            if (enumerable != null)
            {
                var enumerator = enumerable.GetEnumerator();
                return base.Convert(enumerator.Current != null || enumerator.MoveNext(), targetType, parameter, language);
            }
            return base.Convert(true, targetType, parameter, language);
        }

        /// <summary>
        /// Konverterer Visibility til et objekt.
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
