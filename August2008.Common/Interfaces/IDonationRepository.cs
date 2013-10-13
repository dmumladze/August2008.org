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
        void UpdateUserMessage(Donation donation);
        void CompleteTransaction(Donation donation);
        string GetUserMessage(int donationId);
        bool TransactionCompleted(string externalId);
        List<MapPoint> GetDonationsByCity(MapPoint northwest, MapPoint southeast);
        List<MapPoint> GetDonationsByState(MapPoint northwest, MapPoint southeast);
        List<MapPoint> GetDonationsByCountry(MapPoint northwest, MapPoint southeast);
        DonationSearchCriteria SearchDonations(DonationSearchCriteria criteria);
    }
}
