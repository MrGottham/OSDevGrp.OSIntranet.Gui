using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Finansstyring.Commands
{
    /// <summary>
    /// Tester kommando til genindlæsning af regnskabslisten.
    /// </summary>
    [TestFixture]
    public class RegnskabslisteRefreshCommandTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en kommando til genindlæsning af regnskabslisten.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererRegnskabslisteRefreshCommand()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var command = new RegnskabslisteRefreshCommand(fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(command, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis repositoryet til finansstyring er null..
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFinansstyringRepositoryErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            Assert.Throws<ArgumentNullException>(() => new RegnskabslisteRefreshCommand(null, fixture.Create<IExceptionHandlerViewModel>()));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis ViewModel til exceptionhandleren er null..
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisExceptionHandlerViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            Assert.Throws<ArgumentNullException>(() => new RegnskabslisteRefreshCommand(fixture.Create<IFinansstyringRepository>(), null));
        }

        /// <summary>
        /// Tester, at CanExecute returnerer true.
        /// </summary>
        [Test]
        public void TestAtCanExecuteReturnererTrue()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IRegnskabslisteViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabslisteViewModel>()));

            var command = new RegnskabslisteRefreshCommand(fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(command, Is.Not.Null);

            var result = command.CanExecute(fixture.Create<IRegnskabslisteViewModel>());
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tester at Execute tilføjer nye regnskaber listen af regnskaber.
        /// </summary>
        [Test]
        public void TestAtExecuteAdderNyeRegnskaberTilRegnskabslisteViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() =>
                {
                    var regnskabModelMock = MockRepository.GenerateMock<IRegnskabModel>();
                    regnskabModelMock.Expect(m => m.Nummer)
                                     .Return(fixture.Create<int>())
                                     .Repeat.Any();
                    return regnskabModelMock;
                }));

            var regnskabCollection = fixture.CreateMany<IRegnskabModel>(25).ToList();
            Func<IEnumerable<IRegnskabModel>> regnskabCollectionGetter = () => regnskabCollection;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.RegnskabslisteGetAsync())
                                       .Return(Task.Run(regnskabCollectionGetter))
                                       .Repeat.Any();

            var count = regnskabCollection.Count;
            using (var waitEvent = new AutoResetEvent(false))
            {
                var we = waitEvent;
                var regnskabslisteViewModelMock = MockRepository.GenerateMock<IRegnskabslisteViewModel>();
                regnskabslisteViewModelMock.Expect(m => m.StatusDato)
                                           .Return(fixture.Create<DateTime>())
                                           .Repeat.Any();
                regnskabslisteViewModelMock.Expect(m => m.Regnskaber)
                                           .Return(new ObservableCollection<IRegnskabViewModel>(new List<IRegnskabViewModel>()))
                                           .Repeat.Any();
                regnskabslisteViewModelMock.Expect(m => m.RegnskabAdd(Arg<IRegnskabViewModel>.Is.NotNull))
                                           .WhenCalled(e =>
                                               {
                                                   count--;
                                                   if (count == 0)
                                                   {
                                                       we.Set();
                                                   }
                                               })
                                           .Repeat.Any();

                var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

                var command = new RegnskabslisteRefreshCommand(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
                Assert.That(command, Is.Not.Null);

                command.Execute(regnskabslisteViewModelMock);
                waitEvent.WaitOne(3000);

                finansstyringRepositoryMock.AssertWasCalled(m => m.RegnskabslisteGetAsync());
                regnskabslisteViewModelMock.AssertWasCalled(m => m.StatusDato = Arg<DateTime>.Is.LessThanOrEqual(DateTime.Now));
                regnskabslisteViewModelMock.AssertWasCalled(m => m.StatusDato, opt => opt.Repeat.Times(regnskabCollection.Count));
                regnskabslisteViewModelMock.AssertWasCalled(m => m.RegnskabAdd(Arg<IRegnskabViewModel>.Is.NotNull), opt => opt.Repeat.Times(regnskabCollection.Count));
                exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
            }
        }

        /// <summary>
        /// Tester, at Execute opdaterer eksisterede regnskab i listen af regnskaber.
        /// </summary>
        [Test]
        public void TestAtExecuteOpdatererEksisteredeRegnskabIRegnskabslisteViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() =>
                {
                    var regnskabModelMock = MockRepository.GenerateMock<IRegnskabModel>();
                    regnskabModelMock.Expect(m => m.Nummer)
                                     .Return(fixture.Create<int>())
                                     .Repeat.Any();
                    return regnskabModelMock;
                }));

            var regnskabCollection = fixture.CreateMany<IRegnskabModel>(1).ToList();
            Func<IEnumerable<IRegnskabModel>> regnskabCollectionGetter = () => regnskabCollection;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.RegnskabslisteGetAsync())
                                       .Return(Task.Run(regnskabCollectionGetter))
                                       .Repeat.Any();

            using (var waitEvent = new AutoResetEvent(false))
            {
                var we = waitEvent;
                var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
                regnskabViewModelMock.Expect(m => m.Nummer)
                                     .Return(regnskabCollection.ElementAt(0).Nummer)
                                     .Repeat.Any();
                regnskabViewModelMock.Expect(m => m.Navn)
                                     .WhenCalled(e => we.Set())
                                     .Repeat.Any();
                
                var regnskabslisteViewModelMock = MockRepository.GenerateMock<IRegnskabslisteViewModel>();
                regnskabslisteViewModelMock.Expect(m => m.StatusDato)
                                           .Return(fixture.Create<DateTime>())
                                           .Repeat.Any();
                regnskabslisteViewModelMock.Expect(m => m.Regnskaber)
                                           .Return(new ObservableCollection<IRegnskabViewModel>(new List<IRegnskabViewModel> { regnskabViewModelMock }))
                                           .Repeat.Any();

                var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

                var command = new RegnskabslisteRefreshCommand(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
                Assert.That(command, Is.Not.Null);

                command.Execute(regnskabslisteViewModelMock);
                waitEvent.WaitOne(3000);

                finansstyringRepositoryMock.AssertWasCalled(m => m.RegnskabslisteGetAsync());
                regnskabslisteViewModelMock.AssertWasCalled(m => m.StatusDato = Arg<DateTime>.Is.LessThanOrEqual(DateTime.Now));
                regnskabslisteViewModelMock.AssertWasCalled(m => m.StatusDato);
                regnskabViewModelMock.AssertWasCalled(m => m.Navn = Arg<string>.Is.Equal(regnskabCollection.ElementAt(0).Navn));
                regnskabViewModelMock.AssertWasCalled(m => m.StatusDato = Arg<DateTime>.Is.Equal(regnskabslisteViewModelMock.StatusDato));
                regnskabslisteViewModelMock.AssertWasNotCalled(m => m.RegnskabAdd(Arg<IRegnskabViewModel>.Is.Anything));
                exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
            }
        }

        /// <summary>
        /// Tester, at Execute kalder OnFinish efter funktionalitet til opdatering m.m.
        /// </summary>
        [Test]
        public void TestAtExecuteKalderOnFinishEfterRefreshFunktionalitet()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            Func<IEnumerable<IRegnskabModel>> regnskabCollectionGetter = () => new List<IRegnskabModel>(0);
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.RegnskabslisteGetAsync())
                                       .Return(Task.Run(regnskabCollectionGetter))
                                       .Repeat.Any();

            using (var waitEvent = new AutoResetEvent(false))
            {
                var we = waitEvent;
                var regnskabslisteViewModelMock = MockRepository.GenerateMock<IRegnskabslisteViewModel>();
                var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

                var actionCalled = false;
                Action<IRegnskabslisteViewModel> onFinish = obj =>
                    {
                        try
                        {
                            Assert.That(obj, Is.Not.Null);
                            Assert.That(obj, Is.EqualTo(regnskabslisteViewModelMock));
                            actionCalled = true;
                        }
                        finally
                        {
                            we.Set();
                        }
                    };

                var command = new RegnskabslisteRefreshCommand(finansstyringRepositoryMock, exceptionHandlerViewModelMock, onFinish);
                Assert.That(command, Is.Not.Null);

                Assert.That(actionCalled, Is.False);
                command.Execute(regnskabslisteViewModelMock);
                waitEvent.WaitOne(3000);
                Assert.That(actionCalled, Is.True);

                finansstyringRepositoryMock.AssertWasCalled(m => m.RegnskabslisteGetAsync());
                regnskabslisteViewModelMock.AssertWasCalled(m => m.StatusDato = Arg<DateTime>.Is.LessThanOrEqual(DateTime.Now));
                regnskabslisteViewModelMock.AssertWasNotCalled(m => m.Regnskaber);
                regnskabslisteViewModelMock.AssertWasNotCalled(m => m.RegnskabAdd(Arg<IRegnskabViewModel>.Is.Anything));
                exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
            }
        }

        /// <summary>
        /// Tester, at Execute kalder exceptionhandleren med en IntranetGuiRepositoryException ved IntranetGuiRepositoryException.
        /// </summary>
        [Test]
        public void TestAtExecuteKalderExceptionHandlerViewModelMedIntranetGuiRepositoryExceptionVedIntranetGuiRepositoryException()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabslisteViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabslisteViewModel>()));

            Func<IEnumerable<IRegnskabModel>> regnskabCollectionGetter = () =>
                {
                    throw new IntranetGuiRepositoryException(fixture.Create<string>());
                };
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.RegnskabslisteGetAsync())
                                       .Return(Task.Run(regnskabCollectionGetter))
                                       .Repeat.Any();

            using (var waitEvent = new AutoResetEvent(false))
            {
                var we = waitEvent;
                var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
                exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<Exception>.Is.NotNull))
                                             .WhenCalled(e => we.Set())
                                             .Repeat.Any();

                var command = new RegnskabslisteRefreshCommand(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
                Assert.That(command, Is.Not.Null);

                command.Execute(fixture.Create<IRegnskabslisteViewModel>());
                waitEvent.WaitOne(3000);

                finansstyringRepositoryMock.AssertWasCalled(m => m.RegnskabslisteGetAsync());
                exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiRepositoryException>.Is.TypeOf));
            }
        }

        /// <summary>
        /// Tester, at ExecuteRefreshCommandOnRegnskabViewModels kaster en ArgumentNullException, hvis ViewModel for regnskaber er null.
        /// </summary>
        [Test]
        public void TestAtExecuteRefreshCommandOnRegnskabViewModelsKasterArgumentNullExceptionHvisRegnskabslisteViewModelErNull()
        {
            Assert.Throws<ArgumentNullException>(() => RegnskabslisteRefreshCommand.ExecuteRefreshCommandOnRegnskabViewModels(null));
        }

        /// <summary>
        /// Tester, at ExecuteRefreshCommandOnRegnskabViewModels udfører RefreshCommand på alle ViewModels for regnskaber i listen af regnskaber.
        /// </summary>
        [Test]
        public void TestExecuteRefreshCommandOnRegnskabViewModelsExecutesRefreshCommandOnRegnskabViewModels()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommand>(e => e.FromFactory(() =>
                {
                    var commandMock = MockRepository.GenerateMock<ICommand>();
                    commandMock.Expect(m => m.CanExecute(Arg<object>.Is.NotNull))
                               .Return(true)
                               .Repeat.Any();
                    return commandMock;
                }));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() =>
                {
                    var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
                    regnskabViewModelMock.Expect(m => m.RefreshCommand)
                                         .Return(fixture.Create<ICommand>())
                                         .Repeat.Any();
                    return regnskabViewModelMock;
                }));

            var regnskabViewModelMockCollection = fixture.CreateMany<IRegnskabViewModel>(25).ToList();
            var regnskabslisteViewModelMock = MockRepository.GenerateMock<IRegnskabslisteViewModel>();
            regnskabslisteViewModelMock.Expect(m => m.Regnskaber)
                                       .Return(regnskabViewModelMockCollection)
                                       .Repeat.Any();

            RegnskabslisteRefreshCommand.ExecuteRefreshCommandOnRegnskabViewModels(regnskabslisteViewModelMock);

            regnskabslisteViewModelMock.AssertWasCalled(m => m.Regnskaber);
            foreach (var regnskabViewModelMock in regnskabViewModelMockCollection)
            {
                var mock = regnskabViewModelMock;
                regnskabViewModelMock.AssertWasCalled(m => m.RefreshCommand);
                regnskabViewModelMock.RefreshCommand.AssertWasCalled(m => m.CanExecute(Arg<IRegnskabViewModel>.Is.Equal(mock)));
                regnskabViewModelMock.RefreshCommand.AssertWasCalled(m => m.Execute(Arg<IRegnskabViewModel>.Is.Equal(mock)));
            }
        }
    }
}
