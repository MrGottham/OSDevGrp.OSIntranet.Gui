using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Models.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Resources;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Gui.Models.Tests.Finansstyring
{
    /// <summary>
    /// Tester modellen til en konto.
    /// </summary>
    [TestFixture]
    public class KontoModelTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en model til en konto uden saldo og kreditbeløb.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererKontoModelUdenSaldoOgKredit()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var regnskabsnummer = fixture.Create<int>();
            var kontonummer = fixture.Create<string>();
            var kontonavn = fixture.Create<string>();
            var kontogruppe = fixture.Create<int>();
            var statusDato = fixture.Create<DateTime>();
            var kontoModel = new KontoModel(regnskabsnummer, kontonummer, kontonavn, kontogruppe, statusDato);
            Assert.That(kontoModel, Is.Not.Null);
            Assert.That(kontoModel.Regnskabsnummer, Is.EqualTo(regnskabsnummer));
            Assert.That(kontoModel.Kontonummer, Is.Not.Null);
            Assert.That(kontoModel.Kontonummer, Is.Not.Empty);
            Assert.That(kontoModel.Kontonummer, Is.EqualTo(kontonummer));
            Assert.That(kontoModel.Kontonavn, Is.Not.Null);
            Assert.That(kontoModel.Kontonavn, Is.Not.Empty);
            Assert.That(kontoModel.Kontonavn, Is.EqualTo(kontonavn));
            Assert.That(kontoModel.Beskrivelse, Is.Null);
            Assert.That(kontoModel.Notat, Is.Null);
            Assert.That(kontoModel.Kontogruppe, Is.EqualTo(kontogruppe));
            Assert.That(kontoModel.StatusDato, Is.EqualTo(statusDato));
            Assert.That(kontoModel.Kredit, Is.EqualTo(0M));
            Assert.That(kontoModel.Saldo, Is.EqualTo(0M));
            Assert.That(kontoModel.Disponibel, Is.EqualTo(0M));
        }

        /// <summary>
        /// Tester, at konstruktøren initierer en model til en konto med saldo og uden kreditbeløb.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererKontoModelMedSaldoOgUdenKredit()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var regnskabsnummer = fixture.Create<int>();
            var kontonummer = fixture.Create<string>();
            var kontonavn = fixture.Create<string>();
            var kontogruppe = fixture.Create<int>();
            var statusDato = fixture.Create<DateTime>();
            var saldo = fixture.Create<decimal>();
            var kontoModel = new KontoModel(regnskabsnummer, kontonummer, kontonavn, kontogruppe, statusDato, saldo);
            Assert.That(kontoModel, Is.Not.Null);
            Assert.That(kontoModel.Regnskabsnummer, Is.EqualTo(regnskabsnummer));
            Assert.That(kontoModel.Kontonummer, Is.Not.Null);
            Assert.That(kontoModel.Kontonummer, Is.Not.Empty);
            Assert.That(kontoModel.Kontonummer, Is.EqualTo(kontonummer));
            Assert.That(kontoModel.Kontonavn, Is.Not.Null);
            Assert.That(kontoModel.Kontonavn, Is.Not.Empty);
            Assert.That(kontoModel.Kontonavn, Is.EqualTo(kontonavn));
            Assert.That(kontoModel.Beskrivelse, Is.Null);
            Assert.That(kontoModel.Notat, Is.Null);
            Assert.That(kontoModel.Kontogruppe, Is.EqualTo(kontogruppe));
            Assert.That(kontoModel.StatusDato, Is.EqualTo(statusDato));
            Assert.That(kontoModel.Kredit, Is.EqualTo(0M));
            Assert.That(kontoModel.Saldo, Is.EqualTo(saldo));
            Assert.That(kontoModel.Disponibel, Is.EqualTo(saldo));
        }

        /// <summary>
        /// Tester, at konstruktøren initierer en model til en konto uden saldo og med kreditbeløb.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererKontoModelUdenSaldoOgMedKredit()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var regnskabsnummer = fixture.Create<int>();
            var kontonummer = fixture.Create<string>();
            var kontonavn = fixture.Create<string>();
            var kontogruppe = fixture.Create<int>();
            var statusDato = fixture.Create<DateTime>();
            var kredit = fixture.Create<decimal>();
            var kontoModel = new KontoModel(regnskabsnummer, kontonummer, kontonavn, kontogruppe, statusDato, 0M, kredit);
            Assert.That(kontoModel, Is.Not.Null);
            Assert.That(kontoModel.Regnskabsnummer, Is.EqualTo(regnskabsnummer));
            Assert.That(kontoModel.Kontonummer, Is.Not.Null);
            Assert.That(kontoModel.Kontonummer, Is.Not.Empty);
            Assert.That(kontoModel.Kontonummer, Is.EqualTo(kontonummer));
            Assert.That(kontoModel.Kontonavn, Is.Not.Null);
            Assert.That(kontoModel.Kontonavn, Is.Not.Empty);
            Assert.That(kontoModel.Kontonavn, Is.EqualTo(kontonavn));
            Assert.That(kontoModel.Beskrivelse, Is.Null);
            Assert.That(kontoModel.Notat, Is.Null);
            Assert.That(kontoModel.Kontogruppe, Is.EqualTo(kontogruppe));
            Assert.That(kontoModel.StatusDato, Is.EqualTo(statusDato));
            Assert.That(kontoModel.Kredit, Is.EqualTo(kredit));
            Assert.That(kontoModel.Saldo, Is.EqualTo(0));
            Assert.That(kontoModel.Disponibel, Is.EqualTo(kredit));
        }

        /// <summary>
        /// Tester, at konstruktøren initierer en model til en konto uden saldo og med kreditbeløb.
        /// </summary>
        [Test] 
        public void TestAtConstructorInitiererKontoModelMedSaldoOgKredit()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var regnskabsnummer = fixture.Create<int>();
            var kontonummer = fixture.Create<string>();
            var kontonavn = fixture.Create<string>();
            var kontogruppe = fixture.Create<int>();
            var statusDato = fixture.Create<DateTime>();
            var saldo = fixture.Create<decimal>();
            var kredit = fixture.Create<decimal>();
            var kontoModel = new KontoModel(regnskabsnummer, kontonummer, kontonavn, kontogruppe, statusDato, saldo, kredit);
            Assert.That(kontoModel, Is.Not.Null);
            Assert.That(kontoModel.Regnskabsnummer, Is.EqualTo(regnskabsnummer));
            Assert.That(kontoModel.Kontonummer, Is.Not.Null);
            Assert.That(kontoModel.Kontonummer, Is.Not.Empty);
            Assert.That(kontoModel.Kontonummer, Is.EqualTo(kontonummer));
            Assert.That(kontoModel.Kontonavn, Is.Not.Null);
            Assert.That(kontoModel.Kontonavn, Is.Not.Empty);
            Assert.That(kontoModel.Kontonavn, Is.EqualTo(kontonavn));
            Assert.That(kontoModel.Beskrivelse, Is.Null);
            Assert.That(kontoModel.Notat, Is.Null);
            Assert.That(kontoModel.Kontogruppe, Is.EqualTo(kontogruppe));
            Assert.That(kontoModel.StatusDato, Is.EqualTo(statusDato));
            Assert.That(kontoModel.Kredit, Is.EqualTo(kredit));
            Assert.That(kontoModel.Saldo, Is.EqualTo(saldo));
            Assert.That(kontoModel.Disponibel, Is.EqualTo(kredit + saldo));
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

            var exception = Assert.Throws<ArgumentException>(() => new KontoModel(illegalValue, fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<decimal>(), fixture.Create<decimal>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Does.StartWith(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "regnskabsnummer", illegalValue)));
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

            var exception = Assert.Throws<ArgumentNullException>(() => new KontoModel(fixture.Create<int>(), illegalValue, fixture.Create<string>(), fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<decimal>(), fixture.Create<decimal>()));
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

            var exception = Assert.Throws<ArgumentNullException>(() => new KontoModel(fixture.Create<int>(), fixture.Create<string>(), illegalValue, fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<decimal>(), fixture.Create<decimal>()));
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

            var exception = Assert.Throws<ArgumentException>(() => new KontoModel(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<string>(), illegalValue, fixture.Create<DateTime>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Does.StartWith(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "kontogruppe", illegalValue)));
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("kontogruppe"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at sætteren til Kredit opdaterer kreditbeløbet.
        /// </summary>
        [Test]
        public void TestAtKreditSetterOpdatererKredit()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var saldo = fixture.Create<decimal>();
            var kredit = fixture.Create<decimal>();
            var kontoModel = new KontoModel(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>(), fixture.Create<DateTime>(), saldo, kredit);
            Assert.That(kontoModel, Is.Not.Null);
            Assert.That(kontoModel.Kredit, Is.EqualTo(kredit));
            Assert.That(kontoModel.Saldo, Is.EqualTo(saldo));
            Assert.That(kontoModel.Disponibel, Is.EqualTo(kredit + saldo));

            var newValue = fixture.Create<decimal>();
            Assert.That(kontoModel.Kredit, Is.Not.EqualTo(newValue));

            kontoModel.Kredit = newValue;
            Assert.That(kontoModel.Kredit, Is.EqualTo(newValue));
            Assert.That(kontoModel.Saldo, Is.EqualTo(saldo));
            Assert.That(kontoModel.Disponibel, Is.EqualTo(newValue + saldo));
        }

        /// <summary>
        /// Tester, at sætteren til Kredit rejser PropertyChanged ved opdatering af kreditbeløbet.
        /// </summary>
        [Test]
        [TestCase("Kredit")]
        [TestCase("Disponibel")]
        public void TestAtKreditSetterRejserPropertyChangedVedOpdateringAfKredit(string propertyNameToRaise)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var kontoModel = new KontoModel(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<decimal>(), fixture.Create<decimal>());
            Assert.That(kontoModel, Is.Not.Null);

            var eventCalled = false;
            kontoModel.PropertyChanged += (s, e) =>
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
            kontoModel.Kredit = kontoModel.Kredit;
            Assert.That(eventCalled, Is.False);
            kontoModel.Kredit = fixture.Create<decimal>();
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at sætteren til Saldo opdaterer saldoen.
        /// </summary>
        [Test]
        public void TestAtSaldoSetterOpdatererSaldo()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var saldo = fixture.Create<decimal>();
            var kredit = fixture.Create<decimal>();
            var kontoModel = new KontoModel(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>(), fixture.Create<DateTime>(), saldo, kredit);
            Assert.That(kontoModel, Is.Not.Null);
            Assert.That(kontoModel.Kredit, Is.EqualTo(kredit));
            Assert.That(kontoModel.Saldo, Is.EqualTo(saldo));
            Assert.That(kontoModel.Disponibel, Is.EqualTo(kredit + saldo));

            var newValue = fixture.Create<decimal>();
            Assert.That(kontoModel.Saldo, Is.Not.EqualTo(newValue));

            kontoModel.Saldo = newValue;
            Assert.That(kontoModel.Kredit, Is.EqualTo(kredit));
            Assert.That(kontoModel.Saldo, Is.EqualTo(newValue));
            Assert.That(kontoModel.Disponibel, Is.EqualTo(kredit + newValue));
        }

        /// <summary>
        /// Tester, at sætteren til Saldo rejser PropertyChanged ved opdatering af saldoen.
        /// </summary>
        [Test]
        [TestCase("Saldo")]
        [TestCase("Disponibel")]
        public void TestAtSaldoSetterRejserPropertyChangedVedOpdateringAfSaldo(string propertyNameToRaise)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var kontoModel = new KontoModel(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<decimal>(), fixture.Create<decimal>());
            Assert.That(kontoModel, Is.Not.Null);

            var eventCalled = false;
            kontoModel.PropertyChanged += (s, e) =>
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
            kontoModel.Saldo = kontoModel.Saldo;
            Assert.That(eventCalled, Is.False);
            kontoModel.Saldo = fixture.Create<decimal>();
            Assert.That(eventCalled, Is.True);
        }
    }
}
