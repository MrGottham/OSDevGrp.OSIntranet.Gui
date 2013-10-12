using System;
using System.Linq;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Finansstyring.Tests
{
    /// <summary>
    /// Tester repository, der supporterer finansstyring.
    /// </summary>
    [TestFixture]
    public class FinansstyringRepositoryTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer repository, der supporterer finansstyring.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererFinansstyringRepository()
        {
            var finansstyringRepository = new FinansstyringRepository();
            Assert.That(finansstyringRepository, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at RegnskabslisteGetAsync henter regnskaber.
        /// </summary>
        [Test]
        public async void TestAtRegnskabslisteGetAsyncHenterRegnskaber()
        {
            var finansstyringRepository = new FinansstyringRepository();
            Assert.That(finansstyringRepository, Is.Not.Null);

            var regnskaber = await finansstyringRepository.RegnskabslisteGetAsync();
            Assert.That(regnskaber, Is.Not.Null);
            Assert.That(regnskaber, Is.Not.Empty);
            Assert.That(regnskaber.Count(), Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at BogføringslinjerGetAsync henter et givent antal bogføringslinjer til et givent regnskab.
        /// </summary>
        [Test]
        public async void TestAtBogføringslinjerGetAsyncHenterBogføringslinjer()
        {
            var finansstyringRepository = new FinansstyringRepository();
            Assert.That(finansstyringRepository, Is.Not.Null);

            var bogføringslinjer = await finansstyringRepository.BogføringslinjerGetAsync(1, DateTime.Now, 50);
            Assert.That(bogføringslinjer, Is.Not.Null);
            Assert.That(bogføringslinjer, Is.Not.Empty);
            Assert.That(bogføringslinjer.Count(), Is.EqualTo(50));
        }
    }
}
