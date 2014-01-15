using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.Comparers;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Finansstyring.Comparers
{
    /// <summary>
    /// Tester funktionalitet til sammenligning af to ViewModels for adressekonti.
    /// </summary>
    [TestFixture]
    public class AdressekontoViewModelComparerTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer funktionalitet til sammenligning af to ViewModels for adressekonti.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererAdressekontoViewModelComparer()
        {
            var comparer = new AdressekontoViewModelComparer();
            Assert.That(comparer, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at Compare kaster en ArgumentNullException, hvis X er null.
        /// </summary>
        [Test]
        public void TestAtCompareKasterArgumentNullExceptionHvisXErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IAdressekontoViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IAdressekontoViewModel>()));

            var comparer = new AdressekontoViewModelComparer();
            Assert.That(comparer, Is.Not.Null);

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            var exception = Assert.Throws<ArgumentNullException>(() => comparer.Compare(null, fixture.Create<IAdressekontoViewModel>()));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("x"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at Compare kaster en ArgumentNullException, hvis Y er null.
        /// </summary>
        [Test]
        public void TestAtCompareKasterArgumentNullExceptionHvisYErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IAdressekontoViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IAdressekontoViewModel>()));

            var comparer = new AdressekontoViewModelComparer();
            Assert.That(comparer, Is.Not.Null);

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            var exception = Assert.Throws<ArgumentNullException>(() => comparer.Compare(fixture.Create<IAdressekontoViewModel>(), null));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("y"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at compare returnerer sammenligningsresultat.
        /// </summary>
        [Test]
        [TestCase(2013, 12, 31, "XXX", 1500, 2013, 01, 01, "XXX", 1500, -1)]
        [TestCase(2013, 01, 01, "XXX", 1500, 2013, 12, 31, "XXX", 1500, 1)]
        [TestCase(2013, 12, 31, "xxx", 1500, 2013, 12, 31, "YYY", 1500, -1)]
        [TestCase(2013, 12, 31, "xxx", 1500, 2013, 12, 31, "yyy", 1500, -1)]
        [TestCase(2013, 12, 31, "XXX", 1500, 2013, 12, 31, "YYY", 1500, -1)]
        [TestCase(2013, 12, 31, "XXX", 1500, 2013, 12, 31, "yyy", 1500, -1)]
        [TestCase(2013, 12, 31, "yyy", 1500, 2013, 12, 31, "XXX", 1500, 1)]
        [TestCase(2013, 12, 31, "yyy", 1500, 2013, 12, 31, "xxx", 1500, 1)]
        [TestCase(2013, 12, 31, "YYY", 1500, 2013, 12, 31, "XXX", 1500, 1)]
        [TestCase(2013, 12, 31, "YYY", 1500, 2013, 12, 31, "xxx", 1500, 1)]
        [TestCase(2013, 12, 31, "XXX", 1500, 2013, 12, 31, "XXX", 1500, 0)]
        [TestCase(2013, 12, 31, "XXX", 500, 2013, 12, 31, "XXX", 1500, 1)]
        [TestCase(2013, 12, 31, "XXX", 1500, 2013, 12, 31, "XXX", 500, -1)]
        public void TestAtCompareReturnererSammenlignignsresultat(int xYear, int xMonth, int xDay, string xName, decimal xSaldo, int yYear, int yMonth, int yDay, string yName, decimal ySaldo, int expectedValue)
        {
            var xAdressekontoViewModelMock = MockRepository.GenerateMock<IAdressekontoViewModel>();
            xAdressekontoViewModelMock.Expect(m => m.StatusDato)
                                      .Return(new DateTime(xYear, xMonth, xDay))
                                      .Repeat.Any();
            xAdressekontoViewModelMock.Expect(m => m.Navn)
                                      .Return(xName)
                                      .Repeat.Any();
            xAdressekontoViewModelMock.Expect(m => m.Saldo)
                                      .Return(xSaldo)
                                      .Repeat.Any();

            var yAdressekontoViewModelMock = MockRepository.GenerateMock<IAdressekontoViewModel>();
            yAdressekontoViewModelMock.Expect(m => m.StatusDato)
                                      .Return(new DateTime(yYear, yMonth, yDay))
                                      .Repeat.Any();
            yAdressekontoViewModelMock.Expect(m => m.Navn)
                                      .Return(yName)
                                      .Repeat.Any();
            yAdressekontoViewModelMock.Expect(m => m.Saldo)
                                      .Return(ySaldo)
                                      .Repeat.Any();

            var comparer = new AdressekontoViewModelComparer();
            Assert.That(comparer, Is.Not.Null);

            var result = comparer.Compare(xAdressekontoViewModelMock, yAdressekontoViewModelMock);
            Assert.That(result, Is.EqualTo(expectedValue));
        }
    }
}
