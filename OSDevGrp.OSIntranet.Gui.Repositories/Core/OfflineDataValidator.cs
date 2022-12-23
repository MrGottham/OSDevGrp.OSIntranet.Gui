using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting.Enums;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core;
using System;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Core
{
    internal static class OfflineDataValidator
    {
        #region Private varibales

        private static XmlSchemaSet? _offlineDataSchemaSet;

        #endregion

        #region Methods

        internal static void Validate(XmlDocument offlineDataDocument)
        {
            NullGuard.NotNull(offlineDataDocument, nameof(offlineDataDocument));

            _offlineDataSchemaSet ??= BuildOfflineDataSchemaSet();

            foreach (XmlSchema schema in _offlineDataSchemaSet.Schemas())
            {
                if (offlineDataDocument.Schemas.Contains(schema))
                {
                    continue;
                }

                offlineDataDocument.Schemas.Add(schema);
            }

            offlineDataDocument.Validate(ValidationEventHandler);
        }

        private static XmlSchemaSet BuildOfflineDataSchemaSet()
        {
            XmlNamespaceManager namespaceManager = OfflineDataNamespaceManagerFactory.Build();

            XmlSchemaSet offlineDataSchemaSet = new XmlSchemaSet(namespaceManager.NameTable);
            offlineDataSchemaSet.Add(BuildOfflineDataSchema(namespaceManager));

            offlineDataSchemaSet.ValidationEventHandler += ValidationEventHandler;
            offlineDataSchemaSet.Compile();

            return offlineDataSchemaSet;
        }

        private static XmlSchema BuildOfflineDataSchema(XmlNamespaceManager namespaceManager)
        {
            NullGuard.NotNull(namespaceManager, nameof(namespaceManager));

            XmlSchema offlineDataSchema = new XmlSchema
            {
                TargetNamespace = namespaceManager.DefaultNamespace,
                ElementFormDefault = XmlSchemaForm.Qualified
            };

            offlineDataSchema.Items.Add(BuildAccountingIdentificationSimpleType());
            offlineDataSchema.Items.Add(BuildAccountingNameSimpleType());
            offlineDataSchema.Items.Add(BuildBalanceBelowZeroSimpleType());
            offlineDataSchema.Items.Add(BuildBackDatingSimpleType());
            offlineDataSchema.Items.Add(BuildAccountingComplexType(offlineDataSchema.TargetNamespace));
            offlineDataSchema.Items.Add(BuildLetterHeadIdentificationSimpleType());
            offlineDataSchema.Items.Add(BuildLetterHeadNameSimpleType());
            offlineDataSchema.Items.Add(BuildLetterHeadComplexType(offlineDataSchema.TargetNamespace));
            offlineDataSchema.Items.Add(BuildOfflineDataComplexType(offlineDataSchema.TargetNamespace));

            XmlSchemaElement rootElement = new XmlSchemaElement
            {
                Name = "OfflineData",
                SchemaTypeName = new XmlQualifiedName("OfflineDataType", offlineDataSchema.TargetNamespace)
            };
            offlineDataSchema.Items.Add(rootElement);

            return offlineDataSchema;
        }

        private static XmlSchemaSimpleType BuildAccountingIdentificationSimpleType()
        {
            return BuildIntegerSimpleType("AccountingIdentificationType", 1, 99);
        }

        private static XmlSchemaSimpleType BuildAccountingNameSimpleType()
        {
            return BuildStringSimpleType("AccountingNameType", 1, 256);
        }

        private static XmlSchemaSimpleType BuildBalanceBelowZeroSimpleType()
        {
            return BuildStringSimpleType("BalanceBelowZeroType", typeof(BalanceBelowZeroType));
        }

        private static XmlSchemaSimpleType BuildBackDatingSimpleType()
        {
            return BuildIntegerSimpleType("BackDatingType", 0, 365);
        }

        private static XmlSchemaComplexType BuildAccountingComplexType(string targetNamespace)
        {
            NullGuard.NotNullOrWhiteSpace(targetNamespace, nameof(targetNamespace));

            XmlSchemaComplexType accountingComplexType = new XmlSchemaComplexType
            {
                Name = "AccountingType"
            };

            XmlSchemaAttribute numberAttribute = new XmlSchemaAttribute
            {
                Name = "number",
                SchemaTypeName = new XmlQualifiedName("AccountingIdentificationType", targetNamespace),
                Use = XmlSchemaUse.Required
            };
            accountingComplexType.Attributes.Add(numberAttribute);

            XmlSchemaAttribute nameAttribute = new XmlSchemaAttribute
            {
                Name = "name",
                SchemaTypeName = new XmlQualifiedName("AccountingNameType", targetNamespace),
                Use = XmlSchemaUse.Required
            };
            accountingComplexType.Attributes.Add(nameAttribute);

            XmlSchemaAttribute letterHeadIdentificationAttribute = new XmlSchemaAttribute
            {
                Name = "letterHeadIdentification",
                SchemaTypeName = new XmlQualifiedName("LetterHeadIdentificationType", targetNamespace),
                Use = XmlSchemaUse.Required
            };
            accountingComplexType.Attributes.Add(letterHeadIdentificationAttribute);

            XmlSchemaAttribute balanceBelowZeroAttribute = new XmlSchemaAttribute
            {
                Name = "balanceBelowZero",
                SchemaTypeName = new XmlQualifiedName("BalanceBelowZeroType", targetNamespace),
                Use = XmlSchemaUse.Required
            };
            accountingComplexType.Attributes.Add(balanceBelowZeroAttribute);

            XmlSchemaAttribute backDatingAttribute = new XmlSchemaAttribute
            {
                Name = "backDating",
                SchemaTypeName = new XmlQualifiedName("BackDatingType", targetNamespace),
                Use = XmlSchemaUse.Required
            };
            accountingComplexType.Attributes.Add(backDatingAttribute);

            return accountingComplexType;
        }

        private static XmlSchemaSimpleType BuildLetterHeadIdentificationSimpleType()
        {
            return BuildIntegerSimpleType("LetterHeadIdentificationType", 1, 99);
        }

        private static XmlSchemaSimpleType BuildLetterHeadNameSimpleType()
        {
            return BuildStringSimpleType("LetterHeadNameType", 1, 256);
        }

        private static XmlSchemaComplexType BuildLetterHeadComplexType(string targetNamespace)
        {
            NullGuard.NotNullOrWhiteSpace(targetNamespace, nameof(targetNamespace));

            XmlSchemaComplexType letterHeadComplexType = new XmlSchemaComplexType
            {
                Name = "LetterHeadType"
            };

            XmlSchemaAttribute numberAttribute = new XmlSchemaAttribute
            {
                Name = "number",
                SchemaTypeName = new XmlQualifiedName("LetterHeadIdentificationType", targetNamespace),
                Use = XmlSchemaUse.Required
            };
            letterHeadComplexType.Attributes.Add(numberAttribute);

            XmlSchemaAttribute nameAttribute = new XmlSchemaAttribute
            {
                Name = "name",
                SchemaTypeName = new XmlQualifiedName("LetterHeadNameType", targetNamespace),
                Use = XmlSchemaUse.Required
            };
            letterHeadComplexType.Attributes.Add(nameAttribute);

            return letterHeadComplexType;
        }

        private static XmlSchemaComplexType BuildOfflineDataComplexType(string targetNamespace)
        {
            NullGuard.NotNullOrWhiteSpace(targetNamespace, nameof(targetNamespace));

            XmlSchemaComplexType offlineDataComplexType = new XmlSchemaComplexType
            {
                Name = "OfflineDataType"
            };

            XmlSchemaChoice choice = new XmlSchemaChoice
            {
                MinOccurs = 0,
                MaxOccursString = "unbounded"
            };

            XmlSchemaElement accountingElement = new XmlSchemaElement
            {
                Name = "Accounting",
                SchemaTypeName = new XmlQualifiedName("AccountingType", targetNamespace)
            };
            choice.Items.Add(accountingElement);

            XmlSchemaElement letterHeadElement = new XmlSchemaElement
            {
                Name = "LetterHead",
                SchemaTypeName = new XmlQualifiedName("LetterHeadType", targetNamespace)
            };
            choice.Items.Add(letterHeadElement);

            offlineDataComplexType.Particle = choice;

            return offlineDataComplexType;
        }

        private static XmlSchemaSimpleType BuildIntegerSimpleType(string typeName, int minValue, int maxValue)
        {
            NullGuard.NotNullOrWhiteSpace(typeName, nameof(typeName));

            XmlSchemaSimpleType integerSimpleType = new XmlSchemaSimpleType
            {
                Name = typeName
            };

            XmlSchemaSimpleTypeRestriction restriction = new XmlSchemaSimpleTypeRestriction
            {
                BaseTypeName = new XmlQualifiedName("integer", "http://www.w3.org/2001/XMLSchema")
            };
            restriction.Facets.Add(new XmlSchemaMinInclusiveFacet { Value = minValue.ToString(CultureInfo.InvariantCulture) });
            restriction.Facets.Add(new XmlSchemaMaxInclusiveFacet { Value = maxValue.ToString(CultureInfo.InvariantCulture) });

            integerSimpleType.Content = restriction;

            return integerSimpleType;
        }

        private static XmlSchemaSimpleType BuildStringSimpleType(string typeName, int minLength, int maxLength)
        {
            NullGuard.NotNullOrWhiteSpace(typeName, nameof(typeName));

            XmlSchemaSimpleType integerSimpleType = new XmlSchemaSimpleType
            {
                Name = typeName
            };

            XmlSchemaSimpleTypeRestriction restriction = new XmlSchemaSimpleTypeRestriction
            {
                BaseTypeName = new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema")
            };
            restriction.Facets.Add(new XmlSchemaMinLengthFacet { Value = minLength.ToString(CultureInfo.InvariantCulture) });
            restriction.Facets.Add(new XmlSchemaMaxLengthFacet { Value = maxLength.ToString(CultureInfo.InvariantCulture) });

            integerSimpleType.Content = restriction;

            return integerSimpleType;
        }

        private static XmlSchemaSimpleType BuildStringSimpleType(string typeName, Type enumType)
        {
            NullGuard.NotNullOrWhiteSpace(typeName, nameof(typeName))
                .NotNull(enumType, nameof(enumType));

            XmlSchemaSimpleType integerSimpleType = new XmlSchemaSimpleType
            {
                Name = typeName
            };

            XmlSchemaSimpleTypeRestriction restriction = new XmlSchemaSimpleTypeRestriction
            {
                BaseTypeName = new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema")
            };
            foreach (string name in Enum.GetNames(enumType))
            {
                restriction.Facets.Add(new XmlSchemaEnumerationFacet { Value = name });
            }

            integerSimpleType.Content = restriction;

            return integerSimpleType;
        }

        private static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Warning)
            {
                throw e.Exception;
            }

            if (e.Severity == XmlSeverityType.Error)
            {
                throw e.Exception;
            }
        }

        #endregion
    }
}