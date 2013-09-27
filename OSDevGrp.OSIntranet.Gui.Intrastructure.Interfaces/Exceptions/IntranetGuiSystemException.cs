using System;

namespace OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions
{
    /// <summary>
    /// Systemexception fra OS Intranet.
    /// </summary>
    public class IntranetGuiSystemException : IntranetGuiExceptionBase
    {
        #region Constructors

        /// <summary>
        /// Danner systemexception fra OS Intranet.
        /// </summary>
        /// <param name="message">Fejlbesked.</param>
        public IntranetGuiSystemException(string message) 
            : base(message)
        {
        }

        /// <summary>
        /// Danner systemexception fra OS Intranet.
        /// </summary>
        /// <param name="message">Fejlbesked.</param>
        /// <param name="innerException">Inner exception.</param>
        public IntranetGuiSystemException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        #endregion
    }
}
