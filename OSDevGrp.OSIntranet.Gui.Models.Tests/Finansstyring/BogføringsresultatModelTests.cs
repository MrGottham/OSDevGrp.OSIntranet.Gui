using System;
using System.Linq;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Models.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;

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
            Fixture fixture = new Fixture();
            fixture.Customize<IBogføringslinjeModel>(e => e.FromFactory(() => new Mock<IBogføringslinjeModel>().Object));
            fixture.Customize<IBogføringsadvarselModel>(e => e.FromFactory(() => new Mock<IBogføringsadvarselModel>().Object));

            IBogføringslinjeModel bogføringslinjeModelMock = fixture.Create<IBogføringslinjeModel>();
            IBogføringsadvarselModel[] bogføringsadvarselModelMockCollection = fixture.CreateMany<IBogføringsadvarselModel>(7).ToArray();
            IBogføringsresultatModel bogføringsresultatModel = new BogføringsresultatModel(bogføringslinjeModelMock, bogføringsadvarselModelMockCollection);
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
            Fixture fixture = new Fixture();
            fixture.Customize<IBogføringsadvarselModel>(e => e.FromFactory(() => new Mock<IBogføringsadvarselModel>().Object));

            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new BogføringsresultatModel(null, fixture.CreateMany<IBogføringsadvarselModel>(7).ToList()));
            // ReSharper restore ObjectCreationAsStatement
            Assert.That(exception, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("bogføringslinje"));
            Assert.That(exception.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis bogføringsadvarsler er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisBogføringsadvarselModelCollectionErNull()
        {
            Fixture fixture = new Fixture();
            fixture.Customize<IBogføringslinjeModel>(e => e.FromFactory(() => new Mock<IBogføringslinjeModel>().Object));

            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new BogføringsresultatModel(fixture.Create<IBogføringslinjeModel>(), null));
            // ReSharper restore ObjectCreationAsStatement
            Assert.That(exception, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("bogføringsadvarsler"));
            Assert.That(exception.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
        }
    }
}