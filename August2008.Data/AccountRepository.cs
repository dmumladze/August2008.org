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
                db.AddInParameter("@ProviderId", DbType.Int32, userId);
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
    }
}
