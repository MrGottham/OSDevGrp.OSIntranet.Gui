﻿using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security.Models;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Clients
{
    internal static class AccessTokenModelExtensions
    {
        #region Methods

        internal static IAccessTokenModel AsInterface(this AccessTokenModel accessTokenModel)
        {
            NullGuard.NotNull(accessTokenModel, nameof(accessTokenModel));

            return Security.Models.AccessTokenModel.Create(accessTokenModel.TokenType, accessTokenModel.AccessToken, accessTokenModel.Expires.LocalDateTime);
        }

        #endregion
    }
}