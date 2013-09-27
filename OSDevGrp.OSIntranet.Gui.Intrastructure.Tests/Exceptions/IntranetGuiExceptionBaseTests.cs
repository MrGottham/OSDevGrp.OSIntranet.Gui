using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Gui.Intrastructure.Tests.Exceptions
{
    /// <summary>
    /// Tester basisfunktionalitet for en exception til OS Intranet.
    /// </summary>
    [TestFixture]
    public class IntranetGuiExceptionBaseTests
    {
        /// <summary>
        /// Egen klasse til test af basisfunktionalitet for en exception til OS Intranet.
        /// </summary>
        private class MyIntranetGuiException : IntranetGuiExceptionBase
        {
            #region Constructors

            /// <summary>
            /// Danner egen klasse til test af basisfunktionalitet for en exception til OS Intranet.
            /// </summary>
            /// <param name="message">Fejlbesked.</param>
            public MyIntranetGuiException(string message) 
                : base(message)
            {
            }

            /// <summary>
            /// Danner basisfunktionalitet til en exception til OS Intranet.
            /// </summary>
            /// <param name="message">Fejlbesked.</param>
            /// <param name="innerException">Inner exception.</param>
            public MyIntranetGuiException(string message, Exception innerException) 
                : base(message, innerException)
            {
            }

            #endregion
        }

        /// <summary>
        /// Tester, at konstruktøren danner en IntranetGuiExceptionBase uden en inner exception.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererIntranetGuiExceptionBaseUdenInnerException()
        {
            var fixture = new Fixture();

            var message = fixture.Create<string>();
            var exception = new MyIntranetGuiException(message);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(message));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren danner en IntranetGuiExceptionBase med en inner exception.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererIntranetGuiExceptionBaseMedInnerException()
        {
            var fixture = new Fixture();

            var message = fixture.Create<string>();
            var innerException = fixture.Create<Exception>();
            var exception = new MyIntranetGuiException(message, innerException);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(message));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.EqualTo(innerException));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis fejlbesked er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisMessageErNull()
        {
            var fixture = new Fixture();

            Assert.Throws<ArgumentNullException>(() => new MyIntranetGuiException(null));
            Assert.Throws<ArgumentNullException>(() => new MyIntranetGuiException(null, fixture.Create<Exception>()));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis fejlbesked er tom.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisMessageErEmpty()
        {
            var fixture = new Fixture();

            Assert.Throws<ArgumentNullException>(() => new MyIntranetGuiException(string.Empty));
            Assert.Throws<ArgumentNullException>(() => new MyIntranetGuiException(string.Empty, fixture.Create<Exception>()));
        }
    }
}
