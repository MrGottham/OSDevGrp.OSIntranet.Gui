using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security.Options;
using System;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Tests.Security
{
    public abstract class SecurityTestBase : TestBase
    {
        #region Methods

        protected static IAccessTokenProvider CreateTestAccessTokenProvider(IServiceProvider serviceProvider)
        {
            NullGuard.NotNull(serviceProvider, nameof(serviceProvider));

            return new Repositories.Security.AccessTokenProvider(serviceProvider.GetRequiredService<IOptions<SecurityOptions>>(), serviceProvider.GetRequiredService<IEventPublisher>());
        }

        #endregion
    }
}