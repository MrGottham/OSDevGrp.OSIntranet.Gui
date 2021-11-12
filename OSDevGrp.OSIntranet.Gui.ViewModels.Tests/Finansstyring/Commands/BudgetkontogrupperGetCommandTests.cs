using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Finansstyring.Commands
{
    /// <summary>
    /// Tester kommando, der kan hente kontogrupper for budgetkonti til et regnskab.
    /// </summary>
    [TestFixture]
    public class BudgetkontogrupperGetCommandTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en kommando, der kan hente kontogrupper for budgetkonti til et regnskab.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererBudgetkontogrupperGetCommand()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var command = new BudgetkontogrupperGetCommand(fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(command, Is.Not.Null);
            Assert.That(command.ExecuteTask, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis den repositoryet til finansstyring er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFinansstyringRepositoryErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new BudgetkontogrupperGetCommand(null, fixture.Create<IExceptionHandlerViewModel>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("finansstyringRepository"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis den ViewModel til exceptionhandleren er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisExceptionHandlerViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new BudgetkontogrupperGetCommand(fixture.Create<IFinansstyringRepository>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("exceptionHandlerViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at CanExecute returnerer true.
        /// </summary>
        [Test]
        public void TestAtCanExecuteReturnererTrue()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var command = new BudgetkontogrupperGetCommand(fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(command, Is.Not.Null);

            var result = command.CanExecute(fixture.Create<IRegnskabViewModel>());
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tester, at Execute returnerer, hvis kontogrupper til budgetkonti allerede er hentet.
        /// </summary>
        [Test]
        public void TestAtExecuteReturnererHvisKontogrupperErHentet()
        {
            var fixture = new Fixture();
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontogruppeViewModel>()));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IRegnskabViewModel>();
                    mock.Expect(m => m.Budgetkontogrupper)
                        .Return(fixture.CreateMany<IBudgetkontogruppeViewModel>(7).ToList())
                        .Repeat.Any();
                    return mock;
                }));

            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var regnskabViewModelMock = fixture.Create<IRegnskabViewModel>();

            var command = new BudgetkontogrupperGetCommand(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);
            Assert.That(command.ExecuteTask, Is.Null);

            Action action = () =>
                {
                    command.Execute(regnskabViewModelMock);
                    Assert.That(command.ExecuteTask, Is.Null);
                };
            Task.Run(action).Wait(3000);

            regnskabViewModelMock.AssertWasCalled(m => m.Budgetkontogrupper);
            finansstyringRepositoryMock.AssertWasNotCalled(m => m.BudgetkontogruppelisteGetAsync());
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Execute henter kontogrupper for budgetkonti til regnskabet.
        /// </summary>
        [Test]
        public void TestAtExecuteHenterBudgetkontogrupper()
        {
            var fixture = new Fixture();
            fixture.Customize<IBudgetkontogruppeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontogruppeModel>()));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IRegnskabViewModel>();
                    mock.Expect(m => m.Budgetkontogrupper)
                        .Return(new List<IBudgetkontogruppeViewModel>(0))
                        .Repeat.Any();
                    return mock;
                }));

            var budgetkontogruppeModelMockCollection = fixture.CreateMany<IBudgetkontogruppeModel>(7).ToList();
            Func<IEnumerable<IBudgetkontogruppeModel>> kontogruppelisteGetter = () => budgetkontogruppeModelMockCollection;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.BudgetkontogruppelisteGetAsync())
                                       .Return(Task.Run(kontogruppelisteGetter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var regnskabViewModelMock = fixture.Create<IRegnskabViewModel>();

            var command = new BudgetkontogrupperGetCommand(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);
            Assert.That(command.ExecuteTask, Is.Null);

            Action action = () =>
                {
                    command.Execute(regnskabViewModelMock);
                    Assert.That(command.ExecuteTask, Is.Not.Null);
                    command.ExecuteTask.Wait();
                };
            Task.Run(action).Wait(3000);

            regnskabViewModelMock.AssertWasCalled(m => m.Budgetkontogrupper);
            finansstyringRepositoryMock.AssertWasCalled(m => m.BudgetkontogruppelisteGetAsync());
            regnskabViewModelMock.AssertWasCalled(m => m.BudgetkontogruppeAdd(Arg<IBudgetkontogruppeViewModel>.Is.NotNull), opt => opt.Repeat.Times(budgetkontogruppeModelMockCollection.Count));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Execute kalder HandleException på exceptionhandleren ved exceptions.
        /// </summary>
        [Test]
        public void TestAtExecuteKalderHandleExceptionOnExceptionHandlerViewModelVedException()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IRegnskabViewModel>();
                    mock.Expect(m => m.Budgetkontogrupper)
                        .Return(new List<IBudgetkontogruppeViewModel>(0))
                        .Repeat.Any();
                    return mock;
                }));

            var exception = fixture.Create<IntranetGuiRepositoryException>();
            Func<IEnumerable<IBudgetkontogruppeModel>> kontogruppelisteGetter = () =>
                {
                    throw exception;
                };
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.BudgetkontogruppelisteGetAsync())
                                       .Return(Task.Run(kontogruppelisteGetter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var regnskabViewModelMock = fixture.Create<IRegnskabViewModel>();

            var command = new BudgetkontogrupperGetCommand(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);
            Assert.That(command.ExecuteTask, Is.Null);

            Action action = () =>
                {
                    command.Execute(regnskabViewModelMock);
                    Assert.That(command.ExecuteTask, Is.Not.Null);
                    command.ExecuteTask.Wait();
                };
            Task.Run(action).Wait(3000);

            regnskabViewModelMock.AssertWasCalled(m => m.Budgetkontogrupper);
            finansstyringRepositoryMock.AssertWasCalled(m => m.BudgetkontogruppelisteGetAsync());
            regnskabViewModelMock.AssertWasNotCalled(m => m.BudgetkontogruppeAdd(Arg<IBudgetkontogruppeViewModel>.Is.Anything));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiRepositoryException>.Is.Equal(exception)));
        }
    }
}
