using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using Ploeh.AutoFixture;
using Rhino.Mocks;
using Text = OSDevGrp.OSIntranet.Gui.Resources.Text;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Finansstyring
{
    /// <summary>
    /// Tester ViewModel indeholdende konfiguration til finansstyring.
    /// </summary>
    [TestFixture]
    public class FinansstyringKonfigurationViewModelTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en ViewModel indeholdende konfiguration til finansstyring.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererFinansstyringKonfigurationViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(fixture.Create<IFinansstyringKonfigurationRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(finansstyringKonfigurationViewModel, Is.Not.Null);
            Assert.That(finansstyringKonfigurationViewModel.Configuration, Is.Not.Null);
            Assert.That(finansstyringKonfigurationViewModel.Configuration, Is.Not.Empty);
            Assert.That(finansstyringKonfigurationViewModel.Configuration, Is.EqualTo("OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.FinansstyringKonfigurationViewModel"));
            Assert.That(finansstyringKonfigurationViewModel.Keys, Is.Not.Null);
            Assert.That(finansstyringKonfigurationViewModel.Keys, Is.Not.Empty);
            Assert.That(finansstyringKonfigurationViewModel.DisplayName, Is.Not.Null);
            Assert.That(finansstyringKonfigurationViewModel.DisplayName, Is.Not.Empty);
            Assert.That(finansstyringKonfigurationViewModel.DisplayName, Is.EqualTo(Resource.GetText(Text.Configuration)));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis konfigurationsrepositoryet til finansstyring er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFinansstyringKonfigurationRepositoryErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            Assert.Throws<ArgumentNullException>(() => new FinansstyringKonfigurationViewModel(null, fixture.Create<IExceptionHandlerViewModel>()));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis ViewModel for exceptionhandleren er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisExceptionHandlerViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringKonfigurationRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>()));

            Assert.Throws<ArgumentNullException>(() => new FinansstyringKonfigurationViewModel(fixture.Create<IFinansstyringKonfigurationRepository>(), null));
        }
    }
}
