using System;
using System.Windows.Input;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Finansstyring
{
    /// <summary>
    /// Tester ViewModel indeholdende grundlæggende kontooplysninger.
    /// </summary>
    [TestFixture]
    public class KontoViewModelBaseTests
    {
        /// <summary>
        /// Egen klasse til test af ViewModel indeholdende grundlæggende kontooplysninger.
        /// </summary>
        private class MyKontoViewModel : KontoViewModelBase<IKontoModelBase, IKontogruppeViewModelBase>
        {
            #region Constructor

            /// <summary>
            /// Danner egen klasse til test af ViewModel indeholdende grundlæggende kontooplysninger.
            /// </summary>
            /// <param name="regnskabViewModel">ViewModel for regnskabet, som kontoen er tilknyttet.</param>
            /// <param name="kontoModel">Model indeholdende grundlæggende kontooplysninger.</param>
            /// <param name="kontogruppeViewModel">ViewModel for kontogruppen.</param>
            /// <param name="displayName">Navn for ViewModel, som kan benyttes til visning i brugergrænsefladen.</param>
            /// <param name="image">Billede, der illustrerer en kontoen.</param>
            /// <param name="finansstyringRepository">Implementering af repositoryet til finansstyring.</param>
            /// <param name="exceptionHandlerViewModel">Implementering af ViewModel for exceptionhandleren.</param>
            public MyKontoViewModel(IRegnskabViewModel regnskabViewModel, IKontoModelBase kontoModel, IKontogruppeViewModelBase kontogruppeViewModel, string displayName, byte[] image, IFinansstyringRepository finansstyringRepository, IExceptionHandlerViewModel exceptionHandlerViewModel)
                : base(regnskabViewModel, kontoModel, kontogruppeViewModel, displayName, image, finansstyringRepository, exceptionHandlerViewModel)
            {
            }

            #endregion

            #region Properties

            /// <summary>
            /// Kommando til genindlæsning og opdatering.
            /// </summary>
            public override ICommand RefreshCommand
            {
                get
                {
                    return MockRepository.GenerateMock<ICommand>();
                }
            }

            /// <summary>
            /// Modellen, der indeholdende grundlæggende kontooplysninger.
            /// </summary>
            public new IKontoModelBase Model
            {
                get
                {
                    return base.Model;
                }
            }

            /// <summary>
            /// Repository til finansstyring.
            /// </summary>
            public new IFinansstyringRepository FinansstyringRepository
            {
                get
                {
                    return base.FinansstyringRepository;
                }
            }

            /// <summary>
            /// ViewModel for en exceptionhandler.
            /// </summary>
            public new IExceptionHandlerViewModel ExceptionHandler
            {
                get
                {
                    return base.ExceptionHandler;
                }
            }

            #endregion
        }

        /// <summary>
        /// Tester, at konstruktøren initierer en ViewModel indeholdende grundlæggende kontooplysninger.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererKontoViewModelBase()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IKontogruppeViewModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeViewModelBase>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var regnskabViewModelMock = fixture.Create<IRegnskabViewModel>();

            var kontoModelMock = MockRepository.GenerateMock<IKontoModelBase>();
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

            var kontogruppeViewModelMock = fixture.Create<IKontogruppeViewModelBase>();

            var displayName = fixture.Create<string>();

            var image = Resource.GetEmbeddedResource("Images.Konto.png");

            var finansstyringRepositoryMock = fixture.Create<IFinansstyringRepository>();

            var exceptionHandlerViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var kontoViewModel = new MyKontoViewModel(regnskabViewModelMock, kontoModelMock, kontogruppeViewModelMock, displayName, image, finansstyringRepositoryMock, exceptionHandlerViewModelMock);
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
            Assert.That(kontoViewModel.DisplayName, Is.Not.Null);
            Assert.That(kontoViewModel.DisplayName, Is.Not.Empty);
            Assert.That(kontoViewModel.DisplayName, Is.EqualTo(displayName));
            Assert.That(kontoViewModel.Image, Is.Not.Null);
            Assert.That(kontoViewModel.Image, Is.Not.Empty);
            Assert.That(kontoViewModel.Image, Is.EqualTo(image));
            Assert.That(kontoViewModel.RefreshCommand, Is.Not.Null);
            Assert.That(kontoViewModel.Model, Is.Not.Null);
            Assert.That(kontoViewModel.Model, Is.EqualTo(kontoModelMock));
            Assert.That(kontoViewModel.FinansstyringRepository, Is.Not.Null);
            Assert.That(kontoViewModel.FinansstyringRepository, Is.EqualTo(finansstyringRepositoryMock));
            Assert.That(kontoViewModel.ExceptionHandler, Is.Not.Null);
            Assert.That(kontoViewModel.ExceptionHandler, Is.EqualTo(exceptionHandlerViewModelMock));

            kontoModelMock.AssertWasCalled(m => m.Kontonummer);
            kontoModelMock.AssertWasCalled(m => m.Kontonavn);
            kontoModelMock.AssertWasCalled(m => m.Beskrivelse);
            kontoModelMock.AssertWasCalled(m => m.Notat);
            kontoModelMock.AssertWasNotCalled(m => m.Kontogruppe);
            kontoModelMock.AssertWasCalled(m => m.StatusDato);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis ViewModel for regnskabet er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisRegnskabViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IKontoModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontoModel>()));
            fixture.Customize<IKontogruppeViewModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeViewModelBase>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new MyKontoViewModel(null, fixture.Create<IKontoModel>(), fixture.Create<IKontogruppeViewModelBase>(), fixture.Create<string>(), Resource.GetEmbeddedResource("Images.Konto.png"), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("regnskabViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis modellen, der indeholder grundlæggende kontooplysninger, er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisKontoModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IKontogruppeViewModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeViewModelBase>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new MyKontoViewModel(fixture.Create<IRegnskabViewModel>(), null, fixture.Create<IKontogruppeViewModelBase>(), fixture.Create<string>(), Resource.GetEmbeddedResource("Images.Konto.png"), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>()));
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
            fixture.Customize<IKontoModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontoModelBase>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new MyKontoViewModel(fixture.Create<IRegnskabViewModel>(), fixture.Create<IKontoModelBase>(), null, fixture.Create<string>(), Resource.GetEmbeddedResource("Images.Konto.png"), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("kontogruppeViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis navnet for ViewModel, som kan benyttes til visning i brugergrænsefladen, er invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestAtConstructorKasterArgumentNullExceptionHvisDisplayNameErInvalid(string invalidValue)
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IKontoModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontoModelBase>()));
            fixture.Customize<IKontogruppeViewModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeViewModelBase>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new MyKontoViewModel(fixture.Create<IRegnskabViewModel>(), fixture.Create<IKontoModelBase>(), fixture.Create<IKontogruppeViewModelBase>(), invalidValue, Resource.GetEmbeddedResource("Images.Konto.png"), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("displayName"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis billedet, der illustrerer kontoen, er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisImageErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IKontoModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontoModelBase>()));
            fixture.Customize<IKontogruppeViewModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeViewModelBase>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new MyKontoViewModel(fixture.Create<IRegnskabViewModel>(), fixture.Create<IKontoModelBase>(), fixture.Create<IKontogruppeViewModelBase>(), fixture.Create<string>(), null, fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>()));
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
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IKontoModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontoModelBase>()));
            fixture.Customize<IKontogruppeViewModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeViewModelBase>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new MyKontoViewModel(fixture.Create<IRegnskabViewModel>(), fixture.Create<IKontoModelBase>(), fixture.Create<IKontogruppeViewModelBase>(), fixture.Create<string>(), Resource.GetEmbeddedResource("Images.Konto.png"), null, fixture.Create<IExceptionHandlerViewModel>()));
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
            fixture.Customize<IKontoModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontoModelBase>()));
            fixture.Customize<IKontogruppeViewModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeViewModelBase>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new MyKontoViewModel(fixture.Create<IRegnskabViewModel>(), fixture.Create<IKontoModelBase>(), fixture.Create<IKontogruppeViewModelBase>(), fixture.Create<string>(), Resource.GetEmbeddedResource("Images.Konto.png"), fixture.Create<IFinansstyringRepository>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("exceptionHandlerViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at sætteren til Kontonavn opdaterer Kontonavn på modellen, der indeholder de grundlæggende kontooplysninger.
        /// </summary>
        [Test]
        public void TestAtKontonavnSetterOpdatererKontonavnOnKontoModelBase()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IKontoModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontoModelBase>()));
            fixture.Customize<IKontogruppeViewModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeViewModelBase>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var kontoModelMock = fixture.Create<IKontoModelBase>();

            var exceptionHandlerViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var kontoViewModel = new MyKontoViewModel(fixture.Create<IRegnskabViewModel>(), kontoModelMock, fixture.Create<IKontogruppeViewModelBase>(), fixture.Create<string>(), Resource.GetEmbeddedResource("Images.Konto.png"), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(kontoViewModel, Is.Not.Null);

            var newValue = fixture.Create<string>();
            kontoViewModel.Kontonavn = newValue;

            kontoModelMock.AssertWasCalled(m => m.Kontonavn = Arg<string>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at sætteren til Kontonavn kalder HandleException på exceptionhandleren ved exceptions.
        /// </summary>
        [Test]
        public void TestAtKontonavnSetterKalderHandleExceptionOnExceptionHandlerViewModelVedExceptions()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IKontogruppeViewModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeViewModelBase>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = fixture.Create<Exception>();
            var kontoModelMock = MockRepository.GenerateMock<IKontoModelBase>();
            kontoModelMock.Expect(m => m.Kontonavn = Arg<string>.Is.NotNull)
                          .Throw(exception)
                          .Repeat.Any();

            var exceptionHandlerViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var kontoViewModel = new MyKontoViewModel(fixture.Create<IRegnskabViewModel>(), kontoModelMock, fixture.Create<IKontogruppeViewModelBase>(), fixture.Create<string>(), Resource.GetEmbeddedResource("Images.Konto.png"), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(kontoViewModel, Is.Not.Null);

            var newValue = fixture.Create<string>();
            kontoViewModel.Kontonavn = newValue;

            kontoModelMock.AssertWasCalled(m => m.Kontonavn = Arg<string>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<Exception>.Is.Equal(exception)));
        }

        /// <summary>
        /// Tester, at sætteren til Beskrivelse opdaterer Beskrivelse på modellen, der indeholder de grundlæggende kontooplysninger.
        /// </summary>
        [Test]
        public void TestAtBeskrivelseSetterOpdatererBeskrivelseOnKontoModelBase()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IKontoModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontoModelBase>()));
            fixture.Customize<IKontogruppeViewModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeViewModelBase>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var kontoModelMock = fixture.Create<IKontoModelBase>();

            var exceptionHandlerViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var kontoViewModel = new MyKontoViewModel(fixture.Create<IRegnskabViewModel>(), kontoModelMock, fixture.Create<IKontogruppeViewModelBase>(), fixture.Create<string>(), Resource.GetEmbeddedResource("Images.Konto.png"), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(kontoViewModel, Is.Not.Null);

            var newValue = fixture.Create<string>();
            kontoViewModel.Beskrivelse = newValue;

            kontoModelMock.AssertWasCalled(m => m.Beskrivelse = Arg<string>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at sætteren til Beskrivelse kalder HandleException på exceptionhandleren ved exceptions.
        /// </summary>
        [Test]
        public void TestAtBeskrivelseSetterKalderHandleExceptionOnExceptionHandlerViewModelVedExceptions()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IKontogruppeViewModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeViewModelBase>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = fixture.Create<Exception>();
            var kontoModelMock = MockRepository.GenerateMock<IKontoModelBase>();
            kontoModelMock.Expect(m => m.Beskrivelse = Arg<string>.Is.NotNull)
                          .Throw(exception)
                          .Repeat.Any();

            var exceptionHandlerViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var kontoViewModel = new MyKontoViewModel(fixture.Create<IRegnskabViewModel>(), kontoModelMock, fixture.Create<IKontogruppeViewModelBase>(), fixture.Create<string>(), Resource.GetEmbeddedResource("Images.Konto.png"), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(kontoViewModel, Is.Not.Null);

            var newValue = fixture.Create<string>();
            kontoViewModel.Beskrivelse = newValue;

            kontoModelMock.AssertWasCalled(m => m.Beskrivelse = Arg<string>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<Exception>.Is.Equal(exception)));
        }

        /// <summary>
        /// Tester, at sætteren til Notat opdaterer Notat på modellen, der indeholder de grundlæggende kontooplysninger.
        /// </summary>
        [Test]
        public void TestAtNotatSetterOpdatererNotatOnKontoModelBase()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IKontoModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontoModelBase>()));
            fixture.Customize<IKontogruppeViewModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeViewModelBase>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var kontoModelMock = fixture.Create<IKontoModelBase>();

            var exceptionHandlerViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var kontoViewModel = new MyKontoViewModel(fixture.Create<IRegnskabViewModel>(), kontoModelMock, fixture.Create<IKontogruppeViewModelBase>(), fixture.Create<string>(), Resource.GetEmbeddedResource("Images.Konto.png"), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(kontoViewModel, Is.Not.Null);

            var newValue = fixture.Create<string>();
            kontoViewModel.Notat = newValue;

            kontoModelMock.AssertWasCalled(m => m.Notat = Arg<string>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at sætteren til Notat kalder HandleException på exceptionhandleren ved exceptions.
        /// </summary>
        [Test]
        public void TestAtNotatSetterKalderHandleExceptionOnExceptionHandlerViewModelVedExceptions()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IKontogruppeViewModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeViewModelBase>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = fixture.Create<Exception>();
            var kontoModelMock = MockRepository.GenerateMock<IKontoModelBase>();
            kontoModelMock.Expect(m => m.Notat = Arg<string>.Is.NotNull)
                          .Throw(exception)
                          .Repeat.Any();

            var exceptionHandlerViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var kontoViewModel = new MyKontoViewModel(fixture.Create<IRegnskabViewModel>(), kontoModelMock, fixture.Create<IKontogruppeViewModelBase>(), fixture.Create<string>(), Resource.GetEmbeddedResource("Images.Konto.png"), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(kontoViewModel, Is.Not.Null);

            var newValue = fixture.Create<string>();
            kontoViewModel.Notat = newValue;

            kontoModelMock.AssertWasCalled(m => m.Notat = Arg<string>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<Exception>.Is.Equal(exception)));
        }

        /// <summary>
        /// Tester, at sætteren til Kontogruppe kalder HandleException på exceptionhandleren men en ArgumentNullException, hvis value er null.
        /// </summary>
        [Test]
        public void TestAtKontogruppeSetterKalderHandleExceptionOnExceptionHandlerViewModelMedArgumentNullExceptionHvisValueErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IKontogruppeViewModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeViewModelBase>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var kontoModelMock = MockRepository.GenerateMock<IKontoModelBase>();

            var exceptionHandlerViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var kontoViewModel = new MyKontoViewModel(fixture.Create<IRegnskabViewModel>(), kontoModelMock, fixture.Create<IKontogruppeViewModelBase>(), fixture.Create<string>(), Resource.GetEmbeddedResource("Images.Konto.png"), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(kontoViewModel, Is.Not.Null);

            kontoViewModel.Kontogruppe = null;

            kontoModelMock.AssertWasNotCalled(m => m.Kontogruppe = Arg<int>.Is.Anything);
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<ArgumentNullException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til Kontogruppe ikke opdaterer Kontogruppe på modellen, der indeholder de grundlæggende kontooplysninger, hvis value er den samme ViewModel for kontogruppen.
        /// </summary>
        [Test]
        public void TestAtKontogruppeSetterIkkeOpdatererKontogruppeOnKontoModelBaseHvisValueErDenSammeKontogruppeViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var kontoModelMock = MockRepository.GenerateMock<IKontoModelBase>();

            var kontogruppeViewModelMock = MockRepository.GenerateMock<IKontogruppeViewModelBase>();

            var exceptionHandlerViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var kontoViewModel = new MyKontoViewModel(fixture.Create<IRegnskabViewModel>(), kontoModelMock, kontogruppeViewModelMock, fixture.Create<string>(), Resource.GetEmbeddedResource("Images.Konto.png"), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(kontoViewModel, Is.Not.Null);
            Assert.That(kontoViewModel.Kontogruppe, Is.Not.Null);
            Assert.That(kontoViewModel.Kontogruppe, Is.EqualTo(kontogruppeViewModelMock));
            
            kontoViewModel.Kontogruppe = kontogruppeViewModelMock;
            Assert.That(kontoViewModel.Kontogruppe, Is.Not.Null);
            Assert.That(kontoViewModel.Kontogruppe, Is.EqualTo(kontogruppeViewModelMock));

            kontoModelMock.AssertWasNotCalled(m => m.Kontogruppe = Arg<int>.Is.Anything);
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at sætteren til Kontogruppe opdaterer Kontogruppe på modellen, der indeholder de grundlæggende kontooplysninger, hvis value ikke er den samme ViewModel for kontogruppen.
        /// </summary>
        [Test]
        public void TestAtKontogruppeSetterOpdatererKontogruppeOnKontoModelBaseHvisValueIkkeErDenSammeKontogruppeViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IKontogruppeViewModelBase>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IKontogruppeViewModelBase>();
                    return mock;
                }));

            var kontoModelMock = MockRepository.GenerateMock<IKontoModelBase>();
            kontoModelMock.Expect(m => m.Kontogruppe)
                          .Return(fixture.Create<int>())
                          .Repeat.Any();

            var exceptionHandlerViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var kontoViewModel = new MyKontoViewModel(fixture.Create<IRegnskabViewModel>(), kontoModelMock, fixture.Create<IKontogruppeViewModelBase>(), fixture.Create<string>(), Resource.GetEmbeddedResource("Images.Konto.png"), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(kontoViewModel, Is.Not.Null);

            var newValue = fixture.Create<IKontogruppeViewModelBase>();
            Assert.That(newValue, Is.Not.Null);
            Assert.That(newValue.Nummer, Is.Not.EqualTo(kontoModelMock.Kontogruppe));

            kontoViewModel.Kontogruppe = newValue;

            kontoModelMock.AssertWasCalled(m => m.Kontogruppe);
            kontoModelMock.AssertWasCalled(m => m.Kontogruppe = Arg<int>.Is.Equal(newValue.Nummer));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at sætteren til StatusDato opdaterer StatusDato på modellen, der indeholder de grundlæggende kontooplysninger.
        /// </summary>
        [Test]
        public void TestAtStatusDatoSetterOpdatererStatusDatoOnKontoModelBase()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IKontoModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontoModelBase>()));
            fixture.Customize<IKontogruppeViewModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeViewModelBase>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var kontoModelMock = fixture.Create<IKontoModelBase>();

            var exceptionHandlerViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var kontoViewModel = new MyKontoViewModel(fixture.Create<IRegnskabViewModel>(), kontoModelMock, fixture.Create<IKontogruppeViewModelBase>(), fixture.Create<string>(), Resource.GetEmbeddedResource("Images.Konto.png"), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(kontoViewModel, Is.Not.Null);

            var newValue = fixture.Create<DateTime>().AddDays(7);
            kontoViewModel.StatusDato = newValue;

            kontoModelMock.AssertWasCalled(m => m.StatusDato = Arg<DateTime>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at sætteren til StatusDato kalder HandleException på exceptionhandleren ved exceptions.
        /// </summary>
        [Test]
        public void TestAtStatusDatoSetterKalderHandleExceptionOnExceptionHandlerViewModelVedExceptions()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IKontogruppeViewModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeViewModelBase>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = fixture.Create<Exception>();
            var kontoModelMock = MockRepository.GenerateMock<IKontoModelBase>();
            kontoModelMock.Expect(m => m.StatusDato = Arg<DateTime>.Is.GreaterThan(DateTime.Now))
                          .Throw(exception)
                          .Repeat.Any();

            var exceptionHandlerViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var kontoViewModel = new MyKontoViewModel(fixture.Create<IRegnskabViewModel>(), kontoModelMock, fixture.Create<IKontogruppeViewModelBase>(), fixture.Create<string>(), Resource.GetEmbeddedResource("Images.Konto.png"), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(kontoViewModel, Is.Not.Null);

            var newValue = fixture.Create<DateTime>().AddDays(7);
            kontoViewModel.StatusDato = newValue;

            kontoModelMock.AssertWasCalled(m => m.StatusDato = Arg<DateTime>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<Exception>.Is.Equal(exception)));
        }
    }
}
