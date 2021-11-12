using System;
using System.Text;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core;
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
            var fixture = new Fixture();

            var exception = fixture.Create<Exception>();
            var detailBuilder = new StringBuilder(exception.Message);
            if (string.IsNullOrEmpty(exception.StackTrace) == false)
            {
                detailBuilder.AppendLine();
                detailBuilder.AppendLine(exception.StackTrace);
            }
            var exceptionViewModel = new ExceptionViewModel(exception);
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
            Assert.Throws<ArgumentNullException>(() => new ExceptionViewModel(null));
        }

        /// <summary>
        /// Tester, at Message returnerer fejlbesked fra exception.
        /// </summary>
        [Test]
        public void TestAtMessageReturnererFejlbeskedFraException()
        {
            var fixture = new Fixture();
            fixture.Customize<IntranetGuiRepositoryException>(e => e.FromFactory(() => new IntranetGuiRepositoryException(fixture.Create<string>(), fixture.Create<Exception>())));

            var exception = fixture.Create<IntranetGuiRepositoryException>();
            var exceptionViewModel = new ExceptionViewModel(exception);
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
            var fixture = new Fixture();
            fixture.Customize<IntranetGuiRepositoryException>(e => e.FromFactory(() => new IntranetGuiRepositoryException(fixture.Create<string>(), fixture.Create<Exception>())));

            var exception = fixture.Create<IntranetGuiRepositoryException>();
            var detailsBuilder = new StringBuilder(exception.Message);
            var detailBuilder = new StringBuilder(exception.Message);
            if (string.IsNullOrEmpty(exception.StackTrace) == false)
            {
                detailBuilder.AppendLine();
                detailBuilder.AppendLine(exception.StackTrace);
            }
            var innerException = exception.InnerException;
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

            var exceptionViewModel = new ExceptionViewModel(exception);
            Assert.That(exceptionViewModel, Is.Not.Null);
            Assert.That(exceptionViewModel.Details, Is.Not.Null);
            Assert.That(exceptionViewModel.Details, Is.Not.Empty);
            Assert.That(exceptionViewModel.Details, Is.EqualTo(detailsBuilder.ToString()));
        }
    }
}
