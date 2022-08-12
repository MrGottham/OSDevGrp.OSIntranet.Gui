using System;
using System.ComponentModel;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;

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
            public new ITabelModelBase Model => base.Model;

            /// <summary>
            /// ViewModel for exceptionhandleren.
            /// </summary>
            public new IExceptionHandlerViewModel ExceptionHandler => base.ExceptionHandler;

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

                Fixture fixture = new Fixture();
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
            Fixture fixture = new Fixture();

            string id = fixture.Create<string>();
            string tekst = fixture.Create<string>();
            Mock<ITabelModelBase> tabelModelMock = fixture.BuildTabelModelMock(id, tekst);

            Mock<IExceptionHandlerViewModel> exceptionHandleViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            MyTabelViewModel tabelViewModel = new MyTabelViewModel(tabelModelMock.Object, exceptionHandleViewModelMock.Object);
            Assert.That(tabelViewModel, Is.Not.Null);
            Assert.That(tabelViewModel.Id, Is.Not.Null);
            Assert.That(tabelViewModel.Id, Is.Not.Empty);
            Assert.That(tabelViewModel.Id, Is.EqualTo(id));
            Assert.That(tabelViewModel.Tekst, Is.Not.Null);
            Assert.That(tabelViewModel.Tekst, Is.Not.Empty);
            Assert.That(tabelViewModel.Tekst, Is.EqualTo(tekst));
            Assert.That(tabelViewModel.DisplayName, Is.Not.Null);
            Assert.That(tabelViewModel.DisplayName, Is.Not.Empty);
            Assert.That(tabelViewModel.DisplayName, Is.EqualTo(tekst));
            Assert.That(tabelViewModel.Model, Is.Not.Null);
            Assert.That(tabelViewModel.Model, Is.EqualTo(tabelModelMock));
            Assert.That(tabelViewModel.ExceptionHandler, Is.Not.Null);
            Assert.That(tabelViewModel.ExceptionHandler, Is.EqualTo(exceptionHandleViewModelMock));

            tabelModelMock.Verify(m => m.Id, Times.Once);
            tabelModelMock.Verify(m => m.Tekst, Times.Once);

            exceptionHandleViewModelMock.Verify(m => m.HandleException(It.IsAny<Exception>()), Times.Never);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis modellen, der indeholder grundlæggende tabeloplysninger, såsom typer, grupper m.m., er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisTabelModelErNull()
        {
            Fixture fixture = new Fixture();

            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new MyTabelViewModel(null, fixture.BuildExceptionHandlerViewModel()));
            // ReSharper restore ObjectCreationAsStatement
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
            Fixture fixture = new Fixture();

            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new MyTabelViewModel(fixture.BuildTabelModel(), null));
            // ReSharper restore ObjectCreationAsStatement
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
            Fixture fixture = new Fixture();

            Mock<ITabelModelBase> tabelModelMock = fixture.BuildTabelModelMock();
            Mock<IExceptionHandlerViewModel> exceptionHandleViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            ITabelViewModelBase tabelViewModel = new MyTabelViewModel(tabelModelMock.Object, exceptionHandleViewModelMock.Object);
            Assert.That(tabelViewModel, Is.Not.Null);

            string newValue = fixture.Create<string>();
            tabelViewModel.Tekst = newValue;

            tabelModelMock.VerifySet(m => m.Tekst = It.Is<string>(value => string.CompareOrdinal(value, newValue) == 0), Times.Once);
            exceptionHandleViewModelMock.Verify(m => m.HandleException(It.IsAny<Exception>()), Times.Never);
        }

        /// <summary>
        /// Tester, at sætteren til Tekst kalder HandleException på exceptionhandleren ved exceptions.
        /// </summary>
        [Test]
        public void TestAtTekstSetterKalderHandleExceptionOnExceptionHandlerViewModelVedExceptions()
        {
            Fixture fixture = new Fixture();

            Exception exception = fixture.Create<Exception>();
            Mock<ITabelModelBase> tabelModelMock = fixture.BuildTabelModelMock(setupCallback: mock => mock.SetupSet(m => m.Tekst = It.IsAny<string>()).Throws(exception));
            Mock<IExceptionHandlerViewModel> exceptionHandleViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            ITabelViewModelBase tabelViewModel = new MyTabelViewModel(tabelModelMock.Object, exceptionHandleViewModelMock.Object);
            Assert.That(tabelViewModel, Is.Not.Null);

            string newValue = fixture.Create<string>();
            tabelViewModel.Tekst = newValue;

            tabelModelMock.VerifySet(m => m.Tekst = It.Is<string>(value => string.CompareOrdinal(value, newValue) == 0), Times.Once);
            exceptionHandleViewModelMock.Verify(m => m.HandleException(It.Is<Exception>(value => value != null && value == exception)), Times.Once);
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
            Fixture fixture = new Fixture();

            Mock<ITabelModelBase> tabelModelMock = fixture.BuildTabelModelMock();
            Mock<IExceptionHandlerViewModel> exceptionHandleViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            MyTabelViewModel tabelViewModel = new MyTabelViewModel(tabelModelMock.Object, exceptionHandleViewModelMock.Object)
            {
                ThrowExceptionInModelChanged = false
            };
            Assert.That(tabelViewModel, Is.Not.Null);
            Assert.That(tabelViewModel.ThrowExceptionInModelChanged, Is.False);

            bool eventCalled = false;
            tabelViewModel.PropertyChanged += (s, e) =>
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
            tabelModelMock.Raise(m => m.PropertyChanged += null, tabelModelMock, new PropertyChangedEventArgs(propertyNameToRaise));
            Assert.That(eventCalled, Is.True);

            exceptionHandleViewModelMock.Verify(m => m.HandleException(It.IsAny<Exception>()), Times.Never);
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
            Fixture fixture = new Fixture();

            Mock<ITabelModelBase> tabelModelMock = fixture.BuildTabelModelMock();
            Mock<IExceptionHandlerViewModel> exceptionHandleViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            MyTabelViewModel tabelViewModel = new MyTabelViewModel(tabelModelMock.Object, exceptionHandleViewModelMock.Object)
            {
                ThrowExceptionInModelChanged = false
            };
            Assert.That(tabelViewModel, Is.Not.Null);
            Assert.That(tabelViewModel.ThrowExceptionInModelChanged, Is.False);

            Assert.That(tabelViewModel.IsModelChangedCalled, Is.False);
            tabelModelMock.Raise(m => m.PropertyChanged += null, tabelModelMock, new PropertyChangedEventArgs(propertyNameToRaise));
            Assert.That(tabelViewModel.IsModelChangedCalled, Is.True);

            exceptionHandleViewModelMock.Verify(m => m.HandleException(It.IsAny<Exception>()), Times.Never);
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
            Fixture fixture = new Fixture();

            Mock<ITabelModelBase> tabelModelMock = fixture.BuildTabelModelMock();
            Mock<IExceptionHandlerViewModel> exceptionHandleViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            MyTabelViewModel tabelViewModel = new MyTabelViewModel(tabelModelMock.Object, exceptionHandleViewModelMock.Object)
            {
                ThrowExceptionInModelChanged = true
            };
            Assert.That(tabelViewModel, Is.Not.Null);
            Assert.That(tabelViewModel.ThrowExceptionInModelChanged, Is.True);

            Assert.That(tabelViewModel.IsModelChangedCalled, Is.False);
            tabelModelMock.Raise(m => m.PropertyChanged += null, tabelModelMock, new PropertyChangedEventArgs(propertyNameToRaise));
            Assert.That(tabelViewModel.IsModelChangedCalled, Is.True);

            exceptionHandleViewModelMock.Verify(m => m.HandleException(It.Is<IntranetGuiSystemException>(value => value != null)), Times.Once);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnTabelModelEventHandler kaster en ArgumentNullException, hvis objektet, der rejser eventet, er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnTabelModelEventHandlerKasterArgumentNullExceptionHvisSenderErNull()
        {
            Fixture fixture = new Fixture();

            Mock<ITabelModelBase> tabelModelMock = fixture.BuildTabelModelMock();
            Mock<IExceptionHandlerViewModel> exceptionHandleViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            ITabelViewModelBase tabelViewModel = new MyTabelViewModel(tabelModelMock.Object, exceptionHandleViewModelMock.Object);
            Assert.That(tabelViewModel, Is.Not.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => tabelModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("sender"));
            Assert.That(exception.InnerException, Is.Null);

            exceptionHandleViewModelMock.Verify(m => m.HandleException(It.IsAny<Exception>()), Times.Never);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnTabelModelEventHandler kaster en ArgumentNullException, hvis argumenter til eventet er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnTabelModelEventHandlerKasterArgumentNullExceptionHvisEventArgsErNull()
        {
            Fixture fixture = new Fixture();

            Mock<ITabelModelBase> tabelModelMock = fixture.BuildTabelModelMock();
            Mock<IExceptionHandlerViewModel> exceptionHandleViewModelMock = fixture.BuildExceptionHandlerViewModelMock();

            ITabelViewModelBase tabelViewModel = new MyTabelViewModel(tabelModelMock.Object, exceptionHandleViewModelMock.Object);
            Assert.That(tabelViewModel, Is.Not.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => tabelModelMock.Raise(m => m.PropertyChanged += null, fixture.Create<object>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("eventArgs"));
            Assert.That(exception.InnerException, Is.Null);

            exceptionHandleViewModelMock.Verify(m => m.HandleException(It.IsAny<Exception>()), Times.Never);
        }
    }
}