using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Kernel;
using Moq;
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
            Fixture fixture = new Fixture();

            int nummer = fixture.Create<int>();
            string navn = fixture.Create<string>();
            IRegnskabModel regnskabModel = fixture.BuildRegnskabModel(nummer, navn);

            DateTime statusDato = fixture.Create<DateTime>();
            string statusDatoAsMonthText = statusDato.ToString("MMMM yyyy");
            string statusDatoAsLastMonthText = statusDato.AddMonths(-1).ToString("MMMM yyyy");

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(regnskabModel, statusDato, fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Nummer, Is.EqualTo(nummer));
            Assert.That(regnskabViewModel.Navn, Is.Not.Null);
            Assert.That(regnskabViewModel.Navn, Is.Not.Empty);
            Assert.That(regnskabViewModel.Navn, Is.EqualTo(navn));
            Assert.That(regnskabViewModel.DisplayName, Is.Not.Null);
            Assert.That(regnskabViewModel.DisplayName, Is.Not.Empty);
            Assert.That(regnskabViewModel.DisplayName, Is.EqualTo(navn));
            Assert.That(regnskabViewModel.StatusDato, Is.EqualTo(statusDato));
            Assert.That(regnskabViewModel.StatusDatoAsMonthText, Is.Not.Null);
            Assert.That(regnskabViewModel.StatusDatoAsMonthText, Is.Not.Empty);
            Assert.That(regnskabViewModel.StatusDatoAsMonthText, Is.EqualTo($"{statusDatoAsMonthText.Substring(0, 1).ToUpper()}{statusDatoAsMonthText.Substring(1).ToLower()}"));
            Assert.That(regnskabViewModel.StatusDatoAsLastMonthText, Is.Not.Null);
            Assert.That(regnskabViewModel.StatusDatoAsLastMonthText, Is.Not.Empty);
            Assert.That(regnskabViewModel.StatusDatoAsLastMonthText, Is.EqualTo($"{statusDatoAsLastMonthText.Substring(0, 1).ToUpper()}{statusDatoAsLastMonthText.Substring(1).ToLower()}"));
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
            Assert.That(regnskabViewModel.BudgetkontiColumns.ElementAt(3), Is.EqualTo(Resource.GetText(Text.Posted)));
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
            Assert.That(regnskabViewModel.BogføringslinjerColumns.ElementAt(1), Is.EqualTo(Resource.GetText(Text.Reference)));
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
            Assert.That(regnskabViewModel.OpgørelseslinjerHeaders, Is.Not.Null);
            Assert.That(regnskabViewModel.OpgørelseslinjerHeaders, Is.Not.Empty);
            Assert.That(regnskabViewModel.OpgørelseslinjerHeaders.Count(), Is.EqualTo(2));
            Assert.That(regnskabViewModel.OpgørelseslinjerHeaders.ElementAt(0), Is.Not.Null);
            Assert.That(regnskabViewModel.OpgørelseslinjerHeaders.ElementAt(0), Is.Not.Empty);
            Assert.That(regnskabViewModel.OpgørelseslinjerHeaders.ElementAt(0), Is.EqualTo(Resource.GetText(Text.MonthlyStatement)));
            Assert.That(regnskabViewModel.OpgørelseslinjerHeaders.ElementAt(1), Is.Not.Null);
            Assert.That(regnskabViewModel.OpgørelseslinjerHeaders.ElementAt(1), Is.Not.Empty);
            Assert.That(regnskabViewModel.OpgørelseslinjerHeaders.ElementAt(1), Is.EqualTo(Resource.GetText(Text.AnnualStatement)));
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
            Assert.That(regnskabViewModel.OpgørelseslinjerColumns.ElementAt(2), Is.EqualTo(Resource.GetText(Text.Posted)));
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
            Assert.That(regnskabViewModel.BogførtLabel, Is.EqualTo(Resource.GetText(Text.Posted)));
            Assert.That(regnskabViewModel.BogførtSidsteMåned, Is.EqualTo(0M));
            Assert.That(regnskabViewModel.BogførtSidsteMånedAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.BogførtSidsteMånedAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.BogførtSidsteMånedAsText, Is.EqualTo(Convert.ToDecimal(0M).ToString("C")));
            Assert.That(regnskabViewModel.BogførtSidsteMånedLabel, Is.Not.Null);
            Assert.That(regnskabViewModel.BogførtSidsteMånedLabel, Is.Not.Empty);
            Assert.That(regnskabViewModel.BogførtSidsteMånedLabel, Is.EqualTo(Resource.GetText(Text.PostedLastMonth)));
            Assert.That(regnskabViewModel.BogførtÅrTilDato, Is.EqualTo(0M));
            Assert.That(regnskabViewModel.BogførtÅrTilDatoAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.BogførtÅrTilDatoAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.BogførtÅrTilDatoAsText, Is.EqualTo(Convert.ToDecimal(0M).ToString("C")));
            Assert.That(regnskabViewModel.BogførtÅrTilDatoLabel, Is.Not.Null);
            Assert.That(regnskabViewModel.BogførtÅrTilDatoLabel, Is.Not.Empty);
            Assert.That(regnskabViewModel.BogførtÅrTilDatoLabel, Is.EqualTo(Resource.GetText(Text.PostedYearToDate)));
            Assert.That(regnskabViewModel.BogførtSidsteÅr, Is.EqualTo(0M));
            Assert.That(regnskabViewModel.BogførtSidsteÅrAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.BogførtSidsteÅrAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.BogførtSidsteÅrAsText, Is.EqualTo(Convert.ToDecimal(0M).ToString("C")));
            Assert.That(regnskabViewModel.BogførtSidsteÅrLabel, Is.Not.Null);
            Assert.That(regnskabViewModel.BogførtSidsteÅrLabel, Is.Not.Empty);
            Assert.That(regnskabViewModel.BogførtSidsteÅrLabel, Is.EqualTo(Resource.GetText(Text.PostedLastYear)));
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
            Fixture fixture = new Fixture();

            // ReSharper disable ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new RegnskabViewModel(null, fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel()));
            // ReSharper restore ObjectCreationAsStatement
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis repositoryet til finansstyring er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFinansstyringRepositoryErNull()
        {
            Fixture fixture = new Fixture();

            // ReSharper disable ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), null, fixture.BuildExceptionHandlerViewModel()));
            // ReSharper restore ObjectCreationAsStatement
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis ViewModel for exceptionhandleren er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisExceptionHandlerViewModelErNull()
        {
            Fixture fixture = new Fixture();

            // ReSharper disable ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), null));
            // ReSharper restore ObjectCreationAsStatement
        }

        /// <summary>
        /// Tester, at sætteren på Navn opdaterer Navn på modellen for regnskabet.
        /// </summary>
        [Test]
        public void TestAtNavnSetterOpdatererNavnOnRegnskabModel()
        {
            Fixture fixture = new Fixture();

            Mock<IRegnskabModel> regnskabModelMock = fixture.BuildRegnskabModelMock();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(regnskabModelMock.Object, fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            string newValue = fixture.Create<string>();
            regnskabViewModel.Navn = newValue;

            regnskabModelMock.VerifySet(m => m.Navn = It.Is<string>(value => string.CompareOrdinal(value, newValue) == 0));
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
            Fixture fixture = new Fixture();

            DateTime statusDato = DateTime.Parse(originalDateTime, new CultureInfo("en-US"));
            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), statusDato, fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
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
            Fixture fixture = new Fixture();

            DateTime statusDato = DateTime.Parse(originalDateTime, new CultureInfo("en-US"));
            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), statusDato, fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.StatusDato, Is.EqualTo(statusDato));

            DateTime newValue = DateTime.Parse(newDateTime, new CultureInfo("en-US"));
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
            Fixture fixture = new Fixture();

            DateTime statusDato = DateTime.Parse(originalDateTime, new CultureInfo("en-US"));
            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), statusDato, fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            bool eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Empty);
                if (string.CompareOrdinal(e.PropertyName, expectPropertyName) == 0)
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
            Fixture fixture = new Fixture();

            IKontogruppeViewModel[] kontogruppeViewModelCollection = kontogrupper.Select(id =>
                {
                    IBalanceViewModel balanceViewModel = fixture.BuildBalanceViewModel(id, balanceType: (Balancetype)Enum.Parse(typeof(Balancetype), balanceType));
                    return fixture.BuildKontogruppeViewModel(id, balanceViewModel: balanceViewModel);
                })
                .ToArray();
            IEnumerable<IKontoViewModel> kontoViewModelCollection = CreateKontoViewModels(fixture, kontogruppeViewModelCollection, new Random(fixture.Create<int>()), 250);

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Konti, Is.Not.Null);
            Assert.That(regnskabViewModel.Konti, Is.Empty);
            Assert.That(regnskabViewModel.KontiGrouped, Is.Not.Null);
            Assert.That(regnskabViewModel.KontiGrouped, Is.Empty);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            regnskabViewModel.BogføringSet(fixture.BuildBogføringViewModel());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            foreach (IKontogruppeViewModel kontogruppeViewModel in kontogruppeViewModelCollection)
            {
                regnskabViewModel.KontogruppeAdd(kontogruppeViewModel);
            }
            foreach (IKontoViewModel kontoViewModel in kontoViewModelCollection)
            {
                regnskabViewModel.KontoAdd(kontoViewModel);
            }
            Assert.That(regnskabViewModel.Konti, Is.Not.Null);
            Assert.That(regnskabViewModel.Konti, Is.Not.Empty);

            IEnumerable<KeyValuePair<IKontogruppeViewModel, IEnumerable<IKontoViewModel>>> kontiGrouped = regnskabViewModel.KontiGrouped;
            // ReSharper disable PossibleMultipleEnumeration
            Assert.That(kontiGrouped, Is.Not.Null);
            Assert.That(kontiGrouped, Is.Not.Empty);
            Assert.That(kontiGrouped.Sum(m => m.Value.Count()), Is.EqualTo(regnskabViewModel.Konti.Count()));
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Tester, at getteren til KontiTop udelader konti, hvor kontoværdien er 0.
        /// </summary>
        [Test]
        [TestCase("Aktiver")]
        [TestCase("Passiver")]
        public void TestAtKontiTopGetterUdeladerKontiHvorKontoValueErNul(string balanceType)
        {
            Fixture fixture = new Fixture();

            List<IKontoViewModel> kontoViewModelCollection = new List<IKontoViewModel>(250);
            while (kontoViewModelCollection.Count < kontoViewModelCollection.Capacity)
            {
                IBalanceViewModel balanceViewModel = fixture.BuildBalanceViewModel(balanceType: (Balancetype)Enum.Parse(typeof(Balancetype), balanceType));
                IKontogruppeViewModel kontogruppeViewModel = fixture.BuildKontogruppeViewModel(balanceViewModel: balanceViewModel);
                kontoViewModelCollection.Add(fixture.BuildKontoViewModel(kontogruppeViewModel: kontogruppeViewModel, kontoværdi: 0M));
            }

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Konti, Is.Not.Null);
            Assert.That(regnskabViewModel.Konti, Is.Empty);
            Assert.That(regnskabViewModel.KontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.KontiTop, Is.Empty);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            regnskabViewModel.BogføringSet(fixture.BuildBogføringViewModel());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            kontoViewModelCollection.ForEach(regnskabViewModel.KontoAdd);
            Assert.That(regnskabViewModel.Konti, Is.Not.Null);
            Assert.That(regnskabViewModel.Konti, Is.Not.Empty);
            Assert.That(regnskabViewModel.Konti.Count(), Is.EqualTo(kontoViewModelCollection.Count));
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
            Fixture fixture = new Fixture();

            List<IKontoViewModel> kontoViewModelCollection = new List<IKontoViewModel>(250);
            while (kontoViewModelCollection.Count < kontoViewModelCollection.Capacity)
            {
                IBalanceViewModel balanceViewModel = fixture.BuildBalanceViewModel(balanceType: (Balancetype)Enum.Parse(typeof(Balancetype), balanceType));
                IKontogruppeViewModel kontogruppeViewModel = fixture.BuildKontogruppeViewModel(balanceViewModel: balanceViewModel);
                kontoViewModelCollection.Add(fixture.BuildKontoViewModel(kontogruppeViewModel: kontogruppeViewModel, kontoværdi: fixture.Create<decimal>()));
            }

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Konti, Is.Not.Null);
            Assert.That(regnskabViewModel.Konti, Is.Empty);
            Assert.That(regnskabViewModel.KontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.KontiTop, Is.Empty);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            regnskabViewModel.BogføringSet(fixture.BuildBogføringViewModel());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            kontoViewModelCollection.ForEach(regnskabViewModel.KontoAdd);
            Assert.That(regnskabViewModel.Konti, Is.Not.Null);
            Assert.That(regnskabViewModel.Konti, Is.Not.Empty);
            Assert.That(regnskabViewModel.Konti.Count(), Is.EqualTo(kontoViewModelCollection.Count));
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
            Fixture fixture = new Fixture();

            IKontogruppeViewModel[] kontogruppeViewModelCollection = kontogrupper.Select(id =>
                {
                    IBalanceViewModel balanceViewModel = fixture.BuildBalanceViewModel(id, balanceType: (Balancetype)Enum.Parse(typeof(Balancetype), balanceType));
                    return fixture.BuildKontogruppeViewModel(id, balanceViewModel: balanceViewModel);
                })
                .ToArray();
            IEnumerable<IKontoViewModel> kontoViewModelCollection = CreateKontoViewModels(fixture, kontogruppeViewModelCollection, new Random(fixture.Create<int>()), 250);

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.KontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.KontiTop, Is.Empty);
            Assert.That(regnskabViewModel.KontiTopGrouped, Is.Not.Null);
            Assert.That(regnskabViewModel.KontiTopGrouped, Is.Empty);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            regnskabViewModel.BogføringSet(fixture.BuildBogføringViewModel());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            foreach (IKontogruppeViewModel kontogruppeViewModel in kontogruppeViewModelCollection)
            {
                regnskabViewModel.KontogruppeAdd(kontogruppeViewModel);
            }
            foreach (IKontoViewModel kontoViewModel in kontoViewModelCollection)
            {
                regnskabViewModel.KontoAdd(kontoViewModel);
            }
            Assert.That(regnskabViewModel.Konti, Is.Not.Null);
            Assert.That(regnskabViewModel.Konti, Is.Not.Empty);

            IEnumerable<KeyValuePair<IKontogruppeViewModel, IEnumerable<IKontoViewModel>>> kontiTopGrouped = regnskabViewModel.KontiTopGrouped;
            // ReSharper disable PossibleMultipleEnumeration
            Assert.That(kontiTopGrouped, Is.Not.Null);
            Assert.That(kontiTopGrouped, Is.Not.Empty);
            Assert.That(kontiTopGrouped.Sum(m => m.Value.Count()), Is.EqualTo(regnskabViewModel.KontiTop.Count()));
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Tester, at getteren til BudgetkontiGroup returnerer grupperede budgetkonti.
        /// </summary>
        [Test]
        [TestCase(new[] {5, 6, 7, 8, 9})]
        public void TestAtBudgetkontiGroupedGetterReturnererDictionary(int[] budgetkontogrupper)
        {
            Fixture fixture = new Fixture();

            IBudgetkontogruppeViewModel[] budgetkontogruppeViewModelCollection = budgetkontogrupper.Select(id =>
                {
                    IOpgørelseViewModel opgørelseViewModel = fixture.BuildOpgørelseViewModel(id);
                    return fixture.BuildBudgetkontogruppeViewModel(id, opgørelseViewModel: opgørelseViewModel);
                })
                .ToArray();
            IEnumerable<IBudgetkontoViewModel> budgetkontoViewModelCollection = CreateBudgetkontoViewModels(fixture, budgetkontogruppeViewModelCollection, new Random(fixture.Create<int>()), 250).ToList();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Empty);
            Assert.That(regnskabViewModel.BudgetkontiGrouped, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiGrouped, Is.Empty);

            foreach (IBudgetkontogruppeViewModel budgetkontogruppeViewModel in budgetkontogruppeViewModelCollection)
            {
                regnskabViewModel.BudgetkontogruppeAdd(budgetkontogruppeViewModel);
            }
            foreach (IBudgetkontoViewModel budgetkontoViewModel in budgetkontoViewModelCollection)
            {
                regnskabViewModel.BudgetkontoAdd(budgetkontoViewModel);
            }
            Assert.That(regnskabViewModel.Budgetkonti, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Not.Empty);

            IEnumerable<KeyValuePair<IBudgetkontogruppeViewModel, IEnumerable<IBudgetkontoViewModel>>> budgetkontiGrouped = regnskabViewModel.BudgetkontiGrouped;
            // ReSharper disable PossibleMultipleEnumeration
            Assert.That(budgetkontiGrouped, Is.Not.Null);
            Assert.That(budgetkontiGrouped, Is.Not.Empty);
            Assert.That(budgetkontiGrouped.Sum(m => m.Value.Count()), Is.EqualTo(regnskabViewModel.Budgetkonti.Count()));
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Tester, at getteren til BudgetkontiTop udelader budgetkonti, hvor kontoværdien er 0.
        /// </summary>
        [Test]
        public void TestAtBudgetkontiTopGetterUdeladerBudgetkontiHvorKontoValueErNul()
        {
            Fixture fixture = new Fixture();

            List<IBudgetkontoViewModel> budgetkontoViewModelCollection = new List<IBudgetkontoViewModel>(250);
            while (budgetkontoViewModelCollection.Count < budgetkontoViewModelCollection.Capacity)
            {
                IOpgørelseViewModel opgørelseViewModel = fixture.BuildOpgørelseViewModel();
                IBudgetkontogruppeViewModel budgetkontogruppeViewModel = fixture.BuildBudgetkontogruppeViewModel(opgørelseViewModel: opgørelseViewModel);
                budgetkontoViewModelCollection.Add(fixture.BuildBudgetkontoViewModel(budgetkontogruppeViewModel: budgetkontogruppeViewModel, kontoværdi: 0M));
            }

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Empty);
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Empty);

            budgetkontoViewModelCollection.ForEach(regnskabViewModel.BudgetkontoAdd);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Not.Empty);
            Assert.That(regnskabViewModel.Budgetkonti.Count(), Is.EqualTo(budgetkontoViewModelCollection.Count));
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Empty);
        }

        /// <summary>
        /// Tester, at getteren til BudgetkontiTop returnerer topbenyttede budgetkonti.
        /// </summary>
        [Test]
        public void TestAtBudgetkontiTopGetterReturnererTopbenyttedeBudgetkonti()
        {
            Fixture fixture = new Fixture();

            List<IBudgetkontoViewModel> budgetkontoViewModelCollection = new List<IBudgetkontoViewModel>(250);
            while (budgetkontoViewModelCollection.Count < budgetkontoViewModelCollection.Capacity)
            {
                IOpgørelseViewModel opgørelseViewModel = fixture.BuildOpgørelseViewModel();
                IBudgetkontogruppeViewModel budgetkontogruppeViewModel = fixture.BuildBudgetkontogruppeViewModel(opgørelseViewModel: opgørelseViewModel);
                budgetkontoViewModelCollection.Add(fixture.BuildBudgetkontoViewModel(budgetkontogruppeViewModel: budgetkontogruppeViewModel, kontoværdi: fixture.Create<decimal>()));
            }

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Empty);
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Empty);

            budgetkontoViewModelCollection.ForEach(regnskabViewModel.BudgetkontoAdd);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Not.Empty);
            Assert.That(regnskabViewModel.Budgetkonti.Count(), Is.EqualTo(budgetkontoViewModelCollection.Count));
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
            Fixture fixture = new Fixture();

            IBudgetkontogruppeViewModel[] budgetkontogruppeViewModelCollection = budgetkontogrupper.Select(id =>
                {
                    IOpgørelseViewModel opgørelseViewModel = fixture.BuildOpgørelseViewModel(id);
                    return fixture.BuildBudgetkontogruppeViewModel(id, opgørelseViewModel: opgørelseViewModel);
                })
                .ToArray();
            IEnumerable<IBudgetkontoViewModel> budgetkontoViewModelCollection = CreateBudgetkontoViewModels(fixture, budgetkontogruppeViewModelCollection, new Random(fixture.Create<int>()), 250).ToList();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Empty);
            Assert.That(regnskabViewModel.BudgetkontiTopGrouped, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiTopGrouped, Is.Empty);

            foreach (IBudgetkontogruppeViewModel budgetkontogruppeViewModel in budgetkontogruppeViewModelCollection)
            {
                regnskabViewModel.BudgetkontogruppeAdd(budgetkontogruppeViewModel);
            }
            foreach (IBudgetkontoViewModel budgetkontoViewModel in budgetkontoViewModelCollection)
            {
                regnskabViewModel.BudgetkontoAdd(budgetkontoViewModel);
            }
            Assert.That(regnskabViewModel.Budgetkonti, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Not.Empty);

            IEnumerable<KeyValuePair<IBudgetkontogruppeViewModel, IEnumerable<IBudgetkontoViewModel>>> budgetkontiTopGrouped = regnskabViewModel.BudgetkontiTopGrouped;
            // ReSharper disable PossibleMultipleEnumeration
            Assert.That(budgetkontiTopGrouped, Is.Not.Null);
            Assert.That(budgetkontiTopGrouped, Is.Not.Empty);
            Assert.That(budgetkontiTopGrouped.Sum(m => m.Value.Count()), Is.EqualTo(regnskabViewModel.BudgetkontiTop.Count()));
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Tester, at getteren til AktiverIAlt returnerer aktiver i alt.
        /// </summary>
        [Test]
        [TestCase(new[] {5, 6, 7, 8, 9})]
        public void TestAtAktiverIAltGetterReturnererAktiverIAlt(int[] kontogrupper)
        {
            Fixture fixture = new Fixture();

            IKontogruppeViewModel[] kontogruppeViewModelCollection = kontogrupper.Select(id =>
                {
                    IBalanceViewModel balanceViewModel = fixture.BuildBalanceViewModel(id, balanceType: Balancetype.Aktiver, saldo: fixture.Create<decimal>());
                    return fixture.BuildKontogruppeViewModel(id, balanceViewModel: balanceViewModel);
                })
                .ToArray();
            IKontoViewModel[] kontoViewModelCollection = CreateKontoViewModels(fixture, kontogruppeViewModelCollection, new Random(fixture.Create<int>()), 250).ToArray();

            IEnumerable<int> kontogrupperInUse = kontoViewModelCollection
                .GroupBy(m => m.Kontogruppe.Nummer)
                .Select(m => m.Key)
                .ToArray();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.AktiverIAlt, Is.EqualTo(0M));
            Assert.That(regnskabViewModel.AktiverIAltAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.AktiverIAltAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.AktiverIAltAsText, Is.EqualTo(Convert.ToDecimal(0M).ToString("C")));
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            decimal expectedValue = kontogruppeViewModelCollection
                .Where(m => kontogrupperInUse.Contains(m.Nummer))
                .Select(m => m.CreateBalancelinje(regnskabViewModel))
                .Sum(m => m.Saldo);

            regnskabViewModel.BogføringSet(fixture.Create<IBogføringViewModel>());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            foreach (IKontogruppeViewModel kontogruppeViewModel in kontogruppeViewModelCollection)
            {
                regnskabViewModel.KontogruppeAdd(kontogruppeViewModel);
            }
            foreach (IKontoViewModel kontoViewModel in kontoViewModelCollection)
            {
                regnskabViewModel.KontoAdd(kontoViewModel);
            }

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
            Fixture fixture = new Fixture();

            IKontogruppeViewModel[] kontogruppeViewModelCollection = kontogrupper.Select(id =>
                {
                    IBalanceViewModel balanceViewModel = fixture.BuildBalanceViewModel(id, balanceType: Balancetype.Passiver, saldo: fixture.Create<decimal>());
                    return fixture.BuildKontogruppeViewModel(id, balanceViewModel: balanceViewModel);
                })
                .ToArray();
            IKontoViewModel[] kontoViewModelCollection = CreateKontoViewModels(fixture, kontogruppeViewModelCollection, new Random(fixture.Create<int>()), 250).ToArray();

            IEnumerable<int> kontogrupperInUse = kontoViewModelCollection
                .GroupBy(m => m.Kontogruppe.Nummer)
                .Select(m => m.Key)
                .ToArray();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.PassiverIAlt, Is.EqualTo(0M));
            Assert.That(regnskabViewModel.PassiverIAltAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.PassiverIAltAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.PassiverIAltAsText, Is.EqualTo(Convert.ToDecimal(0M).ToString("C")));

            decimal expectedValue = kontogruppeViewModelCollection
                .Where(m => kontogrupperInUse.Contains(m.Nummer))
                .Select(m => m.CreateBalancelinje(regnskabViewModel))
                .Sum(m => m.Saldo);

            regnskabViewModel.BogføringSet(fixture.Create<IBogføringViewModel>());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            foreach (IKontogruppeViewModel kontogruppeViewModel in kontogruppeViewModelCollection)
            {
                regnskabViewModel.KontogruppeAdd(kontogruppeViewModel);
            }
            foreach (IKontoViewModel kontoViewModel in kontoViewModelCollection)
            {
                regnskabViewModel.KontoAdd(kontoViewModel);
            }

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
            Fixture fixture = new Fixture();

            IBudgetkontogruppeViewModel[] budgetkontogruppeViewModelCollection = budgetkontogrupper.Select(id =>
                {
                    IOpgørelseViewModel opgørelseViewModel = fixture.BuildOpgørelseViewModel(id, budget: fixture.Create<decimal>());
                    return fixture.BuildBudgetkontogruppeViewModel(id, opgørelseViewModel: opgørelseViewModel);
                })
                .ToArray();
            IEnumerable<IBudgetkontoViewModel> budgetkontoViewModelCollection = CreateBudgetkontoViewModels(fixture, budgetkontogruppeViewModelCollection, new Random(fixture.Create<int>()), 250).ToList();

            IEnumerable<int> budgetkontogrupperInUse = budgetkontoViewModelCollection
                .GroupBy(m => m.Kontogruppe.Nummer)
                .Select(m => m.Key)
                .ToList();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Budget, Is.EqualTo(0M));
            Assert.That(regnskabViewModel.BudgetAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.BudgetAsText, Is.EqualTo(Convert.ToDecimal(0M).ToString("C")));

            decimal expectedValue = budgetkontogruppeViewModelCollection
                .Where(m => budgetkontogrupperInUse.Contains(m.Nummer))
                .Select(m => m.CreateOpgørelseslinje(regnskabViewModel))
                .Sum(m => m.Budget);

            foreach (IBudgetkontogruppeViewModel budgetkontogruppeViewModel in budgetkontogruppeViewModelCollection)
            {
                regnskabViewModel.BudgetkontogruppeAdd(budgetkontogruppeViewModel);
            }
            foreach (IBudgetkontoViewModel budgetkontoViewModel in budgetkontoViewModelCollection)
            {
                regnskabViewModel.BudgetkontoAdd(budgetkontoViewModel);
            }

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
            Fixture fixture = new Fixture();

            IBudgetkontogruppeViewModel[] budgetkontogruppeViewModelCollection = budgetkontogrupper.Select(id =>
                {
                    IOpgørelseViewModel opgørelseViewModel = fixture.BuildOpgørelseViewModel(id, budgetSidsteMåned: fixture.Create<decimal>());
                    return fixture.BuildBudgetkontogruppeViewModel(id, opgørelseViewModel: opgørelseViewModel);
                })
                .ToArray();
            IEnumerable<IBudgetkontoViewModel> budgetkontoViewModelCollection = CreateBudgetkontoViewModels(fixture, budgetkontogruppeViewModelCollection, new Random(fixture.Create<int>()), 250).ToList();

            IEnumerable<int> budgetkontogrupperInUse = budgetkontoViewModelCollection
                .GroupBy(m => m.Kontogruppe.Nummer)
                .Select(m => m.Key)
                .ToList();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetSidsteMåned, Is.EqualTo(0M));
            Assert.That(regnskabViewModel.BudgetSidsteMånedAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetSidsteMånedAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.BudgetSidsteMånedAsText, Is.EqualTo(Convert.ToDecimal(0M).ToString("C")));

            decimal expectedValue = budgetkontogruppeViewModelCollection
                .Where(m => budgetkontogrupperInUse.Contains(m.Nummer))
                .Select(m => m.CreateOpgørelseslinje(regnskabViewModel))
                .Sum(m => m.BudgetSidsteMåned);

            foreach (IBudgetkontogruppeViewModel budgetkontogruppeViewModel in budgetkontogruppeViewModelCollection)
            {
                regnskabViewModel.BudgetkontogruppeAdd(budgetkontogruppeViewModel);
            }
            foreach (IBudgetkontoViewModel budgetkontoViewModel in budgetkontoViewModelCollection)
            {
                regnskabViewModel.BudgetkontoAdd(budgetkontoViewModel);
            }

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
            Fixture fixture = new Fixture();

            IBudgetkontogruppeViewModel[] budgetkontogruppeViewModelCollection = budgetkontogrupper.Select(id =>
                {
                    IOpgørelseViewModel opgørelseViewModel = fixture.BuildOpgørelseViewModel(id, budgetÅrTilDato: fixture.Create<decimal>());
                    return fixture.BuildBudgetkontogruppeViewModel(id, opgørelseViewModel: opgørelseViewModel);
                })
                .ToArray();
            IEnumerable<IBudgetkontoViewModel> budgetkontoViewModelCollection = CreateBudgetkontoViewModels(fixture, budgetkontogruppeViewModelCollection, new Random(fixture.Create<int>()), 250).ToList();

            IEnumerable<int> budgetkontogrupperInUse = budgetkontoViewModelCollection
                .GroupBy(m => m.Kontogruppe.Nummer)
                .Select(m => m.Key)
                .ToList();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetÅrTilDato, Is.EqualTo(0M));
            Assert.That(regnskabViewModel.BudgetÅrTilDatoAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetÅrTilDatoAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.BudgetÅrTilDatoAsText, Is.EqualTo(Convert.ToDecimal(0M).ToString("C")));

            decimal expectedValue = budgetkontogruppeViewModelCollection
                .Where(m => budgetkontogrupperInUse.Contains(m.Nummer))
                .Select(m => m.CreateOpgørelseslinje(regnskabViewModel))
                .Sum(m => m.BudgetÅrTilDato);

            foreach (IBudgetkontogruppeViewModel budgetkontogruppeViewModel in budgetkontogruppeViewModelCollection)
            {
                regnskabViewModel.BudgetkontogruppeAdd(budgetkontogruppeViewModel);
            }
            foreach (IBudgetkontoViewModel budgetkontoViewModel in budgetkontoViewModelCollection)
            {
                regnskabViewModel.BudgetkontoAdd(budgetkontoViewModel);
            }

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
            Fixture fixture = new Fixture();

            IBudgetkontogruppeViewModel[] budgetkontogruppeViewModelCollection = budgetkontogrupper.Select(id =>
                {
                    IOpgørelseViewModel opgørelseViewModel = fixture.BuildOpgørelseViewModel(id, budgetSidsteÅr: fixture.Create<decimal>());
                    return fixture.BuildBudgetkontogruppeViewModel(id, opgørelseViewModel: opgørelseViewModel);
                })
                .ToArray();
            IEnumerable<IBudgetkontoViewModel> budgetkontoViewModelCollection = CreateBudgetkontoViewModels(fixture, budgetkontogruppeViewModelCollection, new Random(fixture.Create<int>()), 250).ToList();

            IEnumerable<int> budgetkontogrupperInUse = budgetkontoViewModelCollection
                .GroupBy(m => m.Kontogruppe.Nummer)
                .Select(m => m.Key)
                .ToList();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetSidsteÅr, Is.EqualTo(0M));
            Assert.That(regnskabViewModel.BudgetSidsteÅrAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetSidsteÅrAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.BudgetSidsteÅrAsText, Is.EqualTo(Convert.ToDecimal(0M).ToString("C")));

            decimal expectedValue = budgetkontogruppeViewModelCollection
                .Where(m => budgetkontogrupperInUse.Contains(m.Nummer))
                .Select(m => m.CreateOpgørelseslinje(regnskabViewModel))
                .Sum(m => m.BudgetSidsteÅr);

            foreach (IBudgetkontogruppeViewModel budgetkontogruppeViewModel in budgetkontogruppeViewModelCollection)
            {
                regnskabViewModel.BudgetkontogruppeAdd(budgetkontogruppeViewModel);
            }
            foreach (IBudgetkontoViewModel budgetkontoViewModel in budgetkontoViewModelCollection)
            {
                regnskabViewModel.BudgetkontoAdd(budgetkontoViewModel);
            }

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
            Fixture fixture = new Fixture();

            IBudgetkontogruppeViewModel[] budgetkontogruppeViewModelCollection = budgetkontogrupper.Select(id =>
                {
                    IOpgørelseViewModel opgørelseViewModel = fixture.BuildOpgørelseViewModel(id, bogført: fixture.Create<decimal>());
                    return fixture.BuildBudgetkontogruppeViewModel(id, opgørelseViewModel: opgørelseViewModel);
                })
                .ToArray();
            IEnumerable<IBudgetkontoViewModel> budgetkontoViewModelCollection = CreateBudgetkontoViewModels(fixture, budgetkontogruppeViewModelCollection, new Random(fixture.Create<int>()), 250).ToList();

            IEnumerable<int> budgetkontogrupperInUse = budgetkontoViewModelCollection
                .GroupBy(m => m.Kontogruppe.Nummer)
                .Select(m => m.Key)
                .ToList();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogført, Is.EqualTo(0M));
            Assert.That(regnskabViewModel.BogførtAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.BogførtAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.BogførtAsText, Is.EqualTo(Convert.ToDecimal(0M).ToString("C")));

            decimal expectedValue = budgetkontogruppeViewModelCollection
                .Where(m => budgetkontogrupperInUse.Contains(m.Nummer))
                .Select(m => m.CreateOpgørelseslinje(regnskabViewModel))
                .Sum(m => m.Bogført);

            foreach (IBudgetkontogruppeViewModel budgetkontogruppeViewModel in budgetkontogruppeViewModelCollection)
            {
                regnskabViewModel.BudgetkontogruppeAdd(budgetkontogruppeViewModel);
            }
            foreach (IBudgetkontoViewModel budgetkontoViewModel in budgetkontoViewModelCollection)
            {
                regnskabViewModel.BudgetkontoAdd(budgetkontoViewModel);
            }

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
            Fixture fixture = new Fixture();

            IBudgetkontogruppeViewModel[] budgetkontogruppeViewModelCollection = budgetkontogrupper.Select(id =>
                {
                    IOpgørelseViewModel opgørelseViewModel = fixture.BuildOpgørelseViewModel(id, bogførtSidsteMåned: fixture.Create<decimal>());
                    return fixture.BuildBudgetkontogruppeViewModel(id, opgørelseViewModel: opgørelseViewModel);
                })
                .ToArray();
            IEnumerable<IBudgetkontoViewModel> budgetkontoViewModelCollection = CreateBudgetkontoViewModels(fixture, budgetkontogruppeViewModelCollection, new Random(fixture.Create<int>()), 250).ToList();

            IEnumerable<int> budgetkontogrupperInUse = budgetkontoViewModelCollection
                .GroupBy(m => m.Kontogruppe.Nummer)
                .Select(m => m.Key)
                .ToList();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.BogførtSidsteMåned, Is.EqualTo(0M));
            Assert.That(regnskabViewModel.BogførtSidsteMånedAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.BogførtSidsteMånedAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.BogførtSidsteMånedAsText, Is.EqualTo(Convert.ToDecimal(0M).ToString("C")));

            decimal expectedValue = budgetkontogruppeViewModelCollection
                .Where(m => budgetkontogrupperInUse.Contains(m.Nummer))
                .Select(m => m.CreateOpgørelseslinje(regnskabViewModel))
                .Sum(m => m.BogførtSidsteMåned);

            foreach (IBudgetkontogruppeViewModel budgetkontogruppeViewModel in budgetkontogruppeViewModelCollection)
            {
                regnskabViewModel.BudgetkontogruppeAdd(budgetkontogruppeViewModel);
            }
            foreach (IBudgetkontoViewModel budgetkontoViewModel in budgetkontoViewModelCollection)
            {
                regnskabViewModel.BudgetkontoAdd(budgetkontoViewModel);
            }

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
            Fixture fixture = new Fixture();

            IBudgetkontogruppeViewModel[] budgetkontogruppeViewModelCollection = budgetkontogrupper.Select(id =>
                {
                    IOpgørelseViewModel opgørelseViewModel = fixture.BuildOpgørelseViewModel(id, bogførtÅrTilDato: fixture.Create<decimal>());
                    return fixture.BuildBudgetkontogruppeViewModel(id, opgørelseViewModel: opgørelseViewModel);
                })
                .ToArray();
            IEnumerable<IBudgetkontoViewModel> budgetkontoViewModelCollection = CreateBudgetkontoViewModels(fixture, budgetkontogruppeViewModelCollection, new Random(fixture.Create<int>()), 250).ToList();

            IEnumerable<int> budgetkontogrupperInUse = budgetkontoViewModelCollection
                .GroupBy(m => m.Kontogruppe.Nummer)
                .Select(m => m.Key)
                .ToList();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.BogførtÅrTilDato, Is.EqualTo(0M));
            Assert.That(regnskabViewModel.BogførtÅrTilDatoAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.BogførtÅrTilDatoAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.BogførtÅrTilDatoAsText, Is.EqualTo(Convert.ToDecimal(0M).ToString("C")));

            decimal expectedValue = budgetkontogruppeViewModelCollection
                .Where(m => budgetkontogrupperInUse.Contains(m.Nummer))
                .Select(m => m.CreateOpgørelseslinje(regnskabViewModel))
                .Sum(m => m.BogførtÅrTilDato);

            foreach (IBudgetkontogruppeViewModel budgetkontogruppeViewModel in budgetkontogruppeViewModelCollection)
            {
                regnskabViewModel.BudgetkontogruppeAdd(budgetkontogruppeViewModel);
            }
            foreach (IBudgetkontoViewModel budgetkontoViewModel in budgetkontoViewModelCollection)
            {
                regnskabViewModel.BudgetkontoAdd(budgetkontoViewModel);
            }

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
            Fixture fixture = new Fixture();

            IBudgetkontogruppeViewModel[] budgetkontogruppeViewModelCollection = budgetkontogrupper.Select(id =>
                {
                    IOpgørelseViewModel opgørelseViewModel = fixture.BuildOpgørelseViewModel(id, bogførtSidsteÅr: fixture.Create<decimal>());
                    return fixture.BuildBudgetkontogruppeViewModel(id, opgørelseViewModel: opgørelseViewModel);
                })
                .ToArray();
            IEnumerable<IBudgetkontoViewModel> budgetkontoViewModelCollection = CreateBudgetkontoViewModels(fixture, budgetkontogruppeViewModelCollection, new Random(fixture.Create<int>()), 250).ToList();

            IEnumerable<int> budgetkontogrupperInUse = budgetkontoViewModelCollection
                .GroupBy(m => m.Kontogruppe.Nummer)
                .Select(m => m.Key)
                .ToList();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.BogførtSidsteÅr, Is.EqualTo(0M));
            Assert.That(regnskabViewModel.BogførtSidsteÅrAsText, Is.Not.Null);
            Assert.That(regnskabViewModel.BogførtSidsteÅrAsText, Is.Not.Empty);
            Assert.That(regnskabViewModel.BogførtSidsteÅrAsText, Is.EqualTo(Convert.ToDecimal(0M).ToString("C")));

            decimal expectedValue = budgetkontogruppeViewModelCollection
                .Where(m => budgetkontogrupperInUse.Contains(m.Nummer))
                .Select(m => m.CreateOpgørelseslinje(regnskabViewModel))
                .Sum(m => m.BogførtSidsteÅr);

            foreach (IBudgetkontogruppeViewModel budgetkontogruppeViewModel in budgetkontogruppeViewModelCollection)
            {
                regnskabViewModel.BudgetkontogruppeAdd(budgetkontogruppeViewModel);
            }
            foreach (IBudgetkontoViewModel budgetkontoViewModel in budgetkontoViewModelCollection)
            {
                regnskabViewModel.BudgetkontoAdd(budgetkontoViewModel);
            }

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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => regnskabViewModel.KontoAdd(null));
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
            Fixture fixture = new Fixture();

            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), exceptionHandlerViewModelMock.Object);
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Konti, Is.Not.Null);
            Assert.That(regnskabViewModel.Konti, Is.Empty);
            Assert.That(regnskabViewModel.KontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.KontiTop, Is.Empty);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            regnskabViewModel.BogføringSet(fixture.BuildBogføringViewModel());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            int kontogruppenummer = fixture.Create<int>();
            IBalanceViewModel balanceViewModel = fixture.BuildBalanceViewModel(kontogruppenummer, balanceType: (Balancetype)Enum.Parse(typeof(Balancetype), balanceType));
            IKontogruppeViewModel kontogruppeViewModel = fixture.BuildKontogruppeViewModel(kontogruppenummer, balanceViewModel: balanceViewModel);
            IKontoViewModel kontoViewModel = fixture.BuildKontoViewModel(kontogruppeViewModel: kontogruppeViewModel, kontoværdi: fixture.Create<decimal>());

            regnskabViewModel.KontoAdd(kontoViewModel);
            Assert.That(regnskabViewModel.BogføringSetCommand, Is.Not.Null);
            Assert.That(regnskabViewModel.BogføringSetCommand.ExecuteTask, Is.Null);

            Assert.That(regnskabViewModel.Konti, Is.Not.Null);
            Assert.That(regnskabViewModel.Konti, Is.Not.Empty);
            Assert.That(regnskabViewModel.KontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.KontiTop, Is.Not.Empty);

            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.IsAny<Exception>()), Times.Never);
        }

        /// <summary>
        /// Tester, at KontoAdd udfører kommandoen, der kan sætte ViewModel til bogføring, hvis ViewModel til bogføring er null.
        /// </summary>
        [Test]
        [TestCase("Aktiver")]
        [TestCase("Passiver")]
        public async Task TestAtKontoAddExecutesBogføringSetCommandHvisBogføringErNull(string balanceType)
        {
            Fixture fixture = new Fixture();

            int regnskabsnummer = fixture.Create<int>();
            IRegnskabModel regnskabModel = fixture.BuildRegnskabModel(regnskabsnummer);

            Mock<IFinansstyringRepository> finansstyringRepositoryMock = fixture.BuildFinansstyringRepositoryMock();
            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(regnskabModel, fixture.Create<DateTime>(), finansstyringRepositoryMock.Object, exceptionHandlerViewModelMock.Object);
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            int kontogruppenummer = fixture.Create<int>();
            string kontonummer = fixture.Create<string>();
            IBalanceViewModel balanceViewModel = fixture.BuildBalanceViewModel(kontogruppenummer, balanceType: (Balancetype)Enum.Parse(typeof(Balancetype), balanceType));
            IKontogruppeViewModel kontogruppeViewModel = fixture.BuildKontogruppeViewModel(kontogruppenummer, balanceViewModel: balanceViewModel);
            IKontoViewModel kontoViewModel = fixture.BuildKontoViewModel(kontonummer: kontonummer, kontogruppeViewModel: kontogruppeViewModel);

            regnskabViewModel.KontoAdd(kontoViewModel);
            Assert.That(regnskabViewModel.BogføringSetCommand, Is.Not.Null);
            Assert.That(regnskabViewModel.BogføringSetCommand.ExecuteTask, Is.Not.Null);
            await regnskabViewModel.BogføringSetCommand.ExecuteTask;

            finansstyringRepositoryMock.Verify(m => m.BogføringslinjeCreateNewAsync(It.Is<int>(value => value == regnskabsnummer), It.Is<DateTime>(value => value > DateTime.MinValue), It.Is<string>(value => string.CompareOrdinal(value, kontonummer) == 0)), Times.Once);
            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.IsAny<Exception>()), Times.Never);
        }

        /// <summary>
        /// Tester, at KontoAdd tilføjer en balancelinje til regnskabet.
        /// </summary>
        [Test]
        [TestCase("Aktiver")]
        [TestCase("Passiver")]
        public void TestAtKontoAddAddsBalanceViewModel(string balanceType)
        {
            Fixture fixture = new Fixture();

            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), exceptionHandlerViewModelMock.Object);
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Aktiver, Is.Not.Null);
            Assert.That(regnskabViewModel.Aktiver, Is.Empty);
            Assert.That(regnskabViewModel.Passiver, Is.Not.Null);
            Assert.That(regnskabViewModel.Passiver, Is.Empty);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            regnskabViewModel.BogføringSet(fixture.Create<IBogføringViewModel>());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            int kontogruppenummer = fixture.Create<int>();
            IBalanceViewModel balanceViewModel = fixture.BuildBalanceViewModel(kontogruppenummer, balanceType: (Balancetype)Enum.Parse(typeof(Balancetype), balanceType));
            IKontogruppeViewModel kontogruppeViewModel = fixture.BuildKontogruppeViewModel(kontogruppenummer, balanceViewModel: balanceViewModel);
            IKontoViewModel kontoViewModel = fixture.BuildKontoViewModel(kontogruppeViewModel: kontogruppeViewModel, kontoværdi: fixture.Create<decimal>());

            regnskabViewModel.KontoAdd(kontoViewModel);
            Assert.That(regnskabViewModel.BogføringSetCommand, Is.Not.Null);
            Assert.That(regnskabViewModel.BogføringSetCommand.ExecuteTask, Is.Null);

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

            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.IsAny<Exception>()), Times.Never);
        }

        /// <summary>
        /// Tester, at KontoAdd registreret kontoen til brug i balancen til regnskabet.
        /// </summary>
        [Test]
        [TestCase("Aktiver")]
        [TestCase("Passiver")]
        public void TestAtKontoAddKalderRegisterOnBalanceViewModel(string balanceType)
        {
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Aktiver, Is.Not.Null);
            Assert.That(regnskabViewModel.Aktiver, Is.Empty);
            Assert.That(regnskabViewModel.Passiver, Is.Not.Null);
            Assert.That(regnskabViewModel.Passiver, Is.Empty);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            regnskabViewModel.BogføringSet(fixture.BuildBogføringViewModel());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            int nummer = fixture.Create<int>();
            Mock<IBalanceViewModel> balanceViewModelMock = fixture.BuildBalanceViewModelMock(nummer, balanceType: (Balancetype)Enum.Parse(typeof(Balancetype), balanceType));
            IKontogruppeViewModel kontogruppeViewModel = fixture.BuildKontogruppeViewModel(nummer, balanceViewModel: balanceViewModelMock.Object);
            IKontoViewModel kontoViewModel = fixture.BuildKontoViewModel(kontogruppeViewModel: kontogruppeViewModel, kontoværdi: fixture.Create<decimal>());

            regnskabViewModel.KontoAdd(kontoViewModel);
            Assert.That(regnskabViewModel.BogføringSetCommand, Is.Not.Null);
            Assert.That(regnskabViewModel.BogføringSetCommand.ExecuteTask, Is.Null);

            balanceViewModelMock.Verify(m => m.Register(It.Is<IKontoViewModel>(value => value == kontoViewModel)), Times.Once);
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
            Fixture fixture = new Fixture();

            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), exceptionHandlerViewModelMock.Object);
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            regnskabViewModel.BogføringSet(fixture.BuildBogføringViewModel());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            bool eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Empty);
                if (string.CompareOrdinal(e.PropertyName, expectedPropertyName) == 0)
                {
                    eventCalled = true;
                }
            };

            int nummer = fixture.Create<int>();
            IBalanceViewModel balanceViewModel = fixture.BuildBalanceViewModel(nummer, balanceType: (Balancetype)Enum.Parse(typeof(Balancetype), balanceType));
            IKontogruppeViewModel kontogruppeViewModel = fixture.BuildKontogruppeViewModel(nummer, balanceViewModel: balanceViewModel);
            IKontoViewModel kontoViewModel = fixture.BuildKontoViewModel(kontogruppeViewModel: kontogruppeViewModel);

            Assert.That(eventCalled, Is.False);
            regnskabViewModel.KontoAdd(kontoViewModel);
            Assert.That(regnskabViewModel.BogføringSetCommand, Is.Not.Null);
            Assert.That(regnskabViewModel.BogføringSetCommand.ExecuteTask, Is.Null);
            Assert.That(eventCalled, Is.True);

            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.IsAny<Exception>()), Times.Never);
        }

        /// <summary>
        /// Tester, at BudgetkontoAdd kaster en ArgumentNullException, hvis ViewModel for budgetkontoen er null.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoAddKasterArgumentNullExceptionHvisBudgetkontoViewModelErNull()
        {
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => regnskabViewModel.BudgetkontoAdd(null));
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkonti, Is.Empty);
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Not.Null);
            Assert.That(regnskabViewModel.BudgetkontiTop, Is.Empty);

            int nummer = fixture.Create<int>();
            IOpgørelseViewModel opgørelseViewModel = fixture.BuildOpgørelseViewModel(nummer);
            IBudgetkontogruppeViewModel budgetkontogruppeViewModel = fixture.BuildBudgetkontogruppeViewModel(nummer, opgørelseViewModel: opgørelseViewModel);
            IBudgetkontoViewModel budgetkontoViewModel = fixture.BuildBudgetkontoViewModel(budgetkontogruppeViewModel: budgetkontogruppeViewModel, kontoværdi: fixture.Create<decimal>());

            regnskabViewModel.BudgetkontoAdd(budgetkontoViewModel);
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Opgørelseslinjer, Is.Not.Null);
            Assert.That(regnskabViewModel.Opgørelseslinjer, Is.Empty);

            int nummer = fixture.Create<int>();
            IOpgørelseViewModel opgørelseViewModel = fixture.BuildOpgørelseViewModel(nummer);
            IBudgetkontogruppeViewModel budgetkontogruppeViewModel = fixture.BuildBudgetkontogruppeViewModel(nummer, opgørelseViewModel: opgørelseViewModel);
            IBudgetkontoViewModel budgetkontoViewModel = fixture.BuildBudgetkontoViewModel(budgetkontogruppeViewModel: budgetkontogruppeViewModel);

            regnskabViewModel.BudgetkontoAdd(budgetkontoViewModel);
            Assert.That(regnskabViewModel.Opgørelseslinjer, Is.Not.Null);
            Assert.That(regnskabViewModel.Opgørelseslinjer, Is.Not.Empty);
        }

        /// <summary>
        /// Tester, at BudgetkontoAdd registreret budgetkontoen til brug i opgørelseslinjen til regnskabet.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoAddKalderRegisterOnOpgørelseViewModel()
        {
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            int nummer = fixture.Create<int>();
            Mock<IOpgørelseViewModel> opgørelseViewModelMock = fixture.BuildOpgørelseViewModelMock(nummer);
            IBudgetkontogruppeViewModel budgetkontogruppeViewModel = fixture.BuildBudgetkontogruppeViewModel(nummer, opgørelseViewModel: opgørelseViewModelMock.Object);
            IBudgetkontoViewModel budgetkontoViewModel = fixture.BuildBudgetkontoViewModel(budgetkontogruppeViewModel: budgetkontogruppeViewModel);

            regnskabViewModel.BudgetkontoAdd(budgetkontoViewModel);

            opgørelseViewModelMock.Verify(m => m.Register(It.Is<IBudgetkontoViewModel>(value => value == budgetkontoViewModel)), Times.Once);
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            bool eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Empty);
                if (string.CompareOrdinal(e.PropertyName, expectedPropertyName) == 0)
                {
                    eventCalled = true;
                }
            };

            int nummer = fixture.Create<int>();
            IOpgørelseViewModel opgørelseViewModel = fixture.BuildOpgørelseViewModel(nummer);
            IBudgetkontogruppeViewModel budgetkontogruppeViewModel = fixture.BuildBudgetkontogruppeViewModel(nummer, opgørelseViewModel: opgørelseViewModel);
            IBudgetkontoViewModel budgetkontoViewModel = fixture.BuildBudgetkontoViewModel(budgetkontogruppeViewModel: budgetkontogruppeViewModel);

            Assert.That(eventCalled, Is.False);
            regnskabViewModel.BudgetkontoAdd(budgetkontoViewModel);
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at BogføringslinjeAdd kaster en ArgumentNullException, hvis ViewModel for bogføringslinjen er null.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjeAddKasterArgumentNullExceptionHvisBogføringslinjeViewModelErNull()
        {
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => regnskabViewModel.BogføringslinjeAdd(null));
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
            Fixture fixture = new Fixture();

            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), exceptionHandlerViewModelMock.Object);
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføringslinjer, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføringslinjer, Is.Empty);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            regnskabViewModel.BogføringSet(fixture.BuildBogføringViewModel());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            regnskabViewModel.BogføringslinjeAdd(fixture.BuildReadOnlyBogføringslinjeViewModel());
            Assert.That(regnskabViewModel.BogføringSetCommand, Is.Not.Null);
            Assert.That(regnskabViewModel.BogføringSetCommand.ExecuteTask, Is.Null);

            Assert.That(regnskabViewModel.Bogføringslinjer, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføringslinjer, Is.Not.Empty);

            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.IsAny<Exception>()), Times.Never);
        }

        /// <summary>
        /// Tester, at BogføringslinjeAdd udfører kommandoen, der kan sætte ViewModel til bogføring, hvis ViewModel til bogføring er null.
        /// </summary>
        [Test]
        public async Task TestAtBogføringslinjeAddExecutesBogføringSetCommandHvisBogføringErNull()
        {
            Fixture fixture = new Fixture();

            int regnskabsnummer = fixture.Create<int>();
            IRegnskabModel regnskabModel = fixture.BuildRegnskabModel(regnskabsnummer);

            Mock<IFinansstyringRepository> finansstyringRepositoryMock = fixture.BuildFinansstyringRepositoryMock();
            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(regnskabModel, fixture.Create<DateTime>(), finansstyringRepositoryMock.Object, exceptionHandlerViewModelMock.Object);
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            string kontonummer = fixture.Create<string>();
            IReadOnlyBogføringslinjeViewModel bogføringslinjeViewModel = fixture.BuildReadOnlyBogføringslinjeViewModel(kontonummer: kontonummer);

            regnskabViewModel.BogføringslinjeAdd(bogføringslinjeViewModel);
            Assert.That(regnskabViewModel.BogføringSetCommand, Is.Not.Null);
            Assert.That(regnskabViewModel.BogføringSetCommand.ExecuteTask, Is.Not.Null);

            await regnskabViewModel.BogføringSetCommand.ExecuteTask;

            finansstyringRepositoryMock.Verify(m => m.BogføringslinjeCreateNewAsync(It.Is<int>(value => value == regnskabsnummer), It.Is<DateTime>(value => value > DateTime.MinValue), It.Is<string>(value => string.CompareOrdinal(value, kontonummer) == 0)), Times.Once);
            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.IsAny<Exception>()), Times.Never);
        }

        /// <summary>
        /// Tester, at BogføringslinjeAdd rejser PropertyChanged, når en bogføringslinje tilføjes regnskabet.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjeAddRejserPropertyChangedVedAddAfBogføringslinjeViewModel()
        {
            Fixture fixture = new Fixture();

            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), exceptionHandlerViewModelMock.Object);
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            regnskabViewModel.BogføringSet(fixture.BuildBogføringViewModel());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            bool eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Empty);
                if (string.CompareOrdinal(e.PropertyName, "Bogføringslinjer") == 0)
                {
                    eventCalled = true;
                }
            };

            Assert.That(eventCalled, Is.False);
            regnskabViewModel.BogføringslinjeAdd(fixture.BuildReadOnlyBogføringslinjeViewModel());
            Assert.That(regnskabViewModel.BogføringSetCommand, Is.Not.Null);
            Assert.That(regnskabViewModel.BogføringSetCommand.ExecuteTask, Is.Null);
            Assert.That(eventCalled, Is.True);

            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.IsAny<Exception>()), Times.Never);
        }

        /// <summary>
        /// Tester, at BogføringSet kaster en ArgumentNullException, hvis ViewModel til bogføring er null.
        /// </summary>
        [Test]
        public void TestAtBogføringSetKasterArgumentNullExceptionHvisBogføringViewModelErNull()
        {
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => regnskabViewModel.BogføringSet(null));
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            IBogføringViewModel bogføringViewModel = fixture.BuildBogføringViewModel();
            regnskabViewModel.BogføringSet(bogføringViewModel);

            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføring, Is.EqualTo(bogføringViewModel));
        }

        /// <summary>
        /// Tester, at BogføringSet rejser PropertyChanged ved opdatering af ViewModel til bogføring.
        /// </summary>
        [Test]
        [TestCase("Bogføring")]
        public void TestAtBogføringSetRejserPropertyChangedVedVedOpdateringAfBogføring(string expectedPropertyName)
        {
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            regnskabViewModel.BogføringSet(fixture.Create<IBogføringViewModel>());
            Assert.That(regnskabViewModel, Is.Not.Null);

            bool eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Empty);
                if (string.CompareOrdinal(e.PropertyName, expectedPropertyName) == 0)
                {
                    eventCalled = true;
                }
            };

            Assert.That(eventCalled, Is.False);
            regnskabViewModel.BogføringSet(regnskabViewModel.Bogføring);
            Assert.That(eventCalled, Is.False);
            regnskabViewModel.BogføringSet(fixture.BuildBogføringViewModel());
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at BogføringsadvarselAdd kaster en ArgumentNullException, hvis ViewModel for bogføringsadvarslen er null.
        /// </summary>
        [Test]
        public void TestAtBogføringsadvarselAddKasterArgumentNullExceptionHvisBogføringsadvarselViewModelErNull()
        {
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => regnskabViewModel.BogføringsadvarselAdd(null));
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføringsadvarsler, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføringsadvarsler, Is.Empty);

            IBogføringsadvarselViewModel bogføringsadvarselViewModel = fixture.BuildBogføringsadvarselViewModel();
            regnskabViewModel.BogføringsadvarselAdd(bogføringsadvarselViewModel);

            Assert.That(regnskabViewModel.Bogføringsadvarsler, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføringsadvarsler, Is.Not.Empty);
        }

        /// <summary>
        /// Tester, at BogføringsadvarselAdd rejser PropertyChanged, når en bogføringsadvarsel tilføjes regnskabet.
        /// </summary>
        [Test]
        public void TestAtBogføringsadvarselAddRejserPropertyChangedVedAddAfBogføringsadvarselViewModel()
        {
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            bool eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Empty);
                if (string.CompareOrdinal(e.PropertyName, "Bogføringsadvarsler") == 0)
                {
                    eventCalled = true;
                }
            };

            Assert.That(eventCalled, Is.False);
            regnskabViewModel.BogføringsadvarselAdd(fixture.BuildBogføringsadvarselViewModel());
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at BogføringsadvarselRemove kaster en ArgumentNullException, hvis ViewModel for bogføringsadvarslen er null.
        /// </summary>
        [Test]
        public void TestAtBogføringsadvarselRemoveKasterArgumentNullExceptionHvisBogføringsadvarselViewModelErNull()
        {
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => regnskabViewModel.BogføringsadvarselRemove(null));
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføringsadvarsler, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføringsadvarsler, Is.Empty);

            regnskabViewModel.BogføringsadvarselAdd(fixture.BuildBogføringsadvarselViewModel());
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            regnskabViewModel.BogføringsadvarselAdd(fixture.BuildBogføringsadvarselViewModel());
            Assert.That(regnskabViewModel.Bogføringsadvarsler, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføringsadvarsler, Is.Not.Empty);

            bool eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Empty);
                if (string.CompareOrdinal(e.PropertyName, "Bogføringsadvarsler") == 0)
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => regnskabViewModel.DebitorAdd(null));
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Debitorer, Is.Not.Null);
            Assert.That(regnskabViewModel.Debitorer, Is.Empty);

            IAdressekontoViewModel adressekontoViewModel = fixture.BuildAdressekontoViewModel();
            regnskabViewModel.DebitorAdd(adressekontoViewModel);

            Assert.That(regnskabViewModel.Debitorer, Is.Not.Null);
            Assert.That(regnskabViewModel.Debitorer, Is.Not.Empty);
        }

        /// <summary>
        /// Tester, at DebitorAdd rejser PropertyChanged, når en debitor tilføjes regnskabet.
        /// </summary>
        [Test]
        public void TestAtDebitorAddRejserPropertyChangedVedAddAfAdressekontoViewModel()
        {
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            bool eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Empty);
                if (string.CompareOrdinal(e.PropertyName, "Debitorer") == 0)
                {
                    eventCalled = true;
                }
            };

            Assert.That(eventCalled, Is.False);
            regnskabViewModel.DebitorAdd(fixture.BuildAdressekontoViewModel());
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at KreditorAdd kaster en ArgumentNullException, hvis ViewModel for adressekontoen, der skal tilføjes som kreditor, er null.
        /// </summary>
        [Test]
        public void TestAtKreditorAddKasterArgumentNullExceptionHvisAdressekontoViewModelErNull()
        {
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => regnskabViewModel.KreditorAdd(null));
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Kreditorer, Is.Not.Null);
            Assert.That(regnskabViewModel.Kreditorer, Is.Empty);

            IAdressekontoViewModel adressekontoViewModel = fixture.BuildAdressekontoViewModel();
            regnskabViewModel.KreditorAdd(adressekontoViewModel);

            Assert.That(regnskabViewModel.Kreditorer, Is.Not.Null);
            Assert.That(regnskabViewModel.Kreditorer, Is.Not.Empty);
        }

        /// <summary>
        /// Tester, at KreditorAdd rejser PropertyChanged, når en debitor tilføjes regnskabet.
        /// </summary>
        [Test]
        public void TestAtKreditorAddRejserPropertyChangedVedAddAfAdressekontoViewModel()
        {
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            bool eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Empty);
                if (string.CompareOrdinal(e.PropertyName, "Kreditorer") == 0)
                {
                    eventCalled = true;
                }
            };

            Assert.That(eventCalled, Is.False);
            regnskabViewModel.KreditorAdd(fixture.BuildAdressekontoViewModel());
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at NyhedAdd kaster en ArgumentNullException, hvis ViewModel for nyheden er null.
        /// </summary>
        [Test]
        public void TestAtNyhedAddKasterArgumentNullExceptionHvisNyhedViewModelErNull()
        {
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => regnskabViewModel.NyhedAdd(null));
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Nyheder, Is.Not.Null);
            Assert.That(regnskabViewModel.Nyheder, Is.Empty);

            INyhedViewModel nyhedViewModelMock = fixture.BuildNyhedViewModel();
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            bool eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Empty);
                if (string.CompareOrdinal(e.PropertyName, "Nyheder") == 0)
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => regnskabViewModel.KontogruppeAdd(null));
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Kontogrupper, Is.Not.Null);

            Mock<IKontogruppeViewModel> kontogruppeViewModelMock = fixture.BuildKontogruppeViewModelMock(nummer);
            Assert.That(kontogruppeViewModelMock, Is.Not.Null);

            regnskabViewModel.KontogruppeAdd(kontogruppeViewModelMock.Object);
            Assert.That(regnskabViewModel.Kontogrupper, Is.Not.Null);
            Assert.That(regnskabViewModel.Kontogrupper.Count(m => m.Nummer == nummer), Is.EqualTo(1));

            kontogruppeViewModelMock.Verify(m => m.Nummer, Times.Once);
        }

        /// <summary>
        /// Tester, at KontogruppeAdd kun tilføjer en kontogruppe med samme nummer én gang.
        /// </summary>
        [Test]
        [TestCase(2)]
        public void TestAtKontogruppeAddOnlyAddsKontogruppeViewModelOnce(int nummer)
        {
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Kontogrupper, Is.Not.Null);

            regnskabViewModel.KontogruppeAdd(fixture.BuildKontogruppeViewModel(nummer));
            regnskabViewModel.KontogruppeAdd(fixture.BuildKontogruppeViewModel(nummer));
            regnskabViewModel.KontogruppeAdd(fixture.BuildKontogruppeViewModel(nummer));
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            bool eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Empty);
                if (string.CompareOrdinal(e.PropertyName, "Kontogrupper") == 0)
                {
                    eventCalled = true;
                }
            };

            Assert.That(eventCalled, Is.False);
            regnskabViewModel.KontogruppeAdd(fixture.BuildKontogruppeViewModel(nummer));
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at BudgetkontogruppeAdd kaster en ArgumentNullException, hvis ViewModel for kontogruppen til budgetkonti er null.
        /// </summary>
        [Test]
        public void TestAtBudgetkontogruppeAddKasterArgumentNullExceptionHvisBudgetkontogruppeViewModelErNull()
        {
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => regnskabViewModel.BudgetkontogruppeAdd(null));
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkontogrupper, Is.Not.Null);

            Mock<IBudgetkontogruppeViewModel> budgetkontogruppeViewModelMock = fixture.BuildBudgetkontogruppeViewModelMock(nummer);
            Assert.That(budgetkontogruppeViewModelMock, Is.Not.Null);

            regnskabViewModel.BudgetkontogruppeAdd(budgetkontogruppeViewModelMock.Object);
            Assert.That(regnskabViewModel.Budgetkontogrupper, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkontogrupper.Count(m => m.Nummer == nummer), Is.EqualTo(1));

            budgetkontogruppeViewModelMock.Verify(m => m.Nummer, Times.Once);
        }

        /// <summary>
        /// Tester, at BudgetkontogruppeAdd kun tilføjer en kontogruppe til budgetkonti med samme nummer én gang.
        /// </summary>
        [Test]
        [TestCase(2)]
        public void TestAtBudgetkontogruppeAddAddOnlyAddsBudgetkontogruppeViewModelOnce(int nummer)
        {
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Budgetkontogrupper, Is.Not.Null);

            regnskabViewModel.BudgetkontogruppeAdd(fixture.BuildBudgetkontogruppeViewModel(nummer));
            regnskabViewModel.BudgetkontogruppeAdd(fixture.BuildBudgetkontogruppeViewModel(nummer));
            regnskabViewModel.BudgetkontogruppeAdd(fixture.BuildBudgetkontogruppeViewModel(nummer));
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            bool eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Empty);
                if (string.CompareOrdinal(e.PropertyName, "Budgetkontogrupper") == 0)
                {
                    eventCalled = true;
                }
            };

            Assert.That(eventCalled, Is.False);
            regnskabViewModel.BudgetkontogruppeAdd(fixture.BuildBudgetkontogruppeViewModel(nummer));
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
            Fixture fixture = new Fixture();

            Mock<IRegnskabModel> regnskabModelMock = fixture.BuildRegnskabModelMock();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(regnskabModelMock.Object, fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            bool eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Empty);
                if (string.CompareOrdinal(e.PropertyName, expectPropertyName) == 0)
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
            Fixture fixture = new Fixture();

            Mock<IRegnskabModel> regnskabModelMock = fixture.BuildRegnskabModelMock();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(regnskabModelMock.Object, fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => regnskabModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
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
            Fixture fixture = new Fixture();

            Mock<IRegnskabModel> regnskabModelMock = fixture.BuildRegnskabModelMock();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(regnskabModelMock.Object, fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => regnskabModelMock.Raise(m => m.PropertyChanged += null, fixture.Create<object>(), null));
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            regnskabViewModel.BogføringSet(fixture.BuildBogføringViewModel());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            int nummer = fixture.Create<int>();
            IBalanceViewModel balanceViewModel = fixture.BuildBalanceViewModel(nummer, balanceType: (Balancetype)Enum.Parse(typeof(Balancetype), balanceType));
            IKontogruppeViewModel kontogruppeViewModel = fixture.BuildKontogruppeViewModel(nummer, balanceViewModel: balanceViewModel);
            Mock<IKontoViewModel> kontoViewModelMock = fixture.BuildKontoViewModelMock(kontogruppeViewModel: kontogruppeViewModel);
            regnskabViewModel.KontoAdd(kontoViewModelMock.Object);

            bool eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Empty);
                if (string.CompareOrdinal(e.PropertyName, expectedPropertyName) == 0)
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            regnskabViewModel.BogføringSet(fixture.BuildBogføringViewModel());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            int nummer = fixture.Create<int>();
            IBalanceViewModel balanceViewModel = fixture.BuildBalanceViewModel(nummer, balanceType: (Balancetype)Enum.Parse(typeof(Balancetype), balanceType));
            IKontogruppeViewModel kontogruppeViewModel = fixture.BuildKontogruppeViewModel(nummer, balanceViewModel: balanceViewModel);
            Mock<IKontoViewModel> kontoViewModelMock = fixture.BuildKontoViewModelMock(kontogruppeViewModel: kontogruppeViewModel);
            regnskabViewModel.KontoAdd(kontoViewModelMock.Object);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => kontoViewModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            regnskabViewModel.BogføringSet(fixture.BuildBogføringViewModel());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            int nummer = fixture.Create<int>();
            IBalanceViewModel balanceViewModel = fixture.BuildBalanceViewModel(nummer, balanceType: (Balancetype)Enum.Parse(typeof(Balancetype), balanceType));
            IKontogruppeViewModel kontogruppeViewModel = fixture.BuildKontogruppeViewModel(nummer, balanceViewModel: balanceViewModel);
            Mock<IKontoViewModel> kontoViewModelMock = fixture.BuildKontoViewModelMock(kontogruppeViewModel: kontogruppeViewModel);
            regnskabViewModel.KontoAdd(kontoViewModelMock.Object);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => kontoViewModelMock.Raise(m => m.PropertyChanged += null, kontoViewModelMock, null));
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            int nummer = fixture.Create<int>();
            IOpgørelseViewModel opgørelseViewModel = fixture.BuildOpgørelseViewModel(nummer);
            IBudgetkontogruppeViewModel budgetkontogruppeViewModel = fixture.BuildBudgetkontogruppeViewModel(nummer, opgørelseViewModel: opgørelseViewModel);
            Mock<IBudgetkontoViewModel> budgetkontoViewModelMock = fixture.BuildBudgetkontoViewModelMock(budgetkontogruppeViewModel: budgetkontogruppeViewModel);
            regnskabViewModel.BudgetkontoAdd(budgetkontoViewModelMock.Object);

            bool eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Empty);
                if (string.CompareOrdinal(e.PropertyName, expectedPropertyName) == 0)
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            int nummer = fixture.Create<int>();
            IOpgørelseViewModel opgørelseViewModel = fixture.BuildOpgørelseViewModel(nummer);
            IBudgetkontogruppeViewModel budgetkontogruppeViewModel = fixture.BuildBudgetkontogruppeViewModel(nummer, opgørelseViewModel: opgørelseViewModel);
            Mock<IBudgetkontoViewModel> budgetkontoViewModelMock = fixture.BuildBudgetkontoViewModelMock(budgetkontogruppeViewModel: budgetkontogruppeViewModel);
            regnskabViewModel.BudgetkontoAdd(budgetkontoViewModelMock.Object);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => budgetkontoViewModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            int nummer = fixture.Create<int>();
            IOpgørelseViewModel opgørelseViewModel = fixture.BuildOpgørelseViewModel(nummer);
            IBudgetkontogruppeViewModel budgetkontogruppeViewModel = fixture.BuildBudgetkontogruppeViewModel(nummer, opgørelseViewModel: opgørelseViewModel);
            Mock<IBudgetkontoViewModel> budgetkontoViewModelMock = fixture.BuildBudgetkontoViewModelMock(budgetkontogruppeViewModel: budgetkontogruppeViewModel);
            regnskabViewModel.BudgetkontoAdd(budgetkontoViewModelMock.Object);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => budgetkontoViewModelMock.Raise(m => m.PropertyChanged += null, budgetkontoViewModelMock, null));
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            regnskabViewModel.BogføringSet(fixture.BuildBogføringViewModel());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            Mock<IReadOnlyBogføringslinjeViewModel> readOnlyBogføringslinjeViewModelMock = fixture.BuildReadOnlyBogføringslinjeViewModelMock();
            regnskabViewModel.BogføringslinjeAdd(readOnlyBogføringslinjeViewModelMock.Object);

            bool eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Empty);
                if (string.CompareOrdinal(e.PropertyName, expectedPropertyName) == 0)
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            regnskabViewModel.BogføringSet(fixture.BuildBogføringViewModel());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            Mock<IReadOnlyBogføringslinjeViewModel> readOnlyBogføringslinjeViewModelMock = fixture.BuildReadOnlyBogføringslinjeViewModelMock();
            regnskabViewModel.BogføringslinjeAdd(readOnlyBogføringslinjeViewModelMock.Object);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => readOnlyBogføringslinjeViewModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            regnskabViewModel.BogføringSet(fixture.BuildBogføringViewModel());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            Mock<IReadOnlyBogføringslinjeViewModel> readOnlyBogføringslinjeViewModelMock = fixture.BuildReadOnlyBogføringslinjeViewModelMock();
            regnskabViewModel.BogføringslinjeAdd(readOnlyBogføringslinjeViewModelMock.Object);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => readOnlyBogføringslinjeViewModelMock.Raise(m => m.PropertyChanged += null, readOnlyBogføringslinjeViewModelMock, null));
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            Mock<IBogføringsadvarselViewModel> bogføringsadvarselViewModelMock = fixture.BuildBogføringsadvarselViewModelMock();
            regnskabViewModel.BogføringsadvarselAdd(bogføringsadvarselViewModelMock.Object);

            bool eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Empty);
                if (string.CompareOrdinal(e.PropertyName, expectedPropertyName) == 0)
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            Mock<IBogføringsadvarselViewModel> bogføringsadvarselViewModelMock = fixture.BuildBogføringsadvarselViewModelMock();
            regnskabViewModel.BogføringsadvarselAdd(bogføringsadvarselViewModelMock.Object);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => bogføringsadvarselViewModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            Mock<IBogføringsadvarselViewModel> bogføringsadvarselViewModelMock = fixture.BuildBogføringsadvarselViewModelMock();
            regnskabViewModel.BogføringsadvarselAdd(bogføringsadvarselViewModelMock.Object);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => bogføringsadvarselViewModelMock.Raise(m => m.PropertyChanged += null, bogføringsadvarselViewModelMock, null));
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            int nummer = fixture.Create<int>();
            Mock<IOpgørelseViewModel> opgørelseViewModelMock = fixture.BuildOpgørelseViewModelMock(nummer);
            IBudgetkontogruppeViewModel budgetkontogruppeViewModel = fixture.BuildBudgetkontogruppeViewModel(nummer, opgørelseViewModel: opgørelseViewModelMock.Object);
            IBudgetkontoViewModel budgetkontoViewModel = fixture.BuildBudgetkontoViewModel(budgetkontogruppeViewModel: budgetkontogruppeViewModel);
            regnskabViewModel.BudgetkontoAdd(budgetkontoViewModel);

            bool eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Empty);
                if (string.CompareOrdinal(e.PropertyName, expectedPropertyName) == 0)
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            int nummer = fixture.Create<int>();
            Mock<IOpgørelseViewModel> opgørelseViewModelMock = fixture.BuildOpgørelseViewModelMock(nummer);
            IBudgetkontogruppeViewModel budgetkontogruppeViewModel = fixture.BuildBudgetkontogruppeViewModel(nummer, opgørelseViewModel: opgørelseViewModelMock.Object);
            IBudgetkontoViewModel budgetkontoViewModel = fixture.BuildBudgetkontoViewModel(budgetkontogruppeViewModel: budgetkontogruppeViewModel);
            regnskabViewModel.BudgetkontoAdd(budgetkontoViewModel);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => opgørelseViewModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            int nummer = fixture.Create<int>();
            Mock<IOpgørelseViewModel> opgørelseViewModelMock = fixture.BuildOpgørelseViewModelMock(nummer);
            IBudgetkontogruppeViewModel budgetkontogruppeViewModel = fixture.BuildBudgetkontogruppeViewModel(nummer, opgørelseViewModel: opgørelseViewModelMock.Object);
            IBudgetkontoViewModel budgetkontoViewModel = fixture.BuildBudgetkontoViewModel(budgetkontogruppeViewModel: budgetkontogruppeViewModel);
            regnskabViewModel.BudgetkontoAdd(budgetkontoViewModel);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => opgørelseViewModelMock.Raise(m => m.PropertyChanged += null, opgørelseViewModelMock, null));
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Aktiver, Is.Not.Null);
            Assert.That(regnskabViewModel.Aktiver, Is.Empty);
            Assert.That(regnskabViewModel.Passiver, Is.Not.Null);
            Assert.That(regnskabViewModel.Passiver, Is.Empty);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            regnskabViewModel.BogføringSet(fixture.BuildBogføringViewModel());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            int nummer = fixture.Create<int>();
            Mock<IBalanceViewModel> balanceViewModelMock = fixture.BuildBalanceViewModelMock(nummer, balanceType: (Balancetype)Enum.Parse(typeof(Balancetype), balanceType));
            IKontogruppeViewModel kontogruppeViewModel = fixture.BuildKontogruppeViewModel(nummer, balanceViewModel: balanceViewModelMock.Object);
            IKontoViewModel kontoViewModel = fixture.BuildKontoViewModel(kontogruppeViewModel: kontogruppeViewModel, kontoværdi: fixture.Create<decimal>());
            regnskabViewModel.KontoAdd(kontoViewModel);

            Assert.That(regnskabViewModel.BogføringSetCommand, Is.Not.Null);
            Assert.That(regnskabViewModel.BogføringSetCommand.ExecuteTask, Is.Null);

            bool eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Empty);
                if (string.CompareOrdinal(e.PropertyName, expectedPropertyName) == 0)
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Aktiver, Is.Not.Null);
            Assert.That(regnskabViewModel.Aktiver, Is.Empty);
            Assert.That(regnskabViewModel.Passiver, Is.Not.Null);
            Assert.That(regnskabViewModel.Passiver, Is.Empty);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            regnskabViewModel.BogføringSet(fixture.BuildBogføringViewModel());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            int nummer = fixture.Create<int>();
            Mock<IBalanceViewModel> balanceViewModelMock = fixture.BuildBalanceViewModelMock(nummer, balanceType: (Balancetype)Enum.Parse(typeof(Balancetype), balanceType));
            IKontogruppeViewModel kontogruppeViewModel = fixture.BuildKontogruppeViewModel(nummer, balanceViewModel: balanceViewModelMock.Object);
            IKontoViewModel kontoViewModel = fixture.BuildKontoViewModel(kontogruppeViewModel: kontogruppeViewModel, kontoværdi: fixture.Create<decimal>());
            regnskabViewModel.KontoAdd(kontoViewModel);

            Assert.That(regnskabViewModel.BogføringSetCommand, Is.Not.Null);
            Assert.That(regnskabViewModel.BogføringSetCommand.ExecuteTask, Is.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => balanceViewModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Aktiver, Is.Not.Null);
            Assert.That(regnskabViewModel.Aktiver, Is.Empty);
            Assert.That(regnskabViewModel.Passiver, Is.Not.Null);
            Assert.That(regnskabViewModel.Passiver, Is.Empty);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            regnskabViewModel.BogføringSet(fixture.BuildBogføringViewModel());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            int nummer = fixture.Create<int>();
            Mock<IBalanceViewModel> balanceViewModelMock = fixture.BuildBalanceViewModelMock(nummer, balanceType: (Balancetype)Enum.Parse(typeof(Balancetype), balanceType));
            IKontogruppeViewModel kontogruppeViewModel = fixture.BuildKontogruppeViewModel(nummer, balanceViewModel: balanceViewModelMock.Object);
            IKontoViewModel kontoViewModel = fixture.BuildKontoViewModel(kontogruppeViewModel: kontogruppeViewModel, kontoværdi: fixture.Create<decimal>());
            regnskabViewModel.KontoAdd(kontoViewModel);

            Assert.That(regnskabViewModel.BogføringSetCommand, Is.Not.Null);
            Assert.That(regnskabViewModel.BogføringSetCommand.ExecuteTask, Is.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => balanceViewModelMock.Raise(m => m.PropertyChanged += null, balanceViewModelMock, null));
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);
            Assert.That(regnskabViewModel.Aktiver, Is.Not.Null);
            Assert.That(regnskabViewModel.Aktiver, Is.Empty);
            Assert.That(regnskabViewModel.Passiver, Is.Not.Null);
            Assert.That(regnskabViewModel.Passiver, Is.Empty);
            Assert.That(regnskabViewModel.Bogføring, Is.Null);

            regnskabViewModel.BogføringSet(fixture.BuildBogføringViewModel());
            Assert.That(regnskabViewModel.Bogføring, Is.Not.Null);

            int nummer = fixture.Create<int>();
            Mock<IBalanceViewModel> balanceViewModelMock = fixture.BuildBalanceViewModelMock(nummer, balanceType: (Balancetype)Enum.Parse(typeof(Balancetype), balanceType));
            IKontogruppeViewModel kontogruppeViewModel = fixture.BuildKontogruppeViewModel(nummer, balanceViewModel: balanceViewModelMock.Object);
            IKontoViewModel kontoViewModel = fixture.BuildKontoViewModel(kontogruppeViewModel: kontogruppeViewModel, kontoværdi: fixture.Create<decimal>());
            regnskabViewModel.KontoAdd(kontoViewModel);

            Assert.That(regnskabViewModel.BogføringSetCommand, Is.Not.Null);
            Assert.That(regnskabViewModel.BogføringSetCommand.ExecuteTask, Is.Null);

            object sender = fixture.Create<object>();
            IntranetGuiSystemException exception = Assert.Throws<IntranetGuiSystemException>(() => balanceViewModelMock.Raise(m => m.PropertyChanged += null, sender, fixture.Create<PropertyChangedEventArgs>()));
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            Mock<IAdressekontoViewModel> adressekontoViewModelMock = fixture.BuildAdressekontoViewModelMock();
            regnskabViewModel.DebitorAdd(adressekontoViewModelMock.Object);

            bool eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Empty);
                if (string.CompareOrdinal(e.PropertyName, expectedPropertyName) == 0)
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            Mock<IAdressekontoViewModel> adressekontoViewModelMock = fixture.BuildAdressekontoViewModelMock();
            regnskabViewModel.DebitorAdd(adressekontoViewModelMock.Object);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => adressekontoViewModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            Mock<IAdressekontoViewModel> adressekontoViewModelMock = fixture.BuildAdressekontoViewModelMock();
            regnskabViewModel.DebitorAdd(adressekontoViewModelMock.Object);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => adressekontoViewModelMock.Raise(m => m.PropertyChanged += null, adressekontoViewModelMock, null));
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            Mock<IAdressekontoViewModel> adressekontoViewModelMock = fixture.BuildAdressekontoViewModelMock();
            regnskabViewModel.KreditorAdd(adressekontoViewModelMock.Object);

            bool eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Empty);
                if (string.CompareOrdinal(e.PropertyName, expectedPropertyName) == 0)
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            Mock<IAdressekontoViewModel> adressekontoViewModelMock = fixture.BuildAdressekontoViewModelMock();
            regnskabViewModel.KreditorAdd(adressekontoViewModelMock.Object);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => adressekontoViewModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            Mock<IAdressekontoViewModel> adressekontoViewModelMock = fixture.BuildAdressekontoViewModelMock();
            regnskabViewModel.KreditorAdd(adressekontoViewModelMock.Object);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => adressekontoViewModelMock.Raise(m => m.PropertyChanged += null, adressekontoViewModelMock, null));
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            Mock<INyhedViewModel> nyhedViewModelMock = fixture.BuildNyhedViewModelMock();
            regnskabViewModel.NyhedAdd(nyhedViewModelMock.Object);

            bool eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Empty);
                if (string.CompareOrdinal(e.PropertyName, expectedPropertyName) == 0)
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            Mock<INyhedViewModel> nyhedViewModelMock = fixture.BuildNyhedViewModelMock();
            regnskabViewModel.NyhedAdd(nyhedViewModelMock.Object);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => nyhedViewModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            Mock<INyhedViewModel> nyhedViewModelMock = fixture.BuildNyhedViewModelMock();
            regnskabViewModel.NyhedAdd(nyhedViewModelMock.Object);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => nyhedViewModelMock.Raise(m => m.PropertyChanged += null, nyhedViewModelMock, null));
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            Mock<IKontogruppeViewModel> kontogruppeViewModelMock = fixture.BuildKontogruppeViewModelMock(nummer);
            regnskabViewModel.KontogruppeAdd(kontogruppeViewModelMock.Object);

            bool eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Empty);
                if (string.CompareOrdinal(e.PropertyName, expectedPropertyName) == 0)
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            Mock<IKontogruppeViewModel> kontogruppeViewModelMock = fixture.BuildKontogruppeViewModelMock(nummer);
            regnskabViewModel.KontogruppeAdd(kontogruppeViewModelMock.Object);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => kontogruppeViewModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            Mock<IKontogruppeViewModel> kontogruppeViewModelMock = fixture.BuildKontogruppeViewModelMock(nummer);
            regnskabViewModel.KontogruppeAdd(kontogruppeViewModelMock.Object);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => kontogruppeViewModelMock.Raise(m => m.PropertyChanged += null, kontogruppeViewModelMock, null));
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            Mock<IBudgetkontogruppeViewModel> budgetkontogruppeViewModelMock = fixture.BuildBudgetkontogruppeViewModelMock(nummer);
            regnskabViewModel.BudgetkontogruppeAdd(budgetkontogruppeViewModelMock.Object);

            bool eventCalled = false;
            regnskabViewModel.PropertyChanged += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Empty);
                if (string.CompareOrdinal(e.PropertyName, expectedPropertyName) == 0)
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            Mock<IBudgetkontogruppeViewModel> budgetkontogruppeViewModelMock = fixture.BuildBudgetkontogruppeViewModelMock(nummer);
            regnskabViewModel.BudgetkontogruppeAdd(budgetkontogruppeViewModelMock.Object);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => budgetkontogruppeViewModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
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
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = new RegnskabViewModel(fixture.BuildRegnskabModel(), fixture.Create<DateTime>(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(regnskabViewModel, Is.Not.Null);

            Mock<IBudgetkontogruppeViewModel> budgetkontogruppeViewModelMock = fixture.BuildBudgetkontogruppeViewModelMock(nummer);
            regnskabViewModel.BudgetkontogruppeAdd(budgetkontogruppeViewModelMock.Object);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => budgetkontogruppeViewModelMock.Raise(m => m.PropertyChanged += null, budgetkontogruppeViewModelMock, null));
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
        private static IEnumerable<IKontoViewModel> CreateKontoViewModels(ISpecimenBuilder fixture, IKontogruppeViewModel[] kontogrupper, Random random, int count)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            if (kontogrupper == null)
            {
                throw new ArgumentNullException(nameof(kontogrupper));
            }

            if (random == null)
            {
                throw new ArgumentNullException(nameof(random));
            }

            IList<IKontoViewModel> result = new List<IKontoViewModel>(count);
            while (result.Count < count)
            {
                result.Add(fixture.BuildKontoViewModel(kontogruppeViewModel: kontogrupper[random.Next(0, kontogrupper.Length - 1)]));
            }

            return result;
        }

        /// <summary>
        /// Danner og returnerer en liste af budgetkonti.
        /// </summary>
        /// <returns>Liste af budgetkonti.</returns>
        private static IEnumerable<IBudgetkontoViewModel> CreateBudgetkontoViewModels(ISpecimenBuilder fixture, IBudgetkontogruppeViewModel[] budgetkontogrupper, Random random, int count)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            if (budgetkontogrupper == null)
            {
                throw new ArgumentNullException(nameof(budgetkontogrupper));
            }

            if (random == null)
            {
                throw new ArgumentNullException(nameof(random));
            }

            IList<IBudgetkontoViewModel> result = new List<IBudgetkontoViewModel>(count);
            while (result.Count < count)
            {
                result.Add(fixture.BuildBudgetkontoViewModel(budgetkontogruppeViewModel: budgetkontogrupper[random.Next(0, budgetkontogrupper.Length - 1)]));
            }

            return result;
        }
    }
}