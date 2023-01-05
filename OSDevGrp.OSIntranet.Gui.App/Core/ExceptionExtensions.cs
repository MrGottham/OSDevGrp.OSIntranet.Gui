using Microsoft.Extensions.Logging;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Exceptions;

namespace OSDevGrp.OSIntranet.Gui.App.Core
{
    internal static class ExceptionExtensions
    {
        #region Methods

        internal static async Task HandleAsync(this Exception exception, ILogger logger, Page page = null)
        {
            NullGuard.NotNull(exception, nameof(exception))
                .NotNull(logger, nameof(logger));

            if (exception is IntranetGuiUserFriendlyException intranetGuiUserFriendlyException)
            {
                logger.LogWarning(intranetGuiUserFriendlyException, intranetGuiUserFriendlyException.ToString());

                await intranetGuiUserFriendlyException.HandleAsync(page);

                return;
            }

            if (exception is IntranetGuiExceptionBase intranetGuiException)
            {
                logger.LogError(intranetGuiException, intranetGuiException.ToString());

                await intranetGuiException.HandleAsync();

                return;
            }

            logger.LogCritical(exception, exception.ToString());

            if (await exception.HandleAsync(page))
            {
                throw new IntranetGuiSystemException(exception.Message, exception);
            }
        }

        private static Task HandleAsync(this IntranetGuiUserFriendlyException exception, Page page = null)
        {
            NullGuard.NotNull(exception, nameof(exception));

            return page == null
                ? Task.CompletedTask
                : page.DisplayAlert("Der er opstået en fejl", exception.Message, "OK");
        }

        private static Task HandleAsync(this IntranetGuiExceptionBase exception)
        {
            NullGuard.NotNull(exception, nameof(exception));

            return Task.CompletedTask;
        }

        private static Task<bool> HandleAsync(this Exception exception, Page page = null)
        {
            NullGuard.NotNull(exception, nameof(exception));

            return page == null
                ? Task.FromResult(false)
                : page.DisplayAlert("Der er opstået en kritisk fejl", "Det anbefales, at du afbryder applikationen. Dog kan applikationen fortsætte i en mulig uønsket tilstand.", "Afbryd", "Fortsæt");
        }

        #endregion
    }
}