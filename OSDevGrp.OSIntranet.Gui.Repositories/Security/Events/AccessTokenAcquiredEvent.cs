using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security.Events;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security.Models;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Security.Events
{
    internal class AccessTokenAcquiredEvent : IAccessTokenAcquiredEvent
    {
        #region Constructor

        public AccessTokenAcquiredEvent(IAccessTokenModel accessToken)
        {
            NullGuard.NotNull(accessToken, nameof(accessToken));

            AccessToken = accessToken;
        }

        #endregion

        #region Properties

        public IAccessTokenModel AccessToken { get; }

        #endregion
    }
}