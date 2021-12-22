using System;
using AutoFixture;
using AutoFixture.Kernel;
using Moq;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Finansstyring.Tests
{
    internal static class DomainObjectBuilder
    {
        internal static IRegnskabModel BuildRegnskabModel(this ISpecimenBuilder fixture, int nummer)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            Mock<IRegnskabModel> regnskabModelMock = new Mock<IRegnskabModel>();
            regnskabModelMock.Setup(m => m.Nummer)
                .Returns(nummer);
            regnskabModelMock.Setup(m => m.Navn)
                .Returns(fixture.Create<string>());
            return regnskabModelMock.Object;
        }

        internal static IKontoModel BuildKontoModel(this ISpecimenBuilder fixture, Random random, int regnskabsnummer, string kontonummer, int kontogruppe, DateTime? statusdato = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            if (random == null)
            {
                throw new ArgumentNullException(nameof(random));
            }

            if (string.IsNullOrWhiteSpace(kontonummer))
            {
                throw new ArgumentNullException(nameof(kontonummer));
            }

            Mock<IKontoModel> kontoModelMock = new Mock<IKontoModel>();
            kontoModelMock.Setup(m => m.Regnskabsnummer)
                .Returns(regnskabsnummer);
            kontoModelMock.Setup(m => m.Kontonummer)
                .Returns(kontonummer);
            kontoModelMock.Setup(m => m.Kontonavn)
                .Returns(fixture.Create<string>());
            kontoModelMock.Setup(m => m.Beskrivelse)
                .Returns(random.Next(100) > 50 ? fixture.Create<string>() : null);
            kontoModelMock.Setup(m => m.Notat)
                .Returns(random.Next(100) > 50 ? fixture.Create<string>() : null);
            kontoModelMock.Setup(m => m.Kontogruppe)
                .Returns(kontogruppe);
            kontoModelMock.Setup(m => m.StatusDato)
                .Returns(statusdato ?? DateTime.Now.Date);
            kontoModelMock.Setup(m => m.Kredit)
                .Returns(fixture.Create<decimal>());
            kontoModelMock.Setup(m => m.Saldo)
                .Returns(fixture.Create<decimal>());
            kontoModelMock.Setup(m => m.Disponibel)
                .Returns(fixture.Create<decimal>());
            return kontoModelMock.Object;
        }

        internal static IBudgetkontoModel BuildBudgetkontoModel(this ISpecimenBuilder fixture, Random random, int regnskabsnummer, string kontonummer, int budgetkontogruppe, DateTime? statusdato = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            if (random == null)
            {
                throw new ArgumentNullException(nameof(random));
            }

            if (string.IsNullOrWhiteSpace(kontonummer))
            {
                throw new ArgumentNullException(nameof(kontonummer));
            }

            decimal indtægter = random.Next(100) > 50 ? Math.Abs(fixture.Create<decimal>()) : 0M;
            decimal udgifter = indtægter == 0M ? Math.Abs(fixture.Create<decimal>()) : 0M;
            decimal budget = indtægter - udgifter;
            Mock<IBudgetkontoModel> budgetkontoModelMock = new Mock<IBudgetkontoModel>();
            budgetkontoModelMock.Setup(m => m.Regnskabsnummer)
                .Returns(regnskabsnummer);
            budgetkontoModelMock.Setup(m => m.Kontonummer)
                .Returns(kontonummer);
            budgetkontoModelMock.Setup(m => m.Kontonavn)
                .Returns(fixture.Create<string>());
            budgetkontoModelMock.Setup(m => m.Beskrivelse)
                .Returns(random.Next(100) > 50 ? fixture.Create<string>() : null);
            budgetkontoModelMock.Setup(m => m.Notat)
                .Returns(random.Next(100) > 50 ? fixture.Create<string>() : null);
            budgetkontoModelMock.Setup(m => m.Kontogruppe)
                .Returns(budgetkontogruppe);
            budgetkontoModelMock.Setup(m => m.StatusDato)
                .Returns(statusdato ?? DateTime.Now.Date);
            budgetkontoModelMock.Setup(m => m.Indtægter)
                .Returns(indtægter);
            budgetkontoModelMock.Setup(m => m.Udgifter)
                .Returns(udgifter);
            budgetkontoModelMock.Setup(m => m.Budget)
                .Returns(budget);
            budgetkontoModelMock.Setup(m => m.BudgetSidsteMåned)
                .Returns(fixture.Create<decimal>() * (budget < 0M ? -1 : 1));
            budgetkontoModelMock.Setup(m => m.BudgetÅrTilDato)
                .Returns(fixture.Create<decimal>() * (budget < 0M ? -1 : 1));
            budgetkontoModelMock.Setup(m => m.BudgetSidsteÅr)
                .Returns(fixture.Create<decimal>() * (budget < 0M ? -1 : 1));
            budgetkontoModelMock.Setup(m => m.Bogført)
                .Returns(fixture.Create<decimal>() * (budget < 0M ? -1 : 1));
            budgetkontoModelMock.Setup(m => m.BogførtSidsteMåned)
                .Returns(fixture.Create<decimal>() * (budget < 0M ? -1 : 1));
            budgetkontoModelMock.Setup(m => m.BogførtÅrTilDato)
                .Returns(fixture.Create<decimal>() * (budget < 0M ? -1 : 1));
            budgetkontoModelMock.Setup(m => m.BogførtSidsteÅr)
                .Returns(fixture.Create<decimal>() * (budget < 0M ? -1 : 1));
            budgetkontoModelMock.Setup(m => m.Disponibel)
                .Returns(fixture.Create<decimal>());
            return budgetkontoModelMock.Object;
        }

        internal static IAdressekontoModel BuildAdressekontoModel(this ISpecimenBuilder fixture, Random random, int regnskabsnummer, int nummer, DateTime? statusdato = null, int? saldoOffset = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            if (random == null)
            {
                throw new ArgumentNullException(nameof(random));
            }

            Mock<IAdressekontoModel> adressekontoModelMock = new Mock<IAdressekontoModel>();
            adressekontoModelMock.Setup(m => m.Regnskabsnummer)
                .Returns(regnskabsnummer);
            adressekontoModelMock.Setup(m => m.Nummer)
                .Returns(nummer);
            adressekontoModelMock.Setup(m => m.Navn)
                .Returns(fixture.Create<string>());
            adressekontoModelMock.Setup(m => m.PrimærTelefon)
                .Returns(random.Next(100) > 50 ? fixture.Create<string>() : null);
            adressekontoModelMock.Setup(m => m.SekundærTelefon)
                .Returns(random.Next(100) > 50 ? fixture.Create<string>() : null);
            adressekontoModelMock.Setup(m => m.StatusDato)
                .Returns(statusdato ?? DateTime.Now.Date);
            adressekontoModelMock.Setup(m => m.Saldo)
                .Returns(Math.Abs(fixture.Create<decimal>()) * (saldoOffset ?? 1));
            return adressekontoModelMock.Object;
        }

        internal static IBogføringslinjeModel BuildBogføringslinjeModel(this ISpecimenBuilder fixture, Random random, int regnskabsnummer, int løbenummer, DateTime dato, string kontonummer, string budgetkontonummer, int? adressekonto)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            if (random == null)
            {
                throw new ArgumentNullException(nameof(random));
            }

            if (string.IsNullOrWhiteSpace(kontonummer))
            {
                throw new ArgumentNullException(nameof(kontonummer));
            }

            decimal debit = random.Next(100) > 50 ? Math.Abs(fixture.Create<decimal>()) : 0M;
            decimal kredit = debit == 0M ? Math.Abs(fixture.Create<decimal>()) : 0M;
            Mock<IBogføringslinjeModel> bogføringslinjeModelMock = new Mock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Setup(m => m.Regnskabsnummer)
                .Returns(regnskabsnummer);
            bogføringslinjeModelMock.Setup(m => m.Løbenummer)
                .Returns(løbenummer);
            bogføringslinjeModelMock.Setup(m => m.Dato)
                .Returns(dato);
            bogføringslinjeModelMock.Setup(m => m.Bilag)
                .Returns(random.Next(100) > 50 ? fixture.Create<string>() : null);
            bogføringslinjeModelMock.Setup(m => m.Kontonummer)
                .Returns(kontonummer);
            bogføringslinjeModelMock.Setup(m => m.Tekst)
                .Returns(fixture.Create<string>());
            bogføringslinjeModelMock.Setup(m => m.Budgetkontonummer)
                .Returns(string.IsNullOrWhiteSpace(budgetkontonummer) ? budgetkontonummer : null);
            bogføringslinjeModelMock.Setup(m => m.Debit)
                .Returns(debit);
            bogføringslinjeModelMock.Setup(m => m.Kredit)
                .Returns(kredit);
            bogføringslinjeModelMock.Setup(m => m.Bogført)
                .Returns(debit - kredit);
            bogføringslinjeModelMock.Setup(m => m.Adressekonto)
                .Returns(adressekonto ?? 0);
            return bogføringslinjeModelMock.Object;
        }

        internal static IKontogruppeModel BuildKontogruppeModel(this ISpecimenBuilder fixture, int nummer, Balancetype? balancetype = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            Mock<IKontogruppeModel> kontogruppeModelMock = new Mock<IKontogruppeModel>();
            kontogruppeModelMock.Setup(m => m.Nummer)
                .Returns(nummer);
            kontogruppeModelMock.Setup(m => m.Tekst)
                .Returns(fixture.Create<string>());
            kontogruppeModelMock.Setup(m => m.Balancetype)
                .Returns(balancetype ?? (nummer % 2 != 0 ? Balancetype.Aktiver : Balancetype.Passiver));
            return kontogruppeModelMock.Object;
        }

        internal static IBudgetkontogruppeModel BuildBudgetkontogruppeModel(this ISpecimenBuilder fixture, int nummer)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            Mock<IBudgetkontogruppeModel> budgetkontogruppeModelMock = new Mock<IBudgetkontogruppeModel>();
            budgetkontogruppeModelMock.Setup(m => m.Nummer)
                .Returns(nummer);
            budgetkontogruppeModelMock.Setup(m => m.Tekst)
                .Returns(fixture.Create<string>());
            return budgetkontogruppeModelMock.Object;
        }
    }
}