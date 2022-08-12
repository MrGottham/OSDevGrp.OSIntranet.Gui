using System;
using System.ComponentModel;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
using Text = OSDevGrp.OSIntranet.Gui.Resources.Text;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Finansstyring
{
    /// <summary>
    /// Tester ViewModel til en budgetkonto.
    /// </summary>
    [TestFixture]
    public class BudgetkontoViewModelTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en ViewModel til en budgetkonto.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererBudgetkontoViewModel()
        {
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModel = fixture.BuildRegnskabViewModel();
            string kontonummer = fixture.Create<string>();
            string kontonavn = fixture.Create<string>();
            string beskrivelse = fixture.Create<string>();
            string notat = fixture.Create<string>();
            DateTime statusDato = fixture.Create<DateTime>();
            decimal indtægter = fixture.Create<decimal>();
            decimal udgifter = fixture.Create<decimal>();
            decimal budget = fixture.Create<decimal>();
            decimal budgetSidsteMåned = fixture.Create<decimal>();
            decimal budgetÅrTilDato = fixture.Create<decimal>();
            decimal budgetSidsteÅr = fixture.Create<decimal>();
            decimal bogført = fixture.Create<decimal>();
            decimal bogførtSidsteMåned = fixture.Create<decimal>();
            decimal bogførtÅrTilDato = fixture.Create<decimal>();
            decimal bogførtSidsteÅr = fixture.Create<decimal>();
            decimal disponibel = fixture.Create<decimal>();
            Mock<IBudgetkontoModel> budgetkontoModelMock = fixture.BuildBudgetkontoModelMock(kontonummer: kontonummer, kontonavn: kontonavn, beskrivelse: beskrivelse, notat: notat, statusDato: statusDato, indtægter: indtægter, udgifter: udgifter, budget: budget, budgetSidsteMåned: budgetSidsteMåned, budgetÅrTilDato: budgetÅrTilDato, budgetSidsteÅr: budgetSidsteÅr, bogført: bogført, bogførtSidsteMåned: bogførtSidsteMåned, bogførtÅrTilDato: bogførtÅrTilDato, bogførtSidsteÅr: bogførtSidsteÅr, disponibel: disponibel);
            IBudgetkontogruppeViewModel budgetkontogruppeViewModel = fixture.BuildBudgetkontogruppeViewModel();
            IBudgetkontoViewModel budgetkontoViewModel = new BudgetkontoViewModel(regnskabViewModel, budgetkontoModelMock.Object, budgetkontogruppeViewModel, fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel());
            Assert.That(budgetkontoViewModel, Is.Not.Null);
            Assert.That(budgetkontoViewModel.Regnskab, Is.Not.Null);
            Assert.That(budgetkontoViewModel.Regnskab, Is.EqualTo(regnskabViewModel));
            Assert.That(budgetkontoViewModel.Kontonummer, Is.Not.Null);
            Assert.That(budgetkontoViewModel.Kontonummer, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.Kontonummer, Is.EqualTo(kontonummer));
            Assert.That(budgetkontoViewModel.Kontonavn, Is.Not.Null);
            Assert.That(budgetkontoViewModel.Kontonavn, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.Kontonavn, Is.EqualTo(kontonavn));
            Assert.That(budgetkontoViewModel.Beskrivelse, Is.Not.Null);
            Assert.That(budgetkontoViewModel.Beskrivelse, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.Beskrivelse, Is.EqualTo(beskrivelse));
            Assert.That(budgetkontoViewModel.Notat, Is.Not.Null);
            Assert.That(budgetkontoViewModel.Notat, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.Notat, Is.EqualTo(notat));
            Assert.That(budgetkontoViewModel.Kontogruppe, Is.Not.Null);
            Assert.That(budgetkontoViewModel.Kontogruppe, Is.EqualTo(budgetkontogruppeViewModel));
            Assert.That(budgetkontoViewModel.StatusDato, Is.EqualTo(statusDato));
            Assert.That(budgetkontoViewModel.Indtægter, Is.EqualTo(indtægter));
            Assert.That(budgetkontoViewModel.IndtægterAsText, Is.Not.Null);
            Assert.That(budgetkontoViewModel.IndtægterAsText, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.IndtægterAsText, Is.EqualTo(indtægter.ToString("C")));
            Assert.That(budgetkontoViewModel.IndtægterLabel, Is.Not.Null);
            Assert.That(budgetkontoViewModel.IndtægterLabel, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.IndtægterLabel, Is.EqualTo(Resource.GetText(Text.Income)));
            Assert.That(budgetkontoViewModel.Udgifter, Is.EqualTo(udgifter));
            Assert.That(budgetkontoViewModel.UdgifterAsText, Is.Not.Null);
            Assert.That(budgetkontoViewModel.UdgifterAsText, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.UdgifterAsText, Is.EqualTo(udgifter.ToString("C")));
            Assert.That(budgetkontoViewModel.UdgifterLabel, Is.Not.Null);
            Assert.That(budgetkontoViewModel.UdgifterLabel, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.UdgifterLabel, Is.EqualTo(Resource.GetText(Text.Expenses)));
            Assert.That(budgetkontoViewModel.Budget, Is.EqualTo(budget));
            Assert.That(budgetkontoViewModel.BudgetAsText, Is.Not.Null);
            Assert.That(budgetkontoViewModel.BudgetAsText, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.BudgetAsText, Is.EqualTo(budget.ToString("C")));
            Assert.That(budgetkontoViewModel.BudgetLabel, Is.Not.Null);
            Assert.That(budgetkontoViewModel.BudgetLabel, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.BudgetLabel, Is.EqualTo(Resource.GetText(Text.Budget)));
            Assert.That(budgetkontoViewModel.BudgetSidsteMåned, Is.EqualTo(budgetSidsteMåned));
            Assert.That(budgetkontoViewModel.BudgetSidsteMånedAsText, Is.Not.Null);
            Assert.That(budgetkontoViewModel.BudgetSidsteMånedAsText, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.BudgetSidsteMånedAsText, Is.EqualTo(budgetSidsteMåned.ToString("C")));
            Assert.That(budgetkontoViewModel.BudgetSidsteMånedLabel, Is.Not.Null);
            Assert.That(budgetkontoViewModel.BudgetSidsteMånedLabel, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.BudgetSidsteMånedLabel, Is.EqualTo(Resource.GetText(Text.BudgetLastMonth)));
            Assert.That(budgetkontoViewModel.BudgetÅrTilDato, Is.EqualTo(budgetÅrTilDato));
            Assert.That(budgetkontoViewModel.BudgetÅrTilDatoAsText, Is.Not.Null);
            Assert.That(budgetkontoViewModel.BudgetÅrTilDatoAsText, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.BudgetÅrTilDatoAsText, Is.EqualTo(budgetÅrTilDato.ToString("C")));
            Assert.That(budgetkontoViewModel.BudgetÅrTilDatoLabel, Is.Not.Null);
            Assert.That(budgetkontoViewModel.BudgetÅrTilDatoLabel, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.BudgetÅrTilDatoLabel, Is.EqualTo(Resource.GetText(Text.BudgetYearToDate)));
            Assert.That(budgetkontoViewModel.BudgetSidsteÅr, Is.EqualTo(budgetSidsteÅr));
            Assert.That(budgetkontoViewModel.BudgetSidsteÅrAsText, Is.Not.Null);
            Assert.That(budgetkontoViewModel.BudgetSidsteÅrAsText, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.BudgetSidsteÅrAsText, Is.EqualTo(budgetSidsteÅr.ToString("C")));
            Assert.That(budgetkontoViewModel.BudgetSidsteÅrLabel, Is.Not.Null);
            Assert.That(budgetkontoViewModel.BudgetSidsteÅrLabel, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.BudgetSidsteÅrLabel, Is.EqualTo(Resource.GetText(Text.BudgetLastYear)));
            Assert.That(budgetkontoViewModel.Bogført, Is.EqualTo(bogført));
            Assert.That(budgetkontoViewModel.BogførtAsText, Is.Not.Null);
            Assert.That(budgetkontoViewModel.BogførtAsText, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.BogførtAsText, Is.EqualTo(bogført.ToString("C")));
            Assert.That(budgetkontoViewModel.BogførtLabel, Is.Not.Null);
            Assert.That(budgetkontoViewModel.BogførtLabel, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.BogførtLabel, Is.EqualTo(Resource.GetText(Text.Posted)));
            Assert.That(budgetkontoViewModel.BogførtSidsteMåned, Is.EqualTo(bogførtSidsteMåned));
            Assert.That(budgetkontoViewModel.BogførtSidsteMånedAsText, Is.Not.Null);
            Assert.That(budgetkontoViewModel.BogførtSidsteMånedAsText, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.BogførtSidsteMånedAsText, Is.EqualTo(bogførtSidsteMåned.ToString("C")));
            Assert.That(budgetkontoViewModel.BogførtSidsteMånedLabel, Is.Not.Null);
            Assert.That(budgetkontoViewModel.BogførtSidsteMånedLabel, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.BogførtSidsteMånedLabel, Is.EqualTo(Resource.GetText(Text.PostedLastMonth)));
            Assert.That(budgetkontoViewModel.BogførtÅrTilDato, Is.EqualTo(bogførtÅrTilDato));
            Assert.That(budgetkontoViewModel.BogførtÅrTilDatoAsText, Is.Not.Null);
            Assert.That(budgetkontoViewModel.BogførtÅrTilDatoAsText, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.BogførtÅrTilDatoAsText, Is.EqualTo(bogførtÅrTilDato.ToString("C")));
            Assert.That(budgetkontoViewModel.BogførtÅrTilDatoLabel, Is.Not.Null);
            Assert.That(budgetkontoViewModel.BogførtÅrTilDatoLabel, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.BogførtÅrTilDatoLabel, Is.EqualTo(Resource.GetText(Text.Bookkeeping)));
            Assert.That(budgetkontoViewModel.BogførtSidsteÅr, Is.EqualTo(bogførtSidsteÅr));
            Assert.That(budgetkontoViewModel.BogførtSidsteÅrAsText, Is.Not.Null);
            Assert.That(budgetkontoViewModel.BogførtSidsteÅrAsText, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.BogførtSidsteÅrAsText, Is.EqualTo(bogførtSidsteÅr.ToString("C")));
            Assert.That(budgetkontoViewModel.BogførtSidsteÅrLabel, Is.Not.Null);
            Assert.That(budgetkontoViewModel.BogførtSidsteÅrLabel, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.BogførtSidsteÅrLabel, Is.EqualTo(Resource.GetText(Text.PostedLastYear)));
            Assert.That(budgetkontoViewModel.Disponibel, Is.EqualTo(disponibel));
            Assert.That(budgetkontoViewModel.DisponibelAsText, Is.Not.Null);
            Assert.That(budgetkontoViewModel.DisponibelAsText, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.DisponibelAsText, Is.EqualTo(disponibel.ToString("C")));
            Assert.That(budgetkontoViewModel.DisponibelLabel, Is.Not.Null);
            Assert.That(budgetkontoViewModel.DisponibelLabel, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.DisponibelLabel, Is.EqualTo(Resource.GetText(Text.Available)));
            Assert.That(budgetkontoViewModel.Kontoværdi, Is.EqualTo(Math.Abs(bogført)));
            Assert.That(budgetkontoViewModel.DisplayName, Is.Not.Null);
            Assert.That(budgetkontoViewModel.DisplayName, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.DisplayName, Is.EqualTo(Resource.GetText(Text.BudgetAccount)));
            Assert.That(budgetkontoViewModel.Image, Is.Not.Null);
            Assert.That(budgetkontoViewModel.Image, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.Image, Is.EqualTo(Resource.GetEmbeddedResource("Images.Budgetkonto.png")));
            Assert.That(budgetkontoViewModel.ErRegistreret, Is.False);
            Assert.That(budgetkontoViewModel.RefreshCommand, Is.Not.Null);
            Assert.That(budgetkontoViewModel.RefreshCommand, Is.TypeOf<BudgetkontoGetCommand>());

            budgetkontoModelMock.Verify(m => m.Regnskabsnummer, Times.Never);
            budgetkontoModelMock.Verify(m => m.Kontonummer, Times.Once);
            budgetkontoModelMock.Verify(m => m.Kontonavn, Times.Once);
            budgetkontoModelMock.Verify(m => m.Beskrivelse, Times.Once);
            budgetkontoModelMock.Verify(m => m.Notat, Times.Once);
            budgetkontoModelMock.Verify(m => m.Kontogruppe, Times.Never);
            budgetkontoModelMock.Verify(m => m.StatusDato, Times.Once);
            budgetkontoModelMock.Verify(m => m.Indtægter, Times.Once);
            budgetkontoModelMock.Verify(m => m.Udgifter, Times.Once);
            budgetkontoModelMock.Verify(m => m.Budget, Times.Once);
            budgetkontoModelMock.Verify(m => m.BudgetSidsteMåned, Times.Once);
            budgetkontoModelMock.Verify(m => m.BudgetÅrTilDato, Times.Once);
            budgetkontoModelMock.Verify(m => m.BudgetSidsteÅr, Times.Once);
            budgetkontoModelMock.Verify(m => m.Bogført, Times.Once);
            budgetkontoModelMock.Verify(m => m.BogførtSidsteMåned, Times.Once);
            budgetkontoModelMock.Verify(m => m.BogførtÅrTilDato, Times.Once);
            budgetkontoModelMock.Verify(m => m.BogførtSidsteÅr, Times.Once);
            budgetkontoModelMock.Verify(m => m.Disponibel, Times.Once);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis ViewModel for regnskabet er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisRegnskabViewModelErNull()
        {
            Fixture fixture = new Fixture();

            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new BudgetkontoViewModel(null, fixture.BuildBudgetkontoModel(), fixture.BuildBudgetkontogruppeViewModel(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel()));
            // ReSharper restore ObjectCreationAsStatement
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("regnskabViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis modellen for budgetkontoen er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisBudgetontoModelErNull()
        {
            Fixture fixture = new Fixture();

            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new BudgetkontoViewModel(fixture.BuildRegnskabViewModel(), null, fixture.BuildBudgetkontogruppeViewModel(), fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel()));
            // ReSharper restore ObjectCreationAsStatement
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("kontoModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis ViewModel for kontogruppen til budgetkontoen er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisBudgetkontogruppeViewModelErNull()
        {
            Fixture fixture = new Fixture();

            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new BudgetkontoViewModel(fixture.BuildRegnskabViewModel(), fixture.BuildBudgetkontoModel(), null, fixture.BuildFinansstyringRepository(), fixture.BuildExceptionHandlerViewModel()));
            // ReSharper restore ObjectCreationAsStatement
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("kontogruppeViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis repositoryet til finansstyring er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFinansstyringRepositoryErNull()
        {
            Fixture fixture = new Fixture();

            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new BudgetkontoViewModel(fixture.BuildRegnskabViewModel(), fixture.BuildBudgetkontoModel(), fixture.BuildBudgetkontogruppeViewModel(), null, fixture.BuildExceptionHandlerViewModel()));
            // ReSharper restore ObjectCreationAsStatement
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("finansstyringRepository"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis ViewModel for exceptionhandleren er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisExceptionHandlerViewModelErNull()
        {
            Fixture fixture = new Fixture();

            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new BudgetkontoViewModel(fixture.BuildRegnskabViewModel(), fixture.BuildBudgetkontoModel(), fixture.BuildBudgetkontogruppeViewModel(), fixture.BuildFinansstyringRepository(), null));
            // ReSharper restore ObjectCreationAsStatement
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("exceptionHandlerViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at sætteren til Indtægter opdaterer Indtægter på modellen til budgetkontoen.
        /// </summary>
        [Test]
        public void TestAtIndtægterSetterOpdatererIndtægterOnBudgetkontoModel()
        {
            Fixture fixture = new Fixture();

            Mock<IBudgetkontoModel> budgetkontoModelMock = fixture.BuildBudgetkontoModelMock();
            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IBudgetkontoViewModel budgetkontoViewModel = new BudgetkontoViewModel(fixture.BuildRegnskabViewModel(), budgetkontoModelMock.Object, fixture.BuildBudgetkontogruppeViewModel(), fixture.BuildFinansstyringRepository(), exceptionHandlerViewModelMock.Object);
            Assert.That(budgetkontoViewModel, Is.Not.Null);

            decimal newValue = fixture.Create<decimal>();
            budgetkontoViewModel.Indtægter = newValue;

            budgetkontoModelMock.VerifySet(m => m.Indtægter = It.Is<decimal>(value => value == newValue), Times.Once);
            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.IsAny<Exception>()), Times.Never);
        }

        /// <summary>
        /// Tester, at sætteren til Indtægter kalder HandleException på exceptionhandleren ved exceptions.
        /// </summary>
        [Test]
        public void TestAtIndtægterSetterKalderHandleExceptionOnExceptionHandlerViewModelVedExceptions()
        {
            Fixture fixture = new Fixture();

            Exception exception = fixture.Create<Exception>();
            Mock<IBudgetkontoModel> budgetkontoModelMock = fixture.BuildBudgetkontoModelMock(setupCallback: mock => mock.SetupSet(m => m.Indtægter = It.IsAny<decimal>()).Throws(exception));
            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IBudgetkontoViewModel budgetkontoViewModel = new BudgetkontoViewModel(fixture.BuildRegnskabViewModel(), budgetkontoModelMock.Object, fixture.BuildBudgetkontogruppeViewModel(), fixture.BuildFinansstyringRepository(), exceptionHandlerViewModelMock.Object);
            Assert.That(budgetkontoViewModel, Is.Not.Null);

            decimal newValue = fixture.Create<decimal>();
            budgetkontoViewModel.Indtægter = newValue;

            budgetkontoModelMock.VerifySet(m => m.Indtægter = It.Is<decimal>(value => value == newValue), Times.Once);
            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.Is<Exception>(value => value != null && value == exception)), Times.Once);
        }

        /// <summary>
        /// Tester, at sætteren til Udgifter opdaterer Udgifter på modellen til budgetkontoen.
        /// </summary>
        [Test]
        public void TestAtUdgifterSetterOpdatererUdgifterOnBudgetkontoModel()
        {
            Fixture fixture = new Fixture();

            Mock<IBudgetkontoModel> budgetkontoModelMock = fixture.BuildBudgetkontoModelMock();
            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IBudgetkontoViewModel budgetkontoViewModel = new BudgetkontoViewModel(fixture.BuildRegnskabViewModel(), budgetkontoModelMock.Object, fixture.BuildBudgetkontogruppeViewModel(), fixture.BuildFinansstyringRepository(), exceptionHandlerViewModelMock.Object);
            Assert.That(budgetkontoViewModel, Is.Not.Null);

            decimal newValue = fixture.Create<decimal>();
            budgetkontoViewModel.Udgifter = newValue;

            budgetkontoModelMock.VerifySet(m => m.Udgifter = It.Is<decimal>(value => value == newValue), Times.Once);
            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.IsAny<Exception>()), Times.Never);
        }

        /// <summary>
        /// Tester, at sætteren til Udgifter kalder HandleException på exceptionhandleren ved exceptions.
        /// </summary>
        [Test]
        public void TestAtUdgifterSetterKalderHandleExceptionOnExceptionHandlerViewModelVedExceptions()
        {
            Fixture fixture = new Fixture();

            Exception exception = fixture.Create<Exception>();
            Mock<IBudgetkontoModel> budgetkontoModelMock = fixture.BuildBudgetkontoModelMock(setupCallback: mock => mock.SetupSet(m => m.Udgifter = It.IsAny<decimal>()).Throws(exception));
            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IBudgetkontoViewModel budgetkontoViewModel = new BudgetkontoViewModel(fixture.BuildRegnskabViewModel(), budgetkontoModelMock.Object, fixture.BuildBudgetkontogruppeViewModel(), fixture.BuildFinansstyringRepository(), exceptionHandlerViewModelMock.Object);
            Assert.That(budgetkontoViewModel, Is.Not.Null);

            decimal newValue = fixture.Create<decimal>();
            budgetkontoViewModel.Udgifter = newValue;

            budgetkontoModelMock.VerifySet(m => m.Udgifter = It.Is<decimal>(value => value == newValue), Times.Once);
            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.Is<Exception>(value => value != null && value == exception)), Times.Once);
        }

        /// <summary>
        /// Tester, at sætteren til BudgetSidsteMåned opdaterer BudgetSidsteMåned på modellen til budgetkontoen.
        /// </summary>
        [Test]
        public void TestAtBudgetSidsteMånedSetterOpdatererBudgetSidsteMånedOnBudgetkontoModel()
        {
            Fixture fixture = new Fixture();

            Mock<IBudgetkontoModel> budgetkontoModelMock = fixture.BuildBudgetkontoModelMock();
            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IBudgetkontoViewModel budgetkontoViewModel = new BudgetkontoViewModel(fixture.BuildRegnskabViewModel(), budgetkontoModelMock.Object, fixture.BuildBudgetkontogruppeViewModel(), fixture.BuildFinansstyringRepository(), exceptionHandlerViewModelMock.Object);
            Assert.That(budgetkontoViewModel, Is.Not.Null);

            decimal newValue = fixture.Create<decimal>();
            budgetkontoViewModel.BudgetSidsteMåned = newValue;

            budgetkontoModelMock.VerifySet(m => m.BudgetSidsteMåned = It.Is<decimal>(value => value == newValue), Times.Once);
            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.IsAny<Exception>()), Times.Never);
        }

        /// <summary>
        /// Tester, at sætteren til BudgetSidsteMåned kalder HandleException på exceptionhandleren ved exceptions.
        /// </summary>
        [Test]
        public void TestAtBudgetSidsteMånedSetterKalderHandleExceptionOnExceptionHandlerViewModelVedExceptions()
        {
            Fixture fixture = new Fixture();

            Exception exception = fixture.Create<Exception>();
            Mock<IBudgetkontoModel> budgetkontoModelMock = fixture.BuildBudgetkontoModelMock(setupCallback: mock => mock.SetupSet(m => m.BudgetSidsteMåned = It.IsAny<decimal>()).Throws(exception));
            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IBudgetkontoViewModel budgetkontoViewModel = new BudgetkontoViewModel(fixture.BuildRegnskabViewModel(), budgetkontoModelMock.Object, fixture.BuildBudgetkontogruppeViewModel(), fixture.BuildFinansstyringRepository(), exceptionHandlerViewModelMock.Object);
            Assert.That(budgetkontoViewModel, Is.Not.Null);

            decimal newValue = fixture.Create<decimal>();
            budgetkontoViewModel.BudgetSidsteMåned = newValue;

            budgetkontoModelMock.VerifySet(m => m.BudgetSidsteMåned = It.Is<decimal>(value => value == newValue), Times.Once);
            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.Is<Exception>(value => value != null && value == exception)), Times.Once);
        }

        /// <summary>
        /// Tester, at sætteren til BudgetÅrTilDato opdaterer BudgetÅrTilDato på modellen til budgetkontoen.
        /// </summary>
        [Test]
        public void TestAtBudgetÅrTilDatoSetterOpdatererBudgetÅrTilDatoOnBudgetkontoModel()
        {
            Fixture fixture = new Fixture();

            Mock<IBudgetkontoModel> budgetkontoModelMock = fixture.BuildBudgetkontoModelMock();
            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IBudgetkontoViewModel budgetkontoViewModel = new BudgetkontoViewModel(fixture.BuildRegnskabViewModel(), budgetkontoModelMock.Object, fixture.BuildBudgetkontogruppeViewModel(), fixture.BuildFinansstyringRepository(), exceptionHandlerViewModelMock.Object);
            Assert.That(budgetkontoViewModel, Is.Not.Null);

            decimal newValue = fixture.Create<decimal>();
            budgetkontoViewModel.BudgetÅrTilDato = newValue;

            budgetkontoModelMock.VerifySet(m => m.BudgetÅrTilDato = It.Is<decimal>(value => value == newValue), Times.Once);
            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.IsAny<Exception>()), Times.Never);
        }

        /// <summary>
        /// Tester, at sætteren til BudgetÅrTilDato kalder HandleException på exceptionhandleren ved exceptions.
        /// </summary>
        [Test]
        public void TestAtBudgetÅrTilDatoSetterKalderHandleExceptionOnExceptionHandlerViewModelVedExceptions()
        {
            Fixture fixture = new Fixture();

            Exception exception = fixture.Create<Exception>();
            Mock<IBudgetkontoModel> budgetkontoModelMock = fixture.BuildBudgetkontoModelMock(setupCallback: mock => mock.SetupSet(m => m.BudgetÅrTilDato = It.IsAny<decimal>()).Throws(exception));
            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IBudgetkontoViewModel budgetkontoViewModel = new BudgetkontoViewModel(fixture.BuildRegnskabViewModel(), budgetkontoModelMock.Object, fixture.BuildBudgetkontogruppeViewModel(), fixture.BuildFinansstyringRepository(), exceptionHandlerViewModelMock.Object);
            Assert.That(budgetkontoViewModel, Is.Not.Null);

            decimal newValue = fixture.Create<decimal>();
            budgetkontoViewModel.BudgetÅrTilDato = newValue;

            budgetkontoModelMock.VerifySet(m => m.BudgetÅrTilDato = It.Is<decimal>(value => value == newValue), Times.Once);
            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.Is<Exception>(value => value != null && value == exception)), Times.Once);
        }

        /// <summary>
        /// Tester, at sætteren til BudgetSidsteÅr opdaterer BudgetSidsteÅr på modellen til budgetkontoen.
        /// </summary>
        [Test]
        public void TestAtBudgetSidsteÅrSetterOpdatererBudgetSidsteÅrOnBudgetkontoModel()
        {
            Fixture fixture = new Fixture();

            Mock<IBudgetkontoModel> budgetkontoModelMock = fixture.BuildBudgetkontoModelMock();
            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IBudgetkontoViewModel budgetkontoViewModel = new BudgetkontoViewModel(fixture.BuildRegnskabViewModel(), budgetkontoModelMock.Object, fixture.BuildBudgetkontogruppeViewModel(), fixture.BuildFinansstyringRepository(), exceptionHandlerViewModelMock.Object);
            Assert.That(budgetkontoViewModel, Is.Not.Null);

            decimal newValue = fixture.Create<decimal>();
            budgetkontoViewModel.BudgetSidsteÅr = newValue;

            budgetkontoModelMock.VerifySet(m => m.BudgetSidsteÅr = It.Is<decimal>(value => value == newValue), Times.Once);
            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.IsAny<Exception>()), Times.Never);
        }

        /// <summary>
        /// Tester, at sætteren til BudgetSidsteÅr kalder HandleException på exceptionhandleren ved exceptions.
        /// </summary>
        [Test]
        public void TestAtBudgetSidsteÅrSetterKalderHandleExceptionOnExceptionHandlerViewModelVedExceptions()
        {
            Fixture fixture = new Fixture();

            Exception exception = fixture.Create<Exception>();
            Mock<IBudgetkontoModel> budgetkontoModelMock = fixture.BuildBudgetkontoModelMock(setupCallback: mock => mock.SetupSet(m => m.BudgetSidsteÅr = It.IsAny<decimal>()).Throws(exception));
            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IBudgetkontoViewModel budgetkontoViewModel = new BudgetkontoViewModel(fixture.BuildRegnskabViewModel(), budgetkontoModelMock.Object, fixture.BuildBudgetkontogruppeViewModel(), fixture.BuildFinansstyringRepository(), exceptionHandlerViewModelMock.Object);
            Assert.That(budgetkontoViewModel, Is.Not.Null);

            decimal newValue = fixture.Create<decimal>();
            budgetkontoViewModel.BudgetSidsteÅr = newValue;

            budgetkontoModelMock.VerifySet(m => m.BudgetSidsteÅr = It.Is<decimal>(value => value == newValue), Times.Once);
            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.Is<Exception>(value => value != null && value == exception)), Times.Once);
        }

        /// <summary>
        /// Tester, at sætteren til Bogført opdaterer Bogført på modellen til budgetkontoen.
        /// </summary>
        [Test]
        public void TestAtBogførtSetterOpdatererBogførtOnBudgetkontoModel()
        {
            Fixture fixture = new Fixture();

            Mock<IBudgetkontoModel> budgetkontoModelMock = fixture.BuildBudgetkontoModelMock();
            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IBudgetkontoViewModel budgetkontoViewModel = new BudgetkontoViewModel(fixture.BuildRegnskabViewModel(), budgetkontoModelMock.Object, fixture.BuildBudgetkontogruppeViewModel(), fixture.BuildFinansstyringRepository(), exceptionHandlerViewModelMock.Object);
            Assert.That(budgetkontoViewModel, Is.Not.Null);

            decimal newValue = fixture.Create<decimal>();
            budgetkontoViewModel.Bogført = newValue;

            budgetkontoModelMock.VerifySet(m => m.Bogført = It.Is<decimal>(value => value == newValue), Times.Once);
            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.IsAny<Exception>()), Times.Never);
        }

        /// <summary>
        /// Tester, at sætteren til Bogført kalder HandleException på exceptionhandleren ved exceptions.
        /// </summary>
        [Test]
        public void TestAtBogførtSetterKalderHandleExceptionOnExceptionHandlerViewModelVedExceptions()
        {
            Fixture fixture = new Fixture();

            Exception exception = fixture.Create<Exception>();
            Mock<IBudgetkontoModel> budgetkontoModelMock = fixture.BuildBudgetkontoModelMock(setupCallback: mock => mock.SetupSet(m => m.Bogført = It.IsAny<decimal>()).Throws(exception));
            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IBudgetkontoViewModel budgetkontoViewModel = new BudgetkontoViewModel(fixture.BuildRegnskabViewModel(), budgetkontoModelMock.Object, fixture.BuildBudgetkontogruppeViewModel(), fixture.BuildFinansstyringRepository(), exceptionHandlerViewModelMock.Object);
            Assert.That(budgetkontoViewModel, Is.Not.Null);

            decimal newValue = fixture.Create<decimal>();
            budgetkontoViewModel.Bogført = newValue;

            budgetkontoModelMock.VerifySet(m => m.Bogført = It.Is<decimal>(value => value == newValue), Times.Once);
            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.Is<Exception>(value => value != null && value == exception)), Times.Once);
        }

        /// <summary>
        /// Tester, at sætteren til BogførtSidsteMåned opdaterer BogførtSidsteMåned på modellen til budgetkontoen.
        /// </summary>
        [Test]
        public void TestAtBogførtSidsteMånedSetterOpdatererBogførtSidsteMånedOnBudgetkontoModel()
        {
            Fixture fixture = new Fixture();

            Mock<IBudgetkontoModel> budgetkontoModelMock = fixture.BuildBudgetkontoModelMock();
            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IBudgetkontoViewModel budgetkontoViewModel = new BudgetkontoViewModel(fixture.BuildRegnskabViewModel(), budgetkontoModelMock.Object, fixture.BuildBudgetkontogruppeViewModel(), fixture.BuildFinansstyringRepository(), exceptionHandlerViewModelMock.Object);
            Assert.That(budgetkontoViewModel, Is.Not.Null);

            decimal newValue = fixture.Create<decimal>();
            budgetkontoViewModel.BogførtSidsteMåned = newValue;

            budgetkontoModelMock.VerifySet(m => m.BogførtSidsteMåned = It.Is<decimal>(value => value == newValue), Times.Once);
            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.IsAny<Exception>()), Times.Never);
        }

        /// <summary>
        /// Tester, at sætteren til BogførtSidsteMåned kalder HandleException på exceptionhandleren ved exceptions.
        /// </summary>
        [Test]
        public void TestAtBogførtSidsteMånedSetterKalderHandleExceptionOnExceptionHandlerViewModelVedExceptions()
        {
            Fixture fixture = new Fixture();

            Exception exception = fixture.Create<Exception>();
            Mock<IBudgetkontoModel> budgetkontoModelMock = fixture.BuildBudgetkontoModelMock(setupCallback: mock => mock.SetupSet(m => m.BogførtSidsteMåned = It.IsAny<decimal>()).Throws(exception));
            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IBudgetkontoViewModel budgetkontoViewModel = new BudgetkontoViewModel(fixture.BuildRegnskabViewModel(), budgetkontoModelMock.Object, fixture.BuildBudgetkontogruppeViewModel(), fixture.BuildFinansstyringRepository(), exceptionHandlerViewModelMock.Object);
            Assert.That(budgetkontoViewModel, Is.Not.Null);

            decimal newValue = fixture.Create<decimal>();
            budgetkontoViewModel.BogførtSidsteMåned = newValue;

            budgetkontoModelMock.VerifySet(m => m.BogførtSidsteMåned = It.Is<decimal>(value => value == newValue), Times.Once);
            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.Is<Exception>(value => value != null && value == exception)), Times.Once);
        }

        /// <summary>
        /// Tester, at sætteren til BogførtÅrTilDato opdaterer BogførtÅrTilDato på modellen til budgetkontoen.
        /// </summary>
        [Test]
        public void TestAtBogførtÅrTilDatoSetterOpdatererBogførtÅrTilDatoOnBudgetkontoModel()
        {
            Fixture fixture = new Fixture();

            Mock<IBudgetkontoModel> budgetkontoModelMock = fixture.BuildBudgetkontoModelMock();
            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IBudgetkontoViewModel budgetkontoViewModel = new BudgetkontoViewModel(fixture.BuildRegnskabViewModel(), budgetkontoModelMock.Object, fixture.BuildBudgetkontogruppeViewModel(), fixture.BuildFinansstyringRepository(), exceptionHandlerViewModelMock.Object);
            Assert.That(budgetkontoViewModel, Is.Not.Null);

            decimal newValue = fixture.Create<decimal>();
            budgetkontoViewModel.BogførtÅrTilDato = newValue;

            budgetkontoModelMock.VerifySet(m => m.BogførtÅrTilDato = It.Is<decimal>(value => value == newValue), Times.Once);
            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.IsAny<Exception>()), Times.Never);
        }

        /// <summary>
        /// Tester, at sætteren til BogførtÅrTilDato kalder HandleException på exceptionhandleren ved exceptions.
        /// </summary>
        [Test]
        public void TestAtBogførtÅrTilDatoSetterKalderHandleExceptionOnExceptionHandlerViewModelVedExceptions()
        {
            Fixture fixture = new Fixture();

            Exception exception = fixture.Create<Exception>();
            Mock<IBudgetkontoModel> budgetkontoModelMock = fixture.BuildBudgetkontoModelMock(setupCallback: mock => mock.SetupSet(m => m.BogførtÅrTilDato = It.IsAny<decimal>()).Throws(exception));
            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IBudgetkontoViewModel budgetkontoViewModel = new BudgetkontoViewModel(fixture.BuildRegnskabViewModel(), budgetkontoModelMock.Object, fixture.BuildBudgetkontogruppeViewModel(), fixture.BuildFinansstyringRepository(), exceptionHandlerViewModelMock.Object);
            Assert.That(budgetkontoViewModel, Is.Not.Null);

            decimal newValue = fixture.Create<decimal>();
            budgetkontoViewModel.BogførtÅrTilDato = newValue;

            budgetkontoModelMock.VerifySet(m => m.BogførtÅrTilDato = It.Is<decimal>(value => value == newValue), Times.Once);
            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.Is<Exception>(value => value != null && value == exception)), Times.Once);
        }

        /// <summary>
        /// Tester, at sætteren til BogførtSidsteÅr opdaterer BogførtSidsteÅr på modellen til budgetkontoen.
        /// </summary>
        [Test]
        public void TestAtBogførtSidsteÅrSetterOpdatererBogførtSidsteÅrOnBudgetkontoModel()
        {
            Fixture fixture = new Fixture();

            Mock<IBudgetkontoModel> budgetkontoModelMock = fixture.BuildBudgetkontoModelMock();
            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IBudgetkontoViewModel budgetkontoViewModel = new BudgetkontoViewModel(fixture.BuildRegnskabViewModel(), budgetkontoModelMock.Object, fixture.BuildBudgetkontogruppeViewModel(), fixture.BuildFinansstyringRepository(), exceptionHandlerViewModelMock.Object);
            Assert.That(budgetkontoViewModel, Is.Not.Null);

            decimal newValue = fixture.Create<decimal>();
            budgetkontoViewModel.BogførtSidsteÅr = newValue;

            budgetkontoModelMock.VerifySet(m => m.BogførtSidsteÅr = It.Is<decimal>(value => value == newValue), Times.Once);
            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.IsAny<Exception>()), Times.Never);
        }

        /// <summary>
        /// Tester, at sætteren til BogførtSidsteÅr kalder HandleException på exceptionhandleren ved exceptions.
        /// </summary>
        [Test]
        public void TestAtBogførtSidsteÅrSetterKalderHandleExceptionOnExceptionHandlerViewModelVedExceptions()
        {
            Fixture fixture = new Fixture();

            Exception exception = fixture.Create<Exception>();
            Mock<IBudgetkontoModel> budgetkontoModelMock = fixture.BuildBudgetkontoModelMock(setupCallback: mock => mock.SetupSet(m => m.BogførtSidsteÅr = It.IsAny<decimal>()).Throws(exception));
            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IBudgetkontoViewModel budgetkontoViewModel = new BudgetkontoViewModel(fixture.BuildRegnskabViewModel(), budgetkontoModelMock.Object, fixture.BuildBudgetkontogruppeViewModel(), fixture.BuildFinansstyringRepository(), exceptionHandlerViewModelMock.Object);
            Assert.That(budgetkontoViewModel, Is.Not.Null);

            decimal newValue = fixture.Create<decimal>();
            budgetkontoViewModel.BogførtSidsteÅr = newValue;

            budgetkontoModelMock.VerifySet(m => m.BogførtSidsteÅr = It.Is<decimal>(value => value == newValue), Times.Once);
            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.Is<Exception>(value => value != null && value == exception)), Times.Once);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnKontoModelEventHandler rejser PropertyChanged, når modellen for budgetkontoen opdateres.
        /// </summary>
        [Test]
        [TestCase("Regnskabsnummer", "Regnskabsnummer")]
        [TestCase("Kontonummer", "Kontonummer")]
        [TestCase("Kontonavn", "Kontonavn")]
        [TestCase("Beskrivelse", "Beskrivelse")]
        [TestCase("Notat", "Notat")]
        [TestCase("Kontogruppe", "Kontogruppe")]
        [TestCase("StatusDato", "StatusDato")]
        [TestCase("Indtægter", "Indtægter")]
        [TestCase("Indtægter", "IndtægterAsText")]
        [TestCase("Udgifter", "Udgifter")]
        [TestCase("Udgifter", "UdgifterAsText")]
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
        [TestCase("Disponibel", "Disponibel")]
        [TestCase("Disponibel", "DisponibelAsText")]
        [TestCase("Bogført", "Kontoværdi")]
        public void TestAtPropertyChangedOnKontoModelEventHandlerRejserPropertyChangedOnBudgetkontoModelUpdate(string propertyNameToRaise, string expectPropertyName)
        {
            Fixture fixture = new Fixture();

            Mock<IBudgetkontoModel> budgetkontoModelMock = fixture.BuildBudgetkontoModelMock();
            Mock<IExceptionHandlerViewModel> exceptionHandlerViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            IBudgetkontoViewModel budgetkontoViewModel = new BudgetkontoViewModel(fixture.BuildRegnskabViewModel(), budgetkontoModelMock.Object, fixture.BuildBudgetkontogruppeViewModel(), fixture.BuildFinansstyringRepository(), exceptionHandlerViewModelMock.Object);
            Assert.That(budgetkontoViewModel, Is.Not.Null);

            bool  eventCalled = false;
            budgetkontoViewModel.PropertyChanged += (s, e) =>
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
            budgetkontoModelMock.Raise(m => m.PropertyChanged += null, budgetkontoModelMock, new PropertyChangedEventArgs(propertyNameToRaise));
            Assert.That(eventCalled, Is.True);

            exceptionHandlerViewModelMock.Verify(m => m.HandleException(It.IsAny<Exception>()), Times.Never);
        }
    }
}