using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Models.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Resources;

namespace OSDevGrp.OSIntranet.Gui.Models.Tests.Finansstyring
{
    /// <summary>
    /// Tester modellen til en budgetkonto.
    /// </summary>
    [TestFixture]
    public class BudgetkontoModelTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en model til en budgetkonto uden budgetteret beløb og bogført beløb.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererBudgetkontoModelUdenBudgetOgBogført()
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
            Assert.That(budgetkontoModel.BudgetSidsteMåned, Is.EqualTo(0M));
            Assert.That(budgetkontoModel.BudgetÅrTilDato, Is.EqualTo(0M));
            Assert.That(budgetkontoModel.BudgetSidsteÅr, Is.EqualTo(0M));
            Assert.That(budgetkontoModel.Bogført, Is.EqualTo(0M));
            Assert.That(budgetkontoModel.BogførtSidsteMåned, Is.EqualTo(0M));
            Assert.That(budgetkontoModel.BogførtÅrTilDato, Is.EqualTo(0M));
            Assert.That(budgetkontoModel.BogførtSidsteÅr, Is.EqualTo(0M));
            Assert.That(budgetkontoModel.Disponibel, Is.EqualTo(0M));
        }

        /// <summary>
        /// Tester, at konstruktøren initierer en model til en budgetkonto med et positivt budgetterede beløb og uden bogført beløb.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererBudgetkontoModelMedPositivBudgetOgUdenBogført()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var regnskabsnummer = fixture.Create<int>();
            var kontonummer = fixture.Create<string>();
            var kontonavn = fixture.Create<string>();
            var kontogruppe = fixture.Create<int>();
            var statusDato = fixture.Create<DateTime>();
            var budget = Math.Abs(fixture.Create<decimal>());
            var budgetkontoModel = new BudgetkontoModel(regnskabsnummer, kontonummer, kontonavn, kontogruppe, statusDato, budget);
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
            Assert.That(budgetkontoModel.Indtægter, Is.EqualTo(budget));
            Assert.That(budgetkontoModel.Udgifter, Is.EqualTo(0M));
            Assert.That(budgetkontoModel.Budget, Is.EqualTo(budget));
            Assert.That(budgetkontoModel.BudgetSidsteMåned, Is.EqualTo(0M));
            Assert.That(budgetkontoModel.BudgetÅrTilDato, Is.EqualTo(0M));
            Assert.That(budgetkontoModel.BudgetSidsteÅr, Is.EqualTo(0M));
            Assert.That(budgetkontoModel.Bogført, Is.EqualTo(0M));
            Assert.That(budgetkontoModel.BogførtSidsteMåned, Is.EqualTo(0M));
            Assert.That(budgetkontoModel.BogførtÅrTilDato, Is.EqualTo(0M));
            Assert.That(budgetkontoModel.BogførtSidsteÅr, Is.EqualTo(0M));
            Assert.That(budgetkontoModel.Disponibel, Is.EqualTo(0M));
        }

        /// <summary>
        /// Tester, at konstruktøren initierer en model til en budgetkonto med et negativt budgetterede beløb og uden bogført beløb.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererBudgetkontoModelMedNegativBudgetOgUdenBogført()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var regnskabsnummer = fixture.Create<int>();
            var kontonummer = fixture.Create<string>();
            var kontonavn = fixture.Create<string>();
            var kontogruppe = fixture.Create<int>();
            var statusDato = fixture.Create<DateTime>();
            var budget = Math.Abs(fixture.Create<decimal>())*-1;
            var budgetkontoModel = new BudgetkontoModel(regnskabsnummer, kontonummer, kontonavn, kontogruppe, statusDato, budget);
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
            Assert.That(budgetkontoModel.Udgifter, Is.EqualTo(Math.Abs(budget)));
            Assert.That(budgetkontoModel.Budget, Is.EqualTo(budget));
            Assert.That(budgetkontoModel.BudgetSidsteMåned, Is.EqualTo(0M));
            Assert.That(budgetkontoModel.BudgetÅrTilDato, Is.EqualTo(0M));
            Assert.That(budgetkontoModel.BudgetSidsteÅr, Is.EqualTo(0M));
            Assert.That(budgetkontoModel.Bogført, Is.EqualTo(0M));
            Assert.That(budgetkontoModel.BogførtSidsteMåned, Is.EqualTo(0M));
            Assert.That(budgetkontoModel.BogførtÅrTilDato, Is.EqualTo(0M));
            Assert.That(budgetkontoModel.BogførtSidsteÅr, Is.EqualTo(0M));
            Assert.That(budgetkontoModel.Disponibel, Is.EqualTo(Math.Abs(budget) - 0M));
        }

        /// <summary>
        /// Tester, at konstruktøren initierer en model til en budgetkonto uden budgetterede beløb og med bogført beløb.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererBudgetkontoModelUdenBudgetOgMedBogført()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var regnskabsnummer = fixture.Create<int>();
            var kontonummer = fixture.Create<string>();
            var kontonavn = fixture.Create<string>();
            var kontogruppe = fixture.Create<int>();
            var statusDato = fixture.Create<DateTime>();
            var bogført = Math.Abs(fixture.Create<decimal>())*-1;
            var budgetkontoModel = new BudgetkontoModel(regnskabsnummer, kontonummer, kontonavn, kontogruppe, statusDato, 0M, bogført);
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
            Assert.That(budgetkontoModel.BudgetSidsteMåned, Is.EqualTo(0M));
            Assert.That(budgetkontoModel.BudgetÅrTilDato, Is.EqualTo(0M));
            Assert.That(budgetkontoModel.BudgetSidsteÅr, Is.EqualTo(0M));
            Assert.That(budgetkontoModel.Bogført, Is.EqualTo(bogført));
            Assert.That(budgetkontoModel.BogførtSidsteMåned, Is.EqualTo(0M));
            Assert.That(budgetkontoModel.BogførtÅrTilDato, Is.EqualTo(0M));
            Assert.That(budgetkontoModel.BogførtSidsteÅr, Is.EqualTo(0M));
            Assert.That(budgetkontoModel.Disponibel, Is.EqualTo(0M));
        }

        /// <summary>
        /// Tester, at konstruktøren initierer en model til en budgetkonto med budgetterede beløb og med bogført beløb.
        /// </summary>
        [Test]
        [TestCase(-1000, -500, 500)]
        [TestCase(-1000, -500, 500)]
        [TestCase(-1000, -750, 250)]
        [TestCase(-1000, -1000, 0)]
        [TestCase(-1000, -1500, 0)]
        [TestCase(-1000, -1750, 0)]
        [TestCase(1000, -1000, 0)]
        [TestCase(1000, -750, 0)]
        [TestCase(1000, -500, 0)]
        [TestCase(1000, 500, 0)]
        [TestCase(1000, 750, 0)]
        [TestCase(1000, 1000, 0)]
        public void TestAtConstructorInitiererBudgetkontoModelMedBudgetOgBogført(decimal budget, decimal bogført, decimal expectedDisponibel)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var regnskabsnummer = fixture.Create<int>();
            var kontonummer = fixture.Create<string>();
            var kontonavn = fixture.Create<string>();
            var kontogruppe = fixture.Create<int>();
            var statusDato = fixture.Create<DateTime>();
            var budgetkontoModel = new BudgetkontoModel(regnskabsnummer, kontonummer, kontonavn, kontogruppe, statusDato, budget, bogført);
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
            Assert.That(budgetkontoModel.Indtægter, Is.EqualTo(budget > 0M ? Math.Abs(budget) : 0M));
            Assert.That(budgetkontoModel.Udgifter, Is.EqualTo(budget > 0M ? 0M : Math.Abs(budget)));
            Assert.That(budgetkontoModel.Budget, Is.EqualTo(budget));
            Assert.That(budgetkontoModel.BudgetSidsteMåned, Is.EqualTo(0M));
            Assert.That(budgetkontoModel.BudgetÅrTilDato, Is.EqualTo(0M));
            Assert.That(budgetkontoModel.BudgetSidsteÅr, Is.EqualTo(0M));
            Assert.That(budgetkontoModel.Bogført, Is.EqualTo(bogført));
            Assert.That(budgetkontoModel.BogførtSidsteMåned, Is.EqualTo(0M));
            Assert.That(budgetkontoModel.BogførtÅrTilDato, Is.EqualTo(0M));
            Assert.That(budgetkontoModel.BogførtSidsteÅr, Is.EqualTo(0M));
            Assert.That(budgetkontoModel.Disponibel, Is.EqualTo(expectedDisponibel));
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
            Assert.That(exception.Message, Does.StartWith(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "kontogruppe", illegalValue)));
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("kontogruppe"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at sætteren til Indtægter kaster en ArgumentException ved ulovlige værdier
        /// </summary>
        [Test]
        [TestCase(-1024)]
        [TestCase(-1)]
        public void TestAtIndtægterSetterKasterArgumentExceptionVedIllegalValues(decimal illegalvalue)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var budgetkontoModel = new BudgetkontoModel(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<decimal>(), fixture.Create<decimal>());
            Assert.That(budgetkontoModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentException>(() => budgetkontoModel.Indtægter = illegalvalue);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Does.StartWith(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "value", illegalvalue)));
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("value"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at sætteren til Indtægter opdaterer budgetterede indtægter.
        /// </summary>
        [Test]
        public void TestAtIndtægterSetterOpdatererIndtægter()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var budget = Math.Abs(fixture.Create<decimal>());
            var bogført = Math.Abs(fixture.Create<decimal>());
            var budgetkontoModel = new BudgetkontoModel(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>(), fixture.Create<DateTime>(), budget, bogført);
            Assert.That(budgetkontoModel, Is.Not.Null);
            Assert.That(budgetkontoModel.Indtægter, Is.EqualTo(budget));
            Assert.That(budgetkontoModel.Udgifter, Is.EqualTo(0M));
            Assert.That(budgetkontoModel.Budget, Is.EqualTo(budget));
            Assert.That(budgetkontoModel.Bogført, Is.EqualTo(bogført));
            Assert.That(budgetkontoModel.Disponibel, Is.EqualTo(0M));

            var newValue = Math.Abs(fixture.Create<decimal>());
            Assert.That(budgetkontoModel.Indtægter, Is.Not.EqualTo(newValue));

            budgetkontoModel.Indtægter = newValue;
            Assert.That(budgetkontoModel.Indtægter, Is.EqualTo(newValue));
            Assert.That(budgetkontoModel.Udgifter, Is.EqualTo(0M));
            Assert.That(budgetkontoModel.Budget, Is.EqualTo(newValue));
            Assert.That(budgetkontoModel.Bogført, Is.EqualTo(bogført));
            Assert.That(budgetkontoModel.Disponibel, Is.EqualTo(0M));
        }

        /// <summary>
        /// Tester, at sætteren til Indtægter rejser PropertyChanged ved opdatering af budgetterede indtægter.
        /// </summary>
        [TestCase("Indtægter")]
        [TestCase("Budget")]
        [TestCase("Disponibel")]
        public void TestAtIndtægterSetterRejserPropertyChangedVedOpdateringAfIndtægter(string propertyNameToRaise)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var budgetkontoModel = new BudgetkontoModel(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<decimal>(), fixture.Create<decimal>());
            Assert.That(budgetkontoModel, Is.Not.Null);

            var eventCalled = false;
            budgetkontoModel.PropertyChanged += (s, e) =>
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
            budgetkontoModel.Indtægter = budgetkontoModel.Indtægter;
            Assert.That(eventCalled, Is.False);
            budgetkontoModel.Indtægter = Math.Abs(fixture.Create<decimal>());
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at sætteren til Udgifter kaster en ArgumentException ved ulovlige værdier
        /// </summary>
        [Test]
        [TestCase(-1024)]
        [TestCase(-1)]
        public void TestAtUdgifterSetterKasterArgumentExceptionVedIllegalValues(decimal illegalvalue)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var budgetkontoModel = new BudgetkontoModel(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>(), fixture.Create<DateTime>(), Math.Abs(fixture.Create<decimal>())*-1, Math.Abs(fixture.Create<decimal>())*-1);
            Assert.That(budgetkontoModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentException>(() => budgetkontoModel.Udgifter = illegalvalue);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Does.StartWith(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "value", illegalvalue)));
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("value"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at sætteren til Udgifter opdaterer budgetterede udgifter.
        /// </summary>
        [Test]
        public void TestAtUdgifterSetterOpdatererUdgifter()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var budget = Math.Abs(fixture.Create<decimal>())*-1;
            var bogført = (budget/4)*2;
            var budgetkontoModel = new BudgetkontoModel(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>(), fixture.Create<DateTime>(), budget, bogført);
            Assert.That(budgetkontoModel, Is.Not.Null);
            Assert.That(budgetkontoModel.Indtægter, Is.EqualTo(0M));
            Assert.That(budgetkontoModel.Udgifter, Is.EqualTo(Math.Abs(budget)));
            Assert.That(budgetkontoModel.Budget, Is.EqualTo(budget));
            Assert.That(budgetkontoModel.Bogført, Is.EqualTo(bogført));
            Assert.That(budgetkontoModel.Disponibel, Is.EqualTo(Math.Abs(budget) - Math.Abs(bogført)));

            var newValue = Math.Abs((budget/4)*3);
            Assert.That(budgetkontoModel.Udgifter, Is.Not.EqualTo(newValue));

            budgetkontoModel.Udgifter = newValue;
            Assert.That(budgetkontoModel.Indtægter, Is.EqualTo(0));
            Assert.That(budgetkontoModel.Udgifter, Is.EqualTo(newValue));
            Assert.That(budgetkontoModel.Budget, Is.EqualTo(newValue*-1));
            Assert.That(budgetkontoModel.Bogført, Is.EqualTo(bogført));
            Assert.That(budgetkontoModel.Disponibel, Is.EqualTo(Math.Abs(newValue) - Math.Abs(bogført)));
        }

        /// <summary>
        /// Tester, at sætteren til Udgifter rejser PropertyChanged ved opdatering af budgetterede udgifter.
        /// </summary>
        [TestCase("Udgifter")]
        [TestCase("Budget")]
        [TestCase("Disponibel")]
        public void TestAtUdgifterSetterRejserPropertyChangedVedOpdateringAfUdgifter(string propertyNameToRaise)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var budgetkontoModel = new BudgetkontoModel(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>(), fixture.Create<DateTime>(), Math.Abs(fixture.Create<decimal>())*-1, Math.Abs(fixture.Create<decimal>())*-1);
            Assert.That(budgetkontoModel, Is.Not.Null);

            var eventCalled = false;
            budgetkontoModel.PropertyChanged += (s, e) =>
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
            budgetkontoModel.Udgifter = budgetkontoModel.Udgifter;
            Assert.That(eventCalled, Is.False);
            budgetkontoModel.Udgifter = Math.Abs(fixture.Create<decimal>());
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at sætteren til BudgetSidsteMåned opdaterer det budgetterede beløb for sidste måned.
        /// </summary>
        [Test]
        public void TestAtBudgetSidsteMånedSetterOpdatererBudgetSidsteMåned()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var budgetkontoModel = new BudgetkontoModel(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>(), fixture.Create<DateTime>(), Math.Abs(fixture.Create<decimal>()) * -1, Math.Abs(fixture.Create<decimal>()) * -1);
            Assert.That(budgetkontoModel, Is.Not.Null);
            Assert.That(budgetkontoModel.BudgetSidsteMåned, Is.EqualTo(0M));

            var newValue = fixture.Create<decimal>();
            budgetkontoModel.BudgetSidsteMåned = newValue;
            Assert.That(budgetkontoModel.BudgetSidsteMåned, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tester, at sætteren til BudgetSidsteMåned rejser PropertyChanged ved opdatering af det budgetterede beløb for sidste måned.
        /// </summary>
        [TestCase("BudgetSidsteMåned")]
        public void TestAtBudgetSidsteMånedSetterRejserPropertyChangedVedOpdateringAfBudgetSidsteMåned(string propertyNameToRaise)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var budgetkontoModel = new BudgetkontoModel(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>(), fixture.Create<DateTime>(), Math.Abs(fixture.Create<decimal>()) * -1, Math.Abs(fixture.Create<decimal>()) * -1);
            Assert.That(budgetkontoModel, Is.Not.Null);

            var eventCalled = false;
            budgetkontoModel.PropertyChanged += (s, e) =>
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
            budgetkontoModel.BudgetSidsteMåned = budgetkontoModel.BudgetSidsteMåned;
            Assert.That(eventCalled, Is.False);
            budgetkontoModel.BudgetSidsteMåned = Math.Abs(fixture.Create<decimal>());
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at sætteren til BudgetÅrTilDato opdaterer det budgetterede beløb for år til dato.
        /// </summary>
        [Test]
        public void TestAtBudgetÅrTilDatoSetterOpdatererBudgetÅrTilDato()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var budgetkontoModel = new BudgetkontoModel(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>(), fixture.Create<DateTime>(), Math.Abs(fixture.Create<decimal>()) * -1, Math.Abs(fixture.Create<decimal>()) * -1);
            Assert.That(budgetkontoModel, Is.Not.Null);
            Assert.That(budgetkontoModel.BudgetÅrTilDato, Is.EqualTo(0M));

            var newValue = fixture.Create<decimal>();
            budgetkontoModel.BudgetÅrTilDato = newValue;
            Assert.That(budgetkontoModel.BudgetÅrTilDato, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tester, at sætteren til BudgetÅrTilDato rejser PropertyChanged ved opdatering af det budgetterede beløb for år til dato.
        /// </summary>
        [TestCase("BudgetÅrTilDato")]
        public void TestAtBudgetÅrTilDatoSetterRejserPropertyChangedVedOpdateringAfBudgetÅrTilDato(string propertyNameToRaise)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var budgetkontoModel = new BudgetkontoModel(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>(), fixture.Create<DateTime>(), Math.Abs(fixture.Create<decimal>()) * -1, Math.Abs(fixture.Create<decimal>()) * -1);
            Assert.That(budgetkontoModel, Is.Not.Null);

            var eventCalled = false;
            budgetkontoModel.PropertyChanged += (s, e) =>
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
            budgetkontoModel.BudgetÅrTilDato = budgetkontoModel.BudgetÅrTilDato;
            Assert.That(eventCalled, Is.False);
            budgetkontoModel.BudgetÅrTilDato = Math.Abs(fixture.Create<decimal>());
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at sætteren til BudgetSidsteÅr opdaterer det budgetterede beløb for sidste år.
        /// </summary>
        [Test]
        public void TestAtBudgetSidsteÅrSetterOpdatererBudgetSidsteÅr()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var budgetkontoModel = new BudgetkontoModel(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>(), fixture.Create<DateTime>(), Math.Abs(fixture.Create<decimal>()) * -1, Math.Abs(fixture.Create<decimal>()) * -1);
            Assert.That(budgetkontoModel, Is.Not.Null);
            Assert.That(budgetkontoModel.BudgetSidsteÅr, Is.EqualTo(0M));

            var newValue = fixture.Create<decimal>();
            budgetkontoModel.BudgetSidsteÅr = newValue;
            Assert.That(budgetkontoModel.BudgetSidsteÅr, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tester, at sætteren til BudgetSidsteÅr rejser PropertyChanged ved opdatering af det budgetterede beløb for sidste år.
        /// </summary>
        [TestCase("BudgetSidsteÅr")]
        public void TestAtBudgetSidsteÅrSetterRejserPropertyChangedVedOpdateringAfBudgetSidsteÅr(string propertyNameToRaise)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var budgetkontoModel = new BudgetkontoModel(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>(), fixture.Create<DateTime>(), Math.Abs(fixture.Create<decimal>()) * -1, Math.Abs(fixture.Create<decimal>()) * -1);
            Assert.That(budgetkontoModel, Is.Not.Null);

            var eventCalled = false;
            budgetkontoModel.PropertyChanged += (s, e) =>
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
            budgetkontoModel.BudgetSidsteÅr = budgetkontoModel.BudgetSidsteÅr;
            Assert.That(eventCalled, Is.False);
            budgetkontoModel.BudgetSidsteÅr = Math.Abs(fixture.Create<decimal>());
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at sætteren til Bogført opdaterer det bogførte beløb.
        /// </summary>
        [Test]
        public void TestAtBogførtSetterOpdatererBogført()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var budget = Math.Abs(fixture.Create<decimal>())*-1;
            var bogført = (budget/4)*2;
            var budgetkontoModel = new BudgetkontoModel(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>(), fixture.Create<DateTime>(), budget, bogført);
            Assert.That(budgetkontoModel, Is.Not.Null);
            Assert.That(budgetkontoModel.Indtægter, Is.EqualTo(0M));
            Assert.That(budgetkontoModel.Udgifter, Is.EqualTo(Math.Abs(budget)));
            Assert.That(budgetkontoModel.Budget, Is.EqualTo(budget));
            Assert.That(budgetkontoModel.Bogført, Is.EqualTo(bogført));
            Assert.That(budgetkontoModel.Disponibel, Is.EqualTo(Math.Abs(budget) - Math.Abs(bogført)));

            var newValue = (budget/4)*3;
            Assert.That(budgetkontoModel.Bogført, Is.Not.EqualTo(newValue));

            budgetkontoModel.Bogført = newValue;
            Assert.That(budgetkontoModel.Indtægter, Is.EqualTo(0));
            Assert.That(budgetkontoModel.Udgifter, Is.EqualTo(Math.Abs(budget)));
            Assert.That(budgetkontoModel.Budget, Is.EqualTo(budget));
            Assert.That(budgetkontoModel.Bogført, Is.EqualTo(newValue));
            Assert.That(budgetkontoModel.Disponibel, Is.EqualTo(Math.Abs(budget) - Math.Abs(newValue)));
        }

        /// <summary>
        /// Tester, at sætteren til Bogført rejser PropertyChanged ved opdatering af det bogførte beløb.
        /// </summary>
        [TestCase("Bogført")]
        [TestCase("Disponibel")]
        public void TestAtBogførtSetterRejserPropertyChangedVedOpdateringAfBogført(string propertyNameToRaise)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var budgetkontoModel = new BudgetkontoModel(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>(), fixture.Create<DateTime>(), Math.Abs(fixture.Create<decimal>()) * -1, Math.Abs(fixture.Create<decimal>()) * -1);
            Assert.That(budgetkontoModel, Is.Not.Null);

            var eventCalled = false;
            budgetkontoModel.PropertyChanged += (s, e) =>
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
            budgetkontoModel.Bogført = budgetkontoModel.Bogført;
            Assert.That(eventCalled, Is.False);
            budgetkontoModel.Bogført = Math.Abs(fixture.Create<decimal>());
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at sætteren til BogførtSidsteMåned opdaterer det bogførte beløb for sidste måned.
        /// </summary>
        [Test]
        public void TestAtBogførtSidsteMånedSetterOpdatererBogførtSidsteMåned()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var budgetkontoModel = new BudgetkontoModel(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>(), fixture.Create<DateTime>(), Math.Abs(fixture.Create<decimal>()) * -1, Math.Abs(fixture.Create<decimal>()) * -1);
            Assert.That(budgetkontoModel, Is.Not.Null);
            Assert.That(budgetkontoModel.BogførtSidsteMåned, Is.EqualTo(0M));

            var newValue = fixture.Create<decimal>();
            budgetkontoModel.BogførtSidsteMåned = newValue;
            Assert.That(budgetkontoModel.BogførtSidsteMåned, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tester, at sætteren til BogførtSidsteMåned rejser PropertyChanged ved opdatering af det bogførte beløb for sidste måned.
        /// </summary>
        [TestCase("BogførtSidsteMåned")]
        public void TestAtBogførtSidsteMånedSetterRejserPropertyChangedVedOpdateringAfBogførtSidsteMåned(string propertyNameToRaise)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var budgetkontoModel = new BudgetkontoModel(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>(), fixture.Create<DateTime>(), Math.Abs(fixture.Create<decimal>()) * -1, Math.Abs(fixture.Create<decimal>()) * -1);
            Assert.That(budgetkontoModel, Is.Not.Null);

            var eventCalled = false;
            budgetkontoModel.PropertyChanged += (s, e) =>
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
            budgetkontoModel.BogførtSidsteMåned = budgetkontoModel.BogførtSidsteMåned;
            Assert.That(eventCalled, Is.False);
            budgetkontoModel.BogførtSidsteMåned = Math.Abs(fixture.Create<decimal>());
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at sætteren til BogførtÅrTilDato opdaterer det bogførte beløb for år til dato.
        /// </summary>
        [Test]
        public void TestAtBogførtÅrTilDatoSetterOpdatererBogførtÅrTilDato()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var budgetkontoModel = new BudgetkontoModel(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>(), fixture.Create<DateTime>(), Math.Abs(fixture.Create<decimal>()) * -1, Math.Abs(fixture.Create<decimal>()) * -1);
            Assert.That(budgetkontoModel, Is.Not.Null);
            Assert.That(budgetkontoModel.BogførtÅrTilDato, Is.EqualTo(0M));

            var newValue = fixture.Create<decimal>();
            budgetkontoModel.BogførtÅrTilDato = newValue;
            Assert.That(budgetkontoModel.BogførtÅrTilDato, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tester, at sætteren til BogførtÅrTilDato rejser PropertyChanged ved opdatering af det bogførte beløb for år til dato.
        /// </summary>
        [TestCase("BogførtÅrTilDato")]
        public void TestAtBogførtÅrTilDatoSetterRejserPropertyChangedVedOpdateringAfBogførtÅrTilDato(string propertyNameToRaise)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var budgetkontoModel = new BudgetkontoModel(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>(), fixture.Create<DateTime>(), Math.Abs(fixture.Create<decimal>()) * -1, Math.Abs(fixture.Create<decimal>()) * -1);
            Assert.That(budgetkontoModel, Is.Not.Null);

            var eventCalled = false;
            budgetkontoModel.PropertyChanged += (s, e) =>
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
            budgetkontoModel.BogførtÅrTilDato = budgetkontoModel.BogførtÅrTilDato;
            Assert.That(eventCalled, Is.False);
            budgetkontoModel.BogførtÅrTilDato = Math.Abs(fixture.Create<decimal>());
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at sætteren til BogførtSidsteÅr opdaterer det bogførte beløb for sidste år.
        /// </summary>
        [Test]
        public void TestAtBogførtSidsteÅrSetterOpdatererBogførtSidsteÅr()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var budgetkontoModel = new BudgetkontoModel(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>(), fixture.Create<DateTime>(), Math.Abs(fixture.Create<decimal>()) * -1, Math.Abs(fixture.Create<decimal>()) * -1);
            Assert.That(budgetkontoModel, Is.Not.Null);
            Assert.That(budgetkontoModel.BogførtSidsteÅr, Is.EqualTo(0M));

            var newValue = fixture.Create<decimal>();
            budgetkontoModel.BogførtSidsteÅr = newValue;
            Assert.That(budgetkontoModel.BogførtSidsteÅr, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tester, at sætteren til BogførtSidsteÅr rejser PropertyChanged ved opdatering af det bogførte beløb for sidste år.
        /// </summary>
        [TestCase("BogførtSidsteÅr")]
        public void TestAtBogførtSidsteÅrSetterRejserPropertyChangedVedOpdateringAfBogførtSidsteÅr(string propertyNameToRaise)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var budgetkontoModel = new BudgetkontoModel(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<int>(), fixture.Create<DateTime>(), Math.Abs(fixture.Create<decimal>()) * -1, Math.Abs(fixture.Create<decimal>()) * -1);
            Assert.That(budgetkontoModel, Is.Not.Null);

            var eventCalled = false;
            budgetkontoModel.PropertyChanged += (s, e) =>
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
            budgetkontoModel.BogførtSidsteÅr = budgetkontoModel.BogførtSidsteÅr;
            Assert.That(eventCalled, Is.False);
            budgetkontoModel.BogførtSidsteÅr = Math.Abs(fixture.Create<decimal>());
            Assert.That(eventCalled, Is.True);
        }
    }
}
