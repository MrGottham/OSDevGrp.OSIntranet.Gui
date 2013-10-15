using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core.Comparers;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using Ploeh.AutoFixture;
using Rhino.Mocks;

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
            var comparer = new NyhedViewModelComparer();
            Assert.That(comparer, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at Compare kaster en ArgumentNullException, hvis X er null.
        /// </summary>
        [Test]
        public void TestAtCompareKasterArgumentNullExceptionHvisXErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<INyhedViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<INyhedViewModel>()));

            var comparer = new NyhedViewModelComparer();
            Assert.That(comparer, Is.Not.Null);

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => comparer.Compare(null, fixture.Create<INyhedViewModel>()));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        /// <summary>
        /// Tester, at Compare kaster en ArgumentNullException, hvis Y er null.
        /// </summary>
        [Test]
        public void TestAtCompareKasterArgumentNullExceptionHvisYErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<INyhedViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<INyhedViewModel>()));

            var comparer = new NyhedViewModelComparer();
            Assert.That(comparer, Is.Not.Null);

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => comparer.Compare(fixture.Create<INyhedViewModel>(), null));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        /// <summary>
        /// Tester, at Compare returnerer værdi mindre end 0, hvis Nyhedsudgivelsestidspunkt på X er større end Nyhedsudgivelsestidspunkt på Y.
        /// </summary>
        [Test]
        public void TestAtCompareReturnererValueLowerThanZeroHvisNyhedsudgivelsestidspunktOnXErGreaterThanNyhedsudgivelsestidspunktOnY()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var dato = fixture.Create<DateTime>();
            var x = MockRepository.GenerateMock<INyhedViewModel>();
            x.Expect(m => m.Nyhedsudgivelsestidspunkt)
             .Return(dato)
             .Repeat.Any();

            var y = MockRepository.GenerateMock<INyhedViewModel>();
            y.Expect(m => m.Nyhedsudgivelsestidspunkt)
             .Return(dato.AddDays(-7))
             .Repeat.Any();

            var comparer = new NyhedViewModelComparer();
            Assert.That(comparer, Is.Not.Null);

            var result = comparer.Compare(x, y);
            Assert.That(result, Is.LessThan(0));

            x.AssertWasCalled(m => m.Nyhedsudgivelsestidspunkt);
            x.AssertWasNotCalled(m => m.Nyhedsaktualitet);
            y.AssertWasCalled(m => m.Nyhedsudgivelsestidspunkt);
            y.AssertWasNotCalled(m => m.Nyhedsaktualitet);
        }

        /// <summary>
        /// Tester, at Compare returnerer værdi større end 0, hvis Nyhedsudgivelsestidspunkt på X er mindre end Nyhedsudgivelsestidspunkt på Y.
        /// </summary>
        [Test]
        public void TestAtCompareReturnererValueGreaterThanZeroHvisNyhedsudgivelsestidspunktOnXErLowerThanNyhedsudgivelsestidspunktOnY()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var dato = fixture.Create<DateTime>();
            var x = MockRepository.GenerateMock<INyhedViewModel>();
            x.Expect(m => m.Nyhedsudgivelsestidspunkt)
             .Return(dato.AddDays(-7))
             .Repeat.Any();

            var y = MockRepository.GenerateMock<INyhedViewModel>();
            y.Expect(m => m.Nyhedsudgivelsestidspunkt)
             .Return(dato)
             .Repeat.Any();

            var comparer = new NyhedViewModelComparer();
            Assert.That(comparer, Is.Not.Null);

            var result = comparer.Compare(x, y);
            Assert.That(result, Is.GreaterThan(0));

            x.AssertWasCalled(m => m.Nyhedsudgivelsestidspunkt);
            x.AssertWasNotCalled(m => m.Nyhedsaktualitet);
            y.AssertWasCalled(m => m.Nyhedsudgivelsestidspunkt);
            y.AssertWasNotCalled(m => m.Nyhedsaktualitet);
        }

        /// <summary>
        /// Tester, at Compare returnerer værdi mindre end 0, hvis Nyhedsudgivelsestidspunkt på X er lig Nyhedsudgivelsestidspunkt på Y og Nyhedsaktualitet på X er større end Nyhedsaktualitet på Y.
        /// </summary>
        [Test]
        public void TestAtCompareReturnererValueLowerThanZeroHvisNyhedsudgivelsestidspunktOnXEqualsNyhedsudgivelsestidspunktOnYOgNyhedsaktualitetOnXErGreaterThanNyhedsaktualitetOnY()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var dato = fixture.Create<DateTime>();
            var x = MockRepository.GenerateMock<INyhedViewModel>();
            x.Expect(m => m.Nyhedsudgivelsestidspunkt)
             .Return(dato)
             .Repeat.Any();
            x.Expect(m => m.Nyhedsaktualitet)
             .Return(Nyhedsaktualitet.High)
             .Repeat.Any();

            var y = MockRepository.GenerateMock<INyhedViewModel>();
            y.Expect(m => m.Nyhedsudgivelsestidspunkt)
             .Return(dato)
             .Repeat.Any();
            y.Expect(m => m.Nyhedsaktualitet)
             .Return(Nyhedsaktualitet.Low)
             .Repeat.Any();

            var comparer = new NyhedViewModelComparer();
            Assert.That(comparer, Is.Not.Null);

            var result = comparer.Compare(x, y);
            Assert.That(result, Is.LessThan(0));

            x.AssertWasCalled(m => m.Nyhedsudgivelsestidspunkt);
            x.AssertWasCalled(m => m.Nyhedsaktualitet);
            y.AssertWasCalled(m => m.Nyhedsudgivelsestidspunkt);
            y.AssertWasCalled(m => m.Nyhedsaktualitet);
        }

        /// <summary>
        /// Tester, at Compare returnerer værdi større end 0, hvis Nyhedsudgivelsestidspunkt på X er lig Nyhedsudgivelsestidspunkt på Y og Nyhedsaktualitet på X er mindre end Nyhedsaktualitet på Y.
        /// </summary>
        [Test]
        public void TestAtCompareReturnererValueGreaterThanZeroHvisNyhedsudgivelsestidspunktOnXEqualsNyhedsudgivelsestidspunktOnYOgNyhedsaktualitetOnXErLowerThanNyhedsaktualitetOnY()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var dato = fixture.Create<DateTime>();
            var x = MockRepository.GenerateMock<INyhedViewModel>();
            x.Expect(m => m.Nyhedsudgivelsestidspunkt)
             .Return(dato)
             .Repeat.Any();
            x.Expect(m => m.Nyhedsaktualitet)
             .Return(Nyhedsaktualitet.Low)
             .Repeat.Any();

            var y = MockRepository.GenerateMock<INyhedViewModel>();
            y.Expect(m => m.Nyhedsudgivelsestidspunkt)
             .Return(dato)
             .Repeat.Any();
            y.Expect(m => m.Nyhedsaktualitet)
             .Return(Nyhedsaktualitet.High)
             .Repeat.Any();

            var comparer = new NyhedViewModelComparer();
            Assert.That(comparer, Is.Not.Null);

            var result = comparer.Compare(x, y);
            Assert.That(result, Is.GreaterThan(0));

            x.AssertWasCalled(m => m.Nyhedsudgivelsestidspunkt);
            x.AssertWasCalled(m => m.Nyhedsaktualitet);
            y.AssertWasCalled(m => m.Nyhedsudgivelsestidspunkt);
            y.AssertWasCalled(m => m.Nyhedsaktualitet);
        }

        /// <summary>
        /// Tester, at Compare returnerer 0, hvis Nyhedsudgivelsestidspunkt på X er lig Nyhedsudgivelsestidspunkt på Y og Nyhedsaktualitet på X erlig Nyhedsaktualitet på Y.
        /// </summary>
        [Test]
        public void TestAtCompareReturnererZeroHvisNyhedsudgivelsestidspunktOnXEqualsNyhedsudgivelsestidspunktOnYOgNyhedsaktualitetOnXEqualsNyhedsaktualitetOnY()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var dato = fixture.Create<DateTime>();
            var x = MockRepository.GenerateMock<INyhedViewModel>();
            x.Expect(m => m.Nyhedsudgivelsestidspunkt)
             .Return(dato)
             .Repeat.Any();
            x.Expect(m => m.Nyhedsaktualitet)
             .Return(Nyhedsaktualitet.Medium)
             .Repeat.Any();

            var y = MockRepository.GenerateMock<INyhedViewModel>();
            y.Expect(m => m.Nyhedsudgivelsestidspunkt)
             .Return(dato)
             .Repeat.Any();
            y.Expect(m => m.Nyhedsaktualitet)
             .Return(Nyhedsaktualitet.Medium)
             .Repeat.Any();

            var comparer = new NyhedViewModelComparer();
            Assert.That(comparer, Is.Not.Null);

            var result = comparer.Compare(x, y);
            Assert.That(result, Is.EqualTo(0));

            x.AssertWasCalled(m => m.Nyhedsudgivelsestidspunkt);
            x.AssertWasCalled(m => m.Nyhedsaktualitet);
            y.AssertWasCalled(m => m.Nyhedsudgivelsestidspunkt);
            y.AssertWasCalled(m => m.Nyhedsaktualitet);
        }
    }
}
