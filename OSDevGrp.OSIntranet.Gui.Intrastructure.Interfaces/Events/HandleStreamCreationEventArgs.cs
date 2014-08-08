using System;
using System.IO;

namespace OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Events
{
    /// <summary>
    /// Argumenter til et event, der håndterer oprettelse af en stream.
    /// </summary>
    public class HandleStreamCreationEventArgs : EventArgs, IHandleStreamCreationEventArgs
    {
        #region Private variables

        private readonly object _creationContext;
        private Stream _stream;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner argumenter til et event, der håndterer oprettelse af en stream.
        /// </summary>
        /// <param name="creationContext">Kontekst, hvorfra stream skal oprettes.</param>
        public HandleStreamCreationEventArgs(object creationContext)
        {
            if (creationContext == null)
            {
                throw new ArgumentNullException("creationContext");
            }
            _creationContext = creationContext;
            _stream = null;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Kontekst, hvorfra stream skal oprettes.
        /// </summary>
        public virtual object CreationContext
        {
            get
            {
                return _creationContext;
            }
        }

        /// <summary>
        /// Stream, der er blevet oprettet.
        /// </summary>
        public virtual Stream Result
        {
            get
            {
                return _stream;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                _stream = value;
            }
        }

        #endregion
    }
}
