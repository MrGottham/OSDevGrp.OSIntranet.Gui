using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Resources;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Finansstyring.Tests
{
    /// <summary>
    /// Konfigurationsrepository, der supporterer finansstyring.
    /// </summary>
    [TestFixture]
    public class FinansstyringKonfigurationRepositoryTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer et konfigurationsrepository, der supporterer finansstyring.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererFinansstyringKonfigurationRepository()
        {
            var finansstyringKonfigurationRepository = new FinansstyringKonfigurationRepository();
            Assert.That(finansstyringKonfigurationRepository, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at Instanse returnerer en instans af konfigurationsrepository, der supporterer finansstyring.
        /// </summary>
        [Test]
        public void TestAtInstanceReturnererInstansAfFinansstyringKonfigurationRepository()
        {
            var instance = FinansstyringKonfigurationRepository.Instance;
            Assert.That(instance, Is.Not.Null);
            Assert.That(instance, Is.TypeOf<FinansstyringKonfigurationRepository>());
        }

        /// <summary>
        /// Tester, at FinansstyringServiceUri kaster en IntranetGuiRepositoryException, hvis setting for Uri til servicen, der supporterer finansstyring, mangler.
        /// </summary>
        [Test]
        public void TestAtFinansstyringServiceUriKasterIntranetGuiRepositoryExceptionHvisSettingMangler()
        {
            var finansstyringKonfigurationRepository = new FinansstyringKonfigurationRepository();
            Assert.That(finansstyringKonfigurationRepository, Is.Not.Null);

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            var exception = Assert.Throws<IntranetGuiRepositoryException>(() => finansstyringKonfigurationRepository.FinansstyringServiceUri.ToString());
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.MissingConfigurationSetting, "FinansstyringServiceUri")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at FinansstyringServiceUri kaster en IntranetGuiRepositoryException, hvis konfigurationsværiden er invalid.
        /// </summary>
        [Test]
        public void TestAtFinansstyringServiceUriKasterIntranetGuiRepositoryExceptionVedInvalidSettingValue()
        {
            var fixture = new Fixture();

            var finansstyringKonfigurationRepository = new FinansstyringKonfigurationRepository();
            Assert.That(finansstyringKonfigurationRepository, Is.Not.Null);

            var konfigurationer = new Dictionary<string, object>
                {
                    {"FinansstyringServiceUri", fixture.Create<string>()}
                };
            finansstyringKonfigurationRepository.KonfigurationerAdd(konfigurationer);

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            var exception = Assert.Throws<IntranetGuiRepositoryException>(() => finansstyringKonfigurationRepository.FinansstyringServiceUri.ToString());
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.InvalidConfigurationSettingValue, "FinansstyringServiceUri")));
            Assert.That(exception.InnerException, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at FinansstyringServiceUri returnerer konfigurationsværdi.
        /// </summary>
        [Test]
        public void TestAtFinansstyringServiceUriReturnererKonfigurationValue()
        {
            var finansstyringKonfigurationRepository = new FinansstyringKonfigurationRepository();
            Assert.That(finansstyringKonfigurationRepository, Is.Not.Null);

            const string uri = "http://www.google.dk";
            var konfigurationer = new Dictionary<string, object>
                {
                    {"FinansstyringServiceUri", uri}
                };
            finansstyringKonfigurationRepository.KonfigurationerAdd(konfigurationer);

            var result = finansstyringKonfigurationRepository.FinansstyringServiceUri;
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(new Uri(uri)));
        }

        /// <summary>
        /// Tester, at AntalBogføringslinjer kaster en IntranetGuiRepositoryException, hvis setting for Uri til servicen, der supporterer finansstyring, mangler.
        /// </summary>
        [Test]
        public void TestAtAntalBogføringslinjerKasterIntranetGuiRepositoryExceptionHvisSettingMangler()
        {
            var finansstyringKonfigurationRepository = new FinansstyringKonfigurationRepository();
            Assert.That(finansstyringKonfigurationRepository, Is.Not.Null);

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            var exception = Assert.Throws<IntranetGuiRepositoryException>(() => finansstyringKonfigurationRepository.AntalBogføringslinjer.ToString(Thread.CurrentThread.CurrentUICulture));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.MissingConfigurationSetting, "AntalBogføringslinjer")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at AntalBogføringslinjer kaster en IntranetGuiRepositoryException, hvis konfigurationsværiden er invalid.
        /// </summary>
        [Test]
        public void TestAtAntalBogføringslinjerKasterIntranetGuiRepositoryExceptionVedInvalidSettingValue()
        {
            var fixture = new Fixture();

            var finansstyringKonfigurationRepository = new FinansstyringKonfigurationRepository();
            Assert.That(finansstyringKonfigurationRepository, Is.Not.Null);

            var konfigurationer = new Dictionary<string, object>
                {
                    {"AntalBogføringslinjer", fixture.Create<string>()}
                };
            finansstyringKonfigurationRepository.KonfigurationerAdd(konfigurationer);

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            var exception = Assert.Throws<IntranetGuiRepositoryException>(() => finansstyringKonfigurationRepository.AntalBogføringslinjer.ToString(Thread.CurrentThread.CurrentUICulture));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.InvalidConfigurationSettingValue, "AntalBogføringslinjer")));
            Assert.That(exception.InnerException, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at AntalBogføringslinjer returnerer konfigurationsværdi.
        /// </summary>
        [Test]
        public void TestAtAntalBogføringslinjerReturnererKonfigurationValue()
        {
            var fixture = new Fixture();

            var finansstyringKonfigurationRepository = new FinansstyringKonfigurationRepository();
            Assert.That(finansstyringKonfigurationRepository, Is.Not.Null);

            var antalBogføringslinjer = fixture.Create<int>();
            var konfigurationer = new Dictionary<string, object>
                {
                    {"AntalBogføringslinjer", antalBogføringslinjer}
                };
            finansstyringKonfigurationRepository.KonfigurationerAdd(konfigurationer);

            var result = finansstyringKonfigurationRepository.AntalBogføringslinjer;
            Assert.That(result, Is.EqualTo(antalBogføringslinjer));
        }

        /// <summary>
        /// Tester, at DageForNyheder kaster en IntranetGuiRepositoryException, hvis setting for Uri til servicen, der supporterer finansstyring, mangler.
        /// </summary>
        [Test]
        public void TestAtDageForNyhederKasterIntranetGuiRepositoryExceptionHvisSettingMangler()
        {
            var finansstyringKonfigurationRepository = new FinansstyringKonfigurationRepository();
            Assert.That(finansstyringKonfigurationRepository, Is.Not.Null);

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            var exception = Assert.Throws<IntranetGuiRepositoryException>(() => finansstyringKonfigurationRepository.DageForNyheder.ToString(Thread.CurrentThread.CurrentUICulture));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.MissingConfigurationSetting, "DageForNyheder")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at DageForNyheder kaster en IntranetGuiRepositoryException, hvis konfigurationsværiden er invalid.
        /// </summary>
        [Test]
        public void TestAtDageForNyhederKasterIntranetGuiRepositoryExceptionVedInvalidSettingValue()
        {
            var fixture = new Fixture();

            var finansstyringKonfigurationRepository = new FinansstyringKonfigurationRepository();
            Assert.That(finansstyringKonfigurationRepository, Is.Not.Null);

            var konfigurationer = new Dictionary<string, object>
                {
                    {"DageForNyheder", fixture.Create<string>()}
                };
            finansstyringKonfigurationRepository.KonfigurationerAdd(konfigurationer);

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            var exception = Assert.Throws<IntranetGuiRepositoryException>(() => finansstyringKonfigurationRepository.DageForNyheder.ToString(Thread.CurrentThread.CurrentUICulture));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.InvalidConfigurationSettingValue, "DageForNyheder")));
            Assert.That(exception.InnerException, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at DageForNyheder returnerer konfigurationsværdi.
        /// </summary>
        [Test]
        public void TestAtDageForNyhederReturnererKonfigurationValue()
        {
            var fixture = new Fixture();

            var finansstyringKonfigurationRepository = new FinansstyringKonfigurationRepository();
            Assert.That(finansstyringKonfigurationRepository, Is.Not.Null);

            var dageForNyheder = fixture.Create<int>();
            var konfigurationer = new Dictionary<string, object>
                {
                    {"DageForNyheder", dageForNyheder}
                };
            finansstyringKonfigurationRepository.KonfigurationerAdd(konfigurationer);

            var result = finansstyringKonfigurationRepository.DageForNyheder;
            Assert.That(result, Is.EqualTo(dageForNyheder));
        }

        /// <summary>
        /// Tester, at KonfigurationerAdd kaster en ArgumentNullException, hvis konfigurationer er null.
        /// </summary>
        [Test]
        public void TestAtKonfigurationerAddKasterArgumentNullExceptionHvisKonfigurationerErNull()
        {
            var finansstyringKonfigurationRepository = new FinansstyringKonfigurationRepository();
            Assert.That(finansstyringKonfigurationRepository, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => finansstyringKonfigurationRepository.KonfigurationerAdd(null));
        }

        /// <summary>
        /// Tester, at KonfigurationerAdd tilføjer konfigurationer.
        /// </summary>
        [Test]
        public void TestAtKonfigurationerAddAddsKonfigurationer()
        {
            var fixture = new Fixture();

            var finansstyringKonfigurationRepository = new FinansstyringKonfigurationRepository();
            Assert.That(finansstyringKonfigurationRepository, Is.Not.Null);

            var konfigurationer = new Dictionary<string, object>
                {
                    {"FinansstyringServiceUri", "http://www.google.dk"},
                    {"AntalBogføringslinjer", fixture.Create<int>()},
                    {"DageForNyheder", fixture.Create<int>()}
                };
            finansstyringKonfigurationRepository.KonfigurationerAdd(konfigurationer);

            Assert.That(finansstyringKonfigurationRepository.FinansstyringServiceUri, Is.Not.Null);
            Assert.That(finansstyringKonfigurationRepository.AntalBogføringslinjer, Is.GreaterThan(0));
            Assert.That(finansstyringKonfigurationRepository.DageForNyheder, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at KonfigurationerAdd opdaterer konfigurationer.
        /// </summary>
        [Test]
        public void TestAtKonfigurationerAddUpdatesKonfigurationer()
        {
            var fixture = new Fixture();

            var finansstyringKonfigurationRepository = new FinansstyringKonfigurationRepository();
            Assert.That(finansstyringKonfigurationRepository, Is.Not.Null);

            var konfigurationer = new Dictionary<string, object>
                {
                    {"AntalBogføringslinjer", fixture.Create<int>()},
                };
            finansstyringKonfigurationRepository.KonfigurationerAdd(konfigurationer);

            Assert.That(finansstyringKonfigurationRepository.AntalBogføringslinjer, Is.GreaterThan(0));

            var newValue = finansstyringKonfigurationRepository.AntalBogføringslinjer + fixture.Create<int>();
            var newKonfigurationer = new Dictionary<string, object>
                {
                    {"AntalBogføringslinjer", newValue}
                };
            finansstyringKonfigurationRepository.KonfigurationerAdd(newKonfigurationer);

            Assert.That(finansstyringKonfigurationRepository.AntalBogføringslinjer, Is.EqualTo(newValue));
        }
    }
}
