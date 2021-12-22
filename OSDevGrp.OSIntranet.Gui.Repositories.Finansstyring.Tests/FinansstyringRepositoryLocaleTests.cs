using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Schema;
using AutoFixture;
using AutoFixture.Kernel;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Events;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.Resources;

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
            IFinansstyringKonfigurationRepository finansstyringKonfigurationRepositoryMock = new Mock<IFinansstyringKonfigurationRepository>().Object;

            IFinansstyringRepository finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(finansstyringKonfigurationRepositoryMock, new Mock<ILocaleDataStorage>().Object);
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
            Mock<ILocaleDataStorage> localeDataStorageMock = new Mock<ILocaleDataStorage>();
            localeDataStorageMock.Setup(m => m.HasLocaleData)
                .Returns(true);

            IFinansstyringRepository finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(new Mock<IFinansstyringKonfigurationRepository>().Object, localeDataStorageMock.Object);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            localeDataStorageMock.Verify(m => m.HasLocaleData, Times.Once);
            localeDataStorageMock.Verify(m => m.StoreLocaleData(It.IsAny<IRegnskabModel>()), Times.Never);
            localeDataStorageMock.Verify(m => m.StoreLocaleData(It.IsAny<IKontogruppeModel>()), Times.Never);
            localeDataStorageMock.Verify(m => m.StoreLocaleData(It.IsAny<IBudgetkontogruppeModel>()), Times.Never);
            localeDataStorageMock.Verify(m => m.StoreLocaleData(It.IsAny<IKontoModel>()), Times.Never);
            localeDataStorageMock.Verify(m => m.StoreLocaleData(It.IsAny<IBudgetkontoModel>()), Times.Never);
            localeDataStorageMock.Verify(m => m.StoreLocaleData(It.IsAny<IAdressekontoModel>()), Times.Never);
            localeDataStorageMock.Verify(m => m.StoreLocaleData(It.IsAny<IBogføringslinjeModel>()), Times.Never);
        }

        /// <summary>
        /// Tester, at konstruktøren initierer repositoryet, der ikke indeholdende lokale data.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererFinansstyringRepositoryLocaleUdenLokaleData()
        {
            Mock<ILocaleDataStorage> localeDataStorageMock = new Mock<ILocaleDataStorage>();
            localeDataStorageMock.Setup(m => m.HasLocaleData)
                .Returns(false);

            IFinansstyringRepository finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(new Mock<IFinansstyringKonfigurationRepository>().Object, localeDataStorageMock.Object);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            localeDataStorageMock.Verify(m => m.HasLocaleData, Times.Once);
            localeDataStorageMock.Verify(m => m.StoreLocaleData(It.IsNotNull<IRegnskabModel>()), Times.Once);
            localeDataStorageMock.Verify(m => m.StoreLocaleData(It.IsNotNull<IKontogruppeModel>()), Times.Exactly(3));
            localeDataStorageMock.Verify(m => m.StoreLocaleData(It.IsNotNull<IBudgetkontogruppeModel>()), Times.Exactly(2));
            localeDataStorageMock.Verify(m => m.StoreLocaleData(It.IsNotNull<IKontoModel>()), Times.Exactly(3 + 5 + 5));
            localeDataStorageMock.Verify(m => m.StoreLocaleData(It.IsNotNull<IBudgetkontoModel>()), Times.Exactly(2 + 7));
            localeDataStorageMock.Verify(m => m.StoreLocaleData(It.IsAny<IAdressekontoModel>()), Times.Never);
            localeDataStorageMock.Verify(m => m.StoreLocaleData(It.IsAny<IBogføringslinjeModel>()), Times.Never);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis konfigurationsrepository, der supporterer finansstyring, er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFinansstyringKonfigurationRepositoryErNull()
        {
            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new FinansstyringRepositoryLocale(null, new Mock<ILocaleDataStorage>().Object));
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
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis det lokale datalager er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisLocaleDataStorageErNull()
        {
            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new FinansstyringRepositoryLocale(new Mock<IFinansstyringKonfigurationRepository>().Object, null));
            // ReSharper restore ObjectCreationAsStatement
            Assert.That(exception, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("localeDataStorage"));
            Assert.That(exception.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Tester, at RegnskabslisteGetAsync henter regnskaber.
        /// </summary>
        [Test]
        public async Task TestAtRegnskabslisteGetAsyncHenterRegnskaber()
        {
            Fixture fixture = new Fixture();

            Mock<ILocaleDataStorage> localeDataStorageMock = new Mock<ILocaleDataStorage>();
            localeDataStorageMock.Setup(m => m.HasLocaleData)
                .Returns(true);
            localeDataStorageMock.Setup(m => m.GetLocaleData())
                .Returns(GenerateTestData(fixture));

            IFinansstyringRepository finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(new Mock<IFinansstyringKonfigurationRepository>().Object, localeDataStorageMock.Object);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            IRegnskabModel[] result = (await finansstyringRepositoryLocale.RegnskabslisteGetAsync()).ToArray();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count, Is.EqualTo(3));

            localeDataStorageMock.Verify(m => m.GetLocaleData(), Times.Once);
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
        public async Task TestAtKontoplanGetAsyncHenterKontoplan(int regnskabsnummer, int expectedKonti)
        {
            Fixture fixture = new Fixture();

            Mock<ILocaleDataStorage> localeDataStorageMock = new Mock<ILocaleDataStorage>();
            localeDataStorageMock.Setup(m => m.HasLocaleData)
                .Returns(true);
            localeDataStorageMock.Setup(m => m.GetLocaleData())
                .Returns(GenerateTestData(fixture));

            IFinansstyringRepository finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(new Mock<IFinansstyringKonfigurationRepository>().Object, localeDataStorageMock.Object);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            IKontoModel[] result = (await finansstyringRepositoryLocale.KontoplanGetAsync(regnskabsnummer, DateTime.Now)).ToArray();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(expectedKonti));

            localeDataStorageMock.Verify(m => m.GetLocaleData(), Times.AtLeastOnce);
        }

        /// <summary>
        /// Tester, at KontoGetAsync kaster en ArgumentNullException, hvis kontonummeret er invalid.
        /// </summary>
        [Test]
        [TestCase(1, null)]
        [TestCase(1, "")]
        [TestCase(1, " ")]
        [TestCase(2, null)]
        [TestCase(2, "")]
        [TestCase(2, " ")]
        [TestCase(3, null)]
        [TestCase(3, "")]
        [TestCase(3, " ")]
        [TestCase(4, null)]
        [TestCase(4, "")]
        [TestCase(4, " ")]
        [TestCase(5, null)]
        [TestCase(5, "")]
        [TestCase(5, " ")]
        public void TestAtKontoGetAsyncKasterArgumentNullExceptionHvisKontonummerErInvalid(int regnskabsnummer, string invalidKontonummer)
        {
            Mock<ILocaleDataStorage> localeDataStorageMock = new Mock<ILocaleDataStorage>();

            IFinansstyringRepository finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(new Mock<IFinansstyringKonfigurationRepository>().Object, localeDataStorageMock.Object);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            ArgumentNullException exception = Assert.ThrowsAsync<ArgumentNullException>(async () => await finansstyringRepositoryLocale.KontoGetAsync(regnskabsnummer, invalidKontonummer, DateTime.Now));
            Assert.That(exception, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("kontonummer"));
            Assert.That(exception.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
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
            Fixture fixture = new Fixture();

            Mock<ILocaleDataStorage> localeDataStorageMock = new Mock<ILocaleDataStorage>();
            localeDataStorageMock.Setup(m => m.HasLocaleData)
                .Returns(true);
            localeDataStorageMock.Setup(m => m.GetLocaleData())
                .Returns(GenerateTestData(fixture));

            IFinansstyringRepository finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(new Mock<IFinansstyringKonfigurationRepository>().Object, localeDataStorageMock.Object);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            IntranetGuiRepositoryException exception = Assert.ThrowsAsync<IntranetGuiRepositoryException>(async () => await finansstyringRepositoryLocale.KontoGetAsync(regnskabsnummer, unknownKontonummer, DateTime.Now));
            Assert.That(exception, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.AccountNotFound, unknownKontonummer)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<InvalidOperationException>());
            // ReSharper restore PossibleNullReferenceException

            localeDataStorageMock.Verify(m => m.GetLocaleData(), Times.AtLeastOnce);
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
        public async Task TestAtKontoGetAsyncHenterKontoModel(int regnskabsnummer, string kontonummer)
        {
            Fixture fixture = new Fixture();

            Mock<ILocaleDataStorage> localeDataStorageMock = new Mock<ILocaleDataStorage>();
            localeDataStorageMock.Setup(m => m.HasLocaleData)
                .Returns(true);
            localeDataStorageMock.Setup(m => m.GetLocaleData())
                .Returns(GenerateTestData(fixture));

            IFinansstyringRepository finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(new Mock<IFinansstyringKonfigurationRepository>().Object, localeDataStorageMock.Object);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            IKontoModel kontoModel = await finansstyringRepositoryLocale.KontoGetAsync(regnskabsnummer, kontonummer, DateTime.Now);
            Assert.That(kontoModel, Is.Not.Null);
            Assert.That(kontoModel.Regnskabsnummer, Is.EqualTo(regnskabsnummer));
            Assert.That(kontoModel.Kontonummer, Is.Not.Null);
            Assert.That(kontoModel.Kontonummer, Is.Not.Empty);
            Assert.That(kontoModel.Kontonummer, Is.EqualTo(kontonummer));

            localeDataStorageMock.Verify(m => m.GetLocaleData(), Times.AtLeastOnce);
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
        public async Task TestAtBudgetkontoplanGetAsyncHenterBudgetkontoplan(int regnskabsnummer, int expectedBudgetkonti)
        {
            Fixture fixture = new Fixture();

            Mock<ILocaleDataStorage> localeDataStorageMock = new Mock<ILocaleDataStorage>();
            localeDataStorageMock.Setup(m => m.HasLocaleData)
                .Returns(true);
            localeDataStorageMock.Setup(m => m.GetLocaleData())
                .Returns(GenerateTestData(fixture));

            IFinansstyringRepository finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(new Mock<IFinansstyringKonfigurationRepository>().Object, localeDataStorageMock.Object);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            IBudgetkontoModel[] result = (await finansstyringRepositoryLocale.BudgetkontoplanGetAsync(regnskabsnummer, DateTime.Now)).ToArray();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(expectedBudgetkonti));

            localeDataStorageMock.Verify(m => m.GetLocaleData(), Times.AtLeastOnce);
        }

        /// <summary>
        /// Tester, at BudgetkontoGetAsync kaster en ArgumentNullException, hvis budgetkontonummeret er invalid.
        /// </summary>
        [Test]
        [TestCase(1, null)]
        [TestCase(1, "")]
        [TestCase(1, " ")]
        [TestCase(2, null)]
        [TestCase(2, "")]
        [TestCase(2, " ")]
        [TestCase(3, null)]
        [TestCase(3, "")]
        [TestCase(3, " ")]
        [TestCase(4, null)]
        [TestCase(4, "")]
        [TestCase(4, " ")]
        [TestCase(5, null)]
        [TestCase(5, "")]
        [TestCase(5, " ")]
        public void TestAtBudgetkontoGetAsyncKasterArgumentNullExceptionHvisBudgetkontonummerErInvalid(int regnskabsnummer, string invalidBudgetkontonummer)
        {
            Mock<ILocaleDataStorage> localeDataStorageMock = new Mock<ILocaleDataStorage>();

            IFinansstyringRepository finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(new Mock<IFinansstyringKonfigurationRepository>().Object, localeDataStorageMock.Object);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            ArgumentNullException exception = Assert.ThrowsAsync<ArgumentNullException>(async () => await finansstyringRepositoryLocale.BudgetkontoGetAsync(regnskabsnummer, invalidBudgetkontonummer, DateTime.Now));
            Assert.That(exception, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("budgetkontonummer"));
            Assert.That(exception.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
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
            Fixture fixture = new Fixture();

            Mock<ILocaleDataStorage> localeDataStorageMock = new Mock<ILocaleDataStorage>();
            localeDataStorageMock.Setup(m => m.HasLocaleData)
                .Returns(true);
            localeDataStorageMock.Setup(m => m.GetLocaleData())
                .Returns(GenerateTestData(fixture));

            IFinansstyringRepository finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(new Mock<IFinansstyringKonfigurationRepository>().Object, localeDataStorageMock.Object);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            IntranetGuiRepositoryException exception = Assert.ThrowsAsync<IntranetGuiRepositoryException>(async () => await finansstyringRepositoryLocale.BudgetkontoGetAsync(regnskabsnummer, unknownBudgetkontonummer, DateTime.Now));
            Assert.That(exception, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.BudgetAccountNotFound, unknownBudgetkontonummer)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<InvalidOperationException>());
            // ReSharper restore PossibleNullReferenceException

            localeDataStorageMock.Verify(m => m.GetLocaleData(), Times.AtLeastOnce);
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
        public async Task TestAtBudgetkontoGetAsyncHenterBudgetkontoModel(int regnskabsnummer, string budgetkontonummer)
        {
            Fixture fixture = new Fixture();

            Mock<ILocaleDataStorage> localeDataStorageMock = new Mock<ILocaleDataStorage>();
            localeDataStorageMock.Setup(m => m.HasLocaleData)
                .Returns(true);
            localeDataStorageMock.Setup(m => m.GetLocaleData())
                .Returns(GenerateTestData(fixture));

            IFinansstyringRepository finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(new Mock<IFinansstyringKonfigurationRepository>().Object, localeDataStorageMock.Object);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            IBudgetkontoModel budgetkontoModel = await finansstyringRepositoryLocale.BudgetkontoGetAsync(regnskabsnummer, budgetkontonummer, DateTime.Now);
            Assert.That(budgetkontoModel, Is.Not.Null);
            Assert.That(budgetkontoModel.Regnskabsnummer, Is.EqualTo(regnskabsnummer));
            Assert.That(budgetkontoModel.Kontonummer, Is.Not.Null);
            Assert.That(budgetkontoModel.Kontonummer, Is.Not.Empty);
            Assert.That(budgetkontoModel.Kontonummer, Is.EqualTo(budgetkontonummer));

            localeDataStorageMock.Verify(m => m.GetLocaleData(), Times.AtLeastOnce);
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
        public async Task TestAtBogføringslinjerGetAsyncHenterBogføringslinjer(int regnskabsnummer, int expectedBogføringslinjer)
        {
            Fixture fixture = new Fixture();

            Mock<ILocaleDataStorage> localeDataStorageMock = new Mock<ILocaleDataStorage>();
            localeDataStorageMock.Setup(m => m.HasLocaleData)
                .Returns(true);
            localeDataStorageMock.Setup(m => m.GetLocaleData())
                .Returns(GenerateTestData(fixture));

            IFinansstyringRepository finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(new Mock<IFinansstyringKonfigurationRepository>().Object, localeDataStorageMock.Object);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            IBogføringslinjeModel[] result = (await finansstyringRepositoryLocale.BogføringslinjerGetAsync(regnskabsnummer, DateTime.Now, expectedBogføringslinjer)).ToArray();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(expectedBogføringslinjer));

            localeDataStorageMock.Verify(m => m.GetLocaleData(), Times.AtLeastOnce);
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

            Mock<ILocaleDataStorage> localeDataStorageMock = new Mock<ILocaleDataStorage>();

            IFinansstyringRepository finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(new Mock<IFinansstyringKonfigurationRepository>().Object, localeDataStorageMock.Object);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            ArgumentException exception = Assert.ThrowsAsync<ArgumentException>(async () => await finansstyringRepositoryLocale.BogføringslinjeCreateNewAsync(illegalValue, DateTime.Now, fixture.Create<string>()));
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

            Mock<ILocaleDataStorage> localeDataStorageMock = new Mock<ILocaleDataStorage>();

            IFinansstyringRepository finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(new Mock<IFinansstyringKonfigurationRepository>().Object, localeDataStorageMock.Object);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            DateTime dato = Convert.ToDateTime(illegalValue, new CultureInfo("en-US"));

            ArgumentException exception = Assert.ThrowsAsync<ArgumentException>(async () => await finansstyringRepositoryLocale.BogføringslinjeCreateNewAsync(fixture.Create<int>(), dato, fixture.Create<string>()));
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

            Mock<ILocaleDataStorage> localeDataStorageMock = new Mock<ILocaleDataStorage>();

            IFinansstyringRepository finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(new Mock<IFinansstyringKonfigurationRepository>().Object, localeDataStorageMock.Object);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            ArgumentNullException exception = Assert.ThrowsAsync<ArgumentNullException>(async () => await finansstyringRepositoryLocale.BogføringslinjeCreateNewAsync(fixture.Create<int>(), DateTime.Now, illegalValue));
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

            Mock<ILocaleDataStorage> localeDataStorageMock = new Mock<ILocaleDataStorage>();

            IFinansstyringRepository finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(new Mock<IFinansstyringKonfigurationRepository>().Object, localeDataStorageMock.Object);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            int regnskabsnummer = fixture.Create<int>();
            DateTime dato = DateTime.Today.AddDays(random.Next(0, 30) * -1);
            string kontonummer = fixture.Create<string>();

            IBogføringslinjeModel bogføringslinjeModel = await finansstyringRepositoryLocale.BogføringslinjeCreateNewAsync(regnskabsnummer, dato, kontonummer);
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
            Mock<ILocaleDataStorage> localeDataStorageMock = new Mock<ILocaleDataStorage>();

            IFinansstyringRepository finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(new Mock<IFinansstyringKonfigurationRepository>().Object, localeDataStorageMock.Object);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            ArgumentNullException exception = Assert.ThrowsAsync<ArgumentNullException>(async () => await finansstyringRepositoryLocale.BogførAsync(null));
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

            Mock<ILocaleDataStorage> localeDataStorageMock = new Mock<ILocaleDataStorage>();

            IFinansstyringRepository finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(new Mock<IFinansstyringKonfigurationRepository>().Object, localeDataStorageMock.Object);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

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

            ArgumentException exception = Assert.ThrowsAsync<ArgumentException>(async () => await finansstyringRepositoryLocale.BogførAsync(bogføringslinjeModelMock.Object));
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

            Mock<ILocaleDataStorage> localeDataStorageMock = new Mock<ILocaleDataStorage>();
            localeDataStorageMock.Setup(m => m.HasLocaleData)
                .Returns(true);

            IFinansstyringRepository finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(new Mock<IFinansstyringKonfigurationRepository>().Object, localeDataStorageMock.Object);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

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

            ArgumentException exception = Assert.ThrowsAsync<ArgumentException>(async () => await finansstyringRepositoryLocale.BogførAsync(bogføringslinjeModelMock.Object));
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

            Mock<ILocaleDataStorage> localeDataStorageMock = new Mock<ILocaleDataStorage>();
            localeDataStorageMock.Setup(m => m.HasLocaleData)
                .Returns(true);

            IFinansstyringRepository finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(new Mock<IFinansstyringKonfigurationRepository>().Object, localeDataStorageMock.Object);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

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

            ArgumentException exception = Assert.ThrowsAsync<ArgumentException>(async () => await finansstyringRepositoryLocale.BogførAsync(bogføringslinjeModelMock.Object));
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
        public void TestAtBogførAsyncKasteArgumentExceptionVedIllegalTekst(string illegalValue)
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            Mock<ILocaleDataStorage> localeDataStorageMock = new Mock<ILocaleDataStorage>();
            localeDataStorageMock.Setup(m => m.HasLocaleData)
                .Returns(true);

            IFinansstyringRepository finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(new Mock<IFinansstyringKonfigurationRepository>().Object, localeDataStorageMock.Object);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

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

            ArgumentException exception = Assert.ThrowsAsync<ArgumentException>(async () => await finansstyringRepositoryLocale.BogførAsync(bogføringslinjeModelMock.Object));
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

            Mock<ILocaleDataStorage> localeDataStorageMock = new Mock<ILocaleDataStorage>();
            localeDataStorageMock.Setup(m => m.HasLocaleData)
                .Returns(true);

            IFinansstyringRepository finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(new Mock<IFinansstyringKonfigurationRepository>().Object, localeDataStorageMock.Object);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

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

            ArgumentException exception = Assert.ThrowsAsync<ArgumentException>(async () => await finansstyringRepositoryLocale.BogførAsync(bogføringslinjeModelMock.Object));
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

            Mock<ILocaleDataStorage> localeDataStorageMock = new Mock<ILocaleDataStorage>();
            localeDataStorageMock.Setup(m => m.HasLocaleData)
                .Returns(true);

            IFinansstyringRepository finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(new Mock<IFinansstyringKonfigurationRepository>().Object, localeDataStorageMock.Object);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

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

            ArgumentException exception = Assert.ThrowsAsync<ArgumentException>(async () => await finansstyringRepositoryLocale.BogførAsync(bogføringslinjeModelMock.Object));
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
        public async Task TestAtBogførAsyncBogførerValues()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            XDocument localeDataDocument = GenerateTestData(fixture);
            Mock<ILocaleDataStorage> localeDataStorageMock = new Mock<ILocaleDataStorage>();
            localeDataStorageMock.Setup(m => m.HasLocaleData)
                .Returns(true);
            localeDataStorageMock.Setup(m => m.GetLocaleData())
                .Returns(localeDataDocument);

            IFinansstyringRepository finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(new Mock<IFinansstyringKonfigurationRepository>().Object, localeDataStorageMock.Object);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            Mock<IBogføringslinjeModel> bogføringslinjeModelMock = new Mock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Setup(m => m.Regnskabsnummer)
                .Returns(1);
            bogføringslinjeModelMock.Setup(m => m.Dato)
                .Returns(DateTime.Today.AddDays(random.Next(0, 30) * -1));
            bogføringslinjeModelMock.Setup(m => m.Bilag)
                .Returns(random.Next(100) > 50 ? fixture.Create<string>() : null);
            bogføringslinjeModelMock.Setup(m => m.Kontonummer)
                .Returns("KONTO01");
            bogføringslinjeModelMock.Setup(m => m.Tekst)
                .Returns(fixture.Create<string>());
            bogføringslinjeModelMock.Setup(m => m.Budgetkontonummer)
                .Returns(random.Next(100) > 50 ? "BUDGETKONTO01" : null);
            bogføringslinjeModelMock.Setup(m => m.Debit)
                .Returns(random.Next(100) > 50 ? Math.Abs(fixture.Create<decimal>()) : 0M);
            bogføringslinjeModelMock.Setup(m => m.Kredit)
                .Returns(random.Next(100) > 50 ? Math.Abs(fixture.Create<decimal>()) : 0M);
            bogføringslinjeModelMock.Setup(m => m.Adressekonto)
                .Returns(random.Next(100) > 50 ? 101 : 0);

            IBogføringsresultatModel bogføringsresultat = (await finansstyringRepositoryLocale.BogførAsync(bogføringslinjeModelMock.Object)).Single();
            Assert.That(bogføringsresultat, Is.Not.Null);
            Assert.That(bogføringsresultat.Bogføringslinje, Is.Not.Null);
            Assert.That(bogføringsresultat.Bogføringsadvarsler, Is.Not.Null);
            Assert.That(bogføringsresultat.Bogføringsadvarsler, Is.Empty);

            localeDataStorageMock.Verify(m => m.GetLocaleData(), Times.AtLeast(2));
            bogføringslinjeModelMock.Verify(m => m.Regnskabsnummer, Times.AtLeast(3));
            bogføringslinjeModelMock.Verify(m => m.Løbenummer, Times.Never);
            bogføringslinjeModelMock.Verify(m => m.Dato, Times.AtLeast(3));
            bogføringslinjeModelMock.Verify(m => m.Bilag, Times.AtLeast(1));
            bogføringslinjeModelMock.Verify(m => m.Kontonummer, Times.Exactly(2));
            bogføringslinjeModelMock.Verify(m => m.Tekst, Times.Exactly(2));
            bogføringslinjeModelMock.Verify(m => m.Budgetkontonummer, Times.AtLeast(1));
            bogføringslinjeModelMock.Verify(m => m.Debit, Times.Exactly(2));
            bogføringslinjeModelMock.Verify(m => m.Kredit, Times.Exactly(2));
            bogføringslinjeModelMock.Verify(m => m.Bogført, Times.Never);
            bogføringslinjeModelMock.Verify(m => m.Adressekonto, Times.AtLeast(1));
            localeDataStorageMock.Verify(m => m.StoreLocaleDocument(It.Is<XDocument>(value => value != null && value == localeDataDocument)), Times.Once);
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
        public async Task TestAtDebitorlisteGetAsyncHenterDebitorer(int regnskabsnummer, int expectedDebitorer)
        {
            Fixture fixture = new Fixture();

            Mock<ILocaleDataStorage> localeDataStorageMock = new Mock<ILocaleDataStorage>();
            localeDataStorageMock.Setup(m => m.HasLocaleData)
                .Returns(true);
            localeDataStorageMock.Setup(m => m.GetLocaleData())
                .Returns(GenerateTestData(fixture));

            IFinansstyringRepository finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(new Mock<IFinansstyringKonfigurationRepository>().Object, localeDataStorageMock.Object);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            IAdressekontoModel[] result = (await finansstyringRepositoryLocale.DebitorlisteGetAsync(regnskabsnummer, DateTime.Now)).ToArray();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(expectedDebitorer));

            localeDataStorageMock.Verify(m => m.GetLocaleData(), Times.Once);
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
        public async Task TestAtKreditorlisteGetAsyncHenterKreditorer(int regnskabsnummer, int expectedKreditorer)
        {
            Fixture fixture = new Fixture();

            Mock<ILocaleDataStorage> localeDataStorageMock = new Mock<ILocaleDataStorage>();
            localeDataStorageMock.Setup(m => m.HasLocaleData)
                .Returns(true);
            localeDataStorageMock.Setup(m => m.GetLocaleData())
                .Returns(GenerateTestData(fixture));

            IFinansstyringRepository finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(new Mock<IFinansstyringKonfigurationRepository>().Object, localeDataStorageMock.Object);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            IAdressekontoModel[] result = (await finansstyringRepositoryLocale.KreditorlisteGetAsync(regnskabsnummer, DateTime.Now)).ToArray();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(expectedKreditorer));

            localeDataStorageMock.Verify(m => m.GetLocaleData(), Times.Once);
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
        public async Task TestAtAdressekontolisteGetAsyncHenterAdressekonti(int regnskabsnummer, int expectedAdressekonti)
        {
            Fixture fixture = new Fixture();

            Mock<ILocaleDataStorage> localeDataStorageMock = new Mock<ILocaleDataStorage>();
            localeDataStorageMock.Setup(m => m.HasLocaleData)
                .Returns(true);
            localeDataStorageMock.Setup(m => m.GetLocaleData())
                .Returns(GenerateTestData(fixture));

            IFinansstyringRepository finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(new Mock<IFinansstyringKonfigurationRepository>().Object, localeDataStorageMock.Object);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            IAdressekontoModel[] result = (await finansstyringRepositoryLocale.AdressekontolisteGetAsync(regnskabsnummer, DateTime.Now)).ToArray();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(expectedAdressekonti));

            localeDataStorageMock.Verify(m => m.GetLocaleData(), Times.Once);
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
            Fixture fixture = new Fixture();

            Mock<ILocaleDataStorage> localeDataStorageMock = new Mock<ILocaleDataStorage>();
            localeDataStorageMock.Setup(m => m.HasLocaleData)
                .Returns(true);
            localeDataStorageMock.Setup(m => m.GetLocaleData())
                .Returns(GenerateTestData(fixture));

            IFinansstyringRepository finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(new Mock<IFinansstyringKonfigurationRepository>().Object, localeDataStorageMock.Object);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            IntranetGuiRepositoryException exception = Assert.ThrowsAsync<IntranetGuiRepositoryException>(async () => await finansstyringRepositoryLocale.AdressekontoGetAsync(regnskabsnummer, unknownAdressekonto, DateTime.Now));
            Assert.That(exception, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.AddressAccountNotFound, unknownAdressekonto)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<InvalidOperationException>());
            // ReSharper restore PossibleNullReferenceException

            localeDataStorageMock.Verify(m => m.GetLocaleData(), Times.Once);
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
        public async Task TestAtAdressekontoGetAsyncHenterAdressekontoModel(int regnskabsnummer, int adressekonto)
        {
            Fixture fixture = new Fixture();

            Mock<ILocaleDataStorage> localeDataStorageMock = new Mock<ILocaleDataStorage>();
            localeDataStorageMock.Setup(m => m.HasLocaleData)
                .Returns(true);
            localeDataStorageMock.Setup(m => m.GetLocaleData())
                .Returns(GenerateTestData(fixture));

            IFinansstyringRepository finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(new Mock<IFinansstyringKonfigurationRepository>().Object, localeDataStorageMock.Object);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            IAdressekontoModel adressekontoModel = await finansstyringRepositoryLocale.AdressekontoGetAsync(regnskabsnummer, adressekonto, DateTime.Now);
            Assert.That(adressekontoModel, Is.Not.Null);
            Assert.That(adressekontoModel.Regnskabsnummer, Is.EqualTo(regnskabsnummer));
            Assert.That(adressekontoModel.Nummer, Is.EqualTo(adressekonto));

            localeDataStorageMock.Verify(m => m.GetLocaleData(), Times.Once);
        }

        /// <summary>
        /// Tester, at KontogruppelisteGetAsync henter listen af kontogrupper.
        /// </summary>
        [Test]
        public async Task TestAtKontogruppelisteGetAsyncHenterKontogrupper()
        {
            Fixture fixture = new Fixture();

            Mock<ILocaleDataStorage> localeDataStorageMock = new Mock<ILocaleDataStorage>();
            localeDataStorageMock.Setup(m => m.HasLocaleData)
                .Returns(true);
            localeDataStorageMock.Setup(m => m.GetLocaleData())
                .Returns(GenerateTestData(fixture));

            IFinansstyringRepository finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(new Mock<IFinansstyringKonfigurationRepository>().Object, localeDataStorageMock.Object);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            IKontogruppeModel[] result = (await finansstyringRepositoryLocale.KontogruppelisteGetAsync()).ToArray();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count, Is.EqualTo(15));

            localeDataStorageMock.Verify(m => m.GetLocaleData(), Times.Once);
        }

        /// <summary>
        /// Tester, at BudgetkontogruppelisteGetAsync henter listen af kontogrupper til budgetkonti.
        /// </summary>
        [Test]
        public async Task TestAtBudgetkontogruppelisteGetAsyncHenterBudgetkontogrupper()
        {
            Fixture fixture = new Fixture();

            Mock<ILocaleDataStorage> localeDataStorageMock = new Mock<ILocaleDataStorage>();
            localeDataStorageMock.Setup(m => m.HasLocaleData)
                .Returns(true);
            localeDataStorageMock.Setup(m => m.GetLocaleData())
                .Returns(GenerateTestData(fixture));

            IFinansstyringRepository finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(new Mock<IFinansstyringKonfigurationRepository>().Object, localeDataStorageMock.Object);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            IBudgetkontogruppeModel[] result = (await finansstyringRepositoryLocale.BudgetkontogruppelisteGetAsync()).ToArray();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count, Is.EqualTo(25));

            localeDataStorageMock.Verify(m => m.GetLocaleData(), Times.Once);
        }

        /// <summary>
        /// Tester, at PrepareLocaleDataEventHandler returnerer, hvis der ikke er dannet et rodelement.
        /// </summary>
        [Test]
        [TestCase(true, false)]
        [TestCase(false, true)]
        public void TestAtPrepareLocaleDataEventHandlerReturnererHvisRootElementIkkeErDannet(bool readingContext, bool writingContext)
        {
            Fixture fixture = new Fixture();

            Mock<ILocaleDataStorage> localeDataStorageMock = new Mock<ILocaleDataStorage>();
            localeDataStorageMock.Setup(m => m.HasLocaleData)
                .Returns(true);

            IFinansstyringRepository finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(new Mock<IFinansstyringKonfigurationRepository>().Object, localeDataStorageMock.Object);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            XDocument localeDataDocument = new XDocument(new XDeclaration("1.0", "utf-8", null));
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
            Fixture fixture = new Fixture();

            Mock<ILocaleDataStorage> localeDataStorageMock = new Mock<ILocaleDataStorage>();
            localeDataStorageMock.Setup(m => m.HasLocaleData)
                .Returns(true);

            IFinansstyringRepository finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(new Mock<IFinansstyringKonfigurationRepository>().Object, localeDataStorageMock.Object);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            XDocument localeDataDocument = new XDocument(new XDeclaration("1.0", "utf-8", null));
            XAttribute versionAttribute = new XAttribute(XName.Get("version", string.Empty), fixture.Create<string>());
            XElement rootElement = new XElement(XName.Get("FinansstyringRepository", FinansstyringRepositoryLocale.Namespace));
            rootElement.Add(versionAttribute);
            localeDataDocument.Add(rootElement);

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
            Fixture fixture = new Fixture();

            Mock<ILocaleDataStorage> localeDataStorageMock = new Mock<ILocaleDataStorage>();
            localeDataStorageMock.Setup(m => m.HasLocaleData)
                .Returns(true);

            IFinansstyringRepository finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(new Mock<IFinansstyringKonfigurationRepository>().Object, localeDataStorageMock.Object);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            XDocument localeDataDocument = new XDocument(new XDeclaration("1.0", "utf-8", null));
            string versionValue = fixture.Create<string>();
            XAttribute versionAttribute = new XAttribute(XName.Get("version", string.Empty), versionValue);
            XElement rootElement = new XElement(XName.Get("FinansstyringRepository", FinansstyringRepositoryLocale.Namespace));
            rootElement.Add(versionAttribute);
            localeDataDocument.Add(rootElement);

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
            Fixture fixture = new Fixture();

            Mock<ILocaleDataStorage> localeDataStorageMock = new Mock<ILocaleDataStorage>();
            localeDataStorageMock.Setup(m => m.HasLocaleData)
                .Returns(true);

            IFinansstyringRepository finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(new Mock<IFinansstyringKonfigurationRepository>().Object, localeDataStorageMock.Object);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            XDocument localeDataDocument = new XDocument(new XDeclaration("1.0", "utf-8", null));
            XElement rootElement = new XElement(XName.Get("FinansstyringRepository", FinansstyringRepositoryLocale.Namespace));
            localeDataDocument.Add(rootElement);

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
            Fixture fixture = new Fixture();

            Mock<ILocaleDataStorage> localeDataStorageMock = new Mock<ILocaleDataStorage>();
            localeDataStorageMock.Setup(m => m.HasLocaleData)
                .Returns(true);

            IFinansstyringRepository finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(new Mock<IFinansstyringKonfigurationRepository>().Object, localeDataStorageMock.Object);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            XDocument localeDataDocument = new XDocument(new XDeclaration("1.0", "utf-8", null));
            XElement rootElement = new XElement(XName.Get("FinansstyringRepository", FinansstyringRepositoryLocale.Namespace));
            localeDataDocument.Add(rootElement);

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
            Fixture fixture = new Fixture();

            Mock<ILocaleDataStorage> localeDataStorageMock = new Mock<ILocaleDataStorage>();
            localeDataStorageMock.Setup(m => m.HasLocaleData)
                .Returns(true);

            IFinansstyringRepository finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(new Mock<IFinansstyringKonfigurationRepository>().Object, localeDataStorageMock.Object);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            XDocument localeDataDocument = new XDocument(new XDeclaration("1.0", "utf-8", null));
            localeDataDocument.Add(new XElement(XName.Get("FinansstyringRepository", FinansstyringRepositoryLocale.Namespace)));

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => localeDataStorageMock.Raise(m => m.PrepareLocaleData += null, null, new PrepareLocaleDataEventArgs(localeDataDocument, fixture.Create<bool>(), fixture.Create<bool>(), fixture.Create<bool>())));
            Assert.That(exception, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("sender"));
            Assert.That(exception.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Tester, at PrepareLocaleDataEventHandler kaster en ArgumentNullException, hvis argumenter eventet, er null
        /// </summary>
        [Test]
        public void TestAtPrepareLocaleDataEventHandlerKasterArgumentNullExceptionHvisEventArgsErNull()
        {
            Mock<ILocaleDataStorage> localeDataStorageMock = new Mock<ILocaleDataStorage>();
            localeDataStorageMock.Setup(m => m.HasLocaleData)
                .Returns(true);

            IFinansstyringRepository finansstyringRepositoryLocale = new FinansstyringRepositoryLocale(new Mock<IFinansstyringKonfigurationRepository>().Object, localeDataStorageMock.Object);
            Assert.That(finansstyringRepositoryLocale, Is.Not.Null);

            XDocument localeDataDocument = new XDocument(new XDeclaration("1.0", "utf-8", null));
            localeDataDocument.Add(new XElement(XName.Get("FinansstyringRepository", FinansstyringRepositoryLocale.Namespace)));

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => localeDataStorageMock.Raise(m => m.PrepareLocaleData += null, localeDataStorageMock, null));
            Assert.That(exception, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("eventArgs"));
            Assert.That(exception.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
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
                throw new ArgumentNullException(nameof(fixture));
            }

            lock (SyncRoot)
            {
                if (_localeDataDocument != null)
                {
                    return _localeDataDocument;
                }

                // Create testdata.
                Random random = new Random(fixture.Create<int>());
                XDocument localeDataDocument = new XDocument(new XDeclaration("1.0", "utf-8", null));
                XElement rootElement = new XElement(XName.Get("FinansstyringRepository", FinansstyringRepositoryLocale.Namespace));
                rootElement.Add(new XAttribute(XName.Get("version", string.Empty), "1.0"));
                localeDataDocument.Add(rootElement);
                for (int i = 0; i < 3; i++)
                {
                    IRegnskabModel regnskabModel = fixture.BuildRegnskabModel(i + 1);
                    regnskabModel.StoreInDocument(localeDataDocument);

                    for (int j = 0; j < 15; j++)
                    {
                        IKontoModel kontoModel = fixture.BuildKontoModel(random, i + 1, $"KONTO{j + 1:00}", DateTime.Now.Millisecond % 15 + 1);
                        kontoModel.StoreInDocument(localeDataDocument);
                    }

                    for (int j = 0; j < 30; j++)
                    {
                        IBudgetkontoModel budgetkontoModel = fixture.BuildBudgetkontoModel(random, i + 1, $"BUDGETKONTO{j + 1:00}", DateTime.Now.Millisecond % 25 + 1);
                        budgetkontoModel.StoreInDocument(localeDataDocument);
                    }

                    for (int j = 0; j < 25; j++)
                    {
                        IAdressekontoModel adressekontoModel = fixture.BuildAdressekontoModel(random, i + 1, 100 + j + 1, saldoOffset: 1);
                        adressekontoModel.StoreInDocument(localeDataDocument);
                    }

                    for (int j = 0; j < 15; j++)
                    {
                        IAdressekontoModel adressekontoModel = fixture.BuildAdressekontoModel(random, i + 1, 200 + j + 1, saldoOffset: -1);
                        adressekontoModel.StoreInDocument(localeDataDocument);
                    }

                    for (int j = 0; j < 150; j++)
                    {
                        IBogføringslinjeModel bogføringslinje = fixture.BuildBogføringslinjeModel(random, i + 1, j + 1, DateTime.Today.AddDays(random.Next(0, 365) * -1), $"KONTO{random.Next(1, 15):00}", random.Next(100) > 50 ? $"BUDGETKONTO{random.Next(1, 30):00}" : null, random.Next(100) > 50 ? 100 + random.Next(1, 25) : (int?)null);
                        bogføringslinje.StoreInDocument(localeDataDocument, false);
                    }
                }

                for (int i = 0; i < 15; i++)
                {
                    IKontogruppeModel kontogruppeModel = fixture.BuildKontogruppeModel(i + 1);
                    kontogruppeModel.StoreInDocument(localeDataDocument);
                }

                for (int i = 0; i < 25; i++)
                {
                    IBudgetkontogruppeModel budgetkontogruppeModel = fixture.BuildBudgetkontogruppeModel(i + 1);
                    budgetkontogruppeModel.StoreInDocument(localeDataDocument);
                }

                // Validation.
                XmlSchemaSet schemaSet = new XmlSchemaSet();
                using (MemoryStream memoryStream = new MemoryStream(Resource.GetEmbeddedResource(FinansstyringRepositoryLocale.XmlSchema)))
                {
                    schemaSet.Add(XmlSchema.Read(memoryStream, ValidationHelper.ValidationEventHandler));
                }
                localeDataDocument.Validate(schemaSet, ValidationHelper.ValidationEventHandler);

                return _localeDataDocument = localeDataDocument;
            }
        }
    }
}