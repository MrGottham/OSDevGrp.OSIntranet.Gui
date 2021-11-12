using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Models.Core;

namespace OSDevGrp.OSIntranet.Gui.Models.Tests.Core
{
    /// <summary>
    /// Tester modellen, der indeholder grundlæggende tabeloplysninger, såsom typer, grupper m.m.
    /// </summary>
    [TestFixture]
    public class TabelModelBaseTests
    {
        /// <summary>
        /// Egen klasse til test at modellen, der indeholder grundlæggende tabeloplysninger, såsom typer, grupper m.m.
        /// </summary>
        private class MyTabelModel : TabelModelBase
        {
            #region Constructor

            /// <summary>
            /// Danner egen klasse til test at modellen, der indeholder grundlæggende tabeloplysninger, såsom typer, grupper m.m.
            /// </summary>
            /// <param name="id">Unik identifikation af de grundlæggende tabeloplysningerne i denne model.</param>
            /// <param name="tekst">Teksten der beskriver de grundlæggende tabeloplysninger i denne model.</param>
            public MyTabelModel(string id, string tekst)
                : base(id, tekst)
            {
            }

            #endregion
        }

        /// <summary>
        /// Tester, at konstruktøren initierer en model, der indeholder grundlæggende tabeloplysninger, såsom typer, grupper m.m.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererTabelModel()
        {
            var fixture = new Fixture();

            var id = fixture.Create<string>();
            var tekst = fixture.Create<string>();
            var tabelModel = new MyTabelModel(id, tekst);
            Assert.That(tabelModel, Is.Not.Null);
            Assert.That(tabelModel.Id, Is.Not.Null);
            Assert.That(tabelModel.Id, Is.Not.Empty);
            Assert.That(tabelModel.Id, Is.EqualTo(id));
            Assert.That(tabelModel.Tekst, Is.Not.Null);
            Assert.That(tabelModel.Tekst, Is.Not.Empty);
            Assert.That(tabelModel.Tekst, Is.EqualTo(tekst));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis den unikke identifikation er invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestAtConstructorKasterArgumentNullExceptionHvisIdErInvalid(string invalidValue)
        {
            var fixture = new Fixture();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyTabelModel(invalidValue, fixture.Create<string>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("id"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis teksten er invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestAtConstructorKasterArgumentNullExceptionHvisTekstErInvalid(string invalidValue)
        {
            var fixture = new Fixture();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyTabelModel(fixture.Create<string>(), invalidValue));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("tekst"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at sætteren til Tekst kaster en ArgumentNullException, hvis value er invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestAtTekstSetterKasterArgumentNullExceptionHvisValueErInvalid(string invalidValue)
        {
            var fixture = new Fixture();

            var tabelModel = new MyTabelModel(fixture.Create<string>(), fixture.Create<string>());
            Assert.That(tabelModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => tabelModel.Tekst = invalidValue);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("value"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at sætteren til Tekst opdaterer teksten, der beskriver de grundlæggende tabeloplysninger i denne model.
        /// </summary>
        [Test]
        public void TestAtTekstSetterOpdatererTekst()
        {
            var fixture = new Fixture();

            var tabelModel = new MyTabelModel(fixture.Create<string>(), fixture.Create<string>());
            Assert.That(tabelModel, Is.Not.Null);

            var newValue = fixture.Create<string>();
            Assert.That(newValue, Is.Not.EqualTo(tabelModel.Tekst));

            tabelModel.Tekst = newValue;
            Assert.That(newValue, Is.Not.Null);
            Assert.That(newValue, Is.Not.Empty);
            Assert.That(newValue, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tester, at sætteren til Tekst rejser PropertyChanged ved opdatering af teksten, der beskriver de grundlæggende tabeloplysninger i denne model.
        /// </summary>
        [Test]
        [TestCase("Tekst")]
        public void TestAtTekstSetterRejserPropertyChangedVedOpdateringAfTekst(string propertyNameToRaise)
        {
            var fixture = new Fixture();

            var tabelModel = new MyTabelModel(fixture.Create<string>(), fixture.Create<string>());
            Assert.That(tabelModel, Is.Not.Null);

            var eventCalled = false;
            tabelModel.PropertyChanged += (s, e) =>
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
            tabelModel.Tekst = tabelModel.Tekst;
            Assert.That(eventCalled, Is.False);
            tabelModel.Tekst = fixture.Create<string>();
            Assert.That(eventCalled, Is.True);
        }
    }
}
