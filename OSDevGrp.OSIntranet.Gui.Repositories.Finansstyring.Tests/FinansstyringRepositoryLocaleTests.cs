﻿using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Schema;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Events;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.Resources;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Finansstyring.Tests
{
    /// <summary>
    /// Tester repository, der supporterer lokale data til finansstyring.
    /// </summary>
    [TestFixture]
    public class FinansstyringRepositoryLocaleTests
    {
        #region Private variables

        private static XDocument _localeDataDocument;
        private static readonly object SyncRoot = new object();

        #endregion

        /// <summary>
        /// Tester, at konstruktøren initierer repositoryet, der supporterer lokale data til finansstyring.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererFinansstyringRepositoryLocale()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));
            fixture.Customize<ILocaleDataStorage>(e => e.FromFactory(() => MockRepository.GenerateMock<ILocaleDataStorage>()));

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            var finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(finansstyringKonfigurationRepositoryMock, fixture.Create<ILocaleDataStorage>());
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);
            Assert.That(finansstyringRepositoryLocale.Konfiguration, Is.Not.Null);
            Assert.That(finansstyringRepositoryLocale.Konfiguration, Is.EqualTo(finansstyringKonfigurationRepositoryMock));
        }

        /// <summary>
        /// Tester, at konstruktøren initierer repositoryet, der indeholdende lokale data.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererFinansstyringRepositoryLocaleMedLokaleData()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            var localeDataStorageMock = MockRepository.GenerateMock<ILocaleDataStorage>();
            localeDataStorageMock.Stub(m => m.HasLocaleData)
                .Return(true)
                .Repeat.Any();

           var finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>(), localeDataStorageMock);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            localeDataStorageMock.AssertWasCalled(m => m.HasLocaleData, opt => opt.Repeat.Times(1));
            localeDataStorageMock.AssertWasNotCalled(m => m.StoreLocaleData(Arg<IRegnskabModel>.Is.Anything));
            localeDataStorageMock.AssertWasNotCalled(m => m.StoreLocaleData(Arg<IKontogruppeModel>.Is.Anything));
            localeDataStorageMock.AssertWasNotCalled(m => m.StoreLocaleData(Arg<IBudgetkontogruppeModel>.Is.Anything));
            localeDataStorageMock.AssertWasNotCalled(m => m.StoreLocaleData(Arg<IKontoModel>.Is.Anything));
            localeDataStorageMock.AssertWasNotCalled(m => m.StoreLocaleData(Arg<IBudgetkontoModel>.Is.Anything));
            localeDataStorageMock.AssertWasNotCalled(m => m.StoreLocaleData(Arg<IAdressekontoModel>.Is.Anything));
            localeDataStorageMock.AssertWasNotCalled(m => m.StoreLocaleData(Arg<IBogføringslinjeModel>.Is.Anything));
        }

        /// <summary>
        /// Tester, at konstruktøren initierer repositoryet, der ikke indeholdende lokale data.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererFinansstyringRepositoryLocaleUdenLokaleData()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            var localeDataStorageMock = MockRepository.GenerateMock<ILocaleDataStorage>();
            localeDataStorageMock.Stub(m => m.HasLocaleData)
                .Return(false)
                .Repeat.Any();

            var finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>(), localeDataStorageMock);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            localeDataStorageMock.AssertWasCalled(m => m.HasLocaleData, opt => opt.Repeat.Times(1));
            localeDataStorageMock.AssertWasCalled(m => m.StoreLocaleData(Arg<IRegnskabModel>.Is.NotNull), opt => opt.Repeat.Times(1));
            localeDataStorageMock.AssertWasCalled(m => m.StoreLocaleData(Arg<IKontogruppeModel>.Is.NotNull), opt => opt.Repeat.Times(3));
            localeDataStorageMock.AssertWasCalled(m => m.StoreLocaleData(Arg<IBudgetkontogruppeModel>.Is.NotNull), opt => opt.Repeat.Times(2));
            localeDataStorageMock.AssertWasCalled(m => m.StoreLocaleData(Arg<IKontoModel>.Is.NotNull), opt => opt.Repeat.Times(3 + 5 + 5));
            localeDataStorageMock.AssertWasCalled(m => m.StoreLocaleData(Arg<IBudgetkontoModel>.Is.NotNull), opt => opt.Repeat.Times(2 + 7));
            localeDataStorageMock.AssertWasNotCalled(m => m.StoreLocaleData(Arg<IAdressekontoModel>.Is.Anything));
            localeDataStorageMock.AssertWasNotCalled(m => m.StoreLocaleData(Arg<IBogføringslinjeModel>.Is.Anything));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis konfigurationsrepository, der supporterer finansstyring, er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFinansstyringKonfigurationRepositoryErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<ILocaleDataStorage>(e => e.FromFactory(() => MockRepository.GenerateMock<ILocaleDataStorage>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new FinansstyringRepositoryLocale(null, fixture.Create<ILocaleDataStorage>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("finansstyringKonfigurationRepository"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis det lokale datalager er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisLocaleDataStorageErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new FinansstyringRepositoryLocale(fixture.Create<IFinansstyringKonfigurationRepository>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("localeDataStorage"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at RegnskabslisteGetAsync henter regnskaber.
        /// </summary>
        [Test]
        public void TestAtRegnskabslisteGetAsyncHenterRegnskaber()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            var localeDataStorageMock = MockRepository.GenerateMock<ILocaleDataStorage>();
            localeDataStorageMock.Stub(m => m.HasLocaleData)
                .Return(true)
                .Repeat.Any();
            localeDataStorageMock.Stub(m => m.GetLocaleData())
                .Return(GenerateTestData(fixture))
                .Repeat.Any();

            var finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(fixture.Create<IFinansstyringKonfigurationRepository>(), localeDataStorageMock);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            var task = finansstyringRepositoryLocale.RegnskabslisteGetAsync();
            task.Wait();

            Assert.That(task.Result, Is.Not.Null);
            Assert.That(task.Result, Is.Not.Empty);
            Assert.That(task.Result.Count(), Is.EqualTo(3));

            localeDataStorageMock.AssertWasCalled(m => m.GetLocaleData());
        }

        /// <summary>
        /// Tester, at KontoplanGetAsync henter kontoplanen til et regnskab.
        /// </summary>
        [Test]
        [TestCase(1, 15)]
        [TestCase(2, 15)]
        [TestCase(3, 15)]
        [TestCase(4, 0)]
        [TestCase(5, 0)]
        public void TestAtKontoplanGetAsyncHenterKontoplan(int regnskabsnummer, int expectedKonti)
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            var localeDataStorageMock = MockRepository.GenerateMock<ILocaleDataStorage>();
            localeDataStorageMock.Stub(m => m.HasLocaleData)
                .Return(true)
                .Repeat.Any();
            localeDataStorageMock.Stub(m => m.GetLocaleData())
                .Return(GenerateTestData(fixture))
                .Repeat.Any();

            var finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(fixture.Create<IFinansstyringKonfigurationRepository>(), localeDataStorageMock);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            var task = finansstyringRepositoryLocale.KontoplanGetAsync(regnskabsnummer, DateTime.Now);
            task.Wait();

            Assert.That(task.Result, Is.Not.Null);
            Assert.That(task.Result.Count(), Is.EqualTo(expectedKonti));

            localeDataStorageMock.AssertWasCalled(m => m.GetLocaleData());
        }

        /// <summary>
        /// Tester, at KontoGetAsync kaster en ArgumentNullException, hvis kontonummeret er invalid.
        /// </summary>
        [Test]
        [TestCase(1, null)]
        [TestCase(1, "")]
        [TestCase(2, null)]
        [TestCase(2, "")]
        [TestCase(3, null)]
        [TestCase(3, "")]
        [TestCase(4, null)]
        [TestCase(4, "")]
        [TestCase(5, null)]
        [TestCase(5, "")]
        public void TestAtKontoGetAsyncKasterArgumentNullExceptionHvisKontonummerErInvalid(int regnskabsnummer, string invalidKontonummer)
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            var localeDataStorageMock = MockRepository.GenerateMock<ILocaleDataStorage>();
            localeDataStorageMock.Stub(m => m.HasLocaleData)
                .Return(true)
                .Repeat.Any();

            var finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(fixture.Create<IFinansstyringKonfigurationRepository>(), localeDataStorageMock);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => finansstyringRepositoryLocale.KontoGetAsync(regnskabsnummer, invalidKontonummer, DateTime.Now).Wait());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("kontonummer"));
            Assert.That(exception.InnerException, Is.Null);

            localeDataStorageMock.AssertWasNotCalled(m => m.GetLocaleData());
        }

        /// <summary>
        /// Tester, at KontoGetAsync kaster en IntranetGuiRepositoryException, hvis kontonummeret ikke findes.
        /// </summary>
        [Test]
        [TestCase(1, "XXX")]
        [TestCase(2, "YYY")]
        [TestCase(3, "ZZZ")]
        [TestCase(4, "KONTO01")]
        [TestCase(5, "KONTO02")]
        public void TestAtKontoGetAsyncKasterIntranetGuiRepositoryExceptionHvisKontonummerIkkeFindes(int regnskabsnummer, string unknownKontonummer)
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            var localeDataStorageMock = MockRepository.GenerateMock<ILocaleDataStorage>();
            localeDataStorageMock.Stub(m => m.HasLocaleData)
                .Return(true)
                .Repeat.Any();
            localeDataStorageMock.Stub(m => m.GetLocaleData())
                .Return(GenerateTestData(fixture))
                .Repeat.Any();

            var finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(fixture.Create<IFinansstyringKonfigurationRepository>(), localeDataStorageMock);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            IntranetGuiRepositoryException exception = null;
            var aggregateException = Assert.Throws<AggregateException>(() => finansstyringRepositoryLocale.KontoGetAsync(regnskabsnummer, unknownKontonummer, DateTime.Now).Wait());
            aggregateException.Handle(e =>
            {
                exception = e as IntranetGuiRepositoryException;
                return true;
            });

            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.AccountNotFound, unknownKontonummer)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<InvalidOperationException>());

            localeDataStorageMock.AssertWasCalled(m => m.GetLocaleData());
        }

        /// <summary>
        /// Tester, at KontoGetAsync henter en given konto.
        /// </summary>
        [Test]
        [TestCase(1, "KONTO01")]
        [TestCase(1, "KONTO02")]
        [TestCase(1, "KONTO03")]
        [TestCase(2, "KONTO01")]
        [TestCase(2, "KONTO02")]
        [TestCase(2, "KONTO03")]
        [TestCase(3, "KONTO01")]
        [TestCase(3, "KONTO02")]
        [TestCase(3, "KONTO03")]
        public void TestAtKontoGetAsyncHenterKontoModel(int regnskabsnummer, string kontonummer)
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            var localeDataStorageMock = MockRepository.GenerateMock<ILocaleDataStorage>();
            localeDataStorageMock.Stub(m => m.HasLocaleData)
                .Return(true)
                .Repeat.Any();
            localeDataStorageMock.Stub(m => m.GetLocaleData())
                .Return(GenerateTestData(fixture))
                .Repeat.Any();

            var finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(fixture.Create<IFinansstyringKonfigurationRepository>(), localeDataStorageMock);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            var task = finansstyringRepositoryLocale.KontoGetAsync(regnskabsnummer, kontonummer, DateTime.Now);
            task.Wait();

            var kontoModel = task.Result;
            Assert.That(kontoModel, Is.Not.Null);
            Assert.That(kontoModel.Regnskabsnummer, Is.EqualTo(regnskabsnummer));
            Assert.That(kontoModel.Kontonummer, Is.Not.Null);
            Assert.That(kontoModel.Kontonummer, Is.Not.Empty);
            Assert.That(kontoModel.Kontonummer, Is.EqualTo(kontonummer));

            localeDataStorageMock.AssertWasCalled(m => m.GetLocaleData());
        }

        /// <summary>
        /// Tester, at BudgetkontoplanGetAsync henter budgetkontoplanen til et regnskab.
        /// </summary>
        [Test]
        [TestCase(1, 30)]
        [TestCase(2, 30)]
        [TestCase(3, 30)]
        [TestCase(4, 0)]
        [TestCase(5, 0)]
        public void TestAtBudgetkontoplanGetAsyncHenterBudgetkontoplan(int regnskabsnummer, int expectedBudgetkonti)
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            var localeDataStorageMock = MockRepository.GenerateMock<ILocaleDataStorage>();
            localeDataStorageMock.Stub(m => m.HasLocaleData)
                .Return(true)
                .Repeat.Any();
            localeDataStorageMock.Stub(m => m.GetLocaleData())
                .Return(GenerateTestData(fixture))
                .Repeat.Any();

            var finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(fixture.Create<IFinansstyringKonfigurationRepository>(), localeDataStorageMock);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            var task = finansstyringRepositoryLocale.BudgetkontoplanGetAsync(regnskabsnummer, DateTime.Now);
            task.Wait();

            Assert.That(task.Result, Is.Not.Null);
            Assert.That(task.Result.Count(), Is.EqualTo(expectedBudgetkonti));

            localeDataStorageMock.AssertWasCalled(m => m.GetLocaleData());
        }

        /// <summary>
        /// Tester, at BudgetkontoGetAsync kaster en ArgumentNullException, hvis budgetkontonummeret er invalid.
        /// </summary>
        [Test]
        [TestCase(1, null)]
        [TestCase(1, "")]
        [TestCase(2, null)]
        [TestCase(2, "")]
        [TestCase(3, null)]
        [TestCase(3, "")]
        [TestCase(4, null)]
        [TestCase(4, "")]
        [TestCase(5, null)]
        [TestCase(5, "")]
        public void TestAtBudgetkontoGetAsyncKasterArgumentNullExceptionHvisBudgetkontonummerErInvalid(int regnskabsnummer, string invalidBudgetkontonummer)
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            var localeDataStorageMock = MockRepository.GenerateMock<ILocaleDataStorage>();
            localeDataStorageMock.Stub(m => m.HasLocaleData)
                .Return(true)
                .Repeat.Any();

            var finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(fixture.Create<IFinansstyringKonfigurationRepository>(), localeDataStorageMock);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => finansstyringRepositoryLocale.BudgetkontoGetAsync(regnskabsnummer, invalidBudgetkontonummer, DateTime.Now).Wait());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("budgetkontonummer"));
            Assert.That(exception.InnerException, Is.Null);

            localeDataStorageMock.AssertWasNotCalled(m => m.GetLocaleData());
        }

        /// <summary>
        /// Tester, at BudgetkontoGetAsync kaster en IntranetGuiRepositoryException, hvis budgetkontonummeret ikke findes.
        /// </summary>
        [Test]
        [TestCase(1, "XXX")]
        [TestCase(2, "YYY")]
        [TestCase(3, "ZZZ")]
        [TestCase(4, "BUDGETKONTO01")]
        [TestCase(5, "BUDGETKONTO02")]
        public void TestAtBudgetkontoGetAsyncKasterIntranetGuiRepositoryExceptionHvisBudgetkontonummerIkkeFindes(int regnskabsnummer, string unknownBudgetkontonummer)
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            var localeDataStorageMock = MockRepository.GenerateMock<ILocaleDataStorage>();
            localeDataStorageMock.Stub(m => m.HasLocaleData)
                .Return(true)
                .Repeat.Any();
            localeDataStorageMock.Stub(m => m.GetLocaleData())
                .Return(GenerateTestData(fixture))
                .Repeat.Any();

            var finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(fixture.Create<IFinansstyringKonfigurationRepository>(), localeDataStorageMock);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            IntranetGuiRepositoryException exception = null;
            var aggregateException = Assert.Throws<AggregateException>(() => finansstyringRepositoryLocale.BudgetkontoGetAsync(regnskabsnummer, unknownBudgetkontonummer, DateTime.Now).Wait());
            aggregateException.Handle(e =>
            {
                exception = e as IntranetGuiRepositoryException;
                return true;
            });

            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.BudgetAccountNotFound, unknownBudgetkontonummer)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<InvalidOperationException>());

            localeDataStorageMock.AssertWasCalled(m => m.GetLocaleData());
        }

        /// <summary>
        /// Tester, at BudgetkontoGetAsync henter en given budgetkonto.
        /// </summary>
        [Test]
        [TestCase(1, "BUDGETKONTO01")]
        [TestCase(1, "BUDGETKONTO02")]
        [TestCase(1, "BUDGETKONTO03")]
        [TestCase(2, "BUDGETKONTO01")]
        [TestCase(2, "BUDGETKONTO02")]
        [TestCase(2, "BUDGETKONTO03")]
        [TestCase(3, "BUDGETKONTO01")]
        [TestCase(3, "BUDGETKONTO02")]
        [TestCase(3, "BUDGETKONTO03")]
        public void TestAtBudgetkontoGetAsyncHenterBudgetkontoModel(int regnskabsnummer, string budgetkontonummer)
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            var localeDataStorageMock = MockRepository.GenerateMock<ILocaleDataStorage>();
            localeDataStorageMock.Stub(m => m.HasLocaleData)
                .Return(true)
                .Repeat.Any();
            localeDataStorageMock.Stub(m => m.GetLocaleData())
                .Return(GenerateTestData(fixture))
                .Repeat.Any();

            var finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(fixture.Create<IFinansstyringKonfigurationRepository>(), localeDataStorageMock);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            var task = finansstyringRepositoryLocale.BudgetkontoGetAsync(regnskabsnummer, budgetkontonummer, DateTime.Now);
            task.Wait();

            var budgetkontoModel = task.Result;
            Assert.That(budgetkontoModel, Is.Not.Null);
            Assert.That(budgetkontoModel.Regnskabsnummer, Is.EqualTo(regnskabsnummer));
            Assert.That(budgetkontoModel.Kontonummer, Is.Not.Null);
            Assert.That(budgetkontoModel.Kontonummer, Is.Not.Empty);
            Assert.That(budgetkontoModel.Kontonummer, Is.EqualTo(budgetkontonummer));

            localeDataStorageMock.AssertWasCalled(m => m.GetLocaleData());
        }

        /// <summary>
        /// Tester, at BogføringslinjerGetAsync henter bogføringslinjer til et regnskab.
        /// </summary>
        [Test]
        [TestCase(1, 50)]
        [TestCase(2, 50)]
        [TestCase(3, 50)]
        [TestCase(4, 0)]
        [TestCase(5, 0)]
        public void TestAtBogføringslinjerGetAsyncHenterBogføringslinjer(int regnskabsnummer, int expectedBogføringslinjer)
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            var localeDataStorageMock = MockRepository.GenerateMock<ILocaleDataStorage>();
            localeDataStorageMock.Stub(m => m.HasLocaleData)
                .Return(true)
                .Repeat.Any();
            localeDataStorageMock.Stub(m => m.GetLocaleData())
                .Return(GenerateTestData(fixture))
                .Repeat.Any();

            var finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(fixture.Create<IFinansstyringKonfigurationRepository>(), localeDataStorageMock);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            var task = finansstyringRepositoryLocale.BogføringslinjerGetAsync(regnskabsnummer, DateTime.Now, expectedBogføringslinjer);
            task.Wait();

            Assert.That(task.Result, Is.Not.Null);
            Assert.That(task.Result.Count(), Is.EqualTo(expectedBogføringslinjer));

            localeDataStorageMock.AssertWasCalled(m => m.GetLocaleData());
        }

        /// <summary>
        /// Tester, at BogføringslinjeCreateNewAsync kaster en ArgumentException ved illegale regnskabsnumre.
        /// </summary>
        [Test]
        [TestCase(-1024)]
        [TestCase(-1)]
        [TestCase(0)]
        public void TestAtBogføringslinjeCreateNewAsyncArgumentExceptionVedIllegalRegnskabsnummer(int illegalValue)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Today));
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            var localeDataStorageMock = MockRepository.GenerateMock<ILocaleDataStorage>();
            localeDataStorageMock.Stub(m => m.HasLocaleData)
                .Return(true)
                .Repeat.Any();

            var finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(fixture.Create<IFinansstyringKonfigurationRepository>(), localeDataStorageMock);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            ArgumentException exception = null;
            var aggregateException = Assert.Throws<AggregateException>(() => finansstyringRepositoryLocale.BogføringslinjeCreateNewAsync(illegalValue, fixture.Create<DateTime>(), fixture.Create<string>()).Wait());
            aggregateException.Handle(e =>
            {
                exception = e as ArgumentException;
                return true;
            });

            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Does.StartWith(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "regnskabsnummer", illegalValue)));
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("regnskabsnummer"));
            Assert.That(exception.InnerException, Is.Null);

            localeDataStorageMock.AssertWasNotCalled(m => m.GetLocaleData());
        }

        /// <summary>
        /// Tester, at BogføringslinjeCreateNewAsync kaster en ArgumentException ved illegale datoer.
        /// </summary>
        [Test]
        [TestCase("2050-01-01")]
        [TestCase("2055-09-01")]
        [TestCase("2055-12-31")]
        public void TestAtBogføringslinjeCreateNewAsyncArgumentExceptionVedIllegalDato(string illegalValue)
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            var localeDataStorageMock = MockRepository.GenerateMock<ILocaleDataStorage>();
            localeDataStorageMock.Stub(m => m.HasLocaleData)
                .Return(true)
                .Repeat.Any();

            var finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(fixture.Create<IFinansstyringKonfigurationRepository>(), localeDataStorageMock);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            var dato = Convert.ToDateTime(illegalValue, new CultureInfo("en-US"));

            ArgumentException exception = null;
            var aggregateException = Assert.Throws<AggregateException>(() => finansstyringRepositoryLocale.BogføringslinjeCreateNewAsync(fixture.Create<int>(), dato, fixture.Create<string>()).Wait());
            aggregateException.Handle(e =>
            {
                exception = e as ArgumentException;
                return true;
            });

            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Does.StartWith(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "dato", dato)));
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("dato"));
            Assert.That(exception.InnerException, Is.Null);

            localeDataStorageMock.AssertWasNotCalled(m => m.GetLocaleData());
        }

        /// <summary>
        /// Tester, at BogføringslinjeCreateNewAsync kaster en ArgumentNullException ved illegale kontonumre.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestAtBogføringslinjeCreateNewAsyncArgumentNullExceptionVedIllegalKontonummer(string illegalValue)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Today));
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            var localeDataStorageMock = MockRepository.GenerateMock<ILocaleDataStorage>();
            localeDataStorageMock.Stub(m => m.HasLocaleData)
                .Return(true)
                .Repeat.Any();

            var finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(fixture.Create<IFinansstyringKonfigurationRepository>(), localeDataStorageMock);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            ArgumentNullException exception = null;
            var aggregateException = Assert.Throws<AggregateException>(() => finansstyringRepositoryLocale.BogføringslinjeCreateNewAsync(fixture.Create<int>(), fixture.Create<DateTime>(), illegalValue).Wait());
            aggregateException.Handle(e =>
            {
                exception = e as ArgumentNullException;
                return true;
            });

            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("kontonummer"));
            Assert.That(exception.InnerException, Is.Null);

            localeDataStorageMock.AssertWasNotCalled(m => m.GetLocaleData());
        }

        /// <summary>
        /// Tester, at BogføringslinjeCreateNewAsync danner og returnerer en ny bogføringslinje, der efterfølgende kan bogføres.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjeCreateNewAsyncDannerBogføringslinjeModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Today));
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            var localeDataStorageMock = MockRepository.GenerateMock<ILocaleDataStorage>();
            localeDataStorageMock.Stub(m => m.HasLocaleData)
                .Return(true)
                .Repeat.Any();

            var finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(fixture.Create<IFinansstyringKonfigurationRepository>(), localeDataStorageMock);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            var regnskabsnummer = fixture.Create<int>();
            var dato = fixture.Create<DateTime>();
            var kontonummer = fixture.Create<string>();

            var task = finansstyringRepositoryLocale.BogføringslinjeCreateNewAsync(regnskabsnummer, dato, kontonummer);
            task.Wait();

            var bogføringslinjeModel = task.Result;
            Assert.That(bogføringslinjeModel, Is.Not.Null);
            Assert.That(bogføringslinjeModel.Regnskabsnummer, Is.EqualTo(regnskabsnummer));
            Assert.That(bogføringslinjeModel.Løbenummer, Is.EqualTo(int.MinValue));
            Assert.That(bogføringslinjeModel.Dato, Is.EqualTo(dato));
            Assert.That(bogføringslinjeModel.Bilag, Is.Null);
            Assert.That(bogføringslinjeModel.Kontonummer, Is.Not.Null);
            Assert.That(bogføringslinjeModel.Kontonummer, Is.Not.Empty);
            Assert.That(bogføringslinjeModel.Kontonummer, Is.EqualTo(kontonummer));
            Assert.That(bogføringslinjeModel.Tekst, Is.Not.Null);
            Assert.That(bogføringslinjeModel.Tekst, Is.Empty);
            Assert.That(bogføringslinjeModel.Budgetkontonummer, Is.Null);
            Assert.That(bogføringslinjeModel.Debit, Is.EqualTo(0M));
            Assert.That(bogføringslinjeModel.Kredit, Is.EqualTo(0M));

            localeDataStorageMock.AssertWasNotCalled(m => m.GetLocaleData());
        }

        /// <summary>
        /// Tester, at BogførAsync kaster en ArgumentException ved illegale regnskabsnumre.
        /// </summary>
        [Test]
        [TestCase(-1024)]
        [TestCase(-1)]
        [TestCase(0)]
        public void TestAtBogførAsyncKasterArgumentExceptionVedIllegalRegnskabsnummer(int illegalValue)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Today));
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            var localeDataStorageMock = MockRepository.GenerateMock<ILocaleDataStorage>();
            localeDataStorageMock.Stub(m => m.HasLocaleData)
                .Return(true)
                .Repeat.Any();

            var finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(fixture.Create<IFinansstyringKonfigurationRepository>(), localeDataStorageMock);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            var exception = Assert.Throws<ArgumentException>(() => finansstyringRepositoryLocale.BogførAsync(illegalValue, fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), fixture.Create<decimal>(), fixture.Create<int>()).Wait());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Does.StartWith(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "regnskabsnummer", illegalValue)));
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("regnskabsnummer"));
            Assert.That(exception.InnerException, Is.Null);

            localeDataStorageMock.AssertWasNotCalled(m => m.GetLocaleData());
        }

        /// <summary>
        /// Tester, at BogførAsync kaster en ArgumentException ved illegale datoer.
        /// </summary>
        [Test]
        [TestCase("2050-01-01")]
        [TestCase("2055-09-01")]
        [TestCase("2055-12-31")]
        public void TestAtBogførAsyncKasterArgumentExceptionVedIllegalDato(string illegalValue)
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            var localeDataStorageMock = MockRepository.GenerateMock<ILocaleDataStorage>();
            localeDataStorageMock.Stub(m => m.HasLocaleData)
                .Return(true)
                .Repeat.Any();

            var finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(fixture.Create<IFinansstyringKonfigurationRepository>(), localeDataStorageMock);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            var dato = Convert.ToDateTime(illegalValue, new CultureInfo("en-US"));
            var exception = Assert.Throws<ArgumentException>(() => finansstyringRepositoryLocale.BogførAsync(fixture.Create<int>(), dato, fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), fixture.Create<decimal>(), fixture.Create<int>()).Wait());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Does.StartWith(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "dato", dato)));
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("dato"));
            Assert.That(exception.InnerException, Is.Null);

            localeDataStorageMock.AssertWasNotCalled(m => m.GetLocaleData());
        }

        /// <summary>
        /// Tester, at BogførAsync kaster en ArgumentNullException ved illegale kontonumre.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestAtBogførAsyncKasterArgumentNullExceptionVedIllegalKontonummer(string illegalValue)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Today));
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            var localeDataStorageMock = MockRepository.GenerateMock<ILocaleDataStorage>();
            localeDataStorageMock.Stub(m => m.HasLocaleData)
                .Return(true)
                .Repeat.Any();

            var finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(fixture.Create<IFinansstyringKonfigurationRepository>(), localeDataStorageMock);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => finansstyringRepositoryLocale.BogførAsync(fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), illegalValue, fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), fixture.Create<decimal>(), fixture.Create<int>()).Wait());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("kontonummer"));
            Assert.That(exception.InnerException, Is.Null);

            localeDataStorageMock.AssertWasNotCalled(m => m.GetLocaleData());
        }

        /// <summary>
        /// Tester, at BogførAsync kaster en ArgumentNullException ved illegale tekster.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestAtBogførAsyncKasterArgumentNullExceptionVedIllegalTekst(string illegalValue)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Today));
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            var localeDataStorageMock = MockRepository.GenerateMock<ILocaleDataStorage>();
            localeDataStorageMock.Stub(m => m.HasLocaleData)
                .Return(true)
                .Repeat.Any();

            var finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(fixture.Create<IFinansstyringKonfigurationRepository>(), localeDataStorageMock);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => finansstyringRepositoryLocale.BogførAsync(fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), illegalValue, fixture.Create<string>(), fixture.Create<decimal>(), fixture.Create<decimal>(), fixture.Create<int>()).Wait());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("tekst"));
            Assert.That(exception.InnerException, Is.Null);

            localeDataStorageMock.AssertWasNotCalled(m => m.GetLocaleData());
        }

        /// <summary>
        /// Tester, at BogførAsync kaster en ArgumentException ved illegale debitbeløb.
        /// </summary>
        [Test]
        [TestCase(-1024)]
        [TestCase(-1)]
        public void TestAtBogførAsyncKasterArgumentExceptionVedIllegalDebit(decimal illegalValue)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Today));
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            var localeDataStorageMock = MockRepository.GenerateMock<ILocaleDataStorage>();
            localeDataStorageMock.Stub(m => m.HasLocaleData)
                .Return(true)
                .Repeat.Any();

            var finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(fixture.Create<IFinansstyringKonfigurationRepository>(), localeDataStorageMock);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            var exception = Assert.Throws<ArgumentException>(() => finansstyringRepositoryLocale.BogførAsync(fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>(), illegalValue, fixture.Create<decimal>(), fixture.Create<int>()).Wait());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Does.StartWith(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "debit", illegalValue)));
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("debit"));
            Assert.That(exception.InnerException, Is.Null);

            localeDataStorageMock.AssertWasNotCalled(m => m.GetLocaleData());
        }

        /// <summary>
        /// Tester, at BogførAsync kaster en ArgumentException ved illegale kreditbeløb.
        /// </summary>
        [Test]
        [TestCase(-1024)]
        [TestCase(-1)]
        public void TestAtBogførAsyncKasterArgumentExceptionVedIllegalKredit(decimal illegalValue)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Today));
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            var localeDataStorageMock = MockRepository.GenerateMock<ILocaleDataStorage>();
            localeDataStorageMock.Stub(m => m.HasLocaleData)
                .Return(true)
                .Repeat.Any();

            var finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(fixture.Create<IFinansstyringKonfigurationRepository>(), localeDataStorageMock);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            var exception = Assert.Throws<ArgumentException>(() => finansstyringRepositoryLocale.BogførAsync(fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), illegalValue, fixture.Create<int>()).Wait());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Does.StartWith(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "kredit", illegalValue)));
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("kredit"));
            Assert.That(exception.InnerException, Is.Null);

            localeDataStorageMock.AssertWasNotCalled(m => m.GetLocaleData());
        }

        /// <summary>
        /// Tester, at BogførAsync bogfører værdier.
        /// </summary>
        [Test]
        public void TestAtBogførAsyncBogførerValues()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            var localData = GenerateTestData(fixture);
            var localeDataStorageMock = MockRepository.GenerateMock<ILocaleDataStorage>();
            localeDataStorageMock.Stub(m => m.HasLocaleData)
                .Return(true)
                .Repeat.Any();
            localeDataStorageMock.Stub(m => m.GetLocaleData())
                .Return(localData)
                .Repeat.Any();

            var finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(fixture.Create<IFinansstyringKonfigurationRepository>(), localeDataStorageMock);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            var task = finansstyringRepositoryLocale.BogførAsync(1, DateTime.Now.AddDays(-3), null, "KONTO01", "Test", "BUDGETKONTO01", 5000M, 0M, 101);
            task.Wait();

            var bogføringsresultat = task.Result;
            Assert.That(bogføringsresultat, Is.Not.Null);
            Assert.That(bogføringsresultat.Bogføringslinje, Is.Not.Null);
            Assert.That(bogføringsresultat.Bogføringsadvarsler, Is.Not.Null);

            localeDataStorageMock.AssertWasCalled(m => m.GetLocaleData());
            localeDataStorageMock.AssertWasCalled(m => m.StoreLocaleDocument(Arg<XDocument>.Is.Equal(localData)));
        }

        /// <summary>
        /// Tester, at DebitorlisteGetAsync henter listen af debitorer i et regnskab.
        /// </summary>
        [Test]
        [TestCase(1, 25)]
        [TestCase(2, 25)]
        [TestCase(3, 25)]
        [TestCase(4, 0)]
        [TestCase(5, 0)]
        public void TestAtDebitorlisteGetAsyncHenterDebitorer(int regnskabsnummer, int expectedDebitorer)
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            var localeDataStorageMock = MockRepository.GenerateMock<ILocaleDataStorage>();
            localeDataStorageMock.Stub(m => m.HasLocaleData)
                .Return(true)
                .Repeat.Any();
            localeDataStorageMock.Stub(m => m.GetLocaleData())
                .Return(GenerateTestData(fixture))
                .Repeat.Any();

            var finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(fixture.Create<IFinansstyringKonfigurationRepository>(), localeDataStorageMock);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            var task = finansstyringRepositoryLocale.DebitorlisteGetAsync(regnskabsnummer, DateTime.Now);
            task.Wait();

            Assert.That(task.Result, Is.Not.Null);
            Assert.That(task.Result.Count(), Is.EqualTo(expectedDebitorer));

            localeDataStorageMock.AssertWasCalled(m => m.GetLocaleData());
        }

        /// <summary>
        /// Tester, at KreditorlisteGetAsync henter listen af kreditorer i et regnskab.
        /// </summary>
        [Test]
        [TestCase(1, 15)]
        [TestCase(2, 15)]
        [TestCase(3, 15)]
        [TestCase(4, 0)]
        [TestCase(5, 0)]
        public void TestAtKreditorlisteGetAsyncHenterKreditorer(int regnskabsnummer, int expectedKreditorer)
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            var localeDataStorageMock = MockRepository.GenerateMock<ILocaleDataStorage>();
            localeDataStorageMock.Stub(m => m.HasLocaleData)
                .Return(true)
                .Repeat.Any();
            localeDataStorageMock.Stub(m => m.GetLocaleData())
                .Return(GenerateTestData(fixture))
                .Repeat.Any();

            var finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(fixture.Create<IFinansstyringKonfigurationRepository>(), localeDataStorageMock);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            var task = finansstyringRepositoryLocale.KreditorlisteGetAsync(regnskabsnummer, DateTime.Now);
            task.Wait();

            Assert.That(task.Result, Is.Not.Null);
            Assert.That(task.Result.Count(), Is.EqualTo(expectedKreditorer));

            localeDataStorageMock.AssertWasCalled(m => m.GetLocaleData());
        }

        /// <summary>
        /// Tester, at AdressekontolisteGetAsync henter adressekonti til et regnskab.
        /// </summary>
        [Test]
        [TestCase(1, 40)]
        [TestCase(2, 40)]
        [TestCase(3, 40)]
        [TestCase(4, 0)]
        [TestCase(5, 0)]
        public void TestAtAdressekontolisteGetAsyncHenterAdressekonti(int regnskabsnummer, int expectedAdressekonti)
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            var localeDataStorageMock = MockRepository.GenerateMock<ILocaleDataStorage>();
            localeDataStorageMock.Stub(m => m.HasLocaleData)
                .Return(true)
                .Repeat.Any();
            localeDataStorageMock.Stub(m => m.GetLocaleData())
                .Return(GenerateTestData(fixture))
                .Repeat.Any();

            var finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(fixture.Create<IFinansstyringKonfigurationRepository>(), localeDataStorageMock);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            var task = finansstyringRepositoryLocale.AdressekontolisteGetAsync(regnskabsnummer, DateTime.Now);
            task.Wait();

            Assert.That(task.Result, Is.Not.Null);
            Assert.That(task.Result.Count(), Is.EqualTo(expectedAdressekonti));

            localeDataStorageMock.AssertWasCalled(m => m.GetLocaleData());
        }

        /// <summary>
        /// Tester, at AdressekontoGetAsync kaster en IntranetGuiRepositoryException, hvis adressekontoen ikke findes.
        /// </summary>
        [Test]
        [TestCase(1, 777)]
        [TestCase(2, 888)]
        [TestCase(3, 999)]
        [TestCase(4, 101)]
        [TestCase(5, 102)]
        public void TestAtAdressekontoGetAsyncKasterIntranetGuiRepositoryExceptionHvisAdressekontoIkkeFindes(int regnskabsnummer, int unknownAdressekonto)
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            var localeDataStorageMock = MockRepository.GenerateMock<ILocaleDataStorage>();
            localeDataStorageMock.Stub(m => m.HasLocaleData)
                .Return(true)
                .Repeat.Any();
            localeDataStorageMock.Stub(m => m.GetLocaleData())
                .Return(GenerateTestData(fixture))
                .Repeat.Any();

            var finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(fixture.Create<IFinansstyringKonfigurationRepository>(), localeDataStorageMock);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            Exception exception = null;
            var aggregateException = Assert.Throws<AggregateException>(() => finansstyringRepositoryLocale.AdressekontoGetAsync(regnskabsnummer, unknownAdressekonto, DateTime.Now).Wait());
            aggregateException.Handle(e =>
            {
                exception = e;
                return true;
            });

            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.AddressAccountNotFound, unknownAdressekonto)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<InvalidOperationException>());

            localeDataStorageMock.AssertWasCalled(m => m.GetLocaleData());
        }

        /// <summary>
        /// Tester, at AdressekontoGetAsync henter en given adressekonto.
        /// </summary>
        [Test]
        [TestCase(1, 101)]
        [TestCase(1, 102)]
        [TestCase(1, 103)]
        [TestCase(1, 201)]
        [TestCase(1, 202)]
        [TestCase(1, 203)]
        [TestCase(2, 101)]
        [TestCase(2, 102)]
        [TestCase(2, 103)]
        [TestCase(2, 201)]
        [TestCase(2, 202)]
        [TestCase(2, 203)]
        [TestCase(3, 101)]
        [TestCase(3, 102)]
        [TestCase(3, 103)]
        [TestCase(3, 201)]
        [TestCase(3, 202)]
        [TestCase(3, 203)]
        public void TestAtAdressekontoGetAsyncHenterAdressekontoModel(int regnskabsnummer, int adressekonto)
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            var localeDataStorageMock = MockRepository.GenerateMock<ILocaleDataStorage>();
            localeDataStorageMock.Stub(m => m.HasLocaleData)
                .Return(true)
                .Repeat.Any();
            localeDataStorageMock.Stub(m => m.GetLocaleData())
                .Return(GenerateTestData(fixture))
                .Repeat.Any();

            var finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(fixture.Create<IFinansstyringKonfigurationRepository>(), localeDataStorageMock);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            var task = finansstyringRepositoryLocale.AdressekontoGetAsync(regnskabsnummer, adressekonto, DateTime.Now);
            task.Wait();

            var adressekontoModel = task.Result;
            Assert.That(adressekontoModel, Is.Not.Null);
            Assert.That(adressekontoModel.Regnskabsnummer, Is.EqualTo(regnskabsnummer));
            Assert.That(adressekontoModel.Nummer, Is.EqualTo(adressekonto));

            localeDataStorageMock.AssertWasCalled(m => m.GetLocaleData());
        }

        /// <summary>
        /// Tester, at KontogruppelisteGetAsync henter listen af kontogrupper.
        /// </summary>
        [Test]
        public void TestAtKontogruppelisteGetAsyncHenterKontogrupper()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            var localeDataStorageMock = MockRepository.GenerateMock<ILocaleDataStorage>();
            localeDataStorageMock.Stub(m => m.HasLocaleData)
                .Return(true)
                .Repeat.Any();
            localeDataStorageMock.Stub(m => m.GetLocaleData())
                .Return(GenerateTestData(fixture))
                .Repeat.Any();

            var finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(fixture.Create<IFinansstyringKonfigurationRepository>(), localeDataStorageMock);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            var task = finansstyringRepositoryLocale.KontogruppelisteGetAsync();
            task.Wait();

            Assert.That(task.Result, Is.Not.Null);
            Assert.That(task.Result, Is.Not.Empty);
            Assert.That(task.Result.Count(), Is.EqualTo(15));

            localeDataStorageMock.AssertWasCalled(m => m.GetLocaleData());
        }

        /// <summary>
        /// Tester, at BudgetkontogruppelisteGetAsync henter listen af kontogrupper til budgetkonti.
        /// </summary>
        [Test]
        public void TestAtBudgetkontogruppelisteGetAsyncHenterBudgetkontogrupper()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            var localeDataStorageMock = MockRepository.GenerateMock<ILocaleDataStorage>();
            localeDataStorageMock.Stub(m => m.HasLocaleData)
                .Return(true)
                .Repeat.Any();
            localeDataStorageMock.Stub(m => m.GetLocaleData())
                .Return(GenerateTestData(fixture))
                .Repeat.Any();

            var finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(fixture.Create<IFinansstyringKonfigurationRepository>(), localeDataStorageMock);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            var task = finansstyringRepositoryLocale.BudgetkontogruppelisteGetAsync();
            task.Wait();

            Assert.That(task.Result, Is.Not.Null);
            Assert.That(task.Result, Is.Not.Empty);
            Assert.That(task.Result.Count(), Is.EqualTo(25));

            localeDataStorageMock.AssertWasCalled(m => m.GetLocaleData());
        }

        /// <summary>
        /// Tester, at PrepareLocaleDataEventHandler returnerer, hvis der ikke er dannet et rodelement.
        /// </summary>
        [Test]
        [TestCase(true, false)]
        [TestCase(false, true)]
        public void TestAtPrepareLocaleDataEventHandlerReturnererHvisRootElementIkkeErDannet(bool readingContext, bool writingContext)
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            var localeDataDocument = new XDocument(new XDeclaration("1.0", "utf-8", null));

            var localeDataStorageMock = MockRepository.GenerateMock<ILocaleDataStorage>();
            localeDataStorageMock.Stub(m => m.HasLocaleData)
                .Return(true)
                .Repeat.Any();

            var finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(fixture.Create<IFinansstyringKonfigurationRepository>(), localeDataStorageMock);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            Assert.That(localeDataDocument.Root, Is.Null);
            localeDataStorageMock.Raise(m => m.PrepareLocaleData += null, localeDataStorageMock, new PrepareLocaleDataEventArgs(localeDataDocument, readingContext, writingContext, fixture.Create<bool>()));
            Assert.That(localeDataDocument.Root, Is.Null);
        }

        /// <summary>
        /// Tester, at PrepareLocaleDataEventHandler ikke danner attribute med versionsnummer, hvis attributten allerede er dannet.
        /// </summary>
        [Test]
        [TestCase(true, false)]
        [TestCase(false, true)]
        public void TestAtPrepareLocaleDataEventHandlerIkkeDannerVersionAttributeHvisDenneErDannet(bool readingContext, bool writingContext)
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            var localeDataDocument = new XDocument(new XDeclaration("1.0", "utf-8", null));
            var versionAttribute = new XAttribute(XName.Get("version", string.Empty), fixture.Create<string>());
            var rootElement = new XElement(XName.Get("FinansstyringRepository", FinansstyringRepositoryLocale.Namespace));
            rootElement.Add(versionAttribute);
            localeDataDocument.Add(rootElement);

            var localeDataStorageMock = MockRepository.GenerateMock<ILocaleDataStorage>();
            localeDataStorageMock.Stub(m => m.HasLocaleData)
                .Return(true)
                .Repeat.Any();

            var finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(fixture.Create<IFinansstyringKonfigurationRepository>(), localeDataStorageMock);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            Assert.That(localeDataDocument.Root, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(localeDataDocument.Root.Attribute(XName.Get("version", string.Empty)), Is.Not.Null);
            // ReSharper restore PossibleNullReferenceException

            localeDataStorageMock.Raise(m => m.PrepareLocaleData += null, localeDataStorageMock, new PrepareLocaleDataEventArgs(localeDataDocument, readingContext, writingContext, fixture.Create<bool>()));
            
            Assert.That(localeDataDocument.Root, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(localeDataDocument.Root.Attribute(XName.Get("version", string.Empty)), Is.Not.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Tester, at PrepareLocaleDataEventHandler ikke opdaterer versionsnummer på attributten til versionsnummer, hvis attributten allerede er dannet.
        /// </summary>
        [Test]
        [TestCase(true, false)]
        [TestCase(false, true)]
        public void TestAtPrepareLocaleDataEventHandlerIkkeOpdatererVersionAttributeHvisDenneErDannet(bool readingContext, bool writingContext)
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            var localeDataDocument = new XDocument(new XDeclaration("1.0", "utf-8", null));
            var versionValue = fixture.Create<string>();
            var versionAttribute = new XAttribute(XName.Get("version", string.Empty), versionValue);
            var rootElement = new XElement(XName.Get("FinansstyringRepository", FinansstyringRepositoryLocale.Namespace));
            rootElement.Add(versionAttribute);
            localeDataDocument.Add(rootElement);

            var localeDataStorageMock = MockRepository.GenerateMock<ILocaleDataStorage>();
            localeDataStorageMock.Stub(m => m.HasLocaleData)
                .Return(true)
                .Repeat.Any();

            var finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(fixture.Create<IFinansstyringKonfigurationRepository>(), localeDataStorageMock);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            Assert.That(localeDataDocument.Root, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(localeDataDocument.Root.Attribute(XName.Get("version", string.Empty)), Is.Not.Null);
            Assert.That(localeDataDocument.Root.Attribute(XName.Get("version", string.Empty)).Value, Is.Not.Null);
            Assert.That(localeDataDocument.Root.Attribute(XName.Get("version", string.Empty)).Value, Is.Not.Empty);
            Assert.That(localeDataDocument.Root.Attribute(XName.Get("version", string.Empty)).Value, Is.EqualTo(versionValue));
            // ReSharper restore PossibleNullReferenceException

            localeDataStorageMock.Raise(m => m.PrepareLocaleData += null, localeDataStorageMock, new PrepareLocaleDataEventArgs(localeDataDocument, readingContext, writingContext, fixture.Create<bool>()));

            Assert.That(localeDataDocument.Root, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(localeDataDocument.Root.Attribute(XName.Get("version", string.Empty)), Is.Not.Null);
            Assert.That(localeDataDocument.Root.Attribute(XName.Get("version", string.Empty)).Value, Is.Not.Null);
            Assert.That(localeDataDocument.Root.Attribute(XName.Get("version", string.Empty)).Value, Is.Not.Empty);
            Assert.That(localeDataDocument.Root.Attribute(XName.Get("version", string.Empty)).Value, Is.EqualTo(versionValue));
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Tester, at PrepareLocaleDataEventHandler danner attributten med versionsnummer, hvis attributten ikke er dannet og man samtidig enten er i læse- eller skrivekontekst.
        /// </summary>
        [Test]
        [TestCase(true, false)]
        [TestCase(false, true)]
        public void TestAtPrepareLocaleDataEventHandlerDannerVersionAttributeHvisDenneIkkeErDannetOgReadingContextEllerWritingContextErTrue(bool readingContext, bool writingContext)
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            var localeDataDocument = new XDocument(new XDeclaration("1.0", "utf-8", null));
            var rootElement = new XElement(XName.Get("FinansstyringRepository", FinansstyringRepositoryLocale.Namespace));
            localeDataDocument.Add(rootElement);

            var localeDataStorageMock = MockRepository.GenerateMock<ILocaleDataStorage>();
            localeDataStorageMock.Stub(m => m.HasLocaleData)
                .Return(true)
                .Repeat.Any();

            var finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(fixture.Create<IFinansstyringKonfigurationRepository>(), localeDataStorageMock);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            Assert.That(localeDataDocument.Root, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(localeDataDocument.Root.Attribute(XName.Get("version", string.Empty)), Is.Null);
            // ReSharper restore PossibleNullReferenceException

            localeDataStorageMock.Raise(m => m.PrepareLocaleData += null, localeDataStorageMock, new PrepareLocaleDataEventArgs(localeDataDocument, readingContext, writingContext, fixture.Create<bool>()));

            Assert.That(localeDataDocument.Root, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(localeDataDocument.Root.Attribute(XName.Get("version", string.Empty)), Is.Not.Null);
            Assert.That(localeDataDocument.Root.Attribute(XName.Get("version", string.Empty)).Value, Is.Not.Null);
            Assert.That(localeDataDocument.Root.Attribute(XName.Get("version", string.Empty)).Value, Is.Not.Empty);
            Assert.That(localeDataDocument.Root.Attribute(XName.Get("version", string.Empty)).Value, Is.EqualTo("1.0"));
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Tester, at PrepareLocaleDataEventHandler ikke danner attributten med versionsnummer, hvis attributten ikke er dannet og man samtidig ikke er i læse- eller skrivekontekst.
        /// </summary>
        [Test]
        public void TestAtPrepareLocaleDataEventHandlerIkkeDannerVersionAttributeHvisDenneIkkeErDannetOgReadingContextOgWritingContextErFalse()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            var localeDataDocument = new XDocument(new XDeclaration("1.0", "utf-8", null));
            var rootElement = new XElement(XName.Get("FinansstyringRepository", FinansstyringRepositoryLocale.Namespace));
            localeDataDocument.Add(rootElement);

            var localeDataStorageMock = MockRepository.GenerateMock<ILocaleDataStorage>();
            localeDataStorageMock.Stub(m => m.HasLocaleData)
                .Return(true)
                .Repeat.Any();

            var finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(fixture.Create<IFinansstyringKonfigurationRepository>(), localeDataStorageMock);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            Assert.That(localeDataDocument.Root, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(localeDataDocument.Root.Attribute(XName.Get("version", string.Empty)), Is.Null);
            // ReSharper restore PossibleNullReferenceException

            localeDataStorageMock.Raise(m => m.PrepareLocaleData += null, localeDataStorageMock, new PrepareLocaleDataEventArgs(localeDataDocument, false, false, fixture.Create<bool>()));

            Assert.That(localeDataDocument.Root, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(localeDataDocument.Root.Attribute(XName.Get("version", string.Empty)), Is.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Tester, at PrepareLocaleDataEventHandler kaster en ArgumentNullException, hvis objektet, der rejser eventet, er null
        /// </summary>
        [Test]
        public void TestAtPrepareLocaleDataEventHandlerKasterArgumentNullExceptionHvisSenderErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            var localeDataDocument = new XDocument(new XDeclaration("1.0", "utf-8", null));
            localeDataDocument.Add(new XElement(XName.Get("FinansstyringRepository", FinansstyringRepositoryLocale.Namespace)));

            var localeDataStorageMock = MockRepository.GenerateMock<ILocaleDataStorage>();
            localeDataStorageMock.Stub(m => m.HasLocaleData)
                .Return(true)
                .Repeat.Any();

            var finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(fixture.Create<IFinansstyringKonfigurationRepository>(), localeDataStorageMock);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => localeDataStorageMock.Raise(m => m.PrepareLocaleData += null, null, new PrepareLocaleDataEventArgs(localeDataDocument, fixture.Create<bool>(), fixture.Create<bool>(), fixture.Create<bool>())));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("sender"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PrepareLocaleDataEventHandler kaster en ArgumentNullException, hvis argumenter eventet, er null
        /// </summary>
        [Test]
        public void TestAtPrepareLocaleDataEventHandlerKasterArgumentNullExceptionHvisEventArgsErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            var localeDataDocument = new XDocument(new XDeclaration("1.0", "utf-8", null));
            localeDataDocument.Add(new XElement(XName.Get("FinansstyringRepository", FinansstyringRepositoryLocale.Namespace)));

            var localeDataStorageMock = MockRepository.GenerateMock<ILocaleDataStorage>();
            localeDataStorageMock.Stub(m => m.HasLocaleData)
                .Return(true)
                .Repeat.Any();

            var finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(fixture.Create<IFinansstyringKonfigurationRepository>(), localeDataStorageMock);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => localeDataStorageMock.Raise(m => m.PrepareLocaleData += null, localeDataStorageMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("eventArgs"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Danner testdata.
        /// </summary>
        /// <param name="fixture">Fixture, der kan generere random data.</param>
        /// <returns>Testdata.</returns>
        private static XDocument GenerateTestData(ISpecimenBuilder fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }
            lock (SyncRoot)
            {
                if (_localeDataDocument != null)
                {
                    return _localeDataDocument;
                }
                // Create testdata.
                var random = new Random(fixture.Create<int>());
                var localeDataDocument = new XDocument(new XDeclaration("1.0", "utf-8", null));
                var rootElement = new XElement(XName.Get("FinansstyringRepository", FinansstyringRepositoryLocale.Namespace));
                rootElement.Add(new XAttribute(XName.Get("version", string.Empty), "1.0"));
                localeDataDocument.Add(rootElement);
                for (var i = 0; i < 3; i++)
                {
                    var regnskabModel = CreateRegnskab(fixture, i + 1);
                    regnskabModel.StoreInDocument(localeDataDocument);
                    for (var j = 0; j < 15; j++)
                    {
                        var kontoModel = CreateKonto(fixture, i + 1, string.Format("KONTO{0:00}", j + 1));
                        kontoModel.StoreInDocument(localeDataDocument);
                    }
                    for (var j = 0; j < 30; j++)
                    {
                        var budgetkontoModel = CreateBudgetkonto(fixture, i + 1, string.Format("BUDGETKONTO{0:00}", j + 1));
                        budgetkontoModel.StoreInDocument(localeDataDocument);
                    }
                    for (var j = 0; j < 25; j++)
                    {
                        var adressekontoModel = CreateAdressekontoModel(fixture, i + 1, 100 + j + 1, 1);
                        adressekontoModel.StoreInDocument(localeDataDocument);
                    }
                    for (var j = 0; j < 15; j++)
                    {
                        var adressekontoModel = CreateAdressekontoModel(fixture, i + 1, 200 + j + 1, -1);
                        adressekontoModel.StoreInDocument(localeDataDocument);
                    }
                    for (var j = 0; j < 150; j++)
                    {
                        var bogføringslinje = CreateBogføringslinjeModel(fixture, i + 1, j + 1, random.Next(1, 365)*-1, string.Format("KONTO{0:00}", random.Next(1, 15)), string.Format("BUDGETKONTO{0:00}", random.Next(1, 30)), 100 + random.Next(1, 25));
                        bogføringslinje.StoreInDocument(localeDataDocument, false);
                    }
                }
                for (var i = 0; i < 15; i++)
                {
                    var kontogruppeModel = CreateKontogruppe(fixture, i + 1);
                    kontogruppeModel.StoreInDocument(localeDataDocument);
                }
                for (var i = 0; i < 25; i++)
                {
                    var budgetkontogruppeModel = CreateBudgetkontogruppe(fixture, i + 1);
                    budgetkontogruppeModel.StoreInDocument(localeDataDocument);
                }

                // Validation.
                var schemaSet = new XmlSchemaSet();
                var assembly = typeof(FinansstyringRepositoryLocale).Assembly;
                using (var resourceReader = assembly.GetManifestResourceStream(string.Format("{0}.{1}", assembly.GetName().Name, FinansstyringRepositoryLocale.XmlSchema)))
                {
                    if (resourceReader == null)
                    {
                        throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.UnableToLoadResource, FinansstyringRepositoryLocale.XmlSchema));
                    }
                    schemaSet.Add(XmlSchema.Read(resourceReader, ValidationEventHandler));
                    resourceReader.Close();
                }
                localeDataDocument.Validate(schemaSet, ValidationEventHandler);

                _localeDataDocument = localeDataDocument;
                return _localeDataDocument;
            }
        }

        /// <summary>
        /// Danner testdata til et regnskab.
        /// </summary>
        /// <param name="fixture">Fixture, der kan generere random data.</param>
        /// <param name="nummer">Unik identifikation af regnskabet.</param>
        /// <returns>Testdata til et regnskab.</returns>
        private static IRegnskabModel CreateRegnskab(ISpecimenBuilder fixture, int nummer)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }
            var regnskabModelMock = MockRepository.GenerateMock<IRegnskabModel>();
            regnskabModelMock.Expect(m => m.Nummer)
                .Return(nummer)
                .Repeat.Any();
            regnskabModelMock.Expect(m => m.Navn)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            return regnskabModelMock;
        }

        /// <summary>
        /// Danner testdata til en konto.
        /// </summary>
        /// <param name="fixture">Fixture, der kan generere random data.</param>
        /// <param name="regnskabsnummer">Unik identifikation af regnskabet, som kontoen skal være tilknyttet.</param>
        /// <param name="kontonummer">Unik identifikation af kontoen.</param>
        /// <returns>Testdata til en konto.</returns>
        private static IKontoModel CreateKonto(ISpecimenBuilder fixture, int regnskabsnummer, string kontonummer)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }
            if (string.IsNullOrEmpty(kontonummer))
            {
                throw new ArgumentNullException("kontonummer");
            }
            var kontogruppe = (DateTime.Now.Millisecond%15) + 1;
            var kontoModelMock = MockRepository.GenerateMock<IKontoModel>();
            kontoModelMock.Expect(m => m.Regnskabsnummer)
                .Return(regnskabsnummer)
                .Repeat.Any();
            kontoModelMock.Expect(m => m.Kontonummer)
                .Return(kontonummer)
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
            kontoModelMock.Expect(m => m.Kontogruppe)
                .Return(kontogruppe)
                .Repeat.Any();
            kontoModelMock.Expect(m => m.StatusDato)
                .Return(DateTime.Now.Date)
                .Repeat.Any();
            kontoModelMock.Expect(m => m.Kredit)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            kontoModelMock.Expect(m => m.Saldo)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            return kontoModelMock;
        }

        /// <summary>
        /// Danner testdata til en budgetkonto.
        /// </summary>
        /// <param name="fixture">Fixture, der kan generere random data.</param>
        /// <param name="regnskabsnummer">Unik identifikation af regnskabet, som budgetkontoen skal være tilknyttet.</param>
        /// <param name="kontonummer">Unik identifikation af kontoen.</param>
        /// <returns>Testdata til en budgetkonto.</returns>
        private static IBudgetkontoModel CreateBudgetkonto(ISpecimenBuilder fixture, int regnskabsnummer, string kontonummer)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }
            if (string.IsNullOrEmpty(kontonummer))
            {
                throw new ArgumentNullException("kontonummer");
            }
            var budgetkontogruppe = (DateTime.Now.Millisecond%25) + 1;
            var budgetkontoModelMock = MockRepository.GenerateMock<IBudgetkontoModel>();
            budgetkontoModelMock.Expect(m => m.Regnskabsnummer)
                .Return(regnskabsnummer)
                .Repeat.Any();
            budgetkontoModelMock.Expect(m => m.Kontonummer)
                .Return(kontonummer)
                .Repeat.Any();
            budgetkontoModelMock.Expect(m => m.Kontonavn)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            budgetkontoModelMock.Expect(m => m.Beskrivelse)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            budgetkontoModelMock.Expect(m => m.Notat)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            budgetkontoModelMock.Expect(m => m.Kontogruppe)
                .Return(budgetkontogruppe)
                .Repeat.Any();
            budgetkontoModelMock.Expect(m => m.StatusDato)
                .Return(DateTime.Now.Date)
                .Repeat.Any();
            budgetkontoModelMock.Expect(m => m.Indtægter)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            budgetkontoModelMock.Expect(m => m.Udgifter)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            budgetkontoModelMock.Expect(m => m.BudgetSidsteMåned)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            budgetkontoModelMock.Expect(m => m.Bogført)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            budgetkontoModelMock.Expect(m => m.BogførtSidsteMåned)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            return budgetkontoModelMock;
        }

        /// <summary>
        /// Danner testdata til en adressekonto.
        /// </summary>
        /// <param name="fixture">Fixture, der kan generere random data.</param>
        /// <param name="regnskabsnummer">Unik identifikation af regnskabet, som adressekontoen skal være tilknyttet.</param>
        /// <param name="nummer">Unik identifikation af adressekontoen.</param>
        /// <param name="saldoOffset">Offset, som saldoen skal ganges med.</param>
        /// <returns>Testdata til en adressekonto.</returns>
        private static IAdressekontoModel CreateAdressekontoModel(ISpecimenBuilder fixture, int regnskabsnummer, int nummer, int saldoOffset)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }
            var adressekontoModelMock = MockRepository.GenerateMock<IAdressekontoModel>();
            adressekontoModelMock.Expect(m => m.Regnskabsnummer)
                .Return(regnskabsnummer)
                .Repeat.Any();
            adressekontoModelMock.Expect(m => m.Nummer)
                .Return(nummer)
                .Repeat.Any();
            adressekontoModelMock.Expect(m => m.Navn)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            adressekontoModelMock.Expect(m => m.PrimærTelefon)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            adressekontoModelMock.Expect(m => m.SekundærTelefon)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            adressekontoModelMock.Expect(m => m.StatusDato)
                .Return(DateTime.Now.Date)
                .Repeat.Any();
            adressekontoModelMock.Expect(m => m.Saldo)
                .Return(Math.Abs(fixture.Create<decimal>())*saldoOffset)
                .Repeat.Any();
            return adressekontoModelMock;
        }

        /// <summary>
        /// Danner testdata til en bogføringslinje.
        /// </summary>
        /// <param name="fixture">Fixture, der kan generere random data.</param>
        /// <param name="regnskabsnummer">Unik identifikation af regnskabet, som bogføringslinjen skal være tilknyttet.</param>
        /// <param name="løbenummer">Løbenummeret på bogføringslinjen.</param>
        /// <param name="datoOffset">Offset, som datoen skal adderes med.</param>
        /// <param name="kontonummer">Kontonummer, som bogføringslinjen skal være tilknyttet.</param>
        /// <param name="budgetkontonummer">Budgetkontonummer, som bogføringslinjen skal være tilknyttet.</param>
        /// <param name="adressekonto">Adressekonto, som bogføringslinjen skal være tilknyttet.</param>
        /// <returns>Testdata til en bogføringslinje.</returns>
        private static IBogføringslinjeModel CreateBogføringslinjeModel(ISpecimenBuilder fixture, int regnskabsnummer, int løbenummer, int datoOffset, string kontonummer, string budgetkontonummer, int adressekonto)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }
            if (string.IsNullOrEmpty(kontonummer))
            {
                throw new ArgumentNullException("kontonummer");
            }
            if (string.IsNullOrEmpty(budgetkontonummer))
            {
                throw new ArgumentNullException("budgetkontonummer");
            }
            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Regnskabsnummer)
                .Return(regnskabsnummer)
                .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Løbenummer)
                .Return(løbenummer)
                .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                .Return(DateTime.Today.AddDays(datoOffset))
                .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Bilag)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                .Return(kontonummer)
                .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Tekst)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                .Return(budgetkontonummer)
                .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Debit)
                .Return(Math.Abs(fixture.Create<decimal>()))
                .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kredit)
                .Return(Math.Abs(fixture.Create<decimal>()))
                .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                .Return(adressekonto)
                .Repeat.Any();
            return bogføringslinjeModelMock;
        }

        /// <summary>
        /// Danner testdata til en kontogruppe.
        /// </summary>
        /// <param name="fixture">Fixture, der kan generere random data.</param>
        /// <param name="nummer">Unik identifikation af kontogruppen.</param>
        /// <returns>Testdata til en kontogruppe.</returns>
        private static IKontogruppeModel CreateKontogruppe(ISpecimenBuilder fixture, int nummer)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }
            var kontogruppeModelMock = MockRepository.GenerateMock<IKontogruppeModel>();
            kontogruppeModelMock.Expect(m => m.Nummer)
                .Return(nummer)
                .Repeat.Any();
            kontogruppeModelMock.Expect(m => m.Tekst)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            kontogruppeModelMock.Expect(m => m.Balancetype)
                .Return(nummer%2 != 0 ? Balancetype.Aktiver : Balancetype.Passiver)
                .Repeat.Any();
            return kontogruppeModelMock;
        }

        /// <summary>
        /// Danner testdata til en kontogruppe for budgetkonti.
        /// </summary>
        /// <param name="fixture">Fixture, der kan generere random data.</param>
        /// <param name="nummer">Unik identifikation af kontogruppen.</param>
        /// <returns>Testdata til en kontogruppe.</returns>
        private static IBudgetkontogruppeModel CreateBudgetkontogruppe(ISpecimenBuilder fixture, int nummer)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }
            var budgetkontogruppeModelMock = MockRepository.GenerateMock<IBudgetkontogruppeModel>();
            budgetkontogruppeModelMock.Expect(m => m.Nummer)
                .Return(nummer)
                .Repeat.Any();
            budgetkontogruppeModelMock.Expect(m => m.Tekst)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            return budgetkontogruppeModelMock;
        }

        /// <summary>
        /// Eventhandler, der håndterer XML validering.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        private static void ValidationEventHandler(object sender, ValidationEventArgs eventArgs)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            if (eventArgs == null)
            {
                throw new ArgumentNullException("eventArgs");
            }
            switch (eventArgs.Severity)
            {
                case XmlSeverityType.Warning:
                    throw new IntranetGuiRepositoryException(eventArgs.Message, eventArgs.Exception);

                case XmlSeverityType.Error:
                    throw new IntranetGuiRepositoryException(eventArgs.Message, eventArgs.Exception);
            }
        }
    }
}
