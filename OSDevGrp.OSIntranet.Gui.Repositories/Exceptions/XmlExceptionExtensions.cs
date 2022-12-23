using OSDevGrp.OSIntranet.Core;
using System;
using System.Xml;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Exceptions
{
    internal static class XmlExceptionExtensions
    {
        #region Methods

        internal static Exception ToException(this XmlException xmlException)
        {
            NullGuard.NotNull(xmlException, nameof(xmlException));

            return new IntranetGuiSystemException(xmlException.Message, xmlException);
        }

        #endregion
    }
}