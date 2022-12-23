using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;
using OSDevGrp.OSIntranet.Gui.Repositories.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Exceptions;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Accounting
{
    internal class AccountingRepository : RepositoryBase, IAccountingRepository
    {
        #region Private variables

        private bool _isOffline;
        private readonly IOnlineAccountingRepository _onlineAccountingRepository;
        private readonly IOfflineAccountingRepository _offlineAccountingRepository;

        #endregion

        #region Constructors

        public AccountingRepository(IOnlineAccountingRepository onlineAccountingRepository, IOfflineAccountingRepository offlineAccountingRepository, IEventPublisher eventPublisher) 
            : this(onlineAccountingRepository, offlineAccountingRepository, eventPublisher, false)
        {
        }

        internal AccountingRepository(IOnlineAccountingRepository onlineAccountingRepository, IOfflineAccountingRepository offlineAccountingRepository, IEventPublisher eventPublisher, bool isOffline)
            : base(eventPublisher)
        {
            NullGuard.NotNull(onlineAccountingRepository, nameof(onlineAccountingRepository))
                .NotNull(offlineAccountingRepository, nameof(offlineAccountingRepository));

            _onlineAccountingRepository = onlineAccountingRepository;
            _offlineAccountingRepository = offlineAccountingRepository;
            _isOffline = isOffline;
        }

        #endregion

        #region Methdos

        public async Task<IEnumerable<IAccountingModel>> GetAccountingsAsync()
        {
            try
            {
                if (_isOffline)
                {
                    return await _offlineAccountingRepository.GetAccountingsAsync();
                }

                return await _onlineAccountingRepository.GetAccountingsAsync();
            }
            catch (IntranetGuiOfflineException)
            {
                _isOffline = true;

                await EventPublisher.PublishSystemWentOfflineEventAsync();

                return await _offlineAccountingRepository.GetAccountingsAsync();
            }
        }

        #endregion
    }
}