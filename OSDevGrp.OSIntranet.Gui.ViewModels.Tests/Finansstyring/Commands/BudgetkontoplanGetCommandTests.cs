using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Finansstyring.Commands
{
    /// <summary>
    /// Tester kommandoen, der kan hente og opdatere budgetkontoplanen til et givent regnskab.
    /// </summary>
    [TestFixture]
    public class BudgetkontoplanGetCommandTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en kommando, der kan hente og opdatere budgetkontoplanen til et givent regnskab.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererBudgetkontoplanGetCommand()
        {
            var fixture = new Fixture();
            fixture.Customize<ITaskableCommand>(e => e.FromFactory(() => MockRepository.GenerateMock<ITaskableCommand>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var command = new BudgetkontoplanGetCommand(fixture.Create<ITaskableCommand>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
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

            var exception = Assert.Throws<ArgumentNullException>(() => new BudgetkontoplanGetCommand(null, fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>()));
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

            var exception = Assert.Throws<ArgumentNullException>(() => new BudgetkontoplanGetCommand(fixture.Create<ITaskableCommand>(), null, fixture.Create<IExceptionHandlerViewModel>()));
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

            var exception = Assert.Throws<ArgumentNullException>(() => new BudgetkontoplanGetCommand(fixture.Create<ITaskableCommand>(), fixture.Create<IFinansstyringRepository>(), null));
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
            fixture.Customize<ITaskableCommand>(e => e.FromFactory(() => MockRepository.GenerateMock<ITaskableCommand>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var command = new BudgetkontoplanGetCommand(fixture.Create<ITaskableCommand>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(command, Is.Not.Null);

            var result = command.CanExecute(fixture.Create<IRegnskabViewModel>());
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

            Func<IEnumerable<IBudgetkontoModel>> budgetkontoplanGetter = () => new List<IBudgetkontoModel>(0);
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.BudgetkontoplanGetAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue)))
                                       .Return(Task.Run(budgetkontoplanGetter))
                                       .Repeat.Any();

            var exceptionHandlerMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var regnskabsnummer = fixture.Create<int>();
            var statusDato = fixture.Create<DateTime>();
            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(regnskabsnummer)
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.StatusDato)
                                 .Return(statusDato)
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Budgetkonti)
                                 .Return(new List<IBudgetkontoViewModel>(0))
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Budgetkontogrupper)
                                 .Return(new List<IBudgetkontogruppeViewModel>(0))
                                 .Repeat.Any();

            var command = new BudgetkontoplanGetCommand(dependencyCommandMock, finansstyringRepositoryMock, exceptionHandlerMock);
            Assert.That(command, Is.Not.Null);
            Assert.That(command.ExecuteTask, Is.Null);

            var mock = regnskabViewModelMock;
            Action action = () =>
                {
                    command.Execute(mock);
                    Assert.That(command.ExecuteTask, Is.Not.Null);
                    command.ExecuteTask.Wait();
                };
            Task.Run(action).Wait(3000);

            dependencyCommandMock.AssertWasCalled(m => m.CanExecute(Arg<IRegnskabViewModel>.Is.Equal(regnskabViewModelMock)));
            dependencyCommandMock.AssertWasCalled(m => m.Execute(Arg<IRegnskabViewModel>.Is.Equal(regnskabViewModelMock)));
            dependencyCommandMock.AssertWasCalled(m => m.ExecuteTask);
            finansstyringRepositoryMock.AssertWasCalled(m => m.BudgetkontoplanGetAsync(Arg<int>.Is.Equal(regnskabsnummer), Arg<DateTime>.Is.Equal(statusDato)));
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

            Func<IEnumerable<IBudgetkontoModel>> budgetkontoplanGetter = () => new List<IBudgetkontoModel>(0);
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.BudgetkontoplanGetAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue)))
                                       .Return(Task.Run(budgetkontoplanGetter))
                                       .Repeat.Any();

            var exceptionHandlerMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var regnskabsnummer = fixture.Create<int>();
            var statusDato = fixture.Create<DateTime>();
            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(regnskabsnummer)
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.StatusDato)
                                 .Return(statusDato)
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Budgetkonti)
                                 .Return(new List<IBudgetkontoViewModel>(0))
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Budgetkontogrupper)
                                 .Return(new List<IBudgetkontogruppeViewModel>(0))
                                 .Repeat.Any();

            var command = new BudgetkontoplanGetCommand(dependencyCommandMock, finansstyringRepositoryMock, exceptionHandlerMock);
            Assert.That(command, Is.Not.Null);
            Assert.That(command.ExecuteTask, Is.Null);

            var mock = regnskabViewModelMock;
            Action action = () =>
                {
                    command.Execute(mock);
                    Assert.That(command.ExecuteTask, Is.Not.Null);
                    command.ExecuteTask.Wait();
                };
            Task.Run(action).Wait(3000);

            dependencyCommandMock.AssertWasCalled(m => m.CanExecute(Arg<IRegnskabViewModel>.Is.Equal(regnskabViewModelMock)));
            dependencyCommandMock.AssertWasNotCalled(m => m.Execute(Arg<object>.Is.Anything));
            dependencyCommandMock.AssertWasNotCalled(m => m.ExecuteTask);
            finansstyringRepositoryMock.AssertWasCalled(m => m.BudgetkontoplanGetAsync(Arg<int>.Is.Equal(regnskabsnummer), Arg<DateTime>.Is.Equal(statusDato)));
            exceptionHandlerMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Execute ignorerer en budgetkonto med ukendt kontogruppe til budgetkonti.
        /// </summary>
        [Test]
        public void TestAtExecuteIgnorererBudgetkontoMedUkendtBudgetkontogruppe()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var budgetkontogruppeViewModelMock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                    budgetkontogruppeViewModelMock.Expect(m => m.Nummer)
                                                  .Return(fixture.Create<int>())
                                                  .Repeat.Any();
                    return budgetkontogruppeViewModelMock;
                }));

            var dependencyCommandMock = MockRepository.GenerateMock<ITaskableCommand>();
            dependencyCommandMock.Expect(m => m.CanExecute(Arg<IRegnskabViewModel>.Is.TypeOf))
                                 .Return(false)
                                 .Repeat.Any();

            var budgetkontogruppeViewModelMockCollection = fixture.CreateMany<IBudgetkontogruppeViewModel>(7).ToList();
            var budgetkontoModelMockCollection = GetBudgetkontoModels(fixture, new List<IBudgetkontogruppeViewModel>(0), new Random(DateTime.Now.Second), 1);
            Func<IEnumerable<IBudgetkontoModel>> budgetkontoplanGetter = () => budgetkontoModelMockCollection;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.BudgetkontoplanGetAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue)))
                                       .Return(Task.Run(budgetkontoplanGetter))
                                       .Repeat.Any();

            var exceptionHandlerMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var regnskabsnummer = fixture.Create<int>();
            var statusDato = fixture.Create<DateTime>();
            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(regnskabsnummer)
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.StatusDato)
                                 .Return(statusDato)
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Budgetkonti)
                                 .Return(new List<IBudgetkontoViewModel>(0))
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Budgetkontogrupper)
                                 .Return(budgetkontogruppeViewModelMockCollection)
                                 .Repeat.Any();

            var command = new BudgetkontoplanGetCommand(dependencyCommandMock, finansstyringRepositoryMock, exceptionHandlerMock);
            Assert.That(command, Is.Not.Null);
            Assert.That(command.ExecuteTask, Is.Null);

            var mock = regnskabViewModelMock;
            Action action = () =>
                {
                    command.Execute(mock);
                    Assert.That(command.ExecuteTask, Is.Not.Null);
                    command.ExecuteTask.Wait();
                };
            Task.Run(action).Wait(3000);

            dependencyCommandMock.AssertWasCalled(m => m.CanExecute(Arg<IRegnskabViewModel>.Is.Equal(regnskabViewModelMock)));
            dependencyCommandMock.AssertWasNotCalled(m => m.Execute(Arg<object>.Is.Anything));
            dependencyCommandMock.AssertWasNotCalled(m => m.ExecuteTask);
            finansstyringRepositoryMock.AssertWasCalled(m => m.BudgetkontoplanGetAsync(Arg<int>.Is.Equal(regnskabsnummer), Arg<DateTime>.Is.Equal(statusDato)));
            regnskabViewModelMock.AssertWasCalled(m => m.Budgetkontogrupper);
            regnskabViewModelMock.AssertWasCalled(m => m.Budgetkonti, opt => opt.Repeat.Times(1));
            regnskabViewModelMock.AssertWasNotCalled(m => m.BudgetkontoAdd(Arg<IBudgetkontoViewModel>.Is.Anything));
            exceptionHandlerMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Execute tilføjer nye budgetkonti til regnskabet.
        /// </summary>
        [Test]
        public void TestAtExecuteAddsNyeBudgetkontiTilRegnskabViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var budgetkontogruppeViewModelMock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                    budgetkontogruppeViewModelMock.Expect(m => m.Nummer)
                                                  .Return(fixture.Create<int>())
                                                  .Repeat.Any();
                    return budgetkontogruppeViewModelMock;
                }));

            var dependencyCommandMock = MockRepository.GenerateMock<ITaskableCommand>();
            dependencyCommandMock.Expect(m => m.CanExecute(Arg<IRegnskabViewModel>.Is.TypeOf))
                                 .Return(false)
                                 .Repeat.Any();

            var budgetkontogruppeViewModelMockCollection = fixture.CreateMany<IBudgetkontogruppeViewModel>(7).ToList();
            var budgetkontoModelMockCollection = GetBudgetkontoModels(fixture, budgetkontogruppeViewModelMockCollection, new Random(DateTime.Now.Second), 250);
            Func<IEnumerable<IBudgetkontoModel>> budgetkontoplanGetter = () => budgetkontoModelMockCollection;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.BudgetkontoplanGetAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue)))
                                       .Return(Task.Run(budgetkontoplanGetter))
                                       .Repeat.Any();

            var exceptionHandlerMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var regnskabsnummer = fixture.Create<int>();
            var statusDato = fixture.Create<DateTime>();
            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(regnskabsnummer)
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.StatusDato)
                                 .Return(statusDato)
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Budgetkonti)
                                 .Return(new List<IBudgetkontoViewModel>(0))
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Budgetkontogrupper)
                                 .Return(budgetkontogruppeViewModelMockCollection)
                                 .Repeat.Any();

            var command = new BudgetkontoplanGetCommand(dependencyCommandMock, finansstyringRepositoryMock, exceptionHandlerMock);
            Assert.That(command, Is.Not.Null);
            Assert.That(command.ExecuteTask, Is.Null);

            var mock = regnskabViewModelMock;
            Action action = () =>
                {
                    command.Execute(mock);
                    Assert.That(command.ExecuteTask, Is.Not.Null);
                    command.ExecuteTask.Wait();
                };
            Task.Run(action).Wait(3000);

            dependencyCommandMock.AssertWasCalled(m => m.CanExecute(Arg<IRegnskabViewModel>.Is.Equal(regnskabViewModelMock)));
            dependencyCommandMock.AssertWasNotCalled(m => m.Execute(Arg<object>.Is.Anything));
            dependencyCommandMock.AssertWasNotCalled(m => m.ExecuteTask);
            finansstyringRepositoryMock.AssertWasCalled(m => m.BudgetkontoplanGetAsync(Arg<int>.Is.Equal(regnskabsnummer), Arg<DateTime>.Is.Equal(statusDato)));
            regnskabViewModelMock.AssertWasCalled(m => m.Budgetkontogrupper);
            regnskabViewModelMock.AssertWasCalled(m => m.Budgetkonti, opt => opt.Repeat.Times(budgetkontoModelMockCollection.Count() + 1));
            regnskabViewModelMock.AssertWasCalled(m => m.BudgetkontoAdd(Arg<IBudgetkontoViewModel>.Is.NotNull), opt => opt.Repeat.Times(budgetkontoModelMockCollection.Count()));
            exceptionHandlerMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Execute opdaterer en eksisterende budgetkonto på regnskabet.
        /// </summary>
        [Test]
        public void TestAtExecuteOpdatererEksisterendeBudgetkontoOnRegnskabViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var budgetkontogruppeViewModelMock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                    budgetkontogruppeViewModelMock.Expect(m => m.Nummer)
                                                  .Return(fixture.Create<int>())
                                                  .Repeat.Any();
                    return budgetkontogruppeViewModelMock;
                }));

            var dependencyCommandMock = MockRepository.GenerateMock<ITaskableCommand>();
            dependencyCommandMock.Expect(m => m.CanExecute(Arg<IRegnskabViewModel>.Is.TypeOf))
                                 .Return(false)
                                 .Repeat.Any();

            var budgetkontogruppeViewModelMockCollection = fixture.CreateMany<IBudgetkontogruppeViewModel>(7).ToList();
            var budgetkontoModelMockCollection = GetBudgetkontoModels(fixture, budgetkontogruppeViewModelMockCollection, new Random(DateTime.Now.Second), 1).ToArray();
            Func<IEnumerable<IBudgetkontoModel>> budgetkontoplanGetter = () => budgetkontoModelMockCollection;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.BudgetkontoplanGetAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue)))
                                       .Return(Task.Run(budgetkontoplanGetter))
                                       .Repeat.Any();

            var exceptionHandlerMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var budgetkontoViewModelMock = MockRepository.GenerateMock<IBudgetkontoViewModel>();
            budgetkontoViewModelMock.Expect(m => m.Kontonummer)
                                    .Return(budgetkontoModelMockCollection.ElementAt(0).Kontonummer)
                                    .Repeat.Any();

            var regnskabsnummer = fixture.Create<int>();
            var statusDato = fixture.Create<DateTime>();
            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(regnskabsnummer)
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.StatusDato)
                                 .Return(statusDato)
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Budgetkonti)
                                 .Return(new List<IBudgetkontoViewModel> {budgetkontoViewModelMock})
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Budgetkontogrupper)
                                 .Return(budgetkontogruppeViewModelMockCollection)
                                 .Repeat.Any();

            var command = new BudgetkontoplanGetCommand(dependencyCommandMock, finansstyringRepositoryMock, exceptionHandlerMock);
            Assert.That(command, Is.Not.Null);
            Assert.That(command.ExecuteTask, Is.Null);

            var mock = regnskabViewModelMock;
            Action action = () =>
                {
                    command.Execute(mock);
                    Assert.That(command.ExecuteTask, Is.Not.Null);
                    command.ExecuteTask.Wait();
                };
            Task.Run(action).Wait(3000);

            dependencyCommandMock.AssertWasCalled(m => m.CanExecute(Arg<IRegnskabViewModel>.Is.Equal(regnskabViewModelMock)));
            dependencyCommandMock.AssertWasNotCalled(m => m.Execute(Arg<object>.Is.Anything));
            dependencyCommandMock.AssertWasNotCalled(m => m.ExecuteTask);
            finansstyringRepositoryMock.AssertWasCalled(m => m.BudgetkontoplanGetAsync(Arg<int>.Is.Equal(regnskabsnummer), Arg<DateTime>.Is.Equal(statusDato)));
            regnskabViewModelMock.AssertWasCalled(m => m.Budgetkontogrupper);
            regnskabViewModelMock.AssertWasCalled(m => m.Budgetkonti, opt => opt.Repeat.Times(budgetkontoModelMockCollection.Count() + 1));
            regnskabViewModelMock.AssertWasNotCalled(m => m.BudgetkontoAdd(Arg<IBudgetkontoViewModel>.Is.Anything));
            budgetkontoViewModelMock.AssertWasCalled(m => m.Kontonavn = Arg<string>.Is.Equal(budgetkontoModelMockCollection.ElementAt(0).Kontonavn));
            budgetkontoViewModelMock.AssertWasCalled(m => m.Beskrivelse = Arg<string>.Is.Equal(budgetkontoModelMockCollection.ElementAt(0).Beskrivelse));
            budgetkontoViewModelMock.AssertWasCalled(m => m.Notat = Arg<string>.Is.Equal(budgetkontoModelMockCollection.ElementAt(0).Notat));
            budgetkontoViewModelMock.AssertWasCalled(m => m.Kontogruppe = Arg<IBudgetkontogruppeViewModel>.Is.NotNull);
            budgetkontoViewModelMock.AssertWasCalled(m => m.StatusDato = Arg<DateTime>.Is.Equal(budgetkontoModelMockCollection.ElementAt(0).StatusDato));
            budgetkontoViewModelMock.AssertWasCalled(m => m.Indtægter = Arg<decimal>.Is.Equal(budgetkontoModelMockCollection.ElementAt(0).Indtægter));
            budgetkontoViewModelMock.AssertWasCalled(m => m.Udgifter = Arg<decimal>.Is.Equal(budgetkontoModelMockCollection.ElementAt(0).Udgifter));
            budgetkontoViewModelMock.AssertWasCalled(m => m.Bogført = Arg<decimal>.Is.Equal(budgetkontoModelMockCollection.ElementAt(0).Bogført));
            exceptionHandlerMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Execute udfører RefreshCommand på budgetkonti, som ikke er blevet hentet.
        /// </summary>
        [Test]
        public void TestAtExecuteExecutesRefreshCommandOnBudgetkontiSomIkkeErHentet()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var budgetkontogruppeViewModelMock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                    budgetkontogruppeViewModelMock.Expect(m => m.Nummer)
                                                  .Return(fixture.Create<int>())
                                                  .Repeat.Any();
                    return budgetkontogruppeViewModelMock;
                }));

            var dependencyCommandMock = MockRepository.GenerateMock<ITaskableCommand>();
            dependencyCommandMock.Expect(m => m.CanExecute(Arg<IRegnskabViewModel>.Is.TypeOf))
                                 .Return(false)
                                 .Repeat.Any();

            var budgetkontogruppeViewModelMockCollection = fixture.CreateMany<IBudgetkontogruppeViewModel>(7).ToList();
            var budgetkontoModelMockCollection = GetBudgetkontoModels(fixture, budgetkontogruppeViewModelMockCollection, new Random(DateTime.Now.Second), 1).ToArray();
            Func<IEnumerable<IBudgetkontoModel>> budgetkontoplanGetter = () => budgetkontoModelMockCollection;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.BudgetkontoplanGetAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue)))
                                       .Return(Task.Run(budgetkontoplanGetter))
                                       .Repeat.Any();

            var exceptionHandlerMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var refreshCommandMock = MockRepository.GenerateMock<ICommand>();
            refreshCommandMock.Expect(m => m.CanExecute(Arg<object>.Is.NotNull))
                              .Return(true)
                              .Repeat.Any();

            var budgetkontoViewModelMock = MockRepository.GenerateMock<IBudgetkontoViewModel>();
            budgetkontoViewModelMock.Expect(m => m.Kontonummer)
                                    .Return(budgetkontoModelMockCollection.ElementAt(0).Kontonummer)
                                    .Repeat.Any();

            var unreadedBudgetkontoViewModelMock = MockRepository.GenerateMock<IBudgetkontoViewModel>();
            unreadedBudgetkontoViewModelMock.Expect(m => m.Kontonummer)
                                            .Return(fixture.Create<string>())
                                            .Repeat.Any();
            unreadedBudgetkontoViewModelMock.Expect(m => m.RefreshCommand)
                                            .Return(refreshCommandMock)
                                            .Repeat.Any();

            var regnskabsnummer = fixture.Create<int>();
            var statusDato = fixture.Create<DateTime>();
            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(regnskabsnummer)
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.StatusDato)
                                 .Return(statusDato)
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Budgetkonti)
                                 .Return(new List<IBudgetkontoViewModel> {budgetkontoViewModelMock, unreadedBudgetkontoViewModelMock})
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Budgetkontogrupper)
                                 .Return(budgetkontogruppeViewModelMockCollection)
                                 .Repeat.Any();

            var command = new BudgetkontoplanGetCommand(dependencyCommandMock, finansstyringRepositoryMock, exceptionHandlerMock);
            Assert.That(command, Is.Not.Null);
            Assert.That(command.ExecuteTask, Is.Null);

            var mock = regnskabViewModelMock;
            Action action = () =>
                {
                    command.Execute(mock);
                    Assert.That(command.ExecuteTask, Is.Not.Null);
                    command.ExecuteTask.Wait();
                };
            Task.Run(action).Wait(3000);

            dependencyCommandMock.AssertWasCalled(m => m.CanExecute(Arg<IRegnskabViewModel>.Is.Equal(regnskabViewModelMock)));
            dependencyCommandMock.AssertWasNotCalled(m => m.Execute(Arg<object>.Is.Anything));
            dependencyCommandMock.AssertWasNotCalled(m => m.ExecuteTask);
            finansstyringRepositoryMock.AssertWasCalled(m => m.BudgetkontoplanGetAsync(Arg<int>.Is.Equal(regnskabsnummer), Arg<DateTime>.Is.Equal(statusDato)));
            regnskabViewModelMock.AssertWasCalled(m => m.Budgetkontogrupper);
            regnskabViewModelMock.AssertWasCalled(m => m.Budgetkonti, opt => opt.Repeat.Times(budgetkontoModelMockCollection.Count() + 1));
            regnskabViewModelMock.AssertWasNotCalled(m => m.BudgetkontoAdd(Arg<IBudgetkontoViewModel>.Is.Anything));
            unreadedBudgetkontoViewModelMock.AssertWasCalled(m => m.RefreshCommand);
            refreshCommandMock.AssertWasCalled(m => m.CanExecute(Arg<object>.Is.Equal(unreadedBudgetkontoViewModelMock)));
            refreshCommandMock.AssertWasCalled(m => m.Execute(Arg<object>.Is.Equal(unreadedBudgetkontoViewModelMock)));
            exceptionHandlerMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
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
                    var budgetkontogruppeViewModelMock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                    budgetkontogruppeViewModelMock.Expect(m => m.Nummer)
                                                  .Return(fixture.Create<int>())
                                                  .Repeat.Any();
                    return budgetkontogruppeViewModelMock;
                }));

            var dependencyCommandMock = MockRepository.GenerateMock<ITaskableCommand>();
            dependencyCommandMock.Expect(m => m.CanExecute(Arg<IRegnskabViewModel>.Is.TypeOf))
                                 .Return(false)
                                 .Repeat.Any();

            var budgetkontogruppeViewModelMockCollection = fixture.CreateMany<IBudgetkontogruppeViewModel>(7).ToList();
            var exception = fixture.Create<IntranetGuiRepositoryException>();
            Func<IEnumerable<IBudgetkontoModel>> budgetkontoplanGetter = () =>
                {
                    throw exception;
                };
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.BudgetkontoplanGetAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue)))
                                       .Return(Task.Run(budgetkontoplanGetter))
                                       .Repeat.Any();

            var exceptionHandlerMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var regnskabsnummer = fixture.Create<int>();
            var statusDato = fixture.Create<DateTime>();
            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(regnskabsnummer)
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.StatusDato)
                                 .Return(statusDato)
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Budgetkontogrupper)
                                 .Return(budgetkontogruppeViewModelMockCollection)
                                 .Repeat.Any();

            var command = new BudgetkontoplanGetCommand(dependencyCommandMock, finansstyringRepositoryMock, exceptionHandlerMock);
            Assert.That(command, Is.Not.Null);
            Assert.That(command.ExecuteTask, Is.Null);

            var mock = regnskabViewModelMock;
            Action action = () =>
                {
                    command.Execute(mock);
                    Assert.That(command.ExecuteTask, Is.Not.Null);
                    command.ExecuteTask.Wait();
                };
            Task.Run(action).Wait(3000);

            dependencyCommandMock.AssertWasCalled(m => m.CanExecute(Arg<IRegnskabViewModel>.Is.Equal(regnskabViewModelMock)));
            dependencyCommandMock.AssertWasNotCalled(m => m.Execute(Arg<object>.Is.Anything));
            dependencyCommandMock.AssertWasNotCalled(m => m.ExecuteTask);
            finansstyringRepositoryMock.AssertWasCalled(m => m.BudgetkontoplanGetAsync(Arg<int>.Is.Equal(regnskabsnummer), Arg<DateTime>.Is.Equal(statusDato)));
            regnskabViewModelMock.AssertWasCalled(m => m.Budgetkontogrupper);
            regnskabViewModelMock.AssertWasNotCalled(m => m.Budgetkonti);
            exceptionHandlerMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiRepositoryException>.Is.TypeOf));
        }

        /// <summary>
        /// Danner en liste indeholdende modeller for budgetkonti.
        /// </summary>
        /// <returns>Liste indeholdende modeller for budgetkonti.</returns>
        private static IEnumerable<IBudgetkontoModel> GetBudgetkontoModels(ISpecimenBuilder fixture, IEnumerable<IBudgetkontogruppeViewModel> budgetkontogrupper, Random random, int count)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }
            if (budgetkontogrupper == null)
            {
                throw new ArgumentNullException("budgetkontogrupper");
            }
            if (random == null)
            {
                throw new ArgumentNullException("random");
            }
            var budgetkontogruppeArray = budgetkontogrupper.ToArray();
            var result = new List<IBudgetkontoModel>(count);
            while (result.Count < count)
            {
                var budgetkontoModelMock = MockRepository.GenerateMock<IBudgetkontoModel>();
                budgetkontoModelMock.Expect(m => m.Kontonummer)
                                    .Return(fixture.Create<string>())
                                    .Repeat.Any();
                budgetkontoModelMock.Expect(m => m.Kontonavn)
                                    .Return(fixture.Create<string>())
                                    .Repeat.Any();
                budgetkontoModelMock.Expect(m => m.Beskrivelse)
                                    .Return(fixture.Create<string>())
                                    .Repeat.Any();
                budgetkontoModelMock.Expect(m => m.Notat)
                                    .Return(fixture.Create<string>())
                                    .Repeat.Any();
                budgetkontoModelMock.Expect(m => m.Kontogruppe)
                                    .Return(budgetkontogruppeArray.Length == 0 ? -1 : budgetkontogruppeArray.ElementAt(random.Next(budgetkontogruppeArray.Length - 1)).Nummer)
                                    .Repeat.Any();
                budgetkontoModelMock.Expect(m => m.StatusDato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
                budgetkontoModelMock.Expect(m => m.Indtægter)
                                    .Return(fixture.Create<decimal>())
                                    .Repeat.Any();
                budgetkontoModelMock.Expect(m => m.Udgifter)
                                    .Return(fixture.Create<decimal>())
                                    .Repeat.Any();
                budgetkontoModelMock.Expect(m => m.Bogført)
                                    .Return(fixture.Create<decimal>())
                                    .Repeat.Any();
                result.Add(budgetkontoModelMock);
            }
            return result;
        }
    }
}
