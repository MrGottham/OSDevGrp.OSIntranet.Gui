using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Models.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.Models.Tests.Finansstyring
{
    /// <summary>
    /// Tester modellen for et regnskab.
    /// </summary>
    [TestFixture]
    public class RegnskabModelTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en regnskabsmodel.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererRegnskabModel()
        {
            var fixture = new Fixture();
            var nummer = fixture.Create<int>();
            var navn = fixture.Create<string>();

            var regnskabModel = new RegnskabModel(nummer, navn);
            Assert.That(regnskabModel, Is.Not.Null);
            Assert.That(regnskabModel.Nummer, Is.EqualTo(nummer));
            Assert.That(regnskabModel.Navn, Is.Not.Null);
            Assert.That(regnskabModel.Navn, Is.Not.Empty);
            Assert.That(regnskabModel.Navn, Is.EqualTo(navn));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis navnet er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisNavnErNull()
        {
            var fixture = new Fixture();

            Assert.Throws<ArgumentNullException>(() => new RegnskabModel(fixture.Create<int>(), null));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis navnet er tomt.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisNavnErEmpty()
        {
            var fixture = new Fixture();

            Assert.Throws<ArgumentNullException>(() => new RegnskabModel(fixture.Create<int>(), string.Empty));
        }

        /// <summary>
        /// Tester, at sætteren til Navn kaster en ArgumentNullException, hvis value er null.
        /// </summary>
        [Test]
        public void TestAtNavnSetterKasterArgumentNullExceptionHvisNavnErNull()
        {
            var fixture = new Fixture();

            var regnskabModel = new RegnskabModel(fixture.Create<int>(), fixture.Create<string>());
            Assert.That(regnskabModel, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => regnskabModel.Navn = null);
        }

        /// <summary>
        /// Tester, at sætteren til Navn kaster en ArgumentNullException, hvis value er tom.
        /// </summary>
        [Test]
        public void TestAtNavnSetterKasterArgumentNullExceptionHvisNavnErEmpty()
        {
            var fixture = new Fixture();

            var regnskabModel = new RegnskabModel(fixture.Create<int>(), fixture.Create<string>());
            Assert.That(regnskabModel, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => regnskabModel.Navn = string.Empty);
        }

        /// <summary>
        /// Tester, at sætteren til Navn retter navnet på modellen.
        /// </summary>
        [Test]
        public void TestAtNavnSetterRetterNavn()
        {
            var fixture = new Fixture();

            var regnskabModel = new RegnskabModel(fixture.Create<int>(), fixture.Create<string>());
            Assert.That(regnskabModel, Is.Not.Null);

            var newValue = fixture.Create<string>();
            regnskabModel.Navn = newValue;
            Assert.That(regnskabModel.Navn, Is.Not.Null);
            Assert.That(regnskabModel.Navn, Is.Not.Empty);
            Assert.That(regnskabModel.Navn, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tester, at sætteren til Navn rejser PropertyChanged ved ændring af navn.
        /// </summary>
        [Test]
        public void TestAtNavnSetterRejserPropertyChangedVedRettelseAfNavn()
        {
            var fixture = new Fixture();

            var regnskabModel = new RegnskabModel(fixture.Create<int>(), fixture.Create<string>());
            Assert.That(regnskabModel, Is.Not.Null);

            var eventCalled = false;
            regnskabModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    Assert.That(e.PropertyName, Is.EqualTo("Navn"));
                    eventCalled = true;
                };

            Assert.That(eventCalled, Is.False);
            regnskabModel.Navn = regnskabModel.Navn;
            Assert.That(eventCalled, Is.False);
            regnskabModel.Navn = fixture.Create<string>();
            Assert.That(eventCalled, Is.True);
        }
    }
}
