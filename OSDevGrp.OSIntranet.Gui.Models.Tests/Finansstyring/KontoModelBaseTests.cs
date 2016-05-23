using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Models.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Resources;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Gui.Models.Tests.Finansstyring
{
    /// <summary>
    /// Tester modellen indeholdende grundlæggende kontooplysninger.
    /// </summary>
    [TestFixture]
    public class KontoModelBaseTests
    {
        /// <summary>
        /// Egen klasse til test af modellen indeholdende grundlæggende kontooplysninger.
        /// </summary>
        private class MyKontoModel : KontoModelBase
        {
            #region Constructor

            /// <summary>
            /// Danner egen klasse til test af modellen indeholdende grundlæggende kontooplysninger.
            /// </summary>
            /// <param name="regnskabsnummer">Regnskabsnummer, som kontoen er tilknyttet.</param>
            /// <param name="kontonummer">Kontonummer.</param>
            /// <param name="kontonavn">Kontonavn.</param>
            /// <param name="kontogruppe">Unik identifikation af kontogruppen.</param>
            /// <param name="statusDato">Statusdato for opgørelse af kontoen.</param>
            public MyKontoModel(int regnskabsnummer, string kontonummer, string kontonavn, int kontogruppe, DateTime statusDato)
                : base(regnskabsnummer, kontonummer, kontonavn, kontogruppe, statusDato)
            {
            }

            #endregion
        }

        /// <summary>
        /// Tester, at konstruktøren initierer en model indeholdende grundlæggende kontooplysninger.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererKontoModelBase()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var regnskabsnummer = fixture.Create<int>();
            var kontonummer = fixture.Create<string>();
            var kontonavn = fixture.Create<string>();
            var kontogruppe = fixture.Create<int>();
            var statusDato = fixture.Create<DateTime>();
            var kontoModelBase = new MyKontoModel(regnskabsnummer, kontonummer, kontonavn, kontogruppe, statusDato);
            Assert.That(kontoModelBase, Is.Not.Null);
            Assert.That(kontoModelBase.Regnskabsnummer, Is.EqualTo(regnskabsnummer));
            Assert.That(kontoModelBase.Kontonummer, Is.Not.Null);
            Assert.That(kontoModelBase.Kontonummer, Is.Not.Empty);
            Assert.That(kontoModelBase.Kontonummer, Is.EqualTo(kontonummer));
            Assert.That(kontoModelBase.Kontonavn, Is.Not.Null);
            Assert.That(kontoModelBase.Kontonavn, Is.Not.Empty);
            Assert.That(kontoModelBase.Kontonavn, Is.EqualTo(kontonavn));
            Assert.That(kontoModelBase.Beskrivelse, Is.Null);
            Assert.That(kontoModelBase.Notat, Is.Null);
            Assert.That(kontoModelBase.Kontogruppe, Is.EqualTo(kontogruppe));
            Assert.That(kontoModelBase.StatusDato, Is.EqualTo(statusDato));
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

            var exception = Assert.Throws<ArgumentException>(() => new MyKontoModel(illegalValue, fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>(), fixture.Create<DateTime>()));
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

            var exception = Assert.Throws<ArgumentNullException>(() => new MyKontoModel(fixture.Create<int>(), illegalValue, fixture.Create<string>(), fixture.Create<int>(), fixture.Create<DateTime>()));
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

            var exception = Assert.Throws<ArgumentNullException>(() => new MyKontoModel(fixture.Create<int>(), fixture.Create<string>(), illegalValue, fixture.Create<int>(), fixture.Create<DateTime>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("kontonavn"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentException ved illegale kontogrupper.
        /// </summary>
        [Test]
        [TestCase(-1024)]
        [TestCase(-1)]
        [TestCase(0)]
        public void TestAtConstructorKasterArgumentExceptionVedIllegalKontogruppe(int illegalValue)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var exception = Assert.Throws<ArgumentException>(() => new MyKontoModel(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<string>(), illegalValue, fixture.Create<DateTime>()));
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
        /// Tester, at sætteren til Kontonavn kaster en ArgumentNullException ved illegale kontonavne.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestAtKontonavnSetterKasterArgumentNullExceptionVedIllegalValue(string illegalValue)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var kontoModelBase = new MyKontoModel(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>(), fixture.Create<DateTime>());
            Assert.That(kontoModelBase, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => kontoModelBase.Kontonavn = illegalValue);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("value"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at sætteren til Kontonavn opdaterer kontonavnet.
        /// </summary>
        [Test]
        public void TestAtKontonavnSetterOpdatererKontonavn()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var kontoModelBase = new MyKontoModel(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>(), fixture.Create<DateTime>());
            Assert.That(kontoModelBase, Is.Not.Null);

            var newValue = fixture.Create<string>();
            Assert.That(kontoModelBase.Kontonavn, Is.Not.EqualTo(newValue));

            kontoModelBase.Kontonavn = newValue;
            Assert.That(kontoModelBase.Kontonavn, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tester, at sætteren til Kontonavn rejser PropertyChanged ved opdatering af kontonavnet.
        /// </summary>
        [Test]
        [TestCase("Kontonavn")]
        public void TestAtKontonavnSetterRejserPropertyChangedVedOpdateringAfKontonavn(string propertyNameToRaise)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var kontoModelBase = new MyKontoModel(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>(), fixture.Create<DateTime>());
            Assert.That(kontoModelBase, Is.Not.Null);

            var eventCalled = false;
            kontoModelBase.PropertyChanged += (s, e) =>
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
            kontoModelBase.Kontonavn = kontoModelBase.Kontonavn;
            Assert.That(eventCalled, Is.False);
            kontoModelBase.Kontonavn = fixture.Create<string>();
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at sætteren til Beskrivelse opdaterer beskrivelsen.
        /// </summary>
        [Test]
        public void TestAtBeskrivelseSetterOpdatererBeskrivelse()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var kontoModelBase = new MyKontoModel(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>(), fixture.Create<DateTime>());
            Assert.That(kontoModelBase, Is.Not.Null);

            var newValue = fixture.Create<string>();
            Assert.That(kontoModelBase.Beskrivelse, Is.Not.EqualTo(newValue));

            kontoModelBase.Beskrivelse = newValue;
            Assert.That(kontoModelBase.Beskrivelse, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tester, at sætteren til Beskrivelse rejser PropertyChanged ved opdatering af beskrivelsen.
        /// </summary>
        [Test]
        [TestCase("Beskrivelse")]
        public void TestAtBeskrivelseSetterRejserPropertyChangedVedOpdateringAfBeskrivelse(string propertyNameToRaise)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var kontoModelBase = new MyKontoModel(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>(), fixture.Create<DateTime>());
            Assert.That(kontoModelBase, Is.Not.Null);

            var eventCalled = false;
            kontoModelBase.PropertyChanged += (s, e) =>
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
            kontoModelBase.Beskrivelse = kontoModelBase.Beskrivelse;
            Assert.That(eventCalled, Is.False);
            kontoModelBase.Beskrivelse = fixture.Create<string>();
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at sætteren til Notat opdaterer notatet.
        /// </summary>
        [Test]
        public void TestAtNotatSetterOpdatererNotat()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var kontoModelBase = new MyKontoModel(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>(), fixture.Create<DateTime>());
            Assert.That(kontoModelBase, Is.Not.Null);

            var newValue = fixture.Create<string>();
            Assert.That(kontoModelBase.Notat, Is.Not.EqualTo(newValue));

            kontoModelBase.Notat = newValue;
            Assert.That(kontoModelBase.Notat, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tester, at sætteren til Notat rejser PropertyChanged ved opdatering af notatet.
        /// </summary>
        [Test]
        [TestCase("Notat")]
        public void TestAtNotatSetterRejserPropertyChangedVedOpdateringAfNotat(string propertyNameToRaise)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var kontoModelBase = new MyKontoModel(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>(), fixture.Create<DateTime>());
            Assert.That(kontoModelBase, Is.Not.Null);

            var eventCalled = false;
            kontoModelBase.PropertyChanged += (s, e) =>
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
            kontoModelBase.Notat = kontoModelBase.Notat;
            Assert.That(eventCalled, Is.False);
            kontoModelBase.Notat = fixture.Create<string>();
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at sætteren til Kontogruppe kaster en ArgumentException ved illegale kontogrupper.
        /// </summary>
        [Test]
        [TestCase(-1024)]
        [TestCase(-1)]
        [TestCase(0)]
        public void TestAtKontogruppeSetterKasterArgumentExceptionVedIllegalValue(int illegalValue)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var kontoModelBase = new MyKontoModel(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>(), fixture.Create<DateTime>());
            Assert.That(kontoModelBase, Is.Not.Null);

            var exception = Assert.Throws<ArgumentException>(() => kontoModelBase.Kontogruppe = illegalValue);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Does.StartWith(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "value", illegalValue)));
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("value"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at sætteren til Kontogruppe opdaterer kontogruppen.
        /// </summary>
        [Test]
        public void TestAtKontogruppeSetterOpdatererKontogruppe()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var kontoModelBase = new MyKontoModel(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>(), fixture.Create<DateTime>());
            Assert.That(kontoModelBase, Is.Not.Null);

            var newValue = fixture.Create<int>();
            Assert.That(kontoModelBase.Kontogruppe, Is.Not.EqualTo(newValue));

            kontoModelBase.Kontogruppe = newValue;
            Assert.That(kontoModelBase.Kontogruppe, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tester, at sætteren til Kontogruppe rejser PropertyChanged ved opdatering af kontogruppen.
        /// </summary>
        [Test]
        [TestCase("Kontogruppe")]
        public void TestAtKontogruppeSetterRejserPropertyChangedVedOpdateringAfKontogruppe(string propertyNameToRaise)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var kontoModelBase = new MyKontoModel(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>(), fixture.Create<DateTime>());
            Assert.That(kontoModelBase, Is.Not.Null);

            var eventCalled = false;
            kontoModelBase.PropertyChanged += (s, e) =>
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
            kontoModelBase.Kontogruppe = kontoModelBase.Kontogruppe;
            Assert.That(eventCalled, Is.False);
            kontoModelBase.Kontogruppe = fixture.Create<int>();
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at sætteren til StatusDato opdaterer statusdatoen.
        /// </summary>
        [Test]
        public void TestAtStatusDatoSetterOpdatererStatusDato()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var kontoModelBase = new MyKontoModel(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>(), fixture.Create<DateTime>());
            Assert.That(kontoModelBase, Is.Not.Null);

            var newValue = kontoModelBase.StatusDato.AddDays(7);
            Assert.That(kontoModelBase.StatusDato, Is.Not.EqualTo(newValue));

            kontoModelBase.StatusDato = newValue;
            Assert.That(kontoModelBase.StatusDato, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tester, at sætteren til StatusDato rejser PropertyChanged ved opdatering af statusdatoen.
        /// </summary>
        [Test]
        [TestCase("StatusDato")]
        public void TestAtStatusDatoSetterRejserPropertyChangedVedOpdateringAfStatusDato(string propertyNameToRaise)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var kontoModelBase = new MyKontoModel(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>(), fixture.Create<DateTime>());
            Assert.That(kontoModelBase, Is.Not.Null);

            var eventCalled = false;
            kontoModelBase.PropertyChanged += (s, e) =>
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
            kontoModelBase.StatusDato = kontoModelBase.StatusDato;
            Assert.That(eventCalled, Is.False);
            kontoModelBase.StatusDato = kontoModelBase.StatusDato.AddDays(7);
            Assert.That(eventCalled, Is.True);
        }
    }
}
