using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core;
using Text = OSDevGrp.OSIntranet.Gui.Resources.Text;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Core
{
    /// <summary>
    /// Tests the ViewModel which can view Privacy Policy.
    /// </summary>
    [TestFixture]
    public class PrivacyPolicyViewModelTests
    {
        /// <summary>
        /// Tests that the constructor initialize the ViewModel which can view Privacy Policy.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializePrivacyPolicyViewModel()
        {
            var privacyPolicyViewModel = new PrivacyPolicyViewModel();
            Assert.That(privacyPolicyViewModel, Is.Not.Null);
            Assert.That(privacyPolicyViewModel.DisplayName, Is.Not.Null);
            Assert.That(privacyPolicyViewModel.DisplayName, Is.Not.Empty);
            Assert.That(privacyPolicyViewModel.DisplayName, Is.EqualTo(Resource.GetText(Text.PrivacyPolicyHeader)));
            Assert.That(privacyPolicyViewModel.Header, Is.Not.Null);
            Assert.That(privacyPolicyViewModel.Header, Is.Not.Empty);
            Assert.That(privacyPolicyViewModel.Header, Is.EqualTo(Resource.GetText(Text.PrivacyPolicyHeader)));
            Assert.That(privacyPolicyViewModel.Text, Is.Not.Null);
            Assert.That(privacyPolicyViewModel.Text, Is.Not.Empty);
            Assert.That(privacyPolicyViewModel.Text, Is.EqualTo(Resource.GetText(Text.PrivacyPolicyText)));
        }
    }
}
