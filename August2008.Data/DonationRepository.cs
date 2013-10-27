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
        private readonly ILog Log;

        public DonationRepository(ILog log)
        {
            Log = log;
        }
        public Donation CreateDonation(Donation donation)
        {
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.CreateDonation");
                db.AddInputParameter("@DonationProviderId", donation.DonationProviderId);
                db.AddInputParameter("@UserId", donation.UserId);
                db.AddInputParameter("@ExternalId", donation.ExternalId);
                db.AddInputParameter("@ExternalStatus", donation.ExternalStatus);
                db.AddInputParameter("@IsCompleted", donation.IsCompleted);
                db.AddInputParameter("@CountryId", donation.CountryId);
                db.AddInputParameter("@StateId", donation.StateId);
                db.AddInputParameter("@CityId", donation.CityId);
                db.AddInputParameter("@Amount", donation.Amount);
                db.AddInputParameter("@Currency", donation.Currency);
                db.AddInputParameter("@UserMessage", donation.UserMessage);
                db.AddInputParameter("@ProviderData", donation.ProviderXml);                
                db.AddInputParameter("@DonationSubscriptionId", donation.DonationSubscriptionId);
                db.AddInputParameter("@TransactionType", donation.TransactionType);
                db.AddOutputParameter("@DonationId", DbType.Int32);
                try
                {
                    db.ExecuteNonQuery();
                    donation.DonationId = db.GetParameterValue<int>("@DonationId");
                }
                catch (Exception ex)
                {
                    Log.Error("Error while creating donation.", ex);
                    throw;
                }
            }
            return donation;
        }
        public Donation RefundDonation(string externalId, string externalStatus)
        {
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.RefundDonation");
                db.AddInputParameter("@ExternalId", externalId);
                db.AddInputParameter("@ExternalStatus", externalStatus);
                try
                {
                    var donation = new Donation();
                    db.ReadInto(donation);
                    return donation;
                }
                catch (Exception ex)
                {
                    Log.Error("Error while refunding donation.", ex);
                    throw;
                }
            }
        }
        public void UpdateUserMessage(Donation donation)
        {
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.UpdateUserMessage");
                db.AddInputParameter("@DonationId", DbType.Int32, donation.DonationId);
                db.AddInputParameter("@UserId", DbType.Int32, donation.UserId);
                db.AddInputParameter("@UserMessage", DbType.String, donation.UserMessage);
                try
                {
                    db.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Log.Error("Error while updating user message.", ex);
                    throw;
                }
            }
        }
        public void CompleteTransaction(Donation donation)
        {
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.CompleteDonation");
                db.AddInputParameter("@ExternalId", DbType.String, donation.ExternalId);
                db.AddInputParameter("@ExternalStatus", DbType.String, donation.ExternalStatus);
                db.AddInputParameter("@IsCompleted", DbType.Boolean, donation.IsCompleted);
                db.AddInputParameter("@CountryId", DbType.Int32, donation.CountryId);
                db.AddInputParameter("@StateId", DbType.Int32, donation.StateId);
                db.AddInputParameter("@CityId", DbType.Int32, donation.CityId);
                try
                {
                    db.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Log.Error("Error while verifying donation.", ex);
                    throw;
                }
            }
        }
        public bool DonationCompleted(string externalId)
        {
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.GetTransactionCompleted");
                db.AddInputParameter("@ExternalId", DbType.String, externalId);
                try
                {
                    return db.ExecuteScalar<bool>();
                }
                catch (Exception ex)
                {
                    Log.Error("Error while validating transaction.", ex);
                    throw;
                }
            }
        }
        public bool SubscriptionCompleted(string subscriptionId)
        {
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.GetSubscriptionCompleted");
                db.AddInputParameter("@SubscriptionId", subscriptionId);
                try
                {
                    return db.ExecuteScalar<bool>();
                }
                catch (Exception ex)
                {
                    Log.Error("Error while validating transaction.", ex);
                    throw;
                }
            }
        }
        public string GetUserMessage(int donationId)
        {
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.GetDonationMessage");
                db.AddInputParameter("@DonationId", DbType.Int32, donationId);
                try
                {
                    return db.ExecuteScalar<string>();
                }
                catch (Exception ex)
                {
                    Log.Error("Error while updating user message.", ex);
                    throw;
                }
            }
        }
        public DonationSearchCriteria SearchDonations(DonationSearchCriteria criteria)
        {
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.SearchDonations");
                db.AddInputParameter("@UserId", DbType.Int32, criteria.UserId);
                db.AddInputParameter("@Name", DbType.String, criteria.Name);
                db.AddInputParameter("@FromDate", DbType.DateTime, criteria.FromDate.ToFromDate());
                db.AddInputParameter("@ToDate", DbType.DateTime, criteria.ToDate.ToToDate());
                try
                {
                    criteria.Result = new List<Donation>();
                    db.ReadInto(criteria.Result);
                }
                catch (Exception ex)
                {
                    Log.Error("Error while searching donations.", ex);
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
                db.AddInputParameter("@NwLat", DbType.Double, northwest.Latitude);
                db.AddInputParameter("@NwLng", DbType.Double, northwest.Longitude);
                db.AddInputParameter("@SeLat", DbType.Double, southeast.Latitude);
                db.AddInputParameter("@SeLng", DbType.Double, southeast.Longitude);
                try
                {
                    var points = new List<MapPoint>();
                    db.ReadInto(points);
                    return points;
                }
                catch (Exception ex)
                {
                    Log.Error("Error while searching donations.", ex);
                    throw;
                }
            }  
        }
        public DonationSubscription CreateDonationSubscription(DonationSubscription subscription)
        {
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.CreateDonationSubscription");
                db.AddInputParameter("@StartDate", subscription.StartDate);
                db.AddInputParameter("@EndDate", subscription.EndDate);
                db.AddInputParameter("@RecurrenceTimes", subscription.RecurrenceTimes);
                db.AddInputParameter("@SubscriptionId", subscription.SubscriptionId);
                db.AddInputParameter("@UserId", subscription.UserId);
                db.AddInputParameter("@Username", subscription.Username);
                db.AddInputParameter("@Password", subscription.Password);
                db.AddInputParameter("@ProviderData", subscription.ProviderXml);
                db.AddOutputParameter("@DonationSubscriptionId", DbType.Int32);
                try
                {
                    db.ExecuteNonQuery();
                    subscription.DonationSubscriptionId = db.GetParameterValue<int>("@DonationSubscriptionId");
                }
                catch (Exception ex)
                {
                    Log.Error("Error while crating subscription.", ex);
                    throw;
                }
            }
            return subscription;
        }
        public DonationSubscription GetDonationSubscription(string subscriptionId)
        {
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.GetDonationSubscription");
                db.AddInputParameter("@SubscriptionId", subscriptionId);
                try
                {
                    var subscription = new DonationSubscription();
                    db.ReadInto(subscription);
                    return subscription;
                }
                catch (Exception ex)
                {
                    Log.Error("Error while gettng subscription.", ex);
                    throw;
                }
            }
        }
    }
}
