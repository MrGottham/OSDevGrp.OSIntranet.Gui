using System;
using System.IO;
using System.Linq;
using System.Xml.Schema;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces;
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
                e.Result = new MemoryStream();
                eventCalled = true;
            };

            Assert.That(eventCalled, Is.False);
            Assert.That(localeDataStorage.GetLocaleData(), Is.Not.Null);
            Assert.That(eventCalled, Is.True);
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

            var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>());
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
                e.Result = new MemoryStream();
                eventCalled = true;
            };

            Assert.That(eventCalled, Is.False);
            localeDataStorage.StoreLocaleData(fixture.Create<IModel>());
            Assert.That(eventCalled, Is.True);
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

            var localeDataStorage = new LocaleDataStorage(fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>());
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
                e.Result = new MemoryStream();
                eventCalled = true;
            };

            Assert.That(eventCalled, Is.False);
            localeDataStorage.StoreSyncData(fixture.Create<IModel>());
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Eventhandler, der håndterer XML validering.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
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
    }
}
