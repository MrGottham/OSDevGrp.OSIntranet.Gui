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
        [TestCase(null)]
        [TestCase("XYZ")]
        [TestCase(1)]
        [TestCase(0.5)]
        public void TestAtConstructorInitiererIntranetGuiValidationExceptionUdenInnerException(object setValue)
        {
            var fixture = new Fixture();

            var message = fixture.Create<string>();
            var validationContext = fixture.Create<object>();
            var propertyName = fixture.Create<string>();
            var value = fixture.Create<object>();
            var exception = new IntranetGuiValidationException(message, validationContext, propertyName, value);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(message));
            Assert.That(exception.ValidationContext, Is.Not.Null);
            Assert.That(exception.ValidationContext, Is.EqualTo(validationContext));
            Assert.That(exception.PropertyName, Is.Not.Null);
            Assert.That(exception.PropertyName, Is.Not.Empty);
            Assert.That(exception.PropertyName, Is.EqualTo(propertyName));
            Assert.That(exception.Value, Is.EqualTo(value));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren danner en valideringsexception med en inner exception.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("XYZ")]
        [TestCase(1)]
        [TestCase(0.5)]
        public void TestAtConstructorInitiererIntranetGuiValidationExceptionMedInnerException(object setValue)
        {
            var fixture = new Fixture();

            var message = fixture.Create<string>();
            var validationContext = fixture.Create<object>();
            var propertyName = fixture.Create<string>();
            var value = fixture.Create<object>();
            var innerException = fixture.Create<Exception>();
            var exception = new IntranetGuiValidationException(message, validationContext, propertyName, value, innerException);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(message));
            Assert.That(exception.ValidationContext, Is.Not.Null);
            Assert.That(exception.ValidationContext, Is.EqualTo(validationContext));
            Assert.That(exception.PropertyName, Is.Not.Null);
            Assert.That(exception.PropertyName, Is.Not.Empty);
            Assert.That(exception.PropertyName, Is.EqualTo(propertyName));
            Assert.That(exception.Value, Is.EqualTo(value));
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

            Assert.Throws<ArgumentNullException>(() => new IntranetGuiValidationException(fixture.Create<string>(), null, fixture.Create<string>(), fixture.Create<object>()));
            Assert.Throws<ArgumentNullException>(() => new IntranetGuiValidationException(fixture.Create<string>(), null, fixture.Create<string>(), fixture.Create<object>(), fixture.Create<Exception>()));
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

            Assert.Throws<ArgumentNullException>(() => new IntranetGuiValidationException(fixture.Create<string>(), fixture.Create<object>(), illegalValue, fixture.Create<object>()));
            Assert.Throws<ArgumentNullException>(() => new IntranetGuiValidationException(fixture.Create<string>(), fixture.Create<object>(), illegalValue, fixture.Create<object>(), fixture.Create<Exception>()));
        }
    }
}
