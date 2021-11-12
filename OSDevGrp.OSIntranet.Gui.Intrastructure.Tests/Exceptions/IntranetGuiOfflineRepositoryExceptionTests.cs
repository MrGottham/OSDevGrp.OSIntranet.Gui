using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using System;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Gui.Intrastructure.Tests.Exceptions
{
    /// <summary>
    /// Tester exception, der angiver, at et repository er offline i OS Intranet.
    /// </summary>
    [TestFixture]
    public class IntranetGuiOfflineRepositoryExceptionTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en IntranetGuiOfflineRepositoryException uden en inner exception.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererIntranetGuiOfflineRepositoryExceptionUdenInnerException()
        {
            var fixture = new Fixture();

            var message = fixture.Create<string>();
            var repositoryContext = fixture.Create<object>();
            var intranetGuiOfflineRepositoryException = new IntranetGuiOfflineRepositoryException(message, repositoryContext);
            Assert.That(intranetGuiOfflineRepositoryException, Is.Not.Null);
            Assert.That(intranetGuiOfflineRepositoryException.Message, Is.Not.Null);
            Assert.That(intranetGuiOfflineRepositoryException.Message, Is.Not.Empty);
            Assert.That(intranetGuiOfflineRepositoryException.Message, Is.EqualTo(message));
            Assert.That(intranetGuiOfflineRepositoryException.RepositoryContext, Is.Not.Null);
            Assert.That(intranetGuiOfflineRepositoryException.RepositoryContext, Is.EqualTo(repositoryContext));
            Assert.That(intranetGuiOfflineRepositoryException.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren initierer en IntranetGuiOfflineRepositoryException med en inner exception.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererIntranetGuiOfflineRepositoryExceptionMedInnerException()
        {
            var fixture = new Fixture();

            var message = fixture.Create<string>();
            var repositoryContext = fixture.Create<object>();
            var innerException = fixture.Create<Exception>();
            var intranetGuiOfflineRepositoryException = new IntranetGuiOfflineRepositoryException(message, repositoryContext, innerException);
            Assert.That(intranetGuiOfflineRepositoryException, Is.Not.Null);
            Assert.That(intranetGuiOfflineRepositoryException.Message, Is.Not.Null);
            Assert.That(intranetGuiOfflineRepositoryException.Message, Is.Not.Empty);
            Assert.That(intranetGuiOfflineRepositoryException.Message, Is.EqualTo(message));
            Assert.That(intranetGuiOfflineRepositoryException.RepositoryContext, Is.Not.Null);
            Assert.That(intranetGuiOfflineRepositoryException.RepositoryContext, Is.EqualTo(repositoryContext));
            Assert.That(intranetGuiOfflineRepositoryException.InnerException, Is.Not.Null);
            Assert.That(intranetGuiOfflineRepositoryException.InnerException, Is.EqualTo(innerException));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis fejlbeskeden er invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestAtConstructorKasterArgumentNullExceptionHvisMessageErInvalid(string invalidValue)
        {
            var fixture = new Fixture();

            var exception = Assert.Throws<ArgumentNullException>(() => new IntranetGuiOfflineRepositoryException(invalidValue, fixture.Create<object>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("message"));
            Assert.That(exception.InnerException, Is.Null);

            exception = Assert.Throws<ArgumentNullException>(() => new IntranetGuiOfflineRepositoryException(invalidValue, fixture.Create<object>(), fixture.Create<Exception>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("message"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis instansen, af repositoryet, der er offline, er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisRepositoryContextErInvalid()
        {
            var fixture = new Fixture();

            var exception = Assert.Throws<ArgumentNullException>(() => new IntranetGuiOfflineRepositoryException(fixture.Create<string>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("repositoryContext"));
            Assert.That(exception.InnerException, Is.Null);

            exception = Assert.Throws<ArgumentNullException>(() => new IntranetGuiOfflineRepositoryException(fixture.Create<string>(), null, fixture.Create<Exception>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("repositoryContext"));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
