using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Events;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Gui.Intrastructure.Tests.Events
{
    /// <summary>
    /// Tester argumenter til et event, der håndtere en exception i OS Intranet.
    /// </summary>
    [TestFixture]
    public class HandleExceptionEventArgsTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer argumenter til et event, der håndtere en exception i OS Intranet.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererHandleExceptionEventArgs()
        {
            var fixture = new Fixture();

            var error = fixture.Create<IntranetGuiSystemException>();
            var eventArgs = new HandleExceptionEventArgs(error);
            Assert.That(eventArgs, Is.Not.Null);
            Assert.That(eventArgs.Error, Is.Not.Null);
            Assert.That(eventArgs.Error, Is.EqualTo(error));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis den exception, der skal håndteres, er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisErrorErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new HandleExceptionEventArgs(null));
        }
    }
}
