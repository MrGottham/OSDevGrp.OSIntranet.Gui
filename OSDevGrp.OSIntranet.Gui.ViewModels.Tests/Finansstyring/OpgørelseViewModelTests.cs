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
    /// Tester ViewModel for en linje i opgørelsen.
    /// </summary>
    [TestFixture]
    public class OpgørelseViewModelTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en ViewModel til en linje i opgørelsen.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererOpgørelseViewModel()
        {
            var fixture = new Fixture();

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();

            var budgetkontogruppeModelMock = MockRepository.GenerateMock<IBudgetkontogruppeModel>();
            budgetkontogruppeModelMock.Expect(m => m.Id)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            budgetkontogruppeModelMock.Expect(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();
            budgetkontogruppeModelMock.Expect(m => m.Tekst)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var exceptionHandleViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var opgørelseViewModel = new OpgørelseViewModel(regnskabViewModelMock, budgetkontogruppeModelMock, exceptionHandleViewModelMock);
            Assert.That(opgørelseViewModel, Is.Not.Null);
            Assert.That(opgørelseViewModel.Id, Is.Not.Null);
            Assert.That(opgørelseViewModel.Id, Is.Not.Empty);
            Assert.That(opgørelseViewModel.Id, Is.EqualTo(budgetkontogruppeModelMock.Id));
            Assert.That(opgørelseViewModel.Nummer, Is.EqualTo(budgetkontogruppeModelMock.Nummer));
            Assert.That(opgørelseViewModel.Tekst, Is.Not.Null);
            Assert.That(opgørelseViewModel.Tekst, Is.Not.Empty);
            Assert.That(opgørelseViewModel.Tekst, Is.EqualTo(budgetkontogruppeModelMock.Tekst));
            Assert.That(opgørelseViewModel.DisplayName, Is.Not.Null);
            Assert.That(opgørelseViewModel.DisplayName, Is.Not.Empty);
            Assert.That(opgørelseViewModel.DisplayName, Is.EqualTo(budgetkontogruppeModelMock.Tekst));

            budgetkontogruppeModelMock.AssertWasCalled(m => m.Id);
            budgetkontogruppeModelMock.AssertWasCalled(m => m.Nummer);
            budgetkontogruppeModelMock.AssertWasCalled(m => m.Tekst);

            exceptionHandleViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis ViewModel for regnskabet, som opgørelseslinjen er tilknyttet, er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisRegnskabViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IBudgetkontogruppeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontogruppeModel>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new OpgørelseViewModel(null, fixture.Create<IBudgetkontogruppeModel>(), fixture.Create<IExceptionHandlerViewModel>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("regnskabViewModel"));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis modellen for gruppen af budgetkonti, som opgørelseslinjen baserer sig på, er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisBudgetkontogruppeModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new OpgørelseViewModel(fixture.Create<IRegnskabViewModel>(), null, fixture.Create<IExceptionHandlerViewModel>()));
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
            fixture.Customize<IBudgetkontogruppeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontogruppeModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new OpgørelseViewModel(fixture.Create<IRegnskabViewModel>(), fixture.Create<IBudgetkontogruppeModel>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("exceptionHandlerViewModel"));
        }

        /// <summary>
        /// Tester, at sætteren til Tekst opdaterer Tekst på modellen for gruppen af budgetkonti, som opgørelseslinjen baserer sig på.
        /// </summary>
        [Test]
        public void TestAtTekstSetterOpdatererTekstOnOpgørelseViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var budgetkontogruppeModelMock = MockRepository.GenerateMock<IBudgetkontogruppeModel>();
            var exceptionHandleViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var opgørelseViewModel = new OpgørelseViewModel(fixture.Create<IRegnskabViewModel>(), budgetkontogruppeModelMock, exceptionHandleViewModelMock);
            Assert.That(opgørelseViewModel, Is.Not.Null);

            var newValue = fixture.Create<string>();
            opgørelseViewModel.Tekst = newValue;

            budgetkontogruppeModelMock.AssertWasCalled(m => m.Tekst = Arg<string>.Is.Equal(newValue));
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
            var budgetkontogruppeModelMock = MockRepository.GenerateMock<IBudgetkontogruppeModel>();
            budgetkontogruppeModelMock.Expect(m => m.Tekst = Arg<string>.Is.Anything)
                .Throw(exception)
                .Repeat.Any();

            var exceptionHandleViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var opgørelseViewModel = new OpgørelseViewModel(fixture.Create<IRegnskabViewModel>(), budgetkontogruppeModelMock, exceptionHandleViewModelMock);
            Assert.That(opgørelseViewModel, Is.Not.Null);

            var newValue = fixture.Create<string>();
            opgørelseViewModel.Tekst = newValue;

            budgetkontogruppeModelMock.AssertWasCalled(m => m.Tekst = Arg<string>.Is.Equal(newValue));
            exceptionHandleViewModelMock.AssertWasCalled(m => m.HandleException(Arg<Exception>.Is.Equal(exception)));
        }

        /// <summary>
        /// Tester, at PropertyChangedOnTabelModelEventHandler rejser PropertyChanged, modellen for gruppen af budgetkonti, som opgørelseslinjen baserer sig på, opdateres.
        /// </summary>
        [Test]
        [TestCase("Id", "Id")]
        [TestCase("Nummer", "Nummer")]
        [TestCase("Tekst", "Tekst")]
        [TestCase("Tekst", "DisplayName")]
        public void TestAtPropertyChangedOnTabelModelEventHandlerRejserPropertyChangedOnBudgetkontogruppeModelUpdate(string propertyNameToRaise, string expectPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IBudgetkontogruppeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontogruppeModel>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var budgetkontogruppeModelMock = fixture.Create<IBudgetkontogruppeModel>();
            var exceptionHandleViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var opgørelseViewModel = new OpgørelseViewModel(fixture.Create<IRegnskabViewModel>(), budgetkontogruppeModelMock, exceptionHandleViewModelMock);
            Assert.That(opgørelseViewModel, Is.Not.Null);

            var eventCalled = false;
            opgørelseViewModel.PropertyChanged += (s, e) =>
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
            budgetkontogruppeModelMock.Raise(m => m.PropertyChanged += null, budgetkontogruppeModelMock, new PropertyChangedEventArgs(propertyNameToRaise));
            Assert.That(eventCalled, Is.True);

            exceptionHandleViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }
    }
}
