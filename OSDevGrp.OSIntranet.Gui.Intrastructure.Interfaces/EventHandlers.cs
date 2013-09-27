using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Events;

namespace OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces
{
    /// <summary>
    /// Eventhandler til events i OS Intranet.
    /// </summary>
    /// <typeparam name="TIntranetGuiEventArgs">Typen på argumenter til eventet.</typeparam>
    /// <param name="sender">Objekt, der rejser eventet.</param>
    /// <param name="eventArgs">Argumenter til eventet.</param>
    public delegate void IntranetGuiEventHandler<in TIntranetGuiEventArgs>(object sender, TIntranetGuiEventArgs eventArgs) where TIntranetGuiEventArgs : IIntranetGuiEventArgs;
}
