using OSDevGrp.OSIntranet.Core;
using System;
using System.Xml.Schema;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Exceptions
{
    internal static class XmlSchemaValidationExceptionExtensions
    {
        #region Methods

        internal static Exception ToException(this XmlSchemaValidationException xmlSchemaValidationException)
        {
            NullGuard.NotNull(xmlSchemaValidationException, nameof(xmlSchemaValidationException));

            return new IntranetGuiUserFriendlyException(xmlSchemaValidationException.Message, xmlSchemaValidationException);
        }

        #endregion
    }
}