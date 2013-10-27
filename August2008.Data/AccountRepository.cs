using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using August2008.Common;
using August2008.Model;
using August2008.Common.Interfaces;
using System.Diagnostics;
using log4net;

namespace August2008.Data
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ILog Logger;

        public AccountRepository(ILog logger)
        {
            Logger = logger;
        }
        public User GetUser(int userId)
        {
            var user = new User();
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.GetUserByUserId");
                db.AddInputParameter("@UserId", DbType.Int32, userId);
                try
                {
                    db.ReadInto(user,
                                user.Profile,
                                user.Profile.Lang,
                                user.OAuth,
                                user.Roles);
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while getting user.", ex);
                    throw;
                }
            }
            return user;
        }
        public bool TryGetUserRegistered(string email, string provider, out int? userId, out bool isOAuthUser)
        {
            userId = null;
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.GetUserRegistered");
                db.AddInputParameter("@Email", DbType.String, email);
                db.AddInputParameter("@Provider", DbType.String, provider);
                db.AddOutputParameter("@UserId", DbType.Int32);
                db.AddOutputParameter("@IsOAuthUser", DbType.Boolean);
                try
                {
                    db.ExecuteNonQuery();
                    userId = db.GetParameterValue<int?>("@UserId");
                    isOAuthUser = db.GetParameterValue<bool>("@IsOAuthUser");
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while checking registered user.", ex);
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
                        db.AddInputParameter("@Email", DbType.String, user.Email);
                        db.AddInputParameter("@DisplayName", DbType.String, user.DisplayName);
                        db.AddOutputParameter("@UserId", DbType.Int32);
                        db.ExecuteNonQuery();
                        user.UserId = db.GetParameterValue<int>("@UserId");

                        user.OAuth.UserId = user.UserId;
                        user.OAuth = CreateOAuthUser(user.OAuth, tran);

                        db.CreateStoredProcCommand("dbo.CreateUserProfile");
                        db.AddInputParameter("@UserId", DbType.String, user.UserId);
                        db.AddInputParameter("@LanguageId", DbType.String, user.Profile.Lang.LanguageId);
                        db.AddInputParameter("@Dob", DbType.String, user.Profile.Dob);
                        db.AddInputParameter("@Nationality", DbType.String, user.Profile.Nationality);
                        db.AddOutputParameter("@UserProfileId", DbType.Int32);
                        db.ExecuteNonQuery();
                        user.Profile.UserProfileId = db.GetParameterValue<int>("@UserProfileId");

                        user = GetUser(user.UserId);
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        Logger.Error("Error while creating user.", ex);
                        throw;
                    }
                    return user;
                }
            }
        }
        public OAuthUser CreateOAuthUser(OAuthUser user)
        {
            using (var tran = new DbTransactionManager())
            {
                try
                {
                    tran.BeginTransaction();
                    user = CreateOAuthUser(user, tran);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    Logger.Error("Error while creating OAuth user.", ex);
                    throw;
                }
            }
            return user;
        }
        private OAuthUser CreateOAuthUser(OAuthUser user, DbTransactionManager tran)
        {
            using (var db = new DataAccess(tran))
            {
                db.CreateStoredProcCommand("dbo.CreateOAuthUser");
                db.AddInputParameter("@UserId", DbType.String, user.UserId);
                db.AddInputParameter("@Email", DbType.String, user.Email);
                db.AddInputParameter("@ProviderId", DbType.String, user.ProviderId);
                db.AddInputParameter("@ProviderName", DbType.String, user.ProviderName);
                db.AddInputParameter("@ProviderData", DbType.Xml, user.ProviderData.ToDbXml());
                db.AddOutputParameter("@OAuthUserId", DbType.Int32);
                try
                {
                    db.ExecuteNonQuery();
                    user.OAuthUserId = db.GetParameterValue<int>("@OAuthUserId");
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while creating OAuth user", ex);
                    throw;
                }
            }
            return user;
        }
        public void UpdateUser(User user)
        {
            using (var tran = new DbTransactionManager())
            {
                tran.BeginTransaction();
                using (var db = new DataAccess(tran))
                {
                    db.CreateStoredProcCommand("dbo.UpdateUser");
                    db.AddInputParameter("@UserId", DbType.Int32, user.UserId);
                    db.AddInputParameter("@Email", DbType.String, user.Email);
                    db.AddInputParameter("@DisplayName", DbType.String, user.DisplayName);
                    try
                    {
                        db.ExecuteNonQuery();

                        if (user.Profile != null)
                        {
                            UpdateUserProfile(user.Profile);
                        }
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        Logger.Error("Error while updating user.", ex);
                        throw;
                    }
                }
            }
        }
        public void UpdateUserProfile(UserProfile profile)
        {
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.UpdateUserProfile");
                db.AddInputParameter("@UserId", DbType.Int32, profile.UserId);
                db.AddInputParameter("@LanguageId", DbType.String, profile.Lang.LanguageId);
                db.AddInputParameter("@Dob", DbType.String, profile.Dob);
                db.AddInputParameter("@Nationality", DbType.String, profile.Nationality);
                try
                {
                    db.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while updating user profile.", ex);
                    throw;
                }
            }
        }
        public void UpdateUserProfileAddress(int userId, Address address)
        {
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.UpdateUserProfileAddress");                
                db.AddInputParameter("@UserId", DbType.Int32, userId);
                db.AddInputParameter("@Street", DbType.String, address.Street);
                db.AddInputParameter("@CityId", DbType.String, address.CityId);
                db.AddInputParameter("@StateId", DbType.String, address.StateId);
                db.AddInputParameter("@CountryId", DbType.String, address.CountryId);
                db.AddInputParameter("@Latitude", DbType.Double, address.Latitude);
                db.AddInputParameter("@Longitude", DbType.Double, address.Longitude);
                try
                {
                    db.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while updating user profile address.", ex);
                    throw;
                }
            }
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
                catch (Exception ex)
                {
                    Logger.Error("Error while getting user.", ex);
                    throw;
                }
            }
        }
        public IEnumerable<User> SearchUsers(string name = null)
        {
            using (var db = new DataAccess())
            {
                db.CreateStoredProcCommand("dbo.SearchUsers");
                db.AddInputParameter("@StartsWith", DbType.String, name);
                try
                {
                    var users = new List<User>();
                    db.ReadInto(users);
                    return users;
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while searching users.", ex);
                    throw;
                }
            }
        }
        public IEnumerable<Role> GetUserRoles(int userId)
        {
            using (var db = new DataAccess())
            {
                try
                {
                    db.CreateStoredProcCommand("dbo.GetUserRoles");
                    db.AddInputParameter("@UserId", DbType.Int32, userId);
                    var roles = new List<Role>();
                    db.ReadInto(roles);
                    return roles;
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while getting user roles.", ex);
                    throw;
                }
            }
        }
        public void AssignUserToRoles(int userId, IEnumerable<int> roles)
        {
            using (var tran = new DbTransactionManager())
            {
                tran.BeginTransaction();
                using (var db = new DataAccess(tran))
                {
                    try
                    {
                        db.CreateStoredProcCommand("dbo.AssignUserToRole");
                        foreach (var id in roles)
                        {
                            db.AddInputParameter("@UserId", DbType.Int32, userId);
                            db.AddInputParameter("@RoleId", DbType.Int32, id);
                            db.ExecuteNonQuery();
                            db.ResetCommand(false);
                        }
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        Logger.Error("Error while assigning roles.", ex);
                        throw;
                    }
                }
            }
        }
        public void RevokeUserFromRoles(int userId, IEnumerable<int> roles)
        {
            using (var tran = new DbTransactionManager())
            {
                tran.BeginTransaction();
                using (var db = new DataAccess(tran))
                {
                    try
                    {
                        db.CreateStoredProcCommand("dbo.RevokeUserFromRole");
                        foreach (var id in roles)
                        {
                            db.AddInputParameter("@UserId", DbType.Int32, userId);
                            db.AddInputParameter("@RoleId", DbType.Int32, id);
                            db.ExecuteNonQuery();
                            db.ResetCommand(false);
                        }
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        Logger.Error("Error while revoking roles.", ex);
                        throw;
                    }
                }
            }
        }
        public UserContactInfo GetUserContactInfo(int userId)
        {
            using (var db = new DataAccess())
            {
                try
                {
                    db.CreateStoredProcCommand("dbo.GetUserContactInfo");
                    db.AddInputParameter("@UserId", DbType.Int32, userId);
                    var info = new UserContactInfo();
                    info.Address = new Address();
                    db.ReadInto(info);
                    return info;
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while getting user contact info.", ex);
                    throw;
                }
            }            
        }
    }
}
