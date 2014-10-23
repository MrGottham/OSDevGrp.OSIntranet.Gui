using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Resources;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Finansstyring.Tests
{
    /// <summary>
    /// Tester det lokale datalager.
    /// </summary>
    [TestFixture]
    public class LocaleDataStorageTests
    {
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

        // TODO: Test StoreLocaleData with Models (creations and updates).

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
        /// Tester, at StoreSyncData( gemmer data i et lokalt fil storage.
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

        // TODO: Test StoreSyncData with Models (creations and updates).

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
        /// Danner en stream indeholdende XML, der kan benyttes til test.
        /// </summary>
        private static Stream CreateMemoryStreamWithXmlContent()
        {
            var memoryStream = new MemoryStream();
            var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8);
            streamWriter.Write("<?xml version=\"1.0\" encoding=\"utf-8\"?><FinansstyringRepository xmlns=\"{0}\"/>", FinansstyringRepositoryLocale.Namespace);
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
