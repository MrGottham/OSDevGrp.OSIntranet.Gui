using System;
using System.ComponentModel;
using System.Linq;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using Text = OSDevGrp.OSIntranet.Gui.Resources.Text;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Core
{
    /// <summary>
    /// Tester ViewModel for en nyhed.
    /// </summary>
    [TestFixture]
    public class NyhedViewModelTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en ViewModel for en nyhed.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererNyhedViewModel()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            Nyhedsaktualitet nyhedsaktualitet = fixture.Create<Nyhedsaktualitet>();
            DateTime nyhedsudgivelsestidspunkt = DateTime.Now.AddDays(random.Next(0, 7) * -1).AddMinutes(random.Next(0, 120) * -1);
            string nyhedsinformation = fixture.Create<string>();
            Mock<INyhedModel> nyhedModelMock = fixture.BuildNyhedModelMock(nyhedsaktualitet, nyhedsudgivelsestidspunkt, nyhedsinformation);

            byte[] image = Resource.GetEmbeddedResource("Images.Bogføringslinje.png");

            INyhedViewModel nyhedViewModel = new NyhedViewModel(nyhedModelMock.Object, image);
            Assert.That(nyhedViewModel, Is.Not.Null);
            Assert.That(nyhedViewModel.Nyhedsaktualitet, Is.EqualTo(nyhedsaktualitet));
            Assert.That(nyhedViewModel.Nyhedsudgivelsestidspunkt, Is.EqualTo(nyhedsudgivelsestidspunkt));
            Assert.That(nyhedViewModel.DisplayName, Is.Not.Null);
            Assert.That(nyhedViewModel.DisplayName, Is.Not.Empty);
            Assert.That(nyhedViewModel.DisplayName, Is.EqualTo(Resource.GetText(Text.News)));
            Assert.That(nyhedViewModel.Image, Is.Not.Null);
            Assert.That(nyhedViewModel.Image, Is.Not.Empty);
            Assert.That(nyhedViewModel.Image, Is.EqualTo(image));
            Assert.That(nyhedViewModel.Nyhedsinformation, Is.Not.Null);
            Assert.That(nyhedViewModel.Nyhedsinformation, Is.Not.Empty);
            Assert.That(nyhedViewModel.Nyhedsinformation, Is.EqualTo(nyhedsinformation));

            nyhedModelMock.Verify(m => m.Nyhedsaktualitet, Times.Once);
            nyhedModelMock.Verify(m => m.Nyhedsudgivelsestidspunkt, Times.Once);
            nyhedModelMock.Verify(m => m.Nyhedsinformation, Times.Once);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis modellen for nyheden er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisNyhedModelErNull()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            // ReSharper disable ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new NyhedViewModel(null, fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray()));
            // ReSharper restore ObjectCreationAsStatement
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis billedet, der illustrerer nyheden, er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisImageErNull()
        {
            Fixture fixture = new Fixture();

            // ReSharper disable ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new NyhedViewModel(fixture.BuildNyhedModel(), null));
            // ReSharper restore ObjectCreationAsStatement
        }

        /// <summary>
        /// Tester, at PropertyChangedOnNyhedModelEventHandler rejser PropertyChanged, når modellen for nyheden opdateres.
        /// </summary>
        [Test]
        [TestCase("Nyhedsaktualitet", "Nyhedsaktualitet")]
        [TestCase("Nyhedsudgivelsestidspunkt", "Nyhedsudgivelsestidspunkt")]
        [TestCase("Nyhedsinformation", "Nyhedsinformation")]
        public void TestAtPropertyChangedOnNyhedModelEventHandlerRejserPropertyChangedOnNyhedModelUpdate(string propertyNameToRaise, string expectPropertyName)
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            Mock<INyhedModel> nyhedModelMock = fixture.BuildNyhedModelMock();

            INyhedViewModel nyhedViewModel = new NyhedViewModel(nyhedModelMock.Object, fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray());
            Assert.That(nyhedViewModel, Is.Not.Null);

            bool eventCalled = false;
            nyhedViewModel.PropertyChanged += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Empty);
                if (string.CompareOrdinal(e.PropertyName, expectPropertyName) == 0)
                {
                    eventCalled = true;
                }
            };

            Assert.That(eventCalled, Is.False);
            nyhedModelMock.Raise(m => m.PropertyChanged += null, nyhedModelMock, new PropertyChangedEventArgs(propertyNameToRaise));
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnNyhedModelEventHandler kaster en ArgumentNullException, hvis objektet, der rejser eventet er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnNyhedModelEventHandlerKasterArgumentNullExceptionHvisSenderErNull()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            Mock<INyhedModel> nyhedModelMock = fixture.BuildNyhedModelMock();

            INyhedViewModel nyhedViewModel = new NyhedViewModel(nyhedModelMock.Object, fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray());
            Assert.That(nyhedViewModel, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => nyhedModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
        }

        /// <summary>
        /// Tester, at PropertyChangedOnNyhedModelEventHandler kaster en ArgumentNullException, hvis argumenter til eventet er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnNyhedModelEventHandlerKasterArgumentNullExceptionHvisEventArgsErNull()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            Mock<INyhedModel> nyhedModelMock = fixture.BuildNyhedModelMock();

            INyhedViewModel nyhedViewModel = new NyhedViewModel(nyhedModelMock.Object, fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray());
            Assert.That(nyhedViewModel, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => nyhedModelMock.Raise(m => m.PropertyChanged += null, fixture.Create<object>(), null));
        }
    }
}