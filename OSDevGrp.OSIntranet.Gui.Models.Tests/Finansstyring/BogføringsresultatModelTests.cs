using System;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Models.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Gui.Models.Tests.Finansstyring
{
    /// <summary>
    /// Tester modellen, der indeholder resultatet af en bogføring.
    /// </summary>
    [TestFixture]
    public class BogføringsresultatModelTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en model, , der indeholder resultatet af en bogføring.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererBogføringsresultatModel()
        {
            var fixture = new Fixture();
            fixture.Customize<IBogføringslinjeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringslinjeModel>()));
            fixture.Customize<IBogføringsadvarselModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringsadvarselModel>()));

            var bogføringslinjeModelMock = fixture.Create<IBogføringslinjeModel>();
            var bogføringsadvarselModelMockCollection = fixture.CreateMany<IBogføringsadvarselModel>(7).ToList();
            var bogføringsresultatModel = new BogføringsresultatModel(bogføringslinjeModelMock, bogføringsadvarselModelMockCollection);
            Assert.That(bogføringsresultatModel, Is.Not.Null);
            Assert.That(bogføringsresultatModel.Bogføringslinje, Is.Not.Null);
            Assert.That(bogføringsresultatModel.Bogføringslinje, Is.EqualTo(bogføringslinjeModelMock));
            Assert.That(bogføringsresultatModel.Bogføringsadvarsler, Is.Not.Null);
            Assert.That(bogføringsresultatModel.Bogføringsadvarsler, Is.Not.Empty);
            Assert.That(bogføringsresultatModel.Bogføringsadvarsler, Is.EqualTo(bogføringsadvarselModelMockCollection));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis den bogførte linje er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisBogføringslinjeModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IBogføringsadvarselModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringsadvarselModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new BogføringsresultatModel(null, fixture.CreateMany<IBogføringsadvarselModel>(7).ToList()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("bogføringslinje"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis bogføringsadvarsler er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisBogføringsadvarselModelCollectionErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IBogføringslinjeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringslinjeModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new BogføringsresultatModel(fixture.Create<IBogføringslinjeModel>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("bogføringsadvarsler"));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
