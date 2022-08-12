using System;
using System.Linq;
using System.Windows.Input;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using Text = OSDevGrp.OSIntranet.Gui.Resources.Text;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Core
{
    /// <summary>
    /// Tester ViewModel for en exceptionhandler.
    /// </summary>
    [TestFixture]
    public class ExceptionHandlerViewModelTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en ViewModel for en exceptionhandler. 
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererExceptionHandlerViewModel()
        {
            IExceptionHandlerViewModel exceptionHandlerViewModel = new ExceptionHandlerViewModel();
            Assert.That(exceptionHandlerViewModel, Is.Not.Null);
            Assert.That(exceptionHandlerViewModel.Last, Is.Null);
            Assert.That(exceptionHandlerViewModel.ShowLast, Is.False);
            Assert.That(exceptionHandlerViewModel.DisplayName, Is.Not.Null);
            Assert.That(exceptionHandlerViewModel.DisplayName, Is.Not.Empty);
            Assert.That(exceptionHandlerViewModel.DisplayName, Is.EqualTo(Resource.GetText(Text.ExceptionHandler)));
            Assert.That(exceptionHandlerViewModel.Exceptions, Is.Not.Null);
            Assert.That(exceptionHandlerViewModel.Exceptions, Is.Empty);
            Assert.That(exceptionHandlerViewModel.HideCommandText, Is.Not.Null);
            Assert.That(exceptionHandlerViewModel.HideCommandText, Is.Not.Empty);
            Assert.That(exceptionHandlerViewModel.HideCommandText, Is.EqualTo(Resource.GetText(Text.Hide)));
            Assert.That(exceptionHandlerViewModel.HideCommand, Is.Not.Null);
            Assert.That(exceptionHandlerViewModel.HideCommand, Is.TypeOf<RelayCommand>());
        }

        /// <summary>
        /// Tester, at HandleException kaster en ArgumentNullException, hvis exception er null.
        /// </summary>
        [Test]
        public void TestAtHandleExceptionKasterArgumentNullExceptionHvisExceptionErNull()
        {
            IExceptionHandlerViewModel exceptionHandlerViewModel = new ExceptionHandlerViewModel();
            Assert.That(exceptionHandlerViewModel, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => exceptionHandlerViewModel.HandleException(null));
        }

        /// <summary>
        /// Tester, at HandleException tilføjer exception til listen af håndterede exceptions.
        /// </summary>
        [Test]
        public void TestAtHandleExceptionAdderExceptionTilExceptions()
        {
            Fixture fixture = new Fixture();

            IExceptionHandlerViewModel exceptionHandlerViewModel = new ExceptionHandlerViewModel();
            Assert.That(exceptionHandlerViewModel, Is.Not.Null);
            Assert.That(exceptionHandlerViewModel.Exceptions, Is.Not.Null);
            Assert.That(exceptionHandlerViewModel.Exceptions, Is.Empty);

            exceptionHandlerViewModel.HandleException(fixture.Create<Exception>());
            Assert.That(exceptionHandlerViewModel.Exceptions, Is.Not.Null);
            Assert.That(exceptionHandlerViewModel.Exceptions, Is.Not.Empty);
            Assert.That(exceptionHandlerViewModel.Exceptions.Count(), Is.EqualTo(1));
        }

        /// <summary>
        /// Tester, at HandleException kalder OnHandleException, når en exception håndteres.
        /// </summary>
        [Test]
        public void TestAtHandleExceptionKalderOnHandleException()
        {
            Fixture fixture = new Fixture();

            IExceptionHandlerViewModel exceptionHandlerViewModel = new ExceptionHandlerViewModel();
            Assert.That(exceptionHandlerViewModel, Is.Not.Null);

            Exception exceptionToHandle = fixture.Create<Exception>();
            bool eventCalled = false;
            exceptionHandlerViewModel.OnHandleException += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.Error, Is.Not.Null);
                Assert.That(e.Error, Is.EqualTo(exceptionToHandle));
                Assert.That(e.IsHandled, Is.False);
                eventCalled = true;
            };

            Assert.That(eventCalled, Is.False);
            exceptionHandlerViewModel.HandleException(exceptionToHandle);
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at HandleException tilføjer exception til listen af håndterede exceptions, hvis OnHandleException sætter IsHandled til false.
        /// </summary>
        [Test]
        public void TestAtHandleExceptionAdderExceptionTilExceptionHvisOnHandleExceptionSetsIsHandledTilFalse()
        {
            Fixture fixture = new Fixture();

            IExceptionHandlerViewModel exceptionHandlerViewModel = new ExceptionHandlerViewModel();
            Assert.That(exceptionHandlerViewModel, Is.Not.Null);
            Assert.That(exceptionHandlerViewModel.Exceptions, Is.Not.Null);
            Assert.That(exceptionHandlerViewModel.Exceptions, Is.Empty);

            exceptionHandlerViewModel.OnHandleException += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                e.IsHandled = false;
            };

            exceptionHandlerViewModel.HandleException(fixture.Create<Exception>());
            Assert.That(exceptionHandlerViewModel.Exceptions, Is.Not.Null);
            Assert.That(exceptionHandlerViewModel.Exceptions, Is.Not.Empty);
            Assert.That(exceptionHandlerViewModel.Exceptions.Count(), Is.EqualTo(1));
        }

        /// <summary>
        /// Tester, at HandleException ikke tilføjer exception til listen af håndterede exceptions, hvis OnHandleException sætter IsHandled til true.
        /// </summary>
        [Test]
        public void TestAtHandleExceptionIkkeAdderExceptionTilExceptionHvisOnHandleExceptionSetsIsHandledTilTrue()
        {
            Fixture fixture = new Fixture();

            IExceptionHandlerViewModel exceptionHandlerViewModel = new ExceptionHandlerViewModel();
            Assert.That(exceptionHandlerViewModel, Is.Not.Null);
            Assert.That(exceptionHandlerViewModel.Exceptions, Is.Not.Null);
            Assert.That(exceptionHandlerViewModel.Exceptions, Is.Empty);

            exceptionHandlerViewModel.OnHandleException += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                e.IsHandled = true;
            };

            exceptionHandlerViewModel.HandleException(fixture.Create<Exception>());
            Assert.That(exceptionHandlerViewModel.Exceptions, Is.Not.Null);
            Assert.That(exceptionHandlerViewModel.Exceptions, Is.Empty);
        }

        /// <summary>
        /// Tester, at Last returnerer seneste håndterede exception.
        /// </summary>
        [Test]
        public void TestAtLastExceptionReturnererSenesteException()
        {
            Fixture fixture = new Fixture();

            IExceptionHandlerViewModel exceptionHandlerViewModel = new ExceptionHandlerViewModel();
            Assert.That(exceptionHandlerViewModel, Is.Not.Null);
            Assert.That(exceptionHandlerViewModel.Last, Is.Null);

            IntranetGuiRepositoryException repositoryException = fixture.Create<IntranetGuiRepositoryException>();
            exceptionHandlerViewModel.HandleException(repositoryException);
            Assert.That(exceptionHandlerViewModel.Last, Is.Not.Null);
            Assert.That(exceptionHandlerViewModel.Last.Message, Is.Not.Null);
            Assert.That(exceptionHandlerViewModel.Last.Message, Is.Not.Empty);
            Assert.That(exceptionHandlerViewModel.Last.Message, Is.EqualTo(repositoryException.Message));

            IntranetGuiSystemException systemException = fixture.Create<IntranetGuiSystemException>();
            exceptionHandlerViewModel.HandleException(systemException);
            Assert.That(exceptionHandlerViewModel.Last, Is.Not.Null);
            Assert.That(exceptionHandlerViewModel.Last.Message, Is.Not.Null);
            Assert.That(exceptionHandlerViewModel.Last.Message, Is.Not.Empty);
            Assert.That(exceptionHandlerViewModel.Last.Message, Is.EqualTo(systemException.Message));
        }

        /// <summary>
        /// Tester, at HandleException rejser PropertyChanged, når et exception håndteres.
        /// </summary>
        [Test]
        [TestCase("Exceptions")]
        [TestCase("Last")]
        [TestCase("ShowLast")]
        public void TestAtHandleExceptionRejserPropertyChangedVedAddException(string propertyNameToRaise)
        {
            Fixture fixture = new Fixture();

            IExceptionHandlerViewModel exceptionHandlerViewModel = new ExceptionHandlerViewModel();
            Assert.That(exceptionHandlerViewModel, Is.Not.Null);

            bool eventCalled = false;
            exceptionHandlerViewModel.PropertyChanged += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Empty);
                if (string.CompareOrdinal(e.PropertyName, propertyNameToRaise) == 0)
                {
                    eventCalled = true;
                }
            };

            Assert.That(eventCalled, Is.False);
            exceptionHandlerViewModel.HandleException(fixture.Create<Exception>());
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at ShowLast returnerer true, når en exception håndteres.
        /// </summary>
        [Test]
        public void TestAtShowLastReturnererTrueVedHandleException()
        {
            Fixture fixture = new Fixture();

            IExceptionHandlerViewModel exceptionHandlerViewModel = new ExceptionHandlerViewModel();
            Assert.That(exceptionHandlerViewModel, Is.Not.Null);
            Assert.That(exceptionHandlerViewModel.ShowLast, Is.False);

            exceptionHandlerViewModel.HandleException(fixture.Create<Exception>());
            Assert.That(exceptionHandlerViewModel.ShowLast, Is.True);
        }

        /// <summary>
        /// Tester, at sætteren til ShowLast rejser PropertyChanged ved rettelse af ShowLast.
        /// </summary>
        [Test]
        public void TestAtShowLastSetterRejserPropertyChangedVedRettelseAfShowLast()
        {
            IExceptionHandlerViewModel exceptionHandlerViewModel = new ExceptionHandlerViewModel();
            Assert.That(exceptionHandlerViewModel, Is.Not.Null);

            bool eventCalled = false;
            exceptionHandlerViewModel.PropertyChanged += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Empty);
                Assert.That(e.PropertyName, Is.EqualTo("ShowLast"));
                eventCalled = true;
            };

            Assert.That(eventCalled, Is.False);
            exceptionHandlerViewModel.ShowLast = exceptionHandlerViewModel.ShowLast;
            Assert.That(eventCalled, Is.False);
            exceptionHandlerViewModel.ShowLast = !exceptionHandlerViewModel.ShowLast;
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at Excecute på HideCommand sætter ShowLast til false.
        /// </summary>
        [Test]
        public void TestAtExecuteOnHideCommandSetsShowLastTilFalse()
        {
            Fixture fixture = new Fixture();

            IExceptionHandlerViewModel exceptionHandlerViewModel = new ExceptionHandlerViewModel();
            Assert.That(exceptionHandlerViewModel, Is.Not.Null);

            exceptionHandlerViewModel.HandleException(fixture.Create<Exception>());
            Assert.That(exceptionHandlerViewModel.ShowLast, Is.True);

            ICommand hideCommand = exceptionHandlerViewModel.HideCommand;
            Assert.That(hideCommand, Is.Not.Null);
            Assert.That(hideCommand.CanExecute(null), Is.True);

            hideCommand.Execute(null);
            Assert.That(exceptionHandlerViewModel.ShowLast, Is.False);
        }
    }
}