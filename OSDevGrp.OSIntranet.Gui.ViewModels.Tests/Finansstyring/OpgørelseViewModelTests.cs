using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
using Ploeh.AutoFixture;
using Rhino.Mocks;
using Text=OSDevGrp.OSIntranet.Gui.Resources.Text;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Finansstyring
{
    /// <summary>
    /// Tester ViewModel for en linje i opgørelsen.
    /// </summary>
    [TestFixture]
    public class OpgørelseViewModelTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en ViewModel til en linje i opgørelsen.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererOpgørelseViewModel()
        {
            var fixture = new Fixture();

            var budgetkontogruppeModelMock = MockRepository.GenerateMock<IBudgetkontogruppeModel>();
            budgetkontogruppeModelMock.Expect(m => m.Id)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            budgetkontogruppeModelMock.Expect(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();
            budgetkontogruppeModelMock.Expect(m => m.Tekst)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var budgetkontogruppeViewModelMock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
            budgetkontogruppeViewModelMock.Expect(m => m.Nummer)
                .Return(budgetkontogruppeModelMock.Nummer)
                .Repeat.Any();

            var budgetkontoViewModelCollection = new List<IBudgetkontoViewModel>(fixture.Create<Generator<int>>().First(m => m >= 25 && m <= 100));
            while (budgetkontoViewModelCollection.Count < budgetkontoViewModelCollection.Capacity)
            {
                var budgetkontoViewModelMock = MockRepository.GenerateMock<IBudgetkontoViewModel>();
                budgetkontoViewModelMock.Expect(m => m.Kontonummer)
                    .Return(fixture.Create<string>())
                    .Repeat.Any();
                budgetkontoViewModelMock.Expect(m => m.Kontogruppe)
                    .Return(budgetkontogruppeViewModelMock)
                    .Repeat.Any();
                budgetkontoViewModelMock.Expect(m => m.Budget)
                    .Return(fixture.Create<decimal>())
                    .Repeat.Any();
                budgetkontoViewModelMock.Expect(m => m.BudgetSidsteMåned)
                    .Return(fixture.Create<decimal>())
                    .Repeat.Any();
                budgetkontoViewModelMock.Expect(m => m.BudgetÅrTilDato)
                    .Return(fixture.Create<decimal>())
                    .Repeat.Any();
                budgetkontoViewModelMock.Expect(m => m.BudgetSidsteÅr)
                    .Return(fixture.Create<decimal>())
                    .Repeat.Any();
                budgetkontoViewModelMock.Expect(m => m.Bogført)
                    .Return(fixture.Create<decimal>())
                    .Repeat.Any();
                budgetkontoViewModelMock.Expect(m => m.BogførtSidsteMåned)
                    .Return(fixture.Create<decimal>())
                    .Repeat.Any();
                budgetkontoViewModelMock.Expect(m => m.BogførtÅrTilDato)
                    .Return(fixture.Create<decimal>())
                    .Repeat.Any();
                budgetkontoViewModelMock.Expect(m => m.BogførtSidsteÅr)
                    .Return(fixture.Create<decimal>())
                    .Repeat.Any();
                budgetkontoViewModelMock.Expect(m => m.Disponibel)
                    .Return(fixture.Create<decimal>())
                    .Repeat.Any();
                budgetkontoViewModelMock.Expect(m => m.ErRegistreret)
                    .Return(true)
                    .Repeat.Any();
                budgetkontoViewModelCollection.Add(budgetkontoViewModelMock);
            }
            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Budgetkonti)
                .Return(budgetkontoViewModelCollection)
                .Repeat.Any();

            var exceptionHandleViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var opgørelseViewModel = new OpgørelseViewModel(regnskabViewModelMock, budgetkontogruppeModelMock, exceptionHandleViewModelMock);
            Assert.That(opgørelseViewModel, Is.Not.Null);
            Assert.That(opgørelseViewModel.Id, Is.Not.Null);
            Assert.That(opgørelseViewModel.Id, Is.Not.Empty);
            Assert.That(opgørelseViewModel.Id, Is.EqualTo(budgetkontogruppeModelMock.Id));
            Assert.That(opgørelseViewModel.Nummer, Is.EqualTo(budgetkontogruppeModelMock.Nummer));
            Assert.That(opgørelseViewModel.Tekst, Is.Not.Null);
            Assert.That(opgørelseViewModel.Tekst, Is.Not.Empty);
            Assert.That(opgørelseViewModel.Tekst, Is.EqualTo(budgetkontogruppeModelMock.Tekst));
            Assert.That(opgørelseViewModel.DisplayName, Is.Not.Null);
            Assert.That(opgørelseViewModel.DisplayName, Is.Not.Empty);
            Assert.That(opgørelseViewModel.DisplayName, Is.EqualTo(budgetkontogruppeModelMock.Tekst));
            Assert.That(opgørelseViewModel.Regnskab, Is.Not.Null);
            Assert.That(opgørelseViewModel.Regnskab, Is.EqualTo(regnskabViewModelMock));
            Assert.That(opgørelseViewModel.Budgetkonti, Is.Not.Null);
            Assert.That(opgørelseViewModel.Budgetkonti, Is.Not.Empty);
            Assert.That(opgørelseViewModel.Budgetkonti.Count(), Is.EqualTo(budgetkontoViewModelCollection.Count));
            Assert.That(opgørelseViewModel.Budget, Is.EqualTo(budgetkontoViewModelCollection.Sum(m => m.Budget)));
            Assert.That(opgørelseViewModel.BudgetAsText, Is.Not.Null);
            Assert.That(opgørelseViewModel.BudgetAsText, Is.Not.Empty);
            Assert.That(opgørelseViewModel.BudgetAsText, Is.EqualTo(budgetkontoViewModelCollection.Sum(m => m.Budget).ToString("C")));
            Assert.That(opgørelseViewModel.BudgetLabel, Is.Not.Null);
            Assert.That(opgørelseViewModel.BudgetLabel, Is.Not.Empty);
            Assert.That(opgørelseViewModel.BudgetLabel, Is.EqualTo(Resource.GetText(Text.Budget)));
            Assert.That(opgørelseViewModel.BudgetSidsteMåned, Is.EqualTo(budgetkontoViewModelCollection.Sum(m => m.BudgetSidsteMåned)));
            Assert.That(opgørelseViewModel.BudgetSidsteMånedAsText, Is.Not.Null);
            Assert.That(opgørelseViewModel.BudgetSidsteMånedAsText, Is.Not.Empty);
            Assert.That(opgørelseViewModel.BudgetSidsteMånedAsText, Is.EqualTo(budgetkontoViewModelCollection.Sum(m => m.BudgetSidsteMåned).ToString("C")));
            Assert.That(opgørelseViewModel.BudgetSidsteMånedLabel, Is.Not.Null);
            Assert.That(opgørelseViewModel.BudgetSidsteMånedLabel, Is.Not.Empty);
            Assert.That(opgørelseViewModel.BudgetSidsteMånedLabel, Is.EqualTo(Resource.GetText(Text.BudgetLastMonth)));
            Assert.That(opgørelseViewModel.BudgetÅrTilDato, Is.EqualTo(budgetkontoViewModelCollection.Sum(m => m.BudgetÅrTilDato)));
            Assert.That(opgørelseViewModel.BudgetÅrTilDatoAsText, Is.Not.Null);
            Assert.That(opgørelseViewModel.BudgetÅrTilDatoAsText, Is.Not.Empty);
            Assert.That(opgørelseViewModel.BudgetÅrTilDatoAsText, Is.EqualTo(budgetkontoViewModelCollection.Sum(m => m.BudgetÅrTilDato).ToString("C")));
            Assert.That(opgørelseViewModel.BudgetÅrTilDatoLabel, Is.Not.Null);
            Assert.That(opgørelseViewModel.BudgetÅrTilDatoLabel, Is.Not.Empty);
            Assert.That(opgørelseViewModel.BudgetÅrTilDatoLabel, Is.EqualTo(Resource.GetText(Text.BudgetYearToDate)));
            Assert.That(opgørelseViewModel.BudgetSidsteÅr, Is.EqualTo(budgetkontoViewModelCollection.Sum(m => m.BudgetSidsteÅr)));
            Assert.That(opgørelseViewModel.BudgetSidsteÅrAsText, Is.Not.Null);
            Assert.That(opgørelseViewModel.BudgetSidsteÅrAsText, Is.Not.Empty);
            Assert.That(opgørelseViewModel.BudgetSidsteÅrAsText, Is.EqualTo(budgetkontoViewModelCollection.Sum(m => m.BudgetSidsteÅr).ToString("C")));
            Assert.That(opgørelseViewModel.BudgetSidsteÅrLabel, Is.Not.Null);
            Assert.That(opgørelseViewModel.BudgetSidsteÅrLabel, Is.Not.Empty);
            Assert.That(opgørelseViewModel.BudgetSidsteÅrLabel, Is.EqualTo(Resource.GetText(Text.BudgetLastYear)));
            Assert.That(opgørelseViewModel.Bogført, Is.EqualTo(budgetkontoViewModelCollection.Sum(m => m.Bogført)));
            Assert.That(opgørelseViewModel.BogførtAsText, Is.Not.Null);
            Assert.That(opgørelseViewModel.BogførtAsText, Is.Not.Empty);
            Assert.That(opgørelseViewModel.BogførtAsText, Is.EqualTo(budgetkontoViewModelCollection.Sum(m => m.Bogført).ToString("C")));
            Assert.That(opgørelseViewModel.BogførtLabel, Is.Not.Null);
            Assert.That(opgørelseViewModel.BogførtLabel, Is.Not.Empty);
            Assert.That(opgørelseViewModel.BogførtLabel, Is.EqualTo(Resource.GetText(Text.Bookkeeped)));
            Assert.That(opgørelseViewModel.BogførtSidsteMåned, Is.EqualTo(budgetkontoViewModelCollection.Sum(m => m.BogførtSidsteMåned)));
            Assert.That(opgørelseViewModel.BogførtSidsteMånedAsText, Is.Not.Null);
            Assert.That(opgørelseViewModel.BogførtSidsteMånedAsText, Is.Not.Empty);
            Assert.That(opgørelseViewModel.BogførtSidsteMånedAsText, Is.EqualTo(budgetkontoViewModelCollection.Sum(m => m.BogførtSidsteMåned).ToString("C")));
            Assert.That(opgørelseViewModel.BogførtSidsteMånedLabel, Is.Not.Null);
            Assert.That(opgørelseViewModel.BogførtSidsteMånedLabel, Is.Not.Empty);
            Assert.That(opgørelseViewModel.BogførtSidsteMånedLabel, Is.EqualTo(Resource.GetText(Text.BookkeepedLastMonth)));
            Assert.That(opgørelseViewModel.BogførtÅrTilDato, Is.EqualTo(budgetkontoViewModelCollection.Sum(m => m.BogførtÅrTilDato)));
            Assert.That(opgørelseViewModel.BogførtÅrTilDatoAsText, Is.Not.Null);
            Assert.That(opgørelseViewModel.BogførtÅrTilDatoAsText, Is.Not.Empty);
            Assert.That(opgørelseViewModel.BogførtÅrTilDatoAsText, Is.EqualTo(budgetkontoViewModelCollection.Sum(m => m.BogførtÅrTilDato).ToString("C")));
            Assert.That(opgørelseViewModel.BogførtÅrTilDatoLabel, Is.Not.Null);
            Assert.That(opgørelseViewModel.BogførtÅrTilDatoLabel, Is.Not.Empty);
            Assert.That(opgørelseViewModel.BogførtÅrTilDatoLabel, Is.EqualTo(Resource.GetText(Text.BookkeepedYearToDate)));
            Assert.That(opgørelseViewModel.BogførtSidsteÅr, Is.EqualTo(budgetkontoViewModelCollection.Sum(m => m.BogførtSidsteÅr)));
            Assert.That(opgørelseViewModel.BogførtSidsteÅrAsText, Is.Not.Null);
            Assert.That(opgørelseViewModel.BogførtSidsteÅrAsText, Is.Not.Empty);
            Assert.That(opgørelseViewModel.BogførtSidsteÅrAsText, Is.EqualTo(budgetkontoViewModelCollection.Sum(m => m.BogførtSidsteÅr).ToString("C")));
            Assert.That(opgørelseViewModel.BogførtSidsteÅrLabel, Is.Not.Null);
            Assert.That(opgørelseViewModel.BogførtSidsteÅrLabel, Is.Not.Empty);
            Assert.That(opgørelseViewModel.BogførtSidsteÅrLabel, Is.EqualTo(Resource.GetText(Text.BookkeepedLastYear)));
            Assert.That(opgørelseViewModel.Disponibel, Is.EqualTo(budgetkontoViewModelCollection.Sum(m => m.Disponibel)));
            Assert.That(opgørelseViewModel.DisponibelAsText, Is.Not.Null);
            Assert.That(opgørelseViewModel.DisponibelAsText, Is.Not.Empty);
            Assert.That(opgørelseViewModel.DisponibelAsText, Is.EqualTo(budgetkontoViewModelCollection.Sum(m => m.Disponibel).ToString("C")));
            Assert.That(opgørelseViewModel.DisponibelLabel, Is.Not.Null);
            Assert.That(opgørelseViewModel.DisponibelLabel, Is.Not.Empty);
            Assert.That(opgørelseViewModel.DisponibelLabel, Is.EqualTo(Resource.GetText(Text.Available)));

            budgetkontogruppeModelMock.AssertWasCalled(m => m.Id);
            budgetkontogruppeModelMock.AssertWasCalled(m => m.Nummer);
            budgetkontogruppeModelMock.AssertWasCalled(m => m.Tekst);

            regnskabViewModelMock.AssertWasCalled(m => m.Budgetkonti);

            exceptionHandleViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis ViewModel for regnskabet, som opgørelseslinjen er tilknyttet, er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisRegnskabViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IBudgetkontogruppeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontogruppeModel>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new OpgørelseViewModel(null, fixture.Create<IBudgetkontogruppeModel>(), fixture.Create<IExceptionHandlerViewModel>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("regnskabViewModel"));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis modellen for gruppen af budgetkonti, som opgørelseslinjen baserer sig på, er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisBudgetkontogruppeModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new OpgørelseViewModel(fixture.Create<IRegnskabViewModel>(), null, fixture.Create<IExceptionHandlerViewModel>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("tabelModel"));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis ViewModel for exceptionhandleren er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisExceptionHandlerViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IBudgetkontogruppeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontogruppeModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new OpgørelseViewModel(fixture.Create<IRegnskabViewModel>(), fixture.Create<IBudgetkontogruppeModel>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("exceptionHandlerViewModel"));
        }

        /// <summary>
        /// Tester, at sætteren til Tekst opdaterer Tekst på modellen for gruppen af budgetkonti, som opgørelseslinjen baserer sig på.
        /// </summary>
        [Test]
        public void TestAtTekstSetterOpdatererTekstOnOpgørelseViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var budgetkontogruppeModelMock = MockRepository.GenerateMock<IBudgetkontogruppeModel>();
            var exceptionHandleViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var opgørelseViewModel = new OpgørelseViewModel(fixture.Create<IRegnskabViewModel>(), budgetkontogruppeModelMock, exceptionHandleViewModelMock);
            Assert.That(opgørelseViewModel, Is.Not.Null);

            var newValue = fixture.Create<string>();
            opgørelseViewModel.Tekst = newValue;

            budgetkontogruppeModelMock.AssertWasCalled(m => m.Tekst = Arg<string>.Is.Equal(newValue));
            exceptionHandleViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at sætteren til Tekst kalder HandleException på exceptionhandleren ved exceptions.
        /// </summary>
        [Test]
        public void TestAtTekstSetterKalderHandleExceptionOnExceptionHandlerViewModelVedExceptions()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = fixture.Create<Exception>();
            var budgetkontogruppeModelMock = MockRepository.GenerateMock<IBudgetkontogruppeModel>();
            budgetkontogruppeModelMock.Expect(m => m.Tekst = Arg<string>.Is.Anything)
                .Throw(exception)
                .Repeat.Any();

            var exceptionHandleViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var opgørelseViewModel = new OpgørelseViewModel(fixture.Create<IRegnskabViewModel>(), budgetkontogruppeModelMock, exceptionHandleViewModelMock);
            Assert.That(opgørelseViewModel, Is.Not.Null);

            var newValue = fixture.Create<string>();
            opgørelseViewModel.Tekst = newValue;

            budgetkontogruppeModelMock.AssertWasCalled(m => m.Tekst = Arg<string>.Is.Equal(newValue));
            exceptionHandleViewModelMock.AssertWasCalled(m => m.HandleException(Arg<Exception>.Is.Equal(exception)));
        }

        /// <summary>
        /// Tester, at Register kaster en ArgumentNullException, hvis budgetkontoen, der skal indgå i opgørelseslinjen, er null.
        /// </summary>
        [Test]
        public void TestAtRegisterKasterArgumentNullExceptionHvisBudgetkontoViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IBudgetkontogruppeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontogruppeModel>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var opgørelseViewModel = new OpgørelseViewModel(fixture.Create<IRegnskabViewModel>(), fixture.Create<IBudgetkontogruppeModel>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(opgørelseViewModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => opgørelseViewModel.Register(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("budgetkontoViewModel"));
        }

        /// <summary>
        /// Tester, at Register returnerer, hvis budgetkontoen ikke er tilknyttet regnskabet.
        /// </summary>
        [Test]
        public void TestAtRegisterReturnererHvisBudgetkontoViewModelIkkeErTilknyttetRegnskabViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var budgetkontogruppeModelMock = MockRepository.GenerateMock<IBudgetkontogruppeModel>();
            budgetkontogruppeModelMock.Expect(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();

            var budgetkontogruppeViewModelMock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
            budgetkontogruppeViewModelMock.Expect(m => m.Nummer)
                .Return(budgetkontogruppeModelMock.Nummer)
                .Repeat.Any();

            var budgetkontoViewModel = MockRepository.GenerateMock<IBudgetkontoViewModel>();
            budgetkontoViewModel.Expect(m => m.Kontogruppe)
                .Return(budgetkontogruppeViewModelMock)
                .Repeat.Any();
            budgetkontoViewModel.Expect(m => m.ErRegistreret)
                .Return(false)
                .Repeat.Any();

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Budgetkonti)
                .Return(new List<IBudgetkontoViewModel>(0))
                .Repeat.Any();

            var exceptionHandlerViewModel = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var opgørelseViewModel = new OpgørelseViewModel(regnskabViewModelMock, budgetkontogruppeModelMock, exceptionHandlerViewModel);
            Assert.That(opgørelseViewModel, Is.Not.Null);

            opgørelseViewModel.Register(budgetkontoViewModel);

            budgetkontoViewModel.AssertWasNotCalled(m => m.ErRegistreret = Arg<bool>.Is.Anything);
            exceptionHandlerViewModel.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Register returnerer, hvis kontogruppen på budgetkontoen er null.
        /// </summary>
        [Test]
        public void TestAtRegisterReturnererHvisKontogruppeOnBudgetkontoViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var budgetkontogruppeModelMock = MockRepository.GenerateMock<IBudgetkontogruppeModel>();
            budgetkontogruppeModelMock.Expect(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();

            var budgetkontoViewModel = MockRepository.GenerateMock<IBudgetkontoViewModel>();
            budgetkontoViewModel.Expect(m => m.Kontogruppe)
                .Return(null)
                .Repeat.Any();
            budgetkontoViewModel.Expect(m => m.ErRegistreret)
                .Return(false)
                .Repeat.Any();

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Budgetkonti)
                .Return(new List<IBudgetkontoViewModel> {budgetkontoViewModel})
                .Repeat.Any();

            var exceptionHandlerViewModel = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var opgørelseViewModel = new OpgørelseViewModel(regnskabViewModelMock, budgetkontogruppeModelMock, exceptionHandlerViewModel);
            Assert.That(opgørelseViewModel, Is.Not.Null);

            opgørelseViewModel.Register(budgetkontoViewModel);

            budgetkontoViewModel.AssertWasNotCalled(m => m.ErRegistreret = Arg<bool>.Is.Anything);
            exceptionHandlerViewModel.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Register returnerer, hvis kontogruppen på budgetkontoen ikke matcher kontogruppen for budgetkonti, som opgørelseslinjen baserer sig på.
        /// </summary>
        [Test]
        public void TestAtRegisterReturnererHvisKontogruppeOnBudgetkontoViewModelIkkeMatcherOpgørelseViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var budgetkontogruppeModelMock = MockRepository.GenerateMock<IBudgetkontogruppeModel>();
            budgetkontogruppeModelMock.Expect(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();

            var budgetkontogruppeViewModelMock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
            budgetkontogruppeViewModelMock.Expect(m => m.Nummer)
                .Return(fixture.Create<Generator<int>>().First(m => m != budgetkontogruppeModelMock.Nummer))
                .Repeat.Any();

            var budgetkontoViewModel = MockRepository.GenerateMock<IBudgetkontoViewModel>();
            budgetkontoViewModel.Expect(m => m.Kontogruppe)
                .Return(budgetkontogruppeViewModelMock)
                .Repeat.Any();
            budgetkontoViewModel.Expect(m => m.ErRegistreret)
                .Return(false)
                .Repeat.Any();

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Budgetkonti)
                .Return(new List<IBudgetkontoViewModel> {budgetkontoViewModel})
                .Repeat.Any();

            var exceptionHandlerViewModel = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var opgørelseViewModel = new OpgørelseViewModel(regnskabViewModelMock, budgetkontogruppeModelMock, exceptionHandlerViewModel);
            Assert.That(opgørelseViewModel, Is.Not.Null);

            opgørelseViewModel.Register(budgetkontoViewModel);

            budgetkontoViewModel.AssertWasNotCalled(m => m.ErRegistreret = Arg<bool>.Is.Anything);
            exceptionHandlerViewModel.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Register returnerer, hvis budgetkontoen allerede er registreret til brug på opgørelseslinjer.
        /// </summary>
        [Test]
        public void TestAtRegisterReturnererHvisErRegistreretOnBudgetkontoViewModelErTrue()
        {
            var fixture = new Fixture();
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var budgetkontogruppeModelMock = MockRepository.GenerateMock<IBudgetkontogruppeModel>();
            budgetkontogruppeModelMock.Expect(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();

            var budgetkontogruppeViewModelMock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
            budgetkontogruppeViewModelMock.Expect(m => m.Nummer)
                .Return(budgetkontogruppeModelMock.Nummer)
                .Repeat.Any();

            var budgetkontoViewModel = MockRepository.GenerateMock<IBudgetkontoViewModel>();
            budgetkontoViewModel.Expect(m => m.Kontogruppe)
                .Return(budgetkontogruppeViewModelMock)
                .Repeat.Any();
            budgetkontoViewModel.Expect(m => m.ErRegistreret)
                .Return(true)
                .Repeat.Any();

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Budgetkonti)
                .Return(new List<IBudgetkontoViewModel> {budgetkontoViewModel})
                .Repeat.Any();

            var exceptionHandlerViewModel = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var opgørelseViewModel = new OpgørelseViewModel(regnskabViewModelMock, budgetkontogruppeModelMock, exceptionHandlerViewModel);
            Assert.That(opgørelseViewModel, Is.Not.Null);

            opgørelseViewModel.Register(budgetkontoViewModel);

            budgetkontoViewModel.AssertWasNotCalled(m => m.ErRegistreret = Arg<bool>.Is.Anything);
            exceptionHandlerViewModel.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Register registrerer budgetkontoen til brug i den aktuelle opgørelseslinje.
        /// </summary>
        [Test]
        public void TestAtRegisterRegistrererBudgetkontoViewModelTilBrugForOpgørelseViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var budgetkontogruppeModelMock = MockRepository.GenerateMock<IBudgetkontogruppeModel>();
            budgetkontogruppeModelMock.Expect(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();

            var budgetkontogruppeViewModelMock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
            budgetkontogruppeViewModelMock.Expect(m => m.Nummer)
                .Return(budgetkontogruppeModelMock.Nummer)
                .Repeat.Any();

            var budgetkontoViewModel = MockRepository.GenerateMock<IBudgetkontoViewModel>();
            budgetkontoViewModel.Expect(m => m.Kontogruppe)
                .Return(budgetkontogruppeViewModelMock)
                .Repeat.Any();
            budgetkontoViewModel.Expect(m => m.ErRegistreret)
                .Return(false)
                .Repeat.Any();

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Budgetkonti)
                .Return(new List<IBudgetkontoViewModel> {budgetkontoViewModel})
                .Repeat.Any();

            var exceptionHandlerViewModel = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var opgørelseViewModel = new OpgørelseViewModel(regnskabViewModelMock, budgetkontogruppeModelMock, exceptionHandlerViewModel);
            Assert.That(opgørelseViewModel, Is.Not.Null);

            opgørelseViewModel.Register(budgetkontoViewModel);

            budgetkontoViewModel.AssertWasCalled(m => m.ErRegistreret = Arg<bool>.Is.Equal(true));
            exceptionHandlerViewModel.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Register rejser PropertyChanged ved registrering af en budgetkonto til brug i den aktuelle opgørelseslinje.
        /// </summary>
        [Test]
        [TestCase("Budgetkonti")]
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
        [TestCase("Disponibel")]
        [TestCase("DisponibelAsText")]
        public void TestAtRegisterRejserPropertyChangedVedRegistreringAfBudgetkontoViewModelTilBrugForOpgørelseViewModel(string expectPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var budgetkontogruppeModelMock = MockRepository.GenerateMock<IBudgetkontogruppeModel>();
            budgetkontogruppeModelMock.Expect(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();

            var budgetkontogruppeViewModelMock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
            budgetkontogruppeViewModelMock.Expect(m => m.Nummer)
                .Return(budgetkontogruppeModelMock.Nummer)
                .Repeat.Any();

            var budgetkontoViewModelMock = MockRepository.GenerateMock<IBudgetkontoViewModel>();
            budgetkontoViewModelMock.Expect(m => m.Kontogruppe)
                .Return(budgetkontogruppeViewModelMock)
                .Repeat.Any();
            budgetkontoViewModelMock.Expect(m => m.ErRegistreret)
                .Return(false)
                .Repeat.Any();

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Budgetkonti)
                .Return(new List<IBudgetkontoViewModel> {budgetkontoViewModelMock})
                .Repeat.Any();

            var exceptionHandlerViewModel = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var opgørelseViewModel = new OpgørelseViewModel(regnskabViewModelMock, budgetkontogruppeModelMock, exceptionHandlerViewModel);
            Assert.That(opgørelseViewModel, Is.Not.Null);

            var eventCalled = false;
            opgørelseViewModel.PropertyChanged += (s, e) =>
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
            opgørelseViewModel.Register(budgetkontoViewModelMock);
            Assert.That(eventCalled, Is.True);

            exceptionHandlerViewModel.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at PropertyChangedOnRegnskabViewModelEventHandler rejser PropertyChanged, når ViewModel for regnskabet opdateres.
        /// </summary>
        [Test]
        [TestCase("Budgetkonti", "Budgetkonti")]
        [TestCase("Budgetkonti", "Budget")]
        [TestCase("Budgetkonti", "BudgetAsText")]
        [TestCase("Budgetkonti", "BudgetSidsteMåned")]
        [TestCase("Budgetkonti", "BudgetSidsteMånedAsText")]
        [TestCase("Budgetkonti", "BudgetÅrTilDato")]
        [TestCase("Budgetkonti", "BudgetÅrTilDatoAsText")]
        [TestCase("Budgetkonti", "BudgetSidsteÅr")]
        [TestCase("Budgetkonti", "BudgetSidsteÅrAsText")]
        [TestCase("Budgetkonti", "Bogført")]
        [TestCase("Budgetkonti", "BogførtAsText")]
        [TestCase("Budgetkonti", "BogførtSidsteMåned")]
        [TestCase("Budgetkonti", "BogførtSidsteMånedAsText")]
        [TestCase("Budgetkonti", "BogførtÅrTilDato")]
        [TestCase("Budgetkonti", "BogførtÅrTilDatoAsText")]
        [TestCase("Budgetkonti", "BogførtSidsteÅr")]
        [TestCase("Budgetkonti", "BogførtSidsteÅrAsText")]
        [TestCase("Budgetkonti", "Disponibel")]
        [TestCase("Budgetkonti", "DisponibelAsText")]
        public void TestAtPropertyChangedOnRegnskabViewModelEventHandlerRejserPropertyChangedVedOpdateringAfRegnskabViewModel(string propertyNameToRaise, string expectPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<IBudgetkontogruppeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontogruppeModel>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();

            var opgørelseViewModel = new OpgørelseViewModel(regnskabViewModelMock, fixture.Create<IBudgetkontogruppeModel>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(opgørelseViewModel, Is.Not.Null);

            var eventCalled = false;
            opgørelseViewModel.PropertyChanged += (s, e) =>
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
            regnskabViewModelMock.Raise(m => m.PropertyChanged += null, regnskabViewModelMock, new PropertyChangedEventArgs(propertyNameToRaise));
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnRegnskabViewModelEventHandler kaster en ArgumentNullException, hvis objektet, der rejser eventet, er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnRegnskabViewModelEventHandlerKasterArgumentNullExceptionHvisSenderErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IBudgetkontogruppeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontogruppeModel>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();

            var opgørelseViewModel = new OpgørelseViewModel(regnskabViewModelMock, fixture.Create<IBudgetkontogruppeModel>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(opgørelseViewModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => regnskabViewModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("sender"));
        }

        /// <summary>
        /// Tester, at PropertyChangedOnRegnskabViewModelEventHandler kaster en ArgumentNullException, hvis argumenter til eventet er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnRegnskabViewModelEventHandlerKasterArgumentNullExceptionHvisEventArgsErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IBudgetkontogruppeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontogruppeModel>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();

            var opgørelseViewModel = new OpgørelseViewModel(regnskabViewModelMock, fixture.Create<IBudgetkontogruppeModel>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(opgørelseViewModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => regnskabViewModelMock.Raise(m => m.PropertyChanged += null, fixture.Create<object>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("eventArgs"));
        }

        /// <summary>
        /// Tester, at PropertyChangedOnTabelModelEventHandler rejser PropertyChanged, modellen for gruppen af budgetkonti, som opgørelseslinjen baserer sig på, opdateres.
        /// </summary>
        [Test]
        [TestCase("Id", "Id")]
        [TestCase("Nummer", "Nummer")]
        [TestCase("Nummer", "Budgetkonti")]
        [TestCase("Nummer", "Budget")]
        [TestCase("Nummer", "BudgetAsText")]
        [TestCase("Nummer", "BudgetSidsteMåned")]
        [TestCase("Nummer", "BudgetSidsteMånedAsText")]
        [TestCase("Nummer", "BudgetÅrTilDato")]
        [TestCase("Nummer", "BudgetÅrTilDatoAsText")]
        [TestCase("Nummer", "BudgetSidsteÅr")]
        [TestCase("Nummer", "BudgetSidsteÅrAsText")]
        [TestCase("Nummer", "Bogført")]
        [TestCase("Nummer", "BogførtAsText")]
        [TestCase("Nummer", "BogførtSidsteMåned")]
        [TestCase("Nummer", "BogførtSidsteMånedAsText")]
        [TestCase("Nummer", "BogførtÅrTilDato")]
        [TestCase("Nummer", "BogførtÅrTilDatoAsText")]
        [TestCase("Nummer", "BogførtSidsteÅr")]
        [TestCase("Nummer", "BogførtSidsteÅrAsText")]
        [TestCase("Nummer", "Disponibel")]
        [TestCase("Nummer", "DisponibelAsText")]
        [TestCase("Tekst", "Tekst")]
        [TestCase("Tekst", "DisplayName")]
        public void TestAtPropertyChangedOnTabelModelEventHandlerRejserPropertyChangedOnBudgetkontogruppeModelUpdate(string propertyNameToRaise, string expectPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() =>
            {
                var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
                regnskabViewModelMock.Expect(m => m.Budgetkonti)
                    .Return(new List<IBudgetkontoViewModel>())
                    .Repeat.Any();
                return regnskabViewModelMock;
            }));
            fixture.Customize<IBudgetkontogruppeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontogruppeModel>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var budgetkontogruppeModelMock = fixture.Create<IBudgetkontogruppeModel>();
            var exceptionHandleViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var opgørelseViewModel = new OpgørelseViewModel(fixture.Create<IRegnskabViewModel>(), budgetkontogruppeModelMock, exceptionHandleViewModelMock);
            Assert.That(opgørelseViewModel, Is.Not.Null);

            var eventCalled = false;
            opgørelseViewModel.PropertyChanged += (s, e) =>
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
            budgetkontogruppeModelMock.Raise(m => m.PropertyChanged += null, budgetkontogruppeModelMock, new PropertyChangedEventArgs(propertyNameToRaise));
            Assert.That(eventCalled, Is.True);

            exceptionHandleViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at PropertyChangedOnTabelModelEventHandler behandler registreringer af budgetkonti i forhold til brug i den aktuelle opgørelseslinje.
        /// </summary>
        [Test]
        [TestCase("Nummer")]
        public void TestAtPropertyChangedOnTabelModelEventHandlerBehandlerBudgetkontoViewModelCollectionForOpgørelseViewModel(string propertyNameToRaise)
        {
            var fixture = new Fixture();
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var budgetkontogruppeModelMock = MockRepository.GenerateMock<IBudgetkontogruppeModel>();
            budgetkontogruppeModelMock.Expect(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();

            var budgetkontogruppeViewModelMock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
            budgetkontogruppeViewModelMock.Expect(m => m.Nummer)
                .Return(budgetkontogruppeModelMock.Nummer)
                .Repeat.Any();

            var budgetkontoViewModelMockCollection1 = new List<IBudgetkontoViewModel>(15);
            while (budgetkontoViewModelMockCollection1.Count < budgetkontoViewModelMockCollection1.Capacity)
            {
                var budgetkontoViewModelMock = MockRepository.GenerateMock<IBudgetkontoViewModel>();
                budgetkontoViewModelMock.Expect(m => m.Kontogruppe)
                    .Return(budgetkontogruppeViewModelMock)
                    .Repeat.Any();
                budgetkontoViewModelMock.Expect(m => m.ErRegistreret)
                    .Return(true)
                    .Repeat.Any();
                budgetkontoViewModelMockCollection1.Add(budgetkontoViewModelMock);
            }

            var budgetkontoViewModelMockCollection2 = new List<IBudgetkontoViewModel>(10);
            while (budgetkontoViewModelMockCollection2.Count < budgetkontoViewModelMockCollection2.Capacity)
            {
                var budgetkontoViewModelMock = MockRepository.GenerateMock<IBudgetkontoViewModel>();
                budgetkontoViewModelMock.Expect(m => m.Kontogruppe)
                    .Return(budgetkontogruppeViewModelMock)
                    .Repeat.Any();
                budgetkontoViewModelMock.Expect(m => m.ErRegistreret)
                    .Return(false)
                    .Repeat.Any();
                budgetkontoViewModelMockCollection2.Add(budgetkontoViewModelMock);
            }

            var budgetkontoViewModelMockCollection = new List<IBudgetkontoViewModel>(budgetkontoViewModelMockCollection1);
            budgetkontoViewModelMockCollection.AddRange(budgetkontoViewModelMockCollection2);
            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Budgetkonti)
                .Return(budgetkontoViewModelMockCollection)
                .Repeat.Any();

            var exceptionHandleViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var opgørelseViewModel = new OpgørelseViewModel(regnskabViewModelMock, budgetkontogruppeModelMock, exceptionHandleViewModelMock);
            Assert.That(opgørelseViewModel, Is.Not.Null);

            budgetkontogruppeModelMock.Raise(m => m.PropertyChanged += null, budgetkontogruppeModelMock, new PropertyChangedEventArgs(propertyNameToRaise));

            foreach (var budgetkontoViewModelMock in budgetkontoViewModelMockCollection1)
            {
                budgetkontoViewModelMock.AssertWasCalled(m => m.ErRegistreret = Arg<bool>.Is.Equal(false));
            }
            foreach (var budgetkontoViewModelMock in budgetkontoViewModelMockCollection2)
            {
                budgetkontoViewModelMock.AssertWasCalled(m => m.ErRegistreret = Arg<bool>.Is.Equal(true));
            }

            exceptionHandleViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBudgetkontoViewModelEventHandler rejser PropertyChanged, når ViewModel for en registreret budgetkonto opdateres.
        /// </summary>
        [Test]
        [TestCase("Kontonummer", "Budgetkonti")]
        [TestCase("Kontogruppe", "Budgetkonti")]
        [TestCase("Kontogruppe", "Budget")]
        [TestCase("Kontogruppe", "BudgetAsText")]
        [TestCase("Kontogruppe", "BudgetSidsteMåned")]
        [TestCase("Kontogruppe", "BudgetSidsteMånedAsText")]
        [TestCase("Kontogruppe", "BudgetÅrTilDato")]
        [TestCase("Kontogruppe", "BudgetÅrTilDatoAsText")]
        [TestCase("Kontogruppe", "BudgetSidsteÅr")]
        [TestCase("Kontogruppe", "BudgetSidsteÅrAsText")]
        [TestCase("Kontogruppe", "Bogført")]
        [TestCase("Kontogruppe", "BogførtAsText")]
        [TestCase("Kontogruppe", "BogførtSidsteMåned")]
        [TestCase("Kontogruppe", "BogførtSidsteMånedAsText")]
        [TestCase("Kontogruppe", "BogførtÅrTilDato")]
        [TestCase("Kontogruppe", "BogførtÅrTilDatoAsText")]
        [TestCase("Kontogruppe", "BogførtSidsteÅr")]
        [TestCase("Kontogruppe", "BogførtSidsteÅrAsText")]
        [TestCase("Kontogruppe", "Disponibel")]
        [TestCase("Kontogruppe", "DisponibelAsText")]
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
        [TestCase("ErRegistreret", "Budgetkonti")]
        [TestCase("ErRegistreret", "Budget")]
        [TestCase("ErRegistreret", "BudgetAsText")]
        [TestCase("ErRegistreret", "BudgetSidsteMåned")]
        [TestCase("ErRegistreret", "BudgetSidsteMånedAsText")]
        [TestCase("ErRegistreret", "BudgetÅrTilDato")]
        [TestCase("ErRegistreret", "BudgetÅrTilDatoAsText")]
        [TestCase("ErRegistreret", "BudgetSidsteÅr")]
        [TestCase("ErRegistreret", "BudgetSidsteÅrAsText")]
        [TestCase("ErRegistreret", "Bogført")]
        [TestCase("ErRegistreret", "BogførtAsText")]
        [TestCase("ErRegistreret", "BogførtSidsteMåned")]
        [TestCase("ErRegistreret", "BogførtSidsteMånedAsText")]
        [TestCase("ErRegistreret", "BogførtÅrTilDato")]
        [TestCase("ErRegistreret", "BogførtÅrTilDatoAsText")]
        [TestCase("ErRegistreret", "BogførtSidsteÅr")]
        [TestCase("ErRegistreret", "BogførtSidsteÅrAsText")]
        [TestCase("ErRegistreret", "Disponibel")]
        [TestCase("ErRegistreret", "DisponibelAsText")]
        public void TestAtPropertyChangedOnBudgetkontoViewModelEventHandlerRejserPropertyChangedVedOpdateringAfBudgetkontoViewModel(string propertyNameToRaise, string expectPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var budgetkontogruppeModelMock = MockRepository.GenerateMock<IBudgetkontogruppeModel>();
            budgetkontogruppeModelMock.Expect(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();

            var budgetkontogruppeViewModelMock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
            budgetkontogruppeViewModelMock.Expect(m => m.Nummer)
                .Return(budgetkontogruppeModelMock.Nummer)
                .Repeat.Any();

            var budgetkontoViewModelMock = MockRepository.GenerateMock<IBudgetkontoViewModel>();
            budgetkontoViewModelMock.Expect(m => m.Kontogruppe)
                .Return(budgetkontogruppeViewModelMock)
                .Repeat.Any();
            budgetkontoViewModelMock.Expect(m => m.ErRegistreret)
                .Return(false)
                .Repeat.Any();

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Budgetkonti)
                .Return(new List<IBudgetkontoViewModel> { budgetkontoViewModelMock })
                .Repeat.Any();

            var opgørelseViewModel = new OpgørelseViewModel(regnskabViewModelMock, budgetkontogruppeModelMock, fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(opgørelseViewModel, Is.Not.Null);

            opgørelseViewModel.Register(budgetkontoViewModelMock);

            var eventCalled = false;
            opgørelseViewModel.PropertyChanged += (s, e) =>
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
            budgetkontoViewModelMock.Raise(m => m.PropertyChanged += null, budgetkontoViewModelMock, new PropertyChangedEventArgs(propertyNameToRaise));
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBudgetkontoViewModelEventHandler afregistrerer budgetkontoen fra brug i den aktuelle opgørelseslinje.
        /// </summary>
        [Test]
        [TestCase("Kontogruppe")]
        [TestCase("ErRegistreret")]
        public void TestAtPropertyChangedOnBudgetkontoViewModelEventHandlerAfregistrererBudgetkontoViewModelFraBrugForOpgørelseViewModel(string propertyNameToRaise)
        {
            var fixture = new Fixture();
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var budgetkontogruppeModelMock = MockRepository.GenerateMock<IBudgetkontogruppeModel>();
            budgetkontogruppeModelMock.Expect(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();

            var budgetkontogruppeViewModelMock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
            budgetkontogruppeViewModelMock.Expect(m => m.Nummer)
                .Return(budgetkontogruppeModelMock.Nummer)
                .Repeat.Any();

            var erRegistreret = false;
            var budgetkontoViewModelMock = MockRepository.GenerateMock<IBudgetkontoViewModel>();
            // ReSharper disable AccessToModifiedClosure
            budgetkontoViewModelMock.Expect(m => m.Kontogruppe)
                .WhenCalled(e => e.ReturnValue = budgetkontogruppeViewModelMock)
                .Return(null)
                .Repeat.Any();
            // ReSharper restore AccessToModifiedClosure
            budgetkontoViewModelMock.Expect(m => m.ErRegistreret)
                .WhenCalled(e => e.ReturnValue = erRegistreret)
                .Return(false)
                .Repeat.Any();
            budgetkontoViewModelMock.Expect(m => m.ErRegistreret = Arg<bool>.Is.Anything)
                .WhenCalled(e => erRegistreret = (bool) e.Arguments.ElementAt(0))
                .Repeat.Any();

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Budgetkonti)
                .Return(new List<IBudgetkontoViewModel> { budgetkontoViewModelMock })
                .Repeat.Any();

            var opgørelseViewModel = new OpgørelseViewModel(regnskabViewModelMock, budgetkontogruppeModelMock, fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(opgørelseViewModel, Is.Not.Null);

            opgørelseViewModel.Register(budgetkontoViewModelMock);

            switch (propertyNameToRaise)
            {
                case "Kontogruppe":
                    var nummer = fixture.Create<Generator<int>>().First(m => m != budgetkontogruppeViewModelMock.Nummer);
                    budgetkontogruppeViewModelMock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
                    budgetkontogruppeViewModelMock.Expect(m => m.Nummer)
                        .Return(nummer)
                        .Repeat.Any();
                    break;
            }
            budgetkontoViewModelMock.Raise(m => m.PropertyChanged += null, budgetkontoViewModelMock, new PropertyChangedEventArgs(propertyNameToRaise));

            budgetkontoViewModelMock.AssertWasCalled(m => m.ErRegistreret = Arg<bool>.Is.Equal(false), opt => opt.Repeat.Times(1));
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBudgetkontoViewModelEventHandler kaster en ArgumentNullException, hvis objektet, der rejser eventet, er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnBudgetkontoViewModelEventHandlerKasterArgumentNullExceptionHvisSenderErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var budgetkontogruppeModelMock = MockRepository.GenerateMock<IBudgetkontogruppeModel>();
            budgetkontogruppeModelMock.Expect(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();

            var budgetkontogruppeViewModelMock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
            budgetkontogruppeViewModelMock.Expect(m => m.Nummer)
                .Return(budgetkontogruppeModelMock.Nummer)
                .Repeat.Any();

            var budgetkontoViewModelMock = MockRepository.GenerateMock<IBudgetkontoViewModel>();
            budgetkontoViewModelMock.Expect(m => m.Kontogruppe)
                .Return(budgetkontogruppeViewModelMock)
                .Repeat.Any();
            budgetkontoViewModelMock.Expect(m => m.ErRegistreret)
                .Return(false)
                .Repeat.Any();

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Budgetkonti)
                .Return(new List<IBudgetkontoViewModel> { budgetkontoViewModelMock })
                .Repeat.Any();

            var opgørelseViewModel = new OpgørelseViewModel(regnskabViewModelMock, budgetkontogruppeModelMock, fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(opgørelseViewModel, Is.Not.Null);

            opgørelseViewModel.Register(budgetkontoViewModelMock);

            var exception = Assert.Throws<ArgumentNullException>(() => budgetkontoViewModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("sender"));
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBudgetkontoViewModelEventHandler kaster en ArgumentNullException, hvis argumenter til eventet er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnBudgetkontoViewModelEventHandlerKasterArgumentNullExceptionHvisEventArgsErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var budgetkontogruppeModelMock = MockRepository.GenerateMock<IBudgetkontogruppeModel>();
            budgetkontogruppeModelMock.Expect(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();

            var budgetkontogruppeViewModelMock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
            budgetkontogruppeViewModelMock.Expect(m => m.Nummer)
                .Return(budgetkontogruppeModelMock.Nummer)
                .Repeat.Any();

            var budgetkontoViewModelMock = MockRepository.GenerateMock<IBudgetkontoViewModel>();
            budgetkontoViewModelMock.Expect(m => m.Kontogruppe)
                .Return(budgetkontogruppeViewModelMock)
                .Repeat.Any();
            budgetkontoViewModelMock.Expect(m => m.ErRegistreret)
                .Return(false)
                .Repeat.Any();

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Budgetkonti)
                .Return(new List<IBudgetkontoViewModel> {budgetkontoViewModelMock})
                .Repeat.Any();

            var opgørelseViewModel = new OpgørelseViewModel(regnskabViewModelMock, budgetkontogruppeModelMock, fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(opgørelseViewModel, Is.Not.Null);

            opgørelseViewModel.Register(budgetkontoViewModelMock);

            var exception = Assert.Throws<ArgumentNullException>(() => budgetkontoViewModelMock.Raise(m => m.PropertyChanged += null, fixture.Create<object>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("eventArgs"));
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBudgetkontoViewModelEventHandler kaster en IntranetGuiSystemException, hvis objektet, der rejser eventet, ikke er en ViewModel for en budgetkonto.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnBudgetkontoViewModelEventHandlerKasterIntranetGuiSystemExceptionHvisSenderIkkeErBudgetkontoViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var budgetkontogruppeModelMock = MockRepository.GenerateMock<IBudgetkontogruppeModel>();
            budgetkontogruppeModelMock.Expect(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();

            var budgetkontogruppeViewModelMock = MockRepository.GenerateMock<IBudgetkontogruppeViewModel>();
            budgetkontogruppeViewModelMock.Expect(m => m.Nummer)
                .Return(budgetkontogruppeModelMock.Nummer)
                .Repeat.Any();

            var budgetkontoViewModelMock = MockRepository.GenerateMock<IBudgetkontoViewModel>();
            budgetkontoViewModelMock.Expect(m => m.Kontogruppe)
                .Return(budgetkontogruppeViewModelMock)
                .Repeat.Any();
            budgetkontoViewModelMock.Expect(m => m.ErRegistreret)
                .Return(false)
                .Repeat.Any();

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Budgetkonti)
                .Return(new List<IBudgetkontoViewModel> { budgetkontoViewModelMock })
                .Repeat.Any();

            var opgørelseViewModel = new OpgørelseViewModel(regnskabViewModelMock, budgetkontogruppeModelMock, fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(opgørelseViewModel, Is.Not.Null);

            opgørelseViewModel.Register(budgetkontoViewModelMock);

            var sender = fixture.Create<object>();
            var exception = Assert.Throws<IntranetGuiSystemException>(() => budgetkontoViewModelMock.Raise(m => m.PropertyChanged += null, sender , fixture.Create<PropertyChangedEventArgs>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "sender", sender.GetType().Name)));
        }
    }
}
