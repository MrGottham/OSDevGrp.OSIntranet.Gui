using System;
using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Exceptions
{
    [Serializable]
    public class IntranetGuiSystemException : IntranetGuiExceptionBase
    {
        #region Constructors

        public IntranetGuiSystemException(string message)
            : base(message)
        {
        }

        public IntranetGuiSystemException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected IntranetGuiSystemException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}