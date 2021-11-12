using System;
using System.Xml.Linq;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Events;

namespace OSDevGrp.OSIntranet.Gui.Intrastructure.Tests.Events
{
    /// <summary>
    /// Tester argumenter til et event, der forbereder data i det lokale datalager for læsning og skrivning.
    /// </summary>
    [TestFixture]
    public class PrepareLocaleDataEventArgsTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer argumenter  til et event, der forbereder data i det lokale datalager for læsning og skrivning.
        /// </summary>
        [Test]
        [TestCase(true, false, false)]
        [TestCase(false, true, false)]
        [TestCase(false, false, true)]
        public void TestAtConstructorInitiererPrepareLocaleDataEventArgs(bool expectedReadingContext, bool expectedWritingContext, bool expectedSynchronizationContext)
        {
            var localeDataDocument = new XDocument();

            var prepareLocaleDataEventArgs = new PrepareLocaleDataEventArgs(localeDataDocument, expectedReadingContext, expectedWritingContext, expectedSynchronizationContext);
            Assert.That(prepareLocaleDataEventArgs, Is.Not.Null);
            Assert.That(prepareLocaleDataEventArgs.LocaleDataDocument, Is.Not.Null);
            Assert.That(prepareLocaleDataEventArgs.LocaleDataDocument, Is.EqualTo(localeDataDocument));
            Assert.That(prepareLocaleDataEventArgs.ReadingContext, Is.EqualTo(expectedReadingContext));
            Assert.That(prepareLocaleDataEventArgs.WritingContext, Is.EqualTo(expectedWritingContext));
            Assert.That(prepareLocaleDataEventArgs.SynchronizationContext, Is.EqualTo(expectedSynchronizationContext));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis XML dokumentet indeholdende data i det lokale datalager er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisLocaleDataDocumentErNull()
        {
            var fixture = new Fixture();

            var exception = Assert.Throws<ArgumentNullException>(() => new PrepareLocaleDataEventArgs(null, fixture.Create<bool>(), fixture.Create<bool>(), fixture.Create<bool>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("localeDataDocument"));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
