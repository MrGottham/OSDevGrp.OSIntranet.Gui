using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Gui.Intrastructure.Tests.Exceptions
{
    /// <summary>
    /// Tester valideringsexception fra OS Intranet.
    /// </summary>
    [TestFixture]
    public class IntranetGuiValidationExceptionTests
    {
        /// <summary>
        /// Tester, at konstruktøren danner en valideringsexception uden en inner exception.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererIntranetGuiValidationExceptionUdenInnerException()
        {
            var fixture = new Fixture();

            var message = fixture.Create<string>();
            var validationContext = fixture.Create<object>();
            var propertyName = fixture.Create<string>();
            var exception = new IntranetGuiValidationException(message, validationContext, propertyName);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(message));
            Assert.That(exception.ValidationContext, Is.Not.Null);
            Assert.That(exception.ValidationContext, Is.EqualTo(validationContext));
            Assert.That(exception.PropertyName, Is.Not.Null);
            Assert.That(exception.PropertyName, Is.Not.Empty);
            Assert.That(exception.PropertyName, Is.EqualTo(propertyName));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren danner en valideringsexception med en inner exception.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererIntranetGuiValidationExceptionMedInnerException()
        {
            var fixture = new Fixture();

            var message = fixture.Create<string>();
            var validationContext = fixture.Create<object>();
            var propertyName = fixture.Create<string>();
            var innerException = fixture.Create<Exception>();
            var exception = new IntranetGuiValidationException(message, validationContext, propertyName, innerException);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(message));
            Assert.That(exception.ValidationContext, Is.Not.Null);
            Assert.That(exception.ValidationContext, Is.EqualTo(validationContext));
            Assert.That(exception.PropertyName, Is.Not.Null);
            Assert.That(exception.PropertyName, Is.Not.Empty);
            Assert.That(exception.PropertyName, Is.EqualTo(propertyName));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.EqualTo(innerException));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis instans af objektet, hvorpå validering er fejlet, er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisValidationContextErNull()
        {
            var fixture = new Fixture();

            Assert.Throws<ArgumentNullException>(() => new IntranetGuiValidationException(fixture.Create<string>(), null, fixture.Create<string>()));
            Assert.Throws<ArgumentNullException>(() => new IntranetGuiValidationException(fixture.Create<string>(), null, fixture.Create<string>(), fixture.Create<Exception>()));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis navnet på den property, hvor validering er fejlet, er null.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestAtConstructorKasterArgumentNullExceptionHvisPropertyNameErNullOrEmpty(string illegalValue)
        {
            var fixture = new Fixture();

            Assert.Throws<ArgumentNullException>(() => new IntranetGuiValidationException(fixture.Create<string>(), fixture.Create<object>(), illegalValue));
            Assert.Throws<ArgumentNullException>(() => new IntranetGuiValidationException(fixture.Create<string>(), fixture.Create<object>(), illegalValue, fixture.Create<Exception>()));
        }
    }
}
