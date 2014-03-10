using System;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Finansstyring.Tests
{
    /// <summary>
    /// Tester repository, der supporterer finansstyring.
    /// </summary>
    [TestFixture]
    public class FinansstyringRepositoryTests
    {
        #region Private constants

        private const string FinansstyringServiceTestUri = "http://mother/osintranet/finansstyringservice.svc/mobile";

        #endregion

        /// <summary>
        /// Tester, at konstruktøren initierer repository, der supporterer finansstyring.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererFinansstyringRepository()
        {
            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();

            var finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock);
            Assert.That(finansstyringRepository, Is.Not.Null);
            Assert.That(finansstyringRepository.Konfiguration, Is.Not.Null);
            Assert.That(finansstyringRepository.Konfiguration, Is.EqualTo(finansstyringKonfigurationRepositoryMock));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis konfigurationrepositoryet, der supporterer finansstyring, er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFinansstyringKonfigurationRepositoryErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new FinansstyringRepository(null));
        }

        /// <summary>
        /// Tester, at RegnskabslisteGetAsync henter regnskaber.
        /// </summary>
        [Test]
        public async void TestAtRegnskabslisteGetAsyncHenterRegnskaber()
        {
            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.FinansstyringServiceUri)
                                                    .Return(new Uri(FinansstyringServiceTestUri))
                                                    .Repeat.Any();

            var finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock);
            Assert.That(finansstyringRepository, Is.Not.Null);

            var regnskaber = await finansstyringRepository.RegnskabslisteGetAsync();
            Assert.That(regnskaber, Is.Not.Null);
            Assert.That(regnskaber, Is.Not.Empty);
            Assert.That(regnskaber.Count(), Is.GreaterThan(0));

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.FinansstyringServiceUri);
        }

        /// <summary>
        /// Tester, at KontoplanGetAsync henter kontoplanen til et regnskab.
        /// </summary>
        [Test]
        public async void TestAtKontoplanGetAsyncHenterKontoplan()
        {
            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.FinansstyringServiceUri)
                                                    .Return(new Uri(FinansstyringServiceTestUri))
                                                    .Repeat.Any();

            var finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock);
            Assert.That(finansstyringRepository, Is.Not.Null);

            var kontoplan = await finansstyringRepository.KontoplanGetAsync(1, DateTime.Now);
            Assert.That(kontoplan, Is.Not.Null);
            Assert.That(kontoplan, Is.Not.Empty);

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.FinansstyringServiceUri);
        }

        /// <summary>
        /// Tester, at KontoGet kaster en ArgumentNullException ved illegale kontonumre.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestAtKontoGetKasterArgumentNullExceptionVedIllegalKontonummer(string illegalKontonummer)
        {
            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();

            var finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock);
            Assert.That(finansstyringRepository, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(async () => await finansstyringRepository.KontoGetAsync(1, illegalKontonummer, DateTime.Now));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("kontonummer"));
            Assert.That(exception.InnerException, Is.Null);

            finansstyringKonfigurationRepositoryMock.AssertWasNotCalled(m => m.FinansstyringServiceUri);
        }

        /// <summary>
        /// Tester, at BogføringslinjerGetAsync henter et givent antal bogføringslinjer til et givent regnskab.
        /// </summary>
        [Test]
        public async void TestAtBogføringslinjerGetAsyncHenterBogføringslinjer()
        {
            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.FinansstyringServiceUri)
                                                    .Return(new Uri(FinansstyringServiceTestUri))
                                                    .Repeat.Any();

            var finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock);
            Assert.That(finansstyringRepository, Is.Not.Null);

            var bogføringslinjer = await finansstyringRepository.BogføringslinjerGetAsync(1, DateTime.Now, 50);
            Assert.That(bogføringslinjer, Is.Not.Null);
            Assert.That(bogføringslinjer, Is.Not.Empty);
            Assert.That(bogføringslinjer.Count(), Is.EqualTo(50));

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.FinansstyringServiceUri);
        }

        /// <summary>
        /// Tester, at DebitorlisteGetAsync henter listen af debitorer til et givent regnskab.
        /// </summary>
        [Test]
        public async void TestAtDebitorlisteGetAsyncHenterDebitorliste()
        {
            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.FinansstyringServiceUri)
                                                    .Return(new Uri(FinansstyringServiceTestUri))
                                                    .Repeat.Any();

            var finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock);
            Assert.That(finansstyringRepository, Is.Not.Null);

            var debitorer = await finansstyringRepository.DebitorlisteGetAsync(1, DateTime.Now);
            Assert.That(debitorer, Is.Not.Null);
            Assert.That(debitorer.Count(), Is.GreaterThanOrEqualTo(0));

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.FinansstyringServiceUri);
        }

        /// <summary>
        /// Tester, at KreditorlisteGetAsync henter listen af kreditorer til et givent regnskab.
        /// </summary>
        [Test]
        public async void TestAtKreditorlisteGetAsyncHenterKreditorliste()
        {
            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.FinansstyringServiceUri)
                                                    .Return(new Uri(FinansstyringServiceTestUri))
                                                    .Repeat.Any();

            var finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock);
            Assert.That(finansstyringRepository, Is.Not.Null);

            var kreditorer = await finansstyringRepository.KreditorlisteGetAsync(1, DateTime.Now);
            Assert.That(kreditorer, Is.Not.Null);
            Assert.That(kreditorer.Count(), Is.GreaterThanOrEqualTo(0));

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.FinansstyringServiceUri);
        }

        /// <summary>
        /// Tester, at AdressekontoGetAsync henter en adressekonto.
        /// </summary>
        [Test]
        public async void TestAtAdressekontoGetAsyncHenterAdressekonto()
        {
            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.FinansstyringServiceUri)
                                                    .Return(new Uri(FinansstyringServiceTestUri))
                                                    .Repeat.Any();

            var finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock);
            Assert.That(finansstyringRepository, Is.Not.Null);

            var adressekonto = await finansstyringRepository.AdressekontoGetAsync(1, 1, DateTime.Now);
            Assert.That(adressekonto, Is.Not.Null);

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.FinansstyringServiceUri);
        }
    }
}
