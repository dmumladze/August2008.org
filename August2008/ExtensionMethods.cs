using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using August2008.Common.Interfaces;
using August2008.Model;
using August2008.Models;
using DotNetOpenAuth.AspNet;
using Newtonsoft.Json;
using FormsIdentity = August2008.Models.FormsIdentity;

namespace August2008
{
    public static class ExtensionMethods
    {
        public static bool IsNull(this object value)
        {
            return !(value != null);
        }
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> value)
        {
            return !(value != null && value.Count() != 0);
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
        public static RegisterUser ToRegisterUser(this AuthenticationResult source) 
        {
            var user = new RegisterUser();

            if (source != null)
            {
                user.Provider = source.Provider;
                user.ProviderId = string.Concat(source.Provider, "-", source.ProviderUserId);

                if (source.ExtraData != null)
                {
                    switch (source.Provider.ToLowerInvariant())
                    {
                        case "facebook":
                            user.Email = source.ExtraData.GetValueOrDefault(x => x.Key.Equals("username"));
                            user.DisplayName = source.ExtraData.GetValueOrDefault(x => x.Key.Equals("name")).ToTitleCase();
                            user.SocialLink = source.ExtraData.GetValueOrDefault(x => x.Key.Equals("link"));
                            user.Gender = source.ExtraData.GetValueOrDefault(x => x.Key.Equals("gender")).ToTitleCase();
                            user.AccessToken = source.ExtraData.GetValueOrDefault(x => x.Key.Equals("accesstoken"));
                            break;
                        case "google":
                            user.Email = source.ExtraData.GetValueOrDefault(x => x.Key.Equals("email"));
                            break;
                    }
                }
            }
            return user; 
        }
        public static OAuthUser ToOAuthUser(this AuthenticationResult source)
        {
            var user = new OAuthUser();
            if (source != null)
            {
                user.ProviderName = source.Provider;
                user.ProviderId = string.Concat(source.Provider, "-", source.ProviderUserId);
                user.ProviderData = source.ExtraData;
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
        public static HttpCookie ToAuthCookie(this User user)
        {
            if (user != null)
            {
                var ticket = new FormsAuthenticationTicket(
                    2,
                    user.UserId.ToString(),
                    DateTime.Now,
                    DateTime.Now.AddDays(1),
                    false,
                    user.ToFormsPrincipal().ToJson()
                    );
                var encrypted = FormsAuthentication.Encrypt(ticket);
                return new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
            }
            return null;
        }
        public static HttpCookie ToAuthCookie(this RegisterUser user)
        {
            if (user != null)
            {
                var ticket = new FormsAuthenticationTicket(
                    2,
                    user.UserId.ToString(),
                    DateTime.Now,
                    DateTime.Now.AddDays(25),
                    true,
                    user.ToFormsPrincipal().ToJson()
                    );
                var encrypted = FormsAuthentication.Encrypt(ticket);
                return new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
            }
            return null;
        }
        public static FormsPrincipal ToFormsPrincipal(this User source)
        {
            if (source != null)
            {
                var isAuthenticated = source.UserId != 0;
                var target = new FormsPrincipal(new FormsIdentity(source.DisplayName, isAuthenticated))
                    {
                        UserId = source.UserId,
                        SuperUser = source.SuperAdmin,
                        Roles = source.Roles.Select(x => x.Name).ToList(),
                        LanguageId = source.Profile.Lang.LanguageId,
                        Culture = source.Profile.Lang.Culture                        
                    };
                return target;
            }
            return default(FormsPrincipal);
        }
        public static FormsPrincipal ToFormsPrincipal(this RegisterUser source)
        {
            if (source != null)
            {
                var target = new FormsPrincipal(new FormsIdentity(source.DisplayName, true))
                    {                    
                        LanguageId = 1,
                        Culture = "ka-GE"
                    };
                return target;
            }
            return default(FormsPrincipal);
        }
        public static string ToTitleCase(this string value)
        {
            return !string.IsNullOrWhiteSpace(value) ? CultureInfo.InvariantCulture.TextInfo.ToTitleCase(value) : value;
        }
    }
}