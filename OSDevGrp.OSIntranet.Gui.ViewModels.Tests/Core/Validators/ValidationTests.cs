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
        }
    }
}
