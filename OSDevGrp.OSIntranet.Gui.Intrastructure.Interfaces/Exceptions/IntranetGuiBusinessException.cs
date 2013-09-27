using System;

namespace OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions
{
    /// <summary>
    /// Forretningsexception fra OS Intranet.
    /// </summary>
    public class IntranetGuiBusinessException : IntranetGuiExceptionBase
    {
        #region Constructors

        /// <summary>
        /// Danner forretningsexception fra OS Intranet.
        /// </summary>
        /// <param name="message">Fejlbesked.</param>
        public IntranetGuiBusinessException(string message) 
            : base(message)
        {
        }

        /// <summary>
        /// Danner forretningsexception fra OS Intranet.
        /// </summary>
        /// <param name="message">Fejlbesked.</param>
        /// <param name="innerException">Inner exception.</param>
        public IntranetGuiBusinessException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        #endregion
    }
}
