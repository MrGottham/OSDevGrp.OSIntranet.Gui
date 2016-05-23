using System;
using System.Globalization;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Models.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Resources;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Gui.Models.Tests.Finansstyring
{
    /// <summary>
    /// Tester modellen til en budgetkontogruppe.
    /// </summary>
    [TestFixture]
    public class BudgetkontogruppeModelTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en model til en budgetkontogruppe.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererBudgetkontogruppeModel()
        {
            var fixture = new Fixture();

            var nummer = fixture.Create<int>();
            var tekst = fixture.Create<string>();
            var budgetkontogruppeModel = new BudgetkontogruppeModel(nummer, tekst);
            Assert.That(budgetkontogruppeModel, Is.Not.Null);
            Assert.That(budgetkontogruppeModel.Id, Is.Not.Null);
            Assert.That(budgetkontogruppeModel.Id, Is.Not.Empty);
            Assert.That(budgetkontogruppeModel.Id, Is.EqualTo(nummer.ToString(CultureInfo.InvariantCulture)));
            Assert.That(budgetkontogruppeModel.Nummer, Is.EqualTo(nummer));
            Assert.That(budgetkontogruppeModel.Tekst, Is.Not.Null);
            Assert.That(budgetkontogruppeModel.Tekst, Is.Not.Empty);
            Assert.That(budgetkontogruppeModel.Tekst, Is.EqualTo(tekst));
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

            var exception = Assert.Throws<ArgumentException>(() => new BudgetkontogruppeModel(invalidValue, fixture.Create<string>()));
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

            var exception = Assert.Throws<ArgumentNullException>(() => new BudgetkontogruppeModel(fixture.Create<int>(), invalidValue));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("tekst"));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
