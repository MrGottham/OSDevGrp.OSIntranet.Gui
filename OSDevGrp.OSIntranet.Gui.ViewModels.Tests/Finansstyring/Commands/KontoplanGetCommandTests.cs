using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoFixture;
using AutoFixture.Kernel;
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
    /// Tester kommandoen, der kan hente og opdaterer kontoplanen til et givent regnskab.
    /// </summary>
    [TestFixture]
    public class KontoplanGetCommandTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en kommando, der kan hente og opdaterer kontoplanen til et givent regnskab.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererKontoplanGetCommand()
        {
            var fixture = new Fixture();
            fixture.Customize<ITaskableCommand>(e => e.FromFactory(() => MockRepository.GenerateMock<ITaskableCommand>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var command = new KontoplanGetCommand(fixture.Create<ITaskableCommand>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
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

            var exception = Assert.Throws<ArgumentNullException>(() => new KontoplanGetCommand(null, fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>()));
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

            var exception = Assert.Throws<ArgumentNullException>(() => new KontoplanGetCommand(fixture.Create<ITaskableCommand>(), null, fixture.Create<IExceptionHandlerViewModel>()));
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

            var exception = Assert.Throws<ArgumentNullException>(() => new KontoplanGetCommand(fixture.Create<ITaskableCommand>(), fixture.Create<IFinansstyringRepository>(), null));
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

            var command = new KontoplanGetCommand(fixture.Create<ITaskableCommand>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
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

            Func<IEnumerable<IKontoModel>> kontoplanGetter = () => new List<IKontoModel>(0);
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.KontoplanGetAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue)))
                                       .Return(Task.Run(kontoplanGetter))
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
            regnskabViewModelMock.Expect(m => m.Konti)
                                 .Return(new List<IKontoViewModel>(0))
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Kontogrupper)
                                 .Return(new List<IKontogruppeViewModel>(0))
                                 .Repeat.Any();

            var command = new KontoplanGetCommand(dependencyCommandMock, finansstyringRepositoryMock, exceptionHandlerMock);
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
            finansstyringRepositoryMock.AssertWasCalled(m => m.KontoplanGetAsync(Arg<int>.Is.Equal(regnskabsnummer), Arg<DateTime>.Is.Equal(statusDato)));
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

            Func<IEnumerable<IKontoModel>> kontoplanGetter = () => new List<IKontoModel>(0);
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.KontoplanGetAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue)))
                                       .Return(Task.Run(kontoplanGetter))
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
            regnskabViewModelMock.Expect(m => m.Konti)
                                 .Return(new List<IKontoViewModel>(0))
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Kontogrupper)
                                 .Return(new List<IKontogruppeViewModel>(0))
                                 .Repeat.Any();

            var command = new KontoplanGetCommand(dependencyCommandMock, finansstyringRepositoryMock, exceptionHandlerMock);
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
            finansstyringRepositoryMock.AssertWasCalled(m => m.KontoplanGetAsync(Arg<int>.Is.Equal(regnskabsnummer), Arg<DateTime>.Is.Equal(statusDato)));
            exceptionHandlerMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Execute ignorerer en konto med ukendt kontogruppe.
        /// </summary>
        [Test]
        public void TestAtExecuteIgnorererKontoMedUkendtKontogruppe()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var kontogruppeViewModelMock = MockRepository.GenerateMock<IKontogruppeViewModel>();
                    kontogruppeViewModelMock.Expect(m => m.Nummer)
                                            .Return(fixture.Create<int>())
                                            .Repeat.Any();
                    return kontogruppeViewModelMock;
                }));

            var dependencyCommandMock = MockRepository.GenerateMock<ITaskableCommand>();
            dependencyCommandMock.Expect(m => m.CanExecute(Arg<IRegnskabViewModel>.Is.TypeOf))
                                 .Return(false)
                                 .Repeat.Any();

            var kontogruppeViewModelMockCollection = fixture.CreateMany<IKontogruppeViewModel>(7).ToList();
            var kontoModelMockCollection = GetKontoModels(fixture, new List<IKontogruppeViewModel>(0), new Random(DateTime.Now.Second), 1);
            Func<IEnumerable<IKontoModel>> kontoplanGetter = () => kontoModelMockCollection;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.KontoplanGetAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue)))
                                       .Return(Task.Run(kontoplanGetter))
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
            regnskabViewModelMock.Expect(m => m.Konti)
                                 .Return(new List<IKontoViewModel>(0))
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Kontogrupper)
                                 .Return(kontogruppeViewModelMockCollection)
                                 .Repeat.Any();

            var command = new KontoplanGetCommand(dependencyCommandMock, finansstyringRepositoryMock, exceptionHandlerMock);
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
            finansstyringRepositoryMock.AssertWasCalled(m => m.KontoplanGetAsync(Arg<int>.Is.Equal(regnskabsnummer), Arg<DateTime>.Is.Equal(statusDato)));
            regnskabViewModelMock.AssertWasCalled(m => m.Kontogrupper);
            regnskabViewModelMock.AssertWasCalled(m => m.Konti, opt => opt.Repeat.Times(1));
            regnskabViewModelMock.AssertWasNotCalled(m => m.KontoAdd(Arg<IKontoViewModel>.Is.Anything));
            exceptionHandlerMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Execute tilføjer nye konti til regnskabet.
        /// </summary>
        [Test]
        public void TestAtExecuteAddsNyeKontiTilRegnskabViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var kontogruppeViewModelMock = MockRepository.GenerateMock<IKontogruppeViewModel>();
                    kontogruppeViewModelMock.Expect(m => m.Nummer)
                                            .Return(fixture.Create<int>())
                                            .Repeat.Any();
                    return kontogruppeViewModelMock;
                }));

            var dependencyCommandMock = MockRepository.GenerateMock<ITaskableCommand>();
            dependencyCommandMock.Expect(m => m.CanExecute(Arg<IRegnskabViewModel>.Is.TypeOf))
                                 .Return(false)
                                 .Repeat.Any();

            var kontogruppeViewModelMockCollection = fixture.CreateMany<IKontogruppeViewModel>(7).ToList();
            var kontoModelMockCollection = GetKontoModels(fixture, kontogruppeViewModelMockCollection, new Random(DateTime.Now.Second), 250);
            Func<IEnumerable<IKontoModel>> kontoplanGetter = () => kontoModelMockCollection;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.KontoplanGetAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue)))
                                       .Return(Task.Run(kontoplanGetter))
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
            regnskabViewModelMock.Expect(m => m.Konti)
                                 .Return(new List<IKontoViewModel>(0))
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Kontogrupper)
                                 .Return(kontogruppeViewModelMockCollection)
                                 .Repeat.Any();

            var command = new KontoplanGetCommand(dependencyCommandMock, finansstyringRepositoryMock, exceptionHandlerMock);
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
            finansstyringRepositoryMock.AssertWasCalled(m => m.KontoplanGetAsync(Arg<int>.Is.Equal(regnskabsnummer), Arg<DateTime>.Is.Equal(statusDato)));
            regnskabViewModelMock.AssertWasCalled(m => m.Kontogrupper);
            regnskabViewModelMock.AssertWasCalled(m => m.Konti, opt => opt.Repeat.Times(kontoModelMockCollection.Count() + 1));
            regnskabViewModelMock.AssertWasCalled(m => m.KontoAdd(Arg<IKontoViewModel>.Is.NotNull), opt => opt.Repeat.Times(kontoModelMockCollection.Count()));
            exceptionHandlerMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Execute opdaterer en eksisterende konto på regnskabet.
        /// </summary>
        [Test]
        public void TestAtExecuteOpdatererEksisterendeKontoOnRegnskabViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var kontogruppeViewModelMock = MockRepository.GenerateMock<IKontogruppeViewModel>();
                    kontogruppeViewModelMock.Expect(m => m.Nummer)
                                            .Return(fixture.Create<int>())
                                            .Repeat.Any();
                    return kontogruppeViewModelMock;
                }));

            var dependencyCommandMock = MockRepository.GenerateMock<ITaskableCommand>();
            dependencyCommandMock.Expect(m => m.CanExecute(Arg<IRegnskabViewModel>.Is.TypeOf))
                                 .Return(false)
                                 .Repeat.Any();

            var kontogruppeViewModelMockCollection = fixture.CreateMany<IKontogruppeViewModel>(7).ToList();
            var kontoModelMockCollection = GetKontoModels(fixture, kontogruppeViewModelMockCollection, new Random(DateTime.Now.Second), 1).ToArray();
            Func<IEnumerable<IKontoModel>> kontoplanGetter = () => kontoModelMockCollection;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.KontoplanGetAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue)))
                                       .Return(Task.Run(kontoplanGetter))
                                       .Repeat.Any();

            var exceptionHandlerMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var kontoViewModelMock = MockRepository.GenerateMock<IKontoViewModel>();
            kontoViewModelMock.Expect(m => m.Kontonummer)
                              .Return(kontoModelMockCollection.ElementAt(0).Kontonummer)
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
            regnskabViewModelMock.Expect(m => m.Konti)
                                 .Return(new List<IKontoViewModel> {kontoViewModelMock})
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Kontogrupper)
                                 .Return(kontogruppeViewModelMockCollection)
                                 .Repeat.Any();

            var command = new KontoplanGetCommand(dependencyCommandMock, finansstyringRepositoryMock, exceptionHandlerMock);
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
            finansstyringRepositoryMock.AssertWasCalled(m => m.KontoplanGetAsync(Arg<int>.Is.Equal(regnskabsnummer), Arg<DateTime>.Is.Equal(statusDato)));
            regnskabViewModelMock.AssertWasCalled(m => m.Kontogrupper);
            regnskabViewModelMock.AssertWasCalled(m => m.Konti, opt => opt.Repeat.Times(kontoModelMockCollection.Count() + 1));
            regnskabViewModelMock.AssertWasNotCalled(m => m.KontoAdd(Arg<IKontoViewModel>.Is.Anything));
            kontoViewModelMock.AssertWasCalled(m => m.Kontonavn = Arg<string>.Is.Equal(kontoModelMockCollection.ElementAt(0).Kontonavn));
            kontoViewModelMock.AssertWasCalled(m => m.Beskrivelse = Arg<string>.Is.Equal(kontoModelMockCollection.ElementAt(0).Beskrivelse));
            kontoViewModelMock.AssertWasCalled(m => m.Notat = Arg<string>.Is.Equal(kontoModelMockCollection.ElementAt(0).Notat));
            kontoViewModelMock.AssertWasCalled(m => m.Kontogruppe = Arg<IKontogruppeViewModel>.Is.NotNull);
            kontoViewModelMock.AssertWasCalled(m => m.StatusDato = Arg<DateTime>.Is.Equal(kontoModelMockCollection.ElementAt(0).StatusDato));
            kontoViewModelMock.AssertWasCalled(m => m.Kredit = Arg<decimal>.Is.Equal(kontoModelMockCollection.ElementAt(0).Kredit));
            kontoViewModelMock.AssertWasCalled(m => m.Saldo = Arg<decimal>.Is.Equal(kontoModelMockCollection.ElementAt(0).Saldo));
            exceptionHandlerMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Execute udfører RefreshCommand på konti, som ikke er blevet hentet.
        /// </summary>
        [Test]
        public void TestAtExecuteExecutesRefreshCommandOnKontiSomIkkeErHentet()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var kontogruppeViewModelMock = MockRepository.GenerateMock<IKontogruppeViewModel>();
                    kontogruppeViewModelMock.Expect(m => m.Nummer)
                                            .Return(fixture.Create<int>())
                                            .Repeat.Any();
                    return kontogruppeViewModelMock;
                }));

            var dependencyCommandMock = MockRepository.GenerateMock<ITaskableCommand>();
            dependencyCommandMock.Expect(m => m.CanExecute(Arg<IRegnskabViewModel>.Is.TypeOf))
                                 .Return(false)
                                 .Repeat.Any();

            var kontogruppeViewModelMockCollection = fixture.CreateMany<IKontogruppeViewModel>(7).ToList();
            var kontoModelMockCollection = GetKontoModels(fixture, kontogruppeViewModelMockCollection, new Random(DateTime.Now.Second), 1).ToArray();
            Func<IEnumerable<IKontoModel>> kontoplanGetter = () => kontoModelMockCollection;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.KontoplanGetAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue)))
                                       .Return(Task.Run(kontoplanGetter))
                                       .Repeat.Any();

            var exceptionHandlerMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var refreshCommandMock = MockRepository.GenerateMock<ICommand>();
            refreshCommandMock.Expect(m => m.CanExecute(Arg<object>.Is.NotNull))
                              .Return(true)
                              .Repeat.Any();

            var kontoViewModelMock = MockRepository.GenerateMock<IKontoViewModel>();
            kontoViewModelMock.Expect(m => m.Kontonummer)
                              .Return(kontoModelMockCollection.ElementAt(0).Kontonummer)
                              .Repeat.Any();

            var unreadedKontoViewModelMock = MockRepository.GenerateMock<IKontoViewModel>();
            unreadedKontoViewModelMock.Expect(m => m.Kontonummer)
                                      .Return(fixture.Create<string>())
                                      .Repeat.Any();
            unreadedKontoViewModelMock.Expect(m => m.RefreshCommand)
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
            regnskabViewModelMock.Expect(m => m.Konti)
                                 .Return(new List<IKontoViewModel> {kontoViewModelMock, unreadedKontoViewModelMock})
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Kontogrupper)
                                 .Return(kontogruppeViewModelMockCollection)
                                 .Repeat.Any();

            var command = new KontoplanGetCommand(dependencyCommandMock, finansstyringRepositoryMock, exceptionHandlerMock);
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
            finansstyringRepositoryMock.AssertWasCalled(m => m.KontoplanGetAsync(Arg<int>.Is.Equal(regnskabsnummer), Arg<DateTime>.Is.Equal(statusDato)));
            regnskabViewModelMock.AssertWasCalled(m => m.Kontogrupper);
            regnskabViewModelMock.AssertWasCalled(m => m.Konti, opt => opt.Repeat.Times(kontoModelMockCollection.Count() + 1));
            regnskabViewModelMock.AssertWasNotCalled(m => m.KontoAdd(Arg<IKontoViewModel>.Is.Anything));
            unreadedKontoViewModelMock.AssertWasCalled(m => m.RefreshCommand);
            refreshCommandMock.AssertWasCalled(m => m.CanExecute(Arg<object>.Is.Equal(unreadedKontoViewModelMock)));
            refreshCommandMock.AssertWasCalled(m => m.Execute(Arg<object>.Is.Equal(unreadedKontoViewModelMock)));
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
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var kontogruppeViewModelMock = MockRepository.GenerateMock<IKontogruppeViewModel>();
                    kontogruppeViewModelMock.Expect(m => m.Nummer)
                                            .Return(fixture.Create<int>())
                                            .Repeat.Any();
                    return kontogruppeViewModelMock;
                }));

            var dependencyCommandMock = MockRepository.GenerateMock<ITaskableCommand>();
            dependencyCommandMock.Expect(m => m.CanExecute(Arg<IRegnskabViewModel>.Is.TypeOf))
                                 .Return(false)
                                 .Repeat.Any();

            var kontogruppeViewModelMockCollection = fixture.CreateMany<IKontogruppeViewModel>(7).ToList();
            var exception = fixture.Create<IntranetGuiRepositoryException>();
            Func<IEnumerable<IKontoModel>> kontoplanGetter = () =>
                {
                    throw exception;
                };
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.KontoplanGetAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue)))
                                       .Return(Task.Run(kontoplanGetter))
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
            regnskabViewModelMock.Expect(m => m.Kontogrupper)
                                 .Return(kontogruppeViewModelMockCollection)
                                 .Repeat.Any();

            var command = new KontoplanGetCommand(dependencyCommandMock, finansstyringRepositoryMock, exceptionHandlerMock);
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
            finansstyringRepositoryMock.AssertWasCalled(m => m.KontoplanGetAsync(Arg<int>.Is.Equal(regnskabsnummer), Arg<DateTime>.Is.Equal(statusDato)));
            regnskabViewModelMock.AssertWasCalled(m => m.Kontogrupper);
            regnskabViewModelMock.AssertWasNotCalled(m => m.Konti);
            exceptionHandlerMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiRepositoryException>.Is.TypeOf));
        }

        /// <summary>
        /// Danner en liste indeholdende modeller for konti.
        /// </summary>
        /// <returns>Liste indeholdende modeller for konti.</returns>
        private static IEnumerable<IKontoModel> GetKontoModels(ISpecimenBuilder fixture, IEnumerable<IKontogruppeViewModel> kontogrupper, Random random, int count)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }
            if (kontogrupper == null)
            {
                throw new ArgumentNullException("kontogrupper");
            }
            if (random == null)
            {
                throw new ArgumentNullException("random");
            }
            var kontogruppeArray = kontogrupper.ToArray();
            var result = new List<IKontoModel>(count);
            while (result.Count < count)
            {
                var kontoModelMock = MockRepository.GenerateMock<IKontoModel>();
                kontoModelMock.Expect(m => m.Kontonummer)
                              .Return(fixture.Create<string>())
                              .Repeat.Any();
                kontoModelMock.Expect(m => m.Kontonavn)
                              .Return(fixture.Create<string>())
                              .Repeat.Any();
                kontoModelMock.Expect(m => m.Beskrivelse)
                              .Return(fixture.Create<string>())
                              .Repeat.Any();
                kontoModelMock.Expect(m => m.Notat)
                              .Return(fixture.Create<string>())
                              .Repeat.Any();
                kontoModelMock.Expect(m => m.Kontogruppe)
                              .Return(kontogruppeArray.Length == 0 ? -1 : kontogruppeArray.ElementAt(random.Next(kontogruppeArray.Length - 1)).Nummer)
                              .Repeat.Any();
                kontoModelMock.Expect(m => m.StatusDato)
                              .Return(fixture.Create<DateTime>())
                              .Repeat.Any();
                kontoModelMock.Expect(m => m.Kredit)
                              .Return(fixture.Create<decimal>())
                              .Repeat.Any();
                kontoModelMock.Expect(m => m.Saldo)
                              .Return(fixture.Create<decimal>())
                              .Repeat.Any();
                result.Add(kontoModelMock);
            }
            return result;
        }
    }
}
