using System;
using System.Globalization;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Models.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Resources;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Gui.Models.Tests.Finansstyring
{
    /// <summary>
    /// Tester modellen til en kontogruppe.
    /// </summary>
    [TestFixture]
    public class KontogruppeModelTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en model til en kontogruppe.
        /// </summary>
        [Test]
        [TestCase(Balancetype.Aktiver)]
        [TestCase(Balancetype.Passiver)]
        public void TestAtConstructorInitiererKontogruppeModel(Balancetype balancetype)
        {
            var fixture = new Fixture();

            var nummer = fixture.Create<int>();
            var tekst = fixture.Create<string>();
            var kontogruppeModel = new KontogruppeModel(nummer, tekst, balancetype);
            Assert.That(kontogruppeModel, Is.Not.Null);
            Assert.That(kontogruppeModel.Id, Is.Not.Null);
            Assert.That(kontogruppeModel.Id, Is.Not.Empty);
            Assert.That(kontogruppeModel.Id, Is.EqualTo(nummer.ToString(CultureInfo.InvariantCulture)));
            Assert.That(kontogruppeModel.Nummer, Is.EqualTo(nummer));
            Assert.That(kontogruppeModel.Tekst, Is.Not.Null);
            Assert.That(kontogruppeModel.Tekst, Is.Not.Empty);
            Assert.That(kontogruppeModel.Tekst, Is.EqualTo(tekst));
            Assert.That(kontogruppeModel.Balancetype, Is.EqualTo(balancetype));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentException ved invalide identifikationer af kontogruppen.
        /// </summary>
        [Test]
        [TestCase(-1024)]
        [TestCase(-1)]
        [TestCase(-0)]
        public void TestAtConstructorKasterArgumentExceptionVedInvalidNummer(int invalidValue)
        {
            var fixture = new Fixture();
            fixture.Customize<Balancetype>(e => e.FromFactory(() => Balancetype.Aktiver));

            var exception = Assert.Throws<ArgumentException>(() => new KontogruppeModel(invalidValue, fixture.Create<string>(), fixture.Create<Balancetype>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.StringStarting(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "nummer", invalidValue)));
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("nummer"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException ved invalide tekster, der beskriver kontogruppen.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestAtConstructorKasterArgumentExceptionVedInvalidTekst(string invalidValue)
        {
            var fixture = new Fixture();
            fixture.Customize<Balancetype>(e => e.FromFactory(() => Balancetype.Aktiver));

            var exception = Assert.Throws<ArgumentNullException>(() => new KontogruppeModel(fixture.Create<int>(), invalidValue, fixture.Create<Balancetype>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("tekst"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at sætteren til Balancetype opdaterer siden i balancen, hvor kontogruppen er placeret.
        /// </summary>
        [Test]
        [TestCase(Balancetype.Aktiver)]
        [TestCase(Balancetype.Passiver)]
        public void TestAtBalancetypeSetterOpdatererBalancetype(Balancetype balancetype)
        {
            var fixture = new Fixture();

            var kontogruppeModel = new KontogruppeModel(fixture.Create<int>(), fixture.Create<string>(), balancetype == Balancetype.Aktiver ? Balancetype.Passiver : Balancetype.Aktiver);
            Assert.That(kontogruppeModel, Is.Not.Null);
            Assert.That(kontogruppeModel.Balancetype, Is.Not.EqualTo(balancetype));

            kontogruppeModel.Balancetype = balancetype;
            Assert.That(kontogruppeModel.Balancetype, Is.EqualTo(balancetype));
        }

        /// <summary>
        /// Tester, at sætteren til Balancetype rejser PropertyChanged ved opdatering af siden i balancen, hvor kontogruppen er placeret.
        /// </summary>
        [Test]
        [TestCase("Balancetype")]
        public void TestAtBalancetypeSetterRejserPropertyChangedVedOpdateringAfBalancetype(string propertyNameToRaise)
        {
            var fixture = new Fixture();
            fixture.Customize<Balancetype>(e => e.FromFactory(() => Balancetype.Aktiver));

            var kontogruppeModel = new KontogruppeModel(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<Balancetype>());
            Assert.That(kontogruppeModel, Is.Not.Null);

            var eventCalled = false;
            kontogruppeModel.PropertyChanged += (s, e) =>
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
            kontogruppeModel.Balancetype = kontogruppeModel.Balancetype;
            Assert.That(eventCalled, Is.False);
            kontogruppeModel.Balancetype = Balancetype.Passiver;
            Assert.That(eventCalled, Is.True);
        }
    }
}
