using System;
using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Exceptions
{
    [Serializable]
    public class IntranetGuiUserFriendlyException : IntranetGuiExceptionBase
    {
        #region Constructors

        public IntranetGuiUserFriendlyException(string message)
            : base(message)
        {
        }

        public IntranetGuiUserFriendlyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected IntranetGuiUserFriendlyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}