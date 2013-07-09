using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using August2008.Common.Interfaces;
using August2008.Model;
using August2008.Models;
using DotNetOpenAuth.AspNet;
using Newtonsoft.Json;

namespace August2008
{
    public static class ExtensionMethods
    {
        public static bool NotNull(this object value)
        {
            return value != null;
        }
        public static bool NotNullOrEmpty<T>(this IEnumerable<T> value)
        {
            return (value != null && value.Count() != 0);
        }
        public static string ToJson(this object value)
        {
            return (value != null ? JsonConvert.SerializeObject(value) : string.Empty);
        }
        public static T FromJson<T>(this string value) 
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            return default(T);
        }
        public static void SaveAll(this IEnumerable<IPostedFile> photos)
        {
            if (photos == null) return;
            try
            {
                foreach (var photo in photos)
                {
                    photo.Save();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static ExternalUser ToExternalUser(this AuthenticationResult source) 
        {
            var user = new ExternalUser();

            if (source != null)
            {
                user.Provider = source.Provider;
                user.UniqueUserId = source.Provider + "-" + source.ProviderUserId;

                if (source.ExtraData != null)
                {
                    switch (source.Provider.ToLowerInvariant())
                    {
                        case "facebook":
                            user.UserId = source.ExtraData.GetValueOrDefault(x => x.Key.Equals("id"));
                            user.Username = source.ExtraData.GetValueOrDefault(x => x.Key.Equals("username"));
                            user.DisplayName = source.ExtraData.GetValueOrDefault(x => x.Key.Equals("name"));
                            user.Link = source.ExtraData.GetValueOrDefault(x => x.Key.Equals("link"));
                            user.Gender = source.ExtraData.GetValueOrDefault(x => x.Key.Equals("gender"));
                            user.AccessToken = source.ExtraData.GetValueOrDefault(x => x.Key.Equals("accesstoken"));
                            break;
                        case "google":
                            user.Username =
                                user.Username = source.ExtraData.GetValueOrDefault(x => x.Key.Equals("email"));
                            break;
                    }
                }
            }
            return user; 
        }    
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> source, Func<KeyValuePair<TKey, TValue>, bool> predicate)
        {
            if (source != null)
            {
                foreach (var kvp in source)
                {
                    if (predicate(kvp))
                    {
                        return kvp.Value;
                    }
                }
            }
            return default(TValue);
        }
        public static FormsPrincipal ToFormsPrincipal(this User source)
        {
            if (source != null)
            {
                var isAuthenticated = source.UserId != 0;
                var target = new FormsPrincipal(new FormsIdentity(source.DisplayName, isAuthenticated))
                    {
                        UserId = source.UserId,
                        SuperUser = source.SuperUser,
                        Roles = source.Roles,
                        LanguageId = source.Profile.Lang.LanguageId,
                        Culture = source.Profile.Lang.Culture                        
                    };
                return target;
            }
            return default(FormsPrincipal);
        }
    }
}