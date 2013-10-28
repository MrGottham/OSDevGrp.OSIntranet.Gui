using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using Ploeh.AutoFixture;
using Rhino.Mocks;
using Text = OSDevGrp.OSIntranet.Gui.Resources.Text;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Finansstyring
{
    /// <summary>
    /// Tester ViewModel indeholdende konfiguration til finansstyring.
    /// </summary>
    [TestFixture]
    public class FinansstyringKonfigurationViewModelTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en ViewModel indeholdende konfiguration til finansstyring.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererFinansstyringKonfigurationViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(fixture.Create<IFinansstyringKonfigurationRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);
            Assert.That(finansstyringKonfigurationViewModel.Configuration, Is.Not.Null);
            Assert.That(finansstyringKonfigurationViewModel.Configuration, Is.Not.Empty);
            Assert.That(finansstyringKonfigurationViewModel.Configuration, Is.EqualTo("OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.FinansstyringKonfigurationViewModel"));
            Assert.That(finansstyringKonfigurationViewModel.Keys, Is.Not.Null);
            Assert.That(finansstyringKonfigurationViewModel.Keys, Is.Not.Empty);
            Assert.That(finansstyringKonfigurationViewModel.DisplayName, Is.Not.Null);
            Assert.That(finansstyringKonfigurationViewModel.DisplayName, Is.Not.Empty);
            Assert.That(finansstyringKonfigurationViewModel.DisplayName, Is.EqualTo(Resource.GetText(Text.Configuration)));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis konfigurationsrepositoryet til finansstyring er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFinansstyringKonfigurationRepositoryErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            Assert.Throws<ArgumentNullException>(() => new FinansstyringKonfigurationViewModel(null, fixture.Create<IExceptionHandlerViewModel>()));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis ViewModel for exceptionhandleren er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisExceptionHandlerViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            Assert.Throws<ArgumentNullException>(() => new FinansstyringKonfigurationViewModel(fixture.Create<IFinansstyringKonfigurationRepository>(), null));
        }

        /// <summary>
        /// Tester, at getteren til FinansstyringServiceUri henter og returnerer værdien fra konfigurationsrepositoryet til finansstyring.
        /// </summary>
        [Test]
        public void TestAtFinansstyringServiceUriGetterReturnererValueFraFinansstyringKonfigurationRepository()
        {
            var fixture = new Fixture();
            fixture.Customize<Uri>(e => e.FromFactory(() => new Uri("http://localhost")));

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.FinansstyringServiceUri)
                                                    .Return(fixture.Create<Uri>())
                                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            var result = finansstyringKonfigurationViewModel.FinansstyringServiceUri;
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(finansstyringKonfigurationRepositoryMock.FinansstyringServiceUri.ToString()));

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.FinansstyringServiceUri);
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at getteren til FinansstyringServiceUri kalder exceptionhandleren ved en IntranetGuiRepositoryException.
        /// </summary>
        [Test]
        public void TestAtFinansstyringServiceUriGetterKalderExceptionHandlerViewModelVedIntranetGuiRepositoryException()
        {
            var fixture = new Fixture();
            fixture.Customize<Uri>(e => e.FromFactory(() => new Uri("http://localhost")));

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.FinansstyringServiceUri)
                                                    .Throw(fixture.Create<IntranetGuiRepositoryException>())
                                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            var result = finansstyringKonfigurationViewModel.FinansstyringServiceUri;
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.FinansstyringServiceUri);
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiRepositoryException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at getteren til FinansstyringServiceUri kalder exceptionhandleren ved en IntranetGuiBusinessException.
        /// </summary>
        [Test]
        public void TestAtFinansstyringServiceUriGetterKalderExceptionHandlerViewModelVedIntranetGuiBusinessException()
        {
            var fixture = new Fixture();
            fixture.Customize<Uri>(e => e.FromFactory(() => new Uri("http://localhost")));

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.FinansstyringServiceUri)
                                                    .Throw(fixture.Create<IntranetGuiBusinessException>())
                                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            var result = finansstyringKonfigurationViewModel.FinansstyringServiceUri;
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.FinansstyringServiceUri);
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiBusinessException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at getteren til FinansstyringServiceUri kalder exceptionhandleren ved en IntranetGuiSystemException.
        /// </summary>
        [Test]
        public void TestAtFinansstyringServiceUriGetterKalderExceptionHandlerViewModelVedIntranetGuiSystemException()
        {
            var fixture = new Fixture();
            fixture.Customize<Uri>(e => e.FromFactory(() => new Uri("http://localhost")));

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.FinansstyringServiceUri)
                                                    .Throw(fixture.Create<IntranetGuiSystemException>())
                                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            var result = finansstyringKonfigurationViewModel.FinansstyringServiceUri;
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.FinansstyringServiceUri);
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at getteren til FinansstyringServiceUri kalder exceptionhandleren ved en Exception.
        /// </summary>
        [Test]
        public void TestAtFinansstyringServiceUriGetterKalderExceptionHandlerViewModelVedException()
        {
            var fixture = new Fixture();
            fixture.Customize<Uri>(e => e.FromFactory(() => new Uri("http://localhost")));

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.FinansstyringServiceUri)
                                                    .Throw(fixture.Create<Exception>())
                                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<Exception>.Is.NotNull))
                                         .WhenCalled(e =>
                                             {
                                                 var exception = (Exception) e.Arguments.ElementAt(0);
                                                 Assert.That(exception, Is.Not.Null);
                                                 Assert.That(exception.Message, Is.Not.Null);
                                                 Assert.That(exception.Message, Is.Not.Empty);
                                                 Assert.That(exception.Message, Is.StringStarting(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileGettingPropertyValue, "FinansstyringServiceUri", null)));
                                                 Assert.That(exception.InnerException, Is.Not.Null);
                                             })
                                         .Repeat.Any();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            var result = finansstyringKonfigurationViewModel.FinansstyringServiceUri;
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.FinansstyringServiceUri);
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at setteren til FinansstyringServiceUri opdaterer værdien i konfigurationsrepositoryet til finansstyring.
        /// </summary>
        [Test]
        public void TestAtFinansstyringServiceUriSetterOpdatererValueOnFinansstyringKonfigurationRepository()
        {
            var fixture = new Fixture();
            fixture.Customize<Uri>(e => e.FromFactory(() => new Uri("http://localhost")));

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.FinansstyringServiceUri)
                                                    .Return(fixture.Create<Uri>())
                                                    .Repeat.Any();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull))
                                                    .WhenCalled(e =>
                                                        {
                                                            var updateConfigurations = (IDictionary<string, object>) e.Arguments.ElementAt(0);
                                                            Assert.That(updateConfigurations, Is.Not.Null);
                                                            Assert.That(updateConfigurations, Is.Not.Empty);
                                                            Assert.That(updateConfigurations.Count, Is.EqualTo(1));

                                                            var updateConfiguration = updateConfigurations.ElementAt(0);
                                                            Assert.That(updateConfiguration, Is.Not.Null);
                                                            Assert.That(updateConfiguration.Key, Is.Not.Null);
                                                            Assert.That(updateConfiguration.Key, Is.Not.Empty);
                                                            Assert.That(updateConfiguration.Key, Is.EqualTo("FinansstyringServiceUri"));
                                                            Assert.That(updateConfiguration.Value, Is.Not.Null);
                                                        })
                                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            finansstyringKonfigurationViewModel.FinansstyringServiceUri = "http://localhost/OSIntranet/FinansstyringService.svc";

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.FinansstyringServiceUri);
            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at setteren til FinansstyringServiceUri rejser PropertyChanged ved opdatering af værdi.
        /// </summary>
        [Test]
        [TestCase("FinansstyringServiceUri")]
        public void TestAtFinansstyringServiceUriSetterRejserPropertyChangedVedOpdateringAfValue(string exceptPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<Uri>(e => e.FromFactory(() => new Uri("http://localhost")));

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.FinansstyringServiceUri)
                                                    .Return(fixture.Create<Uri>())
                                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            var eventCalled = false;
            finansstyringKonfigurationViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, exceptPropertyName, StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            finansstyringKonfigurationViewModel.FinansstyringServiceUri = "http://localhost/OSIntranet/FinansstyringService.svc";
            Assert.That(eventCalled, Is.True);

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.FinansstyringServiceUri);
            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at setteren til FinansstyringServiceUri kalder exceptionhandleren ved en IntranetGuiRepositoryException.
        /// </summary>
        [Test]
        public void TestAtFinansstyringServiceUriSetterKalderExceptionHandlerViewModelVedIntranetGuiRepositoryException()
        {
            var fixture = new Fixture();
            fixture.Customize<Uri>(e => e.FromFactory(() => new Uri("http://localhost")));

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.FinansstyringServiceUri)
                                                    .Return(fixture.Create<Uri>())
                                                    .Repeat.Any();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull))
                                                    .Throw(fixture.Create<IntranetGuiRepositoryException>())
                                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            finansstyringKonfigurationViewModel.FinansstyringServiceUri = "http://localhost/OSIntranet/FinansstyringService.svc";

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.FinansstyringServiceUri);
            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiRepositoryException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at setteren til FinansstyringServiceUri kalder exceptionhandleren ved en IntranetGuiBusinessException.
        /// </summary>
        [Test]
        public void TestAtFinansstyringServiceUriSetterKalderExceptionHandlerViewModelVedIntranetGuiBusinessException()
        {
            var fixture = new Fixture();
            fixture.Customize<Uri>(e => e.FromFactory(() => new Uri("http://localhost")));

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.FinansstyringServiceUri)
                                                    .Return(fixture.Create<Uri>())
                                                    .Repeat.Any();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull))
                                                    .Throw(fixture.Create<IntranetGuiBusinessException>())
                                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            finansstyringKonfigurationViewModel.FinansstyringServiceUri = "http://localhost/OSIntranet/FinansstyringService.svc";

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.FinansstyringServiceUri);
            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiBusinessException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at setteren til FinansstyringServiceUri kalder exceptionhandleren ved en IntranetGuiSystemException.
        /// </summary>
        [Test]
        public void TestAtFinansstyringServiceUriSetterKalderExceptionHandlerViewModelVedIntranetGuiSystemException()
        {
            var fixture = new Fixture();
            fixture.Customize<Uri>(e => e.FromFactory(() => new Uri("http://localhost")));

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.FinansstyringServiceUri)
                                                    .Return(fixture.Create<Uri>())
                                                    .Repeat.Any();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull))
                                                    .Throw(fixture.Create<IntranetGuiSystemException>())
                                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            finansstyringKonfigurationViewModel.FinansstyringServiceUri = "http://localhost/OSIntranet/FinansstyringService.svc";

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.FinansstyringServiceUri);
            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at setteren til FinansstyringServiceUri kalder exceptionhandleren ved en Exception.
        /// </summary>
        [Test]
        public void TestAtFinansstyringServiceUriSetterKalderExceptionHandlerViewModelVedException()
        {
            var fixture = new Fixture();
            fixture.Customize<Uri>(e => e.FromFactory(() => new Uri("http://localhost")));

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.FinansstyringServiceUri)
                                                    .Return(fixture.Create<Uri>())
                                                    .Repeat.Any();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull))
                                                    .Throw(fixture.Create<Exception>())
                                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<Exception>.Is.NotNull))
                                         .WhenCalled(e =>
                                             {
                                                 var exception = (Exception) e.Arguments.ElementAt(0);
                                                 Assert.That(exception, Is.Not.Null);
                                                 Assert.That(exception.Message, Is.Not.Null);
                                                 Assert.That(exception.Message, Is.Not.Empty);
                                                 Assert.That(exception.Message, Is.StringStarting(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingPropertyValue, "FinansstyringServiceUri", null)));
                                                 Assert.That(exception.InnerException, Is.Not.Null);
                                             })
                                         .Repeat.Any();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            finansstyringKonfigurationViewModel.FinansstyringServiceUri = "http://localhost/OSIntranet/FinansstyringService.svc";

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.FinansstyringServiceUri);
            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at getteren til AntalBogføringslinjer henter og returnerer værdien fra konfigurationsrepositoryet til finansstyring.
        /// </summary>
        [Test]
        public void TestAtAntalBogføringslinjerGetterReturnererValueFraFinansstyringKonfigurationRepository()
        {
            var fixture = new Fixture();

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.AntalBogføringslinjer)
                                                    .Return(fixture.Create<int>())
                                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            var result = finansstyringKonfigurationViewModel.AntalBogføringslinjer;
            Assert.That(result, Is.EqualTo(finansstyringKonfigurationRepositoryMock.AntalBogføringslinjer));

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.AntalBogføringslinjer);
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at getteren til AntalBogføringslinjer kalder exceptionhandleren ved en IntranetGuiRepositoryException.
        /// </summary>
        [Test]
        public void TestAtAntalBogføringslinjerGetterKalderExceptionHandlerViewModelVedIntranetGuiRepositoryException()
        {
            var fixture = new Fixture();
            fixture.Customize<Uri>(e => e.FromFactory(() => new Uri("http://localhost")));

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.AntalBogføringslinjer)
                                                    .Throw(fixture.Create<IntranetGuiRepositoryException>())
                                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            var result = finansstyringKonfigurationViewModel.AntalBogføringslinjer;
            Assert.That(result, Is.EqualTo(0));

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.AntalBogføringslinjer);
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiRepositoryException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at getteren til AntalBogføringslinjer kalder exceptionhandleren ved en IntranetGuiBusinessException.
        /// </summary>
        [Test]
        public void TestAtAntalBogføringslinjerGetterKalderExceptionHandlerViewModelVedIntranetGuiBusinessException()
        {
            var fixture = new Fixture();
            fixture.Customize<Uri>(e => e.FromFactory(() => new Uri("http://localhost")));

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.AntalBogføringslinjer)
                                                    .Throw(fixture.Create<IntranetGuiBusinessException>())
                                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            var result = finansstyringKonfigurationViewModel.AntalBogføringslinjer;
            Assert.That(result, Is.EqualTo(0));

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.AntalBogføringslinjer);
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiBusinessException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at getteren til AntalBogføringslinjer kalder exceptionhandleren ved en IntranetGuiSystemException.
        /// </summary>
        [Test]
        public void TestAtAntalBogføringslinjerGetterKalderExceptionHandlerViewModelVedIntranetGuiSystemException()
        {
            var fixture = new Fixture();
            fixture.Customize<Uri>(e => e.FromFactory(() => new Uri("http://localhost")));

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.AntalBogføringslinjer)
                                                    .Throw(fixture.Create<IntranetGuiSystemException>())
                                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            var result = finansstyringKonfigurationViewModel.AntalBogføringslinjer;
            Assert.That(result, Is.EqualTo(0));

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.AntalBogføringslinjer);
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at getteren til AntalBogføringslinjer kalder exceptionhandleren ved en Exception.
        /// </summary>
        [Test]
        public void TestAtAntalBogføringslinjerGetterKalderExceptionHandlerViewModelVedException()
        {
            var fixture = new Fixture();
            fixture.Customize<Uri>(e => e.FromFactory(() => new Uri("http://localhost")));

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.AntalBogføringslinjer)
                                                    .Throw(fixture.Create<Exception>())
                                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<Exception>.Is.NotNull))
                                         .WhenCalled(e =>
                                             {
                                                 var exception = (Exception) e.Arguments.ElementAt(0);
                                                 Assert.That(exception, Is.Not.Null);
                                                 Assert.That(exception.Message, Is.Not.Null);
                                                 Assert.That(exception.Message, Is.Not.Empty);
                                                 Assert.That(exception.Message, Is.StringStarting(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileGettingPropertyValue, "AntalBogføringslinjer", null)));
                                                 Assert.That(exception.InnerException, Is.Not.Null);
                                             })
                                         .Repeat.Any();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            var result = finansstyringKonfigurationViewModel.AntalBogføringslinjer;
            Assert.That(result, Is.EqualTo(0));

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.AntalBogføringslinjer);
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at setteren til AntalBogføringslinjer opdaterer værdien i konfigurationsrepositoryet til finansstyring.
        /// </summary>
        [Test]
        public void TestAtAntalBogføringslinjerSetterOpdatererValueOnFinansstyringKonfigurationRepository()
        {
            var fixture = new Fixture();

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.AntalBogføringslinjer)
                                                    .Return(fixture.Create<int>())
                                                    .Repeat.Any();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull))
                                                    .WhenCalled(e =>
                                                        {
                                                            var updateConfigurations = (IDictionary<string, object>) e.Arguments.ElementAt(0);
                                                            Assert.That(updateConfigurations, Is.Not.Null);
                                                            Assert.That(updateConfigurations, Is.Not.Empty);
                                                            Assert.That(updateConfigurations.Count, Is.EqualTo(1));

                                                            var updateConfiguration = updateConfigurations.ElementAt(0);
                                                            Assert.That(updateConfiguration, Is.Not.Null);
                                                            Assert.That(updateConfiguration.Key, Is.Not.Null);
                                                            Assert.That(updateConfiguration.Key, Is.Not.Empty);
                                                            Assert.That(updateConfiguration.Key, Is.EqualTo("AntalBogføringslinjer"));
                                                            Assert.That(updateConfiguration.Value, Is.Not.Null);
                                                        })
                                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            finansstyringKonfigurationViewModel.AntalBogføringslinjer = fixture.Create<int>();

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.AntalBogføringslinjer);
            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at setteren til AntalBogføringslinjer rejser PropertyChanged ved opdatering af værdi.
        /// </summary>
        [Test]
        [TestCase("AntalBogføringslinjer")]
        public void TestAtAntalBogføringslinjerSetterRejserPropertyChangedVedOpdateringAfValue(string exceptPropertyName)
        {
            var fixture = new Fixture();

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.AntalBogføringslinjer)
                                                    .Return(fixture.Create<int>())
                                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            var eventCalled = false;
            finansstyringKonfigurationViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, exceptPropertyName, StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            finansstyringKonfigurationViewModel.AntalBogføringslinjer = fixture.Create<int>();
            Assert.That(eventCalled, Is.True);

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.AntalBogføringslinjer);
            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at setteren til AntalBogføringslinjer kalder exceptionhandleren ved en IntranetGuiRepositoryException.
        /// </summary>
        [Test]
        public void TestAtAntalBogføringslinjerSetterKalderExceptionHandlerViewModelVedIntranetGuiRepositoryException()
        {
            var fixture = new Fixture();

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.AntalBogføringslinjer)
                                                    .Return(fixture.Create<int>())
                                                    .Repeat.Any();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull))
                                                    .Throw(fixture.Create<IntranetGuiRepositoryException>())
                                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            finansstyringKonfigurationViewModel.AntalBogføringslinjer = fixture.Create<int>();

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.AntalBogføringslinjer);
            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiRepositoryException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at setteren til AntalBogføringslinjer kalder exceptionhandleren ved en IntranetGuiBusinessException.
        /// </summary>
        [Test]
        public void TestAtAntalBogføringslinjerSetterKalderExceptionHandlerViewModelVedIntranetGuiBusinessException()
        {
            var fixture = new Fixture();

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.AntalBogføringslinjer)
                                                    .Return(fixture.Create<int>())
                                                    .Repeat.Any();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull))
                                                    .Throw(fixture.Create<IntranetGuiBusinessException>())
                                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            finansstyringKonfigurationViewModel.AntalBogføringslinjer = fixture.Create<int>();

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.AntalBogføringslinjer);
            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiBusinessException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at setteren til AntalBogføringslinjer kalder exceptionhandleren ved en IntranetGuiSystemException.
        /// </summary>
        [Test]
        public void TestAtAntalBogføringslinjerSetterKalderExceptionHandlerViewModelVedIntranetGuiSystemException()
        {
            var fixture = new Fixture();

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.AntalBogføringslinjer)
                                                    .Return(fixture.Create<int>())
                                                    .Repeat.Any();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull))
                                                    .Throw(fixture.Create<IntranetGuiSystemException>())
                                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            finansstyringKonfigurationViewModel.AntalBogføringslinjer = fixture.Create<int>();

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.AntalBogføringslinjer);
            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at setteren til AntalBogføringslinjer kalder exceptionhandleren ved en Exception.
        /// </summary>
        [Test]
        public void TestAtAntalBogføringslinjerSetterKalderExceptionHandlerViewModelVedException()
        {
            var fixture = new Fixture();

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.AntalBogføringslinjer)
                                                    .Return(fixture.Create<int>())
                                                    .Repeat.Any();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull))
                                                    .Throw(fixture.Create<Exception>())
                                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<Exception>.Is.NotNull))
                                         .WhenCalled(e =>
                                             {
                                                 var exception = (Exception) e.Arguments.ElementAt(0);
                                                 Assert.That(exception, Is.Not.Null);
                                                 Assert.That(exception.Message, Is.Not.Null);
                                                 Assert.That(exception.Message, Is.Not.Empty);
                                                 Assert.That(exception.Message, Is.StringStarting(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingPropertyValue, "AntalBogføringslinjer", null)));
                                                 Assert.That(exception.InnerException, Is.Not.Null);
                                             })
                                         .Repeat.Any();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            finansstyringKonfigurationViewModel.AntalBogføringslinjer = fixture.Create<int>();

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.AntalBogføringslinjer);
            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at getteren til DageForNyheder henter og returnerer værdien fra konfigurationsrepositoryet til finansstyring.
        /// </summary>
        [Test]
        public void TestAtDageForNyhederGetterReturnererValueFraFinansstyringKonfigurationRepository()
        {
            var fixture = new Fixture();

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.DageForNyheder)
                                                    .Return(fixture.Create<int>())
                                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            var result = finansstyringKonfigurationViewModel.DageForNyheder;
            Assert.That(result, Is.EqualTo(finansstyringKonfigurationRepositoryMock.DageForNyheder));

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.DageForNyheder);
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at getteren til DageForNyheder kalder exceptionhandleren ved en IntranetGuiRepositoryException.
        /// </summary>
        [Test]
        public void TestAtDageForNyhederGetterKalderExceptionHandlerViewModelVedIntranetGuiRepositoryException()
        {
            var fixture = new Fixture();
            fixture.Customize<Uri>(e => e.FromFactory(() => new Uri("http://localhost")));

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.DageForNyheder)
                                                    .Throw(fixture.Create<IntranetGuiRepositoryException>())
                                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            var result = finansstyringKonfigurationViewModel.DageForNyheder;
            Assert.That(result, Is.EqualTo(0));

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.DageForNyheder);
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiRepositoryException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at getteren til DageForNyheder kalder exceptionhandleren ved en IntranetGuiBusinessException.
        /// </summary>
        [Test]
        public void TestAtDageForNyhederGetterKalderExceptionHandlerViewModelVedIntranetGuiBusinessException()
        {
            var fixture = new Fixture();
            fixture.Customize<Uri>(e => e.FromFactory(() => new Uri("http://localhost")));

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.DageForNyheder)
                                                    .Throw(fixture.Create<IntranetGuiBusinessException>())
                                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            var result = finansstyringKonfigurationViewModel.DageForNyheder;
            Assert.That(result, Is.EqualTo(0));

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.DageForNyheder);
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiBusinessException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at getteren til DageForNyheder kalder exceptionhandleren ved en IntranetGuiSystemException.
        /// </summary>
        [Test]
        public void TestAtDageForNyhederGetterKalderExceptionHandlerViewModelVedIntranetGuiSystemException()
        {
            var fixture = new Fixture();
            fixture.Customize<Uri>(e => e.FromFactory(() => new Uri("http://localhost")));

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.DageForNyheder)
                                                    .Throw(fixture.Create<IntranetGuiSystemException>())
                                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            var result = finansstyringKonfigurationViewModel.DageForNyheder;
            Assert.That(result, Is.EqualTo(0));

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.DageForNyheder);
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at getteren til DageForNyheder kalder exceptionhandleren ved en Exception.
        /// </summary>
        [Test]
        public void TestAtDageForNyhederGetterKalderExceptionHandlerViewModelVedException()
        {
            var fixture = new Fixture();
            fixture.Customize<Uri>(e => e.FromFactory(() => new Uri("http://localhost")));

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.DageForNyheder)
                                                    .Throw(fixture.Create<Exception>())
                                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<Exception>.Is.NotNull))
                                         .WhenCalled(e =>
                                             {
                                                 var exception = (Exception) e.Arguments.ElementAt(0);
                                                 Assert.That(exception, Is.Not.Null);
                                                 Assert.That(exception.Message, Is.Not.Null);
                                                 Assert.That(exception.Message, Is.Not.Empty);
                                                 Assert.That(exception.Message, Is.StringStarting(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileGettingPropertyValue, "DageForNyheder", null)));
                                                 Assert.That(exception.InnerException, Is.Not.Null);
                                             })
                                         .Repeat.Any();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            var result = finansstyringKonfigurationViewModel.DageForNyheder;
            Assert.That(result, Is.EqualTo(0));

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.DageForNyheder);
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at setteren til DageForNyheder opdaterer værdien i konfigurationsrepositoryet til finansstyring.
        /// </summary>
        [Test]
        public void TestAtDageForNyhederSetterOpdatererValueOnFinansstyringKonfigurationRepository()
        {
            var fixture = new Fixture();

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.DageForNyheder)
                                                    .Return(fixture.Create<int>())
                                                    .Repeat.Any();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull))
                                                    .WhenCalled(e =>
                                                        {
                                                            var updateConfigurations = (IDictionary<string, object>) e.Arguments.ElementAt(0);
                                                            Assert.That(updateConfigurations, Is.Not.Null);
                                                            Assert.That(updateConfigurations, Is.Not.Empty);
                                                            Assert.That(updateConfigurations.Count, Is.EqualTo(1));

                                                            var updateConfiguration = updateConfigurations.ElementAt(0);
                                                            Assert.That(updateConfiguration, Is.Not.Null);
                                                            Assert.That(updateConfiguration.Key, Is.Not.Null);
                                                            Assert.That(updateConfiguration.Key, Is.Not.Empty);
                                                            Assert.That(updateConfiguration.Key, Is.EqualTo("DageForNyheder"));
                                                            Assert.That(updateConfiguration.Value, Is.Not.Null);
                                                        })
                                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            finansstyringKonfigurationViewModel.DageForNyheder = fixture.Create<int>();

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.DageForNyheder);
            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at setteren til DageForNyheder rejser PropertyChanged ved opdatering af værdi.
        /// </summary>
        [Test]
        [TestCase("DageForNyheder")]
        public void TestAtDageForNyhederSetterRejserPropertyChangedVedOpdateringAfValue(string exceptPropertyName)
        {
            var fixture = new Fixture();

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.DageForNyheder)
                                                    .Return(fixture.Create<int>())
                                                    .Repeat.Any();
            
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            var eventCalled = false;
            finansstyringKonfigurationViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, exceptPropertyName, StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            finansstyringKonfigurationViewModel.DageForNyheder = fixture.Create<int>();
            Assert.That(eventCalled, Is.True);

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.DageForNyheder);
            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at setteren til DageForNyheder kalder exceptionhandleren ved en IntranetGuiRepositoryException.
        /// </summary>
        [Test]
        public void TestAtDageForNyhederSetterKalderExceptionHandlerViewModelVedIntranetGuiRepositoryException()
        {
            var fixture = new Fixture();

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.DageForNyheder)
                                                    .Return(fixture.Create<int>())
                                                    .Repeat.Any();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull))
                                                    .Throw(fixture.Create<IntranetGuiRepositoryException>())
                                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            finansstyringKonfigurationViewModel.DageForNyheder = fixture.Create<int>();

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.DageForNyheder);
            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiRepositoryException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at setteren til DageForNyheder kalder exceptionhandleren ved en IntranetGuiBusinessException.
        /// </summary>
        [Test]
        public void TestAtDageForNyhederSetterKalderExceptionHandlerViewModelVedIntranetGuiBusinessException()
        {
            var fixture = new Fixture();

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.DageForNyheder)
                                                    .Return(fixture.Create<int>())
                                                    .Repeat.Any();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull))
                                                    .Throw(fixture.Create<IntranetGuiBusinessException>())
                                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            finansstyringKonfigurationViewModel.DageForNyheder = fixture.Create<int>();

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.DageForNyheder);
            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiBusinessException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at setteren til DageForNyheder kalder exceptionhandleren ved en IntranetGuiSystemException.
        /// </summary>
        [Test]
        public void TestAtDageForNyhederSetterKalderExceptionHandlerViewModelVedIntranetGuiSystemException()
        {
            var fixture = new Fixture();

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.DageForNyheder)
                                                    .Return(fixture.Create<int>())
                                                    .Repeat.Any();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull))
                                                    .Throw(fixture.Create<IntranetGuiSystemException>())
                                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            finansstyringKonfigurationViewModel.DageForNyheder = fixture.Create<int>();

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.DageForNyheder);
            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at setteren til DageForNyheder kalder exceptionhandleren ved en Exception.
        /// </summary>
        [Test]
        public void TestAtDageForNyhederSetterKalderExceptionHandlerViewModelVedException()
        {
            var fixture = new Fixture();

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.DageForNyheder)
                                                    .Return(fixture.Create<int>())
                                                    .Repeat.Any();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull))
                                                    .Throw(fixture.Create<Exception>())
                                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<Exception>.Is.NotNull))
                                         .WhenCalled(e =>
                                             {
                                                 var exception = (Exception) e.Arguments.ElementAt(0);
                                                 Assert.That(exception, Is.Not.Null);
                                                 Assert.That(exception.Message, Is.Not.Null);
                                                 Assert.That(exception.Message, Is.Not.Empty);
                                                 Assert.That(exception.Message, Is.StringStarting(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingPropertyValue, "DageForNyheder", null)));
                                                 Assert.That(exception.InnerException, Is.Not.Null);
                                             })
                                         .Repeat.Any();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            finansstyringKonfigurationViewModel.DageForNyheder = fixture.Create<int>();

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.DageForNyheder);
            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf));
        }
    }
}
