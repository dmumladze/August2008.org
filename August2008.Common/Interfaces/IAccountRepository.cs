using System;
using System.Collections.Generic;
using August2008.Model;

namespace August2008.Common.Interfaces
{
    public interface IAccountRepository
    {
        User CreateUser(User user);
        User GetUser(int userId);
        void UpdateUser(User user);
        void UpdateUserProfile(UserProfile profile);
        void UpdateUserProfileAddress(int userId, Address address);
        OAuthUser CreateOAuthUser(OAuthUser user);
        bool TryGetUserRegistered(string email, string provider, out int? userId, out bool isOAuthUser);
        IEnumerable<User> GetUsers();
        UserContactInfo GetUserContactInfo(int userId);
        IEnumerable<User> SearchUsers(string name = null);
        IEnumerable<Role> GetUserRoles(int userId);
        void AssignUserToRoles(int userId, IEnumerable<int> roles);
        void RevokeUserFromRoles(int userId, IEnumerable<int> roles); 
    }
}
