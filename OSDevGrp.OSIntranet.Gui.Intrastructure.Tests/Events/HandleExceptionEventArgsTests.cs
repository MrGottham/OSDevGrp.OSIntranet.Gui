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
            Assert.That(eventArgs.IsHandled, Is.False);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis den exception, der skal håndteres, er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisErrorErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new HandleExceptionEventArgs(null));
        }

        /// <summary>
        /// Tester, at sætteren til IsHandled opdaterer værdi for, om exception er blevet håndteret.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestAtIsHandledSetterOpdatererValue(bool value)
        {
            var fixture = new Fixture();

            var eventArgs = new HandleExceptionEventArgs(fixture.Create<IntranetGuiSystemException>());
            Assert.That(eventArgs, Is.Not.Null);

            eventArgs.IsHandled = value;
            Assert.That(eventArgs.IsHandled, Is.EqualTo(value));
        }
    }
}
