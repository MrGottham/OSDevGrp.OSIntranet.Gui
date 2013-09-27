using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Gui.Intrastructure.Tests.Exceptions
{
    /// <summary>
    /// Tester repositoryexception fra OS Intranet.
    /// </summary>
    [TestFixture]
    public class IntranetGuiRepositoryExceptionTests
    {
        /// <summary>
        /// Tester, at konstruktøren danner en repositoryexception uden en inner exception.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererIntranetGuiRepositoryExceptionUdenInnerException()
        {
            var fixture = new Fixture();

            var message = fixture.Create<string>();
            var exception = new IntranetGuiRepositoryException(message);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(message));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren danner en repositoryexception med en inner exception.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererIntranetGuiRepositoryExceptionMedInnerException()
        {
            var fixture = new Fixture();

            var message = fixture.Create<string>();
            var innerException = fixture.Create<Exception>();
            var exception = new IntranetGuiRepositoryException(message, innerException);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(message));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.EqualTo(innerException));
        }
    }
}
