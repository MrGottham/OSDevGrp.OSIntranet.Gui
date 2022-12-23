using Microsoft.Extensions.Options;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;
using OSDevGrp.OSIntranet.Gui.Repositories.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Exceptions;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting.Events;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting.Models;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Accounting
{
    internal class OfflineAccountingRepository : OfflineRepositoryBase, IOfflineAccountingRepository, IEventHandler<IAccountingCollectionReceivedEvent>
    {
        #region Constructor

        public OfflineAccountingRepository(IOptions<OfflineOptions> offlineOptions, IOfflineDataProvider offlineDataProvider, IOfflineDataCommitter offlineDataCommitter, IEventPublisher eventPublisher) 
            : base(offlineOptions, offlineDataProvider, offlineDataCommitter, eventPublisher)
        {
            eventPublisher.AddSubscriber(this);
        }

        #endregion

        #region Methdos

        public Task<IEnumerable<IAccountingModel>> GetAccountingsAsync()
        {
            try
            {
                lock (OfflineDataProvider.GetSyncRoot())
                {
                    XmlNamespaceManager namespaceManager = OfflineDataNamespaceManagerFactory.Build();
                    XmlDocument offlineDataDocument = OfflineDataProvider.GetOfflineDataDocumentAsync().GetAwaiter().GetResult();

                    XmlNode? offlineDataNode = offlineDataDocument.DocumentElement;
                    if (offlineDataNode == null)
                    {
                        return Task.FromResult<IEnumerable<IAccountingModel>>(Array.Empty<IAccountingModel>());
                    }

                    XmlNodeList? accountingNodes = offlineDataNode.SelectAccountingNodes(namespaceManager);
                    if (accountingNodes == null)
                    {
                        return Task.FromResult<IEnumerable<IAccountingModel>>(Array.Empty<IAccountingModel>());
                    }

                    return Task.FromResult<IEnumerable<IAccountingModel>>(accountingNodes.OfType<XmlElement>()
                        .Select(accountingElement => accountingElement.ToAccountingModel(namespaceManager))
                        .OrderBy(accountingModel => accountingModel.Number)
                        .ToArray());
                }
            }
            catch (XmlException ex)
            {
                throw ex.ToException();
            }
        }

        public async Task HandleAsync(IAccountingCollectionReceivedEvent accountingCollectionReceivedEvent)
        {
            NullGuard.NotNull(accountingCollectionReceivedEvent, nameof(accountingCollectionReceivedEvent));

            foreach (IAccountingModel accounting in accountingCollectionReceivedEvent.Accountings)
            {
                await OfflineDataCommitter.PushAsync(accounting);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            EventPublisher.RemoveSubscriber(this);
        }

        #endregion
    }
}