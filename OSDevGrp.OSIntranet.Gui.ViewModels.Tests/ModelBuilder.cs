using System;
using System.Collections.Generic;
using AutoFixture;
using AutoFixture.Kernel;
using Moq;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests
{
    internal static class ModelBuilder
    {
        internal static IEnumerable<IRegnskabModel> BuildRegnskabModelCollection(this ISpecimenBuilder fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return new[]
            {
                fixture.BuildRegnskabModel(),
                fixture.BuildRegnskabModel(),
                fixture.BuildRegnskabModel()
            };
        }

        internal static IRegnskabModel BuildRegnskabModel(this ISpecimenBuilder fixture, int? nummer = null, string navn = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return fixture.BuildRegnskabModelMock(nummer, navn).Object;
        }

        internal static Mock<IRegnskabModel> BuildRegnskabModelMock(this ISpecimenBuilder fixture, int? nummer = null, string navn = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            Mock<IRegnskabModel> regnskabModelMock = new Mock<IRegnskabModel>();
            regnskabModelMock.Setup(m => m.Nummer)
                .Returns(nummer ?? fixture.Create<int>());
            regnskabModelMock.Setup(m => m.Navn)
                .Returns(navn ?? fixture.Create<string>());
            return regnskabModelMock;
        }

        internal static IEnumerable<IKontoModel> BuildKontoModelCollection(this ISpecimenBuilder fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return new[]
            {
                fixture.BuildKontoModel(),
                fixture.BuildKontoModel(),
                fixture.BuildKontoModel(),
                fixture.BuildKontoModel(),
                fixture.BuildKontoModel(),
                fixture.BuildKontoModel(),
                fixture.BuildKontoModel()
            };
        }

        internal static IKontoModelBase BuildKontoModelBase(this ISpecimenBuilder fixture, int? regnskabsnummer = null, string kontonummer = null, string kontonavn = null, string beskrivelse = null, string notat = null, int? kontogruppe = null, DateTime? statusDato = null, Action<Mock<IKontoModelBase>> setupCallback = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return fixture.BuildKontoModelBaseMock(regnskabsnummer, kontonummer, kontonavn, beskrivelse, notat, kontogruppe, statusDato, setupCallback).Object;
        }

        internal static Mock<IKontoModelBase> BuildKontoModelBaseMock(this ISpecimenBuilder fixture, int? regnskabsnummer = null, string kontonummer = null, string kontonavn = null, string beskrivelse = null, string notat = null, int? kontogruppe = null, DateTime? statusDato = null, Action<Mock<IKontoModelBase>> setupCallback = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            Mock<IKontoModelBase> mock = new Mock<IKontoModelBase>();
            mock.Setup(m => m.Regnskabsnummer)
                .Returns(regnskabsnummer ?? fixture.Create<int>());
            mock.Setup(m => m.Kontonummer)
                .Returns(kontonummer ?? fixture.Create<string>());
            mock.Setup(m => m.Kontonavn)
                .Returns(kontonavn ?? fixture.Create<string>());
            mock.Setup(m => m.Beskrivelse)
                .Returns(beskrivelse ?? fixture.Create<string>());
            mock.Setup(m => m.Notat)
                .Returns(notat ?? fixture.Create<string>());
            mock.Setup(m => m.Kontogruppe)
                .Returns(kontogruppe ?? fixture.Create<int>());
            mock.Setup(m => m.StatusDato)
                .Returns(statusDato ?? fixture.Create<DateTime>());

            if (setupCallback == null)
            {
                return mock;
            }

            setupCallback(mock);

            return mock;
        }

        internal static IKontoModel BuildKontoModel(this ISpecimenBuilder fixture, int? regnskabsnummer = null, string kontonummer = null, string kontonavn = null, string beskrivelse = null, string notat = null, int? kontogruppe = null, DateTime? statusDato = null, decimal? kredit = null, decimal? saldo = null, decimal? disponibel = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return fixture.BuildKontoModelMock(regnskabsnummer, kontonummer, kontonavn, beskrivelse, notat, kontogruppe, statusDato, kredit, saldo, disponibel).Object;
        }

        private static Mock<IKontoModel> BuildKontoModelMock(this ISpecimenBuilder fixture, int? regnskabsnummer = null, string kontonummer = null, string kontonavn = null, string beskrivelse = null, string notat = null, int? kontogruppe = null, DateTime? statusDato = null, decimal? kredit = null, decimal? saldo = null, decimal? disponibel = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            Mock<IKontoModel> mock = new Mock<IKontoModel>();
            mock.Setup(m => m.Regnskabsnummer)
                .Returns(regnskabsnummer ?? fixture.Create<int>());
            mock.Setup(m => m.Kontonummer)
                .Returns(kontonummer ?? fixture.Create<string>());
            mock.Setup(m => m.Kontonavn)
                .Returns(kontonavn ?? fixture.Create<string>());
            mock.Setup(m => m.Beskrivelse)
                .Returns(beskrivelse ?? fixture.Create<string>());
            mock.Setup(m => m.Notat)
                .Returns(notat ?? fixture.Create<string>());
            mock.Setup(m => m.Kontogruppe)
                .Returns(kontogruppe ?? fixture.Create<int>());
            mock.Setup(m => m.StatusDato)
                .Returns(statusDato ?? fixture.Create<DateTime>());
            mock.Setup(m => m.Kredit)
                .Returns(kredit ?? fixture.Create<decimal>());
            mock.Setup(m => m.Saldo)
                .Returns(saldo ?? fixture.Create<decimal>());
            mock.Setup(m => m.Disponibel)
                .Returns(disponibel ?? fixture.Create<decimal>());
            return mock;
        }

        internal static IEnumerable<IBudgetkontoModel> BuildBudgetkontoModelCollection(this ISpecimenBuilder fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return new[]
            {
                fixture.BuildBudgetkontoModel(),
                fixture.BuildBudgetkontoModel(),
                fixture.BuildBudgetkontoModel(),
                fixture.BuildBudgetkontoModel(),
                fixture.BuildBudgetkontoModel(),
                fixture.BuildBudgetkontoModel(),
                fixture.BuildBudgetkontoModel()
            };
        }

        internal static IBudgetkontoModel BuildBudgetkontoModel(this ISpecimenBuilder fixture, int? regnskabsnummer = null, string kontonummer = null, string kontonavn = null, string beskrivelse = null, string notat = null, int? kontogruppe = null, DateTime? statusDato = null, decimal? indtægter = null, decimal? udgifter = null, decimal? budget = null, decimal? budgetSidsteMåned = null, decimal? budgetÅrTilDato = null, decimal? budgetSidsteÅr = null, decimal? bogført = null, decimal? bogførtSidsteMåned = null, decimal? bogførtÅrTilDato = null, decimal? bogførtSidsteÅr = null, decimal? disponibel = null, Action<Mock<IBudgetkontoModel>> setupCallback = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return fixture.BuildBudgetkontoModelMock(regnskabsnummer, kontonummer, kontonavn, beskrivelse, notat, kontogruppe, statusDato, indtægter, udgifter, budget, budgetSidsteMåned, budgetÅrTilDato, budgetSidsteÅr, bogført, bogførtSidsteMåned, bogførtÅrTilDato, bogførtSidsteÅr, disponibel, setupCallback).Object;
        }

        internal static Mock<IBudgetkontoModel> BuildBudgetkontoModelMock(this ISpecimenBuilder fixture, int? regnskabsnummer = null, string kontonummer = null, string kontonavn = null, string beskrivelse = null, string notat = null, int? kontogruppe = null, DateTime? statusDato = null, decimal? indtægter = null, decimal? udgifter = null, decimal? budget = null, decimal? budgetSidsteMåned = null, decimal? budgetÅrTilDato = null, decimal? budgetSidsteÅr = null, decimal? bogført = null, decimal? bogførtSidsteMåned = null, decimal? bogførtÅrTilDato = null, decimal? bogførtSidsteÅr = null, decimal? disponibel = null, Action<Mock<IBudgetkontoModel>> setupCallback = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            Mock<IBudgetkontoModel> mock = new Mock<IBudgetkontoModel>();
            mock.Setup(m => m.Regnskabsnummer)
                .Returns(regnskabsnummer ?? fixture.Create<int>());
            mock.Setup(m => m.Kontonummer)
                .Returns(kontonummer ?? fixture.Create<string>());
            mock.Setup(m => m.Kontonavn)
                .Returns(kontonavn ?? fixture.Create<string>());
            mock.Setup(m => m.Beskrivelse)
                .Returns(beskrivelse ?? fixture.Create<string>());
            mock.Setup(m => m.Notat)
                .Returns(notat ?? fixture.Create<string>());
            mock.Setup(m => m.Kontogruppe)
                .Returns(kontogruppe ?? fixture.Create<int>());
            mock.Setup(m => m.StatusDato)
                .Returns(statusDato ?? fixture.Create<DateTime>());
            mock.Setup(m => m.Indtægter)
                .Returns(indtægter ?? fixture.Create<decimal>());
            mock.Setup(m => m.Udgifter)
                .Returns(udgifter ?? fixture.Create<decimal>());
            mock.Setup(m => m.Budget)
                .Returns(budget ?? fixture.Create<decimal>());
            mock.Setup(m => m.BudgetSidsteMåned)
                .Returns(budgetSidsteMåned ?? fixture.Create<decimal>());
            mock.Setup(m => m.BudgetÅrTilDato)
                .Returns(budgetÅrTilDato ?? fixture.Create<decimal>());
            mock.Setup(m => m.BudgetSidsteÅr)
                .Returns(budgetSidsteÅr ?? fixture.Create<decimal>());
            mock.Setup(m => m.Bogført)
                .Returns(bogført ?? fixture.Create<decimal>());
            mock.Setup(m => m.BogførtSidsteMåned)
                .Returns(bogførtSidsteMåned ?? fixture.Create<decimal>());
            mock.Setup(m => m.BogførtÅrTilDato)
                .Returns(bogførtÅrTilDato ?? fixture.Create<decimal>());
            mock.Setup(m => m.BogførtSidsteÅr)
                .Returns(bogførtSidsteÅr ?? fixture.Create<decimal>());
            mock.Setup(m => m.Disponibel)
                .Returns(disponibel ?? fixture.Create<decimal>());

            if (setupCallback == null)
            {
                return mock;
            }

            setupCallback(mock);

            return mock;
        }

        internal static IEnumerable<IAdressekontoModel> BuildAdressekontoModelCollection(this ISpecimenBuilder fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return new[]
            {
                fixture.BuildAdressekontoModel(),
                fixture.BuildAdressekontoModel(),
                fixture.BuildAdressekontoModel(),
                fixture.BuildAdressekontoModel(),
                fixture.BuildAdressekontoModel(),
                fixture.BuildAdressekontoModel(),
                fixture.BuildAdressekontoModel()
            };
        }

        internal static IAdressekontoModel BuildAdressekontoModel(this ISpecimenBuilder fixture, int? regnskabsnummer = null, int? nummer = null, string navn = null, string primærTelefon = null, string sekundærTelefon = null, DateTime? statusDato = null, decimal? saldo = null, Nyhedsaktualitet? nyhedsaktualitet = null, DateTime? nyhedsudgivelsestidspunkt = null, string nyhedsinformation = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return fixture.BuildAdressekontoModelMock(regnskabsnummer, nummer, navn, primærTelefon, sekundærTelefon, statusDato, saldo, nyhedsaktualitet, nyhedsudgivelsestidspunkt, nyhedsinformation).Object;
        }

        private static Mock<IAdressekontoModel> BuildAdressekontoModelMock(this ISpecimenBuilder fixture, int? regnskabsnummer = null, int? nummer = null, string navn = null, string primærTelefon = null, string sekundærTelefon = null, DateTime? statusDato = null, decimal? saldo = null, Nyhedsaktualitet? nyhedsaktualitet = null, DateTime? nyhedsudgivelsestidspunkt = null, string nyhedsinformation = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            Mock<IAdressekontoModel> mock = new Mock<IAdressekontoModel>();
            mock.Setup(m => m.Regnskabsnummer)
                .Returns(regnskabsnummer ?? fixture.Create<int>());
            mock.Setup(m => m.Nummer)
                .Returns(nummer ?? fixture.Create<int>());
            mock.Setup(m => m.Navn)
                .Returns(navn ?? fixture.Create<string>());
            mock.Setup(m => m.PrimærTelefon)
                .Returns(primærTelefon ?? fixture.Create<string>());
            mock.Setup(m => m.SekundærTelefon)
                .Returns(sekundærTelefon ?? fixture.Create<string>());
            mock.Setup(m => m.StatusDato)
                .Returns(statusDato ?? fixture.Create<DateTime>());
            mock.Setup(m => m.Saldo)
                .Returns(saldo ?? fixture.Create<decimal>());
            mock.Setup(m => m.Nyhedsaktualitet)
                .Returns(nyhedsaktualitet ?? fixture.Create<Nyhedsaktualitet>());
            mock.Setup(m => m.Nyhedsudgivelsestidspunkt)
                .Returns(nyhedsudgivelsestidspunkt ?? fixture.Create<DateTime>());
            mock.Setup(m => m.Nyhedsinformation)
                .Returns(nyhedsinformation ?? fixture.Create<string>());
            return mock;
        }

        internal static IEnumerable<IBogføringslinjeModel> BuildBogføringslinjeModelCollection(this ISpecimenBuilder fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return new[]
            {
                fixture.BuildBogføringslinjeModel(),
                fixture.BuildBogføringslinjeModel(),
                fixture.BuildBogføringslinjeModel(),
                fixture.BuildBogføringslinjeModel(),
                fixture.BuildBogføringslinjeModel(),
                fixture.BuildBogføringslinjeModel(),
                fixture.BuildBogføringslinjeModel(),
                fixture.BuildBogføringslinjeModel(),
                fixture.BuildBogføringslinjeModel(),
                fixture.BuildBogføringslinjeModel(),
                fixture.BuildBogføringslinjeModel(),
                fixture.BuildBogføringslinjeModel(),
                fixture.BuildBogføringslinjeModel(),
                fixture.BuildBogføringslinjeModel(),
                fixture.BuildBogføringslinjeModel()
            };
        }

        internal static IBogføringslinjeModel BuildBogføringslinjeModel(this ISpecimenBuilder fixture, int? regnskabsnummer = null, int? løbenummer = null, DateTime? dato = null, bool hasBilag = true, string bilag = null, string kontonummer = null, string tekst = null, bool hasBudgetkontonummer = true, string budgetkontonummer = null, decimal? debit = null, decimal? kredit = null, bool hasAdressekonto = true, int? adressekonto = null, Nyhedsaktualitet? nyhedsaktualitet = null, DateTime? nyhedsudgivelsestidspunkt = null, string nyhedsinformation = null, Action<Mock<IBogføringslinjeModel>> setupCallback = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return fixture.BuildBogføringslinjeModelMock(regnskabsnummer, løbenummer, dato, hasBilag, bilag, kontonummer, tekst, hasBudgetkontonummer, budgetkontonummer, debit, kredit, hasAdressekonto, adressekonto, nyhedsaktualitet, nyhedsudgivelsestidspunkt, nyhedsinformation, setupCallback).Object;
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
                .Returns(hasBudgetkontonummer ? budgetkontonummer ?? fixture.Create<string>() : null);
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

            if (setupCallback == null)
            {
                return mock;
            }

            setupCallback(mock);

            return mock;
        }

        internal static IEnumerable<IBogføringsresultatModel> BuildBogføringsresultatModelCollection(this ISpecimenBuilder fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return new[]
            {
                fixture.BuildBogføringsresultatModel(),
                fixture.BuildBogføringsresultatModel(),
                fixture.BuildBogføringsresultatModel()
            };
        }

        private static IBogføringsresultatModel BuildBogføringsresultatModel(this ISpecimenBuilder fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return fixture.BuildBogføringsresultatModelMock().Object;
        }

        private static Mock<IBogføringsresultatModel> BuildBogføringsresultatModelMock(this ISpecimenBuilder fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return new Mock<IBogføringsresultatModel>();
        }

        internal static IBogføringsadvarselModel BuildBogføringsadvarselModel(this ISpecimenBuilder fixture, string advarsel = null, string kontonummer = null, string kontonavn = null, decimal? beløb = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return fixture.BuildBogføringsadvarselModelMock(advarsel, kontonummer, kontonavn, beløb).Object;
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

        internal static IEnumerable<IKontogruppeModel> BuildKontogruppeModelCollection(this ISpecimenBuilder fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return new[]
            {
                fixture.BuildKontogruppeModel(),
                fixture.BuildKontogruppeModel(),
                fixture.BuildKontogruppeModel(),
                fixture.BuildKontogruppeModel(),
                fixture.BuildKontogruppeModel(),
                fixture.BuildKontogruppeModel(),
                fixture.BuildKontogruppeModel()
            };
        }

        private static IKontogruppeModel BuildKontogruppeModel(this ISpecimenBuilder fixture, int? nummer = null, string tekst = null, Balancetype? balancetype = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return fixture.BuildKontogruppeModelMock(nummer, tekst, balancetype).Object;
        }

        private static Mock<IKontogruppeModel> BuildKontogruppeModelMock(this ISpecimenBuilder fixture, int? nummer = null, string tekst = null, Balancetype? balancetype = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            Mock<IKontogruppeModel> mock = new Mock<IKontogruppeModel>();
            mock.Setup(m => m.Nummer)
                .Returns(nummer ?? fixture.Create<int>());
            mock.Setup(m => m.Tekst)
                .Returns(tekst ?? fixture.Create<string>());
            mock.Setup(m => m.Balancetype)
                .Returns(balancetype ?? fixture.Create<Balancetype>());
            return mock;
        }

        internal static IEnumerable<IBudgetkontogruppeModel> BuildBudgetkontogruppeModelCollection(this ISpecimenBuilder fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return new[]
            {
                fixture.BuildBudgetkontogruppeModel(),
                fixture.BuildBudgetkontogruppeModel(),
                fixture.BuildBudgetkontogruppeModel(),
                fixture.BuildBudgetkontogruppeModel(),
                fixture.BuildBudgetkontogruppeModel(),
                fixture.BuildBudgetkontogruppeModel(),
                fixture.BuildBudgetkontogruppeModel()
            };
        }

        private static IBudgetkontogruppeModel BuildBudgetkontogruppeModel(this ISpecimenBuilder fixture, int? nummer = null, string tekst = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return fixture.BuildBudgetkontogruppeModelMock(nummer, tekst).Object;
        }

        private static Mock<IBudgetkontogruppeModel> BuildBudgetkontogruppeModelMock(this ISpecimenBuilder fixture, int? nummer = null, string tekst = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            Mock<IBudgetkontogruppeModel> mock = new Mock<IBudgetkontogruppeModel>();
            mock.Setup(m => m.Nummer)
                .Returns(nummer ?? fixture.Create<int>());
            mock.Setup(m => m.Tekst)
                .Returns(tekst ?? fixture.Create<string>());
            return mock;
        }

        internal static INyhedModel BuildNyhedModel(this ISpecimenBuilder fixture, Nyhedsaktualitet? nyhedsaktualitet = null, DateTime? nyhedsudgivelsestidspunkt = null, string nyhedsinformation = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return fixture.BuildNyhedModelMock(nyhedsaktualitet, nyhedsudgivelsestidspunkt, nyhedsinformation).Object;
        }

        internal static Mock<INyhedModel> BuildNyhedModelMock(this ISpecimenBuilder fixture, Nyhedsaktualitet? nyhedsaktualitet = null, DateTime? nyhedsudgivelsestidspunkt = null, string nyhedsinformation = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            Random random = new Random(fixture.Create<int>());

            Mock<INyhedModel> mock = new Mock<INyhedModel>();
            mock.Setup(m => m.Nyhedsaktualitet)
                .Returns(nyhedsaktualitet ?? fixture.Create<Nyhedsaktualitet>());
            mock.Setup(m => m.Nyhedsudgivelsestidspunkt)
                .Returns(nyhedsudgivelsestidspunkt ?? DateTime.Now.AddDays(random.Next(0, 7) * -1).AddMinutes(random.Next(0, 120) * -1));
            mock.Setup(m => m.Nyhedsinformation)
                .Returns(nyhedsinformation ?? fixture.Create<string>());
            return mock;
        }

        internal static ITabelModelBase BuildTabelModel(this ISpecimenBuilder fixture, string id = null, string tekst = null, Action<Mock<ITabelModelBase>> setupCallback = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return fixture.BuildTabelModelMock(id, tekst, setupCallback).Object;
        }

        internal static Mock<ITabelModelBase> BuildTabelModelMock(this ISpecimenBuilder fixture, string id = null, string tekst = null, Action<Mock<ITabelModelBase>> setupCallback = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            Mock<ITabelModelBase> mock = new Mock<ITabelModelBase>();
            mock.Setup(m => m.Id)
                .Returns(id ?? fixture.Create<string>());
            mock.Setup(m => m.Tekst)
                .Returns(tekst ?? fixture.Create<string>());

            if (setupCallback == null)
            {
                return mock;
            }

            setupCallback(mock);

            return mock;
        }

        internal static IModel BuildModel(this ISpecimenBuilder fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return fixture.BuildModelMock().Object;
        }

        private static Mock<IModel> BuildModelMock(this ISpecimenBuilder fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return new Mock<IModel>();
        }
    }
}