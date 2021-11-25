using System;

namespace OSDevGrp.OSIntranet.Gui.Resources
{
    internal abstract class EmbeddedResourceBase
    {
        #region Private variables

        private byte[] _value;
        private readonly object _syncRoot = new object();

        #endregion

        #region Properties

        internal abstract string Name { get; }

        internal byte[] Value
        {
            get
            {
                lock (_syncRoot)
                {
                    if (_value != null)
                    {
                        return _value;
                    }

                    return _value = Convert.FromBase64String(ValueAsBase64);
                }
            }
        }

        protected abstract string ValueAsBase64 { get; }

        #endregion
    }
}