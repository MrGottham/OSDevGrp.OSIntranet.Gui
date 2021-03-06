﻿using System;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.Resources;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Finansstyring.Tests
{
    /// <summary>
    /// Tester repository, der supporterer finansstyring.
    /// </summary>
    [TestFixture]
    public class FinansstyringRepositoryTests
    {
        #region Private constants

        private const string FinansstyringServiceTestUri = "http://services.osdevgrp.local/osintranet/finansstyringservice.svc/mobile";

        #endregion

        /// <summary>
        /// Tester, at konstruktøren initierer repository, der supporterer finansstyring.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererFinansstyringRepository()
        {
            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();

            var finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock);
            Assert.That(finansstyringRepository, Is.Not.Null);
            Assert.That(finansstyringRepository.Konfiguration, Is.Not.Null);
            Assert.That(finansstyringRepository.Konfiguration, Is.EqualTo(finansstyringKonfigurationRepositoryMock));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis konfigurationrepositoryet, der supporterer finansstyring, er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFinansstyringKonfigurationRepositoryErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new FinansstyringRepository(null));
        }

        /// <summary>
        /// Tester, at RegnskabslisteGetAsync henter regnskaber.
        /// </summary>
        [Test]
        public void TestAtRegnskabslisteGetAsyncHenterRegnskaber()
        {
            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.FinansstyringServiceUri)
                                                    .Return(new Uri(FinansstyringServiceTestUri))
                                                    .Repeat.Any();

            var finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock);
            Assert.That(finansstyringRepository, Is.Not.Null);

            var task = finansstyringRepository.RegnskabslisteGetAsync();
            task.Wait();

            Assert.That(task.Result, Is.Not.Null);
            Assert.That(task.Result, Is.Not.Empty);
            Assert.That(task.Result.Count(), Is.GreaterThan(0));

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.FinansstyringServiceUri);
        }

        /// <summary>
        /// Tester, at KontoplanGetAsync henter kontoplanen til et regnskab.
        /// </summary>
        [Test]
        public void TestAtKontoplanGetAsyncHenterKontoplan()
        {
            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.FinansstyringServiceUri)
                                                    .Return(new Uri(FinansstyringServiceTestUri))
                                                    .Repeat.Any();

            var finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock);
            Assert.That(finansstyringRepository, Is.Not.Null);

            var task = finansstyringRepository.KontoplanGetAsync(1, DateTime.Now);
            task.Wait();

            Assert.That(task.Result, Is.Not.Null);
            Assert.That(task.Result, Is.Not.Empty);

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.FinansstyringServiceUri);
        }

        /// <summary>
        /// Tester, at KontoGetAsync kaster en ArgumentNullException ved illegale kontonumre.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestAtKontoGetAsyncKasterArgumentNullExceptionVedIllegalKontonummer(string illegalKontonummer)
        {
            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();

            var finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock);
            Assert.That(finansstyringRepository, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => finansstyringRepository.KontoGetAsync(1, illegalKontonummer, DateTime.Now).Wait());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("kontonummer"));
            Assert.That(exception.InnerException, Is.Null);

            finansstyringKonfigurationRepositoryMock.AssertWasNotCalled(m => m.FinansstyringServiceUri);
        }

        /// <summary>
        /// Tester, at KontoGetAsync henter en given konto.
        /// </summary>
        [Test]
        public void TestAtKontoGetAsyncHenterKonto()
        {
            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.FinansstyringServiceUri)
                                                    .Return(new Uri(FinansstyringServiceTestUri))
                                                    .Repeat.Any();

            var finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock);
            Assert.That(finansstyringRepository, Is.Not.Null);

            var statusDato = DateTime.Now;
            var task = finansstyringRepository.KontoGetAsync(1, "DANKORT", statusDato);
            task.Wait();

            var konto = task.Result;
            Assert.That(konto, Is.Not.Null);
            Assert.That(konto.Regnskabsnummer, Is.EqualTo(1));
            Assert.That(konto.Kontonummer, Is.Not.Null);
            Assert.That(konto.Kontonummer, Is.Not.Empty);
            Assert.That(konto.Kontonummer, Is.EqualTo("DANKORT"));
            Assert.That(konto.StatusDato, Is.EqualTo(statusDato));

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.FinansstyringServiceUri);
        }

        /// <summary>
        /// Tester, at BudgetkontoplanGetAsync henter budgetkontoplanen til et regnskab.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoplanGetAsyncHenterBudgetkontoplan()
        {
            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.FinansstyringServiceUri)
                                                    .Return(new Uri(FinansstyringServiceTestUri))
                                                    .Repeat.Any();

            var finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock);
            Assert.That(finansstyringRepository, Is.Not.Null);

            var task = finansstyringRepository.BudgetkontoplanGetAsync(1, DateTime.Now);
            task.Wait();

            Assert.That(task.Result, Is.Not.Null);
            Assert.That(task.Result, Is.Not.Empty);

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.FinansstyringServiceUri);
        }

        /// <summary>
        /// Tester, at BudgetkontoGetAsync kaster en ArgumentNullException ved illegale kontonumre.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestAtBudgetkontoGetAsyncKasterArgumentNullExceptionVedIllegalKontonummer(string illegalKontonummer)
        {
            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();

            var finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock);
            Assert.That(finansstyringRepository, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => finansstyringRepository.BudgetkontoGetAsync(1, illegalKontonummer, DateTime.Now).Wait());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("budgetkontonummer"));
            Assert.That(exception.InnerException, Is.Null);

            finansstyringKonfigurationRepositoryMock.AssertWasNotCalled(m => m.FinansstyringServiceUri);
        }

        /// <summary>
        /// Tester, at BudgetkontoGetAsync henter en given budgetkonto.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoGetAsyncHenterBudgetkonto()
        {
            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.FinansstyringServiceUri)
                                                    .Return(new Uri(FinansstyringServiceTestUri))
                                                    .Repeat.Any();

            var finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock);
            Assert.That(finansstyringRepository, Is.Not.Null);

            var statusDato = DateTime.Now;
            var task = finansstyringRepository.BudgetkontoGetAsync(1, "3000", statusDato);
            task.Wait();

            var budgetkonto = task.Result; 
            Assert.That(budgetkonto, Is.Not.Null);
            Assert.That(budgetkonto.Regnskabsnummer, Is.EqualTo(1));
            Assert.That(budgetkonto.Kontonummer, Is.Not.Null);
            Assert.That(budgetkonto.Kontonummer, Is.Not.Empty);
            Assert.That(budgetkonto.Kontonummer, Is.EqualTo("3000"));
            Assert.That(budgetkonto.StatusDato, Is.EqualTo(statusDato));

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.FinansstyringServiceUri);
        }

        /// <summary>
        /// Tester, at BogføringslinjerGetAsync henter et givent antal bogføringslinjer til et givent regnskab.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjerGetAsyncHenterBogføringslinjer()
        {
            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.FinansstyringServiceUri)
                                                    .Return(new Uri(FinansstyringServiceTestUri))
                                                    .Repeat.Any();

            var finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock);
            Assert.That(finansstyringRepository, Is.Not.Null);

            var task = finansstyringRepository.BogføringslinjerGetAsync(1, DateTime.Now, 50);
            task.Wait();

            Assert.That(task.Result, Is.Not.Null);
            Assert.That(task.Result, Is.Not.Empty);
            Assert.That(task.Result.Count(), Is.EqualTo(50));

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.FinansstyringServiceUri);
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
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();

            var finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock);
            Assert.That(finansstyringRepository, Is.Not.Null);

            ArgumentException exception = null;
            var aggregateException = Assert.Throws<AggregateException>(() => finansstyringRepository.BogføringslinjeCreateNewAsync(illegalValue, fixture.Create<DateTime>(), fixture.Create<string>()).Wait());
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

            finansstyringKonfigurationRepositoryMock.AssertWasNotCalled(m => m.FinansstyringServiceUri);
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

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();

            var finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock);
            Assert.That(finansstyringRepository, Is.Not.Null);

            var dato = Convert.ToDateTime(illegalValue, new CultureInfo("en-US"));

            ArgumentException exception = null;
            var aggregateException = Assert.Throws<AggregateException>(() => finansstyringRepository.BogføringslinjeCreateNewAsync(fixture.Create<int>(), dato, fixture.Create<string>()).Wait());
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

            finansstyringKonfigurationRepositoryMock.AssertWasNotCalled(m => m.FinansstyringServiceUri);
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
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();

            var finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock);
            Assert.That(finansstyringRepository, Is.Not.Null);

            ArgumentNullException exception = null;
            var aggregateException = Assert.Throws<AggregateException>(() => finansstyringRepository.BogføringslinjeCreateNewAsync(fixture.Create<int>(), fixture.Create<DateTime>(), illegalValue).Wait());
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

            finansstyringKonfigurationRepositoryMock.AssertWasNotCalled(m => m.FinansstyringServiceUri);
        }

        /// <summary>
        /// Tester, at BogføringslinjeCreateNewAsync danner og returnerer en ny bogføringslinje, der efterfølgende kan bogføres.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjeCreateNewAsyncDannerBogføringslinjeModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();

            var finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock);
            Assert.That(finansstyringRepository, Is.Not.Null);

            var regnskabsnummer = fixture.Create<int>();
            var dato = fixture.Create<DateTime>();
            var kontonummer = fixture.Create<string>();
            var task = finansstyringRepository.BogføringslinjeCreateNewAsync(regnskabsnummer, dato, kontonummer);
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

            finansstyringKonfigurationRepositoryMock.AssertWasNotCalled(m => m.FinansstyringServiceUri);
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
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();

            var finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock);
            Assert.That(finansstyringRepository, Is.Not.Null);

            var exception = Assert.Throws<ArgumentException>(() => finansstyringRepository.BogførAsync(illegalValue, fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), fixture.Create<decimal>(), fixture.Create<int>()).Wait());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Does.StartWith(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "regnskabsnummer", illegalValue)));
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("regnskabsnummer"));
            Assert.That(exception.InnerException, Is.Null);

            finansstyringKonfigurationRepositoryMock.AssertWasNotCalled(m => m.FinansstyringServiceUri);
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

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();

            var finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock);
            Assert.That(finansstyringRepository, Is.Not.Null);

            var dato = Convert.ToDateTime(illegalValue, new CultureInfo("en-US"));
            var exception = Assert.Throws<ArgumentException>(() => finansstyringRepository.BogførAsync(fixture.Create<int>(), dato, fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), fixture.Create<decimal>(), fixture.Create<int>()).Wait());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Does.StartWith(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "dato", dato)));
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("dato"));
            Assert.That(exception.InnerException, Is.Null);

            finansstyringKonfigurationRepositoryMock.AssertWasNotCalled(m => m.FinansstyringServiceUri);
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
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();

            var finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock);
            Assert.That(finansstyringRepository, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => finansstyringRepository.BogførAsync(fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), illegalValue, fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), fixture.Create<decimal>(), fixture.Create<int>()).Wait());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("kontonummer"));
            Assert.That(exception.InnerException, Is.Null);

            finansstyringKonfigurationRepositoryMock.AssertWasNotCalled(m => m.FinansstyringServiceUri);
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
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();

            var finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock);
            Assert.That(finansstyringRepository, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => finansstyringRepository.BogførAsync(fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), illegalValue, fixture.Create<string>(), fixture.Create<decimal>(), fixture.Create<decimal>(), fixture.Create<int>()).Wait());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("tekst"));
            Assert.That(exception.InnerException, Is.Null);

            finansstyringKonfigurationRepositoryMock.AssertWasNotCalled(m => m.FinansstyringServiceUri);
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
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();

            var finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock);
            Assert.That(finansstyringRepository, Is.Not.Null);

            var exception = Assert.Throws<ArgumentException>(() => finansstyringRepository.BogførAsync(fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>(), illegalValue, fixture.Create<decimal>(), fixture.Create<int>()).Wait());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Does.StartWith(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "debit", illegalValue)));
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("debit"));
            Assert.That(exception.InnerException, Is.Null);

            finansstyringKonfigurationRepositoryMock.AssertWasNotCalled(m => m.FinansstyringServiceUri);
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
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();

            var finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock);
            Assert.That(finansstyringRepository, Is.Not.Null);

            var exception = Assert.Throws<ArgumentException>(() => finansstyringRepository.BogførAsync(fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), illegalValue, fixture.Create<int>()).Wait());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Does.StartWith(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "kredit", illegalValue)));
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("kredit"));
            Assert.That(exception.InnerException, Is.Null);

            finansstyringKonfigurationRepositoryMock.AssertWasNotCalled(m => m.FinansstyringServiceUri);
        }

        /// <summary>
        /// Tester, at BogførAsync bogfører værdier.
        /// </summary>
        [Test]
        [Ignore("Skal ikke køres")]
        public void TestAtBogførAsyncBogførerValues()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.FinansstyringServiceUri)
                                                    .Return(new Uri(FinansstyringServiceTestUri))
                                                    .Repeat.Any();

            var finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock);
            Assert.That(finansstyringRepository, Is.Not.Null);

            var task1 = finansstyringRepository.BogførAsync(1, fixture.Create<DateTime>(), null, "DANKORT", "Test", "8990", 5000M, 0M, 0);
            task1.Wait();

            Assert.That(task1.Result, Is.Not.Null);
            Assert.That(task1.Result.Bogføringslinje, Is.Not.Null);
            Assert.That(task1.Result.Bogføringsadvarsler, Is.Not.Null);

            var task2 = finansstyringRepository.BogførAsync(1, fixture.Create<DateTime>(), null, "DANKORT", "Test", "8990", 0M, 5000M, 0);
            task2.Wait();

            Assert.That(task2.Result, Is.Not.Null);
            Assert.That(task2.Result.Bogføringslinje, Is.Not.Null);
            Assert.That(task2.Result.Bogføringsadvarsler, Is.Not.Null);

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.FinansstyringServiceUri);
        }

        /// <summary>
        /// Tester, at DebitorlisteGetAsync henter listen af debitorer til et givent regnskab.
        /// </summary>
        [Test]
        public void TestAtDebitorlisteGetAsyncHenterDebitorliste()
        {
            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.FinansstyringServiceUri)
                                                    .Return(new Uri(FinansstyringServiceTestUri))
                                                    .Repeat.Any();

            var finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock);
            Assert.That(finansstyringRepository, Is.Not.Null);

            var task = finansstyringRepository.DebitorlisteGetAsync(1, DateTime.Now);
            task.Wait();

            Assert.That(task.Result, Is.Not.Null);
            Assert.That(task.Result.Count(), Is.GreaterThanOrEqualTo(0));

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.FinansstyringServiceUri);
        }

        /// <summary>
        /// Tester, at KreditorlisteGetAsync henter listen af kreditorer til et givent regnskab.
        /// </summary>
        [Test]
        public void TestAtKreditorlisteGetAsyncHenterKreditorliste()
        {
            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.FinansstyringServiceUri)
                                                    .Return(new Uri(FinansstyringServiceTestUri))
                                                    .Repeat.Any();

            var finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock);
            Assert.That(finansstyringRepository, Is.Not.Null);

            var task = finansstyringRepository.KreditorlisteGetAsync(1, DateTime.Now);
            task.Wait();

            Assert.That(task.Result, Is.Not.Null);
            Assert.That(task.Result.Count(), Is.GreaterThanOrEqualTo(0));

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.FinansstyringServiceUri);
        }

        /// <summary>
        /// Tester, at AdressekontolisteGetAsync henter listen af adressekonti til et givent regnskab.
        /// </summary>
        [Test]
        public void TestAtAdressekontolisteGetAsyncHenterAdressekontoliste()
        {
            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.FinansstyringServiceUri)
                                                    .Return(new Uri(FinansstyringServiceTestUri))
                                                    .Repeat.Any();

            var finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock);
            Assert.That(finansstyringRepository, Is.Not.Null);

            var task = finansstyringRepository.AdressekontolisteGetAsync(1, DateTime.Now);
            task.Wait();

            Assert.That(task.Result, Is.Not.Null);
            Assert.That(task.Result.Count(), Is.GreaterThanOrEqualTo(0));

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.FinansstyringServiceUri);
        }

        /// <summary>
        /// Tester, at AdressekontoGetAsync henter en adressekonto.
        /// </summary>
        [Test]
        public void TestAtAdressekontoGetAsyncHenterAdressekonto()
        {
            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.FinansstyringServiceUri)
                                                    .Return(new Uri(FinansstyringServiceTestUri))
                                                    .Repeat.Any();

            var finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock);
            Assert.That(finansstyringRepository, Is.Not.Null);

            var statusDato = DateTime.Now;
            var task = finansstyringRepository.AdressekontoGetAsync(1, 1, statusDato);
            task.Wait();

            var adressekonto = task.Result;
            Assert.That(adressekonto, Is.Not.Null);
            Assert.That(adressekonto.Regnskabsnummer, Is.EqualTo(1));
            Assert.That(adressekonto.Nummer, Is.EqualTo(1));
            Assert.That(adressekonto.StatusDato, Is.EqualTo(statusDato));

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.FinansstyringServiceUri);
        }

        /// <summary>
        /// Tester, at KontogruppelisteGetAsync henter listen af kontogrupper.
        /// </summary>
        [Test]
        public void TestAtKontogruppelisteGetAsyncHenterKontogrupper()
        {
            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.FinansstyringServiceUri)
                                                    .Return(new Uri(FinansstyringServiceTestUri))
                                                    .Repeat.Any();

            var finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock);
            Assert.That(finansstyringRepository, Is.Not.Null);

            var task = finansstyringRepository.KontogruppelisteGetAsync();
            task.Wait();

            Assert.That(task.Result, Is.Not.Null);
            Assert.That(task.Result, Is.Not.Empty);

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.FinansstyringServiceUri);
        }

        /// <summary>
        /// Tester, at BudgetkontogruppelisteGetAsync henter liste af grupper til budgetkonti.
        /// </summary>
        [Test]
        public void TestAtBudgetkontogruppelisteGetAsyncHenterBudgetkontogrupper()
        {
            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.FinansstyringServiceUri)
                                                    .Return(new Uri(FinansstyringServiceTestUri))
                                                    .Repeat.Any();

            var finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock);
            Assert.That(finansstyringRepository, Is.Not.Null);

            var task = finansstyringRepository.BudgetkontogruppelisteGetAsync();
            task.Wait();

            Assert.That(task.Result, Is.Not.Null);
            Assert.That(task.Result, Is.Not.Empty);

            finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.FinansstyringServiceUri);
        }
    }
}
