using System;
using August2008.Model;

namespace August2008.Common.Interfaces
{
    public interface IDonationService
    {
        bool ProcessPayPalDonation(byte[] ipnBytes, PayPalTransaction transaction);
    }
}
