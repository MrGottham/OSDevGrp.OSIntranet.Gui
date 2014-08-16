using System;
using System.IO;
using System.Reflection;
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
        private readonly string _schemaLocation;
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
            if (string.IsNullOrEmpty(localeDataFileName))
            {
                throw new ArgumentNullException("localeDataFileName");
            }
            if (string.IsNullOrEmpty(syncDataFileName))
            {
                throw new ArgumentNullException("syncDataFileName");
            }
            if (string.IsNullOrEmpty(schemaLocation))
            {
                throw new ArgumentNullException("schemaLocation");
            }
            _localeDataFileName = localeDataFileName;
            _syncDataFileName = syncDataFileName;
            _schemaLocation = schemaLocation;
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
                    var handleEvaluationEventArgs = new HandleEvaluationEventArgs(this);
                    OnHasLocaleData.Invoke(this, handleEvaluationEventArgs);
                    return handleEvaluationEventArgs.Result;
                }
            }
        }

        /// <summary>
        /// Filnavn indeholdende data i det lokale datalager.
        /// </summary>
        public virtual string LocaleDataFileName
        {
            get
            {
                return Path.GetFileName(_localeDataFileName);
            }
        }

        /// <summary>
        /// Filnavn indeholdende synkroniseringsdata i det lokale datalager.
        /// </summary>
        public virtual string SyncDataFileName
        {
            get
            {
                return Path.GetFileName(_syncDataFileName);
            }
        }

        /// <summary>
        /// XML reader, der kan læse XML schema, som kan benyttes til validering af de lokale data.
        /// </summary>
        public virtual XDocument Schema
        {
            get
            {
                var assembly = GetType().GetTypeInfo().Assembly;
                using (var resourceStream = assembly.GetManifestResourceStream(string.Format("{0}.{1}", assembly.GetName().Name, _schemaLocation)))
                {
                    if (resourceStream == null)
                    {
                        throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.UnableToLoadResource, _schemaLocation));
                    }
                    return ReadDocument(resourceStream);
                }
            }
        }

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
                    var handleStreamCreationEventArgs = new HandleStreamCreationEventArgs(this);
                    OnCreateReaderStream.Invoke(this, handleStreamCreationEventArgs);
                    using (var readerStream = handleStreamCreationEventArgs.Result)
                    {
                        return (ReadDocument(readerStream));
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
        /// Gemmer data i det lokale datalager.
        /// </summary>
        /// <typeparam name="T">Typen på data, der skal gemmes i det lokale datalager.</typeparam>
        /// <param name="model">Data, der skal gemmes i det lokale datalager.</param>
        public virtual void StoreLocaleData<T>(T model) where T : IModel
        {
            if (Equals(model, null))
            {
                throw new ArgumentNullException("model");
            }
            if (OnCreateWriterStream == null)
            {
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.EventHandlerNotDefined, "OnCreateWriterStream"));
            }
            try
            {
                lock (SyncRoot)
                {
                    var handleStreamCreationEventArgs = new HandleStreamCreationEventArgs(false);
                    OnCreateWriterStream.Invoke(this, handleStreamCreationEventArgs);
                    using (var writerStream = handleStreamCreationEventArgs.Result)
                    {
                        XDocument localeDataDocument;
                        if (writerStream.Length == 0)
                        {
                            var localeDataSchema = Schema;
                            var localeDataNamespace = localeDataSchema.Root.GetDefaultNamespace().NamespaceName;
                            var localeDataRootName = localeDataSchema.Root.Element(XName.Get("element", "http://www.w3.org/2001/XMLSchema")).Attribute(XName.Get("name", string.Empty)).Value;
                            localeDataDocument = new XDocument(new XDeclaration("1.0", "utf-8", null));
                            localeDataDocument.Add(new XElement(XName.Get(localeDataRootName, localeDataNamespace)));
                        }
                        else
                        {
                            localeDataDocument = ReadDocument(writerStream);
                        }
                        StoreInDocument(model, localeDataDocument);
                        WriteDocument(localeDataDocument, writerStream);
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
        /// Gemmer synkroniseringsdata i det lokale datalager.
        /// </summary>
        /// <typeparam name="T">Typen på synkroniseringsdata, der skal gemmes i det lokale datalager.</typeparam>
        /// <param name="model">Synkroniseringsdata, der skal gemmes i det lokale datalager.</param>
        public virtual void StoreSyncData<T>(T model) where T : IModel
        {
            if (Equals(model, null))
            {
                throw new ArgumentNullException("model");
            }
            if (OnCreateWriterStream == null)
            {
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.EventHandlerNotDefined, "OnCreateWriterStream"));
            }
            try
            {
                lock (SyncRoot)
                {
                    var handleStreamCreationEventArgs = new HandleStreamCreationEventArgs(true);
                    OnCreateWriterStream.Invoke(this, handleStreamCreationEventArgs);
                    using (var writerStream = handleStreamCreationEventArgs.Result)
                    {
                        XDocument localeDataDocument;
                        if (writerStream.Length == 0)
                        {
                            var localeDataSchema = Schema;
                            var localeDataNamespace = localeDataSchema.Root.GetDefaultNamespace().NamespaceName;
                            var localeDataRootName = localeDataSchema.Root.Element(XName.Get("element", "http://www.w3.org/2001/XMLSchema")).Attribute(XName.Get("name", string.Empty)).Value;
                            localeDataDocument = new XDocument(new XDeclaration("1.0", "utf-8", null));
                            localeDataDocument.Add(new XElement(XName.Get(localeDataRootName, localeDataNamespace)));
                        }
                        else
                        {
                            localeDataDocument = ReadDocument(writerStream);
                        }
                        StoreInDocument(model, localeDataDocument);
                        WriteDocument(localeDataDocument, writerStream);
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

        /// <summary>
        /// Indlæser en given stream i et XDocument.
        /// </summary>
        /// <param name="stream">Stream, der skal indlæses i et XDocument.</param>
        /// <returns>XDocument indeholdende data fra det givne stream.</returns>
        private static XDocument ReadDocument(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            var readerSettings = new XmlReaderSettings
            {
                IgnoreComments = true,
                IgnoreProcessingInstructions = true,
                IgnoreWhitespace = true
            };
            using (var reader = XmlReader.Create(stream, readerSettings))
            {
                return XDocument.Load(reader);
            }
        }

        /// <summary>
        /// Skriver et XDocument til en given stream.
        /// </summary>
        /// <param name="document">XDocument, der skal skrives.</param>
        /// <param name="stream">Stream, der skal skrive indholdet af det given XDocument.</param>
        private static void WriteDocument(XDocument document, Stream stream)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            stream.Seek(0, SeekOrigin.Begin);
            var writerSettings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                Indent = true
            };
            using (var writer = XmlWriter.Create(stream, writerSettings))
            {
                document.WriteTo(writer);
                writer.Flush();
            }
        }

        /// <summary>
        /// Gemmer data i et XDocument.
        /// </summary>
        /// <param name="model">Data, der skal gemmes i XDocument.</param>
        /// <param name="document">XDocument, hvori data skal gemmes.</param>
        private static void StoreInDocument(IModel model, XDocument document)
        {
            if (Equals(model, null))
            {
                throw new ArgumentNullException("model");
            }
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }
            var regnskabModel = model as IRegnskabModel;
            if (regnskabModel != null)
            {
                regnskabModel.StoreInDocument(document);
                return;
            }
            var kontoModel = model as IKontoModel;
            if (kontoModel != null)
            {
                kontoModel.StoreInDocument(document);
                return;
            }
            var kontogruppeModel = model as IKontogruppeModel;
            if (kontogruppeModel != null)
            {
                kontogruppeModel.StoreInDocument(document);
                return;
            }
        }

        #endregion
    }
}
