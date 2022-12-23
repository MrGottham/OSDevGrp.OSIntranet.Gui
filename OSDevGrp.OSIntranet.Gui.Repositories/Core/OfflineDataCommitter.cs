using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;
using OSDevGrp.OSIntranet.Gui.Repositories.Core.Events;
using OSDevGrp.OSIntranet.Gui.Repositories.Exceptions;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting.Models;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core;
using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Core
{
    internal class OfflineDataCommitter : IOfflineDataCommitter
    {
        #region Private variables

        private BackgroundWorker? _backgroundWorker;
        private readonly IOfflineDataProvider _offlineDataProvider;
        private readonly IEventPublisher _eventPublisher;
        private readonly ConcurrentQueue<Action<XmlDocument, XmlNamespaceManager>> _pushActions = new ConcurrentQueue<Action<XmlDocument, XmlNamespaceManager>>();
        private readonly object _syncRoot = new object();

        #endregion

        #region Constructor

        public OfflineDataCommitter(IOfflineDataProvider offlineDataProvider, IEventPublisher eventPublisher)
        {
            NullGuard.NotNull(offlineDataProvider, nameof(offlineDataProvider))
                .NotNull(eventPublisher, nameof(eventPublisher));

            _offlineDataProvider = offlineDataProvider;
            _eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        public void Dispose()
        {
            lock (_syncRoot)
            {
                if (_backgroundWorker == null)
                {
                    return;
                }

                _backgroundWorker.CancelAsync();
                while (_backgroundWorker.IsBusy)
                {
                    Task.Delay(250).GetAwaiter().GetResult();
                }
                _backgroundWorker.Dispose();
            }
        }

        public async Task PushAsync(IAccountingModel accountingModel)
        {
            NullGuard.NotNull(accountingModel, nameof(accountingModel));

            _pushActions.Enqueue((offlineDataDocument, namespaceManager) =>
            {
                XmlNode? offlineDataNode = offlineDataDocument.DocumentElement;
                if (offlineDataNode == null)
                {
                    return;
                }

                int accountingIdentification = accountingModel.Number;
                int letterHeadIdentification = accountingModel.LetterHead.Number;

                XmlElement accountingElement = PushElement(offlineDataNode, parent => parent.SelectAccountingNode(accountingIdentification, namespaceManager), "Accounting");
                PushRequiredAttribute(accountingElement, "number", accountingIdentification.ToString(CultureInfo.InvariantCulture));
                PushRequiredAttribute(accountingElement, "name", accountingModel.Name);
                PushRequiredAttribute(accountingElement, "letterHeadIdentification", letterHeadIdentification.ToString(CultureInfo.InvariantCulture));
                PushRequiredAttribute(accountingElement, "balanceBelowZero", accountingModel.BalanceBelowZero.ToString());
                PushRequiredAttribute(accountingElement, "backDating", accountingModel.BackDating.ToString(CultureInfo.InvariantCulture));

                XmlElement letterHeadElement = PushElement(offlineDataNode, parent => parent.SelectLetterHeadNode(letterHeadIdentification, namespaceManager), "LetterHead");
                PushRequiredAttribute(letterHeadElement, "number", letterHeadIdentification.ToString(CultureInfo.InvariantCulture));
                PushRequiredAttribute(letterHeadElement, "name", accountingModel.LetterHead.Name);
            });

            await EnsureBackgroundWorkerIsStartedAsync();
        }

        private Task EnsureBackgroundWorkerIsStartedAsync()
        {
            return Task.Run(() =>
            {
                lock (_syncRoot)
                {
                    if (_backgroundWorker == null)
                    {
                        _backgroundWorker = CreateBackgroundWorker().GetAwaiter().GetResult();
                        return;
                    }

                    if (_backgroundWorker.IsBusy)
                    {
                        return;
                    }

                    _backgroundWorker.CancelAsync();
                    while (_backgroundWorker.IsBusy)
                    {
                        Task.Delay(250).GetAwaiter().GetResult();
                    }
                    _backgroundWorker.Dispose();

                    _backgroundWorker = CreateBackgroundWorker().GetAwaiter().GetResult();
                }
            });
        }

        private Task<BackgroundWorker> CreateBackgroundWorker()
        {
            lock (_syncRoot)
            {
                BackgroundWorker worker = new BackgroundWorker
                {
                    WorkerReportsProgress = false,
                    WorkerSupportsCancellation = true
                };
                worker.DoWork += PushOfflineData;

                worker.RunWorkerAsync(new Tuple<IOfflineDataProvider, IEventPublisher, ConcurrentQueue<Action<XmlDocument, XmlNamespaceManager>>>(_offlineDataProvider, _eventPublisher, _pushActions));

                return Task.FromResult(worker);
            }
        }

        private static void PushOfflineData(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            NullGuard.NotNull(sender, nameof(sender))
                .NotNull(doWorkEventArgs, nameof(doWorkEventArgs));

            BackgroundWorker backgroundWorker = (BackgroundWorker)sender;
            Tuple<IOfflineDataProvider, IEventPublisher, ConcurrentQueue<Action<XmlDocument, XmlNamespaceManager>>> tuple = (Tuple<IOfflineDataProvider, IEventPublisher, ConcurrentQueue<Action<XmlDocument, XmlNamespaceManager>>>)doWorkEventArgs.Argument;
            IOfflineDataProvider offlineDataProvider = tuple.Item1;
            IEventPublisher eventPublisher = tuple.Item2;
            ConcurrentQueue<Action<XmlDocument, XmlNamespaceManager>> pushActions = tuple.Item3;

            XmlNamespaceManager namespaceManager = OfflineDataNamespaceManagerFactory.Build();

            DateTime? offlineDataUpdated = null;
            while (backgroundWorker.CancellationPending == false && pushActions.TryDequeue(out Action<XmlDocument, XmlNamespaceManager> pushAction))
            {
                lock (offlineDataProvider.GetSyncRoot())
                {
                    XmlDocument offlineDataDocument = offlineDataProvider.GetOfflineDataDocumentAsync().GetAwaiter().GetResult();
                    try
                    {
                        pushAction(offlineDataDocument, namespaceManager);
                        OfflineDataValidator.Validate(offlineDataDocument);

                        offlineDataUpdated = DateTime.Now;
                    }
                    catch (XmlSchemaValidationException ex)
                    {
                        if (ex.SourceObject is XmlNode node)
                        {
                            node.ParentNode?.RemoveChild(node);
                        }

                        throw ex.ToException();
                    }
                    catch (XmlException ex)
                    {
                        throw ex.ToException();
                    }
                }
            }

            if (offlineDataUpdated == null)
            {
                return;
            }

            lock (offlineDataProvider.GetSyncRoot())
            {
                XmlDocument offlineDataDocument = offlineDataProvider.GetOfflineDataDocumentAsync().GetAwaiter().GetResult();
                eventPublisher.PublishAsync(new OfflineDataUpdatedEvent(offlineDataDocument, offlineDataUpdated.Value)).GetAwaiter().GetResult();
            }
        }

        private static XmlElement PushElement(XmlNode parentNode, Func<XmlNode, XmlNode?> selector, string elementName)
        {
            NullGuard.NotNull(parentNode, nameof(parentNode))
                .NotNull(selector, nameof(selector))
                .NotNullOrWhiteSpace(elementName, nameof(elementName));

            XmlElement? element = (XmlElement?)selector(parentNode);
            if (element != null)
            {
                return element;
            }

            if (parentNode.OwnerDocument == null)
            {
                throw new XmlException($"Cannot create a XML element when the XML node named '{parentNode.Name}' has no owner document.");
            }

            element = parentNode.OwnerDocument.CreateElement(elementName, parentNode.NamespaceURI);
            parentNode.AppendChild(element);

            return element;
        }

        private static void PushRequiredAttribute(XmlElement element, string attributeName, string attributeValue)
        {
            NullGuard.NotNull(element, nameof(element))
                .NotNullOrWhiteSpace(attributeName, nameof(attributeName))
                .NotNullOrWhiteSpace(attributeValue, nameof(attributeValue));

            XmlAttribute? attribute = element.Attributes[attributeName, string.Empty];
            if (attribute == null)
            {
                if (element.OwnerDocument == null)
                {
                    throw new XmlException($"Cannot create a XML attribute when the XML element named '{element.Name}' has no owner document.");
                }

                attribute = element.OwnerDocument.CreateAttribute(attributeName, string.Empty);
                element.Attributes.Append(attribute);
            }

            attribute.Value = attributeValue;
        }

        #endregion
    }
}