using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.Comparers;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Finansstyring.Comparers
{
    /// <summary>
    /// Tester funktionalitet til sammenligning af to ViewModels indeholdende grundlæggende kontooplysninger.
    /// </summary>
    [TestFixture]
    public class KontoViewModelBaseComparerTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer funktionalitet til sammenligning af to ViewModels indeholdende grundlæggende kontooplysninger.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererKontoViewModelBaseComparer()
        {
            var fixture = new Fixture();

            var comparer = new KontoViewModelBaseComparer<IKontoViewModelBase<IKontogruppeViewModelBase>, IKontogruppeViewModelBase>(fixture.Create<bool>());
            Assert.That(comparer, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at Compare kaster en ArgumentNullException, hvis X er null.
        /// </summary>
        [Test]
        public void TestAtComparerKasterArgumentNullExceptionHvisXErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IKontogruppeViewModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeViewModelBase>()));
            fixture.Customize<IKontoViewModelBase<IKontogruppeViewModelBase>>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontoViewModelBase<IKontogruppeViewModelBase>>()));

            var comparer = new KontoViewModelBaseComparer<IKontoViewModelBase<IKontogruppeViewModelBase>, IKontogruppeViewModelBase>(fixture.Create<bool>());
            Assert.That(comparer, Is.Not.Null);

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            var exception = Assert.Throws<ArgumentNullException>(() => comparer.Compare(null, fixture.Create<IKontoViewModelBase<IKontogruppeViewModelBase>>()));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.EqualTo("x"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at Compare kaster en ArgumentNullException, hvis Y er null.
        /// </summary>
        [Test]
        public void TestAtComparerKasterArgumentNullExceptionHvisYErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IKontogruppeViewModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeViewModelBase>()));
            fixture.Customize<IKontoViewModelBase<IKontogruppeViewModelBase>>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontoViewModelBase<IKontogruppeViewModelBase>>()));

            var comparer = new KontoViewModelBaseComparer<IKontoViewModelBase<IKontogruppeViewModelBase>, IKontogruppeViewModelBase>(fixture.Create<bool>());
            Assert.That(comparer, Is.Not.Null);

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            var exception = Assert.Throws<ArgumentNullException>(() => comparer.Compare(fixture.Create<IKontoViewModelBase<IKontogruppeViewModelBase>>(), null));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.EqualTo("y"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at Compare returnerer sammenligningsresultat.
        /// </summary>
        [Test]
        [TestCase(false, 1, 0, "DANKORT", 2, 0, "KONTANTER", -1)]
        [TestCase(false, 2, 0, "DANKORT", 1, 0, "KONTANTER", 1)]
        [TestCase(false, 1, 0, "DANKORT", 1, 0, "KONTANTER", -1)]
        [TestCase(false, 1, 0, "DANKORT", 1, 0, "DANKORT", 0)]
        [TestCase(false, 1, 0, "KONTANTER", 1, 0, "DANKORT", 1)]
        [TestCase(true, 1, 0, "DANKORT", 2, 0, "KONTANTER", -1)]
        [TestCase(true, 2, 0, "DANKORT", 1, 0, "KONTANTER", 1)]
        [TestCase(true, 1, 1000, "DANKORT", 1, 2000, "KONTANTER", -1)]
        [TestCase(true, 1, 2000, "DANKORT", 1, 1000, "KONTANTER", 1)]
        [TestCase(true, 1, 1000, "DANKORT", 1, 1000, "KONTANTER", -1)]
        [TestCase(true, 1, 1000, "DANKORT", 1, 1000, "DANKORT", 0)]
        [TestCase(true, 1, 1000, "KONTANTER", 1, 1000, "DANKORT", 1)]
        public void TestAtCompareReturnererSammenligningsresultat(bool sortByKontoværdi, int xKontogruppe, decimal xKontoværdi, string xKontonummer, int yKontogruppe, decimal yKontoværdi, string yKontonummer, int expectedResult)
        {
            var xKontogruppeViewModelMock = MockRepository.GenerateMock<IKontogruppeViewModelBase>();
            xKontogruppeViewModelMock.Expect(m => m.Nummer)
                                     .Return(xKontogruppe)
                                     .Repeat.Any();

            var xKontoViewModelMock = MockRepository.GenerateMock<IKontoViewModelBase<IKontogruppeViewModelBase>>();
            xKontoViewModelMock.Expect(m => m.Kontonummer)
                               .Return(xKontonummer)
                               .Repeat.Any();
            xKontoViewModelMock.Expect(m => m.Kontogruppe)
                               .Return(xKontogruppeViewModelMock)
                               .Repeat.Any();
            xKontoViewModelMock.Expect(m => m.Kontoværdi)
                               .Return(xKontoværdi)
                               .Repeat.Any();

            var yKontogruppeViewModelMock = MockRepository.GenerateMock<IKontogruppeViewModelBase>();
            yKontogruppeViewModelMock.Expect(m => m.Nummer)
                                     .Return(yKontogruppe)
                                     .Repeat.Any();

            var yKontoViewModelMock = MockRepository.GenerateMock<IKontoViewModelBase<IKontogruppeViewModelBase>>();
            yKontoViewModelMock.Expect(m => m.Kontonummer)
                               .Return(yKontonummer)
                               .Repeat.Any();
            yKontoViewModelMock.Expect(m => m.Kontogruppe)
                               .Return(yKontogruppeViewModelMock)
                               .Repeat.Any();
            yKontoViewModelMock.Expect(m => m.Kontoværdi)
                               .Return(yKontoværdi)
                               .Repeat.Any();

            var comparer = new KontoViewModelBaseComparer<IKontoViewModelBase<IKontogruppeViewModelBase>, IKontogruppeViewModelBase>(sortByKontoværdi);
            Assert.That(comparer, Is.Not.Null);

            var result = comparer.Compare(xKontoViewModelMock, yKontoViewModelMock);
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}
