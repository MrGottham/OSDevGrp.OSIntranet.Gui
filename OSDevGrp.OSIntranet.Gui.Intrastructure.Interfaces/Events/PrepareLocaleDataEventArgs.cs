using System;
using System.Xml.Linq;

namespace OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Events
{
    /// <summary>
    /// Argumenter til et event, der forbereder data i det lokale datalager for læsning og skrivning.
    /// </summary>
    public class PrepareLocaleDataEventArgs : EventArgs, IPrepareLocaleDataEventArgs
    {
        #region Private variables

        private readonly XDocument _localeDataDocument;
        private readonly bool _readingContext;
        private readonly bool _writingContext;
        private readonly bool _synchronizationContext;

        #endregion

        #region Constructors

        /// <summary>
        /// Danner argumenter til et event, der forbereder data i det lokale datalager for læsning og skrivning.
        /// </summary>
        /// <param name="localeDataDocument">XML dokument indeholdende data i det lokale datalager.</param>
        /// <param name="readingContext">Angivelse af, om data skal forberedes for læsning.</param>
        /// <param name="writingContext">Angivelse af, om data skal forberedes for skrivning.</param>
        /// <param name="synchronizationContext">SynchronizationContext</param>
        public PrepareLocaleDataEventArgs(XDocument localeDataDocument, bool readingContext, bool writingContext, bool synchronizationContext)
        {
            if (localeDataDocument == null)
            {
                throw new ArgumentNullException("localeDataDocument");
            }
            _localeDataDocument = localeDataDocument;
            _readingContext = readingContext;
            _writingContext = writingContext;
            _synchronizationContext = synchronizationContext;
        }

        #endregion

        #region Properties

        /// <summary>
        /// XML dokument indeholdende data i det lokale datalager.
        /// </summary>
        public virtual XDocument LocaleDataDocument
        {
            get
            {
                return _localeDataDocument;
            }
        }

        /// <summary>
        /// Angivelse af, om data skal forberedes for læsning.
        /// </summary>
        public virtual bool ReadingContext
        {
            get
            {
                return _readingContext;
            }
        }

        /// <summary>
        /// Angivelse af, om data skal forberedes for skrivning.
        /// </summary>
        public virtual bool WritingContext
        {
            get
            {
                return _writingContext;
            }
        }

        /// <summary>
        /// Angivelse af, om data skal forberedes for synkronisering.
        /// </summary>
        public virtual bool SynchronizationContext
        {
            get
            {
                return _synchronizationContext;
            }
        }

        #endregion
    }
}
