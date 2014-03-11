using System;
using NUnit.Framework;
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
    }
}
