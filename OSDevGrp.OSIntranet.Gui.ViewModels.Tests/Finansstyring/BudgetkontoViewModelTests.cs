using System;
using System.ComponentModel;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
using Ploeh.AutoFixture;
using Rhino.Mocks;
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
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontogruppeViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var regnskabViewModelMock = fixture.Create<IRegnskabViewModel>();
            var budgetkontoModelMock = MockRepository.GenerateMock<IBudgetkontoModel>();
            budgetkontoModelMock.Expect(m => m.Kontonummer)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            budgetkontoModelMock.Expect(m => m.Kontonavn)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            budgetkontoModelMock.Expect(m => m.Beskrivelse)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            budgetkontoModelMock.Expect(m => m.Notat)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            budgetkontoModelMock.Expect(m => m.StatusDato)
                .Return(fixture.Create<DateTime>())
                .Repeat.Any();
            budgetkontoModelMock.Expect(m => m.Indtægter)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            budgetkontoModelMock.Expect(m => m.Udgifter)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            budgetkontoModelMock.Expect(m => m.Budget)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            budgetkontoModelMock.Expect(m => m.BudgetSidsteMåned)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            budgetkontoModelMock.Expect(m => m.Bogført)
                .Return(Math.Abs(fixture.Create<decimal>())*-1)
                .Repeat.Any();
            budgetkontoModelMock.Expect(m => m.Disponibel)
                .Return(fixture.Create<decimal>())
                .Repeat.Any();
            var budgetkontogruppeViewModelMock = fixture.Create<IBudgetkontogruppeViewModel>();
            var budgetkontoViewModel = new BudgetkontoViewModel(regnskabViewModelMock, budgetkontoModelMock, budgetkontogruppeViewModelMock, fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(budgetkontoViewModel, Is.Not.Null);
            Assert.That(budgetkontoViewModel.Regnskab, Is.Not.Null);
            Assert.That(budgetkontoViewModel.Regnskab, Is.EqualTo(regnskabViewModelMock));
            Assert.That(budgetkontoViewModel.Kontonummer, Is.Not.Null);
            Assert.That(budgetkontoViewModel.Kontonummer, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.Kontonummer, Is.EqualTo(budgetkontoModelMock.Kontonummer));
            Assert.That(budgetkontoViewModel.Kontonavn, Is.Not.Null);
            Assert.That(budgetkontoViewModel.Kontonavn, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.Kontonavn, Is.EqualTo(budgetkontoModelMock.Kontonavn));
            Assert.That(budgetkontoViewModel.Beskrivelse, Is.Not.Null);
            Assert.That(budgetkontoViewModel.Beskrivelse, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.Beskrivelse, Is.EqualTo(budgetkontoModelMock.Beskrivelse));
            Assert.That(budgetkontoViewModel.Notat, Is.Not.Null);
            Assert.That(budgetkontoViewModel.Notat, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.Notat, Is.EqualTo(budgetkontoModelMock.Notat));
            Assert.That(budgetkontoViewModel.Kontogruppe, Is.Not.Null);
            Assert.That(budgetkontoViewModel.Kontogruppe, Is.EqualTo(budgetkontogruppeViewModelMock));
            Assert.That(budgetkontoViewModel.StatusDato, Is.EqualTo(budgetkontoModelMock.StatusDato));
            Assert.That(budgetkontoViewModel.Indtægter, Is.EqualTo(budgetkontoModelMock.Indtægter));
            Assert.That(budgetkontoViewModel.IndtægterAsText, Is.Not.Null);
            Assert.That(budgetkontoViewModel.IndtægterAsText, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.IndtægterAsText, Is.EqualTo(budgetkontoModelMock.Indtægter.ToString("C")));
            Assert.That(budgetkontoViewModel.IndtægterLabel, Is.Not.Null);
            Assert.That(budgetkontoViewModel.IndtægterLabel, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.IndtægterLabel, Is.EqualTo(Resource.GetText(Text.Income)));
            Assert.That(budgetkontoViewModel.Udgifter, Is.EqualTo(budgetkontoModelMock.Udgifter));
            Assert.That(budgetkontoViewModel.UdgifterAsText, Is.Not.Null);
            Assert.That(budgetkontoViewModel.UdgifterAsText, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.UdgifterAsText, Is.EqualTo(budgetkontoModelMock.Udgifter.ToString("C")));
            Assert.That(budgetkontoViewModel.UdgifterLabel, Is.Not.Null);
            Assert.That(budgetkontoViewModel.UdgifterLabel, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.UdgifterLabel, Is.EqualTo(Resource.GetText(Text.Expenses)));
            Assert.That(budgetkontoViewModel.Budget, Is.EqualTo(budgetkontoModelMock.Budget));
            Assert.That(budgetkontoViewModel.BudgetAsText, Is.Not.Null);
            Assert.That(budgetkontoViewModel.BudgetAsText, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.BudgetAsText, Is.EqualTo(budgetkontoModelMock.Budget.ToString("C")));
            Assert.That(budgetkontoViewModel.BudgetLabel, Is.Not.Null);
            Assert.That(budgetkontoViewModel.BudgetLabel, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.BudgetLabel, Is.EqualTo(Resource.GetText(Text.Budget)));
            Assert.That(budgetkontoViewModel.BudgetSidsteMåned, Is.EqualTo(budgetkontoModelMock.BudgetSidsteMåned));
            Assert.That(budgetkontoViewModel.BudgetSidsteMånedAsText, Is.Not.Null);
            Assert.That(budgetkontoViewModel.BudgetSidsteMånedAsText, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.BudgetSidsteMånedAsText, Is.EqualTo(budgetkontoModelMock.BudgetSidsteMåned.ToString("C")));
            Assert.That(budgetkontoViewModel.BudgetSidsteMånedLabel, Is.Not.Null);
            Assert.That(budgetkontoViewModel.BudgetSidsteMånedLabel, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.BudgetSidsteMånedLabel, Is.EqualTo(Resource.GetText(Text.BudgetLastMonth)));
            Assert.That(budgetkontoViewModel.Bogført, Is.EqualTo(budgetkontoModelMock.Bogført));
            Assert.That(budgetkontoViewModel.BogførtAsText, Is.Not.Null);
            Assert.That(budgetkontoViewModel.BogførtAsText, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.BogførtAsText, Is.EqualTo(budgetkontoModelMock.Bogført.ToString("C")));
            Assert.That(budgetkontoViewModel.BogførtLabel, Is.Not.Null);
            Assert.That(budgetkontoViewModel.BogførtLabel, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.BogførtLabel, Is.EqualTo(Resource.GetText(Text.Bookkeeped)));
            Assert.That(budgetkontoViewModel.Disponibel, Is.EqualTo(budgetkontoModelMock.Disponibel));
            Assert.That(budgetkontoViewModel.DisponibelAsText, Is.Not.Null);
            Assert.That(budgetkontoViewModel.DisponibelAsText, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.DisponibelAsText, Is.EqualTo(budgetkontoModelMock.Disponibel.ToString("C")));
            Assert.That(budgetkontoViewModel.DisponibelLabel, Is.Not.Null);
            Assert.That(budgetkontoViewModel.DisponibelLabel, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.DisponibelLabel, Is.EqualTo(Resource.GetText(Text.Available)));
            Assert.That(budgetkontoViewModel.Kontoværdi, Is.EqualTo(Math.Abs(budgetkontoModelMock.Bogført)));
            Assert.That(budgetkontoViewModel.DisplayName, Is.Not.Null);
            Assert.That(budgetkontoViewModel.DisplayName, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.DisplayName, Is.EqualTo(Resource.GetText(Text.BudgetAccount)));
            Assert.That(budgetkontoViewModel.Image, Is.Not.Null);
            Assert.That(budgetkontoViewModel.Image, Is.Not.Empty);
            Assert.That(budgetkontoViewModel.Image, Is.EqualTo(Resource.GetEmbeddedResource("Images.Budgetkonto.png")));
            Assert.That(budgetkontoViewModel.RefreshCommand, Is.Not.Null);
            Assert.That(budgetkontoViewModel.RefreshCommand, Is.TypeOf<BudgetkontoGetCommand>());

            budgetkontoModelMock.AssertWasNotCalled(m => m.Regnskabsnummer);
            budgetkontoModelMock.AssertWasCalled(m => m.Kontonummer);
            budgetkontoModelMock.AssertWasCalled(m => m.Kontonavn);
            budgetkontoModelMock.AssertWasCalled(m => m.Beskrivelse);
            budgetkontoModelMock.AssertWasCalled(m => m.Notat);
            budgetkontoModelMock.AssertWasNotCalled(m => m.Kontogruppe);
            budgetkontoModelMock.AssertWasCalled(m => m.StatusDato);
            budgetkontoModelMock.AssertWasCalled(m => m.Indtægter);
            budgetkontoModelMock.AssertWasCalled(m => m.Udgifter);
            budgetkontoModelMock.AssertWasCalled(m => m.Budget);
            budgetkontoModelMock.AssertWasCalled(m => m.BudgetSidsteMåned);
            budgetkontoModelMock.AssertWasCalled(m => m.Bogført);
            budgetkontoModelMock.AssertWasCalled(m => m.Disponibel);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis ViewModel for regnskabet er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisRegnskabViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IBudgetkontoModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontoModel>()));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontogruppeViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new BudgetkontoViewModel(null, fixture.Create<IBudgetkontoModel>(), fixture.Create<IBudgetkontogruppeViewModel>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>()));
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
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontogruppeViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new BudgetkontoViewModel(fixture.Create<IRegnskabViewModel>(), null, fixture.Create<IBudgetkontogruppeViewModel>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>()));
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
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IBudgetkontoModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontoModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new BudgetkontoViewModel(fixture.Create<IRegnskabViewModel>(), fixture.Create<IBudgetkontoModel>(), null, fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>()));
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
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IBudgetkontoModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontoModel>()));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontogruppeViewModel>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new BudgetkontoViewModel(fixture.Create<IRegnskabViewModel>(), fixture.Create<IBudgetkontoModel>(), fixture.Create<IBudgetkontogruppeViewModel>(), null, fixture.Create<IExceptionHandlerViewModel>()));
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
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IBudgetkontoModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontoModel>()));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontogruppeViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new BudgetkontoViewModel(fixture.Create<IRegnskabViewModel>(), fixture.Create<IBudgetkontoModel>(), fixture.Create<IBudgetkontogruppeViewModel>(), fixture.Create<IFinansstyringRepository>(), null));
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
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IBudgetkontoModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontoModel>()));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontogruppeViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var budgetkontoModelMock = fixture.Create<IBudgetkontoModel>();
            var exceptionHandlerViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var budgetkontoViewModel = new BudgetkontoViewModel(fixture.Create<IRegnskabViewModel>(), budgetkontoModelMock, fixture.Create<IBudgetkontogruppeViewModel>(), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(budgetkontoViewModel, Is.Not.Null);

            var newValue = fixture.Create<decimal>();
            budgetkontoViewModel.Indtægter = newValue;

            budgetkontoModelMock.AssertWasCalled(m => m.Indtægter = Arg<decimal>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at sætteren til Indtægter kalder HandleException på exceptionhandleren ved exceptions.
        /// </summary>
        [Test]
        public void TestAtIndtægterSetterKalderHandleExceptionOnExceptionHandlerViewModelVedExceptions()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontogruppeViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = fixture.Create<Exception>();
            var budgetkontoModelMock = MockRepository.GenerateMock<IBudgetkontoModel>();
            budgetkontoModelMock.Expect(m => m.Indtægter = Arg<decimal>.Is.GreaterThan(0M))
                                .Throw(exception)
                                .Repeat.Any();

            var exceptionHandlerViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var budgetkontoViewModel = new BudgetkontoViewModel(fixture.Create<IRegnskabViewModel>(), budgetkontoModelMock, fixture.Create<IBudgetkontogruppeViewModel>(), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(budgetkontoViewModel, Is.Not.Null);

            var newValue = fixture.Create<decimal>();
            budgetkontoViewModel.Indtægter = newValue;

            budgetkontoModelMock.AssertWasCalled(m => m.Indtægter = Arg<decimal>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<Exception>.Is.Equal(exception)));
        }

        /// <summary>
        /// Tester, at sætteren til Udgifter opdaterer Udgifter på modellen til budgetkontoen.
        /// </summary>
        [Test]
        public void TestAtUdgifterSetterOpdatererUdgifterOnBudgetkontoModel()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IBudgetkontoModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontoModel>()));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontogruppeViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var budgetkontoModelMock = fixture.Create<IBudgetkontoModel>();
            var exceptionHandlerViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var budgetkontoViewModel = new BudgetkontoViewModel(fixture.Create<IRegnskabViewModel>(), budgetkontoModelMock, fixture.Create<IBudgetkontogruppeViewModel>(), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(budgetkontoViewModel, Is.Not.Null);

            var newValue = fixture.Create<decimal>();
            budgetkontoViewModel.Udgifter = newValue;

            budgetkontoModelMock.AssertWasCalled(m => m.Udgifter = Arg<decimal>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at sætteren til Udgifter kalder HandleException på exceptionhandleren ved exceptions.
        /// </summary>
        [Test]
        public void TestAtUdgifterSetterKalderHandleExceptionOnExceptionHandlerViewModelVedExceptions()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontogruppeViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = fixture.Create<Exception>();
            var budgetkontoModelMock = MockRepository.GenerateMock<IBudgetkontoModel>();
            budgetkontoModelMock.Expect(m => m.Udgifter = Arg<decimal>.Is.GreaterThan(0M))
                                .Throw(exception)
                                .Repeat.Any();

            var exceptionHandlerViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var budgetkontoViewModel = new BudgetkontoViewModel(fixture.Create<IRegnskabViewModel>(), budgetkontoModelMock, fixture.Create<IBudgetkontogruppeViewModel>(), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(budgetkontoViewModel, Is.Not.Null);

            var newValue = fixture.Create<decimal>();
            budgetkontoViewModel.Udgifter = newValue;

            budgetkontoModelMock.AssertWasCalled(m => m.Udgifter = Arg<decimal>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<Exception>.Is.Equal(exception)));
        }

        /// <summary>
        /// Tester, at sætteren til BudgetSidsteMåned opdaterer BudgetSidsteMåned på modellen til budgetkontoen.
        /// </summary>
        [Test]
        public void TestAtBudgetSidsteMånedSetterOpdatererBudgetSidsteMånedOnBudgetkontoModel()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IBudgetkontoModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontoModel>()));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontogruppeViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var budgetkontoModelMock = fixture.Create<IBudgetkontoModel>();
            var exceptionHandlerViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var budgetkontoViewModel = new BudgetkontoViewModel(fixture.Create<IRegnskabViewModel>(), budgetkontoModelMock, fixture.Create<IBudgetkontogruppeViewModel>(), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(budgetkontoViewModel, Is.Not.Null);

            var newValue = fixture.Create<decimal>();
            budgetkontoViewModel.BudgetSidsteMåned = newValue;

            budgetkontoModelMock.AssertWasCalled(m => m.BudgetSidsteMåned = Arg<decimal>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at sætteren til BudgetSidsteMåned kalder HandleException på exceptionhandleren ved exceptions.
        /// </summary>
        [Test]
        public void TestAtBudgetSidsteMånedSetterKalderHandleExceptionOnExceptionHandlerViewModelVedExceptions()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontogruppeViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = fixture.Create<Exception>();
            var budgetkontoModelMock = MockRepository.GenerateMock<IBudgetkontoModel>();
            budgetkontoModelMock.Expect(m => m.BudgetSidsteMåned = Arg<decimal>.Is.GreaterThan(0M))
                                .Throw(exception)
                                .Repeat.Any();

            var exceptionHandlerViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var budgetkontoViewModel = new BudgetkontoViewModel(fixture.Create<IRegnskabViewModel>(), budgetkontoModelMock, fixture.Create<IBudgetkontogruppeViewModel>(), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(budgetkontoViewModel, Is.Not.Null);

            var newValue = fixture.Create<decimal>();
            budgetkontoViewModel.BudgetSidsteMåned = newValue;

            budgetkontoModelMock.AssertWasCalled(m => m.BudgetSidsteMåned = Arg<decimal>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<Exception>.Is.Equal(exception)));
        }

        /// <summary>
        /// Tester, at sætteren til Bogført opdaterer Bogført på modellen til budgetkontoen.
        /// </summary>
        [Test]
        public void TestAtBogførtSetterOpdatererBogførtOnBudgetkontoModel()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IBudgetkontoModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontoModel>()));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontogruppeViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var budgetkontoModelMock = fixture.Create<IBudgetkontoModel>();
            var exceptionHandlerViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var budgetkontoViewModel = new BudgetkontoViewModel(fixture.Create<IRegnskabViewModel>(), budgetkontoModelMock, fixture.Create<IBudgetkontogruppeViewModel>(), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(budgetkontoViewModel, Is.Not.Null);

            var newValue = fixture.Create<decimal>();
            budgetkontoViewModel.Bogført = newValue;

            budgetkontoModelMock.AssertWasCalled(m => m.Bogført = Arg<decimal>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at sætteren til Bogført kalder HandleException på exceptionhandleren ved exceptions.
        /// </summary>
        [Test]
        public void TestAtBogførtSetterKalderHandleExceptionOnExceptionHandlerViewModelVedExceptions()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontogruppeViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = fixture.Create<Exception>();
            var budgetkontoModelMock = MockRepository.GenerateMock<IBudgetkontoModel>();
            budgetkontoModelMock.Expect(m => m.Bogført = Arg<decimal>.Is.GreaterThan(0M))
                                .Throw(exception)
                                .Repeat.Any();

            var exceptionHandlerViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var budgetkontoViewModel = new BudgetkontoViewModel(fixture.Create<IRegnskabViewModel>(), budgetkontoModelMock, fixture.Create<IBudgetkontogruppeViewModel>(), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(budgetkontoViewModel, Is.Not.Null);

            var newValue = fixture.Create<decimal>();
            budgetkontoViewModel.Bogført = newValue;

            budgetkontoModelMock.AssertWasCalled(m => m.Bogført = Arg<decimal>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<Exception>.Is.Equal(exception)));
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
        [TestCase("Bogført", "Bogført")]
        [TestCase("Bogført", "BogførtAsText")]
        [TestCase("Disponibel", "Disponibel")]
        [TestCase("Disponibel", "DisponibelAsText")]
        [TestCase("Bogført", "Kontoværdi")]
        public void TestAtPropertyChangedOnKontoModelEventHandlerRejserPropertyChangedOnBudgetkontoModelUpdate(string propertyNameToRaise, string expectPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IBudgetkontoModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontoModel>()));
            fixture.Customize<IBudgetkontogruppeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBudgetkontogruppeViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var budgetkontoModelMock = fixture.Create<IBudgetkontoModel>();
            var exceptionHandlerViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var budgetkontoViewModel = new BudgetkontoViewModel(fixture.Create<IRegnskabViewModel>(), budgetkontoModelMock, fixture.Create<IBudgetkontogruppeViewModel>(), fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(budgetkontoViewModel, Is.Not.Null);

            var eventCalled = false;
            budgetkontoViewModel.PropertyChanged += (s, e) =>
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
            budgetkontoModelMock.Raise(m => m.PropertyChanged += null, budgetkontoModelMock, new PropertyChangedEventArgs(propertyNameToRaise));
            Assert.That(eventCalled, Is.True);

            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }
    }
}
