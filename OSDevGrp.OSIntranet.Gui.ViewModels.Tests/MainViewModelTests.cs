using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Events;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces;
using Rhino.Mocks;

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
            var mainViewModel = new MainViewModel();
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
            var mainViewModel = new MainViewModel();
            Assert.That(mainViewModel, Is.Not.Null);

            var regnskabslisteViewModel = mainViewModel.Regnskabsliste;
            Assert.That(regnskabslisteViewModel, Is.Not.Null);
            Assert.That(regnskabslisteViewModel, Is.TypeOf<RegnskabslisteViewModel>());
        }

        /// <summary>
        /// Tester, at getteren til FinansstyringKonfiguration returnerer ViewModel indeholdende konfiguration til finansstyring.
        /// </summary>
        [Test]
        public void TestAtFinansstyringKonfigurationGetterReturnererFinansstyringKonfigurationViewModel()
        {
            var mainViewModel = new MainViewModel();
            Assert.That(mainViewModel, Is.Not.Null);

            var finansstyringKonfigurationViewModel = mainViewModel.FinansstyringKonfiguration;
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);
            Assert.That(finansstyringKonfigurationViewModel, Is.TypeOf<FinansstyringKonfigurationViewModel>());
        }

        /// <summary>
        /// Tester, at getteren til ExceptionHandler returnerer ViewModel for en exceptionhandler.
        /// </summary>
        [Test]
        public void TestAtExceptionHandlerGetterReturnererExceptionHandlerViewModel()
        {
            var mainViewModel = new MainViewModel();
            Assert.That(mainViewModel, Is.Not.Null);

            var exceptionHandlerViewModel = mainViewModel.ExceptionHandler;
            Assert.That(exceptionHandlerViewModel, Is.Not.Null);
            Assert.That(exceptionHandlerViewModel, Is.TypeOf<ExceptionHandlerViewModel>());
        }

        /// <summary>
        /// Tester, at getteren til PrivacyPolicy returnerer ViewModel for Privacy Policy.
        /// </summary>
        [Test]
        public void TestAtPrivacyPolicyGetterReturnererPrivacyPolicyViewModel()
        {
            var mainViewModel = new MainViewModel();
            Assert.That(mainViewModel, Is.Not.Null);

            var privacyPolicyViewModel = mainViewModel.PrivacyPolicy;
            Assert.That(privacyPolicyViewModel, Is.Not.Null);
            Assert.That(privacyPolicyViewModel, Is.TypeOf<PrivacyPolicyViewModel>());
        }

        /// <summary>
        /// Tester, at ApplyConfiguration kaster ArgumentNullException, hvis dictionaryet indeholdende konfiguration, er null.
        /// </summary>
        [Test]
        public void TestAtApplyConfigurationKasterArgumentNullExceptionHvisConfigurationSettingsErNull()
        {
            var mainViewModel = new MainViewModel();
            Assert.That(mainViewModel, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => mainViewModel.ApplyConfiguration(null));
        }

        /// <summary>
        /// Tester, at SwitchToLocaleDataStorage kaster en ArgumentNullException, hvis det lokale datalager til finansstyring er null.
        /// </summary>
        [Test]
        public void TestAtSwitchToLocaleDataStorageKasterArgumentNullExceptionHvisFinansstyringRepositoryLocaleErNull()
        {
            var mainViewModel = new MainViewModel();
            Assert.That(mainViewModel, Is.Not.Null);

            var regnskabslisteViewModel = mainViewModel.Regnskabsliste;
            Assert.That(regnskabslisteViewModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => mainViewModel.SwitchToLocaleDataStorage(null));
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
            var mainViewModel = new MainViewModel();
            Assert.That(mainViewModel, Is.Not.Null);

            var regnskabslisteViewModel = mainViewModel.Regnskabsliste;
            Assert.That(regnskabslisteViewModel, Is.Not.Null);

            mainViewModel.SwitchToLocaleDataStorage(MockRepository.GenerateMock<IFinansstyringRepository>());
            Assert.That(mainViewModel.Regnskabsliste, Is.Not.SameAs(regnskabslisteViewModel));
        }

        /// <summary>
        /// Tester, at SwitchToLocaleDataStorage rejser PropertyChanged.
        /// </summary>
        [Test]
        [TestCase("Regnskabsliste")]
        public void TestAtSwitchToLocaleDataStorageRejserPropertyChanged(string expectedPropertyName)
        {
            var mainViewModel = new MainViewModel();
            Assert.That(mainViewModel, Is.Not.Null);

            var eventCalled = false;
            mainViewModel.PropertyChanged += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(s, Is.TypeOf<MainViewModel>());
                Assert.That(e, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Empty);
                if (string.Compare(e.PropertyName, expectedPropertyName, StringComparison.InvariantCulture) == 0)
                {
                    eventCalled = true;
                }
            };

            Assert.That(eventCalled, Is.False);
            mainViewModel.SwitchToLocaleDataStorage(MockRepository.GenerateMock<IFinansstyringRepository>());
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at ApplyConfiguration tilføjer konfiguration.
        /// </summary>
        [Test]
        public void TestAtApplyConfigurationAdderConfigurationSettings()
        {
            var fixture = new Fixture();

            var mainViewModel = new MainViewModel();
            Assert.That(mainViewModel, Is.Not.Null);

            var configurationSettings = new Dictionary<string, object>
                {
                    {"FinansstyringServiceUri", "http://www.google.dk"},
                    {"AntalBogføringslinjer", fixture.Create<int>()},
                    {"DageForNyheder", fixture.Create<int>()}
                };
            mainViewModel.ApplyConfiguration(configurationSettings);

            var finansstyringKonfigurationRepository = (IFinansstyringKonfigurationRepository) mainViewModel.GetType().GetProperty("FinansstyringKonfigurationRepository", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static).GetValue(mainViewModel);
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
            var mainViewModel = new MainViewModel();
            Assert.That(mainViewModel, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => mainViewModel.Subscribe<IIntranetGuiEventArgs>(null));
        }

        /// <summary>
        /// Tester, at Subscribe tilmelder en event subscriber.
        /// </summary>
        [Test]
        public void TestAtSubscribeTilmelderSubscriber()
        {
            var mainViewModel = new MainViewModel();
            Assert.That(mainViewModel, Is.Not.Null);

            var eventSubscriberMock = MockRepository.GenerateMock<IEventSubscriber<IIntranetGuiEventArgs>>();
            mainViewModel.Subscribe(eventSubscriberMock);

            var field = mainViewModel.GetType().GetField("_eventSubscribers", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.That(field, Is.Not.Null);

            // ReSharper disable PossibleNullReferenceException
            var eventSubscribers = (IList<object>) field.GetValue(mainViewModel);
            // ReSharper restore PossibleNullReferenceException
            Assert.That(eventSubscribers, Is.Not.Null);
            Assert.That(eventSubscribers.Contains(eventSubscriberMock), Is.True);
        }

        /// <summary>
        /// Tester, at Unsubscribe kaster en ArgumentNullException, hvis subscriber, der skal frameldes, er null.
        /// </summary>
        [Test]
        public void TestAtUnsubscribeKasterArgumentNullExceptionHvisEventSubscriberErNull()
        {
            var mainViewModel = new MainViewModel();
            Assert.That(mainViewModel, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => mainViewModel.Unsubscribe<IIntranetGuiEventArgs>(null));
        }

        /// <summary>
        /// Tester, at Unsubscribe framelder en event subscriber.
        /// </summary>
        [Test]
        public void TestAtUnsubscribeFramelderSubscriber()
        {
            var mainViewModel = new MainViewModel();
            Assert.That(mainViewModel, Is.Not.Null);

            var eventSubscriberMock = MockRepository.GenerateMock<IEventSubscriber<IIntranetGuiEventArgs>>();
            mainViewModel.Subscribe(eventSubscriberMock);

            var field = mainViewModel.GetType().GetField("_eventSubscribers", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.That(field, Is.Not.Null);

            // ReSharper disable PossibleNullReferenceException
            var eventSubscribers = (IList<object>) field.GetValue(mainViewModel);
            // ReSharper restore PossibleNullReferenceException
            Assert.That(eventSubscribers, Is.Not.Null);
            Assert.That(eventSubscribers.Contains(eventSubscriberMock), Is.True);

            mainViewModel.Unsubscribe(eventSubscriberMock);

            eventSubscribers = (IList<object>) field.GetValue(mainViewModel);
            Assert.That(eventSubscribers, Is.Not.Null);
            Assert.That(eventSubscribers.Contains(eventSubscriberMock), Is.False);
        }

        /// <summary>
        /// Tester, at OnEvent kaldes for alle event subscribers, der venter på events, hvor argumenter er af typen HandleExceptionEventArgs.
        /// </summary>
        [Test]
        public void TestAtOnEventKaldesForAlleHandleExceptionEventArgsSubscribers()
        {
            var fixture = new Fixture();
            fixture.Customize<IEventSubscriber<IHandleExceptionEventArgs>>(e => e.FromFactory(() =>
                {
                    var eventSubscriberMock = MockRepository.GenerateMock<IEventSubscriber<IHandleExceptionEventArgs>>();
                    eventSubscriberMock.Expect(m => m.OnEvent(Arg<IHandleExceptionEventArgs>.Is.NotNull))
                                       .WhenCalled(a =>
                                           {
                                               var handleExceptionEventArgs =
                                                   (IHandleExceptionEventArgs) a.Arguments.ElementAt(0);
                                               handleExceptionEventArgs.IsHandled = false;
                                           });
                    return eventSubscriberMock;
                }));

            var mainViewModel = new MainViewModel();
            Assert.That(mainViewModel, Is.Not.Null);

            var exceptionHandlerViewModel = mainViewModel.ExceptionHandler;
            Assert.That(exceptionHandlerViewModel, Is.Not.Null);

            var eventSubscribers = fixture.CreateMany<IEventSubscriber<IHandleExceptionEventArgs>>(7).ToList();
            try
            {
                eventSubscribers.ForEach(mainViewModel.Subscribe);

                exceptionHandlerViewModel.HandleException(fixture.Create<IntranetGuiSystemException>());

                eventSubscribers.ForEach(m => m.AssertWasCalled(n => n.OnEvent(Arg<IHandleExceptionEventArgs>.Is.NotNull)));
            }
            finally
            {
                eventSubscribers.ForEach(mainViewModel.Unsubscribe);
            }
        }
    }
}
