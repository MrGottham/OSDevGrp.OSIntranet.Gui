using System;
using AutoFixture;
using AutoFixture.Kernel;
using Moq;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests
{
    internal static class ModelBuilder
    {
        internal static IBogføringslinjeModel BuildBogføringslinjeModel(this ISpecimenBuilder fixture, int? regnskabsnummer = null, int? løbenummer = null, DateTime? dato = null, bool hasBilag = true, string bilag = null, string kontonummer = null, string tekst = null, bool hasBudgetkontonummer = true, string budgetkontonummer = null, decimal? debit = null, decimal? kredit = null, bool hasAdressekonto = true, int? adressekonto = null, Nyhedsaktualitet? nyhedsaktualitet = null, DateTime? nyhedsudgivelsestidspunkt = null, string nyhedsinformation = null, Action<Mock<IBogføringslinjeModel>> setupCallback = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return BuildBogføringslinjeModelMock(fixture, regnskabsnummer, løbenummer, dato, hasBilag, bilag, kontonummer, tekst, hasBudgetkontonummer, budgetkontonummer, debit, kredit, hasAdressekonto, adressekonto, nyhedsaktualitet, nyhedsudgivelsestidspunkt, nyhedsinformation, setupCallback).Object;
        }

        internal static Mock<IBogføringslinjeModel> BuildBogføringslinjeModelMock(this ISpecimenBuilder fixture, int? regnskabsnummer = null, int? løbenummer = null, DateTime? dato = null, bool hasBilag = true, string bilag = null, string kontonummer = null, string tekst = null, bool hasBudgetkontonummer = true, string budgetkontonummer = null, decimal? debit = null, decimal? kredit = null, bool hasAdressekonto = true, int? adressekonto = null, Nyhedsaktualitet? nyhedsaktualitet = null, DateTime? nyhedsudgivelsestidspunkt = null, string nyhedsinformation = null, Action<Mock<IBogføringslinjeModel>> setupCallback = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            Random random = new Random(fixture.Create<int>());

            if (debit.HasValue == false)
            {
                debit = kredit.HasValue ? null : random.Next(100) > 50 ? Math.Abs(fixture.Create<decimal>()) : (decimal?)null;
            }

            if (kredit.HasValue == false)
            {
                kredit = debit.HasValue ? null : (decimal?)Math.Abs(fixture.Create<decimal>());
            }

            Mock<IBogføringslinjeModel> mock = new Mock<IBogføringslinjeModel>();
            mock.Setup(m => m.Regnskabsnummer)
                .Returns(regnskabsnummer ?? fixture.Create<int>());
            mock.Setup(m => m.Løbenummer)
                .Returns(løbenummer ?? fixture.Create<int>());
            mock.Setup(m => m.Dato)
                .Returns((dato ?? DateTime.Today.AddDays(random.Next(0, 365) * -1)).Date);
            mock.Setup(m => m.Bilag)
                .Returns(hasBilag ? bilag ?? fixture.Create<string>() : null);
            mock.Setup(m => m.Kontonummer)
                .Returns(kontonummer ?? fixture.Create<string>());
            mock.Setup(m => m.Tekst)
                .Returns(tekst ?? fixture.Create<string>());
            mock.Setup(m => m.Budgetkontonummer)
                .Returns(budgetkontonummer ?? fixture.Create<string>());
            mock.Setup(m => m.Debit)
                .Returns(debit ?? 0M);
            mock.Setup(m => m.Kredit)
                .Returns(kredit ?? 0M);
            mock.Setup(m => m.Bogført)
                .Returns((debit ?? 0M) - (kredit ?? 0M));
            mock.Setup(m => m.Adressekonto)
                .Returns(hasAdressekonto ? adressekonto ?? fixture.Create<int>() : 0);
            mock.Setup(m => m.Nyhedsaktualitet)
                .Returns(nyhedsaktualitet ?? fixture.Create<Nyhedsaktualitet>());
            mock.Setup(m => m.Nyhedsudgivelsestidspunkt)
                .Returns(nyhedsudgivelsestidspunkt ?? DateTime.Now.AddDays(random.Next(0, 7) * -1));
            mock.Setup(m => m.Nyhedsinformation)
                .Returns(nyhedsinformation ?? fixture.Create<string>());
            return mock;
        }

        internal static IBogføringsadvarselModel BuildBogføringsadvarselModel(this ISpecimenBuilder fixture, string advarsel = null, string kontonummer = null, string kontonavn = null, decimal? beløb = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return BuildBogføringsadvarselModelMock(fixture, advarsel, kontonummer, kontonavn, beløb).Object;
        }

        internal static Mock<IBogføringsadvarselModel> BuildBogføringsadvarselModelMock(this ISpecimenBuilder fixture, string advarsel = null, string kontonummer = null, string kontonavn = null, decimal? beløb = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            Mock<IBogføringsadvarselModel> mock = new Mock<IBogføringsadvarselModel>();
            mock.Setup(m => m.Advarsel)
                .Returns(advarsel ?? fixture.Create<string>());
            mock.Setup(m => m.Kontonummer)
                .Returns(kontonummer ?? fixture.Create<string>());
            mock.Setup(m => m.Kontonavn)
                .Returns(kontonavn ?? fixture.Create<string>());
            mock.Setup(m => m.Beløb)
                .Returns(beløb ?? fixture.Create<decimal>() * -1);
            return mock;
        }
    }
}