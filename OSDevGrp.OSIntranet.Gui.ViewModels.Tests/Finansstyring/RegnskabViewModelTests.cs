using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Rhino.Mocks;
using Text = OSDevGrp.OSIntranet.Gui.Resources.Text;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Finansstyring
{
    /// <summary>
    /// Tester ViewModel for et regnskab.
    /// </summary>
    [TestFixture]
    public class RegnskabViewModelTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en ViewModel for et regnskab.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererRegnskabViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() =>
            {
                var mock = MockRepository.GenerateMock<IRegnskabModel>();
                mock.Expect(m => m.Nummer)
                    .Return(fixture.Create<int>())
                    .Repeat.Any();
                mock.Expect(m => m.Navn)
                    .Return(fixture.Create<string>())
                    .Repeat.Any();
                return mock;
            }));

            var regnskabModelMock = fixture.Create<IRegnskabModel>();
            var statusDato = fixture.Create<DateTime>();
            var statusDatoAsMonthText = statusDato.ToString("MMMM yyyy");
            var statusDatoAsLastMonthText = statusDato.AddMonths(-1).ToString("MMMM yyyy");
            var regnskabViewModel = new RegnskabViewModel(regnskabModelMock, statusDato, fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Nummer, Is.EqualTo(regnskabModelMock.Nummer));
            Assert.That(regnskabViewModel.Navn, Is.Not.Null);
            Assert.That(regnskabViewModel.Navn, Is.Not.Empty);
            Assert.That(regnskabViewModel.Navn, Is.EqualTo(regnskabModelMock.Navn));
            Assert.That(regnskabViewModel.DisplayName, Is.Not.Null);
            Assert.That(regnskabViewModel.DisplayName, Is.Not.Empty);
            Assert.That(regnskabViewModel.DisplayName, Is.EqualTo(regnskabModelMock.Navn));
            Assert.That(regnskabViewModel.StatusDato, Is.EqualTo(statusDato));
            Assert.That(regnskabViewModel.StatusDatoAsMonthText, Is.Not.Null);
            Assert.That(regnskabViewModel.StatusDatoAsMonthText, Is.Not.Empty);
            Assert.That(regnskabViewModel.StatusDatoAsMonthText, Is.EqualTo(string.Format("{0}{1}", statusDatoAsMonthText.Substring(0, 1).ToUpper(), statusDatoAsMonthText.Substring(1).ToLower())));
            Assert.That(regnskabViewModel.StatusDatoAsLastMonthText, Is.Not.Null);
            Assert.That(regnskabViewModel.StatusDatoAsLastMonthText, Is.Not.Empty);
            Assert.That(regnskabViewModel.StatusDatoAsLastMonthText, Is.EqualTo(string.Format("{0}{1}", statusDatoAsLastMonthText.Substring(0, 1).ToUpper(), statusDatoAsLastMonthText.Substring(1).ToLower())));
            Assert.That(regnskabViewModel.StatusDatoAsYearToDateText, Is.Not.Null);
            Assert.That(regnskabViewModel.StatusDatoAsYearToDateText, Is.Not.Empty);
            Assert.That(regnskabViewModel.StatusDatoAsYearToDateText, Is.EqualTo(Resource.GetText(Text.YearToDate, statusDato.Year)));
            Assert.That(regnskabViewModel.StatusDatoAsLastYearText, Is.Not.Null);
            Assert.That(regnskabViewModel.StatusDatoAsLastYearText, Is.Not.Empty);
            Assert.That(regnskabViewModel.StatusDatoAsLastYearText, Is.EqualTo(Resource.GetText(Text.LastYear, statusDato.AddYears(-1).Year)));
            Assert.That(regnskabViewModel.Konti, Is.Not.Null);
            Assert.That(regnskabViewModel.Konti, Is.Empty);
            Assert.That(regnskabViewModel.KontiGrouped, Is.Not.Null);
            Assert.That(regnskabViewModel.KontiGrouped, Is.Empty);
            Assert.That(regnskabViewModel.KontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.KontiTop, Is.Empty);
            Assert.That(regnskabViewModel.KontiTopGrouped, Is.Not.Null);
            Assert.That(regnskabViewModel.KontiTopGrouped, Is.Empty);
            Assert.That(regnskabViewModel.KontiHeader, Is.Not.Null);
            Assert.That(regnskabViewModel.KontiHeader, Is.Not.Empty);
            Assert.That(regnskabViewModel.KontiHeader, Is.EqualTo(Resource.GetText(Text.Accounts)));
            Assert.That(regnskabViewModel.KontiColumns, Is.Not.Null);
            Assert.That(regnskabViewModel.KontiColumns, Is.Not.Empty);
            Assert.That(regnskabViewModel.KontiColumns.Count(), Is.EqualTo(5));
            Assert.That(regnskabViewModel.KontiColumns.ElementAt(0), Is.Not.Null);
            Assert.That(regnskabViewModel.KontiColumns.ElementAt(0), Is.Not.Empty);
            Assert.That(regnskabViewModel.KontiColumns.ElementAt(0), Is.EqualTo(Resource.GetText(Text.AccountNumber)));
            Assert.That(regnskabViewModel.KontiColumns.ElementAt(1), Is.Not.Null);
            Assert.That(regnskabViewModel.KontiColumns.ElementAt(1), Is.Not.Empty);
            Assert.That(regnskabViewModel.KontiColumns.ElementAt(1), Is.EqualTo(Resource.GetText(Text.AccountName)));
            Assert.That(regnskabViewModel.KontiColumns.ElementAt(2), Is.Not.Null);
            Assert.That(regnskabViewModel.KontiColumns.ElementAt(2), Is.Not.Empty);
            Assert.That(regnskabViewModel.KontiColumns.ElementAt(2), Is.EqualTo(Resource.GetText(Text.Credit)));
            Assert.That(regnskabViewModel.KontiColumns.ElementAt(3), Is.Not.Null);
            Assert.That(regnskabViewModel.KontiColumns.ElementAt(3), Is.Not.Empty);
            Assert.That(regnskabViewModel.KontiColumns.ElementAt(3), Is.EqualTo(Resource.GetText(Text.Balance)));
            Assert.That(regnskabViewModel.KontiColumns.ElementAt(4), Is.Not.Null);
            Assert.That(regnskabViewModel.KontiColumns.ElementAt(4), Is.Not.Empty);
            Assert.That(regnskabViewModel.KontiColumns.ElementAt(4), Is.EqualTo(Resource.GetText(Text.Available)));
            Assert.That(regnskabViewModel.Budgetkonti, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Empty);
            Assert.That(regnskabViewModel.BudgetkontiGrouped, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiGrouped, Is.Empty);
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Empty);
            Assert.That(regnskabViewModel.BudgetkontiTopGrouped, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiTopGrouped, Is.Empty);
            Assert.That(regnskabViewModel.BudgetkontiHeader, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiHeader, Is.Not.Empty);
            Assert.That(regnskabViewModel.BudgetkontiHeader, Is.EqualTo(Resource.GetText(Text.BudgetAccounts)));
            Assert.That(regnskabViewModel.BudgetkontiColumns, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiColumns, Is.Not.Empty);
            Assert.That(regnskabViewModel.BudgetkontiColumns.Count(), Is.EqualTo(4));
            Assert.That(regnskabViewModel.BudgetkontiColumns.ElementAt(0), Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiColumns.ElementAt(0), Is.Not.Empty);
            Assert.That(regnskabViewModel.BudgetkontiColumns.ElementAt(0), Is.EqualTo(Resource.GetText(Text.AccountNumber)));
            Assert.That(regnskabViewModel.BudgetkontiColumns.ElementAt(1), Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiColumns.ElementAt(1), Is.Not.Empty);
            Assert.That(regnskabViewModel.BudgetkontiColumns.ElementAt(1), Is.EqualTo(Resource.GetText(Text.AccountName)));
            Assert.That(regnskabViewModel.BudgetkontiColumns.ElementAt(2), Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiColumns.ElementAt(2), Is.Not.Empty);
            Assert.That(regnskabViewModel.BudgetkontiColumns.ElementAt(2), Is.EqualTo(Resource.GetText(Text.Budget)));
            Assert.That(regnskabViewModel.BudgetkontiColumns.ElementAt(3), Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiColumns.ElementAt(3), Is.Not.Empty);
            Assert.That(regnskabViewModel.BudgetkontiColumns.ElementAt(3), Is.EqualTo(Resource.GetText(Text.Bookkeeped)));
            Assert.That(regnskabViewModel.Bogføringslinjer, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføringslinjer, Is.Empty);
            Assert.That(regnskabViewModel.BogføringslinjerHeader, Is.Not.Null);
            Assert.That(regnskabViewModel.BogføringslinjerHeader, Is.Not.Empty);
            Assert.That(regnskabViewModel.BogføringslinjerHeader, Is.EqualTo(Resource.GetText(Text.Bookkeeping)));
            Assert.That(regnskabViewModel.BogføringslinjerColumns, Is.Not.Null);
            Assert.That(regnskabViewModel.BogføringslinjerColumns, Is.Not.Empty);
            Assert.That(regnskabViewModel.BogføringslinjerColumns.Count(), Is.EqualTo(7));
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(0), Is.Not.Null);
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(0), Is.Not.Empty);
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(0), Is.EqualTo(Resource.GetText(Text.Date)));
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(1), Is.Not.Null);
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(1), Is.Not.Empty);
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(1), Is.EqualTo(Resource.GetText(Text.Annex)));
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(2), Is.Not.Null);
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(2), Is.Not.Empty);
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(2), Is.EqualTo(Resource.GetText(Text.Account)));
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(3), Is.Not.Null);
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(3), Is.Not.Empty);
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(3), Is.EqualTo(Resource.GetText(Text.Text)));
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(4), Is.Not.Null);
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(4), Is.Not.Empty);
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(4), Is.EqualTo(Resource.GetText(Text.BudgetAccount)));
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(5), Is.Not.Null);
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(5), Is.Not.Empty);
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(5), Is.EqualTo(Resource.GetText(Text.Debit)));
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(6), Is.Not.Null);
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(6), Is.Not.Empty);
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(6), Is.EqualTo(Resource.GetText(Text.Credit)));
            Assert.That(regnskabViewModel.Bogføring, Is.Null);
            Assert.That(regnskabViewModel.BogføringHeader, Is.Not.Null);
            Assert.That(regnskabViewModel.BogføringHeader, Is.Not.Empty);
            Assert.That(regnskabViewModel.BogføringHeader, Is.EqualTo(Resource.GetText(Text.Bookkeeping)));
            Assert.That(regnskabViewModel.BogføringSetCommand, Is.Not.Null);
            Assert.That(regnskabViewModel.BogføringSetCommand, Is.TypeOf<BogføringSetCommand>());
            Assert.That(regnskabViewModel.Bogføringsadvarsler, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføringsadvarsler, Is.Empty);
            Assert.That(regnskabViewModel.BogføringsadvarslerHeader, Is.Not.Null);
            Assert.That(regnskabViewModel.BogføringsadvarslerHeader, Is.Not.Empty);
            Assert.That(regnskabViewModel.BogføringsadvarslerHeader, Is.EqualTo(Resource.GetText(Text.PostingWarnings)));
            Assert.That(regnskabViewModel.Opgørelseslinjer, Is.Not.Null);
            Assert.That(regnskabViewModel.Opgørelseslinjer, Is.Empty);
            Assert.That(regnskabViewModel.OpgørelseslinjerHeader, Is.Not.Null);
            Assert.That(regnskabViewModel.OpgørelseslinjerHeader, Is.Not.Empty);
            Assert.That(regnskabViewModel.OpgørelseslinjerHeader, Is.EqualTo(Resource.GetText(Text.AnnualStatement)));
            Assert.That(regnskabViewModel.OpgørelseslinjerColumns, Is.Not.Null);
            Assert.That(regnskabViewModel.OpgørelseslinjerColumns, Is.Not.Empty);
            Assert.That(regnskabViewModel.OpgørelseslinjerColumns.Count(), Is.EqualTo(3));
            Assert.That(regnskabViewModel.OpgørelseslinjerColumns.ElementAt(0), Is.Not.Null);
            Assert.That(regnskabViewModel.OpgørelseslinjerColumns.ElementAt(0), Is.Empty);
            Assert.That(regnskabViewModel.OpgørelseslinjerColumns.ElementAt(1), Is.Not.Null);
            Assert.That(regnskabViewModel.OpgørelseslinjerColumns.ElementAt(1), Is.Not.Empty);
            Assert.That(regnskabViewModel.OpgørelseslinjerColumns.ElementAt(1), Is.EqualTo(Resource.GetText(Text.Budget)));
            Assert.That(regnskabViewModel.OpgørelseslinjerColumns.ElementAt(2), Is.Not.Null);
            Assert.That(regnskabViewModel.OpgørelseslinjerColumns.ElementAt(2), Is.Not.Empty);
            Assert.That(regnskabViewModel.OpgørelseslinjerColumns.ElementAt(2), Is.EqualTo(Resource.GetText(Text.Bookkeeped)));
            Assert.That(regnskabViewModel.Budget, Is.EqualTo(0M));
            Assert.That(regnskabViewModel.BudgetAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.BudgetAsText, Is.EqualTo(Convert.ToDecimal(0M).ToString("C")));
            Assert.That(regnskabViewModel.BudgetLabel, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetLabel, Is.Not.Empty);
            Assert.That(regnskabViewModel.BudgetLabel, Is.EqualTo(Resource.GetText(Text.Budget)));
            Assert.That(regnskabViewModel.BudgetSidsteMåned, Is.EqualTo(0M));
            Assert.That(regnskabViewModel.BudgetSidsteMånedAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetSidsteMånedAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.BudgetSidsteMånedAsText, Is.EqualTo(Convert.ToDecimal(0M).ToString("C")));
            Assert.That(regnskabViewModel.BudgetSidsteMånedLabel, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetSidsteMånedLabel, Is.Not.Empty);
            Assert.That(regnskabViewModel.BudgetSidsteMånedLabel, Is.EqualTo(Resource.GetText(Text.BudgetLastMonth)));
            Assert.That(regnskabViewModel.BudgetÅrTilDato, Is.EqualTo(0M));
            Assert.That(regnskabViewModel.BudgetÅrTilDatoAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetÅrTilDatoAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.BudgetÅrTilDatoAsText, Is.EqualTo(Convert.ToDecimal(0M).ToString("C")));
            Assert.That(regnskabViewModel.BudgetÅrTilDatoLabel, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetÅrTilDatoLabel, Is.Not.Empty);
            Assert.That(regnskabViewModel.BudgetÅrTilDatoLabel, Is.EqualTo(Resource.GetText(Text.BudgetYearToDate)));
            Assert.That(regnskabViewModel.BudgetSidsteÅr, Is.EqualTo(0M));
            Assert.That(regnskabViewModel.BudgetSidsteÅrAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetSidsteÅrAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.BudgetSidsteÅrAsText, Is.EqualTo(Convert.ToDecimal(0M).ToString("C")));
            Assert.That(regnskabViewModel.BudgetSidsteÅrLabel, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetSidsteÅrLabel, Is.Not.Empty);
            Assert.That(regnskabViewModel.BudgetSidsteÅrLabel, Is.EqualTo(Resource.GetText(Text.BudgetLastYear)));
            Assert.That(regnskabViewModel.Bogført, Is.EqualTo(0M));
            Assert.That(regnskabViewModel.BogførtAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.BogførtAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.BogførtAsText, Is.EqualTo(Convert.ToDecimal(0M).ToString("C")));
            Assert.That(regnskabViewModel.BogførtLabel, Is.Not.Null);
            Assert.That(regnskabViewModel.BogførtLabel, Is.Not.Empty);
            Assert.That(regnskabViewModel.BogførtLabel, Is.EqualTo(Resource.GetText(Text.Bookkeeped)));
            Assert.That(regnskabViewModel.BogførtSidsteMåned, Is.EqualTo(0M));
            Assert.That(regnskabViewModel.BogførtSidsteMånedAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.BogførtSidsteMånedAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.BogførtSidsteMånedAsText, Is.EqualTo(Convert.ToDecimal(0M).ToString("C")));
            Assert.That(regnskabViewModel.BogførtSidsteMånedLabel, Is.Not.Null);
            Assert.That(regnskabViewModel.BogførtSidsteMånedLabel, Is.Not.Empty);
            Assert.That(regnskabViewModel.BogførtSidsteMånedLabel, Is.EqualTo(Resource.GetText(Text.BookkeepedLastMonth)));
            Assert.That(regnskabViewModel.BogførtÅrTilDato, Is.EqualTo(0M));
            Assert.That(regnskabViewModel.BogførtÅrTilDatoAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.BogførtÅrTilDatoAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.BogførtÅrTilDatoAsText, Is.EqualTo(Convert.ToDecimal(0M).ToString("C")));
            Assert.That(regnskabViewModel.BogførtÅrTilDatoLabel, Is.Not.Null);
            Assert.That(regnskabViewModel.BogførtÅrTilDatoLabel, Is.Not.Empty);
            Assert.That(regnskabViewModel.BogførtÅrTilDatoLabel, Is.EqualTo(Resource.GetText(Text.BookkeepedYearToDate)));
            Assert.That(regnskabViewModel.BogførtSidsteÅr, Is.EqualTo(0M));
            Assert.That(regnskabViewModel.BogførtSidsteÅrAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.BogførtSidsteÅrAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.BogførtSidsteÅrAsText, Is.EqualTo(Convert.ToDecimal(0M).ToString("C")));
            Assert.That(regnskabViewModel.BogførtSidsteÅrLabel, Is.Not.Null);
            Assert.That(regnskabViewModel.BogførtSidsteÅrLabel, Is.Not.Empty);
            Assert.That(regnskabViewModel.BogførtSidsteÅrLabel, Is.EqualTo(Resource.GetText(Text.BookkeepedLastYear)));
            Assert.That(regnskabViewModel.BalanceHeader, Is.Not.Null);
            Assert.That(regnskabViewModel.BalanceHeader, Is.Not.Empty);
            Assert.That(regnskabViewModel.BalanceHeader, Is.EqualTo(Resource.GetText(Text.AccountingBalance)));
            Assert.That(regnskabViewModel.Aktiver, Is.Not.Null);
            Assert.That(regnskabViewModel.Aktiver, Is.Empty);
            Assert.That(regnskabViewModel.AktiverHeader, Is.Not.Null);
            Assert.That(regnskabViewModel.AktiverHeader, Is.Not.Empty);
            Assert.That(regnskabViewModel.AktiverHeader, Is.EqualTo(Resource.GetText(Text.Asserts)));
            Assert.That(regnskabViewModel.AktiverColumns.Count(), Is.EqualTo(2));
            Assert.That(regnskabViewModel.AktiverColumns.ElementAt(0), Is.Not.Null);
            Assert.That(regnskabViewModel.AktiverColumns.ElementAt(0), Is.Empty);
            Assert.That(regnskabViewModel.AktiverColumns.ElementAt(1), Is.Not.Null);
            Assert.That(regnskabViewModel.AktiverColumns.ElementAt(1), Is.Not.Empty);
            Assert.That(regnskabViewModel.AktiverColumns.ElementAt(1), Is.EqualTo(Resource.GetText(Text.Balance)));
            Assert.That(regnskabViewModel.AktiverIAlt, Is.EqualTo(0M));
            Assert.That(regnskabViewModel.AktiverIAltAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.AktiverIAltAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.AktiverIAltAsText, Is.EqualTo(Convert.ToDecimal(0M).ToString("C")));
            Assert.That(regnskabViewModel.AktiverIAltLabel, Is.Not.Null);
            Assert.That(regnskabViewModel.AktiverIAltLabel, Is.Not.Empty);
            Assert.That(regnskabViewModel.AktiverIAltLabel, Is.EqualTo(Resource.GetText(Text.AssertsTotal)));
            Assert.That(regnskabViewModel.Passiver, Is.Not.Null);
            Assert.That(regnskabViewModel.Passiver, Is.Empty);
            Assert.That(regnskabViewModel.PassiverHeader, Is.Not.Null);
            Assert.That(regnskabViewModel.PassiverHeader, Is.Not.Empty);
            Assert.That(regnskabViewModel.PassiverHeader, Is.EqualTo(Resource.GetText(Text.Liabilities)));
            Assert.That(regnskabViewModel.PassiverColumns.Count(), Is.EqualTo(2));
            Assert.That(regnskabViewModel.PassiverColumns.ElementAt(0), Is.Not.Null);
            Assert.That(regnskabViewModel.PassiverColumns.ElementAt(0), Is.Empty);
            Assert.That(regnskabViewModel.PassiverColumns.ElementAt(1), Is.Not.Null);
            Assert.That(regnskabViewModel.PassiverColumns.ElementAt(1), Is.Not.Empty);
            Assert.That(regnskabViewModel.PassiverColumns.ElementAt(1), Is.EqualTo(Resource.GetText(Text.Balance)));
            Assert.That(regnskabViewModel.PassiverIAlt, Is.EqualTo(0M));
            Assert.That(regnskabViewModel.PassiverIAltAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.PassiverIAltAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.PassiverIAltAsText, Is.EqualTo(Convert.ToDecimal(0M).ToString("C")));
            Assert.That(regnskabViewModel.PassiverIAltLabel, Is.Not.Null);
            Assert.That(regnskabViewModel.PassiverIAltLabel, Is.Not.Empty);
            Assert.That(regnskabViewModel.PassiverIAltLabel, Is.EqualTo(Resource.GetText(Text.LiabilitiesTotal)));
            Assert.That(regnskabViewModel.Debitorer, Is.Not.Null);
            Assert.That(regnskabViewModel.Debitorer, Is.Empty);
            Assert.That(regnskabViewModel.DebitorerHeader, Is.Not.Null);
            Assert.That(regnskabViewModel.DebitorerHeader, Is.Not.Empty);
            Assert.That(regnskabViewModel.DebitorerHeader, Is.EqualTo(Resource.GetText(Text.Debtors)));
            Assert.That(regnskabViewModel.Kreditorer, Is.Not.Null);
            Assert.That(regnskabViewModel.Kreditorer, Is.Empty);
            Assert.That(regnskabViewModel.KreditorerHeader, Is.Not.Null);
            Assert.That(regnskabViewModel.KreditorerHeader, Is.Not.Empty);
            Assert.That(regnskabViewModel.KreditorerHeader, Is.EqualTo(Resource.GetText(Text.Creditors)));
            Assert.That(regnskabViewModel.Nyheder, Is.Not.Null);
            Assert.That(regnskabViewModel.Nyheder, Is.Empty);
            Assert.That(regnskabViewModel.NyhederHeader, Is.Not.Null);
            Assert.That(regnskabViewModel.NyhederHeader, Is.Not.Empty);
            Assert.That(regnskabViewModel.NyhederHeader, Is.EqualTo(Resource.GetText(Text.NewsMultiple)));
            Assert.That(regnskabViewModel.Kontogrupper, Is.Not.Null);
            Assert.That(regnskabViewModel.Kontogrupper.Count(), Is.GreaterThanOrEqualTo(0));
            Assert.That(regnskabViewModel.Budgetkontogrupper, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkontogrupper.Count(), Is.GreaterThanOrEqualTo(0));
            Assert.That(regnskabViewModel.RefreshCommand, Is.Not.Null);
            Assert.That(regnskabViewModel.RefreshCommand, Is.TypeOf<CommandCollectionExecuterCommand>());
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis modellen for regnskabet er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisRegnskabModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            Assert.Throws<ArgumentNullException>(() => new RegnskabViewModel(null, fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>()));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis repositoryet til finansstyring er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFinansstyringRepositoryErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            Assert.Throws<ArgumentNullException>(() => new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), null, fixture.Create<IExceptionHandlerViewModel>()));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis ViewModel for exceptionhandleren er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisExceptionHandlerViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            Assert.Throws<ArgumentNullException>(() => new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), null));
        }

        /// <summary>
        /// Tester, at sætteren på Navn opdaterer Navn på modellen for regnskabet.
        /// </summary>
        [Test]
        public void TestAtNavnSetterOpdatererNavnOnRegnskabModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var regnskabModelMock = fixture.Create<IRegnskabModel>();
            var regnskabViewModel = new RegnskabViewModel(regnskabModelMock, fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var newValue = fixture.Create<string>();
            regnskabViewModel.Navn = newValue;

            regnskabModelMock.AssertWasCalled(m => m.Navn = Arg<string>.Is.Equal(newValue));
        }

        /// <summary>
        /// Tester, at sætteren til StatusDato kaster en ArgumentException, hvis værdien er mindre end den allerede satte statusdato.
        /// </summary>
        [Test]
        [TestCase("2013-01-01T12:00:00", "2013-01-01T11:59:59")]
        [TestCase("2013-01-01T12:00:00", "2012-12-31T23:00:00")]
        [TestCase("2013-01-01T12:00:00", "2012-12-31T12:00:00")]
        public void TestAtStatusDatoSetterKasterArgumentExceptionHvisValueErMindreEndStatusDato(string originalDateTime, string newDateTime)
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var statusDato = DateTime.Parse(originalDateTime, new CultureInfo("en-US"));
            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), statusDato, fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.StatusDato, Is.EqualTo(statusDato));

            Assert.Throws<ArgumentException>(() => regnskabViewModel.StatusDato = DateTime.Parse(newDateTime, new CultureInfo("en-US")));
        }

        /// <summary>
        /// Tester, at sætteren til StatusDato opdaterer statusdato.
        /// </summary>
        [Test]
        [TestCase("2013-01-01T12:00:00", "2013-01-01T12:00:01")]
        [TestCase("2013-01-01T12:00:00", "2013-06-30T12:00:00")]
        [TestCase("2013-01-01T12:00:00", "2013-12-31T00:00:00")]
        public void TestAtStatusDatoSetterOpdatererStatusDato(string originalDateTime, string newDateTime)
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var statusDato = DateTime.Parse(originalDateTime, new CultureInfo("en-US"));
            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), statusDato, fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.StatusDato, Is.EqualTo(statusDato));

            var newValue = DateTime.Parse(newDateTime, new CultureInfo("en-US"));
            regnskabViewModel.StatusDato = newValue;
            Assert.That(regnskabViewModel.StatusDato, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tester, at sætteren til StatusDato rejser PropertyChanged ved ændring af statusdatoen.
        /// </summary>
        [Test]
        [TestCase("2013-01-01T12:00:00", "2013-01-01T12:00:01", "StatusDato")]
        [TestCase("2013-01-01T12:00:00", "2013-06-30T12:00:00", "StatusDato")]
        [TestCase("2013-01-01T12:00:00", "2013-12-31T00:00:00", "StatusDato")]
        [TestCase("2013-01-01T12:00:00", "2013-01-01T12:00:01", "StatusDatoAsMonthText")]
        [TestCase("2013-01-01T12:00:00", "2013-06-30T12:00:00", "StatusDatoAsMonthText")]
        [TestCase("2013-01-01T12:00:00", "2013-12-31T00:00:00", "StatusDatoAsMonthText")]
        [TestCase("2013-01-01T12:00:00", "2013-01-01T12:00:01", "StatusDatoAsLastMonthText")]
        [TestCase("2013-01-01T12:00:00", "2013-06-30T12:00:00", "StatusDatoAsLastMonthText")]
        [TestCase("2013-01-01T12:00:00", "2013-12-31T00:00:00", "StatusDatoAsLastMonthText")]
        [TestCase("2013-01-01T12:00:00", "2013-01-01T12:00:01", "StatusDatoAsYearToDateText")]
        [TestCase("2013-01-01T12:00:00", "2013-06-30T12:00:00", "StatusDatoAsYearToDateText")]
        [TestCase("2013-01-01T12:00:00", "2013-12-31T00:00:00", "StatusDatoAsYearToDateText")]
        [TestCase("2013-01-01T12:00:00", "2013-01-01T12:00:01", "StatusDatoAsLastYearText")]
        [TestCase("2013-01-01T12:00:00", "2013-06-30T12:00:00", "StatusDatoAsLastYearText")]
        [TestCase("2013-01-01T12:00:00", "2013-12-31T00:00:00", "StatusDatoAsLastYearText")]
        public void TestAtStatusDatoSetterRejserPropertyChangedVedVedOpdateringAfStatusDato(string originalDateTime, string newDateTime, string expectPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var statusDato = DateTime.Parse(originalDateTime, new CultureInfo("en-US"));
            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), statusDato, fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, expectPropertyName, StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            regnskabViewModel.StatusDato = DateTime.Parse(newDateTime, new CultureInfo("en-US"));
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at getteren til KontiGroup returnerer grupperede konti.
        /// </summary>
        [Test]
        [TestCase(new[] {5, 6, 7, 8, 9}, "Aktiver")]
        [TestCase(new[] {5, 6, 7, 8, 9}, "Passiver")]
        public void TestAtKontiGroupedGetterReturnererDictionary(int[] kontogrupper, string balanceType)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IBogføringViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringViewModel>()));

            var kontogruppeViewModelMockCollection = new List<IKontogruppeViewModel>(kontogrupper.Length);
            foreach (var id in kontogrupper)
            {
                var balanceViewModelMock = MockRepository.GenerateMock<IBalanceViewModel>();
                balanceViewModelMock.Expect(m => m.Nummer)
                    .Return(id)
                    .Repeat.Any();
                balanceViewModelMock.Expect(m => m.Balancetype)
                    .Return((Balancetype) Enum.Parse(typeof (Balancetype), balanceType))
                    .Repeat.Any();
                var kontogruppeViewModelMock = MockRepository.GenerateMock<IKontogruppeViewModel>();
                kontogruppeViewModelMock.Expect(m => m.Nummer)
                                        .Return(balanceViewModelMock.Nummer)
                                        .Repeat.Any();
                kontogruppeViewModelMock.Expect(m => m.CreateBalancelinje(Arg<IRegnskabViewModel>.Is.NotNull))
                    .Return(balanceViewModelMock)
                    .Repeat.Any();
                kontogruppeViewModelMockCollection.Add(kontogruppeViewModelMock);
            }

            var kontoViewModelMockCollection = CreateKontoViewModels(fixture, kontogruppeViewModelMockCollection, new Random(DateTime.Now.Second), 250).ToList();

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Konti, Is.Not.Null);
            Assert.That(regnskabViewModel.Konti, Is.Empty);
            Assert.That(regnskabViewModel.KontiGrouped, Is.Not.Null);
            Assert.That(regnskabViewModel.KontiGrouped, Is.Empty);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            regnskabViewModel.BogføringSet(fixture.Create<IBogføringViewModel>());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            kontogruppeViewModelMockCollection.ForEach(regnskabViewModel.KontogruppeAdd);
            kontoViewModelMockCollection.ForEach(regnskabViewModel.KontoAdd);
            Assert.That(regnskabViewModel.Konti, Is.Not.Null);
            Assert.That(regnskabViewModel.Konti, Is.Not.Empty);

            var kontiGrouped = regnskabViewModel.KontiGrouped;
            Assert.That(kontiGrouped, Is.Not.Null);
            Assert.That(kontiGrouped, Is.Not.Empty);
            Assert.That(kontiGrouped.Sum(m => m.Value.Count()), Is.EqualTo(regnskabViewModel.Konti.Count()));
        }

        /// <summary>
        /// Tester, at getteren til KontiTop udelader konti, hvor kontoværdien er 0.
        /// </summary>
        [Test]
        [TestCase("Aktiver")]
        [TestCase("Passiver")]
        public void TestAtKontiTopGetterUdeladerKontiHvorKontoValueErNul(string balanceType)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IBogføringViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringViewModel>()));
            fixture.Customize<IBalanceViewModel>(e => e.FromFactory(() =>
            {
                var mock = MockRepository.GenerateMock<IBalanceViewModel>();
                mock.Expect(m => m.Nummer)
                    .Return(fixture.Create<int>())
                    .Repeat.Any();
                mock.Expect(m => m.Balancetype)
                    .Return((Balancetype) Enum.Parse(typeof (Balancetype), balanceType))
                    .Repeat.Any();
                return mock;
            }));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() =>
            {
                var balanceViewModelMock = fixture.Create<IBalanceViewModel>();
                var mock = MockRepository.GenerateMock<IKontogruppeViewModel>();
                mock.Expect(m => m.Nummer)
                    .Return(balanceViewModelMock.Nummer)
                    .Repeat.Any();
                mock.Expect(m => m.CreateBalancelinje(Arg<IRegnskabViewModel>.Is.NotNull))
                    .Return(balanceViewModelMock)
                    .Repeat.Any();
                return mock;
            }));
            fixture.Customize<IKontoViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IKontoViewModel>();
                    mock.Expect(m => m.Kontonummer)
                        .Return(fixture.Create<string>())
                        .Repeat.Any();
                    mock.Expect(m => m.Kontogruppe)
                        .Return(fixture.Create<IKontogruppeViewModel>())
                        .Repeat.Any();
                    mock.Expect(m => m.Kontoværdi)
                        .Return(0M)
                        .Repeat.Any();
                    return mock;
                }));

            var kontoViewModelMockCollection = fixture.CreateMany<IKontoViewModel>(250).ToList();

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Konti, Is.Not.Null);
            Assert.That(regnskabViewModel.Konti, Is.Empty);
            Assert.That(regnskabViewModel.KontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.KontiTop, Is.Empty);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            regnskabViewModel.BogføringSet(fixture.Create<IBogføringViewModel>());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            kontoViewModelMockCollection.ForEach(regnskabViewModel.KontoAdd);
            Assert.That(regnskabViewModel.Konti, Is.Not.Null);
            Assert.That(regnskabViewModel.Konti, Is.Not.Empty);
            Assert.That(regnskabViewModel.Konti.Count(), Is.EqualTo(kontoViewModelMockCollection.Count));
            Assert.That(regnskabViewModel.KontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.KontiTop, Is.Empty);
        }

        /// <summary>
        /// Tester, at getteren til KontiTop returnerer topbenyttede konti.
        /// </summary>
        [Test]
        [TestCase("Aktiver")]
        [TestCase("Passiver")]
        public void TestAtKontiTopGetterReturnererTopbenyttedeKonti(string balanceType)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IBogføringViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringViewModel>()));
            fixture.Customize<IBalanceViewModel>(e => e.FromFactory(() =>
            {
                var mock = MockRepository.GenerateMock<IBalanceViewModel>();
                mock.Expect(m => m.Nummer)
                    .Return(fixture.Create<int>())
                    .Repeat.Any();
                mock.Expect(m => m.Balancetype)
                    .Return((Balancetype)Enum.Parse(typeof(Balancetype), balanceType))
                    .Repeat.Any();
                return mock;
            }));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() =>
            {
                var balanceViewModelMock = fixture.Create<IBalanceViewModel>();
                var mock = MockRepository.GenerateMock<IKontogruppeViewModel>();
                mock.Expect(m => m.Nummer)
                    .Return(balanceViewModelMock.Nummer)
                    .Repeat.Any();
                mock.Expect(m => m.CreateBalancelinje(Arg<IRegnskabViewModel>.Is.NotNull))
                    .Return(balanceViewModelMock)
                    .Repeat.Any();
                return mock;
            }));
            fixture.Customize<IKontoViewModel>(e => e.FromFactory(() =>
            {
                var mock = MockRepository.GenerateMock<IKontoViewModel>();
                mock.Expect(m => m.Kontonummer)
                    .Return(fixture.Create<string>())
                    .Repeat.Any();
                mock.Expect(m => m.Kontogruppe)
                    .Return(fixture.Create<IKontogruppeViewModel>())
                    .Repeat.Any();
                mock.Expect(m => m.Kontoværdi)
                    .Return(fixture.Create<decimal>())
                    .Repeat.Any();
                return mock;
            }));

            var kontoViewModelMockCollection = fixture.CreateMany<IKontoViewModel>(250).ToList();

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Konti, Is.Not.Null);
            Assert.That(regnskabViewModel.Konti, Is.Empty);
            Assert.That(regnskabViewModel.KontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.KontiTop, Is.Empty);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            regnskabViewModel.BogføringSet(fixture.Create<IBogføringViewModel>());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            kontoViewModelMockCollection.ForEach(regnskabViewModel.KontoAdd);
            Assert.That(regnskabViewModel.Konti, Is.Not.Null);
            Assert.That(regnskabViewModel.Konti, Is.Not.Empty);
            Assert.That(regnskabViewModel.Konti.Count(), Is.EqualTo(kontoViewModelMockCollection.Count));
            Assert.That(regnskabViewModel.KontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.KontiTop, Is.Not.Empty);
            Assert.That(regnskabViewModel.KontiTop.Count(), Is.EqualTo(25));
        }

        /// <summary>
        /// Tester, at getteren til KontiTopGroup returnerer grupperede topbenyttede konti.
        /// </summary>
        [Test]
        [TestCase(new[] {5, 6, 7, 8, 9}, "Aktiver")]
        [TestCase(new[] {5, 6, 7, 8, 9}, "Passiver")]
        public void TestAtKontiTopGroupedGetterReturnererDictionary(int[] kontogrupper, string balanceType)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IBogføringViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringViewModel>()));

            var kontogruppeViewModelMockCollection = new List<IKontogruppeViewModel>(kontogrupper.Length);
            foreach (var id in kontogrupper)
            {
                var balanceViewModelMock = MockRepository.GenerateMock<IBalanceViewModel>();
                balanceViewModelMock.Expect(m => m.Nummer)
                    .Return(id)
                    .Repeat.Any();
                balanceViewModelMock.Expect(m => m.Balancetype)
                    .Return((Balancetype) Enum.Parse(typeof (Balancetype), balanceType))
                    .Repeat.Any();
                var kontogruppeViewModelMock = MockRepository.GenerateMock<IKontogruppeViewModel>();
                kontogruppeViewModelMock.Expect(m => m.Nummer)
                    .Return(balanceViewModelMock.Nummer)
                    .Repeat.Any();
                kontogruppeViewModelMock.Expect(m => m.CreateBalancelinje(Arg<IRegnskabViewModel>.Is.NotNull))
                    .Return(balanceViewModelMock)
                    .Repeat.Any();
                kontogruppeViewModelMockCollection.Add(kontogruppeViewModelMock);
            }

            var kontoViewModelMockCollection = CreateKontoViewModels(fixture, kontogruppeViewModelMockCollection, new Random(DateTime.Now.Second), 250).ToList();

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.KontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.KontiTop, Is.Empty);
            Assert.That(regnskabViewModel.KontiTopGrouped, Is.Not.Null);
            Assert.That(regnskabViewModel.KontiTopGrouped, Is.Empty);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            regnskabViewModel.BogføringSet(fixture.Create<IBogføringViewModel>());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            kontogruppeViewModelMockCollection.ForEach(regnskabViewModel.KontogruppeAdd);
            kontoViewModelMockCollection.ForEach(regnskabViewModel.KontoAdd);
            Assert.That(regnskabViewModel.KontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.KontiTop, Is.Not.Empty);

            var kontiTopGrouped = regnskabViewModel.KontiTopGrouped;
            Assert.That(kontiTopGrouped, Is.Not.Null);
            Assert.That(kontiTopGrouped, Is.Not.Empty);
            Assert.That(kontiTopGrouped.Sum(m => m.Value.Count()), Is.EqualTo(regnskabViewModel.KontiTop.Count()));
        }

        /// <summary>
        /// Tester, at getteren til BudgetkontiGroup returnerer grupperede budgetkonti.
        /// </summary>
        [Test]
        [TestCase(new[] {5, 6, 7, 8, 9})]
        public void TestAtBudgetkontiGroupedGetterReturnererDictionary(int[] budgetkontogrupper)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var budgetkontogruppeViewModelMockCollection = new List<IBudgetkontogruppeViewModel>(budgetkontogrupper.Length);
            foreach (var id in budgetkontogrupper)
            {
                var opgørelseViewModelMock = MockRepository.GenerateMock<IOpgørelseViewModel>();
                opgørelseViewModelMock.Expect(m => m.Nummer)
                    .Return(id)
                    .Repeat.Any();
                var budgetkontogruppeViewModelMock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                budgetkontogruppeViewModelMock.Expect(m => m.Nummer)
                    .Return(id)
                    .Repeat.Any();
                budgetkontogruppeViewModelMock.Expect(m => m.CreateOpgørelseslinje(Arg<IRegnskabViewModel>.Is.NotNull))
                    .Return(opgørelseViewModelMock)
                    .Repeat.Any();
                budgetkontogruppeViewModelMockCollection.Add(budgetkontogruppeViewModelMock);
            }

            var budgetkontoViewModelMockCollection = CreateBudgetkontoViewModels(fixture, budgetkontogruppeViewModelMockCollection, new Random(DateTime.Now.Second), 250).ToList();

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Empty);
            Assert.That(regnskabViewModel.BudgetkontiGrouped, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiGrouped, Is.Empty);

            budgetkontogruppeViewModelMockCollection.ForEach(regnskabViewModel.BudgetkontogruppeAdd);
            budgetkontoViewModelMockCollection.ForEach(regnskabViewModel.BudgetkontoAdd);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Not.Empty);

            var budgetkontiGrouped = regnskabViewModel.BudgetkontiGrouped;
            Assert.That(budgetkontiGrouped, Is.Not.Null);
            Assert.That(budgetkontiGrouped, Is.Not.Empty);
            Assert.That(budgetkontiGrouped.Sum(m => m.Value.Count()), Is.EqualTo(regnskabViewModel.Budgetkonti.Count()));
        }

        /// <summary>
        /// Tester, at getteren til BudgetkontiTop udelader budgetkonti, hvor kontoværdien er 0.
        /// </summary>
        [Test]
        public void TestAtBudgetkontiTopGetterUdeladerBudgetkontiHvorKontoValueErNul()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IOpgørelseViewModel>(e => e.FromFactory(() =>
            {
                var mock = MockRepository.GenerateMock<IOpgørelseViewModel>();
                mock.Expect(m => m.Nummer)
                    .Return(fixture.Create<int>())
                    .Repeat.Any();
                return mock;
            }));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() =>
            {
                var opgørelseViewModelMock = fixture.Create<IOpgørelseViewModel>();
                var mock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                mock.Expect(m => m.Nummer)
                    .Return(opgørelseViewModelMock.Nummer)
                    .Repeat.Any();
                mock.Expect(m => m.CreateOpgørelseslinje(Arg<IRegnskabViewModel>.Is.NotNull))
                    .Return(opgørelseViewModelMock)
                    .Repeat.Any();
                return mock;
            }));
            fixture.Customize<IBudgetkontoViewModel>(e => e.FromFactory(() =>
            {
                var mock = MockRepository.GenerateMock<IBudgetkontoViewModel>();
                mock.Expect(m => m.Kontonummer)
                    .Return(fixture.Create<string>())
                    .Repeat.Any();
                mock.Expect(m => m.Kontogruppe)
                    .Return(fixture.Create<IBudgetkontogruppeViewModel>())
                    .Repeat.Any();
                mock.Expect(m => m.Kontoværdi)
                    .Return(0M)
                    .Repeat.Any();
                return mock;
            }));

            var budgetkontoViewModelMockCollection = fixture.CreateMany<IBudgetkontoViewModel>(250).ToList();

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Empty);
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Empty);

            budgetkontoViewModelMockCollection.ForEach(regnskabViewModel.BudgetkontoAdd);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Not.Empty);
            Assert.That(regnskabViewModel.Budgetkonti.Count(), Is.EqualTo(budgetkontoViewModelMockCollection.Count));
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Empty);
        }

        /// <summary>
        /// Tester, at getteren til BudgetkontiTop returnerer topbenyttede budgetkonti.
        /// </summary>
        [Test]
        public void TestAtBudgetkontiTopGetterReturnererTopbenyttedeBudgetkonti()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IOpgørelseViewModel>(e => e.FromFactory(() =>
            {
                var mock = MockRepository.GenerateMock<IOpgørelseViewModel>();
                mock.Expect(m => m.Nummer)
                    .Return(fixture.Create<int>())
                    .Repeat.Any();
                return mock;
            }));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() =>
            {
                var opgørelseViewModelMock = fixture.Create<IOpgørelseViewModel>();
                var mock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                mock.Expect(m => m.Nummer)
                    .Return(opgørelseViewModelMock.Nummer)
                    .Repeat.Any();
                mock.Expect(m => m.CreateOpgørelseslinje(Arg<IRegnskabViewModel>.Is.NotNull))
                    .Return(opgørelseViewModelMock)
                    .Repeat.Any();
                return mock;
            }));
            fixture.Customize<IBudgetkontoViewModel>(e => e.FromFactory(() =>
            {
                var mock = MockRepository.GenerateMock<IBudgetkontoViewModel>();
                mock.Expect(m => m.Kontonummer)
                    .Return(fixture.Create<string>())
                    .Repeat.Any();
                mock.Expect(m => m.Kontogruppe)
                    .Return(fixture.Create<IBudgetkontogruppeViewModel>())
                    .Repeat.Any();
                mock.Expect(m => m.Kontoværdi)
                    .Return(fixture.Create<decimal>())
                    .Repeat.Any();
                return mock;
            }));

            var budgetkontoViewModelMockCollection = fixture.CreateMany<IBudgetkontoViewModel>(250).ToList();

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Empty);
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Empty);

            budgetkontoViewModelMockCollection.ForEach(regnskabViewModel.BudgetkontoAdd);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Not.Empty);
            Assert.That(regnskabViewModel.Budgetkonti.Count(), Is.EqualTo(budgetkontoViewModelMockCollection.Count));
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Not.Empty);
            Assert.That(regnskabViewModel.BudgetkontiTop.Count(), Is.EqualTo(25));
        }

        /// <summary>
        /// Tester, at getteren til BudgetkontiTopGroup returnerer grupperede topbenyttede budgetkonti.
        /// </summary>
        [Test]
        [TestCase(new[] {5, 6, 7, 8, 9})]
        public void TestAtBudgetkontiTopGroupedGetterReturnererDictionary(int[] budgetkontogrupper)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var budgetkontogruppeViewModelMockCollection = new List<IBudgetkontogruppeViewModel>(budgetkontogrupper.Length);
            foreach (var id in budgetkontogrupper)
            {
                var opgørelseViewModelMock = MockRepository.GenerateMock<IOpgørelseViewModel>();
                opgørelseViewModelMock.Expect(m => m.Nummer)
                    .Return(id)
                    .Repeat.Any();
                var budgetkontogruppeViewModelMock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                budgetkontogruppeViewModelMock.Expect(m => m.Nummer)
                    .Return(id)
                    .Repeat.Any();
                budgetkontogruppeViewModelMock.Expect(m => m.CreateOpgørelseslinje(Arg<IRegnskabViewModel>.Is.NotNull))
                    .Return(opgørelseViewModelMock)
                    .Repeat.Any();
                budgetkontogruppeViewModelMockCollection.Add(budgetkontogruppeViewModelMock);
            }

            var budgetkontoViewModelMockCollection = CreateBudgetkontoViewModels(fixture, budgetkontogruppeViewModelMockCollection, new Random(DateTime.Now.Second), 250).ToList();

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Empty);
            Assert.That(regnskabViewModel.BudgetkontiTopGrouped, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiTopGrouped, Is.Empty);

            budgetkontogruppeViewModelMockCollection.ForEach(regnskabViewModel.BudgetkontogruppeAdd);
            budgetkontoViewModelMockCollection.ForEach(regnskabViewModel.BudgetkontoAdd);
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Not.Empty);

            var budgetkontiTopGrouped = regnskabViewModel.BudgetkontiTopGrouped;
            Assert.That(budgetkontiTopGrouped, Is.Not.Null);
            Assert.That(budgetkontiTopGrouped, Is.Not.Empty);
            Assert.That(budgetkontiTopGrouped.Sum(m => m.Value.Count()), Is.EqualTo(regnskabViewModel.BudgetkontiTop.Count()));
        }

        /// <summary>
        /// Tester, at getteren til AktiverIAlt returnerer aktiver i alt.
        /// </summary>
        [Test]
        [TestCase(new[] {5, 6, 7, 8, 9})]
        public void TestAtAktiverIAltGetterReturnererAktiverIAlt(int[] kontogrupper)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IBogføringViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringViewModel>()));

            var kontogruppeViewModelMockCollection = new List<IKontogruppeViewModel>(kontogrupper.Length);
            foreach (var id in kontogrupper)
            {
                var balanceViewModelMock = MockRepository.GenerateMock<IBalanceViewModel>();
                balanceViewModelMock.Expect(m => m.Nummer)
                    .Return(id)
                    .Repeat.Any();
                balanceViewModelMock.Expect(m => m.Balancetype)
                    .Return(Balancetype.Aktiver)
                    .Repeat.Any();
                balanceViewModelMock.Expect(m => m.Saldo)
                    .Return(fixture.Create<decimal>())
                    .Repeat.Any();
                var kontogruppeViewModelMock = MockRepository.GenerateMock<IKontogruppeViewModel>();
                kontogruppeViewModelMock.Expect(m => m.Nummer)
                    .Return(balanceViewModelMock.Nummer)
                    .Repeat.Any();
                kontogruppeViewModelMock.Expect(m => m.CreateBalancelinje(Arg<IRegnskabViewModel>.Is.NotNull))
                    .Return(balanceViewModelMock)
                    .Repeat.Any();
                kontogruppeViewModelMockCollection.Add(kontogruppeViewModelMock);
            }

            var kontoViewModelMockCollection = CreateKontoViewModels(fixture, kontogruppeViewModelMockCollection, new Random(DateTime.Now.Second), 250).ToList();

            var kontogrupperInUse = kontoViewModelMockCollection
                .GroupBy(m => m.Kontogruppe.Nummer)
                .Select(m => m.Key)
                .ToList();

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.AktiverIAlt, Is.EqualTo(0M));
            Assert.That(regnskabViewModel.AktiverIAltAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.AktiverIAltAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.AktiverIAltAsText, Is.EqualTo(Convert.ToDecimal(0M).ToString("C")));
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            var expectedValue = kontogruppeViewModelMockCollection
                .Where(m => kontogrupperInUse.Contains(m.Nummer))
                .Select(m => m.CreateBalancelinje(regnskabViewModel))
                .Sum(m => m.Saldo);

            regnskabViewModel.BogføringSet(fixture.Create<IBogføringViewModel>());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            kontogruppeViewModelMockCollection.ForEach(regnskabViewModel.KontogruppeAdd);
            kontoViewModelMockCollection.ForEach(regnskabViewModel.KontoAdd);

            Assert.That(regnskabViewModel.AktiverIAlt, Is.EqualTo(expectedValue));
            Assert.That(regnskabViewModel.AktiverIAltAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.AktiverIAltAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.AktiverIAltAsText, Is.EqualTo(expectedValue.ToString("C")));
        }

        /// <summary>
        /// Tester, at getteren til PassiverIAlt returnerer passiver i alt.
        /// </summary>
        [Test]
        [TestCase(new[] {5, 6, 7, 8, 9})]
        public void TestAtPassiverIAltGetterReturnererPassiverIAlt(int[] kontogrupper)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IBogføringViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringViewModel>()));

            var kontogruppeViewModelMockCollection = new List<IKontogruppeViewModel>(kontogrupper.Length);
            foreach (var id in kontogrupper)
            {
                var balanceViewModelMock = MockRepository.GenerateMock<IBalanceViewModel>();
                balanceViewModelMock.Expect(m => m.Nummer)
                    .Return(id)
                    .Repeat.Any();
                balanceViewModelMock.Expect(m => m.Balancetype)
                    .Return(Balancetype.Passiver)
                    .Repeat.Any();
                balanceViewModelMock.Expect(m => m.Saldo)
                    .Return(fixture.Create<decimal>())
                    .Repeat.Any();
                var kontogruppeViewModelMock = MockRepository.GenerateMock<IKontogruppeViewModel>();
                kontogruppeViewModelMock.Expect(m => m.Nummer)
                    .Return(balanceViewModelMock.Nummer)
                    .Repeat.Any();
                kontogruppeViewModelMock.Expect(m => m.CreateBalancelinje(Arg<IRegnskabViewModel>.Is.NotNull))
                    .Return(balanceViewModelMock)
                    .Repeat.Any();
                kontogruppeViewModelMockCollection.Add(kontogruppeViewModelMock);
            }

            var kontoViewModelMockCollection = CreateKontoViewModels(fixture, kontogruppeViewModelMockCollection, new Random(DateTime.Now.Second), 250).ToList();

            var kontogrupperInUse = kontoViewModelMockCollection
                .GroupBy(m => m.Kontogruppe.Nummer)
                .Select(m => m.Key)
                .ToList();

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.PassiverIAlt, Is.EqualTo(0M));
            Assert.That(regnskabViewModel.PassiverIAltAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.PassiverIAltAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.PassiverIAltAsText, Is.EqualTo(Convert.ToDecimal(0M).ToString("C")));

            var expectedValue = kontogruppeViewModelMockCollection
                .Where(m => kontogrupperInUse.Contains(m.Nummer))
                .Select(m => m.CreateBalancelinje(regnskabViewModel))
                .Sum(m => m.Saldo);

            regnskabViewModel.BogføringSet(fixture.Create<IBogføringViewModel>());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            kontogruppeViewModelMockCollection.ForEach(regnskabViewModel.KontogruppeAdd);
            kontoViewModelMockCollection.ForEach(regnskabViewModel.KontoAdd);
            
            Assert.That(regnskabViewModel.PassiverIAlt, Is.EqualTo(expectedValue));
            Assert.That(regnskabViewModel.PassiverIAltAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.PassiverIAltAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.PassiverIAltAsText, Is.EqualTo(expectedValue.ToString("C")));
        }

        /// <summary>
        /// Tester, at getteren til Budget returnerer budgetteret beløb fra årsopgørelsen.
        /// </summary>
        [Test]
        [TestCase(new[] {5, 6, 7, 8, 9})]
        public void TestAtBudgetGetterReturnererBudget(int[] budgetkontogrupper)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var budgetkontogruppeViewModelMockCollection = new List<IBudgetkontogruppeViewModel>(budgetkontogrupper.Length);
            foreach (var id in budgetkontogrupper)
            {
                var opgørelseViewModelMock = MockRepository.GenerateMock<IOpgørelseViewModel>();
                opgørelseViewModelMock.Expect(m => m.Nummer)
                    .Return(id)
                    .Repeat.Any();
                opgørelseViewModelMock.Expect(m => m.Budget)
                    .Return(fixture.Create<decimal>())
                    .Repeat.Any();
                var budgetkontogruppeViewModelMock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                budgetkontogruppeViewModelMock.Expect(m => m.Nummer)
                    .Return(id)
                    .Repeat.Any();
                budgetkontogruppeViewModelMock.Expect(m => m.CreateOpgørelseslinje(Arg<IRegnskabViewModel>.Is.NotNull))
                    .Return(opgørelseViewModelMock)
                    .Repeat.Any();
                budgetkontogruppeViewModelMockCollection.Add(budgetkontogruppeViewModelMock);
            }

            var budgetkontoViewModelMockCollection = CreateBudgetkontoViewModels(fixture, budgetkontogruppeViewModelMockCollection, new Random(DateTime.Now.Second), 250).ToList();

            var budgetkontogrupperInUse = budgetkontoViewModelMockCollection
                .GroupBy(m => m.Kontogruppe.Nummer)
                .Select(m => m.Key)
                .ToList();

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Budget, Is.EqualTo(0M));
            Assert.That(regnskabViewModel.BudgetAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.BudgetAsText, Is.EqualTo(Convert.ToDecimal(0M).ToString("C")));

            var expectedValue = budgetkontogruppeViewModelMockCollection
                .Where(m => budgetkontogrupperInUse.Contains(m.Nummer))
                .Select(m => m.CreateOpgørelseslinje(regnskabViewModel))
                .Sum(m => m.Budget);

            budgetkontogruppeViewModelMockCollection.ForEach(regnskabViewModel.BudgetkontogruppeAdd);
            budgetkontoViewModelMockCollection.ForEach(regnskabViewModel.BudgetkontoAdd);

            Assert.That(regnskabViewModel.Budget, Is.EqualTo(expectedValue));
            Assert.That(regnskabViewModel.BudgetAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.BudgetAsText, Is.EqualTo(expectedValue.ToString("C")));
        }

        /// <summary>
        /// Tester, at getteren til BudgetSidsteMåned returnerer budgetteret beløb for sidste måned fra årsopgørelsen.
        /// </summary>
        [Test]
        [TestCase(new[] {5, 6, 7, 8, 9})]
        public void TestAtBudgetSidsteMånedGetterReturnererBudgetSidsteMåned(int[] budgetkontogrupper)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var budgetkontogruppeViewModelMockCollection = new List<IBudgetkontogruppeViewModel>(budgetkontogrupper.Length);
            foreach (var id in budgetkontogrupper)
            {
                var opgørelseViewModelMock = MockRepository.GenerateMock<IOpgørelseViewModel>();
                opgørelseViewModelMock.Expect(m => m.Nummer)
                    .Return(id)
                    .Repeat.Any();
                opgørelseViewModelMock.Expect(m => m.BudgetSidsteMåned)
                    .Return(fixture.Create<decimal>())
                    .Repeat.Any();
                var budgetkontogruppeViewModelMock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                budgetkontogruppeViewModelMock.Expect(m => m.Nummer)
                    .Return(id)
                    .Repeat.Any();
                budgetkontogruppeViewModelMock.Expect(m => m.CreateOpgørelseslinje(Arg<IRegnskabViewModel>.Is.NotNull))
                    .Return(opgørelseViewModelMock)
                    .Repeat.Any();
                budgetkontogruppeViewModelMockCollection.Add(budgetkontogruppeViewModelMock);
            }

            var budgetkontoViewModelMockCollection = CreateBudgetkontoViewModels(fixture, budgetkontogruppeViewModelMockCollection, new Random(DateTime.Now.Second), 250).ToList();

            var budgetkontogrupperInUse = budgetkontoViewModelMockCollection
                .GroupBy(m => m.Kontogruppe.Nummer)
                .Select(m => m.Key)
                .ToList();

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetSidsteMåned, Is.EqualTo(0M));
            Assert.That(regnskabViewModel.BudgetSidsteMånedAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetSidsteMånedAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.BudgetSidsteMånedAsText, Is.EqualTo(Convert.ToDecimal(0M).ToString("C")));

            var expectedValue = budgetkontogruppeViewModelMockCollection
                .Where(m => budgetkontogrupperInUse.Contains(m.Nummer))
                .Select(m => m.CreateOpgørelseslinje(regnskabViewModel))
                .Sum(m => m.BudgetSidsteMåned);

            budgetkontogruppeViewModelMockCollection.ForEach(regnskabViewModel.BudgetkontogruppeAdd);
            budgetkontoViewModelMockCollection.ForEach(regnskabViewModel.BudgetkontoAdd);

            Assert.That(regnskabViewModel.BudgetSidsteMåned, Is.EqualTo(expectedValue));
            Assert.That(regnskabViewModel.BudgetSidsteMånedAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetSidsteMånedAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.BudgetSidsteMånedAsText, Is.EqualTo(expectedValue.ToString("C")));
        }

        /// <summary>
        /// Tester, at getteren til BudgetÅrTilDato returnerer budgetteret beløb for år til dato fra årsopgørelsen.
        /// </summary>
        [Test]
        [TestCase(new[] {5, 6, 7, 8, 9})]
        public void TestAtBudgetÅrTilDatoGetterReturnererBudgetÅrTilDato(int[] budgetkontogrupper)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var budgetkontogruppeViewModelMockCollection = new List<IBudgetkontogruppeViewModel>(budgetkontogrupper.Length);
            foreach (var id in budgetkontogrupper)
            {
                var opgørelseViewModelMock = MockRepository.GenerateMock<IOpgørelseViewModel>();
                opgørelseViewModelMock.Expect(m => m.Nummer)
                    .Return(id)
                    .Repeat.Any();
                opgørelseViewModelMock.Expect(m => m.BudgetÅrTilDato)
                    .Return(fixture.Create<decimal>())
                    .Repeat.Any();
                var budgetkontogruppeViewModelMock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                budgetkontogruppeViewModelMock.Expect(m => m.Nummer)
                    .Return(id)
                    .Repeat.Any();
                budgetkontogruppeViewModelMock.Expect(m => m.CreateOpgørelseslinje(Arg<IRegnskabViewModel>.Is.NotNull))
                    .Return(opgørelseViewModelMock)
                    .Repeat.Any();
                budgetkontogruppeViewModelMockCollection.Add(budgetkontogruppeViewModelMock);
            }

            var budgetkontoViewModelMockCollection = CreateBudgetkontoViewModels(fixture, budgetkontogruppeViewModelMockCollection, new Random(DateTime.Now.Second), 250).ToList();

            var budgetkontogrupperInUse = budgetkontoViewModelMockCollection
                .GroupBy(m => m.Kontogruppe.Nummer)
                .Select(m => m.Key)
                .ToList();

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetÅrTilDato, Is.EqualTo(0M));
            Assert.That(regnskabViewModel.BudgetÅrTilDatoAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetÅrTilDatoAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.BudgetÅrTilDatoAsText, Is.EqualTo(Convert.ToDecimal(0M).ToString("C")));

            var expectedValue = budgetkontogruppeViewModelMockCollection
                .Where(m => budgetkontogrupperInUse.Contains(m.Nummer))
                .Select(m => m.CreateOpgørelseslinje(regnskabViewModel))
                .Sum(m => m.BudgetÅrTilDato);

            budgetkontogruppeViewModelMockCollection.ForEach(regnskabViewModel.BudgetkontogruppeAdd);
            budgetkontoViewModelMockCollection.ForEach(regnskabViewModel.BudgetkontoAdd);

            Assert.That(regnskabViewModel.BudgetÅrTilDato, Is.EqualTo(expectedValue));
            Assert.That(regnskabViewModel.BudgetÅrTilDatoAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetÅrTilDatoAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.BudgetÅrTilDatoAsText, Is.EqualTo(expectedValue.ToString("C")));
        }

        /// <summary>
        /// Tester, at getteren til BudgetSidsteÅr returnerer budgetteret beløb for sidste år fra årsopgørelsen.
        /// </summary>
        [Test]
        [TestCase(new[] {5, 6, 7, 8, 9})]
        public void TestAtBudgetSidsteÅrGetterReturnererBudgetSidsteÅr(int[] budgetkontogrupper)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var budgetkontogruppeViewModelMockCollection = new List<IBudgetkontogruppeViewModel>(budgetkontogrupper.Length);
            foreach (var id in budgetkontogrupper)
            {
                var opgørelseViewModelMock = MockRepository.GenerateMock<IOpgørelseViewModel>();
                opgørelseViewModelMock.Expect(m => m.Nummer)
                    .Return(id)
                    .Repeat.Any();
                opgørelseViewModelMock.Expect(m => m.BudgetSidsteÅr)
                    .Return(fixture.Create<decimal>())
                    .Repeat.Any();
                var budgetkontogruppeViewModelMock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                budgetkontogruppeViewModelMock.Expect(m => m.Nummer)
                    .Return(id)
                    .Repeat.Any();
                budgetkontogruppeViewModelMock.Expect(m => m.CreateOpgørelseslinje(Arg<IRegnskabViewModel>.Is.NotNull))
                    .Return(opgørelseViewModelMock)
                    .Repeat.Any();
                budgetkontogruppeViewModelMockCollection.Add(budgetkontogruppeViewModelMock);
            }

            var budgetkontoViewModelMockCollection = CreateBudgetkontoViewModels(fixture, budgetkontogruppeViewModelMockCollection, new Random(DateTime.Now.Second), 250).ToList();

            var budgetkontogrupperInUse = budgetkontoViewModelMockCollection
                .GroupBy(m => m.Kontogruppe.Nummer)
                .Select(m => m.Key)
                .ToList();

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetSidsteÅr, Is.EqualTo(0M));
            Assert.That(regnskabViewModel.BudgetSidsteÅrAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetSidsteÅrAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.BudgetSidsteÅrAsText, Is.EqualTo(Convert.ToDecimal(0M).ToString("C")));

            var expectedValue = budgetkontogruppeViewModelMockCollection
                .Where(m => budgetkontogrupperInUse.Contains(m.Nummer))
                .Select(m => m.CreateOpgørelseslinje(regnskabViewModel))
                .Sum(m => m.BudgetSidsteÅr);

            budgetkontogruppeViewModelMockCollection.ForEach(regnskabViewModel.BudgetkontogruppeAdd);
            budgetkontoViewModelMockCollection.ForEach(regnskabViewModel.BudgetkontoAdd);

            Assert.That(regnskabViewModel.BudgetSidsteÅr, Is.EqualTo(expectedValue));
            Assert.That(regnskabViewModel.BudgetSidsteÅrAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetSidsteÅrAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.BudgetSidsteÅrAsText, Is.EqualTo(expectedValue.ToString("C")));
        }

        /// <summary>
        /// Tester, at getteren til Bogført returnerer bogført beløb fra årsopgørelsen.
        /// </summary>
        [Test]
        [TestCase(new[] {5, 6, 7, 8, 9})]
        public void TestAtBogførtGetterReturnererBogført(int[] budgetkontogrupper)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var budgetkontogruppeViewModelMockCollection = new List<IBudgetkontogruppeViewModel>(budgetkontogrupper.Length);
            foreach (var id in budgetkontogrupper)
            {
                var opgørelseViewModelMock = MockRepository.GenerateMock<IOpgørelseViewModel>();
                opgørelseViewModelMock.Expect(m => m.Nummer)
                    .Return(id)
                    .Repeat.Any();
                opgørelseViewModelMock.Expect(m => m.Bogført)
                    .Return(fixture.Create<decimal>())
                    .Repeat.Any();
                var budgetkontogruppeViewModelMock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                budgetkontogruppeViewModelMock.Expect(m => m.Nummer)
                    .Return(id)
                    .Repeat.Any();
                budgetkontogruppeViewModelMock.Expect(m => m.CreateOpgørelseslinje(Arg<IRegnskabViewModel>.Is.NotNull))
                    .Return(opgørelseViewModelMock)
                    .Repeat.Any();
                budgetkontogruppeViewModelMockCollection.Add(budgetkontogruppeViewModelMock);
            }

            var budgetkontoViewModelMockCollection = CreateBudgetkontoViewModels(fixture, budgetkontogruppeViewModelMockCollection, new Random(DateTime.Now.Second), 250).ToList();

            var budgetkontogrupperInUse = budgetkontoViewModelMockCollection
                .GroupBy(m => m.Kontogruppe.Nummer)
                .Select(m => m.Key)
                .ToList();

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogført, Is.EqualTo(0M));
            Assert.That(regnskabViewModel.BogførtAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.BogførtAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.BogførtAsText, Is.EqualTo(Convert.ToDecimal(0M).ToString("C")));

            var expectedValue = budgetkontogruppeViewModelMockCollection
                .Where(m => budgetkontogrupperInUse.Contains(m.Nummer))
                .Select(m => m.CreateOpgørelseslinje(regnskabViewModel))
                .Sum(m => m.Bogført);

            budgetkontogruppeViewModelMockCollection.ForEach(regnskabViewModel.BudgetkontogruppeAdd);
            budgetkontoViewModelMockCollection.ForEach(regnskabViewModel.BudgetkontoAdd);

            Assert.That(regnskabViewModel.Bogført, Is.EqualTo(expectedValue));
            Assert.That(regnskabViewModel.BogførtAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.BogførtAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.BogførtAsText, Is.EqualTo(expectedValue.ToString("C")));
        }

        /// <summary>
        /// Tester, at getteren til BogførtSidsteMåned returnerer bogført beløb for sidste måned fra årsopgørelsen.
        /// </summary>
        [Test]
        [TestCase(new[] {5, 6, 7, 8, 9})]
        public void TestAtBogførtSidsteMånedGetterReturnererBogførtSidsteMåned(int[] budgetkontogrupper)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var budgetkontogruppeViewModelMockCollection = new List<IBudgetkontogruppeViewModel>(budgetkontogrupper.Length);
            foreach (var id in budgetkontogrupper)
            {
                var opgørelseViewModelMock = MockRepository.GenerateMock<IOpgørelseViewModel>();
                opgørelseViewModelMock.Expect(m => m.Nummer)
                    .Return(id)
                    .Repeat.Any();
                opgørelseViewModelMock.Expect(m => m.BogførtSidsteMåned)
                    .Return(fixture.Create<decimal>())
                    .Repeat.Any();
                var budgetkontogruppeViewModelMock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                budgetkontogruppeViewModelMock.Expect(m => m.Nummer)
                    .Return(id)
                    .Repeat.Any();
                budgetkontogruppeViewModelMock.Expect(m => m.CreateOpgørelseslinje(Arg<IRegnskabViewModel>.Is.NotNull))
                    .Return(opgørelseViewModelMock)
                    .Repeat.Any();
                budgetkontogruppeViewModelMockCollection.Add(budgetkontogruppeViewModelMock);
            }

            var budgetkontoViewModelMockCollection = CreateBudgetkontoViewModels(fixture, budgetkontogruppeViewModelMockCollection, new Random(DateTime.Now.Second), 250).ToList();

            var budgetkontogrupperInUse = budgetkontoViewModelMockCollection
                .GroupBy(m => m.Kontogruppe.Nummer)
                .Select(m => m.Key)
                .ToList();

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.BogførtSidsteMåned, Is.EqualTo(0M));
            Assert.That(regnskabViewModel.BogførtSidsteMånedAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.BogførtSidsteMånedAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.BogførtSidsteMånedAsText, Is.EqualTo(Convert.ToDecimal(0M).ToString("C")));

            var expectedValue = budgetkontogruppeViewModelMockCollection
                .Where(m => budgetkontogrupperInUse.Contains(m.Nummer))
                .Select(m => m.CreateOpgørelseslinje(regnskabViewModel))
                .Sum(m => m.BogførtSidsteMåned);

            budgetkontogruppeViewModelMockCollection.ForEach(regnskabViewModel.BudgetkontogruppeAdd);
            budgetkontoViewModelMockCollection.ForEach(regnskabViewModel.BudgetkontoAdd);

            Assert.That(regnskabViewModel.BogførtSidsteMåned, Is.EqualTo(expectedValue));
            Assert.That(regnskabViewModel.BogførtSidsteMånedAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.BogførtSidsteMånedAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.BogførtSidsteMånedAsText, Is.EqualTo(expectedValue.ToString("C")));
        }

        /// <summary>
        /// Tester, at getteren til BogførtÅrTilDato returnerer bogført beløb for år til dato fra årsopgørelsen.
        /// </summary>
        [Test]
        [TestCase(new[] {5, 6, 7, 8, 9})]
        public void TestAtBogførtÅrTilDatoGetterReturnererBogførtÅrTilDato(int[] budgetkontogrupper)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var budgetkontogruppeViewModelMockCollection = new List<IBudgetkontogruppeViewModel>(budgetkontogrupper.Length);
            foreach (var id in budgetkontogrupper)
            {
                var opgørelseViewModelMock = MockRepository.GenerateMock<IOpgørelseViewModel>();
                opgørelseViewModelMock.Expect(m => m.Nummer)
                    .Return(id)
                    .Repeat.Any();
                opgørelseViewModelMock.Expect(m => m.BogførtÅrTilDato)
                    .Return(fixture.Create<decimal>())
                    .Repeat.Any();
                var budgetkontogruppeViewModelMock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                budgetkontogruppeViewModelMock.Expect(m => m.Nummer)
                    .Return(id)
                    .Repeat.Any();
                budgetkontogruppeViewModelMock.Expect(m => m.CreateOpgørelseslinje(Arg<IRegnskabViewModel>.Is.NotNull))
                    .Return(opgørelseViewModelMock)
                    .Repeat.Any();
                budgetkontogruppeViewModelMockCollection.Add(budgetkontogruppeViewModelMock);
            }

            var budgetkontoViewModelMockCollection = CreateBudgetkontoViewModels(fixture, budgetkontogruppeViewModelMockCollection, new Random(DateTime.Now.Second), 250).ToList();

            var budgetkontogrupperInUse = budgetkontoViewModelMockCollection
                .GroupBy(m => m.Kontogruppe.Nummer)
                .Select(m => m.Key)
                .ToList();

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.BogførtÅrTilDato, Is.EqualTo(0M));
            Assert.That(regnskabViewModel.BogførtÅrTilDatoAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.BogførtÅrTilDatoAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.BogførtÅrTilDatoAsText, Is.EqualTo(Convert.ToDecimal(0M).ToString("C")));

            var expectedValue = budgetkontogruppeViewModelMockCollection
                .Where(m => budgetkontogrupperInUse.Contains(m.Nummer))
                .Select(m => m.CreateOpgørelseslinje(regnskabViewModel))
                .Sum(m => m.BogførtÅrTilDato);

            budgetkontogruppeViewModelMockCollection.ForEach(regnskabViewModel.BudgetkontogruppeAdd);
            budgetkontoViewModelMockCollection.ForEach(regnskabViewModel.BudgetkontoAdd);

            Assert.That(regnskabViewModel.BogførtÅrTilDato, Is.EqualTo(expectedValue));
            Assert.That(regnskabViewModel.BogførtÅrTilDatoAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.BogførtÅrTilDatoAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.BogførtÅrTilDatoAsText, Is.EqualTo(expectedValue.ToString("C")));
        }

        /// <summary>
        /// Tester, at getteren til BogførtSidsteÅr returnerer bogført beløb for sidste år fra årsopgørelsen.
        /// </summary>
        [Test]
        [TestCase(new[] {5, 6, 7, 8, 9})]
        public void TestAtBogførtSidsteÅrGetterReturnererBogførtSidsteÅr(int[] budgetkontogrupper)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var budgetkontogruppeViewModelMockCollection = new List<IBudgetkontogruppeViewModel>(budgetkontogrupper.Length);
            foreach (var id in budgetkontogrupper)
            {
                var opgørelseViewModelMock = MockRepository.GenerateMock<IOpgørelseViewModel>();
                opgørelseViewModelMock.Expect(m => m.Nummer)
                    .Return(id)
                    .Repeat.Any();
                opgørelseViewModelMock.Expect(m => m.BogførtSidsteÅr)
                    .Return(fixture.Create<decimal>())
                    .Repeat.Any();
                var budgetkontogruppeViewModelMock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                budgetkontogruppeViewModelMock.Expect(m => m.Nummer)
                    .Return(id)
                    .Repeat.Any();
                budgetkontogruppeViewModelMock.Expect(m => m.CreateOpgørelseslinje(Arg<IRegnskabViewModel>.Is.NotNull))
                    .Return(opgørelseViewModelMock)
                    .Repeat.Any();
                budgetkontogruppeViewModelMockCollection.Add(budgetkontogruppeViewModelMock);
            }

            var budgetkontoViewModelMockCollection = CreateBudgetkontoViewModels(fixture, budgetkontogruppeViewModelMockCollection, new Random(DateTime.Now.Second), 250).ToList();

            var budgetkontogrupperInUse = budgetkontoViewModelMockCollection
                .GroupBy(m => m.Kontogruppe.Nummer)
                .Select(m => m.Key)
                .ToList();

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.BogførtSidsteÅr, Is.EqualTo(0M));
            Assert.That(regnskabViewModel.BogførtSidsteÅrAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.BogførtSidsteÅrAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.BogførtSidsteÅrAsText, Is.EqualTo(Convert.ToDecimal(0M).ToString("C")));

            var expectedValue = budgetkontogruppeViewModelMockCollection
                .Where(m => budgetkontogrupperInUse.Contains(m.Nummer))
                .Select(m => m.CreateOpgørelseslinje(regnskabViewModel))
                .Sum(m => m.BogførtSidsteÅr);

            budgetkontogruppeViewModelMockCollection.ForEach(regnskabViewModel.BudgetkontogruppeAdd);
            budgetkontoViewModelMockCollection.ForEach(regnskabViewModel.BudgetkontoAdd);

            Assert.That(regnskabViewModel.BogførtSidsteÅr, Is.EqualTo(expectedValue));
            Assert.That(regnskabViewModel.BogførtSidsteÅrAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.BogførtSidsteÅrAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.BogførtSidsteÅrAsText, Is.EqualTo(expectedValue.ToString("C")));
        }

        /// <summary>
        /// Tester, at KontoAdd kaster en ArgumentNullException, hvis ViewModel for kontoen er null.
        /// </summary>
        [Test]
        public void TestAtKontoAddKasterArgumentNullExceptionHvisKontoViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => regnskabViewModel.KontoAdd(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("kontoViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at KontoAdd tilføjer en konto til regnskabet.
        /// </summary>
        [Test]
        [TestCase("Aktiver")]
        [TestCase("Passiver")]
        public void TestAtKontoAddAddsKontoViewModel(string balanceType)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IBogføringViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringViewModel>()));
            fixture.Customize<IBalanceViewModel>(e => e.FromFactory(() =>
            {
                var mock = MockRepository.GenerateMock<IBalanceViewModel>();
                mock.Expect(m => m.Nummer)
                    .Return(fixture.Create<int>())
                    .Repeat.Any();
                mock.Expect(m => m.Balancetype)
                    .Return((Balancetype) Enum.Parse(typeof (Balancetype), balanceType))
                    .Repeat.Any();
                return mock;
            }));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() =>
            {
                var balanceViewModelMock = fixture.Create<IBalanceViewModel>();
                var mock = MockRepository.GenerateMock<IKontogruppeViewModel>();
                mock.Expect(m => m.Nummer)
                    .Return(balanceViewModelMock.Nummer)
                    .Repeat.Any();
                mock.Expect(m => m.CreateBalancelinje(Arg<IRegnskabViewModel>.Is.NotNull))
                    .Return(balanceViewModelMock)
                    .Repeat.Any();
                return mock;
            }));
            fixture.Customize<IKontoViewModel>(e => e.FromFactory(() =>
            {
                var mock = MockRepository.GenerateMock<IKontoViewModel>();
                mock.Expect(m => m.Kontonummer)
                    .Return(fixture.Create<string>())
                    .Repeat.Any();
                mock.Expect(m => m.Kontogruppe)
                    .Return(fixture.Create<IKontogruppeViewModel>())
                    .Repeat.Any();
                mock.Expect(m => m.Kontoværdi)
                    .Return(fixture.Create<decimal>())
                    .Repeat.Any();
                return mock;
            }));

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Konti, Is.Not.Null);
            Assert.That(regnskabViewModel.Konti, Is.Empty);
            Assert.That(regnskabViewModel.KontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.KontiTop, Is.Empty);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            regnskabViewModel.BogføringSet(fixture.Create<IBogføringViewModel>());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            Action action = () =>
            {
                regnskabViewModel.KontoAdd(fixture.Create<IKontoViewModel>());
                Assert.That(regnskabViewModel.BogføringSetCommand, Is.Not.Null);
                Assert.That(regnskabViewModel.BogføringSetCommand.ExecuteTask, Is.Null);
            };
            Task.Run(action).Wait(3000);

            Assert.That(regnskabViewModel.Konti, Is.Not.Null);
            Assert.That(regnskabViewModel.Konti, Is.Not.Empty);
            Assert.That(regnskabViewModel.KontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.KontiTop, Is.Not.Empty);

            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at KontoAdd udfører kommandoen, der kan sætte ViewModel til bogføring, hvis ViewModel til bogføring er null.
        /// </summary>
        [Test]
        [TestCase("Aktiver")]
        [TestCase("Passiver")]
        public void TestAtKontoAddExecutesBogføringSetCommandHvisBogføringErNull(string balanceType)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IBogføringslinjeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringslinjeModel>()));
            fixture.Customize<IBalanceViewModel>(e => e.FromFactory(() =>
            {
                var mock = MockRepository.GenerateMock<IBalanceViewModel>();
                mock.Expect(m => m.Nummer)
                    .Return(fixture.Create<int>())
                    .Repeat.Any();
                mock.Expect(m => m.Balancetype)
                    .Return((Balancetype)Enum.Parse(typeof(Balancetype), balanceType))
                    .Repeat.Any();
                return mock;
            }));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() =>
            {
                var balanceViewModelMock = fixture.Create<IBalanceViewModel>();
                var mock = MockRepository.GenerateMock<IKontogruppeViewModel>();
                mock.Expect(m => m.Nummer)
                    .Return(balanceViewModelMock.Nummer)
                    .Repeat.Any();
                mock.Expect(m => m.CreateBalancelinje(Arg<IRegnskabViewModel>.Is.NotNull))
                    .Return(balanceViewModelMock)
                    .Repeat.Any();
                return mock;
            }));

            var regnskabsnummer = fixture.Create<int>();
            var regnskabModelMock = MockRepository.GenerateMock<IRegnskabModel>();
            regnskabModelMock.Expect(m => m.Nummer)
                .Return(regnskabsnummer)
                .Repeat.Any();

            var kontonummer = fixture.Create<string>();
            var kontoViewModelMock = MockRepository.GenerateMock<IKontoViewModel>();
            kontoViewModelMock.Expect(m => m.Kontonummer)
                .Return(kontonummer)
                .Repeat.Any();
            kontoViewModelMock.Expect(m => m.Kontogruppe)
                .Return(fixture.Create<IKontogruppeViewModel>())
                .Repeat.Any();

            Func<IBogføringslinjeModel> getter = fixture.Create<IBogføringslinjeModel>;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.BogføringslinjeCreateNewAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.NotNull))
                .Return(Task.Run(getter))
                .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var regnskabViewModel = new RegnskabViewModel(regnskabModelMock, fixture.Create<DateTime>(), finansstyringRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            Action action = () =>
            {
                regnskabViewModel.KontoAdd(kontoViewModelMock);
                Assert.That(regnskabViewModel.BogføringSetCommand, Is.Not.Null);
                Assert.That(regnskabViewModel.BogføringSetCommand.ExecuteTask, Is.Not.Null);
                regnskabViewModel.BogføringSetCommand.ExecuteTask.Wait();
            };
            Task.Run(action).Wait(3000);

            finansstyringRepositoryMock.AssertWasCalled(m => m.BogføringslinjeCreateNewAsync(Arg<int>.Is.Equal(regnskabsnummer), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.Equal(kontonummer)));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at KontoAdd tilføjer en balancelinje til regnskabet.
        /// </summary>
        [Test]
        [TestCase("Aktiver")]
        [TestCase("Passiver")]
        public void TestAtKontoAddAddsBalanceViewModel(string balanceType)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IBogføringViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringViewModel>()));
            fixture.Customize<IBalanceViewModel>(e => e.FromFactory(() =>
            {
                var mock = MockRepository.GenerateMock<IBalanceViewModel>();
                mock.Expect(m => m.Nummer)
                    .Return(fixture.Create<int>())
                    .Repeat.Any();
                mock.Expect(m => m.Balancetype)
                    .Return((Balancetype) Enum.Parse(typeof (Balancetype), balanceType))
                    .Repeat.Any();
                return mock;
            }));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() =>
            {
                var balanceViewModelMock = fixture.Create<IBalanceViewModel>();
                var mock = MockRepository.GenerateMock<IKontogruppeViewModel>();
                mock.Expect(m => m.Nummer)
                    .Return(balanceViewModelMock.Nummer)
                    .Repeat.Any();
                mock.Expect(m => m.CreateBalancelinje(Arg<IRegnskabViewModel>.Is.NotNull))
                    .Return(balanceViewModelMock)
                    .Repeat.Any();
                return mock;
            }));
            fixture.Customize<IKontoViewModel>(e => e.FromFactory(() =>
            {
                var mock = MockRepository.GenerateMock<IKontoViewModel>();
                mock.Expect(m => m.Kontonummer)
                    .Return(fixture.Create<string>())
                    .Repeat.Any();
                mock.Expect(m => m.Kontogruppe)
                    .Return(fixture.Create<IKontogruppeViewModel>())
                    .Repeat.Any();
                mock.Expect(m => m.Kontoværdi)
                    .Return(fixture.Create<decimal>())
                    .Repeat.Any();
                return mock;
            }));

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Aktiver, Is.Not.Null);
            Assert.That(regnskabViewModel.Aktiver, Is.Empty);
            Assert.That(regnskabViewModel.Passiver, Is.Not.Null);
            Assert.That(regnskabViewModel.Passiver, Is.Empty);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            regnskabViewModel.BogføringSet(fixture.Create<IBogføringViewModel>());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            Action action = () =>
            {
                regnskabViewModel.KontoAdd(fixture.Create<IKontoViewModel>());
                Assert.That(regnskabViewModel.BogføringSetCommand, Is.Not.Null);
                Assert.That(regnskabViewModel.BogføringSetCommand.ExecuteTask, Is.Null);
            };
            Task.Run(action).Wait(3000);

            Assert.That(regnskabViewModel.Aktiver, Is.Not.Null);
            Assert.That(regnskabViewModel.Passiver, Is.Not.Null);
            switch (balanceType)
            {
                case "Aktiver":
                    Assert.That(regnskabViewModel.Aktiver, Is.Not.Empty);
                    Assert.That(regnskabViewModel.Passiver, Is.Empty);
                    break;

                case "Passiver":
                    Assert.That(regnskabViewModel.Aktiver, Is.Empty);
                    Assert.That(regnskabViewModel.Passiver, Is.Not.Empty);
                    break;

                default:
                    Assert.Fail("Ukendt balancetype: {0}", balanceType);
                    break;
            }

            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at KontoAdd registreret kontoen til brug i balancen til regnskabet.
        /// </summary>
        [Test]
        [TestCase("Aktiver")]
        [TestCase("Passiver")]
        public void TestAtKontoAddKalderRegisterOnBalanceViewModel(string balanceType)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IBogføringViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringViewModel>()));

            var balanceViewModelMock = MockRepository.GenerateMock<IBalanceViewModel>();
            balanceViewModelMock.Expect(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();
            balanceViewModelMock.Expect(m => m.Balancetype)
                .Return((Balancetype) Enum.Parse(typeof (Balancetype), balanceType))
                .Repeat.Any();

            var kontogruppeViewModelMock = MockRepository.GenerateMock<IKontogruppeViewModel>();
            kontogruppeViewModelMock.Expect(m => m.Nummer)
                .Return(balanceViewModelMock.Nummer)
                .Repeat.Any();
            kontogruppeViewModelMock.Expect(m => m.CreateBalancelinje(Arg<IRegnskabViewModel>.Is.NotNull))
                .Return(balanceViewModelMock)
                .Repeat.Any();

            var kontoViewModelMock = MockRepository.GenerateMock<IKontoViewModel>();
            kontoViewModelMock.Expect(m => m.Kontonummer)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            kontoViewModelMock.Expect(m => m.Kontogruppe)
                .Return(kontogruppeViewModelMock)
                .Repeat.Any();
            kontoViewModelMock.Expect(m => m.Kontoværdi)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Aktiver, Is.Not.Null);
            Assert.That(regnskabViewModel.Aktiver, Is.Empty);
            Assert.That(regnskabViewModel.Passiver, Is.Not.Null);
            Assert.That(regnskabViewModel.Passiver, Is.Empty);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            regnskabViewModel.BogføringSet(fixture.Create<IBogføringViewModel>());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            Action action = () =>
            {
                regnskabViewModel.KontoAdd(kontoViewModelMock);
                Assert.That(regnskabViewModel.BogføringSetCommand, Is.Not.Null);
                Assert.That(regnskabViewModel.BogføringSetCommand.ExecuteTask, Is.Null);
            };
            Task.Run(action).Wait(3000);

            balanceViewModelMock.AssertWasCalled(m => m.Register(Arg<KontoViewModel>.Is.Equal(kontoViewModelMock)));
        }

        /// <summary>
        /// Tester, at KontoAdd rejser PropertyChanged, når en konto tilføjes regnskabet.
        /// </summary>
        [Test]
        [TestCase("Konti", "Aktiver")]
        [TestCase("KontiGrouped", "Aktiver")]
        [TestCase("KontiTop", "Aktiver")]
        [TestCase("KontiTopGrouped", "Aktiver")]
        [TestCase("Aktiver", "Aktiver")]
        [TestCase("AktiverIAlt", "Aktiver")]
        [TestCase("AktiverIAltAsText", "Aktiver")]
        [TestCase("Konti", "Passiver")]
        [TestCase("KontiGrouped", "Passiver")]
        [TestCase("KontiTop", "Passiver")]
        [TestCase("KontiTopGrouped", "Passiver")]
        [TestCase("Passiver", "Passiver")]
        [TestCase("PassiverIAlt", "Passiver")]
        [TestCase("PassiverIAltAsText", "Passiver")]
        public void TestAtKontoAddRejserPropertyChangedVedAddAfKontoViewModel(string expectedPropertyName, string balanceType)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IBogføringViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringViewModel>()));
            fixture.Customize<IBalanceViewModel>(e => e.FromFactory(() =>
            {
                var mock = MockRepository.GenerateMock<IBalanceViewModel>();
                mock.Expect(m => m.Nummer)
                    .Return(fixture.Create<int>())
                    .Repeat.Any();
                mock.Expect(m => m.Balancetype)
                    .Return((Balancetype)Enum.Parse(typeof(Balancetype), balanceType))
                    .Repeat.Any();
                return mock;
            }));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() =>
            {
                var balanceViewModelMock = fixture.Create<IBalanceViewModel>();
                var mock = MockRepository.GenerateMock<IKontogruppeViewModel>();
                mock.Expect(m => m.Nummer)
                    .Return(balanceViewModelMock.Nummer)
                    .Repeat.Any();
                mock.Expect(m => m.CreateBalancelinje(Arg<IRegnskabViewModel>.Is.NotNull))
                    .Return(balanceViewModelMock)
                    .Repeat.Any();
                return mock;
            }));
            fixture.Customize<IKontoViewModel>(e => e.FromFactory(() =>
            {
                var mock = MockRepository.GenerateMock<IKontoViewModel>();
                mock.Expect(m => m.Kontogruppe)
                    .Return(fixture.Create<IKontogruppeViewModel>())
                    .Repeat.Any();
                return mock;
            }));

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            regnskabViewModel.BogføringSet(fixture.Create<IBogføringViewModel>());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Empty);
                if (string.Compare(e.PropertyName, expectedPropertyName, StringComparison.Ordinal) == 0)
                {
                    eventCalled = true;
                }
            };

            Assert.That(eventCalled, Is.False);
            Action action = () =>
            {
                regnskabViewModel.KontoAdd(fixture.Create<IKontoViewModel>());
                Assert.That(regnskabViewModel.BogføringSetCommand, Is.Not.Null);
                Assert.That(regnskabViewModel.BogføringSetCommand.ExecuteTask, Is.Null);
            };
            Task.Run(action).Wait(3000);
            Assert.That(eventCalled, Is.True);

            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at BudgetkontoAdd kaster en ArgumentNullException, hvis ViewModel for budgetkontoen er null.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoAddKasterArgumentNullExceptionHvisBudgetkontoViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => regnskabViewModel.BudgetkontoAdd(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("budgetkontoViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at BudgetkontoAdd tilføjer en budgetkonto til regnskabet.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoAddAddsBudgetkontoViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IOpgørelseViewModel>(e => e.FromFactory(() =>
            {
                var mock = MockRepository.GenerateMock<IOpgørelseViewModel>();
                mock.Expect(m => m.Nummer)
                    .Return(fixture.Create<int>())
                    .Repeat.Any();
                return mock;
            }));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() =>
            {
                var opgørelseViewModelMock = fixture.Create<IOpgørelseViewModel>();
                var mock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                mock.Expect(m => m.Nummer)
                    .Return(opgørelseViewModelMock.Nummer)
                    .Repeat.Any();
                mock.Expect(m => m.CreateOpgørelseslinje(Arg<IRegnskabViewModel>.Is.NotNull))
                    .Return(opgørelseViewModelMock)
                    .Repeat.Any();
                return mock;
            }));
            fixture.Customize<IBudgetkontoViewModel>(e => e.FromFactory(() =>
            {
                var mock = MockRepository.GenerateMock<IBudgetkontoViewModel>();
                mock.Expect(m => m.Kontonummer)
                    .Return(fixture.Create<string>())
                    .Repeat.Any();
                mock.Expect(m => m.Kontogruppe)
                    .Return(fixture.Create<IBudgetkontogruppeViewModel>())
                    .Repeat.Any();
                mock.Expect(m => m.Kontoværdi)
                    .Return(fixture.Create<decimal>())
                    .Repeat.Any();
                return mock;
            }));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Empty);
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Empty);

            regnskabViewModel.BudgetkontoAdd(fixture.Create<IBudgetkontoViewModel>());
            Assert.That(regnskabViewModel.Budgetkonti, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Not.Empty);
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Not.Empty);
        }

        /// <summary>
        /// Tester, at BudgetkontoAdd tilføjer en opgørelseslinje til regnskabet.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoAddAddsOpgørelseViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IOpgørelseViewModel>(e => e.FromFactory(() =>
            {
                var mock = MockRepository.GenerateMock<IOpgørelseViewModel>();
                mock.Expect(m => m.Nummer)
                    .Return(fixture.Create<int>())
                    .Repeat.Any();
                return mock;
            }));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() =>
            {
                var opgørelseViewModelMock = fixture.Create<IOpgørelseViewModel>();
                var mock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                mock.Expect(m => m.Nummer)
                    .Return(opgørelseViewModelMock.Nummer)
                    .Repeat.Any();
                mock.Expect(m => m.CreateOpgørelseslinje(Arg<IRegnskabViewModel>.Is.NotNull))
                    .Return(opgørelseViewModelMock)
                    .Repeat.Any();
                return mock;
            }));
            fixture.Customize<IBudgetkontoViewModel>(e => e.FromFactory(() =>
            {
                var mock = MockRepository.GenerateMock<IBudgetkontoViewModel>();
                mock.Expect(m => m.Kontogruppe)
                    .Return(fixture.Create<IBudgetkontogruppeViewModel>())
                    .Repeat.Any();
                return mock;
            }));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Opgørelseslinjer, Is.Not.Null);
            Assert.That(regnskabViewModel.Opgørelseslinjer, Is.Empty);

            regnskabViewModel.BudgetkontoAdd(fixture.Create<IBudgetkontoViewModel>());
            Assert.That(regnskabViewModel.Opgørelseslinjer, Is.Not.Null);
            Assert.That(regnskabViewModel.Opgørelseslinjer, Is.Not.Empty);
        }

        /// <summary>
        /// Tester, at BudgetkontoAdd registreret budgetkontoen til brug i opgørelseslinjen til regnskabet.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoAddKalderRegisterOnOpgørelseViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var opgørelseViewModelMock = MockRepository.GenerateMock<IOpgørelseViewModel>();
            opgørelseViewModelMock.Expect(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();

            var budgetkontogruppeViewModelMock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
            budgetkontogruppeViewModelMock.Expect(m => m.Nummer)
                .Return(opgørelseViewModelMock.Nummer)
                .Repeat.Any();
            budgetkontogruppeViewModelMock.Expect(m => m.CreateOpgørelseslinje(Arg<IRegnskabViewModel>.Is.NotNull))
                .Return(opgørelseViewModelMock)
                .Repeat.Any();

            var budgetkontoViewModelMock = MockRepository.GenerateMock<IBudgetkontoViewModel>();
            budgetkontoViewModelMock.Expect(m => m.Kontogruppe)
                .Return(budgetkontogruppeViewModelMock)
                .Repeat.Any();

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            regnskabViewModel.BudgetkontoAdd(budgetkontoViewModelMock);

            opgørelseViewModelMock.AssertWasCalled(m => m.Register(Arg<IBudgetkontoViewModel>.Is.Equal(budgetkontoViewModelMock)));
        }

        /// <summary>
        /// Tester, at BudgetkontoAdd rejser PropertyChanged, når en budgetkonto tilføjes regnskabet.
        /// </summary>
        [Test]
        [TestCase("Budgetkonti")]
        [TestCase("BudgetkontiGrouped")]
        [TestCase("BudgetkontiTop")]
        [TestCase("BudgetkontiTopGrouped")]
        [TestCase("Opgørelseslinjer")]
        [TestCase("Budget")]
        [TestCase("BudgetAsText")]
        [TestCase("BudgetSidsteMåned")]
        [TestCase("BudgetSidsteMånedAsText")]
        [TestCase("BudgetÅrTilDato")]
        [TestCase("BudgetÅrTilDatoAsText")]
        [TestCase("BudgetSidsteÅr")]
        [TestCase("BudgetSidsteÅrAsText")]
        [TestCase("Bogført")]
        [TestCase("BogførtAsText")]
        [TestCase("BogførtSidsteMåned")]
        [TestCase("BogførtSidsteMånedAsText")]
        [TestCase("BogførtÅrTilDato")]
        [TestCase("BogførtÅrTilDatoAsText")]
        [TestCase("BogførtSidsteÅr")]
        [TestCase("BogførtSidsteÅrAsText")]
        public void TestAtBudgetkontoAddRejserPropertyChangedVedAddAfBudgetkontoViewModel(string expectedPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IOpgørelseViewModel>(e => e.FromFactory(() =>
            {
                var mock = MockRepository.GenerateMock<IOpgørelseViewModel>();
                mock.Expect(m => m.Nummer)
                    .Return(fixture.Create<int>())
                    .Repeat.Any();
                return mock;
            }));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() =>
            {
                var opgørelseViewModelMock = fixture.Create<IOpgørelseViewModel>();
                var mock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                mock.Expect(m => m.Nummer)
                    .Return(opgørelseViewModelMock.Nummer)
                    .Repeat.Any();
                mock.Expect(m => m.CreateOpgørelseslinje(Arg<IRegnskabViewModel>.Is.NotNull))
                    .Return(opgørelseViewModelMock)
                    .Repeat.Any();
                return mock;
            }));
            fixture.Customize<IBudgetkontoViewModel>(e => e.FromFactory(() =>
            {
                var mock = MockRepository.GenerateMock<IBudgetkontoViewModel>();
                mock.Expect(m => m.Kontogruppe)
                    .Return(fixture.Create<IBudgetkontogruppeViewModel>())
                    .Repeat.Any();
                return mock;
            }));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, expectedPropertyName, StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            regnskabViewModel.BudgetkontoAdd(fixture.Create<IBudgetkontoViewModel>());
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at BogføringslinjeAdd kaster en ArgumentNullException, hvis ViewModel for bogføringslinjen er null.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjeAddKasterArgumentNullExceptionHvisBogføringslinjeViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => regnskabViewModel.BogføringslinjeAdd(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("bogføringslinjeViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at BogføringslinjeAdd tilføjer en bogføringslinje til regnskabet.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjeAddAddsBogføringslinjeViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IReadOnlyBogføringslinjeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>()));
            fixture.Customize<IBogføringViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringViewModel>()));

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføringslinjer, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføringslinjer, Is.Empty);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            regnskabViewModel.BogføringSet(fixture.Create<IBogføringViewModel>());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            var bogføringslinjeViewModelMock = fixture.Create<IReadOnlyBogføringslinjeViewModel>();
            Action action = () =>
                {
                    regnskabViewModel.BogføringslinjeAdd(bogføringslinjeViewModelMock);
                    Assert.That(regnskabViewModel.BogføringSetCommand, Is.Not.Null);
                    Assert.That(regnskabViewModel.BogføringSetCommand.ExecuteTask, Is.Null);
                };
            Task.Run(action).Wait();
            
            Assert.That(regnskabViewModel.Bogføringslinjer, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføringslinjer, Is.Not.Empty);

            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at BogføringslinjeAdd udfører kommandoen, der kan sætte ViewModel til bogføring, hvis ViewModel til bogføring er null.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjeAddExecutesBogføringSetCommandHvisBogføringErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IBogføringslinjeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringslinjeModel>()));

            var regnskabsnummer = fixture.Create<int>();
            var regnskabModelMock = MockRepository.GenerateMock<IRegnskabModel>();
            regnskabModelMock.Expect(m => m.Nummer)
                             .Return(regnskabsnummer)
                             .Repeat.Any();

            var kontonummer = fixture.Create<string>();
            var bogføringslinjeViewModelMock = MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>();
            bogføringslinjeViewModelMock.Expect(m => m.Løbenummer)
                                        .Return(fixture.Create<int>())
                                        .Repeat.Any();
            bogføringslinjeViewModelMock.Expect(m => m.Dato)
                                        .Return(fixture.Create<DateTime>())
                                        .Repeat.Any();
            bogføringslinjeViewModelMock.Expect(m => m.Kontonummer)
                                        .Return(kontonummer)
                                        .Repeat.Any();

            Func<IBogføringslinjeModel> getter = fixture.Create<IBogføringslinjeModel>;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.BogføringslinjeCreateNewAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.NotNull))
                                       .Return(Task.Run(getter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var regnskabViewModel = new RegnskabViewModel(regnskabModelMock, fixture.Create<DateTime>(), finansstyringRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            Action action = () =>
                {
                    regnskabViewModel.BogføringslinjeAdd(bogføringslinjeViewModelMock);
                    Assert.That(regnskabViewModel.BogføringSetCommand, Is.Not.Null);
                    Assert.That(regnskabViewModel.BogføringSetCommand.ExecuteTask, Is.Not.Null);
                    regnskabViewModel.BogføringSetCommand.ExecuteTask.Wait();
                };
            Task.Run(action).Wait();

            finansstyringRepositoryMock.AssertWasCalled(m => m.BogføringslinjeCreateNewAsync(Arg<int>.Is.Equal(regnskabsnummer), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue), Arg<string>.Is.Equal(kontonummer)));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at BogføringslinjeAdd rejser PropertyChanged, når en bogføringslinje tilføjes regnskabet.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjeAddRejserPropertyChangedVedAddAfBogføringslinjeViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IReadOnlyBogføringslinjeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>()));
            fixture.Customize<IBogføringViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringViewModel>()));

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            regnskabViewModel.BogføringSet(fixture.Create<IBogføringViewModel>());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, "Bogføringslinjer", StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            Action action = () =>
                {
                    regnskabViewModel.BogføringslinjeAdd(fixture.Create<IReadOnlyBogføringslinjeViewModel>());
                    Assert.That(regnskabViewModel.BogføringSetCommand, Is.Not.Null);
                    Assert.That(regnskabViewModel.BogføringSetCommand.ExecuteTask, Is.Null);
                };
            Task.Run(action).Wait();
            Assert.That(eventCalled, Is.True);

            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at BogføringSet kaster en ArgumentNullException, hvis ViewModel til bogføring er null.
        /// </summary>
        [Test]
        public void TestAtBogføringSetKasterArgumentNullExceptionHvisBogføringViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => regnskabViewModel.BogføringSet(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("bogføringViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at BogføringSet opdaterer ViewModel til bogføring.
        /// </summary>
        [Test]
        public void TestAtBogføringSetOpdatererBogføring()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IBogføringViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            var bogføringViewModelMock = fixture.Create<IBogføringViewModel>();
            regnskabViewModel.BogføringSet(bogføringViewModelMock);
            
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføring, Is.EqualTo(bogføringViewModelMock));
        }

        /// <summary>
        /// Tester, at BogføringSet rejser PropertyChanged ved opdatering af ViewModel til bogføring.
        /// </summary>
        [Test]
        [TestCase("Bogføring")]
        public void TestAtBogføringSetRejserPropertyChangedVedVedOpdateringAfBogføring(string expectedPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IBogføringViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            regnskabViewModel.BogføringSet(fixture.Create<IBogføringViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, expectedPropertyName, StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            regnskabViewModel.BogføringSet(regnskabViewModel.Bogføring);
            Assert.That(eventCalled, Is.False);
            regnskabViewModel.BogføringSet(fixture.Create<IBogføringViewModel>());
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at BogføringsadvarselAdd kaster en ArgumentNullException, hvis ViewModel for bogføringsadvarslen er null.
        /// </summary>
        [Test]
        public void TestAtBogføringsadvarselAddKasterArgumentNullExceptionHvisBogføringsadvarselViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => regnskabViewModel.BogføringsadvarselAdd(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("bogføringsadvarselViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at BogføringsadvarselAdd tilføjer en bogføringsadvarsel til regnskabet.
        /// </summary>
        [Test]
        public void TestAtBogføringsadvarselAddAddsBogføringsadvarselViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IBogføringsadvarselViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringsadvarselViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføringsadvarsler, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføringsadvarsler, Is.Empty);

            var bogføringsadvarselViewModelMock = fixture.Create<IBogføringsadvarselViewModel>();
            regnskabViewModel.BogføringsadvarselAdd(bogføringsadvarselViewModelMock);

            Assert.That(regnskabViewModel.Bogføringsadvarsler, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføringsadvarsler, Is.Not.Empty);
        }

        /// <summary>
        /// Tester, at BogføringsadvarselAdd rejser PropertyChanged, når en bogføringsadvarsel tilføjes regnskabet.
        /// </summary>
        [Test]
        public void TestAtBogføringsadvarselAddRejserPropertyChangedVedAddAfBogføringsadvarselViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IBogføringsadvarselViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringsadvarselViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, "Bogføringsadvarsler", StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            regnskabViewModel.BogføringsadvarselAdd(fixture.Create<IBogføringsadvarselViewModel>());
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at BogføringsadvarselRemove kaster en ArgumentNullException, hvis ViewModel for bogføringsadvarslen er null.
        /// </summary>
        [Test]
        public void TestAtBogføringsadvarselRemoveKasterArgumentNullExceptionHvisBogføringsadvarselViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => regnskabViewModel.BogføringsadvarselRemove(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("bogføringsadvarselViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at BogføringsadvarselRemove fjerner en bogføringsadvarsel fra regnskabet.
        /// </summary>
        [Test]
        public void TestAtBogføringsadvarselRemoveRemovesBogføringsadvarselViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IBogføringsadvarselViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringsadvarselViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføringsadvarsler, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføringsadvarsler, Is.Empty);

            regnskabViewModel.BogføringsadvarselAdd(fixture.Create<IBogføringsadvarselViewModel>());
            Assert.That(regnskabViewModel.Bogføringsadvarsler, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføringsadvarsler, Is.Not.Empty);

            regnskabViewModel.BogføringsadvarselRemove(regnskabViewModel.Bogføringsadvarsler.ElementAt(0));
            Assert.That(regnskabViewModel.Bogføringsadvarsler, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføringsadvarsler, Is.Empty);
        }

        /// <summary>
        /// Tester, at BogføringsadvarselRemove rejser PropertyChanged, når en bogføringsadvarsel fjernes fra regnskabet.
        /// </summary>
        [Test]
        public void TestAtBogføringsadvarselRemoveRejserPropertyChangedVedRemoveAfBogføringsadvarselViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IBogføringsadvarselViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringsadvarselViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            regnskabViewModel.BogføringsadvarselAdd(fixture.Create<IBogføringsadvarselViewModel>());
            Assert.That(regnskabViewModel.Bogføringsadvarsler, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføringsadvarsler, Is.Not.Empty);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, "Bogføringsadvarsler", StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            regnskabViewModel.BogføringsadvarselRemove(regnskabViewModel.Bogføringsadvarsler.ElementAt(0));
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at DebitorAdd kaster en ArgumentNullException, hvis ViewModel for adressekontoen, der skal tilføjes som debitor, er null.
        /// </summary>
        [Test]
        public void TestAtDebitorAddKasterArgumentNullExceptionHvisAdressekontoViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => regnskabViewModel.DebitorAdd(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("adressekontoViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at DebitorAdd tilføjer en debitor til regnskabet.
        /// </summary>
        [Test]
        public void TestAtDebitorAddAddsAdressekontoViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IAdressekontoViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IAdressekontoViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Debitorer, Is.Not.Null);
            Assert.That(regnskabViewModel.Debitorer, Is.Empty);

            var adressekontoViewModelMock = fixture.Create<IAdressekontoViewModel>();
            regnskabViewModel.DebitorAdd(adressekontoViewModelMock);

            Assert.That(regnskabViewModel.Debitorer, Is.Not.Null);
            Assert.That(regnskabViewModel.Debitorer, Is.Not.Empty);
        }

        /// <summary>
        /// Tester, at DebitorAdd rejser PropertyChanged, når en debitor tilføjes regnskabet.
        /// </summary>
        [Test]
        public void TestAtDebitorAddRejserPropertyChangedVedAddAfAdressekontoViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IAdressekontoViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IAdressekontoViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, "Debitorer", StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            regnskabViewModel.DebitorAdd(fixture.Create<IAdressekontoViewModel>());
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at KreditorAdd kaster en ArgumentNullException, hvis ViewModel for adressekontoen, der skal tilføjes som kreditor, er null.
        /// </summary>
        [Test]
        public void TestAtKreditorAddKasterArgumentNullExceptionHvisAdressekontoViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => regnskabViewModel.KreditorAdd(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("adressekontoViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at KreditorAdd tilføjer en kreditor til regnskabet.
        /// </summary>
        [Test]
        public void TestAtKreditorAddAddsAdressekontoViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IAdressekontoViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IAdressekontoViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Kreditorer, Is.Not.Null);
            Assert.That(regnskabViewModel.Kreditorer, Is.Empty);

            var adressekontoViewModelMock = fixture.Create<IAdressekontoViewModel>();
            regnskabViewModel.KreditorAdd(adressekontoViewModelMock);

            Assert.That(regnskabViewModel.Kreditorer, Is.Not.Null);
            Assert.That(regnskabViewModel.Kreditorer, Is.Not.Empty);
        }

        /// <summary>
        /// Tester, at KreditorAdd rejser PropertyChanged, når en debitor tilføjes regnskabet.
        /// </summary>
        [Test]
        public void TestAtKreditorAddRejserPropertyChangedVedAddAfAdressekontoViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IAdressekontoViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IAdressekontoViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, "Kreditorer", StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            regnskabViewModel.KreditorAdd(fixture.Create<IAdressekontoViewModel>());
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at NyhedAdd kaster en ArgumentNullException, hvis ViewModel for nyheden er null.
        /// </summary>
        [Test]
        public void TestAtNyhedAddKasterArgumentNullExceptionHvisNyhedViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => regnskabViewModel.NyhedAdd(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("nyhedViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at NyhedAdd tilføjer en nyhed til regnskabet.
        /// </summary>
        [Test]
        public void TestAtNyhedAddAddsNyhedViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<INyhedViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<INyhedViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Nyheder, Is.Not.Null);
            Assert.That(regnskabViewModel.Nyheder, Is.Empty);

            var nyhedViewModelMock = fixture.Create<INyhedViewModel>();
            regnskabViewModel.NyhedAdd(nyhedViewModelMock);
            Assert.That(regnskabViewModel.Nyheder, Is.Not.Null);
            Assert.That(regnskabViewModel.Nyheder, Is.Not.Empty);
        }

        /// <summary>
        /// Tester, at NyhedAdd rejser PropertyChanged, når en nyhed tilføjes regnskabet.
        /// </summary>
        [Test]
        public void TestAtNyhedAddRejserPropertyChangedVedAddAfNyhedViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<INyhedViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<INyhedViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, "Nyheder", StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            regnskabViewModel.NyhedAdd(fixture.Create<INyhedViewModel>());
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at KontogruppeAdd kaster en ArgumentNullException, hvis ViewModel for kontogruppen er null.
        /// </summary>
        [Test]
        public void TestAtKontogruppeAddKasterArgumentNullExceptionHvisKontogruppeViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => regnskabViewModel.KontogruppeAdd(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("kontogruppeViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at KontogruppeAdd tilføjer en kontogruppe til regnskabet.
        /// </summary>
        [Test]
        [TestCase(1)]
        public void TestAtKontogruppeAddAddsKontogruppeViewModel(int nummer)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IKontogruppeViewModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(nummer)
                        .Repeat.Any();
                    return mock;
                }));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Kontogrupper, Is.Not.Null);

            var kontogruppeViewModelMock = fixture.Create<IKontogruppeViewModel>();
            Assert.That(kontogruppeViewModelMock, Is.Not.Null);

            regnskabViewModel.KontogruppeAdd(kontogruppeViewModelMock);
            Assert.That(regnskabViewModel.Kontogrupper, Is.Not.Null);
            Assert.That(regnskabViewModel.Kontogrupper.Count(m => m.Nummer == nummer), Is.EqualTo(1));

            kontogruppeViewModelMock.AssertWasCalled(m => m.Nummer);
        }

        /// <summary>
        /// Tester, at KontogruppeAdd kun tilføjer en kontogruppe med samme nummer én gang.
        /// </summary>
        [Test]
        [TestCase(2)]
        public void TestAtKontogruppeAddOnlyAddsKontogruppeViewModelOnce(int nummer)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IKontogruppeViewModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(nummer)
                        .Repeat.Any();
                    return mock;
                }));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Kontogrupper, Is.Not.Null);

            regnskabViewModel.KontogruppeAdd(fixture.Create<IKontogruppeViewModel>());
            regnskabViewModel.KontogruppeAdd(fixture.Create<IKontogruppeViewModel>());
            regnskabViewModel.KontogruppeAdd(fixture.Create<IKontogruppeViewModel>());
            Assert.That(regnskabViewModel.Kontogrupper, Is.Not.Null);
            Assert.That(regnskabViewModel.Kontogrupper.Count(m => m.Nummer == nummer), Is.EqualTo(1));
        }

        /// <summary>
        /// Tester, at KontogruppeAdd rejser PropertyChanged, når en kontogruppe tilføjes regnskabet.
        /// </summary>
        [Test]
        [TestCase(3)]
        public void TestAtKontogruppeAddRejserPropertyChangedVedAddAfKontogruppeViewModel(int nummer)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IKontogruppeViewModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(nummer)
                        .Repeat.Any();
                    return mock;
                }));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, "Kontogrupper", StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            regnskabViewModel.KontogruppeAdd(fixture.Create<IKontogruppeViewModel>());
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at BudgetkontogruppeAdd kaster en ArgumentNullException, hvis ViewModel for kontogruppen til budgetkonti er null.
        /// </summary>
        [Test]
        public void TestAtBudgetkontogruppeAddKasterArgumentNullExceptionHvisBudgetkontogruppeViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => regnskabViewModel.BudgetkontogruppeAdd(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("budgetkontogruppeViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at BudgetkontogruppeAdd tilføjer en kontogruppe for budgetkonti til regnskabet.
        /// </summary>
        [Test]
        [TestCase(1)]
        public void TestAtBudgetkontogruppeAddAddsBudgetkontogruppeViewModel(int nummer)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(nummer)
                        .Repeat.Any();
                    return mock;
                }));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkontogrupper, Is.Not.Null);

            var budgetkontogruppeViewModelMock = fixture.Create<IBudgetkontogruppeViewModel>();
            Assert.That(budgetkontogruppeViewModelMock, Is.Not.Null);

            regnskabViewModel.BudgetkontogruppeAdd(budgetkontogruppeViewModelMock);
            Assert.That(regnskabViewModel.Budgetkontogrupper, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkontogrupper.Count(m => m.Nummer == nummer), Is.EqualTo(1));

            budgetkontogruppeViewModelMock.AssertWasCalled(m => m.Nummer);
        }

        /// <summary>
        /// Tester, at BudgetkontogruppeAdd kun tilføjer en kontogruppe til budgetkonti med samme nummer én gang.
        /// </summary>
        [Test]
        [TestCase(2)]
        public void TestAtBudgetkontogruppeAddAddOnlyAddsBudgetkontogruppeViewModelOnce(int nummer)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(nummer)
                        .Repeat.Any();
                    return mock;
                }));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkontogrupper, Is.Not.Null);

            regnskabViewModel.BudgetkontogruppeAdd(fixture.Create<IBudgetkontogruppeViewModel>());
            regnskabViewModel.BudgetkontogruppeAdd(fixture.Create<IBudgetkontogruppeViewModel>());
            regnskabViewModel.BudgetkontogruppeAdd(fixture.Create<IBudgetkontogruppeViewModel>());
            Assert.That(regnskabViewModel.Budgetkontogrupper, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkontogrupper.Count(m => m.Nummer == nummer), Is.EqualTo(1));
        }

        /// <summary>
        /// Tester, at BudgetkontogruppeAdd rejser PropertyChanged, når en kontogruppe til budgetkonti tilføjes regnskabet.
        /// </summary>
        [Test]
        [TestCase(3)]
        public void TestAtBudgetkontogruppeAddRejserPropertyChangedVedAddAfBudgetkontogruppeViewModel(int nummer)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(nummer)
                        .Repeat.Any();
                    return mock;
                }));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, "Budgetkontogrupper", StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            regnskabViewModel.BudgetkontogruppeAdd(fixture.Create<IBudgetkontogruppeViewModel>());
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnRegnskabModelEventHandler rejser PropertyChanged, når modellen for regnskabet opdateres.
        /// </summary>
        [Test]
        [TestCase("Nummer", "Nummer")]
        [TestCase("Navn", "Navn")]
        [TestCase("Navn", "DisplayName")]
        public void TestAtPropertyChangedOnRegnskabModelEventHandlerRejserPropertyChangedOnRegnskabModelUpdate(string propertyNameToRaise, string expectPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var regnskabModelMock = fixture.Create<IRegnskabModel>();
            var regnskabViewModel = new RegnskabViewModel(regnskabModelMock, fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, expectPropertyName, StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            regnskabModelMock.Raise(m => m.PropertyChanged += null, regnskabModelMock, new PropertyChangedEventArgs(propertyNameToRaise));
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnRegnskabModelEventHandler kaster en ArgumentNullException, hvis objektet, der rejser eventet, er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnRegnskabModelEventHandlerKasterArgumentNullExceptionHvisSenderErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var regnskabModelMock = fixture.Create<IRegnskabModel>();
            var regnskabViewModel = new RegnskabViewModel(regnskabModelMock, fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => regnskabModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("sender"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnRegnskabModelEventHandler kaster en ArgumentNullException, hvis argumenter til eventet er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnRegnskabModelEventHandlerKasterArgumentNullExceptionHvisEventArgsErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var regnskabModelMock = fixture.Create<IRegnskabModel>();
            var regnskabViewModel = new RegnskabViewModel(regnskabModelMock, fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => regnskabModelMock.Raise(m => m.PropertyChanged += null, fixture.Create<object>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("e"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnKontoViewModelEventHandler rejser PropertyChanged, når ViewModel for en konto opdateres.
        /// </summary>
        [Test]
        [TestCase("Kontonummer", "Konti", "Aktiver")]
        [TestCase("Kontonummer", "KontiGrouped", "Aktiver")]
        [TestCase("Kontonummer", "KontiTop", "Aktiver")]
        [TestCase("Kontonummer", "KontiTopGrouped", "Aktiver")]
        [TestCase("Kontogruppe", "Konti", "Aktiver")]
        [TestCase("Kontogruppe", "KontiGrouped", "Aktiver")]
        [TestCase("Kontogruppe", "KontiTop", "Aktiver")]
        [TestCase("Kontogruppe", "KontiTopGrouped", "Aktiver")]
        [TestCase("Kontoværdi", "KontiTop", "Aktiver")]
        [TestCase("Kontoværdi", "KontiTopGrouped", "Aktiver")]
        [TestCase("Kontonummer", "Konti", "Passiver")]
        [TestCase("Kontonummer", "KontiGrouped", "Passiver")]
        [TestCase("Kontonummer", "KontiTop", "Passiver")]
        [TestCase("Kontonummer", "KontiTopGrouped", "Passiver")]
        [TestCase("Kontogruppe", "Konti", "Passiver")]
        [TestCase("Kontogruppe", "KontiGrouped", "Passiver")]
        [TestCase("Kontogruppe", "KontiTop", "Passiver")]
        [TestCase("Kontogruppe", "KontiTopGrouped", "Passiver")]
        [TestCase("Kontoværdi", "KontiTop", "Passiver")]
        [TestCase("Kontoværdi", "KontiTopGrouped", "Passiver")]
        public void TestAtPropertyChangedOnKontoViewModelEventHandlerRejserPropertyChangedOnKontoViewModelUpdate(string propertyName, string expectedPropertyName, string balanceType)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IBogføringViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringViewModel>()));
            fixture.Customize<IBalanceViewModel>(e => e.FromFactory(() =>
            {
                var mock = MockRepository.GenerateMock<IBalanceViewModel>();
                mock.Expect(m => m.Nummer)
                    .Return(fixture.Create<int>())
                    .Repeat.Any();
                mock.Expect(m => m.Balancetype)
                    .Return((Balancetype)Enum.Parse(typeof(Balancetype), balanceType))
                    .Repeat.Any();
                return mock;
            }));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() =>
            {
                var balanceViewModelMock = fixture.Create<IBalanceViewModel>();
                var mock = MockRepository.GenerateMock<IKontogruppeViewModel>();
                mock.Expect(m => m.Nummer)
                    .Return(balanceViewModelMock.Nummer)
                    .Repeat.Any();
                mock.Expect(m => m.CreateBalancelinje(Arg<IRegnskabViewModel>.Is.NotNull))
                    .Return(balanceViewModelMock)
                    .Repeat.Any();
                return mock;
            }));
            fixture.Customize<IKontoViewModel>(e => e.FromFactory(() =>
            {
                var mock = MockRepository.GenerateMock<IKontoViewModel>();
                mock.Expect(m => m.Kontogruppe)
                    .Return(fixture.Create<IKontogruppeViewModel>())
                    .Repeat.Any();
                return mock;
            }));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            regnskabViewModel.BogføringSet(fixture.Create<IBogføringViewModel>());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            var kontoViewModelMock = fixture.Create<IKontoViewModel>();
            regnskabViewModel.KontoAdd(kontoViewModelMock);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Empty);
                if (string.Compare(e.PropertyName, expectedPropertyName, StringComparison.Ordinal) == 0)
                {
                    eventCalled = true;
                }
            };

            Assert.That(eventCalled, Is.False);
            kontoViewModelMock.Raise(m => m.PropertyChanged += null, kontoViewModelMock, new PropertyChangedEventArgs(propertyName));
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnKontoViewModelEventHandler kaster en ArgumentNullException, hvis objektet, der rejser eventet, er null.
        /// </summary>
        [Test]
        [TestCase("Aktiver")]
        [TestCase("Passiver")]
        public void TestAtPropertyChangedOnKontoViewModelEventHandlerKasterArgumentNullExceptionHvisSenderErNull(string balanceType)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IBogføringViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringViewModel>()));
            fixture.Customize<IBalanceViewModel>(e => e.FromFactory(() =>
            {
                var mock = MockRepository.GenerateMock<IBalanceViewModel>();
                mock.Expect(m => m.Nummer)
                    .Return(fixture.Create<int>())
                    .Repeat.Any();
                mock.Expect(m => m.Balancetype)
                    .Return((Balancetype)Enum.Parse(typeof(Balancetype), balanceType))
                    .Repeat.Any();
                return mock;
            }));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() =>
            {
                var balanceViewModelMock = fixture.Create<IBalanceViewModel>();
                var mock = MockRepository.GenerateMock<IKontogruppeViewModel>();
                mock.Expect(m => m.Nummer)
                    .Return(balanceViewModelMock.Nummer)
                    .Repeat.Any();
                mock.Expect(m => m.CreateBalancelinje(Arg<IRegnskabViewModel>.Is.NotNull))
                    .Return(balanceViewModelMock)
                    .Repeat.Any();
                return mock;
            }));
            fixture.Customize<IKontoViewModel>(e => e.FromFactory(() =>
            {
                var mock = MockRepository.GenerateMock<IKontoViewModel>();
                mock.Expect(m => m.Kontogruppe)
                    .Return(fixture.Create<IKontogruppeViewModel>())
                    .Repeat.Any();
                return mock;
            }));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            regnskabViewModel.BogføringSet(fixture.Create<IBogføringViewModel>());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            var kontoViewModelMock = fixture.Create<IKontoViewModel>();
            regnskabViewModel.KontoAdd(kontoViewModelMock);

            var exception = Assert.Throws<ArgumentNullException>(() => kontoViewModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("sender"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnKontoViewModelEventHandler kaster en ArgumentNullException, hvis argumenter til eventet er null.
        /// </summary>
        [Test]
        [TestCase("Aktiver")]
        [TestCase("Passiver")]
        public void TestAtPropertyChangedOnKontoViewModelEventHandlerKasterArgumentNullExceptionHvisEventArgsErNull(string balanceType)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IBogføringViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringViewModel>()));
            fixture.Customize<IBalanceViewModel>(e => e.FromFactory(() =>
            {
                var mock = MockRepository.GenerateMock<IBalanceViewModel>();
                mock.Expect(m => m.Nummer)
                    .Return(fixture.Create<int>())
                    .Repeat.Any();
                mock.Expect(m => m.Balancetype)
                    .Return((Balancetype)Enum.Parse(typeof(Balancetype), balanceType))
                    .Repeat.Any();
                return mock;
            }));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() =>
            {
                var balanceViewModelMock = fixture.Create<IBalanceViewModel>();
                var mock = MockRepository.GenerateMock<IKontogruppeViewModel>();
                mock.Expect(m => m.Nummer)
                    .Return(balanceViewModelMock.Nummer)
                    .Repeat.Any();
                mock.Expect(m => m.CreateBalancelinje(Arg<IRegnskabViewModel>.Is.NotNull))
                    .Return(balanceViewModelMock)
                    .Repeat.Any();
                return mock;
            }));
            fixture.Customize<IKontoViewModel>(e => e.FromFactory(() =>
            {
                var mock = MockRepository.GenerateMock<IKontoViewModel>();
                mock.Expect(m => m.Kontogruppe)
                    .Return(fixture.Create<IKontogruppeViewModel>())
                    .Repeat.Any();
                return mock;
            }));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            regnskabViewModel.BogføringSet(fixture.Create<IBogføringViewModel>());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            var kontoViewModelMock = fixture.Create<IKontoViewModel>();
            regnskabViewModel.KontoAdd(kontoViewModelMock);

            var exception = Assert.Throws<ArgumentNullException>(() => kontoViewModelMock.Raise(m => m.PropertyChanged += null, kontoViewModelMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("e"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBudgetkontoViewModelEventHandler rejser PropertyChanged, når ViewModel for en budgetkonto opdateres.
        /// </summary>
        [Test]
        [TestCase("Kontonummer", "Budgetkonti")]
        [TestCase("Kontonummer", "BudgetkontiGrouped")]
        [TestCase("Kontonummer", "BudgetkontiTop")]
        [TestCase("Kontonummer", "BudgetkontiTopGrouped")]
        [TestCase("Kontogruppe", "Budgetkonti")]
        [TestCase("Kontogruppe", "BudgetkontiGrouped")]
        [TestCase("Kontogruppe", "BudgetkontiTop")]
        [TestCase("Kontogruppe", "BudgetkontiTopGrouped")]
        [TestCase("Kontoværdi", "BudgetkontiTop")]
        [TestCase("Kontoværdi", "BudgetkontiTopGrouped")]
        public void TestAtPropertyChangedOnBudgetkontoViewModelEventHandlerRejserPropertyChangedOnBudgetkontoViewModelUpdate(string propertyName, string expectedPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IOpgørelseViewModel>(e => e.FromFactory(() =>
            {
                var mock = MockRepository.GenerateMock<IOpgørelseViewModel>();
                mock.Expect(m => m.Nummer)
                    .Return(fixture.Create<int>())
                    .Repeat.Any();
                return mock;
            }));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() =>
            {
                var opgørelseViewModelMock = fixture.Create<IOpgørelseViewModel>();
                var mock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                mock.Expect(m => m.Nummer)
                    .Return(opgørelseViewModelMock.Nummer)
                    .Repeat.Any();
                mock.Expect(m => m.CreateOpgørelseslinje(Arg<IRegnskabViewModel>.Is.NotNull))
                    .Return(opgørelseViewModelMock)
                    .Repeat.Any();
                return mock;
            }));
            fixture.Customize<IBudgetkontoViewModel>(e => e.FromFactory(() =>
            {
                var mock = MockRepository.GenerateMock<IBudgetkontoViewModel>();
                mock.Expect(m => m.Kontogruppe)
                    .Return(fixture.Create<IBudgetkontogruppeViewModel>())
                    .Repeat.Any();
                return mock;
            }));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var budgetkontoViewModelMock = fixture.Create<IBudgetkontoViewModel>();
            regnskabViewModel.BudgetkontoAdd(budgetkontoViewModelMock);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, expectedPropertyName, StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            budgetkontoViewModelMock.Raise(m => m.PropertyChanged += null, budgetkontoViewModelMock, new PropertyChangedEventArgs(propertyName));
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBudgetkontoViewModelEventHandler kaster en ArgumentNullException, hvis objektet, der rejser eventet, er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnBudgetkontoViewModelEventHandlerKasterArgumentNullExceptionHvisSenderErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IOpgørelseViewModel>(e => e.FromFactory(() =>
            {
                var mock = MockRepository.GenerateMock<IOpgørelseViewModel>();
                mock.Expect(m => m.Nummer)
                    .Return(fixture.Create<int>())
                    .Repeat.Any();
                return mock;
            }));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() =>
            {
                var opgørelseViewModelMock = fixture.Create<IOpgørelseViewModel>();
                var mock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                mock.Expect(m => m.Nummer)
                    .Return(opgørelseViewModelMock.Nummer)
                    .Repeat.Any();
                mock.Expect(m => m.CreateOpgørelseslinje(Arg<IRegnskabViewModel>.Is.NotNull))
                    .Return(opgørelseViewModelMock)
                    .Repeat.Any();
                return mock;
            }));
            fixture.Customize<IBudgetkontoViewModel>(e => e.FromFactory(() =>
            {
                var mock = MockRepository.GenerateMock<IBudgetkontoViewModel>();
                mock.Expect(m => m.Kontogruppe)
                    .Return(fixture.Create<IBudgetkontogruppeViewModel>())
                    .Repeat.Any();
                return mock;
            }));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var budgetkontoViewModelMock = fixture.Create<IBudgetkontoViewModel>();
            regnskabViewModel.BudgetkontoAdd(budgetkontoViewModelMock);

            var exception = Assert.Throws<ArgumentNullException>(() => budgetkontoViewModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("sender"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBudgetkontoViewModelEventHandler kaster en ArgumentNullException, hvis argumenter til eventet er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnBudgetkontoViewModelEventHandlerKasterArgumentNullExceptionHvisEventArgsErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IOpgørelseViewModel>(e => e.FromFactory(() =>
            {
                var mock = MockRepository.GenerateMock<IOpgørelseViewModel>();
                mock.Expect(m => m.Nummer)
                    .Return(fixture.Create<int>())
                    .Repeat.Any();
                return mock;
            }));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() =>
            {
                var opgørelseViewModelMock = fixture.Create<IOpgørelseViewModel>();
                var mock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                mock.Expect(m => m.Nummer)
                    .Return(opgørelseViewModelMock.Nummer)
                    .Repeat.Any();
                mock.Expect(m => m.CreateOpgørelseslinje(Arg<IRegnskabViewModel>.Is.NotNull))
                    .Return(opgørelseViewModelMock)
                    .Repeat.Any();
                return mock;
            }));
            fixture.Customize<IBudgetkontoViewModel>(e => e.FromFactory(() =>
            {
                var mock = MockRepository.GenerateMock<IBudgetkontoViewModel>();
                mock.Expect(m => m.Kontogruppe)
                    .Return(fixture.Create<IBudgetkontogruppeViewModel>())
                    .Repeat.Any();
                return mock;
            }));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var budgetkontoViewModelMock = fixture.Create<IBudgetkontoViewModel>();
            regnskabViewModel.BudgetkontoAdd(budgetkontoViewModelMock);

            var exception = Assert.Throws<ArgumentNullException>(() => budgetkontoViewModelMock.Raise(m => m.PropertyChanged += null, budgetkontoViewModelMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("e"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBogføringslinjeViewModelEventHandler rejser PropertyChanged, når ViewModel for en bogføringslinje opdateres.
        /// </summary>
        [Test]
        [TestCase("Løbenummer", "Bogføringslinjer")]
        [TestCase("Dato", "Bogføringslinjer")]
        public void TestAtPropertyChangedOnBogføringslinjeViewModelEventHandlerRejserPropertyChangedOnBogføringslinjeViewModelUpdate(string propertyName, string expectedPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IReadOnlyBogføringslinjeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>()));
            fixture.Customize<IBogføringViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            regnskabViewModel.BogføringSet(fixture.Create<IBogføringViewModel>());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            var readOnlyBogføringslinjeViewModelMock = fixture.Create<IReadOnlyBogføringslinjeViewModel>();
            regnskabViewModel.BogføringslinjeAdd(readOnlyBogføringslinjeViewModelMock);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, expectedPropertyName, StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            readOnlyBogføringslinjeViewModelMock.Raise(m => m.PropertyChanged += null, readOnlyBogføringslinjeViewModelMock, new PropertyChangedEventArgs(propertyName));
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBogføringslinjeViewModelEventHandler kaster en ArgumentNullException, hvis objektet, der rejser eventet, er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnBogføringslinjeViewModelEventHandlerKasterArgumentNullExceptionHvisSenderErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IReadOnlyBogføringslinjeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>()));
            fixture.Customize<IBogføringViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            regnskabViewModel.BogføringSet(fixture.Create<IBogføringViewModel>());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            var readOnlyBogføringslinjeViewModelMock = fixture.Create<IReadOnlyBogføringslinjeViewModel>();
            regnskabViewModel.BogføringslinjeAdd(readOnlyBogføringslinjeViewModelMock);

            var exception = Assert.Throws<ArgumentNullException>(() => readOnlyBogføringslinjeViewModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("sender"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBogføringslinjeViewModelEventHandler kaster en ArgumentNullException, hvis argumenter til eventet er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnBogføringslinjeViewModelEventHandlerKasterArgumentNullExceptionHvisEventArgsErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IReadOnlyBogføringslinjeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>()));
            fixture.Customize<IBogføringViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            regnskabViewModel.BogføringSet(fixture.Create<IBogføringViewModel>());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            var readOnlyBogføringslinjeViewModelMock = fixture.Create<IReadOnlyBogføringslinjeViewModel>();
            regnskabViewModel.BogføringslinjeAdd(readOnlyBogføringslinjeViewModelMock);

            var exception = Assert.Throws<ArgumentNullException>(() => readOnlyBogføringslinjeViewModelMock.Raise(m => m.PropertyChanged += null, readOnlyBogføringslinjeViewModelMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("e"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBogføringsadvarselViewModelEventHandler rejser PropertyChanged, når ViewModel for en bogføringsadvarsel opdateres.
        /// </summary>
        [Test]
        [TestCase("Tidspunkt", "Bogføringsadvarsler")]
        public void TestAtPropertyChangedOnBogføringsadvarselViewModelEventHandlerRejserPropertyChangedOnBogføringsadvarselViewModelUpdate(string propertyName, string expectedPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IBogføringsadvarselViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringsadvarselViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var bogføringsadvarselViewModelMock = fixture.Create<IBogføringsadvarselViewModel>();
            regnskabViewModel.BogføringsadvarselAdd(bogføringsadvarselViewModelMock);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, expectedPropertyName, StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            bogføringsadvarselViewModelMock.Raise(m => m.PropertyChanged += null, bogføringsadvarselViewModelMock, new PropertyChangedEventArgs(propertyName));
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBogføringsadvarselViewModelEventHandler kaster en ArgumentNullException, hvis objektet, der rejser eventet, er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnBogføringsadvarselViewModelEventHandlerKasterArgumentNullExceptionHvisSenderErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IBogføringsadvarselViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringsadvarselViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var bogføringsadvarselViewModelMock = fixture.Create<IBogføringsadvarselViewModel>();
            regnskabViewModel.BogføringsadvarselAdd(bogføringsadvarselViewModelMock);

            var exception = Assert.Throws<ArgumentNullException>(() => bogføringsadvarselViewModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("sender"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBogføringsadvarselViewModelEventHandler kaster en ArgumentNullException, hvis argumenter til eventet er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnBogføringsadvarselViewModelEventHandlerKasterArgumentNullExceptionHvisEventArgsErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IBogføringsadvarselViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringsadvarselViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var bogføringsadvarselViewModelMock = fixture.Create<IBogføringsadvarselViewModel>();
            regnskabViewModel.BogføringsadvarselAdd(bogføringsadvarselViewModelMock);

            var exception = Assert.Throws<ArgumentNullException>(() => bogføringsadvarselViewModelMock.Raise(m => m.PropertyChanged += null, bogføringsadvarselViewModelMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("e"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnOpgørelseViewModelEventHandler rejser PropertyChanged, når ViewModel for en linje til årsopgørelsen opdateres.
        /// </summary>
        [Test]
        [TestCase("Nummer", "Opgørelseslinjer")]
        [TestCase("Budget", "Budget")]
        [TestCase("Budget", "BudgetAsText")]
        [TestCase("BudgetSidsteMåned", "BudgetSidsteMåned")]
        [TestCase("BudgetSidsteMåned", "BudgetSidsteMånedAsText")]
        [TestCase("BudgetÅrTilDato", "BudgetÅrTilDato")]
        [TestCase("BudgetÅrTilDato", "BudgetÅrTilDatoAsText")]
        [TestCase("BudgetSidsteÅr", "BudgetSidsteÅr")]
        [TestCase("BudgetSidsteÅr", "BudgetSidsteÅrAsText")]
        [TestCase("Bogført", "Bogført")]
        [TestCase("Bogført", "BogførtAsText")]
        [TestCase("BogførtSidsteMåned", "BogførtSidsteMåned")]
        [TestCase("BogførtSidsteMåned", "BogførtSidsteMånedAsText")]
        [TestCase("BogførtÅrTilDato", "BogførtÅrTilDato")]
        [TestCase("BogførtÅrTilDato", "BogførtÅrTilDatoAsText")]
        [TestCase("BogførtSidsteÅr", "BogførtSidsteÅr")]
        [TestCase("BogførtSidsteÅr", "BogførtSidsteÅrAsText")]
        public void TestAtPropertyChangedOnOpgørelseViewModelEventHandlerRejserPropertyChangedOnOpgørelseViewModelUpdate(string propertyName, string expectedPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var opgørelseViewModelMock = MockRepository.GenerateMock<IOpgørelseViewModel>();
            opgørelseViewModelMock.Expect(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();

            var budgetkontogruppeViewModelMock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
            budgetkontogruppeViewModelMock.Expect(m => m.Nummer)
                .Return(opgørelseViewModelMock.Nummer)
                .Repeat.Any();
            budgetkontogruppeViewModelMock.Expect(m => m.CreateOpgørelseslinje(Arg<IRegnskabViewModel>.Is.NotNull))
                .Return(opgørelseViewModelMock)
                .Repeat.Any();

            var budgetkontoViewModelMock = MockRepository.GenerateMock<IBudgetkontoViewModel>();
            budgetkontoViewModelMock.Expect(m => m.Kontogruppe)
                .Return(budgetkontogruppeViewModelMock)
                .Repeat.Any();

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            regnskabViewModel.BudgetkontoAdd(budgetkontoViewModelMock);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Empty);
                if (string.Compare(e.PropertyName, expectedPropertyName, StringComparison.Ordinal) == 0)
                {
                    eventCalled = true;
                }
            };

            Assert.That(eventCalled, Is.False);
            opgørelseViewModelMock.Raise(m => m.PropertyChanged += null, opgørelseViewModelMock, new PropertyChangedEventArgs(propertyName));
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnOpgørelseViewModelEventHandlerr kaster en ArgumentNullException, hvis objektet, der rejser eventet, er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnOpgørelseViewModelEventHandlerKasterArgumentNullExceptionHvisSenderErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var opgørelseViewModelMock = MockRepository.GenerateMock<IOpgørelseViewModel>();
            opgørelseViewModelMock.Expect(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();

            var budgetkontogruppeViewModelMock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
            budgetkontogruppeViewModelMock.Expect(m => m.Nummer)
                .Return(opgørelseViewModelMock.Nummer)
                .Repeat.Any();
            budgetkontogruppeViewModelMock.Expect(m => m.CreateOpgørelseslinje(Arg<IRegnskabViewModel>.Is.NotNull))
                .Return(opgørelseViewModelMock)
                .Repeat.Any();

            var budgetkontoViewModelMock = MockRepository.GenerateMock<IBudgetkontoViewModel>();
            budgetkontoViewModelMock.Expect(m => m.Kontogruppe)
                .Return(budgetkontogruppeViewModelMock)
                .Repeat.Any();

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            regnskabViewModel.BudgetkontoAdd(budgetkontoViewModelMock);

            var exception = Assert.Throws<ArgumentNullException>(() => opgørelseViewModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("sender"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnOpgørelseViewModelEventHandler kaster en ArgumentNullException, hvis argumenter til eventet er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnOpgørelseViewModelEventHandlerKasterArgumentNullExceptionHvisEventArgsErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var opgørelseViewModelMock = MockRepository.GenerateMock<IOpgørelseViewModel>();
            opgørelseViewModelMock.Expect(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();

            var budgetkontogruppeViewModelMock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
            budgetkontogruppeViewModelMock.Expect(m => m.Nummer)
                .Return(opgørelseViewModelMock.Nummer)
                .Repeat.Any();
            budgetkontogruppeViewModelMock.Expect(m => m.CreateOpgørelseslinje(Arg<IRegnskabViewModel>.Is.NotNull))
                .Return(opgørelseViewModelMock)
                .Repeat.Any();

            var budgetkontoViewModelMock = MockRepository.GenerateMock<IBudgetkontoViewModel>();
            budgetkontoViewModelMock.Expect(m => m.Kontogruppe)
                .Return(budgetkontogruppeViewModelMock)
                .Repeat.Any();

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            regnskabViewModel.BudgetkontoAdd(budgetkontoViewModelMock);

            var exception = Assert.Throws<ArgumentNullException>(() => opgørelseViewModelMock.Raise(m => m.PropertyChanged += null, opgørelseViewModelMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("e"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBalanceViewModelEventHandler rejser PropertyChanged, når ViewModel for en linje til balancen opdateres.
        /// </summary>
        [Test]
        [TestCase("Nummer", "Aktiver", "Aktiver")]
        [TestCase("Nummer", "Passiver", "Passiver")]
        [TestCase("Balancetype", "Aktiver", "Aktiver")]
        [TestCase("Balancetype", "Aktiver", "Passiver")]
        [TestCase("Balancetype", "AktiverIAlt", "Aktiver")]
        [TestCase("Balancetype", "AktiverIAlt", "Passiver")]
        [TestCase("Balancetype", "AktiverIAltAsText", "Aktiver")]
        [TestCase("Balancetype", "AktiverIAltAsText", "Passiver")]
        [TestCase("Balancetype", "Passiver", "Aktiver")]
        [TestCase("Balancetype", "Passiver", "Passiver")]
        [TestCase("Balancetype", "PassiverIAlt", "Aktiver")]
        [TestCase("Balancetype", "PassiverIAlt", "Passiver")]
        [TestCase("Balancetype", "PassiverIAltAsText", "Aktiver")]
        [TestCase("Balancetype", "PassiverIAltAsText", "Passiver")]
        [TestCase("Saldo", "AktiverIAlt", "Aktiver")]
        [TestCase("Saldo", "AktiverIAltAsText", "Aktiver")]
        [TestCase("Saldo", "PassiverIAlt", "Passiver")]
        [TestCase("Saldo", "PassiverIAltAsText", "Passiver")]
        public void TestAtPropertyChangedOnBalanceViewModelEventHandlerRejserPropertyChangedOnBalanceViewModelUpdate(string propertyName, string expectedPropertyName, string balanceType)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IBogføringViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringViewModel>()));

            var balanceViewModelMock = MockRepository.GenerateMock<IBalanceViewModel>();
            balanceViewModelMock.Expect(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();
            balanceViewModelMock.Expect(m => m.Balancetype)
                .Return((Balancetype)Enum.Parse(typeof(Balancetype), balanceType))
                .Repeat.Any();

            var kontogruppeViewModelMock = MockRepository.GenerateMock<IKontogruppeViewModel>();
            kontogruppeViewModelMock.Expect(m => m.Nummer)
                .Return(balanceViewModelMock.Nummer)
                .Repeat.Any();
            kontogruppeViewModelMock.Expect(m => m.CreateBalancelinje(Arg<IRegnskabViewModel>.Is.NotNull))
                .Return(balanceViewModelMock)
                .Repeat.Any();

            var kontoViewModelMock = MockRepository.GenerateMock<IKontoViewModel>();
            kontoViewModelMock.Expect(m => m.Kontonummer)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            kontoViewModelMock.Expect(m => m.Kontogruppe)
                .Return(kontogruppeViewModelMock)
                .Repeat.Any();
            kontoViewModelMock.Expect(m => m.Kontoværdi)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Aktiver, Is.Not.Null);
            Assert.That(regnskabViewModel.Aktiver, Is.Empty);
            Assert.That(regnskabViewModel.Passiver, Is.Not.Null);
            Assert.That(regnskabViewModel.Passiver, Is.Empty);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            regnskabViewModel.BogføringSet(fixture.Create<IBogføringViewModel>());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            Action action = () =>
            {
                regnskabViewModel.KontoAdd(kontoViewModelMock);
                Assert.That(regnskabViewModel.BogføringSetCommand, Is.Not.Null);
                Assert.That(regnskabViewModel.BogføringSetCommand.ExecuteTask, Is.Null);
            };
            Task.Run(action).Wait(3000);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Empty);
                if (string.Compare(e.PropertyName, expectedPropertyName, StringComparison.Ordinal) == 0)
                {
                    eventCalled = true;
                }
            };

            Assert.That(eventCalled, Is.False);
            balanceViewModelMock.Raise(m => m.PropertyChanged += null, balanceViewModelMock, new PropertyChangedEventArgs(propertyName));
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBalanceViewModelEventHandler kaster en ArgumentNullException, hvis objektet, der rejser eventet, er null.
        /// </summary>
        [Test]
        [TestCase("Aktiver")]
        [TestCase("Passiver")]
        public void TestAtPropertyChangedOnBalanceViewModelEventHandlerKasterArgumentNullExceptionHvisSenderErNull(string balanceType)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IBogføringViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringViewModel>()));

            var balanceViewModelMock = MockRepository.GenerateMock<IBalanceViewModel>();
            balanceViewModelMock.Expect(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();
            balanceViewModelMock.Expect(m => m.Balancetype)
                .Return((Balancetype) Enum.Parse(typeof (Balancetype), balanceType))
                .Repeat.Any();

            var kontogruppeViewModelMock = MockRepository.GenerateMock<IKontogruppeViewModel>();
            kontogruppeViewModelMock.Expect(m => m.Nummer)
                .Return(balanceViewModelMock.Nummer)
                .Repeat.Any();
            kontogruppeViewModelMock.Expect(m => m.CreateBalancelinje(Arg<IRegnskabViewModel>.Is.NotNull))
                .Return(balanceViewModelMock)
                .Repeat.Any();

            var kontoViewModelMock = MockRepository.GenerateMock<IKontoViewModel>();
            kontoViewModelMock.Expect(m => m.Kontonummer)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            kontoViewModelMock.Expect(m => m.Kontogruppe)
                .Return(kontogruppeViewModelMock)
                .Repeat.Any();
            kontoViewModelMock.Expect(m => m.Kontoværdi)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Aktiver, Is.Not.Null);
            Assert.That(regnskabViewModel.Aktiver, Is.Empty);
            Assert.That(regnskabViewModel.Passiver, Is.Not.Null);
            Assert.That(regnskabViewModel.Passiver, Is.Empty);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            regnskabViewModel.BogføringSet(fixture.Create<IBogføringViewModel>());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            Action action = () =>
            {
                regnskabViewModel.KontoAdd(kontoViewModelMock);
                Assert.That(regnskabViewModel.BogføringSetCommand, Is.Not.Null);
                Assert.That(regnskabViewModel.BogføringSetCommand.ExecuteTask, Is.Null);
            };
            Task.Run(action).Wait(3000);

            var exception = Assert.Throws<ArgumentNullException>(() => balanceViewModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("sender"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBalanceViewModelEventHandler kaster en ArgumentNullException, hvis argumenter til eventet er null.
        /// </summary>
        [Test]
        [TestCase("Aktiver")]
        [TestCase("Passiver")]
        public void TestAtPropertyChangedOnBalanceViewModelEventHandlerKasterArgumentNullExceptionHvisEventArgsErNull(string balanceType)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IBogføringViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringViewModel>()));

            var balanceViewModelMock = MockRepository.GenerateMock<IBalanceViewModel>();
            balanceViewModelMock.Expect(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();
            balanceViewModelMock.Expect(m => m.Balancetype)
                .Return((Balancetype) Enum.Parse(typeof (Balancetype), balanceType))
                .Repeat.Any();

            var kontogruppeViewModelMock = MockRepository.GenerateMock<IKontogruppeViewModel>();
            kontogruppeViewModelMock.Expect(m => m.Nummer)
                .Return(balanceViewModelMock.Nummer)
                .Repeat.Any();
            kontogruppeViewModelMock.Expect(m => m.CreateBalancelinje(Arg<IRegnskabViewModel>.Is.NotNull))
                .Return(balanceViewModelMock)
                .Repeat.Any();

            var kontoViewModelMock = MockRepository.GenerateMock<IKontoViewModel>();
            kontoViewModelMock.Expect(m => m.Kontonummer)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            kontoViewModelMock.Expect(m => m.Kontogruppe)
                .Return(kontogruppeViewModelMock)
                .Repeat.Any();
            kontoViewModelMock.Expect(m => m.Kontoværdi)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Aktiver, Is.Not.Null);
            Assert.That(regnskabViewModel.Aktiver, Is.Empty);
            Assert.That(regnskabViewModel.Passiver, Is.Not.Null);
            Assert.That(regnskabViewModel.Passiver, Is.Empty);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            regnskabViewModel.BogføringSet(fixture.Create<IBogføringViewModel>());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            Action action = () =>
            {
                regnskabViewModel.KontoAdd(kontoViewModelMock);
                Assert.That(regnskabViewModel.BogføringSetCommand, Is.Not.Null);
                Assert.That(regnskabViewModel.BogføringSetCommand.ExecuteTask, Is.Null);
            };
            Task.Run(action).Wait(3000);

            var exception = Assert.Throws<ArgumentNullException>(() => balanceViewModelMock.Raise(m => m.PropertyChanged += null, balanceViewModelMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("e"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBalanceViewModelEventHandler kaster en IntranetGuiSystemException, hvis objektet, der rejser eventet, ikke er en ViewModel for en linje, der indgår i balancen.
        /// </summary>
        [Test]
        [TestCase("Aktiver")]
        [TestCase("Passiver")]
        public void TestAtPropertyChangedOnBalanceViewModelEventHandlerKasterIntranetGuiSystemExceptionHvisSenderIkkeErBalanceViewModel(string balanceType)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IBogføringViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringViewModel>()));

            var balanceViewModelMock = MockRepository.GenerateMock<IBalanceViewModel>();
            balanceViewModelMock.Expect(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();
            balanceViewModelMock.Expect(m => m.Balancetype)
                .Return((Balancetype) Enum.Parse(typeof (Balancetype), balanceType))
                .Repeat.Any();

            var kontogruppeViewModelMock = MockRepository.GenerateMock<IKontogruppeViewModel>();
            kontogruppeViewModelMock.Expect(m => m.Nummer)
                .Return(balanceViewModelMock.Nummer)
                .Repeat.Any();
            kontogruppeViewModelMock.Expect(m => m.CreateBalancelinje(Arg<IRegnskabViewModel>.Is.NotNull))
                .Return(balanceViewModelMock)
                .Repeat.Any();

            var kontoViewModelMock = MockRepository.GenerateMock<IKontoViewModel>();
            kontoViewModelMock.Expect(m => m.Kontonummer)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            kontoViewModelMock.Expect(m => m.Kontogruppe)
                .Return(kontogruppeViewModelMock)
                .Repeat.Any();
            kontoViewModelMock.Expect(m => m.Kontoværdi)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Aktiver, Is.Not.Null);
            Assert.That(regnskabViewModel.Aktiver, Is.Empty);
            Assert.That(regnskabViewModel.Passiver, Is.Not.Null);
            Assert.That(regnskabViewModel.Passiver, Is.Empty);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            regnskabViewModel.BogføringSet(fixture.Create<IBogføringViewModel>());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            Action action = () =>
            {
                regnskabViewModel.KontoAdd(kontoViewModelMock);
                Assert.That(regnskabViewModel.BogføringSetCommand, Is.Not.Null);
                Assert.That(regnskabViewModel.BogføringSetCommand.ExecuteTask, Is.Null);
            };
            Task.Run(action).Wait(3000);

            var sender = fixture.Create<object>();
            var exception = Assert.Throws<IntranetGuiSystemException>(() => balanceViewModelMock.Raise(m => m.PropertyChanged += null, sender, fixture.Create<PropertyChangedEventArgs>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "sender", sender.GetType().Name)));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnAdressekontoViewModelForDebitorEventHandler rejser PropertyChanged, når ViewModel for adressekontoen til en debitor opdateres.
        /// </summary>
        [Test]
        [TestCase("Navn", "Debitorer")]
        [TestCase("StatusDato", "Debitorer")]
        [TestCase("Saldo", "Debitorer")]
        public void TestAtPropertyChangedOnAdressekontoViewModelForDebitorEventHandlerRejserPropertyChangedOnAdressekontoViewModelUpdate(string propertyName, string expectedPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IAdressekontoViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IAdressekontoViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var adressekontoViewModelMock = fixture.Create<IAdressekontoViewModel>();
            regnskabViewModel.DebitorAdd(adressekontoViewModelMock);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, expectedPropertyName, StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            adressekontoViewModelMock.Raise(m => m.PropertyChanged += null, adressekontoViewModelMock, new PropertyChangedEventArgs(propertyName));
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnAdressekontoViewModelForDebitorEventHandler kaster en ArgumentNullException, hvis objektet, der rejser eventet, er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnAdressekontoViewModelForDebitorEventHandlerKasterArgumentNullExceptionHvisSenderErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IAdressekontoViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IAdressekontoViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var adressekontoViewModelMock = fixture.Create<IAdressekontoViewModel>();
            regnskabViewModel.DebitorAdd(adressekontoViewModelMock);

            var exception = Assert.Throws<ArgumentNullException>(() => adressekontoViewModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("sender"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnAdressekontoViewModelForDebitorEventHandler kaster en ArgumentNullException, hvis argumenter til eventet er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnAdressekontoViewModelForDebitorEventHandlerKasterArgumentNullExceptionHvisEventArgsErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IAdressekontoViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IAdressekontoViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var adressekontoViewModelMock = fixture.Create<IAdressekontoViewModel>();
            regnskabViewModel.DebitorAdd(adressekontoViewModelMock);

            var exception = Assert.Throws<ArgumentNullException>(() => adressekontoViewModelMock.Raise(m => m.PropertyChanged += null, adressekontoViewModelMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("e"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnAdressekontoViewModelForKreditorEventHandler rejser PropertyChanged, når ViewModel for adressekontoen til en kreditor opdateres.
        /// </summary>
        [Test]
        [TestCase("Navn", "Kreditorer")]
        [TestCase("StatusDato", "Kreditorer")]
        [TestCase("Saldo", "Kreditorer")]
        public void TestAtPropertyChangedOnAdressekontoViewModelForKreditorEventHandlerRejserPropertyChangedOnAdressekontoViewModelUpdate(string propertyName, string expectedPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IAdressekontoViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IAdressekontoViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var adressekontoViewModelMock = fixture.Create<IAdressekontoViewModel>();
            regnskabViewModel.KreditorAdd(adressekontoViewModelMock);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, expectedPropertyName, StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            adressekontoViewModelMock.Raise(m => m.PropertyChanged += null, adressekontoViewModelMock, new PropertyChangedEventArgs(propertyName));
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnAdressekontoViewModelForKreditorEventHandler kaster en ArgumentNullException, hvis objektet, der rejser eventet, er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnAdressekontoViewModelForKreditorEventHandlerKasterArgumentNullExceptionHvisSenderErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IAdressekontoViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IAdressekontoViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var adressekontoViewModelMock = fixture.Create<IAdressekontoViewModel>();
            regnskabViewModel.KreditorAdd(adressekontoViewModelMock);

            var exception = Assert.Throws<ArgumentNullException>(() => adressekontoViewModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("sender"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnAdressekontoViewModelForKreditorEventHandler kaster en ArgumentNullException, hvis argumenter til eventet er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnAdressekontoViewModelForKreditorEventHandlerKasterArgumentNullExceptionHvisEventArgsErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IAdressekontoViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IAdressekontoViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var adressekontoViewModelMock = fixture.Create<IAdressekontoViewModel>();
            regnskabViewModel.KreditorAdd(adressekontoViewModelMock);

            var exception = Assert.Throws<ArgumentNullException>(() => adressekontoViewModelMock.Raise(m => m.PropertyChanged += null, adressekontoViewModelMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("e"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnNyhedViewModelEventHandler rejser PropertyChanged, når ViewModel for en nyhed opdateres.
        /// </summary>
        [Test]
        [TestCase("Nyhedsudgivelsestidspunkt", "Nyheder")]
        [TestCase("Nyhedsaktualitet", "Nyheder")]
        public void TestAtPropertyChangedOnNyhedViewModelEventHandlerRejserPropertyChangedOnNyhedViewModelUpdate(string propertyName, string expectedPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<INyhedViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<INyhedViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var nyhedViewModelMock = fixture.Create<INyhedViewModel>();
            regnskabViewModel.NyhedAdd(nyhedViewModelMock);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, expectedPropertyName, StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            nyhedViewModelMock.Raise(m => m.PropertyChanged += null, nyhedViewModelMock, new PropertyChangedEventArgs(propertyName));
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnNyhedViewModelEventHandler kaster en ArgumentNullException, hvis objektet, der rejser eventet, er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnNyhedViewModelEventHandlerKasterArgumentNullExceptionHvisSenderErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<INyhedViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<INyhedViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var nyhedViewModelMock = fixture.Create<INyhedViewModel>();
            regnskabViewModel.NyhedAdd(nyhedViewModelMock);

            var exception = Assert.Throws<ArgumentNullException>(() => nyhedViewModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("sender"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnNyhedViewModelEventHandler kaster en ArgumentNullException, hvis argumenter til eventet er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnNyhedViewModelEventHandlerKasterArgumentNullExceptionHvisEventArgsErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<INyhedViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<INyhedViewModel>()));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var nyhedViewModelMock = fixture.Create<INyhedViewModel>();
            regnskabViewModel.NyhedAdd(nyhedViewModelMock);

            var exception = Assert.Throws<ArgumentNullException>(() => nyhedViewModelMock.Raise(m => m.PropertyChanged += null, nyhedViewModelMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("e"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnKontogruppeViewModelEventHandler rejser PropertyChanged, når ViewModel for en kontogruppe opdateres.
        /// </summary>
        [Test]
        [TestCase(4, "Nummer", "Kontogrupper")]
        public void TestAtPropertyChangedOnKontogruppeViewModelEventHandlerRejserPropertyChangedOnKontogruppeViewModelUpdate(int nummer, string propertyName, string expectedPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IKontogruppeViewModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(nummer)
                        .Repeat.Any();
                    return mock;
                }));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var kontogruppeViewModelMock = fixture.Create<IKontogruppeViewModel>();
            regnskabViewModel.KontogruppeAdd(kontogruppeViewModelMock);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, expectedPropertyName, StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            kontogruppeViewModelMock.Raise(m => m.PropertyChanged += null, kontogruppeViewModelMock, new PropertyChangedEventArgs(propertyName));
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnKontogruppeViewModelEventHandler kaster en ArgumentNullException, hvis objektet, der rejser eventet, er null.
        /// </summary>
        [Test]
        [TestCase(98)]
        public void TestAtPropertyChangedOnKontogruppeViewModelEventHandlerKasterArgumentNullExceptionHvisSenderErNull(int nummer)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IKontogruppeViewModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(nummer)
                        .Repeat.Any();
                    return mock;
                }));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var kontogruppeViewModelMock = fixture.Create<IKontogruppeViewModel>();
            regnskabViewModel.KontogruppeAdd(kontogruppeViewModelMock);

            var exception = Assert.Throws<ArgumentNullException>(() => kontogruppeViewModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("sender"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnKontogruppeViewModelEventHandler kaster en ArgumentNullException, hvis argumenter til eventet er null.
        /// </summary>
        [Test]
        [TestCase(99)]
        public void TestAtPropertyChangedOnKontogruppeViewModelEventHandlerKasterArgumentNullExceptionHvisEventArgsErNull(int nummer)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IKontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IKontogruppeViewModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(nummer)
                        .Repeat.Any();
                    return mock;
                }));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var kontogruppeViewModelMock = fixture.Create<IKontogruppeViewModel>();
            regnskabViewModel.KontogruppeAdd(kontogruppeViewModelMock);

            var exception = Assert.Throws<ArgumentNullException>(() => kontogruppeViewModelMock.Raise(m => m.PropertyChanged += null, kontogruppeViewModelMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("e"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBudgetkontogruppeViewModelEventHandler rejser PropertyChanged, når ViewModel for en kontogruppe til budgetkonti opdateres.
        /// </summary>
        [Test]
        [TestCase(4, "Nummer", "Budgetkontogrupper")]
        public void TestAtPropertyChangedOnBudgetkontogruppeViewModelEventHandlerRejserPropertyChangedOnBudgetkontogruppeViewModelUpdate(int nummer, string propertyName, string expectedPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(nummer)
                        .Repeat.Any();
                    return mock;
                }));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var budgetkontogruppeViewModelMock = fixture.Create<IBudgetkontogruppeViewModel>();
            regnskabViewModel.BudgetkontogruppeAdd(budgetkontogruppeViewModelMock);

            var eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, expectedPropertyName, StringComparison.Ordinal) == 0)
                    {
                        eventCalled = true;
                    }
                };

            Assert.That(eventCalled, Is.False);
            budgetkontogruppeViewModelMock.Raise(m => m.PropertyChanged += null, budgetkontogruppeViewModelMock, new PropertyChangedEventArgs(propertyName));
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBudgetkontogruppeViewModelEventHandler kaster en ArgumentNullException, hvis objektet, der rejser eventet, er null.
        /// </summary>
        [Test]
        [TestCase(98)]
        public void TestAtPropertyChangedOnBudgetkontogruppeViewModelEventHandlerKasterArgumentNullExceptionHvisSenderErNull(int nummer)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(nummer)
                        .Repeat.Any();
                    return mock;
                }));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var budgetkontogruppeViewModelMock = fixture.Create<IBudgetkontogruppeViewModel>();
            regnskabViewModel.BudgetkontogruppeAdd(budgetkontogruppeViewModelMock);

            var exception = Assert.Throws<ArgumentNullException>(() => budgetkontogruppeViewModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("sender"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBudgetkontogruppeViewModelEventHandler kaster en ArgumentNullException, hvis argumenter til eventet er null.
        /// </summary>
        [Test]
        [TestCase(99)]
        public void TestAtPropertyChangedOnBudgetkontogruppeViewModelEventHandlerKasterArgumentNullExceptionHvisEventArgsErNull(int nummer)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(nummer)
                        .Repeat.Any();
                    return mock;
                }));

            var regnskabViewModel = new RegnskabViewModel(fixture.Create<IRegnskabModel>(), fixture.Create<DateTime>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            var budgetkontogruppeViewModelMock = fixture.Create<IBudgetkontogruppeViewModel>();
            regnskabViewModel.BudgetkontogruppeAdd(budgetkontogruppeViewModelMock);

            var exception = Assert.Throws<ArgumentNullException>(() => budgetkontogruppeViewModelMock.Raise(m => m.PropertyChanged += null, budgetkontogruppeViewModelMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("e"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Danner og returnerer en liste af konti.
        /// </summary>
        /// <returns>Liste af konti.</returns>
        private static IEnumerable<IKontoViewModel> CreateKontoViewModels(ISpecimenBuilder fixture, IEnumerable<IKontogruppeViewModel> kontogrupper, Random random, int count)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }
            if (kontogrupper == null)
            {
                throw new ArgumentNullException("kontogrupper");
            }
            if (random == null)
            {
                throw new ArgumentNullException("random");
            }
            var kontogruppeArray = kontogrupper.ToArray();
            var result = new List<IKontoViewModel>(count);
            while (result.Count < count)
            {
                var kontoViewModelMock = MockRepository.GenerateMock<IKontoViewModel>();
                kontoViewModelMock.Expect(m => m.Kontonummer)
                    .Return(fixture.Create<string>())
                    .Repeat.Any();
                kontoViewModelMock.Expect(m => m.Kontogruppe)
                    .Return(kontogruppeArray.ElementAt(random.Next(kontogruppeArray.Length - 1)))
                    .Repeat.Any();
                kontoViewModelMock.Expect(m => m.Kontoværdi)
                    .Return(fixture.Create<decimal>())
                    .Repeat.Any();
                result.Add(kontoViewModelMock);
            }
            return result;
        }

        /// <summary>
        /// Danner og returnerer en liste af budgetkonti.
        /// </summary>
        /// <returns>Liste af budgetkonti.</returns>
        private static IEnumerable<IBudgetkontoViewModel> CreateBudgetkontoViewModels(ISpecimenBuilder fixture, IEnumerable<IBudgetkontogruppeViewModel> budgetkontogrupper, Random random, int count)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }
            if (budgetkontogrupper == null)
            {
                throw new ArgumentNullException("budgetkontogrupper");
            }
            if (random == null)
            {
                throw new ArgumentNullException("random");
            }
            var budgetkontogrupperArray = budgetkontogrupper.ToArray();
            var result = new List<IBudgetkontoViewModel>(count);
            while (result.Count < count)
            {
                var budgetkontoViewModelMock = MockRepository.GenerateMock<IBudgetkontoViewModel>();
                budgetkontoViewModelMock.Expect(m => m.Kontonummer)
                                        .Return(fixture.Create<string>())
                                        .Repeat.Any();
                budgetkontoViewModelMock.Expect(m => m.Kontogruppe)
                                        .Return(budgetkontogrupperArray.ElementAt(random.Next(budgetkontogrupperArray.Length - 1)))
                                        .Repeat.Any();
                budgetkontoViewModelMock.Expect(m => m.Kontoværdi)
                                        .Return(fixture.Create<decimal>())
                                        .Repeat.Any();
                result.Add(budgetkontoViewModelMock);
            }
            return result;
        }
    }
}
