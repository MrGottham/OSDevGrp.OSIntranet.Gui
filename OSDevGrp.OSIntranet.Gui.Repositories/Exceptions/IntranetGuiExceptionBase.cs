using System;
using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Exceptions
{
    [Serializable]
    public abstract class IntranetGuiExceptionBase : Exception
    {
        #region Constructors

        protected IntranetGuiExceptionBase(string message)
            : base(message)
        {
        }

        protected IntranetGuiExceptionBase(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected IntranetGuiExceptionBase(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}