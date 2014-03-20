using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Finansstyring.Commands
{
    /// <summary>
    /// Tester kommando, der kan hente og opdatere en kontoen.
    /// </summary>
    [TestFixture]
    public class KontoGetCommandTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en kommando, der kan hente og opdatere en kontoen.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererKontoGetCommand()
        {
            var fixture = new Fixture();
            fixture.Customize<ITaskableCommand>(e => e.FromFactory(() => MockRepository.GenerateMock<ITaskableCommand>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var command = new KontoGetCommand(fixture.Create<ITaskableCommand>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
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

            var exception = Assert.Throws<ArgumentNullException>(() => new KontoGetCommand(null, fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>()));
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

            var exception = Assert.Throws<ArgumentNullException>(() => new KontoGetCommand(fixture.Create<ITaskableCommand>(), null, fixture.Create<IExceptionHandlerViewModel>()));
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

            var exception = Assert.Throws<ArgumentNullException>(() => new KontoGetCommand(fixture.Create<ITaskableCommand>(), fixture.Create<IFinansstyringRepository>(), null));
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
            fixture.Customize<IKontoViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontoViewModel>()));
            fixture.Customize<ITaskableCommand>(e => e.FromFactory(() => MockRepository.GenerateMock<ITaskableCommand>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var command = new KontoGetCommand(fixture.Create<ITaskableCommand>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(command, Is.Not.Null);

            var result = command.CanExecute(fixture.Create<IKontoViewModel>());
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

            Func<IKontoModel> kontoModelGetter = () => null;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.KontoGetAsync(Arg<int>.Is.GreaterThan(0), Arg<string>.Is.NotNull, Arg<DateTime>.Is.GreaterThan(DateTime.MinValue)))
                                       .Return(Task.Run(kontoModelGetter))
                                       .Repeat.Any();

            var exceptionHandlerMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var regnskabsnummer = fixture.Create<int>();
            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(regnskabsnummer)
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Kontogrupper)
                                 .Return(new List<IKontogruppeViewModel>(0))
                                 .Repeat.Any();

            var kontonummer = fixture.Create<string>();
            var statusDato = fixture.Create<DateTime>();
            var kontoViewModelMock = MockRepository.GenerateMock<IKontoViewModel>();
            kontoViewModelMock.Expect(m => m.Regnskab)
                              .Return(regnskabViewModelMock)
                              .Repeat.Any();
            kontoViewModelMock.Expect(m => m.Kontonummer)
                              .Return(kontonummer)
                              .Repeat.Any();
            kontoViewModelMock.Expect(m => m.StatusDato)
                              .Return(statusDato)
                              .Repeat.Any();

            var command = new KontoGetCommand(dependencyCommandMock, finansstyringRepositoryMock, exceptionHandlerMock);
            Assert.That(command, Is.Not.Null);
            Assert.That(command.ExecuteTask, Is.Null);

            Action action = () =>
                {
                    command.Execute(kontoViewModelMock);
                    Assert.That(command.ExecuteTask, Is.Not.Null);
                    command.ExecuteTask.Wait();
                };
            Task.Run(action).Wait(3000);

            kontoViewModelMock.AssertWasCalled(m => m.Regnskab);
            dependencyCommandMock.AssertWasCalled(m => m.CanExecute(Arg<IRegnskabViewModel>.Is.Equal(regnskabViewModelMock)));
            dependencyCommandMock.AssertWasCalled(m => m.Execute(Arg<IRegnskabViewModel>.Is.Equal(regnskabViewModelMock)));
            dependencyCommandMock.AssertWasCalled(m => m.ExecuteTask);
            finansstyringRepositoryMock.AssertWasCalled(m => m.KontoGetAsync(Arg<int>.Is.Equal(regnskabsnummer), Arg<string>.Is.Equal(kontonummer), Arg<DateTime>.Is.Equal(statusDato)));
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

            Func<IKontoModel> kontoModelGetter = () => null;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.KontoGetAsync(Arg<int>.Is.GreaterThan(0), Arg<string>.Is.NotNull, Arg<DateTime>.Is.GreaterThan(DateTime.MinValue)))
                                       .Return(Task.Run(kontoModelGetter))
                                       .Repeat.Any();

            var exceptionHandlerMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var regnskabsnummer = fixture.Create<int>();
            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(regnskabsnummer)
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Kontogrupper)
                                 .Return(new List<IKontogruppeViewModel>(0))
                                 .Repeat.Any();

            var kontonummer = fixture.Create<string>();
            var statusDato = fixture.Create<DateTime>();
            var kontoViewModelMock = MockRepository.GenerateMock<IKontoViewModel>();
            kontoViewModelMock.Expect(m => m.Regnskab)
                              .Return(regnskabViewModelMock)
                              .Repeat.Any();
            kontoViewModelMock.Expect(m => m.Kontonummer)
                              .Return(kontonummer)
                              .Repeat.Any();
            kontoViewModelMock.Expect(m => m.StatusDato)
                              .Return(statusDato)
                              .Repeat.Any();

            var command = new KontoGetCommand(dependencyCommandMock, finansstyringRepositoryMock, exceptionHandlerMock);
            Assert.That(command, Is.Not.Null);
            Assert.That(command.ExecuteTask, Is.Null);

            Action action = () =>
                {
                    command.Execute(kontoViewModelMock);
                    Assert.That(command.ExecuteTask, Is.Not.Null);
                    command.ExecuteTask.Wait();
                };
            Task.Run(action).Wait(3000);

            kontoViewModelMock.AssertWasCalled(m => m.Regnskab);
            dependencyCommandMock.AssertWasCalled(m => m.CanExecute(Arg<IRegnskabViewModel>.Is.Equal(regnskabViewModelMock)));
            dependencyCommandMock.AssertWasNotCalled(m => m.Execute(Arg<object>.Is.Anything));
            dependencyCommandMock.AssertWasNotCalled(m => m.ExecuteTask);
            finansstyringRepositoryMock.AssertWasCalled(m => m.KontoGetAsync(Arg<int>.Is.Equal(regnskabsnummer), Arg<string>.Is.Equal(kontonummer), Arg<DateTime>.Is.Equal(statusDato)));
            exceptionHandlerMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Execute opdaterer ViewModel for kontoen.
        /// </summary>
        [Test]
        public void TestAtExecuteOpdatererKontoViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IKontoModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IKontoModel>();
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
                    return mock;
                }));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IKontogruppeViewModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(fixture.Create<int>())
                        .Repeat.Any();
                    return mock;
                }));

            var dependencyCommandMock = MockRepository.GenerateMock<ITaskableCommand>();
            dependencyCommandMock.Expect(m => m.CanExecute(Arg<IRegnskabViewModel>.Is.TypeOf))
                                 .Return(false)
                                 .Repeat.Any();

            var kontoModelMock = fixture.Create<IKontoModel>();
            Func<IKontoModel> kontoModelGetter = () => kontoModelMock;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.KontoGetAsync(Arg<int>.Is.GreaterThan(0), Arg<string>.Is.NotNull, Arg<DateTime>.Is.GreaterThan(DateTime.MinValue)))
                                       .Return(Task.Run(kontoModelGetter))
                                       .Repeat.Any();

            var exceptionHandlerMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var regnskabsnummer = fixture.Create<int>();
            var kontogruppeViewModelMockCollection = fixture.CreateMany<IKontogruppeViewModel>(6).ToList();
            var kontogruppeViewModelMock = MockRepository.GenerateMock<IKontogruppeViewModel>();
            kontogruppeViewModelMock.Expect(m => m.Nummer)
                                    .Return(kontoModelMock.Kontogruppe)
                                    .Repeat.Any();
            kontogruppeViewModelMockCollection.Add(kontogruppeViewModelMock);
            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(regnskabsnummer)
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Kontogrupper)
                                 .Return(kontogruppeViewModelMockCollection)
                                 .Repeat.Any();

            var kontonummer = fixture.Create<string>();
            var statusDato = fixture.Create<DateTime>();
            var kontoViewModelMock = MockRepository.GenerateMock<IKontoViewModel>();
            kontoViewModelMock.Expect(m => m.Regnskab)
                              .Return(regnskabViewModelMock)
                              .Repeat.Any();
            kontoViewModelMock.Expect(m => m.Kontonummer)
                              .Return(kontonummer)
                              .Repeat.Any();
            kontoViewModelMock.Expect(m => m.StatusDato)
                              .Return(statusDato)
                              .Repeat.Any();

            var command = new KontoGetCommand(dependencyCommandMock, finansstyringRepositoryMock, exceptionHandlerMock);
            Assert.That(command, Is.Not.Null);
            Assert.That(command.ExecuteTask, Is.Null);

            Action action = () =>
                {
                    command.Execute(kontoViewModelMock);
                    Assert.That(command.ExecuteTask, Is.Not.Null);
                    command.ExecuteTask.Wait();
                };
            Task.Run(action).Wait(3000);

            kontoViewModelMock.AssertWasCalled(m => m.Regnskab);
            dependencyCommandMock.AssertWasCalled(m => m.CanExecute(Arg<IRegnskabViewModel>.Is.Equal(regnskabViewModelMock)));
            dependencyCommandMock.AssertWasNotCalled(m => m.Execute(Arg<object>.Is.Anything));
            dependencyCommandMock.AssertWasNotCalled(m => m.ExecuteTask);
            finansstyringRepositoryMock.AssertWasCalled(m => m.KontoGetAsync(Arg<int>.Is.Equal(regnskabsnummer), Arg<string>.Is.Equal(kontonummer), Arg<DateTime>.Is.Equal(statusDato)));

            regnskabViewModelMock.AssertWasCalled(m => m.Kontogrupper);
            kontoViewModelMock.AssertWasCalled(m => m.Kontonavn = Arg<string>.Is.Equal(kontoModelMock.Kontonavn));
            kontoViewModelMock.AssertWasCalled(m => m.Beskrivelse = Arg<string>.Is.Equal(kontoModelMock.Beskrivelse));
            kontoViewModelMock.AssertWasCalled(m => m.Notat = Arg<string>.Is.Equal(kontoModelMock.Notat));
            kontoViewModelMock.AssertWasCalled(m => m.Kontogruppe = Arg<IKontogruppeViewModel>.Is.NotNull);
            kontoViewModelMock.AssertWasCalled(m => m.StatusDato = Arg<DateTime>.Is.Equal(kontoModelMock.StatusDato));

            exceptionHandlerMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }
    }
}
