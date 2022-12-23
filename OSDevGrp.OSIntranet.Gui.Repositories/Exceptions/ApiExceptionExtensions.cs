using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Clients;
using System;
using System.Net;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Exceptions
{
    internal static class ApiExceptionExtensions
    {
        #region Methods

        internal static Exception ToException(this ApiException apiException)
        {
            NullGuard.NotNull(apiException, nameof(apiException));

            switch (apiException.StatusCode)
            {
                case (int)HttpStatusCode.BadRequest:
                    return new IntranetGuiSystemException(HttpStatusCode.BadRequest.ToString(), apiException);

                case (int)HttpStatusCode.Unauthorized:
                    return new UnauthorizedAccessException(HttpStatusCode.Unauthorized.ToString(), apiException);

                case (int)HttpStatusCode.Forbidden:
                    return new UnauthorizedAccessException(HttpStatusCode.Forbidden.ToString(), apiException);

                case (int)HttpStatusCode.InternalServerError:
                    return new IntranetGuiSystemException(HttpStatusCode.InternalServerError.ToString(), apiException);

                default:
                    return new NotSupportedException($"Unhandled exception: {apiException.StatusCode} - {apiException.Message}", apiException);
            }
        }

        internal static Exception ToException(this ApiException<ErrorModel> apiException)
        {
            NullGuard.NotNull(apiException, nameof(apiException));

            switch (apiException.StatusCode)
            {
                case (int)HttpStatusCode.BadRequest:
                    return new IntranetGuiUserFriendlyException(apiException.Result.ErrorMessage, apiException);

                case (int)HttpStatusCode.Unauthorized:
                    return new UnauthorizedAccessException(HttpStatusCode.Unauthorized.ToString(), apiException);

                case (int)HttpStatusCode.Forbidden:
                    return new UnauthorizedAccessException(HttpStatusCode.Forbidden.ToString(), apiException);

                case (int)HttpStatusCode.InternalServerError:
                    return new IntranetGuiSystemException(apiException.Result.ErrorMessage, apiException);

                default:
                    return new NotSupportedException($"Unhandled exception: {apiException.StatusCode} - {apiException.Message}", apiException);
            }
        }

        #endregion
    }
}