﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.Resources;
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

            bogføringslinjeModelMock.AssertWasCalled(m => m.Dato);
            bogføringslinjeModelMock.AssertWasCalled(m => m.Bilag);
            bogføringslinjeModelMock.AssertWasCalled(m => m.Kontonummer);
            bogføringslinjeModelMock.AssertWasCalled(m => m.Tekst);
            bogføringslinjeModelMock.AssertWasCalled(m => m.Budgetkontonummer);
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
        /// Tester, at PropertyChangedOnBogføringslinjeModelEventHandler rejser PropertyChanged, når modellen for bogføringslinjen opdateres.
        /// </summary>
        [Test]
        [TestCase("Dato", "Dato")]
        [TestCase("Dato", "DatoAsText")]
        [TestCase("Bilag", "Bilag")]
        [TestCase("Kontonummer", "Kontonummer")]
        [TestCase("Kontonummer", "Kontonavn")]
        [TestCase("Kontonummer", "KontoSaldo")]
        [TestCase("Kontonummer", "KontoSaldoAsText")]
        [TestCase("Kontonummer", "KontoDisponibel")]
        [TestCase("Kontonummer", "KontoDisponibelAsText")]
        [TestCase("Tekst", "Tekst")]
        public void TestAtPropertyChangedOnBogføringslinjeModelEventHandlerRejserPropertyChangedOnBogføringslinjeModelUpdate(string propertyNameToRaise, string expectPropertyName)
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
            bogføringslinjeModelMock.Raise(m => m.PropertyChanged += null, bogføringslinjeModelMock, new PropertyChangedEventArgs(propertyNameToRaise));
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
    }
}
