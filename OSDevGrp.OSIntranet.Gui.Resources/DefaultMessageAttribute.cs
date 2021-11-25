using System;

namespace OSDevGrp.OSIntranet.Gui.Resources
{
    [AttributeUsage(AttributeTargets.Field)]
    internal class DefaultMessageAttribute : Attribute
    {
        #region Constructor

        internal DefaultMessageAttribute(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }

            Message = message;
        }

        #endregion

        #region Properties

        internal string Message { get; }

        #endregion
    }
}