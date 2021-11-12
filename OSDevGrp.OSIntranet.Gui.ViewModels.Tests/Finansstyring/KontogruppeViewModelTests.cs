using System;
using System.ComponentModel;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using Rhino.Mocks;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Finansstyring
{
    /// <summary>
    /// Tester ViewModel til en kontogruppe.
    /// </summary>
    [TestFixture]
    public class KontogruppeViewModelTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en ViewModel til en kontogruppe.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererKontogruppeViewModel()
        {
            var fixture = new Fixture();

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

            var kontogruppeViewModel = new KontogruppeViewModel(kontogruppeModelMock, exceptionHandleViewModelMock);
            Assert.That(kontogruppeViewModel, Is.Not.Null);
            Assert.That(kontogruppeViewModel.Id, Is.Not.Null);
            Assert.That(kontogruppeViewModel.Id, Is.Not.Empty);
            Assert.That(kontogruppeViewModel.Id, Is.EqualTo(kontogruppeModelMock.Id));
            Assert.That(kontogruppeViewModel.Nummer, Is.EqualTo(kontogruppeModelMock.Nummer));
            Assert.That(kontogruppeViewModel.Tekst, Is.Not.Null);
            Assert.That(kontogruppeViewModel.Tekst, Is.Not.Empty);
            Assert.That(kontogruppeViewModel.Tekst, Is.EqualTo(kontogruppeModelMock.Tekst));
            Assert.That(kontogruppeViewModel.DisplayName, Is.Not.Null);
            Assert.That(kontogruppeViewModel.DisplayName, Is.Not.Empty);
            Assert.That(kontogruppeViewModel.DisplayName, Is.EqualTo(kontogruppeModelMock.Tekst));
            Assert.That(kontogruppeViewModel.Balancetype, Is.EqualTo(kontogruppeModelMock.Balancetype));

            kontogruppeModelMock.AssertWasCalled(m => m.Id);
            kontogruppeModelMock.AssertWasCalled(m => m.Nummer);
            kontogruppeModelMock.AssertWasCalled(m => m.Tekst);
            kontogruppeModelMock.AssertWasCalled(m => m.Balancetype);
            exceptionHandleViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis modellen for kontogruppen, er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisKontogruppeModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new KontogruppeViewModel(null, fixture.Create<IExceptionHandlerViewModel>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("tabelModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis ViewModel for exceptionhandleren er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisExceptionHandleViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IKontogruppeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new KontogruppeViewModel(fixture.Create<IKontogruppeModel>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("exceptionHandlerViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at sætteren til Balancetype opdaterer Balancetype på modellen for kontogruppen.
        /// </summary>
        [Test]
        public void TestAtBalancetypeSetterOpdatererBalancetypeOnKontogruppeModel()
        {
            var fixture = new Fixture();
            fixture.Customize<IKontogruppeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeModel>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var kontogruppeModelMock = fixture.Create<IKontogruppeModel>();
            var exceptionHandleViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var kontogruppeViewModel = new KontogruppeViewModel(kontogruppeModelMock, exceptionHandleViewModelMock);
            Assert.That(kontogruppeViewModel, Is.Not.Null);
            Assert.That(kontogruppeViewModel.Balancetype, Is.Not.EqualTo(Balancetype.Passiver));

            kontogruppeViewModel.Balancetype = Balancetype.Passiver;

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
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = fixture.Create<Exception>();
            var kontogruppeModelMock = MockRepository.GenerateMock<IKontogruppeModel>();
            kontogruppeModelMock.Expect(m => m.Balancetype = Arg<Balancetype>.Is.Anything)
                                .Throw(exception)
                                .Repeat.Any();

            var exceptionHandleViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var kontogruppeViewModel = new KontogruppeViewModel(kontogruppeModelMock, exceptionHandleViewModelMock);
            Assert.That(kontogruppeViewModel, Is.Not.Null);
            Assert.That(kontogruppeViewModel.Balancetype, Is.Not.EqualTo(Balancetype.Passiver));

            kontogruppeViewModel.Balancetype = Balancetype.Passiver;

            kontogruppeModelMock.AssertWasCalled(m => m.Balancetype = Arg<Balancetype>.Is.Equal(Balancetype.Passiver));
            exceptionHandleViewModelMock.AssertWasCalled(m => m.HandleException(Arg<Exception>.Is.Equal(exception)));
        }

        /// <summary>
        /// Tester, at PropertyChangedOnTabelModelEventHandler rejser PropertyChanged, når modellen for kontogruppen opdateres.
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
            fixture.Customize<IKontogruppeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeModel>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var kontogruppeModelMock = fixture.Create<IKontogruppeModel>();
            var exceptionHandleViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var kontogruppeViewModel = new KontogruppeViewModel(kontogruppeModelMock, exceptionHandleViewModelMock);
            Assert.That(kontogruppeViewModel, Is.Not.Null);

            var eventCalled = false;
            kontogruppeViewModel.PropertyChanged += (s, e) =>
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

        /// <summary>
        /// Tester, at CreateBalancelinje kaster en ArgumentNullException, hvis ViewModel for regnskabet, hvori linjen skal indgå i balancen, er null.
        /// </summary>
        [Test]
        public void TestAtCreateBalancelinjeKasterArgumentNullExceptionHvisRegnskabViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IKontogruppeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeModel>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var kontogruppeModelMock = fixture.Create<IKontogruppeModel>();
            var exceptionHandleViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var kontogruppeViewModel = new KontogruppeViewModel(kontogruppeModelMock, exceptionHandleViewModelMock);
            Assert.That(kontogruppeViewModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => kontogruppeViewModel.CreateBalancelinje(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("regnskabViewModel"));
            Assert.That(exception.InnerException, Is.Null);

            exceptionHandleViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at CreateBalancelinje danner en ViewModel for en linje, der kan indgå i balancen i det givne regnskab.
        /// </summary>
        [Test]
        public void TestAtCreateBalancelinjeCreatesBalanceViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var kontogruppeModelMock = MockRepository.GenerateMock<IKontogruppeModel>();
            kontogruppeModelMock.Expect(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();
            kontogruppeModelMock.Expect(m => m.Tekst)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            kontogruppeModelMock.Expect(m => m.Balancetype)
                .Return(Balancetype.Aktiver)
                .Repeat.Any();

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();

            var exceptionHandleViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var kontogruppeViewModel = new KontogruppeViewModel(kontogruppeModelMock, exceptionHandleViewModelMock);
            Assert.That(kontogruppeViewModel, Is.Not.Null);

            var opgørelseViewModel = kontogruppeViewModel.CreateBalancelinje(regnskabViewModelMock);
            Assert.That(opgørelseViewModel, Is.Not.Null);
            Assert.That(opgørelseViewModel.Nummer, Is.EqualTo(kontogruppeModelMock.Nummer));
            Assert.That(opgørelseViewModel.Tekst, Is.Not.Null);
            Assert.That(opgørelseViewModel.Tekst, Is.Not.Empty);
            Assert.That(opgørelseViewModel.Tekst, Is.EqualTo(kontogruppeModelMock.Tekst));
            Assert.That(opgørelseViewModel.Balancetype, Is.EqualTo(kontogruppeModelMock.Balancetype));

            exceptionHandleViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }
    }
}
