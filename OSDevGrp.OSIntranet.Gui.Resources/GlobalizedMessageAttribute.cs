using System;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Gui.Resources
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    internal class GlobalizedMessageAttribute : DefaultMessageAttribute
    {
        #region Constructor

        internal GlobalizedMessageAttribute(string cultureInfoName, string message) : base(message)
        {
            if (string.IsNullOrWhiteSpace(cultureInfoName))
            {
                throw new ArgumentNullException(nameof(cultureInfoName));
            }

            CultureInfo = new CultureInfo(cultureInfoName);
        }

        #endregion

        #region Properties

        internal CultureInfo CultureInfo { get; }

        #endregion
    }
}