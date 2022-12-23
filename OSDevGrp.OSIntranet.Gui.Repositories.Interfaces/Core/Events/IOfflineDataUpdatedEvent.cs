using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;
using System;
using System.Xml;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core.Events
{
    public interface IOfflineDataUpdatedEvent : IEvent
    {
        XmlDocument OfflineDataDocument { get; }

        DateTime Updated { get; }
    }
}