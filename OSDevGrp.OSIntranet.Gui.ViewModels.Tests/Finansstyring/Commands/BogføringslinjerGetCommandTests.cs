using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
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
    /// Tester kommando, der kan hente bogføringslinjer til et regnskab.
    /// </summary>
    [TestFixture]
    public class BogføringslinjerGetCommandTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en kommando, der kan hente bogføringslinjer til et regnskab.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererBogføringslinjerGetCommand()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var command = new BogføringslinjerGetCommand(fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(command, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis repository til finansstyring er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFinansstyringRepositoryErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            Assert.Throws<ArgumentNullException>(() => new BogføringslinjerGetCommand(null, fixture.Create<IExceptionHandlerViewModel>()));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis ViewModel for exceptionhandleren er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisExceptionHandlerViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            Assert.Throws<ArgumentNullException>(() => new BogføringslinjerGetCommand(fixture.Create<IFinansstyringRepository>(), null));
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
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));

            var command = new BogføringslinjerGetCommand(fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(command, Is.Not.Null);

            var result = command.CanExecute(fixture.Create<IRegnskabViewModel>());
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tester, at Execute tilføjer nye bogføringslinjer til regnskabet.
        /// </summary>
        [Test]
        public void TestAtExecuteAddsNyeBogføringslinjerTilRegnskabViewModel()
        {
            var rand = new Random(DateTime.Now.Millisecond);
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IBogføringslinjeModel>(e => e.FromFactory(() =>
                {
                    var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
                    bogføringslinjeModelMock.Expect(m => m.Løbenummer)
                                            .Return(fixture.Create<int>())
                                            .Repeat.Any();
                    bogføringslinjeModelMock.Expect(m => m.Dato)
                                            .Return(fixture.Create<DateTime>().AddDays(rand.Next(0, 30)*-1))
                                            .Repeat.Any();
                    return bogføringslinjeModelMock;
                }));

            var bogføringslinjeModelMockCollection = fixture.CreateMany<IBogføringslinjeModel>(100).ToList();
            Func<IEnumerable<IBogføringslinjeModel>> getter = () => bogføringslinjeModelMockCollection;
            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.AntalBogføringslinjer)
                                                    .Return(bogføringslinjeModelMockCollection.Count)
                                                    .Repeat.Any();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.DageForNyheder)
                                                    .Return(7)
                                                    .Repeat.Any();
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.Konfiguration)
                                       .Return(finansstyringKonfigurationRepositoryMock)
                                       .Repeat.Any();
            finansstyringRepositoryMock.Expect(m => m.BogføringslinjerGetAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.Anything, Arg<int>.Is.GreaterThan(0)))
                                       .Return(Task.Run(getter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var count = bogføringslinjeModelMockCollection.Count;
            using (var waitEvent = new AutoResetEvent(false))
            {
                var we = waitEvent;
                var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
                regnskabViewModelMock.Expect(m => m.Nummer)
                                     .Return(fixture.Create<int>())
                                     .Repeat.Any();
                regnskabViewModelMock.Expect(m => m.StatusDato)
                                     .Return(fixture.Create<DateTime>())
                                     .Repeat.Any();
                regnskabViewModelMock.Expect(m => m.Bogføringslinjer)
                                     .Return(new ReadOnlyObservableCollection<IReadOnlyBogføringslinjeViewModel>(new ObservableCollection<IReadOnlyBogføringslinjeViewModel>(new List<IReadOnlyBogføringslinjeViewModel>(0))))
                                     .Repeat.Any();
                regnskabViewModelMock.Expect(m => m.BogføringslinjeAdd(Arg<IReadOnlyBogføringslinjeViewModel>.Is.NotNull))
                                     .WhenCalled(e =>
                                         {
                                             count--;
                                             if (count == 0)
                                             {
                                                 we.Set();
                                             }
                                         })
                                     .Repeat.Any();

                var command = new BogføringslinjerGetCommand(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
                Assert.That(command, Is.Not.Null);

                command.Execute(regnskabViewModelMock);
                waitEvent.WaitOne(3000);

                finansstyringRepositoryMock.AssertWasCalled(m => m.Konfiguration);
                finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.AntalBogføringslinjer);
                finansstyringRepositoryMock.AssertWasCalled(m => m.BogføringslinjerGetAsync(Arg<int>.Is.Equal(regnskabViewModelMock.Nummer), Arg<DateTime>.Is.Equal(regnskabViewModelMock.StatusDato), Arg<int>.Is.Equal(finansstyringKonfigurationRepositoryMock.AntalBogføringslinjer)));
                finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.DageForNyheder);
                regnskabViewModelMock.AssertWasCalled(m => m.Bogføringslinjer, opt => opt.Repeat.Times(bogføringslinjeModelMockCollection.Count));
                regnskabViewModelMock.AssertWasCalled(m => m.BogføringslinjeAdd(Arg<IReadOnlyBogføringslinjeViewModel>.Is.NotNull), opt => opt.Repeat.Times(bogføringslinjeModelMockCollection.Count));
                exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
            }
        }

        /// <summary>
        /// Tester, at Execute tilføjer nye bogføringslinjer som nyheder til regnskabet.
        /// </summary>
        [Test]
        public void TestAtExecuteAddsNyeBogføringslinjerSomNyhederTilRegnskabViewModel()
        {
            var rand = new Random(DateTime.Now.Millisecond);
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IBogføringslinjeModel>(e => e.FromFactory(() =>
                {
                    var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
                    bogføringslinjeModelMock.Expect(m => m.Løbenummer)
                                            .Return(fixture.Create<int>())
                                            .Repeat.Any();
                    bogføringslinjeModelMock.Expect(m => m.Dato)
                                            .Return(fixture.Create<DateTime>().AddDays(rand.Next(0, 10)*-1))
                                            .Repeat.Any();
                    return bogføringslinjeModelMock;
                }));

            var bogføringslinjeModelMockCollection = fixture.CreateMany<IBogføringslinjeModel>(100).ToList();
            Func<IEnumerable<IBogføringslinjeModel>> getter = () => bogføringslinjeModelMockCollection;
            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.AntalBogføringslinjer)
                                                    .Return(bogføringslinjeModelMockCollection.Count)
                                                    .Repeat.Any();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.DageForNyheder)
                                                    .Return(7)
                                                    .Repeat.Any();
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.Konfiguration)
                                       .Return(finansstyringKonfigurationRepositoryMock)
                                       .Repeat.Any();
            finansstyringRepositoryMock.Expect(m => m.BogføringslinjerGetAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.Anything, Arg<int>.Is.GreaterThan(0)))
                                       .Return(Task.Run(getter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var count = bogføringslinjeModelMockCollection.Count;
            using (var waitEvent = new AutoResetEvent(false))
            {
                var we = waitEvent;
                var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
                regnskabViewModelMock.Expect(m => m.Nummer)
                                     .Return(fixture.Create<int>())
                                     .Repeat.Any();
                regnskabViewModelMock.Expect(m => m.StatusDato)
                                     .Return(fixture.Create<DateTime>())
                                     .Repeat.Any();
                regnskabViewModelMock.Expect(m => m.Bogføringslinjer)
                                     .Return(new ReadOnlyObservableCollection<IReadOnlyBogføringslinjeViewModel>(new ObservableCollection<IReadOnlyBogføringslinjeViewModel>(new List<IReadOnlyBogføringslinjeViewModel>(0))))
                                     .Repeat.Any();
                regnskabViewModelMock.Expect(m => m.BogføringslinjeAdd(Arg<IReadOnlyBogføringslinjeViewModel>.Is.NotNull))
                                     .WhenCalled(e =>
                                         {
                                             count--;
                                             if (count == 0)
                                             {
                                                 we.Set();
                                             }
                                         })
                                     .Repeat.Any();

                var command = new BogføringslinjerGetCommand(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
                Assert.That(command, Is.Not.Null);

                command.Execute(regnskabViewModelMock);
                waitEvent.WaitOne(3000);

                finansstyringRepositoryMock.AssertWasCalled(m => m.Konfiguration);
                finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.AntalBogføringslinjer);
                // ReSharper disable ImplicitlyCapturedClosure
                finansstyringRepositoryMock.AssertWasCalled(m => m.BogføringslinjerGetAsync(Arg<int>.Is.Equal(regnskabViewModelMock.Nummer), Arg<DateTime>.Is.Equal(regnskabViewModelMock.StatusDato), Arg<int>.Is.Equal(finansstyringKonfigurationRepositoryMock.AntalBogføringslinjer)));
                // ReSharper restore ImplicitlyCapturedClosure
                finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.DageForNyheder);
                // ReSharper disable ImplicitlyCapturedClosure
                regnskabViewModelMock.AssertWasCalled(m => m.Bogføringslinjer, opt => opt.Repeat.Times(bogføringslinjeModelMockCollection.Count));
                // ReSharper restore ImplicitlyCapturedClosure
                // ReSharper disable ImplicitlyCapturedClosure
                regnskabViewModelMock.AssertWasCalled(m => m.BogføringslinjeAdd(Arg<IReadOnlyBogføringslinjeViewModel>.Is.NotNull), opt => opt.Repeat.Times(bogføringslinjeModelMockCollection.Count));
                // ReSharper restore ImplicitlyCapturedClosure
                regnskabViewModelMock.AssertWasCalled(m => m.NyhedAdd(Arg<INyhedViewModel>.Is.NotNull), opt => opt.Repeat.Times(bogføringslinjeModelMockCollection.Count(m => m.Dato.Date.CompareTo(regnskabViewModelMock.StatusDato.AddDays(finansstyringKonfigurationRepositoryMock.DageForNyheder*-1).Date) >= 0 && m.Dato.Date.CompareTo(regnskabViewModelMock.StatusDato.Date) <= 0)));
                exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
            }
        }

        /// <summary>
        /// Tester, at Execute skipper allerede håndterede og indsatte bogføringslinjer.
        /// </summary>
        [Test]
        public void TestAtExecuteSkipsHandledBogføringslinjer()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IBogføringslinjeModel>(e => e.FromFactory(() =>
                {
                    var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
                    bogføringslinjeModelMock.Expect(m => m.Løbenummer)
                                            .Return(fixture.Create<int>())
                                            .Repeat.Any();
                    bogføringslinjeModelMock.Expect(m => m.Dato)
                                            .Return(fixture.Create<DateTime>())
                                            .Repeat.Any();
                    return bogføringslinjeModelMock;
                }));

            var bogføringslinjeModelMockCollection = fixture.CreateMany<IBogføringslinjeModel>(1).ToList();
            Func<IEnumerable<IBogføringslinjeModel>> getter = () => bogføringslinjeModelMockCollection;
            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.AntalBogføringslinjer)
                                                    .Return(bogføringslinjeModelMockCollection.Count)
                                                    .Repeat.Any();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.DageForNyheder)
                                                    .Return(7)
                                                    .Repeat.Any();
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.Konfiguration)
                                       .Return(finansstyringKonfigurationRepositoryMock)
                                       .Repeat.Any();
            finansstyringRepositoryMock.Expect(m => m.BogføringslinjerGetAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.Anything, Arg<int>.Is.GreaterThan(0)))
                                       .Return(Task.Run(getter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            using (var waitEvent = new AutoResetEvent(false))
            {
                var we = waitEvent;
                var bogføringslinjeViewModelMock = MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>();
                bogføringslinjeViewModelMock.Expect(m => m.Løbenummer)
                                            .WhenCalled(e =>
                                                {
                                                    e.ReturnValue = bogføringslinjeModelMockCollection.ElementAt(0).Løbenummer;
                                                    we.Set();
                                                })
                                            .Return(bogføringslinjeModelMockCollection.ElementAt(0).Løbenummer)
                                            .Repeat.Any();

                var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
                regnskabViewModelMock.Expect(m => m.Nummer)
                                     .Return(fixture.Create<int>())
                                     .Repeat.Any();
                regnskabViewModelMock.Expect(m => m.StatusDato)
                                     .Return(fixture.Create<DateTime>())
                                     .Repeat.Any();
                regnskabViewModelMock.Expect(m => m.Bogføringslinjer)
                                     .Return(new ReadOnlyObservableCollection<IReadOnlyBogføringslinjeViewModel>(new ObservableCollection<IReadOnlyBogføringslinjeViewModel>(new List<IReadOnlyBogføringslinjeViewModel> {bogføringslinjeViewModelMock})))
                                     .Repeat.Any();

                var command = new BogføringslinjerGetCommand(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
                Assert.That(command, Is.Not.Null);

                command.Execute(regnskabViewModelMock);
                waitEvent.WaitOne(3000);

                finansstyringRepositoryMock.AssertWasCalled(m => m.Konfiguration);
                finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.AntalBogføringslinjer);
                finansstyringRepositoryMock.AssertWasCalled(m => m.BogføringslinjerGetAsync(Arg<int>.Is.Equal(regnskabViewModelMock.Nummer), Arg<DateTime>.Is.Equal(regnskabViewModelMock.StatusDato), Arg<int>.Is.Equal(finansstyringKonfigurationRepositoryMock.AntalBogføringslinjer)));
                finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.DageForNyheder);
                regnskabViewModelMock.AssertWasCalled(m => m.Bogføringslinjer, opt => opt.Repeat.Times(bogføringslinjeModelMockCollection.Count));
                bogføringslinjeViewModelMock.AssertWasCalled(m => m.Løbenummer);
                regnskabViewModelMock.AssertWasNotCalled(m => m.BogføringslinjeAdd(Arg<IReadOnlyBogføringslinjeViewModel>.Is.Anything));
                regnskabViewModelMock.AssertWasNotCalled(m => m.NyhedAdd(Arg<INyhedViewModel>.Is.Anything));
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

            Func<IEnumerable<IBogføringslinjeModel>> getter = () =>
                {
                    throw new IntranetGuiRepositoryException(fixture.Create<string>());
                };
            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.AntalBogføringslinjer)
                                                    .Return(fixture.Create<int>())
                                                    .Repeat.Any();
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.Konfiguration)
                                       .Return(finansstyringKonfigurationRepositoryMock)
                                       .Repeat.Any();
            finansstyringRepositoryMock.Expect(m => m.BogføringslinjerGetAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.Anything, Arg<int>.Is.GreaterThan(0)))
                                       .Return(Task.Run(getter))
                                       .Repeat.Any();

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(fixture.Create<int>())
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.StatusDato)
                                 .Return(fixture.Create<DateTime>())
                                 .Repeat.Any();

            using (var waitEvent = new AutoResetEvent(false))
            {
                var we = waitEvent;
                var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
                exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<Exception>.Is.NotNull))
                                             .WhenCalled(e =>
                                                 {
                                                     Debug.WriteLine(e.Arguments.ElementAt(0));
                                                     we.Set();
                                                 })
                                             .Repeat.Any();

                var command = new BogføringslinjerGetCommand(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
                Assert.That(command, Is.Not.Null);

                command.Execute(regnskabViewModelMock);
                waitEvent.WaitOne(3000);

                finansstyringRepositoryMock.AssertWasCalled(m => m.Konfiguration);
                finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.AntalBogføringslinjer);
                finansstyringRepositoryMock.AssertWasCalled(m => m.BogføringslinjerGetAsync(Arg<int>.Is.Equal(regnskabViewModelMock.Nummer), Arg<DateTime>.Is.Equal(regnskabViewModelMock.StatusDato), Arg<int>.Is.Equal(finansstyringKonfigurationRepositoryMock.AntalBogføringslinjer)));
                finansstyringKonfigurationRepositoryMock.AssertWasNotCalled(m => m.DageForNyheder);
                regnskabViewModelMock.AssertWasNotCalled(m => m.Bogføringslinjer);
                regnskabViewModelMock.AssertWasNotCalled(m => m.BogføringslinjeAdd(Arg<IReadOnlyBogføringslinjeViewModel>.Is.Anything));
                regnskabViewModelMock.AssertWasNotCalled(m => m.NyhedAdd(Arg<INyhedViewModel>.Is.Anything));
                exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiRepositoryException>.Is.TypeOf));
            }
        }
    }
}
