using System;
using August2008.Common.Interfaces;

namespace August2008.Tests.Fakes
{
    public class FakePayPalService : IPayPalService
    {
        static FakePayPalService()
        {
            Success = true;
            ResponseCode = "verified";
        }
        public bool TryReplyToIpn(byte[] bytes, out string response)
        {
            response = ResponseCode;
            return Success;
        }
        public static bool Success;
        public static string ResponseCode;
    }
}
