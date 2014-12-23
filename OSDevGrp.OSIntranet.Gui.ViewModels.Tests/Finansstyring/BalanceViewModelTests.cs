using System;
using System.ComponentModel;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Finansstyring
{
    /// <summary>
    /// Tester ViewModel for en linje i balancen.
    /// </summary>
    [TestFixture]
    public class BalanceViewModelTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en ViewModel til en linje i balancen.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererBalanceViewModel()
        {
            var fixture = new Fixture();

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();

            var kontogruppeModelMock = MockRepository.GenerateMock<IKontogruppeModel>();
            kontogruppeModelMock.Expect(m => m.Id)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            kontogruppeModelMock.Expect(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();
            kontogruppeModelMock.Expect(m => m.Tekst)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            kontogruppeModelMock.Expect(m => m.Balancetype)
                .Return(fixture.Create<Balancetype>())
                .Repeat.Any();

            var exceptionHandleViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var balanceViewModel = new BalanceViewModel(regnskabViewModelMock, kontogruppeModelMock, exceptionHandleViewModelMock);
            Assert.That(balanceViewModel, Is.Not.Null);
            Assert.That(balanceViewModel.Id, Is.Not.Null);
            Assert.That(balanceViewModel.Id, Is.Not.Empty);
            Assert.That(balanceViewModel.Id, Is.EqualTo(kontogruppeModelMock.Id));
            Assert.That(balanceViewModel.Nummer, Is.EqualTo(kontogruppeModelMock.Nummer));
            Assert.That(balanceViewModel.Tekst, Is.Not.Null);
            Assert.That(balanceViewModel.Tekst, Is.Not.Empty);
            Assert.That(balanceViewModel.Tekst, Is.EqualTo(kontogruppeModelMock.Tekst));
            Assert.That(balanceViewModel.DisplayName, Is.Not.Null);
            Assert.That(balanceViewModel.DisplayName, Is.Not.Empty);
            Assert.That(balanceViewModel.DisplayName, Is.EqualTo(kontogruppeModelMock.Tekst));
            Assert.That(balanceViewModel.Balancetype, Is.EqualTo(kontogruppeModelMock.Balancetype));

            kontogruppeModelMock.AssertWasCalled(m => m.Id);
            kontogruppeModelMock.AssertWasCalled(m => m.Nummer);
            kontogruppeModelMock.AssertWasCalled(m => m.Tekst);
            kontogruppeModelMock.AssertWasCalled(m => m.Balancetype);

            exceptionHandleViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis ViewModel for regnskabet, som balancelinjen er tilknyttet, er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisRegnskabViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IKontogruppeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeModel>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new BalanceViewModel(null, fixture.Create<IKontogruppeModel>(), fixture.Create<IExceptionHandlerViewModel>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("regnskabViewModel"));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis modellen for gruppen af konti, som balancelinjen baserer sig på, er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisKontogruppeModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new BalanceViewModel(fixture.Create<IRegnskabViewModel>(), null, fixture.Create<IExceptionHandlerViewModel>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("tabelModel"));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis ViewModel for exceptionhandleren er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisExceptionHandlerViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IKontogruppeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new BalanceViewModel(fixture.Create<IRegnskabViewModel>(), fixture.Create<IKontogruppeModel>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("exceptionHandlerViewModel"));
        }

        /// <summary>
        /// Tester, at sætteren til Tekst opdaterer Tekst på modellen for gruppen af konti, som balancelinjen baserer sig på.
        /// </summary>
        [Test]
        public void TestAtTekstSetterOpdatererTekstOnBalanceViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var kontogruppeModelMock = MockRepository.GenerateMock<IKontogruppeModel>();
            var exceptionHandleViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var balanceViewModel = new BalanceViewModel(fixture.Create<IRegnskabViewModel>(), kontogruppeModelMock, exceptionHandleViewModelMock);
            Assert.That(balanceViewModel, Is.Not.Null);

            var newValue = fixture.Create<string>();
            balanceViewModel.Tekst = newValue;

            kontogruppeModelMock.AssertWasCalled(m => m.Tekst = Arg<string>.Is.Equal(newValue));
            exceptionHandleViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at sætteren til Tekst kalder HandleException på exceptionhandleren ved exceptions.
        /// </summary>
        [Test]
        public void TestAtTekstSetterKalderHandleExceptionOnExceptionHandlerViewModelVedExceptions()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = fixture.Create<Exception>();
            var kontogruppeModelMock = MockRepository.GenerateMock<IKontogruppeModel>();
            kontogruppeModelMock.Expect(m => m.Tekst = Arg<string>.Is.Anything)
                .Throw(exception)
                .Repeat.Any();

            var exceptionHandleViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var balanceViewModel = new BalanceViewModel(fixture.Create<IRegnskabViewModel>(), kontogruppeModelMock, exceptionHandleViewModelMock);
            Assert.That(balanceViewModel, Is.Not.Null);

            var newValue = fixture.Create<string>();
            balanceViewModel.Tekst = newValue;

            kontogruppeModelMock.AssertWasCalled(m => m.Tekst = Arg<string>.Is.Equal(newValue));
            exceptionHandleViewModelMock.AssertWasCalled(m => m.HandleException(Arg<Exception>.Is.Equal(exception)));
        }

        /// <summary>
        /// Tester, at sætteren til Balancetype opdaterer Balancetype på modellen for gruppen af konti, som balancelinjen baserer sig på.
        /// </summary>
        [Test]
        public void TestAtBalancetypeSetterOpdatererBalancetypeOnBalanceViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var kontogruppeModelMock = MockRepository.GenerateMock<IKontogruppeModel>();
            kontogruppeModelMock.Expect(m => m.Balancetype)
                .Return(Balancetype.Aktiver)
                .Repeat.Any();

            var exceptionHandleViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var balanceViewModel = new BalanceViewModel(fixture.Create<IRegnskabViewModel>(), kontogruppeModelMock, exceptionHandleViewModelMock);
            Assert.That(balanceViewModel, Is.Not.Null);
            Assert.That(balanceViewModel.Balancetype, Is.Not.EqualTo(Balancetype.Passiver));

            balanceViewModel.Balancetype = Balancetype.Passiver;

            kontogruppeModelMock.AssertWasCalled(m => m.Balancetype = Arg<Balancetype>.Is.Equal(Balancetype.Passiver));
            exceptionHandleViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at sætteren til Balancetype kalder HandleException på exceptionhandleren ved exceptions.
        /// </summary>
        [Test]
        public void TestAtBalancetypeSetterKalderHandleExceptionOnExceptionHandlerViewModelVedExceptions()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = fixture.Create<Exception>();
            var kontogruppeModelMock = MockRepository.GenerateMock<IKontogruppeModel>();
            kontogruppeModelMock.Expect(m => m.Balancetype)
                .Return(Balancetype.Aktiver)
                .Repeat.Any();
            kontogruppeModelMock.Expect(m => m.Balancetype = Arg<Balancetype>.Is.Anything)
                .Throw(exception)
                .Repeat.Any();

            var exceptionHandleViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var balanceViewModel = new BalanceViewModel(fixture.Create<IRegnskabViewModel>(), kontogruppeModelMock, exceptionHandleViewModelMock);
            Assert.That(balanceViewModel, Is.Not.Null);
            Assert.That(balanceViewModel.Balancetype, Is.Not.EqualTo(Balancetype.Passiver));

            balanceViewModel.Balancetype = Balancetype.Passiver;

            kontogruppeModelMock.AssertWasCalled(m => m.Balancetype = Arg<Balancetype>.Is.Equal(Balancetype.Passiver));
            exceptionHandleViewModelMock.AssertWasCalled(m => m.HandleException(Arg<Exception>.Is.Equal(exception)));
        }

        /// <summary>
        /// Tester, at PropertyChangedOnTabelModelEventHandler rejser PropertyChanged, modellen for gruppen af konti, som balancelinjen baserer sig på, opdateres.
        /// </summary>
        [Test]
        [TestCase("Id", "Id")]
        [TestCase("Nummer", "Nummer")]
        [TestCase("Tekst", "Tekst")]
        [TestCase("Tekst", "DisplayName")]
        [TestCase("Balancetype", "Balancetype")]
        public void TestAtPropertyChangedOnTabelModelEventHandlerRejserPropertyChangedOnKontogruppeModelUpdate(string propertyNameToRaise, string expectPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IKontogruppeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeModel>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var kontogruppeModelMock = fixture.Create<IKontogruppeModel>();
            var exceptionHandleViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var balanceViewModel = new BalanceViewModel(fixture.Create<IRegnskabViewModel>(), kontogruppeModelMock, exceptionHandleViewModelMock);
            Assert.That(balanceViewModel, Is.Not.Null);

            var eventCalled = false;
            balanceViewModel.PropertyChanged += (s, e) =>
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
            kontogruppeModelMock.Raise(m => m.PropertyChanged += null, kontogruppeModelMock, new PropertyChangedEventArgs(propertyNameToRaise));
            Assert.That(eventCalled, Is.True);

            exceptionHandleViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }
    }
}
