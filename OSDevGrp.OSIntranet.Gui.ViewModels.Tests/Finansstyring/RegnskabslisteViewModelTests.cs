using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
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
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var regnskabslisteViewModel = new RegnskabslisteViewModel(fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
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
            var fixture = new Fixture();
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new RegnskabslisteViewModel(null, fixture.Create<IExceptionHandlerViewModel>()));
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

            var exception = Assert.Throws<ArgumentNullException>(() => new RegnskabslisteViewModel(fixture.Create<IFinansstyringRepository>(), null));
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
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var regnskabslisteViewModel = new RegnskabslisteViewModel(fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabslisteViewModel, Is.Not.Null);

            var statusDato = new DateTime(DateTime.Now.Year, 1, 1, 0, 0, 0, 0);
            regnskabslisteViewModel.StatusDato = statusDato;
            Assert.That(regnskabslisteViewModel.StatusDato, Is.EqualTo(statusDato));
        }

        /// <summary>
        /// Tester, at sætteren til StatusDato rejser PropertyChanged ved ændring af statusdatoen.
        /// </summary>
        [Test]
        public void TestAtStatusDatoSetterRejserPropertyChangedVedVedOpdateringAfStatusDato()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var regnskabslisteViewModel = new RegnskabslisteViewModel(fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabslisteViewModel, Is.Not.Null);

            var eventCalled = false;
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
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var regnskabslisteViewModel = new RegnskabslisteViewModel(fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabslisteViewModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => regnskabslisteViewModel.RegnskabAdd(null));
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
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));

            var regnskabslisteViewModel = new RegnskabslisteViewModel(fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabslisteViewModel, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Empty);

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabslisteViewModel.RegnskabAdd(regnskabViewModelMock);
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
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));

            var regnskabslisteViewModel = new RegnskabslisteViewModel(fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabslisteViewModel, Is.Not.Null);

            var eventCalledForRegnskaber = false;
            regnskabslisteViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, "Regnskaber", StringComparison.Ordinal) == 0)
                    {
                        eventCalledForRegnskaber = true;
                    }
                };

            Assert.That(eventCalledForRegnskaber, Is.False);
            regnskabslisteViewModel.RegnskabAdd(fixture.Create<IRegnskabViewModel>());
            Assert.That(eventCalledForRegnskaber, Is.True);
        }

        /// <summary>
        /// Tester, at RegnskabGet henter ViewModel for regnskabet fra listen af regnskaber.
        /// </summary>
        [Test]
        public async void TestAtRegnskabGetHenterRegnskabViewModelFraRegnskaber()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IRegnskabViewModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(fixture.Create<int>())
                        .Repeat.Any();
                    return mock;
                }));

            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var regnskabslisteViewModel = new RegnskabslisteViewModel(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(regnskabslisteViewModel, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Empty);

            var regnskabViewModelMock = fixture.Create<IRegnskabViewModel>();
            regnskabslisteViewModel.RegnskabAdd(regnskabViewModelMock);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Not.Empty);

            var result = await regnskabslisteViewModel.RegnskabGetAsync(regnskabViewModelMock.Nummer);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(regnskabViewModelMock));

            finansstyringRepositoryMock.AssertWasNotCalled(m => m.RegnskabslisteGetAsync());
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at RegnskabGet henter ViewModel for regnskabet gennem repositoryet til regnskaber.
        /// </summary>
        [Test]
        public async void TestAtRegnskabGetHenterRegnskabViewModelGennemFinansstyringRepository()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IRegnskabModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(fixture.Create<int>())
                        .Repeat.Any();
                    return mock;
                }));

            var regnskabModelMockCollection = fixture.CreateMany<IRegnskabModel>(3).ToList();
            Func<IEnumerable<IRegnskabModel>> regnskabslisteGetter = () => regnskabModelMockCollection;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.RegnskabslisteGetAsync())
                                       .Return(Task.Run(regnskabslisteGetter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var regnskabslisteViewModel = new RegnskabslisteViewModel(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(regnskabslisteViewModel, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Empty);

            var result = await regnskabslisteViewModel.RegnskabGetAsync(regnskabModelMockCollection.ElementAt(1).Nummer);
            Assert.That(result, Is.Not.Null);

            finansstyringRepositoryMock.AssertWasCalled(m => m.RegnskabslisteGetAsync());
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at RegnskabGet returnerer null, hvis der ikke findes et regnskab med regnskabsnummeret.
        /// </summary>
        [Test]
        public async void TestAtRegnskabGetReturnererNullHvisRegnskabIkkeFindes()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IRegnskabModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(fixture.Create<int>())
                        .Repeat.Any();
                    return mock;
                }));

            var regnskabModelMockCollection = fixture.CreateMany<IRegnskabModel>(3).ToList();
            Func<IEnumerable<IRegnskabModel>> regnskabslisteGetter = () => regnskabModelMockCollection;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.RegnskabslisteGetAsync())
                                       .Return(Task.Run(regnskabslisteGetter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var regnskabslisteViewModel = new RegnskabslisteViewModel(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(regnskabslisteViewModel, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Empty);

            var result = await regnskabslisteViewModel.RegnskabGetAsync(fixture.Create<int>());
            Assert.That(result, Is.Null);

            finansstyringRepositoryMock.AssertWasCalled(m => m.RegnskabslisteGetAsync());
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at RegnskabGet returnerer null og kalder HandleException i exceptionhandleren ved IntranetGuiRepositoryException.
        /// </summary>
        [Test]
        public async void TestAtRegnskabGetReturnererNullOgKalderHandleExceptionVedIntranetGuiRepositoryException()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IRegnskabModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(fixture.Create<int>())
                        .Repeat.Any();
                    return mock;
                }));

            Func<IEnumerable<IRegnskabModel>> regnskabslisteGetter = () =>
                {
                    var message = fixture.Create<string>();
                    throw new IntranetGuiRepositoryException(message);
                };
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.RegnskabslisteGetAsync())
                                       .Return(Task.Run(regnskabslisteGetter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var regnskabslisteViewModel = new RegnskabslisteViewModel(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(regnskabslisteViewModel, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Empty);

            var result = await regnskabslisteViewModel.RegnskabGetAsync(fixture.Create<int>());
            Assert.That(result, Is.Null);

            finansstyringRepositoryMock.AssertWasCalled(m => m.RegnskabslisteGetAsync());
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiRepositoryException>.Is.Anything));
        }

        /// <summary>
        /// Tester, at RegnskabGet returnerer null og kalder HandleException i exceptionhandleren ved IntranetGuiSystemException.
        /// </summary>
        [Test]
        public async void TestAtRegnskabGetReturnererNullOgKalderHandleExceptionVedIntranetGuiSystemException()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IRegnskabModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(fixture.Create<int>())
                        .Repeat.Any();
                    return mock;
                }));

            Func<IEnumerable<IRegnskabModel>> regnskabslisteGetter = () =>
                {
                    var message = fixture.Create<string>();
                    throw new IntranetGuiSystemException(message);
                };
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.RegnskabslisteGetAsync())
                                       .Return(Task.Run(regnskabslisteGetter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var regnskabslisteViewModel = new RegnskabslisteViewModel(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(regnskabslisteViewModel, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Empty);

            var result = await regnskabslisteViewModel.RegnskabGetAsync(fixture.Create<int>());
            Assert.That(result, Is.Null);

            finansstyringRepositoryMock.AssertWasCalled(m => m.RegnskabslisteGetAsync());
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.Anything));
        }

        /// <summary>
        /// Tester, at RegnskabGet returnerer null og kalder HandleException i exceptionhandleren ved IntranetGuiBusinessException.
        /// </summary>
        [Test]
        public async void TestAtRegnskabGetReturnererNullOgKalderHandleExceptionVedIntranetGuiBusinessException()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IRegnskabModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(fixture.Create<int>())
                        .Repeat.Any();
                    return mock;
                }));

            Func<IEnumerable<IRegnskabModel>> regnskabslisteGetter = () =>
                {
                    var message = fixture.Create<string>();
                    throw new IntranetGuiBusinessException(message);
                };
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.RegnskabslisteGetAsync())
                                       .Return(Task.Run(regnskabslisteGetter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var regnskabslisteViewModel = new RegnskabslisteViewModel(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(regnskabslisteViewModel, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Empty);

            var result = await regnskabslisteViewModel.RegnskabGetAsync(fixture.Create<int>());
            Assert.That(result, Is.Null);

            finansstyringRepositoryMock.AssertWasCalled(m => m.RegnskabslisteGetAsync());
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiBusinessException>.Is.Anything));
        }

        /// <summary>
        /// Tester, at RegnskabGet returnerer null og kalder HandleException i exceptionhandleren ved Exception.
        /// </summary>
        [Test]
        public async void TestAtRegnskabGetReturnererNullOgKalderHandleExceptionVedException()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IRegnskabModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(fixture.Create<int>())
                        .Repeat.Any();
                    return mock;
                }));

            Func<IEnumerable<IRegnskabModel>> regnskabslisteGetter = () =>
                {
                    var message = fixture.Create<string>();
                    throw new Exception(message);
                };
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.RegnskabslisteGetAsync())
                                       .Return(Task.Run(regnskabslisteGetter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var regnskabslisteViewModel = new RegnskabslisteViewModel(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(regnskabslisteViewModel, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Empty);

            var result = await regnskabslisteViewModel.RegnskabGetAsync(fixture.Create<int>());
            Assert.That(result, Is.Null);

            finansstyringRepositoryMock.AssertWasCalled(m => m.RegnskabslisteGetAsync());
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.Anything));
        }

        /// <summary>
        /// Tester, at PropertyChangedOnRegnskabViewModelEventHandler rejser PropertyChanged, når en ViewModel for et regnskab opdateres.
        /// </summary>
        [Test]
        [Ignore]
        [TestCase("DisplayName", "Regnskaber")]
        public void TestAtPropertyChangedOnRegnskabViewModelEventHandlerRejserPropertyChangedOnRegnskabViewModelUpdate(string propertyName, string expectedPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));

            var regnskabslisteViewModel = new RegnskabslisteViewModel(fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabslisteViewModel, Is.Not.Null);

            var regnskabViewModelMock = fixture.Create<IRegnskabViewModel>();
            regnskabslisteViewModel.RegnskabAdd(regnskabViewModelMock);

            var eventCalled = false;
            regnskabslisteViewModel.PropertyChanged += (s, e) =>
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
            regnskabViewModelMock.Raise(m => m.PropertyChanged += null, regnskabViewModelMock, new PropertyChangedEventArgs(propertyName));
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnRegnskabViewModelEventHandler kaster en ArgumentNullException, hvis objektet, der rejser eventet, er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnRegnskabViewModelEventHandlerKasterArgumentNullExceptionHvisSenderErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));

            var regnskabslisteViewModel = new RegnskabslisteViewModel(fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabslisteViewModel, Is.Not.Null);

            var regnskabViewModelMock = fixture.Create<IRegnskabViewModel>();
            regnskabslisteViewModel.RegnskabAdd(regnskabViewModelMock);

            var exception = Assert.Throws<ArgumentNullException>(() => regnskabViewModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
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
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));

            var regnskabslisteViewModel = new RegnskabslisteViewModel(fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabslisteViewModel, Is.Not.Null);

            var regnskabViewModelMock = fixture.Create<IRegnskabViewModel>();
            regnskabslisteViewModel.RegnskabAdd(regnskabViewModelMock);

            var exception = Assert.Throws<ArgumentNullException>(() => regnskabViewModelMock.Raise(m => m.PropertyChanged += null, regnskabViewModelMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("e"));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
