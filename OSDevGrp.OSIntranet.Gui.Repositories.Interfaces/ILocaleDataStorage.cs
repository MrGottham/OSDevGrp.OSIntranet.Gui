using System.Xml;
using System.Xml.Schema;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Events;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces;

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
        /// Event, der rejses, når en læsestream til det lokale datalager skal dannes.
        /// </summary>
        event IntranetGuiEventHandler<IHandleStreamCreationEventArgs> OnCreateReaderStream;

        /// <summary>
        /// Event, der rejses, når en skrivestream til det lokale datalager skal dannes.
        /// </summary>
        event IntranetGuiEventHandler<IHandleStreamCreationEventArgs> OnCreateWriterStream;

        /// <summary>
        /// Angivelse af, om der findes et lokalt datalager indeholdende data.
        /// </summary>
        bool HasLocaleData
        {
            get;
        }

        /// <summary>
        /// Filnavn indeholdende data i det lokale datalager.
        /// </summary>
        string LocaleDataFileName
        {
            get;
        }

        /// <summary>
        /// Filnavn indeholdende synkroniseringsdata i det lokale datalager.
        /// </summary>
        string SyncDataFileName
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

        /// <summary>
        /// Gemmer data i det lokale datalager.
        /// </summary>
        /// <typeparam name="T">Typen på data, der skal gemmes i det lokale datalager.</typeparam>
        /// <param name="model">Data, der skal gemmes i det lokale datalager.</param>
        void StoreLocaleData<T>(T model) where T : IModel;

        /// <summary>
        /// Gemmer synkroniseringsdata i det lokale datalager.
        /// </summary>
        /// <typeparam name="T">Typen på synkroniseringsdata, der skal gemmes i det lokale datalager.</typeparam>
        /// <param name="model">Synkroniseringsdata, der skal gemmes i det lokale datalager.</param>
        void StoreSyncData<T>(T model) where T : IModel;
    }
}
