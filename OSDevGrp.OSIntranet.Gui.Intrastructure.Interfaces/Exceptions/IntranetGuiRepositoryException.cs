using System;

namespace OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions
{
    /// <summary>
    /// Repositoryexception fra OS Intranet.
    /// </summary>
    public class IntranetGuiRepositoryException : IntranetGuiExceptionBase
    {
        #region Constructors

        /// <summary>
        /// Danner repositoryexception fra OS Intranet.
        /// </summary>
        /// <param name="message">Fejlbesked.</param>
        public IntranetGuiRepositoryException(string message) 
            : base(message)
        {
        }

        /// <summary>
        /// Danner repositoryexception fra OS Intranet.
        /// </summary>
        /// <param name="message">Fejlbesked.</param>
        /// <param name="innerException">Inner exception.</param>
        public IntranetGuiRepositoryException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        #endregion
    }
}
