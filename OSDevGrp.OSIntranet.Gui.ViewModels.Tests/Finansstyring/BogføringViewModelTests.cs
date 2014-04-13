using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
using Ploeh.AutoFixture;
using Rhino.Mocks;
using Text = OSDevGrp.OSIntranet.Gui.Resources.Text;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Finansstyring
{
    /// <summary>
    /// Tester ViewModel, hvorfra der kan bogføres.
    /// </summary>
    [TestFixture]
    public class BogføringViewModelTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en ViewModel, hvorfra der kan bogføres.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererBogføringViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var regnskabViewModelMock = fixture.Create<IRegnskabViewModel>();

            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Bilag)
                                    .Return(fixture.Create<string>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Tekst)
                                    .Return(fixture.Create<string>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Debit)
                                    .Return(fixture.Create<decimal>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kredit)
                                    .Return(fixture.Create<decimal>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(regnskabViewModelMock, bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(bogføringViewModel, Is.Not.Null);
            Assert.That(bogføringViewModel.Regnskab, Is.Not.Null);
            Assert.That(bogføringViewModel.Regnskab, Is.EqualTo(regnskabViewModelMock));
            Assert.That(bogføringViewModel.Dato, Is.EqualTo(bogføringslinjeModelMock.Dato));
            Assert.That(bogføringViewModel.DatoAsText, Is.Not.Null);
            Assert.That(bogføringViewModel.DatoAsText, Is.Not.Empty);
            Assert.That(bogføringViewModel.DatoAsText, Is.EqualTo(bogføringslinjeModelMock.Dato.ToShortDateString()));
            Assert.That(bogføringViewModel.DatoAsTextIsReadOnly, Is.EqualTo(false));
            Assert.That(bogføringViewModel.DatoLabel, Is.Not.Null);
            Assert.That(bogføringViewModel.DatoLabel, Is.Not.Empty);
            Assert.That(bogføringViewModel.DatoLabel, Is.EqualTo(Resource.GetText(Text.Date)));
            Assert.That(bogføringViewModel.Bilag, Is.Not.Null);
            Assert.That(bogføringViewModel.Bilag, Is.Not.Empty);
            Assert.That(bogføringViewModel.Bilag, Is.EqualTo(bogføringslinjeModelMock.Bilag));
            Assert.That(bogføringViewModel.BilagMaxLength, Is.EqualTo(FieldInformations.BilagFieldLength));
            Assert.That(bogføringViewModel.BilagIsReadOnly, Is.False);
            Assert.That(bogføringViewModel.BilagLabel, Is.Not.Null);
            Assert.That(bogføringViewModel.BilagLabel, Is.Not.Empty);
            Assert.That(bogføringViewModel.BilagLabel, Is.EqualTo(Resource.GetText(Text.Annex)));
            Assert.That(bogføringViewModel.Kontonummer, Is.Null);
            Assert.That(bogføringViewModel.KontonummerMaxLength, Is.EqualTo(FieldInformations.KontonummerFieldLength));
            Assert.That(bogføringViewModel.KontonummerIsReadOnly, Is.False);
            Assert.That(bogføringViewModel.KontonummerLabel, Is.Not.Null);
            Assert.That(bogføringViewModel.KontonummerLabel, Is.Not.Empty);
            Assert.That(bogføringViewModel.KontonummerLabel, Is.EqualTo(Resource.GetText(Text.Account)));
            Assert.That(bogføringViewModel.Kontonavn, Is.Not.Null);
            Assert.That(bogføringViewModel.Kontonavn, Is.Empty);
            Assert.That(bogføringViewModel.KontonavnLabel, Is.Not.Null);
            Assert.That(bogføringViewModel.KontonavnLabel, Is.Not.Empty);
            Assert.That(bogføringViewModel.KontonavnLabel, Is.EqualTo(Resource.GetText(Text.Account)));
            Assert.That(bogføringViewModel.Kontonavn, Is.Not.Null);
            Assert.That(bogføringViewModel.Kontonavn, Is.Empty);
            Assert.That(bogføringViewModel.KontonavnLabel, Is.Not.Null);
            Assert.That(bogføringViewModel.KontonavnLabel, Is.Not.Empty);
            Assert.That(bogføringViewModel.KontonavnLabel, Is.EqualTo(Resource.GetText(Text.Account)));
            Assert.That(bogføringViewModel.KontoSaldo, Is.EqualTo(0M));
            Assert.That(bogføringViewModel.KontoSaldoAsText, Is.Not.Null);
            Assert.That(bogføringViewModel.KontoSaldoAsText, Is.Empty);
            Assert.That(bogføringViewModel.KontoSaldoLabel, Is.Not.Null);
            Assert.That(bogføringViewModel.KontoSaldoLabel, Is.Not.Empty);
            Assert.That(bogføringViewModel.KontoSaldoLabel, Is.EqualTo(Resource.GetText(Text.Balance)));
            Assert.That(bogføringViewModel.KontoDisponibel, Is.EqualTo(0M));
            Assert.That(bogføringViewModel.KontoDisponibelAsText, Is.Not.Null);
            Assert.That(bogføringViewModel.KontoDisponibelAsText, Is.Empty);
            Assert.That(bogføringViewModel.KontoDisponibelLabel, Is.Not.Null);
            Assert.That(bogføringViewModel.KontoDisponibelLabel, Is.Not.Empty);
            Assert.That(bogføringViewModel.KontoDisponibelLabel, Is.EqualTo(Resource.GetText(Text.Available)));
            Assert.That(bogføringViewModel.Tekst, Is.Not.Null);
            Assert.That(bogføringViewModel.Tekst, Is.Not.Empty);
            Assert.That(bogføringViewModel.Tekst, Is.EqualTo(bogføringslinjeModelMock.Tekst));
            Assert.That(bogføringViewModel.TekstMaxLength, Is.EqualTo(FieldInformations.BogføringstekstFieldLength));
            Assert.That(bogføringViewModel.TekstIsReadOnly, Is.False);
            Assert.That(bogføringViewModel.TekstLabel, Is.Not.Null);
            Assert.That(bogføringViewModel.TekstLabel, Is.Not.Empty);
            Assert.That(bogføringViewModel.TekstLabel, Is.EqualTo(Resource.GetText(Text.Text)));
            Assert.That(bogføringViewModel.Budgetkontonummer, Is.Null);
            Assert.That(bogføringViewModel.BudgetkontonummerMaxLength, Is.EqualTo(FieldInformations.KontonummerFieldLength));
            Assert.That(bogføringViewModel.BudgetkontonummerIsReadOnly, Is.False);
            Assert.That(bogføringViewModel.BudgetkontonummerLabel, Is.Not.Null);
            Assert.That(bogføringViewModel.BudgetkontonummerLabel, Is.Not.Empty);
            Assert.That(bogføringViewModel.BudgetkontonummerLabel, Is.EqualTo(Resource.GetText(Text.BudgetAccount)));
            Assert.That(bogføringViewModel.Budgetkontonavn, Is.Not.Null);
            Assert.That(bogføringViewModel.Budgetkontonavn, Is.Empty);
            Assert.That(bogføringViewModel.BudgetkontonavnLabel, Is.Not.Null);
            Assert.That(bogføringViewModel.BudgetkontonavnLabel, Is.Not.Empty);
            Assert.That(bogføringViewModel.BudgetkontonavnLabel, Is.EqualTo(Resource.GetText(Text.BudgetAccount)));
            Assert.That(bogføringViewModel.BudgetkontoBogført, Is.EqualTo(0M));
            Assert.That(bogføringViewModel.BudgetkontoBogførtAsText, Is.Not.Null);
            Assert.That(bogføringViewModel.BudgetkontoBogførtAsText, Is.Empty);
            Assert.That(bogføringViewModel.BudgetkontoBogførtLabel, Is.Not.Null);
            Assert.That(bogføringViewModel.BudgetkontoBogførtLabel, Is.Not.Empty);
            Assert.That(bogføringViewModel.BudgetkontoBogførtLabel, Is.EqualTo(Resource.GetText(Text.Bookkeeped)));
            Assert.That(bogføringViewModel.BudgetkontoDisponibel, Is.EqualTo(0M));
            Assert.That(bogføringViewModel.BudgetkontoDisponibelAsText, Is.Not.Null);
            Assert.That(bogføringViewModel.BudgetkontoDisponibelAsText, Is.Empty);
            Assert.That(bogføringViewModel.BudgetkontoDisponibelLabel, Is.Not.Null);
            Assert.That(bogføringViewModel.BudgetkontoDisponibelLabel, Is.Not.Empty);
            Assert.That(bogføringViewModel.BudgetkontoDisponibelLabel, Is.EqualTo(Resource.GetText(Text.Available)));
            Assert.That(bogføringViewModel.Debit, Is.EqualTo(bogføringslinjeModelMock.Debit));
            Assert.That(bogføringViewModel.DebitAsText, Is.Not.Null);
            Assert.That(bogføringViewModel.DebitAsText, Is.Not.Empty);
            Assert.That(bogføringViewModel.DebitAsText, Is.EqualTo(bogføringslinjeModelMock.Debit.ToString("C")));
            Assert.That(bogføringViewModel.DebitMaxLength, Is.EqualTo(FieldInformations.DebitFieldLength));
            Assert.That(bogføringViewModel.DebitIsReadOnly, Is.False);
            Assert.That(bogføringViewModel.DebitLabel, Is.Not.Null);
            Assert.That(bogføringViewModel.DebitLabel, Is.Not.Empty);
            Assert.That(bogføringViewModel.DebitLabel, Is.EqualTo(Resource.GetText(Text.Debit)));
            Assert.That(bogføringViewModel.Kredit, Is.EqualTo(bogføringslinjeModelMock.Kredit));
            Assert.That(bogføringViewModel.KreditAsText, Is.Not.Null);
            Assert.That(bogføringViewModel.KreditAsText, Is.Not.Empty);
            Assert.That(bogføringViewModel.KreditAsText, Is.EqualTo(bogføringslinjeModelMock.Kredit.ToString("C")));
            Assert.That(bogføringViewModel.KreditMaxLength, Is.EqualTo(FieldInformations.KreditFieldLength));
            Assert.That(bogføringViewModel.KreditIsReadOnly, Is.False);
            Assert.That(bogføringViewModel.KreditLabel, Is.Not.Null);
            Assert.That(bogføringViewModel.KreditLabel, Is.Not.Empty);
            Assert.That(bogføringViewModel.KreditLabel, Is.EqualTo(Resource.GetText(Text.Credit)));
            Assert.That(bogføringViewModel.Adressekonto, Is.EqualTo(bogføringslinjeModelMock.Adressekonto));
            Assert.That(bogføringViewModel.AdressekontoIsReadOnly, Is.False);
            Assert.That(bogføringViewModel.AdressekontoLabel, Is.Not.Null);
            Assert.That(bogføringViewModel.AdressekontoLabel, Is.Not.Empty);
            Assert.That(bogføringViewModel.AdressekontoLabel, Is.EqualTo(Resource.GetText(Text.AddressAccount)));
            Assert.That(bogføringViewModel.AdressekontoNavn, Is.Not.Null);
            Assert.That(bogføringViewModel.AdressekontoNavn, Is.Empty);
            Assert.That(bogføringViewModel.AdressekontoNavnLabel, Is.Not.Null);
            Assert.That(bogføringViewModel.AdressekontoNavnLabel, Is.Not.Empty);
            Assert.That(bogføringViewModel.AdressekontoNavnLabel, Is.EqualTo(Resource.GetText(Text.Name)));
            Assert.That(bogføringViewModel.AdressekontoSaldo, Is.EqualTo(0M));
            Assert.That(bogføringViewModel.AdressekontoSaldoAsText, Is.Not.Null);
            Assert.That(bogføringViewModel.AdressekontoSaldoAsText, Is.Empty);
            Assert.That(bogføringViewModel.AdressekontoSaldoLabel, Is.Not.Null);
            Assert.That(bogføringViewModel.AdressekontoSaldoLabel, Is.Not.Empty);
            Assert.That(bogføringViewModel.AdressekontoSaldoLabel, Is.EqualTo(Resource.GetText(Text.Balance)));
            Assert.That(bogføringViewModel.Adressekonti, Is.Not.Null);
            Assert.That(bogføringViewModel.Adressekonti, Is.Empty);
            Assert.That(bogføringViewModel.AdressekontiLabel, Is.Not.Null);
            Assert.That(bogføringViewModel.AdressekontiLabel, Is.Not.Empty);
            Assert.That(bogføringViewModel.AdressekontiLabel, Is.EqualTo(Resource.GetText(Text.AddressAccounts)));
            Assert.That(bogføringViewModel.Tasks, Is.Not.Null);
            Assert.That(bogføringViewModel.Tasks, Is.Empty);
            Assert.That(bogføringViewModel.IsWorking, Is.False);
            Assert.That(bogføringViewModel.BogførCommand, Is.Not.Null);
            Assert.That(bogføringViewModel.BogførCommand, Is.TypeOf<CommandCollectionExecuterCommand>());

            bogføringslinjeModelMock.AssertWasCalled(m => m.Dato);
            bogføringslinjeModelMock.AssertWasCalled(m => m.Bilag);
            bogføringslinjeModelMock.AssertWasCalled(m => m.Kontonummer);
            bogføringslinjeModelMock.AssertWasCalled(m => m.Tekst);
            bogføringslinjeModelMock.AssertWasCalled(m => m.Budgetkontonummer);
            bogføringslinjeModelMock.AssertWasCalled(m => m.Debit);
            bogføringslinjeModelMock.AssertWasCalled(m => m.Kredit);
            bogføringslinjeModelMock.AssertWasCalled(m => m.Adressekonto);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis ViewModel for regnskabet, hvorpå der skal bogføres, er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisRegnskabViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IBogføringslinjeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringslinjeModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new BogføringViewModel(null, fixture.Create<IBogføringslinjeModel>(), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("regnskabViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis modellen til den ny bogføringslinje, der kan tilrettes og bogføres, er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisBogføringslinjeModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), null, fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("bogføringslinjeModel"));
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
            fixture.Customize<IBogføringslinjeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringslinjeModel>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), fixture.Create<IBogføringslinjeModel>(), null, fixture.Create<IExceptionHandlerViewModel>()));
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
            fixture.Customize<IBogføringslinjeModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringslinjeModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), fixture.Create<IBogføringslinjeModel>(), fixture.Create<IFinansstyringRepository>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("exceptionHandlerViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at sætteren til DatoAsText opdaterer Dato på modellen for bogføringslinjen.
        /// </summary>
        [Test]
        public void TestAtDatoAsTextSetterOpdatererDatoOnBogføringslinjeModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            var newValue = fixture.Create<DateTime>().AddDays(-7).ToShortDateString();
            bogføringViewModel.DatoAsText = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Dato = Arg<DateTime>.Is.Equal(DateTime.Parse(newValue, CultureInfo.CurrentUICulture)));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at sætteren til DatoAsText kalder HandleExcetion på exceptionhandleren ved valideringsfejl.
        /// </summary>
        [Test]
        [TestCase(null, "ValueIsRequiered")]
        [TestCase("", "ValueIsRequiered")]
        [TestCase(" ", "ValueIsRequiered")]
        [TestCase("XYZ", "ValueIsNotDate")]
        [TestCase("ZYX", "ValueIsNotDate")]
        [TestCase("2014-31-01", "ValueIsNotDate")]
        [TestCase("2014-01-32", "ValueIsNotDate")]
        [TestCase("2050-01-01", "DateGreaterThan")]
        [TestCase("2050-01-31", "DateGreaterThan")]
        [TestCase("2050-12-01", "DateGreaterThan")]
        [TestCase("2050-12-31", "DateGreaterThan")]
        public void TestAtDatoAsTextSetterKalderHandleExceptionOnExceptionHandlerViewModelVedIntranetGuiValidationException(string illegalValue, string validationErrorText)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var text = (Text) Enum.Parse(typeof (Text), validationErrorText);
                                                 var validationException = (IntranetGuiValidationException)e.Arguments.ElementAt(0);
                                                 Assert.That(validationException, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Empty);
                                                 switch (text)
                                                 {
                                                     case Text.DateGreaterThan: 
                                                         Assert.That(validationException.Message, Is.EqualTo(Resource.GetText(text, DateTime.Now.ToLongDateString())));
                                                         break;

                                                     default:
                                                         Assert.That(validationException.Message, Is.EqualTo(Resource.GetText(text)));
                                                         break;
                                                 }
                                                 Assert.That(validationException.ValidationContext, Is.Not.Null);
                                                 Assert.That(validationException.ValidationContext, Is.TypeOf<BogføringViewModel>());
                                                 Assert.That(validationException.PropertyName, Is.Not.Null);
                                                 Assert.That(validationException.PropertyName, Is.Not.Empty);
                                                 Assert.That(validationException.PropertyName, Is.EqualTo("DatoAsText"));
                                                 Assert.That(validationException.Value, Is.EqualTo(illegalValue));
                                                 Assert.That(validationException.InnerException, Is.Null);
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.DatoAsText = illegalValue;

            bogføringslinjeModelMock.AssertWasNotCalled(m => m.Dato = Arg<DateTime>.Is.Anything);
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til DatoAsText kalder HandleException på exceptionhandleren med en IntranetGuiValidationException ved en ArgumentNullException.
        /// </summary>
        [Test]
        public void TestAtDatoAsTextSetterKalderHandleExceptionOnExceptionHandlerViewModelMedIntranetGuiValidationExceptionVedArgumentNullException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Dato = Arg<DateTime>.Is.Anything)
                                    .Throw(fixture.Create<ArgumentNullException>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();

            var newValue = fixture.Create<DateTime>().AddDays(-7).ToShortDateString();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var validationException = (IntranetGuiValidationException) e.Arguments.ElementAt(0);
                                                 Assert.That(validationException, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Empty);
                                                 Assert.That(validationException.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingBookkeepingDate)));
                                                 Assert.That(validationException.ValidationContext, Is.Not.Null);
                                                 Assert.That(validationException.ValidationContext, Is.TypeOf<BogføringViewModel>());
                                                 Assert.That(validationException.PropertyName, Is.Not.Null);
                                                 Assert.That(validationException.PropertyName, Is.Not.Empty);
                                                 Assert.That(validationException.PropertyName, Is.EqualTo("DatoAsText"));
                                                 Assert.That(validationException.Value, Is.EqualTo(newValue));
                                                 Assert.That(validationException.InnerException, Is.Not.Null);
                                                 Assert.That(validationException.InnerException, Is.TypeOf<ArgumentNullException>());
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.DatoAsText = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Dato = Arg<DateTime>.Is.Equal(DateTime.Parse(newValue, CultureInfo.CurrentUICulture)));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til DatoAsText kalder HandleException på exceptionhandleren med en IntranetGuiValidationException ved en ArgumentException.
        /// </summary>
        [Test]
        public void TestAtDatoAsTextSetterKalderHandleExceptionOnExceptionHandlerViewModelMedIntranetGuiValidationExceptionVedArgumentException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Dato = Arg<DateTime>.Is.Anything)
                                    .Throw(fixture.Create<ArgumentException>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();

            var newValue = fixture.Create<DateTime>().AddDays(-7).ToShortDateString();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var validationException = (IntranetGuiValidationException) e.Arguments.ElementAt(0);
                                                 Assert.That(validationException, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Empty);
                                                 Assert.That(validationException.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingBookkeepingDate)));
                                                 Assert.That(validationException.ValidationContext, Is.Not.Null);
                                                 Assert.That(validationException.ValidationContext, Is.TypeOf<BogføringViewModel>());
                                                 Assert.That(validationException.PropertyName, Is.Not.Null);
                                                 Assert.That(validationException.PropertyName, Is.Not.Empty);
                                                 Assert.That(validationException.PropertyName, Is.EqualTo("DatoAsText"));
                                                 Assert.That(validationException.Value, Is.EqualTo(newValue));
                                                 Assert.That(validationException.InnerException, Is.Not.Null);
                                                 Assert.That(validationException.InnerException, Is.TypeOf<ArgumentException>());
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.DatoAsText = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Dato = Arg<DateTime>.Is.Equal(DateTime.Parse(newValue, CultureInfo.CurrentUICulture)));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til DatoAsText kalder HandleException på exceptionhandleren ved IntranetGuiRepositoryException.
        /// </summary>
        [Test]
        public void TestAtDatoAsTextSetterKalderHandleExceptionOnExceptionHandlerViewModelVedIntranetGuiRepositoryException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = fixture.Create<IntranetGuiRepositoryException>();
            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Dato = Arg<DateTime>.Is.Anything)
                                    .Throw(exception)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();

            var newValue = fixture.Create<DateTime>().AddDays(-7).ToShortDateString();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiRepositoryException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var repositoryException = (IntranetGuiRepositoryException) e.Arguments.ElementAt(0);
                                                 Assert.That(repositoryException, Is.Not.Null);
                                                 Assert.That(repositoryException.Message, Is.Not.Null);
                                                 Assert.That(repositoryException.Message, Is.Not.Empty);
                                                 Assert.That(repositoryException.Message, Is.EqualTo(exception.Message));
                                                 Assert.That(repositoryException.InnerException, Is.Null);
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.DatoAsText = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Dato = Arg<DateTime>.Is.Equal(DateTime.Parse(newValue, CultureInfo.CurrentUICulture)));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiRepositoryException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til DatoAsText kalder HandleException på exceptionhandleren ved IntranetGuiBusinessException.
        /// </summary>
        [Test]
        public void TestAtDatoAsTextSetterKalderHandleExceptionOnExceptionHandlerViewModelVedIntranetGuiBusinessException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = fixture.Create<IntranetGuiBusinessException>();
            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Dato = Arg<DateTime>.Is.Anything)
                                    .Throw(exception)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();

            var newValue = fixture.Create<DateTime>().AddDays(-7).ToShortDateString();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiBusinessException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var businessException = (IntranetGuiBusinessException) e.Arguments.ElementAt(0);
                                                 Assert.That(businessException, Is.Not.Null);
                                                 Assert.That(businessException.Message, Is.Not.Null);
                                                 Assert.That(businessException.Message, Is.Not.Empty);
                                                 Assert.That(businessException.Message, Is.EqualTo(exception.Message));
                                                 Assert.That(businessException.InnerException, Is.Null);
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.DatoAsText = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Dato = Arg<DateTime>.Is.Equal(DateTime.Parse(newValue, CultureInfo.CurrentUICulture)));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiBusinessException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til DatoAsText kalder HandleException på exceptionhandleren ved IntranetGuiSystemException.
        /// </summary>
        [Test]
        public void TestAtDatoAsTextSetterKalderHandleExceptionOnExceptionHandlerViewModelVedIntranetGuiSystemException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = fixture.Create<IntranetGuiSystemException>();
            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Dato = Arg<DateTime>.Is.Anything)
                                    .Throw(exception)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();

            var newValue = fixture.Create<DateTime>().AddDays(-7).ToShortDateString();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var systemException = (IntranetGuiSystemException) e.Arguments.ElementAt(0);
                                                 Assert.That(systemException, Is.Not.Null);
                                                 Assert.That(systemException.Message, Is.Not.Null);
                                                 Assert.That(systemException.Message, Is.Not.Empty);
                                                 Assert.That(systemException.Message, Is.EqualTo(exception.Message));
                                                 Assert.That(systemException.InnerException, Is.Null);
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.DatoAsText = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Dato = Arg<DateTime>.Is.Equal(DateTime.Parse(newValue, CultureInfo.CurrentUICulture)));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til DatoAsText kalder HandleException på exceptionhandleren ved Exception.
        /// </summary>
        [Test]
        public void TestAtDatoAsTextSetterKalderHandleExceptionOnExceptionHandlerViewModelVedException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = fixture.Create<Exception>();
            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Dato = Arg<DateTime>.Is.Anything)
                                    .Throw(exception)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();

            var newValue = fixture.Create<DateTime>().AddDays(-7).ToShortDateString();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var systemException = (IntranetGuiSystemException) e.Arguments.ElementAt(0);
                                                 Assert.That(systemException, Is.Not.Null);
                                                 Assert.That(systemException.Message, Is.Not.Null);
                                                 Assert.That(systemException.Message, Is.Not.Empty);
                                                 Assert.That(systemException.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingPropertyValue, "DatoAsText", exception.Message)));
                                                 Assert.That(systemException.InnerException, Is.Not.Null);
                                                 Assert.That(systemException.InnerException, Is.EqualTo(exception));
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.DatoAsText = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Dato = Arg<DateTime>.Is.Equal(DateTime.Parse(newValue, CultureInfo.CurrentUICulture)));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til Bilag opdaterer Bilag på modellen for bogføringslinjen.
        /// </summary>
        [Test]
        public void TestAtBilagSetterOpdatererBilagOnBogføringslinjeModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            var newValue = fixture.Create<string>();
            bogføringViewModel.Bilag = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Bilag = Arg<string>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at sætteren til Bilag kalder HandleException på exceptionhandleren med en IntranetGuiValidationException ved en ArgumentNullException.
        /// </summary>
        [Test]
        public void TestAtBilagSetterKalderHandleExceptionOnExceptionHandlerViewModelMedIntranetGuiValidationExceptionVedArgumentNullException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Bilag = Arg<string>.Is.Anything)
                                    .Throw(fixture.Create<ArgumentNullException>())
                                    .Repeat.Any();

            var newValue = fixture.Create<string>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var validationException = (IntranetGuiValidationException) e.Arguments.ElementAt(0);
                                                 Assert.That(validationException, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Empty);
                                                 Assert.That(validationException.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingAnnex)));
                                                 Assert.That(validationException.ValidationContext, Is.Not.Null);
                                                 Assert.That(validationException.ValidationContext, Is.TypeOf<BogføringViewModel>());
                                                 Assert.That(validationException.PropertyName, Is.Not.Null);
                                                 Assert.That(validationException.PropertyName, Is.Not.Empty);
                                                 Assert.That(validationException.PropertyName, Is.EqualTo("Bilag"));
                                                 Assert.That(validationException.Value, Is.EqualTo(newValue));
                                                 Assert.That(validationException.InnerException, Is.Not.Null);
                                                 Assert.That(validationException.InnerException, Is.TypeOf<ArgumentNullException>());
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.Bilag = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Bilag = Arg<string>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til Bilag kalder HandleException på exceptionhandleren med en IntranetGuiValidationException ved en ArgumentException.
        /// </summary>
        [Test]
        public void TestAtBilagSetterKalderHandleExceptionOnExceptionHandlerViewModelMedIntranetGuiValidationExceptionVedArgumentException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Bilag = Arg<string>.Is.Anything)
                                    .Throw(fixture.Create<ArgumentException>())
                                    .Repeat.Any();

            var newValue = fixture.Create<string>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var validationException = (IntranetGuiValidationException) e.Arguments.ElementAt(0);
                                                 Assert.That(validationException, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Empty);
                                                 Assert.That(validationException.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingAnnex)));
                                                 Assert.That(validationException.ValidationContext, Is.Not.Null);
                                                 Assert.That(validationException.ValidationContext, Is.TypeOf<BogføringViewModel>());
                                                 Assert.That(validationException.PropertyName, Is.Not.Null);
                                                 Assert.That(validationException.PropertyName, Is.Not.Empty);
                                                 Assert.That(validationException.PropertyName, Is.EqualTo("Bilag"));
                                                 Assert.That(validationException.Value, Is.EqualTo(newValue));
                                                 Assert.That(validationException.InnerException, Is.Not.Null);
                                                 Assert.That(validationException.InnerException, Is.TypeOf<ArgumentException>());
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.Bilag = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Bilag = Arg<string>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til Bilag kalder HandleException på exceptionhandleren ved IntranetGuiRepositoryException.
        /// </summary>
        [Test]
        public void TestAtBilagSetterKalderHandleExceptionOnExceptionHandlerViewModelVedIntranetGuiRepositoryException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = fixture.Create<IntranetGuiRepositoryException>();
            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Bilag = Arg<string>.Is.Anything)
                                    .Throw(exception)
                                    .Repeat.Any();

            var newValue = fixture.Create<string>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiRepositoryException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var repositoryException = (IntranetGuiRepositoryException) e.Arguments.ElementAt(0);
                                                 Assert.That(repositoryException, Is.Not.Null);
                                                 Assert.That(repositoryException.Message, Is.Not.Null);
                                                 Assert.That(repositoryException.Message, Is.Not.Empty);
                                                 Assert.That(repositoryException.Message, Is.EqualTo(exception.Message));
                                                 Assert.That(repositoryException.InnerException, Is.Null);
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.Bilag = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Bilag = Arg<string>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiRepositoryException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til Bilag kalder HandleException på exceptionhandleren ved IntranetGuiBusinessException.
        /// </summary>
        [Test]
        public void TestAtBilagSetterKalderHandleExceptionOnExceptionHandlerViewModelVedIntranetGuiBusinessException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = fixture.Create<IntranetGuiBusinessException>();
            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Bilag = Arg<string>.Is.Anything)
                                    .Throw(exception)
                                    .Repeat.Any();

            var newValue = fixture.Create<string>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiBusinessException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var businessException = (IntranetGuiBusinessException) e.Arguments.ElementAt(0);
                                                 Assert.That(businessException, Is.Not.Null);
                                                 Assert.That(businessException.Message, Is.Not.Null);
                                                 Assert.That(businessException.Message, Is.Not.Empty);
                                                 Assert.That(businessException.Message, Is.EqualTo(exception.Message));
                                                 Assert.That(businessException.InnerException, Is.Null);
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.Bilag = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Bilag = Arg<string>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiBusinessException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til Bilag kalder HandleException på exceptionhandleren ved IntranetGuiSystemException.
        /// </summary>
        [Test]
        public void TestAtBilagSetterKalderHandleExceptionOnExceptionHandlerViewModelVedIntranetGuiSystemException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = fixture.Create<IntranetGuiSystemException>();
            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Bilag = Arg<string>.Is.Anything)
                                    .Throw(exception)
                                    .Repeat.Any();

            var newValue = fixture.Create<string>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var systemException = (IntranetGuiSystemException) e.Arguments.ElementAt(0);
                                                 Assert.That(systemException, Is.Not.Null);
                                                 Assert.That(systemException.Message, Is.Not.Null);
                                                 Assert.That(systemException.Message, Is.Not.Empty);
                                                 Assert.That(systemException.Message, Is.EqualTo(exception.Message));
                                                 Assert.That(systemException.InnerException, Is.Null);
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.Bilag = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Bilag = Arg<string>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til Bilag kalder HandleException på exceptionhandleren ved Exception.
        /// </summary>
        [Test]
        public void TestAtBilagSetterKalderHandleExceptionOnExceptionHandlerViewModelVedException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = fixture.Create<Exception>();
            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Bilag = Arg<string>.Is.Anything)
                                    .Throw(exception)
                                    .Repeat.Any();

            var newValue = fixture.Create<string>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var systemException = (IntranetGuiSystemException) e.Arguments.ElementAt(0);
                                                 Assert.That(systemException, Is.Not.Null);
                                                 Assert.That(systemException.Message, Is.Not.Null);
                                                 Assert.That(systemException.Message, Is.Not.Empty);
                                                 Assert.That(systemException.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingPropertyValue, "Bilag", exception.Message)));
                                                 Assert.That(systemException.InnerException, Is.Not.Null);
                                                 Assert.That(systemException.InnerException, Is.EqualTo(exception));
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.Bilag = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Bilag = Arg<string>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til Kontonummer opdaterer Kontonummer på modellen for bogføringslinjen.
        /// </summary>
        [Test]
        public void TestAtKontonummerSetterOpdatererKontonummerOnBogføringslinjeModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            var newValue = fixture.Create<string>();
            bogføringViewModel.Kontonummer = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Kontonummer = Arg<string>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at sætteren til Kontonummer kalder HandleExcetion på exceptionhandleren ved valideringsfejl.
        /// </summary>
        [Test]
        [TestCase(null, "ValueIsRequiered")]
        [TestCase("", "ValueIsRequiered")]
        [TestCase(" ", "ValueIsRequiered")]
        public void TestAtKontonummerSetterKalderHandleExceptionOnExceptionHandlerViewModelVedIntranetGuiValidationException(string illegalValue, string validationErrorText)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var text = (Text) Enum.Parse(typeof (Text), validationErrorText);
                                                 var validationException = (IntranetGuiValidationException) e.Arguments.ElementAt(0);
                                                 Assert.That(validationException, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Empty);
                                                 Assert.That(validationException.Message, Is.EqualTo(Resource.GetText(text)));
                                                 Assert.That(validationException.ValidationContext, Is.Not.Null);
                                                 Assert.That(validationException.ValidationContext, Is.TypeOf<BogføringViewModel>());
                                                 Assert.That(validationException.PropertyName, Is.Not.Null);
                                                 Assert.That(validationException.PropertyName, Is.Not.Empty);
                                                 Assert.That(validationException.PropertyName, Is.EqualTo("Kontonummer"));
                                                 Assert.That(validationException.Value, Is.EqualTo(illegalValue));
                                                 Assert.That(validationException.InnerException, Is.Null);
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.Kontonummer = illegalValue;

            bogføringslinjeModelMock.AssertWasNotCalled(m => m.Kontonummer = Arg<string>.Is.Anything);
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til Kontonummer kalder HandleException på exceptionhandleren med en IntranetGuiValidationException ved en ArgumentNullException.
        /// </summary>
        [Test]
        public void TestAtKontonummerSetterKalderHandleExceptionOnExceptionHandlerViewModelMedIntranetGuiValidationExceptionVedArgumentNullException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer = Arg<string>.Is.Anything)
                                    .Throw(fixture.Create<ArgumentNullException>())
                                    .Repeat.Any();

            var newValue = fixture.Create<string>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var validationException = (IntranetGuiValidationException) e.Arguments.ElementAt(0);
                                                 Assert.That(validationException, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Empty);
                                                 Assert.That(validationException.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingAccountNumber)));
                                                 Assert.That(validationException.ValidationContext, Is.Not.Null);
                                                 Assert.That(validationException.ValidationContext, Is.TypeOf<BogføringViewModel>());
                                                 Assert.That(validationException.PropertyName, Is.Not.Null);
                                                 Assert.That(validationException.PropertyName, Is.Not.Empty);
                                                 Assert.That(validationException.PropertyName, Is.EqualTo("Kontonummer"));
                                                 Assert.That(validationException.Value, Is.EqualTo(newValue));
                                                 Assert.That(validationException.InnerException, Is.Not.Null);
                                                 Assert.That(validationException.InnerException, Is.TypeOf<ArgumentNullException>());
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.Kontonummer = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Kontonummer = Arg<string>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til Kontonummer kalder HandleException på exceptionhandleren med en IntranetGuiValidationException ved en ArgumentException.
        /// </summary>
        [Test]
        public void TestAtKontonummerSetterKalderHandleExceptionOnExceptionHandlerViewModelMedIntranetGuiValidationExceptionVedArgumentException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer = Arg<string>.Is.Anything)
                                    .Throw(fixture.Create<ArgumentException>())
                                    .Repeat.Any();

            var newValue = fixture.Create<string>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var validationException = (IntranetGuiValidationException) e.Arguments.ElementAt(0);
                                                 Assert.That(validationException, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Empty);
                                                 Assert.That(validationException.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingAccountNumber)));
                                                 Assert.That(validationException.ValidationContext, Is.Not.Null);
                                                 Assert.That(validationException.ValidationContext, Is.TypeOf<BogføringViewModel>());
                                                 Assert.That(validationException.PropertyName, Is.Not.Null);
                                                 Assert.That(validationException.PropertyName, Is.Not.Empty);
                                                 Assert.That(validationException.PropertyName, Is.EqualTo("Kontonummer"));
                                                 Assert.That(validationException.Value, Is.EqualTo(newValue));
                                                 Assert.That(validationException.InnerException, Is.Not.Null);
                                                 Assert.That(validationException.InnerException, Is.TypeOf<ArgumentException>());
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.Kontonummer = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Kontonummer = Arg<string>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til Kontonummer kalder HandleException på exceptionhandleren ved IntranetGuiRepositoryException.
        /// </summary>
        [Test]
        public void TestAtKontonummerSetterKalderHandleExceptionOnExceptionHandlerViewModelVedIntranetGuiRepositoryException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = fixture.Create<IntranetGuiRepositoryException>();
            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer = Arg<string>.Is.Anything)
                                    .Throw(exception)
                                    .Repeat.Any();

            var newValue = fixture.Create<string>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiRepositoryException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var repositoryException = (IntranetGuiRepositoryException) e.Arguments.ElementAt(0);
                                                 Assert.That(repositoryException, Is.Not.Null);
                                                 Assert.That(repositoryException.Message, Is.Not.Null);
                                                 Assert.That(repositoryException.Message, Is.Not.Empty);
                                                 Assert.That(repositoryException.Message, Is.EqualTo(exception.Message));
                                                 Assert.That(repositoryException.InnerException, Is.Null);
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.Kontonummer = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Kontonummer = Arg<string>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiRepositoryException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til Kontonummer kalder HandleException på exceptionhandleren ved IntranetGuiBusinessException.
        /// </summary>
        [Test]
        public void TestAtKontonummerSetterKalderHandleExceptionOnExceptionHandlerViewModelVedIntranetGuiBusinessException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = fixture.Create<IntranetGuiBusinessException>();
            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer = Arg<string>.Is.Anything)
                                    .Throw(exception)
                                    .Repeat.Any();

            var newValue = fixture.Create<string>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiBusinessException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var businessException = (IntranetGuiBusinessException) e.Arguments.ElementAt(0);
                                                 Assert.That(businessException, Is.Not.Null);
                                                 Assert.That(businessException.Message, Is.Not.Null);
                                                 Assert.That(businessException.Message, Is.Not.Empty);
                                                 Assert.That(businessException.Message, Is.EqualTo(exception.Message));
                                                 Assert.That(businessException.InnerException, Is.Null);
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.Kontonummer = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Kontonummer = Arg<string>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiBusinessException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til Kontonummer kalder HandleException på exceptionhandleren ved IntranetGuiSystemException.
        /// </summary>
        [Test]
        public void TestAtKontonummerSetterKalderHandleExceptionOnExceptionHandlerViewModelVedIntranetGuiSystemException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = fixture.Create<IntranetGuiSystemException>();
            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer = Arg<string>.Is.Anything)
                                    .Throw(exception)
                                    .Repeat.Any();

            var newValue = fixture.Create<string>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var systemException = (IntranetGuiSystemException) e.Arguments.ElementAt(0);
                                                 Assert.That(systemException, Is.Not.Null);
                                                 Assert.That(systemException.Message, Is.Not.Null);
                                                 Assert.That(systemException.Message, Is.Not.Empty);
                                                 Assert.That(systemException.Message, Is.EqualTo(exception.Message));
                                                 Assert.That(systemException.InnerException, Is.Null);
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.Kontonummer = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Kontonummer = Arg<string>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til Kontonummer kalder HandleException på exceptionhandleren ved Exception.
        /// </summary>
        [Test]
        public void TestAtKontonummerSetterKalderHandleExceptionOnExceptionHandlerViewModelVedException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = fixture.Create<Exception>();
            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer = Arg<string>.Is.Anything)
                                    .Throw(exception)
                                    .Repeat.Any();

            var newValue = fixture.Create<string>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var systemException = (IntranetGuiSystemException) e.Arguments.ElementAt(0);
                                                 Assert.That(systemException, Is.Not.Null);
                                                 Assert.That(systemException.Message, Is.Not.Null);
                                                 Assert.That(systemException.Message, Is.Not.Empty);
                                                 Assert.That(systemException.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingPropertyValue, "Kontonummer", exception.Message)));
                                                 Assert.That(systemException.InnerException, Is.Not.Null);
                                                 Assert.That(systemException.InnerException, Is.EqualTo(exception));
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.Kontonummer = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Kontonummer = Arg<string>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til Tekst opdaterer Tekst på modellen for bogføringslinjen.
        /// </summary>
        [Test]
        public void TestAtTekstSetterOpdatererTekstOnBogføringslinjeModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            var newValue = fixture.Create<string>();
            bogføringViewModel.Tekst = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Tekst = Arg<string>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at sætteren til Tekst kalder HandleExcetion på exceptionhandleren ved valideringsfejl.
        /// </summary>
        [Test]
        [TestCase(null, "ValueIsRequiered")]
        [TestCase("", "ValueIsRequiered")]
        [TestCase(" ", "ValueIsRequiered")]
        public void TestAtTekstSetterKalderHandleExceptionOnExceptionHandlerViewModelVedIntranetGuiValidationException(string illegalValue, string validationErrorText)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var text = (Text) Enum.Parse(typeof (Text), validationErrorText);
                                                 var validationException = (IntranetGuiValidationException) e.Arguments.ElementAt(0);
                                                 Assert.That(validationException, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Empty);
                                                 Assert.That(validationException.Message, Is.EqualTo(Resource.GetText(text)));
                                                 Assert.That(validationException.ValidationContext, Is.Not.Null);
                                                 Assert.That(validationException.ValidationContext, Is.TypeOf<BogføringViewModel>());
                                                 Assert.That(validationException.PropertyName, Is.Not.Null);
                                                 Assert.That(validationException.PropertyName, Is.Not.Empty);
                                                 Assert.That(validationException.PropertyName, Is.EqualTo("Tekst"));
                                                 Assert.That(validationException.Value, Is.EqualTo(illegalValue));
                                                 Assert.That(validationException.InnerException, Is.Null);
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.Tekst = illegalValue;

            bogføringslinjeModelMock.AssertWasNotCalled(m => m.Tekst = Arg<string>.Is.Anything);
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til Tekst kalder HandleException på exceptionhandleren med en IntranetGuiValidationException ved en ArgumentNullException.
        /// </summary>
        [Test]
        public void TestAtTekstSetterKalderHandleExceptionOnExceptionHandlerViewModelMedIntranetGuiValidationExceptionVedArgumentNullException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Tekst = Arg<string>.Is.Anything)
                                    .Throw(fixture.Create<ArgumentNullException>())
                                    .Repeat.Any();

            var newValue = fixture.Create<string>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var validationException = (IntranetGuiValidationException) e.Arguments.ElementAt(0);
                                                 Assert.That(validationException, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Empty);
                                                 Assert.That(validationException.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingBookkeepingText)));
                                                 Assert.That(validationException.ValidationContext, Is.Not.Null);
                                                 Assert.That(validationException.ValidationContext, Is.TypeOf<BogføringViewModel>());
                                                 Assert.That(validationException.PropertyName, Is.Not.Null);
                                                 Assert.That(validationException.PropertyName, Is.Not.Empty);
                                                 Assert.That(validationException.PropertyName, Is.EqualTo("Tekst"));
                                                 Assert.That(validationException.Value, Is.EqualTo(newValue));
                                                 Assert.That(validationException.InnerException, Is.Not.Null);
                                                 Assert.That(validationException.InnerException, Is.TypeOf<ArgumentNullException>());
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.Tekst = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Tekst = Arg<string>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til Tekst kalder HandleException på exceptionhandleren med en IntranetGuiValidationException ved en ArgumentException.
        /// </summary>
        [Test]
        public void TestAtTekstSetterKalderHandleExceptionOnExceptionHandlerViewModelMedIntranetGuiValidationExceptionVedArgumentException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Tekst = Arg<string>.Is.Anything)
                                    .Throw(fixture.Create<ArgumentException>())
                                    .Repeat.Any();

            var newValue = fixture.Create<string>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var validationException = (IntranetGuiValidationException) e.Arguments.ElementAt(0);
                                                 Assert.That(validationException, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Empty);
                                                 Assert.That(validationException.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingBookkeepingText)));
                                                 Assert.That(validationException.ValidationContext, Is.Not.Null);
                                                 Assert.That(validationException.ValidationContext, Is.TypeOf<BogføringViewModel>());
                                                 Assert.That(validationException.PropertyName, Is.Not.Null);
                                                 Assert.That(validationException.PropertyName, Is.Not.Empty);
                                                 Assert.That(validationException.PropertyName, Is.EqualTo("Tekst"));
                                                 Assert.That(validationException.Value, Is.EqualTo(newValue));
                                                 Assert.That(validationException.InnerException, Is.Not.Null);
                                                 Assert.That(validationException.InnerException, Is.TypeOf<ArgumentException>());
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.Tekst = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Tekst = Arg<string>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til Tekst kalder HandleException på exceptionhandleren ved IntranetGuiRepositoryException.
        /// </summary>
        [Test]
        public void TestAtTekstSetterKalderHandleExceptionOnExceptionHandlerViewModelVedIntranetGuiRepositoryException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = fixture.Create<IntranetGuiRepositoryException>();
            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Tekst = Arg<string>.Is.Anything)
                                    .Throw(exception)
                                    .Repeat.Any();

            var newValue = fixture.Create<string>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiRepositoryException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var repositoryException = (IntranetGuiRepositoryException) e.Arguments.ElementAt(0);
                                                 Assert.That(repositoryException, Is.Not.Null);
                                                 Assert.That(repositoryException.Message, Is.Not.Null);
                                                 Assert.That(repositoryException.Message, Is.Not.Empty);
                                                 Assert.That(repositoryException.Message, Is.EqualTo(exception.Message));
                                                 Assert.That(repositoryException.InnerException, Is.Null);
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.Tekst = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Tekst = Arg<string>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiRepositoryException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til Tekst kalder HandleException på exceptionhandleren ved IntranetGuiBusinessException.
        /// </summary>
        [Test]
        public void TestAtTekstSetterKalderHandleExceptionOnExceptionHandlerViewModelVedIntranetGuiBusinessException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = fixture.Create<IntranetGuiBusinessException>();
            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Tekst = Arg<string>.Is.Anything)
                                    .Throw(exception)
                                    .Repeat.Any();

            var newValue = fixture.Create<string>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiBusinessException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var businessException = (IntranetGuiBusinessException) e.Arguments.ElementAt(0);
                                                 Assert.That(businessException, Is.Not.Null);
                                                 Assert.That(businessException.Message, Is.Not.Null);
                                                 Assert.That(businessException.Message, Is.Not.Empty);
                                                 Assert.That(businessException.Message, Is.EqualTo(exception.Message));
                                                 Assert.That(businessException.InnerException, Is.Null);
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.Tekst = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Tekst = Arg<string>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiBusinessException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til Tekst kalder HandleException på exceptionhandleren ved IntranetGuiSystemException.
        /// </summary>
        [Test]
        public void TestAtTekstSetterKalderHandleExceptionOnExceptionHandlerViewModelVedIntranetGuiSystemException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = fixture.Create<IntranetGuiSystemException>();
            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Tekst = Arg<string>.Is.Anything)
                                    .Throw(exception)
                                    .Repeat.Any();

            var newValue = fixture.Create<string>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var systemException = (IntranetGuiSystemException) e.Arguments.ElementAt(0);
                                                 Assert.That(systemException, Is.Not.Null);
                                                 Assert.That(systemException.Message, Is.Not.Null);
                                                 Assert.That(systemException.Message, Is.Not.Empty);
                                                 Assert.That(systemException.Message, Is.EqualTo(exception.Message));
                                                 Assert.That(systemException.InnerException, Is.Null);
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.Tekst = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Tekst = Arg<string>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til Tekst kalder HandleException på exceptionhandleren ved Exception.
        /// </summary>
        [Test]
        public void TestAtTekstSetterKalderHandleExceptionOnExceptionHandlerViewModelVedException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = fixture.Create<Exception>();
            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Tekst = Arg<string>.Is.Anything)
                                    .Throw(exception)
                                    .Repeat.Any();

            var newValue = fixture.Create<string>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var systemException = (IntranetGuiSystemException) e.Arguments.ElementAt(0);
                                                 Assert.That(systemException, Is.Not.Null);
                                                 Assert.That(systemException.Message, Is.Not.Null);
                                                 Assert.That(systemException.Message, Is.Not.Empty);
                                                 Assert.That(systemException.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingPropertyValue, "Tekst", exception.Message)));
                                                 Assert.That(systemException.InnerException, Is.Not.Null);
                                                 Assert.That(systemException.InnerException, Is.EqualTo(exception));
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.Tekst = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Tekst = Arg<string>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til Budgetkontonummer opdaterer Budgetkontonummer på modellen for bogføringslinjen.
        /// </summary>
        [Test]
        public void TestAtBudgetkontonummerSetterOpdatererBudgetkontonummerOnBogføringslinjeModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            var newValue = fixture.Create<string>();
            bogføringViewModel.Budgetkontonummer = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Budgetkontonummer = Arg<string>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at sætteren til Budgetkontonummer kalder HandleException på exceptionhandleren med en IntranetGuiValidationException ved en ArgumentNullException.
        /// </summary>
        [Test]
        public void TestAtBudgetkontonummerSetterKalderHandleExceptionOnExceptionHandlerViewModelMedIntranetGuiValidationExceptionVedArgumentNullException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer = Arg<string>.Is.Anything)
                                    .Throw(fixture.Create<ArgumentNullException>())
                                    .Repeat.Any();

            var newValue = fixture.Create<string>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var validationException = (IntranetGuiValidationException) e.Arguments.ElementAt(0);
                                                 Assert.That(validationException, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Empty);
                                                 Assert.That(validationException.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingBudgetAccountNumber)));
                                                 Assert.That(validationException.ValidationContext, Is.Not.Null);
                                                 Assert.That(validationException.ValidationContext, Is.TypeOf<BogføringViewModel>());
                                                 Assert.That(validationException.PropertyName, Is.Not.Null);
                                                 Assert.That(validationException.PropertyName, Is.Not.Empty);
                                                 Assert.That(validationException.PropertyName, Is.EqualTo("Budgetkontonummer"));
                                                 Assert.That(validationException.Value, Is.EqualTo(newValue));
                                                 Assert.That(validationException.InnerException, Is.Not.Null);
                                                 Assert.That(validationException.InnerException, Is.TypeOf<ArgumentNullException>());
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.Budgetkontonummer = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Budgetkontonummer = Arg<string>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til Budgetkontonummer kalder HandleException på exceptionhandleren med en IntranetGuiValidationException ved en ArgumentException.
        /// </summary>
        [Test]
        public void TestAtBudgetkontonummerSetterKalderHandleExceptionOnExceptionHandlerViewModelMedIntranetGuiValidationExceptionVedArgumentException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer = Arg<string>.Is.Anything)
                                    .Throw(fixture.Create<ArgumentException>())
                                    .Repeat.Any();

            var newValue = fixture.Create<string>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var validationException = (IntranetGuiValidationException) e.Arguments.ElementAt(0);
                                                 Assert.That(validationException, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Empty);
                                                 Assert.That(validationException.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingBudgetAccountNumber)));
                                                 Assert.That(validationException.ValidationContext, Is.Not.Null);
                                                 Assert.That(validationException.ValidationContext, Is.TypeOf<BogføringViewModel>());
                                                 Assert.That(validationException.PropertyName, Is.Not.Null);
                                                 Assert.That(validationException.PropertyName, Is.Not.Empty);
                                                 Assert.That(validationException.PropertyName, Is.EqualTo("Budgetkontonummer"));
                                                 Assert.That(validationException.Value, Is.EqualTo(newValue));
                                                 Assert.That(validationException.InnerException, Is.Not.Null);
                                                 Assert.That(validationException.InnerException, Is.TypeOf<ArgumentException>());
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.Budgetkontonummer = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Budgetkontonummer = Arg<string>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til Budgetkontonummer kalder HandleException på exceptionhandleren ved IntranetGuiRepositoryException.
        /// </summary>
        [Test]
        public void TestAtBudgetkontonummerSetterKalderHandleExceptionOnExceptionHandlerViewModelVedIntranetGuiRepositoryException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = fixture.Create<IntranetGuiRepositoryException>();
            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer = Arg<string>.Is.Anything)
                                    .Throw(exception)
                                    .Repeat.Any();

            var newValue = fixture.Create<string>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiRepositoryException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var repositoryException = (IntranetGuiRepositoryException) e.Arguments.ElementAt(0);
                                                 Assert.That(repositoryException, Is.Not.Null);
                                                 Assert.That(repositoryException.Message, Is.Not.Null);
                                                 Assert.That(repositoryException.Message, Is.Not.Empty);
                                                 Assert.That(repositoryException.Message, Is.EqualTo(exception.Message));
                                                 Assert.That(repositoryException.InnerException, Is.Null);
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.Budgetkontonummer = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Budgetkontonummer = Arg<string>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiRepositoryException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til Budgetkontonummer kalder HandleException på exceptionhandleren ved IntranetGuiBusinessException.
        /// </summary>
        [Test]
        public void TestAtBudgetkontonummerSetterKalderHandleExceptionOnExceptionHandlerViewModelVedIntranetGuiBusinessException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = fixture.Create<IntranetGuiBusinessException>();
            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer = Arg<string>.Is.Anything)
                                    .Throw(exception)
                                    .Repeat.Any();

            var newValue = fixture.Create<string>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiBusinessException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var businessException = (IntranetGuiBusinessException) e.Arguments.ElementAt(0);
                                                 Assert.That(businessException, Is.Not.Null);
                                                 Assert.That(businessException.Message, Is.Not.Null);
                                                 Assert.That(businessException.Message, Is.Not.Empty);
                                                 Assert.That(businessException.Message, Is.EqualTo(exception.Message));
                                                 Assert.That(businessException.InnerException, Is.Null);
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.Budgetkontonummer = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Budgetkontonummer = Arg<string>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiBusinessException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til Budgetkontonummer kalder HandleException på exceptionhandleren ved IntranetGuiSystemException.
        /// </summary>
        [Test]
        public void TestAtBudgetkontonummerSetterKalderHandleExceptionOnExceptionHandlerViewModelVedIntranetGuiSystemException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = fixture.Create<IntranetGuiSystemException>();
            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer = Arg<string>.Is.Anything)
                                    .Throw(exception)
                                    .Repeat.Any();

            var newValue = fixture.Create<string>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var systemException = (IntranetGuiSystemException) e.Arguments.ElementAt(0);
                                                 Assert.That(systemException, Is.Not.Null);
                                                 Assert.That(systemException.Message, Is.Not.Null);
                                                 Assert.That(systemException.Message, Is.Not.Empty);
                                                 Assert.That(systemException.Message, Is.EqualTo(exception.Message));
                                                 Assert.That(systemException.InnerException, Is.Null);
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.Budgetkontonummer = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Budgetkontonummer = Arg<string>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til Budgetkontonummer kalder HandleException på exceptionhandleren ved Exception.
        /// </summary>
        [Test]
        public void TestAtBudgetkontonummerSetterKalderHandleExceptionOnExceptionHandlerViewModelVedException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = fixture.Create<Exception>();
            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer = Arg<string>.Is.Anything)
                                    .Throw(exception)
                                    .Repeat.Any();

            var newValue = fixture.Create<string>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var systemException = (IntranetGuiSystemException) e.Arguments.ElementAt(0);
                                                 Assert.That(systemException, Is.Not.Null);
                                                 Assert.That(systemException.Message, Is.Not.Null);
                                                 Assert.That(systemException.Message, Is.Not.Empty);
                                                 Assert.That(systemException.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingPropertyValue, "Budgetkontonummer", exception.Message)));
                                                 Assert.That(systemException.InnerException, Is.Not.Null);
                                                 Assert.That(systemException.InnerException, Is.EqualTo(exception));
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.Budgetkontonummer = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Budgetkontonummer = Arg<string>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til DebitAsText opdaterer Debit på modellen for bogføringslinjen.
        /// </summary>
        [Test]
        [TestCase(null, 0)]
        [TestCase("", 0)]
        [TestCase(" ", 0)]
        [TestCase("1,000.00", 1000.00)]
        [TestCase("2,000.00", 2000.00)]
        [TestCase("3,000.00", 3000.00)]
        [TestCase("4,000.00", 4000.00)]
        [TestCase("$ 1,000.00", 1000.00)]
        [TestCase("$ 2,000.00", 2000.00)]
        [TestCase("$ 3,000.00", 3000.00)]
        [TestCase("$ 4,000.00", 4000.00)]
        public void TestAtDebitAsTextSetterOpdatererDebitOnBogføringslinjeModel(string value, decimal expectedValue)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.DebitAsText = string.IsNullOrWhiteSpace(value) ? value : decimal.Parse(value, NumberStyles.Any, new CultureInfo("en-US")) .ToString("C");

            bogføringslinjeModelMock.AssertWasCalled(m => m.Debit = Arg<decimal>.Is.Equal(expectedValue));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at sætteren til DebitAsText kalder HandleExcetion på exceptionhandleren ved valideringsfejl.
        /// </summary>
        [Test]
        [TestCase("XYZ", "ValueIsNotDecimal")]
        [TestCase("ZYX", "ValueIsNotDecimal")]
        [TestCase("$ -0.01", "DecimalLowerThan")]
        [TestCase("$ -1000.00", "DecimalLowerThan")]
        [TestCase("$ -2000.00", "DecimalLowerThan")]
        [TestCase("$ -3000.00", "DecimalLowerThan")]
        [TestCase("$ -4000.00", "DecimalLowerThan")]
        public void TestAtDebitAsTextSetterKalderHandleExceptionOnExceptionHandlerViewModelVedIntranetGuiValidationException(string illegalValue, string validationErrorText)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();

            decimal valueAsDecimal;
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var text = (Text) Enum.Parse(typeof (Text), validationErrorText);
                                                 var validationException = (IntranetGuiValidationException) e.Arguments.ElementAt(0);
                                                 Assert.That(validationException, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Empty);
                                                 switch (text)
                                                 {
                                                     case Text.DecimalLowerThan:
                                                         Assert.That(validationException.Message, Is.EqualTo(Resource.GetText(text, 0M)));
                                                         break;

                                                     default:
                                                         Assert.That(validationException.Message, Is.EqualTo(Resource.GetText(text)));
                                                         break;
                                                 }
                                                 Assert.That(validationException.ValidationContext, Is.Not.Null);
                                                 Assert.That(validationException.ValidationContext, Is.TypeOf<BogføringViewModel>());
                                                 Assert.That(validationException.PropertyName, Is.Not.Null);
                                                 Assert.That(validationException.PropertyName, Is.Not.Empty);
                                                 Assert.That(validationException.PropertyName, Is.EqualTo("DebitAsText"));
                                                 Assert.That(validationException.Value, Is.EqualTo(decimal.TryParse(illegalValue, NumberStyles.Any, new CultureInfo("en-US"), out valueAsDecimal) ? valueAsDecimal.ToString("C") : illegalValue));
                                                 Assert.That(validationException.InnerException, Is.Null);
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.DebitAsText = decimal.TryParse(illegalValue, NumberStyles.Any, new CultureInfo("en-US"), out valueAsDecimal) ? valueAsDecimal.ToString("C") : illegalValue;

            bogføringslinjeModelMock.AssertWasNotCalled(m => m.Debit = Arg<decimal>.Is.Anything);
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til DebitAsText kalder HandleException på exceptionhandleren med en IntranetGuiValidationException ved en ArgumentNullException.
        /// </summary>
        [Test]
        public void TestAtDebitAsTextSetterKalderHandleExceptionOnExceptionHandlerViewModelMedIntranetGuiValidationExceptionVedArgumentNullException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Debit = Arg<decimal>.Is.Anything)
                                    .Throw(fixture.Create<ArgumentNullException>())
                                    .Repeat.Any();

            var newValue = fixture.Create<decimal>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var validationException = (IntranetGuiValidationException) e.Arguments.ElementAt(0);
                                                 Assert.That(validationException, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Empty);
                                                 Assert.That(validationException.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingDebit)));
                                                 Assert.That(validationException.ValidationContext, Is.Not.Null);
                                                 Assert.That(validationException.ValidationContext, Is.TypeOf<BogføringViewModel>());
                                                 Assert.That(validationException.PropertyName, Is.Not.Null);
                                                 Assert.That(validationException.PropertyName, Is.Not.Empty);
                                                 Assert.That(validationException.PropertyName, Is.EqualTo("DebitAsText"));
                                                 Assert.That(validationException.Value, Is.EqualTo(newValue.ToString("C")));
                                                 Assert.That(validationException.InnerException, Is.Not.Null);
                                                 Assert.That(validationException.InnerException, Is.TypeOf<ArgumentNullException>());
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.DebitAsText = newValue.ToString("C");

            bogføringslinjeModelMock.AssertWasCalled(m => m.Debit = Arg<decimal>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til DebitAsText kalder HandleException på exceptionhandleren med en IntranetGuiValidationException ved en ArgumentException.
        /// </summary>
        [Test]
        public void TestAtDebitAsTextSetterKalderHandleExceptionOnExceptionHandlerViewModelMedIntranetGuiValidationExceptionVedArgumentException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Debit = Arg<decimal>.Is.Anything)
                                    .Throw(fixture.Create<ArgumentException>())
                                    .Repeat.Any();

            var newValue = fixture.Create<decimal>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var validationException = (IntranetGuiValidationException) e.Arguments.ElementAt(0);
                                                 Assert.That(validationException, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Empty);
                                                 Assert.That(validationException.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingDebit)));
                                                 Assert.That(validationException.ValidationContext, Is.Not.Null);
                                                 Assert.That(validationException.ValidationContext, Is.TypeOf<BogføringViewModel>());
                                                 Assert.That(validationException.PropertyName, Is.Not.Null);
                                                 Assert.That(validationException.PropertyName, Is.Not.Empty);
                                                 Assert.That(validationException.PropertyName, Is.EqualTo("DebitAsText"));
                                                 Assert.That(validationException.Value, Is.EqualTo(newValue.ToString("C")));
                                                 Assert.That(validationException.InnerException, Is.Not.Null);
                                                 Assert.That(validationException.InnerException, Is.TypeOf<ArgumentException>());
                                             })
                                             .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.DebitAsText = newValue.ToString("C");

            bogføringslinjeModelMock.AssertWasCalled(m => m.Debit = Arg<decimal>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til DebitAsText kalder HandleException på exceptionhandleren ved IntranetGuiRepositoryException.
        /// </summary>
        [Test]
        public void TestAtDebitAsTextSetterKalderHandleExceptionOnExceptionHandlerViewModelVedIntranetGuiRepositoryException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = fixture.Create<IntranetGuiRepositoryException>();
            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Debit = Arg<decimal>.Is.Anything)
                                    .Throw(exception)
                                    .Repeat.Any();

            var newValue = fixture.Create<decimal>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiRepositoryException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var repositoryException = (IntranetGuiRepositoryException) e.Arguments.ElementAt(0);
                                                 Assert.That(repositoryException, Is.Not.Null);
                                                 Assert.That(repositoryException.Message, Is.Not.Null);
                                                 Assert.That(repositoryException.Message, Is.Not.Empty);
                                                 Assert.That(repositoryException.Message, Is.EqualTo(exception.Message));
                                                 Assert.That(repositoryException.InnerException, Is.Null);
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.DebitAsText = newValue.ToString("C");

            bogføringslinjeModelMock.AssertWasCalled(m => m.Debit = Arg<decimal>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiRepositoryException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til DebitAsText kalder HandleException på exceptionhandleren ved IntranetGuiBusinessException.
        /// </summary>
        [Test]
        public void TestAtDebitAsTextSetterKalderHandleExceptionOnExceptionHandlerViewModelVedIntranetGuiBusinessException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = fixture.Create<IntranetGuiBusinessException>();
            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Debit = Arg<decimal>.Is.Anything)
                                    .Throw(exception)
                                    .Repeat.Any();

            var newValue = fixture.Create<decimal>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiBusinessException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var businessException = (IntranetGuiBusinessException) e.Arguments.ElementAt(0);
                                                 Assert.That(businessException, Is.Not.Null);
                                                 Assert.That(businessException.Message, Is.Not.Null);
                                                 Assert.That(businessException.Message, Is.Not.Empty);
                                                 Assert.That(businessException.Message, Is.EqualTo(exception.Message));
                                                 Assert.That(businessException.InnerException, Is.Null);
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.DebitAsText = newValue.ToString("C");

            bogføringslinjeModelMock.AssertWasCalled(m => m.Debit = Arg<decimal>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiBusinessException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til DebitAsText kalder HandleException på exceptionhandleren ved IntranetGuiSystemException.
        /// </summary>
        [Test]
        public void TestAtDebitAsTextSetterKalderHandleExceptionOnExceptionHandlerViewModelVedIntranetGuiSystemException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = fixture.Create<IntranetGuiSystemException>();
            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Debit = Arg<decimal>.Is.Anything)
                                    .Throw(exception)
                                    .Repeat.Any();

            var newValue = fixture.Create<decimal>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var systemException = (IntranetGuiSystemException) e.Arguments.ElementAt(0);
                                                 Assert.That(systemException, Is.Not.Null);
                                                 Assert.That(systemException.Message, Is.Not.Null);
                                                 Assert.That(systemException.Message, Is.Not.Empty);
                                                 Assert.That(systemException.Message, Is.EqualTo(exception.Message));
                                                 Assert.That(systemException.InnerException, Is.Null);
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.DebitAsText = newValue.ToString("C");

            bogføringslinjeModelMock.AssertWasCalled(m => m.Debit = Arg<decimal>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til DebitAsText kalder HandleException på exceptionhandleren ved Exception.
        /// </summary>
        [Test]
        public void TestAtDebitAsTextSetterKalderHandleExceptionOnExceptionHandlerViewModelVedException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = fixture.Create<Exception>();
            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Debit = Arg<decimal>.Is.Anything)
                                    .Throw(exception)
                                    .Repeat.Any();

            var newValue = fixture.Create<decimal>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var systemException = (IntranetGuiSystemException) e.Arguments.ElementAt(0);
                                                 Assert.That(systemException, Is.Not.Null);
                                                 Assert.That(systemException.Message, Is.Not.Null);
                                                 Assert.That(systemException.Message, Is.Not.Empty);
                                                 Assert.That(systemException.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingPropertyValue, "DebitAsText", exception.Message)));
                                                 Assert.That(systemException.InnerException, Is.Not.Null);
                                                 Assert.That(systemException.InnerException, Is.EqualTo(exception));
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.DebitAsText = newValue.ToString("C");

            bogføringslinjeModelMock.AssertWasCalled(m => m.Debit = Arg<decimal>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til KreditAsText opdaterer Kredit på modellen for bogføringslinjen.
        /// </summary>
        [Test]
        [TestCase(null, 0)]
        [TestCase("", 0)]
        [TestCase(" ", 0)]
        [TestCase("1,000.00", 1000.00)]
        [TestCase("2,000.00", 2000.00)]
        [TestCase("3,000.00", 3000.00)]
        [TestCase("4,000.00", 4000.00)]
        [TestCase("$ 1,000.00", 1000.00)]
        [TestCase("$ 2,000.00", 2000.00)]
        [TestCase("$ 3,000.00", 3000.00)]
        [TestCase("$ 4,000.00", 4000.00)]
        public void TestAtKreditAsTextSetterOpdatererKreditOnBogføringslinjeModel(string value, decimal expectedValue)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.KreditAsText = string.IsNullOrWhiteSpace(value) ? value : decimal.Parse(value, NumberStyles.Any, new CultureInfo("en-US")).ToString("C");

            bogføringslinjeModelMock.AssertWasCalled(m => m.Kredit = Arg<decimal>.Is.Equal(expectedValue));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at sætteren til KreditAsText kalder HandleExcetion på exceptionhandleren ved valideringsfejl.
        /// </summary>
        [Test]
        [TestCase("XYZ", "ValueIsNotDecimal")]
        [TestCase("ZYX", "ValueIsNotDecimal")]
        [TestCase("$ -0.01", "DecimalLowerThan")]
        [TestCase("$ -1000.00", "DecimalLowerThan")]
        [TestCase("$ -2000.00", "DecimalLowerThan")]
        [TestCase("$ -3000.00", "DecimalLowerThan")]
        [TestCase("$ -4000.00", "DecimalLowerThan")]
        public void TestAtKreditAsTextSetterKalderHandleExceptionOnExceptionHandlerViewModelVedIntranetGuiValidationException(string illegalValue, string validationErrorText)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();

            decimal valueAsDecimal;
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var text = (Text) Enum.Parse(typeof (Text), validationErrorText);
                                                 var validationException = (IntranetGuiValidationException) e.Arguments.ElementAt(0);
                                                 Assert.That(validationException, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Empty);
                                                 switch (text)
                                                 {
                                                     case Text.DecimalLowerThan:
                                                         Assert.That(validationException.Message, Is.EqualTo(Resource.GetText(text, 0M)));
                                                         break;

                                                     default:
                                                         Assert.That(validationException.Message, Is.EqualTo(Resource.GetText(text)));
                                                         break;
                                                 }
                                                 Assert.That(validationException.ValidationContext, Is.Not.Null);
                                                 Assert.That(validationException.ValidationContext, Is.TypeOf<BogføringViewModel>());
                                                 Assert.That(validationException.PropertyName, Is.Not.Null);
                                                 Assert.That(validationException.PropertyName, Is.Not.Empty);
                                                 Assert.That(validationException.PropertyName, Is.EqualTo("KreditAsText"));
                                                 Assert.That(validationException.Value, Is.EqualTo(decimal.TryParse(illegalValue, NumberStyles.Any, new CultureInfo("en-US"), out valueAsDecimal) ? valueAsDecimal.ToString("C") : illegalValue));
                                                 Assert.That(validationException.InnerException, Is.Null);
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.KreditAsText = decimal.TryParse(illegalValue, NumberStyles.Any, new CultureInfo("en-US"), out valueAsDecimal) ? valueAsDecimal.ToString("C") : illegalValue;

            bogføringslinjeModelMock.AssertWasNotCalled(m => m.Kredit = Arg<decimal>.Is.Anything);
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til KreditAsText kalder HandleException på exceptionhandleren med en IntranetGuiValidationException ved en ArgumentNullException.
        /// </summary>
        [Test]
        public void TestAtKreditAsTextSetterKalderHandleExceptionOnExceptionHandlerViewModelMedIntranetGuiValidationExceptionVedArgumentNullException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kredit = Arg<decimal>.Is.Anything)
                                    .Throw(fixture.Create<ArgumentNullException>())
                                    .Repeat.Any();

            var newValue = fixture.Create<decimal>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var validationException = (IntranetGuiValidationException) e.Arguments.ElementAt(0);
                                                 Assert.That(validationException, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Empty);
                                                 Assert.That(validationException.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingCredit)));
                                                 Assert.That(validationException.ValidationContext, Is.Not.Null);
                                                 Assert.That(validationException.ValidationContext, Is.TypeOf<BogføringViewModel>());
                                                 Assert.That(validationException.PropertyName, Is.Not.Null);
                                                 Assert.That(validationException.PropertyName, Is.Not.Empty);
                                                 Assert.That(validationException.PropertyName, Is.EqualTo("KreditAsText"));
                                                 Assert.That(validationException.Value, Is.EqualTo(newValue.ToString("C")));
                                                 Assert.That(validationException.InnerException, Is.Not.Null);
                                                 Assert.That(validationException.InnerException, Is.TypeOf<ArgumentNullException>());
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.KreditAsText = newValue.ToString("C");

            bogføringslinjeModelMock.AssertWasCalled(m => m.Kredit = Arg<decimal>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til KreditAsText kalder HandleException på exceptionhandleren med en IntranetGuiValidationException ved en ArgumentException.
        /// </summary>
        [Test]
        public void TestAtKreditAsTextSetterKalderHandleExceptionOnExceptionHandlerViewModelMedIntranetGuiValidationExceptionVedArgumentException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kredit = Arg<decimal>.Is.Anything)
                                    .Throw(fixture.Create<ArgumentException>())
                                    .Repeat.Any();

            var newValue = fixture.Create<decimal>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var validationException = (IntranetGuiValidationException) e.Arguments.ElementAt(0);
                                                 Assert.That(validationException, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Empty);
                                                 Assert.That(validationException.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingCredit)));
                                                 Assert.That(validationException.ValidationContext, Is.Not.Null);
                                                 Assert.That(validationException.ValidationContext, Is.TypeOf<BogføringViewModel>());
                                                 Assert.That(validationException.PropertyName, Is.Not.Null);
                                                 Assert.That(validationException.PropertyName, Is.Not.Empty);
                                                 Assert.That(validationException.PropertyName, Is.EqualTo("KreditAsText"));
                                                 Assert.That(validationException.Value, Is.EqualTo(newValue.ToString("C")));
                                                 Assert.That(validationException.InnerException, Is.Not.Null);
                                                 Assert.That(validationException.InnerException, Is.TypeOf<ArgumentException>());
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.KreditAsText = newValue.ToString("C");

            bogføringslinjeModelMock.AssertWasCalled(m => m.Kredit = Arg<decimal>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til KreditAsText kalder HandleException på exceptionhandleren ved IntranetGuiRepositoryException.
        /// </summary>
        [Test]
        public void TestAtKreditAsTextSetterKalderHandleExceptionOnExceptionHandlerViewModelVedIntranetGuiRepositoryException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = fixture.Create<IntranetGuiRepositoryException>();
            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kredit = Arg<decimal>.Is.Anything)
                                    .Throw(exception)
                                    .Repeat.Any();

            var newValue = fixture.Create<decimal>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiRepositoryException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var repositoryException = (IntranetGuiRepositoryException) e.Arguments.ElementAt(0);
                                                 Assert.That(repositoryException, Is.Not.Null);
                                                 Assert.That(repositoryException.Message, Is.Not.Null);
                                                 Assert.That(repositoryException.Message, Is.Not.Empty);
                                                 Assert.That(repositoryException.Message, Is.EqualTo(exception.Message));
                                                 Assert.That(repositoryException.InnerException, Is.Null);
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.KreditAsText = newValue.ToString("C");

            bogføringslinjeModelMock.AssertWasCalled(m => m.Kredit = Arg<decimal>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiRepositoryException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til KreditAsText kalder HandleException på exceptionhandleren ved IntranetGuiBusinessException.
        /// </summary>
        [Test]
        public void TestAtKreditAsTextSetterKalderHandleExceptionOnExceptionHandlerViewModelVedIntranetGuiBusinessException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = fixture.Create<IntranetGuiBusinessException>();
            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kredit = Arg<decimal>.Is.Anything)
                                    .Throw(exception)
                                    .Repeat.Any();

            var newValue = fixture.Create<decimal>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiBusinessException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var businessException = (IntranetGuiBusinessException) e.Arguments.ElementAt(0);
                                                 Assert.That(businessException, Is.Not.Null);
                                                 Assert.That(businessException.Message, Is.Not.Null);
                                                 Assert.That(businessException.Message, Is.Not.Empty);
                                                 Assert.That(businessException.Message, Is.EqualTo(exception.Message));
                                                 Assert.That(businessException.InnerException, Is.Null);
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.KreditAsText = newValue.ToString("C");

            bogføringslinjeModelMock.AssertWasCalled(m => m.Kredit = Arg<decimal>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiBusinessException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til KreditAsText kalder HandleException på exceptionhandleren ved IntranetGuiSystemException.
        /// </summary>
        [Test]
        public void TestAtKreditAsTextSetterKalderHandleExceptionOnExceptionHandlerViewModelVedIntranetGuiSystemException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = fixture.Create<IntranetGuiSystemException>();
            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kredit = Arg<decimal>.Is.Anything)
                                    .Throw(exception)
                                    .Repeat.Any();

            var newValue = fixture.Create<decimal>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var systemException = (IntranetGuiSystemException) e.Arguments.ElementAt(0);
                                                 Assert.That(systemException, Is.Not.Null);
                                                 Assert.That(systemException.Message, Is.Not.Null);
                                                 Assert.That(systemException.Message, Is.Not.Empty);
                                                 Assert.That(systemException.Message, Is.EqualTo(exception.Message));
                                                 Assert.That(systemException.InnerException, Is.Null);
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.KreditAsText = newValue.ToString("C");

            bogføringslinjeModelMock.AssertWasCalled(m => m.Kredit = Arg<decimal>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til KreditAsText kalder HandleException på exceptionhandleren ved Exception.
        /// </summary>
        [Test]
        public void TestAtKreditAsTextSetterKalderHandleExceptionOnExceptionHandlerViewModelVedException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = fixture.Create<Exception>();
            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kredit = Arg<decimal>.Is.Anything)
                                    .Throw(exception)
                                    .Repeat.Any();

            var newValue = fixture.Create<decimal>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var systemException = (IntranetGuiSystemException) e.Arguments.ElementAt(0);
                                                 Assert.That(systemException, Is.Not.Null);
                                                 Assert.That(systemException.Message, Is.Not.Null);
                                                 Assert.That(systemException.Message, Is.Not.Empty);
                                                 Assert.That(systemException.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingPropertyValue, "KreditAsText", exception.Message)));
                                                 Assert.That(systemException.InnerException, Is.Not.Null);
                                                 Assert.That(systemException.InnerException, Is.EqualTo(exception));
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.KreditAsText = newValue.ToString("C");

            bogføringslinjeModelMock.AssertWasCalled(m => m.Kredit = Arg<decimal>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til Adressekonto opdaterer Adressekonto på modellen for bogføringslinjen.
        /// </summary>
        [Test]
        public void TestAtAdressekontoSetterOpdatererAdressekontoOnBogføringslinjeModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            var newValue = fixture.Create<int>();
            bogføringViewModel.Adressekonto = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Adressekonto = Arg<int>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at sætteren til Adressekonto kalder HandleException på exceptionhandleren med en IntranetGuiValidationException ved en ArgumentNullException.
        /// </summary>
        [Test]
        public void TestAtAdressekontoSetterKalderHandleExceptionOnExceptionHandlerViewModelMedIntranetGuiValidationExceptionVedArgumentNullException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto = Arg<int>.Is.Anything)
                                    .Throw(fixture.Create<ArgumentNullException>())
                                    .Repeat.Any();

            var newValue = fixture.Create<int>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var validationException = (IntranetGuiValidationException) e.Arguments.ElementAt(0);
                                                 Assert.That(validationException, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Empty);
                                                 Assert.That(validationException.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingAddressAccount)));
                                                 Assert.That(validationException.ValidationContext, Is.Not.Null);
                                                 Assert.That(validationException.ValidationContext, Is.TypeOf<BogføringViewModel>());
                                                 Assert.That(validationException.PropertyName, Is.Not.Null);
                                                 Assert.That(validationException.PropertyName, Is.Not.Empty);
                                                 Assert.That(validationException.PropertyName, Is.EqualTo("Adressekonto"));
                                                 Assert.That(validationException.Value, Is.EqualTo(newValue));
                                                 Assert.That(validationException.InnerException, Is.Not.Null);
                                                 Assert.That(validationException.InnerException, Is.TypeOf<ArgumentNullException>());
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.Adressekonto = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Adressekonto = Arg<int>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til Adressekonto kalder HandleException på exceptionhandleren med en IntranetGuiValidationException ved en ArgumentException.
        /// </summary>
        [Test]
        public void TestAtAdressekontoSetterKalderHandleExceptionOnExceptionHandlerViewModelMedIntranetGuiValidationExceptionVedArgumentException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto = Arg<int>.Is.Anything)
                                    .Throw(fixture.Create<ArgumentException>())
                                    .Repeat.Any();

            var newValue = fixture.Create<int>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var validationException = (IntranetGuiValidationException) e.Arguments.ElementAt(0);
                                                 Assert.That(validationException, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Empty);
                                                 Assert.That(validationException.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingAddressAccount)));
                                                 Assert.That(validationException.ValidationContext, Is.Not.Null);
                                                 Assert.That(validationException.ValidationContext, Is.TypeOf<BogføringViewModel>());
                                                 Assert.That(validationException.PropertyName, Is.Not.Null);
                                                 Assert.That(validationException.PropertyName, Is.Not.Empty);
                                                 Assert.That(validationException.PropertyName, Is.EqualTo("Adressekonto"));
                                                 Assert.That(validationException.Value, Is.EqualTo(newValue));
                                                 Assert.That(validationException.InnerException, Is.Not.Null);
                                                 Assert.That(validationException.InnerException, Is.TypeOf<ArgumentException>());
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.Adressekonto = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Adressekonto = Arg<int>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiValidationException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til Adressekonto kalder HandleException på exceptionhandleren ved IntranetGuiRepositoryException.
        /// </summary>
        [Test]
        public void TestAtAdressekontoSetterKalderHandleExceptionOnExceptionHandlerViewModelVedIntranetGuiRepositoryException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = fixture.Create<IntranetGuiRepositoryException>();
            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto = Arg<int>.Is.Anything)
                                    .Throw(exception)
                                    .Repeat.Any();

            var newValue = fixture.Create<int>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiRepositoryException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var repositoryException = (IntranetGuiRepositoryException) e.Arguments.ElementAt(0);
                                                 Assert.That(repositoryException, Is.Not.Null);
                                                 Assert.That(repositoryException.Message, Is.Not.Null);
                                                 Assert.That(repositoryException.Message, Is.Not.Empty);
                                                 Assert.That(repositoryException.Message, Is.EqualTo(exception.Message));
                                                 Assert.That(repositoryException.InnerException, Is.Null);
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.Adressekonto = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Adressekonto = Arg<int>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiRepositoryException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til Adressekonto kalder HandleException på exceptionhandleren ved IntranetGuiBusinessException.
        /// </summary>
        [Test]
        public void TestAtAdressekontoSetterKalderHandleExceptionOnExceptionHandlerViewModelVedIntranetGuiBusinessException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = fixture.Create<IntranetGuiBusinessException>();
            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto = Arg<int>.Is.Anything)
                                    .Throw(exception)
                                    .Repeat.Any();

            var newValue = fixture.Create<int>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiBusinessException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var businessException = (IntranetGuiBusinessException) e.Arguments.ElementAt(0);
                                                 Assert.That(businessException, Is.Not.Null);
                                                 Assert.That(businessException.Message, Is.Not.Null);
                                                 Assert.That(businessException.Message, Is.Not.Empty);
                                                 Assert.That(businessException.Message, Is.EqualTo(exception.Message));
                                                 Assert.That(businessException.InnerException, Is.Null);
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.Adressekonto = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Adressekonto = Arg<int>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiBusinessException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til Adressekonto kalder HandleException på exceptionhandleren ved IntranetGuiSystemException.
        /// </summary>
        [Test]
        public void TestAtAdressekontoSetterKalderHandleExceptionOnExceptionHandlerViewModelVedIntranetGuiSystemException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = fixture.Create<IntranetGuiSystemException>();
            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto = Arg<int>.Is.Anything)
                                    .Throw(exception)
                                    .Repeat.Any();

            var newValue = fixture.Create<int>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var systemException = (IntranetGuiSystemException) e.Arguments.ElementAt(0);
                                                 Assert.That(systemException, Is.Not.Null);
                                                 Assert.That(systemException.Message, Is.Not.Null);
                                                 Assert.That(systemException.Message, Is.Not.Empty);
                                                 Assert.That(systemException.Message, Is.EqualTo(exception.Message));
                                                 Assert.That(systemException.InnerException, Is.Null);
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.Adressekonto = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Adressekonto = Arg<int>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at sætteren til Adressekonto kalder HandleException på exceptionhandleren ved Exception.
        /// </summary>
        [Test]
        public void TestAtAdressekontoSetterKalderHandleExceptionOnExceptionHandlerViewModelVedException()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = fixture.Create<Exception>();
            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto = Arg<int>.Is.Anything)
                                    .Throw(exception)
                                    .Repeat.Any();

            var newValue = fixture.Create<int>();
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();
            exceptionHandlerViewModelMock.Expect(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf))
                                         .WhenCalled(e =>
                                             {
                                                 var systemException = (IntranetGuiSystemException) e.Arguments.ElementAt(0);
                                                 Assert.That(systemException, Is.Not.Null);
                                                 Assert.That(systemException.Message, Is.Not.Null);
                                                 Assert.That(systemException.Message, Is.Not.Empty);
                                                 Assert.That(systemException.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingPropertyValue, "Adressekonto", exception.Message)));
                                                 Assert.That(systemException.InnerException, Is.Not.Null);
                                                 Assert.That(systemException.InnerException, Is.EqualTo(exception));
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.Adressekonto = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Adressekonto = Arg<int>.Is.Equal(newValue));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBogføringslinjeModelEventHandler rejser PropertyChanged, når modellen for bogføringslinjen opdateres.
        /// </summary>
        [Test]
        [TestCase("Dato", "Dato")]
        [TestCase("Dato", "DatoAsText")]
        [TestCase("Dato", "Kontonavn")]
        [TestCase("Dato", "KontoSaldo")]
        [TestCase("Dato", "KontoSaldoAsText")]
        [TestCase("Dato", "KontoDisponibel")]
        [TestCase("Dato", "KontoDisponibelAsText")]
        [TestCase("Dato", "Budgetkontonavn")]
        [TestCase("Dato", "BudgetkontoBogført")]
        [TestCase("Dato", "BudgetkontoBogførtAsText")]
        [TestCase("Dato", "BudgetkontoDisponibel")]
        [TestCase("Dato", "BudgetkontoDisponibelAsText")]
        [TestCase("Dato", "AdressekontoNavn")]
        [TestCase("Dato", "AdressekontoSaldo")]
        [TestCase("Dato", "AdressekontoSaldoAsText")]
        [TestCase("Bilag", "Bilag")]
        [TestCase("Kontonummer", "Kontonummer")]
        [TestCase("Kontonummer", "Kontonavn")]
        [TestCase("Kontonummer", "KontoSaldo")]
        [TestCase("Kontonummer", "KontoSaldoAsText")]
        [TestCase("Kontonummer", "KontoDisponibel")]
        [TestCase("Kontonummer", "KontoDisponibelAsText")]
        [TestCase("Tekst", "Tekst")]
        [TestCase("Budgetkontonummer", "Budgetkontonummer")]
        [TestCase("Budgetkontonummer", "Budgetkontonavn")]
        [TestCase("Budgetkontonummer", "BudgetkontoBogført")]
        [TestCase("Budgetkontonummer", "BudgetkontoBogførtAsText")]
        [TestCase("Budgetkontonummer", "BudgetkontoDisponibel")]
        [TestCase("Budgetkontonummer", "BudgetkontoDisponibelAsText")]
        [TestCase("Debit", "Debit")]
        [TestCase("Debit", "DebitAsText")]
        [TestCase("Kredit", "Kredit")]
        [TestCase("Kredit", "KreditAsText")]
        [TestCase("Adressekonto", "Adressekonto")]
        [TestCase("Adressekonto", "AdressekontoNavn")]
        [TestCase("Adressekonto", "AdressekontoSaldo")]
        [TestCase("Adressekonto", "AdressekontoSaldoAsText")]
        public void TestAtPropertyChangedOnBogføringslinjeModelEventHandlerRejserPropertyChangedOnBogføringslinjeModelUpdate(string propertyNameToRaise, string expectPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() =>
                {
                    var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
                    regnskabViewModelMock.Expect(m => m.Nummer)
                                         .Return(fixture.Create<int>())
                                         .Repeat.Any();
                    return regnskabViewModelMock;
                }));

            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();

            Func<IEnumerable<IAdressekontoModel>> adressekontoModelCollectionGetter = () => new List<IAdressekontoModel>(0);
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.AdressekontolisteGetAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue)))
                                       .Return(Task.Run(adressekontoModelCollectionGetter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, finansstyringRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);
            Assert.That(bogføringViewModel.Tasks, Is.Not.Null);
            Assert.That(bogføringViewModel.Tasks, Is.Empty);

            var eventCalled = false;
            bogføringViewModel.PropertyChanged += (s, e) =>
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
            Action action = () =>
                {
                    bogføringslinjeModelMock.Raise(m => m.PropertyChanged += null, bogføringslinjeModelMock, new PropertyChangedEventArgs(propertyNameToRaise));
                    var tasks = bogføringViewModel.Tasks.ToArray();
                    Task.WaitAll(tasks);
                };
            Task.Run(action).Wait(3000);
            Assert.That(eventCalled, Is.True);

            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBogføringslinjeModelEventHandler kaster en ArgumentNullException, hvis objektet, der rejser eventet, er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnBogføringslinjeModelEventHandlerKasterArgumentNullExceptionHvisSenderErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => bogføringslinjeModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("sender"));
            Assert.That(exception.InnerException, Is.Null);

            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBogføringslinjeModelEventHandler kaster en ArgumentNullException, hvis argumenter til eventet er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnBogføringslinjeModelEventHandlerKasterArgumentNullExceptionHvisEventArgsErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(fixture.Create<DateTime>())
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => bogføringslinjeModelMock.Raise(m => m.PropertyChanged += null, bogføringslinjeModelMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("eventArgs"));
            Assert.That(exception.InnerException, Is.Null);

            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBogføringslinjeModelEventHandler reloader information, hvis Dato opdateres på modellen for bogføringslinjen.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnBogføringslinjeModelEventHandlerReloaderInformationHvisDatoOpdateresOnBogføringslinjeModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IKontogruppeModel>(e => e.FromFactory(() =>
                {
                    var kontogruppeModelMock = MockRepository.GenerateMock<IKontogruppeModel>();
                    kontogruppeModelMock.Expect(m => m.Nummer)
                                        .Return(fixture.Create<int>())
                                        .Repeat.Any();
                    return kontogruppeModelMock;
                }));
            fixture.Customize<IBudgetkontogruppeModel>(e => e.FromFactory(() =>
                {
                    var budgetkontogruppeModel = MockRepository.GenerateMock<IBudgetkontogruppeModel>();
                    budgetkontogruppeModel.Expect(m => m.Nummer)
                                          .Return(fixture.Create<int>())
                                          .Repeat.Any();
                    return budgetkontogruppeModel;
                }));

            var regnskabsnummer = fixture.Create<int>();
            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(regnskabsnummer)
                                 .Repeat.Any();

            var dato = fixture.Create<DateTime>();
            var kontonummer = fixture.Create<string>();
            var budgetkontonummer = fixture.Create<string>();
            var adressekonto = fixture.Create<int>();
            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(dato)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(kontonummer)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(budgetkontonummer)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(adressekonto)
                                    .Repeat.Any();

            var random = new Random(DateTime.Now.Millisecond);
            var kontogruppeModelMockCollection = fixture.CreateMany<IKontogruppeModel>(7).ToList();
            var kontoModelMock = MockRepository.GenerateMock<IKontoModel>();
            kontoModelMock.Expect(m => m.Kontonavn)
                          .Return(fixture.Create<string>())
                          .Repeat.Any();
            kontoModelMock.Expect(m => m.Kontogruppe)
                          .Return(kontogruppeModelMockCollection.ElementAt(random.Next(0, kontogruppeModelMockCollection.Count - 1)).Nummer)
                          .Repeat.Any();
            kontoModelMock.Expect(m => m.Saldo)
                          .Return(fixture.Create<decimal>())
                          .Repeat.Any();
            kontoModelMock.Expect(m => m.Disponibel)
                          .Return(fixture.Create<decimal>())
                          .Repeat.Any();

            var budgetkontogruppeModelMockCollection = fixture.CreateMany<IBudgetkontogruppeModel>(7).ToList();
            var budgetkontoModelMock = MockRepository.GenerateMock<IBudgetkontoModel>();
            budgetkontoModelMock.Expect(m => m.Kontonavn)
                                .Return(fixture.Create<string>())
                                .Repeat.Any();
            budgetkontoModelMock.Expect(m => m.Kontogruppe)
                                .Return(budgetkontogruppeModelMockCollection.ElementAt(random.Next(0, budgetkontogruppeModelMockCollection.Count - 1)).Nummer)
                                .Repeat.Any();
            budgetkontoModelMock.Expect(m => m.Bogført)
                                .Return(fixture.Create<decimal>())
                                .Repeat.Any();
            budgetkontoModelMock.Expect(m => m.Disponibel)
                                .Return(fixture.Create<decimal>())
                                .Repeat.Any();

            var adressekontoModelMock = MockRepository.GenerateMock<IAdressekontoModel>();
            adressekontoModelMock.Expect(m => m.Nummer)
                                 .Return(adressekonto)
                                 .Repeat.Any();
            adressekontoModelMock.Expect(m => m.Navn)
                                 .Return(fixture.Create<string>())
                                 .Repeat.Any();
            adressekontoModelMock.Expect(m => m.Saldo)
                                 .Return(fixture.Create<decimal>())
                                 .Repeat.Any();

            Func<IKontoModel> kontoModelGetter = () => kontoModelMock;
            Func<IBudgetkontoModel> budgetkontoModelGetter = () => budgetkontoModelMock;
            Func<IEnumerable<IAdressekontoModel>> adressekontoModelCollectionGetter = () => new Collection<IAdressekontoModel> {adressekontoModelMock};
            Func<IEnumerable<IKontogruppeModel>> kontogruppeModelCollectionGetter = () => kontogruppeModelMockCollection;
            Func<IEnumerable<IBudgetkontogruppeModel>> budgetkontogruppeModelCollectionGetter = () => budgetkontogruppeModelMockCollection;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.KontoGetAsync(Arg<int>.Is.GreaterThan(0), Arg<string>.Is.NotNull, Arg<DateTime>.Is.GreaterThan(DateTime.MinValue)))
                                       .Return(Task.Run(kontoModelGetter))
                                       .Repeat.Any();
            finansstyringRepositoryMock.Expect(m => m.BudgetkontoGetAsync(Arg<int>.Is.GreaterThan(0), Arg<string>.Is.NotNull, Arg<DateTime>.Is.GreaterThan(DateTime.MinValue)))
                                       .Return(Task.Run(budgetkontoModelGetter))
                                       .Repeat.Any();
            finansstyringRepositoryMock.Expect(m => m.AdressekontolisteGetAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue)))
                                       .Return(Task.Run(adressekontoModelCollectionGetter))
                                       .Repeat.Any();
            finansstyringRepositoryMock.Expect(m => m.KontogruppelisteGetAsync())
                                       .Return(Task.Run(kontogruppeModelCollectionGetter))
                                       .Repeat.Any();
            finansstyringRepositoryMock.Expect(m => m.BudgetkontogruppelisteGetAsync())
                                       .Return(Task.Run(budgetkontogruppeModelCollectionGetter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var expectedNotifyPropertyChanged = new List<string>
                {
                    "DatoAsTextIsReadOnly",
                    "KontonummerIsReadOnly",
                    "Kontonavn",
                    "KontoSaldo",
                    "KontoSaldoAsText",
                    "KontoDisponibel",
                    "KontoDisponibelAsText",
                    "BudgetkontonummerIsReadOnly",
                    "Budgetkontonavn",
                    "BudgetkontoBogført",
                    "BudgetkontoBogførtAsText",
                    "BudgetkontoDisponibel",
                    "BudgetkontoDisponibelAsText",
                    "AdressekontoIsReadOnly",
                    "AdressekontoNavn",
                    "AdressekontoSaldo",
                    "AdressekontoSaldoAsText",
                    "Adressekonti",
                    "Tasks",
                    "IsWorking"
                };
            Assert.That(expectedNotifyPropertyChanged, Is.Not.Null);
            Assert.That(expectedNotifyPropertyChanged, Is.Not.Empty);

            var bogføringViewModel = new BogføringViewModel(regnskabViewModelMock, bogføringslinjeModelMock, finansstyringRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel.Tasks, Is.Not.Null);
            Assert.That(bogføringViewModel.Tasks, Is.Empty);
            Assert.That(bogføringViewModel.IsWorking, Is.False);

            bogføringViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(s, Is.TypeOf<BogføringViewModel>());
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    var viewModel = (BogføringViewModel) s;
                    switch (e.PropertyName)
                    {
                        case "Kontonavn":
                        case "KontoSaldo":
                        case "KontoSaldoAsText":
                        case "KontoDisponibel":
                        case "KontoDisponibelAsText":
                            if (string.IsNullOrEmpty(viewModel.Kontonavn))
                            {
                                return;
                            }
                            while (expectedNotifyPropertyChanged.Contains(e.PropertyName))
                            {
                                expectedNotifyPropertyChanged.Remove(e.PropertyName);
                            }
                            break;

                        case "Budgetkontonavn":
                        case "BudgetkontoBogført":
                        case "BudgetkontoBogførtAsText":
                        case "BudgetkontoDisponibel":
                        case "BudgetkontoDisponibelAsText":
                            if (string.IsNullOrEmpty(viewModel.Budgetkontonavn))
                            {
                                return;
                            }
                            while (expectedNotifyPropertyChanged.Contains(e.PropertyName))
                            {
                                expectedNotifyPropertyChanged.Remove(e.PropertyName);
                            }
                            break;


                        case "AdressekontoNavn":
                        case "AdressekontoSaldo":
                        case "AdressekontoSaldoAsText":
                            if (string.IsNullOrEmpty(viewModel.AdressekontoNavn))
                            {
                                return;
                            }
                            while (expectedNotifyPropertyChanged.Contains(e.PropertyName))
                            {
                                expectedNotifyPropertyChanged.Remove(e.PropertyName);
                            }
                            break;

                        default:
                            while (expectedNotifyPropertyChanged.Contains(e.PropertyName))
                            {
                                expectedNotifyPropertyChanged.Remove(e.PropertyName);
                            }
                            break;
                    }
                };

            Action action = () =>
                {
                    using (var waitEvent = new AutoResetEvent(false))
                    {
                        var we = waitEvent;
                        bogføringViewModel.PropertyChanged += (s, e) =>
                            {
                                Assert.That(s, Is.Not.Null);
                                Assert.That(s, Is.Not.Null);
                                Assert.That(s, Is.TypeOf<BogføringViewModel>());
                                Assert.That(e, Is.Not.Null);
                                Assert.That(e.PropertyName, Is.Not.Null);
                                Assert.That(e.PropertyName, Is.Not.Empty);
                                var viewModel = (BogføringViewModel) s;
                                switch (e.PropertyName)
                                {
                                    case "IsWorking":
                                        if (viewModel.IsWorking)
                                        {
                                            return;
                                        }
                                        we.Set();
                                        break;
                                }
                            };
                        bogføringslinjeModelMock.Raise(m => m.PropertyChanged += null, bogføringslinjeModelMock, new PropertyChangedEventArgs("Dato"));

                        var tasks = bogføringViewModel.Tasks.ToArray();
                        Assert.That(tasks, Is.Not.Null);
                        Assert.That(tasks, Is.Not.Empty);
                        Task.WaitAll(tasks);

                        waitEvent.WaitOne(2500);
                    }
                };
            Task.Run(action).Wait(3000);

            Assert.That(bogføringViewModel.Kontonavn, Is.Not.Null);
            Assert.That(bogføringViewModel.Kontonavn, Is.Not.Empty);
            Assert.That(bogføringViewModel.Kontonavn, Is.EqualTo(kontoModelMock.Kontonavn));
            Assert.That(bogføringViewModel.KontoSaldo, Is.EqualTo(kontoModelMock.Saldo));
            Assert.That(bogføringViewModel.KontoSaldoAsText, Is.Not.Null);
            Assert.That(bogføringViewModel.KontoSaldoAsText, Is.Not.Empty);
            Assert.That(bogføringViewModel.KontoSaldoAsText, Is.EqualTo(kontoModelMock.Saldo.ToString("C")));
            Assert.That(bogføringViewModel.KontoDisponibel, Is.EqualTo(kontoModelMock.Disponibel));
            Assert.That(bogføringViewModel.KontoDisponibelAsText, Is.Not.Null);
            Assert.That(bogføringViewModel.KontoDisponibelAsText, Is.Not.Empty);
            Assert.That(bogføringViewModel.KontoDisponibelAsText, Is.EqualTo(kontoModelMock.Disponibel.ToString("C")));

            Assert.That(bogføringViewModel.Budgetkontonavn, Is.Not.Null);
            Assert.That(bogføringViewModel.Budgetkontonavn, Is.Not.Empty);
            Assert.That(bogføringViewModel.Budgetkontonavn, Is.EqualTo(budgetkontoModelMock.Kontonavn));
            Assert.That(bogføringViewModel.BudgetkontoBogført, Is.EqualTo(budgetkontoModelMock.Bogført));
            Assert.That(bogføringViewModel.BudgetkontoBogførtAsText, Is.Not.Null);
            Assert.That(bogføringViewModel.BudgetkontoBogførtAsText, Is.Not.Empty);
            Assert.That(bogføringViewModel.BudgetkontoBogførtAsText, Is.EqualTo(budgetkontoModelMock.Bogført.ToString("C")));
            Assert.That(bogføringViewModel.BudgetkontoDisponibel, Is.EqualTo(budgetkontoModelMock.Disponibel));
            Assert.That(bogføringViewModel.BudgetkontoDisponibelAsText, Is.Not.Null);
            Assert.That(bogføringViewModel.BudgetkontoDisponibelAsText, Is.Not.Empty);
            Assert.That(bogføringViewModel.BudgetkontoDisponibelAsText, Is.EqualTo(budgetkontoModelMock.Disponibel.ToString("C")));

            Assert.That(bogføringViewModel.AdressekontoNavn, Is.Not.Null);
            Assert.That(bogføringViewModel.AdressekontoNavn, Is.Not.Empty);
            Assert.That(bogføringViewModel.AdressekontoNavn, Is.EqualTo(adressekontoModelMock.Navn));
            Assert.That(bogføringViewModel.AdressekontoSaldo, Is.EqualTo(adressekontoModelMock.Saldo));
            Assert.That(bogføringViewModel.AdressekontoSaldoAsText, Is.Not.Null);
            Assert.That(bogføringViewModel.AdressekontoSaldoAsText, Is.Not.Empty);
            Assert.That(bogføringViewModel.AdressekontoSaldoAsText, Is.EqualTo(adressekontoModelMock.Saldo.ToString("C")));

            Assert.That(expectedNotifyPropertyChanged, Is.Not.Null);
            Assert.That(expectedNotifyPropertyChanged, Is.Empty);

            // ReSharper disable ImplicitlyCapturedClosure
            finansstyringRepositoryMock.AssertWasCalled(m => m.KontoGetAsync(Arg<int>.Is.Equal(regnskabsnummer), Arg<string>.Is.Equal(kontonummer), Arg<DateTime>.Is.Equal(dato)));
            finansstyringRepositoryMock.AssertWasCalled(m => m.BudgetkontoGetAsync(Arg<int>.Is.Equal(regnskabsnummer), Arg<string>.Is.Equal(budgetkontonummer), Arg<DateTime>.Is.Equal(dato)));
            finansstyringRepositoryMock.AssertWasCalled(m => m.AdressekontolisteGetAsync(Arg<int>.Is.Equal(regnskabsnummer), Arg<DateTime>.Is.Equal(dato)));
            // ReSharper restore ImplicitlyCapturedClosure
            finansstyringRepositoryMock.AssertWasCalled(m => m.KontogruppelisteGetAsync());
            finansstyringRepositoryMock.AssertWasCalled(m => m.BudgetkontogruppelisteGetAsync());
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBogføringslinjeModelEventHandler reloader information, hvis Kontonummer opdateres på modellen for bogføringslinjen.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnBogføringslinjeModelEventHandlerReloaderInformationHvisKontonummerOpdateresOnBogføringslinjeModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IKontogruppeModel>(e => e.FromFactory(() =>
                {
                    var kontogruppeModelMock = MockRepository.GenerateMock<IKontogruppeModel>();
                    kontogruppeModelMock.Expect(m => m.Nummer)
                                        .Return(fixture.Create<int>())
                                        .Repeat.Any();
                    return kontogruppeModelMock;
                }));

            var regnskabsnummer = fixture.Create<int>();
            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(regnskabsnummer)
                                 .Repeat.Any();

            var dato = fixture.Create<DateTime>();
            var kontonummer = fixture.Create<string>();
            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(dato)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(kontonummer)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();

            var random = new Random(DateTime.Now.Millisecond);
            var kontogruppeModelMockCollection = fixture.CreateMany<IKontogruppeModel>(7).ToList();
            var kontoModelMock = MockRepository.GenerateMock<IKontoModel>();
            kontoModelMock.Expect(m => m.Kontonavn)
                          .Return(fixture.Create<string>())
                          .Repeat.Any();
            kontoModelMock.Expect(m => m.Kontogruppe)
                          .Return(kontogruppeModelMockCollection.ElementAt(random.Next(0, kontogruppeModelMockCollection.Count - 1)).Nummer)
                          .Repeat.Any();
            kontoModelMock.Expect(m => m.Saldo)
                          .Return(fixture.Create<decimal>())
                          .Repeat.Any();
            kontoModelMock.Expect(m => m.Disponibel)
                          .Return(fixture.Create<decimal>())
                          .Repeat.Any();

            Func<IKontoModel> kontoModelGetter = () => kontoModelMock;
            Func<IEnumerable<IKontogruppeModel>> kontogruppeModelCollectionGetter = () => kontogruppeModelMockCollection;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.KontoGetAsync(Arg<int>.Is.GreaterThan(0), Arg<string>.Is.NotNull, Arg<DateTime>.Is.GreaterThan(DateTime.MinValue)))
                                       .Return(Task.Run(kontoModelGetter))
                                       .Repeat.Any();
            finansstyringRepositoryMock.Expect(m => m.KontogruppelisteGetAsync())
                                       .Return(Task.Run(kontogruppeModelCollectionGetter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var expectedNotifyPropertyChanged = new List<string>
                {
                    "DatoAsTextIsReadOnly",
                    "KontonummerIsReadOnly",
                    "Kontonavn",
                    "KontoSaldo",
                    "KontoSaldoAsText",
                    "KontoDisponibel",
                    "KontoDisponibelAsText",
                    "Tasks",
                    "IsWorking"
                };
            Assert.That(expectedNotifyPropertyChanged, Is.Not.Null);
            Assert.That(expectedNotifyPropertyChanged, Is.Not.Empty);

            var bogføringViewModel = new BogføringViewModel(regnskabViewModelMock, bogføringslinjeModelMock, finansstyringRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel.Tasks, Is.Not.Null);
            Assert.That(bogføringViewModel.Tasks, Is.Empty);
            Assert.That(bogføringViewModel.IsWorking, Is.False);

            bogføringViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(s, Is.TypeOf<BogføringViewModel>());
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    var viewModel = (BogføringViewModel) s;
                    switch (e.PropertyName)
                    {
                        case "Kontonavn":
                        case "KontoSaldo":
                        case "KontoSaldoAsText":
                        case "KontoDisponibel":
                        case "KontoDisponibelAsText":
                            if (string.IsNullOrEmpty(viewModel.Kontonavn))
                            {
                                return;
                            }
                            while (expectedNotifyPropertyChanged.Contains(e.PropertyName))
                            {
                                expectedNotifyPropertyChanged.Remove(e.PropertyName);
                            }
                            break;

                        default:
                            while (expectedNotifyPropertyChanged.Contains(e.PropertyName))
                            {
                                expectedNotifyPropertyChanged.Remove(e.PropertyName);
                            }
                            break;
                    }
                };

            Action action = () =>
                {
                    using (var waitEvent = new AutoResetEvent(false))
                    {
                        var we = waitEvent;
                        bogføringViewModel.PropertyChanged += (s, e) =>
                            {
                                Assert.That(s, Is.Not.Null);
                                Assert.That(s, Is.Not.Null);
                                Assert.That(s, Is.TypeOf<BogføringViewModel>());
                                Assert.That(e, Is.Not.Null);
                                Assert.That(e.PropertyName, Is.Not.Null);
                                Assert.That(e.PropertyName, Is.Not.Empty);
                                var viewModel = (BogføringViewModel) s;
                                switch (e.PropertyName)
                                {
                                    case "IsWorking":
                                        if (viewModel.IsWorking)
                                        {
                                            return;
                                        }
                                        we.Set();
                                        break;
                                }
                            };
                        bogføringslinjeModelMock.Raise(m => m.PropertyChanged += null, bogføringslinjeModelMock, new PropertyChangedEventArgs("Kontonummer"));

                        var tasks = bogføringViewModel.Tasks.ToArray();
                        Assert.That(tasks, Is.Not.Null);
                        Assert.That(tasks, Is.Not.Empty);
                        Task.WaitAll(tasks);

                        waitEvent.WaitOne(2500);
                    }
                };
            Task.Run(action).Wait(3000);

            Assert.That(bogføringViewModel.Kontonavn, Is.Not.Null);
            Assert.That(bogføringViewModel.Kontonavn, Is.Not.Empty);
            Assert.That(bogføringViewModel.Kontonavn, Is.EqualTo(kontoModelMock.Kontonavn));
            Assert.That(bogføringViewModel.KontoSaldo, Is.EqualTo(kontoModelMock.Saldo));
            Assert.That(bogføringViewModel.KontoSaldoAsText, Is.Not.Null);
            Assert.That(bogføringViewModel.KontoSaldoAsText, Is.Not.Empty);
            Assert.That(bogføringViewModel.KontoSaldoAsText, Is.EqualTo(kontoModelMock.Saldo.ToString("C")));
            Assert.That(bogføringViewModel.KontoDisponibel, Is.EqualTo(kontoModelMock.Disponibel));
            Assert.That(bogføringViewModel.KontoDisponibelAsText, Is.Not.Null);
            Assert.That(bogføringViewModel.KontoDisponibelAsText, Is.Not.Empty);
            Assert.That(bogføringViewModel.KontoDisponibelAsText, Is.EqualTo(kontoModelMock.Disponibel.ToString("C")));

            Assert.That(expectedNotifyPropertyChanged, Is.Not.Null);
            Assert.That(expectedNotifyPropertyChanged, Is.Empty);

            finansstyringRepositoryMock.AssertWasCalled(m => m.KontoGetAsync(Arg<int>.Is.Equal(regnskabsnummer), Arg<string>.Is.Equal(kontonummer), Arg<DateTime>.Is.Equal(dato)));
            finansstyringRepositoryMock.AssertWasCalled(m => m.KontogruppelisteGetAsync());
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBogføringslinjeModelEventHandler reloader information, hvis Budgetkontonummer opdateres på modellen for bogføringslinjen.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnBogføringslinjeModelEventHandlerReloaderInformationHvisBudgetkontonummerOpdateresOnBogføringslinjeModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IBudgetkontogruppeModel>(e => e.FromFactory(() =>
                {
                    var budgetkontogruppeModel = MockRepository.GenerateMock<IBudgetkontogruppeModel>();
                    budgetkontogruppeModel.Expect(m => m.Nummer)
                                          .Return(fixture.Create<int>())
                                          .Repeat.Any();
                    return budgetkontogruppeModel;
                }));

            var regnskabsnummer = fixture.Create<int>();
            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(regnskabsnummer)
                                 .Repeat.Any();

            var dato = fixture.Create<DateTime>();
            var budgetkontonummer = fixture.Create<string>();
            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(dato)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(budgetkontonummer)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(0)
                                    .Repeat.Any();

            var random = new Random(DateTime.Now.Millisecond);
            var budgetkontogruppeModelMockCollection = fixture.CreateMany<IBudgetkontogruppeModel>(7).ToList();
            var budgetkontoModelMock = MockRepository.GenerateMock<IBudgetkontoModel>();
            budgetkontoModelMock.Expect(m => m.Kontonavn)
                                .Return(fixture.Create<string>())
                                .Repeat.Any();
            budgetkontoModelMock.Expect(m => m.Kontogruppe)
                                .Return(budgetkontogruppeModelMockCollection.ElementAt(random.Next(0, budgetkontogruppeModelMockCollection.Count - 1)).Nummer)
                                .Repeat.Any();
            budgetkontoModelMock.Expect(m => m.Bogført)
                                .Return(fixture.Create<decimal>())
                                .Repeat.Any();
            budgetkontoModelMock.Expect(m => m.Disponibel)
                                .Return(fixture.Create<decimal>())
                                .Repeat.Any();

            Func<IBudgetkontoModel> budgetkontoModelGetter = () => budgetkontoModelMock;
            Func<IEnumerable<IBudgetkontogruppeModel>> budgetkontogruppeModelCollectionGetter = () => budgetkontogruppeModelMockCollection;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.BudgetkontoGetAsync(Arg<int>.Is.GreaterThan(0), Arg<string>.Is.NotNull, Arg<DateTime>.Is.GreaterThan(DateTime.MinValue)))
                                       .Return(Task.Run(budgetkontoModelGetter))
                                       .Repeat.Any();
            finansstyringRepositoryMock.Expect(m => m.BudgetkontogruppelisteGetAsync())
                                       .Return(Task.Run(budgetkontogruppeModelCollectionGetter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var expectedNotifyPropertyChanged = new List<string>
                {
                    "DatoAsTextIsReadOnly",
                    "BudgetkontonummerIsReadOnly",
                    "Budgetkontonavn",
                    "BudgetkontoBogført",
                    "BudgetkontoBogførtAsText",
                    "BudgetkontoDisponibel",
                    "BudgetkontoDisponibelAsText",
                    "Tasks",
                    "IsWorking"
                };
            Assert.That(expectedNotifyPropertyChanged, Is.Not.Null);
            Assert.That(expectedNotifyPropertyChanged, Is.Not.Empty);

            var bogføringViewModel = new BogføringViewModel(regnskabViewModelMock, bogføringslinjeModelMock, finansstyringRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel.Tasks, Is.Not.Null);
            Assert.That(bogføringViewModel.Tasks, Is.Empty);
            Assert.That(bogføringViewModel.IsWorking, Is.False);

            bogføringViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(s, Is.TypeOf<BogføringViewModel>());
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    var viewModel = (BogføringViewModel) s;
                    switch (e.PropertyName)
                    {
                        case "Budgetkontonavn":
                        case "BudgetkontoBogført":
                        case "BudgetkontoBogførtAsText":
                        case "BudgetkontoDisponibel":
                        case "BudgetkontoDisponibelAsText":
                            if (string.IsNullOrEmpty(viewModel.Budgetkontonavn))
                            {
                                return;
                            }
                            while (expectedNotifyPropertyChanged.Contains(e.PropertyName))
                            {
                                expectedNotifyPropertyChanged.Remove(e.PropertyName);
                            }
                            break;

                        default:
                            while (expectedNotifyPropertyChanged.Contains(e.PropertyName))
                            {
                                expectedNotifyPropertyChanged.Remove(e.PropertyName);
                            }
                            break;
                    }
                };

            Action action = () =>
                {
                    using (var waitEvent = new AutoResetEvent(false))
                    {
                        var we = waitEvent;
                        bogføringViewModel.PropertyChanged += (s, e) =>
                            {
                                Assert.That(s, Is.Not.Null);
                                Assert.That(s, Is.Not.Null);
                                Assert.That(s, Is.TypeOf<BogføringViewModel>());
                                Assert.That(e, Is.Not.Null);
                                Assert.That(e.PropertyName, Is.Not.Null);
                                Assert.That(e.PropertyName, Is.Not.Empty);
                                var viewModel = (BogføringViewModel) s;
                                switch (e.PropertyName)
                                {
                                    case "IsWorking":
                                        if (viewModel.IsWorking)
                                        {
                                            return;
                                        }
                                        we.Set();
                                        break;
                                }
                            };
                        bogføringslinjeModelMock.Raise(m => m.PropertyChanged += null, bogføringslinjeModelMock, new PropertyChangedEventArgs("Budgetkontonummer"));

                        var tasks = bogføringViewModel.Tasks.ToArray();
                        Assert.That(tasks, Is.Not.Null);
                        Assert.That(tasks, Is.Not.Empty);
                        Task.WaitAll(tasks);

                        waitEvent.WaitOne(2500);
                    }
                };
            Task.Run(action).Wait(3000);

            Assert.That(bogføringViewModel.Budgetkontonavn, Is.Not.Null);
            Assert.That(bogføringViewModel.Budgetkontonavn, Is.Not.Empty);
            Assert.That(bogføringViewModel.Budgetkontonavn, Is.EqualTo(budgetkontoModelMock.Kontonavn));
            Assert.That(bogføringViewModel.BudgetkontoBogført, Is.EqualTo(budgetkontoModelMock.Bogført));
            Assert.That(bogføringViewModel.BudgetkontoBogførtAsText, Is.Not.Null);
            Assert.That(bogføringViewModel.BudgetkontoBogførtAsText, Is.Not.Empty);
            Assert.That(bogføringViewModel.BudgetkontoBogførtAsText, Is.EqualTo(budgetkontoModelMock.Bogført.ToString("C")));
            Assert.That(bogføringViewModel.BudgetkontoDisponibel, Is.EqualTo(budgetkontoModelMock.Disponibel));
            Assert.That(bogføringViewModel.BudgetkontoDisponibelAsText, Is.Not.Null);
            Assert.That(bogføringViewModel.BudgetkontoDisponibelAsText, Is.Not.Empty);
            Assert.That(bogføringViewModel.BudgetkontoDisponibelAsText, Is.EqualTo(budgetkontoModelMock.Disponibel.ToString("C")));

            Assert.That(expectedNotifyPropertyChanged, Is.Not.Null);
            Assert.That(expectedNotifyPropertyChanged, Is.Empty);

            finansstyringRepositoryMock.AssertWasCalled(m => m.BudgetkontoGetAsync(Arg<int>.Is.Equal(regnskabsnummer), Arg<string>.Is.Equal(budgetkontonummer), Arg<DateTime>.Is.Equal(dato)));
            finansstyringRepositoryMock.AssertWasCalled(m => m.BudgetkontogruppelisteGetAsync());
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBogføringslinjeModelEventHandler reloader information, hvis Adressekonto opdateres på modellen for bogføringslinjen.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnBogføringslinjeModelEventHandlerReloaderInformationHvisAdressekontoOpdateresOnBogføringslinjeModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var regnskabsnummer = fixture.Create<int>();
            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(regnskabsnummer)
                                 .Repeat.Any();

            var dato = fixture.Create<DateTime>();
            var adressekonto = fixture.Create<int>();
            var bogføringslinjeModelMock = MockRepository.GenerateMock<IBogføringslinjeModel>();
            bogføringslinjeModelMock.Expect(m => m.Dato)
                                    .Return(dato)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Budgetkontonummer)
                                    .Return(null)
                                    .Repeat.Any();
            bogføringslinjeModelMock.Expect(m => m.Adressekonto)
                                    .Return(adressekonto)
                                    .Repeat.Any();

            var adressekontoModelMock = MockRepository.GenerateMock<IAdressekontoModel>();
            adressekontoModelMock.Expect(m => m.Nummer)
                                 .Return(adressekonto)
                                 .Repeat.Any();
            adressekontoModelMock.Expect(m => m.Navn)
                                 .Return(fixture.Create<string>())
                                 .Repeat.Any();
            adressekontoModelMock.Expect(m => m.Saldo)
                                 .Return(fixture.Create<decimal>())
                                 .Repeat.Any();

            Func<IEnumerable<IAdressekontoModel>> adressekontoModelCollectionGetter = () => new Collection<IAdressekontoModel> {adressekontoModelMock};
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.AdressekontolisteGetAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue)))
                                       .Return(Task.Run(adressekontoModelCollectionGetter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var expectedNotifyPropertyChanged = new List<string>
                {
                   "DatoAsTextIsReadOnly",
                    "AdressekontoIsReadOnly",
                    "AdressekontoNavn",
                    "AdressekontoSaldo",
                    "AdressekontoSaldoAsText",
                    "Adressekonti",
                    "Tasks",
                    "IsWorking"
                };
            Assert.That(expectedNotifyPropertyChanged, Is.Not.Null);
            Assert.That(expectedNotifyPropertyChanged, Is.Not.Empty);

            var bogføringViewModel = new BogføringViewModel(regnskabViewModelMock, bogføringslinjeModelMock, finansstyringRepositoryMock, exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel.Tasks, Is.Not.Null);
            Assert.That(bogføringViewModel.Tasks, Is.Empty);
            Assert.That(bogføringViewModel.IsWorking, Is.False);

            bogføringViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(s, Is.TypeOf<BogføringViewModel>());
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    var viewModel = (BogføringViewModel) s;
                    switch (e.PropertyName)
                    {
                        case "AdressekontoNavn":
                        case "AdressekontoSaldo":
                        case "AdressekontoSaldoAsText":
                            if (string.IsNullOrEmpty(viewModel.AdressekontoNavn))
                            {
                                return;
                            }
                            while (expectedNotifyPropertyChanged.Contains(e.PropertyName))
                            {
                                expectedNotifyPropertyChanged.Remove(e.PropertyName);
                            }
                            break;

                        default:
                            while (expectedNotifyPropertyChanged.Contains(e.PropertyName))
                            {
                                expectedNotifyPropertyChanged.Remove(e.PropertyName);
                            }
                            break;
                    }
                };

            Action action = () =>
                {
                    using (var waitEvent = new AutoResetEvent(false))
                    {
                        var we = waitEvent;
                        bogføringViewModel.PropertyChanged += (s, e) =>
                            {
                                Assert.That(s, Is.Not.Null);
                                Assert.That(s, Is.Not.Null);
                                Assert.That(s, Is.TypeOf<BogføringViewModel>());
                                Assert.That(e, Is.Not.Null);
                                Assert.That(e.PropertyName, Is.Not.Null);
                                Assert.That(e.PropertyName, Is.Not.Empty);
                                var viewModel = (BogføringViewModel) s;
                                switch (e.PropertyName)
                                {
                                    case "IsWorking":
                                        if (viewModel.IsWorking)
                                        {
                                            return;
                                        }
                                        we.Set();
                                        break;
                                }
                            };
                        bogføringslinjeModelMock.Raise(m => m.PropertyChanged += null, bogføringslinjeModelMock, new PropertyChangedEventArgs("Adressekonto"));

                        var tasks = bogføringViewModel.Tasks.ToArray();
                        Assert.That(tasks, Is.Not.Null);
                        Assert.That(tasks, Is.Not.Empty);
                        Task.WaitAll(tasks);

                        waitEvent.WaitOne(2500);
                    }
                };
            Task.Run(action).Wait(3000);

            Assert.That(bogføringViewModel.AdressekontoNavn, Is.Not.Null);
            Assert.That(bogføringViewModel.AdressekontoNavn, Is.Not.Empty);
            Assert.That(bogføringViewModel.AdressekontoNavn, Is.EqualTo(adressekontoModelMock.Navn));
            Assert.That(bogføringViewModel.AdressekontoSaldo, Is.EqualTo(adressekontoModelMock.Saldo));
            Assert.That(bogføringViewModel.AdressekontoSaldoAsText, Is.Not.Null);
            Assert.That(bogføringViewModel.AdressekontoSaldoAsText, Is.Not.Empty);
            Assert.That(bogføringViewModel.AdressekontoSaldoAsText, Is.EqualTo(adressekontoModelMock.Saldo.ToString("C")));

            Assert.That(expectedNotifyPropertyChanged, Is.Not.Null);
            Assert.That(expectedNotifyPropertyChanged, Is.Empty);

            finansstyringRepositoryMock.AssertWasCalled(m => m.AdressekontolisteGetAsync(Arg<int>.Is.Equal(regnskabsnummer), Arg<DateTime>.Is.Equal(dato)));
            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at ValidateDatoAsText returnerer Success ved lovlige værdier.
        /// </summary>
        [Test]
        [TestCase("2014-01-01")]
        [TestCase("2014-01-31")]
        [TestCase("2014-02-01")]
        [TestCase("2014-02-28")]
        [TestCase("2014-03-01")]
        [TestCase("2014-03-31")]
        public void TestAtValidateDatoAsTextReturnererSuccessVedLovligeValues(string value)
        {
            var valueAsDateTime = DateTime.Parse(value, new CultureInfo("en-US"));
            var result = BogføringViewModel.ValidateDatoAsText(valueAsDateTime.ToShortDateString());
            Assert.That(result, Is.EqualTo(ValidationResult.Success));
        }

        /// <summary>
        /// Tester, at ValidateDatoAsText returnerer ValidationResult ved ulovlige værdier.
        /// </summary>
        [Test]
        [TestCase(null, "ValueIsRequiered")]
        [TestCase("", "ValueIsRequiered")]
        [TestCase(" ", "ValueIsRequiered")]
        [TestCase("XYZ", "ValueIsNotDate")]
        [TestCase("ZYX", "ValueIsNotDate")]
        [TestCase("2014-31-01", "ValueIsNotDate")]
        [TestCase("2014-01-32", "ValueIsNotDate")]
        [TestCase("2050-01-01", "DateGreaterThan")]
        [TestCase("2050-01-31", "DateGreaterThan")]
        [TestCase("2050-12-01", "DateGreaterThan")]
        [TestCase("2050-12-31", "DateGreaterThan")]
        public void TestAtValidateDatoAsTextReturnererValidationResultVedUlovligeValues(string value, string validationErrorText)
        {
            var text = (Text) Enum.Parse(typeof (Text), validationErrorText);
            var result = BogføringViewModel.ValidateDatoAsText(value);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.EqualTo(ValidationResult.Success));
            Assert.That(result.ErrorMessage, Is.Not.Null);
            Assert.That(result.ErrorMessage, Is.Not.Empty);
            switch (text)
            {
                case Text.DateGreaterThan:
                    Assert.That(result.ErrorMessage, Is.EqualTo(Resource.GetText(text, DateTime.Now.ToLongDateString())));
                    break;

                default:
                    Assert.That(result.ErrorMessage, Is.EqualTo(Resource.GetText(text)));
                    break;
            }
            Assert.That(result.MemberNames, Is.Not.Null);
            Assert.That(result.MemberNames, Is.Empty);
        }

        /// <summary>
        /// Tester, at ValidateBilag returnerer Success ved lovlige værdier.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("XYZ")]
        [TestCase("ZYX")]
        public void TestAtValidateBilagReturnererSuccessVedLovligeValues(string value)
        {
            var result = BogføringViewModel.ValidateBilag(value);
            Assert.That(result, Is.EqualTo(ValidationResult.Success));
        }

        /// <summary>
        /// Tester, at ValidateKontonummer returnerer Success ved lovlige værdier.
        /// </summary>
        [Test]
        [TestCase("XYZ")]
        [TestCase("ZYX")]
        public void TestAtValidateKontonummerReturnererSuccessVedLovligeValues(string value)
        {
            var result = BogføringViewModel.ValidateKontonummer(value);
            Assert.That(result, Is.EqualTo(ValidationResult.Success));
        }

        /// <summary>
        /// Tester, at ValidateKontonummer returnerer ValidationResult ved ulovlige værdier.
        /// </summary>
        [Test]
        [TestCase(null, "ValueIsRequiered")]
        [TestCase("", "ValueIsRequiered")]
        [TestCase(" ", "ValueIsRequiered")]
        public void TestAtValidateKontonummerReturnererValidationResultVedUlovligeValues(string value, string validationErrorText)
        {
            var text = (Text) Enum.Parse(typeof (Text), validationErrorText);
            var result = BogføringViewModel.ValidateKontonummer(value);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.EqualTo(ValidationResult.Success));
            Assert.That(result.ErrorMessage, Is.Not.Null);
            Assert.That(result.ErrorMessage, Is.Not.Empty);
            Assert.That(result.ErrorMessage, Is.EqualTo(Resource.GetText(text)));
            Assert.That(result.MemberNames, Is.Not.Null);
            Assert.That(result.MemberNames, Is.Empty);
        }

        /// <summary>
        /// Tester, at ValidateTekst returnerer Success ved lovlige værdier.
        /// </summary>
        [Test]
        [TestCase("XYZ")]
        [TestCase("ZYX")]
        public void TestAtValidateTekstReturnererSuccessVedLovligeValues(string value)
        {
            var result = BogføringViewModel.ValidateTekst(value);
            Assert.That(result, Is.EqualTo(ValidationResult.Success));
        }

        /// <summary>
        /// Tester, at ValidateTekst returnerer ValidationResult ved ulovlige værdier.
        /// </summary>
        [Test]
        [TestCase(null, "ValueIsRequiered")]
        [TestCase("", "ValueIsRequiered")]
        [TestCase(" ", "ValueIsRequiered")]
        public void TestAtValidateTekstReturnererValidationResultVedUlovligeValues(string value, string validationErrorText)
        {
            var text = (Text) Enum.Parse(typeof (Text), validationErrorText);
            var result = BogføringViewModel.ValidateTekst(value);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.EqualTo(ValidationResult.Success));
            Assert.That(result.ErrorMessage, Is.Not.Null);
            Assert.That(result.ErrorMessage, Is.Not.Empty);
            Assert.That(result.ErrorMessage, Is.EqualTo(Resource.GetText(text)));
            Assert.That(result.MemberNames, Is.Not.Null);
            Assert.That(result.MemberNames, Is.Empty);
        }

        /// <summary>
        /// Tester, at ValidateBudgetkontonummer returnerer Success ved lovlige værdier.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("XYZ")]
        [TestCase("ZYX")]
        public void TestAtValidateBudgetkontonummerSuccessVedLovligeValues(string value)
        {
            var result = BogføringViewModel.ValidateBudgetkontonummer(value);
            Assert.That(result, Is.EqualTo(ValidationResult.Success));
        }

        /// <summary>
        /// Tester, at ValidateCurrency returnerer Success ved lovlige værdier.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("$ 0.00")]
        [TestCase("$ 1000.00")]
        [TestCase("$ 4000.00")]
        [TestCase("$ 5000.00")]
        public void TestAtValidateCurrencyReturnererSuccessVedLovligeValues(string value)
        {
            var result = BogføringViewModel.ValidateCurrency(string.IsNullOrWhiteSpace(value) ? value : decimal.Parse(value, NumberStyles.Any, new CultureInfo("en-US")).ToString("C"));
            Assert.That(result, Is.EqualTo(ValidationResult.Success));
        }

        /// <summary>
        /// Tester, at ValidateCurrency returnerer ValidationResult ved ulovlige værdier.
        /// </summary>
        [Test]
        [TestCase("XYZ", "ValueIsNotDecimal")]
        [TestCase("ZYX", "ValueIsNotDecimal")]
        [TestCase("$ -0.01", "DecimalLowerThan")]
        [TestCase("$ -1000.00", "DecimalLowerThan")]
        [TestCase("$ -2000.00", "DecimalLowerThan")]
        [TestCase("$ -3000.00", "DecimalLowerThan")]
        [TestCase("$ -4000.00", "DecimalLowerThan")]
        public void TestAtValidateCurrencyReturnererValidationResultVedUlovligeValues(string value, string validationErrorText)
        {
            var text = (Text) Enum.Parse(typeof (Text), validationErrorText);
            decimal valueAsDecimal;
            var result = BogføringViewModel.ValidateCurrency(decimal.TryParse(value, NumberStyles.Any, new CultureInfo("en-US"), out valueAsDecimal) ? valueAsDecimal.ToString("C") : value);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.EqualTo(ValidationResult.Success));
            Assert.That(result.ErrorMessage, Is.Not.Null);
            Assert.That(result.ErrorMessage, Is.Not.Empty);
            switch (text)
            {
                case Text.DecimalLowerThan:
                    Assert.That(result.ErrorMessage, Is.EqualTo(Resource.GetText(text, 0M)));
                    break;

                default:
                    Assert.That(result.ErrorMessage, Is.EqualTo(Resource.GetText(text)));
                    break;
            }
            Assert.That(result.MemberNames, Is.Not.Null);
            Assert.That(result.MemberNames, Is.Empty);
        }

        /// <summary>
        /// Tester, at ValidateAdressekonto returnerer Success ved lovlige værdier.
        /// </summary>
        [Test]
        [TestCase(-1000)]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(1000)]
        public void TestAtValidateAdressekontoReturnererSuccessVedLovligeValues(int value)
        {
            var result = BogføringViewModel.ValidateAdressekonto(value);
            Assert.That(result, Is.EqualTo(ValidationResult.Success));
        }
    }
}
