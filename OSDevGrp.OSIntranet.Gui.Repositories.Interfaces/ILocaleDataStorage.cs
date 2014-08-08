using System.Xml;
using System.Xml.Schema;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Events;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Interfaces
{
    /// <summary>
    /// Interface til et lokalt datalager.
    /// </summary>
    public interface ILocaleDataStorage
    {
        /// <summary>
        /// Event, der rejses, når det skal evalueres, om der findes et lokalt datalager indeholdende data.
        /// </summary>
        event IntranetGuiEventHandler<IHandleEvaluationEventArgs> OnHasLocaleData;

        /// <summary>
        /// Event, der rejses, når stream til det lokale datalager skal dannes.
        /// </summary>
        event IntranetGuiEventHandler<IHandleStreamCreationEventArgs> OnCreateStream; 
 
        /// <summary>
        /// Angivelse af, om der findes et lokalt datalager indeholdende data.
        /// </summary>
        bool HasLocaleData
        {
            get;
        }

        /// <summary>
        /// XML schema, der benyttes til validering af de lokale data.
        /// </summary>
        XmlSchema Schema
        {
            get;
        }

        /// <summary>
        /// Henter data fra det lokale datalager.
        /// </summary>
        /// <returns>XML reader, der kan læse data fra det lokale datalager.</returns>
        XmlReader GetLocaleData();
    }
}
