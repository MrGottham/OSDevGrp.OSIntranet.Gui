using OSDevGrp.OSIntranet.Core;
using System;
using System.Net.Http;
using System.Net.Sockets;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Exceptions
{
    internal static class HttpRequestExceptionExtensions
    {
        #region Methods

        internal static Exception ToException(this HttpRequestException httpRequestException)
        {
            NullGuard.NotNull(httpRequestException, nameof(httpRequestException));

            if (httpRequestException.InnerException is SocketException socketException)
            {
                switch (socketException.SocketErrorCode)
                {
                    case SocketError.ConnectionRefused:
                    case SocketError.HostNotFound:
                    case SocketError.HostUnreachable:
                        return new IntranetGuiOfflineException(socketException.Message, httpRequestException);

                    default:
                        return new NotSupportedException($"Unhandled exception: {socketException.SocketErrorCode} - {socketException.Message}", httpRequestException);
                }
            }

            return new NotSupportedException($"Unhandled exception: {httpRequestException.Message}", httpRequestException);
        }

        #endregion
    }
}