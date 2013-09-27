using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Gui.Intrastructure.Tests.Exceptions
{
    /// <summary>
    /// Tester systemexception fra OS Intranet.
    /// </summary>
    [TestFixture]
    public class IntranetGuiBusinessExceptionTests
    {
        /// <summary>
        /// Tester, at konstruktøren danner en forretningsexception uden en inner exception.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererIntranetGuiBusinessExceptionUdenInnerException()
        {
            var fixture = new Fixture();

            var message = fixture.Create<string>();
            var exception = new IntranetGuiBusinessException(message);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(message));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren danner en forretningsexception med en inner exception.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererIntranetGuiBusinessExceptionMedInnerException()
        {
            var fixture = new Fixture();

            var message = fixture.Create<string>();
            var innerException = fixture.Create<Exception>();
            var exception = new IntranetGuiBusinessException(message, innerException);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(message));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.EqualTo(innerException));
        }
    }
}
