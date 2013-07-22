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
                db.AddInParameter("@DonationId", DbType.Int32, donation.DonationId);
                db.AddInParameter("@DonationProviderId", DbType.Int32, donation.DonationProviderId);
                db.AddInParameter("@UserId", DbType.Int32, donation.UserId);
                db.AddInParameter("@Amount", DbType.Int32, donation.Amount);
                db.AddInParameter("@Currency", DbType.Int32, donation.Currency);
                db.AddInParameter("@FirstName", DbType.Int32, donation.FirstName);
                db.AddInParameter("@LastName", DbType.Int32, donation.LastName);
                db.AddInParameter("@Email", DbType.Int32, donation.Email);
                db.AddInParameter("@UserMessage", DbType.Int32, donation.UserMessage);
                db.AddInParameter("@ProviderData", DbType.Int32, donation.ProviderData.ToDbXml());
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
                db.AddInParameter("@UserMessage", DbType.Int32, donation.UserMessage);
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
    }
}
