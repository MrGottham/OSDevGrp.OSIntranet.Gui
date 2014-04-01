using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core.Validators;
using Text = OSDevGrp.OSIntranet.Gui.Resources.Text;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Core.Validators
{
    /// <summary>
    /// Tester basisfunktionalitet til validering.
    /// </summary>
    [TestFixture]
    public class ValidationTests
    {
        /// <summary>
        /// Tester, at ValidateRequiredValue returnerer Success ved lovlige værdier.
        /// </summary>
        [Test]
        [TestCase("XYZ")]
        [TestCase("ZYX")]
        public void TestAtValidateRequiredValueReturnererSuccessVedLovligeValues(string value)
        {
            var result = Validation.ValidateRequiredValue(value);
            Assert.That(result, Is.EqualTo(ValidationResult.Success));
        }

        /// <summary>
        /// Tester, at ValidateRequiredValue returnerer ValidationResult ved ulovlige værdier.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void TestAtValidateRequiredValueReturnererValidationResultVedUlovligeValues(string value)
        {
            var result = Validation.ValidateRequiredValue(value);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.EqualTo(ValidationResult.Success));
            Assert.That(result.ErrorMessage, Is.Not.Null);
            Assert.That(result.ErrorMessage, Is.Not.Empty);
            Assert.That(result.ErrorMessage, Is.EqualTo(Resource.GetText(Text.ValueIsRequiered)));
            Assert.That(result.MemberNames, Is.Not.Null);
            Assert.That(result.MemberNames, Is.Empty);
        }

        /// <summary>
        /// Tester, at ValidateUri returnerer Success ved lovlige værdier.
        /// </summary>
        [Test]
        [TestCase("http://localhost")]
        [TestCase("http://www.google.dk")]
        public void TestAtValidateUriReturnererSuccessVedLovligeValues(string value)
        {
            var result = Validation.ValidateUri(value);
            Assert.That(result, Is.EqualTo(ValidationResult.Success));
        }

        /// <summary>
        /// Tester, at ValidateUri returnerer ValidationResult ved ulovlige værdier.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("XYZ")]
        public void TestAtValidateUriReturnererValidationResultVedUlovligeValues(string value)
        {
            var result = Validation.ValidateUri(value);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.EqualTo(ValidationResult.Success));
            Assert.That(result.ErrorMessage, Is.Not.Null);
            Assert.That(result.ErrorMessage, Is.Not.Empty);
            Assert.That(result.ErrorMessage, Is.EqualTo(Resource.GetText(Text.InvalidValueForUri, value)));
            Assert.That(result.MemberNames, Is.Not.Null);
            Assert.That(result.MemberNames, Is.Empty);
        }

        /// <summary>
        /// Tester, at ValidateInterval returnerer Success ved lovlige værdier.
        /// </summary>
        [TestCase(0, 0, 30)]
        [TestCase(1, 0, 30)]
        [TestCase(15, 0, 30)]
        [TestCase(29, 0, 30)]
        [TestCase(30, 0, 30)]
        public void TestAtValidateIntervalReturnererSuccessVedLovligeValues(int value, int min, int max)
        {
            var result = Validation.ValidateInterval(value, min, max);
            Assert.That(result, Is.EqualTo(ValidationResult.Success));
        }

        /// <summary>
        /// Tester, at ValidateInterval returnerer ValidationResult ved ulovlige værdier.
        /// </summary>
        [Test]
        [TestCase(-10, 0, 30)]
        [TestCase(-1, 0, 30)]
        [TestCase(31, 0, 30)]
        [TestCase(40, 0, 30)]
        public void TestAtValidateIntervalReturnererValidationResultVedUlovligeValues(int value, int min, int max)
        {
            var result = Validation.ValidateInterval(value, min, max);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.EqualTo(ValidationResult.Success));
            Assert.That(result.ErrorMessage, Is.Not.Null);
            Assert.That(result.ErrorMessage, Is.Not.Empty);
            Assert.That(result.ErrorMessage, Is.EqualTo(Resource.GetText(Text.ValueOutsideInterval, min, max)));
            Assert.That(result.MemberNames, Is.Not.Null);
            Assert.That(result.MemberNames, Is.Empty);
        }
    }
}
