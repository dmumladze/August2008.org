using System;

namespace August2008.Common.Interfaces
{
    public interface IPayPalService
    {
        bool TryReplyToIpn(byte[] bytes, out string response);
    }
}
