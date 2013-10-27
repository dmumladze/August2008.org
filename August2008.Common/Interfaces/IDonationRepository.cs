using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using August2008.Model;

namespace August2008.Common.Interfaces 
{
    public interface IDonationRepository
    { 
        Donation CreateDonation(Donation donation);
        Donation RefundDonation(string externalId, string externalStatus); 
        void UpdateUserMessage(Donation donation);
        void CompleteTransaction(Donation donation);
        string GetUserMessage(int donationId);
        bool DonationCompleted(string externalId);
        bool SubscriptionCompleted(string subscriptionId); 
        List<MapPoint> GetDonationsByCity(MapPoint northwest, MapPoint southeast);
        List<MapPoint> GetDonationsByState(MapPoint northwest, MapPoint southeast);
        List<MapPoint> GetDonationsByCountry(MapPoint northwest, MapPoint southeast);
        DonationSearchCriteria SearchDonations(DonationSearchCriteria criteria);
        DonationSubscription CreateDonationSubscription(DonationSubscription subscription);
        DonationSubscription GetDonationSubscription(string subscriptionId);
    }
}
