using System;
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
    /// Tester kommando, der kan hente og opdaterer en adressekonto.
    /// </summary>
    [TestFixture]
    public class AdressekontoGetCommandTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en kommando, der kan hente og opdaterer en adressekonto.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererAdressekontoGetCommand()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var command = new AdressekontoGetCommand(fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
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

            var exception = Assert.Throws<ArgumentNullException>(() => new AdressekontoGetCommand(null, fixture.Create<IExceptionHandlerViewModel>()));
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

            var exception = Assert.Throws<ArgumentNullException>(() => new AdressekontoGetCommand(fixture.Create<IFinansstyringRepository>(), null));
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
            fixture.Customize<IAdressekontoViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IAdressekontoViewModel>()));

            var command = new AdressekontoGetCommand(fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(command, Is.Not.Null);

            var result = command.CanExecute(fixture.Create<IAdressekontoViewModel>());
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tester, at Execute henter og opdaterer ViewModel for adressekontoen.
        /// </summary>
        [Test]
        public void TestAtExecuteHenterOgOpdatererAdressekontoViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var adressekontoModelMock = MockRepository.GenerateMock<IAdressekontoModel>();
            adressekontoModelMock.Expect(m => m.Navn)
                                 .Return(fixture.Create<string>())
                                 .Repeat.Any();
            adressekontoModelMock.Expect(m => m.PrimærTelefon)
                                 .Return(fixture.Create<string>())
                                 .Repeat.Any();
            adressekontoModelMock.Expect(m => m.SekundærTelefon)
                                 .Return(fixture.Create<string>())
                                 .Repeat.Any();
            adressekontoModelMock.Expect(m => m.StatusDato)
                                 .Return(fixture.Create<DateTime>())
                                 .Repeat.Any();
            adressekontoModelMock.Expect(m => m.Saldo)
                                 .Return(fixture.Create<decimal>())
                                 .Repeat.Any();

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(fixture.Create<int>())
                                 .Repeat.Any();

            var adressekontoViewModelMock = MockRepository.GenerateMock<IAdressekontoViewModel>();
            adressekontoViewModelMock.Expect(m => m.Regnskab)
                                     .Return(regnskabViewModelMock)
                                     .Repeat.Any();
            adressekontoViewModelMock.Expect(m => m.Nummer)
                                     .Return(fixture.Create<int>())
                                     .Repeat.Any();
            adressekontoViewModelMock.Expect(m => m.StatusDato)
                                     .Return(fixture.Create<DateTime>())
                                     .Repeat.Any();

            Func<IAdressekontoModel> getter = () => adressekontoModelMock;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.AdressekontoGetAsync(Arg<int>.Is.GreaterThan(0), Arg<int>.Is.GreaterThan(0),
                                       Arg<DateTime>.Is.GreaterThan(DateTime.MinValue)))
                                       .Return(Task.Run(getter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var command = new AdressekontoGetCommand(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);
            Assert.That(command.ExecuteTask, Is.Null);

            // ReSharper disable ImplicitlyCapturedClosure
            Action action = () =>
                {
                    command.Execute(adressekontoViewModelMock);
                    Assert.That(command.ExecuteTask, Is.Not.Null);
                    command.ExecuteTask.Wait();
                };
            // ReSharper restore ImplicitlyCapturedClosure
            Task.Run(action).Wait(3000);

            // ReSharper disable ImplicitlyCapturedClosure
            finansstyringRepositoryMock.AssertWasCalled(m => m.AdressekontoGetAsync(Arg<int>.Is.Equal(regnskabViewModelMock.Nummer), Arg<int>.Is.Equal(adressekontoViewModelMock.Nummer), Arg<DateTime>.Is.Equal(adressekontoViewModelMock.StatusDato)));
            // ReSharper restore ImplicitlyCapturedClosure
            adressekontoViewModelMock.AssertWasCalled(m => m.Navn = Arg<string>.Is.Equal(adressekontoModelMock.Navn));
            adressekontoViewModelMock.AssertWasCalled(m => m.PrimærTelefon = Arg<string>.Is.Equal(adressekontoModelMock.PrimærTelefon));
            adressekontoViewModelMock.AssertWasCalled(m => m.SekundærTelefon = Arg<string>.Is.Equal(adressekontoModelMock.SekundærTelefon));
            adressekontoViewModelMock.AssertWasCalled(m => m.StatusDato = Arg<DateTime>.Is.Equal(adressekontoModelMock.StatusDato));
            adressekontoViewModelMock.AssertWasCalled(m => m.Saldo = Arg<decimal>.Is.Equal(adressekontoModelMock.Saldo));
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

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(fixture.Create<int>())
                                 .Repeat.Any();

            var adressekontoViewModelMock = MockRepository.GenerateMock<IAdressekontoViewModel>();
            adressekontoViewModelMock.Expect(m => m.Regnskab)
                                     .Return(regnskabViewModelMock)
                                     .Repeat.Any();
            adressekontoViewModelMock.Expect(m => m.Nummer)
                                     .Return(fixture.Create<int>())
                                     .Repeat.Any();
            adressekontoViewModelMock.Expect(m => m.StatusDato)
                                     .Return(fixture.Create<DateTime>())
                                     .Repeat.Any();

            Func<IAdressekontoModel> getter = () =>
                {
                    throw new IntranetGuiRepositoryException(fixture.Create<string>());
                };
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.AdressekontoGetAsync(Arg<int>.Is.GreaterThan(0), Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue)))
                                       .Return(Task.Run(getter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var command = new AdressekontoGetCommand(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);
            Assert.That(command.ExecuteTask, Is.Null);

            // ReSharper disable ImplicitlyCapturedClosure
            Action action = () =>
                {
                    command.Execute(adressekontoViewModelMock);
                    Assert.That(command.ExecuteTask, Is.Not.Null);
                    command.ExecuteTask.Wait();
                };
            // ReSharper restore ImplicitlyCapturedClosure
            Task.Run(action).Wait(3000);

            // ReSharper disable ImplicitlyCapturedClosure
            finansstyringRepositoryMock.AssertWasCalled(m => m.AdressekontoGetAsync(Arg<int>.Is.Equal(regnskabViewModelMock.Nummer), Arg<int>.Is.Equal(adressekontoViewModelMock.Nummer), Arg<DateTime>.Is.Equal(adressekontoViewModelMock.StatusDato)));
            // ReSharper restore ImplicitlyCapturedClosure
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiRepositoryException>.Is.TypeOf));
        }
    }
}
