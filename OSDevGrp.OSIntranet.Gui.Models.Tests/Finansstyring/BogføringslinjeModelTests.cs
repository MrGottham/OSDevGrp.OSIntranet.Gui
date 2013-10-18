using System;
using System.Globalization;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Models.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Core;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Gui.Models.Tests.Finansstyring
{
    /// <summary>
    /// Tester modellen for en bogføringslinje.
    /// </summary>
    [TestFixture]
    public class BogføringslinjeModelTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en model for en bogføringslinje.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererBogføringslinjeModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var regnskabsnummer = fixture.Create<int>();
            var løbenummer = fixture.Create<int>();
            var dato = fixture.Create<DateTime>();
            var bilag = fixture.Create<string>();
            var kontonummer = fixture.Create<string>();
            var tekst = fixture.Create<string>();
            var budgetkontonummer = fixture.Create<string>();
            var debit = fixture.Create<decimal>();
            var kredit = fixture.Create<decimal>();
            var adressekonto = fixture.Create<int>();
            var nyhedsinformation = string.Format("{0} {1}\r\n{2} {3}", dato.ToShortDateString(), kontonummer, tekst, (debit - kredit).ToString("C"));
            var bogføringslinjeModel = new BogføringslinjeModel(regnskabsnummer, løbenummer, dato, kontonummer, tekst, debit, kredit)
                {
                    Bilag = bilag,
                    Budgetkontonummer = budgetkontonummer,
                    Adressekonto = adressekonto
                };
            Assert.That(bogføringslinjeModel, Is.Not.Null);
            Assert.That(bogføringslinjeModel.Regnskabsnummer, Is.EqualTo(regnskabsnummer));
            Assert.That(bogføringslinjeModel.Løbenummer, Is.EqualTo(løbenummer));
            Assert.That(bogføringslinjeModel.Dato, Is.EqualTo(dato));
            Assert.That(bogføringslinjeModel.Bilag, Is.Not.Null);
            Assert.That(bogføringslinjeModel.Bilag, Is.Not.Empty);
            Assert.That(bogføringslinjeModel.Bilag, Is.EqualTo(bilag));
            Assert.That(bogføringslinjeModel.Kontonummer, Is.Not.Null);
            Assert.That(bogføringslinjeModel.Kontonummer, Is.Not.Empty);
            Assert.That(bogføringslinjeModel.Kontonummer, Is.EqualTo(kontonummer));
            Assert.That(bogføringslinjeModel.Tekst, Is.Not.Null);
            Assert.That(bogføringslinjeModel.Tekst, Is.Not.Empty);
            Assert.That(bogføringslinjeModel.Tekst, Is.EqualTo(tekst));
            Assert.That(bogføringslinjeModel.Budgetkontonummer, Is.Not.Null);
            Assert.That(bogføringslinjeModel.Budgetkontonummer, Is.Not.Empty);
            Assert.That(bogføringslinjeModel.Budgetkontonummer, Is.EqualTo(budgetkontonummer));
            Assert.That(bogføringslinjeModel.Debit, Is.EqualTo(debit));
            Assert.That(bogføringslinjeModel.Kredit, Is.EqualTo(kredit));
            Assert.That(bogføringslinjeModel.Bogført, Is.EqualTo(bogføringslinjeModel.Debit - bogføringslinjeModel.Kredit));
            Assert.That(bogføringslinjeModel.Adressekonto, Is.EqualTo(adressekonto));
            Assert.That(bogføringslinjeModel.Nyhedsaktualitet, Is.EqualTo(Nyhedsaktualitet.Medium));
            Assert.That(bogføringslinjeModel.Nyhedsudgivelsestidspunkt, Is.EqualTo(dato));
            Assert.That(bogføringslinjeModel.Nyhedsinformation, Is.Not.Null);
            Assert.That(bogføringslinjeModel.Nyhedsinformation, Is.Not.Empty);
            Assert.That(bogføringslinjeModel.Nyhedsinformation, Is.EqualTo(nyhedsinformation));
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

            Assert.Throws<ArgumentException>(() => new BogføringslinjeModel(illegalValue, fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), fixture.Create<decimal>()));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentException ved illegale løbenumre.
        /// </summary>
        [Test]
        [TestCase(-1024)]
        [TestCase(-1)]
        [TestCase(0)]
        public void TestAtConstructorKasterArgumentExceptionVedIllegalLøbenummer(int illegalValue)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            Assert.Throws<ArgumentException>(() => new BogføringslinjeModel(fixture.Create<int>(), illegalValue, fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), fixture.Create<decimal>()));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentException ved illegale bogføringsdatoer.
        /// </summary>
        [Test]
        [TestCase("2050-01-01")]
        [TestCase("2055-09-01")]
        [TestCase("2055-12-31")]
        public void TestAtConstructorKasterArgumentExceptionVedIllegalDato(string illegalValue)
        {
            var fixture = new Fixture();

            var dato = Convert.ToDateTime(illegalValue, new CultureInfo("en-US"));
            Assert.Throws<ArgumentException>(() => new BogføringslinjeModel(fixture.Create<int>(), fixture.Create<int>(), dato, fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), fixture.Create<decimal>()));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException ved illegale bogføringsdatoer.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestAtConstructorKasterArgumentNullExceptionVedIllegalKontonummer(string illegalValue)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            Assert.Throws<ArgumentNullException>(() => new BogføringslinjeModel(fixture.Create<int>(), fixture.Create<int>(), fixture.Create<DateTime>(), illegalValue, fixture.Create<string>(), fixture.Create<decimal>(), fixture.Create<decimal>()));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException ved illegale tekster.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestAtConstructorKasterArgumentNullExceptionVedIllegalTekst(string illegalValue)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            Assert.Throws<ArgumentNullException>(() => new BogføringslinjeModel(fixture.Create<int>(), fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), illegalValue, fixture.Create<decimal>(), fixture.Create<decimal>()));
        }
 
        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentException ved illegale debitbeløb.
        /// </summary>
        [Test]
        [TestCase(-1024)]
        [TestCase(-1)]
        public void TestAtConstructorKasterArgumentExceptionVedIllegalDebit(decimal illegalValue)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            Assert.Throws<ArgumentException>(() => new BogføringslinjeModel(fixture.Create<int>(), fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), illegalValue, fixture.Create<decimal>()));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentException ved illegale kreditbeløb.
        /// </summary>
        [Test]
        [TestCase(-1024)]
        [TestCase(-1)]
        public void TestAtConstructorKasterArgumentExceptionVedIllegalKredit(decimal illegalValue)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            Assert.Throws<ArgumentException>(() => new BogføringslinjeModel(fixture.Create<int>(), fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), illegalValue));
        }

        /// <summary>
        /// Tester, at sætteren til Dato kaster en ArgumentException ved illegale datoer.
        /// </summary>
        [Test]
        [TestCase("2050-01-01")]
        [TestCase("2055-09-01")]
        [TestCase("2055-12-31")]
        public void TestAtDatoSetterKasterArgumentExceptionVedIllegalValue(string illegalValue)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var bogføringslinjeModel = new BogføringslinjeModel(fixture.Create<int>(), fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), fixture.Create<decimal>());
            Assert.That(bogføringslinjeModel, Is.Not.Null);

            Assert.Throws<ArgumentException>(() => bogføringslinjeModel.Dato = Convert.ToDateTime(illegalValue, new CultureInfo("en-US")));
        }

        /// <summary>
        /// Tester, at sætteren til Dato opdaterer datoen på bogføringslinjen.
        /// </summary>
        [Test]
        public void TestAtDatoSetterOpdatererDato()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now.AddDays(-7)));

            var bogføringslinjeModel = new BogføringslinjeModel(fixture.Create<int>(), fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), fixture.Create<decimal>());
            Assert.That(bogføringslinjeModel, Is.Not.Null);

            var newValue = DateTime.Now;
            Assert.That(bogføringslinjeModel.Dato, Is.Not.EqualTo(newValue));

            bogføringslinjeModel.Dato = newValue;
            Assert.That(bogføringslinjeModel.Dato, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tester, at sætteren til Dato rejser PropertyChanged ved opdatering af dato på bogføringslinjen.
        /// </summary>
        [Test]
        [TestCase("Dato")]
        [TestCase("Nyhedsudgivelsestidspunkt")]
        [TestCase("Nyhedsinformation")]
        public void TestAtDatoSetterRejserPropertyChangedVedOpdateringAfDato(string propertyNameToRaise)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now.AddDays(-7)));

            var bogføringslinjeModel = new BogføringslinjeModel(fixture.Create<int>(), fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), fixture.Create<decimal>());
            Assert.That(bogføringslinjeModel, Is.Not.Null);

            var eventCalled = false;
            bogføringslinjeModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, propertyNameToRaise, StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            bogføringslinjeModel.Dato = bogføringslinjeModel.Dato;
            Assert.That(eventCalled, Is.False);
            bogføringslinjeModel.Dato = DateTime.Now;
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at sætteren til Bilag opdaterer bilag på bogføringslinjen.
        /// </summary>
        [Test]
        public void TestAtBilagSetterOpdatererBilag()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var bogføringslinjeModel = new BogføringslinjeModel(fixture.Create<int>(), fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), fixture.Create<decimal>());
            Assert.That(bogføringslinjeModel, Is.Not.Null);

            var newValue = fixture.Create<string>();
            Assert.That(bogføringslinjeModel.Bilag, Is.Not.EqualTo(newValue));

            bogføringslinjeModel.Bilag = newValue;
            Assert.That(bogføringslinjeModel.Bilag, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tester, at sætteren til Bilag rejser PropertyChanged ved opdatering af bilag på bogføringslinjen.
        /// </summary>
        [Test]
        [TestCase("Bilag")]
        public void TestAtBilagSetterRejserPropertyChangedVedOpdateringAfBilag(string propertyNameToRaise)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var bogføringslinjeModel = new BogføringslinjeModel(fixture.Create<int>(), fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), fixture.Create<decimal>());
            Assert.That(bogføringslinjeModel, Is.Not.Null);

            var eventCalled = false;
            bogføringslinjeModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, propertyNameToRaise, StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            bogføringslinjeModel.Bilag = bogføringslinjeModel.Bilag;
            Assert.That(eventCalled, Is.False);
            bogføringslinjeModel.Bilag = fixture.Create<string>();
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at sætteren til Kontonummer kaster en ArgumentNullException ved illegale kontonumre.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestAtKontonummerSetterKasterArgumentExceptionVedIllegalValue(string illegalValue)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var bogføringslinjeModel = new BogføringslinjeModel(fixture.Create<int>(), fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), fixture.Create<decimal>());
            Assert.That(bogføringslinjeModel, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => bogføringslinjeModel.Kontonummer = illegalValue);
        }

        /// <summary>
        /// Tester, at sætteren til Kontonummer opdaterer kontonummer på bogføringslinjen.
        /// </summary>
        [Test]
        public void TestAtKontonummerSetterOpdatererKontonummer()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var bogføringslinjeModel = new BogføringslinjeModel(fixture.Create<int>(), fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), fixture.Create<decimal>());
            Assert.That(bogføringslinjeModel, Is.Not.Null);

            var newValue = fixture.Create<string>();
            Assert.That(bogføringslinjeModel.Kontonummer, Is.Not.EqualTo(newValue));

            bogføringslinjeModel.Kontonummer = newValue;
            Assert.That(bogføringslinjeModel.Kontonummer, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tester, at sætteren til Kontonummer rejser PropertyChanged ved opdatering af kontonummer på bogføringslinjen.
        /// </summary>
        [Test]
        [TestCase("Kontonummer")]
        [TestCase("Nyhedsinformation")]
        public void TestAtKontonummerSetterRejserPropertyChangedVedOpdateringAfKontonummer(string propertyNameToRaise)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var bogføringslinjeModel = new BogføringslinjeModel(fixture.Create<int>(), fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), fixture.Create<decimal>());
            Assert.That(bogføringslinjeModel, Is.Not.Null);

            var eventCalled = false;
            bogføringslinjeModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, propertyNameToRaise, StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            bogføringslinjeModel.Kontonummer = bogføringslinjeModel.Kontonummer;
            Assert.That(eventCalled, Is.False);
            bogføringslinjeModel.Kontonummer = fixture.Create<string>();
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at sætteren til Tekst kaster en ArgumentNullException ved illegale tekster.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestAtTekstSetterKasterArgumentExceptionVedIllegalValue(string illegalValue)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var bogføringslinjeModel = new BogføringslinjeModel(fixture.Create<int>(), fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), fixture.Create<decimal>());
            Assert.That(bogføringslinjeModel, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => bogføringslinjeModel.Tekst = illegalValue);
        }

        /// <summary>
        /// Tester, at sætteren til Tekst opdaterer tekst på bogføringslinjen.
        /// </summary>
        [Test]
        public void TestAtTekstSetterOpdatererTekst()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var bogføringslinjeModel = new BogføringslinjeModel(fixture.Create<int>(), fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), fixture.Create<decimal>());
            Assert.That(bogføringslinjeModel, Is.Not.Null);

            var newValue = fixture.Create<string>();
            Assert.That(bogføringslinjeModel.Tekst, Is.Not.EqualTo(newValue));

            bogføringslinjeModel.Tekst = newValue;
            Assert.That(bogføringslinjeModel.Tekst, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tester, at sætteren til Tekst rejser PropertyChanged ved opdatering af tekst på bogføringslinjen.
        /// </summary>
        [Test]
        [TestCase("Tekst")]
        [TestCase("Nyhedsinformation")]
        public void TestAtTekstSetterRejserPropertyChangedVedOpdateringAfTekst(string propertyNameToRaise)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var bogføringslinjeModel = new BogføringslinjeModel(fixture.Create<int>(), fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), fixture.Create<decimal>());
            Assert.That(bogføringslinjeModel, Is.Not.Null);

            var eventCalled = false;
            bogføringslinjeModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, propertyNameToRaise, StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            bogføringslinjeModel.Tekst = bogføringslinjeModel.Tekst;
            Assert.That(eventCalled, Is.False);
            bogføringslinjeModel.Tekst = fixture.Create<string>();
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at sætteren til Budgetkontonummer opdaterer budgetkontonummer på bogføringslinjen.
        /// </summary>
        [Test]
        public void TestAtBudgetkontonummerSetterOpdatererBudgetkontonummer()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var bogføringslinjeModel = new BogføringslinjeModel(fixture.Create<int>(), fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), fixture.Create<decimal>());
            Assert.That(bogføringslinjeModel, Is.Not.Null);

            var newValue = fixture.Create<string>();
            Assert.That(bogføringslinjeModel.Budgetkontonummer, Is.Not.EqualTo(newValue));

            bogføringslinjeModel.Budgetkontonummer = newValue;
            Assert.That(bogføringslinjeModel.Budgetkontonummer, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tester, at sætteren til Budgetkontonummer rejser PropertyChanged ved opdatering af budgetkontonummer på bogføringslinjen.
        /// </summary>
        [Test]
        [TestCase("Budgetkontonummer")]
        public void TestAtBudgetkontonummerSetterRejserPropertyChangedVedOpdateringAfBudgetkontonummer(string propertyNameToRaise)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var bogføringslinjeModel = new BogføringslinjeModel(fixture.Create<int>(), fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), fixture.Create<decimal>());
            Assert.That(bogføringslinjeModel, Is.Not.Null);

            var eventCalled = false;
            bogføringslinjeModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, propertyNameToRaise, StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            bogføringslinjeModel.Budgetkontonummer = bogføringslinjeModel.Budgetkontonummer;
            Assert.That(eventCalled, Is.False);
            bogføringslinjeModel.Budgetkontonummer = fixture.Create<string>();
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at sætteren til Debit kaster en ArgumentException ved illegale debitbeløb.
        /// </summary>
        [Test]
        [TestCase(-1024)]
        [TestCase(-1)]
        public void TestAtDebitSetterKasterArgumentExceptionVedIllegalValue(decimal illegalValue)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var bogføringslinjeModel = new BogføringslinjeModel(fixture.Create<int>(), fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), fixture.Create<decimal>());
            Assert.That(bogføringslinjeModel, Is.Not.Null);

            Assert.Throws<ArgumentException>(() => bogføringslinjeModel.Debit = illegalValue);
        }

        /// <summary>
        /// Tester, at sætteren til Debit opdaterer debitbeløb på bogføringslinjen.
        /// </summary>
        [Test]
        public void TestAtDebitSetterOpdatererDebit()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var bogføringslinjeModel = new BogføringslinjeModel(fixture.Create<int>(), fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), fixture.Create<decimal>());
            Assert.That(bogføringslinjeModel, Is.Not.Null);

            var newValue = fixture.Create<decimal>();
            Assert.That(bogføringslinjeModel.Debit, Is.Not.EqualTo(newValue));

            bogføringslinjeModel.Debit = newValue;
            Assert.That(bogføringslinjeModel.Debit, Is.EqualTo(newValue));
            Assert.That(bogføringslinjeModel.Bogført, Is.EqualTo(newValue - bogføringslinjeModel.Kredit));
        }

        /// <summary>
        /// Tester, at sætteren til Debit rejser PropertyChanged ved opdatering af debitbeløb på bogføringslinjen.
        /// </summary>
        [Test]
        [TestCase("Debit")]
        [TestCase("Bogført")]
        [TestCase("Nyhedsinformation")]
        public void TestAtDebitSetterRejserPropertyChangedVedOpdateringAfDebit(string propertyNameToRaise)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var bogføringslinjeModel = new BogføringslinjeModel(fixture.Create<int>(), fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), fixture.Create<decimal>());
            Assert.That(bogføringslinjeModel, Is.Not.Null);

            var eventCalled = false;
            bogføringslinjeModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, propertyNameToRaise, StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            bogføringslinjeModel.Debit = bogføringslinjeModel.Debit;
            Assert.That(eventCalled, Is.False);
            bogføringslinjeModel.Debit = fixture.Create<decimal>();
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at sætteren til Kredit kaster en ArgumentException ved illegale kreditbeløb.
        /// </summary>
        [Test]
        [TestCase(-1024)]
        [TestCase(-1)]
        public void TestAtKreditSetterKasterArgumentExceptionVedIllegalValue(decimal illegalValue)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var bogføringslinjeModel = new BogføringslinjeModel(fixture.Create<int>(), fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), fixture.Create<decimal>());
            Assert.That(bogføringslinjeModel, Is.Not.Null);

            Assert.Throws<ArgumentException>(() => bogføringslinjeModel.Kredit = illegalValue);
        }

        /// <summary>
        /// Tester, at sætteren til Kredit opdaterer kreditbeløb på bogføringslinjen.
        /// </summary>
        [Test]
        public void TestAtKreditSetterOpdatererKredit()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var bogføringslinjeModel = new BogføringslinjeModel(fixture.Create<int>(), fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), fixture.Create<decimal>());
            Assert.That(bogføringslinjeModel, Is.Not.Null);

            var newValue = fixture.Create<decimal>();
            Assert.That(bogføringslinjeModel.Kredit, Is.Not.EqualTo(newValue));

            bogføringslinjeModel.Kredit = newValue;
            Assert.That(bogføringslinjeModel.Kredit, Is.EqualTo(newValue));
            Assert.That(bogføringslinjeModel.Bogført, Is.EqualTo(bogføringslinjeModel.Debit - newValue));
        }

        /// <summary>
        /// Tester, at sætteren til Kredit rejser PropertyChanged ved opdatering af kreditbeløb på bogføringslinjen.
        /// </summary>
        [Test]
        [TestCase("Kredit")]
        [TestCase("Bogført")]
        [TestCase("Nyhedsinformation")]
        public void TestAtKreditSetterRejserPropertyChangedVedOpdateringAfKredit(string propertyNameToRaise)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var bogføringslinjeModel = new BogføringslinjeModel(fixture.Create<int>(), fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), fixture.Create<decimal>());
            Assert.That(bogføringslinjeModel, Is.Not.Null);

            var eventCalled = false;
            bogføringslinjeModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, propertyNameToRaise, StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            bogføringslinjeModel.Kredit = bogføringslinjeModel.Kredit;
            Assert.That(eventCalled, Is.False);
            bogføringslinjeModel.Kredit = fixture.Create<decimal>();
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at sætteren til Adressekonto opdaterer adressekonto på bogføringslinjen.
        /// </summary>
        [Test]
        public void TestAtAdressekontoSetterOpdatererAdressekonto()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var bogføringslinjeModel = new BogføringslinjeModel(fixture.Create<int>(), fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), fixture.Create<decimal>());
            Assert.That(bogføringslinjeModel, Is.Not.Null);

            var newValue = fixture.Create<int>();
            Assert.That(bogføringslinjeModel.Adressekonto, Is.Not.EqualTo(newValue));

            bogføringslinjeModel.Adressekonto = newValue;
            Assert.That(bogføringslinjeModel.Adressekonto, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tester, at sætteren til Adressekonto rejser PropertyChanged ved opdatering af adressekonto på bogføringslinjen.
        /// </summary>
        [Test]
        [TestCase("Adressekonto")]
        public void TestAtAdressekontoSetterRejserPropertyChangedVedOpdateringAfAdressekonto(string propertyNameToRaise)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var bogføringslinjeModel = new BogføringslinjeModel(fixture.Create<int>(), fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), fixture.Create<decimal>());
            Assert.That(bogføringslinjeModel, Is.Not.Null);

            var eventCalled = false;
            bogføringslinjeModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, propertyNameToRaise, StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            bogføringslinjeModel.Adressekonto = bogføringslinjeModel.Adressekonto;
            Assert.That(eventCalled, Is.False);
            bogføringslinjeModel.Adressekonto = fixture.Create<int>();
            Assert.That(eventCalled, Is.True);
        }
    }
}
