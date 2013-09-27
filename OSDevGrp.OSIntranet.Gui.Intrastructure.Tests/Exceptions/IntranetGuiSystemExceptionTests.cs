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
    public class IntranetGuiSystemExceptionTests
    {
        /// <summary>
        /// Tester, at konstruktøren danner en systemexception uden en inner exception.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererIntranetGuiSystemExceptionUdenInnerException()
        {
            var fixture = new Fixture();

            var message = fixture.Create<string>();
            var exception = new IntranetGuiSystemException(message);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(message));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren danner en systemexception med en inner exception.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererIntranetGuiSystemExceptionMedInnerException()
        {
            var fixture = new Fixture();

            var message = fixture.Create<string>();
            var innerException = fixture.Create<Exception>();
            var exception = new IntranetGuiSystemException(message, innerException);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(message));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.EqualTo(innerException));
        }
    }
}
