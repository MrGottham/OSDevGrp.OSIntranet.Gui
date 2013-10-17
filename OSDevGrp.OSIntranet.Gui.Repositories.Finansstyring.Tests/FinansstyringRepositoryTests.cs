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
    }
}
