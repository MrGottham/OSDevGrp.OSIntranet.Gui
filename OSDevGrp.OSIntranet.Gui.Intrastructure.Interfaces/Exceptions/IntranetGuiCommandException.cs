using System;

namespace OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions
{
    /// <summary>
    /// Kommandoexception fra OS Intranet.
    /// </summary>
    public class IntranetGuiCommandException : IntranetGuiBusinessException
    {
        #region Constructors

        /// <summary>
        /// Danner en kommandoexception fra OS Intranet.
        /// </summary>
        /// <param name="message">Fejlbesked.</param>
        /// <param name="reason">Årsag.</param>
        public IntranetGuiCommandException(string message, string reason)
            : base(message)
        {
        }

        /// <summary>
        /// Danner en  kommandoexception fra OS Intranet.
        /// </summary>
        /// <param name="message">Fejlbesked.</param>
        /// <param name="reason">Årsag.</param>
        /// <param name="innerException">Inner exception.</param>
        public IntranetGuiCommandException(string message, string reason, Exception innerException)
            : base(message, innerException)
        {
        }

        #endregion
    }
}
