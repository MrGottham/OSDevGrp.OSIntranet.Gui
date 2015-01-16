namespace OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core
{
    /// <summary>
    /// Interface for a ViewModel which can view Privacy Policy.
    /// </summary>
    public interface IPrivacyPolicyViewModel : IViewModel
    {
        /// <summary>
        /// Header for the Privacy Policy.
        /// </summary>
        string Header { get; }

        /// <summary>
        /// Text containing the Privacy Policy.
        /// </summary>
        string Text { get; }
    }
}
