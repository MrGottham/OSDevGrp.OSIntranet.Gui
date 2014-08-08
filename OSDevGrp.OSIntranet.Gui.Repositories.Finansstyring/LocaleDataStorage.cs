using System;
using System.Xml;
using System.Xml.Schema;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Events;
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
        /// Event, der rejses, når stream til det lokale datalager skal dannes.
        /// </summary>
        public virtual event IntranetGuiEventHandler<IHandleStreamCreationEventArgs> OnCreateStream; 

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
        public XmlReader GetLocaleData()
        {
            if (OnCreateStream != null)
            {
                
            }
            throw new NotImplementedException();
        }

        #endregion
    }
}
