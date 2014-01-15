using System;
using System.ComponentModel;
using System.Globalization;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Finansstyring
{
    /// <summary>
    /// Tester ViewModel for et regnskab.
    /// </summary>
    [TestFixture]
    public class RegnskabViewModelTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en ViewModel for et regnskab.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererRegnskabViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IRegnskabModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(fixture.Create<int>())
                        .Repeat.Any();
                    mock.Expect(m => m.Navn)
                        .Return(fixture.Create<string>())
                        .Repeat.Any();
                    return mock;
                }));

            var regnskabModelMock = fixture.Create<IRegnskabModel>();
            var statusDato = fixture.Create<DateTime>();
            var regnskabViewModel = new RegnskabViewModel(regnskabModelMock, statusDato, fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Nummer, Is.EqualTo(regnskabModelMock.Nummer));
            Assert.That(regnskabViewModel.Navn, Is.Not.Null);
            Assert.That(regnskabViewModel.Navn, Is.Not.Empty);
            Assert.That(regnskabViewModel.Navn, Is.EqualTo(regnskabModelMock.Navn));
            Assert.That(regnskabViewModel.DisplayName, Is.Not.Null);
            Assert.That(regnskabViewModel.DisplayName, Is.Not.Empty);
            Assert.That(regnskabViewModel.DisplayName, Is.EqualTo(regnskabModelMock.Navn));
            Assert.That(regnskabViewModel.StatusDato, Is.EqualTo(statusDato));
            Assert.That(regnskabViewModel.Bogføringslinjer, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføringslinjer, Is.Empty);
            Assert.That(regnskabViewModel.Debitorer, Is.Not.Null);
            Assert.That(regnskabViewModel.Debitorer, Is.Empty);
            Assert.That(regnskabViewModel.Kreditorer, Is.Not.Null);
            Assert.That(regnskabViewModel.Kreditorer, Is.Empty);
            Assert.That(regnskabViewModel.Nyheder, Is.Not.Null);
            Assert.That(regnskabViewModel.Nyheder, Is.Empty);
            Assert.That(regnskabViewModel.RefreshCommand, Is.Not.Null);
            Assert.That(regnskabViewModel.RefreshCommand, Is.TypeOf<CommandCollectionExecuterCommand>());
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis modellen for regnskabet er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisRegnskabModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            Assert.Throws<ArgumentNullException>(() => new RegnskabViewModel(null, fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>()));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis repositoryet til finansstyring er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFinansstyringRepositoryErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            Assert.Throws<ArgumentNullException>(() => new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), null, fixture.Create<IExceptionHandlerViewModel>()));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis ViewModel for exceptionhandleren er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisExceptionHandlerViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            Assert.Throws<ArgumentNullException>(() => new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), null));
        }

        /// <summary>
        /// Tester, at sætteren på Navn opdaterer Navn på modellen for regnskabet.
        /// </summary>
        [Test]
        public void TestAtNavnSetterOpdatererNavnOnRegnskabModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var regnskabModelMock = fixture.Create<IRegnskabModel>();
            var regnskabViewModel = new RegnskabViewModel(regnskabModelMock, fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var newValue = fixture.Create<string>();
            regnskabViewModel.Navn = newValue;

            regnskabModelMock.AssertWasCalled(m => m.Navn = Arg<string>.Is.Equal(newValue));
        }

        /// <summary>
        /// Tester, at sætteren til StatusDato kaster en ArgumentException, hvis værdien er mindre end den allerede satte statusdato.
        /// </summary>
        [Test]
        [TestCase("2013-01-01T12:00:00", "2013-01-01T11:59:59")]
        [TestCase("2013-01-01T12:00:00", "2012-12-31T23:00:00")]
        [TestCase("2013-01-01T12:00:00", "2012-12-31T12:00:00")]
        public void TestAtStatusDatoSetterKasterArgumentExceptionHvisValueErMindreEndStatusDato(string originalDateTime, string newDateTime)
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var statusDato = DateTime.Parse(originalDateTime, new CultureInfo("en-US"));
            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), statusDato, fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.StatusDato, Is.EqualTo(statusDato));

            Assert.Throws<ArgumentException>(() => regnskabViewModel.StatusDato = DateTime.Parse(newDateTime, new CultureInfo("en-US")));
        }

        /// <summary>
        /// Tester, at sætteren til StatusDato opdaterer statusdato.
        /// </summary>
        [Test]
        [TestCase("2013-01-01T12:00:00", "2013-01-01T12:00:01")]
        [TestCase("2013-01-01T12:00:00", "2013-06-30T12:00:00")]
        [TestCase("2013-01-01T12:00:00", "2013-12-31T00:00:00")]
        public void TestAtStatusDatoSetterOpdatererStatusDato(string originalDateTime, string newDateTime)
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var statusDato = DateTime.Parse(originalDateTime, new CultureInfo("en-US"));
            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), statusDato, fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.StatusDato, Is.EqualTo(statusDato));

            var newValue = DateTime.Parse(newDateTime, new CultureInfo("en-US"));
            regnskabViewModel.StatusDato = newValue;
            Assert.That(regnskabViewModel.StatusDato, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tester, at sætteren til StatusDato rejser PropertyChanged ved ændring af statusdatoen.
        /// </summary>
        [Test]
        [TestCase("2013-01-01T12:00:00", "2013-01-01T12:00:01", "StatusDato")]
        [TestCase("2013-01-01T12:00:00", "2013-06-30T12:00:00", "StatusDato")]
        [TestCase("2013-01-01T12:00:00", "2013-12-31T00:00:00", "StatusDato")]
        public void TestAtStatusDatoSetterRejserPropertyChangedVedVedOpdateringAfStatusDato(string originalDateTime, string newDateTime, string expectPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var statusDato = DateTime.Parse(originalDateTime, new CultureInfo("en-US"));
            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), statusDato, fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
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
            regnskabViewModel.StatusDato = DateTime.Parse(newDateTime, new CultureInfo("en-US"));
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at BogføringslinjeAdd kaster en ArgumentNullException, hvis ViewModel for bogføringslinjen er null.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjeAddKasterArgumentNullExceptionHvisBogføringslinjeViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => regnskabViewModel.BogføringslinjeAdd(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("bogføringslinjeViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at BogføringslinjeAdd tilføjer en bogføringslinje til regnskabet.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjeAddAddsBogføringslinjeViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IReadOnlyBogføringslinjeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføringslinjer, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføringslinjer, Is.Empty);

            var bogføringslinjeViewModelMock = fixture.Create<IReadOnlyBogføringslinjeViewModel>();
            regnskabViewModel.BogføringslinjeAdd(bogføringslinjeViewModelMock);
            
            Assert.That(regnskabViewModel.Bogføringslinjer, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføringslinjer, Is.Not.Empty);
        }

        /// <summary>
        /// Tester, at BogføringslinjeAdd rejser PropertyChanged, når en bogføringslinje tilføjes regnskabet.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjeAddRejserPropertyChangedVedAddAfBogføringslinjeViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IReadOnlyBogføringslinjeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, "Bogføringslinjer", StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            regnskabViewModel.BogføringslinjeAdd(fixture.Create<IReadOnlyBogføringslinjeViewModel>());
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at DebitorAdd kaster en ArgumentNullException, hvis ViewModel for adressekontoen, der skal tilføjes som debitor, er null.
        /// </summary>
        [Test]
        public void TestAtDebitorAddKasterArgumentNullExceptionHvisAdressekontoViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => regnskabViewModel.DebitorAdd(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("adressekontoViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at DebitorAdd tilføjer en debitor til regnskabet.
        /// </summary>
        [Test]
        public void TestAtDebitorAddAddsAdressekontoViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IAdressekontoViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IAdressekontoViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Debitorer, Is.Not.Null);
            Assert.That(regnskabViewModel.Debitorer, Is.Empty);

            var adressekontoViewModelMock = fixture.Create<IAdressekontoViewModel>();
            regnskabViewModel.DebitorAdd(adressekontoViewModelMock);

            Assert.That(regnskabViewModel.Debitorer, Is.Not.Null);
            Assert.That(regnskabViewModel.Debitorer, Is.Not.Empty);
        }

        /// <summary>
        /// Tester, at DebitorAdd rejser PropertyChanged, når en debitor tilføjes regnskabet.
        /// </summary>
        [Test]
        public void TestAtDebitorAddRejserPropertyChangedVedAddAfAdressekontoViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IAdressekontoViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IAdressekontoViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, "Debitorer", StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            regnskabViewModel.DebitorAdd(fixture.Create<IAdressekontoViewModel>());
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at KreditorAdd kaster en ArgumentNullException, hvis ViewModel for adressekontoen, der skal tilføjes som kreditor, er null.
        /// </summary>
        [Test]
        public void TestAtKreditorAddKasterArgumentNullExceptionHvisAdressekontoViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => regnskabViewModel.KreditorAdd(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("adressekontoViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at KreditorAdd tilføjer en kreditor til regnskabet.
        /// </summary>
        [Test]
        public void TestAtKreditorAddAddsAdressekontoViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IAdressekontoViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IAdressekontoViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Kreditorer, Is.Not.Null);
            Assert.That(regnskabViewModel.Kreditorer, Is.Empty);

            var adressekontoViewModelMock = fixture.Create<IAdressekontoViewModel>();
            regnskabViewModel.KreditorAdd(adressekontoViewModelMock);

            Assert.That(regnskabViewModel.Kreditorer, Is.Not.Null);
            Assert.That(regnskabViewModel.Kreditorer, Is.Not.Empty);
        }

        /// <summary>
        /// Tester, at KreditorAdd rejser PropertyChanged, når en debitor tilføjes regnskabet.
        /// </summary>
        [Test]
        public void TestAtKreditorAddRejserPropertyChangedVedAddAfAdressekontoViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IAdressekontoViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IAdressekontoViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, "Kreditorer", StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            regnskabViewModel.KreditorAdd(fixture.Create<IAdressekontoViewModel>());
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at NyhedAdd kaster en ArgumentNullException, hvis ViewModel for nyheden er null.
        /// </summary>
        [Test]
        public void TestAtNyhedAddKasterArgumentNullExceptionHvisNyhedViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => regnskabViewModel.NyhedAdd(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("nyhedViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at NyhedAdd tilføjer en nyhed til regnskabet.
        /// </summary>
        [Test]
        public void TestAtNyhedAddAddsNyhedViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<INyhedViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<INyhedViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Nyheder, Is.Not.Null);
            Assert.That(regnskabViewModel.Nyheder, Is.Empty);

            var nyhedViewModelMock = fixture.Create<INyhedViewModel>();
            regnskabViewModel.NyhedAdd(nyhedViewModelMock);
            Assert.That(regnskabViewModel.Nyheder, Is.Not.Null);
            Assert.That(regnskabViewModel.Nyheder, Is.Not.Empty);
        }

        /// <summary>
        /// Tester, at NyhedAdd rejser PropertyChanged, når en nyhed tilføjes regnskabet.
        /// </summary>
        [Test]
        public void TestAtNyhedAddRejserPropertyChangedVedAddAfNyhedViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<INyhedViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<INyhedViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, "Nyheder", StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            regnskabViewModel.NyhedAdd(fixture.Create<INyhedViewModel>());
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnRegnskabModelEventHandler rejser PropertyChanged, når modellen for regnskabet opdateres.
        /// </summary>
        [Test]
        [TestCase("Nummer", "Nummer")]
        [TestCase("Navn", "Navn")]
        [TestCase("Navn", "DisplayName")]
        public void TestAtPropertyChangedOnRegnskabModelEventHandlerRejserPropertyChangedOnRegnskabModelUpdate(string propertyNameToRaise, string expectPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var regnskabModelMock = fixture.Create<IRegnskabModel>();
            var regnskabViewModel = new RegnskabViewModel(regnskabModelMock, fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
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
            regnskabModelMock.Raise(m => m.PropertyChanged += null, regnskabModelMock, new PropertyChangedEventArgs(propertyNameToRaise));
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnRegnskabModelEventHandler kaster en ArgumentNullException, hvis objektet, der rejser eventet, er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnRegnskabModelEventHandlerKasterArgumentNullExceptionHvisSenderErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var regnskabModelMock = fixture.Create<IRegnskabModel>();
            var regnskabViewModel = new RegnskabViewModel(regnskabModelMock, fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => regnskabModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("sender"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnRegnskabModelEventHandler kaster en ArgumentNullException, hvis argumenter til eventet er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnRegnskabModelEventHandlerKasterArgumentNullExceptionHvisEventArgsErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var regnskabModelMock = fixture.Create<IRegnskabModel>();
            var regnskabViewModel = new RegnskabViewModel(regnskabModelMock, fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => regnskabModelMock.Raise(m => m.PropertyChanged += null, fixture.Create<object>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("e"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBogføringslinjeViewModelEventHandler rejser PropertyChanged, når ViewModel for en bogføringslinje opdateres.
        /// </summary>
        [Test]
        [TestCase("Regnskab")]
        [TestCase("Løbenummer")]
        [TestCase("Dato")]
        [TestCase("Kontonummer")]
        [TestCase("Tekst")]
        [TestCase("Budgetkontonummer")]
        [TestCase("Debit")]
        [TestCase("Kredit")]
        [TestCase("Bogført")]
        [TestCase("Adressekonto")]
        [TestCase("Image")]
        [TestCase("DisplayName")]
        public void TestAtPropertyChangedOnBogføringslinjeViewModelEventHandlerRejserPropertyChangedOnBogføringslinjeViewModelUpdate(string propertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IReadOnlyBogføringslinjeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var readOnlyBogføringslinjeViewModelMock = fixture.Create<IReadOnlyBogføringslinjeViewModel>();
            regnskabViewModel.BogføringslinjeAdd(readOnlyBogføringslinjeViewModelMock);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, "Bogføringslinjer", StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            readOnlyBogføringslinjeViewModelMock.Raise(m => m.PropertyChanged += null, readOnlyBogføringslinjeViewModelMock, new PropertyChangedEventArgs(propertyName));
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBogføringslinjeViewModelEventHandler kaster en ArgumentNullException, hvis objektet, der rejser eventet, er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnBogføringslinjeViewModelEventHandlerKasterArgumentNullExceptionHvisSenderErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IReadOnlyBogføringslinjeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var readOnlyBogføringslinjeViewModelMock = fixture.Create<IReadOnlyBogføringslinjeViewModel>();
            regnskabViewModel.BogføringslinjeAdd(readOnlyBogføringslinjeViewModelMock);

            var exception = Assert.Throws<ArgumentNullException>(() => readOnlyBogføringslinjeViewModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("sender"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBogføringslinjeViewModelEventHandler kaster en ArgumentNullException, hvis argumenter til eventet er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnBogføringslinjeViewModelEventHandlerKasterArgumentNullExceptionHvisEventArgsErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IReadOnlyBogføringslinjeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var readOnlyBogføringslinjeViewModelMock = fixture.Create<IReadOnlyBogføringslinjeViewModel>();
            regnskabViewModel.BogføringslinjeAdd(readOnlyBogføringslinjeViewModelMock);

            var exception = Assert.Throws<ArgumentNullException>(() => readOnlyBogføringslinjeViewModelMock.Raise(m => m.PropertyChanged += null, readOnlyBogføringslinjeViewModelMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("e"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnAdressekontoViewModelForDebitorEventHandler rejser PropertyChanged, når ViewModel for adressekontoen til en debitor opdateres.
        /// </summary>
        [Test]
        [TestCase("Regnskab")]
        [TestCase("Nummer")]
        [TestCase("Navn")]
        [TestCase("PrimærTelefon")]
        [TestCase("SekundærTelefon")]
        [TestCase("StatusDato")]
        [TestCase("Saldo")]
        [TestCase("Image")]
        [TestCase("DisplayName")]
        public void TestAtPropertyChangedOnAdressekontoViewModelForDebitorEventHandlerRejserPropertyChangedOnAdressekontoViewModelUpdate(string propertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IAdressekontoViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IAdressekontoViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var adressekontoViewModelMock = fixture.Create<IAdressekontoViewModel>();
            regnskabViewModel.DebitorAdd(adressekontoViewModelMock);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, "Debitorer", StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            adressekontoViewModelMock.Raise(m => m.PropertyChanged += null, adressekontoViewModelMock, new PropertyChangedEventArgs(propertyName));
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnAdressekontoViewModelForDebitorEventHandler kaster en ArgumentNullException, hvis objektet, der rejser eventet, er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnAdressekontoViewModelForDebitorEventHandlerKasterArgumentNullExceptionHvisSenderErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IAdressekontoViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IAdressekontoViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var adressekontoViewModelMock = fixture.Create<IAdressekontoViewModel>();
            regnskabViewModel.DebitorAdd(adressekontoViewModelMock);

            var exception = Assert.Throws<ArgumentNullException>(() => adressekontoViewModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("sender"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnAdressekontoViewModelForDebitorEventHandler kaster en ArgumentNullException, hvis argumenter til eventet er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnAdressekontoViewModelForDebitorEventHandlerKasterArgumentNullExceptionHvisEventArgsErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IAdressekontoViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IAdressekontoViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var adressekontoViewModelMock = fixture.Create<IAdressekontoViewModel>();
            regnskabViewModel.DebitorAdd(adressekontoViewModelMock);

            var exception = Assert.Throws<ArgumentNullException>(() => adressekontoViewModelMock.Raise(m => m.PropertyChanged += null, adressekontoViewModelMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("e"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnAdressekontoViewModelForKreditorEventHandler rejser PropertyChanged, når ViewModel for adressekontoen til en kreditor opdateres.
        /// </summary>
        [Test]
        [TestCase("Regnskab")]
        [TestCase("Nummer")]
        [TestCase("Navn")]
        [TestCase("PrimærTelefon")]
        [TestCase("SekundærTelefon")]
        [TestCase("StatusDato")]
        [TestCase("Saldo")]
        [TestCase("Image")]
        [TestCase("DisplayName")]
        public void TestAtPropertyChangedOnAdressekontoViewModelForKreditorEventHandlerRejserPropertyChangedOnAdressekontoViewModelUpdate(string propertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IAdressekontoViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IAdressekontoViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var adressekontoViewModelMock = fixture.Create<IAdressekontoViewModel>();
            regnskabViewModel.KreditorAdd(adressekontoViewModelMock);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, "Kreditorer", StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            adressekontoViewModelMock.Raise(m => m.PropertyChanged += null, adressekontoViewModelMock, new PropertyChangedEventArgs(propertyName));
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnAdressekontoViewModelForKreditorEventHandler kaster en ArgumentNullException, hvis objektet, der rejser eventet, er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnAdressekontoViewModelForKreditorEventHandlerKasterArgumentNullExceptionHvisSenderErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IAdressekontoViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IAdressekontoViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var adressekontoViewModelMock = fixture.Create<IAdressekontoViewModel>();
            regnskabViewModel.KreditorAdd(adressekontoViewModelMock);

            var exception = Assert.Throws<ArgumentNullException>(() => adressekontoViewModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("sender"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnAdressekontoViewModelForKreditorEventHandler kaster en ArgumentNullException, hvis argumenter til eventet er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnAdressekontoViewModelForKreditorEventHandlerKasterArgumentNullExceptionHvisEventArgsErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IAdressekontoViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IAdressekontoViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var adressekontoViewModelMock = fixture.Create<IAdressekontoViewModel>();
            regnskabViewModel.KreditorAdd(adressekontoViewModelMock);

            var exception = Assert.Throws<ArgumentNullException>(() => adressekontoViewModelMock.Raise(m => m.PropertyChanged += null, adressekontoViewModelMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("e"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnNyhedViewModelEventHandler rejser PropertyChanged, når ViewModel for en nyhed opdateres.
        /// </summary>
        [Test]
        [TestCase("Nyhedsaktualitet")]
        [TestCase("Nyhedsudgivelsestidspunkt")]
        [TestCase("Nyhedsinformation")]
        [TestCase("Image")]
        [TestCase("DisplayName")]
        public void TestAtPropertyChangedOnNyhedViewModelEventHandlerRejserPropertyChangedOnNyhedViewModelUpdate(string propertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<INyhedViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<INyhedViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var nyhedViewModelMock = fixture.Create<INyhedViewModel>();
            regnskabViewModel.NyhedAdd(nyhedViewModelMock);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, "Nyheder", StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            nyhedViewModelMock.Raise(m => m.PropertyChanged += null, nyhedViewModelMock, new PropertyChangedEventArgs(propertyName));
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnNyhedViewModelEventHandler kaster en ArgumentNullException, hvis objektet, der rejser eventet, er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnNyhedViewModelEventHandlerKasterArgumentNullExceptionHvisSenderErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<INyhedViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<INyhedViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var nyhedViewModelMock = fixture.Create<INyhedViewModel>();
            regnskabViewModel.NyhedAdd(nyhedViewModelMock);

            var exception = Assert.Throws<ArgumentNullException>(() => nyhedViewModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("sender"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnNyhedViewModelEventHandler kaster en ArgumentNullException, hvis argumenter til eventet er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnNyhedViewModelEventHandlerKasterArgumentNullExceptionHvisEventArgsErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<INyhedViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<INyhedViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var nyhedViewModelMock = fixture.Create<INyhedViewModel>();
            regnskabViewModel.NyhedAdd(nyhedViewModelMock);

            var exception = Assert.Throws<ArgumentNullException>(() => nyhedViewModelMock.Raise(m => m.PropertyChanged += null, nyhedViewModelMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("e"));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
