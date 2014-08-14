using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Schema;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.Resources;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Finansstyring.Tests
{
    /// <summary>
    /// Tester repository, der supporterer lokale data til finansstyring.
    /// </summary>
    [TestFixture]
    public class FinansstyringRepositoryLocaleTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer repositoryet, der supporterer lokale data til finansstyring.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererFinansstyringRepositoryLocale()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));
            fixture.Customize<ILocaleDataStorage>(e => e.FromFactory(() => MockRepository.GenerateMock<ILocaleDataStorage>()));

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            var finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(finansstyringKonfigurationRepositoryMock, fixture.Create<ILocaleDataStorage>());
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);
            Assert.That(finansstyringRepositoryLocale.Konfiguration, Is.Not.Null);
            Assert.That(finansstyringRepositoryLocale.Konfiguration, Is.EqualTo(finansstyringKonfigurationRepositoryMock));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis konfigurationsrepository, der supporterer finansstyring, er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFinansstyringKonfigurationRepositoryErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<ILocaleDataStorage>(e => e.FromFactory(() => MockRepository.GenerateMock<ILocaleDataStorage>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new FinansstyringRepositoryLocale(null, fixture.Create<ILocaleDataStorage>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("finansstyringKonfigurationRepository"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis det lokale datalager er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisLocaleDataStorageErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new FinansstyringRepositoryLocale(fixture.Create<IFinansstyringKonfigurationRepository>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("localeDataStorage"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at RegnskabslisteGetAsync henter regnskaber.
        /// </summary>
        [Test]
        public async void TestAtRegnskabslisteGetAsyncHenterRegnskaber()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            var localeDataStorageMock = MockRepository.GenerateMock<ILocaleDataStorage>();
            localeDataStorageMock.Stub(m => m.HasLocaleData)
                .Return(true)
                .Repeat.Any();
            localeDataStorageMock.Stub(m => m.GetLocaleData())
                .Return(GenerateTestData(fixture))
                .Repeat.Any();

            var finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(fixture.Create<IFinansstyringKonfigurationRepository>(), localeDataStorageMock);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            var result = await finansstyringRepositoryLocale.RegnskabslisteGetAsync();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count(), Is.EqualTo(3));

            localeDataStorageMock.AssertWasCalled(m => m.GetLocaleData());
        }

        /// <summary>
        /// Danner testdata.
        /// </summary>
        /// <param name="fixture">Fixture, der kan generere random data.</param>
        /// <returns>Testdata.</returns>
        private static XDocument GenerateTestData(ISpecimenBuilder fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }
            // Create testdata.
            var localeDataDocument = new XDocument(new XDeclaration("1.0", "utf-8", null));
            localeDataDocument.Add(new XElement(XName.Get("FinansstyringRepository", FinansstyringRepositoryLocale.Namespace)));
            for (var i = 0; i < 3; i++)
            {
                var regnskabModel = CreateRegnskab(fixture, i + 1);
                regnskabModel.StoreInDocument(localeDataDocument);
            }
            
            // Validation.
            var schemaSet = new XmlSchemaSet();
            var assembly = typeof(FinansstyringRepositoryLocale).Assembly;
            using (var resourceReader = assembly.GetManifestResourceStream(string.Format("{0}.{1}", assembly.GetName().Name, FinansstyringRepositoryLocale.XmlSchema)))
            {
                if (resourceReader == null)
                {
                    throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.UnableToLoadResource, FinansstyringRepositoryLocale.XmlSchema));
                }
                schemaSet.Add(XmlSchema.Read(resourceReader, ValidationEventHandler));
                resourceReader.Close();
            }
            localeDataDocument.Validate(schemaSet, ValidationEventHandler);

            return localeDataDocument;
        }

        /// <summary>
        /// Danner testdata til et regnskab.
        /// </summary>
        /// <param name="fixture">Fixture, der kan generere random data.</param>
        /// <param name="nummer">Unik identifikation af regnskabet.</param>
        /// <returns>Testdata til et regnskab.</returns>
        private static IRegnskabModel CreateRegnskab(ISpecimenBuilder fixture, int nummer)
        {
            var regnskabModelMock = MockRepository.GenerateMock<IRegnskabModel>();
            regnskabModelMock.Expect(m => m.Nummer)
                .Return(nummer)
                .Repeat.Any();
            regnskabModelMock.Expect(m => m.Navn)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            return regnskabModelMock;
        }

        /// <summary>
        /// Eventhandler, der håndterer XML validering.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        private static void ValidationEventHandler(object sender, ValidationEventArgs eventArgs)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            if (eventArgs == null)
            {
                throw new ArgumentNullException("eventArgs");
            }
            switch (eventArgs.Severity)
            {
                case XmlSeverityType.Warning:
                    throw new IntranetGuiRepositoryException(eventArgs.Message, eventArgs.Exception);

                case XmlSeverityType.Error:
                    throw new IntranetGuiRepositoryException(eventArgs.Message, eventArgs.Exception);
            }
        }
    }
}
