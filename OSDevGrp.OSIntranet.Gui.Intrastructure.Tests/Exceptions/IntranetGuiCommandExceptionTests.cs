using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;

namespace OSDevGrp.OSIntranet.Gui.Intrastructure.Tests.Exceptions
{
    /// <summary>
    /// Tester kommandoexception fra OS Intranet.
    /// </summary>
    [TestFixture]
    public class IntranetGuiCommandExceptionTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en kommandexception fra OS Intranet uden en InnerException.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererIntranetGuiCommandExceptionUdenInnerException()
        {
            var fixture = new Fixture();
            var message = fixture.Create<string>();
            var reason = fixture.Create<string>();
            var commandContext = fixture.Create<object>();
            var reasonContext = fixture.Create<object>();
            var intranetGuiCommandException = new IntranetGuiCommandException(message, reason, commandContext, reasonContext);
            Assert.That(intranetGuiCommandException, Is.Not.Null);
            Assert.That(intranetGuiCommandException.Message, Is.Not.Null);
            Assert.That(intranetGuiCommandException.Message, Is.Not.Empty);
            Assert.That(intranetGuiCommandException.Message, Is.EqualTo(message));
            Assert.That(intranetGuiCommandException.Reason, Is.Not.Null);
            Assert.That(intranetGuiCommandException.Reason, Is.Not.Empty);
            Assert.That(intranetGuiCommandException.Reason, Is.EqualTo(reason));
            Assert.That(intranetGuiCommandException.CommandContext, Is.Not.Null);
            Assert.That(intranetGuiCommandException.CommandContext, Is.EqualTo(commandContext));
            Assert.That(intranetGuiCommandException.ReasonContext, Is.Not.Null);
            Assert.That(intranetGuiCommandException.ReasonContext, Is.EqualTo(reasonContext));
            Assert.That(intranetGuiCommandException.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren initierer en kommandexception fra OS Intranet med en InnerException.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererIntranetGuiCommandExceptionMedInnerException()
        {
            var fixture = new Fixture();
            var message = fixture.Create<string>();
            var reason = fixture.Create<string>();
            var commandContext = fixture.Create<object>();
            var reasonContext = fixture.Create<object>();
            var innerException = fixture.Create<Exception>();
            var intranetGuiCommandException = new IntranetGuiCommandException(message, reason, commandContext, reasonContext, innerException);
            Assert.That(intranetGuiCommandException, Is.Not.Null);
            Assert.That(intranetGuiCommandException.Message, Is.Not.Null);
            Assert.That(intranetGuiCommandException.Message, Is.Not.Empty);
            Assert.That(intranetGuiCommandException.Message, Is.EqualTo(message));
            Assert.That(intranetGuiCommandException.Reason, Is.Not.Null);
            Assert.That(intranetGuiCommandException.Reason, Is.Not.Empty);
            Assert.That(intranetGuiCommandException.Reason, Is.EqualTo(reason));
            Assert.That(intranetGuiCommandException.CommandContext, Is.Not.Null);
            Assert.That(intranetGuiCommandException.CommandContext, Is.EqualTo(commandContext));
            Assert.That(intranetGuiCommandException.ReasonContext, Is.Not.Null);
            Assert.That(intranetGuiCommandException.ReasonContext, Is.EqualTo(reasonContext));
            Assert.That(intranetGuiCommandException.InnerException, Is.Not.Null);
            Assert.That(intranetGuiCommandException.InnerException, Is.EqualTo(innerException));
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

            var exception = Assert.Throws<ArgumentNullException>(() => new IntranetGuiCommandException(invalidValue, fixture.Create<string>(), fixture.Create<object>(), fixture.Create<object>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("message"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis årsagen er invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestAtConstructorKasterArgumentNullExceptionHvisReasonErInvalid(string invalidValue)
        {
            var fixture = new Fixture();

            var exception = Assert.Throws<ArgumentNullException>(() => new IntranetGuiCommandException(fixture.Create<string>(), invalidValue, fixture.Create<object>(), fixture.Create<object>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("reason"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis instansen af kommandoen, hvorfra kommandoexception er kastet, er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisCommandContextErNull()
        {
            var fixture = new Fixture();

            var exception = Assert.Throws<ArgumentNullException>(() => new IntranetGuiCommandException(fixture.Create<string>(), fixture.Create<string>(), null, fixture.Create<object>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("commandContext"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis instansen af objektet, som var årsagen til, at kommandoexception blev kastet, er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisReasonContextErNull()
        {
            var fixture = new Fixture();

            var exception = Assert.Throws<ArgumentNullException>(() => new IntranetGuiCommandException(fixture.Create<string>(), fixture.Create<string>(), fixture.Create<object>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("reasonContext"));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
