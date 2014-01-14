using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Finansstyring
{
    /// <summary>
    /// Tester ViewModel for en adressekonto.
    /// </summary>
    [TestFixture]
    public class AdressekontoViewModelTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en ViewModel for en adressekonto.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererAdressekontoViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<Byte[]>(e => e.FromFactory(() => TestHelper.GetEmbeddedResource((new Resource()).GetType().Assembly, "Images.Adressekonto.png")));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IAdressekontoModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IAdressekontoModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(fixture.Create<int>())
                        .Repeat.Any();
                    mock.Expect(m => m.Navn)
                        .Return(fixture.Create<string>())
                        .Repeat.Any();
                    mock.Expect(m => m.PrimærTelefon)
                        .Return(fixture.Create<string>())
                        .Repeat.Any();
                    mock.Expect(m => m.SekundærTelefon)
                        .Return(fixture.Create<string>())
                        .Repeat.Any();
                    mock.Expect(m => m.StatusDato)
                        .Return(fixture.Create<DateTime>())
                        .Repeat.Any();
                    return mock;
                }));

            var regnskabViewModelMock = fixture.Create<IRegnskabViewModel>();
            var adressekontoModelMock = fixture.Create<IAdressekontoModel>();
            var displayName = fixture.Create<string>();
            var image = fixture.Create<byte[]>();
            var adressekontoViewModel = new AdressekontoViewModel(regnskabViewModelMock, adressekontoModelMock, displayName, image);
            Assert.That(adressekontoViewModel, Is.Not.Null);
            Assert.That(adressekontoViewModel.Regnskab, Is.Not.Null);
            Assert.That(adressekontoViewModel.Regnskab, Is.EqualTo(regnskabViewModelMock));
            Assert.That(adressekontoViewModel.Nummer, Is.EqualTo(adressekontoModelMock.Nummer));
            Assert.That(adressekontoViewModel.Navn, Is.Not.Null);
            Assert.That(adressekontoViewModel.Navn, Is.Not.Empty);
            Assert.That(adressekontoViewModel.Navn, Is.EqualTo(adressekontoModelMock.Navn));
            Assert.That(adressekontoViewModel.PrimærTelefon, Is.Not.Null);
            Assert.That(adressekontoViewModel.PrimærTelefon, Is.Not.Empty);
            Assert.That(adressekontoViewModel.PrimærTelefon, Is.EqualTo(adressekontoModelMock.PrimærTelefon));
            Assert.That(adressekontoViewModel.SekundærTelefon, Is.Not.Null);
            Assert.That(adressekontoViewModel.SekundærTelefon, Is.Not.Empty);
            Assert.That(adressekontoViewModel.SekundærTelefon, Is.EqualTo(adressekontoModelMock.SekundærTelefon));
            Assert.That(adressekontoViewModel.StatusDato, Is.EqualTo(adressekontoModelMock.StatusDato));

            adressekontoModelMock.AssertWasCalled(m => m.Nummer);
            adressekontoModelMock.AssertWasCalled(m => m.Navn);
            adressekontoModelMock.AssertWasCalled(m => m.PrimærTelefon);
            adressekontoModelMock.AssertWasCalled(m => m.SekundærTelefon);
            adressekontoModelMock.AssertWasCalled(m => m.StatusDato);
        }
    }
}
