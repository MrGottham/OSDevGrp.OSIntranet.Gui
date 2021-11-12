using System;
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
            var fixture = new Fixture();

            Action<object> execute = obj => { };
            execute.Invoke(fixture.Create<object>());

            var command = new RelayCommand(execute);
            Assert.That(command, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren initierer en relay kommando med et udtryk, der angiver, om kommandoen kan udføres.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererRelayCommandMedPredicate()
        {
            var fixture = new Fixture();

            Action<object> execute = obj => { };
            execute.Invoke(fixture.Create<object>());

            Predicate<object> canExecute = obj => true;
            var result = canExecute.Invoke(fixture.Create<object>());
            Assert.That(result, Is.True);

            var command = new RelayCommand(execute, canExecute);
            Assert.That(command, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis handlingen, der skal udføres ved udførelsen er kommandoen, er null.
        /// </summary>
        [Test]
        public void TestAtConstuctotKasterArgumentNullExceptionHvisActionErNull()
        {
            var fixture = new Fixture();

            Predicate<object> canExecute = obj => true;
            var result = canExecute.Invoke(fixture.Create<object>());
            Assert.That(result, Is.True);

            Assert.Throws<ArgumentNullException>(() => new RelayCommand(null));
            Assert.Throws<ArgumentNullException>(() => new RelayCommand(null, canExecute));
        }

        /// <summary>
        /// Tester, at CanExecute returnerer true, hvis udtrykket, der angiver, om kommandoen kan udføres, er null.
        /// </summary>
        [Test]
        public void TestAtCanExecuteReturnererTrueHvisPredicateErNull()
        {
            var fixture = new Fixture();

            Action<object> execute = obj => { };
            execute.Invoke(fixture.Create<object>());

            var command = new RelayCommand(execute);
            Assert.That(command, Is.Not.Null);

            var result = command.CanExecute(fixture.Create<object>());
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tester, at CanExecute returnerer true, hvis udtrykket, der angiver, om kommandoen kan udføres, returnerer true.
        /// </summary>
        [Test]
        public void TestAtCanExecuteReturnererTrueHvisPredicateReturnererTrue()
        {
            var fixture = new Fixture();

            Action<object> execute = obj => { };
            execute.Invoke(fixture.Create<object>());

            var command = new RelayCommand(execute, obj => true);
            Assert.That(command, Is.Not.Null);

            var result = command.CanExecute(fixture.Create<object>());
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tester, at CanExecute returnerer false, hvis udtrykket, der angiver, om kommandoen kan udføres, returnerer false.
        /// </summary>
        [Test]
        public void TestAtCanExecuteReturnererFalseHvisPredicateReturnererFalse()
        {
            var fixture = new Fixture();

            Action<object> execute = obj => { };
            execute.Invoke(fixture.Create<object>());

            var command = new RelayCommand(execute, obj => false);
            Assert.That(command, Is.Not.Null);

            var result = command.CanExecute(fixture.Create<object>());
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Tester, at CanExecute overfører parameter til udtrykket, der angiver, om kommandoen kan udføres.
        /// </summary>
        [Test]
        public void TestAtCanExecuteVideresenderParameterTilPredicate()
        {
            var fixture = new Fixture();

            Action<object> execute = obj => { };
            execute.Invoke(fixture.Create<object>());

            var parameter = fixture.Create<object>();
            Predicate<object> canExecute = obj =>
                {
                    Assert.That(obj, Is.Not.Null);
                    Assert.That(obj, Is.EqualTo(parameter));
                    return true;
                };

            var command = new RelayCommand(execute, canExecute);
            Assert.That(command, Is.Not.Null);

            var result = command.CanExecute(parameter);
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tester, at Execute udfører handlingen, der skal udføres ved udførelse af kommandoen.
        /// </summary>
        [Test]
        public void TestAtExecuteExecutesAction()
        {
            var fixture = new Fixture();

            var isAcationCalled = false;
            var command = new RelayCommand(obj => isAcationCalled = true);
            Assert.That(command, Is.Not.Null);

            Assert.That(isAcationCalled, Is.False);
            command.Execute(fixture.Create<object>());
            Assert.That(isAcationCalled, Is.True);
        }

        /// <summary>
        /// Tester, at Execute overfører parameter til handlingen, der skal udføres ved udførelse af kommandoen.
        /// </summary>
        [Test]
        public void TestAtExecuteVideresenderParameterTilAction()
        {
            var fixture = new Fixture();

            var parameter = fixture.Create<object>();
            var isAcationCalled = false;
            Action<object> execute = obj =>
                {
                    Assert.That(obj, Is.Not.Null);
                    Assert.That(obj, Is.EqualTo(parameter));
                    isAcationCalled = true;
                };

            var command = new RelayCommand(execute);
            Assert.That(command, Is.Not.Null);

            Assert.That(isAcationCalled, Is.False);
            command.Execute(parameter);
            Assert.That(isAcationCalled, Is.True);
        }
    }
}
