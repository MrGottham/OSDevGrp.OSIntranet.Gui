using System;
using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Exceptions
{
    [Serializable]
    public class IntranetGuiOfflineException : IntranetGuiSystemException
    {
        #region Constructors

        public IntranetGuiOfflineException(string message)
            : base(message)
        {
        }

        public IntranetGuiOfflineException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected IntranetGuiOfflineException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}