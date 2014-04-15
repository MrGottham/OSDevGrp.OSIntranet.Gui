using System;

namespace OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions
{
    /// <summary>
    /// Kommandoexception fra OS Intranet.
    /// </summary>
    public class IntranetGuiCommandException : IntranetGuiBusinessException
    {
        #region Private variables

        private readonly string _reason;
        private readonly object _commandContext;
        private readonly object _reasonContext;

        #endregion

        #region Constructors

        /// <summary>
        /// Danner en kommandoexception fra OS Intranet.
        /// </summary>
        /// <param name="message">Fejlbesked.</param>
        /// <param name="reason">Årsag.</param>
        /// <param name="commandContext">Instans af kommandoen, hvorfra kommandoexception er kastet.</param>
        /// <param name="reasonContext">Instans af objektet, som var årsagen til, at kommandoexception blev kastet.</param>
        public IntranetGuiCommandException(string message, string reason, object commandContext, object reasonContext)
            : this(message, reason, commandContext, reasonContext, null)
        {
        }

        /// <summary>
        /// Danner en  kommandoexception fra OS Intranet.
        /// </summary>
        /// <param name="message">Fejlbesked.</param>
        /// <param name="reason">Årsag.</param>
        /// <param name="commandContext">Instans af kommandoen, hvorfra kommandoexception er kastet.</param>
        /// <param name="reasonContext">Instans af objektet, som var årsagen til, at kommandoexception blev kastet.</param>
        /// <param name="innerException">Inner exception.</param>
        public IntranetGuiCommandException(string message, string reason, object commandContext, object reasonContext, Exception innerException)
            : base(message, innerException)
        {
            if (string.IsNullOrEmpty(reason))
            {
                throw new ArgumentNullException("reason");
            }
            if (commandContext == null)
            {
                throw new ArgumentNullException("commandContext");
            }
            if (reasonContext == null)
            {
                throw new ArgumentNullException("reasonContext");
            }
            _reason = reason;
            _commandContext = commandContext;
            _reasonContext = reasonContext;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Årsag.
        /// </summary>
        public virtual string Reason
        {
            get
            {
                return _reason;
            }
        }

        /// <summary>
        /// Instans af kommandoen, hvorfra denne kommandoexception er kastet.
        /// </summary>
        public virtual object CommandContext
        {
            get
            {
                return _commandContext;
            }
        }

        /// <summary>
        /// Instans af objektet, som var årsagen til, at denne kommandoexception blev kastet.
        /// </summary>
        public virtual object ReasonContext
        {
            get
            {
                return _reasonContext;
            }
        }

        #endregion
    }
}
