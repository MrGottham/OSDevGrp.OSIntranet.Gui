using OSDevGrp.OSIntranet.Core;
using System;
using System.Globalization;
using System.Xml;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Core
{
    internal static class OfflineDataElementExtensions
    {
        #region Methods

        internal static int GetIntegerFromRequiredAttribute(this XmlElement offlineDataElement, string attributeName)
        {
            NullGuard.NotNull(offlineDataElement, nameof(offlineDataElement))
                .NotNullOrWhiteSpace(attributeName, nameof(attributeName));

            XmlAttribute attribute = offlineDataElement.GetRequiredAttribute(attributeName);
            if (int.TryParse(attribute.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out int value))
            {
                return value;
            }

            throw new XmlException($"Unable to parse the value of the XML attribute named '{attribute.Name}' to an integer.");
        }

        internal static string GetNonEmptyStringFromRequiredAttribute(this XmlElement offlineDataElement, string attributeName)
        {
            NullGuard.NotNull(offlineDataElement, nameof(offlineDataElement))
                .NotNullOrWhiteSpace(attributeName, nameof(attributeName));

            XmlAttribute attribute = offlineDataElement.GetRequiredAttribute(attributeName);
            if (string.IsNullOrWhiteSpace(attribute.Value) == false)
            {
                return attribute.Value;
            }

            throw new XmlException($"The value of the XML attribute named '{attribute.Name}' cannot be null, empty or contain only white space.");
        }

        internal static TEnum GetEnumFromRequiredAttribute<TEnum>(this XmlElement offlineDataElement, string attributeName) where TEnum : struct, Enum
        {
            NullGuard.NotNull(offlineDataElement, nameof(offlineDataElement))
                .NotNullOrWhiteSpace(attributeName, nameof(attributeName));

            XmlAttribute attribute = offlineDataElement.GetRequiredAttribute(attributeName);
            if (Enum.TryParse(attribute.Value, out TEnum value))
            {
                return value;
            }

            throw new XmlException($"Unable to parse the value of the XML attribute named '{attribute.Name}' to a value of '{typeof(TEnum).Name}'.");
        }

        internal static XmlAttribute GetRequiredAttribute(this XmlElement offlineDataElement, string attributeName)
        {
            NullGuard.NotNull(offlineDataElement, nameof(offlineDataElement))
                .NotNullOrWhiteSpace(attributeName, nameof(attributeName));

            XmlAttribute attribute = offlineDataElement.Attributes[attributeName, string.Empty];
            if (attribute != null)
            {
                return attribute;
            }

            throw new XmlException($"The XML attribute named '{attributeName}' is missing on the XML element named '{offlineDataElement.Name}'.");
        }

        #endregion
    }
}