using System;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using Moq;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests
{
    internal static class ViewModelBuilder
    {
        internal static IRegnskabViewModel BuildRegnskabViewModel(this ISpecimenBuilder fixture, int? nummer = null, string navn = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return fixture.BuildRegnskabViewModelMock(nummer, navn).Object;
        }

        internal static Mock<IRegnskabViewModel> BuildRegnskabViewModelMock(this ISpecimenBuilder fixture, int? nummer = null, string navn = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            Mock<IRegnskabViewModel> regnskabViewModelMock = new Mock<IRegnskabViewModel>();
            regnskabViewModelMock.Setup(m => m.Nummer)
                .Returns(nummer ?? fixture.Create<int>());
            regnskabViewModelMock.Setup(m => m.Navn)
                .Returns(navn ?? fixture.Create<string>());
            return regnskabViewModelMock;
        }

        internal static IKontoViewModel BuildKontoViewModel(this ISpecimenBuilder fixture, IRegnskabViewModel regnskabViewModel = null, string kontonummer = null, string kontonavn = null, bool hasBeskrivelse = true, string beskrivelse = null, bool hasNotat = true, string notat = null, IKontogruppeViewModel kontogruppeViewModel = null, DateTime? statusDato = null, decimal? kontoværdi = null, decimal? kredit = null, decimal? saldo = null, decimal? disponibel = null, bool erRegistreret = true, byte[] image = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return fixture.BuildKontoViewModelMock(regnskabViewModel, kontonummer, kontonavn, hasBeskrivelse, beskrivelse, hasNotat, notat, kontogruppeViewModel, statusDato, kontoværdi, kredit, saldo, disponibel, erRegistreret, image).Object;
        }

        internal static Mock<IKontoViewModel> BuildKontoViewModelMock(this ISpecimenBuilder fixture, IRegnskabViewModel regnskabViewModel = null, string kontonummer = null, string kontonavn = null, bool hasBeskrivelse = true, string beskrivelse = null, bool hasNotat = true, string notat = null, IKontogruppeViewModel kontogruppeViewModel = null, DateTime? statusDato = null, decimal? kontoværdi = null, decimal? kredit = null, decimal? saldo = null, decimal? disponibel = null , bool erRegistreret = true, byte[] image = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            Random random = new Random(fixture.Create<int>());

            Mock<IKontoViewModel> kontoViewModelMock = new Mock<IKontoViewModel>();
            kontoViewModelMock.Setup(m => m.Regnskab)
                .Returns(regnskabViewModel ?? fixture.BuildRegnskabViewModel());
            kontoViewModelMock.Setup(m => m.Kontonummer)
                .Returns(kontonummer ?? fixture.Create<string>());
            kontoViewModelMock.Setup(m => m.Kontonavn)
                .Returns(kontonavn ?? fixture.Create<string>());
            kontoViewModelMock.Setup(m => m.DisplayName)
                .Returns(kontonavn ?? fixture.Create<string>());
            kontoViewModelMock.Setup(m => m.Beskrivelse)
                .Returns(hasBeskrivelse ? beskrivelse ?? fixture.Create<string>() : null);
            kontoViewModelMock.Setup(m => m.Notat)
                .Returns(hasNotat ? notat ?? fixture.Create<string>() : null);
            kontoViewModelMock.Setup(m => m.Kontogruppe)
                .Returns(kontogruppeViewModel ?? fixture.BuildKontogruppeViewModel());
            kontoViewModelMock.Setup(m => m.StatusDato)
                .Returns(statusDato ?? DateTime.Today);
            kontoViewModelMock.Setup(m => m.Kontoværdi)
                .Returns(kontoværdi ?? fixture.Create<decimal>());
            kontoViewModelMock.Setup(m => m.Kredit)
                .Returns(kredit ?? fixture.Create<decimal>());
            kontoViewModelMock.Setup(m => m.Saldo)
                .Returns(saldo ?? fixture.Create<decimal>());
            kontoViewModelMock.Setup(m => m.Disponibel)
                .Returns(disponibel ?? fixture.Create<decimal>());
            kontoViewModelMock.Setup(m => m.ErRegistreret)
                .Returns(erRegistreret);
            kontoViewModelMock.Setup(m => m.Image)
                .Returns(image ?? fixture.CreateMany<byte>(random.Next(256, 512)).ToArray());
            return kontoViewModelMock;
        }

        internal static IBudgetkontoViewModel BuildBudgetkontoViewModel(this ISpecimenBuilder fixture, IRegnskabViewModel regnskabViewModel = null, string kontonummer = null, string kontonavn = null, bool hasBeskrivelse = true, string beskrivelse = null, bool hasNotat = true, string notat = null, IBudgetkontogruppeViewModel budgetkontogruppeViewModel = null, DateTime? statusDato = null, decimal? kontoværdi = null, decimal? indtægter = null, decimal? udgifter = null, decimal? budget = null, decimal? budgetSidsteMåned = null, decimal? budgetÅrTilDato = null, decimal? budgetSidsteÅr = null, decimal? bogført = null, decimal? bogførtSidsteMåned = null, decimal? bogførtÅrTilDato = null, decimal? bogførtSidsteÅr = null, decimal? disponibel = null, bool erRegistreret = true, byte[] image = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return fixture.BuildBudgetkontoViewModelMock(regnskabViewModel, kontonummer, kontonavn, hasBeskrivelse, beskrivelse, hasNotat, notat, budgetkontogruppeViewModel, statusDato, kontoværdi, indtægter, udgifter, budget, budgetSidsteMåned, budgetÅrTilDato, budgetSidsteÅr, bogført, bogførtSidsteMåned, bogførtÅrTilDato, bogførtSidsteÅr, disponibel, erRegistreret, image).Object;
        }

        internal static Mock<IBudgetkontoViewModel> BuildBudgetkontoViewModelMock(this ISpecimenBuilder fixture, IRegnskabViewModel regnskabViewModel = null, string kontonummer = null, string kontonavn = null, bool hasBeskrivelse = true, string beskrivelse = null, bool hasNotat = true, string notat = null, IBudgetkontogruppeViewModel budgetkontogruppeViewModel = null, DateTime? statusDato = null, decimal? kontoværdi = null, decimal? indtægter = null, decimal? udgifter = null, decimal? budget = null, decimal? budgetSidsteMåned = null, decimal? budgetÅrTilDato = null, decimal? budgetSidsteÅr = null, decimal? bogført = null, decimal? bogførtSidsteMåned = null, decimal? bogførtÅrTilDato = null, decimal? bogførtSidsteÅr = null, decimal? disponibel = null, bool erRegistreret = true, byte[] image = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            Random random = new Random(fixture.Create<int>());

            Mock<IBudgetkontoViewModel> budgetkontoViewModelMock = new Mock<IBudgetkontoViewModel>();
            budgetkontoViewModelMock.Setup(m => m.Regnskab)
                .Returns(regnskabViewModel ?? fixture.BuildRegnskabViewModel());
            budgetkontoViewModelMock.Setup(m => m.Kontonummer)
                .Returns(kontonummer ?? fixture.Create<string>());
            budgetkontoViewModelMock.Setup(m => m.Kontonavn)
                .Returns(kontonavn ?? fixture.Create<string>());
            budgetkontoViewModelMock.Setup(m => m.DisplayName)
                .Returns(kontonavn ?? fixture.Create<string>());
            budgetkontoViewModelMock.Setup(m => m.Beskrivelse)
                .Returns(hasBeskrivelse ? beskrivelse ?? fixture.Create<string>() : null);
            budgetkontoViewModelMock.Setup(m => m.Notat)
                .Returns(hasNotat ? notat ?? fixture.Create<string>() : null);
            budgetkontoViewModelMock.Setup(m => m.Kontogruppe)
                .Returns(budgetkontogruppeViewModel ?? fixture.BuildBudgetkontogruppeViewModel());
            budgetkontoViewModelMock.Setup(m => m.StatusDato)
                .Returns(statusDato ?? DateTime.Today);
            budgetkontoViewModelMock.Setup(m => m.Kontoværdi)
                .Returns(kontoværdi ?? fixture.Create<decimal>());
            budgetkontoViewModelMock.Setup(m => m.Indtægter)
                .Returns(indtægter ?? fixture.Create<decimal>());
            budgetkontoViewModelMock.Setup(m => m.Udgifter)
                .Returns(udgifter ?? fixture.Create<decimal>());
            budgetkontoViewModelMock.Setup(m => m.Budget)
                .Returns(budget ?? fixture.Create<decimal>());
            budgetkontoViewModelMock.Setup(m => m.BudgetSidsteMåned)
                .Returns(budgetSidsteMåned ?? fixture.Create<decimal>());
            budgetkontoViewModelMock.Setup(m => m.BudgetÅrTilDato)
                .Returns(budgetÅrTilDato ?? fixture.Create<decimal>());
            budgetkontoViewModelMock.Setup(m => m.BudgetSidsteÅr)
                .Returns(budgetSidsteÅr ?? fixture.Create<decimal>());
            budgetkontoViewModelMock.Setup(m => m.Bogført)
                .Returns(bogført ?? fixture.Create<decimal>());
            budgetkontoViewModelMock.Setup(m => m.BogførtSidsteMåned)
                .Returns(bogførtSidsteMåned ?? fixture.Create<decimal>());
            budgetkontoViewModelMock.Setup(m => m.BogførtÅrTilDato)
                .Returns(bogførtÅrTilDato ?? fixture.Create<decimal>());
            budgetkontoViewModelMock.Setup(m => m.BogførtSidsteÅr)
                .Returns(bogførtSidsteÅr ?? fixture.Create<decimal>());
            budgetkontoViewModelMock.Setup(m => m.Disponibel)
                .Returns(disponibel ?? fixture.Create<decimal>());
            budgetkontoViewModelMock.Setup(m => m.ErRegistreret)
                .Returns(erRegistreret);
            budgetkontoViewModelMock.Setup(m => m.Image)
                .Returns(image ?? fixture.CreateMany<byte>(random.Next(256, 512)).ToArray());
            return budgetkontoViewModelMock;
        }

        internal static IAdressekontoViewModel BuildAdressekontoViewModel(this ISpecimenBuilder fixture, IRegnskabViewModel regnskabViewModel = null, int? nummer = null, string navn = null, bool hasPrimærTelefon = true, string primærTelefon = null, bool hasSekundærTelefon = true, string sekundærTelefon = null, DateTime? statusDato = null, decimal? saldo = null, byte[] image = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return fixture.BuildAdressekontoViewModelMock(regnskabViewModel, nummer, navn, hasPrimærTelefon, primærTelefon, hasSekundærTelefon, sekundærTelefon, statusDato, saldo, image).Object;
        }

        internal static Mock<IAdressekontoViewModel> BuildAdressekontoViewModelMock(this ISpecimenBuilder fixture, IRegnskabViewModel regnskabViewModel = null, int? nummer = null, string navn = null, bool hasPrimærTelefon = true, string primærTelefon = null, bool hasSekundærTelefon = true, string sekundærTelefon = null, DateTime? statusDato = null, decimal? saldo = null, byte[] image = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            Random random = new Random(fixture.Create<int>());

            Mock<IAdressekontoViewModel> adressekontoViewModelMock = new Mock<IAdressekontoViewModel>();
            adressekontoViewModelMock.Setup(m => m.Regnskab)
                .Returns(regnskabViewModel ?? fixture.BuildRegnskabViewModel());
            adressekontoViewModelMock.Setup(m => m.Nummer)
                .Returns(nummer ?? fixture.Create<int>());
            adressekontoViewModelMock.Setup(m => m.Navn)
                .Returns(navn ?? fixture.Create<string>());
            adressekontoViewModelMock.Setup(m => m.DisplayName)
                .Returns(navn ?? fixture.Create<string>());
            adressekontoViewModelMock.Setup(m => m.PrimærTelefon)
                .Returns(hasPrimærTelefon ? primærTelefon ?? fixture.Create<string>() : null);
            adressekontoViewModelMock.Setup(m => m.SekundærTelefon)
                .Returns(hasSekundærTelefon ? sekundærTelefon ?? fixture.Create<string>() : null);
            adressekontoViewModelMock.Setup(m => m.StatusDato)
                .Returns(statusDato ?? DateTime.Today);
            adressekontoViewModelMock.Setup(m => m.Saldo)
                .Returns(saldo ?? fixture.Create<decimal>());
            adressekontoViewModelMock.Setup(m => m.Image)
                .Returns(image ?? fixture.CreateMany<byte>(random.Next(256, 512)).ToArray());
            return adressekontoViewModelMock;
        }

        internal static IBogføringViewModel BuildBogføringViewModel(this ISpecimenBuilder fixture, IRegnskabViewModel regnskabViewModel = null, DateTime? dato = null, bool hasBilag = true, string bilag = null, string kontonummer = null, string kontonavn = null, decimal? kontoSaldo = null, decimal? kontoDisponibel = null, string tekst = null, bool hasBudgetkontonummer = true, string budgetkontonummer = null, string budgetkontonavn = null, decimal? budgetkontoBogført = null, decimal? budgetkontoDisponibel = null, decimal? debit = null, decimal? kredit = null, bool hasAdressekonto = true, int? adressekonto = null, string adressekontoNavn = null, decimal? adressekontoSaldo = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return fixture.BuildBogføringViewModelMock(regnskabViewModel, dato, hasBilag, bilag, kontonummer, kontonavn, kontoSaldo, kontoDisponibel, tekst, hasBudgetkontonummer, budgetkontonummer, budgetkontonavn, budgetkontoBogført, budgetkontoDisponibel, debit, kredit, hasAdressekonto, adressekonto, adressekontoNavn, adressekontoSaldo).Object;
        }

        private static Mock<IBogføringViewModel> BuildBogføringViewModelMock(this ISpecimenBuilder fixture, IRegnskabViewModel regnskabViewModel = null, DateTime? dato = null, bool hasBilag = true, string bilag = null, string kontonummer = null, string kontonavn = null, decimal? kontoSaldo = null, decimal? kontoDisponibel = null, string tekst = null, bool hasBudgetkontonummer = true, string budgetkontonummer = null, string budgetkontonavn = null, decimal? budgetkontoBogført = null, decimal? budgetkontoDisponibel = null, decimal? debit = null, decimal? kredit = null, bool hasAdressekonto = true, int? adressekonto = null, string adressekontoNavn = null, decimal? adressekontoSaldo = null)
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

            Mock<IBogføringViewModel> mock = new Mock<IBogføringViewModel>();
            mock.Setup(m => m.Regnskab)
                .Returns(regnskabViewModel ?? fixture.BuildRegnskabViewModel());
            mock.Setup(m => m.Dato)
                .Returns((dato ?? DateTime.Today.AddDays(random.Next(0, 365) * -1)).Date);
            mock.Setup(m => m.Bilag)
                .Returns(hasBilag ? bilag ?? fixture.Create<string>() : null);
            mock.Setup(m => m.Kontonummer)
                .Returns(kontonummer ?? fixture.Create<string>());
            mock.Setup(m => m.Kontonavn)
                .Returns(kontonavn ?? fixture.Create<string>());
            mock.Setup(m => m.KontoSaldo)
                .Returns(kontoSaldo ?? fixture.Create<decimal>());
            mock.Setup(m => m.KontoDisponibel)
                .Returns(kontoDisponibel ?? fixture.Create<decimal>());
            mock.Setup(m => m.Tekst)
                .Returns(tekst ?? fixture.Create<string>());
            mock.Setup(m => m.Budgetkontonummer)
                .Returns(hasBudgetkontonummer ? budgetkontonummer ?? fixture.Create<string>() : null);
            mock.Setup(m => m.Budgetkontonavn)
                .Returns(hasBudgetkontonummer ? budgetkontonavn ?? fixture.Create<string>() : null);
            mock.Setup(m => m.BudgetkontoBogført)
                .Returns(hasBudgetkontonummer ? budgetkontoBogført ?? fixture.Create<decimal>() : 0M);
            mock.Setup(m => m.BudgetkontoDisponibel)
                .Returns(hasBudgetkontonummer ? budgetkontoDisponibel ?? fixture.Create<decimal>() : 0M);
            mock.Setup(m => m.Debit)
                .Returns(debit ?? 0M);
            mock.Setup(m => m.Kredit)
                .Returns(kredit ?? 0M);
            mock.Setup(m => m.Adressekonto)
                .Returns(hasAdressekonto ? adressekonto ?? fixture.Create<int>() : 0);
            mock.Setup(m => m.AdressekontoNavn)
                .Returns(hasAdressekonto ? adressekontoNavn ?? fixture.Create<string>() : null);
            mock.Setup(m => m.AdressekontoSaldo)
                .Returns(hasAdressekonto ? adressekontoSaldo ?? fixture.Create<decimal>() : 0M);
            return mock;
        }

        internal static IReadOnlyBogføringslinjeViewModel BuildReadOnlyBogføringslinjeViewModel(this ISpecimenBuilder fixture, IRegnskabViewModel regnskabViewModel = null, int? løbenummer = null, DateTime? dato = null, bool hasBilag = true, string bilag = null, string kontonummer = null, string tekst = null, bool hasBudgetkontonummer = true, string budgetkontonummer = null, decimal? debit = null, decimal? kredit = null, bool hasAdressekonto = true, int? adressekonto = null, string displayName = null, byte[] image = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return fixture.BuildReadOnlyBogføringslinjeViewModelMock(regnskabViewModel, løbenummer, dato, hasBilag, bilag, kontonummer, tekst, hasBudgetkontonummer, budgetkontonummer, debit, kredit, hasAdressekonto, adressekonto, displayName, image).Object;
        }

        internal static Mock<IReadOnlyBogføringslinjeViewModel> BuildReadOnlyBogføringslinjeViewModelMock(this ISpecimenBuilder fixture, IRegnskabViewModel regnskabViewModel = null, int? løbenummer = null, DateTime? dato = null, bool hasBilag = true, string bilag = null, string kontonummer = null, string tekst = null, bool hasBudgetkontonummer = true, string budgetkontonummer = null, decimal? debit = null, decimal? kredit = null, bool hasAdressekonto = true, int? adressekonto = null, string displayName = null, byte[] image = null)
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

            Mock<IReadOnlyBogføringslinjeViewModel> mock = new Mock<IReadOnlyBogføringslinjeViewModel>();
            mock.Setup(m => m.Regnskab)
                .Returns(regnskabViewModel ?? fixture.BuildRegnskabViewModel());
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
            mock.Setup(m => m.DisplayName)
                .Returns(displayName ?? fixture.Create<string>());
            mock.Setup(m => m.Image)
                .Returns(image ?? fixture.CreateMany<byte>(random.Next(256, 512)).ToArray());
            return mock;
        }

        internal static IBogføringsadvarselViewModel BuildBogføringsadvarselViewModel(this ISpecimenBuilder fixture, IRegnskabViewModel regnskabViewModel = null, IReadOnlyBogføringslinjeViewModel bogføringslinjeViewModel = null, DateTime? tidspunkt = null, string advarsel = null, string kontonummer = null, string kontonavn = null, decimal? beløb = null, string information = null, byte[] image = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return fixture.BuildBogføringsadvarselViewModelMock(regnskabViewModel, bogføringslinjeViewModel, tidspunkt, advarsel, kontonummer, kontonavn, beløb, information, image).Object;
        }

        internal static Mock<IBogføringsadvarselViewModel> BuildBogføringsadvarselViewModelMock(this ISpecimenBuilder fixture, IRegnskabViewModel regnskabViewModel = null, IReadOnlyBogføringslinjeViewModel bogføringslinjeViewModel = null, DateTime? tidspunkt = null, string advarsel = null, string kontonummer = null, string kontonavn = null, decimal? beløb = null, string information = null, byte[] image = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            Random random = new Random(fixture.Create<int>());

            Mock<IBogføringsadvarselViewModel> mock = new Mock<IBogføringsadvarselViewModel>();
            mock.Setup(m => m.Regnskab)
                .Returns(regnskabViewModel ?? fixture.BuildRegnskabViewModel());
            mock.Setup(m => m.Bogføringslinje)
                .Returns(bogføringslinjeViewModel ?? fixture.BuildReadOnlyBogføringslinjeViewModel());
            mock.Setup(m => m.Tidspunkt)
                .Returns(tidspunkt ?? DateTime.Now.AddDays(random.Next(0, 7) * -1).AddMinutes(random.Next(0, 60) * -1));
            mock.Setup(m => m.Advarsel)
                .Returns(advarsel ?? fixture.Create<string>());
            mock.Setup(m => m.Kontonummer)
                .Returns(kontonummer ?? fixture.Create<string>());
            mock.Setup(m => m.Kontonavn)
                .Returns(kontonavn ?? fixture.Create<string>());
            mock.Setup(m => m.Beløb)
                .Returns(beløb ?? fixture.Create<decimal>());
            mock.Setup(m => m.Information)
                .Returns(information ?? fixture.Create<string>());
            mock.Setup(m => m.Image)
                .Returns(image ?? fixture.CreateMany<byte>(random.Next(256, 512)).ToArray());
            return mock;
        }

        internal static IKontogruppeViewModelBase BuildKontogruppeViewModelBase(this ISpecimenBuilder fixture, int? nummer = null, string tekst = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return fixture.BuildKontogruppeViewModelBaseMock(nummer, tekst).Object;
        }

        private static Mock<IKontogruppeViewModelBase> BuildKontogruppeViewModelBaseMock(this ISpecimenBuilder fixture, int? nummer = null, string tekst = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            nummer = nummer ?? fixture.Create<int>();
            tekst = tekst ?? fixture.Create<string>();

            Mock<IKontogruppeViewModelBase> kontogruppeViewModelBaseMock = new Mock<IKontogruppeViewModelBase>();
            kontogruppeViewModelBaseMock.Setup(m => m.Id)
                .Returns(nummer.Value.ToString());
            kontogruppeViewModelBaseMock.Setup(m => m.Nummer)
                .Returns(nummer.Value);
            kontogruppeViewModelBaseMock.Setup(m => m.Tekst)
                .Returns(tekst);
            kontogruppeViewModelBaseMock.Setup(m => m.DisplayName)
                .Returns(tekst);
            return kontogruppeViewModelBaseMock;
        }

        internal static IKontogruppeViewModel BuildKontogruppeViewModel(this ISpecimenBuilder fixture, int? nummer = null, string tekst = null, Balancetype? balanceType = null, IBalanceViewModel balanceViewModel = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return fixture.BuildKontogruppeViewModelMock(nummer, tekst, balanceType, balanceViewModel).Object;
        }

        internal static Mock<IKontogruppeViewModel> BuildKontogruppeViewModelMock(this ISpecimenBuilder fixture, int? nummer = null, string tekst = null, Balancetype? balanceType = null, IBalanceViewModel balanceViewModel = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            nummer = nummer ?? fixture.Create<int>();
            tekst = tekst ?? fixture.Create<string>();

            Mock<IKontogruppeViewModel> kontogruppeViewModelMock = new Mock<IKontogruppeViewModel>();
            kontogruppeViewModelMock.Setup(m => m.Id)
                .Returns(nummer.Value.ToString());
            kontogruppeViewModelMock.Setup(m => m.Nummer)
                .Returns(nummer.Value);
            kontogruppeViewModelMock.Setup(m => m.Tekst)
                .Returns(tekst);
            kontogruppeViewModelMock.Setup(m => m.DisplayName)
                .Returns(tekst);
            kontogruppeViewModelMock.Setup(m => m.Balancetype)
                .Returns(balanceType ?? fixture.Create<Balancetype>());
            kontogruppeViewModelMock.Setup(m => m.CreateBalancelinje(It.IsAny<IRegnskabViewModel>()))
                .Returns(balanceViewModel ?? fixture.BuildBalanceViewModel());
            return kontogruppeViewModelMock;
        }

        internal static IBalanceViewModel BuildBalanceViewModel(this ISpecimenBuilder fixture, int? nummer = null, string tekst = null, Balancetype? balanceType = null, decimal? kredit = null, decimal? saldo = null, decimal? disponibel = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return fixture.BuildBalanceViewModelMock(nummer, tekst, balanceType, kredit, saldo, disponibel).Object;
        }

        internal static Mock<IBalanceViewModel> BuildBalanceViewModelMock(this ISpecimenBuilder fixture, int? nummer = null, string tekst = null, Balancetype? balanceType = null, decimal? kredit = null, decimal? saldo = null, decimal? disponibel = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            nummer = nummer ?? fixture.Create<int>();
            tekst = tekst ?? fixture.Create<string>();

            Mock<IBalanceViewModel> balanceViewModelMock = new Mock<IBalanceViewModel>();
            balanceViewModelMock.Setup(m => m.Id)
                .Returns(nummer.Value.ToString());
            balanceViewModelMock.Setup(m => m.Nummer)
                .Returns(nummer.Value);
            balanceViewModelMock.Setup(m => m.Tekst)
                .Returns(tekst);
            balanceViewModelMock.Setup(m => m.DisplayName)
                .Returns(tekst);
            balanceViewModelMock.Setup(m => m.Balancetype)
                .Returns(balanceType ?? fixture.Create<Balancetype>());
            balanceViewModelMock.Setup(m => m.Kredit)
                .Returns(kredit ?? fixture.Create<decimal>());
            balanceViewModelMock.Setup(m => m.Saldo)
                .Returns(saldo ?? fixture.Create<decimal>());
            balanceViewModelMock.Setup(m => m.Disponibel)
                .Returns(disponibel ?? fixture.Create<decimal>());
            balanceViewModelMock.Setup(m => m.CreateBalancelinje(It.IsAny<IRegnskabViewModel>()))
                .Throws<NotSupportedException>();
            return balanceViewModelMock;
        }

        internal static IBudgetkontogruppeViewModel BuildBudgetkontogruppeViewModel(this ISpecimenBuilder fixture, int? nummer = null, string tekst = null, IOpgørelseViewModel opgørelseViewModel = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return fixture.BuildBudgetkontogruppeViewModelMock(nummer, tekst, opgørelseViewModel).Object;
        }

        internal static Mock<IBudgetkontogruppeViewModel> BuildBudgetkontogruppeViewModelMock(this ISpecimenBuilder fixture, int? nummer = null, string tekst = null, IOpgørelseViewModel opgørelseViewModel = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            nummer = nummer ?? fixture.Create<int>();
            tekst = tekst ?? fixture.Create<string>();

            Mock<IBudgetkontogruppeViewModel> budgetkontogruppeViewModelMock = new Mock<IBudgetkontogruppeViewModel>();
            budgetkontogruppeViewModelMock.Setup(m => m.Id)
                .Returns(nummer.Value.ToString());
            budgetkontogruppeViewModelMock.Setup(m => m.Nummer)
                .Returns(nummer.Value);
            budgetkontogruppeViewModelMock.Setup(m => m.Tekst)
                .Returns(tekst);
            budgetkontogruppeViewModelMock.Setup(m => m.DisplayName)
                .Returns(tekst);
            budgetkontogruppeViewModelMock.Setup(m => m.CreateOpgørelseslinje(It.IsAny<IRegnskabViewModel>()))
                .Returns(opgørelseViewModel ?? fixture.BuildOpgørelseViewModel());
            return budgetkontogruppeViewModelMock;
        }

        internal static IOpgørelseViewModel BuildOpgørelseViewModel(this ISpecimenBuilder fixture, int? nummer = null, string tekst = null, decimal? budget = null, decimal? budgetSidsteMåned = null, decimal? budgetÅrTilDato = null, decimal? budgetSidsteÅr = null, decimal? bogført = null, decimal? bogførtSidsteMåned = null, decimal? bogførtÅrTilDato = null, decimal? bogførtSidsteÅr = null, decimal? disponibel = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return fixture.BuildOpgørelseViewModelMock(nummer, tekst, budget, budgetSidsteMåned, budgetÅrTilDato, budgetSidsteÅr, bogført, bogførtSidsteMåned, bogførtÅrTilDato, bogførtSidsteÅr, disponibel).Object;
        }

        internal static Mock<IOpgørelseViewModel> BuildOpgørelseViewModelMock(this ISpecimenBuilder fixture, int? nummer = null, string tekst = null, decimal? budget = null, decimal? budgetSidsteMåned = null, decimal? budgetÅrTilDato = null, decimal? budgetSidsteÅr = null, decimal? bogført = null, decimal? bogførtSidsteMåned = null, decimal? bogførtÅrTilDato = null, decimal? bogførtSidsteÅr = null, decimal? disponibel = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            nummer = nummer ?? fixture.Create<int>();
            tekst = tekst ?? fixture.Create<string>();

            Mock<IOpgørelseViewModel> opgørelseViewModelMock = new Mock<IOpgørelseViewModel>();
            opgørelseViewModelMock.Setup(m => m.Id)
                .Returns(nummer.Value.ToString());
            opgørelseViewModelMock.Setup(m => m.Nummer)
                .Returns(nummer.Value);
            opgørelseViewModelMock.Setup(m => m.Tekst)
                .Returns(tekst);
            opgørelseViewModelMock.Setup(m => m.DisplayName)
                .Returns(tekst);
            opgørelseViewModelMock.Setup(m => m.Budget)
                .Returns(budget ?? fixture.Create<decimal>());
            opgørelseViewModelMock.Setup(m => m.BudgetSidsteMåned)
                .Returns(budgetSidsteMåned ?? fixture.Create<decimal>());
            opgørelseViewModelMock.Setup(m => m.BudgetÅrTilDato)
                .Returns(budgetÅrTilDato ?? fixture.Create<decimal>());
            opgørelseViewModelMock.Setup(m => m.BudgetSidsteÅr)
                .Returns(budgetSidsteÅr ?? fixture.Create<decimal>());
            opgørelseViewModelMock.Setup(m => m.Bogført)
                .Returns(bogført ?? fixture.Create<decimal>());
            opgørelseViewModelMock.Setup(m => m.BogførtSidsteMåned)
                .Returns(bogførtSidsteMåned ?? fixture.Create<decimal>());
            opgørelseViewModelMock.Setup(m => m.BogførtÅrTilDato)
                .Returns(bogførtÅrTilDato ?? fixture.Create<decimal>());
            opgørelseViewModelMock.Setup(m => m.BogførtSidsteÅr)
                .Returns(bogførtSidsteÅr ?? fixture.Create<decimal>());
            opgørelseViewModelMock.Setup(m => m.Disponibel)
                .Returns(disponibel ?? fixture.Create<decimal>());
            opgørelseViewModelMock.Setup(m => m.CreateOpgørelseslinje(It.IsAny<IRegnskabViewModel>()))
                .Throws<NotSupportedException>();
            return opgørelseViewModelMock;
        }

        internal static INyhedViewModel BuildNyhedViewModel(this ISpecimenBuilder fixture, Nyhedsaktualitet? nyhedsaktualitet = null, DateTime? nyhedsudgivelsestidspunkt = null, string nyhedsinformation = null, byte[] image = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return fixture.BuildNyhedViewModelMock(nyhedsaktualitet, nyhedsudgivelsestidspunkt, nyhedsinformation, image).Object;
        }

        internal static Mock<INyhedViewModel> BuildNyhedViewModelMock(this ISpecimenBuilder fixture, Nyhedsaktualitet? nyhedsaktualitet = null, DateTime? nyhedsudgivelsestidspunkt = null, string nyhedsinformation = null, byte[] image = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            Random random = new Random(fixture.Create<int>());

            Mock<INyhedViewModel> mock = new Mock<INyhedViewModel>();
            mock.Setup(m => m.Nyhedsaktualitet)
                .Returns(nyhedsaktualitet ?? fixture.Create<Nyhedsaktualitet>());
            mock.Setup(m => m.Nyhedsudgivelsestidspunkt)
                .Returns(nyhedsudgivelsestidspunkt ?? DateTime.Now.AddDays(random.Next(0, 7) * -1).AddMinutes(random.Next(0, 120) * -1));
            mock.Setup(m => m.Nyhedsinformation)
                .Returns(nyhedsinformation ?? fixture.Create<string>());
            mock.Setup(m => m.Image)
                .Returns(image ?? fixture.CreateMany<byte>(random.Next(256, 512)).ToArray());
            return mock;
        }

        internal static IViewModel BuildViewModel(this ISpecimenBuilder fixture, string displayName = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return fixture.BuildViewModelMock(displayName).Object;
        }

        private static Mock<IViewModel> BuildViewModelMock(this ISpecimenBuilder fixture, string displayName = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            Mock<IViewModel> mock = new Mock<IViewModel>();
            mock.Setup(m => m.DisplayName)
                .Returns(displayName ?? fixture.Create<string>());
            return mock;
        }

        internal static IExceptionHandlerViewModel BuildExceptionHandlerViewModel(this ISpecimenBuilder fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return fixture.BuildExceptionHandlerViewModelMock().Object;
        }

        internal static Mock<IExceptionHandlerViewModel> BuildExceptionHandlerViewModelMock(this ISpecimenBuilder fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return new Mock<IExceptionHandlerViewModel>();
        }
    }
}