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
    /// Tester kommando, der på et regnskab kan initerer en ny ViewModel til bogføring.
    /// </summary>
    [TestFixture]
    public class BogføringSetCommandTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en kommando, der på et regnskab kan initerer en ny ViewModel til bogføring.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererBogføringSetCommand()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var command = new BogføringSetCommand(fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
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

            var exception = Assert.Throws<ArgumentNullException>(() => new BogføringSetCommand(null, fixture.Create<IExceptionHandlerViewModel>()));
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

            var exception = Assert.Throws<ArgumentNullException>(() => new BogføringSetCommand(fixture.Create<IFinansstyringRepository>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("exceptionHandlerViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at CanExecute returnerer angivelse af, om kommandoen kan udføre.
        /// </summary>
        [Test]
        [TestCase(0, 0, false)]
        [TestCase(7, 0, true)]
        [TestCase(0, 25, true)]
        [TestCase(7, 25, true)]
        public void TestAtCanExecuteReturnererResult(int numberOfKonti, int numberOfBogføringslinjer, bool expectedResult)
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IKontoViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontoViewModel>()));
            fixture.Customize<IReadOnlyBogføringslinjeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var command = new BogføringSetCommand(fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(command, Is.Not.Null);

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Konti)
                                 .Return(numberOfKonti == 0 ? new List<IKontoViewModel>(0) : fixture.CreateMany<IKontoViewModel>(numberOfKonti).ToList())
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Bogføringslinjer)
                                 .Return(numberOfBogføringslinjer == 0 ? new List<IReadOnlyBogføringslinjeViewModel>(0) : fixture.CreateMany<IReadOnlyBogføringslinjeViewModel>(numberOfBogføringslinjer).ToList())
                                 .Repeat.Any();

            var result = command.CanExecute(regnskabViewModelMock);
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Tester, at Execute kalder SætBogføring på ViewModel for regnskabet med udgangspunkt i et kontonummer fra konti.
        /// </summary>
        [Test]
        public void TestAtExecuteKalderSætBogføringOnRegnskabViewModelMedKontonummerFraKonti()
        {
            var fixture = new Fixture();
            fixture.Customize<IBogføringslinjeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringslinjeModel>()));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IKontoViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IKontoViewModel>();
                    mock.Expect(m => m.Kontonummer)
                        .Return(fixture.Create<string>())
                        .Repeat.Any();
                    return mock;
                }));

            Func<IBogføringslinjeModel> getter = fixture.Create<IBogføringslinjeModel>;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.BogføringslinjeCreateNewAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.NotNull))
                                       .Return(Task.Run(getter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var command = new BogføringSetCommand(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);
            Assert.That(command.ExecuteTask, Is.Null);

            var regnskabsnummer = fixture.Create<int>();
            var kontoViewModelMockCollection = fixture.CreateMany<IKontoViewModel>(7).ToList();
            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(regnskabsnummer)
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Konti)
                                 .Return(kontoViewModelMockCollection)
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Bogføringslinjer)
                                 .Return(new List<IReadOnlyBogføringslinjeViewModel>(0))
                                 .Repeat.Any();

            command.Execute(regnskabViewModelMock);
            Assert.That(command.ExecuteTask, Is.Not.Null);
            command.ExecuteTask.Wait(3000);

            finansstyringRepositoryMock.AssertWasCalled(m => m.BogføringslinjeCreateNewAsync(Arg<int>.Is.Equal(regnskabsnummer), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.Equal(kontoViewModelMockCollection.ElementAt(0).Kontonummer)));
            regnskabViewModelMock.AssertWasCalled(m => m.BogføringSet(Arg<IBogføringViewModel>.Is.NotNull));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Execute kalder SætBogføring på ViewModel for regnskabet med udgangspunkt i et kontonummer fra bogføringslinjer.
        /// </summary>
        [Test]
        public void TestAtExecuteKalderSætBogføringOnRegnskabViewModelMedKontonummerFraBogføringslinjer()
        {
            var fixture = new Fixture();
            fixture.Customize<IBogføringslinjeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringslinjeModel>()));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IReadOnlyBogføringslinjeViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>();
                    mock.Expect(m => m.Kontonummer)
                        .Return(fixture.Create<string>())
                        .Repeat.Any();
                    return mock;
                }));

            Func<IBogføringslinjeModel> getter = fixture.Create<IBogføringslinjeModel>;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.BogføringslinjeCreateNewAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.NotNull))
                                       .Return(Task.Run(getter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var command = new BogføringSetCommand(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);
            Assert.That(command.ExecuteTask, Is.Null);

            var regnskabsnummer = fixture.Create<int>();
            var bogføringslinjeViewModelMockCollection = fixture.CreateMany<IReadOnlyBogføringslinjeViewModel>(25).ToList();
            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(regnskabsnummer)
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Konti)
                                 .Return(new List<IKontoViewModel>(0))
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Bogføringslinjer)
                                 .Return(bogføringslinjeViewModelMockCollection)
                                 .Repeat.Any();

            command.Execute(regnskabViewModelMock);
            Assert.That(command.ExecuteTask, Is.Not.Null);
            command.ExecuteTask.Wait(3000);

            finansstyringRepositoryMock.AssertWasCalled(m => m.BogføringslinjeCreateNewAsync(Arg<int>.Is.Equal(regnskabsnummer), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.Equal(bogføringslinjeViewModelMockCollection.ElementAt(0).Kontonummer)));
            regnskabViewModelMock.AssertWasCalled(m => m.BogføringSet(Arg<IBogføringViewModel>.Is.NotNull));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Execute kalder exceptionhandleren med en IntranetGuiRepositoryException ved IntranetGuiRepositoryException.
        /// </summary>
        [Test]
        public void TestAtExecuteKalderExceptionHandlerViewModelMedIntranetGuiRepositoryExceptionVedIntranetGuiRepositoryException()
        {
            var fixture = new Fixture();
            fixture.Customize<IBogføringslinjeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringslinjeModel>()));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IReadOnlyBogføringslinjeViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>();
                    mock.Expect(m => m.Kontonummer)
                        .Return(fixture.Create<string>())
                        .Repeat.Any();
                    return mock;
                }));

            Func<IBogføringslinjeModel> getter = () =>
                {
                    throw fixture.Create<IntranetGuiRepositoryException>();
                };
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.BogføringslinjeCreateNewAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.NotNull))
                                       .Return(Task.Run(getter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var command = new BogføringSetCommand(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);
            Assert.That(command.ExecuteTask, Is.Null);

            var regnskabsnummer = fixture.Create<int>();
            var bogføringslinjeViewModelMockCollection = fixture.CreateMany<IReadOnlyBogføringslinjeViewModel>(25).ToList();
            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(regnskabsnummer)
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Konti)
                                 .Return(new List<IKontoViewModel>(0))
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Bogføringslinjer)
                                 .Return(bogføringslinjeViewModelMockCollection)
                                 .Repeat.Any();

            command.Execute(regnskabViewModelMock);
            Assert.That(command.ExecuteTask, Is.Not.Null);
            command.ExecuteTask.Wait(3000);

            finansstyringRepositoryMock.AssertWasCalled(m => m.BogføringslinjeCreateNewAsync(Arg<int>.Is.Equal(regnskabsnummer), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.Equal(bogføringslinjeViewModelMockCollection.ElementAt(0).Kontonummer)));
            regnskabViewModelMock.AssertWasNotCalled(m => m.BogføringSet(Arg<IBogføringViewModel>.Is.Anything));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiRepositoryException>.Is.TypeOf));
        }
    }
}
