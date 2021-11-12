using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.Comparers;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Finansstyring.Comparers
{
    /// <summary>
    /// Tester funktionalitet til sammenligning af to ViewModels for bogføringslinjer.
    /// </summary>
    [TestFixture]
    public class BogføringslinjeViewModelComparerTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer funktionalitet til sammenligning af to ViewModels for bogføringslinjer.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererBogføringslinjeViewModelComparer()
        {
            var comparer = new BogføringslinjeViewModelComparer();
            Assert.That(comparer, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at Compare kaster en ArgumentNullException, hvis X er null.
        /// </summary>
        [Test]
        public void TestAtCompareKasterArgumentNullExceptionHvisXErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IReadOnlyBogføringslinjeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>()));

            var comparer = new BogføringslinjeViewModelComparer();
            Assert.That(comparer, Is.Not.Null);

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => comparer.Compare(null, fixture.Create<IReadOnlyBogføringslinjeViewModel>()));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        /// <summary>
        /// Tester, at Compare kaster en ArgumentNullException, hvis Y er null.
        /// </summary>
        [Test]
        public void TestAtCompareKasterArgumentNullExceptionHvisYErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IReadOnlyBogføringslinjeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>()));

            var comparer = new BogføringslinjeViewModelComparer();
            Assert.That(comparer, Is.Not.Null);

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => comparer.Compare(fixture.Create<IReadOnlyBogføringslinjeViewModel>(), null));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        /// <summary>
        /// Tester, at Compare returnerer værdi mindre end 0, hvis Dato på X er større end Dato på Y.
        /// </summary>
        [Test]
        public void TestAtCompareReturnererValueLowerThanZeroHvisDatoOnXErGreaterThanDatoOnY()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var dato = fixture.Create<DateTime>();
            var x = MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>();
            x.Expect(m => m.Dato)
             .Return(dato)
             .Repeat.Any();

            var y = MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>();
            y.Expect(m => m.Dato)
             .Return(dato.AddDays(-7))
             .Repeat.Any();

            var comparer = new BogføringslinjeViewModelComparer();
            Assert.That(comparer, Is.Not.Null);

            var result = comparer.Compare(x, y);
            Assert.That(result, Is.LessThan(0));

            x.AssertWasCalled(m => m.Dato);
            x.AssertWasNotCalled(m => m.Løbenummer);
            y.AssertWasCalled(m => m.Dato);
            y.AssertWasNotCalled(m => m.Løbenummer);
        }

        /// <summary>
        /// Tester, at Compare returnerer værdi større end 0, hvis Dato på X er mindre end Dato på Y.
        /// </summary>
        [Test]
        public void TestAtCompareReturnererValueGreaterThanZeroHvisDatoOnXErLowerThanDatoOnY()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var dato = fixture.Create<DateTime>();
            var x = MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>();
            x.Expect(m => m.Dato)
             .Return(dato.AddDays(-7))
             .Repeat.Any();

            var y = MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>();
            y.Expect(m => m.Dato)
             .Return(dato)
             .Repeat.Any();

            var comparer = new BogføringslinjeViewModelComparer();
            Assert.That(comparer, Is.Not.Null);

            var result = comparer.Compare(x, y);
            Assert.That(result, Is.GreaterThan(0));

            x.AssertWasCalled(m => m.Dato);
            x.AssertWasNotCalled(m => m.Løbenummer);
            y.AssertWasCalled(m => m.Dato);
            y.AssertWasNotCalled(m => m.Løbenummer);
        }

        /// <summary>
        /// Tester, at Compare returnerer værdi mindre end 0, hvis Dato på X er lig Dato på Y og Løbenummer på X er større end Løbenummer på Y.
        /// </summary>
        [Test]
        public void TestAtCompareReturnererValueLowerThanZeroHvisDatoOnXEqualsDatoOnYOgLøbenummerOnXErGreaterThanLøbenummerOnY()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var dato = fixture.Create<DateTime>();
            var løbenummer = fixture.Create<int>();
            var x = MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>();
            x.Expect(m => m.Dato)
             .Return(dato)
             .Repeat.Any();
            x.Expect(m => m.Løbenummer)
             .Return(løbenummer)
             .Repeat.Any();

            var y = MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>();
            y.Expect(m => m.Dato)
             .Return(dato)
             .Repeat.Any();
            y.Expect(m => m.Løbenummer)
             .Return(løbenummer - fixture.Create<int>())
             .Repeat.Any();

            var comparer = new BogføringslinjeViewModelComparer();
            Assert.That(comparer, Is.Not.Null);

            var result = comparer.Compare(x, y);
            Assert.That(result, Is.LessThan(0));

            x.AssertWasCalled(m => m.Dato);
            x.AssertWasCalled(m => m.Løbenummer);
            y.AssertWasCalled(m => m.Dato);
            y.AssertWasCalled(m => m.Løbenummer);
        }

        /// <summary>
        /// Tester, at Compare returnerer værdi større end 0, hvis Dato på X er lig Dato på Y og Løbenummer på X er mindre end Løbenummer på Y.
        /// </summary>
        [Test]
        public void TestAtCompareReturnererValueGreaterThanZeroHvisDatoOnXEqualsDatoOnYOgLøbenummerOnXErLowerThanLøbenummerOnY()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var dato = fixture.Create<DateTime>();
            var løbenummer = fixture.Create<int>();
            var x = MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>();
            x.Expect(m => m.Dato)
             .Return(dato)
             .Repeat.Any();
            x.Expect(m => m.Løbenummer)
             .Return(løbenummer - fixture.Create<int>())
             .Repeat.Any();

            var y = MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>();
            y.Expect(m => m.Dato)
             .Return(dato)
             .Repeat.Any();
            y.Expect(m => m.Løbenummer)
             .Return(løbenummer)
             .Repeat.Any();

            var comparer = new BogføringslinjeViewModelComparer();
            Assert.That(comparer, Is.Not.Null);

            var result = comparer.Compare(x, y);
            Assert.That(result, Is.GreaterThan(0));

            x.AssertWasCalled(m => m.Dato);
            x.AssertWasCalled(m => m.Løbenummer);
            y.AssertWasCalled(m => m.Dato);
            y.AssertWasCalled(m => m.Løbenummer);
        }

        /// <summary>
        /// Tester, at Compare returnerer 0, hvis Dato på X er lig Dato på Y og Løbenummer på X erlig Løbenummer på Y.
        /// </summary>
        [Test]
        public void TestAtCompareReturnererZeroHvisDatoOnXEqualsDatoOnYOgLøbenummerOnXEqualsLøbenummerOnY()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var dato = fixture.Create<DateTime>();
            var løbenummer = fixture.Create<int>();
            var x = MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>();
            x.Expect(m => m.Dato)
             .Return(dato)
             .Repeat.Any();
            x.Expect(m => m.Løbenummer)
             .Return(løbenummer)
             .Repeat.Any();

            var y = MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>();
            y.Expect(m => m.Dato)
             .Return(dato)
             .Repeat.Any();
            y.Expect(m => m.Løbenummer)
             .Return(løbenummer)
             .Repeat.Any();

            var comparer = new BogføringslinjeViewModelComparer();
            Assert.That(comparer, Is.Not.Null);

            var result = comparer.Compare(x, y);
            Assert.That(result, Is.EqualTo(0));

            x.AssertWasCalled(m => m.Dato);
            x.AssertWasCalled(m => m.Løbenummer);
            y.AssertWasCalled(m => m.Dato);
            y.AssertWasCalled(m => m.Løbenummer);
        }
    }
}
