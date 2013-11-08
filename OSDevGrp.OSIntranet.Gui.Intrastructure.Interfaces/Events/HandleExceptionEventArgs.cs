using System;

namespace OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Events
{
    /// <summary>
    /// Argumenter til et event, der håndtere en exception i OS Intranet.
    /// </summary>
    public class HandleExceptionEventArgs : EventArgs, IHandleExceptionEventArgs
    {
        #region Private variables

        private readonly Exception _error;

        #endregion
        
        #region Constructor

        /// <summary>
        /// Danner argumenter til et event, der håndtere en exception i OS Intranet.
        /// </summary>
        /// <param name="error">Exception, der skal håndteres.</param>
        public HandleExceptionEventArgs(Exception error)
        {
            if (error == null)
            {
                throw new ArgumentNullException("error");
            }
            _error = error;
        }
        
        #endregion

        #region Properties
        
        /// <summary>
        /// Exception, der skal håndteres.
        /// </summary>
        public virtual Exception Error
        {
            get
            {
                return _error;
            }
        }

        /// <summary>
        /// Angivelse af, om exception er blevet håndteret.
        /// </summary>
        public virtual bool IsHandled
        {
            get; 
            set;
        }

        #endregion
    }
}
