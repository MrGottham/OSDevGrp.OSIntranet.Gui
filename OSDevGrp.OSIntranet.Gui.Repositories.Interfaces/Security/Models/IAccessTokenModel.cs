using System;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security.Models
{
    public interface IAccessTokenModel
    {
        string TokenType { get; }

        string TokenValue { get; }

        DateTime Expires { get; }
    }
}