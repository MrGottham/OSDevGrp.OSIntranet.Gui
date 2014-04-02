using System;
using System.ComponentModel;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
using Ploeh.AutoFixture;
using Rhino.Mocks;
using Text = OSDevGrp.OSIntranet.Gui.Resources.Text;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Finansstyring
{
    /// <summary>
    /// Tester ViewModel for en adressekonto.
    /// </summary>
    [TestFixture]
    public class AdressekontoViewModelTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en ViewModel for en adressekonto.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererAdressekontoViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<Byte[]>(e => e.FromFactory(() => TestHelper.GetEmbeddedResource((new Resource()).GetType().Assembly, "Images.Adressekonto.png")));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
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

            var regnskabViewModelMock = fixture.Create<IRegnskabViewModel>();
            var adressekontoModelMock = fixture.Create<IAdressekontoModel>();
            var displayName = fixture.Create<string>();
            var image = fixture.Create<byte[]>();
            var adressekontoViewModel = new AdressekontoViewModel(regnskabViewModelMock, adressekontoModelMock, displayName, image, fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(adressekontoViewModel, Is.Not.Null);
            Assert.That(adressekontoViewModel.Regnskab, Is.Not.Null);
            Assert.That(adressekontoViewModel.Regnskab, Is.EqualTo(regnskabViewModelMock));
            Assert.That(adressekontoViewModel.Nummer, Is.EqualTo(adressekontoModelMock.Nummer));
            Assert.That(adressekontoViewModel.Navn, Is.Not.Null);
            Assert.That(adressekontoViewModel.Navn, Is.Not.Empty);
            Assert.That(adressekontoViewModel.Navn, Is.EqualTo(adressekontoModelMock.Navn));
            Assert.That(adressekontoViewModel.PrimærTelefon, Is.Not.Null);
            Assert.That(adressekontoViewModel.PrimærTelefon, Is.Not.Empty);
            Assert.That(adressekontoViewModel.PrimærTelefon, Is.EqualTo(adressekontoModelMock.PrimærTelefon));
            Assert.That(adressekontoViewModel.PrimærTelefonLabel, Is.Not.Null);
            Assert.That(adressekontoViewModel.PrimærTelefonLabel, Is.Not.Empty);
            Assert.That(adressekontoViewModel.PrimærTelefonLabel, Is.EqualTo(Resource.GetText(Text.Phone)));
            Assert.That(adressekontoViewModel.SekundærTelefon, Is.Not.Null);
            Assert.That(adressekontoViewModel.SekundærTelefon, Is.Not.Empty);
            Assert.That(adressekontoViewModel.SekundærTelefon, Is.EqualTo(adressekontoModelMock.SekundærTelefon));
            Assert.That(adressekontoViewModel.SekundærTelefonLabel, Is.Not.Null);
            Assert.That(adressekontoViewModel.SekundærTelefonLabel, Is.Not.Empty);
            Assert.That(adressekontoViewModel.SekundærTelefonLabel, Is.EqualTo(Resource.GetText(Text.Phone)));
            Assert.That(adressekontoViewModel.StatusDato, Is.EqualTo(adressekontoModelMock.StatusDato));
            Assert.That(adressekontoViewModel.Saldo, Is.EqualTo(adressekontoModelMock.Saldo));
            Assert.That(adressekontoViewModel.SaldoAsText, Is.Not.Null);
            Assert.That(adressekontoViewModel.SaldoAsText, Is.Not.Empty);
            Assert.That(adressekontoViewModel.SaldoAsText, Is.EqualTo(adressekontoModelMock.Saldo.ToString("C")));
            Assert.That(adressekontoViewModel.SaldoLabel, Is.Not.Null);
            Assert.That(adressekontoViewModel.SaldoLabel, Is.Not.Empty);
            Assert.That(adressekontoViewModel.SaldoLabel, Is.EqualTo(Resource.GetText(Text.Balance)));
            Assert.That(adressekontoViewModel.DisplayName, Is.Not.Null);
            Assert.That(adressekontoViewModel.DisplayName, Is.Not.Empty);
            Assert.That(adressekontoViewModel.DisplayName, Is.EqualTo(displayName));
            Assert.That(adressekontoViewModel.Image, Is.Not.Null);
            Assert.That(adressekontoViewModel.Image, Is.Not.Empty);
            Assert.That(adressekontoViewModel.Image, Is.EqualTo(image));
            Assert.That(adressekontoViewModel.RefreshCommand, Is.Not.Null);
            Assert.That(adressekontoViewModel.RefreshCommand, Is.TypeOf<AdressekontoGetCommand>());

            adressekontoModelMock.AssertWasCalled(m => m.Nummer);
            adressekontoModelMock.AssertWasCalled(m => m.Navn);
            adressekontoModelMock.AssertWasCalled(m => m.PrimærTelefon);
            adressekontoModelMock.AssertWasCalled(m => m.SekundærTelefon);
            adressekontoModelMock.AssertWasCalled(m => m.StatusDato);
            adressekontoModelMock.AssertWasCalled(m => m.Saldo);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis ViewModel for regnskabet, som adressekontoen skal være tilknyttet, er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisRegnskabViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<Byte[]>(e => e.FromFactory(() => TestHelper.GetEmbeddedResource((new Resource()).GetType().Assembly, "Images.Adressekonto.png")));
            fixture.Customize<IAdressekontoModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IAdressekontoModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new AdressekontoViewModel(null, fixture.Create<IAdressekontoModel>(), fixture.Create<string>(), fixture.Create<byte[]>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("regnskabViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis modellen for adressekontoen er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisAdressekontoModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<Byte[]>(e => e.FromFactory(() => TestHelper.GetEmbeddedResource((new Resource()).GetType().Assembly, "Images.Adressekonto.png")));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new AdressekontoViewModel(fixture.Create<IRegnskabViewModel>(), null, fixture.Create<string>(), fixture.Create<byte[]>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("adressekontoModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis display navnet er invalid
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestAtConstructorKasterArgumentNullExceptionHvisDisplayNameErInvalid(string invalidValue)
        {
            var fixture = new Fixture();
            fixture.Customize<Byte[]>(e => e.FromFactory(() => TestHelper.GetEmbeddedResource((new Resource()).GetType().Assembly, "Images.Adressekonto.png")));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IAdressekontoModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IAdressekontoModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new AdressekontoViewModel(fixture.Create<IRegnskabViewModel>(), fixture.Create<IAdressekontoModel>(), invalidValue, fixture.Create<byte[]>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("displayName"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis billedet, der illustrerer en adressekontoen, er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisImageErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IAdressekontoModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IAdressekontoModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new AdressekontoViewModel(fixture.Create<IRegnskabViewModel>(), fixture.Create<IAdressekontoModel>(), fixture.Create<string>(), null, fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("image"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis repositoryet til finansstyring er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFinansstyringRepositoryErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<Byte[]>(e => e.FromFactory(() => TestHelper.GetEmbeddedResource((new Resource()).GetType().Assembly, "Images.Adressekonto.png")));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IAdressekontoModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IAdressekontoModel>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new AdressekontoViewModel(fixture.Create<IRegnskabViewModel>(), fixture.Create<IAdressekontoModel>(), fixture.Create<string>(), fixture.Create<byte[]>(), null, fixture.Create<IExceptionHandlerViewModel>()));
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
            fixture.Customize<Byte[]>(e => e.FromFactory(() => TestHelper.GetEmbeddedResource((new Resource()).GetType().Assembly, "Images.Adressekonto.png")));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IAdressekontoModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IAdressekontoModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new AdressekontoViewModel(fixture.Create<IRegnskabViewModel>(), fixture.Create<IAdressekontoModel>(), fixture.Create<string>(), fixture.Create<byte[]>(), fixture.Create<IFinansstyringRepository>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("exceptionHandlerViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at sætteren til Navn opdaterer Navn på modellen for adressekontoen.
        /// </summary>
        [Test]
        public void TestAtNavnSetterOpdatererNavnOnAdressekontoModel()
        {
            var fixture = new Fixture();
            fixture.Customize<Byte[]>(e => e.FromFactory(() => TestHelper.GetEmbeddedResource((new Resource()).GetType().Assembly, "Images.Adressekonto.png")));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var adressekontoModelMock = MockRepository.GenerateMock<IAdressekontoModel>();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var adressekontoViewModel = new AdressekontoViewModel(fixture.Create<IRegnskabViewModel>(), adressekontoModelMock, fixture.Create<string>(), fixture.Create<byte[]>(), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(adressekontoViewModel, Is.Not.Null);

            var newValue = fixture.Create<string>();
            adressekontoViewModel.Navn = newValue;

            adressekontoModelMock.AssertWasCalled(m => m.Navn = Arg<string>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at sætteren til Navn kalder HandleException på ViewModel for exceptionhandleren ved exceptions.
        /// </summary>
        [Test]
        public void TestAtNavnSetterKalderHandleExceptionOnExceptionHandlerViewModelVedExceptions()
        {
            var fixture = new Fixture();
            fixture.Customize<Byte[]>(e => e.FromFactory(() => TestHelper.GetEmbeddedResource((new Resource()).GetType().Assembly, "Images.Adressekonto.png")));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = fixture.Create<Exception>();
            var adressekontoModelMock = MockRepository.GenerateMock<IAdressekontoModel>();
            adressekontoModelMock.Expect(m => m.Navn = Arg<string>.Is.Anything)
                                 .Throw(exception)
                                 .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var adressekontoViewModel = new AdressekontoViewModel(fixture.Create<IRegnskabViewModel>(), adressekontoModelMock, fixture.Create<string>(), fixture.Create<byte[]>(), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(adressekontoViewModel, Is.Not.Null);

            var newValue = fixture.Create<string>();
            adressekontoViewModel.Navn = newValue;

            adressekontoModelMock.AssertWasCalled(m => m.Navn = Arg<string>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<Exception>.Is.Equal(exception)));
        }

        /// <summary>
        /// Tester, at sætteren til PrimærTelefon opdaterer PrimærTelefon på modellen for adressekontoen.
        /// </summary>
        [Test]
        public void TestAtPrimærTelefonSetterOpdatererPrimærTelefonOnAdressekontoModel()
        {
            var fixture = new Fixture();
            fixture.Customize<Byte[]>(e => e.FromFactory(() => TestHelper.GetEmbeddedResource((new Resource()).GetType().Assembly, "Images.Adressekonto.png")));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var adressekontoModelMock = MockRepository.GenerateMock<IAdressekontoModel>();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var adressekontoViewModel = new AdressekontoViewModel(fixture.Create<IRegnskabViewModel>(), adressekontoModelMock, fixture.Create<string>(), fixture.Create<byte[]>(), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(adressekontoViewModel, Is.Not.Null);

            var newValue = fixture.Create<string>();
            adressekontoViewModel.PrimærTelefon = newValue;

            adressekontoModelMock.AssertWasCalled(m => m.PrimærTelefon = Arg<string>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at sætteren til PrimærTelefon kalder HandleException på ViewModel for exceptionhandleren ved exceptions.
        /// </summary>
        [Test]
        public void TestAtPrimærTelefonSetterKalderHandleExceptionOnExceptionHandlerViewModelVedExceptions()
        {
            var fixture = new Fixture();
            fixture.Customize<Byte[]>(e => e.FromFactory(() => TestHelper.GetEmbeddedResource((new Resource()).GetType().Assembly, "Images.Adressekonto.png")));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = fixture.Create<Exception>();
            var adressekontoModelMock = MockRepository.GenerateMock<IAdressekontoModel>();
            adressekontoModelMock.Expect(m => m.PrimærTelefon = Arg<string>.Is.Anything)
                                 .Throw(exception)
                                 .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var adressekontoViewModel = new AdressekontoViewModel(fixture.Create<IRegnskabViewModel>(), adressekontoModelMock, fixture.Create<string>(), fixture.Create<byte[]>(), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(adressekontoViewModel, Is.Not.Null);

            var newValue = fixture.Create<string>();
            adressekontoViewModel.PrimærTelefon = newValue;

            adressekontoModelMock.AssertWasCalled(m => m.PrimærTelefon = Arg<string>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<Exception>.Is.Equal(exception)));
        }

        /// <summary>
        /// Tester, at sætteren til SekundærTelefon opdaterer SekundærTelefon på modellen for adressekontoen.
        /// </summary>
        [Test]
        public void TestAtSekundærTelefonSetterOpdatererSekundærTelefonOnAdressekontoModel()
        {
            var fixture = new Fixture();
            fixture.Customize<Byte[]>(e => e.FromFactory(() => TestHelper.GetEmbeddedResource((new Resource()).GetType().Assembly, "Images.Adressekonto.png")));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var adressekontoModelMock = MockRepository.GenerateMock<IAdressekontoModel>();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var adressekontoViewModel = new AdressekontoViewModel(fixture.Create<IRegnskabViewModel>(), adressekontoModelMock, fixture.Create<string>(), fixture.Create<byte[]>(), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(adressekontoViewModel, Is.Not.Null);

            var newValue = fixture.Create<string>();
            adressekontoViewModel.SekundærTelefon = newValue;

            adressekontoModelMock.AssertWasCalled(m => m.SekundærTelefon = Arg<string>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at sætteren til SekundærTelefon kalder HandleException på ViewModel for exceptionhandleren ved exceptions.
        /// </summary>
        [Test]
        public void TestAtSekundærTelefonSetterKalderHandleExceptionOnExceptionHandlerViewModelVedExceptions()
        {
            var fixture = new Fixture();
            fixture.Customize<Byte[]>(e => e.FromFactory(() => TestHelper.GetEmbeddedResource((new Resource()).GetType().Assembly, "Images.Adressekonto.png")));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = fixture.Create<Exception>();
            var adressekontoModelMock = MockRepository.GenerateMock<IAdressekontoModel>();
            adressekontoModelMock.Expect(m => m.SekundærTelefon = Arg<string>.Is.Anything)
                                 .Throw(exception)
                                 .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var adressekontoViewModel = new AdressekontoViewModel(fixture.Create<IRegnskabViewModel>(), adressekontoModelMock, fixture.Create<string>(), fixture.Create<byte[]>(), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(adressekontoViewModel, Is.Not.Null);

            var newValue = fixture.Create<string>();
            adressekontoViewModel.SekundærTelefon = newValue;

            adressekontoModelMock.AssertWasCalled(m => m.SekundærTelefon = Arg<string>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<Exception>.Is.Equal(exception)));
        }

        /// <summary>
        /// Tester, at sætteren til StatusDato opdaterer StatusDato på modellen for adressekontoen.
        /// </summary>
        [Test]
        public void TestAtStatusDatoSetterOpdatererStatusDatoOnAdressekontoModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<Byte[]>(e => e.FromFactory(() => TestHelper.GetEmbeddedResource((new Resource()).GetType().Assembly, "Images.Adressekonto.png")));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var adressekontoModelMock = MockRepository.GenerateMock<IAdressekontoModel>();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var adressekontoViewModel = new AdressekontoViewModel(fixture.Create<IRegnskabViewModel>(), adressekontoModelMock, fixture.Create<string>(), fixture.Create<byte[]>(), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(adressekontoViewModel, Is.Not.Null);

            var newValue = fixture.Create<DateTime>();
            adressekontoViewModel.StatusDato = newValue;

            adressekontoModelMock.AssertWasCalled(m => m.StatusDato = Arg<DateTime>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at sætteren til StatusDato kalder HandleException på ViewModel for exceptionhandleren ved exceptions.
        /// </summary>
        [Test]
        public void TestAtStatusDatoSetterKalderHandleExceptionOnExceptionHandlerViewModelVedExceptions()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<Byte[]>(e => e.FromFactory(() => TestHelper.GetEmbeddedResource((new Resource()).GetType().Assembly, "Images.Adressekonto.png")));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = fixture.Create<Exception>();
            var adressekontoModelMock = MockRepository.GenerateMock<IAdressekontoModel>();
            adressekontoModelMock.Expect(m => m.StatusDato = Arg<DateTime>.Is.Anything)
                                 .Throw(exception)
                                 .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var adressekontoViewModel = new AdressekontoViewModel(fixture.Create<IRegnskabViewModel>(), adressekontoModelMock, fixture.Create<string>(), fixture.Create<byte[]>(), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(adressekontoViewModel, Is.Not.Null);

            var newValue = fixture.Create<DateTime>();
            adressekontoViewModel.StatusDato = newValue;

            adressekontoModelMock.AssertWasCalled(m => m.StatusDato = Arg<DateTime>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<Exception>.Is.Equal(exception)));
        }

        /// <summary>
        /// Tester, at sætteren til Saldo opdaterer Saldo på modellen for adressekontoen.
        /// </summary>
        [Test]
        public void TestAtSaldoSetterOpdatererSaldoOnAdressekontoModel()
        {
            var fixture = new Fixture();
            fixture.Customize<Byte[]>(e => e.FromFactory(() => TestHelper.GetEmbeddedResource((new Resource()).GetType().Assembly, "Images.Adressekonto.png")));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var adressekontoModelMock = MockRepository.GenerateMock<IAdressekontoModel>();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var adressekontoViewModel = new AdressekontoViewModel(fixture.Create<IRegnskabViewModel>(), adressekontoModelMock, fixture.Create<string>(), fixture.Create<byte[]>(), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(adressekontoViewModel, Is.Not.Null);

            var newValue = fixture.Create<decimal>();
            adressekontoViewModel.Saldo = newValue;

            adressekontoModelMock.AssertWasCalled(m => m.Saldo = Arg<decimal>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at sætteren til Saldo kalder HandleException på ViewModel for exceptionhandleren ved exceptions.
        /// </summary>
        [Test]
        public void TestAtSaldoSetterKalderHandleExceptionOnExceptionHandlerViewModelVedExceptions()
        {
            var fixture = new Fixture();
            fixture.Customize<Byte[]>(e => e.FromFactory(() => TestHelper.GetEmbeddedResource((new Resource()).GetType().Assembly, "Images.Adressekonto.png")));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = fixture.Create<Exception>();
            var adressekontoModelMock = MockRepository.GenerateMock<IAdressekontoModel>();
            adressekontoModelMock.Expect(m => m.Saldo = Arg<decimal>.Is.Anything)
                                 .Throw(exception)
                                 .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var adressekontoViewModel = new AdressekontoViewModel(fixture.Create<IRegnskabViewModel>(), adressekontoModelMock, fixture.Create<string>(), fixture.Create<byte[]>(), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(adressekontoViewModel, Is.Not.Null);

            var newValue = fixture.Create<decimal>();
            adressekontoViewModel.Saldo = newValue;

            adressekontoModelMock.AssertWasCalled(m => m.Saldo = Arg<decimal>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<Exception>.Is.Equal(exception)));
        }

        /// <summary>
        /// Tester, at PropertyChangedOnAdressekontoModelEventHandler rejser PropertyChanged, når modellen for adressekontoen opdateres.
        /// </summary>
        [Test]
        [TestCase("Regnskabsnummer", "Regnskabsnummer")]
        [TestCase("Nummer", "Nummer")]
        [TestCase("Navn", "Navn")]
        [TestCase("PrimærTelefon", "PrimærTelefon")]
        [TestCase("SekundærTelefon", "SekundærTelefon")]
        [TestCase("StatusDato", "StatusDato")]
        [TestCase("Saldo", "Saldo")]
        [TestCase("Saldo", "SaldoAsText")]
        [TestCase("Nyhedsaktualitet", "Nyhedsaktualitet")]
        [TestCase("Nyhedsudgivelsestidspunkt", "Nyhedsudgivelsestidspunkt")]
        [TestCase("Nyhedsinformation", "Nyhedsinformation")]
        public void TestAtPropertyChangedOnAdressekontoModelEventHandlerRejserPropertyChangedOnAdressekontoModelUpdate(string propertyNameToRaise, string expectPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<Byte[]>(e => e.FromFactory(() => TestHelper.GetEmbeddedResource((new Resource()).GetType().Assembly, "Images.Adressekonto.png")));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var adressekontoModelMock = MockRepository.GenerateMock<IAdressekontoModel>();
            var adressekontoViewModel = new AdressekontoViewModel(fixture.Create<IRegnskabViewModel>(), adressekontoModelMock, fixture.Create<string>(), fixture.Create<byte[]>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(adressekontoViewModel, Is.Not.Null);

            var eventCalled = false;
            adressekontoViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, expectPropertyName, StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            adressekontoModelMock.Raise(m => m.PropertyChanged += null, adressekontoModelMock, new PropertyChangedEventArgs(propertyNameToRaise));
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnAdressekontoModelEventHandler kaster en ArgumentNullException, hvis objektet, der rejser eventet, er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnAdressekontoModelEventHandlerKasterArgumentNullExceptionHvisSenderErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<Byte[]>(e => e.FromFactory(() => TestHelper.GetEmbeddedResource((new Resource()).GetType().Assembly, "Images.Adressekonto.png")));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var adressekontoModelMock = MockRepository.GenerateMock<IAdressekontoModel>();
            var adressekontoViewModel = new AdressekontoViewModel(fixture.Create<IRegnskabViewModel>(), adressekontoModelMock, fixture.Create<string>(), fixture.Create<byte[]>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(adressekontoViewModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => adressekontoModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("sender"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnAdressekontoModelEventHandler kaster en ArgumentNullException, hvis argumenter til eventet er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnAdressekontoModelEventHandlerKasterArgumentNullExceptionHvisEventArgsErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<Byte[]>(e => e.FromFactory(() => TestHelper.GetEmbeddedResource((new Resource()).GetType().Assembly, "Images.Adressekonto.png")));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var adressekontoModelMock = MockRepository.GenerateMock<IAdressekontoModel>();
            var adressekontoViewModel = new AdressekontoViewModel(fixture.Create<IRegnskabViewModel>(), adressekontoModelMock, fixture.Create<string>(), fixture.Create<byte[]>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(adressekontoViewModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => adressekontoModelMock.Raise(m => m.PropertyChanged += null, fixture.Create<object>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("eventArgs"));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
