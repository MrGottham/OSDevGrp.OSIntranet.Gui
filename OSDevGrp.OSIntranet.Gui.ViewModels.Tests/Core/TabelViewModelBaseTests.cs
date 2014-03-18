using System;
using System.ComponentModel;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Core
{
    /// <summary>
    /// Tester ViewModel, der indeholder grundlæggende tabeloplysninger, såsom typer, grupper m.m.
    /// </summary>
    [TestFixture]
    public class TabelViewModelBaseTests
    {
        /// <summary>
        /// Egen klasse til test af ViewModel, der indeholder grundlæggende tabeloplysninger, såsom typer, grupper m.m.
        /// </summary>
        private class MyTabelViewModel : TabelViewModelBase<ITabelModelBase>
        {
            #region Constructor

            /// <summary>
            /// Danner egen klasse til test af ViewModel, der indeholder grundlæggende tabeloplysninger, såsom typer, grupper m.m.
            /// </summary>
            /// <param name="tabelModel">Modellen, der indeholder grundlæggende tabeloplysninger, såsom typer, grupper m.m.</param>
            /// <param name="exceptionHandlerViewModel">Implementering af ViewModel for en exceptionhandler.</param>
            public MyTabelViewModel(ITabelModelBase tabelModel, IExceptionHandlerViewModel exceptionHandlerViewModel)
                : base(tabelModel, exceptionHandlerViewModel)
            {
            }

            #endregion

            #region Properties

            /// <summary>
            /// Modellen, der indeholder grundlæggende tabeloplysninger, såsom typer, grupper m.m.
            /// </summary>
            public new ITabelModelBase Model
            {
                get
                {
                    return base.Model;
                }
            }

            /// <summary>
            /// ViewModel for exceptionhandleren.
            /// </summary>
            public new IExceptionHandlerViewModel ExceptionHandler
            {
                get
                {
                    return base.ExceptionHandler;
                }
            }

            /// <summary>
            /// Angivelse af, om metoden ModelChanged er kaldt.
            /// </summary>
            public bool IsModelChangedCalled
            {
                get; 
                private set;
            }

            /// <summary>
            /// Angivelse af, om metoden ModelChanged skal kaste en Exception, når den kaldes.
            /// </summary>
            public bool ThrowExceptionInModelChanged
            {
                get; 
                set; 
            }

            #endregion

            #region Methods

            /// <summary>
            /// Metode, der kaldes, når en property på modellen, der indeholder grundlæggende tabeloplysninger, såsom typer, grupper m.m., ændres.
            /// </summary>
            /// <param name="propertyName">Navn på propertyen, der er blevet ændret.</param>
            protected override void ModelChanged(string propertyName)
            {
                IsModelChangedCalled = true;
                if (ThrowExceptionInModelChanged == false)
                {
                    return;
                }
                var fixture = new Fixture();
                throw new IntranetGuiSystemException(fixture.Create<string>());
            }

            #endregion
        }

        /// <summary>
        /// Tester, at konstruktøren initierer en ViewModel, der indeholder grundlæggende tabeloplysninger, såsom typer, grupper m.m.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererTabelViewModelBase()
        {
            var fixture = new Fixture();

            var tabelModelMock = MockRepository.GenerateMock<ITabelModelBase>();
            tabelModelMock.Expect(m => m.Id)
                          .Return(fixture.Create<string>())
                          .Repeat.Any();
            tabelModelMock.Expect(m => m.Tekst)
                          .Return(fixture.Create<string>())
                          .Repeat.Any();

            var exceptionHandleViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var tabelViewModel = new MyTabelViewModel(tabelModelMock, exceptionHandleViewModelMock);
            Assert.That(tabelViewModel, Is.Not.Null);
            Assert.That(tabelViewModel.Id, Is.Not.Null);
            Assert.That(tabelViewModel.Id, Is.Not.Empty);
            Assert.That(tabelViewModel.Id, Is.EqualTo(tabelModelMock.Id));
            Assert.That(tabelViewModel.Tekst, Is.Not.Null);
            Assert.That(tabelViewModel.Tekst, Is.Not.Empty);
            Assert.That(tabelViewModel.Tekst, Is.EqualTo(tabelModelMock.Tekst));
            Assert.That(tabelViewModel.DisplayName, Is.Not.Null);
            Assert.That(tabelViewModel.DisplayName, Is.Not.Empty);
            Assert.That(tabelViewModel.DisplayName, Is.EqualTo(tabelModelMock.Tekst));
            Assert.That(tabelViewModel.Model, Is.Not.Null);
            Assert.That(tabelViewModel.Model, Is.EqualTo(tabelModelMock));
            Assert.That(tabelViewModel.ExceptionHandler, Is.Not.Null);
            Assert.That(tabelViewModel.ExceptionHandler, Is.EqualTo(exceptionHandleViewModelMock));

            tabelModelMock.AssertWasCalled(m => m.Id);
            tabelModelMock.AssertWasCalled(m => m.Tekst);
            exceptionHandleViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis modellen, der indeholder grundlæggende tabeloplysninger, såsom typer, grupper m.m., er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisTabelModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new MyTabelViewModel(null, fixture.Create<IExceptionHandlerViewModel>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("tabelModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis ViewModel for exceptionhandleren er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisExceptionHandleViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<ITabelModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<ITabelModelBase>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new MyTabelViewModel(fixture.Create<ITabelModelBase>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("exceptionHandlerViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at sætteren til Tekst opdaterer Tekst på modellen, der indeholder grundlæggende tabeloplysninger, såsom typer, grupper m.m.
        /// </summary>
        [Test]
        public void TestAtTekstSetterOpdatererTekstOnTabelModelBase()
        {
            var fixture = new Fixture();
            fixture.Customize<ITabelModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<ITabelModelBase>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var tabelModelMock = fixture.Create<ITabelModelBase>();
            var exceptionHandleViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var tabelViewModel = new MyTabelViewModel(tabelModelMock, exceptionHandleViewModelMock);
            Assert.That(tabelViewModel, Is.Not.Null);

            var newValue = fixture.Create<string>();
            tabelViewModel.Tekst = newValue;

            tabelModelMock.AssertWasCalled(m => m.Tekst = Arg<string>.Is.Equal(newValue));
            exceptionHandleViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at sætteren til Tekst kalder HandleException på exceptionhandleren ved exceptions.
        /// </summary>
        [Test]
        public void TestAtTekstSetterKalderHandleExceptionOnExceptionHandlerViewModelVedExceptions()
        {
            var fixture = new Fixture();
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = fixture.Create<Exception>();
            var tabelModelMock = MockRepository.GenerateMock<ITabelModelBase>();
            tabelModelMock.Expect(m => m.Tekst = Arg<string>.Is.Anything)
                          .Throw(exception)
                          .Repeat.Any();

            var exceptionHandleViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var tabelViewModel = new MyTabelViewModel(tabelModelMock, exceptionHandleViewModelMock);
            Assert.That(tabelViewModel, Is.Not.Null);

            var newValue = fixture.Create<string>();
            tabelViewModel.Tekst = newValue;

            tabelModelMock.AssertWasCalled(m => m.Tekst = Arg<string>.Is.Equal(newValue));
            exceptionHandleViewModelMock.AssertWasCalled(m => m.HandleException(Arg<Exception>.Is.Equal(exception)));
        }

        /// <summary>
        /// Tester, at PropertyChangedOnTabelModelEventHandler rejser PropertyChanged, når modellen, der indeholder grundlæggende tabeloplysninger, såsom typer, grupper m.m., opdateres.
        /// </summary>
        [Test]
        [TestCase("Id", "Id")]
        [TestCase("Tekst", "Tekst")]
        [TestCase("Tekst", "DisplayName")]
        public void TestAtPropertyChangedOnTabelModelEventHandlerRejserPropertyChangedOnTabelModelUpdate(string propertyNameToRaise, string expectPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<ITabelModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<ITabelModelBase>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var tabelModelMock = fixture.Create<ITabelModelBase>();
            var exceptionHandleViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var tabelViewModel = new MyTabelViewModel(tabelModelMock, exceptionHandleViewModelMock)
                {
                    ThrowExceptionInModelChanged = false
                };
            Assert.That(tabelViewModel, Is.Not.Null);
            Assert.That(tabelViewModel.ThrowExceptionInModelChanged, Is.False);

            var eventCalled = false;
            tabelViewModel.PropertyChanged += (s, e) =>
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
            tabelModelMock.Raise(m => m.PropertyChanged += null, tabelModelMock, new PropertyChangedEventArgs(propertyNameToRaise));
            Assert.That(eventCalled, Is.True);

            exceptionHandleViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at PropertyChangedOnTabelModelEventHandler kalder metoden ModelChanged, når modellen, der indeholder grundlæggende tabeloplysninger, såsom typer, grupper m.m., opdateres.
        /// </summary>
        [Test]
        [TestCase("Id")]
        [TestCase("Tekst")]
        [TestCase("DisplayName")]
        public void TestAtPropertyChangedOnTabelModelEventHandlerKalderModelChangedOnTabelModelUpdate(string propertyNameToRaise)
        {
            var fixture = new Fixture();
            fixture.Customize<ITabelModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<ITabelModelBase>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var tabelModelMock = fixture.Create<ITabelModelBase>();
            var exceptionHandleViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var tabelViewModel = new MyTabelViewModel(tabelModelMock, exceptionHandleViewModelMock)
                {
                    ThrowExceptionInModelChanged = false
                };
            Assert.That(tabelViewModel, Is.Not.Null);
            Assert.That(tabelViewModel.ThrowExceptionInModelChanged, Is.False);

            Assert.That(tabelViewModel.IsModelChangedCalled, Is.False);
            tabelModelMock.Raise(m => m.PropertyChanged += null, tabelModelMock, new PropertyChangedEventArgs(propertyNameToRaise));
            Assert.That(tabelViewModel.IsModelChangedCalled, Is.True);

            exceptionHandleViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at PropertyChangedOnTabelModelEventHandler kalder HandleException på exceptionhandleren, hvis metoden ModelChanged kaster en exception.
        /// </summary>
        [Test]
        [TestCase("Id")]
        [TestCase("Tekst")]
        [TestCase("DisplayName")]
        public void TestAtPropertyChangedOnTabelModelEventHandlerKalderHandleExceptionOnExceptionHandlerViewModelHvisModelChangedKasterException(string propertyNameToRaise)
        {
            var fixture = new Fixture();
            fixture.Customize<ITabelModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<ITabelModelBase>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var tabelModelMock = fixture.Create<ITabelModelBase>();
            var exceptionHandleViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var tabelViewModel = new MyTabelViewModel(tabelModelMock, exceptionHandleViewModelMock)
                {
                    ThrowExceptionInModelChanged = true
                };
            Assert.That(tabelViewModel, Is.Not.Null);
            Assert.That(tabelViewModel.ThrowExceptionInModelChanged, Is.True);

            Assert.That(tabelViewModel.IsModelChangedCalled, Is.False);
            tabelModelMock.Raise(m => m.PropertyChanged += null, tabelModelMock, new PropertyChangedEventArgs(propertyNameToRaise));
            Assert.That(tabelViewModel.IsModelChangedCalled, Is.True);

            exceptionHandleViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at PropertyChangedOnTabelModelEventHandler kaster en ArgumentNullException, hvis objektet, der rejser eventet, er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnTabelModelEventHandlerKasterArgumentNullExceptionHvisSenderErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<ITabelModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<ITabelModelBase>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var tabelModelMock = fixture.Create<ITabelModelBase>();
            var exceptionHandleViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var tabelViewModel = new MyTabelViewModel(tabelModelMock, exceptionHandleViewModelMock);
            Assert.That(tabelViewModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => tabelModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("sender"));
            Assert.That(exception.InnerException, Is.Null);

            exceptionHandleViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at PropertyChangedOnTabelModelEventHandler kaster en ArgumentNullException, hvis argumenter til eventet er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnTabelModelEventHandlerKasterArgumentNullExceptionHvisEventArgsErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<ITabelModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<ITabelModelBase>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var tabelModelMock = fixture.Create<ITabelModelBase>();
            var exceptionHandleViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var tabelViewModel = new MyTabelViewModel(tabelModelMock, exceptionHandleViewModelMock);
            Assert.That(tabelViewModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => tabelModelMock.Raise(m => m.PropertyChanged += null, fixture.Create<object>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("eventArgs"));
            Assert.That(exception.InnerException, Is.Null);

            exceptionHandleViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }
    }
}
