using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Core
{
    /// <summary>
    /// ViewModel which can view Privacy Policy.
    /// </summary>
    public class PrivacyPolicyViewModel : ViewModelBase, IPrivacyPolicyViewModel
    {
        #region Properties

        /// <summary>
        /// Display name for the Privacy Policy.
        /// </summary>
        public override string DisplayName
        {
            get
            {
                return Header;
            }
        }

        /// <summary>
        /// Header for the Privacy Policy.
        /// </summary>
        public virtual string Header
        {
            get
            {
                return Resource.GetText(Resources.Text.PrivacyPolicyHeader);
            }
        }

        /// <summary>
        /// Text containing the Privacy Policy.
        /// </summary>
        public virtual string Text
        {
            get
            {
                return Resource.GetText(Resources.Text.PrivacyPolicyText);
            }
        }

        #endregion
    }
}
