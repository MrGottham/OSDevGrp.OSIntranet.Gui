using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Rhino.Mocks;
using Text = OSDevGrp.OSIntranet.Gui.Resources.Text;

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
            Assert.That(regnskabViewModel.Konti, Is.Not.Null);
            Assert.That(regnskabViewModel.Konti, Is.Empty);
            Assert.That(regnskabViewModel.KontiGrouped, Is.Not.Null);
            Assert.That(regnskabViewModel.KontiGrouped, Is.Empty);
            Assert.That(regnskabViewModel.KontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.KontiTop, Is.Empty);
            Assert.That(regnskabViewModel.KontiTopGrouped, Is.Not.Null);
            Assert.That(regnskabViewModel.KontiTopGrouped, Is.Empty);
            Assert.That(regnskabViewModel.KontiHeader, Is.Not.Null);
            Assert.That(regnskabViewModel.KontiHeader, Is.Not.Empty);
            Assert.That(regnskabViewModel.KontiHeader, Is.EqualTo(Resource.GetText(Text.Accounts)));
            Assert.That(regnskabViewModel.Budgetkonti, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Empty);
            Assert.That(regnskabViewModel.BudgetkontiGrouped, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiGrouped, Is.Empty);
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Empty);
            Assert.That(regnskabViewModel.BudgetkontiTopGrouped, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiTopGrouped, Is.Empty);
            Assert.That(regnskabViewModel.BudgetkontiHeader, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiHeader, Is.Not.Empty);
            Assert.That(regnskabViewModel.BudgetkontiHeader, Is.EqualTo(Resource.GetText(Text.BudgetAccounts)));
            Assert.That(regnskabViewModel.Bogføringslinjer, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføringslinjer, Is.Empty);
            Assert.That(regnskabViewModel.BogføringslinjerHeader, Is.Not.Null);
            Assert.That(regnskabViewModel.BogføringslinjerHeader, Is.Not.Empty);
            Assert.That(regnskabViewModel.BogføringslinjerHeader, Is.EqualTo(Resource.GetText(Text.Bookkeeping)));
            Assert.That(regnskabViewModel.BogføringslinjerColumns, Is.Not.Null);
            Assert.That(regnskabViewModel.BogføringslinjerColumns, Is.Not.Empty);
            Assert.That(regnskabViewModel.BogføringslinjerColumns.Count(), Is.EqualTo(7));
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(0), Is.Not.Null);
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(0), Is.Not.Empty);
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(0), Is.EqualTo(Resource.GetText(Text.Date)));
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(1), Is.Not.Null);
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(1), Is.Not.Empty);
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(1), Is.EqualTo(Resource.GetText(Text.Annex)));
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(2), Is.Not.Null);
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(2), Is.Not.Empty);
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(2), Is.EqualTo(Resource.GetText(Text.Account)));
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(3), Is.Not.Null);
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(3), Is.Not.Empty);
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(3), Is.EqualTo(Resource.GetText(Text.Text)));
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(4), Is.Not.Null);
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(4), Is.Not.Empty);
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(4), Is.EqualTo(Resource.GetText(Text.BudgetAccount)));
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(5), Is.Not.Null);
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(5), Is.Not.Empty);
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(5), Is.EqualTo(Resource.GetText(Text.Debit)));
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(6), Is.Not.Null);
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(6), Is.Not.Empty);
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(6), Is.EqualTo(Resource.GetText(Text.Credit)));
            Assert.That(regnskabViewModel.Debitorer, Is.Not.Null);
            Assert.That(regnskabViewModel.Debitorer, Is.Empty);
            Assert.That(regnskabViewModel.DebitorerHeader, Is.Not.Null);
            Assert.That(regnskabViewModel.DebitorerHeader, Is.Not.Empty);
            Assert.That(regnskabViewModel.DebitorerHeader, Is.EqualTo(Resource.GetText(Text.Debtors)));
            Assert.That(regnskabViewModel.Kreditorer, Is.Not.Null);
            Assert.That(regnskabViewModel.Kreditorer, Is.Empty);
            Assert.That(regnskabViewModel.KreditorerHeader, Is.Not.Null);
            Assert.That(regnskabViewModel.KreditorerHeader, Is.Not.Empty);
            Assert.That(regnskabViewModel.KreditorerHeader, Is.EqualTo(Resource.GetText(Text.Creditors)));
            Assert.That(regnskabViewModel.Nyheder, Is.Not.Null);
            Assert.That(regnskabViewModel.Nyheder, Is.Empty);
            Assert.That(regnskabViewModel.NyhederHeader, Is.Not.Null);
            Assert.That(regnskabViewModel.NyhederHeader, Is.Not.Empty);
            Assert.That(regnskabViewModel.NyhederHeader, Is.EqualTo(Resource.GetText(Text.NewsMultiple)));
            Assert.That(regnskabViewModel.Kontogrupper, Is.Not.Null);
            Assert.That(regnskabViewModel.Kontogrupper.Count(), Is.GreaterThanOrEqualTo(0));
            Assert.That(regnskabViewModel.Budgetkontogrupper, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkontogrupper.Count(), Is.GreaterThanOrEqualTo(0));
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
        /// Tester, at getteren til KontiGroup returnerer grupperede konti.
        /// </summary>
        [Test]
        [TestCase(new[] {5, 6, 7, 8, 9})]
        public void TestAtKontiGroupedGetterReturnererDictionary(int[] kontogrupper)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var kontogruppeViewModelMockCollection = new List<IKontogruppeViewModel>(kontogrupper.Length);
            foreach (var id in kontogrupper)
            {
                var kontogruppeViewModelMock = MockRepository.GenerateMock<IKontogruppeViewModel>();
                kontogruppeViewModelMock.Expect(m => m.Nummer)
                                        .Return(id)
                                        .Repeat.Any();
                kontogruppeViewModelMockCollection.Add(kontogruppeViewModelMock);
            }

            var kontoViewModelMockCollection = CreateKontoViewModels(fixture, kontogruppeViewModelMockCollection, new Random(DateTime.Now.Second), 250).ToList();

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Konti, Is.Not.Null);
            Assert.That(regnskabViewModel.Konti, Is.Empty);
            Assert.That(regnskabViewModel.KontiGrouped, Is.Not.Null);
            Assert.That(regnskabViewModel.KontiGrouped, Is.Empty);

            kontogruppeViewModelMockCollection.ForEach(regnskabViewModel.KontogruppeAdd);
            kontoViewModelMockCollection.ForEach(regnskabViewModel.KontoAdd);
            Assert.That(regnskabViewModel.Konti, Is.Not.Null);
            Assert.That(regnskabViewModel.Konti, Is.Not.Empty);

            var kontiGrouped = regnskabViewModel.KontiGrouped;
            Assert.That(kontiGrouped, Is.Not.Null);
            Assert.That(kontiGrouped, Is.Not.Empty);
            Assert.That(kontiGrouped.Sum(m => m.Value.Count()), Is.EqualTo(regnskabViewModel.Konti.Count()));
        }

        /// <summary>
        /// Tester, at getteren til KontiTop udelader konti, hvor kontoværdien er 0.
        /// </summary>
        [Test]
        public void TestAtKontiTopGetterUdeladerKontiHvorKontoValueErNul()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IKontogruppeViewModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(fixture.Create<int>())
                        .Repeat.Any();
                    return mock;
                }));
            fixture.Customize<IKontoViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IKontoViewModel>();
                    mock.Expect(m => m.Kontonummer)
                        .Return(fixture.Create<string>())
                        .Repeat.Any();
                    mock.Expect(m => m.Kontogruppe)
                        .Return(fixture.Create<IKontogruppeViewModel>())
                        .Repeat.Any();
                    mock.Expect(m => m.Kontoværdi)
                        .Return(0M)
                        .Repeat.Any();
                    return mock;
                }));

            var kontoViewModelMockCollection = fixture.CreateMany<IKontoViewModel>(250).ToList();

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Konti, Is.Not.Null);
            Assert.That(regnskabViewModel.Konti, Is.Empty);
            Assert.That(regnskabViewModel.KontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.KontiTop, Is.Empty);

            kontoViewModelMockCollection.ForEach(regnskabViewModel.KontoAdd);
            Assert.That(regnskabViewModel.Konti, Is.Not.Null);
            Assert.That(regnskabViewModel.Konti, Is.Not.Empty);
            Assert.That(regnskabViewModel.Konti.Count(), Is.EqualTo(kontoViewModelMockCollection.Count));
            Assert.That(regnskabViewModel.KontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.KontiTop, Is.Empty);
        }

        /// <summary>
        /// Tester, at getteren til KontiTop returnerer topbenyttede konti.
        /// </summary>
        [Test]
        public void TestAtKontiTopGetterReturnererTopbenyttedeKonti()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IKontogruppeViewModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(fixture.Create<int>())
                        .Repeat.Any();
                    return mock;
                }));
            fixture.Customize<IKontoViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IKontoViewModel>();
                    mock.Expect(m => m.Kontonummer)
                        .Return(fixture.Create<string>())
                        .Repeat.Any();
                    mock.Expect(m => m.Kontogruppe)
                        .Return(fixture.Create<IKontogruppeViewModel>())
                        .Repeat.Any();
                    mock.Expect(m => m.Kontoværdi)
                        .Return(fixture.Create<decimal>())
                        .Repeat.Any();
                    return mock;
                }));

            var kontoViewModelMockCollection = fixture.CreateMany<IKontoViewModel>(250).ToList();

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Konti, Is.Not.Null);
            Assert.That(regnskabViewModel.Konti, Is.Empty);
            Assert.That(regnskabViewModel.KontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.KontiTop, Is.Empty);

            kontoViewModelMockCollection.ForEach(regnskabViewModel.KontoAdd);
            Assert.That(regnskabViewModel.Konti, Is.Not.Null);
            Assert.That(regnskabViewModel.Konti, Is.Not.Empty);
            Assert.That(regnskabViewModel.Konti.Count(), Is.EqualTo(kontoViewModelMockCollection.Count));
            Assert.That(regnskabViewModel.KontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.KontiTop, Is.Not.Empty);
            Assert.That(regnskabViewModel.KontiTop.Count(), Is.EqualTo(25));
        }

        /// <summary>
        /// Tester, at getteren til KontiTopGroup returnerer grupperede topbenyttede konti.
        /// </summary>
        [Test]
        [TestCase(new[] {5, 6, 7, 8, 9})]
        public void TestAtKontiTopGroupedGetterReturnererDictionary(int[] kontogrupper)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var kontogruppeViewModelMockCollection = new List<IKontogruppeViewModel>(kontogrupper.Length);
            foreach (var id in kontogrupper)
            {
                var kontogruppeViewModelMock = MockRepository.GenerateMock<IKontogruppeViewModel>();
                kontogruppeViewModelMock.Expect(m => m.Nummer)
                                        .Return(id)
                                        .Repeat.Any();
                kontogruppeViewModelMockCollection.Add(kontogruppeViewModelMock);
            }

            var kontoViewModelMockCollection = CreateKontoViewModels(fixture, kontogruppeViewModelMockCollection, new Random(DateTime.Now.Second), 250).ToList();

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.KontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.KontiTop, Is.Empty);
            Assert.That(regnskabViewModel.KontiTopGrouped, Is.Not.Null);
            Assert.That(regnskabViewModel.KontiTopGrouped, Is.Empty);

            kontogruppeViewModelMockCollection.ForEach(regnskabViewModel.KontogruppeAdd);
            kontoViewModelMockCollection.ForEach(regnskabViewModel.KontoAdd);
            Assert.That(regnskabViewModel.KontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.KontiTop, Is.Not.Empty);

            var kontiTopGrouped = regnskabViewModel.KontiTopGrouped;
            Assert.That(kontiTopGrouped, Is.Not.Null);
            Assert.That(kontiTopGrouped, Is.Not.Empty);
            Assert.That(kontiTopGrouped.Sum(m => m.Value.Count()), Is.EqualTo(regnskabViewModel.KontiTop.Count()));
        }

        /// <summary>
        /// Tester, at getteren til BudgetkontiGroup returnerer grupperede budgetkonti.
        /// </summary>
        [Test]
        [TestCase(new[] {5, 6, 7, 8, 9})]
        public void TestAtBudgetkontiGroupedGetterReturnererDictionary(int[] budgetkontogrupper)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var budgetkontogruppeViewModelMockCollection = new List<IBudgetkontogruppeViewModel>(budgetkontogrupper.Length);
            foreach (var id in budgetkontogrupper)
            {
                var budgetkontogruppeViewModelMock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                budgetkontogruppeViewModelMock.Expect(m => m.Nummer)
                                              .Return(id)
                                              .Repeat.Any();
                budgetkontogruppeViewModelMockCollection.Add(budgetkontogruppeViewModelMock);
            }

            var budgetkontoViewModelMockCollection = CreateBudgetkontoViewModels(fixture, budgetkontogruppeViewModelMockCollection, new Random(DateTime.Now.Second), 250).ToList();

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Empty);
            Assert.That(regnskabViewModel.BudgetkontiGrouped, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiGrouped, Is.Empty);

            budgetkontogruppeViewModelMockCollection.ForEach(regnskabViewModel.BudgetkontogruppeAdd);
            budgetkontoViewModelMockCollection.ForEach(regnskabViewModel.BudgetkontoAdd);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Not.Empty);

            var budgetkontiGrouped = regnskabViewModel.BudgetkontiGrouped;
            Assert.That(budgetkontiGrouped, Is.Not.Null);
            Assert.That(budgetkontiGrouped, Is.Not.Empty);
            Assert.That(budgetkontiGrouped.Sum(m => m.Value.Count()), Is.EqualTo(regnskabViewModel.Budgetkonti.Count()));
        }

        /// <summary>
        /// Tester, at getteren til BudgetkontiTop udelader budgetkonti, hvor kontoværdien er 0.
        /// </summary>
        [Test]
        public void TestAtBudgetkontiTopGetterUdeladerBudgetkontiHvorKontoValueErNul()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(fixture.Create<int>())
                        .Repeat.Any();
                    return mock;
                }));
            fixture.Customize<IBudgetkontoViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IBudgetkontoViewModel>();
                    mock.Expect(m => m.Kontonummer)
                        .Return(fixture.Create<string>())
                        .Repeat.Any();
                    mock.Expect(m => m.Kontogruppe)
                        .Return(fixture.Create<IBudgetkontogruppeViewModel>())
                        .Repeat.Any();
                    mock.Expect(m => m.Kontoværdi)
                        .Return(0M)
                        .Repeat.Any();
                    return mock;
                }));

            var budgetkontoViewModelMockCollection = fixture.CreateMany<IBudgetkontoViewModel>(250).ToList();

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Empty);
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Empty);

            budgetkontoViewModelMockCollection.ForEach(regnskabViewModel.BudgetkontoAdd);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Not.Empty);
            Assert.That(regnskabViewModel.Budgetkonti.Count(), Is.EqualTo(budgetkontoViewModelMockCollection.Count));
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Empty);
        }

        /// <summary>
        /// Tester, at getteren til BudgetkontiTop returnerer topbenyttede budgetkonti.
        /// </summary>
        [Test]
        public void TestAtBudgetkontiTopGetterReturnererTopbenyttedeBudgetkonti()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(fixture.Create<int>())
                        .Repeat.Any();
                    return mock;
                }));
            fixture.Customize<IBudgetkontoViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IBudgetkontoViewModel>();
                    mock.Expect(m => m.Kontonummer)
                        .Return(fixture.Create<string>())
                        .Repeat.Any();
                    mock.Expect(m => m.Kontogruppe)
                        .Return(fixture.Create<IBudgetkontogruppeViewModel>())
                        .Repeat.Any();
                    mock.Expect(m => m.Kontoværdi)
                        .Return(fixture.Create<decimal>())
                        .Repeat.Any();
                    return mock;
                }));

            var budgetkontoViewModelMockCollection = fixture.CreateMany<IBudgetkontoViewModel>(250).ToList();

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Empty);
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Empty);

            budgetkontoViewModelMockCollection.ForEach(regnskabViewModel.BudgetkontoAdd);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Not.Empty);
            Assert.That(regnskabViewModel.Budgetkonti.Count(), Is.EqualTo(budgetkontoViewModelMockCollection.Count));
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Not.Empty);
            Assert.That(regnskabViewModel.BudgetkontiTop.Count(), Is.EqualTo(25));
        }

        /// <summary>
        /// Tester, at getteren til BudgetkontiTopGroup returnerer grupperede topbenyttede budgetkonti.
        /// </summary>
        [Test]
        [TestCase(new[] {5, 6, 7, 8, 9})]
        public void TestAtBudgetkontiTopGroupedGetterReturnererDictionary(int[] budgetkontogrupper)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var budgetkontogruppeViewModelMockCollection = new List<IBudgetkontogruppeViewModel>(budgetkontogrupper.Length);
            foreach (var id in budgetkontogrupper)
            {
                var budgetkontogruppeViewModelMock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                budgetkontogruppeViewModelMock.Expect(m => m.Nummer)
                                              .Return(id)
                                              .Repeat.Any();
                budgetkontogruppeViewModelMockCollection.Add(budgetkontogruppeViewModelMock);
            }

            var budgetkontoViewModelMockCollection = CreateBudgetkontoViewModels(fixture, budgetkontogruppeViewModelMockCollection, new Random(DateTime.Now.Second), 250).ToList();

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Empty);
            Assert.That(regnskabViewModel.BudgetkontiTopGrouped, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiTopGrouped, Is.Empty);

            budgetkontogruppeViewModelMockCollection.ForEach(regnskabViewModel.BudgetkontogruppeAdd);
            budgetkontoViewModelMockCollection.ForEach(regnskabViewModel.BudgetkontoAdd);
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Not.Empty);

            var budgetkontiTopGrouped = regnskabViewModel.BudgetkontiTopGrouped;
            Assert.That(budgetkontiTopGrouped, Is.Not.Null);
            Assert.That(budgetkontiTopGrouped, Is.Not.Empty);
            Assert.That(budgetkontiTopGrouped.Sum(m => m.Value.Count()), Is.EqualTo(regnskabViewModel.BudgetkontiTop.Count()));
        }

        /// <summary>
        /// Tester, at KontoAdd kaster en ArgumentNullException, hvis ViewModel for kontoen er null.
        /// </summary>
        [Test]
        public void TestAtKontoAddKasterArgumentNullExceptionHvisKontoViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => regnskabViewModel.KontoAdd(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("kontoViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at KontoAdd tilføjer en konto til regnskabet.
        /// </summary>
        [Test]
        public void TestAtKontoAddAddsKontoViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IKontogruppeViewModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(fixture.Create<int>())
                        .Repeat.Any();
                    return mock;
                }));
            fixture.Customize<IKontoViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IKontoViewModel>();
                    mock.Expect(m => m.Kontonummer)
                        .Return(fixture.Create<string>())
                        .Repeat.Any();
                    mock.Expect(m => m.Kontogruppe)
                        .Return(fixture.Create<IKontogruppeViewModel>())
                        .Repeat.Any();
                    mock.Expect(m => m.Kontoværdi)
                        .Return(fixture.Create<decimal>())
                        .Repeat.Any();
                    return mock;
                }));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Konti, Is.Not.Null);
            Assert.That(regnskabViewModel.Konti, Is.Empty);
            Assert.That(regnskabViewModel.KontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.KontiTop, Is.Empty);

            regnskabViewModel.KontoAdd(fixture.Create<IKontoViewModel>());
            Assert.That(regnskabViewModel.Konti, Is.Not.Null);
            Assert.That(regnskabViewModel.Konti, Is.Not.Empty);
            Assert.That(regnskabViewModel.KontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.KontiTop, Is.Not.Empty);
        }

        /// <summary>
        /// Tester, at KontoAdd rejser PropertyChanged, når en konto tilføjes regnskabet.
        /// </summary>
        [Test]
        [TestCase("Konti")]
        [TestCase("KontiGrouped")]
        [TestCase("KontiTop")]
        [TestCase("KontiTopGrouped")]
        public void TestAtKontoAddRejserPropertyChangedVedAddAfKontoViewModel(string expectedPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeViewModel>()));
            fixture.Customize<IKontoViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontoViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
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
            regnskabViewModel.KontoAdd(fixture.Create<IKontoViewModel>());
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at BudgetkontoAdd kaster en ArgumentNullException, hvis ViewModel for budgetkontoen er null.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoAddKasterArgumentNullExceptionHvisBudgetkontoViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => regnskabViewModel.BudgetkontoAdd(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("budgetkontoViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at BudgetkontoAdd tilføjer en budgetkonto til regnskabet.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoAddAddsBudgetkontoViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(fixture.Create<int>())
                        .Repeat.Any();
                    return mock;
                }));
            fixture.Customize<IBudgetkontoViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IBudgetkontoViewModel>();
                    mock.Expect(m => m.Kontonummer)
                        .Return(fixture.Create<string>())
                        .Repeat.Any();
                    mock.Expect(m => m.Kontogruppe)
                        .Return(fixture.Create<IBudgetkontogruppeViewModel>())
                        .Repeat.Any();
                    mock.Expect(m => m.Kontoværdi)
                        .Return(fixture.Create<decimal>())
                        .Repeat.Any();
                    return mock;
                }));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Empty);
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Empty);

            regnskabViewModel.BudgetkontoAdd(fixture.Create<IBudgetkontoViewModel>());
            Assert.That(regnskabViewModel.Budgetkonti, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Not.Empty);
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Not.Empty);
        }

        /// <summary>
        /// Tester, at BudgetkontoAdd rejser PropertyChanged, når en budgetkonto tilføjes regnskabet.
        /// </summary>
        [Test]
        [TestCase("Budgetkonti")]
        [TestCase("BudgetkontiGrouped")]
        [TestCase("BudgetkontiTop")]
        [TestCase("BudgetkontiTopGrouped")]
        public void TestAtBudgetkontoAddRejserPropertyChangedVedAddAfBudgetkontoViewModel(string expectedPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontogruppeViewModel>()));
            fixture.Customize<IBudgetkontoViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontoViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
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
            regnskabViewModel.BudgetkontoAdd(fixture.Create<IBudgetkontoViewModel>());
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
        /// Tester, at KontogruppeAdd kaster en ArgumentNullException, hvis ViewModel for kontogruppen er null.
        /// </summary>
        [Test]
        public void TestAtKontogruppeAddKasterArgumentNullExceptionHvisKontogruppeViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => regnskabViewModel.KontogruppeAdd(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("kontogruppeViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at KontogruppeAdd tilføjer en kontogruppe til regnskabet.
        /// </summary>
        [Test]
        [TestCase(1)]
        public void TestAtKontogruppeAddAddsKontogruppeViewModel(int nummer)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IKontogruppeViewModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(nummer)
                        .Repeat.Any();
                    return mock;
                }));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Kontogrupper, Is.Not.Null);

            var kontogruppeViewModelMock = fixture.Create<IKontogruppeViewModel>();
            Assert.That(kontogruppeViewModelMock, Is.Not.Null);

            regnskabViewModel.KontogruppeAdd(kontogruppeViewModelMock);
            Assert.That(regnskabViewModel.Kontogrupper, Is.Not.Null);
            Assert.That(regnskabViewModel.Kontogrupper.Count(m => m.Nummer == nummer), Is.EqualTo(1));

            kontogruppeViewModelMock.AssertWasCalled(m => m.Nummer);
        }

        /// <summary>
        /// Tester, at KontogruppeAdd kun tilføjer en kontogruppe med samme nummer én gang.
        /// </summary>
        [Test]
        [TestCase(2)]
        public void TestAtKontogruppeAddOnlyAddsKontogruppeViewModelOnce(int nummer)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IKontogruppeViewModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(nummer)
                        .Repeat.Any();
                    return mock;
                }));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Kontogrupper, Is.Not.Null);

            regnskabViewModel.KontogruppeAdd(fixture.Create<IKontogruppeViewModel>());
            regnskabViewModel.KontogruppeAdd(fixture.Create<IKontogruppeViewModel>());
            regnskabViewModel.KontogruppeAdd(fixture.Create<IKontogruppeViewModel>());
            Assert.That(regnskabViewModel.Kontogrupper, Is.Not.Null);
            Assert.That(regnskabViewModel.Kontogrupper.Count(m => m.Nummer == nummer), Is.EqualTo(1));
        }

        /// <summary>
        /// Tester, at KontogruppeAdd rejser PropertyChanged, når en kontogruppe tilføjes regnskabet.
        /// </summary>
        [Test]
        [TestCase(3)]
        public void TestAtKontogruppeAddRejserPropertyChangedVedAddAfKontogruppeViewModel(int nummer)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IKontogruppeViewModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(nummer)
                        .Repeat.Any();
                    return mock;
                }));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, "Kontogrupper", StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            regnskabViewModel.KontogruppeAdd(fixture.Create<IKontogruppeViewModel>());
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at BudgetkontogruppeAdd kaster en ArgumentNullException, hvis ViewModel for kontogruppen til budgetkonti er null.
        /// </summary>
        [Test]
        public void TestAtBudgetkontogruppeAddKasterArgumentNullExceptionHvisBudgetkontogruppeViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => regnskabViewModel.BudgetkontogruppeAdd(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("budgetkontogruppeViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at BudgetkontogruppeAdd tilføjer en kontogruppe for budgetkonti til regnskabet.
        /// </summary>
        [Test]
        [TestCase(1)]
        public void TestAtBudgetkontogruppeAddAddsBudgetkontogruppeViewModel(int nummer)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(nummer)
                        .Repeat.Any();
                    return mock;
                }));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkontogrupper, Is.Not.Null);

            var budgetkontogruppeViewModelMock = fixture.Create<IBudgetkontogruppeViewModel>();
            Assert.That(budgetkontogruppeViewModelMock, Is.Not.Null);

            regnskabViewModel.BudgetkontogruppeAdd(budgetkontogruppeViewModelMock);
            Assert.That(regnskabViewModel.Budgetkontogrupper, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkontogrupper.Count(m => m.Nummer == nummer), Is.EqualTo(1));

            budgetkontogruppeViewModelMock.AssertWasCalled(m => m.Nummer);
        }

        /// <summary>
        /// Tester, at BudgetkontogruppeAdd kun tilføjer en kontogruppe til budgetkonti med samme nummer én gang.
        /// </summary>
        [Test]
        [TestCase(2)]
        public void TestAtBudgetkontogruppeAddAddOnlyAddsBudgetkontogruppeViewModelOnce(int nummer)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(nummer)
                        .Repeat.Any();
                    return mock;
                }));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkontogrupper, Is.Not.Null);

            regnskabViewModel.BudgetkontogruppeAdd(fixture.Create<IBudgetkontogruppeViewModel>());
            regnskabViewModel.BudgetkontogruppeAdd(fixture.Create<IBudgetkontogruppeViewModel>());
            regnskabViewModel.BudgetkontogruppeAdd(fixture.Create<IBudgetkontogruppeViewModel>());
            Assert.That(regnskabViewModel.Budgetkontogrupper, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkontogrupper.Count(m => m.Nummer == nummer), Is.EqualTo(1));
        }

        /// <summary>
        /// Tester, at BudgetkontogruppeAdd rejser PropertyChanged, når en kontogruppe til budgetkonti tilføjes regnskabet.
        /// </summary>
        [Test]
        [TestCase(3)]
        public void TestAtBudgetkontogruppeAddRejserPropertyChangedVedAddAfBudgetkontogruppeViewModel(int nummer)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(nummer)
                        .Repeat.Any();
                    return mock;
                }));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, "Budgetkontogrupper", StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            regnskabViewModel.BudgetkontogruppeAdd(fixture.Create<IBudgetkontogruppeViewModel>());
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
        /// Tester, at PropertyChangedOnKontoViewModelEventHandler rejser PropertyChanged, når ViewModel for en konto opdateres.
        /// </summary>
        [Test]
        [TestCase("Kontonummer", "Konti")]
        [TestCase("Kontonummer", "KontiGrouped")]
        [TestCase("Kontonummer", "KontiTop")]
        [TestCase("Kontonummer", "KontiTopGrouped")]
        [TestCase("Kontogruppe", "Konti")]
        [TestCase("Kontogruppe", "KontiGrouped")]
        [TestCase("Kontogruppe", "KontiTop")]
        [TestCase("Kontogruppe", "KontiTopGrouped")]
        [TestCase("Kontoværdi", "KontiTop")]
        [TestCase("Kontoværdi", "KontiTopGrouped")]
        public void TestAtPropertyChangedOnKontoViewModelEventHandlerRejserPropertyChangedOnKontoViewModelUpdate(string propertyName, string expectedPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeViewModel>()));
            fixture.Customize<IKontoViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontoViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var kontoViewModelMock = fixture.Create<IKontoViewModel>();
            regnskabViewModel.KontoAdd(kontoViewModelMock);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
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
            kontoViewModelMock.Raise(m => m.PropertyChanged += null, kontoViewModelMock, new PropertyChangedEventArgs(propertyName));
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnKontoViewModelEventHandler kaster en ArgumentNullException, hvis objektet, der rejser eventet, er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnKontoViewModelEventHandlerKasterArgumentNullExceptionHvisSenderErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeViewModel>()));
            fixture.Customize<IKontoViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontoViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var kontoViewModelMock = fixture.Create<IKontoViewModel>();
            regnskabViewModel.KontoAdd(kontoViewModelMock);

            var exception = Assert.Throws<ArgumentNullException>(() => kontoViewModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("sender"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnKontoViewModelEventHandler kaster en ArgumentNullException, hvis argumenter til eventet er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnKontoViewModelEventHandlerKasterArgumentNullExceptionHvisEventArgsErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeViewModel>()));
            fixture.Customize<IKontoViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontoViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var kontoViewModelMock = fixture.Create<IKontoViewModel>();
            regnskabViewModel.KontoAdd(kontoViewModelMock);

            var exception = Assert.Throws<ArgumentNullException>(() => kontoViewModelMock.Raise(m => m.PropertyChanged += null, kontoViewModelMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("e"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBudgetkontoViewModelEventHandler rejser PropertyChanged, når ViewModel for en budgetkonto opdateres.
        /// </summary>
        [Test]
        [TestCase("Kontonummer", "Budgetkonti")]
        [TestCase("Kontonummer", "BudgetkontiGrouped")]
        [TestCase("Kontonummer", "BudgetkontiTop")]
        [TestCase("Kontonummer", "BudgetkontiTopGrouped")]
        [TestCase("Kontogruppe", "Budgetkonti")]
        [TestCase("Kontogruppe", "BudgetkontiGrouped")]
        [TestCase("Kontogruppe", "BudgetkontiTop")]
        [TestCase("Kontogruppe", "BudgetkontiTopGrouped")]
        [TestCase("Kontoværdi", "BudgetkontiTop")]
        [TestCase("Kontoværdi", "BudgetkontiTopGrouped")]
        public void TestAtPropertyChangedOnBudgetkontoViewModelEventHandlerRejserPropertyChangedOnBudgetkontoViewModelUpdate(string propertyName, string expectedPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontogruppeViewModel>()));
            fixture.Customize<IBudgetkontoViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontoViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var budgetkontoViewModelMock = fixture.Create<IBudgetkontoViewModel>();
            regnskabViewModel.BudgetkontoAdd(budgetkontoViewModelMock);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
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
            budgetkontoViewModelMock.Raise(m => m.PropertyChanged += null, budgetkontoViewModelMock, new PropertyChangedEventArgs(propertyName));
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBudgetkontoViewModelEventHandler kaster en ArgumentNullException, hvis objektet, der rejser eventet, er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnBudgetkontoViewModelEventHandlerKasterArgumentNullExceptionHvisSenderErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontogruppeViewModel>()));
            fixture.Customize<IBudgetkontoViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontoViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var budgetkontoViewModelMock = fixture.Create<IBudgetkontoViewModel>();
            regnskabViewModel.BudgetkontoAdd(budgetkontoViewModelMock);

            var exception = Assert.Throws<ArgumentNullException>(() => budgetkontoViewModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("sender"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBudgetkontoViewModelEventHandler kaster en ArgumentNullException, hvis argumenter til eventet er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnBudgetkontoViewModelEventHandlerKasterArgumentNullExceptionHvisEventArgsErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontogruppeViewModel>()));
            fixture.Customize<IBudgetkontoViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontoViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var budgetkontoViewModelMock = fixture.Create<IBudgetkontoViewModel>();
            regnskabViewModel.BudgetkontoAdd(budgetkontoViewModelMock);

            var exception = Assert.Throws<ArgumentNullException>(() => budgetkontoViewModelMock.Raise(m => m.PropertyChanged += null, budgetkontoViewModelMock, null));
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
        [TestCase("Løbenummer", "Bogføringslinjer")]
        [TestCase("Dato", "Bogføringslinjer")]
        public void TestAtPropertyChangedOnBogføringslinjeViewModelEventHandlerRejserPropertyChangedOnBogføringslinjeViewModelUpdate(string propertyName, string expectedPropertyName)
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
                    if (string.Compare(e.PropertyName, expectedPropertyName, StringComparison.Ordinal) == 0)
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
        [TestCase("Navn", "Debitorer")]
        [TestCase("StatusDato", "Debitorer")]
        [TestCase("Saldo", "Debitorer")]
        public void TestAtPropertyChangedOnAdressekontoViewModelForDebitorEventHandlerRejserPropertyChangedOnAdressekontoViewModelUpdate(string propertyName, string expectedPropertyName)
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
                    if (string.Compare(e.PropertyName, expectedPropertyName, StringComparison.Ordinal) == 0)
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
        [TestCase("Navn", "Kreditorer")]
        [TestCase("StatusDato", "Kreditorer")]
        [TestCase("Saldo", "Kreditorer")]
        public void TestAtPropertyChangedOnAdressekontoViewModelForKreditorEventHandlerRejserPropertyChangedOnAdressekontoViewModelUpdate(string propertyName, string expectedPropertyName)
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
                    if (string.Compare(e.PropertyName, expectedPropertyName, StringComparison.Ordinal) == 0)
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
        [TestCase("Nyhedsudgivelsestidspunkt", "Nyheder")]
        [TestCase("Nyhedsaktualitet", "Nyheder")]
        public void TestAtPropertyChangedOnNyhedViewModelEventHandlerRejserPropertyChangedOnNyhedViewModelUpdate(string propertyName, string expectedPropertyName)
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
                    if (string.Compare(e.PropertyName, expectedPropertyName, StringComparison.Ordinal) == 0)
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

        /// <summary>
        /// Tester, at PropertyChangedOnKontogruppeViewModelEventHandler rejser PropertyChanged, når ViewModel for en kontogruppe opdateres.
        /// </summary>
        [Test]
        [TestCase(4, "Nummer", "Kontogrupper")]
        public void TestAtPropertyChangedOnKontogruppeViewModelEventHandlerRejserPropertyChangedOnKontogruppeViewModelUpdate(int nummer, string propertyName, string expectedPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IKontogruppeViewModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(nummer)
                        .Repeat.Any();
                    return mock;
                }));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var kontogruppeViewModelMock = fixture.Create<IKontogruppeViewModel>();
            regnskabViewModel.KontogruppeAdd(kontogruppeViewModelMock);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
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
            kontogruppeViewModelMock.Raise(m => m.PropertyChanged += null, kontogruppeViewModelMock, new PropertyChangedEventArgs(propertyName));
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnKontogruppeViewModelEventHandler kaster en ArgumentNullException, hvis objektet, der rejser eventet, er null.
        /// </summary>
        [Test]
        [TestCase(98)]
        public void TestAtPropertyChangedOnKontogruppeViewModelEventHandlerKasterArgumentNullExceptionHvisSenderErNull(int nummer)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IKontogruppeViewModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(nummer)
                        .Repeat.Any();
                    return mock;
                }));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var kontogruppeViewModelMock = fixture.Create<IKontogruppeViewModel>();
            regnskabViewModel.KontogruppeAdd(kontogruppeViewModelMock);

            var exception = Assert.Throws<ArgumentNullException>(() => kontogruppeViewModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("sender"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnKontogruppeViewModelEventHandler kaster en ArgumentNullException, hvis argumenter til eventet er null.
        /// </summary>
        [Test]
        [TestCase(99)]
        public void TestAtPropertyChangedOnKontogruppeViewModelEventHandlerKasterArgumentNullExceptionHvisEventArgsErNull(int nummer)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IKontogruppeViewModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(nummer)
                        .Repeat.Any();
                    return mock;
                }));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var kontogruppeViewModelMock = fixture.Create<IKontogruppeViewModel>();
            regnskabViewModel.KontogruppeAdd(kontogruppeViewModelMock);

            var exception = Assert.Throws<ArgumentNullException>(() => kontogruppeViewModelMock.Raise(m => m.PropertyChanged += null, kontogruppeViewModelMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("e"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBudgetkontogruppeViewModelEventHandler rejser PropertyChanged, når ViewModel for en kontogruppe til budgetkonti opdateres.
        /// </summary>
        [Test]
        [TestCase(4, "Nummer", "Budgetkontogrupper")]
        public void TestAtPropertyChangedOnBudgetkontogruppeViewModelEventHandlerRejserPropertyChangedOnBudgetkontogruppeViewModelUpdate(int nummer, string propertyName, string expectedPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(nummer)
                        .Repeat.Any();
                    return mock;
                }));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var budgetkontogruppeViewModelMock = fixture.Create<IBudgetkontogruppeViewModel>();
            regnskabViewModel.BudgetkontogruppeAdd(budgetkontogruppeViewModelMock);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
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
            budgetkontogruppeViewModelMock.Raise(m => m.PropertyChanged += null, budgetkontogruppeViewModelMock, new PropertyChangedEventArgs(propertyName));
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBudgetkontogruppeViewModelEventHandler kaster en ArgumentNullException, hvis objektet, der rejser eventet, er null.
        /// </summary>
        [Test]
        [TestCase(98)]
        public void TestAtPropertyChangedOnBudgetkontogruppeViewModelEventHandlerKasterArgumentNullExceptionHvisSenderErNull(int nummer)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(nummer)
                        .Repeat.Any();
                    return mock;
                }));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var budgetkontogruppeViewModelMock = fixture.Create<IBudgetkontogruppeViewModel>();
            regnskabViewModel.BudgetkontogruppeAdd(budgetkontogruppeViewModelMock);

            var exception = Assert.Throws<ArgumentNullException>(() => budgetkontogruppeViewModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("sender"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBudgetkontogruppeViewModelEventHandler kaster en ArgumentNullException, hvis argumenter til eventet er null.
        /// </summary>
        [Test]
        [TestCase(99)]
        public void TestAtPropertyChangedOnBudgetkontogruppeViewModelEventHandlerKasterArgumentNullExceptionHvisEventArgsErNull(int nummer)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(nummer)
                        .Repeat.Any();
                    return mock;
                }));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var budgetkontogruppeViewModelMock = fixture.Create<IBudgetkontogruppeViewModel>();
            regnskabViewModel.BudgetkontogruppeAdd(budgetkontogruppeViewModelMock);

            var exception = Assert.Throws<ArgumentNullException>(() => budgetkontogruppeViewModelMock.Raise(m => m.PropertyChanged += null, budgetkontogruppeViewModelMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("e"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Danner og returnerer en liste af konti.
        /// </summary>
        /// <returns>Liste af konti.</returns>
        private static IEnumerable<IKontoViewModel> CreateKontoViewModels(ISpecimenBuilder fixture, IEnumerable<IKontogruppeViewModel> kontogrupper, Random random, int count)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }
            if (kontogrupper == null)
            {
                throw new ArgumentNullException("kontogrupper");
            }
            if (random == null)
            {
                throw new ArgumentNullException("random");
            }
            var kontogruppeArray = kontogrupper.ToArray();
            var result = new List<IKontoViewModel>(count);
            while (result.Count < count)
            {
                var kontoViewModelMock = MockRepository.GenerateMock<IKontoViewModel>();
                kontoViewModelMock.Expect(m => m.Kontonummer)
                                  .Return(fixture.Create<string>())
                                  .Repeat.Any();
                kontoViewModelMock.Expect(m => m.Kontogruppe)
                                  .Return(kontogruppeArray.ElementAt(random.Next(kontogruppeArray.Length - 1)))
                                  .Repeat.Any();
                kontoViewModelMock.Expect(m => m.Kontoværdi)
                                  .Return(fixture.Create<decimal>())
                                  .Repeat.Any();
                result.Add(kontoViewModelMock);
            }
            return result;
        }

        /// <summary>
        /// Danner og returnerer en liste af budgetkonti.
        /// </summary>
        /// <returns>Liste af budgetkonti.</returns>
        private static IEnumerable<IBudgetkontoViewModel> CreateBudgetkontoViewModels(ISpecimenBuilder fixture, IEnumerable<IBudgetkontogruppeViewModel> budgetkontogrupper, Random random, int count)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }
            if (budgetkontogrupper == null)
            {
                throw new ArgumentNullException("budgetkontogrupper");
            }
            if (random == null)
            {
                throw new ArgumentNullException("random");
            }
            var budgetkontogrupperArray = budgetkontogrupper.ToArray();
            var result = new List<IBudgetkontoViewModel>(count);
            while (result.Count < count)
            {
                var budgetkontoViewModelMock = MockRepository.GenerateMock<IBudgetkontoViewModel>();
                budgetkontoViewModelMock.Expect(m => m.Kontonummer)
                                        .Return(fixture.Create<string>())
                                        .Repeat.Any();
                budgetkontoViewModelMock.Expect(m => m.Kontogruppe)
                                        .Return(budgetkontogrupperArray.ElementAt(random.Next(budgetkontogrupperArray.Length - 1)))
                                        .Repeat.Any();
                budgetkontoViewModelMock.Expect(m => m.Kontoværdi)
                                        .Return(fixture.Create<decimal>())
                                        .Repeat.Any();
                result.Add(budgetkontoViewModelMock);
            }
            return result;
        }
    }
}
