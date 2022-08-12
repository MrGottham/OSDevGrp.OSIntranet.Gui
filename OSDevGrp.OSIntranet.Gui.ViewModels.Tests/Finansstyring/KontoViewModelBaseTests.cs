using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
using Text = OSDevGrp.OSIntranet.Gui.Resources.Text;

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
            #region Private variables

            private readonly Fixture _fixture = new Fixture();

            #endregion

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
            /// Kontoens værdi pr. opgørelsestidspunktet.
            /// </summary>
            public override decimal Kontoværdi => 0M;

            /// <summary>
            /// Kommando til genindlæsning og opdatering.
            /// </summary>
            public override ICommand RefreshCommand => _fixture.BuildCommand();

            /// <summary>
            /// Modellen, der indeholdende grundlæggende kontooplysninger.
            /// </summary>
            public new IKontoModelBase Model => base.Model;

            /// <summary>
            /// Repository til finansstyring.
            /// </summary>
            public new IFinansstyringRepository FinansstyringRepository => base.FinansstyringRepository;

            /// <summary>
            /// ViewModel for en exceptionhandler.
            /// </summary>
            public new IExceptionHandlerViewModel ExceptionHandler => base.ExceptionHandler;

            /// <summary>
            /// Angivelse af, om metoden ModelChanged er kaldt.
            /// </summary>
            public bool IsModelChangedCalled
            {
                get;
                private set;
            }

            /// <summary>
            /// Angivelse af, om metoden ModelChanged skal kaste en Exception, når den kaldes.
            /// </summary>
            public bool ThrowExceptionInModelChanged
            {
                get;
                set;
            }

            #endregion

            #region Methods

            /// <summary>
            /// Metode, der kaldes, når en property på modellen, der indeholder grundlæggende kontooplysninger, ændres.
            /// </summary>
            /// <param name="propertyName">Navn på den ændrede property.</param>
            protected override void ModelChanged(string propertyName)
            {
                IsModelChangedCalled = true;
                if (ThrowExceptionInModelChanged == false)
                {
                    return;
                }

                throw new IntranetGuiSystemException(_fixture.Create<string>());
            }

            #endregion
        }

        /// <summary>
        /// Tester, at konstruktøren initierer en ViewModel indeholdende grundlæggende kontooplysninger.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererKontoViewModelBase()
        {
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = fixture.BuildRegnskabViewModel();
            string kontonummer = fixture.Create<string>();
            string kontonavn = fixture.Create<string>();
            string beskrivelse = fixture.Create<string>();
            DateTime statusDato = fixture.Create<DateTime>();
            string notat = fixture.Create<string>();
            Mock<IKontoModelBase> kontoModelMock = fixture.BuildKontoModelBaseMock(kontonummer: kontonummer, kontonavn: kontonavn, beskrivelse: beskrivelse, notat: notat, statusDato: statusDato);
            IKontogruppeViewModel kontogruppeViewModel = fixture.BuildKontogruppeViewModel();
            string displayName = fixture.Create<string>();
            byte[] image = Resource.GetEmbeddedResource("Images.Konto.png");
            IFinansstyringRepository finansstyringRepository = fixture.BuildFinansstyringRepository();
            IExceptionHandlerViewModel exceptionHandlerViewModel = fixture.BuildExceptionHandlerViewModel();
            MyKontoViewModel kontoViewModel = new MyKontoViewModel(regnskabViewModel, kontoModelMock.Object, kontogruppeViewModel, displayName, image, finansstyringRepository, exceptionHandlerViewModel);
            Assert.That(kontoViewModel, Is.Not.Null);
            Assert.That(kontoViewModel.Regnskab, Is.Not.Null);
            Assert.That(kontoViewModel.Regnskab, Is.EqualTo(regnskabViewModel));
            Assert.That(kontoViewModel.Kontonummer, Is.Not.Null);
            Assert.That(kontoViewModel.Kontonummer, Is.Not.Empty);
            Assert.That(kontoViewModel.Kontonummer, Is.EqualTo(kontonummer));
            Assert.That(kontoViewModel.KontonummerLabel, Is.Not.Null);
            Assert.That(kontoViewModel.KontonummerLabel, Is.Not.Empty);
            Assert.That(kontoViewModel.KontonummerLabel, Is.EqualTo(Resource.GetText(Text.AccountNumber)));
            Assert.That(kontoViewModel.Kontonavn, Is.Not.Null);
            Assert.That(kontoViewModel.Kontonavn, Is.Not.Empty);
            Assert.That(kontoViewModel.Kontonavn, Is.EqualTo(kontonavn));
            Assert.That(kontoViewModel.KontonavnLabel, Is.Not.Null);
            Assert.That(kontoViewModel.KontonavnLabel, Is.Not.Empty);
            Assert.That(kontoViewModel.KontonavnLabel, Is.EqualTo(Resource.GetText(Text.AccountName)));
            Assert.That(kontoViewModel.Beskrivelse, Is.Not.Null);
            Assert.That(kontoViewModel.Beskrivelse, Is.Not.Empty);
            Assert.That(kontoViewModel.Beskrivelse, Is.EqualTo(beskrivelse));
            Assert.That(kontoViewModel.Notat, Is.Not.Null);
            Assert.That(kontoViewModel.Notat, Is.Not.Empty);
            Assert.That(kontoViewModel.Notat, Is.EqualTo(notat));
            Assert.That(kontoViewModel.Kontogruppe, Is.Not.Null);
            Assert.That(kontoViewModel.Kontogruppe, Is.EqualTo(kontogruppeViewModel));
            Assert.That(kontoViewModel.StatusDato, Is.EqualTo(statusDato));
            Assert.That(kontoViewModel.Kontoværdi, Is.EqualTo(0M));
            Assert.That(kontoViewModel.DisplayName, Is.Not.Null);
            Assert.That(kontoViewModel.DisplayName, Is.Not.Empty);
            Assert.That(kontoViewModel.DisplayName, Is.EqualTo(displayName));
            Assert.That(kontoViewModel.Image, Is.Not.Null);
            Assert.That(kontoViewModel.Image, Is.Not.Empty);
            Assert.That(kontoViewModel.Image, Is.EqualTo(image));
            Assert.That(kontoViewModel.ErRegistreret, Is.False);
            Assert.That(kontoViewModel.RefreshCommand, Is.Not.Null);
            Assert.That(kontoViewModel.Model, Is.Not.Null);
            Assert.That(kontoViewModel.Model, Is.EqualTo(kontoModelMock));
            Assert.That(kontoViewModel.FinansstyringRepository, Is.Not.Null);
            Assert.That(kontoViewModel.FinansstyringRepository, Is.EqualTo(finansstyringRepository));
            Assert.That(kontoViewModel.ExceptionHandler, Is.Not.Null);
            Assert.That(kontoViewModel.ExceptionHandler, Is.EqualTo(exceptionHandlerViewModel));

            kontoModelMock.Verify(m => m.Kontonummer, Times.Once);
            kontoModelMock.Verify(m => m.Kontonavn, Times.Once);
            kontoModelMock.Verify(m => m.Beskrivelse, Times.Once);
            kontoModelMock.Verify(m => m.Notat, Times.Once);
            kontoModelMock.Verify(m => m.Kontogruppe, Times.Never);
            kontoModelMock.Verify(m => m.StatusDato, Times.Once);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis ViewModel for regnskabet er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisRegnskabViewModelErNull()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new MyKontoViewModel(null, fixture.BuildKontoModelBase(), fixture.BuildKontogruppeViewModelBase(), fixture.Create<string>(), fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel()));
            // ReSharper restore ObjectCreationAsStatement
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
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new MyKontoViewModel(fixture.BuildRegnskabViewModel(), null, fixture.BuildKontogruppeViewModelBase(), fixture.Create<string>(), fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel()));
            // ReSharper restore ObjectCreationAsStatement
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
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new MyKontoViewModel(fixture.BuildRegnskabViewModel(), fixture.BuildKontoModelBase(), null, fixture.Create<string>(), fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel()));
            // ReSharper restore ObjectCreationAsStatement
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
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new MyKontoViewModel(fixture.BuildRegnskabViewModel(), fixture.BuildKontoModelBase(), fixture.BuildKontogruppeViewModelBase(), invalidValue, fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel()));
            // ReSharper restore ObjectCreationAsStatement
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
            Fixture fixture = new Fixture();

            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new MyKontoViewModel(fixture.BuildRegnskabViewModel(), fixture.BuildKontoModelBase(), fixture.BuildKontogruppeViewModelBase(), fixture.Create<string>(), null, fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel()));
            // ReSharper restore ObjectCreationAsStatement
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
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new MyKontoViewModel(fixture.BuildRegnskabViewModel(), fixture.BuildKontoModelBase(), fixture.BuildKontogruppeViewModelBase(), fixture.Create<string>(), fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray(), null, fixture.BuildExceptionHandlerViewModel()));
            // ReSharper restore ObjectCreationAsStatement
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
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new MyKontoViewModel(fixture.BuildRegnskabViewModel(), fixture.BuildKontoModelBase(), fixture.BuildKontogruppeViewModelBase(), fixture.Create<string>(), fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray(), fixture.BuildFinansstyringRepository(), null));
            // ReSharper restore ObjectCreationAsStatement
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
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            Mock<IKontoModelBase> kontoModelMock = fixture.BuildKontoModelBaseMock();
            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IKontoViewModelBase<IKontogruppeViewModelBase> kontoViewModel = new MyKontoViewModel(fixture.BuildRegnskabViewModel(), kontoModelMock.Object, fixture.BuildKontogruppeViewModelBase(), fixture.Create<string>(), fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray(), fixture.BuildFinansstyringRepository(), exceptionHandlerViewModelMock.Object);
            Assert.That(kontoViewModel, Is.Not.Null);

            string newValue = fixture.Create<string>();
            kontoViewModel.Kontonavn = newValue;

            kontoModelMock.VerifySet(m => m.Kontonavn = It.Is<string>(value => string.CompareOrdinal(value, newValue) == 0), Times.Once);
            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.IsAny<Exception>()), Times.Never);
        }

        /// <summary>
        /// Tester, at sætteren til Kontonavn kalder HandleException på exceptionhandleren ved exceptions.
        /// </summary>
        [Test]
        public void TestAtKontonavnSetterKalderHandleExceptionOnExceptionHandlerViewModelVedExceptions()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            Exception exception = fixture.Create<Exception>();
            Mock<IKontoModelBase> kontoModelMock = fixture.BuildKontoModelBaseMock(setupCallback: mock => mock.SetupSet(m => m.Kontonavn = It.IsAny<string>()).Throws(exception));
            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IKontoViewModelBase<IKontogruppeViewModelBase> kontoViewModel = new MyKontoViewModel(fixture.BuildRegnskabViewModel(), kontoModelMock.Object, fixture.BuildKontogruppeViewModelBase(), fixture.Create<string>(), fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray(), fixture.BuildFinansstyringRepository(), exceptionHandlerViewModelMock.Object);
            Assert.That(kontoViewModel, Is.Not.Null);

            string newValue = fixture.Create<string>();
            kontoViewModel.Kontonavn = newValue;

            kontoModelMock.VerifySet(m => m.Kontonavn = It.Is<string>(value => string.CompareOrdinal(value, newValue) == 0), Times.Once);
            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.Is<Exception>(value => value != null && value == exception)), Times.Once);
        }

        /// <summary>
        /// Tester, at sætteren til Beskrivelse opdaterer Beskrivelse på modellen, der indeholder de grundlæggende kontooplysninger.
        /// </summary>
        [Test]
        public void TestAtBeskrivelseSetterOpdatererBeskrivelseOnKontoModelBase()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            Mock<IKontoModelBase> kontoModelMock = fixture.BuildKontoModelBaseMock();
            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IKontoViewModelBase<IKontogruppeViewModelBase> kontoViewModel = new MyKontoViewModel(fixture.BuildRegnskabViewModel(), kontoModelMock.Object, fixture.BuildKontogruppeViewModelBase(), fixture.Create<string>(), fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray(), fixture.BuildFinansstyringRepository(), exceptionHandlerViewModelMock.Object);
            Assert.That(kontoViewModel, Is.Not.Null);

            string newValue = fixture.Create<string>();
            kontoViewModel.Beskrivelse = newValue;

            kontoModelMock.VerifySet(m => m.Beskrivelse = It.Is<string>(value => string.CompareOrdinal(value, newValue) == 0), Times.Once);
            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.IsAny<Exception>()), Times.Never);
        }

        /// <summary>
        /// Tester, at sætteren til Beskrivelse kalder HandleException på exceptionhandleren ved exceptions.
        /// </summary>
        [Test]
        public void TestAtBeskrivelseSetterKalderHandleExceptionOnExceptionHandlerViewModelVedExceptions()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            Exception exception = fixture.Create<Exception>();
            Mock<IKontoModelBase> kontoModelMock = fixture.BuildKontoModelBaseMock(setupCallback: mock => mock.SetupSet(m => m.Beskrivelse = It.IsAny<string>()).Throws(exception));
            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IKontoViewModelBase<IKontogruppeViewModelBase> kontoViewModel = new MyKontoViewModel(fixture.BuildRegnskabViewModel(), kontoModelMock.Object, fixture.BuildKontogruppeViewModelBase(), fixture.Create<string>(), fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray(), fixture.BuildFinansstyringRepository(), exceptionHandlerViewModelMock.Object);
            Assert.That(kontoViewModel, Is.Not.Null);

            string newValue = fixture.Create<string>();
            kontoViewModel.Beskrivelse = newValue;

            kontoModelMock.VerifySet(m => m.Beskrivelse = It.Is<string>(value => string.CompareOrdinal(value, newValue) == 0), Times.Once);
            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.Is<Exception>(value => value != null && value == exception)), Times.Once);
        }

        /// <summary>
        /// Tester, at sætteren til Notat opdaterer Notat på modellen, der indeholder de grundlæggende kontooplysninger.
        /// </summary>
        [Test]
        public void TestAtNotatSetterOpdatererNotatOnKontoModelBase()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            Mock<IKontoModelBase> kontoModelMock = fixture.BuildKontoModelBaseMock();
            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IKontoViewModelBase<IKontogruppeViewModelBase> kontoViewModel = new MyKontoViewModel(fixture.BuildRegnskabViewModel(), kontoModelMock.Object, fixture.BuildKontogruppeViewModelBase(), fixture.Create<string>(), fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray(), fixture.BuildFinansstyringRepository(), exceptionHandlerViewModelMock.Object);
            Assert.That(kontoViewModel, Is.Not.Null);

            string newValue = fixture.Create<string>();
            kontoViewModel.Notat = newValue;

            kontoModelMock.VerifySet(m => m.Notat = It.Is<string>(value => string.CompareOrdinal(value, newValue) == 0), Times.Once);
            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.IsAny<Exception>()), Times.Never);
        }

        /// <summary>
        /// Tester, at sætteren til Notat kalder HandleException på exceptionhandleren ved exceptions.
        /// </summary>
        [Test]
        public void TestAtNotatSetterKalderHandleExceptionOnExceptionHandlerViewModelVedExceptions()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            Exception exception = fixture.Create<Exception>();
            Mock<IKontoModelBase> kontoModelMock = fixture.BuildKontoModelBaseMock(setupCallback: mock => mock.SetupSet(m => m.Notat = It.IsAny<string>()).Throws(exception));
            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IKontoViewModelBase<IKontogruppeViewModelBase> kontoViewModel = new MyKontoViewModel(fixture.BuildRegnskabViewModel(), kontoModelMock.Object, fixture.BuildKontogruppeViewModelBase(), fixture.Create<string>(), fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray(), fixture.BuildFinansstyringRepository(), exceptionHandlerViewModelMock.Object);
            Assert.That(kontoViewModel, Is.Not.Null);

            string newValue = fixture.Create<string>();
            kontoViewModel.Notat = newValue;

            kontoModelMock.VerifySet(m => m.Notat = It.Is<string>(value => string.CompareOrdinal(value, newValue) == 0), Times.Once);
            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.Is<Exception>(value => value != null && value == exception)), Times.Once);
        }

        /// <summary>
        /// Tester, at sætteren til Kontogruppe kalder HandleException på exceptionhandleren men en ArgumentNullException, hvis value er null.
        /// </summary>
        [Test]
        public void TestAtKontogruppeSetterKalderHandleExceptionOnExceptionHandlerViewModelMedArgumentNullExceptionHvisValueErNull()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            Mock<IKontoModelBase> kontoModelMock = fixture.BuildKontoModelBaseMock();
            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IKontoViewModelBase<IKontogruppeViewModelBase> kontoViewModel = new MyKontoViewModel(fixture.BuildRegnskabViewModel(), kontoModelMock.Object, fixture.BuildKontogruppeViewModelBase(), fixture.Create<string>(), fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray(), fixture.BuildFinansstyringRepository(), exceptionHandlerViewModelMock.Object);
            Assert.That(kontoViewModel, Is.Not.Null);

            kontoViewModel.Kontogruppe = null;

            kontoModelMock.VerifySet(m => m.Kontogruppe = It.IsAny<int>(), Times.Never);
            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.Is<ArgumentNullException>(value => value != null)), Times.Once);
        }

        /// <summary>
        /// Tester, at sætteren til Kontogruppe ikke opdaterer Kontogruppe på modellen, der indeholder de grundlæggende kontooplysninger, hvis value er den samme ViewModel for kontogruppen.
        /// </summary>
        [Test]
        public void TestAtKontogruppeSetterIkkeOpdatererKontogruppeOnKontoModelBaseHvisValueErDenSammeKontogruppeViewModel()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            Mock<IKontoModelBase> kontoModelMock = fixture.BuildKontoModelBaseMock();
            IKontogruppeViewModelBase kontogruppeViewModel = fixture.BuildKontogruppeViewModelBase();
            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IKontoViewModelBase<IKontogruppeViewModelBase> kontoViewModel = new MyKontoViewModel(fixture.BuildRegnskabViewModel(), kontoModelMock.Object, kontogruppeViewModel, fixture.Create<string>(), fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray(), fixture.BuildFinansstyringRepository(), exceptionHandlerViewModelMock.Object);
            Assert.That(kontoViewModel, Is.Not.Null);
            Assert.That(kontoViewModel.Kontogruppe, Is.Not.Null);
            Assert.That(kontoViewModel.Kontogruppe, Is.EqualTo(kontogruppeViewModel));

            kontoViewModel.Kontogruppe = kontogruppeViewModel;
            Assert.That(kontoViewModel.Kontogruppe, Is.Not.Null);
            Assert.That(kontoViewModel.Kontogruppe, Is.EqualTo(kontogruppeViewModel));

            kontoModelMock.VerifySet(m => m.Kontogruppe = It.IsAny<int>(), Times.Never);
            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.IsAny<Exception>()), Times.Never);
        }

        /// <summary>
        /// Tester, at sætteren til Kontogruppe opdaterer Kontogruppe på modellen, der indeholder de grundlæggende kontooplysninger, hvis value ikke er den samme ViewModel for kontogruppen.
        /// </summary>
        [Test]
        public void TestAtKontogruppeSetterOpdatererKontogruppeOnKontoModelBaseHvisValueIkkeErDenSammeKontogruppeViewModel()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            Mock<IKontoModelBase> kontoModelMock = fixture.BuildKontoModelBaseMock();
            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IKontoViewModelBase<IKontogruppeViewModelBase> kontoViewModel = new MyKontoViewModel(fixture.BuildRegnskabViewModel(), kontoModelMock.Object, fixture.BuildKontogruppeViewModelBase(), fixture.Create<string>(), fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray(), fixture.BuildFinansstyringRepository(), exceptionHandlerViewModelMock.Object);
            Assert.That(kontoViewModel, Is.Not.Null);

            int nummer = fixture.Create<int>();
            IKontogruppeViewModelBase newValue = fixture.BuildKontogruppeViewModelBase(nummer);
            Assert.That(newValue, Is.Not.Null);
            Assert.That(newValue.Nummer, Is.Not.EqualTo(kontoModelMock.Object.Kontogruppe));

            kontoViewModel.Kontogruppe = newValue;
            Assert.That(kontoViewModel.Kontogruppe, Is.Not.Null);
            Assert.That(kontoViewModel.Kontogruppe, Is.EqualTo(newValue));

            kontoModelMock.VerifySet(m => m.Kontogruppe = It.Is<int>(value => value == newValue.Nummer), Times.Once);
            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.IsAny<Exception>()), Times.Never);
        }

        /// <summary>
        /// Tester, at sætteren til Kontogruppe kalder HandleException på exceptionhandleren ved exceptions.
        /// </summary>
        [Test]
        public void TestAtKontogruppeSetterKalderHandleExceptionOnExceptionHandlerViewModelVedExceptions()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            Exception exception = fixture.Create<Exception>();
            Mock<IKontoModelBase> kontoModelMock = fixture.BuildKontoModelBaseMock(setupCallback: mock => mock.SetupSet(m => m.Kontogruppe = It.IsAny<int>()).Throws(exception));
            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IKontoViewModelBase<IKontogruppeViewModelBase> kontoViewModel = new MyKontoViewModel(fixture.BuildRegnskabViewModel(), kontoModelMock.Object, fixture.BuildKontogruppeViewModelBase(), fixture.Create<string>(), fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray(), fixture.BuildFinansstyringRepository(), exceptionHandlerViewModelMock.Object);
            Assert.That(kontoViewModel, Is.Not.Null);

            int nummer = fixture.Create<int>();
            IKontogruppeViewModelBase newValue = fixture.BuildKontogruppeViewModelBase(nummer);
            Assert.That(newValue, Is.Not.Null);
            Assert.That(newValue.Nummer, Is.Not.EqualTo(kontoModelMock.Object.Kontogruppe));

            kontoViewModel.Kontogruppe = newValue;

            kontoModelMock.VerifySet(m => m.Kontogruppe = It.Is<int>(value => value == newValue.Nummer), Times.Once);
            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.Is<Exception>(value => value != null && value == exception)), Times.Once);
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

        /// <summary>
        /// Tester, at sætteren til ErRegistreret opdaterer angivelsen af, om kontoen er registreret i forhold til opgørelsen og/eller balancen.
        /// </summary>
        [Test]
        public void TestAtErRegistreretSetterOpdatererErRegistreret()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IKontoModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontoModelBase>()));
            fixture.Customize<IKontogruppeViewModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeViewModelBase>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exceptionHandlerViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var kontoViewModel = new MyKontoViewModel(fixture.Create<IRegnskabViewModel>(), fixture.Create<IKontoModelBase>(), fixture.Create<IKontogruppeViewModelBase>(), fixture.Create<string>(), Resource.GetEmbeddedResource("Images.Konto.png"), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(kontoViewModel, Is.Not.Null);
            Assert.That(kontoViewModel.ErRegistreret, Is.False);

            kontoViewModel.ErRegistreret = true;
            Assert.That(kontoViewModel.ErRegistreret, Is.True);
        }

        /// <summary>
        /// Tester, at sætteren til ErRegistreret rejser PropertyChanged, når angivelsen af, om kontoen er registreret i forhold til opgørelsen og/eller balancen, opdateres.
        /// </summary>
        [Test]
        [TestCase("ErRegistreret")]
        public void TestAtErRegistreretSetterRejserPropertyChangedVedOpdateringAfErRegistreret(string expectedPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IKontoModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontoModelBase>()));
            fixture.Customize<IKontogruppeViewModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeViewModelBase>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exceptionHandlerViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var kontoViewModel = new MyKontoViewModel(fixture.Create<IRegnskabViewModel>(), fixture.Create<IKontoModelBase>(), fixture.Create<IKontogruppeViewModelBase>(), fixture.Create<string>(), Resource.GetEmbeddedResource("Images.Konto.png"), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(kontoViewModel, Is.Not.Null);

            var eventCalled = false;
            kontoViewModel.PropertyChanged += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Empty);
                if (string.Compare(e.PropertyName, expectedPropertyName, StringComparison.Ordinal) == 0)
                {
                    eventCalled = true;
                }
            };

            Assert.That(eventCalled, Is.False);
            kontoViewModel.ErRegistreret = kontoViewModel.ErRegistreret;
            Assert.That(eventCalled, Is.False);
            kontoViewModel.ErRegistreret = !kontoViewModel.ErRegistreret;
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnKontoModelEventHandler rejser PropertyChanged, når modellen, der indeholder grundlæggende kontooplysninger, opdateres.
        /// </summary>
        [Test]
        [TestCase("Regnskabsnummer", "Regnskabsnummer")]
        [TestCase("Kontonummer", "Kontonummer")]
        [TestCase("Kontonavn", "Kontonavn")]
        [TestCase("Beskrivelse", "Beskrivelse")]
        [TestCase("Notat", "Notat")]
        [TestCase("Kontogruppe", "Kontogruppe")]
        [TestCase("StatusDato", "StatusDato")]
        public void TestAtPropertyChangedOnKontoModelEventHandlerRejserPropertyChangedOnKontoModelUpdate(string propertyNameToRaise, string expectPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IKontoModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontoModelBase>()));
            fixture.Customize<IKontogruppeViewModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeViewModelBase>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var kontoModelMock = fixture.Create<IKontoModelBase>();

            var exceptionHandlerViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var kontoViewModel = new MyKontoViewModel(fixture.Create<IRegnskabViewModel>(), kontoModelMock, fixture.Create<IKontogruppeViewModelBase>(), fixture.Create<string>(), Resource.GetEmbeddedResource("Images.Konto.png"), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock)
                {
                    ThrowExceptionInModelChanged = false
                };
            Assert.That(kontoViewModel, Is.Not.Null);
            Assert.That(kontoViewModel.ThrowExceptionInModelChanged, Is.False);

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

        /// <summary>
        /// Tester, at PropertyChangedOnKontoModelEventHandler kalder metoden ModelChanged, når modellen, der indeholder grundlæggende kontooplysninger, opdateres.
        /// </summary>
        [Test]
        [TestCase("Regnskabsnummer")]
        [TestCase("Kontonummer")]
        [TestCase("Kontonavn")]
        [TestCase("Beskrivelse")]
        [TestCase("Notat")]
        [TestCase("Kontogruppe")]
        [TestCase("StatusDato")]
        public void TestAtPropertyChangedOnKontoModelEventHandlerKalderModelChangedOnKontoModelUpdate(string propertyNameToRaise)
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IKontoModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontoModelBase>()));
            fixture.Customize<IKontogruppeViewModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeViewModelBase>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var kontoModelMock = fixture.Create<IKontoModelBase>();

            var exceptionHandlerViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var kontoViewModel = new MyKontoViewModel(fixture.Create<IRegnskabViewModel>(), kontoModelMock, fixture.Create<IKontogruppeViewModelBase>(), fixture.Create<string>(), Resource.GetEmbeddedResource("Images.Konto.png"), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock)
                {
                    ThrowExceptionInModelChanged = false
                };
            Assert.That(kontoViewModel, Is.Not.Null);
            Assert.That(kontoViewModel.ThrowExceptionInModelChanged, Is.False);

            Assert.That(kontoViewModel.IsModelChangedCalled, Is.False);
            kontoModelMock.Raise(m => m.PropertyChanged += null, kontoModelMock, new PropertyChangedEventArgs(propertyNameToRaise));
            Assert.That(kontoViewModel.IsModelChangedCalled, Is.True);

            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at PropertyChangedOnKontoModelEventHandler kalder HandleException på exceptionhandleren, hvis metoden ModelChanged kaster en exception.
        /// </summary>
        [Test]
        [TestCase("Regnskabsnummer")]
        [TestCase("Kontonummer")]
        [TestCase("Kontonavn")]
        [TestCase("Beskrivelse")]
        [TestCase("Notat")]
        [TestCase("Kontogruppe")]
        [TestCase("StatusDato")]
        public void TestAtPropertyChangedOnKontoModelEventHandlerKalderHandleExceptionOnExceptionHandlerViewModelHvisModelChangedKasterException(string propertyNameToRaise)
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IKontoModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontoModelBase>()));
            fixture.Customize<IKontogruppeViewModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeViewModelBase>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var kontoModelMock = fixture.Create<IKontoModelBase>();

            var exceptionHandlerViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var kontoViewModel = new MyKontoViewModel(fixture.Create<IRegnskabViewModel>(), kontoModelMock, fixture.Create<IKontogruppeViewModelBase>(), fixture.Create<string>(), Resource.GetEmbeddedResource("Images.Konto.png"), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock)
                {
                    ThrowExceptionInModelChanged = true
                };
            Assert.That(kontoViewModel, Is.Not.Null);
            Assert.That(kontoViewModel.ThrowExceptionInModelChanged, Is.True);

            Assert.That(kontoViewModel.IsModelChangedCalled, Is.False);
            kontoModelMock.Raise(m => m.PropertyChanged += null, kontoModelMock, new PropertyChangedEventArgs(propertyNameToRaise));
            Assert.That(kontoViewModel.IsModelChangedCalled, Is.True);

            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at PropertyChangedOnKontoModelEventHandler kaster en ArgumentNullException, hvis objektet, der rejser eventet, er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnKontoModelEventHandlerKasterArgumentNullExceptionHvisSenderErNull()
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

            var exception = Assert.Throws<ArgumentNullException>(() => kontoModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("sender"));
            Assert.That(exception.InnerException, Is.Null);

            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at PropertyChangedOnKontoModelEventHandler kaster en ArgumentNullException, hvis argumenter til eventet er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnKontoModelEventHandlerKasterArgumentNullExceptionHvisEventArgsErNull()
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

            var exception = Assert.Throws<ArgumentNullException>(() => kontoModelMock.Raise(m => m.PropertyChanged += null, kontoModelMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("eventArgs"));
            Assert.That(exception.InnerException, Is.Null);

            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }
    }
}
