using System;
using System.ComponentModel;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
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
                    mock.Expect(m => m.Bilag)
                        .Return(fixture.Create<string>())
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
                    mock.Expect(m => m.Nyhedsinformation)
                        .Return(fixture.Create<string>())
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
            Assert.That(bogføringslinjeViewModel.Bilag, Is.Not.Null);
            Assert.That(bogføringslinjeViewModel.Bilag, Is.Not.Empty);
            Assert.That(bogføringslinjeViewModel.Bilag, Is.EqualTo(bogføringslinjeModelMock.Bilag));
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
            Assert.That(bogføringslinjeViewModel.DisplayName, Is.Not.Null);
            Assert.That(bogføringslinjeViewModel.DisplayName, Is.Not.Empty);
            Assert.That(bogføringslinjeViewModel.DisplayName, Is.EqualTo(bogføringslinjeModelMock.Nyhedsinformation));

            bogføringslinjeModelMock.AssertWasCalled(m => m.Løbenummer);
            bogføringslinjeModelMock.AssertWasCalled(m => m.Dato);
            bogføringslinjeModelMock.AssertWasCalled(m => m.Bilag);
            bogføringslinjeModelMock.AssertWasCalled(m => m.Kontonummer);
            bogføringslinjeModelMock.AssertWasCalled(m => m.Tekst);
            bogføringslinjeModelMock.AssertWasCalled(m => m.Budgetkontonummer);
            bogføringslinjeModelMock.AssertWasCalled(m => m.Debit);
            bogføringslinjeModelMock.AssertWasCalled(m => m.Kredit);
            bogføringslinjeModelMock.AssertWasCalled(m => m.Bogført);
            bogføringslinjeModelMock.AssertWasCalled(m => m.Adressekonto);
            bogføringslinjeModelMock.AssertWasCalled(m => m.Nyhedsinformation);
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

        /// <summary>
        /// Tester, at getteren til DebitAsText returnerer tekstangivelse af debitbeløb.
        /// </summary>
        [Test]
        [TestCase(-1000)]
        [TestCase(0)]
        [TestCase(1000)]
        public void TestAtDebitAsTextGetterReturnererDebitSomText(decimal debit)
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IBogføringslinjeModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IBogføringslinjeModel>();
                    mock.Expect(m => m.Debit)
                        .Return(debit)
                        .Repeat.Any();
                    return mock;
                }));

            var regnskabViewModelMock = fixture.Create<IRegnskabViewModel>();
            var bogføringslinjeModelMock = fixture.Create<IBogføringslinjeModel>();
            var bogføringslinjeViewModel = new BogføringslinjeViewModel(regnskabViewModelMock, bogføringslinjeModelMock);
            Assert.That(bogføringslinjeViewModel, Is.Not.Null);

            if (debit == 0M)
            {
                Assert.That(bogføringslinjeViewModel.DebitAsText, Is.Not.Null);
                Assert.That(bogføringslinjeViewModel.DebitAsText, Is.Empty);
            }
            else
            {
                Assert.That(bogføringslinjeViewModel.DebitAsText, Is.Not.Null);
                Assert.That(bogføringslinjeViewModel.DebitAsText, Is.Not.Empty);
                Assert.That(bogføringslinjeViewModel.DebitAsText, Is.EqualTo(debit.ToString("C")));
            }

            bogføringslinjeModelMock.AssertWasCalled(m => m.Debit);
        }

        /// <summary>
        /// Tester, at getteren til KreditAsText returnerer tekstangivelse af kreditbeløb.
        /// </summary>
        [Test]
        [TestCase(-1000)]
        [TestCase(0)]
        [TestCase(1000)]
        public void TestAtKreditAsTextGetterReturnererKreditSomText(decimal kredit)
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IBogføringslinjeModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IBogføringslinjeModel>();
                    mock.Expect(m => m.Kredit)
                        .Return(kredit)
                        .Repeat.Any();
                    return mock;
                }));

            var regnskabViewModelMock = fixture.Create<IRegnskabViewModel>();
            var bogføringslinjeModelMock = fixture.Create<IBogføringslinjeModel>();
            var bogføringslinjeViewModel = new BogføringslinjeViewModel(regnskabViewModelMock, bogføringslinjeModelMock);
            Assert.That(bogføringslinjeViewModel, Is.Not.Null);

            if (kredit == 0M)
            {
                Assert.That(bogføringslinjeViewModel.KreditAsText, Is.Not.Null);
                Assert.That(bogføringslinjeViewModel.KreditAsText, Is.Empty);
            }
            else
            {
                Assert.That(bogføringslinjeViewModel.KreditAsText, Is.Not.Null);
                Assert.That(bogføringslinjeViewModel.KreditAsText, Is.Not.Empty);
                Assert.That(bogføringslinjeViewModel.KreditAsText, Is.EqualTo(kredit.ToString("C")));
            }

            bogføringslinjeModelMock.AssertWasCalled(m => m.Kredit);
        }

        /// <summary>
        /// Tester, at getteren til BogførtAsText returnerer tekstangivelse af bogføringsbeløb.
        /// </summary>
        [Test]
        [TestCase(-1000)]
        [TestCase(0)]
        [TestCase(1000)]
        public void TestAtBogførtAsTextGetterReturnererBogførtSomText(decimal bogført)
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IBogføringslinjeModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IBogføringslinjeModel>();
                    mock.Expect(m => m.Bogført)
                        .Return(bogført)
                        .Repeat.Any();
                    return mock;
                }));

            var regnskabViewModelMock = fixture.Create<IRegnskabViewModel>();
            var bogføringslinjeModelMock = fixture.Create<IBogføringslinjeModel>();
            var bogføringslinjeViewModel = new BogføringslinjeViewModel(regnskabViewModelMock, bogføringslinjeModelMock);
            Assert.That(bogføringslinjeViewModel, Is.Not.Null);

            if (bogført == 0M)
            {
                Assert.That(bogføringslinjeViewModel.BogførtAsText, Is.Not.Null);
                Assert.That(bogføringslinjeViewModel.BogførtAsText, Is.Empty);
            }
            else
            {
                Assert.That(bogføringslinjeViewModel.BogførtAsText, Is.Not.Null);
                Assert.That(bogføringslinjeViewModel.BogførtAsText, Is.Not.Empty);
                Assert.That(bogføringslinjeViewModel.BogførtAsText, Is.EqualTo(bogført.ToString("C")));
            }

            bogføringslinjeModelMock.AssertWasCalled(m => m.Bogført);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBogføringslinjeModelEventHandler rejser PropertyChanged, når modellen for bogføringslinjen opdateres.
        /// </summary>
        [Test]
        [TestCase("Løbenummer", "Løbenummer")]
        [TestCase("Dato", "Dato")]
        [TestCase("Bilag", "Bilag")]
        [TestCase("Kontonummer", "Kontonummer")]
        [TestCase("Tekst", "Tekst")]
        [TestCase("Budgetkontonummer", "Budgetkontonummer")]
        [TestCase("Debit", "Debit")]
        [TestCase("Debit", "DebitAsText")]
        [TestCase("Kredit", "Kredit")]
        [TestCase("Kredit", "KreditAsText")]
        [TestCase("Bogført", "Bogført")]
        [TestCase("Bogført", "BogførtAsText")]
        [TestCase("Adressekonto", "Adressekonto")]
        [TestCase("Nyhedsinformation", "Nyhedsinformation")]
        [TestCase("Nyhedsinformation", "DisplayName")]
        public void TestAtPropertyChangedOnBogføringslinjeModelEventHandlerRejserPropertyChangedOnBogføringslinjeModelUpdate(string propertyNameToRaise, string expectPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IBogføringslinjeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringslinjeModel>()));

            var bogføringslinjeModelMock = fixture.Create<IBogføringslinjeModel>();
            var bogføringslinjeViewModel = new BogføringslinjeViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock);
            Assert.That(bogføringslinjeViewModel, Is.Not.Null);

            var eventCalled = false;
            bogføringslinjeViewModel.PropertyChanged += (s, e) =>
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
            bogføringslinjeModelMock.Raise(m => m.PropertyChanged += null,  bogføringslinjeModelMock, new PropertyChangedEventArgs(propertyNameToRaise));
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBogføringslinjeModelEventHandler kaster en ArgumentNullException, hvis objektet, der rejser eventet er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnBogføringslinjeModelEventHandlerKasterArgumentNullExceptionHvisSenderErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IBogføringslinjeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringslinjeModel>()));

            var bogføringslinjeModelMock = fixture.Create<IBogføringslinjeModel>();
            var bogføringslinjeViewModel = new BogføringslinjeViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock);
            Assert.That(bogføringslinjeViewModel, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => bogføringslinjeModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBogføringslinjeModelEventHandler kaster en ArgumentNullException, hvis argumenter til eventet er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnBogføringslinjeModelEventHandlerKasterArgumentNullExceptionHvisEventArgsErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IBogføringslinjeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringslinjeModel>()));

            var bogføringslinjeModelMock = fixture.Create<IBogføringslinjeModel>();
            var bogføringslinjeViewModel = new BogføringslinjeViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock);
            Assert.That(bogføringslinjeViewModel, Is.Not.Null);
        
            Assert.Throws<ArgumentNullException>(() => bogføringslinjeModelMock.Raise(m => m.PropertyChanged += null, fixture.Create<object>(), null));
        }
    }
}
