using System;
using System.Collections.Generic;
using August2008.Model;

namespace August2008.Common.Interfaces
{
    public interface IAccountRepository
    {
        User CreateUser(User user);
        User GetUser(int userId);
        bool TryGetUserIdByProviderId(string providerId, out int? userId);
    }
}
