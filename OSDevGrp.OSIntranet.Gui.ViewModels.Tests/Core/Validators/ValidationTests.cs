using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
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

        /// <summary>
        /// Tester, at ValidateDate returnerer Success ved lovlige værdier.
        /// </summary>
        [Test]
        [TestCase("1901-01-01")]
        [TestCase("1901-06-30")]
        [TestCase("1901-07-01")]
        [TestCase("1901-12-31")]
        [TestCase("2014-01-01")]
        [TestCase("2014-06-30")]
        [TestCase("2014-07-01")]
        [TestCase("2014-12-31")]
        [TestCase("2051-01-01")]
        [TestCase("2051-06-30")]
        [TestCase("2051-07-01")]
        [TestCase("2051-12-31")]
        public void TestAtValidateDateReturnererSuccessVedLovligeValues(string value)
        {
            var valueAsDateTime = DateTime.Parse(value, new CultureInfo("en-US"));
            var result = Validation.ValidateDate(valueAsDateTime.ToShortDateString());
            Assert.That(result, Is.EqualTo(ValidationResult.Success));
        }

        /// <summary>
        /// Tester, at ValidateDate returnerer ValidationResult ved ulovlige værdier.
        /// </summary>
        [TestCase(null)]
        [TestCase("")]
        [TestCase("XYZ")]
        [TestCase("ZYX")]
        [TestCase("2014-01-35")]
        [TestCase("2014-13-01")]
        public void TestAtValidateDateReturnererValidationResultVedUlovligeValues(string value)
        {
            var result = Validation.ValidateDate(value);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.EqualTo(ValidationResult.Success));
            Assert.That(result.ErrorMessage, Is.Not.Null);
            Assert.That(result.ErrorMessage, Is.Not.Empty);
            Assert.That(result.ErrorMessage, Is.EqualTo(Resource.GetText(Text.ValueIsNotDate)));
            Assert.That(result.MemberNames, Is.Not.Null);
            Assert.That(result.MemberNames, Is.Empty);
        }

        /// <summary>
        /// Tester, at validateDate returnerer Success ved lovlige værdier.
        /// </summary>
        [Test]
        [TestCase("1901-01-01", "1901-12-31")]
        [TestCase("1901-06-30", "1901-12-31")]
        [TestCase("1901-07-01", "1901-12-31")]
        [TestCase("1901-12-31", "1901-12-31")]
        [TestCase("2014-01-01", "2014-12-31")]
        [TestCase("2014-06-30", "2014-12-31")]
        [TestCase("2014-07-01", "2014-12-31")]
        [TestCase("2014-12-31", "2014-12-31")]
        [TestCase("2051-01-01", "2051-12-31")]
        [TestCase("2051-06-30", "2051-12-31")]
        [TestCase("2051-07-01", "2051-12-31")]
        [TestCase("2051-12-31", "2051-12-31")]
        public void TestAtValidateDateLowerOrEqualToReturnererSuccessVedLovligeValues(string value, string maxDate)
        {
            var valueAsDateTime = DateTime.Parse(value, new CultureInfo("en-US"));
            var maxDateTime = DateTime.Parse(maxDate, new CultureInfo("en-US"));
            var result = Validation.ValidateDateLowerOrEqualTo(valueAsDateTime.ToShortDateString(), maxDateTime);
            Assert.That(result, Is.EqualTo(ValidationResult.Success));
        }

        /// <summary>
        /// Tester, at validateDate returnerer ValidationResult ved ulovlige værdier.
        /// </summary>
        [Test]
        [TestCase("2014-01-01", "2013-12-31")]
        [TestCase("2014-06-30", "2013-12-31")]
        [TestCase("2014-07-01", "2013-12-31")]
        [TestCase("2014-12-31", "2013-12-31")]
        [TestCase("2051-01-01", "2050-12-31")]
        [TestCase("2051-06-30", "2050-12-31")]
        [TestCase("2051-07-01", "2050-12-31")]
        [TestCase("2051-12-31", "2050-12-31")]
        public void TestAtValidateDateLowerOrEqualToReturnererValidationResultVedUlovligeValues(string value, string maxDate)
        {
            var valueAsDateTime = DateTime.Parse(value, new CultureInfo("en-US"));
            var maxDateTime = DateTime.Parse(maxDate, new CultureInfo("en-US"));
            var result = Validation.ValidateDateLowerOrEqualTo(valueAsDateTime.ToShortDateString(), maxDateTime);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.EqualTo(ValidationResult.Success));
            Assert.That(result.ErrorMessage, Is.Not.Null);
            Assert.That(result.ErrorMessage, Is.Not.Empty);
            Assert.That(result.ErrorMessage, Is.EqualTo(Resource.GetText(Text.DateGreaterThan, maxDateTime.ToLongDateString())));
            Assert.That(result.MemberNames, Is.Not.Null);
            Assert.That(result.MemberNames, Is.Empty);
        }
    }
}
