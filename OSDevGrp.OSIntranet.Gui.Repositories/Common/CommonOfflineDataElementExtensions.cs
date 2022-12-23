using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Common.Models;
using OSDevGrp.OSIntranet.Gui.Repositories.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Exceptions;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Common.Models;
using System.Xml;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Common
{
    internal static class CommonOfflineDataElementExtensions
    {
        #region Methods

        internal static ILetterHeadIdentificationModel ToLetterHeadIdentificationModel(this XmlElement letterHeadElement)
        {
            NullGuard.NotNull(letterHeadElement, nameof(letterHeadElement));

            try
            {
                return new LetterHeadIdentificationModel(
                    letterHeadElement.GetIntegerFromRequiredAttribute("number"),
                    letterHeadElement.GetNonEmptyStringFromRequiredAttribute("name"));
            }
            catch (XmlException ex)
            {
                throw ex.ToException();
            }
        }

        #endregion
    }
}