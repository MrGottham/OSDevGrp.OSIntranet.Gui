using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Finansstyring.Commands
{
    /// <summary>
    /// Tester kommandoen, der kan tilføje en bogføringslinje til et regnskab.
    /// </summary>
    [TestFixture]
    public class BogføringAddCommandTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en kommando, der kan tilføje en bogføringslinje til et regnskab.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererBogføringAddCommand()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var command = new BogføringAddCommand(fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(command, Is.Not.Null);
            Assert.That(command.ExecuteTask, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis repositoryet til finansstyring er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFinansstyringRepositoryErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new BogføringAddCommand(null, fixture.Create<IExceptionHandlerViewModel>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("finansstyringRepository"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis exceptionhandleren er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisExceptionHandlerViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new BogføringAddCommand(fixture.Create<IFinansstyringRepository>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("exceptionHandlerViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at CanExecute returnerer true, hvis der kan bogføres.
        /// </summary>
        [Test]
        public void TestAtCanExecuteReturnererTrueHvisDerKanBogføres()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var bogføringViewModelMock = CreateBogføringViewModelMock(fixture.Create<DateTime>().ToString("d", Thread.CurrentThread.CurrentUICulture), string.Empty, fixture.Create<string>(), fixture.Create<string>(), string.Empty, fixture.Create<decimal>().ToString("C", Thread.CurrentThread.CurrentUICulture), string.Empty, 0);
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var command = new BogføringAddCommand(fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);

            var result = command.CanExecute(bogføringViewModelMock);
            Assert.That(result, Is.True);

            bogføringViewModelMock.AssertWasCalled(m => m.DatoAsText);
            bogføringViewModelMock.AssertWasCalled(m => m.Bilag);
            bogføringViewModelMock.AssertWasCalled(m => m.Kontonummer);
            bogføringViewModelMock.AssertWasCalled(m => m.Tekst);
            bogføringViewModelMock.AssertWasCalled(m => m.Budgetkontonummer);
            bogføringViewModelMock.AssertWasCalled(m => m.Debit);
            bogføringViewModelMock.AssertWasCalled(m => m.DebitAsText);
            bogføringViewModelMock.AssertWasCalled(m => m.Kredit);
            bogføringViewModelMock.AssertWasCalled(m => m.KreditAsText);
            bogføringViewModelMock.AssertWasCalled(m => m.Adressekonto);
            bogføringViewModelMock.AssertWasCalled(m => m.IsWorking);
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at CanExecute returnerer false, hvis validering af bogføringsdatoen fejler.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("XYZ")]
        [TestCase("ZYX")]
        [TestCase("2014-31-01")]
        [TestCase("2014-01-32")]
        [TestCase("2050-01-01")]
        [TestCase("2050-01-31")]
        [TestCase("2050-12-01")]
        [TestCase("2050-12-31")]
        public void TestAtCanExecuteReturnererFalseHvisValideringAfDatoFejler(string invalidValue)
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            DateTime dato;
            var bogføringViewModelMock = CreateBogføringViewModelMock(DateTime.TryParse(invalidValue, new CultureInfo("en-US"), DateTimeStyles.None, out dato) ? dato.ToString("d", Thread.CurrentThread.CurrentUICulture) : invalidValue, string.Empty, fixture.Create<string>(), fixture.Create<string>(), string.Empty, fixture.Create<decimal>().ToString("C", Thread.CurrentThread.CurrentUICulture), string.Empty, 0);
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var command = new BogføringAddCommand(fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);

            var result = command.CanExecute(bogføringViewModelMock);
            Assert.That(result, Is.False);

            bogføringViewModelMock.AssertWasCalled(m => m.DatoAsText);
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at CanExecute returnerer false, hvis validering af kontonummer fejler.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void TestAtCanExecuteReturnererFalseHvisValideringAfKontonummerFejler(string invalidValue)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var bogføringViewModelMock = CreateBogføringViewModelMock(fixture.Create<DateTime>().ToString("d", Thread.CurrentThread.CurrentUICulture), string.Empty, invalidValue, fixture.Create<string>(), string.Empty, fixture.Create<decimal>().ToString("C", Thread.CurrentThread.CurrentUICulture), string.Empty, 0);
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var command = new BogføringAddCommand(fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);

            var result = command.CanExecute(bogføringViewModelMock);
            Assert.That(result, Is.False);

            bogføringViewModelMock.AssertWasCalled(m => m.Kontonummer);
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at CanExecute returnerer false, hvis validering af tekst fejler.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void TestAtCanExecuteReturnererFalseHvisValideringAfTekstFejler(string invalidValue)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var bogføringViewModelMock = CreateBogføringViewModelMock(fixture.Create<DateTime>().ToString("d", Thread.CurrentThread.CurrentUICulture), string.Empty, fixture.Create<string>(), invalidValue, string.Empty, fixture.Create<decimal>().ToString("C", Thread.CurrentThread.CurrentUICulture), string.Empty, 0);
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var command = new BogføringAddCommand(fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);

            var result = command.CanExecute(bogføringViewModelMock);
            Assert.That(result, Is.False);

            bogføringViewModelMock.AssertWasCalled(m => m.Tekst);
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at CanExecute returnerer false, hvis validering af debitbeløb fejler.
        /// </summary>
        [Test]
        [TestCase("XYZ")]
        [TestCase("ZYX")]
        [TestCase("$ -0.01")]
        [TestCase("$ -1000.00")]
        [TestCase("$ -2000.00")]
        [TestCase("$ -3000.00")]
        [TestCase("$ -4000.00")]
        public void TestAtCanExecuteReturnererFalseHvisValideringAfDebitFejler(string invalidValue)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            decimal value;
            var bogføringViewModelMock = CreateBogføringViewModelMock(fixture.Create<DateTime>().ToString("d", Thread.CurrentThread.CurrentUICulture), string.Empty, fixture.Create<string>(), fixture.Create<string>(), string.Empty, decimal.TryParse(invalidValue, NumberStyles.Any, new CultureInfo("en-US"), out value) ? value.ToString("C", Thread.CurrentThread.CurrentUICulture) : invalidValue, string.Empty, 0);
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var command = new BogføringAddCommand(fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);

            var result = command.CanExecute(bogføringViewModelMock);
            Assert.That(result, Is.False);

            bogføringViewModelMock.AssertWasCalled(m => m.DebitAsText);
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at CanExecute returnerer false, hvis validering af kreditbeløb fejler.
        /// </summary>
        [Test]
        [TestCase("XYZ")]
        [TestCase("ZYX")]
        [TestCase("$ -0.01")]
        [TestCase("$ -1000.00")]
        [TestCase("$ -2000.00")]
        [TestCase("$ -3000.00")]
        [TestCase("$ -4000.00")]
        public void TestAtCanExecuteReturnererFalseHvisValideringAfKreditFejler(string invalidValue)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            decimal value;
            var bogføringViewModelMock = CreateBogføringViewModelMock(fixture.Create<DateTime>().ToString("d", Thread.CurrentThread.CurrentUICulture), string.Empty, fixture.Create<string>(), fixture.Create<string>(), string.Empty, string.Empty, decimal.TryParse(invalidValue, NumberStyles.Any, new CultureInfo("en-US"), out value) ? value.ToString("C", Thread.CurrentThread.CurrentUICulture) : invalidValue, 0);
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var command = new BogføringAddCommand(fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);

            var result = command.CanExecute(bogføringViewModelMock);
            Assert.That(result, Is.False);

            bogføringViewModelMock.AssertWasCalled(m => m.KreditAsText);
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at CanExecute returnerer false, hvis hverken debitbeløb eller kreditbeløb er angivet.
        /// </summary>
        [Test]
        [TestCase(null, null)]
        [TestCase("", null)]
        [TestCase(null, "")]
        [TestCase("", "")]
        [TestCase(" ", null)]
        [TestCase(null, " ")]
        [TestCase(" ", " ")]
        [TestCase("$ 0.00", null)]
        [TestCase(null, "$ 0.00")]
        [TestCase("$ 0.00", "$ 0.00")]
        [TestCase("$ 0.00", "")]
        [TestCase("", "$ 0.00")]
        [TestCase("$ 0.00", "$ 0.00")]
        public void TestAtCanExecuteReturnererFalseHvisDebitEllerKreditIkkeErAngivet(string debitAsText, string kreditAsText)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            decimal debit;
            decimal kredit;
            var bogføringViewModelMock = CreateBogføringViewModelMock(fixture.Create<DateTime>().ToString("d", Thread.CurrentThread.CurrentUICulture), string.Empty, fixture.Create<string>(), fixture.Create<string>(), string.Empty, decimal.TryParse(debitAsText, NumberStyles.Any, new CultureInfo("en-US"), out debit) ? debit.ToString("C", Thread.CurrentThread.CurrentUICulture) : debitAsText, decimal.TryParse(kreditAsText, NumberStyles.Any, new CultureInfo("en-US"), out kredit) ? kredit.ToString("C", Thread.CurrentThread.CurrentUICulture) : kreditAsText, 0);
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var command = new BogføringAddCommand(fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);

            var result = command.CanExecute(bogføringViewModelMock);
            Assert.That(result, Is.False);

            bogføringViewModelMock.AssertWasCalled(m => m.Debit);
            bogføringViewModelMock.AssertWasCalled(m => m.Kredit);
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at CanExecute returnerer false, hvis der udføres asynkront arbejde på ViewModel, hvor fra der kan bogføres.
        /// </summary>
        [Test]
        public void TestAtCanExecuteReturnererFalseHvisIsWorkeringErTrueOnBogføringViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var bogføringViewModelMock = CreateBogføringViewModelMock(fixture.Create<DateTime>().ToString("d", Thread.CurrentThread.CurrentUICulture), string.Empty, fixture.Create<string>(), fixture.Create<string>(), string.Empty, fixture.Create<decimal>().ToString("C", Thread.CurrentThread.CurrentUICulture), string.Empty, 0, true);
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var command = new BogføringAddCommand(fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);

            var result = command.CanExecute(bogføringViewModelMock);
            Assert.That(result, Is.False);

            bogføringViewModelMock.AssertWasCalled(m => m.IsWorking);
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at CanExecute returnerer false, hvis der kastes en Exception.
        /// </summary>
        [Test]
        public void TestAtCanExecuteReturnererFalseVedException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var bogføringViewModelMock = MockRepository.GenerateMock<IBogføringViewModel>();
            bogføringViewModelMock.Expect(m => m.DatoAsText)
                                  .Throw(fixture.Create<Exception>())
                                  .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var command = new BogføringAddCommand(fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);

            var result = command.CanExecute(bogføringViewModelMock);
            Assert.That(result, Is.False);

            bogføringViewModelMock.AssertWasCalled(m => m.DatoAsText);
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Execute benytter BogførAsync i repositoryet til finansstyring til bogføring.
        /// </summary>
        [Test]
        public void TestAtExecuteKalderBogførAsyncOnFinansstyringRepository()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IBogføringslinjeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringslinjeModel>()));

            var regnskabsnummer = fixture.Create<int>();
            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(regnskabsnummer)
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Konti)
                                 .Return(new List<IKontoViewModel>(0))
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Budgetkonti)
                                 .Return(new List<IBudgetkontoViewModel>(0))
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Debitorer)
                                 .Return(new List<IAdressekontoViewModel>(0))
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Kreditorer)
                                 .Return(new List<IAdressekontoViewModel>(0))
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.BogføringSetCommand)
                                 .Return(null)
                                 .Repeat.Any();

            var dato = fixture.Create<DateTime>().Date;
            var bilag = fixture.Create<string>();
            var kontonummer = fixture.Create<string>();
            var tekst = fixture.Create<string>();
            var budgetkontonummer = fixture.Create<string>();
            var debit = fixture.Create<decimal>();
            var kredit = fixture.Create<decimal>();
            var adressekonto = fixture.Create<int>();
            var bogføringViewModelMock = CreateBogføringViewModelMock(dato.ToString("d", Thread.CurrentThread.CurrentUICulture), bilag, kontonummer, tekst, budgetkontonummer, debit.ToString("C", Thread.CurrentThread.CurrentUICulture), kredit.ToString("C", Thread.CurrentThread.CurrentUICulture), adressekonto);
            bogføringViewModelMock.Expect(m => m.Regnskab)
                                  .Return(regnskabViewModelMock)
                                  .Repeat.Any();

            var bogføringsresultatModelMock = MockRepository.GenerateMock<IBogføringsresultatModel>();
            bogføringsresultatModelMock.Expect(m => m.Bogføringslinje)
                                       .Return(fixture.Create<IBogføringslinjeModel>())
                                       .Repeat.Any();
            bogføringsresultatModelMock.Expect(m => m.Bogføringsadvarsler)
                                       .Return(new List<IBogføringsadvarselModel>(0))
                                       .Repeat.Any();

            Func<IBogføringsresultatModel> bogføringsresultatGetter = () => bogføringsresultatModelMock;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.BogførAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.Anything, Arg<string>.Is.NotNull, Arg<string>.Is.NotNull, Arg<string>.Is.Anything, Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<int>.Is.Anything))
                                       .Return(Task.Run(bogføringsresultatGetter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var command = new BogføringAddCommand(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);

            Action action = () =>
                {
                    command.Execute(bogføringViewModelMock);
                    Assert.That(command.ExecuteTask, Is.Not.Null);
                    command.ExecuteTask.Wait();
                };
            Task.Run(action).Wait(3000);

            finansstyringRepositoryMock.AssertWasCalled(m => m.BogførAsync(Arg<int>.Is.Equal(regnskabsnummer), Arg<DateTime>.Is.Equal(dato), Arg<string>.Is.Equal(bilag), Arg<string>.Is.Equal(kontonummer), Arg<string>.Is.Equal(tekst), Arg<string>.Is.Equal(budgetkontonummer), Arg<decimal>.Is.Equal(debit), Arg<decimal>.Is.Equal(kredit), Arg<int>.Is.Equal(adressekonto)));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Execute tilføjer den bogførte linje til regnskabet.
        /// </summary>
        [Test]
        public void TestAtExecuteAddsBogføringslinjeViewModelTilRegnskabViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IBogføringslinjeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringslinjeModel>()));

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(fixture.Create<int>())
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Konti)
                                 .Return(new List<IKontoViewModel>(0))
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Budgetkonti)
                                 .Return(new List<IBudgetkontoViewModel>(0))
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Debitorer)
                                 .Return(new List<IAdressekontoViewModel>(0))
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Kreditorer)
                                 .Return(new List<IAdressekontoViewModel>(0))
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.BogføringSetCommand)
                                 .Return(null)
                                 .Repeat.Any();

            var bogføringViewModelMock = CreateBogføringViewModelMock(fixture.Create<DateTime>().Date.ToString(CultureInfo.CurrentUICulture), string.Empty, fixture.Create<string>(), fixture.Create<string>(), string.Empty, fixture.Create<decimal>().ToString("C"), string.Empty, 0);
            bogføringViewModelMock.Expect(m => m.Regnskab)
                                  .Return(regnskabViewModelMock)
                                  .Repeat.Any();

            var bogføringsresultatModelMock = MockRepository.GenerateMock<IBogføringsresultatModel>();
            bogføringsresultatModelMock.Expect(m => m.Bogføringslinje)
                                       .Return(fixture.Create<IBogføringslinjeModel>())
                                       .Repeat.Any();
            bogføringsresultatModelMock.Expect(m => m.Bogføringsadvarsler)
                                       .Return(new List<IBogføringsadvarselModel>(0))
                                       .Repeat.Any();

            Func<IBogføringsresultatModel> bogføringsresultatGetter = () => bogføringsresultatModelMock;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.BogførAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.Anything, Arg<string>.Is.NotNull, Arg<string>.Is.NotNull, Arg<string>.Is.Anything, Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<int>.Is.Anything))
                                       .Return(Task.Run(bogføringsresultatGetter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var command = new BogføringAddCommand(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);

            Action action = () =>
                {
                    command.Execute(bogføringViewModelMock);
                    Assert.That(command.ExecuteTask, Is.Not.Null);
                    command.ExecuteTask.Wait();
                };
            Task.Run(action).Wait(3000);

            finansstyringRepositoryMock.AssertWasCalled(m => m.BogførAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.Anything, Arg<string>.Is.NotNull, Arg<string>.Is.NotNull, Arg<string>.Is.Anything, Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<int>.Is.Anything));
            regnskabViewModelMock.AssertWasCalled(m => m.BogføringslinjeAdd(Arg<IReadOnlyBogføringslinjeViewModel>.Is.NotNull));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Execute tilføjer den bogførte linje som nyhed til regnskabet.
        /// </summary>
        [Test]
        public void TestAtExecuteAddsNyhedViewModelTilRegnskabViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IBogføringslinjeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringslinjeModel>()));

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(fixture.Create<int>())
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Konti)
                                 .Return(new List<IKontoViewModel>(0))
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Budgetkonti)
                                 .Return(new List<IBudgetkontoViewModel>(0))
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Debitorer)
                                 .Return(new List<IAdressekontoViewModel>(0))
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Kreditorer)
                                 .Return(new List<IAdressekontoViewModel>(0))
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.BogføringSetCommand)
                                 .Return(null)
                                 .Repeat.Any();

            var bogføringViewModelMock = CreateBogføringViewModelMock(fixture.Create<DateTime>().Date.ToString(CultureInfo.CurrentUICulture), string.Empty, fixture.Create<string>(), fixture.Create<string>(), string.Empty, fixture.Create<decimal>().ToString("C"), string.Empty, 0);
            bogføringViewModelMock.Expect(m => m.Regnskab)
                                  .Return(regnskabViewModelMock)
                                  .Repeat.Any();

            var bogføringsresultatModelMock = MockRepository.GenerateMock<IBogføringsresultatModel>();
            bogføringsresultatModelMock.Expect(m => m.Bogføringslinje)
                                       .Return(fixture.Create<IBogføringslinjeModel>())
                                       .Repeat.Any();
            bogføringsresultatModelMock.Expect(m => m.Bogføringsadvarsler)
                                       .Return(new List<IBogføringsadvarselModel>(0))
                                       .Repeat.Any();

            Func<IBogføringsresultatModel> bogføringsresultatGetter = () => bogføringsresultatModelMock;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.BogførAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.Anything, Arg<string>.Is.NotNull, Arg<string>.Is.NotNull, Arg<string>.Is.Anything, Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<int>.Is.Anything))
                                       .Return(Task.Run(bogføringsresultatGetter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var command = new BogføringAddCommand(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);

            Action action = () =>
                {
                    command.Execute(bogføringViewModelMock);
                    Assert.That(command.ExecuteTask, Is.Not.Null);
                    command.ExecuteTask.Wait();
                };
            Task.Run(action).Wait(3000);

            finansstyringRepositoryMock.AssertWasCalled(m => m.BogførAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.Anything, Arg<string>.Is.NotNull, Arg<string>.Is.NotNull, Arg<string>.Is.Anything, Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<int>.Is.Anything));
            regnskabViewModelMock.AssertWasCalled(m => m.NyhedAdd(Arg<INyhedViewModel>.Is.NotNull));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Execute tilføjer eventuelle bogføringsadvarsler til regnskabet.
        /// </summary>
        [Test]
        public void TestAtExecuteAddsBogføringsadvarselViewModelsTilRegnskabViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IBogføringslinjeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringslinjeModel>()));
            fixture.Customize<IBogføringsadvarselModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringsadvarselModel>()));

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(fixture.Create<int>())
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Konti)
                                 .Return(new List<IKontoViewModel>(0))
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Budgetkonti)
                                 .Return(new List<IBudgetkontoViewModel>(0))
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Debitorer)
                                 .Return(new List<IAdressekontoViewModel>(0))
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Kreditorer)
                                 .Return(new List<IAdressekontoViewModel>(0))
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.BogføringSetCommand)
                                 .Return(null)
                                 .Repeat.Any();

            var bogføringViewModelMock = CreateBogføringViewModelMock(fixture.Create<DateTime>().Date.ToString(CultureInfo.CurrentUICulture), string.Empty, fixture.Create<string>(), fixture.Create<string>(), string.Empty, fixture.Create<decimal>().ToString("C"), string.Empty, 0);
            bogføringViewModelMock.Expect(m => m.Regnskab)
                                  .Return(regnskabViewModelMock)
                                  .Repeat.Any();

            var bogføringsadvarselModelMockCollection = fixture.CreateMany<IBogføringsadvarselModel>(7).ToList();
            var bogføringsresultatModelMock = MockRepository.GenerateMock<IBogføringsresultatModel>();
            bogføringsresultatModelMock.Expect(m => m.Bogføringslinje)
                                       .Return(fixture.Create<IBogføringslinjeModel>())
                                       .Repeat.Any();
            bogføringsresultatModelMock.Expect(m => m.Bogføringsadvarsler)
                                       .Return(bogføringsadvarselModelMockCollection)
                                       .Repeat.Any();

            Func<IBogføringsresultatModel> bogføringsresultatGetter = () => bogføringsresultatModelMock;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.BogførAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.Anything, Arg<string>.Is.NotNull, Arg<string>.Is.NotNull, Arg<string>.Is.Anything, Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<int>.Is.Anything))
                                       .Return(Task.Run(bogføringsresultatGetter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var command = new BogføringAddCommand(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);

            Action action = () =>
                {
                    command.Execute(bogføringViewModelMock);
                    Assert.That(command.ExecuteTask, Is.Not.Null);
                    command.ExecuteTask.Wait();
                };
            Task.Run(action).Wait(3000);

            finansstyringRepositoryMock.AssertWasCalled(m => m.BogførAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.Anything, Arg<string>.Is.NotNull, Arg<string>.Is.NotNull, Arg<string>.Is.Anything, Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<int>.Is.Anything));
            regnskabViewModelMock.AssertWasCalled(m => m.BogføringsadvarselAdd(Arg<IBogføringsadvarselViewModel>.Is.NotNull), opt => opt.Repeat.Times(bogføringsadvarselModelMockCollection.Count));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Execute udfører RefreshCommand på kontoen, hvorpå bogføringslinjen er bogført.
        /// </summary>
        [Test]
        public void TestAtExecuteExecutesRefreshCommandForKonto()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IBogføringslinjeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringslinjeModel>()));

            var refreshCommandMock = MockRepository.GenerateMock<ICommand>();
            refreshCommandMock.Expect(m => m.CanExecute(Arg<object>.Is.NotNull))
                              .Return(true)
                              .Repeat.Any();

            var kontonummer = fixture.Create<string>();
            var kontoViewModelMock = MockRepository.GenerateMock<IKontoViewModel>();
            kontoViewModelMock.Expect(m => m.Kontonummer)
                              .Return(kontonummer)
                              .Repeat.Any();
            kontoViewModelMock.Expect(m => m.RefreshCommand)
                              .Return(refreshCommandMock)
                              .Repeat.Any();

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(fixture.Create<int>())
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Konti)
                                 .Return(new List<IKontoViewModel> {kontoViewModelMock})
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Budgetkonti)
                                 .Return(new List<IBudgetkontoViewModel>(0))
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Debitorer)
                                 .Return(new List<IAdressekontoViewModel>(0))
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Kreditorer)
                                 .Return(new List<IAdressekontoViewModel>(0))
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.BogføringSetCommand)
                                 .Return(null)
                                 .Repeat.Any();

            var bogføringViewModelMock = CreateBogføringViewModelMock(fixture.Create<DateTime>().Date.ToString(CultureInfo.CurrentUICulture), string.Empty, kontonummer, fixture.Create<string>(), string.Empty, fixture.Create<decimal>().ToString("C"), string.Empty, 0);
            bogføringViewModelMock.Expect(m => m.Regnskab)
                                  .Return(regnskabViewModelMock)
                                  .Repeat.Any();

            var bogføringsresultatModelMock = MockRepository.GenerateMock<IBogføringsresultatModel>();
            bogføringsresultatModelMock.Expect(m => m.Bogføringslinje)
                                       .Return(fixture.Create<IBogføringslinjeModel>())
                                       .Repeat.Any();
            bogføringsresultatModelMock.Expect(m => m.Bogføringsadvarsler)
                                       .Return(new List<IBogføringsadvarselModel>(0))
                                       .Repeat.Any();

            Func<IBogføringsresultatModel> bogføringsresultatGetter = () => bogføringsresultatModelMock;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.BogførAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.Anything, Arg<string>.Is.NotNull, Arg<string>.Is.NotNull, Arg<string>.Is.Anything, Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<int>.Is.Anything))
                                       .Return(Task.Run(bogføringsresultatGetter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var command = new BogføringAddCommand(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);

            Action action = () =>
                {
                    command.Execute(bogføringViewModelMock);
                    Assert.That(command.ExecuteTask, Is.Not.Null);
                    command.ExecuteTask.Wait();
                };
            Task.Run(action).Wait(3000);

            finansstyringRepositoryMock.AssertWasCalled(m => m.BogførAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.Anything, Arg<string>.Is.NotNull, Arg<string>.Is.NotNull, Arg<string>.Is.Anything, Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<int>.Is.Anything));
            regnskabViewModelMock.AssertWasCalled(m => m.Konti);
            kontoViewModelMock.AssertWasCalled(m => m.RefreshCommand);
            refreshCommandMock.AssertWasCalled(m => m.CanExecute(Arg<object>.Is.Equal(kontoViewModelMock)));
            refreshCommandMock.AssertWasCalled(m => m.Execute(Arg<object>.Is.Equal(kontoViewModelMock)));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Execute udfører RefreshCommand på budgetkontoen, hvorpå bogføringslinjen er bogført.
        /// </summary>
        [Test]
        public void TestAtExecuteExecutesRefreshCommandForBudgetkonto()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IBogføringslinjeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringslinjeModel>()));

            var refreshCommandMock = MockRepository.GenerateMock<ICommand>();
            refreshCommandMock.Expect(m => m.CanExecute(Arg<object>.Is.NotNull))
                              .Return(true)
                              .Repeat.Any();

            var budgetkontonummer = fixture.Create<string>();
            var budgetkontoViewModelMock = MockRepository.GenerateMock<IBudgetkontoViewModel>();
            budgetkontoViewModelMock.Expect(m => m.Kontonummer)
                                    .Return(budgetkontonummer)
                                    .Repeat.Any();
            budgetkontoViewModelMock.Expect(m => m.RefreshCommand)
                                    .Return(refreshCommandMock)
                                    .Repeat.Any();

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(fixture.Create<int>())
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Konti)
                                 .Return(new List<IKontoViewModel>(0))
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Budgetkonti)
                                 .Return(new List<IBudgetkontoViewModel> {budgetkontoViewModelMock})
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Debitorer)
                                 .Return(new List<IAdressekontoViewModel>(0))
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Kreditorer)
                                 .Return(new List<IAdressekontoViewModel>(0))
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.BogføringSetCommand)
                                 .Return(null)
                                 .Repeat.Any();

            var bogføringViewModelMock = CreateBogføringViewModelMock(fixture.Create<DateTime>().Date.ToString(CultureInfo.CurrentUICulture), string.Empty, fixture.Create<string>(), fixture.Create<string>(), budgetkontonummer, fixture.Create<decimal>().ToString("C"), string.Empty, 0);
            bogføringViewModelMock.Expect(m => m.Regnskab)
                                  .Return(regnskabViewModelMock)
                                  .Repeat.Any();

            var bogføringsresultatModelMock = MockRepository.GenerateMock<IBogføringsresultatModel>();
            bogføringsresultatModelMock.Expect(m => m.Bogføringslinje)
                                       .Return(fixture.Create<IBogføringslinjeModel>())
                                       .Repeat.Any();
            bogføringsresultatModelMock.Expect(m => m.Bogføringsadvarsler)
                                       .Return(new List<IBogføringsadvarselModel>(0))
                                       .Repeat.Any();

            Func<IBogføringsresultatModel> bogføringsresultatGetter = () => bogføringsresultatModelMock;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.BogførAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.Anything, Arg<string>.Is.NotNull, Arg<string>.Is.NotNull, Arg<string>.Is.Anything, Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<int>.Is.Anything))
                                       .Return(Task.Run(bogføringsresultatGetter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var command = new BogføringAddCommand(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);

            Action action = () =>
                {
                    command.Execute(bogføringViewModelMock);
                    Assert.That(command.ExecuteTask, Is.Not.Null);
                    command.ExecuteTask.Wait();
                };
            Task.Run(action).Wait(3000);

            finansstyringRepositoryMock.AssertWasCalled(m => m.BogførAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.Anything, Arg<string>.Is.NotNull, Arg<string>.Is.NotNull, Arg<string>.Is.NotNull, Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<int>.Is.Anything));
            regnskabViewModelMock.AssertWasCalled(m => m.Budgetkonti);
            budgetkontoViewModelMock.AssertWasCalled(m => m.RefreshCommand);
            refreshCommandMock.AssertWasCalled(m => m.CanExecute(Arg<object>.Is.Equal(budgetkontoViewModelMock)));
            refreshCommandMock.AssertWasCalled(m => m.Execute(Arg<object>.Is.Equal(budgetkontoViewModelMock)));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Execute udfører RefreshCommand på adressekontoen, hvorpå bogføringslinjen er bogført.
        /// </summary>
        [Test]
        public void TestAtExecuteExecutesRefreshCommandForAdressekonto()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IBogføringslinjeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringslinjeModel>()));

            var refreshCommandMock = MockRepository.GenerateMock<ICommand>();
            refreshCommandMock.Expect(m => m.CanExecute(Arg<object>.Is.NotNull))
                              .Return(true)
                              .Repeat.Any();

            var adressekonto = fixture.Create<int>();
            var adressekontoViewModelMock = MockRepository.GenerateMock<IAdressekontoViewModel>();
            adressekontoViewModelMock.Expect(m => m.Nummer)
                                     .Return(adressekonto)
                                     .Repeat.Any();
            adressekontoViewModelMock.Expect(m => m.RefreshCommand)
                                     .Return(refreshCommandMock)
                                     .Repeat.Any();

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(fixture.Create<int>())
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Konti)
                                 .Return(new List<IKontoViewModel>(0))
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Budgetkonti)
                                 .Return(new List<IBudgetkontoViewModel>(0))
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Debitorer)
                                 .Return(new List<IAdressekontoViewModel> {adressekontoViewModelMock})
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Kreditorer)
                                 .Return(new List<IAdressekontoViewModel> {adressekontoViewModelMock})
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.BogføringSetCommand)
                                 .Return(null)
                                 .Repeat.Any();

            var bogføringViewModelMock = CreateBogføringViewModelMock(fixture.Create<DateTime>().Date.ToString(CultureInfo.CurrentUICulture), string.Empty, fixture.Create<string>(), fixture.Create<string>(), string.Empty, fixture.Create<decimal>().ToString("C"), string.Empty, adressekonto);
            bogføringViewModelMock.Expect(m => m.Regnskab)
                                  .Return(regnskabViewModelMock)
                                  .Repeat.Any();

            var bogføringsresultatModelMock = MockRepository.GenerateMock<IBogføringsresultatModel>();
            bogføringsresultatModelMock.Expect(m => m.Bogføringslinje)
                                       .Return(fixture.Create<IBogføringslinjeModel>())
                                       .Repeat.Any();
            bogføringsresultatModelMock.Expect(m => m.Bogføringsadvarsler)
                                       .Return(new List<IBogføringsadvarselModel>(0))
                                       .Repeat.Any();

            Func<IBogføringsresultatModel> bogføringsresultatGetter = () => bogføringsresultatModelMock;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.BogførAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.Anything, Arg<string>.Is.NotNull, Arg<string>.Is.NotNull, Arg<string>.Is.Anything, Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<int>.Is.GreaterThan(0)))
                                       .Return(Task.Run(bogføringsresultatGetter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var command = new BogføringAddCommand(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);

            Action action = () =>
                {
                    command.Execute(bogføringViewModelMock);
                    Assert.That(command.ExecuteTask, Is.Not.Null);
                    command.ExecuteTask.Wait();
                };
            Task.Run(action).Wait(3000);

            finansstyringRepositoryMock.AssertWasCalled(m => m.BogførAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.Anything, Arg<string>.Is.NotNull, Arg<string>.Is.NotNull, Arg<string>.Is.Anything, Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<int>.Is.Anything));
            regnskabViewModelMock.AssertWasCalled(m => m.Debitorer);
            regnskabViewModelMock.AssertWasCalled(m => m.Kreditorer);
            adressekontoViewModelMock.AssertWasCalled(m => m.RefreshCommand, opt => opt.Repeat.Times(2));
            refreshCommandMock.AssertWasCalled(m => m.CanExecute(Arg<object>.Is.Equal(adressekontoViewModelMock)), opt => opt.Repeat.Times(2));
            refreshCommandMock.AssertWasCalled(m => m.Execute(Arg<object>.Is.Equal(adressekontoViewModelMock)), opt => opt.Repeat.Times(2));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Execute rejser OnBogført ved endt bogføring.
        /// </summary>
        [Test]
        public void TestAtExecuteRejserOnBogførtEfterBogføring()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IBogføringslinjeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringslinjeModel>()));

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(fixture.Create<int>())
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Konti)
                                 .Return(new List<IKontoViewModel>(0))
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Budgetkonti)
                                 .Return(new List<IBudgetkontoViewModel>(0))
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Debitorer)
                                 .Return(new List<IAdressekontoViewModel>(0))
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Kreditorer)
                                 .Return(new List<IAdressekontoViewModel>(0))
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.BogføringSetCommand)
                                 .Return(null)
                                 .Repeat.Any();

            var bogføringViewModelMock = CreateBogføringViewModelMock(fixture.Create<DateTime>().Date.ToString(CultureInfo.CurrentUICulture), string.Empty, fixture.Create<string>(), fixture.Create<string>(), string.Empty, fixture.Create<decimal>().ToString("C"), string.Empty, 0);
            bogføringViewModelMock.Expect(m => m.Regnskab)
                                  .Return(regnskabViewModelMock)
                                  .Repeat.Any();

            var bogføringsresultatModelMock = MockRepository.GenerateMock<IBogføringsresultatModel>();
            bogføringsresultatModelMock.Expect(m => m.Bogføringslinje)
                                       .Return(fixture.Create<IBogføringslinjeModel>())
                                       .Repeat.Any();
            bogføringsresultatModelMock.Expect(m => m.Bogføringsadvarsler)
                                       .Return(new List<IBogføringsadvarselModel>(0))
                                       .Repeat.Any();

            Func<IBogføringsresultatModel> bogføringsresultatGetter = () => bogføringsresultatModelMock;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.BogførAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.Anything, Arg<string>.Is.NotNull, Arg<string>.Is.NotNull, Arg<string>.Is.Anything, Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<int>.Is.Anything))
                                       .Return(Task.Run(bogføringsresultatGetter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var command = new BogføringAddCommand(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);

            var eventCalled = false;
            command.OnBogført += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    eventCalled = true;
                };

            Assert.That(eventCalled, Is.False);
            Action action = () =>
                {
                    command.Execute(bogføringViewModelMock);
                    Assert.That(command.ExecuteTask, Is.Not.Null);
                    command.ExecuteTask.Wait();
                };
            Task.Run(action).Wait(3000);
            Assert.That(eventCalled, Is.True);

            finansstyringRepositoryMock.AssertWasCalled(m => m.BogførAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.Anything, Arg<string>.Is.NotNull, Arg<string>.Is.NotNull, Arg<string>.Is.Anything, Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<int>.Is.Anything));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Execute udfører kommandoen, der kan sætte en ny ViewModel til bogføring på regnskabet efter endt bogføring.
        /// </summary>
        [Test]
        public void TestAtExecuteExecutesBogføringSetCommandOnRegnskabViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IBogføringslinjeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringslinjeModel>()));

            var bogføringSetCommandMock = MockRepository.GenerateMock<ITaskableCommand>();
            bogføringSetCommandMock.Expect(m => m.CanExecute(Arg<object>.Is.NotNull))
                                   .Return(true)
                                   .Repeat.Any();

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(fixture.Create<int>())
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Konti)
                                 .Return(new List<IKontoViewModel>(0))
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Budgetkonti)
                                 .Return(new List<IBudgetkontoViewModel>(0))
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Debitorer)
                                 .Return(new List<IAdressekontoViewModel>(0))
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.Kreditorer)
                                 .Return(new List<IAdressekontoViewModel>(0))
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.BogføringSetCommand)
                                 .Return(bogføringSetCommandMock)
                                 .Repeat.Any();

            var bogføringViewModelMock = CreateBogføringViewModelMock(fixture.Create<DateTime>().Date.ToString(CultureInfo.CurrentUICulture), string.Empty, fixture.Create<string>(), fixture.Create<string>(), string.Empty, fixture.Create<decimal>().ToString("C"), string.Empty, 0);
            bogføringViewModelMock.Expect(m => m.Regnskab)
                                  .Return(regnskabViewModelMock)
                                  .Repeat.Any();

            var bogføringsresultatModelMock = MockRepository.GenerateMock<IBogføringsresultatModel>();
            bogføringsresultatModelMock.Expect(m => m.Bogføringslinje)
                                       .Return(fixture.Create<IBogføringslinjeModel>())
                                       .Repeat.Any();
            bogføringsresultatModelMock.Expect(m => m.Bogføringsadvarsler)
                                       .Return(new List<IBogføringsadvarselModel>(0))
                                       .Repeat.Any();

            Func<IBogføringsresultatModel> bogføringsresultatGetter = () => bogføringsresultatModelMock;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.BogførAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.Anything, Arg<string>.Is.NotNull, Arg<string>.Is.NotNull, Arg<string>.Is.Anything, Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<int>.Is.Anything))
                                       .Return(Task.Run(bogføringsresultatGetter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var command = new BogføringAddCommand(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);

            Action action = () =>
                {
                    command.Execute(bogføringViewModelMock);
                    Assert.That(command.ExecuteTask, Is.Not.Null);
                    command.ExecuteTask.Wait();
                };
            Task.Run(action).Wait(3000);

            finansstyringRepositoryMock.AssertWasCalled(m => m.BogførAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.Anything, Arg<string>.Is.NotNull, Arg<string>.Is.NotNull, Arg<string>.Is.Anything, Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<int>.Is.Anything));
            regnskabViewModelMock.AssertWasCalled(m => m.BogføringSetCommand);
            bogføringSetCommandMock.AssertWasCalled(m => m.CanExecute(Arg<object>.Is.Equal(regnskabViewModelMock)));
            bogføringSetCommandMock.AssertWasCalled(m => m.Execute(Arg<object>.Is.Equal(regnskabViewModelMock)));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Execute kalder HandleException på exceptionhandleren med en IntranetGuiCommandException ved IntranetGuiCommandException.
        /// </summary>
        [Test]
        public void TestAtExecuteKalderHandleExceptionOnExceptionHandlerViewModelMedIntranetGuiCommandExceptionVedIntranetGuiCommandException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(fixture.Create<int>())
                                 .Repeat.Any();

            var bogføringViewModelMock = CreateBogføringViewModelMock(fixture.Create<DateTime>().Date.ToString(CultureInfo.CurrentUICulture), string.Empty, fixture.Create<string>(), fixture.Create<string>(), string.Empty, fixture.Create<decimal>().ToString("C"), string.Empty, 0);
            bogføringViewModelMock.Expect(m => m.Regnskab)
                                  .Return(regnskabViewModelMock)
                                  .Repeat.Any();

            var exception = fixture.Create<IntranetGuiCommandException>();
            Func<IBogføringsresultatModel> bogføringsresultatGetter = () =>
                {
                    throw exception;
                };
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.BogførAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.Anything, Arg<string>.Is.NotNull, Arg<string>.Is.NotNull, Arg<string>.Is.Anything, Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<int>.Is.Anything))
                                       .Return(Task.Run(bogføringsresultatGetter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiCommandException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var commandException = (IntranetGuiCommandException) e.Arguments.ElementAt(0);
                                                 Assert.That(commandException, Is.Not.Null);
                                                 Assert.That(commandException.Message, Is.Not.Null);
                                                 Assert.That(commandException.Message, Is.Not.Empty);
                                                 Assert.That(commandException.Message, Is.EqualTo(exception.Message));
                                                 Assert.That(commandException.Reason, Is.Not.Null);
                                                 Assert.That(commandException.Reason, Is.Not.Empty);
                                                 Assert.That(commandException.Reason, Is.EqualTo(exception.Reason));
                                                 Assert.That(commandException.CommandContext, Is.Not.Null);
                                                 Assert.That(commandException.CommandContext, Is.EqualTo(exception.CommandContext));
                                                 Assert.That(commandException.ReasonContext, Is.Not.Null);
                                                 Assert.That(commandException.ReasonContext, Is.EqualTo(exception.ReasonContext));
                                                 Assert.That(commandException.InnerException, Is.Null);
                                             })
                                         .Repeat.Any();

            var command = new BogføringAddCommand(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);

            Action action = () =>
                {
                    command.Execute(bogføringViewModelMock);
                    Assert.That(command.ExecuteTask, Is.Not.Null);
                    command.ExecuteTask.Wait();
                };
            Task.Run(action).Wait(3000);

            finansstyringRepositoryMock.AssertWasCalled(m => m.BogførAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.Anything, Arg<string>.Is.NotNull, Arg<string>.Is.NotNull, Arg<string>.Is.Anything, Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<int>.Is.Anything));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiCommandException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at Execute kalder HandleException på exceptionhandleren med en IntranetGuiCommandException ved IntranetGuiValidationException.
        /// </summary>
        [Test]
        public void TestAtExecuteKalderHandleExceptionOnExceptionHandlerViewModelMedIntranetGuiCommandExceptionVedIntranetGuiValidationException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(fixture.Create<int>())
                                 .Repeat.Any();

            var bogføringViewModelMock = CreateBogføringViewModelMock(fixture.Create<DateTime>().Date.ToString(CultureInfo.CurrentUICulture), string.Empty, fixture.Create<string>(), fixture.Create<string>(), string.Empty, fixture.Create<decimal>().ToString("C"), string.Empty, 0);
            bogføringViewModelMock.Expect(m => m.Regnskab)
                                  .Return(regnskabViewModelMock)
                                  .Repeat.Any();

            var exception = fixture.Create<IntranetGuiValidationException>();
            // ReSharper disable ImplicitlyCapturedClosure
            Func<IBogføringsresultatModel> bogføringsresultatGetter = () =>
                {
                    throw exception;
                };
            // ReSharper restore ImplicitlyCapturedClosure
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.BogførAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.Anything, Arg<string>.Is.NotNull, Arg<string>.Is.NotNull, Arg<string>.Is.Anything, Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<int>.Is.Anything))
                                       .Return(Task.Run(bogføringsresultatGetter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiCommandException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var commandException = (IntranetGuiCommandException) e.Arguments.ElementAt(0);
                                                 Assert.That(commandException, Is.Not.Null);
                                                 Assert.That(commandException.Message, Is.Not.Null);
                                                 Assert.That(commandException.Message, Is.Not.Empty);
                                                 Assert.That(commandException.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.ErrorPostingAccountingLine)));
                                                 Assert.That(commandException.Reason, Is.Not.Null);
                                                 Assert.That(commandException.Reason, Is.Not.Empty);
                                                 Assert.That(commandException.Reason, Is.EqualTo(exception.Message));
                                                 Assert.That(commandException.CommandContext, Is.Not.Null);
                                                 Assert.That(commandException.CommandContext, Is.TypeOf<BogføringAddCommand>());
                                                 Assert.That(commandException.ReasonContext, Is.Not.Null);
                                                 Assert.That(commandException.ReasonContext, Is.EqualTo(bogføringViewModelMock));
                                                 Assert.That(commandException.InnerException, Is.Not.Null);
                                                 Assert.That(commandException.InnerException, Is.EqualTo(exception));
                                             })
                                         .Repeat.Any();

            var command = new BogføringAddCommand(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);

            // ReSharper disable ImplicitlyCapturedClosure
            Action action = () =>
                {
                    command.Execute(bogføringViewModelMock);
                    Assert.That(command.ExecuteTask, Is.Not.Null);
                    command.ExecuteTask.Wait();
                };
            // ReSharper restore ImplicitlyCapturedClosure
            Task.Run(action).Wait(3000);
            
            finansstyringRepositoryMock.AssertWasCalled(m => m.BogførAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.Anything, Arg<string>.Is.NotNull, Arg<string>.Is.NotNull, Arg<string>.Is.Anything, Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<int>.Is.Anything));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiCommandException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at Execute kalder HandleException på exceptionhandleren med en IntranetGuiCommandException ved IntranetGuiBusinessException.
        /// </summary>
        [Test]
        public void TestAtExecuteKalderHandleExceptionOnExceptionHandlerViewModelMedIntranetGuiCommandExceptionVedIntranetGuiBusinessException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(fixture.Create<int>())
                                 .Repeat.Any();

            var bogføringViewModelMock = CreateBogføringViewModelMock(fixture.Create<DateTime>().Date.ToString(CultureInfo.CurrentUICulture), string.Empty, fixture.Create<string>(), fixture.Create<string>(), string.Empty, fixture.Create<decimal>().ToString("C"), string.Empty, 0);
            bogføringViewModelMock.Expect(m => m.Regnskab)
                                  .Return(regnskabViewModelMock)
                                  .Repeat.Any();

            var exception = fixture.Create<IntranetGuiBusinessException>();
            // ReSharper disable ImplicitlyCapturedClosure
            Func<IBogføringsresultatModel> bogføringsresultatGetter = () =>
                {
                    throw exception;
                };
            // ReSharper restore ImplicitlyCapturedClosure
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.BogførAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.Anything, Arg<string>.Is.NotNull, Arg<string>.Is.NotNull, Arg<string>.Is.Anything, Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<int>.Is.Anything))
                                       .Return(Task.Run(bogføringsresultatGetter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiCommandException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var commandException = (IntranetGuiCommandException) e.Arguments.ElementAt(0);
                                                 Assert.That(commandException, Is.Not.Null);
                                                 Assert.That(commandException.Message, Is.Not.Null);
                                                 Assert.That(commandException.Message, Is.Not.Empty);
                                                 Assert.That(commandException.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.ErrorPostingAccountingLine)));
                                                 Assert.That(commandException.Reason, Is.Not.Null);
                                                 Assert.That(commandException.Reason, Is.Not.Empty);
                                                 Assert.That(commandException.Reason, Is.EqualTo(exception.Message));
                                                 Assert.That(commandException.CommandContext, Is.Not.Null);
                                                 Assert.That(commandException.CommandContext, Is.TypeOf<BogføringAddCommand>());
                                                 Assert.That(commandException.ReasonContext, Is.Not.Null);
                                                 Assert.That(commandException.ReasonContext, Is.EqualTo(bogføringViewModelMock));
                                                 Assert.That(commandException.InnerException, Is.Not.Null);
                                                 Assert.That(commandException.InnerException, Is.EqualTo(exception));
                                             })
                                         .Repeat.Any();

            var command = new BogføringAddCommand(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);

            // ReSharper disable ImplicitlyCapturedClosure
            Action action = () =>
                {
                    command.Execute(bogføringViewModelMock);
                    Assert.That(command.ExecuteTask, Is.Not.Null);
                    command.ExecuteTask.Wait();
                };
            // ReSharper restore ImplicitlyCapturedClosure
            Task.Run(action).Wait(3000);

            finansstyringRepositoryMock.AssertWasCalled(m => m.BogførAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.Anything, Arg<string>.Is.NotNull, Arg<string>.Is.NotNull, Arg<string>.Is.Anything, Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<int>.Is.Anything));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiCommandException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at Execute kalder HandleException på exceptionhandleren med en IntranetGuiCommandException ved IntranetGuiRepositoryException.
        /// </summary>
        [Test]
        public void TestAtExecuteKalderHandleExceptionOnExceptionHandlerViewModelMedIntranetGuiCommandExceptionVedIntranetGuiRepositoryException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(fixture.Create<int>())
                                 .Repeat.Any();

            var bogføringViewModelMock = CreateBogføringViewModelMock(fixture.Create<DateTime>().Date.ToString(CultureInfo.CurrentUICulture), string.Empty, fixture.Create<string>(), fixture.Create<string>(), string.Empty, fixture.Create<decimal>().ToString("C"), string.Empty, 0);
            bogføringViewModelMock.Expect(m => m.Regnskab)
                                  .Return(regnskabViewModelMock)
                                  .Repeat.Any();

            var exception = fixture.Create<IntranetGuiRepositoryException>();
            Func<IBogføringsresultatModel> bogføringsresultatGetter = () =>
                {
                    throw exception;
                };
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.BogførAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.Anything, Arg<string>.Is.NotNull, Arg<string>.Is.NotNull, Arg<string>.Is.Anything, Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<int>.Is.Anything))
                                       .Return(Task.Run(bogføringsresultatGetter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiCommandException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var commandException = (IntranetGuiCommandException) e.Arguments.ElementAt(0);
                                                 Assert.That(commandException, Is.Not.Null);
                                                 Assert.That(commandException.Message, Is.Not.Null);
                                                 Assert.That(commandException.Message, Is.Not.Empty);
                                                 Assert.That(commandException.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.ErrorPostingAccountingLine)));
                                                 Assert.That(commandException.Reason, Is.Not.Null);
                                                 Assert.That(commandException.Reason, Is.Not.Empty);
                                                 Assert.That(commandException.Reason, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.ErrorUpdateFinansstyringRepository)));
                                                 Assert.That(commandException.CommandContext, Is.Not.Null);
                                                 Assert.That(commandException.CommandContext, Is.TypeOf<BogføringAddCommand>());
                                                 Assert.That(commandException.ReasonContext, Is.Not.Null);
                                                 Assert.That(commandException.ReasonContext, Is.EqualTo(finansstyringRepositoryMock));
                                                 Assert.That(commandException.InnerException, Is.Not.Null);
                                                 Assert.That(commandException.InnerException, Is.EqualTo(exception));
                                             })
                                         .Repeat.Any();

            var command = new BogføringAddCommand(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);

            Action action = () =>
                {
                    command.Execute(bogføringViewModelMock);
                    Assert.That(command.ExecuteTask, Is.Not.Null);
                    command.ExecuteTask.Wait();
                };
            Task.Run(action).Wait(3000);

            finansstyringRepositoryMock.AssertWasCalled(m => m.BogførAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.Anything, Arg<string>.Is.NotNull, Arg<string>.Is.NotNull, Arg<string>.Is.Anything, Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<int>.Is.Anything));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiCommandException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at Execute kalder HandleException på exceptionhandleren med en IntranetGuiSystemException ved IntranetGuiSystemException.
        /// </summary>
        [Test]
        public void TestAtExecuteKalderHandleExceptionOnExceptionHandlerViewModelMedIntranetGuiSystemExceptionVedIntranetGuiSystemException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(fixture.Create<int>())
                                 .Repeat.Any();

            var bogføringViewModelMock = CreateBogføringViewModelMock(fixture.Create<DateTime>().Date.ToString(CultureInfo.CurrentUICulture), string.Empty, fixture.Create<string>(), fixture.Create<string>(), string.Empty, fixture.Create<decimal>().ToString("C"), string.Empty, 0);
            bogføringViewModelMock.Expect(m => m.Regnskab)
                                  .Return(regnskabViewModelMock)
                                  .Repeat.Any();

            var exception = fixture.Create<IntranetGuiSystemException>();
            Func<IBogføringsresultatModel> bogføringsresultatGetter = () =>
                {
                    throw exception;
                };
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.BogførAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.Anything, Arg<string>.Is.NotNull, Arg<string>.Is.NotNull, Arg<string>.Is.Anything, Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<int>.Is.Anything))
                                       .Return(Task.Run(bogføringsresultatGetter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var systemException = (IntranetGuiSystemException) e.Arguments.ElementAt(0);
                                                 Assert.That(systemException, Is.Not.Null);
                                                 Assert.That(systemException.Message, Is.Not.Null);
                                                 Assert.That(systemException.Message, Is.Not.Empty);
                                                 Assert.That(systemException.Message, Is.EqualTo(exception.Message));
                                                 Assert.That(systemException.InnerException, Is.Null);
                                             })
                                         .Repeat.Any();

            var command = new BogføringAddCommand(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);

            Action action = () =>
                {
                    command.Execute(bogføringViewModelMock);
                    Assert.That(command.ExecuteTask, Is.Not.Null);
                    command.ExecuteTask.Wait();
                };
            Task.Run(action).Wait(3000);

            finansstyringRepositoryMock.AssertWasCalled(m => m.BogførAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.Anything, Arg<string>.Is.NotNull, Arg<string>.Is.NotNull, Arg<string>.Is.Anything, Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<int>.Is.Anything));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at Execute kalder HandleException på exceptionhandleren med en IntranetGuiSystemException ved Exception.
        /// </summary>
        [Test]
        public void TestAtExecuteKalderHandleExceptionOnExceptionHandlerViewModelMedIntranetGuiSystemExceptionVedException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(fixture.Create<int>())
                                 .Repeat.Any();

            var bogføringViewModelMock = CreateBogføringViewModelMock(fixture.Create<DateTime>().Date.ToString(CultureInfo.CurrentUICulture), string.Empty, fixture.Create<string>(), fixture.Create<string>(), string.Empty, fixture.Create<decimal>().ToString("C"), string.Empty, 0);
            bogføringViewModelMock.Expect(m => m.Regnskab)
                                  .Return(regnskabViewModelMock)
                                  .Repeat.Any();

            var exception = fixture.Create<Exception>();
            Func<IBogføringsresultatModel> bogføringsresultatGetter = () =>
                {
                    throw exception;
                };
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.BogførAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.Anything, Arg<string>.Is.NotNull, Arg<string>.Is.NotNull, Arg<string>.Is.Anything, Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<int>.Is.Anything))
                                       .Return(Task.Run(bogføringsresultatGetter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var systemException = (IntranetGuiSystemException) e.Arguments.ElementAt(0);
                                                 Assert.That(systemException, Is.Not.Null);
                                                 Assert.That(systemException.Message, Is.Not.Null);
                                                 Assert.That(systemException.Message, Is.Not.Empty);
                                                 Assert.That(systemException.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.CommandError, "BogføringAddCommand", exception.Message)));
                                                 Assert.That(systemException.InnerException, Is.Not.Null);
                                                 Assert.That(systemException.InnerException, Is.EqualTo(exception));
                                             })
                                         .Repeat.Any();

            var command = new BogføringAddCommand(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);

            Action action = () =>
                {
                    command.Execute(bogføringViewModelMock);
                    Assert.That(command.ExecuteTask, Is.Not.Null);
                    command.ExecuteTask.Wait();
                };
            Task.Run(action).Wait(3000);

            finansstyringRepositoryMock.AssertWasCalled(m => m.BogførAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.Anything, Arg<string>.Is.NotNull, Arg<string>.Is.NotNull, Arg<string>.Is.Anything, Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<int>.Is.Anything));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at Execute kalder HandleException på exceptionhandleren men en IntranetGuiSystemException, hvis der opstår en fejl ved opdatering af ViewModel for regnskabet.
        /// </summary>
        [Test]
        public void TestAtExecuteKalderHandleExceptionOnExceptionHandlerViewModelMedIntranetGuiSystemExceptionHvisRegnskabViewModelOpdateringFejler()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IBogføringslinjeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringslinjeModel>()));

            var exception = fixture.Create<Exception>();
            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(fixture.Create<int>())
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.BogføringSetCommand)
                                 .Return(null)
                                 .Repeat.Any();
            regnskabViewModelMock.Expect(m => m.BogføringslinjeAdd(Arg<IReadOnlyBogføringslinjeViewModel>.Is.NotNull))
                                 .Throw(exception)
                                 .Repeat.Any();

            var bogføringViewModelMock = CreateBogføringViewModelMock(fixture.Create<DateTime>().Date.ToString(CultureInfo.CurrentUICulture), string.Empty, fixture.Create<string>(), fixture.Create<string>(), string.Empty, fixture.Create<decimal>().ToString("C"), string.Empty, 0);
            bogføringViewModelMock.Expect(m => m.Regnskab)
                                  .Return(regnskabViewModelMock)
                                  .Repeat.Any();

            var bogføringsresultatModelMock = MockRepository.GenerateMock<IBogføringsresultatModel>();
            bogføringsresultatModelMock.Expect(m => m.Bogføringslinje)
                                       .Return(fixture.Create<IBogføringslinjeModel>())
                                       .Repeat.Any();
            bogføringsresultatModelMock.Expect(m => m.Bogføringsadvarsler)
                                       .Return(new List<IBogføringsadvarselModel>(0))
                                       .Repeat.Any();

            Func<IBogføringsresultatModel> bogføringsresultatGetter = () => bogføringsresultatModelMock;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.BogførAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.Anything, Arg<string>.Is.NotNull, Arg<string>.Is.NotNull, Arg<string>.Is.Anything, Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<int>.Is.Anything))
                                       .Return(Task.Run(bogføringsresultatGetter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var systemException = (IntranetGuiSystemException) e.Arguments.ElementAt(0);
                                                 Assert.That(systemException, Is.Not.Null);
                                                 Assert.That(systemException.Message, Is.Not.Null);
                                                 Assert.That(systemException.Message, Is.Not.Empty);
                                                 Assert.That(systemException.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.CommandError, "BogføringAddCommand", exception.Message)));
                                                 Assert.That(systemException.InnerException, Is.Not.Null);
                                                 Assert.That(systemException.InnerException, Is.EqualTo(exception));
                                             })
                                         .Repeat.Any();

            var command = new BogføringAddCommand(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);

            Action action = () =>
                {
                    command.Execute(bogføringViewModelMock);
                    Assert.That(command.ExecuteTask, Is.Not.Null);
                    command.ExecuteTask.Wait();
                };
            Task.Run(action).Wait(3000);

            finansstyringRepositoryMock.AssertWasCalled(m => m.BogførAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.Anything, Arg<string>.Is.NotNull, Arg<string>.Is.NotNull, Arg<string>.Is.Anything, Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<int>.Is.Anything));
            regnskabViewModelMock.AssertWasCalled(m => m.BogføringslinjeAdd(Arg<IReadOnlyBogføringslinjeViewModel>.Is.NotNull));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Execute rejser OnError, hvis der opstår en fejl ved bogføring.
        /// </summary>
        [Test]
        public void TestAtExecuteRejserOnErrorVedBogføringsfejl()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(fixture.Create<int>())
                                 .Repeat.Any();

            var bogføringViewModelMock = CreateBogføringViewModelMock(fixture.Create<DateTime>().Date.ToString(CultureInfo.CurrentUICulture), string.Empty, fixture.Create<string>(), fixture.Create<string>(), string.Empty, fixture.Create<decimal>().ToString("C"), string.Empty, 0);
            bogføringViewModelMock.Expect(m => m.Regnskab)
                                  .Return(regnskabViewModelMock)
                                  .Repeat.Any();

            var exception = fixture.Create<Exception>();
            Func<IBogføringsresultatModel> bogføringsresultatGetter = () =>
                {
                    throw exception;
                };
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.BogførAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.Anything, Arg<string>.Is.NotNull, Arg<string>.Is.NotNull, Arg<string>.Is.Anything, Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<int>.Is.Anything))
                                       .Return(Task.Run(bogføringsresultatGetter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var command = new BogføringAddCommand(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);

            var eventCalled = false;
            command.OnError += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.Error, Is.Not.Null);
                    Assert.That(e.Error, Is.TypeOf<IntranetGuiSystemException>());
                    eventCalled = true;
                };

            Assert.That(eventCalled, Is.False);
            Action action = () =>
                {
                    command.Execute(bogføringViewModelMock);
                    Assert.That(command.ExecuteTask, Is.Not.Null);
                    command.ExecuteTask.Wait();
                };
            Task.Run(action).Wait(3000);
            Assert.That(eventCalled, Is.True);

            finansstyringRepositoryMock.AssertWasCalled(m => m.BogførAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.Anything, Arg<string>.Is.NotNull, Arg<string>.Is.NotNull, Arg<string>.Is.Anything, Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<decimal>.Is.GreaterThanOrEqual(0M), Arg<int>.Is.Anything));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf));
        }

        /// <summary>
        /// Danner til brug for test mockup af ViewModel til bogføring-
        /// </summary>
        /// <param name="datoAsText">Tekstangivelse af bogføringsdatoen.</param>
        /// <param name="bilag">Bilag.</param>
        /// <param name="kontonummer">Kontonummer.</param>
        /// <param name="tekst">Tekst.</param>
        /// <param name="budgetkontonummer">Kontonummer for budgetkonto.</param>
        /// <param name="debitAsText">Tekstangivelse af debitbeløb.</param>
        /// <param name="kreditAsText">Tekstangivelse af kreditbeløb.</param>
        /// <param name="adressekonto">Unik identifikation af adressekontoen.</param>
        /// <param name="isWorking">Angivelse af, om der udføres asynkront arbejde fra ViewModel til bogføring.</param>
        /// <returns>Mockup af ViewModel til bogføring.</returns>
        private static IBogføringViewModel CreateBogføringViewModelMock(string datoAsText, string bilag, string kontonummer, string tekst, string budgetkontonummer, string debitAsText, string kreditAsText, int adressekonto, bool isWorking = false)
        {
            var dato = DateTime.MinValue;
            if (string.IsNullOrWhiteSpace(datoAsText) == false)
            {
                DateTime value;
                if (DateTime.TryParse(datoAsText, CultureInfo.CurrentUICulture, DateTimeStyles.None, out value))
                {
                    dato = value;
                }
            }
            var debit = 0M;
            if (string.IsNullOrWhiteSpace(debitAsText) == false)
            {
                decimal value;
                if (decimal.TryParse(debitAsText, NumberStyles.Any, CultureInfo.CurrentUICulture, out value))
                {
                    debit = value;
                }
            }
            var kredit = 0M;
            if (string.IsNullOrWhiteSpace(kreditAsText) == false)
            {
                decimal value;
                if (decimal.TryParse(kreditAsText, NumberStyles.Any, CultureInfo.CurrentUICulture, out value))
                {
                    kredit = value;
                }
            }
            var bogføringViewModelMock = MockRepository.GenerateMock<IBogføringViewModel>();
            bogføringViewModelMock.Expect(m => m.Dato)
                                  .Return(dato)
                                  .Repeat.Any();
            bogføringViewModelMock.Expect(m => m.DatoAsText)
                                  .Return(datoAsText)
                                  .Repeat.Any();
            bogføringViewModelMock.Expect(m => m.Bilag)
                                  .Return(bilag)
                                  .Repeat.Any();
            bogføringViewModelMock.Expect(m => m.Kontonummer)
                                  .Return(kontonummer)
                                  .Repeat.Any();
            bogføringViewModelMock.Expect(m => m.Tekst)
                                  .Return(tekst)
                                  .Repeat.Any();
            bogføringViewModelMock.Expect(m => m.Budgetkontonummer)
                                  .Return(budgetkontonummer)
                                  .Repeat.Any();
            bogføringViewModelMock.Expect(m => m.Debit)
                                  .Return(debit)
                                  .Repeat.Any();
            bogføringViewModelMock.Expect(m => m.DebitAsText)
                                  .Return(debitAsText)
                                  .Repeat.Any();
            bogføringViewModelMock.Expect(m => m.Kredit)
                                  .Return(kredit)
                                  .Repeat.Any();
            bogføringViewModelMock.Expect(m => m.KreditAsText)
                                  .Return(kreditAsText)
                                  .Repeat.Any();
            bogføringViewModelMock.Expect(m => m.Adressekonto)
                                  .Return(adressekonto)
                                  .Repeat.Any();
            bogføringViewModelMock.Expect(m => m.IsWorking)
                                  .Return(isWorking)
                                  .Repeat.Any();
            return bogføringViewModelMock;
        }
    }
}
