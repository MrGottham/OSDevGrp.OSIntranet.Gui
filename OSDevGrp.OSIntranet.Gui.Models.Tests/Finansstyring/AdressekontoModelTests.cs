using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Models.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.Resources;
using Ploeh.AutoFixture;
using Text = OSDevGrp.OSIntranet.Gui.Resources.Text;

namespace OSDevGrp.OSIntranet.Gui.Models.Tests.Finansstyring
{
    /// <summary>
    /// Tester modellen for en adressekonto. 
    /// </summary>
    [TestFixture]
    public class AdressekontoModelTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en model til en adressekonto.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererAdressekontoModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var regnskabsnummer = fixture.Create<int>();
            var nummer = fixture.Create<int>();
            var navn = fixture.Create<string>();
            var statusDato = fixture.Create<DateTime>();
            var saldo = fixture.Create<Decimal>();
            var nyhedsinformation = string.Format("{0} {1}\r\n{2} {3}", statusDato.ToShortDateString(), navn, Resource.GetText(Text.Balance), saldo.ToString("c"));
            var adressekontoModel = new AdressekontoModel(regnskabsnummer, nummer, navn, statusDato, saldo);
            Assert.That(adressekontoModel, Is.Not.Null);
            Assert.That(adressekontoModel.Regnskabsnummer, Is.EqualTo(regnskabsnummer));
            Assert.That(adressekontoModel.Nummer, Is.EqualTo(nummer));
            Assert.That(adressekontoModel.Navn, Is.Not.Null);
            Assert.That(adressekontoModel.Navn, Is.Not.Empty);
            Assert.That(adressekontoModel.Navn, Is.EqualTo(navn));
            Assert.That(adressekontoModel.PrimærTelefon, Is.Null);
            Assert.That(adressekontoModel.SekundærTelefon, Is.Null);
            Assert.That(adressekontoModel.StatusDato, Is.EqualTo(statusDato));
            Assert.That(adressekontoModel.Saldo, Is.EqualTo(saldo));
            Assert.That(adressekontoModel.Nyhedsaktualitet, Is.EqualTo(Nyhedsaktualitet.Medium));
            Assert.That(adressekontoModel.Nyhedsudgivelsestidspunkt, Is.EqualTo(statusDato));
            Assert.That(adressekontoModel.Nyhedsinformation, Is.Not.Null);
            Assert.That(adressekontoModel.Nyhedsinformation, Is.Not.Empty);
            Assert.That(adressekontoModel.Nyhedsinformation, Is.EqualTo(nyhedsinformation));
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

            var exception = Assert.Throws<ArgumentException>(() => new AdressekontoModel(illegalValue, fixture.Create<int>(), fixture.Create<string>(), fixture.Create<DateTime>(), fixture.Create<decimal>()));
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
        /// Tester, at konstruktøren kaster en ArgumentException ved illegale identifikation af adressekonto.
        /// </summary>
        [Test]
        [TestCase(-1024)]
        [TestCase(-1)]
        [TestCase(0)]
        public void TestAtConstructorKasterArgumentExceptionVedIllegalNummer(int illegalValue)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var exception = Assert.Throws<ArgumentException>(() => new AdressekontoModel(fixture.Create<int>(), illegalValue, fixture.Create<string>(), fixture.Create<DateTime>(), fixture.Create<decimal>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.StringStarting(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "nummer", illegalValue)));
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("nummer"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException ved illegale navne på adressekonto.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestAtConstructorKasterArgumentNullExceptionVedIllegalNavn(string illegalValue)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var exception = Assert.Throws<ArgumentNullException>(() => new AdressekontoModel(fixture.Create<int>(), fixture.Create<int>(), illegalValue, fixture.Create<DateTime>(), fixture.Create<decimal>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("navn"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at sætteren til Navn kaster en ArgumentNullException ved illegale navne.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestAtNavnSetterKasterArgumentNullExceptionVedIllegalValue(string illegalValue)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var adressekontoModel = new AdressekontoModel(fixture.Create<int>(), fixture.Create<int>(), fixture.Create<string>(), fixture.Create<DateTime>(), fixture.Create<decimal>());
            Assert.That(adressekontoModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => adressekontoModel.Navn = illegalValue);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("value"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at sætteren til Navn opdaterer navnet på adressenkontoen.
        /// </summary>
        [Test]
        public void TestAtNavnSetterOpdatererNavn()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var adressekontoModel = new AdressekontoModel(fixture.Create<int>(), fixture.Create<int>(), fixture.Create<string>(), fixture.Create<DateTime>(), fixture.Create<decimal>());
            Assert.That(adressekontoModel, Is.Not.Null);

            var newValue = fixture.Create<string>();
            Assert.That(adressekontoModel.Navn, Is.Not.EqualTo(newValue));

            adressekontoModel.Navn = newValue;
            Assert.That(adressekontoModel.Navn, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tester, at sætteren til Navn rejser PropertyChanged ved opdatering af navn på adressekontoen.
        /// </summary>
        [Test]
        [TestCase("Navn")]
        [TestCase("Nyhedsinformation")]
        public void TestAtNavnSetterRejserPropertyChangedVedOpdateringAfNavn(string propertyNameToRaise)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var adressekontoModel = new AdressekontoModel(fixture.Create<int>(), fixture.Create<int>(), fixture.Create<string>(), fixture.Create<DateTime>(), fixture.Create<decimal>());
            Assert.That(adressekontoModel, Is.Not.Null);

            var eventCalled = false;
            adressekontoModel.PropertyChanged += (s, e) =>
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
            adressekontoModel.Navn = adressekontoModel.Navn;
            Assert.That(eventCalled, Is.False);
            adressekontoModel.Navn = fixture.Create<string>();
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at sætteren til PrimærTelefon opdaterer det primære telefonnummer på adressenkontoen.
        /// </summary>
        [Test]
        public void TestAtPrimærTelefonSetterOpdatererPrimærTelefon()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var adressekontoModel = new AdressekontoModel(fixture.Create<int>(), fixture.Create<int>(), fixture.Create<string>(), fixture.Create<DateTime>(), fixture.Create<decimal>());
            Assert.That(adressekontoModel, Is.Not.Null);

            var newValue = fixture.Create<string>();
            Assert.That(adressekontoModel.PrimærTelefon, Is.Not.EqualTo(newValue));

            adressekontoModel.PrimærTelefon = newValue;
            Assert.That(adressekontoModel.PrimærTelefon, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tester, at sætteren til PrimærTelefon rejser PropertyChanged ved opdatering af det primære telefonummer på adressekontoen.
        /// </summary>
        [Test]
        [TestCase("PrimærTelefon")]
        public void TestAtPrimærTelefonSetterRejserPropertyChangedVedOpdateringAfPrimærTelefon(string propertyNameToRaise)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var adressekontoModel = new AdressekontoModel(fixture.Create<int>(), fixture.Create<int>(), fixture.Create<string>(), fixture.Create<DateTime>(), fixture.Create<decimal>());
            Assert.That(adressekontoModel, Is.Not.Null);

            var eventCalled = false;
            adressekontoModel.PropertyChanged += (s, e) =>
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
            adressekontoModel.PrimærTelefon = adressekontoModel.PrimærTelefon;
            Assert.That(eventCalled, Is.False);
            adressekontoModel.PrimærTelefon = fixture.Create<string>();
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at sætteren til SekundærTelefon opdaterer det sekundære telefonnummer på adressenkontoen.
        /// </summary>
        [Test]
        public void TestAtSekundærTelefonSetterOpdatererSekundærTelefon()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var adressekontoModel = new AdressekontoModel(fixture.Create<int>(), fixture.Create<int>(), fixture.Create<string>(), fixture.Create<DateTime>(), fixture.Create<decimal>());
            Assert.That(adressekontoModel, Is.Not.Null);

            var newValue = fixture.Create<string>();
            Assert.That(adressekontoModel.SekundærTelefon, Is.Not.EqualTo(newValue));

            adressekontoModel.SekundærTelefon = newValue;
            Assert.That(adressekontoModel.SekundærTelefon, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tester, at sætteren til SekundærTelefon rejser PropertyChanged ved opdatering af det sekundære telefonummer på adressekontoen.
        /// </summary>
        [Test]
        [TestCase("SekundærTelefon")]
        public void TestAtSekundærTelefonSetterRejserPropertyChangedVedOpdateringAfSekundærTelefon(string propertyNameToRaise)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var adressekontoModel = new AdressekontoModel(fixture.Create<int>(), fixture.Create<int>(), fixture.Create<string>(), fixture.Create<DateTime>(), fixture.Create<decimal>());
            Assert.That(adressekontoModel, Is.Not.Null);

            var eventCalled = false;
            adressekontoModel.PropertyChanged += (s, e) =>
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
            adressekontoModel.SekundærTelefon = adressekontoModel.SekundærTelefon;
            Assert.That(eventCalled, Is.False);
            adressekontoModel.SekundærTelefon = fixture.Create<string>();
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at sætteren til StatusDato opdaterer statusdatoen på adressenkontoen.
        /// </summary>
        [Test]
        public void TestAtStatusDatoSetterOpdatererStatusDato()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var adressekontoModel = new AdressekontoModel(fixture.Create<int>(), fixture.Create<int>(), fixture.Create<string>(), fixture.Create<DateTime>(), fixture.Create<decimal>());
            Assert.That(adressekontoModel, Is.Not.Null);

            var newValue = adressekontoModel.StatusDato.AddDays(-7);
            Assert.That(adressekontoModel.StatusDato, Is.Not.EqualTo(newValue));

            adressekontoModel.StatusDato = newValue;
            Assert.That(adressekontoModel.StatusDato, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tester, at sætteren til StatusDato rejser PropertyChanged ved opdatering af statusdatoen på adressekontoen.
        /// </summary>
        [Test]
        [TestCase("StatusDato")]
        [TestCase("Nyhedsudgivelsestidspunkt")]
        [TestCase("Nyhedsinformation")]
        public void TestAtStatusDatoSetterRejserPropertyChangedVedOpdateringAfStatusDato(string propertyNameToRaise)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var adressekontoModel = new AdressekontoModel(fixture.Create<int>(), fixture.Create<int>(), fixture.Create<string>(), fixture.Create<DateTime>(), fixture.Create<decimal>());
            Assert.That(adressekontoModel, Is.Not.Null);

            var eventCalled = false;
            adressekontoModel.PropertyChanged += (s, e) =>
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
            adressekontoModel.StatusDato = adressekontoModel.StatusDato;
            Assert.That(eventCalled, Is.False);
            adressekontoModel.StatusDato = adressekontoModel.StatusDato.AddDays(-7);
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at sætteren til Saldo opdaterer saldoen på adressenkontoen.
        /// </summary>
        [Test]
        public void TestAtSaldoSetterOpdatererSaldo()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var adressekontoModel = new AdressekontoModel(fixture.Create<int>(), fixture.Create<int>(), fixture.Create<string>(), fixture.Create<DateTime>(), fixture.Create<decimal>());
            Assert.That(adressekontoModel, Is.Not.Null);

            var newValue = fixture.Create<decimal>();
            Assert.That(adressekontoModel.Saldo, Is.Not.EqualTo(newValue));

            adressekontoModel.Saldo = newValue;
            Assert.That(adressekontoModel.Saldo, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tester, at sætteren til Saldo rejser PropertyChanged ved opdatering af saldoen på adressekontoen.
        /// </summary>
        [Test]
        [TestCase("Saldo")]
        [TestCase("Nyhedsinformation")]
        public void TestAtSaldoSetterRejserPropertyChangedVedOpdateringAfSaldo(string propertyNameToRaise)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var adressekontoModel = new AdressekontoModel(fixture.Create<int>(), fixture.Create<int>(), fixture.Create<string>(), fixture.Create<DateTime>(), fixture.Create<decimal>());
            Assert.That(adressekontoModel, Is.Not.Null);

            var eventCalled = false;
            adressekontoModel.PropertyChanged += (s, e) =>
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
            adressekontoModel.Saldo = adressekontoModel.Saldo;
            Assert.That(eventCalled, Is.False);
            adressekontoModel.Saldo = fixture.Create<decimal>();
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at SetNyhedsaktualitet opdaterer nyhedsaktualiteten på adressenkontoen.
        /// </summary>
        [Test]
        public void TestAtSetNyhedsaktualitetOpdatererNyhedsaktualitet()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var adressekontoModel = new AdressekontoModel(fixture.Create<int>(), fixture.Create<int>(), fixture.Create<string>(), fixture.Create<DateTime>(), fixture.Create<decimal>());
            Assert.That(adressekontoModel, Is.Not.Null);

            Assert.That(adressekontoModel.Nyhedsaktualitet, Is.Not.EqualTo(Nyhedsaktualitet.High));

            adressekontoModel.SetNyhedsaktualitet(Nyhedsaktualitet.High);
            Assert.That(adressekontoModel.Nyhedsaktualitet, Is.EqualTo(Nyhedsaktualitet.High));
        }

        /// <summary>
        /// Tester, at SetNyhedsaktualitet rejser PropertyChanged ved opdatering af nyhedsaktualiteten på adressekontoen.
        /// </summary>
        [Test]
        [TestCase("Nyhedsaktualitet")]
        public void TestAtSetNyhedsaktualitetRejserPropertyChangedVedOpdateringAfNyhedsaktualitet(string propertyNameToRaise)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var adressekontoModel = new AdressekontoModel(fixture.Create<int>(), fixture.Create<int>(), fixture.Create<string>(), fixture.Create<DateTime>(), fixture.Create<decimal>());
            Assert.That(adressekontoModel, Is.Not.Null);

            var eventCalled = false;
            adressekontoModel.PropertyChanged += (s, e) =>
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
            adressekontoModel.SetNyhedsaktualitet(adressekontoModel.Nyhedsaktualitet);
            Assert.That(eventCalled, Is.False);
            adressekontoModel.SetNyhedsaktualitet(Nyhedsaktualitet.High);
            Assert.That(eventCalled, Is.True);
        }
    }
}
