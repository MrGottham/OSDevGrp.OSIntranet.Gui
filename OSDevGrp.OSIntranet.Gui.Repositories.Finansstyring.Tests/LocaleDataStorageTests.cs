using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.Resources;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Finansstyring.Tests
{
    /// <summary>
    /// Tester det lokale datalager.
    /// </summary>
    [TestFixture]
    public class LocaleDataStorageTests
    {
        #region Private constants

        private const string DecimalFormat = "#0.00";

        #endregion

        /// <summary>
        /// Tester, at konstruktøren initierer det lokale datalager.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererLocaleDataStorage()
        {
            string localeDataFileName = Path.GetFileName(Path.GetTempFileName());
            string syncDataFileName = Path.GetFileName(Path.GetTempFileName());
            ILocaleDataStorage localeDataStorage = new LocaleDataStorage(localeDataFileName, syncDataFileName, FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);
            Assert.That(localeDataStorage.HasLocaleData, Is.False);
            Assert.That(localeDataStorage.LocaleDataFileName, Is.Not.Null);
            Assert.That(localeDataStorage.LocaleDataFileName, Is.Not.Empty);
            Assert.That(localeDataStorage.LocaleDataFileName, Is.EqualTo(localeDataFileName));
            Assert.That(localeDataStorage.SyncDataFileName, Is.Not.Null);
            Assert.That(localeDataStorage.SyncDataFileName, Is.Not.Empty);
            Assert.That(localeDataStorage.SyncDataFileName, Is.EqualTo(syncDataFileName));
            Assert.That(localeDataStorage.Schema, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis filnavnet indeholdende data i det lokale datalager er null.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void TestAtConstructorKasterArgumentNullExceptionHvisLocaleDataFileNameErInvalid(string invalidValue)
        {
            Fixture fixture = new Fixture();

            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new LocaleDataStorage(invalidValue, fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema));
            // ReSharper restore ObjectCreationAsStatement
            Assert.That(exception, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("localeDataFileName"));
            Assert.That(exception.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis indeholdende synkroniseringsdata i det lokale datalager er null.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void TestAtConstructorKasterArgumentNullExceptionHvisSyncDataFileNameErInvalid(string invalidValue)
        {
            Fixture fixture = new Fixture();

            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new LocaleDataStorage(fixture.Create<string>(), invalidValue, FinansstyringRepositoryLocale.XmlSchema));
            // ReSharper restore ObjectCreationAsStatement
            Assert.That(exception, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("syncDataFileName"));
            Assert.That(exception.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis lokation for XML schema, der benyttes til validering af de lokale data, er null.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void TestAtConstructorKasterArgumentNullExceptionHvisSchemaLocationErInvalid(string invalidValue)
        {
            Fixture fixture = new Fixture();

            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), invalidValue));
            // ReSharper restore ObjectCreationAsStatement
            Assert.That(exception, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("schemaLocation"));
            Assert.That(exception.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en IntranetGuiSystemException, hvis skemaet ikke findes.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterIntranetGuiSystemExceptionHvisSchemaLocationIkkeFindes()
        {
            Fixture fixture = new Fixture();

            string resourceName = fixture.Create<string>();
            // ReSharper disable ObjectCreationAsStatement
            IntranetGuiSystemException exception = Assert.Throws<IntranetGuiSystemException>(() => new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), resourceName));
            // ReSharper restore ObjectCreationAsStatement
            Assert.That(exception, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.UnableToLoadResource, $"OSDevGrp.OSIntranet.Gui.Resources.{resourceName}")));
            Assert.That(exception.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Tester, at HasLocaleData returnerer false, der rejses, når det skal evalueres, om der findes et lokalt datalager indeholdende data, er null.
        /// </summary>
        [Test]
        public void TestAtHasLocaleDataReturnererFalseHvisOnHasLocaleDataErNull()
        {
            Fixture fixture = new Fixture();

            ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);
            Assert.That(localeDataStorage.HasLocaleData, Is.False);
        }

        /// <summary>
        /// Tester, at HasLocaleData rejser eventet, der skal evaluere, om der findes et lokalt datalager indeholdende data.
        /// </summary>
        [Test]
        public void TestAtHasLocaleDataRejserEvent()
        {
            Fixture fixture = new Fixture();

            ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            bool eventCalled = false;
            localeDataStorage.OnHasLocaleData += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(s, Is.TypeOf<LocaleDataStorage>());
                Assert.That(s, Is.EqualTo(localeDataStorage));
                Assert.That(e, Is.Not.Null);
                Assert.That(e.ValidationContext, Is.Not.Null);
                Assert.That(e.ValidationContext, Is.TypeOf<LocaleDataStorage>());
                Assert.That(e.ValidationContext, Is.EqualTo(localeDataStorage));
                Assert.That(e.Result, Is.False);
                eventCalled = true;
            };

            Assert.That(eventCalled, Is.False);
            Assert.That(localeDataStorage.HasLocaleData, Is.False);
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at HasLocaleData returnerer resultatet fra eventhandleren, der rejses, når det skal evalueres, om der findes et lokalt datalager indeholdende data.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestAtHasLocaleDataReturnererResultFraEventHandler(bool result)
        {
            Fixture fixture = new Fixture();

            ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            localeDataStorage.OnHasLocaleData += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                e.Result = result;
            };

            Assert.That(localeDataStorage.HasLocaleData, Is.EqualTo(result));
        }

        /// <summary>
        /// Tester, at LocaleDataFileName returnerer filnavn unden mappenavn.
        /// </summary>
        [Test]
        public void TestAtLocaleDataFileNameReturnererFileNameUdenDirectoryName()
        {
            Fixture fixture = new Fixture();

            string localeDataFileName = Path.GetTempFileName();
            ILocaleDataStorage localeDataStorage = new LocaleDataStorage(localeDataFileName, fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);
            Assert.That(localeDataStorage.LocaleDataFileName, Is.Not.Null);
            Assert.That(localeDataStorage.LocaleDataFileName, Is.Not.Empty);
            Assert.That(localeDataStorage.LocaleDataFileName.Contains(Path.DirectorySeparatorChar), Is.False);
            Assert.That(localeDataStorage.LocaleDataFileName, Is.EqualTo(Path.GetFileName(localeDataFileName)));
        }

        /// <summary>
        /// Tester, at SyncDataFileName returnerer filnavn unden mappenavn.
        /// </summary>
        [Test]
        public void TestAtSyncDataFileNameReturnererFileNameUdenDirectoryName()
        {
            Fixture fixture = new Fixture();

            string syncDataFileName = Path.GetTempFileName();
            ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), syncDataFileName, FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);
            Assert.That(localeDataStorage.SyncDataFileName, Is.Not.Null);
            Assert.That(localeDataStorage.SyncDataFileName, Is.Not.Empty);
            Assert.That(localeDataStorage.SyncDataFileName.Contains(Path.DirectorySeparatorChar), Is.False);
            Assert.That(localeDataStorage.SyncDataFileName, Is.EqualTo(Path.GetFileName(syncDataFileName)));
        }

        /// <summary>
        /// Tester, at Schema giver en XML reader indeholdende XML schema til validering af data i det lokale datalager. 
        /// </summary>
        [Test]
        public void TestAtSchemaGiverXmlReaderIndeholdendeXmlSchema()
        {
            Fixture fixture = new Fixture();

            ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            XDocument schemaDocument = localeDataStorage.Schema;
            Assert.That(schemaDocument, Is.Not.Null);

            using (XmlReader reader = schemaDocument.CreateReader())
            {
                XmlSchema schema = XmlSchema.Read(reader, ValidationHelper.ValidationEventHandler);
                Assert.That(schema, Is.Not.Null);

                reader.Close();
            }
        }

        /// <summary>
        /// Tester, at GetLocaleData kaster en IntranetGuiRepositoryException, hvis eventet, der rejses, når en læsestream til det lokale datalager skal dannes, er null.
        /// </summary>
        [Test]
        public void TestAtGetLocaleDataKasterIntranetGuiRepositoryExceptionHvisOnOnCreateReaderStreamErNull()
        {
            Fixture fixture = new Fixture();

            ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            IntranetGuiRepositoryException exception = Assert.Throws<IntranetGuiRepositoryException>(() => localeDataStorage.GetLocaleData());
            Assert.That(exception, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.EventHandlerNotDefined, "OnCreateReaderStream")));
            Assert.That(exception.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Tester, at GetLocaleData rejser eventet, der skal danne læsestreamen til det lokale datalager.
        /// </summary>
        [Test]
        public void TestAtGetLocaleDataRejserOnCreateReaderStreamEvent()
        {
            Fixture fixture = new Fixture();

            ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            bool eventCalled = false;
            localeDataStorage.OnCreateReaderStream += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(s, Is.TypeOf<LocaleDataStorage>());
                Assert.That(s, Is.EqualTo(localeDataStorage));
                Assert.That(e, Is.Not.Null);
                Assert.That(e.CreationContext, Is.Not.Null);
                Assert.That(e.CreationContext, Is.TypeOf<LocaleDataStorage>());
                Assert.That(e.CreationContext, Is.EqualTo(localeDataStorage));
                Assert.That(e.Result, Is.Null);
                e.Result = CreateMemoryStreamWithXmlContent();
                eventCalled = true;
            };

            Assert.That(eventCalled, Is.False);
            Assert.That(localeDataStorage.GetLocaleData(), Is.Not.Null);
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at GetLocaleData returnerer et XDocument indeholdende lokale data.
        /// </summary>
        [Test]
        public void TestAtGetLocaleDataReturnererXDocumentMedLokaleData()
        {
            Fixture fixture = new Fixture();

            ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            localeDataStorage.OnCreateReaderStream += (s, e) => e.Result = CreateMemoryStreamWithXmlContent();

            XmlSchema schema;
            using (XmlReader schemaReader = localeDataStorage.Schema.CreateReader())
            {
                schema = XmlSchema.Read(schemaReader, ValidationHelper.ValidationEventHandler);
                schemaReader.Close();
            }
            Assert.That(schema, Is.Not.Null);

            XmlDocument localeData = new XmlDocument();
            using (XmlReader localeDataReader = localeDataStorage.GetLocaleData().CreateReader())
            {
                XmlReaderSettings readerSettings = new XmlReaderSettings
                {
                    IgnoreComments = true,
                    IgnoreProcessingInstructions = true,
                    IgnoreWhitespace = true,
                    ValidationType = ValidationType.Schema
                };
                readerSettings.Schemas.Add(schema);
                readerSettings.ValidationEventHandler += ValidationHelper.ValidationEventHandler;
                using (XmlReader reader = XmlReader.Create(localeDataReader, readerSettings))
                {
                    localeData.Load(reader);
                    reader.Close();
                }
                localeDataReader.Close();
            }
        }

        /// <summary>
        /// Tester, at GetLocaleData rejser eventet, der skal forberede data i det lokale datalager for læsning eller skrivning.
        /// </summary>
        [Test]
        public void TestAtGetLocaleDataRejserPrepareLocaleDataEvent()
        {
            Fixture fixture = new Fixture();

            ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            localeDataStorage.OnCreateReaderStream += (s, e) => e.Result = CreateMemoryStreamWithXmlContent();

            bool eventCalled = false;
            localeDataStorage.PrepareLocaleData += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(s, Is.TypeOf<LocaleDataStorage>());
                Assert.That(e, Is.Not.Null);
                Assert.That(e.LocaleDataDocument, Is.Not.Null);
                Assert.That(e.LocaleDataDocument, Is.TypeOf<XDocument>());
                Assert.That(e.ReadingContext, Is.True);
                Assert.That(e.WritingContext, Is.False);
                Assert.That(e.SynchronizationContext, Is.False);
                eventCalled = true;
            };

            Assert.That(eventCalled, Is.False);
            Assert.That(localeDataStorage.GetLocaleData(), Is.Not.Null);
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at GetLocaleData kaster en IntranetGuiRepositoryException ved IntranetGuiRepositoryException.
        /// </summary>
        [Test]
        public void TestAtGetLocaleDataKasterIntranetGuiRepositoryExceptionVedIntranetGuiRepositoryException()
        {
            Fixture fixture = new Fixture();

            ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            IntranetGuiRepositoryException eventException = fixture.Create<IntranetGuiRepositoryException>();
            localeDataStorage.OnCreateReaderStream += (s, e) =>
            {
                throw eventException;
            };

            IntranetGuiRepositoryException exception = Assert.Throws<IntranetGuiRepositoryException>(() => localeDataStorage.GetLocaleData());
            Assert.That(exception, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(eventException.Message));
            Assert.That(exception.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Tester, at GetLocaleData kaster en IntranetGuiRepositoryException ved Exception.
        /// </summary>
        [Test]
        public void TestAtGetLocaleDataKasterIntranetGuiRepositoryExceptionVedException()
        {
            Fixture fixture = new Fixture();

            ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            Exception eventException = fixture.Create<Exception>();
            localeDataStorage.OnCreateReaderStream += (s, e) =>
            {
                throw eventException;
            };

            IntranetGuiRepositoryException exception = Assert.Throws<IntranetGuiRepositoryException>(() => localeDataStorage.GetLocaleData());
            Assert.That(exception, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "GetLocaleData", eventException.Message)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.EqualTo(eventException));
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Tester, at StoreLocaleDocument kaster en ArgumentNullException, hvis XML dokumentet er null.
        /// </summary>
        [Test]
        public void TestAtStoreLocaleDocumentKasterArgumentNullExceptionHvisLocaleDataDocumentErNull()
        {
            Fixture fixture = new Fixture();

            ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => localeDataStorage.StoreLocaleDocument(null));
            Assert.That(exception, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("localeDataDocument"));
            Assert.That(exception.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Tester, at StoreLocaleDocument kaster en IntranetGuiRepositoryException, hvis eventet, der rejses, når en skrivestream til det lokale datalager skal dannes, er null.
        /// </summary>
        [Test]
        public void TestAtStoreLocaleDocumentKasterIntranetGuiRepositoryExceptionHvisOnOnCreateWriterStreamErNull()
        {
            Fixture fixture = new Fixture();

            ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            IntranetGuiRepositoryException exception = Assert.Throws<IntranetGuiRepositoryException>(() => localeDataStorage.StoreLocaleDocument(new XDocument()));
            Assert.That(exception, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.EventHandlerNotDefined, "OnCreateWriterStream")));
            Assert.That(exception.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Tester, at StoreLocaleDocument rejser eventet, der skal danne skrivestreamen til det lokale datalager.
        /// </summary>
        [Test]
        public void TestAtStoreLocaleDocumentRejserOnCreateWriterStreamEvent()
        {
            Fixture fixture = new Fixture();

            ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            bool eventCalled = false;
            localeDataStorage.OnCreateWriterStream += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(s, Is.TypeOf<LocaleDataStorage>());
                Assert.That(s, Is.EqualTo(localeDataStorage));
                Assert.That(e, Is.Not.Null);
                Assert.That(e.CreationContext, Is.Not.Null);
                Assert.That(e.CreationContext, Is.TypeOf<bool>());
                Assert.That(e.CreationContext, Is.EqualTo(false));
                Assert.That(e.Result, Is.Null);
                e.Result = CreateMemoryStreamWithoutXmlContext();
                eventCalled = true;
            };

            XDocument localeDataDocument = new XDocument(new XDeclaration("1.0", Encoding.UTF8.BodyName, null));
            localeDataDocument.Add(new XElement(XName.Get("Test")));

            Assert.That(eventCalled, Is.False);
            localeDataStorage.StoreLocaleDocument(localeDataDocument);
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at StoreLocaleDocument gemmer XML dokumentet.
        /// </summary>
        [Test]
        public void TestAtStoreLocaleDocumentGemmerLocalDataDocument()
        {
            Fixture fixture = new Fixture();

            ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            localeDataStorage.OnCreateWriterStream += (s, e) => e.Result = CreateMemoryStreamWithoutXmlContext();

            XDocument localeDataDocument = new XDocument(new XDeclaration("1.0", Encoding.UTF8.BodyName, null));
            localeDataDocument.Add(new XElement(XName.Get("Test")));

            localeDataStorage.StoreLocaleDocument(localeDataDocument);
        }

        /// <summary>
        /// Tester, at StoreLocaleDocument rejser eventet, der skal forberede data i det lokale datalager for læsning eller skrivning.
        /// </summary>
        [Test]
        public void TestAtStoreLocaleDocumentRejserPrepareLocaleDataEvent()
        {
            Fixture fixture = new Fixture();

            ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            localeDataStorage.OnCreateWriterStream += (s, e) => e.Result = CreateMemoryStreamWithoutXmlContext();

            XDocument localeDataDocument = new XDocument(new XDeclaration("1.0", Encoding.UTF8.BodyName, null));
            localeDataDocument.Add(new XElement(XName.Get("Test")));

            bool eventCalled = false;
            localeDataStorage.PrepareLocaleData += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(s, Is.TypeOf<LocaleDataStorage>());
                Assert.That(e, Is.Not.Null);
                Assert.That(e.LocaleDataDocument, Is.Not.Null);
                Assert.That(e.LocaleDataDocument, Is.TypeOf<XDocument>());
                Assert.That(e.LocaleDataDocument, Is.EqualTo(localeDataDocument));
                Assert.That(e.ReadingContext, Is.False);
                Assert.That(e.WritingContext, Is.True);
                Assert.That(e.SynchronizationContext, Is.False);
                eventCalled = true;
            };

            Assert.That(eventCalled, Is.False);
            localeDataStorage.StoreLocaleDocument(localeDataDocument);
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at StoreLocaleData kaster en ArgumentNullException, hvis data, der skal gemmes i det lokale datalager, er null.
        /// </summary>
        [Test]
        public void TestAtStoreLocaleDataKasterArgumentNullExceptionHvisModelErNull()
        {
            Fixture fixture = new Fixture();

            ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => localeDataStorage.StoreLocaleData((IModel) null));
            Assert.That(exception, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("model"));
            Assert.That(exception.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Tester, at StoreLocaleData kaster en IntranetGuiRepositoryException, hvis eventet, der rejses, når en skrivestream til det lokale datalager skal dannes, er null.
        /// </summary>
        [Test]
        public void TestAtStoreLocaleDataKasterIntranetGuiRepositoryExceptionHvisOnOnCreateWriterStreamErNull()
        {
            Fixture fixture = new Fixture();

            ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            IntranetGuiRepositoryException exception = Assert.Throws<IntranetGuiRepositoryException>(() => localeDataStorage.StoreLocaleData(new Mock<IModel>().Object));
            Assert.That(exception, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.EventHandlerNotDefined, "OnCreateWriterStream")));
            Assert.That(exception.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Tester, at StoreLocaleData rejser eventet, der skal danne skrivestreamen til det lokale datalager.
        /// </summary>
        [Test]
        public void TestAtStoreLocaleDataRejserOnCreateWriterStreamEvent()
        {
            Fixture fixture = new Fixture();

            ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            bool eventCalled = false;
            localeDataStorage.OnCreateWriterStream += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(s, Is.TypeOf<LocaleDataStorage>());
                Assert.That(s, Is.EqualTo(localeDataStorage));
                Assert.That(e, Is.Not.Null);
                Assert.That(e.CreationContext, Is.Not.Null);
                Assert.That(e.CreationContext, Is.TypeOf<bool>());
                Assert.That(e.CreationContext, Is.EqualTo(false));
                Assert.That(e.Result, Is.Null);
                e.Result = CreateMemoryStreamWithoutXmlContext();
                eventCalled = true;
            };

            Assert.That(eventCalled, Is.False);
            localeDataStorage.StoreLocaleData(new Mock<IModel>().Object);
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at StoreLocaleData gemmer data i det lokale datalager, når dette ikke har indhold.
        /// </summary>
        [Test]
        public void TestAtStoreLocaleDataGemmerDataWhenLocaleDataStorageIkkeHarData()
        {
            Fixture fixture = new Fixture();

            ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            localeDataStorage.OnCreateWriterStream += (s, e) => e.Result = CreateMemoryStreamWithoutXmlContext();

            localeDataStorage.StoreLocaleData(new Mock<IModel>().Object);
        }

        /// <summary>
        /// Tester, at StoreLocaleData gemmer data i det lokale datalager, når dette har indhold.
        /// </summary>
        [Test]
        public void TestAtStoreLocaleDataGemmerDataWhenLocaleDataStorageHarData()
        {
            Fixture fixture = new Fixture();

            ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            localeDataStorage.OnCreateWriterStream += (s, e) => e.Result = CreateMemoryStreamWithXmlContent();

            localeDataStorage.StoreLocaleData(new Mock<IModel>().Object);
        }

        /// <summary>
        /// Tester, at StoreLocaleData gemmer data i et lokalt fil storage.
        /// </summary>
        [Test]
        public void TestAtStoreLocaleDataGemmerDataILocalDataFileStorage()
        {
            Fixture fixture = new Fixture();

            FileInfo tempFile = new FileInfo(Path.GetTempFileName());
            try
            {
                ILocaleDataStorage localeDataStorage = new LocaleDataStorage(tempFile.FullName, fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
                Assert.That(localeDataStorage, Is.Not.Null);

                localeDataStorage.OnCreateWriterStream += (s, e) =>
                {
                    tempFile.Refresh();
                    if (tempFile.Exists)
                    {
                        e.Result = tempFile.Open(FileMode.Open, FileAccess.ReadWrite);
                        return;
                    }
                    e.Result = tempFile.Create();
                };

                localeDataStorage.StoreLocaleData(new Mock<IModel>().Object);
                localeDataStorage.StoreLocaleData(new Mock<IModel>().Object);
                localeDataStorage.StoreLocaleData(new Mock<IModel>().Object);
            }
            finally
            {
                tempFile.Refresh();
                while (tempFile.Exists)
                {
                    tempFile.Delete();
                    tempFile.Refresh();
                }
            }
        }

        /// <summary>
        /// Tester, at StoreLocaleData rejser eventet, der skal forberede data i det lokale datalager for læsning eller skrivning.
        /// </summary>
        [Test]
        public void TestAtStoreLocaleDataRejserPrepareLocaleDataEvent()
        {
            Fixture fixture = new Fixture();

            ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            localeDataStorage.OnCreateWriterStream += (s, e) => e.Result = CreateMemoryStreamWithoutXmlContext();

            bool eventCalled = false;
            localeDataStorage.PrepareLocaleData += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(s, Is.TypeOf<LocaleDataStorage>());
                Assert.That(e, Is.Not.Null);
                Assert.That(e.LocaleDataDocument, Is.Not.Null);
                Assert.That(e.LocaleDataDocument, Is.TypeOf<XDocument>());
                Assert.That(e.ReadingContext, Is.False);
                Assert.That(e.WritingContext, Is.True);
                Assert.That(e.SynchronizationContext, Is.False);
                eventCalled = true;
            };

            Assert.That(eventCalled, Is.False);
            localeDataStorage.StoreLocaleData(new Mock<IModel>().Object);
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at StoreLocaleData kaster en IntranetGuiRepositoryException ved IntranetGuiRepositoryException.
        /// </summary>
        [Test]
        public void TestAtStoreLocaleDataKasterIntranetGuiRepositoryExceptionVedIntranetGuiRepositoryException()
        {
            Fixture fixture = new Fixture();

            ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            IntranetGuiRepositoryException eventException = fixture.Create<IntranetGuiRepositoryException>();
            localeDataStorage.OnCreateWriterStream += (s, e) =>
            {
                throw eventException;
            };

            IntranetGuiRepositoryException exception = Assert.Throws<IntranetGuiRepositoryException>(() => localeDataStorage.StoreLocaleData(new Mock<IModel>().Object));
            Assert.That(exception, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(eventException.Message));
            Assert.That(exception.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Tester, at StoreLocaleData kaster en IntranetGuiRepositoryException ved Exception.
        /// </summary>
        [Test]
        public void TestAtStoreLocaleDataKasterIntranetGuiRepositoryExceptionVedException()
        {
            Fixture fixture = new Fixture();

            ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            Exception eventException = fixture.Create<Exception>();
            localeDataStorage.OnCreateWriterStream += (s, e) =>
            {
                throw eventException;
            };

            IntranetGuiRepositoryException exception = Assert.Throws<IntranetGuiRepositoryException>(() => localeDataStorage.StoreLocaleData(new Mock<IModel>().Object));
            Assert.That(exception, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "StoreLocaleData", eventException.Message)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.EqualTo(eventException));
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Tester, at StoreLocaleData gemmer et regnskab.
        /// </summary>
        [Test]
        public void TestAtStoreLocaleDataGemmerRegnskabModel()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            IRegnskabModel regnskabModelMock = fixture.BuildRegnskabModel(random.Next(1, 99));
            IRegnskabModel updatedRegnskabModelMock = fixture.BuildRegnskabModel(regnskabModelMock.Nummer);

            FileInfo tempFile = new FileInfo(Path.GetTempFileName());
            try
            {
                ILocaleDataStorage localeDataStorage = new LocaleDataStorage(tempFile.FullName, fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
                Assert.That(localeDataStorage, Is.Not.Null);

                localeDataStorage.OnCreateWriterStream += (s, e) =>
                {
                    tempFile.Refresh();
                    if (tempFile.Exists)
                    {
                        e.Result = tempFile.Open(FileMode.Open, FileAccess.ReadWrite);
                        return;
                    }
                    e.Result = tempFile.Create();
                };

                localeDataStorage.StoreLocaleData(regnskabModelMock);
                XmlNode regnskabNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(regnskabNode, "navn", regnskabModelMock.Navn));

                localeDataStorage.StoreLocaleData(updatedRegnskabModelMock);
                XmlNode updatedRegnskabNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedRegnskabNode, "navn", updatedRegnskabModelMock.Navn));
            }
            finally
            {
                tempFile.Refresh();
                while (tempFile.Exists)
                {
                    tempFile.Delete();
                    tempFile.Refresh();
                }
            }
        }

        /// <summary>
        /// Tester, at StoreLocaleData gemmer en konto.
        /// </summary>
        [Test]
        public void TestAtStoreLocaleDataGemmerKontoModel()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            IRegnskabModel regnskabModelMock = fixture.BuildRegnskabModel(random.Next(1, 99));
            IKontogruppeModel kontogruppeModel1Mock = fixture.BuildKontogruppeModel(random.Next(1, 99));
            IKontogruppeModel kontogruppeModel2Mock = fixture.BuildKontogruppeModel(random.Next(1, 99));

            DateTime statusDato = DateTime.Today.AddDays(random.Next(1, 365) * -1);
            IKontoModel kontoModelMock = fixture.BuildKontoModel(random, regnskabModelMock.Nummer, fixture.Create<string>(), kontogruppeModel1Mock.Nummer, statusDato);
            IKontoModel updatedKontoModelMock = fixture.BuildKontoModel(random, regnskabModelMock.Nummer, kontoModelMock.Kontonummer, kontogruppeModel2Mock.Nummer, kontoModelMock.StatusDato);
            IKontoModel updatedSaldoOnKontoModelMock = fixture.BuildKontoModel(random, regnskabModelMock.Nummer, kontoModelMock.Kontonummer, kontogruppeModel2Mock.Nummer, kontoModelMock.StatusDato.AddDays(random.Next(1, 365)));

            FileInfo tempFile = new FileInfo(Path.GetTempFileName());
            try
            {
                ILocaleDataStorage localeDataStorage = new LocaleDataStorage(tempFile.FullName, fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
                Assert.That(localeDataStorage, Is.Not.Null);

                localeDataStorage.OnCreateWriterStream += (s, e) =>
                {
                    tempFile.Refresh();
                    if (tempFile.Exists)
                    {
                        e.Result = tempFile.Open(FileMode.Open, FileAccess.ReadWrite);
                        return;
                    }
                    e.Result = tempFile.Create();
                };

                localeDataStorage.StoreLocaleData(regnskabModelMock);
                localeDataStorage.StoreLocaleData(kontogruppeModel1Mock);
                localeDataStorage.StoreLocaleData(kontogruppeModel2Mock);

                localeDataStorage.StoreLocaleData(kontoModelMock);
                XmlNode kontoNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']/ns:Konto[@kontonummer = '{kontoModelMock.Kontonummer}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(kontoNode, "kontonavn", kontoModelMock.Kontonavn));
                Assert.That(HasAttributeWhichMatchValue(kontoNode, "beskrivelse", kontoModelMock.Beskrivelse), Is.EqualTo(string.IsNullOrWhiteSpace(kontoModelMock.Beskrivelse) == false));
                Assert.That(HasAttributeWhichMatchValue(kontoNode, "note", kontoModelMock.Notat), Is.EqualTo(string.IsNullOrWhiteSpace(kontoModelMock.Notat) == false));
                Assert.IsTrue(HasAttributeWhichMatchValue(kontoNode, "kontogruppe", kontoModelMock.Kontogruppe.ToString(CultureInfo.InvariantCulture)));

                XmlNode kontoHistorikNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']/ns:KontoHistorik[@kontonummer = '{kontoModelMock.Kontonummer}' and @dato='{kontoModelMock.StatusDato:yyyyMMdd}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(kontoHistorikNode, "kredit", kontoModelMock.Kredit.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(kontoHistorikNode, "saldo", kontoModelMock.Saldo.ToString(DecimalFormat, CultureInfo.InvariantCulture)));

                localeDataStorage.StoreLocaleData(updatedKontoModelMock);
                XmlNode updatedKontoNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']/ns:Konto[@kontonummer = '{kontoModelMock.Kontonummer}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedKontoNode, "kontonavn", updatedKontoModelMock.Kontonavn));
                Assert.That(HasAttributeWhichMatchValue(updatedKontoNode, "beskrivelse", updatedKontoModelMock.Beskrivelse), Is.EqualTo(string.IsNullOrWhiteSpace(updatedKontoModelMock.Beskrivelse) == false));
                Assert.That(HasAttributeWhichMatchValue(updatedKontoNode, "note", updatedKontoModelMock.Notat), Is.EqualTo(string.IsNullOrWhiteSpace(updatedKontoModelMock.Notat) == false));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedKontoNode, "kontogruppe", updatedKontoModelMock.Kontogruppe.ToString(CultureInfo.InvariantCulture)));

                XmlNode updatedKontoHistorikNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']/ns:KontoHistorik[@kontonummer = '{kontoModelMock.Kontonummer}' and @dato='{kontoModelMock.StatusDato:yyyyMMdd}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedKontoHistorikNode, "kredit", updatedKontoModelMock.Kredit.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedKontoHistorikNode, "saldo", updatedKontoModelMock.Saldo.ToString(DecimalFormat, CultureInfo.InvariantCulture)));

                localeDataStorage.StoreLocaleData(updatedSaldoOnKontoModelMock);
                XmlNode updatedSaldoOnKontoHistorikNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']/ns:KontoHistorik[@kontonummer = '{kontoModelMock.Kontonummer}' and @dato='{updatedSaldoOnKontoModelMock.StatusDato:yyyyMMdd}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedSaldoOnKontoHistorikNode, "kredit", updatedSaldoOnKontoModelMock.Kredit.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedSaldoOnKontoHistorikNode, "saldo", updatedSaldoOnKontoModelMock.Saldo.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
            }
            finally
            {
                tempFile.Refresh();
                while (tempFile.Exists)
                {
                    tempFile.Delete();
                    tempFile.Refresh();
                }
            }
        }

        /// <summary>
        /// Tester, at StoreLocaleData gemmer en budgetkonto.
        /// </summary>
        [Test]
        public void TestAtStoreLocaleDataGemmerBudgetkontoModel()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            IRegnskabModel regnskabModelMock = fixture.BuildRegnskabModel(random.Next(1, 99));
            IBudgetkontogruppeModel budgetkontogruppeModel1Mock = fixture.BuildBudgetkontogruppeModel(random.Next(1, 99));
            IBudgetkontogruppeModel budgetkontogruppeModel2Mock = fixture.BuildBudgetkontogruppeModel(random.Next(1, 99));

            DateTime statusDato = DateTime.Today.AddDays(random.Next(1, 365) * -1);
            IBudgetkontoModel budgetkontoModelMock = fixture.BuildBudgetkontoModel(random, regnskabModelMock.Nummer, fixture.Create<string>(), budgetkontogruppeModel1Mock.Nummer, statusDato);
            IBudgetkontoModel updatedBudgetkontoModelMock = fixture.BuildBudgetkontoModel(random, budgetkontoModelMock.Regnskabsnummer, budgetkontoModelMock.Kontonummer, budgetkontogruppeModel2Mock.Nummer, budgetkontoModelMock.StatusDato);
            IBudgetkontoModel updatedSaldoOnBudgetkontoModelMock = fixture.BuildBudgetkontoModel(random, budgetkontoModelMock.Regnskabsnummer, budgetkontoModelMock.Kontonummer, budgetkontogruppeModel2Mock.Nummer, budgetkontoModelMock.StatusDato.AddDays(random.Next(1, 365)));

            FileInfo tempFile = new FileInfo(Path.GetTempFileName());
            try
            {
                ILocaleDataStorage localeDataStorage = new LocaleDataStorage(tempFile.FullName, fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
                Assert.That(localeDataStorage, Is.Not.Null);

                localeDataStorage.OnCreateWriterStream += (s, e) =>
                {
                    tempFile.Refresh();
                    if (tempFile.Exists)
                    {
                        e.Result = tempFile.Open(FileMode.Open, FileAccess.ReadWrite);
                        return;
                    }
                    e.Result = tempFile.Create();
                };

                localeDataStorage.StoreLocaleData(regnskabModelMock);
                localeDataStorage.StoreLocaleData(budgetkontogruppeModel1Mock);
                localeDataStorage.StoreLocaleData(budgetkontogruppeModel2Mock);

                localeDataStorage.StoreLocaleData(budgetkontoModelMock);
                XmlNode budgetkontoNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']/ns:Budgetkonto[@kontonummer = '{budgetkontoModelMock.Kontonummer}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(budgetkontoNode, "kontonavn", budgetkontoModelMock.Kontonavn));
                Assert.That(HasAttributeWhichMatchValue(budgetkontoNode, "beskrivelse", budgetkontoModelMock.Beskrivelse), Is.EqualTo(string.IsNullOrWhiteSpace(budgetkontoModelMock.Beskrivelse) == false));
                Assert.That(HasAttributeWhichMatchValue(budgetkontoNode, "note", budgetkontoModelMock.Notat), Is.EqualTo(string.IsNullOrWhiteSpace(budgetkontoModelMock.Notat) == false));
                Assert.IsTrue(HasAttributeWhichMatchValue(budgetkontoNode, "kontogruppe", budgetkontoModelMock.Kontogruppe.ToString(CultureInfo.InvariantCulture)));

                XmlNode budgetkontoHistorikNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']/ns:BudgetkontoHistorik[@kontonummer = '{budgetkontoModelMock.Kontonummer}' and @dato='{budgetkontoModelMock.StatusDato:yyyyMMdd}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(budgetkontoHistorikNode, "indtaegter", budgetkontoModelMock.Indtægter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(budgetkontoHistorikNode, "udgifter", budgetkontoModelMock.Udgifter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(budgetkontoHistorikNode, "bogfoert", budgetkontoModelMock.Bogført.ToString(DecimalFormat, CultureInfo.InvariantCulture)));

                DateTime lastMonthStatusDato = new DateTime(budgetkontoModelMock.StatusDato.AddMonths(-1).Year, budgetkontoModelMock.StatusDato.AddMonths(-1).Month, DateTime.DaysInMonth(budgetkontoModelMock.StatusDato.AddMonths(-1).Year, budgetkontoModelMock.StatusDato.AddMonths(-1).Month));
                decimal lastMonthIndtægter = Math.Abs(budgetkontoModelMock.BudgetSidsteMåned > 0 ? budgetkontoModelMock.BudgetSidsteMåned : 0M);
                decimal lastMonthUdgifter = Math.Abs(budgetkontoModelMock.BudgetSidsteMåned < 0 ? budgetkontoModelMock.BudgetSidsteMåned : 0M);
                XmlNode lastMonthBudgetkontoHistorikNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']/ns:BudgetkontoHistorik[@kontonummer = '{budgetkontoModelMock.Kontonummer}' and @dato='{lastMonthStatusDato:yyyyMMdd}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(lastMonthBudgetkontoHistorikNode, "indtaegter", lastMonthIndtægter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(lastMonthBudgetkontoHistorikNode, "udgifter", lastMonthUdgifter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(lastMonthBudgetkontoHistorikNode, "bogfoert", budgetkontoModelMock.BogførtSidsteMåned.ToString(DecimalFormat, CultureInfo.InvariantCulture)));

                localeDataStorage.StoreLocaleData(updatedBudgetkontoModelMock);
                XmlNode updatedBudgetkontoNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']/ns:Budgetkonto[@kontonummer = '{budgetkontoModelMock.Kontonummer}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBudgetkontoNode, "kontonavn", updatedBudgetkontoModelMock.Kontonavn));
                Assert.That(HasAttributeWhichMatchValue(updatedBudgetkontoNode, "beskrivelse", updatedBudgetkontoModelMock.Beskrivelse), Is.EqualTo(string.IsNullOrWhiteSpace(updatedBudgetkontoModelMock.Beskrivelse) == false));
                Assert.That(HasAttributeWhichMatchValue(updatedBudgetkontoNode, "note", updatedBudgetkontoModelMock.Notat), Is.EqualTo(string.IsNullOrWhiteSpace(updatedBudgetkontoModelMock.Notat) == false));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBudgetkontoNode, "kontogruppe", updatedBudgetkontoModelMock.Kontogruppe.ToString(CultureInfo.InvariantCulture)));

                XmlNode updatedBudgetkontoHistorikNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']/ns:BudgetkontoHistorik[@kontonummer = '{budgetkontoModelMock.Kontonummer}' and @dato='{budgetkontoModelMock.StatusDato:yyyyMMdd}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBudgetkontoHistorikNode, "indtaegter", updatedBudgetkontoModelMock.Indtægter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBudgetkontoHistorikNode, "udgifter", updatedBudgetkontoModelMock.Udgifter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBudgetkontoHistorikNode, "bogfoert", updatedBudgetkontoModelMock.Bogført.ToString(DecimalFormat, CultureInfo.InvariantCulture)));

                lastMonthStatusDato = new DateTime(updatedBudgetkontoModelMock.StatusDato.AddMonths(-1).Year, updatedBudgetkontoModelMock.StatusDato.AddMonths(-1).Month, DateTime.DaysInMonth(updatedBudgetkontoModelMock.StatusDato.AddMonths(-1).Year, updatedBudgetkontoModelMock.StatusDato.AddMonths(-1).Month));
                lastMonthIndtægter = Math.Abs(updatedBudgetkontoModelMock.BudgetSidsteMåned > 0 ? updatedBudgetkontoModelMock.BudgetSidsteMåned : 0M);
                lastMonthUdgifter = Math.Abs(updatedBudgetkontoModelMock.BudgetSidsteMåned < 0 ? updatedBudgetkontoModelMock.BudgetSidsteMåned : 0M);
                XmlNode lastMonthUpdatedBudgetkontoHistorikNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']/ns:BudgetkontoHistorik[@kontonummer = '{budgetkontoModelMock.Kontonummer}' and @dato='{lastMonthStatusDato:yyyyMMdd}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(lastMonthUpdatedBudgetkontoHistorikNode, "indtaegter", lastMonthIndtægter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(lastMonthUpdatedBudgetkontoHistorikNode, "udgifter", lastMonthUdgifter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(lastMonthUpdatedBudgetkontoHistorikNode, "bogfoert", updatedBudgetkontoModelMock.BogførtSidsteMåned.ToString(DecimalFormat, CultureInfo.InvariantCulture)));

                localeDataStorage.StoreLocaleData(updatedSaldoOnBudgetkontoModelMock);
                XmlNode updatedSaldoOnBudgetkontoHistorikNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']/ns:BudgetkontoHistorik[@kontonummer = '{budgetkontoModelMock.Kontonummer}' and @dato='{updatedSaldoOnBudgetkontoModelMock.StatusDato:yyyyMMdd}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedSaldoOnBudgetkontoHistorikNode, "indtaegter", updatedSaldoOnBudgetkontoModelMock.Indtægter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedSaldoOnBudgetkontoHistorikNode, "udgifter", updatedSaldoOnBudgetkontoModelMock.Udgifter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedSaldoOnBudgetkontoHistorikNode, "bogfoert", updatedSaldoOnBudgetkontoModelMock.Bogført.ToString(DecimalFormat, CultureInfo.InvariantCulture)));

                lastMonthStatusDato = new DateTime(updatedSaldoOnBudgetkontoModelMock.StatusDato.AddMonths(-1).Year, updatedSaldoOnBudgetkontoModelMock.StatusDato.AddMonths(-1).Month, DateTime.DaysInMonth(updatedSaldoOnBudgetkontoModelMock.StatusDato.AddMonths(-1).Year, updatedSaldoOnBudgetkontoModelMock.StatusDato.AddMonths(-1).Month));
                lastMonthIndtægter = Math.Abs(updatedSaldoOnBudgetkontoModelMock.BudgetSidsteMåned > 0 ? updatedSaldoOnBudgetkontoModelMock.BudgetSidsteMåned : 0M);
                lastMonthUdgifter = Math.Abs(updatedSaldoOnBudgetkontoModelMock.BudgetSidsteMåned < 0 ? updatedSaldoOnBudgetkontoModelMock.BudgetSidsteMåned : 0M);
                XmlNode lastMonthUpdatedSaldoOnBudgetkontoHistorikNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']/ns:BudgetkontoHistorik[@kontonummer = '{budgetkontoModelMock.Kontonummer}' and @dato='{lastMonthStatusDato:yyyyMMdd}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(lastMonthUpdatedSaldoOnBudgetkontoHistorikNode, "indtaegter", lastMonthIndtægter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(lastMonthUpdatedSaldoOnBudgetkontoHistorikNode, "udgifter", lastMonthUdgifter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(lastMonthUpdatedSaldoOnBudgetkontoHistorikNode, "bogfoert", updatedSaldoOnBudgetkontoModelMock.BogførtSidsteMåned.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
            }
            finally
            {
                tempFile.Refresh();
                while (tempFile.Exists)
                {
                    tempFile.Delete();
                    tempFile.Refresh();
                }
            }
        }

        /// <summary>
        /// Tester, at StoreLocaleData gemmer en adressekonto.
        /// </summary>
        [Test]
        public void TestAtStoreLocaleDataGemmerAdressekontoModel()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            IRegnskabModel regnskabModelMock = fixture.BuildRegnskabModel(random.Next(1, 99));

            DateTime statusDato = DateTime.Today.AddDays(random.Next(1, 365) * -1);
            IAdressekontoModel adressekontoModelMock = DomainObjectBuilder.BuildAdressekontoModel(fixture, random, regnskabModelMock.Nummer, fixture.Create<int>(), statusDato);
            IAdressekontoModel updatedAdressekontoModelMock = DomainObjectBuilder.BuildAdressekontoModel(fixture, random, adressekontoModelMock.Regnskabsnummer, adressekontoModelMock.Nummer, adressekontoModelMock.StatusDato);
            IAdressekontoModel updatedSaldoOnAdressekontoModelMock = DomainObjectBuilder.BuildAdressekontoModel(fixture, random, adressekontoModelMock.Regnskabsnummer, adressekontoModelMock.Nummer, adressekontoModelMock.StatusDato.AddDays(random.Next(1, 365)));

            FileInfo tempFile = new FileInfo(Path.GetTempFileName());
            try
            {
                ILocaleDataStorage localeDataStorage = new LocaleDataStorage(tempFile.FullName, fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
                Assert.That(localeDataStorage, Is.Not.Null);

                localeDataStorage.OnCreateWriterStream += (s, e) =>
                {
                    tempFile.Refresh();
                    if (tempFile.Exists)
                    {
                        e.Result = tempFile.Open(FileMode.Open, FileAccess.ReadWrite);
                        return;
                    }
                    e.Result = tempFile.Create();
                };

                localeDataStorage.StoreLocaleData(regnskabModelMock);

                localeDataStorage.StoreLocaleData(adressekontoModelMock);
                XmlNode adressekontoNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']/ns:Adressekonto[@nummer = '{adressekontoModelMock.Nummer.ToString(CultureInfo.InvariantCulture)}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(adressekontoNode, "navn", adressekontoModelMock.Navn));
                Assert.That(HasAttributeWhichMatchValue(adressekontoNode, "primaerTelefon", adressekontoModelMock.PrimærTelefon), Is.EqualTo(string.IsNullOrWhiteSpace(adressekontoModelMock.PrimærTelefon) == false));
                Assert.That(HasAttributeWhichMatchValue(adressekontoNode, "sekundaerTelefon", adressekontoModelMock.SekundærTelefon), Is.EqualTo(string.IsNullOrWhiteSpace(adressekontoModelMock.SekundærTelefon) == false));

                XmlNode adressekontoHistorikNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']/ns:AdressekontoHistorik[@nummer = '{adressekontoModelMock.Nummer.ToString(CultureInfo.InvariantCulture)}' and @dato='{adressekontoModelMock.StatusDato:yyyyMMdd}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(adressekontoHistorikNode, "saldo", adressekontoModelMock.Saldo.ToString(DecimalFormat, CultureInfo.InvariantCulture)));

                localeDataStorage.StoreLocaleData(updatedAdressekontoModelMock);
                XmlNode updatedAdressekontoNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']/ns:Adressekonto[@nummer = '{adressekontoModelMock.Nummer.ToString(CultureInfo.InvariantCulture)}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedAdressekontoNode, "navn", updatedAdressekontoModelMock.Navn));
                Assert.That(HasAttributeWhichMatchValue(updatedAdressekontoNode, "primaerTelefon", updatedAdressekontoModelMock.PrimærTelefon), Is.EqualTo(string.IsNullOrWhiteSpace(updatedAdressekontoModelMock.PrimærTelefon) == false));
                Assert.That(HasAttributeWhichMatchValue(updatedAdressekontoNode, "sekundaerTelefon", updatedAdressekontoModelMock.SekundærTelefon), Is.EqualTo(string.IsNullOrWhiteSpace(updatedAdressekontoModelMock.SekundærTelefon) == false));

                XmlNode updatedAdressekontoHistorikNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']/ns:AdressekontoHistorik[@nummer = '{adressekontoModelMock.Nummer.ToString(CultureInfo.InvariantCulture)}' and @dato='{adressekontoModelMock.StatusDato:yyyyMMdd}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedAdressekontoHistorikNode, "saldo", updatedAdressekontoModelMock.Saldo.ToString(DecimalFormat, CultureInfo.InvariantCulture)));

                localeDataStorage.StoreLocaleData(updatedSaldoOnAdressekontoModelMock);
                XmlNode updatedSaldoOnAdressekontoHistorikNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']/ns:AdressekontoHistorik[@nummer = '{updatedSaldoOnAdressekontoModelMock.Nummer.ToString(CultureInfo.InvariantCulture)}' and @dato='{updatedSaldoOnAdressekontoModelMock.StatusDato:yyyyMMdd}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedSaldoOnAdressekontoHistorikNode, "saldo", updatedSaldoOnAdressekontoModelMock.Saldo.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
            }
            finally
            {
                tempFile.Refresh();
                while (tempFile.Exists)
                {
                    tempFile.Delete();
                    tempFile.Refresh();
                }
            }
        }

        /// <summary>
        /// Tester, at StoreLocaleData gemmer en bogføringslinje.
        /// </summary>
        [Test]
        public void TestAtStoreLocaleDataGemmerBogføringslinjeModel()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            IRegnskabModel regnskabModelMock = fixture.BuildRegnskabModel(random.Next(1, 99));

            DateTime statusDato = DateTime.Today.AddDays(random.Next(1, 365) * -1);
            IBogføringslinjeModel bogføringslinjeModelMock = DomainObjectBuilder.BuildBogføringslinjeModel(fixture, random, regnskabModelMock.Nummer, fixture.Create<int>(), statusDato, fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>());
            IBogføringslinjeModel updatedBogføringslinjeModelMock = DomainObjectBuilder.BuildBogføringslinjeModel(fixture, random, bogføringslinjeModelMock.Regnskabsnummer, bogføringslinjeModelMock.Løbenummer, statusDato.AddDays(random.Next(1, 365)), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>());

            FileInfo tempFile = new FileInfo(Path.GetTempFileName());
            try
            {
                ILocaleDataStorage localeDataStorage = new LocaleDataStorage(tempFile.FullName, fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
                Assert.That(localeDataStorage, Is.Not.Null);

                localeDataStorage.OnCreateWriterStream += (s, e) =>
                {
                    tempFile.Refresh();
                    if (tempFile.Exists)
                    {
                        e.Result = tempFile.Open(FileMode.Open, FileAccess.ReadWrite);
                        return;
                    }
                    e.Result = tempFile.Create();
                };

                localeDataStorage.StoreLocaleData(regnskabModelMock);

                localeDataStorage.StoreLocaleData(bogføringslinjeModelMock);
                XmlNode bogføringslinjeNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']/ns:Bogfoeringslinje[@loebenummer = '{bogføringslinjeModelMock.Løbenummer.ToString(CultureInfo.InvariantCulture)}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(bogføringslinjeNode, "dato", bogføringslinjeModelMock.Dato.ToString("yyyyMMdd")));
                Assert.That(HasAttributeWhichMatchValue(bogføringslinjeNode, "bilag", bogføringslinjeModelMock.Bilag), Is.EqualTo(string.IsNullOrWhiteSpace(bogføringslinjeModelMock.Bilag) == false));
                Assert.IsTrue(HasAttributeWhichMatchValue(bogføringslinjeNode, "kontonummer", bogføringslinjeModelMock.Kontonummer));
                Assert.IsTrue(HasAttributeWhichMatchValue(bogføringslinjeNode, "tekst", bogføringslinjeModelMock.Tekst));
                Assert.That(HasAttributeWhichMatchValue(bogføringslinjeNode, "budgetkontonummer", bogføringslinjeModelMock.Budgetkontonummer), Is.EqualTo(string.IsNullOrWhiteSpace(bogføringslinjeModelMock.Budgetkontonummer) == false));
                Assert.That(HasAttributeWhichMatchValue(bogføringslinjeNode, "debit", bogføringslinjeModelMock.Debit.ToString(DecimalFormat, CultureInfo.InvariantCulture)), Is.EqualTo(bogføringslinjeModelMock.Debit > 0M));
                Assert.That(HasAttributeWhichMatchValue(bogføringslinjeNode, "kredit", bogføringslinjeModelMock.Kredit.ToString(DecimalFormat, CultureInfo.InvariantCulture)), Is.EqualTo(bogføringslinjeModelMock.Kredit > 0M));
                Assert.That(HasAttributeWhichMatchValue(bogføringslinjeNode, "adressekonto", bogføringslinjeModelMock.Adressekonto.ToString(CultureInfo.InvariantCulture)), Is.EqualTo(bogføringslinjeModelMock.Adressekonto != 0));
                Assert.IsTrue(HasAttributeWhichMatchValue(bogføringslinjeNode, "synkroniseret", "false"));
                Assert.IsFalse(HasAttributeWhichMatchValue(bogføringslinjeNode, "verserende", "true"));

                localeDataStorage.StoreLocaleData(updatedBogføringslinjeModelMock);
                XmlNode updatedBogføringslinjeNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']/ns:Bogfoeringslinje[@loebenummer = '{bogføringslinjeModelMock.Løbenummer.ToString(CultureInfo.InvariantCulture)}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBogføringslinjeNode, "dato", updatedBogføringslinjeModelMock.Dato.ToString("yyyyMMdd")));
                Assert.That(HasAttributeWhichMatchValue(updatedBogføringslinjeNode, "bilag", updatedBogføringslinjeModelMock.Bilag), Is.EqualTo(string.IsNullOrWhiteSpace(updatedBogføringslinjeModelMock.Bilag) == false));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBogføringslinjeNode, "kontonummer", updatedBogføringslinjeModelMock.Kontonummer));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBogføringslinjeNode, "tekst", updatedBogføringslinjeModelMock.Tekst));
                Assert.That(HasAttributeWhichMatchValue(updatedBogføringslinjeNode, "budgetkontonummer", updatedBogføringslinjeModelMock.Budgetkontonummer), Is.EqualTo(string.IsNullOrWhiteSpace(updatedBogføringslinjeModelMock.Budgetkontonummer) == false));
                Assert.That(HasAttributeWhichMatchValue(updatedBogføringslinjeNode, "debit", updatedBogføringslinjeModelMock.Debit.ToString(DecimalFormat, CultureInfo.InvariantCulture)), Is.EqualTo(updatedBogføringslinjeModelMock.Debit > 0M));
                Assert.That(HasAttributeWhichMatchValue(updatedBogføringslinjeNode, "kredit", updatedBogføringslinjeModelMock.Kredit.ToString(DecimalFormat, CultureInfo.InvariantCulture)), Is.EqualTo(updatedBogføringslinjeModelMock.Kredit > 0M));
                Assert.That(HasAttributeWhichMatchValue(updatedBogføringslinjeNode, "adressekonto", updatedBogføringslinjeModelMock.Adressekonto.ToString(CultureInfo.InvariantCulture)), Is.EqualTo(updatedBogføringslinjeModelMock.Adressekonto != 0));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBogføringslinjeNode, "synkroniseret", "false"));
                Assert.IsFalse(HasAttributeWhichMatchValue(updatedBogføringslinjeNode, "verserende", "true"));
            }
            finally
            {
                tempFile.Refresh();
                while (tempFile.Exists)
                {
                    tempFile.Delete();
                    tempFile.Refresh();
                }
            }
        }

        /// <summary>
        /// Tester, at StoreLocaleData gemmer en kontogruppe.
        /// </summary>
        [Test]
        public void TestAtStoreLocaleDataGemmerKontogruppeModel()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            IKontogruppeModel kontogruppeModelMock = DomainObjectBuilder.BuildKontogruppeModel(fixture, random.Next(1, 99), Balancetype.Aktiver);
            IKontogruppeModel updatedKontogruppeModelMock = DomainObjectBuilder.BuildKontogruppeModel(fixture, kontogruppeModelMock.Nummer, Balancetype.Passiver);

            FileInfo tempFile = new FileInfo(Path.GetTempFileName());
            try
            {
                ILocaleDataStorage localeDataStorage = new LocaleDataStorage(tempFile.FullName, fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
                Assert.That(localeDataStorage, Is.Not.Null);

                localeDataStorage.OnCreateWriterStream += (s, e) =>
                {
                    tempFile.Refresh();
                    if (tempFile.Exists)
                    {
                        e.Result = tempFile.Open(FileMode.Open, FileAccess.ReadWrite);
                        return;
                    }
                    e.Result = tempFile.Create();
                };

                localeDataStorage.StoreLocaleData(kontogruppeModelMock);
                XmlNode kontogruppeNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Kontogruppe[@nummer = '{kontogruppeModelMock.Nummer}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(kontogruppeNode, "tekst", kontogruppeModelMock.Tekst));
                Assert.IsTrue(HasAttributeWhichMatchValue(kontogruppeNode, "balanceType", kontogruppeModelMock.Balancetype.ToString()));

                localeDataStorage.StoreLocaleData(updatedKontogruppeModelMock);
                XmlNode updatedKontogruppeNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Kontogruppe[@nummer = '{kontogruppeModelMock.Nummer}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedKontogruppeNode, "tekst", updatedKontogruppeModelMock.Tekst));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedKontogruppeNode, "balanceType", updatedKontogruppeModelMock.Balancetype.ToString()));
            }
            finally
            {
                tempFile.Refresh();
                while (tempFile.Exists)
                {
                    tempFile.Delete();
                    tempFile.Refresh();
                }
            }
        }

        /// <summary>
        /// Tester, at StoreLocaleData gemmer en kontogruppe til budgetkonti.
        /// </summary>
        [Test]
        public void TestAtStoreLocaleDataGemmerBudgetkontogruppeModel()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            IBudgetkontogruppeModel budgetkontogruppeModelMock = DomainObjectBuilder.BuildBudgetkontogruppeModel(fixture, random.Next(1, 99));
            IBudgetkontogruppeModel updatedBudgetkontogruppeModelMock = DomainObjectBuilder.BuildBudgetkontogruppeModel(fixture, budgetkontogruppeModelMock.Nummer);

            FileInfo tempFile = new FileInfo(Path.GetTempFileName());
            try
            {
                ILocaleDataStorage localeDataStorage = new LocaleDataStorage(tempFile.FullName, fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
                Assert.That(localeDataStorage, Is.Not.Null);

                localeDataStorage.OnCreateWriterStream += (s, e) =>
                {
                    tempFile.Refresh();
                    if (tempFile.Exists)
                    {
                        e.Result = tempFile.Open(FileMode.Open, FileAccess.ReadWrite);
                        return;
                    }
                    e.Result = tempFile.Create();
                };

                localeDataStorage.StoreLocaleData(budgetkontogruppeModelMock);
                XmlNode budgetkontogruppeNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Budgetkontogruppe[@nummer = '{budgetkontogruppeModelMock.Nummer}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(budgetkontogruppeNode, "tekst", budgetkontogruppeModelMock.Tekst));

                localeDataStorage.StoreLocaleData(updatedBudgetkontogruppeModelMock);
                XmlNode updatedBudgetkontogruppeNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Budgetkontogruppe[@nummer = '{budgetkontogruppeModelMock.Nummer}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBudgetkontogruppeNode, "tekst", updatedBudgetkontogruppeModelMock.Tekst));
            }
            finally
            {
                tempFile.Refresh();
                while (tempFile.Exists)
                {
                    tempFile.Delete();
                    tempFile.Refresh();
                }
            }
        }

        /// <summary>
        /// Tester, at StoreSyncDocument kaster en ArgumentNullException, hvis XML dokumentet er null.
        /// </summary>
        [Test]
        public void TestAtStoreSyncDocumentKasterArgumentNullExceptionHvisLocaleDataDocumentErNull()
        {
            Fixture fixture = new Fixture();

            ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => localeDataStorage.StoreSyncDocument(null));
            Assert.That(exception, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("localeDataDocument"));
            Assert.That(exception.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Tester, at StoreSyncDocument kaster en IntranetGuiRepositoryException, hvis eventet, der rejses, når en skrivestream til det lokale datalager skal dannes, er null.
        /// </summary>
        [Test]
        public void TestAtStoreSyncDocumentKasterIntranetGuiRepositoryExceptionHvisOnOnCreateWriterStreamErNull()
        {
            Fixture fixture = new Fixture();

            ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            IntranetGuiRepositoryException exception = Assert.Throws<IntranetGuiRepositoryException>(() => localeDataStorage.StoreSyncDocument(new XDocument()));
            Assert.That(exception, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.EventHandlerNotDefined, "OnCreateWriterStream")));
            Assert.That(exception.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Tester, at StoreSyncDocument rejser eventet, der skal danne skrivestreamen til det lokale datalager.
        /// </summary>
        [Test]
        public void TestAtStoreSyncDocumentRejserOnCreateWriterStreamEvent()
        {
            Fixture fixture = new Fixture();

            ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            bool eventCalled = false;
            localeDataStorage.OnCreateWriterStream += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(s, Is.TypeOf<LocaleDataStorage>());
                Assert.That(s, Is.EqualTo(localeDataStorage));
                Assert.That(e, Is.Not.Null);
                Assert.That(e.CreationContext, Is.Not.Null);
                Assert.That(e.CreationContext, Is.TypeOf<bool>());
                Assert.That(e.CreationContext, Is.EqualTo(true));
                Assert.That(e.Result, Is.Null);
                e.Result = CreateMemoryStreamWithoutXmlContext();
                eventCalled = true;
            };

            XDocument localeDataDocument = new XDocument(new XDeclaration("1.0", Encoding.UTF8.BodyName, null));
            localeDataDocument.Add(new XElement(XName.Get("Test")));

            Assert.That(eventCalled, Is.False);
            localeDataStorage.StoreSyncDocument(localeDataDocument);
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at StoreSyncDocument gemmer XML dokumentet.
        /// </summary>
        [Test]
        public void TestAtStoreSyncDocumentGemmerLocalDataDocument()
        {
            Fixture fixture = new Fixture();

            ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            localeDataStorage.OnCreateWriterStream += (s, e) => e.Result = CreateMemoryStreamWithoutXmlContext();

            XDocument localeDataDocument = new XDocument(new XDeclaration("1.0", Encoding.UTF8.BodyName, null));
            localeDataDocument.Add(new XElement(XName.Get("Test")));

            localeDataStorage.StoreSyncDocument(localeDataDocument);
        }

        /// <summary>
        /// Tester, at StoreSyncDocument rejser eventet, der skal forberede data i det lokale datalager for læsning eller skrivning.
        /// </summary>
        [Test]
        public void TestAtStoreSyncDocumentRejserPrepareLocaleDataEvent()
        {
            Fixture fixture = new Fixture();

            ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            localeDataStorage.OnCreateWriterStream += (s, e) => e.Result = CreateMemoryStreamWithoutXmlContext();

            XDocument localeDataDocument = new XDocument(new XDeclaration("1.0", Encoding.UTF8.BodyName, null));
            localeDataDocument.Add(new XElement(XName.Get("Test")));

            bool eventCalled = false;
            localeDataStorage.PrepareLocaleData += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(s, Is.TypeOf<LocaleDataStorage>());
                Assert.That(e, Is.Not.Null);
                Assert.That(e.LocaleDataDocument, Is.Not.Null);
                Assert.That(e.LocaleDataDocument, Is.TypeOf<XDocument>());
                Assert.That(e.LocaleDataDocument, Is.EqualTo(localeDataDocument));
                Assert.That(e.ReadingContext, Is.False);
                Assert.That(e.WritingContext, Is.True);
                Assert.That(e.SynchronizationContext, Is.True);
                eventCalled = true;
            };

            Assert.That(eventCalled, Is.False);
            localeDataStorage.StoreSyncDocument(localeDataDocument);
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at StoreSyncData kaster en ArgumentNullException, hvis synkronseringsdata, der skal gemmes i det lokale datalager, er null.
        /// </summary>
        [Test]
        public void TestAtStoreSyncDataKasterArgumentNullExceptionHvisModelErNull()
        {
            Fixture fixture = new Fixture();

            ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => localeDataStorage.StoreSyncData((IModel) null));
            Assert.That(exception, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("model"));
            Assert.That(exception.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Tester, at StoreSyncData kaster en IntranetGuiRepositoryException, hvis eventet, der rejses, når en skrivestream til det lokale datalager skal dannes, er null.
        /// </summary>
        [Test]
        public void TestAtStoreSyncDataKasterIntranetGuiRepositoryExceptionHvisOnOnCreateWriterStreamErNull()
        {
            Fixture fixture = new Fixture();

            ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            IntranetGuiRepositoryException exception = Assert.Throws<IntranetGuiRepositoryException>(() => localeDataStorage.StoreLocaleData(new Mock<IModel>().Object));
            Assert.That(exception, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.EventHandlerNotDefined, "OnCreateWriterStream")));
            Assert.That(exception.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Tester, at StoreSyncData rejser eventet, der skal danne skrivestreamen til det lokale datalager.
        /// </summary>
        [Test]
        public void TestAtStoreSyncDataRejserOnCreateWriterStreamEvent()
        {
            Fixture fixture = new Fixture();

            ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            bool eventCalled = false;
            localeDataStorage.OnCreateWriterStream += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(s, Is.TypeOf<LocaleDataStorage>());
                Assert.That(s, Is.EqualTo(localeDataStorage));
                Assert.That(e, Is.Not.Null);
                Assert.That(e.CreationContext, Is.Not.Null);
                Assert.That(e.CreationContext, Is.TypeOf<bool>());
                Assert.That(e.CreationContext, Is.EqualTo(true));
                Assert.That(e.Result, Is.Null);
                e.Result = CreateMemoryStreamWithoutXmlContext();
                eventCalled = true;
            };

            Assert.That(eventCalled, Is.False);
            localeDataStorage.StoreSyncData(new Mock<IModel>().Object);
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at StoreSyncData gemmer synkroniseringsdata i det lokale datalager, når dette ikke har indhold.
        /// </summary>
        [Test]
        public void TestAtStoreSyncDataGemmerDataWhenLocaleDataStorageIkkeHarData()
        {
            Fixture fixture = new Fixture();

            ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            localeDataStorage.OnCreateWriterStream += (s, e) => e.Result = CreateMemoryStreamWithoutXmlContext();

            localeDataStorage.StoreSyncData(new Mock<IModel>().Object);
        }

        /// <summary>
        /// Tester, at StoreSyncData gemmer synkroniseringsdata i det lokale datalager, når dette har indhold.
        /// </summary>
        [Test]
        public void TestAtStoreSyncDataGemmerDataWhenLocaleDataStorageHarData()
        {
            Fixture fixture = new Fixture();

            ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            localeDataStorage.OnCreateWriterStream += (s, e) => e.Result = CreateMemoryStreamWithXmlContent();

            localeDataStorage.StoreSyncData(new Mock<IModel>().Object);
        }

        /// <summary>
        /// Tester, at StoreSyncData gemmer data i et lokalt fil storage.
        /// </summary>
        [Test]
        public void TestAtStoreSyncDataGemmerDataILocalDataFileStorage()
        {
            Fixture fixture = new Fixture();

            FileInfo tempFile = new FileInfo(Path.GetTempFileName());
            try
            {
                ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), tempFile.FullName, FinansstyringRepositoryLocale.XmlSchema);
                Assert.That(localeDataStorage, Is.Not.Null);

                localeDataStorage.OnCreateWriterStream += (s, e) =>
                {
                    tempFile.Refresh();
                    if (tempFile.Exists)
                    {
                        e.Result = tempFile.Open(FileMode.Open, FileAccess.ReadWrite);
                        return;
                    }
                    e.Result = tempFile.Create();
                };

                localeDataStorage.StoreSyncData(new Mock<IModel>().Object);
                localeDataStorage.StoreSyncData(new Mock<IModel>().Object);
                localeDataStorage.StoreSyncData(new Mock<IModel>().Object);
            }
            finally
            {
                tempFile.Refresh();
                while (tempFile.Exists)
                {
                    tempFile.Delete();
                    tempFile.Refresh();
                }
            }
        }

        /// <summary>
        /// Tester, at StoreSyncData rejser eventet, der skal forberede data i det lokale datalager for læsning eller skrivning.
        /// </summary>
        [Test]
        public void TestAtStoreSyncDataRejserPrepareLocaleDataEvent()
        {
            Fixture fixture = new Fixture();

            ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            localeDataStorage.OnCreateWriterStream += (s, e) => e.Result = CreateMemoryStreamWithoutXmlContext();

            bool eventCalled = false;
            localeDataStorage.PrepareLocaleData += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(s, Is.TypeOf<LocaleDataStorage>());
                Assert.That(e, Is.Not.Null);
                Assert.That(e.LocaleDataDocument, Is.Not.Null);
                Assert.That(e.LocaleDataDocument, Is.TypeOf<XDocument>());
                Assert.That(e.ReadingContext, Is.False);
                Assert.That(e.WritingContext, Is.True);
                Assert.That(e.SynchronizationContext, Is.True);
                eventCalled = true;
            };

            Assert.That(eventCalled, Is.False);
            localeDataStorage.StoreSyncData(new Mock<IModel>().Object);
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at StoreSyncData kaster en IntranetGuiRepositoryException ved IntranetGuiRepositoryException.
        /// </summary>
        [Test]
        public void TestAtStoreSyncDataKasterIntranetGuiRepositoryExceptionVedIntranetGuiRepositoryException()
        {
            Fixture fixture = new Fixture();

            ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            IntranetGuiRepositoryException eventException = fixture.Create<IntranetGuiRepositoryException>();
            localeDataStorage.OnCreateWriterStream += (s, e) =>
            {
                throw eventException;
            };

            IntranetGuiRepositoryException exception = Assert.Throws<IntranetGuiRepositoryException>(() => localeDataStorage.StoreSyncData(new Mock<IModel>().Object));
            Assert.That(exception, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(eventException.Message));
            Assert.That(exception.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Tester, at StoreSyncData kaster en IntranetGuiRepositoryException ved Exception.
        /// </summary>
        [Test]
        public void TestAtStoreSyncDataKasterIntranetGuiRepositoryExceptionVedException()
        {
            Fixture fixture = new Fixture();

            ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            Exception eventException = fixture.Create<Exception>();
            localeDataStorage.OnCreateWriterStream += (s, e) =>
            {
                throw eventException;
            };

            IntranetGuiRepositoryException exception = Assert.Throws<IntranetGuiRepositoryException>(() => localeDataStorage.StoreSyncData(new Mock<IModel>().Object));
            Assert.That(exception, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "StoreSyncData", eventException.Message)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.EqualTo(eventException));
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Tester, at StoreSyncData gemmer et regnskab.
        /// </summary>
        [Test]
        public void TestAtStoreSyncDataGemmerRegnskabModel()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            IRegnskabModel regnskabModelMock = DomainObjectBuilder.BuildRegnskabModel(fixture, random.Next(1, 99));
            IRegnskabModel updatedRegnskabModelMock = DomainObjectBuilder.BuildRegnskabModel(fixture, regnskabModelMock.Nummer);

            FileInfo tempFile = new FileInfo(Path.GetTempFileName());
            try
            {
                ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), tempFile.FullName, FinansstyringRepositoryLocale.XmlSchema);
                Assert.That(localeDataStorage, Is.Not.Null);

                localeDataStorage.OnCreateWriterStream += (s, e) =>
                {
                    tempFile.Refresh();
                    if (tempFile.Exists)
                    {
                        e.Result = tempFile.Open(FileMode.Open, FileAccess.ReadWrite);
                        return;
                    }
                    e.Result = tempFile.Create();
                };

                localeDataStorage.StoreSyncData(regnskabModelMock);
                XmlNode regnskabNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(regnskabNode, "navn", regnskabModelMock.Navn));

                localeDataStorage.StoreSyncData(updatedRegnskabModelMock);
                XmlNode updatedRegnskabNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedRegnskabNode, "navn", updatedRegnskabModelMock.Navn));
            }
            finally
            {
                tempFile.Refresh();
                while (tempFile.Exists)
                {
                    tempFile.Delete();
                    tempFile.Refresh();
                }
            }
        }

        /// <summary>
        /// Tester, at StoreSyncData gemmer en konto.
        /// </summary>
        [Test]
        public void TestAtStoreSyncDataGemmerKontoModel()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            IRegnskabModel regnskabModelMock = DomainObjectBuilder.BuildRegnskabModel(fixture, random.Next(1, 99));
            IKontogruppeModel kontogruppeModel1Mock = DomainObjectBuilder.BuildKontogruppeModel(fixture, random.Next(1, 99));
            IKontogruppeModel kontogruppeModel2Mock = DomainObjectBuilder.BuildKontogruppeModel(fixture, random.Next(1, 99));

            DateTime statusDato = DateTime.Today.AddDays(random.Next(1, 365) * -1);
            IKontoModel kontoModelMock = DomainObjectBuilder.BuildKontoModel(fixture, random, regnskabModelMock.Nummer, fixture.Create<string>(), kontogruppeModel1Mock.Nummer, statusDato);
            IKontoModel updatedKontoModelMock = DomainObjectBuilder.BuildKontoModel(fixture, random, kontoModelMock.Regnskabsnummer, kontoModelMock.Kontonummer, kontogruppeModel2Mock.Nummer, kontoModelMock.StatusDato);
            IKontoModel updatedSaldoOnKontoModelMock = DomainObjectBuilder.BuildKontoModel(fixture, random, kontoModelMock.Regnskabsnummer, kontoModelMock.Kontonummer, kontogruppeModel2Mock.Nummer, kontoModelMock.StatusDato.AddDays(random.Next(1, 365)));

            FileInfo tempFile = new FileInfo(Path.GetTempFileName());
            try
            {
                ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), tempFile.FullName, FinansstyringRepositoryLocale.XmlSchema);
                Assert.That(localeDataStorage, Is.Not.Null);

                localeDataStorage.OnCreateWriterStream += (s, e) =>
                {
                    tempFile.Refresh();
                    if (tempFile.Exists)
                    {
                        e.Result = tempFile.Open(FileMode.Open, FileAccess.ReadWrite);
                        return;
                    }
                    e.Result = tempFile.Create();
                };

                localeDataStorage.StoreSyncData(regnskabModelMock);
                localeDataStorage.StoreSyncData(kontogruppeModel1Mock);
                localeDataStorage.StoreSyncData(kontogruppeModel2Mock);

                localeDataStorage.StoreSyncData(kontoModelMock);
                XmlNode kontoNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']/ns:Konto[@kontonummer = '{kontoModelMock.Kontonummer}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(kontoNode, "kontonavn", kontoModelMock.Kontonavn));
                Assert.That(HasAttributeWhichMatchValue(kontoNode, "beskrivelse", kontoModelMock.Beskrivelse), Is.EqualTo(string.IsNullOrWhiteSpace(kontoModelMock.Beskrivelse) == false));
                Assert.That(HasAttributeWhichMatchValue(kontoNode, "note", kontoModelMock.Notat), Is.EqualTo(string.IsNullOrWhiteSpace(kontoModelMock.Notat) == false));
                Assert.IsTrue(HasAttributeWhichMatchValue(kontoNode, "kontogruppe", kontoModelMock.Kontogruppe.ToString(CultureInfo.InvariantCulture)));

                XmlNode kontoHistorikNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']/ns:KontoHistorik[@kontonummer = '{kontoModelMock.Kontonummer}' and @dato='{kontoModelMock.StatusDato:yyyyMMdd}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(kontoHistorikNode, "kredit", kontoModelMock.Kredit.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(kontoHistorikNode, "saldo", kontoModelMock.Saldo.ToString(DecimalFormat, CultureInfo.InvariantCulture)));

                localeDataStorage.StoreSyncData(updatedKontoModelMock);
                XmlNode updatedKontoNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']/ns:Konto[@kontonummer = '{kontoModelMock.Kontonummer}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedKontoNode, "kontonavn", updatedKontoModelMock.Kontonavn));
                Assert.That(HasAttributeWhichMatchValue(updatedKontoNode, "beskrivelse", updatedKontoModelMock.Beskrivelse), Is.EqualTo(string.IsNullOrWhiteSpace(updatedKontoModelMock.Beskrivelse) == false));
                Assert.That(HasAttributeWhichMatchValue(updatedKontoNode, "note", updatedKontoModelMock.Notat), Is.EqualTo(string.IsNullOrWhiteSpace(updatedKontoModelMock.Notat) == false));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedKontoNode, "kontogruppe", updatedKontoModelMock.Kontogruppe.ToString(CultureInfo.InvariantCulture)));

                XmlNode updatedKontoHistorikNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']/ns:KontoHistorik[@kontonummer = '{kontoModelMock.Kontonummer}' and @dato='{kontoModelMock.StatusDato:yyyyMMdd}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedKontoHistorikNode, "kredit", updatedKontoModelMock.Kredit.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedKontoHistorikNode, "saldo", updatedKontoModelMock.Saldo.ToString(DecimalFormat, CultureInfo.InvariantCulture)));

                localeDataStorage.StoreSyncData(updatedSaldoOnKontoModelMock);
                XmlNode updatedSaldoOnKontoHistorikNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']/ns:KontoHistorik[@kontonummer = '{kontoModelMock.Kontonummer}' and @dato='{updatedSaldoOnKontoModelMock.StatusDato:yyyyMMdd}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedSaldoOnKontoHistorikNode, "kredit", updatedSaldoOnKontoModelMock.Kredit.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedSaldoOnKontoHistorikNode, "saldo", updatedSaldoOnKontoModelMock.Saldo.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
            }
            finally
            {
                tempFile.Refresh();
                while (tempFile.Exists)
                {
                    tempFile.Delete();
                    tempFile.Refresh();
                }
            }
        }

        /// <summary>
        /// Tester, at StoreSyncData gemmer en budgetkonto.
        /// </summary>
        [Test]
        public void TestAtStoreSyncDataGemmerBudgetkontoModel()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            IRegnskabModel regnskabModelMock = DomainObjectBuilder.BuildRegnskabModel(fixture, random.Next(1, 99));
            IBudgetkontogruppeModel budgetkontogruppeModel1Mock = DomainObjectBuilder.BuildBudgetkontogruppeModel(fixture, random.Next(1, 99));
            IBudgetkontogruppeModel budgetkontogruppeModel2Mock = DomainObjectBuilder.BuildBudgetkontogruppeModel(fixture, random.Next(1, 99));

            DateTime statusDato = DateTime.Today.AddDays(random.Next(1, 365) * -1);
            IBudgetkontoModel budgetkontoModelMock = DomainObjectBuilder.BuildBudgetkontoModel(fixture, random, regnskabModelMock.Nummer, fixture.Create<string>(), budgetkontogruppeModel1Mock.Nummer, statusDato);
            IBudgetkontoModel updatedBudgetkontoModelMock = DomainObjectBuilder.BuildBudgetkontoModel(fixture, random, budgetkontoModelMock.Regnskabsnummer, budgetkontoModelMock.Kontonummer, budgetkontogruppeModel2Mock.Nummer, budgetkontoModelMock.StatusDato);
            IBudgetkontoModel updatedSaldoOnBudgetkontoModelMock = DomainObjectBuilder.BuildBudgetkontoModel(fixture, random, budgetkontoModelMock.Regnskabsnummer, budgetkontoModelMock.Kontonummer, budgetkontogruppeModel2Mock.Nummer, budgetkontoModelMock.StatusDato.AddDays(random.Next(1, 365)));

            FileInfo tempFile = new FileInfo(Path.GetTempFileName());
            try
            {
                ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), tempFile.FullName, FinansstyringRepositoryLocale.XmlSchema);
                Assert.That(localeDataStorage, Is.Not.Null);

                localeDataStorage.OnCreateWriterStream += (s, e) =>
                {
                    tempFile.Refresh();
                    if (tempFile.Exists)
                    {
                        e.Result = tempFile.Open(FileMode.Open, FileAccess.ReadWrite);
                        return;
                    }
                    e.Result = tempFile.Create();
                };

                localeDataStorage.StoreSyncData(regnskabModelMock);
                localeDataStorage.StoreSyncData(budgetkontogruppeModel1Mock);
                localeDataStorage.StoreSyncData(budgetkontogruppeModel2Mock);

                localeDataStorage.StoreSyncData(budgetkontoModelMock);
                XmlNode budgetkontoNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']/ns:Budgetkonto[@kontonummer = '{budgetkontoModelMock.Kontonummer}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(budgetkontoNode, "kontonavn", budgetkontoModelMock.Kontonavn));
                Assert.That(HasAttributeWhichMatchValue(budgetkontoNode, "beskrivelse", budgetkontoModelMock.Beskrivelse), Is.EqualTo(string.IsNullOrWhiteSpace(budgetkontoModelMock.Beskrivelse) == false));
                Assert.That(HasAttributeWhichMatchValue(budgetkontoNode, "note", budgetkontoModelMock.Notat), Is.EqualTo(string.IsNullOrWhiteSpace(budgetkontoModelMock.Notat) == false));
                Assert.IsTrue(HasAttributeWhichMatchValue(budgetkontoNode, "kontogruppe", budgetkontoModelMock.Kontogruppe.ToString(CultureInfo.InvariantCulture)));

                XmlNode budgetkontoHistorikNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']/ns:BudgetkontoHistorik[@kontonummer = '{budgetkontoModelMock.Kontonummer}' and @dato='{budgetkontoModelMock.StatusDato:yyyyMMdd}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(budgetkontoHistorikNode, "indtaegter", budgetkontoModelMock.Indtægter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(budgetkontoHistorikNode, "udgifter", budgetkontoModelMock.Udgifter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(budgetkontoHistorikNode, "bogfoert", budgetkontoModelMock.Bogført.ToString(DecimalFormat, CultureInfo.InvariantCulture)));

                DateTime lastMonthStatusDato = new DateTime(budgetkontoModelMock.StatusDato.AddMonths(-1).Year, budgetkontoModelMock.StatusDato.AddMonths(-1).Month, DateTime.DaysInMonth(budgetkontoModelMock.StatusDato.AddMonths(-1).Year, budgetkontoModelMock.StatusDato.AddMonths(-1).Month));
                decimal lastMonthIndtægter = Math.Abs(budgetkontoModelMock.BudgetSidsteMåned > 0 ? budgetkontoModelMock.BudgetSidsteMåned : 0M);
                decimal lastMonthUdgifter = Math.Abs(budgetkontoModelMock.BudgetSidsteMåned < 0 ? budgetkontoModelMock.BudgetSidsteMåned : 0M);
                XmlNode lastMonthBudgetkontoHistorikNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']/ns:BudgetkontoHistorik[@kontonummer = '{budgetkontoModelMock.Kontonummer}' and @dato='{lastMonthStatusDato:yyyyMMdd}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(lastMonthBudgetkontoHistorikNode, "indtaegter", lastMonthIndtægter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(lastMonthBudgetkontoHistorikNode, "udgifter", lastMonthUdgifter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(lastMonthBudgetkontoHistorikNode, "bogfoert", budgetkontoModelMock.BogførtSidsteMåned.ToString(DecimalFormat, CultureInfo.InvariantCulture)));

                localeDataStorage.StoreSyncData(updatedBudgetkontoModelMock);
                XmlNode updatedBudgetkontoNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']/ns:Budgetkonto[@kontonummer = '{budgetkontoModelMock.Kontonummer}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBudgetkontoNode, "kontonavn", updatedBudgetkontoModelMock.Kontonavn));
                Assert.That(HasAttributeWhichMatchValue(updatedBudgetkontoNode, "beskrivelse", updatedBudgetkontoModelMock.Beskrivelse), Is.EqualTo(string.IsNullOrWhiteSpace(updatedBudgetkontoModelMock.Beskrivelse) == false));
                Assert.That(HasAttributeWhichMatchValue(updatedBudgetkontoNode, "note", updatedBudgetkontoModelMock.Notat), Is.EqualTo(string.IsNullOrWhiteSpace(updatedBudgetkontoModelMock.Notat) == false));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBudgetkontoNode, "kontogruppe", updatedBudgetkontoModelMock.Kontogruppe.ToString(CultureInfo.InvariantCulture)));

                XmlNode updatedBudgetkontoHistorikNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']/ns:BudgetkontoHistorik[@kontonummer = '{budgetkontoModelMock.Kontonummer}' and @dato='{budgetkontoModelMock.StatusDato:yyyyMMdd}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBudgetkontoHistorikNode, "indtaegter", updatedBudgetkontoModelMock.Indtægter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBudgetkontoHistorikNode, "udgifter", updatedBudgetkontoModelMock.Udgifter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBudgetkontoHistorikNode, "bogfoert", updatedBudgetkontoModelMock.Bogført.ToString(DecimalFormat, CultureInfo.InvariantCulture)));

                lastMonthStatusDato = new DateTime(updatedBudgetkontoModelMock.StatusDato.AddMonths(-1).Year, updatedBudgetkontoModelMock.StatusDato.AddMonths(-1).Month, DateTime.DaysInMonth(updatedBudgetkontoModelMock.StatusDato.AddMonths(-1).Year, updatedBudgetkontoModelMock.StatusDato.AddMonths(-1).Month));
                lastMonthIndtægter = Math.Abs(updatedBudgetkontoModelMock.BudgetSidsteMåned > 0 ? updatedBudgetkontoModelMock.BudgetSidsteMåned : 0M);
                lastMonthUdgifter = Math.Abs(updatedBudgetkontoModelMock.BudgetSidsteMåned < 0 ? updatedBudgetkontoModelMock.BudgetSidsteMåned : 0M);
                XmlNode lastMonthUpdatedBudgetkontoHistorikNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']/ns:BudgetkontoHistorik[@kontonummer = '{budgetkontoModelMock.Kontonummer}' and @dato='{lastMonthStatusDato:yyyyMMdd}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(lastMonthUpdatedBudgetkontoHistorikNode, "indtaegter", lastMonthIndtægter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(lastMonthUpdatedBudgetkontoHistorikNode, "udgifter", lastMonthUdgifter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(lastMonthUpdatedBudgetkontoHistorikNode, "bogfoert", updatedBudgetkontoModelMock.BogførtSidsteMåned.ToString(DecimalFormat, CultureInfo.InvariantCulture)));

                localeDataStorage.StoreSyncData(updatedSaldoOnBudgetkontoModelMock);
                XmlNode updatedSaldoOnBudgetkontoHistorikNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']/ns:BudgetkontoHistorik[@kontonummer = '{budgetkontoModelMock.Kontonummer}' and @dato='{updatedSaldoOnBudgetkontoModelMock.StatusDato:yyyyMMdd}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedSaldoOnBudgetkontoHistorikNode, "indtaegter", updatedSaldoOnBudgetkontoModelMock.Indtægter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedSaldoOnBudgetkontoHistorikNode, "udgifter", updatedSaldoOnBudgetkontoModelMock.Udgifter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedSaldoOnBudgetkontoHistorikNode, "bogfoert", updatedSaldoOnBudgetkontoModelMock.Bogført.ToString(DecimalFormat, CultureInfo.InvariantCulture)));

                lastMonthStatusDato = new DateTime(updatedSaldoOnBudgetkontoModelMock.StatusDato.AddMonths(-1).Year, updatedSaldoOnBudgetkontoModelMock.StatusDato.AddMonths(-1).Month, DateTime.DaysInMonth(updatedSaldoOnBudgetkontoModelMock.StatusDato.AddMonths(-1).Year, updatedSaldoOnBudgetkontoModelMock.StatusDato.AddMonths(-1).Month));
                lastMonthIndtægter = Math.Abs(updatedSaldoOnBudgetkontoModelMock.BudgetSidsteMåned > 0 ? updatedSaldoOnBudgetkontoModelMock.BudgetSidsteMåned : 0M);
                lastMonthUdgifter = Math.Abs(updatedSaldoOnBudgetkontoModelMock.BudgetSidsteMåned < 0 ? updatedSaldoOnBudgetkontoModelMock.BudgetSidsteMåned : 0M);
                XmlNode lastMonthUpdatedSaldoOnBudgetkontoHistorikNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']/ns:BudgetkontoHistorik[@kontonummer = '{budgetkontoModelMock.Kontonummer}' and @dato='{lastMonthStatusDato:yyyyMMdd}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(lastMonthUpdatedSaldoOnBudgetkontoHistorikNode, "indtaegter", lastMonthIndtægter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(lastMonthUpdatedSaldoOnBudgetkontoHistorikNode, "udgifter", lastMonthUdgifter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(lastMonthUpdatedSaldoOnBudgetkontoHistorikNode, "bogfoert", updatedSaldoOnBudgetkontoModelMock.BogførtSidsteMåned.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
            }
            finally
            {
                tempFile.Refresh();
                while (tempFile.Exists)
                {
                    tempFile.Delete();
                    tempFile.Refresh();
                }
            }
        }

        /// <summary>
        /// Tester, at StoreSyncData gemmer en adressekonto.
        /// </summary>
        [Test]
        public void TestAtStoreSyncDataGemmerAdressekontoModel()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            IRegnskabModel regnskabModelMock = DomainObjectBuilder.BuildRegnskabModel(fixture, random.Next(1, 99));

            DateTime statusDato = DateTime.Today.AddDays(random.Next(1, 365) * -1);
            IAdressekontoModel adressekontoModelMock = DomainObjectBuilder.BuildAdressekontoModel(fixture, random, regnskabModelMock.Nummer, fixture.Create<int>(), statusDato);
            IAdressekontoModel updatedAdressekontoModelMock = DomainObjectBuilder.BuildAdressekontoModel(fixture, random, adressekontoModelMock.Regnskabsnummer, adressekontoModelMock.Nummer, adressekontoModelMock.StatusDato);
            IAdressekontoModel updatedSaldoOnAdressekontoModelMock = DomainObjectBuilder.BuildAdressekontoModel(fixture, random, adressekontoModelMock.Regnskabsnummer, adressekontoModelMock.Nummer, adressekontoModelMock.StatusDato.AddDays(random.Next(1, 365)));

            FileInfo tempFile = new FileInfo(Path.GetTempFileName());
            try
            {
                ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), tempFile.FullName, FinansstyringRepositoryLocale.XmlSchema);
                Assert.That(localeDataStorage, Is.Not.Null);

                localeDataStorage.OnCreateWriterStream += (s, e) =>
                {
                    tempFile.Refresh();
                    if (tempFile.Exists)
                    {
                        e.Result = tempFile.Open(FileMode.Open, FileAccess.ReadWrite);
                        return;
                    }
                    e.Result = tempFile.Create();
                };

                localeDataStorage.StoreSyncData(regnskabModelMock);

                localeDataStorage.StoreSyncData(adressekontoModelMock);
                XmlNode adressekontoNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']/ns:Adressekonto[@nummer = '{adressekontoModelMock.Nummer.ToString(CultureInfo.InvariantCulture)}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(adressekontoNode, "navn", adressekontoModelMock.Navn));
                Assert.That(HasAttributeWhichMatchValue(adressekontoNode, "primaerTelefon", adressekontoModelMock.PrimærTelefon), Is.EqualTo(string.IsNullOrWhiteSpace(adressekontoModelMock.PrimærTelefon) == false));
                Assert.That(HasAttributeWhichMatchValue(adressekontoNode, "sekundaerTelefon", adressekontoModelMock.SekundærTelefon), Is.EqualTo(string.IsNullOrWhiteSpace(adressekontoModelMock.SekundærTelefon) == false));

                XmlNode adressekontoHistorikNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']/ns:AdressekontoHistorik[@nummer = '{adressekontoModelMock.Nummer.ToString(CultureInfo.InvariantCulture)}' and @dato='{adressekontoModelMock.StatusDato:yyyyMMdd}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(adressekontoHistorikNode, "saldo", adressekontoModelMock.Saldo.ToString(DecimalFormat, CultureInfo.InvariantCulture)));

                localeDataStorage.StoreSyncData(updatedAdressekontoModelMock);
                XmlNode updatedAdressekontoNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']/ns:Adressekonto[@nummer = '{adressekontoModelMock.Nummer.ToString(CultureInfo.InvariantCulture)}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedAdressekontoNode, "navn", updatedAdressekontoModelMock.Navn));
                Assert.That(HasAttributeWhichMatchValue(updatedAdressekontoNode, "primaerTelefon", updatedAdressekontoModelMock.PrimærTelefon), Is.EqualTo(string.IsNullOrWhiteSpace(updatedAdressekontoModelMock.PrimærTelefon) == false));
                Assert.That(HasAttributeWhichMatchValue(updatedAdressekontoNode, "sekundaerTelefon", updatedAdressekontoModelMock.SekundærTelefon), Is.EqualTo(string.IsNullOrWhiteSpace(updatedAdressekontoModelMock.SekundærTelefon) == false));

                XmlNode updatedAdressekontoHistorikNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']/ns:AdressekontoHistorik[@nummer = '{adressekontoModelMock.Nummer.ToString(CultureInfo.InvariantCulture)}' and @dato='{adressekontoModelMock.StatusDato:yyyyMMdd}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedAdressekontoHistorikNode, "saldo", updatedAdressekontoModelMock.Saldo.ToString(DecimalFormat, CultureInfo.InvariantCulture)));

                localeDataStorage.StoreSyncData(updatedSaldoOnAdressekontoModelMock);
                XmlNode updatedSaldoOnAdressekontoHistorikNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']/ns:AdressekontoHistorik[@nummer = '{updatedSaldoOnAdressekontoModelMock.Nummer.ToString(CultureInfo.InvariantCulture)}' and @dato='{updatedSaldoOnAdressekontoModelMock.StatusDato:yyyyMMdd}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedSaldoOnAdressekontoHistorikNode, "saldo", updatedSaldoOnAdressekontoModelMock.Saldo.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
            }
            finally
            {
                tempFile.Refresh();
                while (tempFile.Exists)
                {
                    tempFile.Delete();
                    tempFile.Refresh();
                }
            }
        }

        /// <summary>
        /// Tester, at StoreSyncData gemmer en bogføringslinje.
        /// </summary>
        [Test]
        public void TestAtStoreSyncDataGemmerBogføringslinjeModel()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            IRegnskabModel regnskabModelMock = DomainObjectBuilder.BuildRegnskabModel(fixture, random.Next(1, 99));

            DateTime statusDato = DateTime.Today.AddDays(random.Next(1, 365)*-1);
            IBogføringslinjeModel bogføringslinjeModelMock = DomainObjectBuilder.BuildBogføringslinjeModel(fixture, random, regnskabModelMock.Nummer, fixture.Create<int>(), statusDato, fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>());
            IBogføringslinjeModel updatedBogføringslinjeModelMock = DomainObjectBuilder.BuildBogføringslinjeModel(fixture, random, bogføringslinjeModelMock.Regnskabsnummer, bogføringslinjeModelMock.Løbenummer, statusDato.AddDays(random.Next(1, 365)), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>());

            FileInfo tempFile = new FileInfo(Path.GetTempFileName());
            try
            {
                ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), tempFile.FullName, FinansstyringRepositoryLocale.XmlSchema);
                Assert.That(localeDataStorage, Is.Not.Null);

                localeDataStorage.OnCreateWriterStream += (s, e) =>
                {
                    tempFile.Refresh();
                    if (tempFile.Exists)
                    {
                        e.Result = tempFile.Open(FileMode.Open, FileAccess.ReadWrite);
                        return;
                    }
                    e.Result = tempFile.Create();
                };

                localeDataStorage.StoreSyncData(regnskabModelMock);

                localeDataStorage.StoreSyncData(bogføringslinjeModelMock);
                XmlNode bogføringslinjeNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']/ns:Bogfoeringslinje[@loebenummer = '{bogføringslinjeModelMock.Løbenummer.ToString(CultureInfo.InvariantCulture)}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(bogføringslinjeNode, "dato", bogføringslinjeModelMock.Dato.ToString("yyyyMMdd")));
                Assert.That(HasAttributeWhichMatchValue(bogføringslinjeNode, "bilag", bogføringslinjeModelMock.Bilag), Is.EqualTo(string.IsNullOrWhiteSpace(bogføringslinjeModelMock.Bilag) == false));
                Assert.IsTrue(HasAttributeWhichMatchValue(bogføringslinjeNode, "kontonummer", bogføringslinjeModelMock.Kontonummer));
                Assert.IsTrue(HasAttributeWhichMatchValue(bogføringslinjeNode, "tekst", bogføringslinjeModelMock.Tekst));
                Assert.That(HasAttributeWhichMatchValue(bogføringslinjeNode, "budgetkontonummer", bogføringslinjeModelMock.Budgetkontonummer), Is.EqualTo(string.IsNullOrWhiteSpace(bogføringslinjeModelMock.Budgetkontonummer) == false));
                Assert.That(HasAttributeWhichMatchValue(bogføringslinjeNode, "debit", bogføringslinjeModelMock.Debit.ToString(DecimalFormat, CultureInfo.InvariantCulture)), Is.EqualTo(bogføringslinjeModelMock.Debit > 0M));
                Assert.That(HasAttributeWhichMatchValue(bogføringslinjeNode, "kredit", bogføringslinjeModelMock.Kredit.ToString(DecimalFormat, CultureInfo.InvariantCulture)), Is.EqualTo(bogføringslinjeModelMock.Kredit > 0M));
                Assert.That(HasAttributeWhichMatchValue(bogføringslinjeNode, "adressekonto", bogføringslinjeModelMock.Adressekonto.ToString(CultureInfo.InvariantCulture)), Is.EqualTo(bogføringslinjeModelMock.Adressekonto != 0));
                Assert.IsTrue(HasAttributeWhichMatchValue(bogføringslinjeNode, "synkroniseret", "true"));
                Assert.IsFalse(HasAttributeWhichMatchValue(bogføringslinjeNode, "verserende", "true"));

                localeDataStorage.StoreSyncData(updatedBogføringslinjeModelMock);
                XmlNode updatedBogføringslinjeNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{regnskabModelMock.Nummer}']/ns:Bogfoeringslinje[@loebenummer = '{bogføringslinjeModelMock.Løbenummer.ToString(CultureInfo.InvariantCulture)}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBogføringslinjeNode, "dato", updatedBogføringslinjeModelMock.Dato.ToString("yyyyMMdd")));
                Assert.That(HasAttributeWhichMatchValue(updatedBogføringslinjeNode, "bilag", updatedBogføringslinjeModelMock.Bilag), Is.EqualTo(string.IsNullOrWhiteSpace(updatedBogføringslinjeModelMock.Bilag) == false));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBogføringslinjeNode, "kontonummer", updatedBogføringslinjeModelMock.Kontonummer));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBogføringslinjeNode, "tekst", updatedBogføringslinjeModelMock.Tekst));
                Assert.That(HasAttributeWhichMatchValue(updatedBogføringslinjeNode, "budgetkontonummer", updatedBogføringslinjeModelMock.Budgetkontonummer), Is.EqualTo(string.IsNullOrWhiteSpace(updatedBogføringslinjeModelMock.Budgetkontonummer) == false));
                Assert.That(HasAttributeWhichMatchValue(updatedBogføringslinjeNode, "debit", updatedBogføringslinjeModelMock.Debit.ToString(DecimalFormat, CultureInfo.InvariantCulture)), Is.EqualTo(updatedBogføringslinjeModelMock.Debit > 0M));
                Assert.That(HasAttributeWhichMatchValue(updatedBogføringslinjeNode, "kredit", updatedBogføringslinjeModelMock.Kredit.ToString(DecimalFormat, CultureInfo.InvariantCulture)), Is.EqualTo(updatedBogføringslinjeModelMock.Kredit > 0M));
                Assert.That(HasAttributeWhichMatchValue(updatedBogføringslinjeNode, "adressekonto", updatedBogføringslinjeModelMock.Adressekonto.ToString(CultureInfo.InvariantCulture)), Is.EqualTo(updatedBogføringslinjeModelMock.Adressekonto != 0));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBogføringslinjeNode, "synkroniseret", "true"));
                Assert.IsFalse(HasAttributeWhichMatchValue(updatedBogføringslinjeNode, "verserende", "true"));
            }
            finally
            {
                tempFile.Refresh();
                while (tempFile.Exists)
                {
                    tempFile.Delete();
                    tempFile.Refresh();
                }
            }
        }

        /// <summary>
        /// Tester, at StoreSyncData gemmer en kontogruppe.
        /// </summary>
        [Test]
        public void TestAtStoreSyncDataGemmerKontogruppeModel()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            IKontogruppeModel kontogruppeModelMock = DomainObjectBuilder.BuildKontogruppeModel(fixture, random.Next(1, 99), Balancetype.Aktiver);
            IKontogruppeModel updatedKontogruppeModelMock = DomainObjectBuilder.BuildKontogruppeModel(fixture, kontogruppeModelMock.Nummer, Balancetype.Passiver);

            FileInfo tempFile = new FileInfo(Path.GetTempFileName());
            try
            {
                ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), tempFile.FullName, FinansstyringRepositoryLocale.XmlSchema);
                Assert.That(localeDataStorage, Is.Not.Null);

                localeDataStorage.OnCreateWriterStream += (s, e) =>
                {
                    tempFile.Refresh();
                    if (tempFile.Exists)
                    {
                        e.Result = tempFile.Open(FileMode.Open, FileAccess.ReadWrite);
                        return;
                    }
                    e.Result = tempFile.Create();
                };

                localeDataStorage.StoreSyncData(kontogruppeModelMock);
                XmlNode kontogruppeNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Kontogruppe[@nummer = '{kontogruppeModelMock.Nummer}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(kontogruppeNode, "tekst", kontogruppeModelMock.Tekst));
                Assert.IsTrue(HasAttributeWhichMatchValue(kontogruppeNode, "balanceType", kontogruppeModelMock.Balancetype.ToString()));

                localeDataStorage.StoreSyncData(updatedKontogruppeModelMock);
                XmlNode updatedKontogruppeNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Kontogruppe[@nummer = '{kontogruppeModelMock.Nummer}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedKontogruppeNode, "tekst", updatedKontogruppeModelMock.Tekst));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedKontogruppeNode, "balanceType", updatedKontogruppeModelMock.Balancetype.ToString()));
            }
            finally
            {
                tempFile.Refresh();
                while (tempFile.Exists)
                {
                    tempFile.Delete();
                    tempFile.Refresh();
                }
            }
        }

        /// <summary>
        /// Tester, at StoreSyncData gemmer en kontogruppe til budgetkonti.
        /// </summary>
        [Test]
        public void TestAtStoreSyncDataGemmerBudgetkontogruppeModel()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            IBudgetkontogruppeModel budgetkontogruppeModelMock = DomainObjectBuilder.BuildBudgetkontogruppeModel(fixture, random.Next(1, 99));
            IBudgetkontogruppeModel updatedBudgetkontogruppeModelMock = DomainObjectBuilder.BuildBudgetkontogruppeModel(fixture, budgetkontogruppeModelMock.Nummer);

            FileInfo tempFile = new FileInfo(Path.GetTempFileName());
            try
            {
                ILocaleDataStorage localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), tempFile.FullName, FinansstyringRepositoryLocale.XmlSchema);
                Assert.That(localeDataStorage, Is.Not.Null);

                localeDataStorage.OnCreateWriterStream += (s, e) =>
                {
                    tempFile.Refresh();
                    if (tempFile.Exists)
                    {
                        e.Result = tempFile.Open(FileMode.Open, FileAccess.ReadWrite);
                        return;
                    }
                    e.Result = tempFile.Create();
                };

                localeDataStorage.StoreSyncData(budgetkontogruppeModelMock);
                XmlNode budgetkontogruppeNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Budgetkontogruppe[@nummer = '{budgetkontogruppeModelMock.Nummer}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(budgetkontogruppeNode, "tekst", budgetkontogruppeModelMock.Tekst));

                localeDataStorage.StoreSyncData(updatedBudgetkontogruppeModelMock);
                XmlNode updatedBudgetkontogruppeNode = GetNodeFromXPath(tempFile.FullName, $"/ns:FinansstyringRepository/ns:Budgetkontogruppe[@nummer = '{budgetkontogruppeModelMock.Nummer}']");
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBudgetkontogruppeNode, "tekst", updatedBudgetkontogruppeModelMock.Tekst));
            }
            finally
            {
                tempFile.Refresh();
                while (tempFile.Exists)
                {
                    tempFile.Delete();
                    tempFile.Refresh();
                }
            }
        }

        /// <summary>
        /// Danner en stream indeholdende XML, der kan benyttes til test.
        /// </summary>
        private static Stream CreateMemoryStreamWithXmlContent()
        {
            MemoryStream memoryStream = new MemoryStream();

            StreamWriter streamWriter = new StreamWriter(memoryStream, Encoding.UTF8);
            streamWriter.Write("<?xml version=\"1.0\" encoding=\"utf-8\"?><FinansstyringRepository xmlns=\"{0}\" version=\"1.0\"/>", FinansstyringRepositoryLocale.Namespace);
            streamWriter.Flush();
 
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        /// <summary>
        /// Danner en stream uden XML indhold, der kan benyttes  test.
        /// </summary>
        /// <returns></returns>
        private static Stream CreateMemoryStreamWithoutXmlContext()
        {
            return new MemoryStream();
        }

        /// <summary>
        /// Finder og returnerer en given node i et givent XML dokument på baggrund af en xpath.
        /// </summary>
        /// <param name="localeDataFileName">Filnavnet på XML dokumentet, hvorfra en given node skal returneres.</param>
        /// <param name="xpath">XPath til noden, der ønskes returneret.</param>
        /// <returns>XML node.</returns>
        private static XmlNode GetNodeFromXPath(string localeDataFileName, string xpath)
        {
            if (string.IsNullOrWhiteSpace(localeDataFileName))
            {
                throw new ArgumentNullException(nameof(localeDataFileName));
            }

            if (string.IsNullOrWhiteSpace(xpath))
            {
                throw new ArgumentNullException(nameof(xpath));
            }

            XmlDocument localeDataDocument = new XmlDocument();
            localeDataDocument.Load(localeDataFileName);

            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(localeDataDocument.NameTable);
            namespaceManager.AddNamespace("ns", FinansstyringRepositoryLocale.Namespace);

            return localeDataDocument.SelectSingleNode(xpath, namespaceManager);
        }

        /// <summary>
        /// Undersøger om en given node har en givent attribute med en given værdi.
        /// </summary>
        /// <param name="xmlNode">XML noden, der skal have den givne attribute.</param>
        /// <param name="attributeName">Navnet på attributten, som skal have den givne værdi.</param>
        /// <param name="attributeValue">Værdi, som den givne attribute skal have.</param>
        /// <returns>True, hvis den givne attribute findes og har den givne værdi, ellers false.</returns>
        private static bool HasAttributeWhichMatchValue(XmlNode xmlNode, string attributeName, string attributeValue)
        {
            if (xmlNode == null)
            {
                throw new ArgumentNullException(nameof(xmlNode));
            }

            if (string.IsNullOrWhiteSpace(attributeName))
            {
                throw new ArgumentNullException(nameof(attributeName));
            }

            XmlAttribute attribute = xmlNode.Attributes?[attributeName];
            if (attribute == null)
            {
                return false;
            }

            return string.Compare(attribute.Value, attributeValue, StringComparison.InvariantCulture) == 0;
        }
    }
}