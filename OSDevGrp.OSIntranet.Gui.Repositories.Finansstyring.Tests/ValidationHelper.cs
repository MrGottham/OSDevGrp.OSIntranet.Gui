using System;
using System.Xml.Schema;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Finansstyring.Tests
{
    internal static class ValidationHelper
    {
        internal static void ValidationEventHandler(object sender, ValidationEventArgs eventArgs)
        {
            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            if (eventArgs == null)
            {
                throw new ArgumentNullException(nameof(eventArgs));
            }

            switch (eventArgs.Severity)
            {
                case XmlSeverityType.Warning:
                    throw new IntranetGuiRepositoryException(eventArgs.Message, eventArgs.Exception);

                case XmlSeverityType.Error:
                    throw new IntranetGuiRepositoryException(eventArgs.Message, eventArgs.Exception);
            }
        }
    }
}