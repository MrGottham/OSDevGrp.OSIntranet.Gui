using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Events;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces
{
    /// <summary>
    /// Interface, der muliggør, at der kan subscribe på events.
    /// </summary>
    /// <typeparam name="TEventArgs">Typen af argumenter til eventet, der skal subscribes på.</typeparam>
    public interface IEventSubscriber<in TEventArgs> where TEventArgs : IIntranetGuiEventArgs
    {
        /// <summary>
        /// Metode, der udfører, når eventet publiseres.
        /// </summary>
        /// <param name="eventArgs">Argumenter fra eventet.</param>
        void OnEvent(TEventArgs eventArgs);
    }
}
