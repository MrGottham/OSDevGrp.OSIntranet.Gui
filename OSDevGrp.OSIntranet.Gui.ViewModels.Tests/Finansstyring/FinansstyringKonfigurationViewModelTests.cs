using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            Assert.That(finansstyringKonfigurationViewModel.FinansstyringServiceUriLabel, Is.Not.Null);
            Assert.That(finansstyringKonfigurationViewModel.FinansstyringServiceUriLabel, Is.Not.Empty);
            Assert.That(finansstyringKonfigurationViewModel.FinansstyringServiceUriLabel, Is.EqualTo(Resource.GetText(Text.SupportingServiceUri)));
            Assert.That(finansstyringKonfigurationViewModel.FinansstyringServiceUriValidationError, Is.Not.Null);
            Assert.That(finansstyringKonfigurationViewModel.FinansstyringServiceUriValidationError, Is.Empty);
            Assert.That(finansstyringKonfigurationViewModel.LokalDataFilLabel, Is.Not.Null);
            Assert.That(finansstyringKonfigurationViewModel.LokalDataFilLabel, Is.Not.Empty);
            Assert.That(finansstyringKonfigurationViewModel.LokalDataFilLabel, Is.EqualTo(Resource.GetText(Text.LocaleDataFile)));
            Assert.That(finansstyringKonfigurationViewModel.SynkroniseringDataFilLabel, Is.Not.Null);
            Assert.That(finansstyringKonfigurationViewModel.SynkroniseringDataFilLabel, Is.Not.Empty);
            Assert.That(finansstyringKonfigurationViewModel.SynkroniseringDataFilLabel, Is.EqualTo(Resource.GetText(Text.SyncDataFile)));
            Assert.That(finansstyringKonfigurationViewModel.AntalBogføringslinjerLabel, Is.Not.Null);
            Assert.That(finansstyringKonfigurationViewModel.AntalBogføringslinjerLabel, Is.Not.Empty);
            Assert.That(finansstyringKonfigurationViewModel.AntalBogføringslinjerLabel, Is.EqualTo(Resource.GetText(Text.NumberOfAccountingLinesToGet)));
            Assert.That(finansstyringKonfigurationViewModel.AntalBogføringslinjerValidationError, Is.Not.Null);
            Assert.That(finansstyringKonfigurationViewModel.AntalBogføringslinjerValidationError, Is.Empty);
            Assert.That(finansstyringKonfigurationViewModel.DageForNyhederLabel, Is.Not.Null);
            Assert.That(finansstyringKonfigurationViewModel.DageForNyhederLabel, Is.Not.Empty);
            Assert.That(finansstyringKonfigurationViewModel.DageForNyhederLabel, Is.EqualTo(Resource.GetText(Text.DaysForNews)));
            Assert.That(finansstyringKonfigurationViewModel.DageForNyhederValidationError, Is.Not.Null);
            Assert.That(finansstyringKonfigurationViewModel.DageForNyhederValidationError, Is.Empty);
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
        /// Tester, at setteren til FinansstyringServiceUri opdaterer værdien i konfigurationsrepositoryet til finansstyring
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
        [TestCase("FinansstyringServiceUriValidationError")]
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
        /// Tester, at setteren til FinansstyringServiceUri opdaterer FinansstyringServiceUriValidationError ved valideringsfejl.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("XYZ")]
        public void TestAtFinansstyringServiceUriOpdatererFinansstyringServiceUriValidationErrorVedIntranetGuiValidationException(string illegalValue)
        {
            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);
            Assert.That(finansstyringKonfigurationViewModel.FinansstyringServiceUriValidationError, Is.Not.Null);
            Assert.That(finansstyringKonfigurationViewModel.FinansstyringServiceUriValidationError, Is.Empty);

            var eventCalled = false;
            finansstyringKonfigurationViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, "FinansstyringServiceUriValidationError", StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            finansstyringKonfigurationViewModel.FinansstyringServiceUri = illegalValue;
            Assert.That(eventCalled, Is.True);

            Assert.That(finansstyringKonfigurationViewModel.FinansstyringServiceUriValidationError, Is.Not.Null);
            Assert.That(finansstyringKonfigurationViewModel.FinansstyringServiceUriValidationError, Is.Not.Empty);
            Assert.That(finansstyringKonfigurationViewModel.FinansstyringServiceUriValidationError, Is.EqualTo(Resource.GetText(Text.InvalidValueForUri, illegalValue)));

            finansstyringKonfigurationRepositoryMock.AssertWasNotCalled(m => m.FinansstyringServiceUri);
            finansstyringKonfigurationRepositoryMock.AssertWasNotCalled(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.Anything));
        }

        /// <summary>
        /// Tester, at setteren til FinansstyringServiceUri kalder exceptionhandleren ved en IntranetGuiValidationException ved valideringsfejl.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("XYZ")]
        public void TestAtFinansstyringServiceUriSetterKalderExceptionHandlerViewModelVedIntranetGuiValidationException(string illegalValue)
        {
            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var exception = (IntranetGuiValidationException) e.Arguments.ElementAt(0);
                                                 Assert.That(exception, Is.Not.Null);
                                                 Assert.That(exception.Message, Is.Not.Null);
                                                 Assert.That(exception.Message, Is.Not.Empty);
                                                 Assert.That(exception.Message, Is.EqualTo(Resource.GetText(Text.InvalidValueForUri, illegalValue)));
                                                 Assert.That(exception.ValidationContext, Is.Not.Null);
                                                 Assert.That(exception.ValidationContext, Is.TypeOf<FinansstyringKonfigurationViewModel>());
                                                 Assert.That(exception.PropertyName, Is.Not.Null);
                                                 Assert.That(exception.PropertyName, Is.Not.Empty);
                                                 Assert.That(exception.PropertyName, Is.EqualTo("FinansstyringServiceUri"));
                                                 Assert.That(exception.Value, Is.EqualTo(illegalValue));
                                                 Assert.That(exception.InnerException, Is.Null);
                                             });

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            finansstyringKonfigurationViewModel.FinansstyringServiceUri = illegalValue;

            finansstyringKonfigurationRepositoryMock.AssertWasNotCalled(m => m.FinansstyringServiceUri);
            finansstyringKonfigurationRepositoryMock.AssertWasNotCalled(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.Anything));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf));
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
        /// Tester, at getteren til LokalDataFil henter og returnerer værdien fra konfigurationsrepositoryet til finansstyring.
        /// </summary>
        [Test]
        public void TestAtLokalDataFilGetterReturnererValueFraFinansstyringKonfigurationRepository()
        {
            var fixture = new Fixture();

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.LokalDataFil)
                                                    .Return(fixture.Create<string>())
                                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            var result = finansstyringKonfigurationViewModel.LokalDataFil;
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(finansstyringKonfigurationRepositoryMock.LokalDataFil));

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.LokalDataFil);
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at getteren til LokalDataFil kalder exceptionhandleren ved en IntranetGuiRepositoryException.
        /// </summary>
        [Test]
        public void TestAtLokalDataFilGetterKalderExceptionHandlerViewModelVedIntranetGuiRepositoryException()
        {
            var fixture = new Fixture();

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.LokalDataFil)
                .Throw(fixture.Create<IntranetGuiRepositoryException>())
                .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            var result = finansstyringKonfigurationViewModel.LokalDataFil;
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.LokalDataFil);
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiRepositoryException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at getteren til LokalDataFil kalder exceptionhandleren ved en IntranetGuiBusinessException.
        /// </summary>
        [Test]
        public void TestAtLokalDataFilGetterKalderExceptionHandlerViewModelVedIntranetGuiBusinessException()
        {
            var fixture = new Fixture();

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.LokalDataFil)
                .Throw(fixture.Create<IntranetGuiBusinessException>())
                .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            var result = finansstyringKonfigurationViewModel.LokalDataFil;
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.LokalDataFil);
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiBusinessException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at getteren til LokalDataFil kalder exceptionhandleren ved en IntranetGuiSystemException.
        /// </summary>
        [Test]
        public void TestAtLokalDataFilGetterKalderExceptionHandlerViewModelVedIntranetGuiSystemException()
        {
            var fixture = new Fixture();

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.LokalDataFil)
                .Throw(fixture.Create<IntranetGuiSystemException>())
                .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            var result = finansstyringKonfigurationViewModel.LokalDataFil;
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.LokalDataFil);
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at getteren til LokalDataFil kalder exceptionhandleren ved en Exception.
        /// </summary>
        [Test]
        public void TestAtLokalDataFilGetterKalderExceptionHandlerViewModelVedException()
        {
            var fixture = new Fixture();

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.LokalDataFil)
                .Throw(fixture.Create<Exception>())
                .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            var result = finansstyringKonfigurationViewModel.LokalDataFil;
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.LokalDataFil);
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at getteren til SynkroniseringDataFil henter og returnerer værdien fra konfigurationsrepositoryet til finansstyring.
        /// </summary>
        [Test]
        public void TestAtSynkroniseringDataFilGetterReturnererValueFraFinansstyringKonfigurationRepository()
        {
            var fixture = new Fixture();

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.SynkroniseringDataFil)
                                                    .Return(fixture.Create<string>())
                                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            var result = finansstyringKonfigurationViewModel.SynkroniseringDataFil;
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(finansstyringKonfigurationRepositoryMock.SynkroniseringDataFil));

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.SynkroniseringDataFil);
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at getteren til SynkroniseringDataFil kalder exceptionhandleren ved en IntranetGuiRepositoryException.
        /// </summary>
        [Test]
        public void TestAtSynkroniseringDataFilGetterKalderExceptionHandlerViewModelVedIntranetGuiRepositoryException()
        {
            var fixture = new Fixture();

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.SynkroniseringDataFil)
                .Throw(fixture.Create<IntranetGuiRepositoryException>())
                .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            var result = finansstyringKonfigurationViewModel.SynkroniseringDataFil;
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.SynkroniseringDataFil);
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiRepositoryException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at getteren til SynkroniseringDataFil kalder exceptionhandleren ved en IntranetGuiBusinessException.
        /// </summary>
        [Test]
        public void TestAtSynkroniseringDataFilGetterKalderExceptionHandlerViewModelVedIntranetGuiBusinessException()
        {
            var fixture = new Fixture();

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.SynkroniseringDataFil)
                .Throw(fixture.Create<IntranetGuiBusinessException>())
                .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            var result = finansstyringKonfigurationViewModel.SynkroniseringDataFil;
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.SynkroniseringDataFil);
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiBusinessException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at getteren til SynkroniseringDataFil kalder exceptionhandleren ved en IntranetGuiSystemException.
        /// </summary>
        [Test]
        public void TestAtSynkroniseringDataFilGetterKalderExceptionHandlerViewModelVedIntranetGuiSystemException()
        {
            var fixture = new Fixture();

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.SynkroniseringDataFil)
                .Throw(fixture.Create<IntranetGuiSystemException>())
                .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            var result = finansstyringKonfigurationViewModel.SynkroniseringDataFil;
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.SynkroniseringDataFil);
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at getteren til SynkroniseringDataFil kalder exceptionhandleren ved en Exception.
        /// </summary>
        [Test]
        public void TestAtSynkroniseringDataFilGetterKalderExceptionHandlerViewModelVedException()
        {
            var fixture = new Fixture();

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.SynkroniseringDataFil)
                .Throw(fixture.Create<Exception>())
                .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            var result = finansstyringKonfigurationViewModel.SynkroniseringDataFil;
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.SynkroniseringDataFil);
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
            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.AntalBogføringslinjer)
                                                    .Return(30)
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

            finansstyringKonfigurationViewModel.AntalBogføringslinjer = 50;

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.AntalBogføringslinjer);
            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at setteren til AntalBogføringslinjer rejser PropertyChanged ved opdatering af værdi.
        /// </summary>
        [Test]
        [TestCase("AntalBogføringslinjer")]
        [TestCase("AntalBogføringslinjerValidationError")]
        public void TestAtAntalBogføringslinjerSetterRejserPropertyChangedVedOpdateringAfValue(string exceptPropertyName)
        {
            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.AntalBogføringslinjer)
                                                    .Return(30)
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
            finansstyringKonfigurationViewModel.AntalBogføringslinjer = 50;
            Assert.That(eventCalled, Is.True);

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.AntalBogføringslinjer);
            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at setteren til AntalBogføringslinjer opdaterer AntalBogføringslinjerValidationError ved valideringsfejl.
        /// </summary>
        [Test]
        [TestCase(0)]
        [TestCase(9)]
        [TestCase(251)]
        [TestCase(512)]
        public void TestAtAntalBogføringslinjerSetterOpdatererAntalBogføringslinjerValidationErrorVedIntranetGuiValidationException(int illegalValue)
        {
            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);
            Assert.That(finansstyringKonfigurationViewModel.AntalBogføringslinjerValidationError, Is.Not.Null);
            Assert.That(finansstyringKonfigurationViewModel.AntalBogføringslinjerValidationError, Is.Empty);

            var eventCalled = false;
            finansstyringKonfigurationViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, "AntalBogføringslinjerValidationError", StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            finansstyringKonfigurationViewModel.AntalBogføringslinjer = illegalValue;
            Assert.That(eventCalled, Is.True);

            Assert.That(finansstyringKonfigurationViewModel.AntalBogføringslinjerValidationError, Is.Not.Null);
            Assert.That(finansstyringKonfigurationViewModel.AntalBogføringslinjerValidationError, Is.Not.Empty);
            Assert.That(finansstyringKonfigurationViewModel.AntalBogføringslinjerValidationError, Is.EqualTo(Resource.GetText(Text.ValueOutsideInterval, 10, 250)));

            finansstyringKonfigurationRepositoryMock.AssertWasNotCalled(m => m.AntalBogføringslinjer);
            finansstyringKonfigurationRepositoryMock.AssertWasNotCalled(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.Anything));
        }

        /// <summary>
        /// Tester, at setteren til AntalBogføringslinjer kalder exceptionhandleren ved en IntranetGuiValidationException ved valideringsfejl.
        /// </summary>
        [Test]
        [TestCase(0)]
        [TestCase(9)]
        [TestCase(251)]
        [TestCase(512)]
        public void TestAtAntalBogføringslinjerSetterKalderExceptionHandlerViewModelVedIntranetGuiValidationException(int illegalValue)
        {
            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var exception = (IntranetGuiValidationException) e.Arguments.ElementAt(0);
                                                 Assert.That(exception, Is.Not.Null);
                                                 Assert.That(exception.Message, Is.Not.Null);
                                                 Assert.That(exception.Message, Is.Not.Empty);
                                                 Assert.That(exception.Message, Is.EqualTo(Resource.GetText(Text.ValueOutsideInterval, 10, 250)));
                                                 Assert.That(exception.ValidationContext, Is.Not.Null);
                                                 Assert.That(exception.ValidationContext, Is.TypeOf<FinansstyringKonfigurationViewModel>());
                                                 Assert.That(exception.PropertyName, Is.Not.Null);
                                                 Assert.That(exception.PropertyName, Is.Not.Empty);
                                                 Assert.That(exception.PropertyName, Is.EqualTo("AntalBogføringslinjer"));
                                                 Assert.That(exception.Value, Is.EqualTo(illegalValue));
                                                 Assert.That(exception.InnerException, Is.Null);
                                             });

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            finansstyringKonfigurationViewModel.AntalBogføringslinjer = illegalValue;

            finansstyringKonfigurationRepositoryMock.AssertWasNotCalled(m => m.AntalBogføringslinjer);
            finansstyringKonfigurationRepositoryMock.AssertWasNotCalled(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.Anything));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf));
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
                                                    .Return(30)
                                                    .Repeat.Any();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull))
                                                    .Throw(fixture.Create<IntranetGuiRepositoryException>())
                                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            finansstyringKonfigurationViewModel.AntalBogføringslinjer = 50;

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
                                                    .Return(30)
                                                    .Repeat.Any();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull))
                                                    .Throw(fixture.Create<IntranetGuiBusinessException>())
                                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            finansstyringKonfigurationViewModel.AntalBogføringslinjer = 50;

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
                                                    .Return(30)
                                                    .Repeat.Any();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull))
                                                    .Throw(fixture.Create<IntranetGuiSystemException>())
                                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            finansstyringKonfigurationViewModel.AntalBogføringslinjer = 50;

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
                                                    .Return(30)
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

            finansstyringKonfigurationViewModel.AntalBogføringslinjer = 50;

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
            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.DageForNyheder)
                                                    .Return(7)
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

            finansstyringKonfigurationViewModel.DageForNyheder = 14;

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.DageForNyheder);
            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at setteren til DageForNyheder rejser PropertyChanged ved opdatering af værdi.
        /// </summary>
        [Test]
        [TestCase("DageForNyheder")]
        [TestCase("DageForNyhederValidationError")]
        public void TestAtDageForNyhederSetterRejserPropertyChangedVedOpdateringAfValue(string exceptPropertyName)
        {
            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.DageForNyheder)
                                                    .Return(7)
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
            finansstyringKonfigurationViewModel.DageForNyheder = 14;
            Assert.That(eventCalled, Is.True);

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.DageForNyheder);
            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at setteren til DageForNyheder opdaterer DageForNyhederValidationError ved valideringsfejl.
        /// </summary>
        [Test]
        [TestCase(-10)]
        [TestCase(-1)]
        [TestCase(31)]
        [TestCase(50)]
        public void TestAtDageForNyhederSetterOpdatererDageForNyhederValidationErrorVedIntranetGuiValidationException(int illegalValue)
        {
            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);
            Assert.That(finansstyringKonfigurationViewModel.DageForNyhederValidationError, Is.Not.Null);
            Assert.That(finansstyringKonfigurationViewModel.DageForNyhederValidationError, Is.Empty);

            var eventCalled = false;
            finansstyringKonfigurationViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, "DageForNyhederValidationError", StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            finansstyringKonfigurationViewModel.DageForNyheder = illegalValue;
            Assert.That(eventCalled, Is.True);

            Assert.That(finansstyringKonfigurationViewModel.DageForNyhederValidationError, Is.Not.Null);
            Assert.That(finansstyringKonfigurationViewModel.DageForNyhederValidationError, Is.Not.Empty);
            Assert.That(finansstyringKonfigurationViewModel.DageForNyhederValidationError, Is.EqualTo(Resource.GetText(Text.ValueOutsideInterval, 0, 30)));

            finansstyringKonfigurationRepositoryMock.AssertWasNotCalled(m => m.DageForNyheder);
            finansstyringKonfigurationRepositoryMock.AssertWasNotCalled(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.Anything));
        }

        /// <summary>
        /// Tester, at setteren til DageForNyheder kalder exceptionhandleren ved en IntranetGuiValidationException ved valideringsfejl.
        /// </summary>
        [Test]
        [TestCase(-10)]
        [TestCase(-1)]
        [TestCase(31)]
        [TestCase(50)]
        public void TestAtDageForNyhederSetterKalderExceptionHandlerViewModelVedIntranetGuiValidationException(int illegalValue)
        {
            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var exception = (IntranetGuiValidationException) e.Arguments.ElementAt(0);
                                                 Assert.That(exception, Is.Not.Null);
                                                 Assert.That(exception.Message, Is.Not.Null);
                                                 Assert.That(exception.Message, Is.Not.Empty);
                                                 Assert.That(exception.Message, Is.EqualTo(Resource.GetText(Text.ValueOutsideInterval, 0, 30)));
                                                 Assert.That(exception.ValidationContext, Is.Not.Null);
                                                 Assert.That(exception.ValidationContext, Is.TypeOf<FinansstyringKonfigurationViewModel>());
                                                 Assert.That(exception.PropertyName, Is.Not.Null);
                                                 Assert.That(exception.PropertyName, Is.Not.Empty);
                                                 Assert.That(exception.PropertyName, Is.EqualTo("DageForNyheder"));
                                                 Assert.That(exception.Value, Is.EqualTo(illegalValue));
                                                 Assert.That(exception.InnerException, Is.Null);
                                             });

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            finansstyringKonfigurationViewModel.DageForNyheder = illegalValue;

            finansstyringKonfigurationRepositoryMock.AssertWasNotCalled(m => m.DageForNyheder);
            finansstyringKonfigurationRepositoryMock.AssertWasNotCalled(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.Anything));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf));
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
                                                    .Return(7)
                                                    .Repeat.Any();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull))
                                                    .Throw(fixture.Create<IntranetGuiRepositoryException>())
                                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            finansstyringKonfigurationViewModel.DageForNyheder = 14;

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
                                                    .Return(7)
                                                    .Repeat.Any();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull))
                                                    .Throw(fixture.Create<IntranetGuiBusinessException>())
                                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            finansstyringKonfigurationViewModel.DageForNyheder = 14;

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
                                                    .Return(7)
                                                    .Repeat.Any();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull))
                                                    .Throw(fixture.Create<IntranetGuiSystemException>())
                                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(finansstyringKonfigurationRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            finansstyringKonfigurationViewModel.DageForNyheder = 14;

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
                                                    .Return(7)
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

            finansstyringKonfigurationViewModel.DageForNyheder = 14;

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.DageForNyheder);
            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.KonfigurationerAdd(Arg<IDictionary<string, object>>.Is.NotNull));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at CleanValidationErrors nulstiller valideringsfejl.
        /// </summary>
        [Test]
        [TestCase("FinansstyringServiceUriValidationError")]
        [TestCase("AntalBogføringslinjerValidationError")]
        [TestCase("DageForNyhederValidationError")]
        public void TestAtCleanValidationErrorsNulstillerValideringsfejl(string expectedPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(fixture.Create<IFinansstyringKonfigurationRepository>(), exceptionHandlerViewModelMock);
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);

            var eventCalled = false;
            finansstyringKonfigurationViewModel.PropertyChanged += (s, e) =>
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
            finansstyringKonfigurationViewModel.ClearValidationErrors();
            Assert.That(eventCalled, Is.True);

            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at ValidateFinansstyringServiceUri returnerer Success for lovlige værdier.
        /// </summary>
        [Test]
        [TestCase("http://localhost")]
        [TestCase("http://www.google.dk")]
        public void TestAtValidateFinansstyringServiceUriReturnererSuccessForLovligeValues(string value)
        {
            var result = FinansstyringKonfigurationViewModel.ValidateFinansstyringServiceUri(value);
            Assert.That(result, Is.EqualTo(ValidationResult.Success));
        }

        /// <summary>
        /// Tester, at ValidateFinansstyringServiceUri returnerer valideringsresultat for ulovlige værdier.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("XYZ")]
        public void TestAtValidateFinansstyringServiceUriReturnererValidationResultForUlovligeValues(string value)
        {
            var result = FinansstyringKonfigurationViewModel.ValidateFinansstyringServiceUri(value);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.EqualTo(ValidationResult.Success));
            Assert.That(result.ErrorMessage, Is.Not.Null);
            Assert.That(result.ErrorMessage, Is.Not.Empty);
            Assert.That(result.ErrorMessage, Is.EqualTo(Resource.GetText(Text.InvalidValueForUri, value)));
            Assert.That(result.MemberNames, Is.Not.Null);
            Assert.That(result.MemberNames, Is.Empty);
        }

        /// <summary>
        /// Tester, at ValidateAntalBogføringslinjer returnerer Success for lovlige værdier.
        /// </summary>
        [Test]
        [TestCase(10)]
        [TestCase(15)]
        [TestCase(250)]
        public void TestAtValidateAntalBogføringslinjerReturnererSuccessForLovligeValues(int value)
        {
            var result = FinansstyringKonfigurationViewModel.ValidateAntalBogføringslinjer(value);
            Assert.That(result, Is.EqualTo(ValidationResult.Success));
        }

        /// <summary>
        /// Tester, at ValidateAntalBogføringslinjer returnerer valideringsresultat for ulovlige værdier.
        /// </summary>
        [Test]
        [TestCase(0)]
        [TestCase(9)]
        [TestCase(251)]
        [TestCase(512)]
        public void TestAtValidateAntalBogføringslinjerReturnererValidationResultForUlovligeValues(int value)
        {
            var result = FinansstyringKonfigurationViewModel.ValidateAntalBogføringslinjer(value);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.EqualTo(ValidationResult.Success));
            Assert.That(result.ErrorMessage, Is.Not.Null);
            Assert.That(result.ErrorMessage, Is.Not.Empty);
            Assert.That(result.ErrorMessage, Is.EqualTo(Resource.GetText(Text.ValueOutsideInterval, 10, 250)));
            Assert.That(result.MemberNames, Is.Not.Null);
            Assert.That(result.MemberNames, Is.Empty);
        }

        /// <summary>
        /// Tester, at ValidateDageForNyheder returnerer Success for lovlige værdier.
        /// </summary>
        [Test]
        [TestCase(0)]
        [TestCase(15)]
        [TestCase(30)]
        public void TestAtValidateDageForNyhederReturnererSuccessForLovligeValues(int value)
        {
            var result = FinansstyringKonfigurationViewModel.ValidateDageForNyheder(value);
            Assert.That(result, Is.EqualTo(ValidationResult.Success));
        }

        /// <summary>
        /// Tester, at ValidateDageForNyheder returnerer valideringsresultat for ulovlige værdier.
        /// </summary>
        [Test]
        [TestCase(-10)]
        [TestCase(-1)]
        [TestCase(31)]
        [TestCase(50)]
        public void TestAtValidateDageForNyhederReturnererValidationResultForUlovligeValues(int value)
        {
            var result = FinansstyringKonfigurationViewModel.ValidateDageForNyheder(value);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.EqualTo(ValidationResult.Success));
            Assert.That(result.ErrorMessage, Is.Not.Null);
            Assert.That(result.ErrorMessage, Is.Not.Empty);
            Assert.That(result.ErrorMessage, Is.EqualTo(Resource.GetText(Text.ValueOutsideInterval, 0, 30)));
            Assert.That(result.MemberNames, Is.Not.Null);
            Assert.That(result.MemberNames, Is.Empty);
        }
    }
}
