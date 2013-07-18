using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
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
        public bool TryGetUserRegistered(string providerId, out int? userId, out bool isOAuthUser, out bool isRegistered)
        {
            userId = null;
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.GetUserRegistered");
                db.AddInParameter("@ProviderId", DbType.String, providerId);
                db.AddOutParameter("@UserId", DbType.Int32);
                db.AddOutParameter("@IsRegistered", DbType.Boolean);
                db.AddOutParameter("@IsOAuthUser", DbType.Boolean);
                try
                {
                    db.ExecuteNonQuery();
                    userId = db.GetParameterValue<int?>("@UserId");
                    isOAuthUser = db.GetParameterValue<bool>("@IsOAuthUser");
                    isRegistered = db.GetParameterValue<bool>("@IsRegistered");
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
            using (var tran = new TransactionScope())
            using (var db = new DataAccess())
            {
                try
                {
                    db.CreateStoredProcCommand("dbo.CreateUser");
                    db.AddInParameter("@Email", DbType.String, user.Email);
                    db.AddInParameter("@DisplayName", DbType.String, user.DisplayName);
                    db.AddInParameter("@Password", DbType.String, user.Password);
                    db.AddOutParameter("@UserId", DbType.Int32);
                    db.ExecuteNonQuery();
                    user.UserId = db.GetParameterValue<int>("@UserId");

                    user.OAuth.UserId = user.UserId;
                    user.OAuth = CreateOAuthUser(user.OAuth);

                    db.CreateStoredProcCommand("dbo.CreateUserProfile");
                    db.AddInParameter("@UserId", DbType.String, user.UserId);
                    db.AddInParameter("@LanguageId", DbType.String, user.Profile.Lang.LanguageId);
                    db.AddInParameter("@Dob", DbType.String, user.Profile.Dob);
                    db.AddInParameter("@Nationality", DbType.String, user.Profile.Nationality);
                    db.AddOutParameter("@UserProfileId", DbType.Int32);
                    db.ExecuteNonQuery();
                    user.Profile.UserProfileId = db.GetParameterValue<int>("@UserProfileId");

                    user = GetUser(user.UserId);

                    tran.Complete();
                }
                catch (Exception)
                {                    
                    throw;
                }
                return user;
            }
        }
        public OAuthUser CreateOAuthUser(OAuthUser user)
        {
            using (var db = new DataAccess())
            {
                try
                {
                    db.CreateStoredProcCommand("dbo.CreateOAuthUser");
                    db.AddInParameter("@UserId", DbType.String, user.UserId);
                    db.AddInParameter("@ProviderId", DbType.String, user.ProviderId);
                    db.AddInParameter("@ProviderName", DbType.String, user.ProviderName);
                    db.AddInParameter("@ProviderData", DbType.Xml, user.ProviderData.ToDbXml());
                    db.AddOutParameter("@OAuthUserId", DbType.Int32);
                    db.ExecuteNonQuery();
                    user.OAuthUserId = db.GetParameterValue<int>("@OAuthUserId");
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return user;
        }
        public IEnumerable<User> GetUsers()
        {
            using (var db = new DataAccess())
            {
                try
                {
                    db.CreateStoredProcCommand("dbo.GetUsers");
                    var users = new List<User>();
                    db.ReadInto(users);
                    return users;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public IEnumerable<User> SearchUsers(string name = null)
        {
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.SearchUsers");   
                db.AddInParameter("@StartsWith", DbType.String, name);
                try
                {
                    var users = new List<User>();                   
                    db.ReadInto(users);
                    return users;
                }
                catch (Exception)
                {
                    throw;
                }
            }  
        }
        public void AssignUserToRoles(int userId, List<int> roles)
        {
            using (var db = new DataAccess())
            {
                try
                {
                    db.CreateStoredProcCommand("dbo.AssignUserToRoles");
                    foreach (var id in roles)
                    {
                        db.AddInParameter("@UserId", DbType.Int32, userId);
                        db.AddInParameter("@RoleId", DbType.Int32, id);
                        db.ExecuteNonQuery();
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public void RevokeUserFromRoles(int userId, List<int> roles)
        {
            using (var db = new DataAccess())
            {
                try
                {
                    db.CreateStoredProcCommand("dbo.RevokeUserFromRole");
                    foreach (var id in roles)
                    {
                        db.AddInParameter("@UserId", DbType.Int32, userId);
                        db.AddInParameter("@RoleId", DbType.Int32, id);
                        db.ExecuteNonQuery();
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
