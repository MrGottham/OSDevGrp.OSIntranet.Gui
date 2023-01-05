using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security.Models;
using System;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Security.Models
{
    public class AccessTokenModel : IAccessTokenModel
    {
        #region Constructor

        private AccessTokenModel(string tokenType, string tokenValue, DateTime expires)
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

        #region Methods

        public static IAccessTokenModel Create(string tokenType, string tokenValue, DateTime expires)
        {
            NullGuard.NotNullOrWhiteSpace(tokenType, nameof(tokenType))
                .NotNullOrWhiteSpace(tokenValue, nameof(tokenValue));

            return new AccessTokenModel(tokenType, tokenValue, expires);
        }

        #endregion
    }
}