using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Core.Commands
{
    /// <summary>
    /// Tester basisfunktionalitet til en kommando, der skal udføres på en ViewModel.
    /// </summary>
    [TestFixture]
    public class ViewModelCommandBaseTests
    {
        /// <summary>
        /// Egen klasse til test af basisfunktionalitet til en kommando, der skal udføres på en ViewModel.
        /// </summary>
        private class MyViewModelCommand : ViewModelCommandBase<IViewModel>
        {
            #region Constructor

            /// <summary>
            /// Danner egen klasse til test af basisfunktionalitet til en kommando, der skal udføres på en ViewModel.
            /// </summary>
            /// <param name="exceptionHandlerViewModel">Implementering af en ViewModel til en exceptionhandler.</param>
            public MyViewModelCommand(IExceptionHandlerViewModel exceptionHandlerViewModel)
                : base(exceptionHandlerViewModel)
            {
            }

            #endregion

            #region Properties

            /// <summary>
            /// Handling, der udføres ved evaluering af, om kommandoen kan udføres.
            /// </summary>
            public Action<IViewModel> OnCanExecute
            {
                get; 
                set;
            }

            /// <summary>
            /// Handling, der udføres ved udfgørelse af kommandoen.
            /// </summary>
            public Action<IViewModel> OnExecute
            {
                get; 
                set;
            }

            #endregion
            
            #region Methods

            /// <summary>
            /// Returnerer ViewModel for den exceptionhandler, der skal håndtere exceptions.
            /// </summary>
            /// <returns>ViewModel for den exceptionhandler, der skal håndtere exceptions.</returns>
            public IExceptionHandlerViewModel GetExceptionHandler()
            {
                return ExceptionHandler;
            }

            /// <summary>
            /// Returnerer angivelse af, om kommandoen kan udføres på den givne ViewModel.
            /// </summary>
            /// <param name="viewModel">ViewModel, hvorpå kommandoen skal udføres.</param>
            /// <returns></returns>
            protected override bool CanExecute(IViewModel viewModel)
            {
                if (OnCanExecute != null)
                {
                    OnCanExecute.Invoke(viewModel);
                }
                return base.CanExecute(viewModel);
            }

            /// <summary>
            /// Udfører kommandoen på den givne ViewModel.
            /// </summary>
            /// <param name="viewModel">ViewModel, hvorpå kommandoen skal udføres.</param>
            protected override void Execute(IViewModel viewModel)
            {
                if (OnExecute != null)
                {
                    OnExecute.Invoke(viewModel);
                }
            }

            #endregion
        }

        /// <summary>
        /// Tester, at konstruktøren initierer basisfunktionalitet til en kommando, der skal udføres på en ViewModel.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererViewModelCommandBase()
        {
            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var command = new MyViewModelCommand(exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);

            var exceptionHandler = command.GetExceptionHandler();
            Assert.That(exceptionHandler, Is.Not.Null);
            Assert.That(exceptionHandler, Is.EqualTo(exceptionHandlerViewModelMock));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis den ViewModel, der skal håndterer exceptions, er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisExceptionHandlerViewModelErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new MyViewModelCommand(null));
        }

        /// <summary>
        /// Tester, at CanExecute returnerer false, hvis parameteren ikke er af typen IViewModel.
        /// </summary>
        [Test]
        public void TestAtCanExecuteReturnererFalseHvisParameterIkkeErAfTypenIViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var command = new MyViewModelCommand(fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(command, Is.Not.Null);

            var result = command.CanExecute(fixture.Create<object>());
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Tester, at CanExecute returnerer true, hvis parameteren er af typen IViewModel.
        /// </summary>
        [Test]
        public void TestAtCanExecuteReturnererTrueHvisParameterErAfTypenIViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IViewModel>()));

            var command = new MyViewModelCommand(fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(command, Is.Not.Null);

            var result = command.CanExecute(fixture.Create<IViewModel>());
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tester, at CanExecute overfører ViewModel til metoden i den nedarvede klasse.
        /// </summary>
        [Test]
        public void TestAtCanExecuteViderestillerViewModelTilOverride()
        {
            var fixture = new Fixture();
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IViewModel>()));

            var command = new MyViewModelCommand(fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(command, Is.Not.Null);

            var viewModelMock = fixture.Create<IViewModel>();
            var actionCalled = false;
            command.OnCanExecute += vm =>
                {
                    Assert.That(vm, Is.Not.Null);
                    Assert.That(vm, Is.EqualTo(viewModelMock));
                    actionCalled = true;
                };

            Assert.That(actionCalled, Is.False);
            var result = command.CanExecute(viewModelMock);
            Assert.That(result, Is.True);
            Assert.That(actionCalled, Is.True);
        }

        /// <summary>
        /// Tester, at Execute kalder exceptionhandleren med en IntranetGuiSystemException, hvis parameteren ikke er af typen IViewModel.
        /// </summary>
        [Test]
        public void TestAtExecuteKalderExceptionHandlerViewModelMedIntranetGuiSystemExceptionHvisParameterIkkeErAfTypenIViewModel()
        {
            var fixture = new Fixture();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var command = new MyViewModelCommand(exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);

            command.Execute(fixture.Create<object>());

            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.TypeOf));
        }

        /// <summary>
        /// Tester, at Execute kalder metoden i den nedarvede klasse.
        /// </summary>
        [Test]
        public void TestAtExecuteKalderOveride()
        {
            var fixture = new Fixture();
            fixture.Customize<IViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IViewModel>()));

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var command = new MyViewModelCommand(exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);

            var actionCalled = false;
            command.OnExecute += vm => actionCalled = true;

            Assert.That(actionCalled, Is.False);
            command.Execute(fixture.Create<IViewModel>());
            Assert.That(actionCalled, Is.True);

            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Execute overfører ViewModel til metoden i den nedarvede klasse.
        /// </summary>
        [Test]
        public void TestAtExecuteViderestillerViewModelTilOveride()
        {
            var fixture = new Fixture();
            fixture.Customize<IViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IViewModel>()));

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var command = new MyViewModelCommand(exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);

            var viewModelMock = fixture.Create<IViewModel>();
            var actionCalled = false;
            command.OnExecute += vm =>
                {
                    Assert.That(vm, Is.Not.Null);
                    Assert.That(vm, Is.EqualTo(viewModelMock));
                    actionCalled = true;
                };

            Assert.That(actionCalled, Is.False);
            command.Execute(viewModelMock);
            Assert.That(actionCalled, Is.True);

            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Execute kalder exceptionhandleren med en IntranetGuiRepositoryException ved IntranetGuiRepositoryException.
        /// </summary>
        [Test]
        public void TestAtExecuteKalderExceptionHandlerViewModelMedIntranetGuiRepositoryExceptionVedIntranetGuiRepositoryException()
        {
            var fixture = new Fixture();
            fixture.Customize<IViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IViewModel>()));

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var command = new MyViewModelCommand(exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);

            var intranetGuiRepositoryException = fixture.Create<IntranetGuiRepositoryException>();
            command.OnExecute += vm =>
                {
                    throw intranetGuiRepositoryException;
                };

            command.Execute(fixture.Create<IViewModel>());

            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiRepositoryException>.Is.Equal(intranetGuiRepositoryException)));
        }

        /// <summary>
        /// Tester, at Execute kalder exceptionhandleren med en IntranetGuiBusinessException ved IntranetGuiBusinessException.
        /// </summary>
        [Test]
        public void TestAtExecuteKalderExceptionHandlerViewModelMedIntranetGuiBusinessExceptionVedIntranetGuiBusinessException()
        {
            var fixture = new Fixture();
            fixture.Customize<IViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IViewModel>()));

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var command = new MyViewModelCommand(exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);

            var intranetGuiBusinessException = fixture.Create<IntranetGuiBusinessException>();
            command.OnExecute += vm =>
                {
                    throw intranetGuiBusinessException;
                };

            command.Execute(fixture.Create<IViewModel>());

            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiBusinessException>.Is.Equal(intranetGuiBusinessException)));
        }

        /// <summary>
        /// Tester, at Execute kalder exceptionhandleren med en IntranetGuiSystemException ved IntranetGuiSystemException.
        /// </summary>
        [Test]
        public void TestAtExecuteKalderExceptionHandlerViewModelMedIntranetGuiSystemExceptionVedIntranetGuiSystemException()
        {
            var fixture = new Fixture();
            fixture.Customize<IViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IViewModel>()));

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var command = new MyViewModelCommand(exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);

            var intranetGuiSystemException = fixture.Create<IntranetGuiSystemException>();
            command.OnExecute += vm =>
                {
                    throw intranetGuiSystemException;
                };

            command.Execute(fixture.Create<IViewModel>());

            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.Equal(intranetGuiSystemException)));
        }

        /// <summary>
        /// Tester, at Execute kalder exceptionhandleren med en IntranetGuiSystemException ved Exception.
        /// </summary>
        [Test]
        public void TestAtExecuteKalderExceptionHandlerViewModelMedIntranetGuiSystemExceptionVedException()
        {
            var fixture = new Fixture();
            fixture.Customize<IViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IViewModel>()));

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var command = new MyViewModelCommand(exceptionHandlerViewModelMock);
            Assert.That(command, Is.Not.Null);

            var executeException = fixture.Create<Exception>();
            command.OnExecute += vm =>
                {
                    throw executeException;
                };

            command.Execute(fixture.Create<IViewModel>());

            exceptionHandlerViewModelMock.AssertWasCalled(m => m.HandleException(Arg<IntranetGuiSystemException>.Is.NotNull));
        }
    }
}
