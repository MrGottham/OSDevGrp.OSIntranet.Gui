using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Accounting.Models;
using OSDevGrp.OSIntranet.Gui.Repositories.Common;
using OSDevGrp.OSIntranet.Gui.Repositories.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Exceptions;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting.Enums;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting.Models;
using System.Xml;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Accounting
{
    internal static class AccountingOfflineDataElementExtensions
    {
        #region Methods

        internal static IAccountingIdentificationModel ToAccountingIdentificationModel(this XmlElement accountingElement)
        {
            NullGuard.NotNull(accountingElement, nameof(accountingElement));

            try
            {
                return new AccountingIdentificationModel(
                    accountingElement.GetIntegerFromRequiredAttribute("number"),
                    accountingElement.GetNonEmptyStringFromRequiredAttribute("name"));
            }
            catch (XmlException ex)
            {
                throw ex.ToException();
            }
        }

        internal static IAccountingModel ToAccountingModel(this XmlElement accountingElement, XmlNamespaceManager namespaceManager)
        {
            NullGuard.NotNull(accountingElement, nameof(accountingElement));

            try
            {
                int letterHeadIdentification = accountingElement.GetIntegerFromRequiredAttribute("letterHeadIdentification");
                XmlNode? letterHeadNode = accountingElement.ParentNode?.SelectLetterHeadNode(letterHeadIdentification, namespaceManager);
                if (letterHeadNode == null)
                {
                    throw new XmlException($"Unable to resolve the letter head for the given letter head identification: {letterHeadIdentification}");
                }

                return new AccountingModel(
                    accountingElement.GetIntegerFromRequiredAttribute("number"),
                    accountingElement.GetNonEmptyStringFromRequiredAttribute("name"),
                    ((XmlElement)letterHeadNode).ToLetterHeadIdentificationModel(),
                    accountingElement.GetEnumFromRequiredAttribute<BalanceBelowZeroType>("balanceBelowZero"),
                    accountingElement.GetIntegerFromRequiredAttribute("backDating"));
            }
            catch (XmlException ex)
            {
                throw ex.ToException();
            }
        }

        #endregion
    }
}