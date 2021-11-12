using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Models.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.Models.Tests.Finansstyring
{
    /// <summary>
    /// Tester modellen, der indeholder en advarsel ved en bogføring.
    /// </summary>
    [TestFixture]
    public class BogføringsadvarselModelTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en model, der indeholder en advarsel ved en bogføring.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererBogføringsadvarselModel()
        {
            var fixture = new Fixture();

            var advarsel = fixture.Create<string>();
            var kontonummer = fixture.Create<string>();
            var kontonavn = fixture.Create<string>();
            var beløb = fixture.Create<decimal>();
            var bogføringsadvarsel = new BogføringsadvarselModel(advarsel, kontonummer, kontonavn, beløb);
            Assert.That(bogføringsadvarsel, Is.Not.Null);
            Assert.That(bogføringsadvarsel.Advarsel, Is.Not.Null);
            Assert.That(bogføringsadvarsel.Advarsel, Is.Not.Empty);
            Assert.That(bogføringsadvarsel.Advarsel, Is.EqualTo(advarsel));
            Assert.That(bogføringsadvarsel.Kontonummer, Is.Not.Null);
            Assert.That(bogføringsadvarsel.Kontonummer, Is.Not.Empty);
            Assert.That(bogføringsadvarsel.Kontonummer, Is.EqualTo(kontonummer));
            Assert.That(bogføringsadvarsel.Kontonavn, Is.Not.Null);
            Assert.That(bogføringsadvarsel.Kontonavn, Is.Not.Empty);
            Assert.That(bogføringsadvarsel.Kontonavn, Is.EqualTo(kontonavn));
            Assert.That(bogføringsadvarsel.Beløb, Is.EqualTo(beløb));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis tekstangivelsen af advarslen, som er opstået ved bogføring, er invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestAtConstructorKasterArgumentNullExceptionHvisAdvarselErInvalid(string invalidValue)
        {
            var fixture = new Fixture();

            var exception = Assert.Throws<ArgumentNullException>(() => new BogføringsadvarselModel(invalidValue, fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("advarsel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis kontonummeret på kontoen, hvorpå advarslen er opstået ved bogføring, er invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestAtConstructorKasterArgumentNullExceptionHvisKontonummerErInvalid(string invalidValue)
        {
            var fixture = new Fixture();

            var exception = Assert.Throws<ArgumentNullException>(() => new BogføringsadvarselModel(fixture.Create<string>(), invalidValue, fixture.Create<string>(), fixture.Create<decimal>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("kontonummer"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis kontonavnet på kontoen, hvorpå advarslen er opstået ved bogføring, er invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestAtConstructorKasterArgumentNullExceptionHvisKontonavnErInvalid(string invalidValue)
        {
            var fixture = new Fixture();

            var exception = Assert.Throws<ArgumentNullException>(() => new BogføringsadvarselModel(fixture.Create<string>(), fixture.Create<string>(), invalidValue, fixture.Create<decimal>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("kontonavn"));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
