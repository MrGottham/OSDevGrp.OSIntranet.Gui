using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Events;

namespace OSDevGrp.OSIntranet.Gui.Intrastructure.Tests.Events
{
    /// <summary>
    /// Tester implementeringen af et tomt argument til et event, der kan rejses i OS Intranet.
    /// </summary>
    [TestFixture]
    public class EmptyEventArgsTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer et tomt argument til et event, der kan rejses i OS Intranet.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererEmptyEventArgs()
        {
            var emptyEventArgs = new EmptyEventArgs();
            Assert.That(emptyEventArgs, Is.Not.Null);
        }
    }
}
