using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Resources;
using Rhino.Mocks;

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
            var localeDataFileName = Path.GetFileName(Path.GetTempFileName());
            var syncDataFileName = Path.GetFileName(Path.GetTempFileName());
            var localeDataStorage = new LocaleDataStorage(localeDataFileName, syncDataFileName, FinansstyringRepositoryLocale.XmlSchema);
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
        public void TestAtConstructorKasterArgumentNullExceptionHvisLocaleDataFileNameErInvalid(string invalidValue)
        {
            var fixture = new Fixture();

            var exception = Assert.Throws<ArgumentNullException>(() => new LocaleDataStorage(invalidValue, fixture.Create<string>(), fixture.Create<string>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("localeDataFileName"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis indeholdende synkroniseringsdata i det lokale datalager er null.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestAtConstructorKasterArgumentNullExceptionHvisSyncDataFileNameErInvalid(string invalidValue)
        {
            var fixture = new Fixture();

            var exception = Assert.Throws<ArgumentNullException>(() => new LocaleDataStorage(fixture.Create<string>(), invalidValue, fixture.Create<string>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("syncDataFileName"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis lokation for XML schema, der benyttes til validering af de lokale data, er null.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestAtConstructorKasterArgumentNullExceptionHvisSchemaLocationErInvalid(string invalidValue)
        {
            var fixture = new Fixture();

            var exception = Assert.Throws<ArgumentNullException>(() => new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), invalidValue));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("schemaLocation"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at HasLocaleData returnerer false, der rejses, når det skal evalueres, om der findes et lokalt datalager indeholdende data, er null.
        /// </summary>
        [Test]
        public void TestAtHasLocaleDataReturnererFalseHvisOnHasLocaleDataErNull()
        {
            var fixture = new Fixture();

            var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>());
            Assert.That(localeDataStorage, Is.Not.Null);
            Assert.That(localeDataStorage.HasLocaleData, Is.False);
        }

        /// <summary>
        /// Tester, at HasLocaleData rejser eventet, der skal evaluere, om der findes et lokalt datalager indeholdende data.
        /// </summary>
        [Test]
        public void TestAtHasLocaleDataRejserEvent()
        {
            var fixture = new Fixture();

            var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>());
            Assert.That(localeDataStorage, Is.Not.Null);

            var eventCalled = false;
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
            var fixture = new Fixture();

            var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>());
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
            var fixture = new Fixture();

            var localeDataFileName = Path.GetTempFileName();
            var localeDataStorage = new LocaleDataStorage(localeDataFileName, fixture.Create<string>(), fixture.Create<string>());
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
            var fixture = new Fixture();

            var syncDataFileName = Path.GetTempFileName();
            var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), syncDataFileName, fixture.Create<string>());
            Assert.That(localeDataStorage, Is.Not.Null);
            Assert.That(localeDataStorage.SyncDataFileName, Is.Not.Null);
            Assert.That(localeDataStorage.SyncDataFileName, Is.Not.Empty);
            Assert.That(localeDataStorage.SyncDataFileName.Contains(Path.DirectorySeparatorChar), Is.False);
            Assert.That(localeDataStorage.SyncDataFileName, Is.EqualTo(Path.GetFileName(syncDataFileName)));

        }

        /// <summary>
        /// Tester, at Schema kaster en IntranetGuiRepositoryException, hvis skemaet ikke findes.
        /// </summary>
        [Test]
        public void TestAtSchemaKasterIntranetGuiRepositoryExceptionHvisSchemaLocationIkkeFindes()
        {
            var fixture = new Fixture();

            var resourceName = fixture.Create<string>();
            var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), resourceName);
            Assert.That(localeDataStorage, Is.Not.Null);

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            var exception = Assert.Throws<IntranetGuiRepositoryException>(() => localeDataStorage.Schema.ToString());
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.UnableToLoadResource, resourceName)));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at Schema giver en XML reader indeholdende XML schema til validering af data i det lokale datalager. 
        /// </summary>
        [Test]
        public void TestAtSchemaGiverXmlReaderIndeholdendeXmlSchema()
        {
            var fixture = new Fixture();

            var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            var schemaDocument = localeDataStorage.Schema;
            Assert.That(schemaDocument, Is.Not.Null);

            using (var reader = schemaDocument.CreateReader())
            {
                var schema = XmlSchema.Read(reader, ValidationEventHandler);
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
            var fixture = new Fixture();

            var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>());
            Assert.That(localeDataStorage, Is.Not.Null);

            var exception = Assert.Throws<IntranetGuiRepositoryException>(() => localeDataStorage.GetLocaleData());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.EventHandlerNotDefined, "OnCreateReaderStream")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at GetLocaleData rejser eventet, der skal danne læsestreamen til det lokale datalager.
        /// </summary>
        [Test]
        public void TestAtGetLocaleDataRejserOnCreateReaderStreamEvent()
        {
            var fixture = new Fixture();

            var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>());
            Assert.That(localeDataStorage, Is.Not.Null);

            var eventCalled = false;
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
            var fixture = new Fixture();

            var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            localeDataStorage.OnCreateReaderStream += (s, e) => e.Result = CreateMemoryStreamWithXmlContent();

            XmlSchema schema;
            using (var schemaReader = localeDataStorage.Schema.CreateReader())
            {
                schema = XmlSchema.Read(schemaReader, ValidationEventHandler);
                schemaReader.Close();
            }
            Assert.That(schema, Is.Not.Null);

            var localeData = new XmlDocument();
            using (var localeDataReader = localeDataStorage.GetLocaleData().CreateReader())
            {
                var readerSettings = new XmlReaderSettings
                {
                    IgnoreComments = true,
                    IgnoreProcessingInstructions = true,
                    IgnoreWhitespace = true,
                    ValidationType = ValidationType.Schema
                };
                readerSettings.Schemas.Add(schema);
                readerSettings.ValidationEventHandler += ValidationEventHandler;
                using (var reader = XmlReader.Create(localeDataReader, readerSettings))
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
            var fixture = new Fixture();

            var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            localeDataStorage.OnCreateReaderStream += (s, e) => e.Result = CreateMemoryStreamWithXmlContent();

            var eventCalled = false;
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
            var fixture = new Fixture();

            var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>());
            Assert.That(localeDataStorage, Is.Not.Null);

            var eventException = fixture.Create<IntranetGuiRepositoryException>();
            localeDataStorage.OnCreateReaderStream += (s, e) =>
            {
                throw eventException;
            };

            var exception = Assert.Throws<IntranetGuiRepositoryException>(() => localeDataStorage.GetLocaleData());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(eventException.Message));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at GetLocaleData kaster en IntranetGuiRepositoryException ved Exception.
        /// </summary>
        [Test]
        public void TestAtGetLocaleDataKasterIntranetGuiRepositoryExceptionVedException()
        {
            var fixture = new Fixture();

            var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>());
            Assert.That(localeDataStorage, Is.Not.Null);

            var eventException = fixture.Create<Exception>();
            localeDataStorage.OnCreateReaderStream += (s, e) =>
            {
                throw eventException;
            };

            var exception = Assert.Throws<IntranetGuiRepositoryException>(() => localeDataStorage.GetLocaleData());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "GetLocaleData", eventException.Message)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.EqualTo(eventException));
        }

        /// <summary>
        /// Tester, at StoreLocaleDocument kaster en ArgumentNullException, hvis XML dokumentet er null.
        /// </summary>
        [Test]
        public void TestAtStoreLocaleDocumentKasterArgumentNullExceptionHvisLocaleDataDocumentErNull()
        {
            var fixture = new Fixture();

            var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>());
            Assert.That(localeDataStorage, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => localeDataStorage.StoreLocaleDocument(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("localeDataDocument"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at StoreLocaleDocument kaster en IntranetGuiRepositoryException, hvis eventet, der rejses, når en skrivestream til det lokale datalager skal dannes, er null.
        /// </summary>
        [Test]
        public void TestAtStoreLocaleDocumentKasterIntranetGuiRepositoryExceptionHvisOnOnCreateWriterStreamErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IModel>()));

            var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>());
            Assert.That(localeDataStorage, Is.Not.Null);

            var exception = Assert.Throws<IntranetGuiRepositoryException>(() => localeDataStorage.StoreLocaleDocument(new XDocument()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.EventHandlerNotDefined, "OnCreateWriterStream")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at StoreLocaleDocument rejser eventet, der skal danne skrivestreamen til det lokale datalager.
        /// </summary>
        [Test]
        public void TestAtStoreLocaleDocumentRejserOnCreateWriterStreamEvent()
        {
            var fixture = new Fixture();
            fixture.Customize<IModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IModel>()));

            var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            var eventCalled = false;
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

            var localeDataDocument = new XDocument(new XDeclaration("1.0", Encoding.UTF8.BodyName, null));
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
            var fixture = new Fixture();
            fixture.Customize<IModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IModel>()));

            var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            localeDataStorage.OnCreateWriterStream += (s, e) => e.Result = CreateMemoryStreamWithoutXmlContext();

            var localeDataDocument = new XDocument(new XDeclaration("1.0", Encoding.UTF8.BodyName, null));
            localeDataDocument.Add(new XElement(XName.Get("Test")));

            localeDataStorage.StoreLocaleDocument(localeDataDocument);
        }

        /// <summary>
        /// Tester, at StoreLocaleDocument rejser eventet, der skal forberede data i det lokale datalager for læsning eller skrivning.
        /// </summary>
        [Test]
        public void TestAtStoreLocaleDocumentRejserPrepareLocaleDataEvent()
        {
            var fixture = new Fixture();

            var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            localeDataStorage.OnCreateWriterStream += (s, e) => e.Result = CreateMemoryStreamWithoutXmlContext();

            var localeDataDocument = new XDocument(new XDeclaration("1.0", Encoding.UTF8.BodyName, null));
            localeDataDocument.Add(new XElement(XName.Get("Test")));

            var eventCalled = false;
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
            var fixture = new Fixture();

            var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>());
            Assert.That(localeDataStorage, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => localeDataStorage.StoreLocaleData((IModel) null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("model"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at StoreLocaleData kaster en IntranetGuiRepositoryException, hvis eventet, der rejses, når en skrivestream til det lokale datalager skal dannes, er null.
        /// </summary>
        [Test]
        public void TestAtStoreLocaleDataKasterIntranetGuiRepositoryExceptionHvisOnOnCreateWriterStreamErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IModel>()));

            var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>());
            Assert.That(localeDataStorage, Is.Not.Null);

            var exception = Assert.Throws<IntranetGuiRepositoryException>(() => localeDataStorage.StoreLocaleData(fixture.Create<IModel>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.EventHandlerNotDefined, "OnCreateWriterStream")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at StoreLocaleData rejser eventet, der skal danne skrivestreamen til det lokale datalager.
        /// </summary>
        [Test]
        public void TestAtStoreLocaleDataRejserOnCreateWriterStreamEvent()
        {
            var fixture = new Fixture();
            fixture.Customize<IModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IModel>()));

            var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            var eventCalled = false;
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
            localeDataStorage.StoreLocaleData(fixture.Create<IModel>());
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at StoreLocaleData gemmer data i det lokale datalager, når dette ikke har indhold.
        /// </summary>
        [Test]
        public void TestAtStoreLocaleDataGemmerDataWhenLocaleDataStorageIkkeHarData()
        {
            var fixture = new Fixture();
            fixture.Customize<IModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IModel>()));

            var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            localeDataStorage.OnCreateWriterStream += (s, e) => e.Result = CreateMemoryStreamWithoutXmlContext();

            localeDataStorage.StoreLocaleData(fixture.Create<IModel>());
        }

        /// <summary>
        /// Tester, at StoreLocaleData gemmer data i det lokale datalager, når dette har indhold.
        /// </summary>
        [Test]
        public void TestAtStoreLocaleDataGemmerDataWhenLocaleDataStorageHarData()
        {
            var fixture = new Fixture();
            fixture.Customize<IModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IModel>()));

            var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            localeDataStorage.OnCreateWriterStream += (s, e) => e.Result = CreateMemoryStreamWithXmlContent();

            localeDataStorage.StoreLocaleData(fixture.Create<IModel>());
        }

        /// <summary>
        /// Tester, at StoreLocaleData gemmer data i et lokalt fil storage.
        /// </summary>
        [Test]
        public void TestAtStoreLocaleDataGemmerDataILocalDataFileStorage()
        {
            var fixture = new Fixture();
            fixture.Customize<IModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IModel>()));

            var tempFile = new FileInfo(Path.GetTempFileName());
            try
            {
                var localeDataStorage = new LocaleDataStorage(tempFile.FullName, fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
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

                localeDataStorage.StoreLocaleData(fixture.Create<IModel>());
                localeDataStorage.StoreLocaleData(fixture.Create<IModel>());
                localeDataStorage.StoreLocaleData(fixture.Create<IModel>());
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
            var fixture = new Fixture();
            fixture.Customize<IModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IModel>()));

            var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            localeDataStorage.OnCreateWriterStream += (s, e) => e.Result = CreateMemoryStreamWithoutXmlContext();

            var eventCalled = false;
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
            localeDataStorage.StoreLocaleData(fixture.Create<IModel>());
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at StoreLocaleData kaster en IntranetGuiRepositoryException ved IntranetGuiRepositoryException.
        /// </summary>
        [Test]
        public void TestAtStoreLocaleDataKasterIntranetGuiRepositoryExceptionVedIntranetGuiRepositoryException()
        {
            var fixture = new Fixture();
            fixture.Customize<IModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IModel>()));

            var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>());
            Assert.That(localeDataStorage, Is.Not.Null);

            var eventException = fixture.Create<IntranetGuiRepositoryException>();
            localeDataStorage.OnCreateWriterStream += (s, e) =>
            {
                throw eventException;
            };

            var exception = Assert.Throws<IntranetGuiRepositoryException>(() => localeDataStorage.StoreLocaleData(fixture.Create<IModel>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(eventException.Message));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at StoreLocaleData kaster en IntranetGuiRepositoryException ved Exception.
        /// </summary>
        [Test]
        public void TestAtStoreLocaleDataKasterIntranetGuiRepositoryExceptionVedException()
        {
            var fixture = new Fixture();
            fixture.Customize<IModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IModel>()));

            var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>());
            Assert.That(localeDataStorage, Is.Not.Null);

            var eventException = fixture.Create<Exception>();
            localeDataStorage.OnCreateWriterStream += (s, e) =>
            {
                throw eventException;
            };

            var exception = Assert.Throws<IntranetGuiRepositoryException>(() => localeDataStorage.StoreLocaleData(fixture.Create<IModel>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "StoreLocaleData", eventException.Message)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.EqualTo(eventException));
        }

        /// <summary>
        /// Tester, at StoreLocaleData gemmer et regnskab.
        /// </summary>
        [Test]
        public void TestAtStoreLocaleDataGemmerRegnskabModel()
        {
            var fixture = new Fixture();
            var regnskabModelMock = MockRepository.GenerateMock<IRegnskabModel>();
            regnskabModelMock.Stub(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();
            regnskabModelMock.Stub(m => m.Navn)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            var updatedRegnskabModelMock = MockRepository.GenerateMock<IRegnskabModel>();
            updatedRegnskabModelMock.Stub(m => m.Nummer)
                .Return(regnskabModelMock.Nummer)
                .Repeat.Any();
            updatedRegnskabModelMock.Stub(m => m.Navn)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var tempFile = new FileInfo(Path.GetTempFileName());
            try
            {
                var localeDataStorage = new LocaleDataStorage(tempFile.FullName, fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
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
                var regnskabNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']", regnskabModelMock.Nummer));
                Assert.IsTrue(HasAttributeWhichMatchValue(regnskabNode, "navn", regnskabModelMock.Navn));

                localeDataStorage.StoreLocaleData(updatedRegnskabModelMock);
                var updatedRegnskabNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']", regnskabModelMock.Nummer));
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
            var fixture = new Fixture();
            var rand = new Random(fixture.Create<int>());

            var regnskabModelMock = MockRepository.GenerateMock<IRegnskabModel>();
            regnskabModelMock.Stub(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();
            regnskabModelMock.Stub(m => m.Navn)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            
            var kontogruppeModel1Mock = MockRepository.GenerateMock<IKontogruppeModel>();
            kontogruppeModel1Mock.Stub(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();
            kontogruppeModel1Mock.Stub(m => m.Tekst)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            kontogruppeModel1Mock.Stub(m => m.Balancetype)
                .Return(Balancetype.Aktiver)
                .Repeat.Any();

            var kontogruppeModel2Mock = MockRepository.GenerateMock<IKontogruppeModel>();
            kontogruppeModel2Mock.Stub(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();
            kontogruppeModel2Mock.Stub(m => m.Tekst)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            kontogruppeModel2Mock.Stub(m => m.Balancetype)
                .Return(Balancetype.Aktiver)
                .Repeat.Any();

            var statusDato = DateTime.Today.AddDays(rand.Next(1, 365)*-1);
            var kontoModelMock = MockRepository.GenerateMock<IKontoModel>();
            kontoModelMock.Stub(m => m.Regnskabsnummer)
                .Return(regnskabModelMock.Nummer)
                .Repeat.Any();
            kontoModelMock.Stub(m => m.Kontonummer)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            kontoModelMock.Stub(m => m.Kontonavn)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            kontoModelMock.Stub(m => m.Beskrivelse)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            kontoModelMock.Stub(m => m.Notat)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            kontoModelMock.Stub(m => m.Kontogruppe)
                .Return(kontogruppeModel1Mock.Nummer)
                .Repeat.Any();
            kontoModelMock.Stub(m => m.StatusDato)
                .Return(statusDato)
                .Repeat.Any();
            kontoModelMock.Stub(m => m.Kredit)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            kontoModelMock.Stub(m => m.Saldo)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            kontoModelMock.Stub(m => m.Disponibel)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();

            var updatedKontoModelMock = MockRepository.GenerateMock<IKontoModel>();
            updatedKontoModelMock.Stub(m => m.Regnskabsnummer)
                .Return(kontoModelMock.Regnskabsnummer)
                .Repeat.Any();
            updatedKontoModelMock.Stub(m => m.Kontonummer)
                .Return(kontoModelMock.Kontonummer)
                .Repeat.Any();
            updatedKontoModelMock.Stub(m => m.Kontonavn)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedKontoModelMock.Stub(m => m.Beskrivelse)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedKontoModelMock.Stub(m => m.Notat)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedKontoModelMock.Stub(m => m.Kontogruppe)
                .Return(kontogruppeModel2Mock.Nummer)
                .Repeat.Any();
            updatedKontoModelMock.Stub(m => m.StatusDato)
                .Return(kontoModelMock.StatusDato)
                .Repeat.Any();
            updatedKontoModelMock.Stub(m => m.Kredit)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            updatedKontoModelMock.Stub(m => m.Saldo)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            updatedKontoModelMock.Stub(m => m.Disponibel)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();

            var updatedSaldoOnKontoModelMock = MockRepository.GenerateMock<IKontoModel>();
            updatedSaldoOnKontoModelMock.Stub(m => m.Regnskabsnummer)
                .Return(kontoModelMock.Regnskabsnummer)
                .Repeat.Any();
            updatedSaldoOnKontoModelMock.Stub(m => m.Kontonummer)
                .Return(kontoModelMock.Kontonummer)
                .Repeat.Any();
            updatedSaldoOnKontoModelMock.Stub(m => m.Kontonavn)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedSaldoOnKontoModelMock.Stub(m => m.Beskrivelse)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedSaldoOnKontoModelMock.Stub(m => m.Notat)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedSaldoOnKontoModelMock.Stub(m => m.Kontogruppe)
                .Return(kontogruppeModel2Mock.Nummer)
                .Repeat.Any();
            updatedSaldoOnKontoModelMock.Stub(m => m.StatusDato)
                .Return(kontoModelMock.StatusDato.AddDays(rand.Next(1, 365)))
                .Repeat.Any();
            updatedSaldoOnKontoModelMock.Stub(m => m.Kredit)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            updatedSaldoOnKontoModelMock.Stub(m => m.Saldo)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            updatedSaldoOnKontoModelMock.Stub(m => m.Disponibel)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();

            var tempFile = new FileInfo(Path.GetTempFileName());
            try
            {
                var localeDataStorage = new LocaleDataStorage(tempFile.FullName, fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
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
                var kontoNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']/ns:Konto[@kontonummer = '{1}']", regnskabModelMock.Nummer, kontoModelMock.Kontonummer));
                Assert.IsTrue(HasAttributeWhichMatchValue(kontoNode, "kontonavn", kontoModelMock.Kontonavn));
                Assert.IsTrue(HasAttributeWhichMatchValue(kontoNode, "beskrivelse", kontoModelMock.Beskrivelse));
                Assert.IsTrue(HasAttributeWhichMatchValue(kontoNode, "note", kontoModelMock.Notat));
                Assert.IsTrue(HasAttributeWhichMatchValue(kontoNode, "kontogruppe", kontoModelMock.Kontogruppe.ToString(CultureInfo.InvariantCulture)));

                var kontoHistorikNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']/ns:KontoHistorik[@kontonummer = '{1}' and @dato='{2}']", regnskabModelMock.Nummer, kontoModelMock.Kontonummer, kontoModelMock.StatusDato.ToString("yyyyMMdd")));
                Assert.IsTrue(HasAttributeWhichMatchValue(kontoHistorikNode, "kredit", kontoModelMock.Kredit.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(kontoHistorikNode, "saldo", kontoModelMock.Saldo.ToString(DecimalFormat, CultureInfo.InvariantCulture)));

                localeDataStorage.StoreLocaleData(updatedKontoModelMock);
                var updatedKontoNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']/ns:Konto[@kontonummer = '{1}']", regnskabModelMock.Nummer, kontoModelMock.Kontonummer));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedKontoNode, "kontonavn", updatedKontoModelMock.Kontonavn));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedKontoNode, "beskrivelse", updatedKontoModelMock.Beskrivelse));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedKontoNode, "note", updatedKontoModelMock.Notat));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedKontoNode, "kontogruppe", updatedKontoModelMock.Kontogruppe.ToString(CultureInfo.InvariantCulture)));

                var updatedKontoHistorikNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']/ns:KontoHistorik[@kontonummer = '{1}' and @dato='{2}']", regnskabModelMock.Nummer, kontoModelMock.Kontonummer, kontoModelMock.StatusDato.ToString("yyyyMMdd")));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedKontoHistorikNode, "kredit", updatedKontoModelMock.Kredit.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedKontoHistorikNode, "saldo", updatedKontoModelMock.Saldo.ToString(DecimalFormat, CultureInfo.InvariantCulture)));

                localeDataStorage.StoreLocaleData(updatedSaldoOnKontoModelMock);
                var updatedSaldoOnKontoHistorikNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']/ns:KontoHistorik[@kontonummer = '{1}' and @dato='{2}']", regnskabModelMock.Nummer, kontoModelMock.Kontonummer, updatedSaldoOnKontoModelMock.StatusDato.ToString("yyyyMMdd")));
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
            var fixture = new Fixture();
            var rand = new Random(fixture.Create<int>());

            var regnskabModelMock = MockRepository.GenerateMock<IRegnskabModel>();
            regnskabModelMock.Stub(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();
            regnskabModelMock.Stub(m => m.Navn)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var budgetkontogruppeModel1Mock = MockRepository.GenerateMock<IBudgetkontogruppeModel>();
            budgetkontogruppeModel1Mock.Stub(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();
            budgetkontogruppeModel1Mock.Stub(m => m.Tekst)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var budgetkontogruppeModel2Mock = MockRepository.GenerateMock<IBudgetkontogruppeModel>();
            budgetkontogruppeModel2Mock.Stub(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();
            budgetkontogruppeModel2Mock.Stub(m => m.Tekst)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var statusDato = DateTime.Today.AddDays(rand.Next(1, 365)*-1);
            var budgetkontoModelMock = MockRepository.GenerateMock<IBudgetkontoModel>();
            budgetkontoModelMock.Stub(m => m.Regnskabsnummer)
                .Return(regnskabModelMock.Nummer)
                .Repeat.Any();
            budgetkontoModelMock.Stub(m => m.Kontonummer)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            budgetkontoModelMock.Stub(m => m.Kontonavn)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            budgetkontoModelMock.Stub(m => m.Beskrivelse)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            budgetkontoModelMock.Stub(m => m.Notat)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            budgetkontoModelMock.Stub(m => m.Kontogruppe)
                .Return(budgetkontogruppeModel1Mock.Nummer)
                .Repeat.Any();
            budgetkontoModelMock.Stub(m => m.StatusDato)
                .Return(statusDato)
                .Repeat.Any();
            budgetkontoModelMock.Stub(m => m.Indtægter)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            budgetkontoModelMock.Stub(m => m.Udgifter)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            budgetkontoModelMock.Stub(m => m.Budget)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            budgetkontoModelMock.Stub(m => m.BudgetSidsteMåned)
                .Return(Math.Abs(fixture.Create<decimal>())*(rand.Next(0, 100) > 50 ? -1 : 1))
                .Repeat.Any();
            budgetkontoModelMock.Stub(m => m.Bogført)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            budgetkontoModelMock.Stub(m => m.BogførtSidsteMåned)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            budgetkontoModelMock.Stub(m => m.Disponibel)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();

            var updatedBudgetkontoModelMock = MockRepository.GenerateMock<IBudgetkontoModel>();
            updatedBudgetkontoModelMock.Stub(m => m.Regnskabsnummer)
                .Return(budgetkontoModelMock.Regnskabsnummer)
                .Repeat.Any();
            updatedBudgetkontoModelMock.Stub(m => m.Kontonummer)
                .Return(budgetkontoModelMock.Kontonummer)
                .Repeat.Any();
            updatedBudgetkontoModelMock.Stub(m => m.Kontonavn)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedBudgetkontoModelMock.Stub(m => m.Beskrivelse)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedBudgetkontoModelMock.Stub(m => m.Notat)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedBudgetkontoModelMock.Stub(m => m.Kontogruppe)
                .Return(budgetkontogruppeModel2Mock.Nummer)
                .Repeat.Any();
            updatedBudgetkontoModelMock.Stub(m => m.StatusDato)
                .Return(budgetkontoModelMock.StatusDato)
                .Repeat.Any();
            updatedBudgetkontoModelMock.Stub(m => m.Indtægter)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            updatedBudgetkontoModelMock.Stub(m => m.Udgifter)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            updatedBudgetkontoModelMock.Stub(m => m.Budget)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            updatedBudgetkontoModelMock.Stub(m => m.BudgetSidsteMåned)
                .Return(Math.Abs(fixture.Create<decimal>())*(rand.Next(0, 100) > 50 ? -1 : 1))
                .Repeat.Any();
            updatedBudgetkontoModelMock.Stub(m => m.Bogført)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            updatedBudgetkontoModelMock.Stub(m => m.BogførtSidsteMåned)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            updatedBudgetkontoModelMock.Stub(m => m.Disponibel)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();

            var updatedSaldoOnBudgetkontoModelMock = MockRepository.GenerateMock<IBudgetkontoModel>();
            updatedSaldoOnBudgetkontoModelMock.Stub(m => m.Regnskabsnummer)
                .Return(budgetkontoModelMock.Regnskabsnummer)
                .Repeat.Any();
            updatedSaldoOnBudgetkontoModelMock.Stub(m => m.Kontonummer)
                .Return(budgetkontoModelMock.Kontonummer)
                .Repeat.Any();
            updatedSaldoOnBudgetkontoModelMock.Stub(m => m.Kontonavn)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedSaldoOnBudgetkontoModelMock.Stub(m => m.Beskrivelse)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedSaldoOnBudgetkontoModelMock.Stub(m => m.Notat)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedSaldoOnBudgetkontoModelMock.Stub(m => m.Kontogruppe)
                .Return(budgetkontogruppeModel2Mock.Nummer)
                .Repeat.Any();
            updatedSaldoOnBudgetkontoModelMock.Stub(m => m.StatusDato)
                .Return(budgetkontoModelMock.StatusDato.AddDays(rand.Next(1, 365)))
                .Repeat.Any();
            updatedSaldoOnBudgetkontoModelMock.Stub(m => m.Indtægter)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            updatedSaldoOnBudgetkontoModelMock.Stub(m => m.Udgifter)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            updatedSaldoOnBudgetkontoModelMock.Stub(m => m.Budget)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            updatedSaldoOnBudgetkontoModelMock.Stub(m => m.BudgetSidsteMåned)
                .Return(Math.Abs(fixture.Create<decimal>())*(rand.Next(0, 100) > 50 ? -1 : 1))
                .Repeat.Any();
            updatedSaldoOnBudgetkontoModelMock.Stub(m => m.Bogført)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            updatedSaldoOnBudgetkontoModelMock.Stub(m => m.BogførtSidsteMåned)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            updatedSaldoOnBudgetkontoModelMock.Stub(m => m.Disponibel)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();

            var tempFile = new FileInfo(Path.GetTempFileName());
            try
            {
                var localeDataStorage = new LocaleDataStorage(tempFile.FullName, fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
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
                var budgetkontoNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']/ns:Budgetkonto[@kontonummer = '{1}']", regnskabModelMock.Nummer, budgetkontoModelMock.Kontonummer));
                Assert.IsTrue(HasAttributeWhichMatchValue(budgetkontoNode, "kontonavn", budgetkontoModelMock.Kontonavn));
                Assert.IsTrue(HasAttributeWhichMatchValue(budgetkontoNode, "beskrivelse", budgetkontoModelMock.Beskrivelse));
                Assert.IsTrue(HasAttributeWhichMatchValue(budgetkontoNode, "note", budgetkontoModelMock.Notat));
                Assert.IsTrue(HasAttributeWhichMatchValue(budgetkontoNode, "kontogruppe", budgetkontoModelMock.Kontogruppe.ToString(CultureInfo.InvariantCulture)));

                var budgetkontoHistorikNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']/ns:BudgetkontoHistorik[@kontonummer = '{1}' and @dato='{2}']", regnskabModelMock.Nummer, budgetkontoModelMock.Kontonummer, budgetkontoModelMock.StatusDato.ToString("yyyyMMdd")));
                Assert.IsTrue(HasAttributeWhichMatchValue(budgetkontoHistorikNode, "indtaegter", budgetkontoModelMock.Indtægter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(budgetkontoHistorikNode, "udgifter", budgetkontoModelMock.Udgifter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(budgetkontoHistorikNode, "bogfoert", budgetkontoModelMock.Bogført.ToString(DecimalFormat, CultureInfo.InvariantCulture)));

                var lastMonthStatusDato = new DateTime(budgetkontoModelMock.StatusDato.AddMonths(-1).Year, budgetkontoModelMock.StatusDato.AddMonths(-1).Month, DateTime.DaysInMonth(budgetkontoModelMock.StatusDato.AddMonths(-1).Year, budgetkontoModelMock.StatusDato.AddMonths(-1).Month));
                var lastMonthIndtægter = Math.Abs(budgetkontoModelMock.BudgetSidsteMåned > 0 ? budgetkontoModelMock.BudgetSidsteMåned : 0M);
                var lastMonthUdgifter = Math.Abs(budgetkontoModelMock.BudgetSidsteMåned < 0 ? budgetkontoModelMock.BudgetSidsteMåned : 0M);
                var lastMonthBudgetkontoHistorikNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']/ns:BudgetkontoHistorik[@kontonummer = '{1}' and @dato='{2}']", regnskabModelMock.Nummer, budgetkontoModelMock.Kontonummer, lastMonthStatusDato.ToString("yyyyMMdd")));
                Assert.IsTrue(HasAttributeWhichMatchValue(lastMonthBudgetkontoHistorikNode, "indtaegter", lastMonthIndtægter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(lastMonthBudgetkontoHistorikNode, "udgifter", lastMonthUdgifter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(lastMonthBudgetkontoHistorikNode, "bogfoert", budgetkontoModelMock.BogførtSidsteMåned.ToString(DecimalFormat, CultureInfo.InvariantCulture)));

                localeDataStorage.StoreLocaleData(updatedBudgetkontoModelMock);
                var updatedBudgetkontoNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']/ns:Budgetkonto[@kontonummer = '{1}']", regnskabModelMock.Nummer, budgetkontoModelMock.Kontonummer));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBudgetkontoNode, "kontonavn", updatedBudgetkontoModelMock.Kontonavn));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBudgetkontoNode, "beskrivelse", updatedBudgetkontoModelMock.Beskrivelse));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBudgetkontoNode, "note", updatedBudgetkontoModelMock.Notat));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBudgetkontoNode, "kontogruppe", updatedBudgetkontoModelMock.Kontogruppe.ToString(CultureInfo.InvariantCulture)));

                var updatedBudgetkontoHistorikNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']/ns:BudgetkontoHistorik[@kontonummer = '{1}' and @dato='{2}']", regnskabModelMock.Nummer, budgetkontoModelMock.Kontonummer, budgetkontoModelMock.StatusDato.ToString("yyyyMMdd")));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBudgetkontoHistorikNode, "indtaegter", updatedBudgetkontoModelMock.Indtægter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBudgetkontoHistorikNode, "udgifter", updatedBudgetkontoModelMock.Udgifter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBudgetkontoHistorikNode, "bogfoert", updatedBudgetkontoModelMock.Bogført.ToString(DecimalFormat, CultureInfo.InvariantCulture)));

                lastMonthStatusDato = new DateTime(updatedBudgetkontoModelMock.StatusDato.AddMonths(-1).Year, updatedBudgetkontoModelMock.StatusDato.AddMonths(-1).Month, DateTime.DaysInMonth(updatedBudgetkontoModelMock.StatusDato.AddMonths(-1).Year, updatedBudgetkontoModelMock.StatusDato.AddMonths(-1).Month));
                lastMonthIndtægter = Math.Abs(updatedBudgetkontoModelMock.BudgetSidsteMåned > 0 ? updatedBudgetkontoModelMock.BudgetSidsteMåned : 0M);
                lastMonthUdgifter = Math.Abs(updatedBudgetkontoModelMock.BudgetSidsteMåned < 0 ? updatedBudgetkontoModelMock.BudgetSidsteMåned : 0M);
                var lastMonthUpdatedBudgetkontoHistorikNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']/ns:BudgetkontoHistorik[@kontonummer = '{1}' and @dato='{2}']", regnskabModelMock.Nummer, budgetkontoModelMock.Kontonummer, lastMonthStatusDato.ToString("yyyyMMdd")));
                Assert.IsTrue(HasAttributeWhichMatchValue(lastMonthUpdatedBudgetkontoHistorikNode, "indtaegter", lastMonthIndtægter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(lastMonthUpdatedBudgetkontoHistorikNode, "udgifter", lastMonthUdgifter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(lastMonthUpdatedBudgetkontoHistorikNode, "bogfoert", updatedBudgetkontoModelMock.BogførtSidsteMåned.ToString(DecimalFormat, CultureInfo.InvariantCulture)));

                localeDataStorage.StoreLocaleData(updatedSaldoOnBudgetkontoModelMock);
                var updatedSaldoOnBudgetkontoHistorikNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']/ns:BudgetkontoHistorik[@kontonummer = '{1}' and @dato='{2}']", regnskabModelMock.Nummer, budgetkontoModelMock.Kontonummer, updatedSaldoOnBudgetkontoModelMock.StatusDato.ToString("yyyyMMdd")));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedSaldoOnBudgetkontoHistorikNode, "indtaegter", updatedSaldoOnBudgetkontoModelMock.Indtægter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedSaldoOnBudgetkontoHistorikNode, "udgifter", updatedSaldoOnBudgetkontoModelMock.Udgifter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedSaldoOnBudgetkontoHistorikNode, "bogfoert", updatedSaldoOnBudgetkontoModelMock.Bogført.ToString(DecimalFormat, CultureInfo.InvariantCulture)));

                lastMonthStatusDato = new DateTime(updatedSaldoOnBudgetkontoModelMock.StatusDato.AddMonths(-1).Year, updatedSaldoOnBudgetkontoModelMock.StatusDato.AddMonths(-1).Month, DateTime.DaysInMonth(updatedSaldoOnBudgetkontoModelMock.StatusDato.AddMonths(-1).Year, updatedSaldoOnBudgetkontoModelMock.StatusDato.AddMonths(-1).Month));
                lastMonthIndtægter = Math.Abs(updatedSaldoOnBudgetkontoModelMock.BudgetSidsteMåned > 0 ? updatedSaldoOnBudgetkontoModelMock.BudgetSidsteMåned : 0M);
                lastMonthUdgifter = Math.Abs(updatedSaldoOnBudgetkontoModelMock.BudgetSidsteMåned < 0 ? updatedSaldoOnBudgetkontoModelMock.BudgetSidsteMåned : 0M);
                var lastMonthUpdatedSaldoOnBudgetkontoHistorikNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']/ns:BudgetkontoHistorik[@kontonummer = '{1}' and @dato='{2}']", regnskabModelMock.Nummer, budgetkontoModelMock.Kontonummer, lastMonthStatusDato.ToString("yyyyMMdd")));
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
            var fixture = new Fixture();
            var rand = new Random(fixture.Create<int>());

            var regnskabModelMock = MockRepository.GenerateMock<IRegnskabModel>();
            regnskabModelMock.Stub(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();
            regnskabModelMock.Stub(m => m.Navn)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var statusDato = DateTime.Today.AddDays(rand.Next(1, 365)*-1);
            var adressekontoModelMock = MockRepository.GenerateMock<IAdressekontoModel>();
            adressekontoModelMock.Stub(m => m.Regnskabsnummer)
                .Return(regnskabModelMock.Nummer)
                .Repeat.Any();
            adressekontoModelMock.Stub(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();
            adressekontoModelMock.Stub(m => m.Navn)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            adressekontoModelMock.Stub(m => m.PrimærTelefon)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            adressekontoModelMock.Stub(m => m.SekundærTelefon)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            adressekontoModelMock.Stub(m => m.StatusDato)
                .Return(statusDato)
                .Repeat.Any();
            adressekontoModelMock.Stub(m => m.Saldo)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();

            var updatedAdressekontoModelMock = MockRepository.GenerateMock<IAdressekontoModel>();
            updatedAdressekontoModelMock.Stub(m => m.Regnskabsnummer)
                .Return(adressekontoModelMock.Regnskabsnummer)
                .Repeat.Any();
            updatedAdressekontoModelMock.Stub(m => m.Nummer)
                .Return(adressekontoModelMock.Nummer)
                .Repeat.Any();
            updatedAdressekontoModelMock.Stub(m => m.Navn)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedAdressekontoModelMock.Stub(m => m.PrimærTelefon)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedAdressekontoModelMock.Stub(m => m.SekundærTelefon)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedAdressekontoModelMock.Stub(m => m.StatusDato)
                .Return(adressekontoModelMock.StatusDato)
                .Repeat.Any();
            updatedAdressekontoModelMock.Stub(m => m.Saldo)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();

            var updatedSaldoOnAdressekontoModelMock = MockRepository.GenerateMock<IAdressekontoModel>();
            updatedSaldoOnAdressekontoModelMock.Stub(m => m.Regnskabsnummer)
                .Return(adressekontoModelMock.Regnskabsnummer)
                .Repeat.Any();
            updatedSaldoOnAdressekontoModelMock.Stub(m => m.Nummer)
                .Return(adressekontoModelMock.Nummer)
                .Repeat.Any();
            updatedSaldoOnAdressekontoModelMock.Stub(m => m.Navn)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedSaldoOnAdressekontoModelMock.Stub(m => m.PrimærTelefon)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedSaldoOnAdressekontoModelMock.Stub(m => m.SekundærTelefon)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedSaldoOnAdressekontoModelMock.Stub(m => m.StatusDato)
                .Return(adressekontoModelMock.StatusDato.AddDays(rand.Next(1, 365)))
                .Repeat.Any();
            updatedSaldoOnAdressekontoModelMock.Stub(m => m.Saldo)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();

            var tempFile = new FileInfo(Path.GetTempFileName());
            try
            {
                var localeDataStorage = new LocaleDataStorage(tempFile.FullName, fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
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
                var adressekontoNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']/ns:Adressekonto[@nummer = '{1}']", regnskabModelMock.Nummer, adressekontoModelMock.Nummer.ToString(CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(adressekontoNode, "navn", adressekontoModelMock.Navn));
                Assert.IsTrue(HasAttributeWhichMatchValue(adressekontoNode, "primaerTelefon", adressekontoModelMock.PrimærTelefon));
                Assert.IsTrue(HasAttributeWhichMatchValue(adressekontoNode, "sekundaerTelefon", adressekontoModelMock.SekundærTelefon));

                var adressekontoHistorikNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']/ns:AdressekontoHistorik[@nummer = '{1}' and @dato='{2}']", regnskabModelMock.Nummer, adressekontoModelMock.Nummer.ToString(CultureInfo.InvariantCulture), adressekontoModelMock.StatusDato.ToString("yyyyMMdd")));
                Assert.IsTrue(HasAttributeWhichMatchValue(adressekontoHistorikNode, "saldo", adressekontoModelMock.Saldo.ToString(DecimalFormat, CultureInfo.InvariantCulture)));

                localeDataStorage.StoreLocaleData(updatedAdressekontoModelMock);
                var updatedAdressekontoNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']/ns:Adressekonto[@nummer = '{1}']", regnskabModelMock.Nummer, adressekontoModelMock.Nummer.ToString(CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedAdressekontoNode, "navn", updatedAdressekontoModelMock.Navn));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedAdressekontoNode, "primaerTelefon", updatedAdressekontoModelMock.PrimærTelefon));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedAdressekontoNode, "sekundaerTelefon", updatedAdressekontoModelMock.SekundærTelefon));

                var updatedAdressekontoHistorikNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']/ns:AdressekontoHistorik[@nummer = '{1}' and @dato='{2}']", regnskabModelMock.Nummer, adressekontoModelMock.Nummer.ToString(CultureInfo.InvariantCulture), adressekontoModelMock.StatusDato.ToString("yyyyMMdd")));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedAdressekontoHistorikNode, "saldo", updatedAdressekontoModelMock.Saldo.ToString(DecimalFormat, CultureInfo.InvariantCulture)));

                localeDataStorage.StoreLocaleData(updatedSaldoOnAdressekontoModelMock);
                var updatedSaldoOnAdressekontoHistorikNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']/ns:AdressekontoHistorik[@nummer = '{1}' and @dato='{2}']", regnskabModelMock.Nummer, updatedSaldoOnAdressekontoModelMock.Nummer.ToString(CultureInfo.InvariantCulture), updatedSaldoOnAdressekontoModelMock.StatusDato.ToString("yyyyMMdd")));
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
            var fixture = new Fixture();
            var rand = new Random(fixture.Create<int>());

            var regnskabModelMock = MockRepository.GenerateMock<IRegnskabModel>();
            regnskabModelMock.Stub(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();
            regnskabModelMock.Stub(m => m.Navn)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var statusDato = DateTime.Today.AddDays(rand.Next(1, 365)*-1);
            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Stub(m => m.Regnskabsnummer)
                .Return(regnskabModelMock.Nummer)
                .Repeat.Any();
            bogføringslinjeModelMock.Stub(m => m.Løbenummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();
            bogføringslinjeModelMock.Stub(m => m.Dato)
                .Return(statusDato)
                .Repeat.Any();
            bogføringslinjeModelMock.Stub(m => m.Bilag)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            bogføringslinjeModelMock.Stub(m => m.Kontonummer)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            bogføringslinjeModelMock.Stub(m => m.Tekst)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            bogføringslinjeModelMock.Stub(m => m.Budgetkontonummer)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            bogføringslinjeModelMock.Stub(m => m.Debit)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            bogføringslinjeModelMock.Stub(m => m.Kredit)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            bogføringslinjeModelMock.Stub(m => m.Bogført)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            bogføringslinjeModelMock.Stub(m => m.Adressekonto)
                .Return(fixture.Create<int>())
                .Repeat.Any();

            var updatedBogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            updatedBogføringslinjeModelMock.Stub(m => m.Regnskabsnummer)
                .Return(bogføringslinjeModelMock.Regnskabsnummer)
                .Repeat.Any();
            updatedBogføringslinjeModelMock.Stub(m => m.Løbenummer)
                .Return(bogføringslinjeModelMock.Løbenummer)
                .Repeat.Any();
            updatedBogføringslinjeModelMock.Stub(m => m.Dato)
                .Return(statusDato.AddDays(rand.Next(1, 365)))
                .Repeat.Any();
            updatedBogføringslinjeModelMock.Stub(m => m.Bilag)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedBogføringslinjeModelMock.Stub(m => m.Kontonummer)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedBogføringslinjeModelMock.Stub(m => m.Tekst)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedBogføringslinjeModelMock.Stub(m => m.Budgetkontonummer)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedBogføringslinjeModelMock.Stub(m => m.Debit)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            updatedBogføringslinjeModelMock.Stub(m => m.Kredit)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            updatedBogføringslinjeModelMock.Stub(m => m.Bogført)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            updatedBogføringslinjeModelMock.Stub(m => m.Adressekonto)
                .Return(fixture.Create<int>())
                .Repeat.Any();

            var tempFile = new FileInfo(Path.GetTempFileName());
            try
            {
                var localeDataStorage = new LocaleDataStorage(tempFile.FullName, fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
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
                var bogføringslinjeNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']/ns:Bogfoeringslinje[@loebenummer = '{1}']", regnskabModelMock.Nummer, bogføringslinjeModelMock.Løbenummer.ToString(CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(bogføringslinjeNode, "dato", bogføringslinjeModelMock.Dato.ToString("yyyyMMdd")));
                Assert.IsTrue(HasAttributeWhichMatchValue(bogføringslinjeNode, "bilag", bogføringslinjeModelMock.Bilag));
                Assert.IsTrue(HasAttributeWhichMatchValue(bogføringslinjeNode, "kontonummer", bogføringslinjeModelMock.Kontonummer));
                Assert.IsTrue(HasAttributeWhichMatchValue(bogføringslinjeNode, "tekst", bogføringslinjeModelMock.Tekst));
                Assert.IsTrue(HasAttributeWhichMatchValue(bogføringslinjeNode, "budgetkontonummer", bogføringslinjeModelMock.Budgetkontonummer));
                Assert.IsTrue(HasAttributeWhichMatchValue(bogføringslinjeNode, "debit", bogføringslinjeModelMock.Debit.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(bogføringslinjeNode, "kredit", bogføringslinjeModelMock.Kredit.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(bogføringslinjeNode, "adressekonto", bogføringslinjeModelMock.Adressekonto.ToString(CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(bogføringslinjeNode, "synkroniseret", "false"));

                localeDataStorage.StoreLocaleData(updatedBogføringslinjeModelMock);
                var updatedBogføringslinjeNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']/ns:Bogfoeringslinje[@loebenummer = '{1}']", regnskabModelMock.Nummer, bogføringslinjeModelMock.Løbenummer.ToString(CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBogføringslinjeNode, "dato", updatedBogføringslinjeModelMock.Dato.ToString("yyyyMMdd")));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBogføringslinjeNode, "bilag", updatedBogføringslinjeModelMock.Bilag));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBogføringslinjeNode, "kontonummer", updatedBogføringslinjeModelMock.Kontonummer));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBogføringslinjeNode, "tekst", updatedBogføringslinjeModelMock.Tekst));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBogføringslinjeNode, "budgetkontonummer", updatedBogføringslinjeModelMock.Budgetkontonummer));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBogføringslinjeNode, "debit", updatedBogføringslinjeModelMock.Debit.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBogføringslinjeNode, "kredit", updatedBogføringslinjeModelMock.Kredit.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBogføringslinjeNode, "adressekonto", updatedBogføringslinjeModelMock.Adressekonto.ToString(CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBogføringslinjeNode, "synkroniseret", "false"));
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
            var fixture = new Fixture();
            var kontogruppeModelMock = MockRepository.GenerateMock<IKontogruppeModel>();
            kontogruppeModelMock.Stub(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();
            kontogruppeModelMock.Stub(m => m.Tekst)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            kontogruppeModelMock.Stub(m => m.Balancetype)
                .Return(Balancetype.Aktiver)
                .Repeat.Any();
            var updatedKontogruppeModelMock = MockRepository.GenerateMock<IKontogruppeModel>();
            updatedKontogruppeModelMock.Stub(m => m.Nummer)
                .Return(kontogruppeModelMock.Nummer)
                .Repeat.Any();
            updatedKontogruppeModelMock.Stub(m => m.Tekst)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedKontogruppeModelMock.Stub(m => m.Balancetype)
                .Return(Balancetype.Passiver)
                .Repeat.Any();

            var tempFile = new FileInfo(Path.GetTempFileName());
            try
            {
                var localeDataStorage = new LocaleDataStorage(tempFile.FullName, fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
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
                var kontogruppeNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Kontogruppe[@nummer = '{0}']", kontogruppeModelMock.Nummer));
                Assert.IsTrue(HasAttributeWhichMatchValue(kontogruppeNode, "tekst", kontogruppeModelMock.Tekst));
                Assert.IsTrue(HasAttributeWhichMatchValue(kontogruppeNode, "balanceType", kontogruppeModelMock.Balancetype.ToString()));

                localeDataStorage.StoreLocaleData(updatedKontogruppeModelMock);
                var updatedKontogruppeNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Kontogruppe[@nummer = '{0}']", kontogruppeModelMock.Nummer));
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
            var fixture = new Fixture();
            var budgetkontogruppeModelMock = MockRepository.GenerateMock<IBudgetkontogruppeModel>();
            budgetkontogruppeModelMock.Stub(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();
            budgetkontogruppeModelMock.Stub(m => m.Tekst)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            var updatedBudgetkontogruppeModelMock = MockRepository.GenerateMock<IBudgetkontogruppeModel>();
            updatedBudgetkontogruppeModelMock.Stub(m => m.Nummer)
                .Return(budgetkontogruppeModelMock.Nummer)
                .Repeat.Any();
            updatedBudgetkontogruppeModelMock.Stub(m => m.Tekst)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var tempFile = new FileInfo(Path.GetTempFileName());
            try
            {
                var localeDataStorage = new LocaleDataStorage(tempFile.FullName, fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
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
                var budgetkontogruppeNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Budgetkontogruppe[@nummer = '{0}']", budgetkontogruppeModelMock.Nummer));
                Assert.IsTrue(HasAttributeWhichMatchValue(budgetkontogruppeNode, "tekst", budgetkontogruppeModelMock.Tekst));

                localeDataStorage.StoreLocaleData(updatedBudgetkontogruppeModelMock);
                var updatedBudgetkontogruppeNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Budgetkontogruppe[@nummer = '{0}']", budgetkontogruppeModelMock.Nummer));
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
            var fixture = new Fixture();

            var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>());
            Assert.That(localeDataStorage, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => localeDataStorage.StoreSyncDocument(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("localeDataDocument"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at StoreSyncDocument kaster en IntranetGuiRepositoryException, hvis eventet, der rejses, når en skrivestream til det lokale datalager skal dannes, er null.
        /// </summary>
        [Test]
        public void TestAtStoreSyncDocumentKasterIntranetGuiRepositoryExceptionHvisOnOnCreateWriterStreamErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IModel>()));

            var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>());
            Assert.That(localeDataStorage, Is.Not.Null);

            var exception = Assert.Throws<IntranetGuiRepositoryException>(() => localeDataStorage.StoreSyncDocument(new XDocument()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.EventHandlerNotDefined, "OnCreateWriterStream")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at StoreSyncDocument rejser eventet, der skal danne skrivestreamen til det lokale datalager.
        /// </summary>
        [Test]
        public void TestAtStoreSyncDocumentRejserOnCreateWriterStreamEvent()
        {
            var fixture = new Fixture();
            fixture.Customize<IModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IModel>()));

            var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            var eventCalled = false;
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

            var localeDataDocument = new XDocument(new XDeclaration("1.0", Encoding.UTF8.BodyName, null));
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
            var fixture = new Fixture();
            fixture.Customize<IModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IModel>()));

            var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            localeDataStorage.OnCreateWriterStream += (s, e) => e.Result = CreateMemoryStreamWithoutXmlContext();

            var localeDataDocument = new XDocument(new XDeclaration("1.0", Encoding.UTF8.BodyName, null));
            localeDataDocument.Add(new XElement(XName.Get("Test")));

            localeDataStorage.StoreSyncDocument(localeDataDocument);
        }

        /// <summary>
        /// Tester, at StoreSyncDocument rejser eventet, der skal forberede data i det lokale datalager for læsning eller skrivning.
        /// </summary>
        [Test]
        public void TestAtStoreSyncDocumentRejserPrepareLocaleDataEvent()
        {
            var fixture = new Fixture();

            var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            localeDataStorage.OnCreateWriterStream += (s, e) => e.Result = CreateMemoryStreamWithoutXmlContext();

            var localeDataDocument = new XDocument(new XDeclaration("1.0", Encoding.UTF8.BodyName, null));
            localeDataDocument.Add(new XElement(XName.Get("Test")));

            var eventCalled = false;
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
            var fixture = new Fixture();

            var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>());
            Assert.That(localeDataStorage, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => localeDataStorage.StoreSyncData((IModel) null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("model"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at StoreSyncData kaster en IntranetGuiRepositoryException, hvis eventet, der rejses, når en skrivestream til det lokale datalager skal dannes, er null.
        /// </summary>
        [Test]
        public void TestAtStoreSyncDataKasterIntranetGuiRepositoryExceptionHvisOnOnCreateWriterStreamErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IModel>()));

            var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>());
            Assert.That(localeDataStorage, Is.Not.Null);

            var exception = Assert.Throws<IntranetGuiRepositoryException>(() => localeDataStorage.StoreLocaleData(fixture.Create<IModel>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.EventHandlerNotDefined, "OnCreateWriterStream")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at StoreSyncData rejser eventet, der skal danne skrivestreamen til det lokale datalager.
        /// </summary>
        [Test]
        public void TestAtStoreSyncDataRejserOnCreateWriterStreamEvent()
        {
            var fixture = new Fixture();
            fixture.Customize<IModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IModel>()));

            var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            var eventCalled = false;
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
            localeDataStorage.StoreSyncData(fixture.Create<IModel>());
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at StoreSyncData gemmer synkroniseringsdata i det lokale datalager, når dette ikke har indhold.
        /// </summary>
        [Test]
        public void TestAtStoreSyncDataGemmerDataWhenLocaleDataStorageIkkeHarData()
        {
            var fixture = new Fixture();
            fixture.Customize<IModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IModel>()));

            var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            localeDataStorage.OnCreateWriterStream += (s, e) => e.Result = CreateMemoryStreamWithoutXmlContext();

            localeDataStorage.StoreSyncData(fixture.Create<IModel>());
        }

        /// <summary>
        /// Tester, at StoreSyncData gemmer synkroniseringsdata i det lokale datalager, når dette har indhold.
        /// </summary>
        [Test]
        public void TestAtStoreSyncDataGemmerDataWhenLocaleDataStorageHarData()
        {
            var fixture = new Fixture();
            fixture.Customize<IModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IModel>()));

            var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            localeDataStorage.OnCreateWriterStream += (s, e) => e.Result = CreateMemoryStreamWithXmlContent();

            localeDataStorage.StoreSyncData(fixture.Create<IModel>());
        }

        /// <summary>
        /// Tester, at StoreSyncData gemmer data i et lokalt fil storage.
        /// </summary>
        [Test]
        public void TestAtStoreSyncDataGemmerDataILocalDataFileStorage()
        {
            var fixture = new Fixture();
            fixture.Customize<IModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IModel>()));

            var tempFile = new FileInfo(Path.GetTempFileName());
            try
            {
                var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), tempFile.FullName, FinansstyringRepositoryLocale.XmlSchema);
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

                localeDataStorage.StoreSyncData(fixture.Create<IModel>());
                localeDataStorage.StoreSyncData(fixture.Create<IModel>());
                localeDataStorage.StoreSyncData(fixture.Create<IModel>());
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
            var fixture = new Fixture();
            fixture.Customize<IModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IModel>()));

            var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), FinansstyringRepositoryLocale.XmlSchema);
            Assert.That(localeDataStorage, Is.Not.Null);

            localeDataStorage.OnCreateWriterStream += (s, e) => e.Result = CreateMemoryStreamWithoutXmlContext();

            var eventCalled = false;
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
            localeDataStorage.StoreSyncData(fixture.Create<IModel>());
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at StoreSyncData kaster en IntranetGuiRepositoryException ved IntranetGuiRepositoryException.
        /// </summary>
        [Test]
        public void TestAtStoreSyncDataKasterIntranetGuiRepositoryExceptionVedIntranetGuiRepositoryException()
        {
            var fixture = new Fixture();
            fixture.Customize<IModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IModel>()));

            var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>());
            Assert.That(localeDataStorage, Is.Not.Null);

            var eventException = fixture.Create<IntranetGuiRepositoryException>();
            localeDataStorage.OnCreateWriterStream += (s, e) =>
            {
                throw eventException;
            };

            var exception = Assert.Throws<IntranetGuiRepositoryException>(() => localeDataStorage.StoreSyncData(fixture.Create<IModel>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(eventException.Message));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at StoreSyncData kaster en IntranetGuiRepositoryException ved Exception.
        /// </summary>
        [Test]
        public void TestAtStoreSyncDataKasterIntranetGuiRepositoryExceptionVedException()
        {
            var fixture = new Fixture();
            fixture.Customize<IModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IModel>()));

            var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>());
            Assert.That(localeDataStorage, Is.Not.Null);

            var eventException = fixture.Create<Exception>();
            localeDataStorage.OnCreateWriterStream += (s, e) =>
            {
                throw eventException;
            };

            var exception = Assert.Throws<IntranetGuiRepositoryException>(() => localeDataStorage.StoreSyncData(fixture.Create<IModel>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "StoreSyncData", eventException.Message)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.EqualTo(eventException));
        }

        /// <summary>
        /// Tester, at StoreSyncData gemmer et regnskab.
        /// </summary>
        [Test]
        public void TestAtStoreSyncDataGemmerRegnskabModel()
        {
            var fixture = new Fixture();
            var regnskabModelMock = MockRepository.GenerateMock<IRegnskabModel>();
            regnskabModelMock.Stub(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();
            regnskabModelMock.Stub(m => m.Navn)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            var updatedRegnskabModelMock = MockRepository.GenerateMock<IRegnskabModel>();
            updatedRegnskabModelMock.Stub(m => m.Nummer)
                .Return(regnskabModelMock.Nummer)
                .Repeat.Any();
            updatedRegnskabModelMock.Stub(m => m.Navn)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var tempFile = new FileInfo(Path.GetTempFileName());
            try
            {
                var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), tempFile.FullName, FinansstyringRepositoryLocale.XmlSchema);
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
                var regnskabNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']", regnskabModelMock.Nummer));
                Assert.IsTrue(HasAttributeWhichMatchValue(regnskabNode, "navn", regnskabModelMock.Navn));

                localeDataStorage.StoreSyncData(updatedRegnskabModelMock);
                var updatedRegnskabNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']", regnskabModelMock.Nummer));
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
            var fixture = new Fixture();
            var rand = new Random(fixture.Create<int>());

            var regnskabModelMock = MockRepository.GenerateMock<IRegnskabModel>();
            regnskabModelMock.Stub(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();
            regnskabModelMock.Stub(m => m.Navn)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var kontogruppeModel1Mock = MockRepository.GenerateMock<IKontogruppeModel>();
            kontogruppeModel1Mock.Stub(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();
            kontogruppeModel1Mock.Stub(m => m.Tekst)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            kontogruppeModel1Mock.Stub(m => m.Balancetype)
                .Return(Balancetype.Aktiver)
                .Repeat.Any();

            var kontogruppeModel2Mock = MockRepository.GenerateMock<IKontogruppeModel>();
            kontogruppeModel2Mock.Stub(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();
            kontogruppeModel2Mock.Stub(m => m.Tekst)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            kontogruppeModel2Mock.Stub(m => m.Balancetype)
                .Return(Balancetype.Aktiver)
                .Repeat.Any();

            var statusDato = DateTime.Today.AddDays(rand.Next(1, 365)*-1);
            var kontoModelMock = MockRepository.GenerateMock<IKontoModel>();
            kontoModelMock.Stub(m => m.Regnskabsnummer)
                .Return(regnskabModelMock.Nummer)
                .Repeat.Any();
            kontoModelMock.Stub(m => m.Kontonummer)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            kontoModelMock.Stub(m => m.Kontonavn)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            kontoModelMock.Stub(m => m.Beskrivelse)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            kontoModelMock.Stub(m => m.Notat)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            kontoModelMock.Stub(m => m.Kontogruppe)
                .Return(kontogruppeModel1Mock.Nummer)
                .Repeat.Any();
            kontoModelMock.Stub(m => m.StatusDato)
                .Return(statusDato)
                .Repeat.Any();
            kontoModelMock.Stub(m => m.Kredit)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            kontoModelMock.Stub(m => m.Saldo)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            kontoModelMock.Stub(m => m.Disponibel)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();

            var updatedKontoModelMock = MockRepository.GenerateMock<IKontoModel>();
            updatedKontoModelMock.Stub(m => m.Regnskabsnummer)
                .Return(kontoModelMock.Regnskabsnummer)
                .Repeat.Any();
            updatedKontoModelMock.Stub(m => m.Kontonummer)
                .Return(kontoModelMock.Kontonummer)
                .Repeat.Any();
            updatedKontoModelMock.Stub(m => m.Kontonavn)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedKontoModelMock.Stub(m => m.Beskrivelse)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedKontoModelMock.Stub(m => m.Notat)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedKontoModelMock.Stub(m => m.Kontogruppe)
                .Return(kontogruppeModel2Mock.Nummer)
                .Repeat.Any();
            updatedKontoModelMock.Stub(m => m.StatusDato)
                .Return(kontoModelMock.StatusDato)
                .Repeat.Any();
            updatedKontoModelMock.Stub(m => m.Kredit)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            updatedKontoModelMock.Stub(m => m.Saldo)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            updatedKontoModelMock.Stub(m => m.Disponibel)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();

            var updatedSaldoOnKontoModelMock = MockRepository.GenerateMock<IKontoModel>();
            updatedSaldoOnKontoModelMock.Stub(m => m.Regnskabsnummer)
                .Return(kontoModelMock.Regnskabsnummer)
                .Repeat.Any();
            updatedSaldoOnKontoModelMock.Stub(m => m.Kontonummer)
                .Return(kontoModelMock.Kontonummer)
                .Repeat.Any();
            updatedSaldoOnKontoModelMock.Stub(m => m.Kontonavn)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedSaldoOnKontoModelMock.Stub(m => m.Beskrivelse)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedSaldoOnKontoModelMock.Stub(m => m.Notat)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedSaldoOnKontoModelMock.Stub(m => m.Kontogruppe)
                .Return(kontogruppeModel2Mock.Nummer)
                .Repeat.Any();
            updatedSaldoOnKontoModelMock.Stub(m => m.StatusDato)
                .Return(kontoModelMock.StatusDato.AddDays(rand.Next(1, 365)))
                .Repeat.Any();
            updatedSaldoOnKontoModelMock.Stub(m => m.Kredit)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            updatedSaldoOnKontoModelMock.Stub(m => m.Saldo)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            updatedSaldoOnKontoModelMock.Stub(m => m.Disponibel)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();

            var tempFile = new FileInfo(Path.GetTempFileName());
            try
            {
                var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), tempFile.FullName, FinansstyringRepositoryLocale.XmlSchema);
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
                var kontoNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']/ns:Konto[@kontonummer = '{1}']", regnskabModelMock.Nummer, kontoModelMock.Kontonummer));
                Assert.IsTrue(HasAttributeWhichMatchValue(kontoNode, "kontonavn", kontoModelMock.Kontonavn));
                Assert.IsTrue(HasAttributeWhichMatchValue(kontoNode, "beskrivelse", kontoModelMock.Beskrivelse));
                Assert.IsTrue(HasAttributeWhichMatchValue(kontoNode, "note", kontoModelMock.Notat));
                Assert.IsTrue(HasAttributeWhichMatchValue(kontoNode, "kontogruppe", kontoModelMock.Kontogruppe.ToString(CultureInfo.InvariantCulture)));

                var kontoHistorikNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']/ns:KontoHistorik[@kontonummer = '{1}' and @dato='{2}']", regnskabModelMock.Nummer, kontoModelMock.Kontonummer, kontoModelMock.StatusDato.ToString("yyyyMMdd")));
                Assert.IsTrue(HasAttributeWhichMatchValue(kontoHistorikNode, "kredit", kontoModelMock.Kredit.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(kontoHistorikNode, "saldo", kontoModelMock.Saldo.ToString(DecimalFormat, CultureInfo.InvariantCulture)));

                localeDataStorage.StoreSyncData(updatedKontoModelMock);
                var updatedKontoNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']/ns:Konto[@kontonummer = '{1}']", regnskabModelMock.Nummer, kontoModelMock.Kontonummer));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedKontoNode, "kontonavn", updatedKontoModelMock.Kontonavn));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedKontoNode, "beskrivelse", updatedKontoModelMock.Beskrivelse));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedKontoNode, "note", updatedKontoModelMock.Notat));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedKontoNode, "kontogruppe", updatedKontoModelMock.Kontogruppe.ToString(CultureInfo.InvariantCulture)));

                var updatedKontoHistorikNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']/ns:KontoHistorik[@kontonummer = '{1}' and @dato='{2}']", regnskabModelMock.Nummer, kontoModelMock.Kontonummer, kontoModelMock.StatusDato.ToString("yyyyMMdd")));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedKontoHistorikNode, "kredit", updatedKontoModelMock.Kredit.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedKontoHistorikNode, "saldo", updatedKontoModelMock.Saldo.ToString(DecimalFormat, CultureInfo.InvariantCulture)));

                localeDataStorage.StoreSyncData(updatedSaldoOnKontoModelMock);
                var updatedSaldoOnKontoHistorikNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']/ns:KontoHistorik[@kontonummer = '{1}' and @dato='{2}']", regnskabModelMock.Nummer, kontoModelMock.Kontonummer, updatedSaldoOnKontoModelMock.StatusDato.ToString("yyyyMMdd")));
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
            var fixture = new Fixture();
            var rand = new Random(fixture.Create<int>());

            var regnskabModelMock = MockRepository.GenerateMock<IRegnskabModel>();
            regnskabModelMock.Stub(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();
            regnskabModelMock.Stub(m => m.Navn)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var budgetkontogruppeModel1Mock = MockRepository.GenerateMock<IBudgetkontogruppeModel>();
            budgetkontogruppeModel1Mock.Stub(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();
            budgetkontogruppeModel1Mock.Stub(m => m.Tekst)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var budgetkontogruppeModel2Mock = MockRepository.GenerateMock<IBudgetkontogruppeModel>();
            budgetkontogruppeModel2Mock.Stub(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();
            budgetkontogruppeModel2Mock.Stub(m => m.Tekst)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var statusDato = DateTime.Today.AddDays(rand.Next(1, 365)*-1);
            var budgetkontoModelMock = MockRepository.GenerateMock<IBudgetkontoModel>();
            budgetkontoModelMock.Stub(m => m.Regnskabsnummer)
                .Return(regnskabModelMock.Nummer)
                .Repeat.Any();
            budgetkontoModelMock.Stub(m => m.Kontonummer)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            budgetkontoModelMock.Stub(m => m.Kontonavn)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            budgetkontoModelMock.Stub(m => m.Beskrivelse)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            budgetkontoModelMock.Stub(m => m.Notat)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            budgetkontoModelMock.Stub(m => m.Kontogruppe)
                .Return(budgetkontogruppeModel1Mock.Nummer)
                .Repeat.Any();
            budgetkontoModelMock.Stub(m => m.StatusDato)
                .Return(statusDato)
                .Repeat.Any();
            budgetkontoModelMock.Stub(m => m.Indtægter)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            budgetkontoModelMock.Stub(m => m.Udgifter)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            budgetkontoModelMock.Stub(m => m.Budget)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            budgetkontoModelMock.Stub(m => m.BudgetSidsteMåned)
                .Return(Math.Abs(fixture.Create<decimal>())*(rand.Next(0, 100) > 50 ? -1 : 1))
                .Repeat.Any();
            budgetkontoModelMock.Stub(m => m.Bogført)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            budgetkontoModelMock.Stub(m => m.BogførtSidsteMåned)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            budgetkontoModelMock.Stub(m => m.Disponibel)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();

            var updatedBudgetkontoModelMock = MockRepository.GenerateMock<IBudgetkontoModel>();
            updatedBudgetkontoModelMock.Stub(m => m.Regnskabsnummer)
                .Return(budgetkontoModelMock.Regnskabsnummer)
                .Repeat.Any();
            updatedBudgetkontoModelMock.Stub(m => m.Kontonummer)
                .Return(budgetkontoModelMock.Kontonummer)
                .Repeat.Any();
            updatedBudgetkontoModelMock.Stub(m => m.Kontonavn)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedBudgetkontoModelMock.Stub(m => m.Beskrivelse)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedBudgetkontoModelMock.Stub(m => m.Notat)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedBudgetkontoModelMock.Stub(m => m.Kontogruppe)
                .Return(budgetkontogruppeModel2Mock.Nummer)
                .Repeat.Any();
            updatedBudgetkontoModelMock.Stub(m => m.StatusDato)
                .Return(budgetkontoModelMock.StatusDato)
                .Repeat.Any();
            updatedBudgetkontoModelMock.Stub(m => m.Indtægter)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            updatedBudgetkontoModelMock.Stub(m => m.Udgifter)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            updatedBudgetkontoModelMock.Stub(m => m.Budget)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            updatedBudgetkontoModelMock.Stub(m => m.BudgetSidsteMåned)
                .Return(Math.Abs(fixture.Create<decimal>())*(rand.Next(0, 100) > 50 ? -1 : 1))
                .Repeat.Any();
            updatedBudgetkontoModelMock.Stub(m => m.Bogført)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            updatedBudgetkontoModelMock.Stub(m => m.BogførtSidsteMåned)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            updatedBudgetkontoModelMock.Stub(m => m.Disponibel)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();

            var updatedSaldoOnBudgetkontoModelMock = MockRepository.GenerateMock<IBudgetkontoModel>();
            updatedSaldoOnBudgetkontoModelMock.Stub(m => m.Regnskabsnummer)
                .Return(budgetkontoModelMock.Regnskabsnummer)
                .Repeat.Any();
            updatedSaldoOnBudgetkontoModelMock.Stub(m => m.Kontonummer)
                .Return(budgetkontoModelMock.Kontonummer)
                .Repeat.Any();
            updatedSaldoOnBudgetkontoModelMock.Stub(m => m.Kontonavn)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedSaldoOnBudgetkontoModelMock.Stub(m => m.Beskrivelse)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedSaldoOnBudgetkontoModelMock.Stub(m => m.Notat)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedSaldoOnBudgetkontoModelMock.Stub(m => m.Kontogruppe)
                .Return(budgetkontogruppeModel2Mock.Nummer)
                .Repeat.Any();
            updatedSaldoOnBudgetkontoModelMock.Stub(m => m.StatusDato)
                .Return(budgetkontoModelMock.StatusDato.AddDays(rand.Next(1, 365)))
                .Repeat.Any();
            updatedSaldoOnBudgetkontoModelMock.Stub(m => m.Indtægter)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            updatedSaldoOnBudgetkontoModelMock.Stub(m => m.Udgifter)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            updatedSaldoOnBudgetkontoModelMock.Stub(m => m.Budget)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            updatedSaldoOnBudgetkontoModelMock.Stub(m => m.BudgetSidsteMåned)
                .Return(Math.Abs(fixture.Create<decimal>())*(rand.Next(0, 100) > 50 ? -1 : 1))
                .Repeat.Any();
            updatedSaldoOnBudgetkontoModelMock.Stub(m => m.Bogført)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            updatedSaldoOnBudgetkontoModelMock.Stub(m => m.BogførtSidsteMåned)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            updatedSaldoOnBudgetkontoModelMock.Stub(m => m.Disponibel)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();

            var tempFile = new FileInfo(Path.GetTempFileName());
            try
            {
                var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), tempFile.FullName, FinansstyringRepositoryLocale.XmlSchema);
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
                var budgetkontoNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']/ns:Budgetkonto[@kontonummer = '{1}']", regnskabModelMock.Nummer, budgetkontoModelMock.Kontonummer));
                Assert.IsTrue(HasAttributeWhichMatchValue(budgetkontoNode, "kontonavn", budgetkontoModelMock.Kontonavn));
                Assert.IsTrue(HasAttributeWhichMatchValue(budgetkontoNode, "beskrivelse", budgetkontoModelMock.Beskrivelse));
                Assert.IsTrue(HasAttributeWhichMatchValue(budgetkontoNode, "note", budgetkontoModelMock.Notat));
                Assert.IsTrue(HasAttributeWhichMatchValue(budgetkontoNode, "kontogruppe", budgetkontoModelMock.Kontogruppe.ToString(CultureInfo.InvariantCulture)));

                var budgetkontoHistorikNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']/ns:BudgetkontoHistorik[@kontonummer = '{1}' and @dato='{2}']", regnskabModelMock.Nummer, budgetkontoModelMock.Kontonummer, budgetkontoModelMock.StatusDato.ToString("yyyyMMdd")));
                Assert.IsTrue(HasAttributeWhichMatchValue(budgetkontoHistorikNode, "indtaegter", budgetkontoModelMock.Indtægter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(budgetkontoHistorikNode, "udgifter", budgetkontoModelMock.Udgifter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(budgetkontoHistorikNode, "bogfoert", budgetkontoModelMock.Bogført.ToString(DecimalFormat, CultureInfo.InvariantCulture)));

                var lastMonthStatusDato = new DateTime(budgetkontoModelMock.StatusDato.AddMonths(-1).Year, budgetkontoModelMock.StatusDato.AddMonths(-1).Month, DateTime.DaysInMonth(budgetkontoModelMock.StatusDato.AddMonths(-1).Year, budgetkontoModelMock.StatusDato.AddMonths(-1).Month));
                var lastMonthIndtægter = Math.Abs(budgetkontoModelMock.BudgetSidsteMåned > 0 ? budgetkontoModelMock.BudgetSidsteMåned : 0M);
                var lastMonthUdgifter = Math.Abs(budgetkontoModelMock.BudgetSidsteMåned < 0 ? budgetkontoModelMock.BudgetSidsteMåned : 0M);
                var lastMonthBudgetkontoHistorikNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']/ns:BudgetkontoHistorik[@kontonummer = '{1}' and @dato='{2}']", regnskabModelMock.Nummer, budgetkontoModelMock.Kontonummer, lastMonthStatusDato.ToString("yyyyMMdd")));
                Assert.IsTrue(HasAttributeWhichMatchValue(lastMonthBudgetkontoHistorikNode, "indtaegter", lastMonthIndtægter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(lastMonthBudgetkontoHistorikNode, "udgifter", lastMonthUdgifter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(lastMonthBudgetkontoHistorikNode, "bogfoert", budgetkontoModelMock.BogførtSidsteMåned.ToString(DecimalFormat, CultureInfo.InvariantCulture)));

                localeDataStorage.StoreSyncData(updatedBudgetkontoModelMock);
                var updatedBudgetkontoNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']/ns:Budgetkonto[@kontonummer = '{1}']", regnskabModelMock.Nummer, budgetkontoModelMock.Kontonummer));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBudgetkontoNode, "kontonavn", updatedBudgetkontoModelMock.Kontonavn));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBudgetkontoNode, "beskrivelse", updatedBudgetkontoModelMock.Beskrivelse));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBudgetkontoNode, "note", updatedBudgetkontoModelMock.Notat));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBudgetkontoNode, "kontogruppe", updatedBudgetkontoModelMock.Kontogruppe.ToString(CultureInfo.InvariantCulture)));

                var updatedBudgetkontoHistorikNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']/ns:BudgetkontoHistorik[@kontonummer = '{1}' and @dato='{2}']", regnskabModelMock.Nummer, budgetkontoModelMock.Kontonummer, budgetkontoModelMock.StatusDato.ToString("yyyyMMdd")));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBudgetkontoHistorikNode, "indtaegter", updatedBudgetkontoModelMock.Indtægter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBudgetkontoHistorikNode, "udgifter", updatedBudgetkontoModelMock.Udgifter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBudgetkontoHistorikNode, "bogfoert", updatedBudgetkontoModelMock.Bogført.ToString(DecimalFormat, CultureInfo.InvariantCulture)));

                lastMonthStatusDato = new DateTime(updatedBudgetkontoModelMock.StatusDato.AddMonths(-1).Year, updatedBudgetkontoModelMock.StatusDato.AddMonths(-1).Month, DateTime.DaysInMonth(updatedBudgetkontoModelMock.StatusDato.AddMonths(-1).Year, updatedBudgetkontoModelMock.StatusDato.AddMonths(-1).Month));
                lastMonthIndtægter = Math.Abs(updatedBudgetkontoModelMock.BudgetSidsteMåned > 0 ? updatedBudgetkontoModelMock.BudgetSidsteMåned : 0M);
                lastMonthUdgifter = Math.Abs(updatedBudgetkontoModelMock.BudgetSidsteMåned < 0 ? updatedBudgetkontoModelMock.BudgetSidsteMåned : 0M);
                var lastMonthUpdatedBudgetkontoHistorikNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']/ns:BudgetkontoHistorik[@kontonummer = '{1}' and @dato='{2}']", regnskabModelMock.Nummer, budgetkontoModelMock.Kontonummer, lastMonthStatusDato.ToString("yyyyMMdd")));
                Assert.IsTrue(HasAttributeWhichMatchValue(lastMonthUpdatedBudgetkontoHistorikNode, "indtaegter", lastMonthIndtægter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(lastMonthUpdatedBudgetkontoHistorikNode, "udgifter", lastMonthUdgifter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(lastMonthUpdatedBudgetkontoHistorikNode, "bogfoert", updatedBudgetkontoModelMock.BogførtSidsteMåned.ToString(DecimalFormat, CultureInfo.InvariantCulture)));

                localeDataStorage.StoreSyncData(updatedSaldoOnBudgetkontoModelMock);
                var updatedSaldoOnBudgetkontoHistorikNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']/ns:BudgetkontoHistorik[@kontonummer = '{1}' and @dato='{2}']", regnskabModelMock.Nummer, budgetkontoModelMock.Kontonummer, updatedSaldoOnBudgetkontoModelMock.StatusDato.ToString("yyyyMMdd")));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedSaldoOnBudgetkontoHistorikNode, "indtaegter", updatedSaldoOnBudgetkontoModelMock.Indtægter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedSaldoOnBudgetkontoHistorikNode, "udgifter", updatedSaldoOnBudgetkontoModelMock.Udgifter.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedSaldoOnBudgetkontoHistorikNode, "bogfoert", updatedSaldoOnBudgetkontoModelMock.Bogført.ToString(DecimalFormat, CultureInfo.InvariantCulture)));

                lastMonthStatusDato = new DateTime(updatedSaldoOnBudgetkontoModelMock.StatusDato.AddMonths(-1).Year, updatedSaldoOnBudgetkontoModelMock.StatusDato.AddMonths(-1).Month, DateTime.DaysInMonth(updatedSaldoOnBudgetkontoModelMock.StatusDato.AddMonths(-1).Year, updatedSaldoOnBudgetkontoModelMock.StatusDato.AddMonths(-1).Month));
                lastMonthIndtægter = Math.Abs(updatedSaldoOnBudgetkontoModelMock.BudgetSidsteMåned > 0 ? updatedSaldoOnBudgetkontoModelMock.BudgetSidsteMåned : 0M);
                lastMonthUdgifter = Math.Abs(updatedSaldoOnBudgetkontoModelMock.BudgetSidsteMåned < 0 ? updatedSaldoOnBudgetkontoModelMock.BudgetSidsteMåned : 0M);
                var lastMonthUpdatedSaldoOnBudgetkontoHistorikNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']/ns:BudgetkontoHistorik[@kontonummer = '{1}' and @dato='{2}']", regnskabModelMock.Nummer, budgetkontoModelMock.Kontonummer, lastMonthStatusDato.ToString("yyyyMMdd")));
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
            var fixture = new Fixture();
            var rand = new Random(fixture.Create<int>());

            var regnskabModelMock = MockRepository.GenerateMock<IRegnskabModel>();
            regnskabModelMock.Stub(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();
            regnskabModelMock.Stub(m => m.Navn)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var statusDato = DateTime.Today.AddDays(rand.Next(1, 365)*-1);
            var adressekontoModelMock = MockRepository.GenerateMock<IAdressekontoModel>();
            adressekontoModelMock.Stub(m => m.Regnskabsnummer)
                .Return(regnskabModelMock.Nummer)
                .Repeat.Any();
            adressekontoModelMock.Stub(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();
            adressekontoModelMock.Stub(m => m.Navn)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            adressekontoModelMock.Stub(m => m.PrimærTelefon)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            adressekontoModelMock.Stub(m => m.SekundærTelefon)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            adressekontoModelMock.Stub(m => m.StatusDato)
                .Return(statusDato)
                .Repeat.Any();
            adressekontoModelMock.Stub(m => m.Saldo)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();

            var updatedAdressekontoModelMock = MockRepository.GenerateMock<IAdressekontoModel>();
            updatedAdressekontoModelMock.Stub(m => m.Regnskabsnummer)
                .Return(adressekontoModelMock.Regnskabsnummer)
                .Repeat.Any();
            updatedAdressekontoModelMock.Stub(m => m.Nummer)
                .Return(adressekontoModelMock.Nummer)
                .Repeat.Any();
            updatedAdressekontoModelMock.Stub(m => m.Navn)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedAdressekontoModelMock.Stub(m => m.PrimærTelefon)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedAdressekontoModelMock.Stub(m => m.SekundærTelefon)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedAdressekontoModelMock.Stub(m => m.StatusDato)
                .Return(adressekontoModelMock.StatusDato)
                .Repeat.Any();
            updatedAdressekontoModelMock.Stub(m => m.Saldo)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();

            var updatedSaldoOnAdressekontoModelMock = MockRepository.GenerateMock<IAdressekontoModel>();
            updatedSaldoOnAdressekontoModelMock.Stub(m => m.Regnskabsnummer)
                .Return(adressekontoModelMock.Regnskabsnummer)
                .Repeat.Any();
            updatedSaldoOnAdressekontoModelMock.Stub(m => m.Nummer)
                .Return(adressekontoModelMock.Nummer)
                .Repeat.Any();
            updatedSaldoOnAdressekontoModelMock.Stub(m => m.Navn)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedSaldoOnAdressekontoModelMock.Stub(m => m.PrimærTelefon)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedSaldoOnAdressekontoModelMock.Stub(m => m.SekundærTelefon)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedSaldoOnAdressekontoModelMock.Stub(m => m.StatusDato)
                .Return(adressekontoModelMock.StatusDato.AddDays(rand.Next(1, 365)))
                .Repeat.Any();
            updatedSaldoOnAdressekontoModelMock.Stub(m => m.Saldo)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();

            var tempFile = new FileInfo(Path.GetTempFileName());
            try
            {
                var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), tempFile.FullName, FinansstyringRepositoryLocale.XmlSchema);
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
                var adressekontoNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']/ns:Adressekonto[@nummer = '{1}']", regnskabModelMock.Nummer, adressekontoModelMock.Nummer.ToString(CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(adressekontoNode, "navn", adressekontoModelMock.Navn));
                Assert.IsTrue(HasAttributeWhichMatchValue(adressekontoNode, "primaerTelefon", adressekontoModelMock.PrimærTelefon));
                Assert.IsTrue(HasAttributeWhichMatchValue(adressekontoNode, "sekundaerTelefon", adressekontoModelMock.SekundærTelefon));

                var adressekontoHistorikNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']/ns:AdressekontoHistorik[@nummer = '{1}' and @dato='{2}']", regnskabModelMock.Nummer, adressekontoModelMock.Nummer.ToString(CultureInfo.InvariantCulture), adressekontoModelMock.StatusDato.ToString("yyyyMMdd")));
                Assert.IsTrue(HasAttributeWhichMatchValue(adressekontoHistorikNode, "saldo", adressekontoModelMock.Saldo.ToString(DecimalFormat, CultureInfo.InvariantCulture)));

                localeDataStorage.StoreSyncData(updatedAdressekontoModelMock);
                var updatedAdressekontoNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']/ns:Adressekonto[@nummer = '{1}']", regnskabModelMock.Nummer, adressekontoModelMock.Nummer.ToString(CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedAdressekontoNode, "navn", updatedAdressekontoModelMock.Navn));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedAdressekontoNode, "primaerTelefon", updatedAdressekontoModelMock.PrimærTelefon));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedAdressekontoNode, "sekundaerTelefon", updatedAdressekontoModelMock.SekundærTelefon));

                var updatedAdressekontoHistorikNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']/ns:AdressekontoHistorik[@nummer = '{1}' and @dato='{2}']", regnskabModelMock.Nummer, adressekontoModelMock.Nummer.ToString(CultureInfo.InvariantCulture), adressekontoModelMock.StatusDato.ToString("yyyyMMdd")));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedAdressekontoHistorikNode, "saldo", updatedAdressekontoModelMock.Saldo.ToString(DecimalFormat, CultureInfo.InvariantCulture)));

                localeDataStorage.StoreSyncData(updatedSaldoOnAdressekontoModelMock);
                var updatedSaldoOnAdressekontoHistorikNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']/ns:AdressekontoHistorik[@nummer = '{1}' and @dato='{2}']", regnskabModelMock.Nummer, updatedSaldoOnAdressekontoModelMock.Nummer.ToString(CultureInfo.InvariantCulture), updatedSaldoOnAdressekontoModelMock.StatusDato.ToString("yyyyMMdd")));
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
            var fixture = new Fixture();
            var rand = new Random(fixture.Create<int>());

            var regnskabModelMock = MockRepository.GenerateMock<IRegnskabModel>();
            regnskabModelMock.Stub(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();
            regnskabModelMock.Stub(m => m.Navn)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var statusDato = DateTime.Today.AddDays(rand.Next(1, 365)*-1);
            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Stub(m => m.Regnskabsnummer)
                .Return(regnskabModelMock.Nummer)
                .Repeat.Any();
            bogføringslinjeModelMock.Stub(m => m.Løbenummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();
            bogføringslinjeModelMock.Stub(m => m.Dato)
                .Return(statusDato)
                .Repeat.Any();
            bogføringslinjeModelMock.Stub(m => m.Bilag)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            bogføringslinjeModelMock.Stub(m => m.Kontonummer)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            bogføringslinjeModelMock.Stub(m => m.Tekst)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            bogføringslinjeModelMock.Stub(m => m.Budgetkontonummer)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            bogføringslinjeModelMock.Stub(m => m.Debit)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            bogføringslinjeModelMock.Stub(m => m.Kredit)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            bogføringslinjeModelMock.Stub(m => m.Bogført)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            bogføringslinjeModelMock.Stub(m => m.Adressekonto)
                .Return(fixture.Create<int>())
                .Repeat.Any();

            var updatedBogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            updatedBogføringslinjeModelMock.Stub(m => m.Regnskabsnummer)
                .Return(bogføringslinjeModelMock.Regnskabsnummer)
                .Repeat.Any();
            updatedBogføringslinjeModelMock.Stub(m => m.Løbenummer)
                .Return(bogføringslinjeModelMock.Løbenummer)
                .Repeat.Any();
            updatedBogføringslinjeModelMock.Stub(m => m.Dato)
                .Return(statusDato.AddDays(rand.Next(1, 365)))
                .Repeat.Any();
            updatedBogføringslinjeModelMock.Stub(m => m.Bilag)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedBogføringslinjeModelMock.Stub(m => m.Kontonummer)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedBogføringslinjeModelMock.Stub(m => m.Tekst)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedBogføringslinjeModelMock.Stub(m => m.Budgetkontonummer)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedBogføringslinjeModelMock.Stub(m => m.Debit)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            updatedBogføringslinjeModelMock.Stub(m => m.Kredit)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            updatedBogføringslinjeModelMock.Stub(m => m.Bogført)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            updatedBogføringslinjeModelMock.Stub(m => m.Adressekonto)
                .Return(fixture.Create<int>())
                .Repeat.Any();

            var tempFile = new FileInfo(Path.GetTempFileName());
            try
            {
                var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), tempFile.FullName, FinansstyringRepositoryLocale.XmlSchema);
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
                var bogføringslinjeNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']/ns:Bogfoeringslinje[@loebenummer = '{1}']", regnskabModelMock.Nummer, bogføringslinjeModelMock.Løbenummer.ToString(CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(bogføringslinjeNode, "dato", bogføringslinjeModelMock.Dato.ToString("yyyyMMdd")));
                Assert.IsTrue(HasAttributeWhichMatchValue(bogføringslinjeNode, "bilag", bogføringslinjeModelMock.Bilag));
                Assert.IsTrue(HasAttributeWhichMatchValue(bogføringslinjeNode, "kontonummer", bogføringslinjeModelMock.Kontonummer));
                Assert.IsTrue(HasAttributeWhichMatchValue(bogføringslinjeNode, "tekst", bogføringslinjeModelMock.Tekst));
                Assert.IsTrue(HasAttributeWhichMatchValue(bogføringslinjeNode, "budgetkontonummer", bogføringslinjeModelMock.Budgetkontonummer));
                Assert.IsTrue(HasAttributeWhichMatchValue(bogføringslinjeNode, "debit", bogføringslinjeModelMock.Debit.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(bogføringslinjeNode, "kredit", bogføringslinjeModelMock.Kredit.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(bogføringslinjeNode, "adressekonto", bogføringslinjeModelMock.Adressekonto.ToString(CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(bogføringslinjeNode, "synkroniseret", "true"));

                localeDataStorage.StoreSyncData(updatedBogføringslinjeModelMock);
                var updatedBogføringslinjeNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Regnskab[@nummer = '{0}']/ns:Bogfoeringslinje[@loebenummer = '{1}']", regnskabModelMock.Nummer, bogføringslinjeModelMock.Løbenummer.ToString(CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBogføringslinjeNode, "dato", updatedBogføringslinjeModelMock.Dato.ToString("yyyyMMdd")));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBogføringslinjeNode, "bilag", updatedBogføringslinjeModelMock.Bilag));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBogføringslinjeNode, "kontonummer", updatedBogføringslinjeModelMock.Kontonummer));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBogføringslinjeNode, "tekst", updatedBogføringslinjeModelMock.Tekst));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBogføringslinjeNode, "budgetkontonummer", updatedBogføringslinjeModelMock.Budgetkontonummer));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBogføringslinjeNode, "debit", updatedBogføringslinjeModelMock.Debit.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBogføringslinjeNode, "kredit", updatedBogføringslinjeModelMock.Kredit.ToString(DecimalFormat, CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBogføringslinjeNode, "adressekonto", updatedBogføringslinjeModelMock.Adressekonto.ToString(CultureInfo.InvariantCulture)));
                Assert.IsTrue(HasAttributeWhichMatchValue(updatedBogføringslinjeNode, "synkroniseret", "true"));
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
            var fixture = new Fixture();
            var kontogruppeModelMock = MockRepository.GenerateMock<IKontogruppeModel>();
            kontogruppeModelMock.Stub(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();
            kontogruppeModelMock.Stub(m => m.Tekst)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            kontogruppeModelMock.Stub(m => m.Balancetype)
                .Return(Balancetype.Aktiver)
                .Repeat.Any();
            var updatedKontogruppeModelMock = MockRepository.GenerateMock<IKontogruppeModel>();
            updatedKontogruppeModelMock.Stub(m => m.Nummer)
                .Return(kontogruppeModelMock.Nummer)
                .Repeat.Any();
            updatedKontogruppeModelMock.Stub(m => m.Tekst)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            updatedKontogruppeModelMock.Stub(m => m.Balancetype)
                .Return(Balancetype.Passiver)
                .Repeat.Any();

            var tempFile = new FileInfo(Path.GetTempFileName());
            try
            {
                var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), tempFile.FullName, FinansstyringRepositoryLocale.XmlSchema);
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
                var kontogruppeNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Kontogruppe[@nummer = '{0}']", kontogruppeModelMock.Nummer));
                Assert.IsTrue(HasAttributeWhichMatchValue(kontogruppeNode, "tekst", kontogruppeModelMock.Tekst));
                Assert.IsTrue(HasAttributeWhichMatchValue(kontogruppeNode, "balanceType", kontogruppeModelMock.Balancetype.ToString()));

                localeDataStorage.StoreSyncData(updatedKontogruppeModelMock);
                var updatedKontogruppeNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Kontogruppe[@nummer = '{0}']", kontogruppeModelMock.Nummer));
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
            var fixture = new Fixture();
            var budgetkontogruppeModelMock = MockRepository.GenerateMock<IBudgetkontogruppeModel>();
            budgetkontogruppeModelMock.Stub(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();
            budgetkontogruppeModelMock.Stub(m => m.Tekst)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            var updatedBudgetkontogruppeModelMock = MockRepository.GenerateMock<IBudgetkontogruppeModel>();
            updatedBudgetkontogruppeModelMock.Stub(m => m.Nummer)
                .Return(budgetkontogruppeModelMock.Nummer)
                .Repeat.Any();
            updatedBudgetkontogruppeModelMock.Stub(m => m.Tekst)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var tempFile = new FileInfo(Path.GetTempFileName());
            try
            {
                var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), tempFile.FullName, FinansstyringRepositoryLocale.XmlSchema);
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
                var budgetkontogruppeNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Budgetkontogruppe[@nummer = '{0}']", budgetkontogruppeModelMock.Nummer));
                Assert.IsTrue(HasAttributeWhichMatchValue(budgetkontogruppeNode, "tekst", budgetkontogruppeModelMock.Tekst));

                localeDataStorage.StoreSyncData(updatedBudgetkontogruppeModelMock);
                var updatedBudgetkontogruppeNode = GetNodeFromXPath(tempFile.FullName, string.Format("/ns:FinansstyringRepository/ns:Budgetkontogruppe[@nummer = '{0}']", budgetkontogruppeModelMock.Nummer));
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
            var memoryStream = new MemoryStream();
            var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8);
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
        /// Eventhandler, der håndterer XML validering.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        private static void ValidationEventHandler(object sender, ValidationEventArgs eventArgs)
        {
            Assert.That(sender, Is.Not.Null);
            Assert.That(eventArgs, Is.Not.Null);
            switch (eventArgs.Severity)
            {
                case XmlSeverityType.Warning:
                    Assert.Fail(eventArgs.Message);
                    break;

                case XmlSeverityType.Error:
                    Assert.Fail(eventArgs.Message);
                    break;
            }
        }

        /// <summary>
        /// Finder og returnerer en given node i et givent XML dokument på baggrund af en xpath.
        /// </summary>
        /// <param name="localeDataFileName">Filnavnet på XML dokumentet, hvorfra en given node skal returneres.</param>
        /// <param name="xpath">XPath til noden, der ønskes returneret.</param>
        /// <returns>XML node.</returns>
        private static XmlNode GetNodeFromXPath(string localeDataFileName, string xpath)
        {
            if (string.IsNullOrEmpty(localeDataFileName))
            {
                throw new ArgumentNullException("localeDataFileName");
            }
            if (string.IsNullOrEmpty(xpath))
            {
                throw new ArgumentNullException("xpath");
            }
            
            var localeDataDocument = new XmlDocument();
            localeDataDocument.Load(localeDataFileName);

            var namespaceManager = new XmlNamespaceManager(localeDataDocument.NameTable);
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
                throw new ArgumentNullException("xmlNode");
            }
            if (string.IsNullOrEmpty(attributeName))
            {
                throw new ArgumentNullException("attributeName");
            }
            if (xmlNode.Attributes == null)
            {
                return false;
            }
            var attribute = xmlNode.Attributes[attributeName];
            if (attribute == null)
            {
                return false;
            }
            return string.Compare(attribute.Value, attributeValue, StringComparison.InvariantCulture) == 0;
        }
    }
}
