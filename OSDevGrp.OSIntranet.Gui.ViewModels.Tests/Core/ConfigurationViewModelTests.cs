using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using Ploeh.AutoFixture;
using Rhino.Mocks;
using Text = OSDevGrp.OSIntranet.Gui.Resources.Text;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Core
{
    /// <summary>
    /// Tester ViewModel indeholdende konfiguration.
    /// </summary>
    [TestFixture]
    public class ConfigurationViewModelTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en ViewModel indeholdende konfiguration.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererConfigurationViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<IConfigurationViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IConfigurationViewModel>()));

            var configurationViewModel = new ConfigurationViewModel(fixture.CreateMany<IConfigurationViewModel>(5).ToList());
            Assert.That(configurationViewModel, Is.Not.Null);
            Assert.That(configurationViewModel.DisplayName, Is.Not.Null);
            Assert.That(configurationViewModel.DisplayName, Is.Not.Empty);
            Assert.That(configurationViewModel.DisplayName, Is.EqualTo(Resource.GetText(Text.Configuration)));
        }
    }
}
