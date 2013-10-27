using System;
using August2008.Model;

namespace August2008.Common.Interfaces
{
    public interface IDonationService
    {
        bool ValidateDonation(PayPalVariables variables);
        bool ValidateSubscription(PayPalVariables variables);
        bool ProcessPayPalDonation(byte[] ipnBytes, PayPalVariables variables);
        bool ProcessPayPalSubscription(byte[] ipnBytes, PayPalVariables variables);
    }
}
