using System;
using System.Globalization;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Models.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Resources;

namespace OSDevGrp.OSIntranet.Gui.Models.Tests.Finansstyring
{
    /// <summary>
    /// Tester modellen, der indeholder de grundlæggende oplysninger for en kontogruppe.
    /// </summary>
    [TestFixture]
    public class KontogruppeModelBaseTests
    {
        /// <summary>
        /// Egen klasse til test af modellen, der indeholder de grundlæggende oplysninger for en kontogruppe.
        /// </summary>
        private class MyKontogruppeModel : KontogruppeModelBase
        {
            #region Constructor

            /// <summary>
            /// Danner engen klasse til test af modellen, der indeholder de grundlæggende oplysninger for en kontogruppe.
            /// </summary>
            /// <param name="nummer">>Unik identifikation af kontogruppen.</param>
            /// <param name="tekst">Teksten, der beskriver kontogruppen.</param>
            public MyKontogruppeModel(int nummer, string tekst)
                : base(nummer, tekst)
            {
            }

            #endregion
        }

        /// <summary>
        /// Tester, at konstruktøren initierer en model, der indeholder de grundlæggende oplysninger for en kontogruppe.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererKontogruppeModel()
        {
            var fixture = new Fixture();

            var nummer = fixture.Create<int>();
            var tekst = fixture.Create<string>();
            var kontogruppeModel = new MyKontogruppeModel(nummer, tekst);
            Assert.That(kontogruppeModel, Is.Not.Null);
            Assert.That(kontogruppeModel.Id, Is.Not.Null);
            Assert.That(kontogruppeModel.Id, Is.Not.Empty);
            Assert.That(kontogruppeModel.Id, Is.EqualTo(nummer.ToString(CultureInfo.InvariantCulture)));
            Assert.That(kontogruppeModel.Nummer, Is.EqualTo(nummer));
            Assert.That(kontogruppeModel.Tekst, Is.Not.Null);
            Assert.That(kontogruppeModel.Tekst, Is.Not.Empty);
            Assert.That(kontogruppeModel.Tekst, Is.EqualTo(tekst));
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

            var exception = Assert.Throws<ArgumentException>(() => new MyKontogruppeModel(invalidValue, fixture.Create<string>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Does.StartWith(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "nummer", invalidValue)));
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

            var exception = Assert.Throws<ArgumentNullException>(() => new MyKontogruppeModel(fixture.Create<int>(), invalidValue));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("tekst"));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
