using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.Resources;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Finansstyring.Tests
{
    /// <summary>
    /// Tester repository, der supporterer finansstyring.
    /// </summary>
    [TestFixture]
    public class FinansstyringRepositoryTests
    {
        #region Private constants

        private const string FinansstyringServiceTestUri = "https://locahlhost/api";

        #endregion

        /// <summary>
        /// Tester, at konstruktøren initierer repository, der supporterer finansstyring.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererFinansstyringRepository()
        {
            IFinansstyringKonfigurationRepository finansstyringKonfigurationRepositoryMock = new Mock<IFinansstyringKonfigurationRepository>().Object;

            IFinansstyringRepository finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock);
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
            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new FinansstyringRepository(null));
            // ReSharper restore ObjectCreationAsStatement
            Assert.That(exception, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("finansstyringKonfigurationRepository"));
            Assert.That(exception.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Tester, at RegnskabslisteGetAsync henter regnskaber.
        /// </summary>
        [Test]
        public async Task TestAtRegnskabslisteGetAsyncHenterRegnskaber()
        {
            Mock<IFinansstyringKonfigurationRepository> finansstyringKonfigurationRepositoryMock = new Mock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Setup(m => m.FinansstyringServiceUri)
                .Returns(new Uri(FinansstyringServiceTestUri));

            IFinansstyringRepository finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock.Object);
            Assert.That(finansstyringRepository, Is.Not.Null);

            IRegnskabModel[] result = (await finansstyringRepository.RegnskabslisteGetAsync()).ToArray();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count, Is.GreaterThan(0));

            finansstyringKonfigurationRepositoryMock.Verify(m => m.FinansstyringServiceUri, Times.Once);
        }

        /// <summary>
        /// Tester, at KontoplanGetAsync henter kontoplanen til et regnskab.
        /// </summary>
        [Test]
        public async Task TestAtKontoplanGetAsyncHenterKontoplan()
        {
            Mock<IFinansstyringKonfigurationRepository> finansstyringKonfigurationRepositoryMock = new Mock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Setup(m => m.FinansstyringServiceUri)
                .Returns(new Uri(FinansstyringServiceTestUri));

            IFinansstyringRepository finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock.Object);
            Assert.That(finansstyringRepository, Is.Not.Null);

            IKontoModel[] result = (await finansstyringRepository.KontoplanGetAsync(1, DateTime.Now)).ToArray();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);

            finansstyringKonfigurationRepositoryMock.Verify(m => m.FinansstyringServiceUri, Times.Once);
        }

        /// <summary>
        /// Tester, at KontoGetAsync kaster en ArgumentNullException ved illegale kontonumre.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void TestAtKontoGetAsyncKasterArgumentNullExceptionVedIllegalKontonummer(string illegalKontonummer)
        {
            Mock<IFinansstyringKonfigurationRepository> finansstyringKonfigurationRepositoryMock = new Mock<IFinansstyringKonfigurationRepository>();

            IFinansstyringRepository finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock.Object);
            Assert.That(finansstyringRepository, Is.Not.Null);

            ArgumentNullException exception = Assert.ThrowsAsync<ArgumentNullException>(async () => await finansstyringRepository.KontoGetAsync(1, illegalKontonummer, DateTime.Now));
            Assert.That(exception, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("kontonummer"));
            Assert.That(exception.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Tester, at KontoGetAsync henter en given konto.
        /// </summary>
        [Test]
        public async Task TestAtKontoGetAsyncHenterKonto()
        {
            Mock<IFinansstyringKonfigurationRepository> finansstyringKonfigurationRepositoryMock = new Mock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Setup(m => m.FinansstyringServiceUri)
                .Returns(new Uri(FinansstyringServiceTestUri));

            IFinansstyringRepository finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock.Object);
            Assert.That(finansstyringRepository, Is.Not.Null);

            DateTime statusDato = DateTime.Now;
            IKontoModel konto = await finansstyringRepository.KontoGetAsync(1, "DANKORT", statusDato);
            Assert.That(konto, Is.Not.Null);
            Assert.That(konto.Regnskabsnummer, Is.EqualTo(1));
            Assert.That(konto.Kontonummer, Is.Not.Null);
            Assert.That(konto.Kontonummer, Is.Not.Empty);
            Assert.That(konto.Kontonummer, Is.EqualTo("DANKORT"));
            Assert.That(konto.StatusDato, Is.EqualTo(statusDato));

            finansstyringKonfigurationRepositoryMock.Verify(m => m.FinansstyringServiceUri, Times.Once);
        }

        /// <summary>
        /// Tester, at BudgetkontoplanGetAsync henter budgetkontoplanen til et regnskab.
        /// </summary>
        [Test]
        public async Task TestAtBudgetkontoplanGetAsyncHenterBudgetkontoplan()
        {
            Mock<IFinansstyringKonfigurationRepository> finansstyringKonfigurationRepositoryMock = new Mock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Setup(m => m.FinansstyringServiceUri)
                .Returns(new Uri(FinansstyringServiceTestUri));

            IFinansstyringRepository finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock.Object);
            Assert.That(finansstyringRepository, Is.Not.Null);

            IBudgetkontoModel[] result = (await finansstyringRepository.BudgetkontoplanGetAsync(1, DateTime.Now)).ToArray();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);

            finansstyringKonfigurationRepositoryMock.Verify(m => m.FinansstyringServiceUri, Times.Once);
        }

        /// <summary>
        /// Tester, at BudgetkontoGetAsync kaster en ArgumentNullException ved illegale kontonumre.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void TestAtBudgetkontoGetAsyncKasterArgumentNullExceptionVedIllegalKontonummer(string illegalKontonummer)
        {
            Mock<IFinansstyringKonfigurationRepository> finansstyringKonfigurationRepositoryMock = new Mock<IFinansstyringKonfigurationRepository>();

            IFinansstyringRepository finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock.Object);
            Assert.That(finansstyringRepository, Is.Not.Null);

            ArgumentNullException exception = Assert.ThrowsAsync<ArgumentNullException>(async () => await finansstyringRepository.BudgetkontoGetAsync(1, illegalKontonummer, DateTime.Now));
            Assert.That(exception, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("budgetkontonummer"));
            Assert.That(exception.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Tester, at BudgetkontoGetAsync henter en given budgetkonto.
        /// </summary>
        [Test]
        public async Task TestAtBudgetkontoGetAsyncHenterBudgetkonto()
        {
            Mock<IFinansstyringKonfigurationRepository> finansstyringKonfigurationRepositoryMock = new Mock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Setup(m => m.FinansstyringServiceUri)
                .Returns(new Uri(FinansstyringServiceTestUri));

            IFinansstyringRepository finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock.Object);
            Assert.That(finansstyringRepository, Is.Not.Null);

            DateTime statusDato = DateTime.Now;
            IBudgetkontoModel budgetkonto = await finansstyringRepository.BudgetkontoGetAsync(1, "3000", statusDato);
            Assert.That(budgetkonto, Is.Not.Null);
            Assert.That(budgetkonto.Regnskabsnummer, Is.EqualTo(1));
            Assert.That(budgetkonto.Kontonummer, Is.Not.Null);
            Assert.That(budgetkonto.Kontonummer, Is.Not.Empty);
            Assert.That(budgetkonto.Kontonummer, Is.EqualTo("3000"));
            Assert.That(budgetkonto.StatusDato, Is.EqualTo(statusDato));

            finansstyringKonfigurationRepositoryMock.Verify(m => m.FinansstyringServiceUri, Times.Once);
        }

        /// <summary>
        /// Tester, at BogføringslinjerGetAsync henter et givent antal bogføringslinjer til et givent regnskab.
        /// </summary>
        [Test]
        public async Task TestAtBogføringslinjerGetAsyncHenterBogføringslinjer()
        {
            Mock<IFinansstyringKonfigurationRepository> finansstyringKonfigurationRepositoryMock = new Mock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Setup(m => m.FinansstyringServiceUri)
                .Returns(new Uri(FinansstyringServiceTestUri));

            IFinansstyringRepository finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock.Object);
            Assert.That(finansstyringRepository, Is.Not.Null);

            IBogføringslinjeModel[] result = (await finansstyringRepository.BogføringslinjerGetAsync(1, DateTime.Now, 50)).ToArray();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count, Is.EqualTo(50));

            finansstyringKonfigurationRepositoryMock.Verify(m => m.FinansstyringServiceUri, Times.Once);
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
            Fixture fixture = new Fixture();

            Mock<IFinansstyringKonfigurationRepository> finansstyringKonfigurationRepositoryMock = new Mock<IFinansstyringKonfigurationRepository>();

            IFinansstyringRepository finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock.Object);
            Assert.That(finansstyringRepository, Is.Not.Null);

            ArgumentException exception = Assert.ThrowsAsync<ArgumentException>(async () => await finansstyringRepository.BogføringslinjeCreateNewAsync(illegalValue, DateTime.Now, fixture.Create<string>()));
            Assert.That(exception, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Does.StartWith(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "regnskabsnummer", illegalValue)));
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("regnskabsnummer"));
            Assert.That(exception.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
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
            Fixture fixture = new Fixture();

            Mock<IFinansstyringKonfigurationRepository> finansstyringKonfigurationRepositoryMock = new Mock<IFinansstyringKonfigurationRepository>();

            IFinansstyringRepository finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock.Object);
            Assert.That(finansstyringRepository, Is.Not.Null);

            DateTime dato = Convert.ToDateTime(illegalValue, new CultureInfo("en-US"));

            ArgumentException exception = Assert.ThrowsAsync<ArgumentException>(async () => await finansstyringRepository.BogføringslinjeCreateNewAsync(fixture.Create<int>(), dato, fixture.Create<string>()));
            Assert.That(exception, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Does.StartWith(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "dato", dato)));
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("dato"));
            Assert.That(exception.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Tester, at BogføringslinjeCreateNewAsync kaster en ArgumentNullException ved illegale kontonumre.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void TestAtBogføringslinjeCreateNewAsyncArgumentNullExceptionVedIllegalKontonummer(string illegalValue)
        {
            Fixture fixture = new Fixture();

            Mock<IFinansstyringKonfigurationRepository> finansstyringKonfigurationRepositoryMock = new Mock<IFinansstyringKonfigurationRepository>();

            IFinansstyringRepository finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock.Object);
            Assert.That(finansstyringRepository, Is.Not.Null);

            ArgumentNullException exception = Assert.ThrowsAsync<ArgumentNullException>(async () => await finansstyringRepository.BogføringslinjeCreateNewAsync(fixture.Create<int>(), DateTime.Now, illegalValue));
            Assert.That(exception, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("kontonummer"));
            Assert.That(exception.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Tester, at BogføringslinjeCreateNewAsync danner og returnerer en ny bogføringslinje, der efterfølgende kan bogføres.
        /// </summary>
        [Test]
        public async Task TestAtBogføringslinjeCreateNewAsyncDannerBogføringslinjeModel()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            Mock<IFinansstyringKonfigurationRepository> finansstyringKonfigurationRepositoryMock = new Mock<IFinansstyringKonfigurationRepository>();

            IFinansstyringRepository finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock.Object);
            Assert.That(finansstyringRepository, Is.Not.Null);

            int regnskabsnummer = fixture.Create<int>();
            DateTime dato = DateTime.Today.AddDays(random.Next(0, 30) * -1);
            string kontonummer = fixture.Create<string>();

            IBogføringslinjeModel bogføringslinjeModel = await finansstyringRepository.BogføringslinjeCreateNewAsync(regnskabsnummer, dato, kontonummer);
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
        }

        /// <summary>
        /// Tester, at BogførAsync kaster en ArgumentNullException, hvis bogføringslinjer er null.
        /// </summary>
        [Test]
        public void TestAtBogførAsyncKasterArgumentNullExceptionHvisBogføringslinjerErNull()
        {
            Mock<IFinansstyringKonfigurationRepository> finansstyringKonfigurationRepositoryMock = new Mock<IFinansstyringKonfigurationRepository>();

            IFinansstyringRepository finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock.Object);
            Assert.That(finansstyringRepository, Is.Not.Null);

            ArgumentNullException exception = Assert.ThrowsAsync<ArgumentNullException>(async () => await finansstyringRepository.BogførAsync(null));
            Assert.That(exception, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("bogføringslinjer"));
            Assert.That(exception.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
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
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            Mock<IFinansstyringKonfigurationRepository> finansstyringKonfigurationRepositoryMock = new Mock<IFinansstyringKonfigurationRepository>();

            IFinansstyringRepository finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock.Object);
            Assert.That(finansstyringRepository, Is.Not.Null);

            Mock<IBogføringslinjeModel> bogføringslinjeModelMock = new Mock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Setup(m => m.Regnskabsnummer)
                .Returns(illegalValue);
            bogføringslinjeModelMock.Setup(m => m.Dato)
                .Returns(DateTime.Today.AddDays(random.Next(0, 30) * -1));
            bogføringslinjeModelMock.Setup(m => m.Kontonummer)
                .Returns(fixture.Create<string>());
            bogføringslinjeModelMock.Setup(m => m.Tekst)
                .Returns(fixture.Create<string>());
            bogføringslinjeModelMock.Setup(m => m.Debit)
                .Returns(random.Next(100) > 50 ? Math.Abs(fixture.Create<decimal>()) : 0M);
            bogføringslinjeModelMock.Setup(m => m.Kredit)
                .Returns(random.Next(100) > 50 ? Math.Abs(fixture.Create<decimal>()) : 0M);

            ArgumentException exception = Assert.ThrowsAsync<ArgumentException>(async () => await finansstyringRepository.BogførAsync(bogføringslinjeModelMock.Object));
            Assert.That(exception, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Does.StartWith(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "Regnskabsnummer", illegalValue)));
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("bogføringslinjer"));
            Assert.That(exception.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
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
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            Mock<IFinansstyringKonfigurationRepository> finansstyringKonfigurationRepositoryMock = new Mock<IFinansstyringKonfigurationRepository>();

            IFinansstyringRepository finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock.Object);
            Assert.That(finansstyringRepository, Is.Not.Null);

            DateTime dato = Convert.ToDateTime(illegalValue, new CultureInfo("en-US"));
            Mock<IBogføringslinjeModel> bogføringslinjeModelMock = new Mock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Setup(m => m.Regnskabsnummer)
                .Returns(Math.Abs(fixture.Create<int>()));
            bogføringslinjeModelMock.Setup(m => m.Dato)
                .Returns(dato);
            bogføringslinjeModelMock.Setup(m => m.Kontonummer)
                .Returns(fixture.Create<string>());
            bogføringslinjeModelMock.Setup(m => m.Tekst)
                .Returns(fixture.Create<string>());
            bogføringslinjeModelMock.Setup(m => m.Debit)
                .Returns(random.Next(100) > 50 ? Math.Abs(fixture.Create<decimal>()) : 0M);
            bogføringslinjeModelMock.Setup(m => m.Kredit)
                .Returns(random.Next(100) > 50 ? Math.Abs(fixture.Create<decimal>()) : 0M);

            ArgumentException exception = Assert.ThrowsAsync<ArgumentException>(async () => await finansstyringRepository.BogførAsync(bogføringslinjeModelMock.Object));
            Assert.That(exception, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Does.StartWith(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "Dato", dato)));
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("bogføringslinjer"));
            Assert.That(exception.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Tester, at BogførAsync kaster en ArgumentException ved illegale kontonumre.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void TestAtBogførAsyncKasterArgumentExceptionVedIllegalKontonummer(string illegalValue)
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            Mock<IFinansstyringKonfigurationRepository> finansstyringKonfigurationRepositoryMock = new Mock<IFinansstyringKonfigurationRepository>();

            IFinansstyringRepository finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock.Object);
            Assert.That(finansstyringRepository, Is.Not.Null);

            Mock<IBogføringslinjeModel> bogføringslinjeModelMock = new Mock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Setup(m => m.Regnskabsnummer)
                .Returns(Math.Abs(fixture.Create<int>()));
            bogføringslinjeModelMock.Setup(m => m.Dato)
                .Returns(DateTime.Today.AddDays(random.Next(0, 30) * -1));
            bogføringslinjeModelMock.Setup(m => m.Kontonummer)
                .Returns(illegalValue);
            bogføringslinjeModelMock.Setup(m => m.Tekst)
                .Returns(fixture.Create<string>());
            bogføringslinjeModelMock.Setup(m => m.Debit)
                .Returns(random.Next(100) > 50 ? Math.Abs(fixture.Create<decimal>()) : 0M);
            bogføringslinjeModelMock.Setup(m => m.Kredit)
                .Returns(random.Next(100) > 50 ? Math.Abs(fixture.Create<decimal>()) : 0M);

            ArgumentException exception = Assert.ThrowsAsync<ArgumentException>(async () => await finansstyringRepository.BogførAsync(bogføringslinjeModelMock.Object));
            Assert.That(exception, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Does.StartWith(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "Kontonummer", illegalValue)));
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("bogføringslinjer"));
            Assert.That(exception.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Tester, at BogførAsync kaster en ArgumentException ved illegale tekster.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void TestAtBogførAsyncKasterArgumentExceptionVedIllegalTekst(string illegalValue)
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            Mock<IFinansstyringKonfigurationRepository> finansstyringKonfigurationRepositoryMock = new Mock<IFinansstyringKonfigurationRepository>();

            IFinansstyringRepository finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock.Object);
            Assert.That(finansstyringRepository, Is.Not.Null);

            Mock<IBogføringslinjeModel> bogføringslinjeModelMock = new Mock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Setup(m => m.Regnskabsnummer)
                .Returns(Math.Abs(fixture.Create<int>()));
            bogføringslinjeModelMock.Setup(m => m.Dato)
                .Returns(DateTime.Today.AddDays(random.Next(0, 30) * -1));
            bogføringslinjeModelMock.Setup(m => m.Kontonummer)
                .Returns(fixture.Create<string>());
            bogføringslinjeModelMock.Setup(m => m.Tekst)
                .Returns(illegalValue);
            bogføringslinjeModelMock.Setup(m => m.Debit)
                .Returns(random.Next(100) > 50 ? Math.Abs(fixture.Create<decimal>()) : 0M);
            bogføringslinjeModelMock.Setup(m => m.Kredit)
                .Returns(random.Next(100) > 50 ? Math.Abs(fixture.Create<decimal>()) : 0M);

            ArgumentException exception = Assert.ThrowsAsync<ArgumentException>(async () => await finansstyringRepository.BogførAsync(bogføringslinjeModelMock.Object));
            Assert.That(exception, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Does.StartWith(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "Tekst", illegalValue)));
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("bogføringslinjer"));
            Assert.That(exception.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Tester, at BogførAsync kaster en ArgumentException ved illegale debitbeløb.
        /// </summary>
        [Test]
        [TestCase(-1024)]
        [TestCase(-1)]
        public void TestAtBogførAsyncKasterArgumentExceptionVedIllegalDebit(decimal illegalValue)
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            Mock<IFinansstyringKonfigurationRepository> finansstyringKonfigurationRepositoryMock = new Mock<IFinansstyringKonfigurationRepository>();

            IFinansstyringRepository finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock.Object);
            Assert.That(finansstyringRepository, Is.Not.Null);

            Mock<IBogføringslinjeModel> bogføringslinjeModelMock = new Mock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Setup(m => m.Regnskabsnummer)
                .Returns(Math.Abs(fixture.Create<int>()));
            bogføringslinjeModelMock.Setup(m => m.Dato)
                .Returns(DateTime.Today.AddDays(random.Next(0, 30) * -1));
            bogføringslinjeModelMock.Setup(m => m.Kontonummer)
                .Returns(fixture.Create<string>());
            bogføringslinjeModelMock.Setup(m => m.Tekst)
                .Returns(fixture.Create<string>());
            bogføringslinjeModelMock.Setup(m => m.Debit)
                .Returns(illegalValue);
            bogføringslinjeModelMock.Setup(m => m.Kredit)
                .Returns(random.Next(100) > 50 ? Math.Abs(fixture.Create<decimal>()) : 0M);

            ArgumentException exception = Assert.ThrowsAsync<ArgumentException>(async () => await finansstyringRepository.BogførAsync(bogføringslinjeModelMock.Object));
            Assert.That(exception, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Does.StartWith(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "Debit", illegalValue)));
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("bogføringslinjer"));
            Assert.That(exception.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Tester, at BogførAsync kaster en ArgumentException ved illegale kreditbeløb.
        /// </summary>
        [Test]
        [TestCase(-1024)]
        [TestCase(-1)]
        public void TestAtBogførAsyncKasterArgumentExceptionVedIllegalKredit(decimal illegalValue)
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            Mock<IFinansstyringKonfigurationRepository> finansstyringKonfigurationRepositoryMock = new Mock<IFinansstyringKonfigurationRepository>();

            IFinansstyringRepository finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock.Object);
            Assert.That(finansstyringRepository, Is.Not.Null);

            Mock<IBogføringslinjeModel> bogføringslinjeModelMock = new Mock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Setup(m => m.Regnskabsnummer)
                .Returns(Math.Abs(fixture.Create<int>()));
            bogføringslinjeModelMock.Setup(m => m.Dato)
                .Returns(DateTime.Today.AddDays(random.Next(0, 30) * -1));
            bogføringslinjeModelMock.Setup(m => m.Kontonummer)
                .Returns(fixture.Create<string>());
            bogføringslinjeModelMock.Setup(m => m.Tekst)
                .Returns(fixture.Create<string>());
            bogføringslinjeModelMock.Setup(m => m.Debit)
                .Returns(random.Next(100) > 50 ? Math.Abs(fixture.Create<decimal>()) : 0M);
            bogføringslinjeModelMock.Setup(m => m.Kredit)
                .Returns(illegalValue);

            ArgumentException exception = Assert.ThrowsAsync<ArgumentException>(async () => await finansstyringRepository.BogførAsync(bogføringslinjeModelMock.Object));
            Assert.That(exception, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Does.StartWith(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "Kredit", illegalValue)));
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("bogføringslinjer"));
            Assert.That(exception.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Tester, at BogførAsync bogfører værdier.
        /// </summary>
        [Test]
        [Ignore("Skal ikke køres")]
        public async Task TestAtBogførAsyncBogførerValues()
        {
            Mock<IFinansstyringKonfigurationRepository> finansstyringKonfigurationRepositoryMock = new Mock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Setup(m => m.FinansstyringServiceUri)
                .Returns(new Uri(FinansstyringServiceTestUri));

            IFinansstyringRepository finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock.Object);
            Assert.That(finansstyringRepository, Is.Not.Null);

            Mock<IBogføringslinjeModel> bogføringslinjeModel1Mock = new Mock<IBogføringslinjeModel>();
            bogføringslinjeModel1Mock.Setup(m => m.Regnskabsnummer)
                .Returns(1);
            bogføringslinjeModel1Mock.Setup(m => m.Dato)
                .Returns(DateTime.Today);
            bogføringslinjeModel1Mock.Setup(m => m.Bilag)
                .Returns((string)null);
            bogføringslinjeModel1Mock.Setup(m => m.Kontonummer)
                .Returns("DANKORT");
            bogføringslinjeModel1Mock.Setup(m => m.Tekst)
                .Returns("Test");
            bogføringslinjeModel1Mock.Setup(m => m.Budgetkontonummer)
                .Returns("8990");
            bogføringslinjeModel1Mock.Setup(m => m.Debit)
                .Returns(5000M);
            bogføringslinjeModel1Mock.Setup(m => m.Kredit)
                .Returns(0M);
            bogføringslinjeModel1Mock.Setup(m => m.Adressekonto)
                .Returns(0);

            Mock<IBogføringslinjeModel> bogføringslinjeModel2Mock = new Mock<IBogføringslinjeModel>();
            bogføringslinjeModel2Mock.Setup(m => m.Regnskabsnummer)
                .Returns(1);
            bogføringslinjeModel2Mock.Setup(m => m.Dato)
                .Returns(DateTime.Today);
            bogføringslinjeModel2Mock.Setup(m => m.Bilag)
                .Returns((string)null);
            bogføringslinjeModel2Mock.Setup(m => m.Kontonummer)
                .Returns("DANKORT");
            bogføringslinjeModel2Mock.Setup(m => m.Tekst)
                .Returns("Test");
            bogføringslinjeModel2Mock.Setup(m => m.Budgetkontonummer)
                .Returns("8990");
            bogføringslinjeModel2Mock.Setup(m => m.Debit)
                .Returns(0M);
            bogføringslinjeModel2Mock.Setup(m => m.Kredit)
                .Returns(5000M);
            bogføringslinjeModel2Mock.Setup(m => m.Adressekonto)
                .Returns(0);

            IBogføringsresultatModel[] bogføringsresultater = (await finansstyringRepository.BogførAsync(bogføringslinjeModel1Mock.Object, bogføringslinjeModel2Mock.Object)).ToArray();
            Assert.That(bogføringsresultater, Is.Not.Null);
            Assert.That(bogføringsresultater, Is.Not.Empty);
            Assert.That(bogføringsresultater.Count, Is.EqualTo(2));
            foreach (IBogføringsresultatModel bogføringsresultat in bogføringsresultater)
            {
                Assert.That(bogføringsresultat.Bogføringslinje, Is.Not.Null);
                Assert.That(bogføringsresultat.Bogføringsadvarsler, Is.Not.Null);
                Assert.That(bogføringsresultat.Bogføringsadvarsler, Is.Empty);
            }

            finansstyringKonfigurationRepositoryMock.Verify(m => m.FinansstyringServiceUri, Times.Once);
        }

        /// <summary>
        /// Tester, at DebitorlisteGetAsync henter listen af debitorer til et givent regnskab.
        /// </summary>
        [Test]
        public async Task TestAtDebitorlisteGetAsyncHenterDebitorliste()
        {
            Mock<IFinansstyringKonfigurationRepository> finansstyringKonfigurationRepositoryMock = new Mock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Setup(m => m.FinansstyringServiceUri)
                .Returns(new Uri(FinansstyringServiceTestUri));

            IFinansstyringRepository finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock.Object);
            Assert.That(finansstyringRepository, Is.Not.Null);

            IAdressekontoModel[] result = (await finansstyringRepository.DebitorlisteGetAsync(1, DateTime.Now)).ToArray();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.GreaterThanOrEqualTo(0));

            finansstyringKonfigurationRepositoryMock.Verify(m => m.FinansstyringServiceUri, Times.Once);
        }

        /// <summary>
        /// Tester, at KreditorlisteGetAsync henter listen af kreditorer til et givent regnskab.
        /// </summary>
        [Test]
        public async Task TestAtKreditorlisteGetAsyncHenterKreditorliste()
        {
            Mock<IFinansstyringKonfigurationRepository> finansstyringKonfigurationRepositoryMock = new Mock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Setup(m => m.FinansstyringServiceUri)
                .Returns(new Uri(FinansstyringServiceTestUri));

            IFinansstyringRepository finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock.Object);
            Assert.That(finansstyringRepository, Is.Not.Null);

            IAdressekontoModel[] result = (await finansstyringRepository.KreditorlisteGetAsync(1, DateTime.Now)).ToArray();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.GreaterThanOrEqualTo(0));

            finansstyringKonfigurationRepositoryMock.Verify(m => m.FinansstyringServiceUri, Times.Once);
        }

        /// <summary>
        /// Tester, at AdressekontolisteGetAsync henter listen af adressekonti til et givent regnskab.
        /// </summary>
        [Test]
        public async Task TestAtAdressekontolisteGetAsyncHenterAdressekontoliste()
        {
            Mock<IFinansstyringKonfigurationRepository> finansstyringKonfigurationRepositoryMock = new Mock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Setup(m => m.FinansstyringServiceUri)
                .Returns(new Uri(FinansstyringServiceTestUri));

            IFinansstyringRepository finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock.Object);
            Assert.That(finansstyringRepository, Is.Not.Null);

            IAdressekontoModel[] result = (await finansstyringRepository.AdressekontolisteGetAsync(1, DateTime.Now)).ToArray();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.GreaterThanOrEqualTo(0));

            finansstyringKonfigurationRepositoryMock.Verify(m => m.FinansstyringServiceUri, Times.Once);
        }

        /// <summary>
        /// Tester, at AdressekontoGetAsync henter en adressekonto.
        /// </summary>
        [Test]
        public async Task TestAtAdressekontoGetAsyncHenterAdressekonto()
        {
            Mock<IFinansstyringKonfigurationRepository> finansstyringKonfigurationRepositoryMock = new Mock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Setup(m => m.FinansstyringServiceUri)
                .Returns(new Uri(FinansstyringServiceTestUri));

            IFinansstyringRepository finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock.Object);
            Assert.That(finansstyringRepository, Is.Not.Null);

            DateTime statusDato = DateTime.Now;
            IAdressekontoModel adressekonto = await finansstyringRepository.AdressekontoGetAsync(1, 1, statusDato);
            Assert.That(adressekonto, Is.Not.Null);
            Assert.That(adressekonto.Regnskabsnummer, Is.EqualTo(1));
            Assert.That(adressekonto.Nummer, Is.EqualTo(1));
            Assert.That(adressekonto.StatusDato, Is.EqualTo(statusDato));

            finansstyringKonfigurationRepositoryMock.Verify(m => m.FinansstyringServiceUri, Times.Once);
        }

        /// <summary>
        /// Tester, at KontogruppelisteGetAsync henter listen af kontogrupper.
        /// </summary>
        [Test]
        public async Task TestAtKontogruppelisteGetAsyncHenterKontogrupper()
        {
            Mock<IFinansstyringKonfigurationRepository> finansstyringKonfigurationRepositoryMock = new Mock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Setup(m => m.FinansstyringServiceUri)
                .Returns(new Uri(FinansstyringServiceTestUri));

            IFinansstyringRepository finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock.Object);
            Assert.That(finansstyringRepository, Is.Not.Null);

            IKontogruppeModel[] result = (await finansstyringRepository.KontogruppelisteGetAsync()).ToArray();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);

            finansstyringKonfigurationRepositoryMock.Verify(m => m.FinansstyringServiceUri, Times.Once);
        }

        /// <summary>
        /// Tester, at BudgetkontogruppelisteGetAsync henter liste af grupper til budgetkonti.
        /// </summary>
        [Test]
        public async Task TestAtBudgetkontogruppelisteGetAsyncHenterBudgetkontogrupper()
        {
            Mock<IFinansstyringKonfigurationRepository> finansstyringKonfigurationRepositoryMock = new Mock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Setup(m => m.FinansstyringServiceUri)
                .Returns(new Uri(FinansstyringServiceTestUri));

            IFinansstyringRepository finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepositoryMock.Object);
            Assert.That(finansstyringRepository, Is.Not.Null);

            IBudgetkontogruppeModel[] result = (await finansstyringRepository.BudgetkontogruppelisteGetAsync()).ToArray();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);

            finansstyringKonfigurationRepositoryMock.Verify(m => m.FinansstyringServiceUri, Times.Once);
        }
    }
}