using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using August2008.Common;
using August2008.Common.Interfaces;
using August2008.Model;

namespace August2008.Data   
{
    public class DonationRepository : IDonationRepository
    {
        public Donation CreateDonation(Donation donation)
        {
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.CreateDonation");
                db.AddInParameter("@DonationProviderId", DbType.Int32, donation.DonationProviderId);
                db.AddInParameter("@UserId", DbType.Int32, donation.UserId);
                db.AddInParameter("@Amount", DbType.Decimal, donation.Amount);
                db.AddInParameter("@Currency", DbType.String, donation.Currency);
                db.AddInParameter("@FirstName", DbType.String, donation.FirstName);
                db.AddInParameter("@LastName", DbType.String, donation.LastName);
                db.AddInParameter("@Email", DbType.String, donation.Email);
                db.AddInParameter("@UserMessage", DbType.String, donation.UserMessage);
                db.AddInParameter("@ProviderData", DbType.Xml, donation.ProviderData.ToDbXml());
                db.AddOutParameter("@DonationId", DbType.Int32);
                try
                {
                    db.ExecuteNonQuery();
                    donation.DonationId = db.GetParameterValue<int>("@DonationId");
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return donation;
        }
        public void UpdateDonation(Donation donation)
        {
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.UpdateDonation");
                db.AddInParameter("@DonationId", DbType.Int32, donation.DonationId);
                db.AddInParameter("@UserMessage", DbType.String, donation.UserMessage);
                try
                {
                    db.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
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
                catch (Exception)
                {
                    throw;
                }
                return criteria;
            }   
        }
    }
}
