﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Core;
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
    /// Tester kommando, der kan hente og opdatere debitorlisten til et regnskab.
    /// </summary>
    [TestFixture]
    public class DebitorlisteGetCommandTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en kommando, der kan hente og opdatere debitorlisten til et regnskab.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererDebitorlisteGetCommand()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var command = new DebitorlisteGetCommand(fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(command, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis repositoryet til finansstyring er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFinansstyringRepositoryErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new DebitorlisteGetCommand(null, fixture.Create<IExceptionHandlerViewModel>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("finansstyringRepository"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis ViewModel for exceptionhandleren er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisExceptionHandlerViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new DebitorlisteGetCommand(fixture.Create<IFinansstyringRepository>(), null));
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
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));

            var command = new DebitorlisteGetCommand(fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(command, Is.Not.Null);

            var result = command.CanExecute(fixture.Create<IRegnskabViewModel>());
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tester, at Execute tilføjer nye debitorer til regnskabet.
        /// </summary>
        [Test]
        public void TestAtExecuteAddsNyeDebitorerTilRegnskabViewModel()
        {
            var rand = new Random(DateTime.Now.Millisecond);
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IAdressekontoModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IAdressekontoModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(fixture.Create<int>())
                        .Repeat.Any();
                    mock.Expect(m => m.StatusDato)
                        .Return(fixture.Create<DateTime>().AddDays(rand.Next(0, 30)*-1))
                        .Repeat.Any();
                    return mock;
                }));

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.DageForNyheder)
                                                    .Return(7)
                                                    .Repeat.Any();

            var adressekontoModelCollection = fixture.CreateMany<IAdressekontoModel>(250).ToList();
            Func<IEnumerable<IAdressekontoModel>> getter = () => adressekontoModelCollection;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.Konfiguration)
                                       .Return(finansstyringKonfigurationRepositoryMock)
                                       .Repeat.Any();
            finansstyringRepositoryMock.Expect(m => m.DebitorlisteGetAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue)))
                                       .Return(Task.Run(getter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var count = adressekontoModelCollection.Count;
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
                regnskabViewModelMock.Expect(m => m.Debitorer)
                                     .Return(new List<IAdressekontoViewModel>(0))
                                     .Repeat.Any();
                regnskabViewModelMock.Expect(m => m.DebitorAdd(Arg<IAdressekontoViewModel>.Is.NotNull))
                                     .WhenCalled(e =>
                                         {
                                             count--;
                                             if (count == 0)
                                             {
                                                 we.Set();
                                             }
                                         })
                                     .Repeat.Any();

                var command = new DebitorlisteGetCommand(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
                Assert.That(command, Is.Not.Null);

                command.Execute(regnskabViewModelMock);
                waitEvent.WaitOne(3000);

                finansstyringRepositoryMock.AssertWasCalled(m => m.Konfiguration);
                finansstyringRepositoryMock.AssertWasCalled(m => m.DebitorlisteGetAsync(Arg<int>.Is.Equal(regnskabViewModelMock.Nummer), Arg<DateTime>.Is.Equal(regnskabViewModelMock.StatusDato)));
                finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.DageForNyheder);
                regnskabViewModelMock.AssertWasCalled(m => m.Debitorer);
                regnskabViewModelMock.AssertWasCalled(m => m.DebitorAdd(Arg<IAdressekontoViewModel>.Is.NotNull), opt => opt.Repeat.Times(adressekontoModelCollection.Count));
            }

            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Execute tilføjer nye debitorer til regnskabet.
        /// </summary>
        [Test]
        public void TestAtExecuteAddsNyeDebitorerSomNyhederTilRegnskabViewModel()
        {
            var rand = new Random(DateTime.Now.Millisecond);
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IAdressekontoModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IAdressekontoModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(fixture.Create<int>())
                        .Repeat.Any();
                    mock.Expect(m => m.StatusDato)
                        .Return(fixture.Create<DateTime>().AddDays(rand.Next(0, 10)*-1))
                        .Repeat.Any();
                    return mock;
                }));

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.DageForNyheder)
                                                    .Return(7)
                                                    .Repeat.Any();

            var adressekontoModelCollection = fixture.CreateMany<IAdressekontoModel>(250).ToList();
            Func<IEnumerable<IAdressekontoModel>> getter = () => adressekontoModelCollection;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.Konfiguration)
                                       .Return(finansstyringKonfigurationRepositoryMock)
                                       .Repeat.Any();
            finansstyringRepositoryMock.Expect(m => m.DebitorlisteGetAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue)))
                                       .Return(Task.Run(getter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var count = adressekontoModelCollection.Count;
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
                regnskabViewModelMock.Expect(m => m.Debitorer)
                                     .Return(new List<IAdressekontoViewModel>(0))
                                     .Repeat.Any();
                regnskabViewModelMock.Expect(m => m.DebitorAdd(Arg<IAdressekontoViewModel>.Is.NotNull))
                                     .WhenCalled(e =>
                                         {
                                             count--;
                                             if (count == 0)
                                             {
                                                 we.Set();
                                             }
                                         })
                                     .Repeat.Any();

                var command = new DebitorlisteGetCommand(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
                Assert.That(command, Is.Not.Null);

                command.Execute(regnskabViewModelMock);
                waitEvent.WaitOne(3000);

                finansstyringRepositoryMock.AssertWasCalled(m => m.Konfiguration);
                finansstyringRepositoryMock.AssertWasCalled(m => m.DebitorlisteGetAsync(Arg<int>.Is.Equal(regnskabViewModelMock.Nummer), Arg<DateTime>.Is.Equal(regnskabViewModelMock.StatusDato)));
                finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.DageForNyheder);
                regnskabViewModelMock.AssertWasCalled(m => m.Debitorer);
                regnskabViewModelMock.AssertWasCalled(m => m.DebitorAdd(Arg<IAdressekontoViewModel>.Is.NotNull), opt => opt.Repeat.Times(adressekontoModelCollection.Count));

                var dageForNyheder = finansstyringKonfigurationRepositoryMock.DageForNyheder;
                var adressekontoTilNyhedCollection = adressekontoModelCollection.Where(m => m.StatusDato.Date.CompareTo(regnskabViewModelMock.StatusDato.Date.AddDays(dageForNyheder * -1)) >= 0 && m.StatusDato.Date.CompareTo(regnskabViewModelMock.StatusDato.Date) <= 0).ToList();
                adressekontoTilNyhedCollection.ForEach(m => m.AssertWasCalled(n => n.SetNyhedsaktualitet(Nyhedsaktualitet.Low)));
                regnskabViewModelMock.AssertWasCalled(m => m.NyhedAdd(Arg<INyhedViewModel>.Is.NotNull), opt => opt.Repeat.Times(adressekontoTilNyhedCollection.Count));
            }

            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Execute opdaterer eksisterende debitorer i regnskabet.
        /// </summary>
        [Test]
        public void TestAtExecuteUpdatesEksisterendeDebitorerIRegnskabViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IAdressekontoModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IAdressekontoModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(fixture.Create<int>())
                        .Repeat.Any();
                    mock.Expect(m => m.Navn)
                        .Return(fixture.Create<string>())
                        .Repeat.Any();
                    mock.Expect(m => m.PrimærTelefon)
                        .Return(fixture.Create<string>())
                        .Repeat.Any();
                    mock.Expect(m => m.SekundærTelefon)
                        .Return(fixture.Create<string>())
                        .Repeat.Any();
                    mock.Expect(m => m.StatusDato)
                        .Return(fixture.Create<DateTime>())
                        .Repeat.Any();
                    mock.Expect(m => m.Saldo)
                        .Return(fixture.Create<decimal>())
                        .Repeat.Any();
                    return mock;
                }));

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.DageForNyheder)
                                                    .Return(7)
                                                    .Repeat.Any();

            var adressekontoModelCollection = fixture.CreateMany<IAdressekontoModel>(1).ToList();
            Func<IEnumerable<IAdressekontoModel>> getter = () => adressekontoModelCollection;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.Konfiguration)
                                       .Return(finansstyringKonfigurationRepositoryMock)
                                       .Repeat.Any();
            finansstyringRepositoryMock.Expect(m => m.DebitorlisteGetAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue)))
                                       .Return(Task.Run(getter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            using (var waitEvent = new AutoResetEvent(false))
            {
                var we = waitEvent;
                var adressekontoViewModelMock = MockRepository.GenerateMock<IAdressekontoViewModel>();
                adressekontoViewModelMock.Expect(m => m.Nummer)
                                         .Return(adressekontoModelCollection.ElementAt(0).Nummer)
                                         .Repeat.Any();
                adressekontoViewModelMock.Expect(m => m.Saldo = Arg<decimal>.Is.Anything)
                                         .WhenCalled(e => we.Set())
                                         .Repeat.Any();

                var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
                regnskabViewModelMock.Expect(m => m.Nummer)
                                     .Return(fixture.Create<int>())
                                     .Repeat.Any();
                regnskabViewModelMock.Expect(m => m.StatusDato)
                                     .Return(fixture.Create<DateTime>())
                                     .Repeat.Any();
                regnskabViewModelMock.Expect(m => m.Debitorer)
                                     .Return(new List<IAdressekontoViewModel> {adressekontoViewModelMock})
                                     .Repeat.Any();
  
                var command = new DebitorlisteGetCommand(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
                Assert.That(command, Is.Not.Null);

                command.Execute(regnskabViewModelMock);
                waitEvent.WaitOne(3000);

                finansstyringRepositoryMock.AssertWasCalled(m => m.Konfiguration);
                finansstyringRepositoryMock.AssertWasCalled(m => m.DebitorlisteGetAsync(Arg<int>.Is.Equal(regnskabViewModelMock.Nummer), Arg<DateTime>.Is.Equal(regnskabViewModelMock.StatusDato)));
                finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.DageForNyheder);
                regnskabViewModelMock.AssertWasCalled(m => m.Debitorer);
                regnskabViewModelMock.AssertWasNotCalled(m => m.DebitorAdd(Arg<IAdressekontoViewModel>.Is.Anything));
                adressekontoViewModelMock.AssertWasCalled(m => m.Navn = Arg<string>.Is.Equal(adressekontoModelCollection.ElementAt(0).Navn));
                adressekontoViewModelMock.AssertWasCalled(m => m.PrimærTelefon = Arg<string>.Is.Equal(adressekontoModelCollection.ElementAt(0).PrimærTelefon));
                adressekontoViewModelMock.AssertWasCalled(m => m.SekundærTelefon = Arg<string>.Is.Equal(adressekontoModelCollection.ElementAt(0).SekundærTelefon));
                adressekontoViewModelMock.AssertWasCalled(m => m.StatusDato = Arg<DateTime>.Is.Equal(adressekontoModelCollection.ElementAt(0).StatusDato));
                adressekontoViewModelMock.AssertWasCalled(m => m.Saldo = Arg<decimal>.Is.Equal(adressekontoModelCollection.ElementAt(0).Saldo));
            }

            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Execute udfører RefreshCommand på debitorer, som ikke er blevet hentet.
        /// </summary>
        [Test]
        public void TestAtExecuteExecutesRefreshCommandOnDebitorerSomIkkeErHentet()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IAdressekontoModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IAdressekontoModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(fixture.Create<int>())
                        .Repeat.Any();
                    mock.Expect(m => m.Navn)
                        .Return(fixture.Create<string>())
                        .Repeat.Any();
                    mock.Expect(m => m.PrimærTelefon)
                        .Return(fixture.Create<string>())
                        .Repeat.Any();
                    mock.Expect(m => m.SekundærTelefon)
                        .Return(fixture.Create<string>())
                        .Repeat.Any();
                    mock.Expect(m => m.StatusDato)
                        .Return(fixture.Create<DateTime>())
                        .Repeat.Any();
                    mock.Expect(m => m.Saldo)
                        .Return(fixture.Create<decimal>())
                        .Repeat.Any();
                    return mock;
                }));

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.DageForNyheder)
                                                    .Return(7)
                                                    .Repeat.Any();

            var adressekontoModelCollection = fixture.CreateMany<IAdressekontoModel>(1).ToList();
            Func<IEnumerable<IAdressekontoModel>> getter = () => adressekontoModelCollection;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.Konfiguration)
                                       .Return(finansstyringKonfigurationRepositoryMock)
                                       .Repeat.Any();
            finansstyringRepositoryMock.Expect(m => m.DebitorlisteGetAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue)))
                                       .Return(Task.Run(getter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            using (var waitEvent = new AutoResetEvent(false))
            {
                var we = waitEvent;
                var refreshCommandMock = MockRepository.GenerateMock<ICommand>();
                refreshCommandMock.Expect(m => m.CanExecute(Arg<object>.Is.NotNull))
                                  .Return(true)
                                  .Repeat.Any();
                refreshCommandMock.Expect(m => m.Execute(Arg<object>.Is.NotNull))
                                  .WhenCalled(e => we.Set())
                                  .Repeat.Any();

                var adressekontoViewModelMock = MockRepository.GenerateMock<IAdressekontoViewModel>();
                adressekontoViewModelMock.Expect(m => m.Nummer)
                                         .Return(adressekontoModelCollection.ElementAt(0).Nummer)
                                         .Repeat.Any();

                var unreadedAdressekontoViewModelMock = MockRepository.GenerateMock<IAdressekontoViewModel>();
                unreadedAdressekontoViewModelMock.Expect(m => m.Nummer)
                                                 .Return(fixture.Create<int>())
                                                 .Repeat.Any();
                unreadedAdressekontoViewModelMock.Expect(m => m.RefreshCommand)
                                                 .Return(refreshCommandMock)
                                                 .Repeat.Any();

                var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
                regnskabViewModelMock.Expect(m => m.Nummer)
                                     .Return(fixture.Create<int>())
                                     .Repeat.Any();
                regnskabViewModelMock.Expect(m => m.StatusDato)
                                     .Return(fixture.Create<DateTime>())
                                     .Repeat.Any();
                regnskabViewModelMock.Expect(m => m.Debitorer)
                                     .Return(new List<IAdressekontoViewModel> {adressekontoViewModelMock, unreadedAdressekontoViewModelMock})
                                     .Repeat.Any();

                var command = new DebitorlisteGetCommand(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
                Assert.That(command, Is.Not.Null);

                command.Execute(regnskabViewModelMock);
                waitEvent.WaitOne(3000);

                finansstyringRepositoryMock.AssertWasCalled(m => m.Konfiguration);
                finansstyringRepositoryMock.AssertWasCalled(m => m.DebitorlisteGetAsync(Arg<int>.Is.Equal(regnskabViewModelMock.Nummer), Arg<DateTime>.Is.Equal(regnskabViewModelMock.StatusDato)));
                finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.DageForNyheder);
                regnskabViewModelMock.AssertWasCalled(m => m.Debitorer);
                regnskabViewModelMock.AssertWasNotCalled(m => m.DebitorAdd(Arg<IAdressekontoViewModel>.Is.Anything));
                unreadedAdressekontoViewModelMock.AssertWasCalled(m => m.RefreshCommand);
                refreshCommandMock.AssertWasCalled(m => m.CanExecute(Arg<object>.Is.Equal(unreadedAdressekontoViewModelMock)));
                refreshCommandMock.AssertWasCalled(m => m.Execute(Arg<object>.Is.Equal(unreadedAdressekontoViewModelMock)));
            }

            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Execute kalder exceptionhandleren med en IntranetGuiRepositoryException ved IntranetGuiRepositoryException.
        /// </summary>
        [Test]
        public void TestAtExecuteKalderExceptionHandlerViewModelMedIntranetGuiRepositoryExceptionVedIntranetGuiRepositoryException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();

            Func<IEnumerable<IAdressekontoModel>> getter = () =>
                {
                    throw new IntranetGuiRepositoryException(fixture.Create<string>());
                };
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.Konfiguration)
                                       .Return(finansstyringKonfigurationRepositoryMock)
                                       .Repeat.Any();
            finansstyringRepositoryMock.Expect(m => m.DebitorlisteGetAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue)))
                                       .Return(Task.Run(getter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            using (var waitEvent = new AutoResetEvent(false))
            {
                var we = waitEvent;
                exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<Exception>.Is.NotNull))
                                             .WhenCalled(e =>
                                                 {
                                                     Debug.WriteLine(e.Arguments.ElementAt(0));
                                                     we.Set();
                                                 })
                                                 .Repeat.Any();

                var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
                regnskabViewModelMock.Expect(m => m.Nummer)
                                     .Return(fixture.Create<int>())
                                     .Repeat.Any();
                regnskabViewModelMock.Expect(m => m.StatusDato)
                                     .Return(fixture.Create<DateTime>())
                                     .Repeat.Any();

                var command = new DebitorlisteGetCommand(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
                Assert.That(command, Is.Not.Null);

                command.Execute(regnskabViewModelMock);
                waitEvent.WaitOne(3000);

                finansstyringRepositoryMock.AssertWasCalled(m => m.Konfiguration);
                finansstyringRepositoryMock.AssertWasCalled(m => m.DebitorlisteGetAsync(Arg<int>.Is.Equal(regnskabViewModelMock.Nummer), Arg<DateTime>.Is.Equal(regnskabViewModelMock.StatusDato)));
            }

            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiRepositoryException>.Is.TypeOf));
        }
    }
}
