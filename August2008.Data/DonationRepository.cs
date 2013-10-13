using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using August2008.Common;
using August2008.Common.Interfaces;
using August2008.Model;
using log4net;

namespace August2008.Data   
{
    public class DonationRepository : IDonationRepository
    {
        private readonly ILog Logger;

        public DonationRepository(ILog logger)
        {
            Logger = logger;
        }
        public Donation CreateDonation(Donation donation)
        {
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.CreateDonation");
                db.AddInParameter("@DonationProviderId", DbType.Int32, donation.DonationProviderId);
                db.AddInParameter("@UserId", DbType.String, donation.UserId);
                db.AddInParameter("@ExternalId", DbType.String, donation.ExternalId);
                db.AddInParameter("@ExternalStatus", DbType.String, donation.ExternalStatus);
                db.AddInParameter("@IsCompleted", DbType.Boolean, donation.IsCompleted);
                db.AddInParameter("@CountryId", DbType.Int32, donation.CountryId);
                db.AddInParameter("@StateId", DbType.Int32, donation.StateId);
                db.AddInParameter("@CityId", DbType.Int32, donation.CityId);
                db.AddInParameter("@Amount", DbType.Decimal, donation.Amount);
                db.AddInParameter("@Currency", DbType.String, donation.Currency);
                db.AddInParameter("@UserMessage", DbType.String, donation.UserMessage);
                db.AddInParameter("@ProviderData", DbType.Xml, donation.ProviderXml);
                db.AddOutParameter("@DonationId", DbType.Int32);
                try
                {
                    db.ExecuteNonQuery();
                    donation.DonationId = db.GetParameterValue<int>("@DonationId");
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while creating donation.", ex);
                    throw;
                }
            }
            return donation;
        }
        public void UpdateUserMessage(Donation donation)
        {
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.UpdateUserMessage");
                db.AddInParameter("@DonationId", DbType.Int32, donation.DonationId);
                db.AddInParameter("@UserId", DbType.Int32, donation.UserId);
                db.AddInParameter("@UserMessage", DbType.String, donation.UserMessage);
                try
                {
                    db.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while updating user message.", ex);
                    throw;
                }
            }
        }
        public void CompleteTransaction(Donation donation)
        {
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.CompleteDonation");
                db.AddInParameter("@ExternalId", DbType.String, donation.ExternalId);
                db.AddInParameter("@ExternalStatus", DbType.String, donation.ExternalStatus);
                db.AddInParameter("@IsCompleted", DbType.Boolean, donation.IsCompleted);
                db.AddInParameter("@CountryId", DbType.Int32, donation.CountryId);
                db.AddInParameter("@StateId", DbType.Int32, donation.StateId);
                db.AddInParameter("@CityId", DbType.Int32, donation.CityId);
                try
                {
                    db.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while verifying donation.", ex);
                    throw;
                }
            }
        }
        public bool TransactionCompleted(string externalId)
        {
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.GetTransactionCompleted");
                db.AddInParameter("@ExternalId", DbType.String, externalId);
                try
                {
                    return db.ExecuteScalar<bool>();
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while validating transaction.", ex);
                    throw;
                }
            }
        }
        public string GetUserMessage(int donationId)
        {
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.GetDonationMessage");
                db.AddInParameter("@DonationId", DbType.Int32, donationId);
                try
                {
                    return db.ExecuteScalar<string>();
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while updating user message.", ex);
                    throw;
                }
            }
        }
        public DonationSearchCriteria SearchDonations(DonationSearchCriteria criteria)
        {
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.SearchDonations");
                db.AddInParameter("@UserId", DbType.Int32, criteria.UserId);
                db.AddInParameter("@Name", DbType.String, criteria.Name);
                db.AddInParameter("@FromDate", DbType.DateTime, criteria.FromDate.ToFromDate());
                db.AddInParameter("@ToDate", DbType.DateTime, criteria.ToDate.ToToDate());
                try
                {
                    criteria.Result = new List<Donation>();
                    db.ReadInto(criteria.Result);
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while searching donations.", ex);
                    throw;
                }
                return criteria;
            }   
        }
        public List<MapPoint> GetDonationsByCity(MapPoint northwest, MapPoint southeast)
        {
            return GetDonationLocations("dbo.GetDonationsByCity", northwest, southeast);
        }
        public List<MapPoint> GetDonationsByState(MapPoint northwest, MapPoint southeast)
        {
            return GetDonationLocations("dbo.GetDonationsByState", northwest, southeast);
        }
        public List<MapPoint> GetDonationsByCountry(MapPoint northwest, MapPoint southeast)
        {
            return GetDonationLocations("dbo.GetDonationsByCountry", northwest, southeast);
        }
        private List<MapPoint> GetDonationLocations(string sproc, MapPoint northwest, MapPoint southeast)  
        {            
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand(sproc);
                db.AddInParameter("@NwLat", DbType.Double, northwest.Latitude);
                db.AddInParameter("@NwLng", DbType.Double, northwest.Longitude);
                db.AddInParameter("@SeLat", DbType.Double, southeast.Latitude);
                db.AddInParameter("@SeLng", DbType.Double, southeast.Longitude);
                try
                {
                    var points = new List<MapPoint>();
                    db.ReadInto(points);
                    return points;
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while searching donations.", ex);
                    throw;
                }
            }  
        }
    }
}
