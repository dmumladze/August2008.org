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
using August2008.Models;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;
using System.Security.Principal;

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
        public static string ToJson(this object obj)
        {
            return (obj != null ? JsonConvert.SerializeObject(obj) : string.Empty);
        }
        public static string ToXml(this object obj)
        {
            if (obj != null)
            {
                using (var writer = new StringWriter(CultureInfo.InvariantCulture))
                {
                    var serializer = new XmlSerializer(obj.GetType());
                    serializer.Serialize(writer, obj);
                    return writer.ToString();
                }
            }
            return string.Empty;
        }
        public static Dictionary<string, string> ToDictionary(this object obj)
        {
            if (obj != null)
            {                
                var type = obj.GetType();
                var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var toReturn = new Dictionary<string, string>(props.Count());

                foreach (var item in props)
                {
                    var key = item.Name;
                    var value = item.GetValue(obj);
                    toReturn.Add(key, !value.IsNull() ? value.ToString() : string.Empty);
                }
                return toReturn;
            }
            return default(Dictionary<string, string>);
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
        public static RegisterUser ToRegisterUser(this User source) 
        {
            var user = new RegisterUser();
            if (source != null)
            {
                user.UserId = source.UserId;
                user.Email = source.Email;
                user.DisplayName = source.DisplayName;
            }
            return user; 
        }
        public static User ToUser(this RegisterUser source)
        {
            var user = new User();
            if (source != null)
            {
                user.UserId = source.UserId;
                user.Email = source.Email;
                user.DisplayName = source.DisplayName;
            }
            return user;
        }
        public static User ToUser(this AuthenticationResult source)
        {
            var user = new User();
            if (source != null)
            {
                user.OAuth.ProviderName = source.Provider.ToTitleCase();
                user.OAuth.ProviderId = source.ProviderUserId;

                if (source.ExtraData != null)
                {
                    switch (source.Provider.ToLowerInvariant())
                    {
                        case "facebook":
                            user.Email = source.ExtraData.GetValueOrDefault(x => x.Key.Equals("username"));
                            user.DisplayName = source.ExtraData.GetValueOrDefault(x => x.Key.Equals("name")).ToTitleCase();
                            break;
                        case "google":
                            user.Email = source.ExtraData.GetValueOrDefault(x => x.Key.Equals("email"));                            
                            break;
                        case "yahoo":
                            user.Email = source.ExtraData.GetValueOrDefault(x => x.Key.Equals("email"));
                            user.DisplayName = source.ExtraData.GetValueOrDefault(x => x.Key.Equals("fullName")).ToTitleCase();
                            break;
                    }
                    user.OAuth.ProviderData = source.ExtraData;
                    user.OAuth.Email = user.Email;
                    if (string.IsNullOrWhiteSpace(user.DisplayName) && !string.IsNullOrWhiteSpace(user.Email))
                    {
                        user.DisplayName = user.Email.Substring(0, user.Email.IndexOf('@'));
                    }
                    user.Profile.Lang = new Language { LanguageId = 1 };
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
        public static HttpCookie ToAuthCookie(this User user)
        {
            if (user != null)
            {
                var ticket = new FormsAuthenticationTicket(
                    2,
                    user.UserId.ToString(),
                    DateTime.Now,
                    DateTime.Now.AddDays(25),
                    true,
                    user.ToFormsPrincipal().ToJson());
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
                    user.ToFormsPrincipal().ToJson());
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
                var target = new FormsPrincipal(new FormsIdentity2(source.DisplayName, isAuthenticated))
                    {
                        UserId = source.UserId,
                        Email = source.Email,
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
                var target = new FormsPrincipal(new FormsIdentity2(source.DisplayName, true))
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
        public static int GetUserId(this IPrincipal principal)
        {
            var formsPrincipal = HttpContext.Current.User as FormsPrincipal;
            if (formsPrincipal != null)
            {
                return formsPrincipal.UserId;
            }
            return 0;
        }
        public static bool ToBoolean(this object obj)
        {
            bool value;
            bool.TryParse(obj as string, out value);
            return value;
        }
    }
}