using System;
using System.ComponentModel;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Finansstyring
{
    /// <summary>
    /// Tester ViewModel for et regnskab.
    /// </summary>
    [TestFixture]
    public class RegnskabViewModelTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en ViewModel for et regnskab.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererRegnskabViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IRegnskabModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(fixture.Create<int>())
                        .Repeat.Any();
                    mock.Expect(m => m.Navn)
                        .Return(fixture.Create<string>())
                        .Repeat.Any();
                    return mock;
                }));

            var regnskabModelMock = fixture.Create<IRegnskabModel>();
            var regnskabViewModel = new RegnskabViewModel(regnskabModelMock);
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Nummer, Is.EqualTo(regnskabModelMock.Nummer));
            Assert.That(regnskabViewModel.Navn, Is.Not.Null);
            Assert.That(regnskabViewModel.Navn, Is.Not.Empty);
            Assert.That(regnskabViewModel.Navn, Is.EqualTo(regnskabModelMock.Navn));
            Assert.That(regnskabViewModel.DisplayName, Is.Not.Null);
            Assert.That(regnskabViewModel.DisplayName, Is.Not.Empty);
            Assert.That(regnskabViewModel.DisplayName, Is.EqualTo(regnskabModelMock.Navn));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis modellen for regnskabet er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisRegnskabModelErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new RegnskabViewModel(null));
        }

        /// <summary>
        /// Tester, at sætteren på Navn opdaterer Navn på modellen for regnskabet.
        /// </summary>
        [Test]
        public void TestAtNavnSetterOpdatererNavnOnRegnskabModel()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));

            var regnskabModelMock = fixture.Create<IRegnskabModel>();
            var regnskabViewModel = new RegnskabViewModel(regnskabModelMock);
            Assert.That(regnskabViewModel, Is.Not.Null);

            var newValue = fixture.Create<string>();
            regnskabViewModel.Navn = newValue;

            regnskabModelMock.AssertWasCalled(m => m.Navn = Arg<string>.Is.Equal(newValue));
        }

        /// <summary>
        /// Tester, at PropertyChanged rejses, hvis navnet på modellen for regnskabet opdateres.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedRejsesHvisNavnOnRegnskabModelOpdateres()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));

            var regnskabModelMock = fixture.Create<IRegnskabModel>();
            var regnskabViewModel = new RegnskabViewModel(regnskabModelMock);
            Assert.That(regnskabViewModel, Is.Not.Null);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    eventCalled = string.Compare(e.PropertyName, "Navn", StringComparison.Ordinal) == 0;
                };

            Assert.That(eventCalled, Is.False);
            regnskabModelMock.Raise(m => m.PropertyChanged += null, regnskabModelMock, new PropertyChangedEventArgs("Navn"));
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnRegnskabModelEventHandler kaster en ArgumentNullException, hvis objektet, der rejser eventet er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnRegnskabModelEventHandlerKasterArgumentNullExceptionHvisSenderErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));

            var regnskabModelMock = fixture.Create<IRegnskabModel>();
            var regnskabViewModel = new RegnskabViewModel(regnskabModelMock);
            Assert.That(regnskabViewModel, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => regnskabModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
        }

        /// <summary>
        /// Tester, at PropertyChangedOnRegnskabModelEventHandler kaster en ArgumentNullException, hvis argumenter til eventet er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnRegnskabModelEventHandlerKasterArgumentNullExceptionHvisEventArgsErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));

            var regnskabModelMock = fixture.Create<IRegnskabModel>();
            var regnskabViewModel = new RegnskabViewModel(regnskabModelMock);
            Assert.That(regnskabViewModel, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => regnskabModelMock.Raise(m => m.PropertyChanged += null, fixture.Create<object>(), null));
        }
    }
}
