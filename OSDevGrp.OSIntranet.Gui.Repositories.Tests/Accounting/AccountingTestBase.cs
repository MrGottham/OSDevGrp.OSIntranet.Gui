using Microsoft.Extensions.DependencyInjection;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting;
using System;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Tests.Accounting
{
    public abstract class AccountingTestBase : TestBase
    {
        #region Methods

        protected static IAccountingRepository CreateTestAccountingRepository(IServiceProvider serviceProvider)
        {
            NullGuard.NotNull(serviceProvider, nameof(serviceProvider));

            return serviceProvider.GetRequiredService<IAccountingRepository>();
        }

        protected static IOnlineAccountingRepository CreateTestOnlineAccountingRepository(IServiceProvider serviceProvider)
        {
            NullGuard.NotNull(serviceProvider, nameof(serviceProvider));

            return serviceProvider.GetRequiredService<IOnlineAccountingRepository>();
        }

        #endregion
    }
}