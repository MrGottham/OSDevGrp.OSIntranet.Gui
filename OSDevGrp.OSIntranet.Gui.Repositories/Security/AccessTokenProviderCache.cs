using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security.Models;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Security
{
    internal class AccessTokenProviderCache : IAccessTokenProvider, IAccessTokenSetter
    {
        #region Private variables

        private IAccessTokenModel? _accessTokenModel;
        private readonly IAccessTokenProvider _accessTokenProvider;
        private readonly object _syncRoot = new object();

        #endregion

        #region Constructor

        public AccessTokenProviderCache(IAccessTokenProvider accessTokenProvider)
        {
            NullGuard.NotNull(accessTokenProvider, nameof(accessTokenProvider));

            _accessTokenProvider = accessTokenProvider;
        }

        #endregion

        #region Methods

        public Task<IAccessTokenModel> GetAccessTokenAsync()
        {
            return Task.Run(() =>
            {
                lock (_syncRoot)
                {
                    if (_accessTokenModel != null && _accessTokenModel.Expires.ToUniversalTime() > DateTime.UtcNow.AddMinutes(1))
                    {
                        return _accessTokenModel;
                    }

                    _accessTokenModel = _accessTokenProvider.GetAccessTokenAsync().GetAwaiter().GetResult();
                    return _accessTokenModel;
                }
            });
        }

        public Task SetAccessTokenAsync(IAccessTokenModel accessTokenModel)
        {
            NullGuard.NotNull(accessTokenModel, nameof(accessTokenModel));

            return Task.Run(() =>
            {
                lock (_syncRoot)
                {
                    _accessTokenModel = accessTokenModel;
                }
            });
        }

        #endregion
    }
}