using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
using Ploeh.AutoFixture;
using Rhino.Mocks;
using Text=OSDevGrp.OSIntranet.Gui.Resources.Text;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Finansstyring
{
    /// <summary>
    /// Tester ViewModel for en linje i balancen.
    /// </summary>
    [TestFixture]
    public class BalanceViewModelTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en ViewModel til en linje i balancen.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererBalanceViewModel()
        {
            var fixture = new Fixture();

            var kontogruppeModelMock = MockRepository.GenerateMock<IKontogruppeModel>();
            kontogruppeModelMock.Expect(m => m.Id)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            kontogruppeModelMock.Expect(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();
            kontogruppeModelMock.Expect(m => m.Tekst)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            kontogruppeModelMock.Expect(m => m.Balancetype)
                .Return(fixture.Create<Balancetype>())
                .Repeat.Any();

            var kontogruppeViewModelMock = MockRepository.GenerateMock<IKontogruppeViewModel>();
            kontogruppeViewModelMock.Expect(m => m.Nummer)
                .Return(kontogruppeModelMock.Nummer)
                .Repeat.Any();

            var kontoViewModelCollection =new List<IKontoViewModel>(fixture.Create<Generator<int>>().First(m => m >= 25 && m <= 100));
            while (kontoViewModelCollection.Count < kontoViewModelCollection.Capacity)
            {
                var kontoViewModelMock = MockRepository.GenerateMock<IKontoViewModel>();
                kontoViewModelMock.Expect(m => m.Kontonummer)
                    .Return(fixture.Create<string>())
                    .Repeat.Any();
                kontoViewModelMock.Expect(m => m.Kontogruppe)
                    .Return(kontogruppeViewModelMock)
                    .Repeat.Any();
                kontoViewModelMock.Expect(m => m.Kredit)
                    .Return(fixture.Create<decimal>())
                    .Repeat.Any();
                kontoViewModelMock.Expect(m => m.Saldo)
                    .Return(fixture.Create<decimal>())
                    .Repeat.Any();
                kontoViewModelMock.Expect(m => m.Disponibel)
                    .Return(fixture.Create<decimal>())
                    .Repeat.Any();
                kontoViewModelMock.Expect(m => m.ErRegistreret)
                    .Return(true)
                    .Repeat.Any();
                kontoViewModelCollection.Add(kontoViewModelMock);
            }
            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Konti)
                .Return(kontoViewModelCollection)
                .Repeat.Any();

            var exceptionHandleViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var balanceViewModel = new BalanceViewModel(regnskabViewModelMock, kontogruppeModelMock, exceptionHandleViewModelMock);
            Assert.That(balanceViewModel, Is.Not.Null);
            Assert.That(balanceViewModel.Id, Is.Not.Null);
            Assert.That(balanceViewModel.Id, Is.Not.Empty);
            Assert.That(balanceViewModel.Id, Is.EqualTo(kontogruppeModelMock.Id));
            Assert.That(balanceViewModel.Nummer, Is.EqualTo(kontogruppeModelMock.Nummer));
            Assert.That(balanceViewModel.Tekst, Is.Not.Null);
            Assert.That(balanceViewModel.Tekst, Is.Not.Empty);
            Assert.That(balanceViewModel.Tekst, Is.EqualTo(kontogruppeModelMock.Tekst));
            Assert.That(balanceViewModel.DisplayName, Is.Not.Null);
            Assert.That(balanceViewModel.DisplayName, Is.Not.Empty);
            Assert.That(balanceViewModel.DisplayName, Is.EqualTo(kontogruppeModelMock.Tekst));
            Assert.That(balanceViewModel.Balancetype, Is.EqualTo(kontogruppeModelMock.Balancetype));
            Assert.That(balanceViewModel.Konti, Is.Not.Null);
            Assert.That(balanceViewModel.Konti, Is.Not.Empty);
            Assert.That(balanceViewModel.Konti.Count(), Is.EqualTo(kontoViewModelCollection.Count));
            Assert.That(balanceViewModel.Kredit, Is.EqualTo(kontoViewModelCollection.Sum(m => m.Kredit)));
            Assert.That(balanceViewModel.KreditAsText, Is.Not.Null);
            Assert.That(balanceViewModel.KreditAsText, Is.Not.Null);
            Assert.That(balanceViewModel.KreditAsText, Is.EqualTo(kontoViewModelCollection.Sum(m => m.Kredit).ToString("C")));
            Assert.That(balanceViewModel.KreditLabel, Is.Not.Null);
            Assert.That(balanceViewModel.KreditLabel, Is.Not.Null);
            Assert.That(balanceViewModel.KreditLabel, Is.EqualTo(Resource.GetText(Text.Credit)));
            Assert.That(balanceViewModel.Saldo, Is.EqualTo(kontoViewModelCollection.Sum(m => m.Saldo)));
            Assert.That(balanceViewModel.SaldoAsText, Is.Not.Null);
            Assert.That(balanceViewModel.SaldoAsText, Is.Not.Null);
            Assert.That(balanceViewModel.SaldoAsText, Is.EqualTo(kontoViewModelCollection.Sum(m => m.Saldo).ToString("C")));
            Assert.That(balanceViewModel.SaldoLabel, Is.Not.Null);
            Assert.That(balanceViewModel.SaldoLabel, Is.Not.Null);
            Assert.That(balanceViewModel.SaldoLabel, Is.EqualTo(Resource.GetText(Text.Balance)));
            Assert.That(balanceViewModel.Disponibel, Is.EqualTo(kontoViewModelCollection.Sum(m => m.Disponibel)));
            Assert.That(balanceViewModel.DisponibelAsText, Is.Not.Null);
            Assert.That(balanceViewModel.DisponibelAsText, Is.Not.Null);
            Assert.That(balanceViewModel.DisponibelAsText, Is.EqualTo(kontoViewModelCollection.Sum(m => m.Disponibel).ToString("C")));
            Assert.That(balanceViewModel.DisponibelLabel, Is.Not.Null);
            Assert.That(balanceViewModel.DisponibelLabel, Is.Not.Null);
            Assert.That(balanceViewModel.DisponibelLabel, Is.EqualTo(Resource.GetText(Text.Available)));

            kontogruppeModelMock.AssertWasCalled(m => m.Id);
            kontogruppeModelMock.AssertWasCalled(m => m.Nummer);
            kontogruppeModelMock.AssertWasCalled(m => m.Tekst);
            kontogruppeModelMock.AssertWasCalled(m => m.Balancetype);
            
            regnskabViewModelMock.AssertWasCalled(m => m.Konti);

            exceptionHandleViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis ViewModel for regnskabet, som balancelinjen er tilknyttet, er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisRegnskabViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IKontogruppeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeModel>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new BalanceViewModel(null, fixture.Create<IKontogruppeModel>(), fixture.Create<IExceptionHandlerViewModel>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("regnskabViewModel"));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis modellen for gruppen af konti, som balancelinjen baserer sig på, er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisKontogruppeModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new BalanceViewModel(fixture.Create<IRegnskabViewModel>(), null, fixture.Create<IExceptionHandlerViewModel>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("tabelModel"));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis ViewModel for exceptionhandleren er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisExceptionHandlerViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IKontogruppeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new BalanceViewModel(fixture.Create<IRegnskabViewModel>(), fixture.Create<IKontogruppeModel>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("exceptionHandlerViewModel"));
        }

        /// <summary>
        /// Tester, at sætteren til Tekst opdaterer Tekst på modellen for gruppen af konti, som balancelinjen baserer sig på.
        /// </summary>
        [Test]
        public void TestAtTekstSetterOpdatererTekstOnBalanceViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var kontogruppeModelMock = MockRepository.GenerateMock<IKontogruppeModel>();
            var exceptionHandleViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var balanceViewModel = new BalanceViewModel(fixture.Create<IRegnskabViewModel>(), kontogruppeModelMock, exceptionHandleViewModelMock);
            Assert.That(balanceViewModel, Is.Not.Null);

            var newValue = fixture.Create<string>();
            balanceViewModel.Tekst = newValue;

            kontogruppeModelMock.AssertWasCalled(m => m.Tekst = Arg<string>.Is.Equal(newValue));
            exceptionHandleViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at sætteren til Tekst kalder HandleException på exceptionhandleren ved exceptions.
        /// </summary>
        [Test]
        public void TestAtTekstSetterKalderHandleExceptionOnExceptionHandlerViewModelVedExceptions()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = fixture.Create<Exception>();
            var kontogruppeModelMock = MockRepository.GenerateMock<IKontogruppeModel>();
            kontogruppeModelMock.Expect(m => m.Tekst = Arg<string>.Is.Anything)
                .Throw(exception)
                .Repeat.Any();

            var exceptionHandleViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var balanceViewModel = new BalanceViewModel(fixture.Create<IRegnskabViewModel>(), kontogruppeModelMock, exceptionHandleViewModelMock);
            Assert.That(balanceViewModel, Is.Not.Null);

            var newValue = fixture.Create<string>();
            balanceViewModel.Tekst = newValue;

            kontogruppeModelMock.AssertWasCalled(m => m.Tekst = Arg<string>.Is.Equal(newValue));
            exceptionHandleViewModelMock.AssertWasCalled(m => m.HandleException(Arg<Exception>.Is.Equal(exception)));
        }

        /// <summary>
        /// Tester, at sætteren til Balancetype opdaterer Balancetype på modellen for gruppen af konti, som balancelinjen baserer sig på.
        /// </summary>
        [Test]
        public void TestAtBalancetypeSetterOpdatererBalancetypeOnBalanceViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var kontogruppeModelMock = MockRepository.GenerateMock<IKontogruppeModel>();
            kontogruppeModelMock.Expect(m => m.Balancetype)
                .Return(Balancetype.Aktiver)
                .Repeat.Any();

            var exceptionHandleViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var balanceViewModel = new BalanceViewModel(fixture.Create<IRegnskabViewModel>(), kontogruppeModelMock, exceptionHandleViewModelMock);
            Assert.That(balanceViewModel, Is.Not.Null);
            Assert.That(balanceViewModel.Balancetype, Is.Not.EqualTo(Balancetype.Passiver));

            balanceViewModel.Balancetype = Balancetype.Passiver;

            kontogruppeModelMock.AssertWasCalled(m => m.Balancetype = Arg<Balancetype>.Is.Equal(Balancetype.Passiver));
            exceptionHandleViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at sætteren til Balancetype kalder HandleException på exceptionhandleren ved exceptions.
        /// </summary>
        [Test]
        public void TestAtBalancetypeSetterKalderHandleExceptionOnExceptionHandlerViewModelVedExceptions()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = fixture.Create<Exception>();
            var kontogruppeModelMock = MockRepository.GenerateMock<IKontogruppeModel>();
            kontogruppeModelMock.Expect(m => m.Balancetype)
                .Return(Balancetype.Aktiver)
                .Repeat.Any();
            kontogruppeModelMock.Expect(m => m.Balancetype = Arg<Balancetype>.Is.Anything)
                .Throw(exception)
                .Repeat.Any();

            var exceptionHandleViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var balanceViewModel = new BalanceViewModel(fixture.Create<IRegnskabViewModel>(), kontogruppeModelMock, exceptionHandleViewModelMock);
            Assert.That(balanceViewModel, Is.Not.Null);
            Assert.That(balanceViewModel.Balancetype, Is.Not.EqualTo(Balancetype.Passiver));

            balanceViewModel.Balancetype = Balancetype.Passiver;

            kontogruppeModelMock.AssertWasCalled(m => m.Balancetype = Arg<Balancetype>.Is.Equal(Balancetype.Passiver));
            exceptionHandleViewModelMock.AssertWasCalled(m => m.HandleException(Arg<Exception>.Is.Equal(exception)));
        }

        /// <summary>
        /// Tester, at Register kaster en ArgumentNullException, hvis kontoen, der skal indgå i balancelinjen, er null.
        /// </summary>
        [Test]
        public void TestAtRegisterKasterArgumentNullExceptionHvisKontoViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IKontogruppeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeModel>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var balanceViewModel = new BalanceViewModel(fixture.Create<IRegnskabViewModel>(), fixture.Create<IKontogruppeModel>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(balanceViewModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => balanceViewModel.Register(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("kontoViewModel"));
        }

        /// <summary>
        /// Tester, at Register returnerer, hvis kontoen ikke er tilknyttet regnskabet.
        /// </summary>
        [Test]
        public void TestAtRegisterReturnererHvisKontoViewModelIkkeErTilknyttetRegnskabViewModel()
        {
            var fixture = new Fixture();

            var kontogruppeModelMock = MockRepository.GenerateMock<IKontogruppeModel>();
            kontogruppeModelMock.Expect(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();

            var kontogruppeViewModelMock = MockRepository.GenerateMock<IKontogruppeViewModel>();
            kontogruppeViewModelMock.Expect(m => m.Nummer)
                .Return(kontogruppeModelMock.Nummer)
                .Repeat.Any();

            var kontoViewModel = MockRepository.GenerateMock<IKontoViewModel>();
            kontoViewModel.Expect(m => m.Kontogruppe)
                .Return(kontogruppeViewModelMock)
                .Repeat.Any();
            kontoViewModel.Expect(m => m.ErRegistreret)
                .Return(false)
                .Repeat.Any();

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Konti)
                .Return(new List<IKontoViewModel>(0))
                .Repeat.Any();

            var exceptionHandlerViewModel = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var balanceViewModel = new BalanceViewModel(regnskabViewModelMock, kontogruppeModelMock, exceptionHandlerViewModel);
            Assert.That(balanceViewModel, Is.Not.Null);

            balanceViewModel.Register(kontoViewModel);

            kontoViewModel.AssertWasNotCalled(m => m.ErRegistreret = Arg<bool>.Is.Anything);
            exceptionHandlerViewModel.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Register returnerer, hvis kontogruppen på kontoen er null.
        /// </summary>
        [Test]
        public void TestAtRegisterReturnererHvisKontogruppeOnKontoViewModelErNull()
        {
            var fixture = new Fixture();

            var kontogruppeModelMock = MockRepository.GenerateMock<IKontogruppeModel>();
            kontogruppeModelMock.Expect(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();

            var kontoViewModel = MockRepository.GenerateMock<IKontoViewModel>();
            kontoViewModel.Expect(m => m.Kontogruppe)
                .Return(null)
                .Repeat.Any();
            kontoViewModel.Expect(m => m.ErRegistreret)
                .Return(false)
                .Repeat.Any();

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Konti)
                .Return(new List<IKontoViewModel> {kontoViewModel})
                .Repeat.Any();

            var exceptionHandlerViewModel = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var balanceViewModel = new BalanceViewModel(regnskabViewModelMock, kontogruppeModelMock, exceptionHandlerViewModel);
            Assert.That(balanceViewModel, Is.Not.Null);

            balanceViewModel.Register(kontoViewModel);

            kontoViewModel.AssertWasNotCalled(m => m.ErRegistreret = Arg<bool>.Is.Anything);
            exceptionHandlerViewModel.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Register returnerer, hvis kontogruppen på kontoen ikke matcher kontogruppen for konti, som balancelinjen baserer sig på.
        /// </summary>
        [Test]
        public void TestAtRegisterReturnererHvisKontogruppeOnKontoViewModelIkkeMatcherBalanceViewModel()
        {
            var fixture = new Fixture();

            var kontogruppeModelMock = MockRepository.GenerateMock<IKontogruppeModel>();
            kontogruppeModelMock.Expect(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();

            var kontogruppeViewModelMock = MockRepository.GenerateMock<IKontogruppeViewModel>();
            kontogruppeViewModelMock.Expect(m => m.Nummer)
                .Return(fixture.Create<Generator<int>>().First(m => m != kontogruppeModelMock.Nummer))
                .Repeat.Any();

            var kontoViewModel = MockRepository.GenerateMock<IKontoViewModel>();
            kontoViewModel.Expect(m => m.Kontogruppe)
                .Return(kontogruppeViewModelMock)
                .Repeat.Any();
            kontoViewModel.Expect(m => m.ErRegistreret)
                .Return(false)
                .Repeat.Any();

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Konti)
                .Return(new List<IKontoViewModel> {kontoViewModel})
                .Repeat.Any();

            var exceptionHandlerViewModel = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var balanceViewModel = new BalanceViewModel(regnskabViewModelMock, kontogruppeModelMock, exceptionHandlerViewModel);
            Assert.That(balanceViewModel, Is.Not.Null);

            balanceViewModel.Register(kontoViewModel);

            kontoViewModel.AssertWasNotCalled(m => m.ErRegistreret = Arg<bool>.Is.Anything);
            exceptionHandlerViewModel.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Register returnerer, hvis kontoen allerede er registreret til brug på balancerlinjer.
        /// </summary>
        [Test]
        public void TestAtRegisterReturnererHvisErRegistreretOnKontoViewModelErTrue()
        {
            var fixture = new Fixture();

            var kontogruppeModelMock = MockRepository.GenerateMock<IKontogruppeModel>();
            kontogruppeModelMock.Expect(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();

            var kontogruppeViewModelMock = MockRepository.GenerateMock<IKontogruppeViewModel>();
            kontogruppeViewModelMock.Expect(m => m.Nummer)
                .Return(kontogruppeModelMock.Nummer)
                .Repeat.Any();

            var kontoViewModel = MockRepository.GenerateMock<IKontoViewModel>();
            kontoViewModel.Expect(m => m.Kontogruppe)
                .Return(kontogruppeViewModelMock)
                .Repeat.Any();
            kontoViewModel.Expect(m => m.ErRegistreret)
                .Return(true)
                .Repeat.Any();

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Konti)
                .Return(new List<IKontoViewModel> {kontoViewModel})
                .Repeat.Any();

            var exceptionHandlerViewModel = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var balanceViewModel = new BalanceViewModel(regnskabViewModelMock, kontogruppeModelMock, exceptionHandlerViewModel);
            Assert.That(balanceViewModel, Is.Not.Null);

            balanceViewModel.Register(kontoViewModel);

            kontoViewModel.AssertWasNotCalled(m => m.ErRegistreret = Arg<bool>.Is.Anything);
            exceptionHandlerViewModel.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Register registrerer kontoen til brug i den aktuelle balancelinje.
        /// </summary>
        [Test]
        public void TestAtRegisterRegistrererKontoViewModelTilBrugForBalanceViewModel()
        {
            var fixture = new Fixture();

            var kontogruppeModelMock = MockRepository.GenerateMock<IKontogruppeModel>();
            kontogruppeModelMock.Expect(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();

            var kontogruppeViewModelMock = MockRepository.GenerateMock<IKontogruppeViewModel>();
            kontogruppeViewModelMock.Expect(m => m.Nummer)
                .Return(kontogruppeModelMock.Nummer)
                .Repeat.Any();

            var kontoViewModel = MockRepository.GenerateMock<IKontoViewModel>();
            kontoViewModel.Expect(m => m.Kontogruppe)
                .Return(kontogruppeViewModelMock)
                .Repeat.Any();
            kontoViewModel.Expect(m => m.ErRegistreret)
                .Return(false)
                .Repeat.Any();

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Konti)
                .Return(new List<IKontoViewModel> {kontoViewModel})
                .Repeat.Any();

            var exceptionHandlerViewModel = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var balanceViewModel = new BalanceViewModel(regnskabViewModelMock, kontogruppeModelMock, exceptionHandlerViewModel);
            Assert.That(balanceViewModel, Is.Not.Null);

            balanceViewModel.Register(kontoViewModel);

            kontoViewModel.AssertWasCalled(m => m.ErRegistreret = Arg<bool>.Is.Equal(true));
            exceptionHandlerViewModel.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Register rejser PropertyChanged ved registrering af en konto til brug i den aktuelle balancelinje.
        /// </summary>
        [Test]
        [TestCase("Konti")]
        [TestCase("Kredit")]
        [TestCase("KreditAsText")]
        [TestCase("Saldo")]
        [TestCase("SaldoAsText")]
        [TestCase("Disponibel")]
        [TestCase("DisponibelAsText")]
        public void TestAtRegisterRejserPropertyChangedVedRegistreringAfKontoViewModelTilBrugForBalanceViewModel(string expectPropertyName)
        {
            var fixture = new Fixture();

            var kontogruppeModelMock = MockRepository.GenerateMock<IKontogruppeModel>();
            kontogruppeModelMock.Expect(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();

            var kontogruppeViewModelMock = MockRepository.GenerateMock<IKontogruppeViewModel>();
            kontogruppeViewModelMock.Expect(m => m.Nummer)
                .Return(kontogruppeModelMock.Nummer)
                .Repeat.Any();

            var kontoViewModelMock = MockRepository.GenerateMock<IKontoViewModel>();
            kontoViewModelMock.Expect(m => m.Kontogruppe)
                .Return(kontogruppeViewModelMock)
                .Repeat.Any();
            kontoViewModelMock.Expect(m => m.ErRegistreret)
                .Return(false)
                .Repeat.Any();

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Konti)
                .Return(new List<IKontoViewModel> {kontoViewModelMock})
                .Repeat.Any();

            var exceptionHandlerViewModel = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var balanceViewModel = new BalanceViewModel(regnskabViewModelMock, kontogruppeModelMock, exceptionHandlerViewModel);
            Assert.That(balanceViewModel, Is.Not.Null);

            var eventCalled = false;
            balanceViewModel.PropertyChanged += (s, e) =>
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
            balanceViewModel.Register(kontoViewModelMock);
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnRegnskabViewModelEventHandler rejser PropertyChanged, når ViewModel for regnskabet opdateres.
        /// </summary>
        [Test]
        [TestCase("Konti", "Konti")]
        [TestCase("Konti", "Kredit")]
        [TestCase("Konti", "KreditAsText")]
        [TestCase("Konti", "Saldo")]
        [TestCase("Konti", "SaldoAsText")]
        [TestCase("Konti", "Disponibel")]
        [TestCase("Konti", "DisponibelAsText")]
        public void TestAtPropertyChangedOnRegnskabViewModelEventHandlerRejserPropertyChangedVedOpdateringAfRegnskabViewModel(string propertyNameToRaise, string expectPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<IKontogruppeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeModel>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();

            var balanceViewModel = new BalanceViewModel(regnskabViewModelMock, fixture.Create<IKontogruppeModel>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(balanceViewModel, Is.Not.Null);

            var eventCalled = false;
            balanceViewModel.PropertyChanged += (s, e) =>
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
            regnskabViewModelMock.Raise(m => m.PropertyChanged += null, regnskabViewModelMock, new PropertyChangedEventArgs(propertyNameToRaise));
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnRegnskabViewModelEventHandler kaster en ArgumentNullException, hvis objektet, der rejser eventet, er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnRegnskabViewModelEventHandlerKasterArgumentNullExceptionHvisSenderErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IKontogruppeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeModel>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();

            var balanceViewModel = new BalanceViewModel(regnskabViewModelMock, fixture.Create<IKontogruppeModel>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(balanceViewModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => regnskabViewModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("sender"));
        }

        /// <summary>
        /// Tester, at PropertyChangedOnRegnskabViewModelEventHandler kaster en ArgumentNullException, hvis argumenter til eventet er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnRegnskabViewModelEventHandlerKasterArgumentNullExceptionHvisEventArgsErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IKontogruppeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeModel>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();

            var balanceViewModel = new BalanceViewModel(regnskabViewModelMock, fixture.Create<IKontogruppeModel>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(balanceViewModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => regnskabViewModelMock.Raise(m => m.PropertyChanged += null, fixture.Create<object>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("eventArgs"));
        }

        /// <summary>
        /// Tester, at PropertyChangedOnTabelModelEventHandler rejser PropertyChanged, modellen for gruppen af konti, som balancelinjen baserer sig på, opdateres.
        /// </summary>
        [Test]
        [TestCase("Id", "Id")]
        [TestCase("Nummer", "Nummer")]
        [TestCase("Nummer", "Konti")]
        [TestCase("Nummer", "Kredit")]
        [TestCase("Nummer", "KreditAsText")]
        [TestCase("Nummer", "Saldo")]
        [TestCase("Nummer", "SaldoAsText")]
        [TestCase("Nummer", "Disponibel")]
        [TestCase("Nummer", "DisponibelAsText")]
        [TestCase("Tekst", "Tekst")]
        [TestCase("Tekst", "DisplayName")]
        [TestCase("Balancetype", "Balancetype")]
        public void TestAtPropertyChangedOnTabelModelEventHandlerRejserPropertyChangedOnKontogruppeModelUpdate(string propertyNameToRaise, string expectPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() =>
            {
                var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
                regnskabViewModelMock.Expect(m => m.Konti)
                    .Return(new List<IKontoViewModel>())
                    .Repeat.Any();
                return regnskabViewModelMock;
            }));
            fixture.Customize<IKontogruppeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeModel>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var kontogruppeModelMock = fixture.Create<IKontogruppeModel>();
            var exceptionHandleViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var balanceViewModel = new BalanceViewModel(fixture.Create<IRegnskabViewModel>(), kontogruppeModelMock, exceptionHandleViewModelMock);
            Assert.That(balanceViewModel, Is.Not.Null);

            var eventCalled = false;
            balanceViewModel.PropertyChanged += (s, e) =>
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
            kontogruppeModelMock.Raise(m => m.PropertyChanged += null, kontogruppeModelMock, new PropertyChangedEventArgs(propertyNameToRaise));
            Assert.That(eventCalled, Is.True);

            exceptionHandleViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at PropertyChangedOnTabelModelEventHandler behandler registreringer af konti i forhold til brug i den aktuelle balancelinje.
        /// </summary>
        [Test]
        [TestCase("Nummer")]
        public void TestAtPropertyChangedOnTabelModelEventHandlerBehandlerKontoViewModelCollectionForBalanceViewModel(string propertyNameToRaise)
        {
            var fixture = new Fixture();
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var kontogruppeModelMock = MockRepository.GenerateMock<IKontogruppeModel>();
            kontogruppeModelMock.Expect(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();

            var kontogruppeViewModelMock = MockRepository.GenerateMock<IKontogruppeViewModel>();
            kontogruppeViewModelMock.Expect(m => m.Nummer)
                .Return(kontogruppeModelMock.Nummer)
                .Repeat.Any();

            var kontoViewModelMockCollection1 = new List<IKontoViewModel>(15);
            while (kontoViewModelMockCollection1.Count < kontoViewModelMockCollection1.Capacity)
            {
                var kontoViewModelMock = MockRepository.GenerateMock<IKontoViewModel>();
                kontoViewModelMock.Expect(m => m.Kontogruppe)
                    .Return(kontogruppeViewModelMock)
                    .Repeat.Any();
                kontoViewModelMock.Expect(m => m.ErRegistreret)
                    .Return(true)
                    .Repeat.Any();
                kontoViewModelMockCollection1.Add(kontoViewModelMock);
            }

            var kontoViewModelMockCollection2 = new List<IKontoViewModel>(10);
            while (kontoViewModelMockCollection2.Count < kontoViewModelMockCollection2.Capacity)
            {
                var kontoViewModelMock = MockRepository.GenerateMock<IKontoViewModel>();
                kontoViewModelMock.Expect(m => m.Kontogruppe)
                    .Return(kontogruppeViewModelMock)
                    .Repeat.Any();
                kontoViewModelMock.Expect(m => m.ErRegistreret)
                    .Return(false)
                    .Repeat.Any();
                kontoViewModelMockCollection2.Add(kontoViewModelMock);
            }

            var kontoViewModelMockCollection = new List<IKontoViewModel>(kontoViewModelMockCollection1);
            kontoViewModelMockCollection.AddRange(kontoViewModelMockCollection2);
            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Konti)
                .Return(kontoViewModelMockCollection)
                .Repeat.Any();

            var exceptionHandleViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var balanceViewModel = new BalanceViewModel(regnskabViewModelMock, kontogruppeModelMock, exceptionHandleViewModelMock);
            Assert.That(balanceViewModel, Is.Not.Null);

            kontogruppeModelMock.Raise(m => m.PropertyChanged += null, kontogruppeModelMock, new PropertyChangedEventArgs(propertyNameToRaise));

            foreach (var kontoViewModelMock in kontoViewModelMockCollection1)
            {
                kontoViewModelMock.AssertWasCalled(m => m.ErRegistreret = Arg<bool>.Is.Equal(false));
            }
            foreach (var kontoViewModelMock in kontoViewModelMockCollection2)
            {
                kontoViewModelMock.AssertWasCalled(m => m.ErRegistreret = Arg<bool>.Is.Equal(true));
            }

            exceptionHandleViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at PropertyChangedOnKontoViewModelEventHandler rejser PropertyChanged, når ViewModel for en registreret konto opdateres.
        /// </summary>
        [Test]
        [TestCase("Kontonummer", "Konti")]
        [TestCase("Kontogruppe", "Konti")]
        [TestCase("Kontogruppe", "Kredit")]
        [TestCase("Kontogruppe", "KreditAsText")]
        [TestCase("Kontogruppe", "Saldo")]
        [TestCase("Kontogruppe", "SaldoAsText")]
        [TestCase("Kontogruppe", "Disponibel")]
        [TestCase("Kontogruppe", "DisponibelAsText")]
        [TestCase("Kredit", "Kredit")]
        [TestCase("Kredit", "KreditAsText")]
        [TestCase("Saldo", "Saldo")]
        [TestCase("Saldo", "SaldoAsText")]
        [TestCase("Disponibel", "Disponibel")]
        [TestCase("Disponibel", "DisponibelAsText")]
        [TestCase("ErRegistreret", "Konti")]
        [TestCase("ErRegistreret", "Kredit")]
        [TestCase("ErRegistreret", "KreditAsText")]
        [TestCase("ErRegistreret", "Saldo")]
        [TestCase("ErRegistreret", "SaldoAsText")]
        [TestCase("ErRegistreret", "Disponibel")]
        [TestCase("ErRegistreret", "DisponibelAsText")]
        public void TestAtPropertyChangedOnKontoViewModelEventHandlerRejserPropertyChangedVedOpdateringAfKontoViewModel(string propertyNameToRaise, string expectPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var kontogruppeModelMock = MockRepository.GenerateMock<IKontogruppeModel>();
            kontogruppeModelMock.Expect(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();

            var kontogruppeViewModelMock = MockRepository.GenerateMock<IKontogruppeViewModel>();
            kontogruppeViewModelMock.Expect(m => m.Nummer)
                .Return(kontogruppeModelMock.Nummer)
                .Repeat.Any();

            var kontoViewModelMock = MockRepository.GenerateMock<IKontoViewModel>();
            kontoViewModelMock.Expect(m => m.Kontogruppe)
                .Return(kontogruppeViewModelMock)
                .Repeat.Any();
            kontoViewModelMock.Expect(m => m.ErRegistreret)
                .Return(false)
                .Repeat.Any();

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Konti)
                .Return(new List<IKontoViewModel> { kontoViewModelMock })
                .Repeat.Any();

            var balanceViewModel = new BalanceViewModel(regnskabViewModelMock, kontogruppeModelMock, fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(balanceViewModel, Is.Not.Null);

            balanceViewModel.Register(kontoViewModelMock);

            var eventCalled = false;
            balanceViewModel.PropertyChanged += (s, e) =>
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
            kontoViewModelMock.Raise(m => m.PropertyChanged += null, kontoViewModelMock, new PropertyChangedEventArgs(propertyNameToRaise));
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnKontoViewModelEventHandler afregistrerer kontoen fra brug i den aktuelle balancelinje.
        /// </summary>
        [Test]
        [TestCase("Kontogruppe")]
        [TestCase("ErRegistreret")]
        public void TestAtPropertyChangedOnKontoViewModelEventHandlerAfregistrererKontoViewModelFraBrugForBalanceViewModel(string propertyNameToRaise)
        {
            var fixture = new Fixture();
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var kontogruppeModelMock = MockRepository.GenerateMock<IKontogruppeModel>();
            kontogruppeModelMock.Expect(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();

            var kontogruppeViewModelMock = MockRepository.GenerateMock<IKontogruppeViewModel>();
            kontogruppeViewModelMock.Expect(m => m.Nummer)
                .Return(kontogruppeModelMock.Nummer)
                .Repeat.Any();

            var erRegistreret = false;
            var kontoViewModelMock = MockRepository.GenerateMock<IKontoViewModel>();
            // ReSharper disable AccessToModifiedClosure
            kontoViewModelMock.Expect(m => m.Kontogruppe)
                .WhenCalled(e => e.ReturnValue = kontogruppeViewModelMock)
                .Return(null)
                .Repeat.Any();
            // ReSharper restore AccessToModifiedClosure
            kontoViewModelMock.Expect(m => m.ErRegistreret)
                .WhenCalled(e => e.ReturnValue = erRegistreret)
                .Return(false)
                .Repeat.Any();
            kontoViewModelMock.Expect(m => m.ErRegistreret = Arg<bool>.Is.Anything)
                .WhenCalled(e => erRegistreret = (bool) e.Arguments.ElementAt(0))
                .Repeat.Any();

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Konti)
                .Return(new List<IKontoViewModel> {kontoViewModelMock})
                .Repeat.Any();

            var balanceViewModel = new BalanceViewModel(regnskabViewModelMock, kontogruppeModelMock, fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(balanceViewModel, Is.Not.Null);

            balanceViewModel.Register(kontoViewModelMock);

            switch (propertyNameToRaise)
            {
                case "Kontogruppe":
                    var nummer = fixture.Create<Generator<int>>().First(m => m != kontogruppeViewModelMock.Nummer);
                    kontogruppeViewModelMock = MockRepository.GenerateMock<IKontogruppeViewModel>();
                    kontogruppeViewModelMock.Expect(m => m.Nummer)
                        .Return(nummer)
                        .Repeat.Any();
                    break;
            }
            kontoViewModelMock.Raise(m => m.PropertyChanged += null, kontoViewModelMock, new PropertyChangedEventArgs(propertyNameToRaise));

            kontoViewModelMock.AssertWasCalled(m => m.ErRegistreret = Arg<bool>.Is.Equal(false), opt => opt.Repeat.Times(1));
        }

        /// <summary>
        /// Tester, at PropertyChangedOnKontoViewModelEventHandler kaster en ArgumentNullException, hvis objektet, der rejser eventet, er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnKontoViewModelEventHandlerKasterArgumentNullExceptionHvisSenderErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var kontogruppeModelMock = MockRepository.GenerateMock<IKontogruppeModel>();
            kontogruppeModelMock.Expect(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();

            var kontogruppeViewModelMock = MockRepository.GenerateMock<IKontogruppeViewModel>();
            kontogruppeViewModelMock.Expect(m => m.Nummer)
                .Return(kontogruppeModelMock.Nummer)
                .Repeat.Any();

            var kontoViewModelMock = MockRepository.GenerateMock<IKontoViewModel>();
            kontoViewModelMock.Expect(m => m.Kontogruppe)
                .Return(kontogruppeViewModelMock)
                .Repeat.Any();
            kontoViewModelMock.Expect(m => m.ErRegistreret)
                .Return(false)
                .Repeat.Any();

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Konti)
                .Return(new List<IKontoViewModel> { kontoViewModelMock })
                .Repeat.Any();

            var balanceViewModel = new BalanceViewModel(regnskabViewModelMock, kontogruppeModelMock, fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(balanceViewModel, Is.Not.Null);

            balanceViewModel.Register(kontoViewModelMock);

            var exception = Assert.Throws<ArgumentNullException>(() => kontoViewModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("sender"));
        }

        /// <summary>
        /// Tester, at PropertyChangedOnKontoViewModelEventHandler kaster en ArgumentNullException, hvis argumenter til eventet er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnKontoViewModelEventHandlerKasterArgumentNullExceptionHvisEventArgsErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var kontogruppeModelMock = MockRepository.GenerateMock<IKontogruppeModel>();
            kontogruppeModelMock.Expect(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();

            var kontogruppeViewModelMock = MockRepository.GenerateMock<IKontogruppeViewModel>();
            kontogruppeViewModelMock.Expect(m => m.Nummer)
                .Return(kontogruppeModelMock.Nummer)
                .Repeat.Any();

            var kontoViewModelMock = MockRepository.GenerateMock<IKontoViewModel>();
            kontoViewModelMock.Expect(m => m.Kontogruppe)
                .Return(kontogruppeViewModelMock)
                .Repeat.Any();
            kontoViewModelMock.Expect(m => m.ErRegistreret)
                .Return(false)
                .Repeat.Any();

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Konti)
                .Return(new List<IKontoViewModel> { kontoViewModelMock })
                .Repeat.Any();

            var balanceViewModel = new BalanceViewModel(regnskabViewModelMock, kontogruppeModelMock, fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(balanceViewModel, Is.Not.Null);

            balanceViewModel.Register(kontoViewModelMock);

            var exception = Assert.Throws<ArgumentNullException>(() => kontoViewModelMock.Raise(m => m.PropertyChanged += null, fixture.Create<object>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("eventArgs"));
        }

        /// <summary>
        /// Tester, at PropertyChangedOnKontoViewModelEventHandler kaster en IntranetGuiSystemException, hvis objektet, der rejser eventet, ikke er en ViewModel for en konto.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnKontoViewModelEventHandlerKasterIntranetGuiSystemExceptionHvisSenderIkkeErKontoViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var kontogruppeModelMock = MockRepository.GenerateMock<IKontogruppeModel>();
            kontogruppeModelMock.Expect(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();

            var kontogruppeViewModelMock = MockRepository.GenerateMock<IKontogruppeViewModel>();
            kontogruppeViewModelMock.Expect(m => m.Nummer)
                .Return(kontogruppeModelMock.Nummer)
                .Repeat.Any();

            var kontoViewModelMock = MockRepository.GenerateMock<IKontoViewModel>();
            kontoViewModelMock.Expect(m => m.Kontogruppe)
                .Return(kontogruppeViewModelMock)
                .Repeat.Any();
            kontoViewModelMock.Expect(m => m.ErRegistreret)
                .Return(false)
                .Repeat.Any();

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Konti)
                .Return(new List<IKontoViewModel> { kontoViewModelMock })
                .Repeat.Any();

            var balanceViewModel = new BalanceViewModel(regnskabViewModelMock, kontogruppeModelMock, fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(balanceViewModel, Is.Not.Null);

            balanceViewModel.Register(kontoViewModelMock);

            var sender = fixture.Create<object>();
            var exception = Assert.Throws<IntranetGuiSystemException>(() => kontoViewModelMock.Raise(m => m.PropertyChanged += null, sender, fixture.Create<PropertyChangedEventArgs>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "sender", sender.GetType().Name)));
        }
    }
}
