using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Models.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Resources;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Gui.Models.Tests.Finansstyring
{
    /// <summary>
    /// Tester modellen til en budgetkonto.
    /// </summary>
    [TestFixture]
    public class BudgetkontoModelTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en model til en konto uden budgetteret beløb og bogført beløb.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererKontoModelUdenBudgetOgBogført()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var regnskabsnummer = fixture.Create<int>();
            var kontonummer = fixture.Create<string>();
            var kontonavn = fixture.Create<string>();
            var kontogruppe = fixture.Create<int>();
            var statusDato = fixture.Create<DateTime>();
            var budgetkontoModel = new BudgetkontoModel(regnskabsnummer, kontonummer, kontonavn, kontogruppe, statusDato);
            Assert.That(budgetkontoModel, Is.Not.Null);
            Assert.That(budgetkontoModel.Regnskabsnummer, Is.EqualTo(regnskabsnummer));
            Assert.That(budgetkontoModel.Kontonummer, Is.Not.Null);
            Assert.That(budgetkontoModel.Kontonummer, Is.Not.Empty);
            Assert.That(budgetkontoModel.Kontonummer, Is.EqualTo(kontonummer));
            Assert.That(budgetkontoModel.Kontonavn, Is.Not.Null);
            Assert.That(budgetkontoModel.Kontonavn, Is.Not.Empty);
            Assert.That(budgetkontoModel.Kontonavn, Is.EqualTo(kontonavn));
            Assert.That(budgetkontoModel.Beskrivelse, Is.Null);
            Assert.That(budgetkontoModel.Notat, Is.Null);
            Assert.That(budgetkontoModel.Kontogruppe, Is.EqualTo(kontogruppe));
            Assert.That(budgetkontoModel.StatusDato, Is.EqualTo(statusDato));
            Assert.That(budgetkontoModel.Indtægter, Is.EqualTo(0M));
            Assert.That(budgetkontoModel.Udgifter, Is.EqualTo(0M));
            Assert.That(budgetkontoModel.Budget, Is.EqualTo(0M));
            Assert.That(budgetkontoModel.Bogført, Is.EqualTo(0M));
            Assert.That(budgetkontoModel.Disponibel, Is.EqualTo(0M));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentException ved illegale regnskabsnumre.
        /// </summary>
        [Test]
        [TestCase(-1024)]
        [TestCase(-1)]
        [TestCase(0)]
        public void TestAtConstructorKasterArgumentExceptionVedIllegalRegnskabsnummer(int illegalValue)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var exception = Assert.Throws<ArgumentException>(() => new BudgetkontoModel(illegalValue, fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<decimal>(), fixture.Create<decimal>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.StringStarting(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "regnskabsnummer", illegalValue)));
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("regnskabsnummer"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException ved illegale kontonumre.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestAtConstructorKasterArgumentNullExceptionVedIllegalKontonummer(string illegalValue)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var exception = Assert.Throws<ArgumentNullException>(() => new BudgetkontoModel(fixture.Create<int>(), illegalValue, fixture.Create<string>(), fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<decimal>(), fixture.Create<decimal>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("kontonummer"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException ved illegale kontonavne.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestAtConstructorKasterArgumentNullExceptionVedIllegalKontonavn(string illegalValue)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var exception = Assert.Throws<ArgumentNullException>(() => new BudgetkontoModel(fixture.Create<int>(), fixture.Create<string>(), illegalValue, fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<decimal>(), fixture.Create<decimal>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("kontonavn"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentException ved illegale kontogrupper.
        /// </summary>
        [TestCase(-1024)]
        [TestCase(-1)]
        [TestCase(0)]
        public void TestAtConstructorKasterArgumentExceptionVedIllegalKontogruppe(int illegalValue)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var exception = Assert.Throws<ArgumentException>(() => new BudgetkontoModel(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<string>(), illegalValue, fixture.Create<DateTime>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.StringStarting(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "kontogruppe", illegalValue)));
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("kontogruppe"));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
