using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Events;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.Resources;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Finansstyring
{
    /// <summary>
    /// Lokalt datalager.
    /// </summary>
    public class LocaleDataStorage : ILocaleDataStorage
    {
        #region Private variables

        private readonly string _localeDataFileName;
        private readonly string _syncDataFileName;
        private static readonly object SyncRoot = new object();

        #endregion

        #region Constructor

        /// <summary>
        /// Danner det lokale datalager.
        /// </summary>
        /// <param name="localeDataFileName">Filnavn indeholdende data i det lokale datalager.</param>
        /// <param name="syncDataFileName">Filnavn indeholdende synkroniseringsdata i det lokale datalager.</param>
        /// <param name="schemaLocation">Lokation for XML schema, der benyttes til validering af de lokale data.</param>
        public LocaleDataStorage(string localeDataFileName, string syncDataFileName, string schemaLocation)
        {
            if (string.IsNullOrWhiteSpace(localeDataFileName))
            {
                throw new ArgumentNullException(nameof(localeDataFileName));
            }

            if (string.IsNullOrWhiteSpace(syncDataFileName))
            {
                throw new ArgumentNullException(nameof(syncDataFileName));
            }

            if (string.IsNullOrWhiteSpace(schemaLocation))
            {
                throw new ArgumentNullException(nameof(schemaLocation));
            }

            _localeDataFileName = localeDataFileName;
            _syncDataFileName = syncDataFileName;

            using (MemoryStream memoryStream = new MemoryStream(Resource.GetEmbeddedResource(schemaLocation)))
            {
                XmlReaderSettings readerSettings = new XmlReaderSettings
                {
                    IgnoreComments = true,
                    IgnoreProcessingInstructions = true,
                    IgnoreWhitespace = true
                };
                using (XmlReader reader = XmlReader.Create(memoryStream, readerSettings))
                {
                    Schema = XDocument.Load(reader);
                }
            }
        }

        #endregion

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

        /// <summary>
        /// Event, der rejses, når data i det lokale datalager skal forberedes for læsning og skrivning.
        /// </summary>
        public virtual event IntranetGuiEventHandler<IPrepareLocaleDataEventArgs> PrepareLocaleData;

        #endregion

        #region Properties

        /// <summary>
        /// Angivelse af, om der findes et lokalt datalager indeholdende data.
        /// </summary>
        public virtual bool HasLocaleData
        {
            get
            {
                lock (SyncRoot)
                {
                    if (OnHasLocaleData == null)
                    {
                        return false;
                    }

                    HandleEvaluationEventArgs handleEvaluationEventArgs = new HandleEvaluationEventArgs(this);
                    OnHasLocaleData.Invoke(this, handleEvaluationEventArgs);

                    return handleEvaluationEventArgs.Result;
                }
            }
        }

        /// <summary>
        /// Filnavn indeholdende data i det lokale datalager.
        /// </summary>
        public virtual string LocaleDataFileName => Path.GetFileName(_localeDataFileName);

        /// <summary>
        /// Filnavn indeholdende synkroniseringsdata i det lokale datalager.
        /// </summary>
        public virtual string SyncDataFileName => Path.GetFileName(_syncDataFileName);

        /// <summary>
        /// XML reader, der kan læse XML schema, som kan benyttes til validering af de lokale data.
        /// </summary>
        public virtual XDocument Schema { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Henter data fra det lokale datalager.
        /// </summary>
        /// <returns>XDocument, der indeholder data fra det lokale datalager.</returns>
        public virtual XDocument GetLocaleData()
        {
            if (OnCreateReaderStream == null)
            {
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.EventHandlerNotDefined, "OnCreateReaderStream"));
            }

            try
            {
                lock (SyncRoot)
                {
                    HandleStreamCreationEventArgs handleStreamCreationEventArgs = new HandleStreamCreationEventArgs(this);
                    OnCreateReaderStream.Invoke(this, handleStreamCreationEventArgs);
                    using (Stream readerStream = handleStreamCreationEventArgs.Result)
                    {
                        if (readerStream == null)
                        {
                            throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "readerStream", null));
                        }

                        XDocument localeDataDocument = ReadDocument(Schema, readerStream);
                        PrepareLocaleData?.Invoke(this, new PrepareLocaleDataEventArgs(localeDataDocument, true, false, false));

                        return localeDataDocument;
                    }
                }
            }
            catch (IntranetGuiRepositoryException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "GetLocaleData", ex.Message), ex);
            }
        }

        /// <summary>
        /// Gemmer XML dokumentet indeholdende lokale data i det lokale datalager.
        /// </summary>
        /// <param name="localeDataDocument">XML dokumentet indeholdende lokale data.</param>
        public virtual void StoreLocaleDocument(XDocument localeDataDocument)
        {
            if (localeDataDocument == null)
            {
                throw new ArgumentNullException(nameof(localeDataDocument));
            }

            if (OnCreateWriterStream == null)
            {
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.EventHandlerNotDefined, "OnCreateWriterStream"));
            }

            try
            {
                lock (SyncRoot)
                {
                    HandleStreamCreationEventArgs handleStreamCreationEventArgs = new HandleStreamCreationEventArgs(false);
                    OnCreateWriterStream.Invoke(this, handleStreamCreationEventArgs);

                    using (Stream writerStream = handleStreamCreationEventArgs.Result)
                    {
                        PrepareLocaleData?.Invoke(this, new PrepareLocaleDataEventArgs(localeDataDocument, false, true, false));

                        WriteDocument(Schema, localeDataDocument, writerStream);
                    }
                }
            }
            catch (IntranetGuiRepositoryException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "StoreLocaleDocument", ex.Message), ex);
            }
        }

        /// <summary>
        /// Gemmer data i det lokale datalager.
        /// </summary>
        /// <typeparam name="T">Typen på data, der skal gemmes i det lokale datalager.</typeparam>
        /// <param name="model">Data, der skal gemmes i det lokale datalager.</param>
        public virtual void StoreLocaleData<T>(T model) where T : IModel
        {
            if (Equals(model, null))
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (OnCreateWriterStream == null)
            {
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.EventHandlerNotDefined, "OnCreateWriterStream"));
            }

            try
            {
                lock (SyncRoot)
                {
                    HandleStreamCreationEventArgs handleStreamCreationEventArgs = new HandleStreamCreationEventArgs(false);
                    OnCreateWriterStream.Invoke(this, handleStreamCreationEventArgs);

                    using (Stream writerStream = handleStreamCreationEventArgs.Result)
                    {
                        XDocument localeDataDocument = writerStream.Length == 0
                            ? InitializeLocaleDataDocument(Schema)
                            : ReadDocument(Schema, writerStream);

                        StoreInDocument(model, localeDataDocument, false);
                        PrepareLocaleData?.Invoke(this, new PrepareLocaleDataEventArgs(localeDataDocument, false, true, false));

                        WriteDocument(Schema, localeDataDocument, writerStream);
                    }
                }
            }
            catch (IntranetGuiRepositoryException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "StoreLocaleData", ex.Message), ex);
            }
        }

        /// <summary>
        /// Gemmer XML dokumentet indeholdende lokale synkroniserede data i det lokale datalager.
        /// </summary>
        /// <param name="localeDataDocument">XML dokumentet indeholdende lokale synkroniserede data.</param>
        public virtual void StoreSyncDocument(XDocument localeDataDocument)
        {
            if (localeDataDocument == null)
            {
                throw new ArgumentNullException(nameof(localeDataDocument));
            }

            if (OnCreateWriterStream == null)
            {
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.EventHandlerNotDefined, "OnCreateWriterStream"));
            }

            try
            {
                lock (SyncRoot)
                {
                    HandleStreamCreationEventArgs handleStreamCreationEventArgs = new HandleStreamCreationEventArgs(true);
                    OnCreateWriterStream.Invoke(this, handleStreamCreationEventArgs);

                    using (Stream writerStream = handleStreamCreationEventArgs.Result)
                    {
                        PrepareLocaleData?.Invoke(this, new PrepareLocaleDataEventArgs(localeDataDocument, false, true, true));

                        WriteDocument(Schema, localeDataDocument, writerStream);
                    }
                }
            }
            catch (IntranetGuiRepositoryException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "StoreSyncDocument", ex.Message), ex);
            }
        }

        /// <summary>
        /// Gemmer synkroniseringsdata i det lokale datalager.
        /// </summary>
        /// <typeparam name="T">Typen på synkroniseringsdata, der skal gemmes i det lokale datalager.</typeparam>
        /// <param name="model">Synkroniseringsdata, der skal gemmes i det lokale datalager.</param>
        public virtual void StoreSyncData<T>(T model) where T : IModel
        {
            if (Equals(model, null))
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (OnCreateWriterStream == null)
            {
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.EventHandlerNotDefined, "OnCreateWriterStream"));
            }

            try
            {
                lock (SyncRoot)
                {
                    HandleStreamCreationEventArgs handleStreamCreationEventArgs = new HandleStreamCreationEventArgs(true);
                    OnCreateWriterStream.Invoke(this, handleStreamCreationEventArgs);

                    using (Stream writerStream = handleStreamCreationEventArgs.Result)
                    {
                        XDocument localeDataDocument = writerStream.Length == 0
                            ? InitializeLocaleDataDocument(Schema)
                            : ReadDocument(Schema, writerStream);

                        StoreInDocument(model, localeDataDocument, true);
                        PrepareLocaleData?.Invoke(this, new PrepareLocaleDataEventArgs(localeDataDocument, false, true, true));

                        WriteDocument(Schema, localeDataDocument, writerStream);
                    }
                }
            }
            catch (IntranetGuiRepositoryException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "StoreSyncData", ex.Message), ex);
            }
        }

        private static XDocument InitializeLocaleDataDocument(XDocument localeDataSchema)
        {
            if (localeDataSchema == null)
            {
                throw new ArgumentNullException(nameof(localeDataSchema));
            }

            if (localeDataSchema.Root == null)
            {
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.LocaleDataSchemaIsInvalid));
            }

            string localeDataNamespace = localeDataSchema.Root.GetDefaultNamespace().NamespaceName;
            string localeDataRootName = localeDataSchema.Root.Element(XName.Get("element", "http://www.w3.org/2001/XMLSchema"))?.Attribute(XName.Get("name", string.Empty))?.Value;
            if (string.IsNullOrWhiteSpace(localeDataRootName))
            {
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.LocaleDataSchemaIsInvalid));
            }

            XDocument localeDataDocument = new XDocument(new XDeclaration("1.0", "utf-8", null));
            localeDataDocument.Add(new XElement(XName.Get(localeDataRootName, localeDataNamespace)));

            return localeDataDocument;
        }

        private static XDocument ReadDocument(XDocument localeDataSchema, Stream stream)
        {
            if (localeDataSchema == null)
            {
                throw new ArgumentNullException(nameof(localeDataSchema));
            }

            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            XmlReaderSettings readerSettings = new XmlReaderSettings
            {
                IgnoreComments = true,
                IgnoreProcessingInstructions = true,
                IgnoreWhitespace = true
            };
            using (XmlReader reader = XmlReader.Create(stream, readerSettings))
            {
                return XDocument.Load(reader);
            }
        }

        private static void WriteDocument(XDocument localeDataSchema, XDocument localeDataDocument, Stream stream)
        {
            if (localeDataSchema == null)
            {
                throw new ArgumentNullException(nameof(localeDataSchema));
            }

            if (localeDataDocument == null)
            {
                throw new ArgumentNullException(nameof(localeDataDocument));
            }

            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            stream.Seek(0, SeekOrigin.Begin);
            stream.SetLength(0);

            XmlWriterSettings writerSettings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                Indent = true,
            };
            using (XmlWriter writer = XmlWriter.Create(stream, writerSettings))
            {
                localeDataDocument.WriteTo(writer);
                writer.Flush();
            }
        }

        private static void StoreInDocument(IModel model, XDocument document, bool isStoringSynchronizedData)
        {
            if (Equals(model, null))
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            IRegnskabModel regnskabModel = model as IRegnskabModel;
            if (regnskabModel != null)
            {
                regnskabModel.StoreInDocument(document);
                return;
            }

            IKontoModel kontoModel = model as IKontoModel;
            if (kontoModel != null)
            {
                kontoModel.StoreInDocument(document);
                return;
            }

            IBudgetkontoModel budgetkontoModel = model as IBudgetkontoModel;
            if (budgetkontoModel != null)
            {
                budgetkontoModel.StoreInDocument(document);
                return;
            }

            IAdressekontoModel adressekontoModel = model as IAdressekontoModel;
            if (adressekontoModel != null)
            {
                adressekontoModel.StoreInDocument(document);
                return;
            }

            IBogføringslinjeModel bogføringslinjeModel = model as IBogføringslinjeModel;
            if (bogføringslinjeModel != null)
            {
                bogføringslinjeModel.StoreInDocument(document, isStoringSynchronizedData);
                return;
            }

            IKontogruppeModel kontogruppeModel = model as IKontogruppeModel;
            if (kontogruppeModel != null)
            {
                kontogruppeModel.StoreInDocument(document);
                return;
            }

            IBudgetkontogruppeModel budgetkontogruppeModel = model as IBudgetkontogruppeModel;
            budgetkontogruppeModel?.StoreInDocument(document);
        }

        #endregion
    }
}