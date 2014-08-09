using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        /// Tester, at Keys returnerer en colllection af navne for de enkelte konfigurationsværdier.
        /// </summary>
        [Test]
        public void TestAtKeysReturnererCollectionAfNavneForKonfigurationvalues()
        {
            var keys = FinansstyringKonfigurationRepository.Keys;
            Assert.That(keys, Is.Not.Null);
            Assert.That(keys, Is.Not.Empty);

            var keyArray = keys.ToArray();
            Assert.That(keyArray.Length, Is.EqualTo(5));
            Assert.That(keyArray.Contains("FinansstyringServiceUri"), Is.True);
            Assert.That(keyArray.Contains("LokalDataFil"), Is.True);
            Assert.That(keyArray.Contains("SynkroniseringDataFil"), Is.True);
            Assert.That(keyArray.Contains("AntalBogføringslinjer"), Is.True);
            Assert.That(keyArray.Contains("DageForNyheder"), Is.True);
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
        /// Tester, at FinansstyringServiceUri kaster en IntranetGuiRepositoryException, hvis konfigurationsværdien er invalid.
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
        /// Tester, at LokalDataFil kaster en IntranetGuiRepositoryException, hvis setting for filnavnet til det lokale datalager mangler.
        /// </summary>
        [Test]
        public void TestAtLokalDataFilKasterIntranetGuiRepositoryExceptionHvisSettingMangler()
        {
            var finansstyringKonfigurationRepository = new FinansstyringKonfigurationRepository();
            Assert.That(finansstyringKonfigurationRepository, Is.Not.Null);

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            var exception = Assert.Throws<IntranetGuiRepositoryException>(() => finansstyringKonfigurationRepository.LokalDataFil.Clone());
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.MissingConfigurationSetting, "LokalDataFil")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at LokalDataFil kaster en IntranetGuiRepositoryException, hvis konfigurationsværdien er invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void TestAtLokalDataFilKasterIntranetGuiRepositoryExceptionVedInvalidSettingValue(string invalidValue)
        {
            var finansstyringKonfigurationRepository = new FinansstyringKonfigurationRepository();
            Assert.That(finansstyringKonfigurationRepository, Is.Not.Null);

            var konfigurationer = new Dictionary<string, object>
                {
                    {"LokalDataFil", invalidValue}
                };
            finansstyringKonfigurationRepository.KonfigurationerAdd(konfigurationer);

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            var exception = Assert.Throws<IntranetGuiRepositoryException>(() => finansstyringKonfigurationRepository.LokalDataFil.Clone());
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.InvalidConfigurationSettingValue, "LokalDataFil")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at LokalDataFil kaster en IntranetGuiRepositoryException, hvis mappenavnet er invalid.
        /// </summary>
        [Test]
        public void TestAtLokalDataFilKasterIntranetGuiRepositoryExceptionVedDirectoryNameErInvalid()
        {
            var rand = new Random(DateTime.Now.Millisecond);
            var illegalChars = Path.GetInvalidPathChars();
            var tempFile = new FileInfo(Path.GetTempFileName());

            char illegalChar;
            do
            {
                illegalChar = illegalChars[rand.Next(0, illegalChars.Length - 1)];
            } while (illegalChar == Path.DirectorySeparatorChar);
            var invalidFileName = string.Format("{0}{1}{2}{3}{4}", tempFile.DirectoryName, Path.DirectorySeparatorChar, Convert.ToString(illegalChar), Path.DirectorySeparatorChar, tempFile.Name);
            Debug.WriteLine(string.Format("invalidFileName={0}", invalidFileName));

            var finansstyringKonfigurationRepository = new FinansstyringKonfigurationRepository();
            Assert.That(finansstyringKonfigurationRepository, Is.Not.Null);

            var konfigurationer = new Dictionary<string, object>
                {
                    {"LokalDataFil", invalidFileName}
                };
            finansstyringKonfigurationRepository.KonfigurationerAdd(konfigurationer);

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            var exception = Assert.Throws<IntranetGuiRepositoryException>(() => finansstyringKonfigurationRepository.LokalDataFil.Clone());
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.InvalidConfigurationSettingValue, "LokalDataFil")));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<ArgumentException>());
        }

        /// <summary>
        /// Tester, at LokalDataFil kaster en IntranetGuiRepositoryException, hvis filnavnet er invalid.
        /// </summary>
        [Test]
        public void TestAtLokalDataFilKasterIntranetGuiRepositoryExceptionVedFileNameErInvalid()
        {
            var rand = new Random(DateTime.Now.Millisecond);
            var illegalChars = Path.GetInvalidFileNameChars();
            var tempFile = new FileInfo(Path.GetTempFileName());

            char illegalChar;
            do
            {
                illegalChar = illegalChars[rand.Next(0, illegalChars.Length - 1)];
            } while (illegalChar == Path.DirectorySeparatorChar || illegalChar == Path.VolumeSeparatorChar);
            var invalidFileName = string.Format("{0}{1}{2}{3}{4}", tempFile.DirectoryName, Path.DirectorySeparatorChar, Path.GetFileNameWithoutExtension(tempFile.Name), Convert.ToString(illegalChar), Path.GetExtension(tempFile.Name));
            Debug.WriteLine(string.Format("invalidFileName={0}", invalidFileName));

            var finansstyringKonfigurationRepository = new FinansstyringKonfigurationRepository();
            Assert.That(finansstyringKonfigurationRepository, Is.Not.Null);

            var konfigurationer = new Dictionary<string, object>
                {
                    {"LokalDataFil", invalidFileName}
                };
            finansstyringKonfigurationRepository.KonfigurationerAdd(konfigurationer);

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            var exception = Assert.Throws<IntranetGuiRepositoryException>(() => finansstyringKonfigurationRepository.LokalDataFil.Clone());
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.InvalidConfigurationSettingValue, "LokalDataFil")));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<ArgumentException>());
        }

        /// <summary>
        /// Tester, at LokalDataFil returnerer filnavn uden mappenavn.
        /// </summary>
        [Test]
        public void TestAtLokalDataFilReturnererFileNameUdenDirectoryName()
        {
            var tempFileName = Path.GetTempFileName();

            var finansstyringKonfigurationRepository = new FinansstyringKonfigurationRepository();
            Assert.That(finansstyringKonfigurationRepository, Is.Not.Null);

            var konfigurationer = new Dictionary<string, object>
                {
                    {"LokalDataFil", tempFileName}
                };
            finansstyringKonfigurationRepository.KonfigurationerAdd(konfigurationer);

            var result = finansstyringKonfigurationRepository.LokalDataFil;
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Contains(Path.DirectorySeparatorChar), Is.False);
        }

        /// <summary>
        /// Tester, at LokalDataFil returnerer konfigurationsværdi.
        /// </summary>
        [Test]
        public void TestAtLokalDataFilReturnererKonfigurationValue()
        {
            var tempFileName = Path.GetFileName(Path.GetTempFileName());

            var finansstyringKonfigurationRepository = new FinansstyringKonfigurationRepository();
            Assert.That(finansstyringKonfigurationRepository, Is.Not.Null);

            var konfigurationer = new Dictionary<string, object>
                {
                    {"LokalDataFil", tempFileName}
                };
            finansstyringKonfigurationRepository.KonfigurationerAdd(konfigurationer);

            var result = finansstyringKonfigurationRepository.LokalDataFil;
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(tempFileName));
        }

        /// <summary> 
        /// Tester, at SynkroniseringDataFil kaster en IntranetGuiRepositoryException, hvis setting for filnavnet til det lokale synkroniseringslager mangler.
        /// </summary>
        [Test]
        public void TestAtSynkroniseringDataFilKasterIntranetGuiRepositoryExceptionHvisSettingMangler()
        {
            var finansstyringKonfigurationRepository = new FinansstyringKonfigurationRepository();
            Assert.That(finansstyringKonfigurationRepository, Is.Not.Null);

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            var exception = Assert.Throws<IntranetGuiRepositoryException>(() => finansstyringKonfigurationRepository.SynkroniseringDataFil.Clone());
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.MissingConfigurationSetting, "SynkroniseringDataFil")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at SynkroniseringDataFil kaster en IntranetGuiRepositoryException, hvis konfigurationsværdien er invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void TestAtSynkroniseringDataFilKasterIntranetGuiRepositoryExceptionVedInvalidSettingValue(string invalidValue)
        {
            var finansstyringKonfigurationRepository = new FinansstyringKonfigurationRepository();
            Assert.That(finansstyringKonfigurationRepository, Is.Not.Null);

            var konfigurationer = new Dictionary<string, object>
                {
                    {"SynkroniseringDataFil", invalidValue}
                };
            finansstyringKonfigurationRepository.KonfigurationerAdd(konfigurationer);

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            var exception = Assert.Throws<IntranetGuiRepositoryException>(() => finansstyringKonfigurationRepository.SynkroniseringDataFil.Clone());
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.InvalidConfigurationSettingValue, "SynkroniseringDataFil")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at SynkroniseringDataFil kaster en IntranetGuiRepositoryException, hvis mappenavnet er invalid.
        /// </summary>
        [Test]
        public void TestAtSynkroniseringDataFilKasterIntranetGuiRepositoryExceptionVedDirectoryNameErInvalid()
        {
            var rand = new Random(DateTime.Now.Millisecond);
            var illegalChars = Path.GetInvalidPathChars();
            var tempFile = new FileInfo(Path.GetTempFileName());

            char illegalChar;
            do
            {
                illegalChar = illegalChars[rand.Next(0, illegalChars.Length - 1)];
            } while (illegalChar == Path.DirectorySeparatorChar);
            var invalidFileName = string.Format("{0}{1}{2}{3}{4}", tempFile.DirectoryName, Path.DirectorySeparatorChar, Convert.ToString(illegalChar), Path.DirectorySeparatorChar, tempFile.Name);
            Debug.WriteLine(string.Format("invalidFileName={0}", invalidFileName));

            var finansstyringKonfigurationRepository = new FinansstyringKonfigurationRepository();
            Assert.That(finansstyringKonfigurationRepository, Is.Not.Null);

            var konfigurationer = new Dictionary<string, object>
                {
                    {"SynkroniseringDataFil", invalidFileName}
                };
            finansstyringKonfigurationRepository.KonfigurationerAdd(konfigurationer);

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            var exception = Assert.Throws<IntranetGuiRepositoryException>(() => finansstyringKonfigurationRepository.SynkroniseringDataFil.Clone());
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.InvalidConfigurationSettingValue, "SynkroniseringDataFil")));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<ArgumentException>());
        }

        /// <summary>
        /// Tester, at SynkroniseringDataFil kaster en IntranetGuiRepositoryException, hvis filnavnet er invalid.
        /// </summary>
        [Test]
        public void TestAtSynkroniseringDataFilKasterIntranetGuiRepositoryExceptionVedFileNameErInvalid()
        {
            var rand = new Random(DateTime.Now.Millisecond);
            var illegalChars = Path.GetInvalidFileNameChars();
            var tempFile = new FileInfo(Path.GetTempFileName());

            char illegalChar;
            do
            {
                illegalChar = illegalChars[rand.Next(0, illegalChars.Length - 1)];
            } while (illegalChar == Path.DirectorySeparatorChar || illegalChar == Path.VolumeSeparatorChar);
            var invalidFileName = string.Format("{0}{1}{2}{3}{4}", tempFile.DirectoryName, Path.DirectorySeparatorChar, Path.GetFileNameWithoutExtension(tempFile.Name), Convert.ToString(illegalChar), Path.GetExtension(tempFile.Name));
            Debug.WriteLine(string.Format("invalidFileName={0}", invalidFileName));

            var finansstyringKonfigurationRepository = new FinansstyringKonfigurationRepository();
            Assert.That(finansstyringKonfigurationRepository, Is.Not.Null);

            var konfigurationer = new Dictionary<string, object>
                {
                    {"SynkroniseringDataFil", invalidFileName}
                };
            finansstyringKonfigurationRepository.KonfigurationerAdd(konfigurationer);

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            var exception = Assert.Throws<IntranetGuiRepositoryException>(() => finansstyringKonfigurationRepository.SynkroniseringDataFil.Clone());
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.InvalidConfigurationSettingValue, "SynkroniseringDataFil")));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<ArgumentException>());
        }

        /// <summary>
        /// Tester, at SynkroniseringDataFil returnerer filnavn uden mappenavn.
        /// </summary>
        [Test]
        public void TestAtSynkroniseringDataFilReturnererFileNameUdenDirectoryName()
        {
            var tempFileName = Path.GetTempFileName();

            var finansstyringKonfigurationRepository = new FinansstyringKonfigurationRepository();
            Assert.That(finansstyringKonfigurationRepository, Is.Not.Null);

            var konfigurationer = new Dictionary<string, object>
                {
                    {"SynkroniseringDataFil", tempFileName}
                };
            finansstyringKonfigurationRepository.KonfigurationerAdd(konfigurationer);

            var result = finansstyringKonfigurationRepository.SynkroniseringDataFil;
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Contains(Path.DirectorySeparatorChar), Is.False);
        }

        /// <summary>
        /// Tester, at SynkroniseringDataFil returnerer konfigurationsværdi.
        /// </summary>
        [Test]
        public void TestAtSynkroniseringDataFilReturnererKonfigurationValue()
        {
            var tempFileName = Path.GetFileName(Path.GetTempFileName());

            var finansstyringKonfigurationRepository = new FinansstyringKonfigurationRepository();
            Assert.That(finansstyringKonfigurationRepository, Is.Not.Null);

            var konfigurationer = new Dictionary<string, object>
                {
                    {"SynkroniseringDataFil", tempFileName}
                };
            finansstyringKonfigurationRepository.KonfigurationerAdd(konfigurationer);

            var result = finansstyringKonfigurationRepository.SynkroniseringDataFil;
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(tempFileName));
        }

        /// <summary>
        /// Tester, at AntalBogføringslinjer kaster en IntranetGuiRepositoryException, hvis setting for antallet af bogføringslinjer, der skal hentes, mangler.
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
        /// Tester, at AntalBogføringslinjer kaster en IntranetGuiRepositoryException, hvis konfigurationsværdien er invalid.
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
        /// Tester, at DageForNyheder kaster en IntranetGuiRepositoryException, hvis setting for antallet af dage for nyheder mangler.
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
        /// Tester, at DageForNyheder kaster en IntranetGuiRepositoryException, hvis konfigurationsværdien er invalid.
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
                    {"LokalDataFil", Path.GetTempFileName()},
                    {"SynkroniseringDataFil", Path.GetTempFileName()},
                    {"AntalBogføringslinjer", fixture.Create<int>()},
                    {"DageForNyheder", fixture.Create<int>()}
                };
            finansstyringKonfigurationRepository.KonfigurationerAdd(konfigurationer);

            Assert.That(finansstyringKonfigurationRepository.FinansstyringServiceUri, Is.Not.Null);
            Assert.That(finansstyringKonfigurationRepository.LokalDataFil, Is.Not.Null);
            Assert.That(finansstyringKonfigurationRepository.LokalDataFil, Is.Not.Empty);
            Assert.That(finansstyringKonfigurationRepository.SynkroniseringDataFil, Is.Not.Null);
            Assert.That(finansstyringKonfigurationRepository.SynkroniseringDataFil, Is.Not.Empty);
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
