using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Finansstyring
{
    /// <summary>
    /// Tester ViewModel for en bogføringslinje.
    /// </summary>
    [TestFixture]
    public class BogføringslinjeViewModelTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en ViewModel for en bogføringslinje.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererBogføringslinjeViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IBogføringslinjeModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IBogføringslinjeModel>();
                    mock.Expect(m => m.Løbenummer)
                        .Return(fixture.Create<int>())
                        .Repeat.Any();
                    mock.Expect(m => m.Dato)
                        .Return(fixture.Create<DateTime>())
                        .Repeat.Any();
                    mock.Expect(m => m.Kontonummer)
                        .Return(fixture.Create<string>())
                        .Repeat.Any();
                    mock.Expect(m => m.Tekst)
                        .Return(fixture.Create<string>())
                        .Repeat.Any();
                    mock.Expect(m => m.Budgetkontonummer)
                        .Return(fixture.Create<string>())
                        .Repeat.Any();
                    mock.Expect(m => m.Debit)
                        .Return(fixture.Create<decimal>())
                        .Repeat.Any();
                    mock.Expect(m => m.Kredit)
                        .Return(fixture.Create<decimal>())
                        .Repeat.Any();
                    mock.Expect(m => m.Bogført)
                        .Return(fixture.Create<decimal>())
                        .Repeat.Any();
                    mock.Expect(m => m.Adressekonto)
                        .Return(fixture.Create<int>())
                        .Repeat.Any();
                    return mock;
                }));

            var regnskabViewModelMock = fixture.Create<IRegnskabViewModel>();
            var bogføringslinjeModelMock = fixture.Create<IBogføringslinjeModel>();
            var bogføringslinjeViewModel = new BogføringslinjeViewModel(regnskabViewModelMock, bogføringslinjeModelMock);
            Assert.That(bogføringslinjeViewModel, Is.Not.Null);
            Assert.That(bogføringslinjeViewModel.Regnskab, Is.Not.Null);
            Assert.That(bogføringslinjeViewModel.Regnskab, Is.EqualTo(regnskabViewModelMock));
            Assert.That(bogføringslinjeViewModel.Løbenummer, Is.EqualTo(bogføringslinjeModelMock.Løbenummer));
            Assert.That(bogføringslinjeViewModel.Dato, Is.EqualTo(bogføringslinjeModelMock.Dato));
            Assert.That(bogføringslinjeViewModel.Kontonummer, Is.Not.Null);
            Assert.That(bogføringslinjeViewModel.Kontonummer, Is.Not.Empty);
            Assert.That(bogføringslinjeViewModel.Kontonummer, Is.EqualTo(bogføringslinjeModelMock.Kontonummer));
            Assert.That(bogføringslinjeViewModel.Tekst, Is.Not.Null);
            Assert.That(bogføringslinjeViewModel.Tekst, Is.Not.Empty);
            Assert.That(bogføringslinjeViewModel.Tekst, Is.EqualTo(bogføringslinjeModelMock.Tekst));
            Assert.That(bogføringslinjeViewModel.Budgetkontonummer, Is.Not.Null);
            Assert.That(bogføringslinjeViewModel.Budgetkontonummer, Is.Not.Empty);
            Assert.That(bogføringslinjeViewModel.Budgetkontonummer, Is.EqualTo(bogføringslinjeModelMock.Budgetkontonummer));
            Assert.That(bogføringslinjeViewModel.Debit, Is.EqualTo(bogføringslinjeModelMock.Debit));
            Assert.That(bogføringslinjeViewModel.Kredit, Is.EqualTo(bogføringslinjeModelMock.Kredit));
            Assert.That(bogføringslinjeViewModel.Bogført, Is.EqualTo(bogføringslinjeModelMock.Bogført));
            Assert.That(bogføringslinjeViewModel.Adressekonto, Is.EqualTo(bogføringslinjeModelMock.Adressekonto));
            Assert.That(bogføringslinjeViewModel.Image, Is.Not.Null);
            Assert.That(bogføringslinjeViewModel.Image, Is.Not.Empty);
            Assert.That(bogføringslinjeViewModel.Image, Is.EqualTo(TestHelper.GetEmbeddedResource((new Resource()).GetType().Assembly, "Images.Bogføringslinje.png")));

            bogføringslinjeModelMock.AssertWasCalled(m => m.Løbenummer);
            bogføringslinjeModelMock.AssertWasCalled(m => m.Dato);
            bogføringslinjeModelMock.AssertWasCalled(m => m.Kontonummer);
            bogføringslinjeModelMock.AssertWasCalled(m => m.Tekst);
            bogføringslinjeModelMock.AssertWasCalled(m => m.Budgetkontonummer);
            bogføringslinjeModelMock.AssertWasCalled(m => m.Debit);
            bogføringslinjeModelMock.AssertWasCalled(m => m.Kredit);
            bogføringslinjeModelMock.AssertWasCalled(m => m.Bogført);
            bogføringslinjeModelMock.AssertWasCalled(m => m.Adressekonto);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis ViewModel for regnskabet, som bogføringslinjen skal tilknyttes, er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisRegnskabViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IBogføringslinjeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringslinjeModel>()));

            Assert.Throws<ArgumentNullException>(() => new BogføringslinjeViewModel(null, fixture.Create<IBogføringslinjeModel>()));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis modellen for bogføringslinjen er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisBogføringslinjeModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));

            Assert.Throws<ArgumentNullException>(() => new BogføringslinjeViewModel(fixture.Create<IRegnskabViewModel>(), null));
        }
    }
}
