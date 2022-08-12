using System;
using System.Collections.Generic;
using System.Reflection;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Events;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests
{
    /// <summary>
    /// Tester ViewModel til binding mod Views.
    /// </summary>
    [TestFixture]
    public class MainViewModelTests
    {
        /// <summary>
        /// Tester, at konstruktøren initerer en ViewModel til binding mod Views.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererMainViewModel()
        {
            IMainViewModel mainViewModel = new MainViewModel();
            Assert.That(mainViewModel, Is.Not.Null);
            Assert.That(mainViewModel.DisplayName, Is.Not.Null);
            Assert.That(mainViewModel.DisplayName, Is.Not.Empty);
            Assert.That(mainViewModel.DisplayName, Is.EqualTo(mainViewModel.GetType().Name));
        }

        /// <summary>
        /// Tester, at getteren til Regnskabsliste returnerer ViewModel for en liste af regnskaber.
        /// </summary>
        [Test]
        public void TestAtRegnskabslisteGetterReturnererRegnskabslisteViewModel()
        {
            IMainViewModel mainViewModel = new MainViewModel();
            Assert.That(mainViewModel, Is.Not.Null);

            IRegnskabslisteViewModel regnskabslisteViewModel = mainViewModel.Regnskabsliste;
            Assert.That(regnskabslisteViewModel, Is.Not.Null);
            Assert.That(regnskabslisteViewModel, Is.TypeOf<RegnskabslisteViewModel>());
        }

        /// <summary>
        /// Tester, at getteren til FinansstyringKonfiguration returnerer ViewModel indeholdende konfiguration til finansstyring.
        /// </summary>
        [Test]
        public void TestAtFinansstyringKonfigurationGetterReturnererFinansstyringKonfigurationViewModel()
        {
            IMainViewModel mainViewModel = new MainViewModel();
            Assert.That(mainViewModel, Is.Not.Null);

            IFinansstyringKonfigurationViewModel finansstyringKonfigurationViewModel = mainViewModel.FinansstyringKonfiguration;
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);
            Assert.That(finansstyringKonfigurationViewModel, Is.TypeOf<FinansstyringKonfigurationViewModel>());
        }

        /// <summary>
        /// Tester, at getteren til ExceptionHandler returnerer ViewModel for en exceptionhandler.
        /// </summary>
        [Test]
        public void TestAtExceptionHandlerGetterReturnererExceptionHandlerViewModel()
        {
            IMainViewModel mainViewModel = new MainViewModel();
            Assert.That(mainViewModel, Is.Not.Null);

            IExceptionHandlerViewModel exceptionHandlerViewModel = mainViewModel.ExceptionHandler;
            Assert.That(exceptionHandlerViewModel, Is.Not.Null);
            Assert.That(exceptionHandlerViewModel, Is.TypeOf<ExceptionHandlerViewModel>());
        }

        /// <summary>
        /// Tester, at getteren til PrivacyPolicy returnerer ViewModel for Privacy Policy.
        /// </summary>
        [Test]
        public void TestAtPrivacyPolicyGetterReturnererPrivacyPolicyViewModel()
        {
            IMainViewModel mainViewModel = new MainViewModel();
            Assert.That(mainViewModel, Is.Not.Null);

            IPrivacyPolicyViewModel privacyPolicyViewModel = mainViewModel.PrivacyPolicy;
            Assert.That(privacyPolicyViewModel, Is.Not.Null);
            Assert.That(privacyPolicyViewModel, Is.TypeOf<PrivacyPolicyViewModel>());
        }

        /// <summary>
        /// Tester, at ApplyConfiguration kaster ArgumentNullException, hvis dictionaryet indeholdende konfiguration, er null.
        /// </summary>
        [Test]
        public void TestAtApplyConfigurationKasterArgumentNullExceptionHvisConfigurationSettingsErNull()
        {
            IMainViewModel mainViewModel = new MainViewModel();
            Assert.That(mainViewModel, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => mainViewModel.ApplyConfiguration(null));
        }

        /// <summary>
        /// Tester, at SwitchToLocaleDataStorage kaster en ArgumentNullException, hvis det lokale datalager til finansstyring er null.
        /// </summary>
        [Test]
        public void TestAtSwitchToLocaleDataStorageKasterArgumentNullExceptionHvisFinansstyringRepositoryLocaleErNull()
        {
            IMainViewModel mainViewModel = new MainViewModel();
            Assert.That(mainViewModel, Is.Not.Null);

            IRegnskabslisteViewModel regnskabslisteViewModel = mainViewModel.Regnskabsliste;
            Assert.That(regnskabslisteViewModel, Is.Not.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => mainViewModel.SwitchToLocaleDataStorage(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("finansstyringRepositoryLocale"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at SwitchToLocaleDataStorage skifter til det lokale datalager.
        /// </summary>
        [Test]
        public void TestAtSwitchToLocaleDataStorageSkifterTilLokaltDataLager()
        {
            Fixture fixture = new Fixture();

            IMainViewModel mainViewModel = new MainViewModel();
            Assert.That(mainViewModel, Is.Not.Null);

            IRegnskabslisteViewModel regnskabslisteViewModel = mainViewModel.Regnskabsliste;
            Assert.That(regnskabslisteViewModel, Is.Not.Null);

            mainViewModel.SwitchToLocaleDataStorage(fixture.BuildFinansstyringRepository());
            Assert.That(mainViewModel.Regnskabsliste, Is.Not.SameAs(regnskabslisteViewModel));
        }

        /// <summary>
        /// Tester, at SwitchToLocaleDataStorage rejser PropertyChanged.
        /// </summary>
        [Test]
        [TestCase("Regnskabsliste")]
        public void TestAtSwitchToLocaleDataStorageRejserPropertyChanged(string expectedPropertyName)
        {
            Fixture fixture = new Fixture();

            IMainViewModel mainViewModel = new MainViewModel();
            Assert.That(mainViewModel, Is.Not.Null);

            bool eventCalled = false;
            mainViewModel.PropertyChanged += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(s, Is.TypeOf<MainViewModel>());
                Assert.That(e, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Empty);
                if (string.CompareOrdinal(e.PropertyName, expectedPropertyName) == 0)
                {
                    eventCalled = true;
                }
            };

            Assert.That(eventCalled, Is.False);
            mainViewModel.SwitchToLocaleDataStorage(fixture.BuildFinansstyringRepository());
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at ApplyConfiguration tilføjer konfiguration.
        /// </summary>
        [Test]
        public void TestAtApplyConfigurationAdderConfigurationSettings()
        {
            Fixture fixture = new Fixture();

            IMainViewModel mainViewModel = new MainViewModel();
            Assert.That(mainViewModel, Is.Not.Null);

            IDictionary<string, object> configurationSettings = new Dictionary<string, object>
            {
                { "FinansstyringServiceUri", "http://www.google.dk" },
                { "AntalBogføringslinjer", fixture.Create<int>() },
                { "DageForNyheder", fixture.Create<int>() }
            };
            mainViewModel.ApplyConfiguration(configurationSettings);

            // ReSharper disable PossibleNullReferenceException
            IFinansstyringKonfigurationRepository finansstyringKonfigurationRepository = (IFinansstyringKonfigurationRepository) mainViewModel.GetType().GetProperty("FinansstyringKonfigurationRepository", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static).GetValue(mainViewModel);
            // ReSharper restore PossibleNullReferenceException
            Assert.That(finansstyringKonfigurationRepository, Is.Not.Null);
            Assert.That(finansstyringKonfigurationRepository.FinansstyringServiceUri, Is.Not.Null);
            Assert.That(finansstyringKonfigurationRepository.FinansstyringServiceUri, Is.EqualTo(new Uri(Convert.ToString(configurationSettings["FinansstyringServiceUri"]))));
            Assert.That(finansstyringKonfigurationRepository.AntalBogføringslinjer, Is.EqualTo(Convert.ToInt32(configurationSettings["AntalBogføringslinjer"])));
            Assert.That(finansstyringKonfigurationRepository.DageForNyheder, Is.EqualTo(Convert.ToInt32(configurationSettings["DageForNyheder"])));
        }

        /// <summary>
        /// Tester, at Subscribe kaster en ArgumentNullException, hvis subscriber, der skal tilmeldes, er null.
        /// </summary>
        [Test]
        public void TestAtSubscribeKasterArgumentNullExceptionHvisEventSubscriberErNull()
        {
            IMainViewModel mainViewModel = new MainViewModel();
            Assert.That(mainViewModel, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => mainViewModel.Subscribe<IIntranetGuiEventArgs>(null));
        }

        /// <summary>
        /// Tester, at Subscribe tilmelder en event subscriber.
        /// </summary>
        [Test]
        public void TestAtSubscribeTilmelderSubscriber()
        {
            Fixture fixture = new Fixture();

            IMainViewModel mainViewModel = new MainViewModel();
            Assert.That(mainViewModel, Is.Not.Null);

            IEventSubscriber<IIntranetGuiEventArgs> eventSubscriber = fixture.BuildEventSubscriber<IIntranetGuiEventArgs>();
            mainViewModel.Subscribe(eventSubscriber);

            FieldInfo field = mainViewModel.GetType().GetField("_eventSubscribers", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.That(field, Is.Not.Null);

            // ReSharper disable PossibleNullReferenceException
            IList<object> eventSubscribers = (IList<object>) field.GetValue(mainViewModel);
            // ReSharper restore PossibleNullReferenceException
            Assert.That(eventSubscribers, Is.Not.Null);
            Assert.That(eventSubscribers.Contains(eventSubscriber), Is.True);
        }

        /// <summary>
        /// Tester, at Unsubscribe kaster en ArgumentNullException, hvis subscriber, der skal frameldes, er null.
        /// </summary>
        [Test]
        public void TestAtUnsubscribeKasterArgumentNullExceptionHvisEventSubscriberErNull()
        {
            IMainViewModel mainViewModel = new MainViewModel();
            Assert.That(mainViewModel, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => mainViewModel.Unsubscribe<IIntranetGuiEventArgs>(null));
        }

        /// <summary>
        /// Tester, at Unsubscribe framelder en event subscriber.
        /// </summary>
        [Test]
        public void TestAtUnsubscribeFramelderSubscriber()
        {
            Fixture fixture = new Fixture();

            IMainViewModel mainViewModel = new MainViewModel();
            Assert.That(mainViewModel, Is.Not.Null);

            IEventSubscriber<IIntranetGuiEventArgs> eventSubscriber = fixture.BuildEventSubscriber<IIntranetGuiEventArgs>();
            mainViewModel.Subscribe(eventSubscriber);

            FieldInfo field = mainViewModel.GetType().GetField("_eventSubscribers", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.That(field, Is.Not.Null);

            // ReSharper disable PossibleNullReferenceException
            IList<object> eventSubscribers = (IList<object>) field.GetValue(mainViewModel);
            // ReSharper restore PossibleNullReferenceException
            Assert.That(eventSubscribers, Is.Not.Null);
            Assert.That(eventSubscribers.Contains(eventSubscriber), Is.True);

            mainViewModel.Unsubscribe(eventSubscriber);

            eventSubscribers = (IList<object>) field.GetValue(mainViewModel);
            Assert.That(eventSubscribers, Is.Not.Null);
            Assert.That(eventSubscribers.Contains(eventSubscriber), Is.False);
        }

        /// <summary>
        /// Tester, at OnEvent kaldes for alle event subscribers, der venter på events, hvor argumenter er af typen HandleExceptionEventArgs.
        /// </summary>
        [Test]
        public void TestAtOnEventKaldesForAlleHandleExceptionEventArgsSubscribers()
        {
            Fixture fixture = new Fixture();

            IMainViewModel mainViewModel = new MainViewModel();
            Assert.That(mainViewModel, Is.Not.Null);

            List<Mock<IEventSubscriber<IHandleExceptionEventArgs>>> eventSubscriberMockCollection = new List<Mock<IEventSubscriber<IHandleExceptionEventArgs>>>
            {
                fixture.BuildEventSubscriberMock<IHandleExceptionEventArgs>(handleExceptionEventArgs => handleExceptionEventArgs.IsHandled = false),
                fixture.BuildEventSubscriberMock<IHandleExceptionEventArgs>(handleExceptionEventArgs => handleExceptionEventArgs.IsHandled = false),
                fixture.BuildEventSubscriberMock<IHandleExceptionEventArgs>(handleExceptionEventArgs => handleExceptionEventArgs.IsHandled = false),
                fixture.BuildEventSubscriberMock<IHandleExceptionEventArgs>(handleExceptionEventArgs => handleExceptionEventArgs.IsHandled = false),
                fixture.BuildEventSubscriberMock<IHandleExceptionEventArgs>(handleExceptionEventArgs => handleExceptionEventArgs.IsHandled = false),
                fixture.BuildEventSubscriberMock<IHandleExceptionEventArgs>(handleExceptionEventArgs => handleExceptionEventArgs.IsHandled = false),
                fixture.BuildEventSubscriberMock<IHandleExceptionEventArgs>(handleExceptionEventArgs => handleExceptionEventArgs.IsHandled = false)
            };

            IExceptionHandlerViewModel exceptionHandlerViewModel = mainViewModel.ExceptionHandler;
            Assert.That(exceptionHandlerViewModel, Is.Not.Null);

            try
            {
                eventSubscriberMockCollection.ForEach(eventSubscriberMock => mainViewModel.Subscribe(eventSubscriberMock.Object));

                IntranetGuiSystemException exception = fixture.Create<IntranetGuiSystemException>();
                exceptionHandlerViewModel.HandleException(exception);

                eventSubscriberMockCollection.ForEach(eventSubscriberMock => eventSubscriberMock.Verify(m => m.OnEvent(It.Is<IHandleExceptionEventArgs>(value => value != null && value == exception)), Times.Once));
            }
            finally
            {
                eventSubscriberMockCollection.ForEach(eventSubscriberMock => mainViewModel.Unsubscribe(eventSubscriberMock.Object));
            }
        }
    }
}