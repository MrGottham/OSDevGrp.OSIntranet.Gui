using System;
using System.Collections.Generic;
using System.Windows.Input;
using AutoFixture;
using AutoFixture.Kernel;
using Moq;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests
{
    internal static class CommandBuilder
    {
        internal static IEnumerable<ICommand> BuildCommandCollection(this ISpecimenBuilder fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            Random random = new Random(fixture.Create<int>());

            List<ICommand> commandCollection = new List<ICommand>(random.Next(5, 25));
            while (commandCollection.Count < commandCollection.Capacity)
            {
                commandCollection.Add(fixture.BuildCommand());
            }

            return commandCollection;
        }

        internal static ICommand BuildCommand(this ISpecimenBuilder fixture, bool canExecute = true)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return fixture.BuildCommandMock(canExecute).Object;
        }

        internal static Mock<ICommand> BuildCommandMock(this ISpecimenBuilder fixture, bool canExecute = true)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            Mock<ICommand> mock = new Mock<ICommand>();
            mock.Setup(m => m.CanExecute(It.IsAny<object>()))
                .Returns(canExecute);
            return mock;
        }
    }
}