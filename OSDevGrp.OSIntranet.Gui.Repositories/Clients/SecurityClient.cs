using OSDevGrp.OSIntranet.Core;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Clients
{
    internal partial class SecurityClient
    {
        #region Private variables

        private readonly AuthenticationHeaderValue _authenticationHeaderValue;

        #endregion

        #region Constructor

        public SecurityClient(Uri baseUri, HttpClient httpClient, string clientId, string clientSecret)
            : this(baseUri.AbsoluteUri, httpClient)
        {
            NullGuard.NotNullOrWhiteSpace(clientId, nameof(clientId))
                .NotNull(clientSecret, nameof(clientSecret));

            _authenticationHeaderValue = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}")));
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