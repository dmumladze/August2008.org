using System;
using System.Collections.Generic;
using System.Data;
using August2008.Common;
using August2008.Model;
using August2008.Common.Interfaces;

namespace August2008.Data
{
    public class AccountRepository : IAccountRepository
    {
        public User GetUser(int userId)
        {
            var user = new User();
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.GetUserByUserId");
                db.AddInParameter("@UserId", DbType.Int32, userId);
                try
                {
                    db.ReadInto(user,
                                user.Profile,
                                user.Profile.Lang,
                                user.OAuth,
                                user.Roles);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return user;
        }
        public bool TryGetUserIdByProviderId(string providerId, out int? userId)
        {
            userId = null;
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.GetUserIdByProviderId");
                db.AddInParameter("@ProviderId", DbType.String, providerId);
                try
                {
                    userId = db.ExecuteScalar<int?>();
                }
                catch (Exception)
                {                    
                    throw;
                }                
            }
            return userId.HasValue;
        }
        public User CreateUser(User user)
        {
            using (var tran = new DbTransactionManager())
            {
                tran.BeginTransaction();
                using (var db = new DataAccess(tran))
                {                    
                    try
                    {
                        db.CreateStoredProcCommand("dbo.CreateUser");
                        db.AddInParameter("@Email", DbType.String, user.Email);
                        db.AddInParameter("@DisplayName", DbType.String, user.DisplayName);
                        db.AddOutParameter("@UserId", DbType.Int32);
                        db.ExecuteNonQuery();
                        user.UserId = db.GetParameterValue<int>("@UserId");

                        db.CreateStoredProcCommand("dbo.CreateOAuthUser");
                        db.AddInParameter("@UserId", DbType.String, user.UserId);
                        db.AddInParameter("@ProviderId", DbType.String, user.OAuth.ProviderId);
                        db.AddInParameter("@ProviderName", DbType.String, user.OAuth.ProviderName);
                        db.AddInParameter("@ProviderData", DbType.Xml, user.OAuth.ProviderData.ToDbXml());
                        db.AddOutParameter("@OAuthUserId", DbType.Int32);
                        db.ExecuteNonQuery();
                        user.OAuth.OAuthUserId = db.GetParameterValue<int>("@OAuthUserId");

                        db.CreateStoredProcCommand("dbo.CreateUserProfile");
                        db.AddInParameter("@UserId", DbType.String, user.UserId);
                        db.AddInParameter("@LanguageId", DbType.String, user.Profile.Lang.LanguageId);
                        db.AddInParameter("@Dob", DbType.String, user.Profile.Dob);
                        db.AddInParameter("@Nationality", DbType.String, user.Profile.Nationality);
                        db.AddOutParameter("@UserProfileId", DbType.Int32);
                        db.ExecuteNonQuery();
                        user.Profile.UserProfileId = db.GetParameterValue<int>("@UserProfileId");

                        tran.Commit();

                        user = GetUser(user.UserId);
                    }
                    catch (Exception)
                    {
                        tran.Rollback();
                    }
                    return user;
                }
            }
        }
    }
}
