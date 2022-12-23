using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core.Events;
using System;
using System.Xml;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Core.Events
{
    internal class OfflineDataUpdatedEvent : IOfflineDataUpdatedEvent
    {
        #region Constructor

        public OfflineDataUpdatedEvent(XmlDocument offlineDataDocument, DateTime updated)
        {
            NullGuard.NotNull(offlineDataDocument, nameof(offlineDataDocument));

            OfflineDataDocument = offlineDataDocument;
            Updated = updated;
        }

        #endregion

        #region Properties

        public XmlDocument OfflineDataDocument { get; }

        public DateTime Updated { get; }

        #endregion
    }
}