using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
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
    /// Tester kommando, der kan hente kontogrupper til et regnskab.
    /// </summary>
    [TestFixture]
    public class KontogrupperGetCommandTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en kommando, der kan hente kontogrupper til et regnskab.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererKontogrupperGetCommand()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var command = new KontogrupperGetCommand(fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
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

            var exception = Assert.Throws<ArgumentNullException>(() => new KontogrupperGetCommand(null, fixture.Create<IExceptionHandlerViewModel>()));
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

            var exception = Assert.Throws<ArgumentNullException>(() => new KontogrupperGetCommand(fixture.Create<IFinansstyringRepository>(), null));
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

            var command = new KontogrupperGetCommand(fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(command, Is.Not.Null);

            var result = command.CanExecute(fixture.Create<IRegnskabViewModel>());
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tester, at Execute returnerer, hvis kontogrupper allerede er hentet.
        /// </summary>
        [Test]
        public void TestAtExecuteReturnererHvisKontogrupperErHentet()
        {
            var fixture = new Fixture();
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeViewModel>()));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IRegnskabViewModel>();
                    mock.Expect(m => m.Kontogrupper)
                        .Return(fixture.CreateMany<IKontogruppeViewModel>(7).ToList())
                        .Repeat.Any();
                    return mock;
                }));

            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var regnskabViewModelMock = fixture.Create<IRegnskabViewModel>();

            var command = new KontogrupperGetCommand(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);
            Assert.That(command.ExecuteTask, Is.Null);

            Action action = () =>
                {
                    command.Execute(regnskabViewModelMock);
                    Assert.That(command.ExecuteTask, Is.Null);
                };
            Task.Run(action).Wait(3000);

            regnskabViewModelMock.AssertWasCalled(m => m.Kontogrupper);
            finansstyringRepositoryMock.AssertWasNotCalled(m => m.KontogruppelisteGetAsync());
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Execute henter kontogrupper til regnskabet.
        /// </summary>
        [Test]
        public void TestAtExecuteHenterKontogrupper()
        {
            var fixture = new Fixture();
            fixture.Customize<IKontogruppeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeModel>()));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IRegnskabViewModel>();
                    mock.Expect(m => m.Kontogrupper)
                        .Return(new List<IKontogruppeViewModel>(0))
                        .Repeat.Any();
                    return mock;
                }));

            var kontogruppeModelMockCollection = fixture.CreateMany<IKontogruppeModel>(7).ToList();
            Func<IEnumerable<IKontogruppeModel>> kontogruppelisteGetter = () => kontogruppeModelMockCollection;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.KontogruppelisteGetAsync())
                                       .Return(Task.Run(kontogruppelisteGetter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var regnskabViewModelMock = fixture.Create<IRegnskabViewModel>();

            var command = new KontogrupperGetCommand(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);
            Assert.That(command.ExecuteTask, Is.Null);

            Action action = () =>
                {
                    command.Execute(regnskabViewModelMock);
                    Assert.That(command.ExecuteTask, Is.Not.Null);
                    command.ExecuteTask.Wait();
                };
            Task.Run(action).Wait(3000);

            regnskabViewModelMock.AssertWasCalled(m => m.Kontogrupper);
            finansstyringRepositoryMock.AssertWasCalled(m => m.KontogruppelisteGetAsync());
            regnskabViewModelMock.AssertWasCalled(m => m.KontogruppeAdd(Arg<IKontogruppeViewModel>.Is.NotNull), opt => opt.Repeat.Times(kontogruppeModelMockCollection.Count));
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
                    mock.Expect(m => m.Kontogrupper)
                        .Return(new List<IKontogruppeViewModel>(0))
                        .Repeat.Any();
                    return mock;
                }));

            var exception = fixture.Create<IntranetGuiRepositoryException>();
            Func<IEnumerable<IKontogruppeModel>> kontogruppelisteGetter = () =>
                {
                    throw exception;
                };
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.KontogruppelisteGetAsync())
                                       .Return(Task.Run(kontogruppelisteGetter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var regnskabViewModelMock = fixture.Create<IRegnskabViewModel>();

            var command = new KontogrupperGetCommand(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);
            Assert.That(command.ExecuteTask, Is.Null);

            Action action = () =>
                {
                    command.Execute(regnskabViewModelMock);
                    Assert.That(command.ExecuteTask, Is.Not.Null);
                    command.ExecuteTask.Wait();
                };
            Task.Run(action).Wait(3000);

            regnskabViewModelMock.AssertWasCalled(m => m.Kontogrupper);
            finansstyringRepositoryMock.AssertWasCalled(m => m.KontogruppelisteGetAsync());
            regnskabViewModelMock.AssertWasNotCalled(m => m.KontogruppeAdd(Arg<IKontogruppeViewModel>.Is.Anything));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiRepositoryException>.Is.Equal(exception)));
        }
    }
}
