using System;
using System.Reflection;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests
{
    /// <summary>
    /// Tester basisfunktionalitet for en ViewModel i OS Intranet.
    /// </summary>
    public class ViewModelBaseTests
    {
        /// <summary>
        /// Egen klasse til test af basisfunktionalitet for en ViewModel i OS Intranet.
        /// </summary>
        private class MyViewModel : ViewModelBase
        {
            #region Properties

            /// <summary>
            /// Navn for ViewModel, som kan benyttes til visning i brugergrænsefladen.
            /// </summary>
            public override string DisplayName => MethodBase.GetCurrentMethod()?.Name.Substring(4);

            #endregion

            #region Methods

            /// <summary>
            /// Rejser eventet, der notifiserer, at en property er blevet ændret.
            /// </summary>
            /// <param name="propertyName">Navnet på propertyen, der er blevet ændret.</param>
            public new void RaisePropertyChanged(string propertyName)
            {
                base.RaisePropertyChanged(propertyName);
            }

            #endregion
        }

        /// <summary>
        /// Tester, at konstruktøren initerer basisfunktionalitet for en ViewModel i OS Intranet.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererViewModelBase()
        {
            IViewModel myViewModel = new MyViewModel();
            Assert.That(myViewModel, Is.Not.Null);
            Assert.That(myViewModel.DisplayName, Is.Not.Null);
            Assert.That(myViewModel.DisplayName, Is.Not.Empty);
            Assert.That(myViewModel.DisplayName, Is.EqualTo("DisplayName"));
        }

        /// <summary>
        /// Tester, at RaisePropertyChanged kaster en ArgumentNullException, hvis navnet på propertyen er null.
        /// </summary>
        [Test]
        public void TestAtRaisePropertyChangedKasterArgumentNullExceptionHvisPropertyNameErNull()
        {
            MyViewModel myViewModel = new MyViewModel();
            Assert.That(myViewModel, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => myViewModel.RaisePropertyChanged(null));
        }

        /// <summary>
        /// Tester, at RaisePropertyChanged kaster en ArgumentNullException, hvis navnet på propertyen er tom.
        /// </summary>
        [Test]
        public void TestAtRaisePropertyChangedKasterArgumentNullExceptionHvisPropertyNameErEmpty()
        {
            MyViewModel myViewModel = new MyViewModel();
            Assert.That(myViewModel, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => myViewModel.RaisePropertyChanged(string.Empty));
        }

        /// <summary>
        /// Tester, at RaisePropertyChanged rejser event, der notifiserer, hvis en property er blevet ændret.
        /// </summary>
        [Test]
        public void TestAtRaisePropertyChangedRejserEvent()
        {
            Fixture fixture = new Fixture();

            MyViewModel myViewModel = new MyViewModel();
            Assert.That(myViewModel, Is.Not.Null);

            string propertyName = fixture.Create<string>();
            bool eventCalled = false;
            myViewModel.PropertyChanged += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Null);
                Assert.That(e.PropertyName, Is.EqualTo(propertyName));
                eventCalled = true;
            };

            Assert.That(eventCalled, Is.False);
            myViewModel.RaisePropertyChanged(propertyName);
            Assert.That(eventCalled, Is.True);
        }
    }
}