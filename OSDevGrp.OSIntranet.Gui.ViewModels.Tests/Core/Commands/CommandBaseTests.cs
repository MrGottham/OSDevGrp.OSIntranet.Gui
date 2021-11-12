using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core.Commands;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Core.Commands
{
    /// <summary>
    /// Tester basisfunktionalitet til en kommando.
    /// </summary>
    [TestFixture]
    public class CommandBaseTests
    {
        /// <summary>
        /// Egen klasse til test af basisfunktionalitet til en kommando.
        /// </summary>
        private class MyCommand : CommandBase
        {
            #region Properties
            
            /// <summary>
            /// Angivelse af, om CanExecute er kaldt.
            /// </summary>
            public bool IsCanExecuteCalled
            {
                get; 
                private set;
            }
            
            /// <summary>
            /// Angivelse af, om Execute er kaldt.
            /// </summary>
            public bool IsExecuteCalled
            {
                get; 
                private set;
            }
            
            #endregion
            
            #region Methods

            /// <summary>
            /// Returnerer angivelse af, om kommandoen kan udføres.
            /// </summary>
            /// <param name="parameter">Parameter til kommandoen.</param>
            /// <returns>Angivelse af, om kommandoen kan udføres.</returns>
            public override bool CanExecute(object parameter)
            {
                IsCanExecuteCalled = true;
                return true;
            }

            /// <summary>
            /// Udfører kommandoen.
            /// </summary>
            /// <param name="parameter">Parameter til kommandoen.</param>
            public override void Execute(object parameter)
            {
                IsExecuteCalled = true;
            }
            
            #endregion
        }

        /// <summary>
        /// Tester, at konstruktøren initierer basisfunktionalitet til en kommando.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererCommandBase()
        {
            var command = new MyCommand();
            Assert.That(command, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at CanExecute kaldes.
        /// </summary>
        [Test]
        public void TestAtCanExecuteKaldes()
        {
            var fixture = new Fixture();

            var command = new MyCommand();
            Assert.That(command, Is.Not.Null);
            Assert.That(command.IsCanExecuteCalled, Is.False);

            var result = command.CanExecute(fixture.Create<object>());
            Assert.That(result, Is.True);

            Assert.That(command.IsCanExecuteCalled, Is.True);
        }

        /// <summary>
        /// Tester, at Execute kaldes.
        /// </summary>
        [Test]
        public void TestAtxecuteKaldes()
        {
            var fixture = new Fixture();

            var command = new MyCommand();
            Assert.That(command, Is.Not.Null);
            Assert.That(command.IsExecuteCalled, Is.False);

            command.Execute(fixture.Create<object>());

            Assert.That(command.IsExecuteCalled, Is.True);
        }
    }
}
