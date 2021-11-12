using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Finansstyring.Commands
{
    /// <summary>
    /// Tester kommando, der kan hente og opdatere en budgetkontoen.
    /// </summary>
    [TestFixture]
    public class BudgetkontoGetCommandTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en kommando, der kan hente og opdatere en budgetkontoen.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererBudgetkontoGetCommand()
        {
            var fixture = new Fixture();
            fixture.Customize<ITaskableCommand>(e => e.FromFactory(() => MockRepository.GenerateMock<ITaskableCommand>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var command = new BudgetkontoGetCommand(fixture.Create<ITaskableCommand>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(command, Is.Not.Null);
            Assert.That(command.ExecuteTask, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis den kommando, som denne kommando er afhængig af, er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisDependencyCommandErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new BudgetkontoGetCommand(null, fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("dependencyCommand"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis den repositoryet til finansstyring er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFinansstyringRepositoryErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<ITaskableCommand>(e => e.FromFactory(() => MockRepository.GenerateMock<ITaskableCommand>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new BudgetkontoGetCommand(fixture.Create<ITaskableCommand>(), null, fixture.Create<IExceptionHandlerViewModel>()));
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
            fixture.Customize<ITaskableCommand>(e => e.FromFactory(() => MockRepository.GenerateMock<ITaskableCommand>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new BudgetkontoGetCommand(fixture.Create<ITaskableCommand>(), fixture.Create<IFinansstyringRepository>(), null));
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
            fixture.Customize<IBudgetkontoViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontoViewModel>()));
            fixture.Customize<ITaskableCommand>(e => e.FromFactory(() => MockRepository.GenerateMock<ITaskableCommand>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var command = new BudgetkontoGetCommand(fixture.Create<ITaskableCommand>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(command, Is.Not.Null);

            var result = command.CanExecute(fixture.Create<IBudgetkontoViewModel>());
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tester, at Execute udfører den afhængige kommando, hvis CanExecute for den afhængige kommando returnerer true.
        /// </summary>
        [Test]
        public void TestAtExecuteExecuteDependencyCommandHvisCanExecuteOnDepencendyCommandErTrue()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var dependencyCommandMock = MockRepository.GenerateMock<ITaskableCommand>();
            dependencyCommandMock.Expect(m => m.CanExecute(Arg<IRegnskabViewModel>.Is.TypeOf))
                                 .Return(true)
                                 .Repeat.Any();
            dependencyCommandMock.Expect(m => m.ExecuteTask)
                                 .Return(Task.Run(() => Thread.Sleep(1000)))
                                 .Repeat.Any();

            Func<IBudgetkontoModel> budgetkontoModelGetter = () => null;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.BudgetkontoGetAsync(Arg<int>.Is.GreaterThan(0), Arg<string>.Is.NotNull, Arg<DateTime>.Is.GreaterThan(DateTime.MinValue)))
                                       .Return(Task.Run(budgetkontoModelGetter))
                                       .Repeat.Any();

            var exceptionHandlerMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var regnskabsnummer = fixture.Create<int>();
            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(regnskabsnummer)
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Budgetkontogrupper)
                                 .Return(new List<IBudgetkontogruppeViewModel>(0))
                                 .Repeat.Any();

            var kontonummer = fixture.Create<string>();
            var statusDato = fixture.Create<DateTime>();
            var budgetkontoViewModelMock = MockRepository.GenerateMock<IBudgetkontoViewModel>();
            budgetkontoViewModelMock.Expect(m => m.Regnskab)
                                    .Return(regnskabViewModelMock)
                                    .Repeat.Any();
            budgetkontoViewModelMock.Expect(m => m.Kontonummer)
                                    .Return(kontonummer)
                                    .Repeat.Any();
            budgetkontoViewModelMock.Expect(m => m.StatusDato)
                                    .Return(statusDato)
                                    .Repeat.Any();

            var command = new BudgetkontoGetCommand(dependencyCommandMock, finansstyringRepositoryMock, exceptionHandlerMock);
            Assert.That(command, Is.Not.Null);
            Assert.That(command.ExecuteTask, Is.Null);

            Action action = () =>
                {
                    command.Execute(budgetkontoViewModelMock);
                    Assert.That(command.ExecuteTask, Is.Not.Null);
                    command.ExecuteTask.Wait();
                };
            Task.Run(action).Wait(3000);

            budgetkontoViewModelMock.AssertWasCalled(m => m.Regnskab);
            dependencyCommandMock.AssertWasCalled(m => m.CanExecute(Arg<IRegnskabViewModel>.Is.Equal(regnskabViewModelMock)));
            dependencyCommandMock.AssertWasCalled(m => m.Execute(Arg<IRegnskabViewModel>.Is.Equal(regnskabViewModelMock)));
            dependencyCommandMock.AssertWasCalled(m => m.ExecuteTask);
            finansstyringRepositoryMock.AssertWasCalled(m => m.BudgetkontoGetAsync(Arg<int>.Is.Equal(regnskabsnummer), Arg<string>.Is.Equal(kontonummer), Arg<DateTime>.Is.Equal(statusDato)));
            exceptionHandlerMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Execute ikke udfører den afhængige kommando, hvis CanExecute for den afhængige kommando returnerer true.
        /// </summary>
        [Test]
        public void TestAtExecuteDontExecuteDependencyCommandHvisCanExecuteOnDepencendyCommandErFalse()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var dependencyCommandMock = MockRepository.GenerateMock<ITaskableCommand>();
            dependencyCommandMock.Expect(m => m.CanExecute(Arg<IRegnskabViewModel>.Is.TypeOf))
                                 .Return(false)
                                 .Repeat.Any();

            Func<IBudgetkontoModel> budgetkontoModelGetter = () => null;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.BudgetkontoGetAsync(Arg<int>.Is.GreaterThan(0), Arg<string>.Is.NotNull, Arg<DateTime>.Is.GreaterThan(DateTime.MinValue)))
                                       .Return(Task.Run(budgetkontoModelGetter))
                                       .Repeat.Any();

            var exceptionHandlerMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var regnskabsnummer = fixture.Create<int>();
            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(regnskabsnummer)
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Budgetkontogrupper)
                                 .Return(new List<IBudgetkontogruppeViewModel>(0))
                                 .Repeat.Any();

            var kontonummer = fixture.Create<string>();
            var statusDato = fixture.Create<DateTime>();
            var budgetkontoViewModelMock = MockRepository.GenerateMock<IBudgetkontoViewModel>();
            budgetkontoViewModelMock.Expect(m => m.Regnskab)
                                    .Return(regnskabViewModelMock)
                                    .Repeat.Any();
            budgetkontoViewModelMock.Expect(m => m.Kontonummer)
                                    .Return(kontonummer)
                                    .Repeat.Any();
            budgetkontoViewModelMock.Expect(m => m.StatusDato)
                                    .Return(statusDato)
                                    .Repeat.Any();

            var command = new BudgetkontoGetCommand(dependencyCommandMock, finansstyringRepositoryMock, exceptionHandlerMock);
            Assert.That(command, Is.Not.Null);
            Assert.That(command.ExecuteTask, Is.Null);

            Action action = () =>
                {
                    command.Execute(budgetkontoViewModelMock);
                    Assert.That(command.ExecuteTask, Is.Not.Null);
                    command.ExecuteTask.Wait();
                };
            Task.Run(action).Wait(3000);

            budgetkontoViewModelMock.AssertWasCalled(m => m.Regnskab);
            dependencyCommandMock.AssertWasCalled(m => m.CanExecute(Arg<IRegnskabViewModel>.Is.Equal(regnskabViewModelMock)));
            dependencyCommandMock.AssertWasNotCalled(m => m.Execute(Arg<object>.Is.Anything));
            dependencyCommandMock.AssertWasNotCalled(m => m.ExecuteTask);
            finansstyringRepositoryMock.AssertWasCalled(m => m.BudgetkontoGetAsync(Arg<int>.Is.Equal(regnskabsnummer), Arg<string>.Is.Equal(kontonummer), Arg<DateTime>.Is.Equal(statusDato)));
            exceptionHandlerMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Execute opdaterer ViewModel for budgetkontoen.
        /// </summary>
        [Test]
        public void TestAtExecuteOpdatererBudgetkontoViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IBudgetkontoModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IBudgetkontoModel>();
                    mock.Expect(m => m.Kontonavn)
                        .Return(fixture.Create<string>())
                        .Repeat.Any();
                    mock.Expect(m => m.Beskrivelse)
                        .Return(fixture.Create<string>())
                        .Repeat.Any();
                    mock.Expect(m => m.Notat)
                        .Return(fixture.Create<string>())
                        .Repeat.Any();
                    mock.Expect(m => m.Kontogruppe)
                        .Return(fixture.Create<int>())
                        .Repeat.Any();
                    mock.Expect(m => m.StatusDato)
                        .Return(fixture.Create<DateTime>().AddHours(3))
                        .Repeat.Any();
                    mock.Expect(m => m.Indtægter)
                        .Return(fixture.Create<decimal>())
                        .Repeat.Any();
                    mock.Expect(m => m.Udgifter)
                        .Return(fixture.Create<decimal>())
                        .Repeat.Any();
                    mock.Expect(m => m.BudgetSidsteMåned)
                        .Return(fixture.Create<decimal>())
                        .Repeat.Any();
                    mock.Expect(m => m.BudgetÅrTilDato)
                        .Return(fixture.Create<decimal>())
                        .Repeat.Any();
                    mock.Expect(m => m.BudgetSidsteÅr)
                        .Return(fixture.Create<decimal>())
                        .Repeat.Any();
                    mock.Expect(m => m.Bogført)
                        .Return(fixture.Create<decimal>())
                        .Repeat.Any();
                    mock.Expect(m => m.BogførtSidsteMåned)
                        .Return(fixture.Create<decimal>())
                        .Repeat.Any();
                    mock.Expect(m => m.BogførtÅrTilDato)
                        .Return(fixture.Create<decimal>())
                        .Repeat.Any();
                    mock.Expect(m => m.BogførtSidsteÅr)
                        .Return(fixture.Create<decimal>())
                        .Repeat.Any();
                    return mock;
                }));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(fixture.Create<int>())
                        .Repeat.Any();
                    return mock;
                }));

            var dependencyCommandMock = MockRepository.GenerateMock<ITaskableCommand>();
            dependencyCommandMock.Expect(m => m.CanExecute(Arg<IRegnskabViewModel>.Is.TypeOf))
                                 .Return(false)
                                 .Repeat.Any();

            var budgetkontoModelMock = fixture.Create<IBudgetkontoModel>();
            Func<IBudgetkontoModel> budgetkontoModelGetter = () => budgetkontoModelMock;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.BudgetkontoGetAsync(Arg<int>.Is.GreaterThan(0), Arg<string>.Is.NotNull, Arg<DateTime>.Is.GreaterThan(DateTime.MinValue)))
                                       .Return(Task.Run(budgetkontoModelGetter))
                                       .Repeat.Any();

            var exceptionHandlerMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var regnskabsnummer = fixture.Create<int>();
            var budgetkontogruppeViewModelMockCollection = fixture.CreateMany<IBudgetkontogruppeViewModel>(6).ToList();
            var budgetkontogruppeViewModelMock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
            budgetkontogruppeViewModelMock.Expect(m => m.Nummer)
                                          .Return(budgetkontoModelMock.Kontogruppe)
                                          .Repeat.Any();
            budgetkontogruppeViewModelMockCollection.Add(budgetkontogruppeViewModelMock);
            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(regnskabsnummer)
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Budgetkontogrupper)
                                 .Return(budgetkontogruppeViewModelMockCollection)
                                 .Repeat.Any();

            var kontonummer = fixture.Create<string>();
            var statusDato = fixture.Create<DateTime>();
            var budgetkontoViewModelMock = MockRepository.GenerateMock<IBudgetkontoViewModel>();
            budgetkontoViewModelMock.Expect(m => m.Regnskab)
                                    .Return(regnskabViewModelMock)
                                    .Repeat.Any();
            budgetkontoViewModelMock.Expect(m => m.Kontonummer)
                                    .Return(kontonummer)
                                    .Repeat.Any();
            budgetkontoViewModelMock.Expect(m => m.StatusDato)
                                    .Return(statusDato)
                                    .Repeat.Any();

            var command = new BudgetkontoGetCommand(dependencyCommandMock, finansstyringRepositoryMock, exceptionHandlerMock);
            Assert.That(command, Is.Not.Null);
            Assert.That(command.ExecuteTask, Is.Null);

            Action action = () =>
                {
                    command.Execute(budgetkontoViewModelMock);
                    Assert.That(command.ExecuteTask, Is.Not.Null);
                    command.ExecuteTask.Wait();
                };
            Task.Run(action).Wait(3000);

            budgetkontoViewModelMock.AssertWasCalled(m => m.Regnskab);
            dependencyCommandMock.AssertWasCalled(m => m.CanExecute(Arg<IRegnskabViewModel>.Is.Equal(regnskabViewModelMock)));
            dependencyCommandMock.AssertWasNotCalled(m => m.Execute(Arg<object>.Is.Anything));
            dependencyCommandMock.AssertWasNotCalled(m => m.ExecuteTask);
            finansstyringRepositoryMock.AssertWasCalled(m => m.BudgetkontoGetAsync(Arg<int>.Is.Equal(regnskabsnummer), Arg<string>.Is.Equal(kontonummer), Arg<DateTime>.Is.Equal(statusDato)));
            regnskabViewModelMock.AssertWasCalled(m => m.Budgetkontogrupper);
            budgetkontoViewModelMock.AssertWasCalled(m => m.Kontonavn = Arg<string>.Is.Equal(budgetkontoModelMock.Kontonavn));
            budgetkontoViewModelMock.AssertWasCalled(m => m.Beskrivelse = Arg<string>.Is.Equal(budgetkontoModelMock.Beskrivelse));
            budgetkontoViewModelMock.AssertWasCalled(m => m.Notat = Arg<string>.Is.Equal(budgetkontoModelMock.Notat));
            budgetkontoViewModelMock.AssertWasCalled(m => m.Kontogruppe = Arg<IBudgetkontogruppeViewModel>.Is.NotNull);
            budgetkontoViewModelMock.AssertWasCalled(m => m.StatusDato = Arg<DateTime>.Is.Equal(budgetkontoModelMock.StatusDato));
            budgetkontoViewModelMock.AssertWasCalled(m => m.Indtægter = Arg<decimal>.Is.Equal(budgetkontoModelMock.Indtægter));
            budgetkontoViewModelMock.AssertWasCalled(m => m.Udgifter = Arg<decimal>.Is.Equal(budgetkontoModelMock.Udgifter));
            budgetkontoViewModelMock.AssertWasCalled(m => m.BudgetSidsteMåned = Arg<decimal>.Is.Equal(budgetkontoModelMock.BudgetSidsteMåned));
            budgetkontoViewModelMock.AssertWasCalled(m => m.BudgetÅrTilDato = Arg<decimal>.Is.Equal(budgetkontoModelMock.BudgetÅrTilDato));
            budgetkontoViewModelMock.AssertWasCalled(m => m.BudgetSidsteÅr = Arg<decimal>.Is.Equal(budgetkontoModelMock.BudgetSidsteÅr));
            budgetkontoViewModelMock.AssertWasCalled(m => m.Bogført = Arg<decimal>.Is.Equal(budgetkontoModelMock.Bogført));
            budgetkontoViewModelMock.AssertWasCalled(m => m.BogførtSidsteMåned = Arg<decimal>.Is.Equal(budgetkontoModelMock.BogførtSidsteMåned));
            budgetkontoViewModelMock.AssertWasCalled(m => m.BogførtÅrTilDato = Arg<decimal>.Is.Equal(budgetkontoModelMock.BogførtÅrTilDato));
            budgetkontoViewModelMock.AssertWasCalled(m => m.BogførtSidsteÅr = Arg<decimal>.Is.Equal(budgetkontoModelMock.BogførtSidsteÅr));
            exceptionHandlerMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Execute kalder HandleException på exceptionhandleren med en IntranetGuiSystemException, hvis kontogruppen til budgetkonti ikke findes.
        /// </summary>
        [Test]
        public void TestAtExecuteKalderHandleExceptionOnExceptionHandlerViewModelMedIntranetGuiSystemExceptionHvisBudgetkontogruppeViewModelIkkeFindes()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IBudgetkontoModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IBudgetkontoModel>();
                    mock.Expect(m => m.Kontogruppe)
                        .Return(fixture.Create<int>())
                        .Repeat.Any();
                    return mock;
                }));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(fixture.Create<int>())
                        .Repeat.Any();
                    return mock;
                }));

            var dependencyCommandMock = MockRepository.GenerateMock<ITaskableCommand>();
            dependencyCommandMock.Expect(m => m.CanExecute(Arg<IRegnskabViewModel>.Is.TypeOf))
                                 .Return(false)
                                 .Repeat.Any();

            var budgetkontoModelMock = fixture.Create<IBudgetkontoModel>();
            Func<IBudgetkontoModel> budgetkontoModelGetter = () => budgetkontoModelMock;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.BudgetkontoGetAsync(Arg<int>.Is.GreaterThan(0), Arg<string>.Is.NotNull, Arg<DateTime>.Is.GreaterThan(DateTime.MinValue)))
                                       .Return(Task.Run(budgetkontoModelGetter))
                                       .Repeat.Any();

            var exceptionHandlerMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerMock.Expect(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf))
                                .WhenCalled(e =>
                                    {
                                        var exception = (IntranetGuiSystemException) e.Arguments.ElementAt(0);
                                        Assert.That(exception, Is.Not.Null);
                                        Assert.That(exception.Message, Is.Not.Null);
                                        Assert.That(exception.Message, Is.Not.Empty);
                                        Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.BudgetAccountGroupNotFound, budgetkontoModelMock.Kontogruppe)));
                                        Assert.That(exception.InnerException, Is.Null);
                                    })
                                .Repeat.Any();

            var regnskabsnummer = fixture.Create<int>();
            var budgetkontogruppeViewModelMockCollection = fixture.CreateMany<IBudgetkontogruppeViewModel>(7).ToList();
            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(regnskabsnummer)
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Budgetkontogrupper)
                                 .Return(budgetkontogruppeViewModelMockCollection)
                                 .Repeat.Any();

            var kontonummer = fixture.Create<string>();
            var statusDato = fixture.Create<DateTime>();
            var budgetkontoViewModelMock = MockRepository.GenerateMock<IBudgetkontoViewModel>();
            budgetkontoViewModelMock.Expect(m => m.Regnskab)
                                    .Return(regnskabViewModelMock)
                                    .Repeat.Any();
            budgetkontoViewModelMock.Expect(m => m.Kontonummer)
                                    .Return(kontonummer)
                                    .Repeat.Any();
            budgetkontoViewModelMock.Expect(m => m.StatusDato)
                                    .Return(statusDato)
                                    .Repeat.Any();

            var command = new BudgetkontoGetCommand(dependencyCommandMock, finansstyringRepositoryMock, exceptionHandlerMock);
            Assert.That(command, Is.Not.Null);
            Assert.That(command.ExecuteTask, Is.Null);

            Action action = () =>
                {
                    command.Execute(budgetkontoViewModelMock);
                    Assert.That(command.ExecuteTask, Is.Not.Null);
                    command.ExecuteTask.Wait();
                };
            Task.Run(action).Wait(3000);

            budgetkontoViewModelMock.AssertWasCalled(m => m.Regnskab);
            dependencyCommandMock.AssertWasCalled(m => m.CanExecute(Arg<IRegnskabViewModel>.Is.Equal(regnskabViewModelMock)));
            dependencyCommandMock.AssertWasNotCalled(m => m.Execute(Arg<object>.Is.Anything));
            dependencyCommandMock.AssertWasNotCalled(m => m.ExecuteTask);
            finansstyringRepositoryMock.AssertWasCalled(m => m.BudgetkontoGetAsync(Arg<int>.Is.Equal(regnskabsnummer), Arg<string>.Is.Equal(kontonummer), Arg<DateTime>.Is.Equal(statusDato)));
            regnskabViewModelMock.AssertWasCalled(m => m.Budgetkontogrupper);
            budgetkontoViewModelMock.AssertWasNotCalled(m => m.Kontonavn = Arg<string>.Is.Anything);
            budgetkontoViewModelMock.AssertWasNotCalled(m => m.Beskrivelse = Arg<string>.Is.Anything);
            budgetkontoViewModelMock.AssertWasNotCalled(m => m.Notat = Arg<string>.Is.Anything);
            budgetkontoViewModelMock.AssertWasNotCalled(m => m.Kontogruppe = Arg<IBudgetkontogruppeViewModel>.Is.Anything);
            budgetkontoViewModelMock.AssertWasNotCalled(m => m.StatusDato = Arg<DateTime>.Is.Anything);
            budgetkontoViewModelMock.AssertWasNotCalled(m => m.Indtægter = Arg<decimal>.Is.Anything);
            budgetkontoViewModelMock.AssertWasNotCalled(m => m.Udgifter = Arg<decimal>.Is.Anything);
            budgetkontoViewModelMock.AssertWasNotCalled(m => m.BudgetSidsteMåned = Arg<decimal>.Is.Anything);
            budgetkontoViewModelMock.AssertWasNotCalled(m => m.BudgetÅrTilDato = Arg<decimal>.Is.Anything);
            budgetkontoViewModelMock.AssertWasNotCalled(m => m.BudgetSidsteÅr = Arg<decimal>.Is.Anything);
            budgetkontoViewModelMock.AssertWasNotCalled(m => m.Bogført = Arg<decimal>.Is.Anything);
            budgetkontoViewModelMock.AssertWasNotCalled(m => m.BogførtSidsteMåned = Arg<decimal>.Is.Anything);
            budgetkontoViewModelMock.AssertWasNotCalled(m => m.BogførtÅrTilDato = Arg<decimal>.Is.Anything);
            budgetkontoViewModelMock.AssertWasNotCalled(m => m.BogførtSidsteÅr = Arg<decimal>.Is.Anything);
            exceptionHandlerMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at Execute kalder HandleException på exceptionhandleren ved exceptions.
        /// </summary>
        [Test]
        public void TestAtExecuteKalderHandleExceptionOnExceptionHandlerViewModelVedExceptions()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(fixture.Create<int>())
                        .Repeat.Any();
                    return mock;
                }));

            var dependencyCommandMock = MockRepository.GenerateMock<ITaskableCommand>();
            dependencyCommandMock.Expect(m => m.CanExecute(Arg<IRegnskabViewModel>.Is.TypeOf))
                                 .Return(false)
                                 .Repeat.Any();

            var exception = fixture.Create<IntranetGuiRepositoryException>();
            Func<IBudgetkontoModel> budgetkontoModelGetter = () =>
                {
                    throw exception;
                };
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.BudgetkontoGetAsync(Arg<int>.Is.GreaterThan(0), Arg<string>.Is.NotNull, Arg<DateTime>.Is.GreaterThan(DateTime.MinValue)))
                                       .Return(Task.Run(budgetkontoModelGetter))
                                       .Repeat.Any();

            var exceptionHandlerMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var regnskabsnummer = fixture.Create<int>();
            var budgetkontogruppeViewModelMockCollection = fixture.CreateMany<IBudgetkontogruppeViewModel>(7).ToList();
            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(regnskabsnummer)
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Budgetkontogrupper)
                                 .Return(budgetkontogruppeViewModelMockCollection)
                                 .Repeat.Any();

            var kontonummer = fixture.Create<string>();
            var statusDato = fixture.Create<DateTime>();
            var budgetkontoViewModelMock = MockRepository.GenerateMock<IBudgetkontoViewModel>();
            budgetkontoViewModelMock.Expect(m => m.Regnskab)
                                    .Return(regnskabViewModelMock)
                                    .Repeat.Any();
            budgetkontoViewModelMock.Expect(m => m.Kontonummer)
                                    .Return(kontonummer)
                                    .Repeat.Any();
            budgetkontoViewModelMock.Expect(m => m.StatusDato)
                                    .Return(statusDato)
                                    .Repeat.Any();

            var command = new BudgetkontoGetCommand(dependencyCommandMock, finansstyringRepositoryMock, exceptionHandlerMock);
            Assert.That(command, Is.Not.Null);
            Assert.That(command.ExecuteTask, Is.Null);

            Action action = () =>
                {
                    command.Execute(budgetkontoViewModelMock);
                    Assert.That(command.ExecuteTask, Is.Not.Null);
                    command.ExecuteTask.Wait();
                };
            Task.Run(action).Wait(3000);

            budgetkontoViewModelMock.AssertWasCalled(m => m.Regnskab);
            dependencyCommandMock.AssertWasCalled(m => m.CanExecute(Arg<IRegnskabViewModel>.Is.Equal(regnskabViewModelMock)));
            dependencyCommandMock.AssertWasNotCalled(m => m.Execute(Arg<object>.Is.Anything));
            dependencyCommandMock.AssertWasNotCalled(m => m.ExecuteTask);
            finansstyringRepositoryMock.AssertWasCalled(m => m.BudgetkontoGetAsync(Arg<int>.Is.Equal(regnskabsnummer), Arg<string>.Is.Equal(kontonummer), Arg<DateTime>.Is.Equal(statusDato)));
            exceptionHandlerMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.Equal(exception)));
        }
    }
}
