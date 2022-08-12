using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
using Text = OSDevGrp.OSIntranet.Gui.Resources.Text;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Finansstyring
{
    /// <summary>
    /// Tester ViewModel for en liste indeholdende regnskaber.
    /// </summary>
    [TestFixture]
    public class RegnskabslisteViewModelTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer ViewModel for en liste indeholdende regnskaber.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererRegnskabslisteViewModel()
        {
            Fixture fixture = new Fixture();

            IRegnskabslisteViewModel regnskabslisteViewModel = new RegnskabslisteViewModel(fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabslisteViewModel, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.DisplayName, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.DisplayName, Is.Not.Empty);
            Assert.That(regnskabslisteViewModel.DisplayName, Is.EqualTo(Resource.GetText(Text.Accountings)));
            Assert.That(regnskabslisteViewModel.StatusDato, Is.EqualTo(DateTime.Now).Within(3).Seconds);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Empty);
            Assert.That(regnskabslisteViewModel.RefreshCommand, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.RefreshCommand, Is.TypeOf<RegnskabslisteRefreshCommand>());
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis repositoryet til finansstyring er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFinansstyringRepositoryErNull()
        {
            Fixture fixture = new Fixture();

            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new RegnskabslisteViewModel(null, fixture.BuildExceptionHandlerViewModel()));
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

            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new RegnskabslisteViewModel(fixture.BuildFinansstyringRepository(), null));
            // ReSharper restore ObjectCreationAsStatement
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("exceptionHandlerViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at sætteren til StatusDato opdaterer statusdatoen for regnskabslisten.
        /// </summary>
        [Test]
        public void TestAtStatusDatoSetterOpdatererStatusDato()
        {
            Fixture fixture = new Fixture();

            IRegnskabslisteViewModel regnskabslisteViewModel = new RegnskabslisteViewModel(fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabslisteViewModel, Is.Not.Null);

            DateTime statusDato = new DateTime(DateTime.Now.Year, 1, 1, 0, 0, 0, 0);
            regnskabslisteViewModel.StatusDato = statusDato;

            Assert.That(regnskabslisteViewModel.StatusDato, Is.EqualTo(statusDato));
        }

        /// <summary>
        /// Tester, at sætteren til StatusDato rejser PropertyChanged ved ændring af statusdatoen.
        /// </summary>
        [Test]
        public void TestAtStatusDatoSetterRejserPropertyChangedVedVedOpdateringAfStatusDato()
        {
            Fixture fixture = new Fixture();

            IRegnskabslisteViewModel regnskabslisteViewModel = new RegnskabslisteViewModel(fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel())
            {
                StatusDato = DateTime.Now
            };
            Assert.That(regnskabslisteViewModel, Is.Not.Null);

            bool eventCalled = false;
            regnskabslisteViewModel.PropertyChanged += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Empty);
                Assert.That(e.PropertyName, Is.EqualTo("StatusDato"));
                eventCalled = true;
            };

            Assert.That(eventCalled, Is.False);
            regnskabslisteViewModel.StatusDato = regnskabslisteViewModel.StatusDato;
            Assert.That(eventCalled, Is.False);
            regnskabslisteViewModel.StatusDato = regnskabslisteViewModel.StatusDato.AddHours(-1);
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at RegnskabAdd kaster en ArgumentNullException, hvis regnskabet, der forsøges tilføjet, er null.
        /// </summary>
        [Test]
        public void TestAtRegnskabAddKasterArgumentNullExceptionHvisRegnskabViewModelErNull()
        {
            Fixture fixture = new Fixture();

            IRegnskabslisteViewModel regnskabslisteViewModel = new RegnskabslisteViewModel(fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabslisteViewModel, Is.Not.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => regnskabslisteViewModel.RegnskabAdd(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("regnskab"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at RegnskabAdd tilføjer et regnskab til listen af regnskaber.
        /// </summary>
        [Test]
        public void TestAtRegnskabAddAdderRegnskabViewModel()
        {
            Fixture fixture = new Fixture();

            IRegnskabslisteViewModel regnskabslisteViewModel = new RegnskabslisteViewModel(fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabslisteViewModel, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Empty);

            regnskabslisteViewModel.RegnskabAdd(fixture.BuildRegnskabViewModel());
            Assert.That(regnskabslisteViewModel, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Not.Empty);
        }

        /// <summary>
        /// Tester, at RegnskabAdd rejser PropertyChanged ved tilføjelse af et regnskab til listen af regnskaber.
        /// </summary>
        [Test]
        public void TestAtRegnskabAddRejserPropertyChangedVedAddAfRegnskab()
        {
            Fixture fixture = new Fixture();

            IRegnskabslisteViewModel regnskabslisteViewModel = new RegnskabslisteViewModel(fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabslisteViewModel, Is.Not.Null);

            bool eventCalledForRegnskaber = false;
            regnskabslisteViewModel.PropertyChanged += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Empty);
                if (string.CompareOrdinal(e.PropertyName, "Regnskaber") == 0)
                {
                    eventCalledForRegnskaber = true;
                }
            };

            Assert.That(eventCalledForRegnskaber, Is.False);
            regnskabslisteViewModel.RegnskabAdd(fixture.BuildRegnskabViewModel());
            Assert.That(eventCalledForRegnskaber, Is.True);
        }

        /// <summary>
        /// Tester, at RegnskabGet henter ViewModel for regnskabet fra listen af regnskaber.
        /// </summary>
        [Test]
        public async Task TestAtRegnskabGetHenterRegnskabViewModelFraRegnskaber()
        {
            Fixture fixture = new Fixture();

            Mock<IFinansstyringRepository> finansstyringRepositoryMock = fixture.BuildFinansstyringRepositoryMock();
            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IRegnskabslisteViewModel regnskabslisteViewModel = new RegnskabslisteViewModel(finansstyringRepositoryMock.Object, exceptionHandlerViewModelMock.Object);
            Assert.That(regnskabslisteViewModel, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Empty);

            int regnskabsnummer = fixture.Create<int>();
            IRegnskabViewModel regnskabViewModel = fixture.BuildRegnskabViewModel();
            regnskabslisteViewModel.RegnskabAdd(regnskabViewModel);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Not.Empty);

            IRegnskabViewModel loadedRegnskabModel = await regnskabslisteViewModel.RegnskabGetAsync(regnskabsnummer);
            Assert.That(loadedRegnskabModel, Is.Not.Null);
            Assert.That(loadedRegnskabModel, Is.EqualTo(regnskabViewModel));

            finansstyringRepositoryMock.Verify(m => m.RegnskabslisteGetAsync(), Times.Never);
            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.IsAny<Exception>()), Times.Never);
        }

        /// <summary>
        /// Tester, at RegnskabGet henter ViewModel for regnskabet gennem repositoryet til regnskaber.
        /// </summary>
        [Test]
        public async Task TestAtRegnskabGetHenterRegnskabViewModelGennemFinansstyringRepository()
        {
            Fixture fixture = new Fixture();

            int regnskabsnummer = fixture.Create<int>();
            IEnumerable<IRegnskabModel> regnskabModelCollection = new[]
            {
                fixture.BuildRegnskabModel(regnskabsnummer),
                fixture.BuildRegnskabModel(regnskabsnummer + 1),
                fixture.BuildRegnskabModel(regnskabsnummer + 2)
            };
            Mock<IFinansstyringRepository> finansstyringRepositoryMock = fixture.BuildFinansstyringRepositoryMock(regnskabModelCollection);
            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IRegnskabslisteViewModel regnskabslisteViewModel = new RegnskabslisteViewModel(finansstyringRepositoryMock.Object, exceptionHandlerViewModelMock.Object);
            Assert.That(regnskabslisteViewModel, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Empty);

            IRegnskabViewModel regnskabViewModel = await regnskabslisteViewModel.RegnskabGetAsync(regnskabsnummer);
            Assert.That(regnskabViewModel, Is.Not.Null);

            finansstyringRepositoryMock.Verify(m => m.RegnskabslisteGetAsync(), Times.Once);
            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.IsAny<Exception>()), Times.Never);
        }

        /// <summary>
        /// Tester, at RegnskabGet returnerer null, hvis der ikke findes et regnskab med regnskabsnummeret.
        /// </summary>
        [Test]
        public async Task TestAtRegnskabGetReturnererNullHvisRegnskabIkkeFindes()
        {
            Fixture fixture = new Fixture();

            IEnumerable<IRegnskabModel> regnskabModelCollection = new[]
            {
                fixture.BuildRegnskabModel(),
                fixture.BuildRegnskabModel(),
                fixture.BuildRegnskabModel()
            };
            Mock<IFinansstyringRepository> finansstyringRepositoryMock = fixture.BuildFinansstyringRepositoryMock(regnskabModelCollection);
            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IRegnskabslisteViewModel regnskabslisteViewModel = new RegnskabslisteViewModel(finansstyringRepositoryMock.Object, exceptionHandlerViewModelMock.Object);
            Assert.That(regnskabslisteViewModel, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Empty);

            IRegnskabViewModel regnskabViewModel = await regnskabslisteViewModel.RegnskabGetAsync(fixture.Create<int>());
            Assert.That(regnskabViewModel, Is.Null);

            finansstyringRepositoryMock.Verify(m => m.RegnskabslisteGetAsync(), Times.Once);
            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.IsAny<Exception>()), Times.Never);
        }

        /// <summary>
        /// Tester, at RegnskabGet returnerer null og kalder HandleException i exceptionhandleren ved IntranetGuiRepositoryException.
        /// </summary>
        [Test]
        public async Task TestAtRegnskabGetReturnererNullOgKalderHandleExceptionVedIntranetGuiRepositoryException()
        {
            Fixture fixture = new Fixture();

            IntranetGuiRepositoryException intranetGuiRepositoryException = new IntranetGuiRepositoryException(fixture.Create<string>());
            Mock<IFinansstyringRepository> finansstyringRepositoryMock = fixture.BuildFinansstyringRepositoryMock(setupCallback: mock => mock.Setup(m => m.RegnskabslisteGetAsync()).Throws(intranetGuiRepositoryException));
            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IRegnskabslisteViewModel regnskabslisteViewModel = new RegnskabslisteViewModel(finansstyringRepositoryMock.Object, exceptionHandlerViewModelMock.Object);
            Assert.That(regnskabslisteViewModel, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Empty);

            IRegnskabViewModel regnskabViewModel = await regnskabslisteViewModel.RegnskabGetAsync(fixture.Create<int>());
            Assert.That(regnskabViewModel, Is.Null);

            finansstyringRepositoryMock.Verify(m => m.RegnskabslisteGetAsync(), Times.Once);
            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.Is<IntranetGuiRepositoryException>(value => value != null && value == intranetGuiRepositoryException)), Times.Once);
        }

        /// <summary>
        /// Tester, at RegnskabGet returnerer null og kalder HandleException i exceptionhandleren ved IntranetGuiSystemException.
        /// </summary>
        [Test]
        public async Task TestAtRegnskabGetReturnererNullOgKalderHandleExceptionVedIntranetGuiSystemException()
        {
            Fixture fixture = new Fixture();

            IntranetGuiSystemException intranetGuiSystemException = new IntranetGuiSystemException(fixture.Create<string>());
            Mock<IFinansstyringRepository> finansstyringRepositoryMock = fixture.BuildFinansstyringRepositoryMock(setupCallback: mock => mock.Setup(m => m.RegnskabslisteGetAsync()).Throws(intranetGuiSystemException));
            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IRegnskabslisteViewModel regnskabslisteViewModel = new RegnskabslisteViewModel(finansstyringRepositoryMock.Object, exceptionHandlerViewModelMock.Object);
            Assert.That(regnskabslisteViewModel, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Empty);

            IRegnskabViewModel regnskabViewModel = await regnskabslisteViewModel.RegnskabGetAsync(fixture.Create<int>());
            Assert.That(regnskabViewModel, Is.Null);

            finansstyringRepositoryMock.Verify(m => m.RegnskabslisteGetAsync(), Times.Once);
            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.Is<IntranetGuiSystemException>(value => value != null && value == intranetGuiSystemException)), Times.Once);
        }

        /// <summary>
        /// Tester, at RegnskabGet returnerer null og kalder HandleException i exceptionhandleren ved IntranetGuiBusinessException.
        /// </summary>
        [Test]
        public async Task TestAtRegnskabGetReturnererNullOgKalderHandleExceptionVedIntranetGuiBusinessException()
        {
            Fixture fixture = new Fixture();

            IntranetGuiBusinessException intranetGuiBusinessException = new IntranetGuiBusinessException(fixture.Create<string>());
            Mock<IFinansstyringRepository> finansstyringRepositoryMock = fixture.BuildFinansstyringRepositoryMock(setupCallback: mock => mock.Setup(m => m.RegnskabslisteGetAsync()).Throws(intranetGuiBusinessException));
            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IRegnskabslisteViewModel regnskabslisteViewModel = new RegnskabslisteViewModel(finansstyringRepositoryMock.Object, exceptionHandlerViewModelMock.Object);
            Assert.That(regnskabslisteViewModel, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Empty);

            IRegnskabViewModel regnskabViewModel = await regnskabslisteViewModel.RegnskabGetAsync(fixture.Create<int>());
            Assert.That(regnskabViewModel, Is.Null);

            finansstyringRepositoryMock.Verify(m => m.RegnskabslisteGetAsync(), Times.Once);
            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.Is<IntranetGuiBusinessException>(value => value != null && value == intranetGuiBusinessException)), Times.Once);
        }

        /// <summary>
        /// Tester, at RegnskabGet returnerer null og kalder HandleException i exceptionhandleren ved Exception.
        /// </summary>
        [Test]
        public async Task TestAtRegnskabGetReturnererNullOgKalderHandleExceptionVedException()
        {
            Fixture fixture = new Fixture();

            Exception exception = new Exception(fixture.Create<string>());
            Mock<IFinansstyringRepository> finansstyringRepositoryMock = fixture.BuildFinansstyringRepositoryMock(setupCallback: mock => mock.Setup(m => m.RegnskabslisteGetAsync()).Throws(exception));
            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IRegnskabslisteViewModel regnskabslisteViewModel = new RegnskabslisteViewModel(finansstyringRepositoryMock.Object, exceptionHandlerViewModelMock.Object);
            Assert.That(regnskabslisteViewModel, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Empty);

            IRegnskabViewModel regnskabViewModel = await regnskabslisteViewModel.RegnskabGetAsync(fixture.Create<int>());
            Assert.That(regnskabViewModel, Is.Null);

            finansstyringRepositoryMock.Verify(m => m.RegnskabslisteGetAsync(), Times.Once);
            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.Is<IntranetGuiBusinessException>(value => value != null && value.InnerException != null && value.InnerException == exception)), Times.Once);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnRegnskabViewModelEventHandler rejser PropertyChanged, når en ViewModel for et regnskab opdateres.
        /// </summary>
        [Test]
        [TestCase("Nummer", "Regnskaber")]
        public void TestAtPropertyChangedOnRegnskabViewModelEventHandlerRejserPropertyChangedOnRegnskabViewModelUpdate(string propertyName, string expectedPropertyName)
        {
            Fixture fixture = new Fixture();

            IRegnskabslisteViewModel regnskabslisteViewModel = new RegnskabslisteViewModel(fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabslisteViewModel, Is.Not.Null);

            Mock<IRegnskabViewModel> regnskabViewModelMock = fixture.BuildRegnskabViewModelMock();
            regnskabslisteViewModel.RegnskabAdd(regnskabViewModelMock.Object);

            bool eventCalled = false;
            regnskabslisteViewModel.PropertyChanged += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Empty);
                if (string.CompareOrdinal(e.PropertyName, expectedPropertyName) == 0)
                {
                    eventCalled = true;
                }
            };

            Assert.That(eventCalled, Is.False);
            regnskabViewModelMock.Raise(m => m.PropertyChanged += null, regnskabViewModelMock, new PropertyChangedEventArgs(propertyName));
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnRegnskabViewModelEventHandler kaster en ArgumentNullException, hvis objektet, der rejser eventet, er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnRegnskabViewModelEventHandlerKasterArgumentNullExceptionHvisSenderErNull()
        {
            Fixture fixture = new Fixture();

            IRegnskabslisteViewModel regnskabslisteViewModel = new RegnskabslisteViewModel(fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabslisteViewModel, Is.Not.Null);

            Mock<IRegnskabViewModel> regnskabViewModelMock = fixture.BuildRegnskabViewModelMock();
            regnskabslisteViewModel.RegnskabAdd(regnskabViewModelMock.Object);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => regnskabViewModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("sender"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnRegnskabViewModelEventHandler kaster en ArgumentNullException, hvis argumenter til eventet er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnRegnskabViewModelEventHandlerKasterArgumentNullExceptionHvisEventArgsErNull()
        {
            Fixture fixture = new Fixture();

            IRegnskabslisteViewModel regnskabslisteViewModel = new RegnskabslisteViewModel(fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabslisteViewModel, Is.Not.Null);

            Mock<IRegnskabViewModel> regnskabViewModelMock = fixture.BuildRegnskabViewModelMock();
            regnskabslisteViewModel.RegnskabAdd(regnskabViewModelMock.Object);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => regnskabViewModelMock.Raise(m => m.PropertyChanged += null, regnskabViewModelMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("e"));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}