using System.Xml.Linq;
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
        /// Event, der rejses, når data i det lokale datalager skal forberedes for læsning og skrivning.
        /// </summary>
        event IntranetGuiEventHandler<IPrepareLocaleDataEventArgs> PrepareLocaleData; 

        /// <summary>
        /// Angivelse af, om der findes et lokalt datalager indeholdende data.
        /// </summary>
        bool HasLocaleData { get; }

        /// <summary>
        /// Filnavn indeholdende data i det lokale datalager.
        /// </summary>
        string LocaleDataFileName { get; }

        /// <summary>
        /// Filnavn indeholdende synkroniseringsdata i det lokale datalager.
        /// </summary>
        string SyncDataFileName { get; }

        /// <summary>
        /// XDocument, der inderholder XML skemaet, som kan benyttes til validering af de lokale data.
        /// </summary>
        XDocument Schema { get; }

        /// <summary>
        /// Henter data fra det lokale datalager.
        /// </summary>
        /// <returns>XDocument, der indeholder data fra det lokale datalager.</returns>
        XDocument GetLocaleData();

        /// <summary>
        /// Gemmer XML dokumentet indeholdende lokale data i det lokale datalager.
        /// </summary>
        /// <param name="localeDataDocument">XML dokumentet indeholdende lokale data.</param>
        void StoreLocaleDocument(XDocument localeDataDocument);

        /// <summary>
        /// Gemmer data i det lokale datalager.
        /// </summary>
        /// <typeparam name="T">Typen på data, der skal gemmes i det lokale datalager.</typeparam>
        /// <param name="model">Data, der skal gemmes i det lokale datalager.</param>
        void StoreLocaleData<T>(T model) where T : IModel;

        /// <summary>
        /// Gemmer XML dokumentet indeholdende lokale synkroniserede data i det lokale datalager.
        /// </summary>
        /// <param name="localeDataDocument">XML dokumentet indeholdende lokale synkroniserede data.</param>
        void StoreSyncDocument(XDocument localeDataDocument);

        /// <summary>
        /// Gemmer synkroniseringsdata i det lokale datalager.
        /// </summary>
        /// <typeparam name="T">Typen på synkroniseringsdata, der skal gemmes i det lokale datalager.</typeparam>
        /// <param name="model">Synkroniseringsdata, der skal gemmes i det lokale datalager.</param>
        void StoreSyncData<T>(T model) where T : IModel;
    }
}