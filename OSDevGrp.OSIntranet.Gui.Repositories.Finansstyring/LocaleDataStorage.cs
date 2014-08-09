using System;
using System.Xml;
using System.Xml.Schema;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Events;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Finansstyring
{
    /// <summary>
    /// Lokalt datalager.
    /// </summary>
    public class LocaleDataStorage : ILocaleDataStorage
    {
        #region Events

        /// <summary>
        /// Event, der rejses, når det skal evalueres, om der findes et lokalt datalager indeholdende data.
        /// </summary>
        public virtual event IntranetGuiEventHandler<IHandleEvaluationEventArgs> OnHasLocaleData;

        /// <summary>
        /// Event, der rejses, når læsestream til det lokale datalager skal dannes.
        /// </summary>
        public virtual event IntranetGuiEventHandler<IHandleStreamCreationEventArgs> OnCreateReaderStream;

        /// <summary>
        /// Event, der rejses, når skrivetream til det lokale datalager skal dannes.
        /// </summary>
        public virtual event IntranetGuiEventHandler<IHandleStreamCreationEventArgs> OnCreateWriterStream;

        #endregion

        #region Properties

        /// <summary>
        /// Angivelse af, om der findes et lokalt datalager indeholdende data.
        /// </summary>
        public virtual bool HasLocaleData
        {
            get
            {

                if (OnHasLocaleData != null)
                {
                    
                }
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Filnavn indeholdende data i det lokale datalager.
        /// </summary>
        public virtual string LocaleDataFileName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Filnavn indeholdende synkroniseringsdata i det lokale datalager.
        /// </summary>
        public virtual string SyncDataFileName
        {
            get
            {
                throw new NotImplementedException();
            }
        }


        /// <summary>
        /// XML schema, der benyttes til validering af de lokale data.
        /// </summary>
        public virtual XmlSchema Schema
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Henter data fra det lokale datalager.
        /// </summary>
        /// <returns>XML reader, der kan læse data fra det lokale datalager.</returns>
        public virtual XmlReader GetLocaleData()
        {
            if (OnCreateReaderStream != null)
            {
                
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gemmer data i det lokale datalager.
        /// </summary>
        /// <typeparam name="T">Typen på data, der skal gemmes i det lokale datalager.</typeparam>
        /// <param name="model">Data, der skal gemmes i det lokale datalager.</param>
        public virtual void StoreLocaleData<T>(T model) where T : IModel
        {
            if (OnCreateWriterStream != null)
            {
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gemmer synkroniseringsdata i det lokale datalager.
        /// </summary>
        /// <typeparam name="T">Typen på synkroniseringsdata, der skal gemmes i det lokale datalager.</typeparam>
        /// <param name="model">Synkroniseringsdata, der skal gemmes i det lokale datalager.</param>
        public virtual void StoreSyncData<T>(T model) where T : IModel
        {
            if (OnCreateWriterStream != null)
            {
            }
            throw new NotImplementedException();
        }

        #endregion
    }
}
