using System;

namespace OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions
{
    /// <summary>
    /// Basisfunktionalitet for en exception til OS Intranet.
    /// </summary>
    public abstract class IntranetGuiExceptionBase : Exception
    {
        #region Constructors

        /// <summary>
        /// Danner basisfunktionalitet til en exception til OS Intranet.
        /// </summary>
        /// <param name="message">Fejlbesked.</param>
        protected IntranetGuiExceptionBase(string message) 
            : this(message, null)
        {
        }

        /// <summary>
        /// Danner basisfunktionalitet til en exception til OS Intranet.
        /// </summary>
        /// <param name="message">Fejlbesked.</param>
        /// <param name="innerException">Inner exception.</param>
        protected IntranetGuiExceptionBase(string message, Exception innerException) 
            : base(message, innerException)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException("message");
            }
        }

        #endregion
    }
}
