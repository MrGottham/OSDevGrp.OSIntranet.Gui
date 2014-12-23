using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
using Ploeh.AutoFixture;
using Rhino.Mocks;

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
        /// Tester, at PropertyChangedOnRegnskabViewModelEventHandler rejser PropertyChanged, når ViewModel for regnskabet opdateres.
        /// </summary>
        [Test]
        [TestCase("Konti", "Konti")]
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
        [TestCase("Tekst", "Tekst")]
        [TestCase("Tekst", "DisplayName")]
        [TestCase("Balancetype", "Balancetype")]
        public void TestAtPropertyChangedOnTabelModelEventHandlerRejserPropertyChangedOnKontogruppeModelUpdate(string propertyNameToRaise, string expectPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
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

            var exception = Assert.Throws<ArgumentNullException>(() => kontoViewModelMock.Raise(m => m.PropertyChanged += null, fixture.Create<object>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("eventArgs"));
        }

    }
}
