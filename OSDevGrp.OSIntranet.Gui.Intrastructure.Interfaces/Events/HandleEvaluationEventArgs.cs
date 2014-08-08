using System;

namespace OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Events
{
    /// <summary>
    /// Argumenter til et event, der håndterer en validering.
    /// </summary>
    public class HandleEvaluationEventArgs : EventArgs, IHandleEvaluationEventArgs
    {
        #region Constructor

        private readonly object _validationContext;
        private bool _result;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner argumenter til et event, der håndterer en validering.
        /// </summary>
        /// <param name="validationContext">Valideringskontekst.</param>
        public HandleEvaluationEventArgs(object validationContext) 
            : this(validationContext, false)
        {
        }

        /// <summary>
        /// Danner argumenter til et event, der håndterer en validering.
        /// </summary>
        /// <param name="validationContext">Valideringskontekst.</param>
        /// <param name="defaultResult">Initiering af default valideringsresultat.</param>
        public HandleEvaluationEventArgs(object validationContext, bool defaultResult)
        {
            if (validationContext == null)
            {
                throw new ArgumentNullException("validationContext");
            }
            _validationContext = validationContext;
            _result = defaultResult;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Valideringskontekst.
        /// </summary>
        public virtual object ValidationContext
        {
            get
            {
                return _validationContext;
            }
        }

        /// <summary>
        /// Resultat.
        /// </summary>
        public virtual bool Result
        {
            get
            {
                return _result;
            }
            set
            {
                _result = value;
            }
        }

        #endregion
    }
}
