using System;
using System.Linq;
using System.Windows.Input;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core.Commands;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Core.Commands
{
    /// <summary>
    /// Tester kommando, der kan udføre en til flere kommandoer.
    /// </summary>
    [TestFixture]
    public class CommandCollectionExecuterCommandTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en kommando, der kan udføre en til flere kommandoer.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererCommandCollectionExecuterCommand()
        {
            Fixture fixture = new Fixture();

            ICommand command = new CommandCollectionExecuterCommand(fixture.BuildCommandCollection());
            Assert.That(command, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis listen af kommandoer, som denne kommando skal udføre, er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisCommandCollectionErNull()
        {
            // ReSharper disable ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new CommandCollectionExecuterCommand(null));
            // ReSharper restore ObjectCreationAsStatement
        }

        /// <summary>
        /// Tester, at CanExecute returnerer false, hvis listen af kommandoer, som denne kommando skal udføre, er tom.
        /// </summary>
        [Test]
        public void TestAtCanExecuteReturnererFalseHvisCommandCollectionErEmpty()
        {
            Fixture fixture = new Fixture();

            ICommand[] commandMockCollection = Array.Empty<ICommand>();
            Assert.That(commandMockCollection, Is.Not.Null);
            Assert.That(commandMockCollection, Is.Empty);

            ICommand command = new CommandCollectionExecuterCommand(commandMockCollection);
            Assert.That(command, Is.Not.Null);

            Assert.That(command.CanExecute(fixture.Create<object>()), Is.False);
        }

        /// <summary>
        /// Tester, at CanExecute returnerer false, hvis ingen af kommandoerne i listen af kommandoer, som denne kommando skal udføre, kan udføres.
        /// </summary>
        [Test]
        public void TestAtCanExecuteReturnererFalseHvisCanExecuteOnCommandsReturnererFalse()
        {
            Fixture fixture = new Fixture();

            Mock<ICommand>[] commandMockCollection =
            {
                fixture.BuildCommandMock(false),
                fixture.BuildCommandMock(false),
                fixture.BuildCommandMock(false),
                fixture.BuildCommandMock(false),
                fixture.BuildCommandMock(false),
                fixture.BuildCommandMock(false),
                fixture.BuildCommandMock(false)
            };
            Assert.That(commandMockCollection, Is.Not.Null);
            Assert.That(commandMockCollection, Is.Not.Empty);

            ICommand command = new CommandCollectionExecuterCommand(commandMockCollection.Select(commandMock => commandMock.Object).ToArray());
            Assert.That(command, Is.Not.Null);

            object parameter = fixture.Create<object>();
            bool result = command.CanExecute(parameter); 
            Assert.That(result, Is.False);

            foreach (var commandMock in commandMockCollection)
            {
                commandMock.Verify(m => m.CanExecute(It.Is<object>(value => value != null && value == parameter)), Times.Once);
            }
        }

        /// <summary>
        /// Tester, at CanExecute returnerer true, hvis mindst en af kommandoerne i listen af kommandoer, som denne kommando skal udføre, kan udføres.
        /// </summary>
        [Test]
        public void TestAtCanExecuteReturnererTrueHvisCanExecuteOnOneCommandReturnererTrue()
        {
            Fixture fixture = new Fixture();

            Mock<ICommand>[] commandMockCollection =
            {
                fixture.BuildCommandMock(false),
                fixture.BuildCommandMock(false),
                fixture.BuildCommandMock(false),
                fixture.BuildCommandMock(),
                fixture.BuildCommandMock(false),
                fixture.BuildCommandMock(false),
                fixture.BuildCommandMock(false)
            };
            Assert.That(commandMockCollection, Is.Not.Null);
            Assert.That(commandMockCollection, Is.Not.Empty);

            ICommand command = new CommandCollectionExecuterCommand(commandMockCollection.Select(commandMock => commandMock.Object).ToArray());
            Assert.That(command, Is.Not.Null);

            object parameter = fixture.Create<object>();
            bool result = command.CanExecute(parameter);
            Assert.That(result, Is.True);

            foreach (var commandMock in commandMockCollection)
            {
                commandMock.Verify(m => m.CanExecute(It.Is<object>(value => value != null && value == parameter)), Times.Once);
            }
        }

        /// <summary>
        /// Tester, at Execute udfører udførbare kommandoer i listen af kommandoer, som denne kommando skal udføre.
        /// </summary>
        [Test]
        public void TestAtExecuteExecutesExecutableCommands()
        {
            Fixture fixture = new Fixture();

            Mock<ICommand>[] executableCommandMockCollection =
            {
                fixture.BuildCommandMock(),
                fixture.BuildCommandMock(),
                fixture.BuildCommandMock(),
                fixture.BuildCommandMock(),
            };
            Assert.That(executableCommandMockCollection, Is.Not.Null);
            Assert.That(executableCommandMockCollection, Is.Not.Empty);

            Mock<ICommand>[] nonExecutableCommandMockCollection =
            {
                fixture.BuildCommandMock(false),
                fixture.BuildCommandMock(false),
                fixture.BuildCommandMock(false)
            };
            Assert.That(nonExecutableCommandMockCollection, Is.Not.Null);
            Assert.That(nonExecutableCommandMockCollection, Is.Not.Empty);

            ICommand command = new CommandCollectionExecuterCommand(executableCommandMockCollection.Concat(nonExecutableCommandMockCollection).Select(commandMock => commandMock.Object).ToArray());
            Assert.That(command, Is.Not.Null);

            object parameter = fixture.Create<object>();
            command.Execute(parameter);

            foreach (var commandMock in executableCommandMockCollection.Concat(nonExecutableCommandMockCollection))
            {
                commandMock.Verify(m => m.CanExecute(It.Is<object>(value => value != null && value == parameter)), Times.Once);
            }
            foreach (var commandMock in executableCommandMockCollection)
            {
                commandMock.Verify(m => m.Execute(It.Is<object>(value => value != null && value == parameter)), Times.Once);
            }
            foreach (var commandMock in nonExecutableCommandMockCollection)
            {
                commandMock.Verify(m => m.Execute(It.IsAny<object>()), Times.Never);
            }
        }
    }
}