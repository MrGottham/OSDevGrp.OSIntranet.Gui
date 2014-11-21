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
        /// Tester, at BudgetkontoplanGetAsync henter budgetkontoplanen til et regnskab.
        /// </summary>
        [Test]
        [TestCase(1, 30)]
        [TestCase(2, 30)]
        [TestCase(3, 30)]
        [TestCase(4, 0)]
        [TestCase(5, 0)]
        public async void TestAtBudgetkontoplanGetAsyncHenterBudgetkontoplan(int regnskabsnummer, int expectedBudgetkonti)
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

            var result = await finansstyringRepositoryLocale.BudgetkontoplanGetAsync(regnskabsnummer, DateTime.Now);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(expectedBudgetkonti));

            localeDataStorageMock.AssertWasCalled(m => m.GetLocaleData());
        }

        /// <summary>
        /// Tester, at BogføringslinjerGetAsync henter bogføringslinjer til et regnskab.
        /// </summary>
        [Test]
        [TestCase(1, 50)]
        [TestCase(2, 50)]
        [TestCase(3, 50)]
        [TestCase(4, 0)]
        [TestCase(5, 0)]
        public async void TestAtBogføringslinjerGetAsyncHenterBogføringslinjer(int regnskabsnummer, int expectedBogføringslinjer)
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

            var result = await finansstyringRepositoryLocale.BogføringslinjerGetAsync(regnskabsnummer, DateTime.Now, expectedBogføringslinjer);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(expectedBogføringslinjer));

            localeDataStorageMock.AssertWasCalled(m => m.GetLocaleData());
        }

        /// <summary>
        /// Tester, at DebitorlisteGetAsync henter listen af debitorer i et regnskab.
        /// </summary>
        [Test]
        [TestCase(1, 25)]
        [TestCase(2, 25)]
        [TestCase(3, 25)]
        [TestCase(4, 0)]
        [TestCase(5, 0)]
        public async void TestAtDebitorlisteGetAsyncHenterDebitorer(int regnskabsnummer, int expectedDebitorer)
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

            var result = await finansstyringRepositoryLocale.DebitorlisteGetAsync(regnskabsnummer, DateTime.Now);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(expectedDebitorer));

            localeDataStorageMock.AssertWasCalled(m => m.GetLocaleData());
        }

        /// <summary>
        /// Tester, at KreditorlisteGetAsync henter listen af kreditorer i et regnskab.
        /// </summary>
        [Test]
        [TestCase(1, 15)]
        [TestCase(2, 15)]
        [TestCase(3, 15)]
        [TestCase(4, 0)]
        [TestCase(5, 0)]
        public async void TestAtKreditorlisteGetAsyncHenterKreditorer(int regnskabsnummer, int expectedKreditorer)
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

            var result = await finansstyringRepositoryLocale.KreditorlisteGetAsync(regnskabsnummer, DateTime.Now);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(expectedKreditorer));

            localeDataStorageMock.AssertWasCalled(m => m.GetLocaleData());
        }

        /// <summary>
        /// Tester, at AdressekontolisteGetAsync henter adressekonti til et regnskab.
        /// </summary>
        [Test]
        [TestCase(1, 40)]
        [TestCase(2, 40)]
        [TestCase(3, 40)]
        [TestCase(4, 0)]
        [TestCase(5, 0)]
        public async void TestAtAdressekontolisteGetAsyncHenterAdressekonti(int regnskabsnummer, int expectedAdressekonti)
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

            var result = await finansstyringRepositoryLocale.AdressekontolisteGetAsync(regnskabsnummer, DateTime.Now);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(expectedAdressekonti));

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
        /// Tester, at BudgetkontogruppelisteGetAsync henter listen af kontogrupper til budgetkonti.
        /// </summary>
        [Test]
        public async void TestAtBudgetkontogruppelisteGetAsyncHenterBudgetkontogrupper()
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

            var result = await finansstyringRepositoryLocale.BudgetkontogruppelisteGetAsync();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count(), Is.EqualTo(25));

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
                var random = new Random(fixture.Create<int>());
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
                    for (var j = 0; j < 30; j++)
                    {
                        var budgetkontoModel = CreateBudgetkonto(fixture, i + 1, string.Format("BUDGETKONTO{0:00}", j + 1));
                        budgetkontoModel.StoreInDocument(localeDataDocument);
                    }
                    for (var j = 0; j < 25; j++)
                    {
                        var adressekontoModel = CreateAdressekontoModel(fixture, i + 1, 100 + j + 1, 1);
                        adressekontoModel.StoreInDocument(localeDataDocument);
                    }
                    for (var j = 0; j < 15; j++)
                    {
                        var adressekontoModel = CreateAdressekontoModel(fixture, i + 1, 200 + j + 1, -1);
                        adressekontoModel.StoreInDocument(localeDataDocument);
                    }
                    for (var j = 0; j < 150; j++)
                    {
                        var bogføringslinje = CreateBogføringslinjeModel(fixture, i + 1, j + 1, random.Next(1, 365)*-1, string.Format("KONTO{0:00}", random.Next(1, 15)), string.Format("BUDGETKONTO{0:00}", random.Next(1, 30)), 100 + random.Next(1, 25));
                        bogføringslinje.StoreInDocument(localeDataDocument, false);
                    }
                }
                for (var i = 0; i < 15; i++)
                {
                    var kontogruppeModel = CreateKontogruppe(fixture, i + 1);
                    kontogruppeModel.StoreInDocument(localeDataDocument);
                }
                for (var i = 0; i < 25; i++)
                {
                    var budgetkontogruppeModel = CreateBudgetkontogruppe(fixture, i + 1);
                    budgetkontogruppeModel.StoreInDocument(localeDataDocument);
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
        /// Danner testdata til en budgetkonto.
        /// </summary>
        /// <param name="fixture">Fixture, der kan generere random data.</param>
        /// <param name="regnskabsnummer">Unik identifikation af regnskabet, som budgetkontoen skal være tilknyttet.</param>
        /// <param name="kontonummer">Unik identifikation af kontoen.</param>
        /// <returns>Testdata til en budgetkonto.</returns>
        private static IBudgetkontoModel CreateBudgetkonto(ISpecimenBuilder fixture, int regnskabsnummer, string kontonummer)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }
            if (string.IsNullOrEmpty(kontonummer))
            {
                throw new ArgumentNullException("kontonummer");
            }
            var budgetkontogruppe = (DateTime.Now.Millisecond%25) + 1;
            var budgetkontoModelMock = MockRepository.GenerateMock<IBudgetkontoModel>();
            budgetkontoModelMock.Expect(m => m.Regnskabsnummer)
                .Return(regnskabsnummer)
                .Repeat.Any();
            budgetkontoModelMock.Expect(m => m.Kontonummer)
                .Return(kontonummer)
                .Repeat.Any();
            budgetkontoModelMock.Expect(m => m.Kontonavn)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            budgetkontoModelMock.Expect(m => m.Beskrivelse)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            budgetkontoModelMock.Expect(m => m.Notat)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            budgetkontoModelMock.Expect(m => m.Kontogruppe)
                .Return(budgetkontogruppe)
                .Repeat.Any();
            budgetkontoModelMock.Expect(m => m.StatusDato)
                .Return(DateTime.Now.Date)
                .Repeat.Any();
            budgetkontoModelMock.Expect(m => m.Indtægter)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            budgetkontoModelMock.Expect(m => m.Udgifter)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            budgetkontoModelMock.Expect(m => m.Bogført)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            return budgetkontoModelMock;
        }

        /// <summary>
        /// Danner testdata til en adressekonto.
        /// </summary>
        /// <param name="fixture">Fixture, der kan generere random data.</param>
        /// <param name="regnskabsnummer">Unik identifikation af regnskabet, som adressekontoen skal være tilknyttet.</param>
        /// <param name="nummer">Unik identifikation af adressekontoen.</param>
        /// <param name="saldoOffset">Offset, som saldoen skal ganges med.</param>
        /// <returns>Testdata til en adressekonto.</returns>
        private static IAdressekontoModel CreateAdressekontoModel(ISpecimenBuilder fixture, int regnskabsnummer, int nummer, int saldoOffset)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }
            var adressekontoModelMock = MockRepository.GenerateMock<IAdressekontoModel>();
            adressekontoModelMock.Expect(m => m.Regnskabsnummer)
                .Return(regnskabsnummer)
                .Repeat.Any();
            adressekontoModelMock.Expect(m => m.Nummer)
                .Return(nummer)
                .Repeat.Any();
            adressekontoModelMock.Expect(m => m.Navn)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            adressekontoModelMock.Expect(m => m.PrimærTelefon)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            adressekontoModelMock.Expect(m => m.SekundærTelefon)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            adressekontoModelMock.Expect(m => m.StatusDato)
                .Return(DateTime.Now.Date)
                .Repeat.Any();
            adressekontoModelMock.Expect(m => m.Saldo)
                .Return(Math.Abs(fixture.Create<decimal>())*saldoOffset)
                .Repeat.Any();
            return adressekontoModelMock;
        }

        /// <summary>
        /// Danner testdata til en bogføringslinje.
        /// </summary>
        /// <param name="fixture">Fixture, der kan generere random data.</param>
        /// <param name="regnskabsnummer">Unik identifikation af regnskabet, som bogføringslinjen skal være tilknyttet.</param>
        /// <param name="løbenummer">Løbenummeret på bogføringslinjen.</param>
        /// <param name="datoOffset">Offset, som datoen skal adderes med.</param>
        /// <param name="kontonummer">Kontonummer, som bogføringslinjen skal være tilknyttet.</param>
        /// <param name="budgetkontonummer">Budgetkontonummer, som bogføringslinjen skal være tilknyttet.</param>
        /// <param name="adressekonto">Adressekonto, som bogføringslinjen skal være tilknyttet.</param>
        /// <returns>Testdata til en bogføringslinje.</returns>
        private static IBogføringslinjeModel CreateBogføringslinjeModel(ISpecimenBuilder fixture, int regnskabsnummer, int løbenummer, int datoOffset, string kontonummer, string budgetkontonummer, int adressekonto)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }
            if (string.IsNullOrEmpty(kontonummer))
            {
                throw new ArgumentNullException("kontonummer");
            }
            if (string.IsNullOrEmpty(budgetkontonummer))
            {
                throw new ArgumentNullException("budgetkontonummer");
            }
            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Regnskabsnummer)
                .Return(regnskabsnummer)
                .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Løbenummer)
                .Return(løbenummer)
                .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                .Return(DateTime.Today.AddDays(datoOffset))
                .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Bilag)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                .Return(kontonummer)
                .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Tekst)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                .Return(budgetkontonummer)
                .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Debit)
                .Return(Math.Abs(fixture.Create<decimal>()))
                .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kredit)
                .Return(Math.Abs(fixture.Create<decimal>()))
                .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                .Return(adressekonto)
                .Repeat.Any();
            return bogføringslinjeModelMock;
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
        /// Danner testdata til en kontogruppe for budgetkonti.
        /// </summary>
        /// <param name="fixture">Fixture, der kan generere random data.</param>
        /// <param name="nummer">Unik identifikation af kontogruppen.</param>
        /// <returns>Testdata til en kontogruppe.</returns>
        private static IBudgetkontogruppeModel CreateBudgetkontogruppe(ISpecimenBuilder fixture, int nummer)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }
            var budgetkontogruppeModelMock = MockRepository.GenerateMock<IBudgetkontogruppeModel>();
            budgetkontogruppeModelMock.Expect(m => m.Nummer)
                .Return(nummer)
                .Repeat.Any();
            budgetkontogruppeModelMock.Expect(m => m.Tekst)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            return budgetkontogruppeModelMock;
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
