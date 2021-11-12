using System;
using System.ComponentModel;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
using Rhino.Mocks;
using Text = OSDevGrp.OSIntranet.Gui.Resources.Text;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Finansstyring
{
    /// <summary>
    /// Tester ViewModel til en konto.
    /// </summary>
    [TestFixture]
    public class KontoViewModelTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en ViewModel til en konto.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererKontoViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var regnskabViewModelMock = fixture.Create<IRegnskabViewModel>();
            var kontoModelMock = MockRepository.GenerateMock<IKontoModel>();
            kontoModelMock.Expect(m => m.Kontonummer)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            kontoModelMock.Expect(m => m.Kontonavn)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            kontoModelMock.Expect(m => m.Beskrivelse)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            kontoModelMock.Expect(m => m.Notat)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            kontoModelMock.Expect(m => m.StatusDato)
                .Return(fixture.Create<DateTime>())
                .Repeat.Any();
            kontoModelMock.Expect(m => m.Kredit)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            kontoModelMock.Expect(m => m.Saldo)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            kontoModelMock.Expect(m => m.Disponibel)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            var kontogruppeViewModelMock = fixture.Create<IKontogruppeViewModel>();
            var kontoViewModel = new KontoViewModel(regnskabViewModelMock, kontoModelMock, kontogruppeViewModelMock, fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(kontoViewModel, Is.Not.Null);
            Assert.That(kontoViewModel.Regnskab, Is.Not.Null);
            Assert.That(kontoViewModel.Regnskab, Is.EqualTo(regnskabViewModelMock));
            Assert.That(kontoViewModel.Kontonummer, Is.Not.Null);
            Assert.That(kontoViewModel.Kontonummer, Is.Not.Empty);
            Assert.That(kontoViewModel.Kontonummer, Is.EqualTo(kontoModelMock.Kontonummer));
            Assert.That(kontoViewModel.Kontonavn, Is.Not.Null);
            Assert.That(kontoViewModel.Kontonavn, Is.Not.Empty);
            Assert.That(kontoViewModel.Kontonavn, Is.EqualTo(kontoModelMock.Kontonavn));
            Assert.That(kontoViewModel.Beskrivelse, Is.Not.Null);
            Assert.That(kontoViewModel.Beskrivelse, Is.Not.Empty);
            Assert.That(kontoViewModel.Beskrivelse, Is.EqualTo(kontoModelMock.Beskrivelse));
            Assert.That(kontoViewModel.Notat, Is.Not.Null);
            Assert.That(kontoViewModel.Notat, Is.Not.Empty);
            Assert.That(kontoViewModel.Notat, Is.EqualTo(kontoModelMock.Notat));
            Assert.That(kontoViewModel.Kontogruppe, Is.Not.Null);
            Assert.That(kontoViewModel.Kontogruppe, Is.EqualTo(kontogruppeViewModelMock));
            Assert.That(kontoViewModel.StatusDato, Is.EqualTo(kontoModelMock.StatusDato));
            Assert.That(kontoViewModel.Kredit, Is.EqualTo(kontoModelMock.Kredit));
            Assert.That(kontoViewModel.KreditAsText, Is.Not.Null);
            Assert.That(kontoViewModel.KreditAsText, Is.Not.Empty);
            Assert.That(kontoViewModel.KreditAsText, Is.EqualTo(kontoModelMock.Kredit.ToString("C")));
            Assert.That(kontoViewModel.KreditLabel, Is.Not.Null);
            Assert.That(kontoViewModel.KreditLabel, Is.Not.Empty);
            Assert.That(kontoViewModel.KreditLabel, Is.EqualTo(Resource.GetText(Text.Credit)));
            Assert.That(kontoViewModel.Saldo, Is.EqualTo(kontoModelMock.Saldo));
            Assert.That(kontoViewModel.SaldoAsText, Is.Not.Null);
            Assert.That(kontoViewModel.SaldoAsText, Is.Not.Empty);
            Assert.That(kontoViewModel.SaldoAsText, Is.EqualTo(kontoModelMock.Saldo.ToString("C")));
            Assert.That(kontoViewModel.SaldoLabel, Is.Not.Null);
            Assert.That(kontoViewModel.SaldoLabel, Is.Not.Empty);
            Assert.That(kontoViewModel.SaldoLabel, Is.EqualTo(Resource.GetText(Text.Balance)));
            Assert.That(kontoViewModel.Disponibel, Is.EqualTo(kontoModelMock.Disponibel));
            Assert.That(kontoViewModel.DisponibelAsText, Is.Not.Null);
            Assert.That(kontoViewModel.DisponibelAsText, Is.Not.Empty);
            Assert.That(kontoViewModel.DisponibelAsText, Is.EqualTo(kontoModelMock.Disponibel.ToString("C")));
            Assert.That(kontoViewModel.DisponibelLabel, Is.Not.Null);
            Assert.That(kontoViewModel.DisponibelLabel, Is.Not.Empty);
            Assert.That(kontoViewModel.DisponibelLabel, Is.EqualTo(Resource.GetText(Text.Available)));
            Assert.That(kontoViewModel.Kontoværdi, Is.EqualTo(kontoModelMock.Disponibel));
            Assert.That(kontoViewModel.DisplayName, Is.Not.Null);
            Assert.That(kontoViewModel.DisplayName, Is.Not.Empty);
            Assert.That(kontoViewModel.DisplayName, Is.EqualTo(Resource.GetText(Text.Account)));
            Assert.That(kontoViewModel.Image, Is.Not.Null);
            Assert.That(kontoViewModel.Image, Is.Not.Empty);
            Assert.That(kontoViewModel.Image, Is.EqualTo(Resource.GetEmbeddedResource("Images.Konto.png")));
            Assert.That(kontoViewModel.ErRegistreret, Is.False);
            Assert.That(kontoViewModel.RefreshCommand, Is.Not.Null);
            Assert.That(kontoViewModel.RefreshCommand, Is.TypeOf<KontoGetCommand>());

            kontoModelMock.AssertWasNotCalled(m => m.Regnskabsnummer);
            kontoModelMock.AssertWasCalled(m => m.Kontonummer);
            kontoModelMock.AssertWasCalled(m => m.Kontonavn);
            kontoModelMock.AssertWasCalled(m => m.Beskrivelse);
            kontoModelMock.AssertWasCalled(m => m.Notat);
            kontoModelMock.AssertWasNotCalled(m => m.Kontogruppe);
            kontoModelMock.AssertWasCalled(m => m.StatusDato);
            kontoModelMock.AssertWasCalled(m => m.Kredit);
            kontoModelMock.AssertWasCalled(m => m.Saldo);
            kontoModelMock.AssertWasCalled(m => m.Disponibel);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis ViewModel for regnskabet er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisRegnskabViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IKontoModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontoModel>()));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new KontoViewModel(null, fixture.Create<IKontoModel>(), fixture.Create<IKontogruppeViewModel>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("regnskabViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis modellen for kontoen er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisKontoModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new KontoViewModel(fixture.Create<IRegnskabViewModel>(), null, fixture.Create<IKontogruppeViewModel>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("kontoModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis ViewModel for kontogruppen er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisKontogruppeViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IKontoModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontoModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new KontoViewModel(fixture.Create<IRegnskabViewModel>(), fixture.Create<IKontoModel>(), null, fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("kontogruppeViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis repositoryet til finansstyring er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFinansstyringRepositoryErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IKontoModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontoModel>()));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeViewModel>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new KontoViewModel(fixture.Create<IRegnskabViewModel>(), fixture.Create<IKontoModel>(), fixture.Create<IKontogruppeViewModel>(), null, fixture.Create<IExceptionHandlerViewModel>()));
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
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IKontoModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontoModel>()));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new KontoViewModel(fixture.Create<IRegnskabViewModel>(), fixture.Create<IKontoModel>(), fixture.Create<IKontogruppeViewModel>(), fixture.Create<IFinansstyringRepository>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("exceptionHandlerViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at sætteren til Kredit opdaterer Kredit på modellen til kontoen.
        /// </summary>
        [Test]
        public void TestAtKreditSetterOpdatererKreditOnKontoModel()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IKontoModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontoModel>()));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var kontoModelMock = fixture.Create<IKontoModel>();
            var exceptionHandlerViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var kontoViewModel = new KontoViewModel(fixture.Create<IRegnskabViewModel>(), kontoModelMock, fixture.Create<IKontogruppeViewModel>(), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(kontoViewModel, Is.Not.Null);

            var newValue = fixture.Create<decimal>();
            kontoViewModel.Kredit = newValue;

            kontoModelMock.AssertWasCalled(m => m.Kredit = Arg<decimal>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at sætteren til Kredit kalder HandleException på exceptionhandleren ved exceptions.
        /// </summary>
        [Test]
        public void TestAtKreditSetterKalderHandleExceptionOnExceptionHandlerViewModelVedExceptions()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = fixture.Create<Exception>();
            var kontoModelMock = MockRepository.GenerateMock<IKontoModel>();
            kontoModelMock.Expect(m => m.Kredit = Arg<decimal>.Is.GreaterThan(0M))
                          .Throw(exception)
                          .Repeat.Any();

            var exceptionHandlerViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var kontoViewModel = new KontoViewModel(fixture.Create<IRegnskabViewModel>(), kontoModelMock, fixture.Create<IKontogruppeViewModel>(), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(kontoViewModel, Is.Not.Null);

            var newValue = fixture.Create<decimal>();
            kontoViewModel.Kredit = newValue;

            kontoModelMock.AssertWasCalled(m => m.Kredit = Arg<decimal>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<Exception>.Is.Equal(exception)));
        }

        /// <summary>
        /// Tester, at sætteren til Saldo opdaterer Saldo på modellen til kontoen.
        /// </summary>
        [Test]
        public void TestAtSaldoSetterOpdatererSaldoOnKontoModel()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IKontoModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontoModel>()));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var kontoModelMock = fixture.Create<IKontoModel>();
            var exceptionHandlerViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var kontoViewModel = new KontoViewModel(fixture.Create<IRegnskabViewModel>(), kontoModelMock, fixture.Create<IKontogruppeViewModel>(), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(kontoViewModel, Is.Not.Null);

            var newValue = fixture.Create<decimal>();
            kontoViewModel.Saldo = newValue;

            kontoModelMock.AssertWasCalled(m => m.Saldo = Arg<decimal>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at sætteren til Saldo kalder HandleException på exceptionhandleren ved exceptions.
        /// </summary>
        [Test]
        public void TestAtSaldoSetterKalderHandleExceptionOnExceptionHandlerViewModelVedExceptions()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = fixture.Create<Exception>();
            var kontoModelMock = MockRepository.GenerateMock<IKontoModel>();
            kontoModelMock.Expect(m => m.Saldo = Arg<decimal>.Is.GreaterThan(0M))
                          .Throw(exception)
                          .Repeat.Any();

            var exceptionHandlerViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var kontoViewModel = new KontoViewModel(fixture.Create<IRegnskabViewModel>(), kontoModelMock, fixture.Create<IKontogruppeViewModel>(), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(kontoViewModel, Is.Not.Null);

            var newValue = fixture.Create<decimal>();
            kontoViewModel.Saldo = newValue;

            kontoModelMock.AssertWasCalled(m => m.Saldo = Arg<decimal>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<Exception>.Is.Equal(exception)));
        }

        /// <summary>
        /// Tester, at PropertyChangedOnKontoModelEventHandler rejser PropertyChanged, når modellen for kontoen opdateres.
        /// </summary>
        [Test]
        [TestCase("Regnskabsnummer", "Regnskabsnummer")]
        [TestCase("Kontonummer", "Kontonummer")]
        [TestCase("Kontonavn", "Kontonavn")]
        [TestCase("Beskrivelse", "Beskrivelse")]
        [TestCase("Notat", "Notat")]
        [TestCase("Kontogruppe", "Kontogruppe")]
        [TestCase("StatusDato", "StatusDato")]
        [TestCase("Kredit", "Kredit")]
        [TestCase("Kredit", "KreditAsText")]
        [TestCase("Saldo", "Saldo")]
        [TestCase("Saldo", "SaldoAsText")]
        [TestCase("Disponibel", "Disponibel")]
        [TestCase("Disponibel", "DisponibelAsText")]
        [TestCase("Disponibel", "Kontoværdi")]
        public void TestAtPropertyChangedOnKontoModelEventHandlerRejserPropertyChangedOnKontoModelUpdate(string propertyNameToRaise, string expectPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IKontoModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontoModel>()));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var kontoModelMock = fixture.Create<IKontoModel>();
            var exceptionHandlerViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var kontoViewModel = new KontoViewModel(fixture.Create<IRegnskabViewModel>(), kontoModelMock, fixture.Create<IKontogruppeViewModel>(), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(kontoViewModel, Is.Not.Null);

            var eventCalled = false;
            kontoViewModel.PropertyChanged += (s, e) =>
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
            kontoModelMock.Raise(m => m.PropertyChanged += null, kontoModelMock, new PropertyChangedEventArgs(propertyNameToRaise));
            Assert.That(eventCalled, Is.True);

            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }
    }
}
