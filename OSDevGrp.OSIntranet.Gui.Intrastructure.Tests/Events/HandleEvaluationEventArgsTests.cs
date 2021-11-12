using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Events;

namespace OSDevGrp.OSIntranet.Gui.Intrastructure.Tests.Events
{
    /// <summary>
    /// Tester argumenter til et event, der håndterer en validering.
    /// </summary>
    [TestFixture]
    public class HandleEvaluationEventArgsTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer argumenter til et event, der håndterer en validering, uden et default valideringsresultat.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererHandleEvaluationEventArgsUdenDefaultResult()
        {
            var fixture = new Fixture();

            var validationContext = fixture.Create<object>();
            var eventArgs = new HandleEvaluationEventArgs(validationContext);
            Assert.That(eventArgs, Is.Not.Null);
            Assert.That(eventArgs.ValidationContext, Is.Not.Null);
            Assert.That(eventArgs.ValidationContext, Is.EqualTo(validationContext));
            Assert.That(eventArgs.Result, Is.False);
        }

        /// <summary>
        /// Tester, at konstruktøren initierer argumenter til et event, der håndterer en validering, med et default valideringsresultat.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestAtConstructorInitiererHandleEvaluationEventArgsMedDefaultResult(bool defaultResult)
        {
            var fixture = new Fixture();

            var validationContext = fixture.Create<object>();
            var eventArgs = new HandleEvaluationEventArgs(validationContext, defaultResult);
            Assert.That(eventArgs, Is.Not.Null);
            Assert.That(eventArgs.ValidationContext, Is.Not.Null);
            Assert.That(eventArgs.ValidationContext, Is.EqualTo(validationContext));
            Assert.That(eventArgs.Result, Is.EqualTo(defaultResult));
        }

        /// <summary>
        /// Tester, at konstruktøren uden angivelse af default valideringsresultat kaster en ArgumentNullException, hvis valideringskontekst er null.
        /// </summary>
        [Test]
        public void TestAtConstructorUdenAngivelseAfDefaultResultKasterEnArgumentNullExceptionHvisValidationContextErNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new HandleEvaluationEventArgs(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("validationContext"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren med angivelse af default valideringsresultat kaster en ArgumentNullException, hvis valideringskontekst er null.
        /// </summary>
        [Test]
        public void TestAtConstructorMedAngivelseAfDefaultResultKasterEnArgumentNullExceptionHvisValidationContextErNull()
        {
            var fixture = new Fixture();

            var exception = Assert.Throws<ArgumentNullException>(() => new HandleEvaluationEventArgs(null, fixture.Create<bool>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("validationContext"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at sætteren til Result opdaterer valideringsresultatet.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestAtResultSetterOpdatererValideringsresultat(bool testValue)
        {
            var fixture = new Fixture();

            var eventArgs = new HandleEvaluationEventArgs(fixture.Create<object>(), testValue);
            Assert.That(eventArgs, Is.Not.Null);
            Assert.That(eventArgs.Result, Is.EqualTo(testValue));

            var newValue = !testValue;
            eventArgs.Result = newValue;
            Assert.That(eventArgs.Result, Is.EqualTo(newValue));        }
    }
}
