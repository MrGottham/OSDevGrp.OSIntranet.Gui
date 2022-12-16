using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security.Models;
using System;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Security.Models
{
    internal class AccessTokenModel : IAccessTokenModel
    {
        #region Constructor

        public AccessTokenModel(string tokenType, string tokenValue, DateTime expires)
        {
            NullGuard.NotNullOrWhiteSpace(tokenType, nameof(tokenType))
                .NotNullOrWhiteSpace(tokenValue, nameof(tokenValue));

            TokenType = tokenType;
            TokenValue = tokenValue;
            Expires = expires;
        }

        #endregion

        #region Properties

        public string TokenType { get; }

        public string TokenValue { get; }

        public DateTime Expires { get; }

        #endregion
    }
}