using System;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Clients
{
    internal static class EnumConverter
    {
        #region Methods

        internal static TDestination Convert<TSource, TDestination>(this TSource source) where TSource : Enum where TDestination : struct
        {
            return Enum.Parse<TDestination>(source.ToString());
        }

        #endregion
    }
}