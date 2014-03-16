using System;
using System.ComponentModel;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Finansstyring
{
    /// <summary>
    /// Tester ViewModel til en kontogruppe for budgetkonti.
    /// </summary>
    [TestFixture]
    public class BudgetkontogruppeViewModelTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en ViewModel til en kontogruppe til budgetkonti.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererBudgetkontogruppeViewModel()
        {
            var fixture = new Fixture();

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

            var budgetkontogruppeViewModel = new BudgetkontogruppeViewModel(budgetkontogruppeModelMock, exceptionHandleViewModelMock);
            Assert.That(budgetkontogruppeViewModel, Is.Not.Null);
            Assert.That(budgetkontogruppeViewModel.Id, Is.Not.Null);
            Assert.That(budgetkontogruppeViewModel.Id, Is.Not.Empty);
            Assert.That(budgetkontogruppeViewModel.Id, Is.EqualTo(budgetkontogruppeModelMock.Id));
            Assert.That(budgetkontogruppeViewModel.Nummer, Is.EqualTo(budgetkontogruppeModelMock.Nummer));
            Assert.That(budgetkontogruppeViewModel.Tekst, Is.Not.Null);
            Assert.That(budgetkontogruppeViewModel.Tekst, Is.Not.Empty);
            Assert.That(budgetkontogruppeViewModel.Tekst, Is.EqualTo(budgetkontogruppeModelMock.Tekst));
            Assert.That(budgetkontogruppeViewModel.DisplayName, Is.Not.Null);
            Assert.That(budgetkontogruppeViewModel.DisplayName, Is.Not.Empty);
            Assert.That(budgetkontogruppeViewModel.DisplayName, Is.EqualTo(budgetkontogruppeModelMock.Tekst));

            budgetkontogruppeModelMock.AssertWasCalled(m => m.Id);
            budgetkontogruppeModelMock.AssertWasCalled(m => m.Nummer);
            budgetkontogruppeModelMock.AssertWasCalled(m => m.Tekst);
            exceptionHandleViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis modellen for kontogruppen til budgetkonti, er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisBudgetkontogruppeModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new BudgetkontogruppeViewModel(null, fixture.Create<IExceptionHandlerViewModel>()));
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
            fixture.Customize<IBudgetkontogruppeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontogruppeModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new BudgetkontogruppeViewModel(fixture.Create<IBudgetkontogruppeModel>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("exceptionHandlerViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnTabelModelEventHandler rejser PropertyChanged, når modellen for kontogruppen til budgetkonti opdateres.
        /// </summary>
        [Test]
        [TestCase("Id", "Id")]
        [TestCase("Nummer", "Nummer")]
        [TestCase("Tekst", "Tekst")]
        [TestCase("Tekst", "DisplayName")]
        public void TestAtPropertyChangedOnTabelModelEventHandlerRejserPropertyChangedOnBudgetkontogruppeModelUpdate(string propertyNameToRaise, string expectPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<IBudgetkontogruppeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontogruppeModel>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var budgetkontogruppeModelMock = fixture.Create<IBudgetkontogruppeModel>();
            var exceptionHandleViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var kontogruppeViewModel = new BudgetkontogruppeViewModel(budgetkontogruppeModelMock, exceptionHandleViewModelMock);
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
            budgetkontogruppeModelMock.Raise(m => m.PropertyChanged += null, budgetkontogruppeModelMock, new PropertyChangedEventArgs(propertyNameToRaise));
            Assert.That(eventCalled, Is.True);

            exceptionHandleViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }
    }
}
