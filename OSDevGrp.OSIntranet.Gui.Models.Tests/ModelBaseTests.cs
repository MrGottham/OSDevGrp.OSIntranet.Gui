using System;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Gui.Models.Tests
{
    /// <summary>
    /// Tester basisfunktionalitet for en model i OS Intranet.
    /// </summary>
    [TestFixture]
    public class ModelBaseTests
    {
        /// <summary>
        /// Egen klasse til test af basisfunktionalitet for en model i OS Intranet.
        /// </summary>
        private class MyModel : ModelBase
        {
            /// <summary>
            /// Rejser eventet, der notifiserer, at en property er blevet ændret.
            /// </summary>
            /// <param name="propertyName">Navnet på propertyen, der er blevet ændret.</param>
            public new void RaisePropertyChanged(string propertyName)
            {
                base.RaisePropertyChanged(propertyName);
            }
        }

        /// <summary>
        /// Tester, at konstruktøren initerer basisfunktionalitet for en model i OS Intranet.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererModelBase()
        {
            var myModel = new MyModel();
            Assert.That(myModel, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at RaisePropertyChanged kaster en ArgumentNullException, hvis navnet på propertyen er null.
        /// </summary>
        [Test]
        public void TestAtRaisePropertyChangedKasterArgumentNullExceptionHvisPropertyNameErNull()
        {
            var myModel = new MyModel();
            Assert.That(myModel, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => myModel.RaisePropertyChanged(null));
        }

        /// <summary>
        /// Tester, at RaisePropertyChanged kaster en ArgumentNullException, hvis navnet på propertyen er tom.
        /// </summary>
        [Test]
        public void TestAtRaisePropertyChangedKasterArgumentNullExceptionHvisPropertyNameErEmpty()
        {
            var myModel = new MyModel();
            Assert.That(myModel, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => myModel.RaisePropertyChanged(string.Empty));
        }

        /// <summary>
        /// Tester, at RaisePropertyChanged rejser event, der notifiserer, hvis en property er blevet ændret.
        /// </summary>
        [Test]
        public void TestAtRaisePropertyChangedRejserEvent()
        {
            var fixture = new Fixture();

            var myModel = new MyModel();
            Assert.That(myModel, Is.Not.Null);

            var propertyName = fixture.Create<string>();
            var eventCalled = false;
            myModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.EqualTo(propertyName));
                    eventCalled = true;
                };

            Assert.That(eventCalled, Is.False);
            myModel.RaisePropertyChanged(propertyName);
            Assert.That(eventCalled, Is.True);
        }
    }
}
