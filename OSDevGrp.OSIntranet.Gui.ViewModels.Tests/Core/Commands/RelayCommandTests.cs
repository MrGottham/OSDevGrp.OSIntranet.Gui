using System;
using System.Windows.Input;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core.Commands;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Core.Commands
{
    /// <summary>
    /// Tester relay kommando.
    /// </summary>
    [TestFixture]
    public class RelayCommandTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en relay kommando uden et udtryk, der angiver, om kommandoen kan udføres.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererRelayCommandUdenPredicate()
        {
            Fixture fixture = new Fixture();

            Action<object> execute = obj => { };
            execute.Invoke(fixture.Create<object>());

            ICommand command = new RelayCommand(execute);
            Assert.That(command, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren initierer en relay kommando med et udtryk, der angiver, om kommandoen kan udføres.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererRelayCommandMedPredicate()
        {
            Fixture fixture = new Fixture();

            Action<object> execute = obj => { };
            execute.Invoke(fixture.Create<object>());

            Predicate<object> canExecute = obj => true;
            bool result = canExecute.Invoke(fixture.Create<object>());
            Assert.That(result, Is.True);

            ICommand command = new RelayCommand(execute, canExecute);
            Assert.That(command, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis handlingen, der skal udføres ved udførelsen er kommandoen, er null.
        /// </summary>
        [Test]
        public void TestAtConstuctotKasterArgumentNullExceptionHvisActionErNull()
        {
            Fixture fixture = new Fixture();

            Predicate<object> canExecute = obj => true;
            bool result = canExecute.Invoke(fixture.Create<object>());
            Assert.That(result, Is.True);

            // ReSharper disable ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new RelayCommand(null));
            Assert.Throws<ArgumentNullException>(() => new RelayCommand(null, canExecute));
            // ReSharper restore ObjectCreationAsStatement
        }

        /// <summary>
        /// Tester, at CanExecute returnerer true, hvis udtrykket, der angiver, om kommandoen kan udføres, er null.
        /// </summary>
        [Test]
        public void TestAtCanExecuteReturnererTrueHvisPredicateErNull()
        {
            Fixture fixture = new Fixture();

            Action<object> execute = obj => { };
            execute.Invoke(fixture.Create<object>());

            ICommand command = new RelayCommand(execute);
            Assert.That(command, Is.Not.Null);

            bool result = command.CanExecute(fixture.Create<object>());
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tester, at CanExecute returnerer true, hvis udtrykket, der angiver, om kommandoen kan udføres, returnerer true.
        /// </summary>
        [Test]
        public void TestAtCanExecuteReturnererTrueHvisPredicateReturnererTrue()
        {
            Fixture fixture = new Fixture();

            Action<object> execute = obj => { };
            execute.Invoke(fixture.Create<object>());

            ICommand command = new RelayCommand(execute, obj => true);
            Assert.That(command, Is.Not.Null);

            bool result = command.CanExecute(fixture.Create<object>());
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tester, at CanExecute returnerer false, hvis udtrykket, der angiver, om kommandoen kan udføres, returnerer false.
        /// </summary>
        [Test]
        public void TestAtCanExecuteReturnererFalseHvisPredicateReturnererFalse()
        {
            Fixture fixture = new Fixture();

            Action<object> execute = obj => { };
            execute.Invoke(fixture.Create<object>());

            ICommand command = new RelayCommand(execute, obj => false);
            Assert.That(command, Is.Not.Null);

            bool result = command.CanExecute(fixture.Create<object>());
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Tester, at CanExecute overfører parameter til udtrykket, der angiver, om kommandoen kan udføres.
        /// </summary>
        [Test]
        public void TestAtCanExecuteVideresenderParameterTilPredicate()
        {
            Fixture fixture = new Fixture();

            Action<object> execute = obj => { };
            execute.Invoke(fixture.Create<object>());

            object parameter = fixture.Create<object>();
            Predicate<object> canExecute = obj =>
            {
                Assert.That(obj, Is.Not.Null);
                Assert.That(obj, Is.EqualTo(parameter));
                return true;
            };

            ICommand command = new RelayCommand(execute, canExecute);
            Assert.That(command, Is.Not.Null);

            bool result = command.CanExecute(parameter);
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tester, at Execute udfører handlingen, der skal udføres ved udførelse af kommandoen.
        /// </summary>
        [Test]
        public void TestAtExecuteExecutesAction()
        {
            Fixture fixture = new Fixture();

            bool isActionCalled = false;
            ICommand command = new RelayCommand(obj => isActionCalled = true);
            Assert.That(command, Is.Not.Null);

            Assert.That(isActionCalled, Is.False);
            command.Execute(fixture.Create<object>());
            Assert.That(isActionCalled, Is.True);
        }

        /// <summary>
        /// Tester, at Execute overfører parameter til handlingen, der skal udføres ved udførelse af kommandoen.
        /// </summary>
        [Test]
        public void TestAtExecuteVideresenderParameterTilAction()
        {
            Fixture fixture = new Fixture();

            object parameter = fixture.Create<object>();
            bool isActionCalled = false;
            Action<object> execute = obj =>
            {
                Assert.That(obj, Is.Not.Null);
                Assert.That(obj, Is.EqualTo(parameter));
                isActionCalled = true;
            };

            ICommand command = new RelayCommand(execute);
            Assert.That(command, Is.Not.Null);

            Assert.That(isActionCalled, Is.False);
            command.Execute(parameter);
            Assert.That(isActionCalled, Is.True);
        }
    }
}