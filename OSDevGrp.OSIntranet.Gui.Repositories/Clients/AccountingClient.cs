using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Clients
{
    internal partial class AccountingClient
    {
        #region Private variables

        private readonly AuthenticationHeaderValue _authenticationHeaderValue;

        #endregion

        #region Constructor

        public AccountingClient(Uri baseUri, HttpClient httpClient, IAccessTokenModel accessTokenModel)
            : this(baseUri.AbsoluteUri, httpClient)
        {
            NullGuard.NotNull(accessTokenModel, nameof(accessTokenModel));

            _authenticationHeaderValue = new AuthenticationHeaderValue(accessTokenModel.TokenType, accessTokenModel.TokenValue);
        }

        #endregion

        #region Methods

        partial void PrepareRequest(HttpClient client, HttpRequestMessage request, StringBuilder urlBuilder)
        {
            NullGuard.NotNull(client, nameof(client))
                .NotNull(request, nameof(request))
                .NotNull(urlBuilder, nameof(urlBuilder));

            request.Headers.Add("Authorization", _authenticationHeaderValue.ToString());
        }

        #endregion
    }
}