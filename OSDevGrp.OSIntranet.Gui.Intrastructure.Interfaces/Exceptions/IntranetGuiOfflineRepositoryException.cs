using System;

namespace OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions
{
    /// <summary>
    /// Exception, der angiver, at et repository er offline i OS Intranet.
    /// </summary>
    public class IntranetGuiOfflineRepositoryException : IntranetGuiRepositoryException
    {
        #region Private variables

        private readonly object _repositoryContext;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner en exception, der angiver, at et repository er offline i OS Intranet.
        /// </summary>
        /// <param name="message">Fejlbesked.</param>
        /// <param name="repositoryContext">Instans af repositoryet, der er offline.</param>
        public IntranetGuiOfflineRepositoryException(string message, object repositoryContext)
            : this(message, repositoryContext, null)
        {
        }

        /// <summary>
        /// Danner en exception, der angiver, at et repository er offline i OS Intranet.
        /// </summary>
        /// <param name="message">Fejlbesked.</param>
        /// <param name="repositoryContext">Instans af repositoryet, der er offline.</param>
        /// <param name="innerException">Inner exception.</param>
        public IntranetGuiOfflineRepositoryException(string message, object repositoryContext, Exception innerException)
            : base(message, innerException)
        {
            if (repositoryContext == null)
            {
                throw new ArgumentNullException("repositoryContext");
            }
            _repositoryContext = repositoryContext;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Instans af repositoryet, der er offline.
        /// </summary>
        public object RepositoryContext
        {
            get
            {
                return _repositoryContext;
            }
        }

        #endregion
    }
}
