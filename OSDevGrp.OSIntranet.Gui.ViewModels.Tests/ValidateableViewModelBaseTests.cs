using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests
{
    /// <summary>
    /// Tester basisfunktionalitet for en validérbar ViewModel i OS Intranet.
    /// </summary>
    [TestFixture]
    public class ValidateableViewModelBaseTests
    {
        /// <summary>
        /// Egen klasse til test af basisfunktionalitet for en validérbar ViewModel i OS Intranet.
        /// </summary>
        private class MyValidateableViewModel : ValidateableViewModelBase
        {
            #region Properties

            /// <summary>
            /// Navn for ViewModel, som kan benyttes til visning i brugergrænsefladen.
            /// </summary>
            public override string DisplayName => GetType().Name;

            #endregion

            #region Methods

            /// <summary>
            /// Returnerer valideringsfejlen til en given property.
            /// </summary>
            /// <param name="propertyName">Navn på property, hvortil valideringsfejlen skal returneres.</param>
            /// <returns>Valideringsfejl til en given property.</returns>
            public new string GetValidationError(string propertyName)
            {
                return base.GetValidationError(propertyName);
            }

            /// <summary>
            /// Sætter valideringsfejlen til en given property.
            /// </summary>
            /// <param name="propertyName">Navn på property, hvorpå valideringsfejl skal sættes.</param>
            /// <param name="validationError">Valideringsfejl.</param>
            /// <param name="raisePropertyName">Navn på den property, som returnerer valideringsfejlen.</param>
            public new void SetValidationError(string propertyName, string validationError, string raisePropertyName)
            {
                base.SetValidationError(propertyName, validationError, raisePropertyName);
            }

            #endregion
        }

        /// <summary>
        /// Tester, at konstruktøren initierer en validérbar ViewModel i OS Intranet.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererValidateableViewModelBase()
        {
            IValidateableViewModel validateableViewModel = new MyValidateableViewModel();
            Assert.That(validateableViewModel, Is.Not.Null);
            Assert.That(validateableViewModel.DisplayName, Is.Not.Null);
            Assert.That(validateableViewModel.DisplayName, Is.Not.Empty);
            Assert.That(validateableViewModel.DisplayName, Is.EqualTo(nameof(MyValidateableViewModel)));
        }

        /// <summary>
        /// Tester, at ClearValidationErrors kan kaldes.
        /// </summary>
        [Test]
        public void TestAtClearValidationErrorsNulstillerValidationErrors()
        {
            Fixture fixture = new Fixture();

            MyValidateableViewModel validateableViewModel = new MyValidateableViewModel();
            Assert.That(validateableViewModel, Is.Not.Null);

            IEnumerable<string> propertyNames = fixture.CreateMany<string>(7).ToList();
            foreach (var propertyName in propertyNames)
            {
                validateableViewModel.SetValidationError(propertyName, fixture.Create<string>(), fixture.Create<string>());

                string validationError = validateableViewModel.GetValidationError(propertyName);
                Assert.That(validationError, Is.Not.Null);
                Assert.That(validationError, Is.Not.Empty);
            }

            validateableViewModel.ClearValidationErrors();

            foreach (var propertyName in propertyNames)
            {
                string validationError = validateableViewModel.GetValidationError(propertyName);
                Assert.That(validationError, Is.Not.Null);
                Assert.That(validationError, Is.Empty);
            }
        }

        /// <summary>
        /// Tester, at GetValidationError kaster en ArgumentNullException, hvis navnet på den property, hvortil valideringsfejlen skal returneres, er invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestAtGetValidationErrorKasterArgumentNullExceptionHvisPropertyNameErInvalid(string invalidPropertyName)
        {
            MyValidateableViewModel validateableViewModel = new MyValidateableViewModel();
            Assert.That(validateableViewModel, Is.Not.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => validateableViewModel.GetValidationError(invalidPropertyName));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("propertyName"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at GetValidationError returnerer en tom streng, hvis valideringsfejlen ikke er sat på den angivne property.
        /// </summary>
        [Test]
        public void TestAtGetValidationErrorReturnererEmptyHvisValideringsfejlIkkeErSat()
        {
            Fixture fixture = new Fixture();

            MyValidateableViewModel validateableViewModel = new MyValidateableViewModel();
            Assert.That(validateableViewModel, Is.Not.Null);

            string propertyName = fixture.Create<string>();
            string validationError = validateableViewModel.GetValidationError(propertyName);
            Assert.That(validationError, Is.Not.Null);
            Assert.That(validationError, Is.Empty);
        }

        /// <summary>
        /// Tester, at GetValidationError returnerer valideringsfejlen til den angivne property.
        /// </summary>
        [Test]
        public void TestAtGetValidationErrorReturnererValideringsfejl()
        {
            Fixture fixture = new Fixture();

            MyValidateableViewModel validateableViewModel = new MyValidateableViewModel();
            Assert.That(validateableViewModel, Is.Not.Null);

            string propertyName = fixture.Create<string>();
            string validationError = fixture.Create<string>();

            validateableViewModel.SetValidationError(propertyName, validationError, fixture.Create<string>());

            string result = validateableViewModel.GetValidationError(propertyName);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(result));
        }

        /// <summary>
        /// Tester, at SetValidationError kaster en ArgumentNullException, hvis navnet på den property, hvorpå valideringsfejlen skal sættes, er invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestAtSetValidationErrorKasterArgumentNullExceptionHvisPropertyNameErInvalid(string invalidPropertyName)
        {
            Fixture fixture = new Fixture();

            MyValidateableViewModel validateableViewModel = new MyValidateableViewModel();
            Assert.That(validateableViewModel, Is.Not.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => validateableViewModel.SetValidationError(invalidPropertyName, fixture.Create<string>(), fixture.Create<string>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("propertyName"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at SetValidationError kaster en ArgumentNullException, hvis navnet på den property, der returnerer valideringsfejlen, er invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestAtSetValidationErrorKasterArgumentNullExceptionHvisRaisePropertyNameErInvalid(string invalidRaisePropertyName)
        {
            Fixture fixture = new Fixture();

            MyValidateableViewModel validateableViewModel = new MyValidateableViewModel();
            Assert.That(validateableViewModel, Is.Not.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => validateableViewModel.SetValidationError(fixture.Create<string>(), fixture.Create<string>(), invalidRaisePropertyName));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("raisePropertyName"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at SetValidationError sætter valideringsfejl til en given property, hvor valideringsfejlen ikke er sat.
        /// </summary>
        [Test]
        [TestCase("Dato", "Invalid datoformat.")]
        [TestCase("Debit", "Invalid format.")]
        [TestCase("Tekst", "Teksten skal angives.")]
        public void TestAtSetValidationErrorSetsValidationErrorHvorValideringsfejlIkkeErSat(string propertyName, string expectedValidationError)
        {
            Fixture fixture = new Fixture();

            MyValidateableViewModel validateableViewModel = new MyValidateableViewModel();
            Assert.That(validateableViewModel, Is.Not.Null);

            string validationError = validateableViewModel.GetValidationError(propertyName);
            Assert.That(validationError, Is.Not.Null);
            Assert.That(validationError, Is.Empty);

            validateableViewModel.SetValidationError(propertyName, expectedValidationError, fixture.Create<string>());
            
            validationError = validateableViewModel.GetValidationError(propertyName);
            Assert.That(validationError, Is.Not.Null);
            Assert.That(validationError, Is.Not.Empty);
            Assert.That(validationError, Is.EqualTo(expectedValidationError));
        }

        /// <summary>
        /// Tester, at SetValidationError sætter valideringsfejl til en given property, hvor valideringsfejlen allerede er sat.
        /// </summary>
        [Test]
        [TestCase("Dato", "Invalid datoformat.")]
        [TestCase("Debit", "Invalid format.")]
        [TestCase("Tekst", "Teksten skal angives.")]
        public void TestAtSetValidationErrorSetsValidationErrorHvorValideringsfejlErSat(string propertyName, string expectedValidationError)
        {
            Fixture fixture = new Fixture();

            MyValidateableViewModel validateableViewModel = new MyValidateableViewModel();
            Assert.That(validateableViewModel, Is.Not.Null);

            string validationError = validateableViewModel.GetValidationError(propertyName);
            Assert.That(validationError, Is.Not.Null);
            Assert.That(validationError, Is.Empty);

            validateableViewModel.SetValidationError(propertyName, fixture.Create<string>(), fixture.Create<string>());

            validationError = validateableViewModel.GetValidationError(propertyName);
            Assert.That(validationError, Is.Not.Null);
            Assert.That(validationError, Is.Not.Empty);
            Assert.That(validationError, Is.Not.EqualTo(expectedValidationError));

            validateableViewModel.SetValidationError(propertyName, expectedValidationError, fixture.Create<string>());

            validationError = validateableViewModel.GetValidationError(propertyName);
            Assert.That(validationError, Is.Not.Null);
            Assert.That(validationError, Is.Not.Empty);
            Assert.That(validationError, Is.EqualTo(expectedValidationError));
        }

        /// <summary>
        /// Tester, at SetValidationError nulstiller valderingsfejl på en given property, når værdi hertil angives.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestAtSetValidationErrorClearsValidationError(string clearValue)
        {
            Fixture fixture = new Fixture();

            MyValidateableViewModel validateableViewModel = new MyValidateableViewModel();
            Assert.That(validateableViewModel, Is.Not.Null);

            string propertyName = fixture.Create<string>();
            validateableViewModel.SetValidationError(propertyName, fixture.Create<string>(), fixture.Create<string>());

            string validationError = validateableViewModel.GetValidationError(propertyName);
            Assert.That(validationError, Is.Not.Null);
            Assert.That(validationError, Is.Not.Empty);

            validateableViewModel.SetValidationError(propertyName, clearValue, fixture.Create<string>());

            validationError = validateableViewModel.GetValidationError(propertyName);
            Assert.That(validationError, Is.Not.Null);
            Assert.That(validationError, Is.Empty);
        }

        /// <summary>
        /// Tester, at SetValidationError rejser PropertyChanged for den property, der returnerer valideringsfejlen.
        /// </summary>
        [Test]
        [TestCase("DatoValidationError", null)]
        [TestCase("DatoValidationError", "")]
        [TestCase("DatoValidationError", "Invalid datoformat.")]
        [TestCase("DebitValidationError", null)]
        [TestCase("DebitValidationError", "")]
        [TestCase("DebitValidationError", "Invalid format.")]
        [TestCase("TekstValidationError", null)]
        [TestCase("TekstValidationError", "")]
        [TestCase("TekstValidationError", "Teksten skal angives.")]
        public void TestAtSetValidationErrorRejserPropertyChanged(string expectedPropertyName, string validationError)
        {
            Fixture fixture = new Fixture();

            MyValidateableViewModel validateableViewModel = new MyValidateableViewModel();
            Assert.That(validateableViewModel, Is.Not.Null);

            bool eventCalled = false;
            validateableViewModel.PropertyChanged += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Empty);
                if (string.CompareOrdinal(e.PropertyName, expectedPropertyName) == 0)
                {
                    eventCalled = true;
                }
            };

            Assert.That(eventCalled, Is.False);
            validateableViewModel.SetValidationError(fixture.Create<string>(), validationError, expectedPropertyName);
            Assert.That(eventCalled, Is.True);
        }
    }
}