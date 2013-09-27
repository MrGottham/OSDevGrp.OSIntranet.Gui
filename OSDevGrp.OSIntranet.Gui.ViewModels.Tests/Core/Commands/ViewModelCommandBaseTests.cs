using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces;
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
            var command = new MyViewModelCommand();
            Assert.That(command, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at CanExecute returnerer false, hvis parameteren ikke er af typen IViewModel.
        /// </summary>
        [Test]
        public void TestAtCanExecuteReturnererFalseHvisParameterIkkeErAfTypenIViewModel()
        {
            var fixture = new Fixture();

            var command = new MyViewModelCommand();
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
            fixture.Customize<IViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IViewModel>()));

            var command = new MyViewModelCommand();
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
            fixture.Customize<IViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IViewModel>()));

            var command = new MyViewModelCommand();
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
        /// Tester, at Execute kaster en IntranetGuiSystemException, hvis parameteren ikke er af typen IViewModel.
        /// </summary>
        [Test]
        public void TestAtExecuteKasterIntranetGuiSystemExceptionHvisParameterIkkeErAfTypenIViewModel()
        {
            var fixture = new Fixture();

            var command = new MyViewModelCommand();
            Assert.That(command, Is.Not.Null);

            Assert.Throws<IntranetGuiSystemException>(() => command.Execute(fixture.Create<object>()));
        }

        /// <summary>
        /// Tester, at Execute kalder metoden i den nedarvede klasse.
        /// </summary>
        [Test]
        public void TestAtExecuteKalderOveride()
        {
            var fixture = new Fixture();
            fixture.Customize<IViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IViewModel>()));

            var command = new MyViewModelCommand();
            Assert.That(command, Is.Not.Null);

            var actionCalled = false;
            command.OnExecute += vm => actionCalled = true;

            Assert.That(actionCalled, Is.False);
            command.Execute(fixture.Create<IViewModel>());
            Assert.That(actionCalled, Is.True);
        }

        /// <summary>
        /// Tester, at Execute overfører ViewModel til metoden i den nedarvede klasse.
        /// </summary>
        [Test]
        public void TestAtExecuteViderestillerViewModelTilOveride()
        {
            var fixture = new Fixture();
            fixture.Customize<IViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IViewModel>()));

            var command = new MyViewModelCommand();
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
        }

        /// <summary>
        /// Tester, at Execute kaster en IntranetGuiRepositoryException ved IntranetGuiRepositoryException.
        /// </summary>
        [Test]
        public void TestAtExecuteKasterIntranetGuiRepositoryExceptionVedIntranetGuiRepositoryException()
        {
            var fixture = new Fixture();
            fixture.Customize<IViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IViewModel>()));

            var command = new MyViewModelCommand();
            Assert.That(command, Is.Not.Null);

            var intranetGuiRepositoryException = fixture.Create<IntranetGuiRepositoryException>();
            command.OnExecute += vm =>
                {
                    throw intranetGuiRepositoryException;
                };

            var exception = Assert.Throws<IntranetGuiRepositoryException>(() => command.Execute(fixture.Create<IViewModel>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(intranetGuiRepositoryException.Message));
        }

        /// <summary>
        /// Tester, at Execute kaster en IntranetGuiBusinessException ved IntranetGuiBusinessException.
        /// </summary>
        [Test]
        public void TestAtExecuteKasterIntranetGuiBusinessExceptionVedIntranetGuiBusinessException()
        {
            var fixture = new Fixture();
            fixture.Customize<IViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IViewModel>()));

            var command = new MyViewModelCommand();
            Assert.That(command, Is.Not.Null);

            var intranetGuiBusinessException = fixture.Create<IntranetGuiBusinessException>();
            command.OnExecute += vm =>
                {
                    throw intranetGuiBusinessException;
                };

            var exception = Assert.Throws<IntranetGuiBusinessException>(() => command.Execute(fixture.Create<IViewModel>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(intranetGuiBusinessException.Message));
        }

        /// <summary>
        /// Tester, at Execute kaster en IntranetGuiSystemException ved IntranetGuiSystemException.
        /// </summary>
        [Test]
        public void TestAtExecuteKasterIntranetGuiSystemExceptionVedIntranetGuiSystemException()
        {
            var fixture = new Fixture();
            fixture.Customize<IViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IViewModel>()));

            var command = new MyViewModelCommand();
            Assert.That(command, Is.Not.Null);

            var intranetGuiSystemException = fixture.Create<IntranetGuiSystemException>();
            command.OnExecute += vm =>
                {
                    throw intranetGuiSystemException;
                };

            var exception = Assert.Throws<IntranetGuiSystemException>(() => command.Execute(fixture.Create<IViewModel>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(intranetGuiSystemException.Message));
        }

        /// <summary>
        /// Tester, at Execute kaster en IntranetGuiSystemException ved Exception.
        /// </summary>
        [Test]
        public void TestAtExecuteKasterIntranetGuiSystemExceptionVedException()
        {
            var fixture = new Fixture();
            fixture.Customize<IViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IViewModel>()));

            var command = new MyViewModelCommand();
            Assert.That(command, Is.Not.Null);

            var executeException = fixture.Create<Exception>();
            command.OnExecute += vm =>
                {
                    throw executeException;
                };

            var exception = Assert.Throws<IntranetGuiSystemException>(() => command.Execute(fixture.Create<IViewModel>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.CommandError, command.GetType().Name, executeException.Message)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.EqualTo(executeException));
        }
    }
}
