using System;
using System.Collections.Generic;
using System.Globalization;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core.Comparers;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Core.Comparers
{
    /// <summary>
    /// Tester funktionalitet til sammenligning af to ViewModels for nyheder.
    /// </summary>
    [TestFixture]
    public class NyhedViewModelComparerTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer funktionalitet til sammenligning af to ViewModels for nyheder.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererNyhedViewModelComparer()
        {
            IComparer<INyhedViewModel> comparer = new NyhedViewModelComparer();
            Assert.That(comparer, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at Compare kaster en ArgumentNullException, hvis X er null.
        /// </summary>
        [Test]
        public void TestAtCompareKasterArgumentNullExceptionHvisXErNull()
        {
            Fixture fixture = new Fixture();

            IComparer<INyhedViewModel> comparer = new NyhedViewModelComparer();
            Assert.That(comparer, Is.Not.Null);

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => comparer.Compare(null, fixture.BuildNyhedViewModel()));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        /// <summary>
        /// Tester, at Compare kaster en ArgumentNullException, hvis Y er null.
        /// </summary>
        [Test]
        public void TestAtCompareKasterArgumentNullExceptionHvisYErNull()
        {
            Fixture fixture = new Fixture();

            IComparer<INyhedViewModel> comparer = new NyhedViewModelComparer();
            Assert.That(comparer, Is.Not.Null);

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => comparer.Compare(fixture.BuildNyhedViewModel(), null));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        /// <summary>
        /// Tester, at Compare returnerer værdi mindre end 0, hvis Nyhedsudgivelsestidspunkt på X er større end Nyhedsudgivelsestidspunkt på Y.
        /// </summary>
        [Test]
        public void TestAtCompareReturnererValueLowerThanZeroHvisNyhedsudgivelsestidspunktOnXErGreaterThanNyhedsudgivelsestidspunktOnY()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            DateTime nyhedsudgivelsestidspunkt = DateTime.Now.AddDays(random.Next(0, 7) * -1).AddMinutes(random.Next(0, 120) * -1);
            Mock<INyhedViewModel> x = fixture.BuildNyhedViewModelMock(nyhedsudgivelsestidspunkt: nyhedsudgivelsestidspunkt);
            Mock<INyhedViewModel> y = fixture.BuildNyhedViewModelMock(nyhedsudgivelsestidspunkt: nyhedsudgivelsestidspunkt.AddDays(-7));

            IComparer<INyhedViewModel> comparer = new NyhedViewModelComparer();
            Assert.That(comparer, Is.Not.Null);

            int result = comparer.Compare(x.Object, y.Object);
            Assert.That(result, Is.LessThan(0));

            x.Verify(m => m.Nyhedsudgivelsestidspunkt, Times.Once);
            x.Verify(m => m.Nyhedsaktualitet, Times.Never);
            y.Verify(m => m.Nyhedsudgivelsestidspunkt, Times.Once);
            y.Verify(m => m.Nyhedsaktualitet, Times.Never);
        }

        /// <summary>
        /// Tester, at Compare returnerer værdi større end 0, hvis Nyhedsudgivelsestidspunkt på X er mindre end Nyhedsudgivelsestidspunkt på Y.
        /// </summary>
        [Test]
        public void TestAtCompareReturnererValueGreaterThanZeroHvisNyhedsudgivelsestidspunktOnXErLowerThanNyhedsudgivelsestidspunktOnY()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            DateTime nyhedsudgivelsestidspunkt = DateTime.Now.AddDays(random.Next(0, 7) * -1).AddMinutes(random.Next(0, 120) * -1);
            Mock<INyhedViewModel> x = fixture.BuildNyhedViewModelMock(nyhedsudgivelsestidspunkt: nyhedsudgivelsestidspunkt.AddDays(-7));
            Mock<INyhedViewModel> y = fixture.BuildNyhedViewModelMock(nyhedsudgivelsestidspunkt: nyhedsudgivelsestidspunkt);

            IComparer<INyhedViewModel> comparer = new NyhedViewModelComparer();
            Assert.That(comparer, Is.Not.Null);

            int result = comparer.Compare(x.Object, y.Object);
            Assert.That(result, Is.GreaterThan(0));

            x.Verify(m => m.Nyhedsudgivelsestidspunkt, Times.Once);
            x.Verify(m => m.Nyhedsaktualitet, Times.Never);
            y.Verify(m => m.Nyhedsudgivelsestidspunkt, Times.Once);
            y.Verify(m => m.Nyhedsaktualitet, Times.Never);
        }

        /// <summary>
        /// Tester, at Compare returnerer værdi mindre end 0, hvis Nyhedsudgivelsestidspunkt på X er lig Nyhedsudgivelsestidspunkt på Y og Nyhedsaktualitet på X er større end Nyhedsaktualitet på Y.
        /// </summary>
        [Test]
        public void TestAtCompareReturnererValueLowerThanZeroHvisNyhedsudgivelsestidspunktOnXEqualsNyhedsudgivelsestidspunktOnYOgNyhedsaktualitetOnXErGreaterThanNyhedsaktualitetOnY()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            DateTime nyhedsudgivelsestidspunkt = DateTime.Now.AddDays(random.Next(0, 7) * -1).AddMinutes(random.Next(0, 120) * -1);
            Mock<INyhedViewModel> x = fixture.BuildNyhedViewModelMock(nyhedsaktualitet: Nyhedsaktualitet.High, nyhedsudgivelsestidspunkt: nyhedsudgivelsestidspunkt);
            Mock<INyhedViewModel> y = fixture.BuildNyhedViewModelMock(nyhedsaktualitet: Nyhedsaktualitet.Low, nyhedsudgivelsestidspunkt: nyhedsudgivelsestidspunkt);

            IComparer<INyhedViewModel> comparer = new NyhedViewModelComparer();
            Assert.That(comparer, Is.Not.Null);

            int result = comparer.Compare(x.Object, y.Object);
            Assert.That(result, Is.LessThan(0));

            x.Verify(m => m.Nyhedsudgivelsestidspunkt, Times.Once);
            x.Verify(m => m.Nyhedsaktualitet, Times.Once);
            y.Verify(m => m.Nyhedsudgivelsestidspunkt, Times.Once);
            y.Verify(m => m.Nyhedsaktualitet, Times.Once);
        }

        /// <summary>
        /// Tester, at Compare returnerer værdi større end 0, hvis Nyhedsudgivelsestidspunkt på X er lig Nyhedsudgivelsestidspunkt på Y og Nyhedsaktualitet på X er mindre end Nyhedsaktualitet på Y.
        /// </summary>
        [Test]
        public void TestAtCompareReturnererValueGreaterThanZeroHvisNyhedsudgivelsestidspunktOnXEqualsNyhedsudgivelsestidspunktOnYOgNyhedsaktualitetOnXErLowerThanNyhedsaktualitetOnY()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            DateTime nyhedsudgivelsestidspunkt = DateTime.Now.AddDays(random.Next(0, 7) * -1).AddMinutes(random.Next(0, 120) * -1);
            Mock<INyhedViewModel> x = fixture.BuildNyhedViewModelMock(nyhedsaktualitet: Nyhedsaktualitet.Low, nyhedsudgivelsestidspunkt: nyhedsudgivelsestidspunkt);
            Mock<INyhedViewModel> y = fixture.BuildNyhedViewModelMock(nyhedsaktualitet: Nyhedsaktualitet.High, nyhedsudgivelsestidspunkt: nyhedsudgivelsestidspunkt);

            IComparer<INyhedViewModel> comparer = new NyhedViewModelComparer();
            Assert.That(comparer, Is.Not.Null);

            int result = comparer.Compare(x.Object, y.Object);
            Assert.That(result, Is.GreaterThan(0));

            x.Verify(m => m.Nyhedsudgivelsestidspunkt, Times.Once);
            x.Verify(m => m.Nyhedsaktualitet, Times.Once);
            y.Verify(m => m.Nyhedsudgivelsestidspunkt, Times.Once);
            y.Verify(m => m.Nyhedsaktualitet, Times.Once);
        }

        /// <summary>
        /// Tester, at Compare returnerer sammenligningsresultat, hvis Nyhedsudgivelsestidspunkt på X er lig Nyhedsudgivelsestidspunkt på Y og Nyhedsaktualitet på X erlig Nyhedsaktualitet på Y.
        /// </summary>
        [Test]
        [TestCase("2013-06-30T10:00:00", "2013-06-30T09:00:00", -1)]
        [TestCase("2013-06-30T10:00:00", "2013-06-30T10:00:00", 0)]
        [TestCase("2013-06-30T10:00:00", "2013-06-30T11:00:00", 1)]
        public void TestAtCompareReturnererCompareResultHvisNyhedsudgivelsestidspunktOnXEqualsNyhedsudgivelsestidspunktOnYOgNyhedsaktualitetOnXEqualsNyhedsaktualitetOnY(string xDate, string yDate, int expectCompareResult)
        {
            Fixture fixture = new Fixture();

            Mock<INyhedViewModel> x = fixture.BuildNyhedViewModelMock(nyhedsaktualitet: Nyhedsaktualitet.Medium, nyhedsudgivelsestidspunkt: DateTime.Parse(xDate, new CultureInfo("en-US")));
            Mock<INyhedViewModel> y = fixture.BuildNyhedViewModelMock(nyhedsaktualitet: Nyhedsaktualitet.Medium, nyhedsudgivelsestidspunkt: DateTime.Parse(yDate, new CultureInfo("en-US")));

            IComparer<INyhedViewModel> comparer = new NyhedViewModelComparer();
            Assert.That(comparer, Is.Not.Null);

            int result = comparer.Compare(x.Object, y.Object);
            Assert.That(result, Is.EqualTo(expectCompareResult));

            x.Verify(m => m.Nyhedsudgivelsestidspunkt, Times.Once);
            x.Verify(m => m.Nyhedsaktualitet, Times.Once);
            y.Verify(m => m.Nyhedsudgivelsestidspunkt, Times.Once);
            y.Verify(m => m.Nyhedsaktualitet, Times.Once);
        }
    }
}