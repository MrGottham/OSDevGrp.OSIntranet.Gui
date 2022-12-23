using Microsoft.Extensions.Options;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Exceptions;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core.Options;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Core
{
    internal class OfflineDataProvider : IOfflineDataProvider
    {
        #region Private variables

        private readonly IOptions<OfflineOptions> _offlineOptions;
        private readonly object _syncRoot = new object();

        #endregion

        #region Constructor

        public OfflineDataProvider(IOptions<OfflineOptions> offlineOptions)
        {
            NullGuard.NotNull(offlineOptions, nameof(offlineOptions));

            _offlineOptions = offlineOptions;
        }

        #endregion

        #region Methods

        public Task<XmlDocument> GetOfflineDataDocumentAsync()
        {
            try
            {
                lock (GetSyncRoot())
                {
                    OfflineDataValidator.Validate(_offlineOptions.Value.OfflineDataDocument);

                    return Task.FromResult(_offlineOptions.Value.OfflineDataDocument);
                }
            }
            catch (XmlSchemaValidationException ex)
            {
                throw ex.ToException();
            }
            catch (XmlException ex)
            {
                throw ex.ToException();
            }
        }

        public object GetSyncRoot() => _syncRoot;

        #endregion
    }
}