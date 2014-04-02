using System;
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
            bogføringslinjeModelMock.Expect(m => m.Kontonummer)
                                    .Return(null)
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
            Assert.That(bogføringViewModel.DatoLabel, Is.Not.Null);
            Assert.That(bogføringViewModel.DatoLabel, Is.Not.Empty);
            Assert.That(bogføringViewModel.DatoLabel, Is.EqualTo(Resource.GetText(Text.Date)));

            bogføringslinjeModelMock.AssertWasCalled(m => m.Dato);
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
                                                 Assert.That(validationException.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingBookeepingDate)));
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
                                                 Assert.That(validationException.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingBookeepingDate)));
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
                                                 var validationException = (IntranetGuiRepositoryException) e.Arguments.ElementAt(0);
                                                 Assert.That(validationException, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Empty);
                                                 Assert.That(validationException.Message, Is.EqualTo(exception.Message));
                                                 Assert.That(validationException.InnerException, Is.Null);
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
                                                 var validationException = (IntranetGuiBusinessException) e.Arguments.ElementAt(0);
                                                 Assert.That(validationException, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Empty);
                                                 Assert.That(validationException.Message, Is.EqualTo(exception.Message));
                                                 Assert.That(validationException.InnerException, Is.Null);
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
                                                 var validationException = (IntranetGuiSystemException) e.Arguments.ElementAt(0);
                                                 Assert.That(validationException, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Empty);
                                                 Assert.That(validationException.Message, Is.EqualTo(exception.Message));
                                                 Assert.That(validationException.InnerException, Is.Null);
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
                                                 var validationException = (IntranetGuiSystemException) e.Arguments.ElementAt(0);
                                                 Assert.That(validationException, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Null);
                                                 Assert.That(validationException.Message, Is.Not.Empty);
                                                 Assert.That(validationException.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingPropertyValue, "DatoAsText", exception.Message)));
                                                 Assert.That(validationException.InnerException, Is.Not.Null);
                                                 Assert.That(validationException.InnerException, Is.EqualTo(exception));
                                             })
                                         .Repeat.Any();

            var bogføringViewModel = new BogføringViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeModelMock, fixture.Create<IFinansstyringRepository>(), exceptionHandlerViewModelMock);
            Assert.That(bogføringViewModel, Is.Not.Null);

            bogføringViewModel.DatoAsText = newValue;

            bogføringslinjeModelMock.AssertWasCalled(m => m.Dato = Arg<DateTime>.Is.Equal(DateTime.Parse(newValue, CultureInfo.CurrentUICulture)));
            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBogføringslinjeModelEventHandler rejser PropertyChanged, når modellen for bogføringslinjen opdateres.
        /// </summary>
        [Test]
        [TestCase("Dato", "Dato")]
        [TestCase("Dato", "DatoAsText")]
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
    }
}
