using System;
using System.IO;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Events;

namespace OSDevGrp.OSIntranet.Gui.Intrastructure.Tests.Events
{
    /// <summary>
    /// Tester argumenter til et event, der håndterer oprettelse af en stream.
    /// </summary>
    [TestFixture]
    public class HandleStreamCreationEventArgsTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer argumenter til et event, der håndterer oprettelse af en stream.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererHandleStreamCreationEventArgs()
        {
            var fixture = new Fixture();

            var creationContext = fixture.Create<object>();
            var eventArgs = new HandleStreamCreationEventArgs(creationContext);
            Assert.That(eventArgs, Is.Not.Null);
            Assert.That(eventArgs.CreationContext, Is.Not.Null);
            Assert.That(eventArgs.CreationContext, Is.EqualTo(creationContext));
            Assert.That(eventArgs.Result, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis kontekst, hvorfra stream skal oprettes, er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisCreationContextErNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new HandleStreamCreationEventArgs(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("creationContext"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at sætteren til Result opdaterere stream til resultatet.
        /// </summary>
        [Test]
        public void TestAtResultSetterOpdatererStream()
        {
            var fixture = new Fixture();

            var eventArgs = new HandleStreamCreationEventArgs(fixture.Create<object>());
            Assert.That(eventArgs, Is.Not.Null);
            Assert.That(eventArgs.Result, Is.Null);

            using (var stream = new MemoryStream())
            {
                eventArgs.Result = stream;
                Assert.That(eventArgs.Result, Is.Not.Null);
                Assert.That(eventArgs.Result, Is.EqualTo(stream));

                stream.Close();
            }
        }

        /// <summary>
        /// Tester, at sætteren til Result kaster en ArgumentNullException, hvis value er null.
        /// </summary>
        [Test]
        public void TestAtResultSetterKasterArgumentNullExceptionHvisValueErNull()
        {
            var fixture = new Fixture();

            var eventArgs = new HandleStreamCreationEventArgs(fixture.Create<object>());
            Assert.That(eventArgs, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => eventArgs.Result = null);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("value"));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
