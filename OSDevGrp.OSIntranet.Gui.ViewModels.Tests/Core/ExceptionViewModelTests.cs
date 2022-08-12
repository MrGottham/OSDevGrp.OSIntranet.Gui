using System;
using System.Text;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using Text = OSDevGrp.OSIntranet.Gui.Resources.Text;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Core
{
    /// <summary>
    /// Tester ViewModel for en præsenterbar exception.
    /// </summary>
    [TestFixture]
    public class ExceptionViewModelTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en ViewModel for en præsenterbar exception.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererExceptionViewModel()
        {
            Fixture fixture = new Fixture();

            Exception exception = fixture.Create<Exception>();
            StringBuilder detailBuilder = new StringBuilder(exception.Message);
            if (string.IsNullOrEmpty(exception.StackTrace) == false)
            {
                detailBuilder.AppendLine();
                detailBuilder.AppendLine(exception.StackTrace);
            }

            IExceptionViewModel exceptionViewModel = new ExceptionViewModel(exception);
            Assert.That(exceptionViewModel, Is.Not.Null);
            Assert.That(exceptionViewModel.Message, Is.Not.Null);
            Assert.That(exceptionViewModel.Message, Is.Not.Empty);
            Assert.That(exceptionViewModel.Message, Is.EqualTo(exception.Message));
            Assert.That(exceptionViewModel.Details, Is.Not.Null);
            Assert.That(exceptionViewModel.Details, Is.Not.Empty);
            Assert.That(exceptionViewModel.Details, Is.EqualTo(detailBuilder.ToString()));
            Assert.That(exceptionViewModel.DisplayName, Is.Not.Null);
            Assert.That(exceptionViewModel.DisplayName, Is.Not.Empty);
            Assert.That(exceptionViewModel.DisplayName, Is.EqualTo(Resource.GetText(Text.Exception)));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis Exception er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisExceptionErNull()
        {
            // ReSharper disable ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new ExceptionViewModel(null));
            // ReSharper restore ObjectCreationAsStatement
        }

        /// <summary>
        /// Tester, at Message returnerer fejlbesked fra exception.
        /// </summary>
        [Test]
        public void TestAtMessageReturnererFejlbeskedFraException()
        {
            Fixture fixture = new Fixture();

            IntranetGuiRepositoryException exception = new IntranetGuiRepositoryException(fixture.Create<string>(), fixture.Create<Exception>();

            IExceptionViewModel exceptionViewModel = new ExceptionViewModel(exception);
            Assert.That(exceptionViewModel, Is.Not.Null);
            Assert.That(exceptionViewModel.Message, Is.Not.Null);
            Assert.That(exceptionViewModel.Message, Is.Not.Empty);
            Assert.That(exceptionViewModel.Message, Is.EqualTo(exception.Message));
        }

        /// <summary>
        /// Tester, at Details returnerer detaljeret fejlbesked.
        /// </summary>
        [Test]
        public void TestAtDetailsReturnererDetaljeretFejlbesked()
        {
            Fixture fixture = new Fixture();

            IntranetGuiRepositoryException exception = new IntranetGuiRepositoryException(fixture.Create<string>(), fixture.Create<Exception>();
            StringBuilder detailsBuilder = new StringBuilder(exception.Message);
            StringBuilder detailBuilder = new StringBuilder(exception.Message);
            if (string.IsNullOrEmpty(exception.StackTrace) == false)
            {
                detailBuilder.AppendLine();
                detailBuilder.AppendLine(exception.StackTrace);
            }

            Exception innerException = exception.InnerException;
            while (innerException != null)
            {
                detailsBuilder.AppendLine();
                detailsBuilder.AppendLine(innerException.Message);
                if (string.IsNullOrEmpty(innerException.StackTrace) == false)
                {
                    detailsBuilder.AppendLine(innerException.StackTrace);
                }
                innerException = innerException.InnerException;
            }

            IExceptionViewModel exceptionViewModel = new ExceptionViewModel(exception);
            Assert.That(exceptionViewModel, Is.Not.Null);
            Assert.That(exceptionViewModel.Details, Is.Not.Null);
            Assert.That(exceptionViewModel.Details, Is.Not.Empty);
            Assert.That(exceptionViewModel.Details, Is.EqualTo(detailsBuilder.ToString()));
        }
    }
}