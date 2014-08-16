﻿using System;
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
        #region Private variables

        private static XDocument _localeDataDocument;
        private static readonly object SyncRoot = new object();

        #endregion

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
        /// Tester, at KontoplanGetAsync henter kontoplanen til et regnskab.
        /// </summary>
        [Test]
        [TestCase(1, 15)]
        [TestCase(2, 15)]
        [TestCase(3, 15)]
        [TestCase(4, 0)]
        [TestCase(5, 0)]
        public async void TestAtKontoplanGetAsyncHenterKontoplan(int regnskabsnummer, int expectedKonti)
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

            var result = await finansstyringRepositoryLocale.KontoplanGetAsync(regnskabsnummer, DateTime.Now);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(expectedKonti));

            localeDataStorageMock.AssertWasCalled(m => m.GetLocaleData());
        }

        /// <summary>
        /// Tester, at KontogruppelisteGetAsync henter listen af kontogrupper.
        /// </summary>
        [Test]
        public async void TestAtKontogruppelisteGetAsyncHenterKontogrupper()
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

            var result = await finansstyringRepositoryLocale.KontogruppelisteGetAsync();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count(), Is.EqualTo(15));

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
            lock (SyncRoot)
            {
                if (_localeDataDocument != null)
                {
                    return _localeDataDocument;
                }
                // Create testdata.
                var localeDataDocument = new XDocument(new XDeclaration("1.0", "utf-8", null));
                localeDataDocument.Add(new XElement(XName.Get("FinansstyringRepository", FinansstyringRepositoryLocale.Namespace)));
                for (var i = 0; i < 3; i++)
                {
                    var regnskabModel = CreateRegnskab(fixture, i + 1);
                    regnskabModel.StoreInDocument(localeDataDocument);
                    for (var j = 0; j < 15; j++)
                    {
                        var kontoModel = CreateKonto(fixture, i + 1, string.Format("KONTO{0:00}", j + 1));
                        kontoModel.StoreInDocument(localeDataDocument);
                    }
                }
                for (var i = 0; i < 15; i++)
                {
                    var kontogruppeModel = CreateKontogruppe(fixture, i + 1);
                    kontogruppeModel.StoreInDocument(localeDataDocument);
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

                _localeDataDocument = localeDataDocument;
                return _localeDataDocument;
            }
        }

        /// <summary>
        /// Danner testdata til et regnskab.
        /// </summary>
        /// <param name="fixture">Fixture, der kan generere random data.</param>
        /// <param name="nummer">Unik identifikation af regnskabet.</param>
        /// <returns>Testdata til et regnskab.</returns>
        private static IRegnskabModel CreateRegnskab(ISpecimenBuilder fixture, int nummer)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }
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
        /// Danner testdata til en konto.
        /// </summary>
        /// <param name="fixture">Fixture, der kan generere random data.</param>
        /// <param name="regnskabsnummer">Unik identifikation af regnskabet, som kontoen skal være tilknyttet.</param>
        /// <param name="kontonummer">Unik identifikation af kontoen.</param>
        /// <returns>Testdata til en konto.</returns>
        private static IKontoModel CreateKonto(ISpecimenBuilder fixture, int regnskabsnummer, string kontonummer)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }
            if (string.IsNullOrEmpty(kontonummer))
            {
                throw new ArgumentNullException("kontonummer");
            }
            var kontogruppe = (DateTime.Now.Millisecond%15) + 1;
            var kontoModelMock = MockRepository.GenerateMock<IKontoModel>();
            kontoModelMock.Expect(m => m.Regnskabsnummer)
                .Return(regnskabsnummer)
                .Repeat.Any();
            kontoModelMock.Expect(m => m.Kontonummer)
                .Return(kontonummer)
                .Repeat.Any();
            kontoModelMock.Expect(m => m.Kontonavn)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            kontoModelMock.Expect(m => m.Beskrivelse)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            kontoModelMock.Expect(m => m.Notat)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            kontoModelMock.Expect(m => m.Kontogruppe)
                .Return(kontogruppe)
                .Repeat.Any();
            kontoModelMock.Expect(m => m.StatusDato)
                .Return(DateTime.Now.Date)
                .Repeat.Any();
            kontoModelMock.Expect(m => m.Kredit)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            kontoModelMock.Expect(m => m.Saldo)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            return kontoModelMock;
        }

        /// <summary>
        /// Danner testdata til en kontogruppe.
        /// </summary>
        /// <param name="fixture">Fixture, der kan generere random data.</param>
        /// <param name="nummer">Unik identifikation af kontogruppen.</param>
        /// <returns>Testdata til en kontogruppe.</returns>
        private static IKontogruppeModel CreateKontogruppe(ISpecimenBuilder fixture, int nummer)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }
            var kontogruppeModelMock = MockRepository.GenerateMock<IKontogruppeModel>();
            kontogruppeModelMock.Expect(m => m.Nummer)
                .Return(nummer)
                .Repeat.Any();
            kontogruppeModelMock.Expect(m => m.Tekst)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            kontogruppeModelMock.Expect(m => m.Balancetype)
                .Return(nummer%2 != 0 ? Balancetype.Aktiver : Balancetype.Passiver)
                .Repeat.Any();
            return kontogruppeModelMock;
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
