using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core.Commands;
using Rhino.Mocks;

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
            var fixture = new Fixture();
            fixture.Customize<ICommand>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommand>()));

            var command = new CommandCollectionExecuterCommand(fixture.CreateMany<ICommand>().ToList());
            Assert.That(command, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis listen af kommandoer, som denne kommando skal udføre, er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisCommandCollectionErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new CommandCollectionExecuterCommand(null));
        }

        /// <summary>
        /// Tester, at CanExecute returnerer false, hvis listen af kommandoer, som denne kommando skal udføre, er tom.
        /// </summary>
        [Test]
        public void TestAtCanExecuteReturnererFalseHvisCommandCollectionErEmpty()
        {
            var fixture = new Fixture();

            var commandMockCollection = new List<ICommand>(0);
            Assert.That(commandMockCollection, Is.Not.Null);
            Assert.That(commandMockCollection, Is.Empty);

            var command = new CommandCollectionExecuterCommand(commandMockCollection);
            Assert.That(command, Is.Not.Null);

            Assert.That(command.CanExecute(fixture.Create<object>()), Is.False);
        }

        /// <summary>
        /// Tester, at CanExecute returnerer false, hvis ingen af kommandoerne i listen af kommandoer, som denne kommando skal udføre, kan udføres.
        /// </summary>
        [Test]
        public void TestAtCanExecuteReturnererFalseHvisCanExecuteOnCommandsReturnererFalse()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommand>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<ICommand>();
                    mock.Expect(m => m.CanExecute(Arg<object>.Is.Anything))
                        .Return(false)
                        .Repeat.Any();
                    return mock;
                }));

            var commandMockCollection = fixture.CreateMany<ICommand>(25).ToList();
            Assert.That(commandMockCollection, Is.Not.Null);
            Assert.That(commandMockCollection, Is.Not.Empty);

            var command = new CommandCollectionExecuterCommand(commandMockCollection);
            Assert.That(command, Is.Not.Null);

            var parameter = fixture.Create<object>();
            var result = command.CanExecute(parameter); 
            Assert.That(result, Is.False);

            foreach (var commandMock in commandMockCollection)
            {
                commandMock.AssertWasCalled(m => m.CanExecute(Arg<object>.Is.Equal(parameter)));
            }
        }

        /// <summary>
        /// Tester, at CanExecute returnerer true, hvis mindst en af kommandoerne i listen af kommandoer, som denne kommando skal udføre, kan udføres.
        /// </summary>
        [Test]
        public void TestAtCanExecuteReturnererTrueHvisCanExecuteOnOneCommandReturnererTrue()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommand>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<ICommand>();
                    mock.Expect(m => m.CanExecute(Arg<object>.Is.Anything))
                        .Return(false)
                        .Repeat.Any();
                    return mock;
                }));

            var executeableCommand = MockRepository.GenerateMock<ICommand>();
            executeableCommand.Expect(m => m.CanExecute(Arg<object>.Is.Anything))
                              .Return(true)
                              .Repeat.Any();

            var commandMockCollection = fixture.CreateMany<ICommand>(24).ToList();
            commandMockCollection.Add(executeableCommand);
            Assert.That(commandMockCollection, Is.Not.Null);
            Assert.That(commandMockCollection, Is.Not.Empty);

            var command = new CommandCollectionExecuterCommand(commandMockCollection);
            Assert.That(command, Is.Not.Null);

            var parameter = fixture.Create<object>();
            var result = command.CanExecute(parameter); 
            Assert.That(result, Is.True);

            foreach (var commandMock in commandMockCollection)
            {
                commandMock.AssertWasCalled(m => m.CanExecute(Arg<object>.Is.Equal(parameter)));
            }
        }

        /// <summary>
        /// Tester, at Execute udfører udførbare kommandoer i listen af kommandoer, som denne kommando skal udføre.
        /// </summary>
        [Test]
        public void TestAtExecuteExecutesExecutableCommands()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommand>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<ICommand>();
                    mock.Expect(m => m.CanExecute(Arg<object>.Is.Anything))
                        .Return(false)
                        .Repeat.Any();
                    return mock;
                }));

            var executeableCommand = MockRepository.GenerateMock<ICommand>();
            executeableCommand.Expect(m => m.CanExecute(Arg<object>.Is.Anything))
                              .Return(true)
                              .Repeat.Any();

            var commandMockCollection = fixture.CreateMany<ICommand>(24).ToList();
            commandMockCollection.Add(executeableCommand);
            Assert.That(commandMockCollection, Is.Not.Null);
            Assert.That(commandMockCollection, Is.Not.Empty);

            var command = new CommandCollectionExecuterCommand(commandMockCollection);
            Assert.That(command, Is.Not.Null);

            var parameter = fixture.Create<object>();
            command.Execute(parameter);

            foreach (var commandMock in commandMockCollection)
            {
                commandMock.AssertWasCalled(m => m.CanExecute(Arg<object>.Is.Equal(parameter)));
            }
            for (var commandNo = 0; commandNo < commandMockCollection.Count - 1; commandNo++)
            {
                commandMockCollection.ElementAt(commandNo).AssertWasNotCalled(m => m.Execute(Arg<object>.Is.Anything));
            }
            executeableCommand.AssertWasCalled(m => m.Execute(Arg<object>.Is.Equal(parameter)));
        }
    }
}
