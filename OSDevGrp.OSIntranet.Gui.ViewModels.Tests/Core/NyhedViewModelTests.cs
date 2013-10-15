using System;
using System.ComponentModel;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core;
using Ploeh.AutoFixture;
using Rhino.Mocks;
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
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<byte[]>(e => e.FromFactory(() => TestHelper.GetEmbeddedResource((new Resource()).GetType().Assembly, "Images.Bogføringslinje.png")));
            fixture.Customize<INyhedModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<INyhedModel>();
                    mock.Expect(m => m.Nyhedsaktualitet)
                        .Return(Nyhedsaktualitet.Medium)
                        .Repeat.Any();
                    mock.Expect(m => m.Nyhedsudgivelsestidspunkt)
                        .Return(fixture.Create<DateTime>())
                        .Repeat.Any();
                    mock.Expect(m => m.Nyhedsinformation)
                        .Return(fixture.Create<string>())
                        .Repeat.Any();
                    return mock;
                }));

            var nyhedModelMock = fixture.Create<INyhedModel>();
            var image = fixture.Create<byte[]>();
            var nyhedViewModel = new NyhedViewModel(nyhedModelMock, image);
            Assert.That(nyhedViewModel, Is.Not.Null);
            Assert.That(nyhedViewModel.Nyhedsaktualitet, Is.EqualTo(nyhedModelMock.Nyhedsaktualitet));
            Assert.That(nyhedViewModel.Nyhedsudgivelsestidspunkt, Is.EqualTo(nyhedModelMock.Nyhedsudgivelsestidspunkt));
            Assert.That(nyhedViewModel.DisplayName, Is.Not.Null);
            Assert.That(nyhedViewModel.DisplayName, Is.Not.Empty);
            Assert.That(nyhedViewModel.DisplayName, Is.EqualTo(Resource.GetText(Text.News)));
            Assert.That(nyhedViewModel.Image, Is.Not.Null);
            Assert.That(nyhedViewModel.Image, Is.Not.Empty);
            Assert.That(nyhedViewModel.Image, Is.EqualTo(image));
            Assert.That(nyhedViewModel.Nyhedsinformation, Is.Not.Null);
            Assert.That(nyhedViewModel.Nyhedsinformation, Is.Not.Empty);
            Assert.That(nyhedViewModel.Nyhedsinformation, Is.EqualTo(nyhedModelMock.Nyhedsinformation));

            nyhedModelMock.AssertWasCalled(m => m.Nyhedsaktualitet);
            nyhedModelMock.AssertWasCalled(m => m.Nyhedsudgivelsestidspunkt);
            nyhedModelMock.AssertWasCalled(m => m.Nyhedsinformation);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis modellen for nyheden er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisNyhedModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<byte[]>(e => e.FromFactory(() => TestHelper.GetEmbeddedResource((new Resource()).GetType().Assembly, "Images.Bogføringslinje.png")));

            Assert.Throws<ArgumentNullException>(() => new NyhedViewModel(null, fixture.Create<byte[]>()));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis billedet, der illustrerer nyheden, er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisImageErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<INyhedModel>(e => e.FromFactory(() => MockRepository.GenerateMock<INyhedModel>()));

            Assert.Throws<ArgumentNullException>(() => new NyhedViewModel(fixture.Create<INyhedModel>(), null));
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
            var fixture = new Fixture();
            fixture.Customize<byte[]>(e => e.FromFactory(() => TestHelper.GetEmbeddedResource((new Resource()).GetType().Assembly, "Images.Bogføringslinje.png")));
            fixture.Customize<INyhedModel>(e => e.FromFactory(() => MockRepository.GenerateMock<INyhedModel>()));

            var nyhedModelMock = fixture.Create<INyhedModel>();
            var nyhedViewModel = new NyhedViewModel(nyhedModelMock, fixture.Create<byte[]>());
            Assert.That(nyhedViewModel, Is.Not.Null);

            var eventCalled = false;
            nyhedModelMock.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, expectPropertyName, StringComparison.Ordinal) == 0)
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
            var fixture = new Fixture();
            fixture.Customize<byte[]>(e => e.FromFactory(() => TestHelper.GetEmbeddedResource((new Resource()).GetType().Assembly, "Images.Bogføringslinje.png")));
            fixture.Customize<INyhedModel>(e => e.FromFactory(() => MockRepository.GenerateMock<INyhedModel>()));

            var nyhedModelMock = fixture.Create<INyhedModel>();
            var nyhedViewModel = new NyhedViewModel(nyhedModelMock, fixture.Create<byte[]>());
            Assert.That(nyhedViewModel, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => nyhedModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
        }

        /// <summary>
        /// Tester, at PropertyChangedOnNyhedModelEventHandler kaster en ArgumentNullException, hvis argumenter til eventet er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnNyhedModelEventHandlerKasterArgumentNullExceptionHvisEventArgsErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<byte[]>(e => e.FromFactory(() => TestHelper.GetEmbeddedResource((new Resource()).GetType().Assembly, "Images.Bogføringslinje.png")));
            fixture.Customize<INyhedModel>(e => e.FromFactory(() => MockRepository.GenerateMock<INyhedModel>()));

            var nyhedModelMock = fixture.Create<INyhedModel>();
            var nyhedViewModel = new NyhedViewModel(nyhedModelMock, fixture.Create<byte[]>());
            Assert.That(nyhedViewModel, Is.Not.Null);
 
            Assert.Throws<ArgumentNullException>(() => nyhedModelMock.Raise(m => m.PropertyChanged += null, fixture.Create<object>(), null));
        }
    }
}
