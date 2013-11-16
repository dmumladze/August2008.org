using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using August2008.Models;
using August2008.Properties;
using ImageResizer;

namespace August2008.Helpers
{
    public static class SiteHelper
    {
        public static FileContentResult GetHeroPhoto(string uri, PhotoSize size)
        {
            var wr = WebRequest.Create(uri);
            using (var response = wr.GetResponse())
            {
                var bmp = ResizeImage(response.GetResponseStream(), size);
                return new FileContentResult(bmp.ToByteArray(), MediaTypeNames.Application.Octet);
            }
        }
        public static Bitmap ResizeImage(Stream imageStream, PhotoSize size)
        {
            var maxHeight = 0;
            var maxWidth = 0;

            switch (size)
            {
                case PhotoSize.Medium:
                    maxHeight = 225;
                    maxWidth = 225;
                    break;

                case PhotoSize.Fullsize:
                    break;

                default:
                    maxHeight = 125;
                    maxWidth = 125;
                    break;
            }
            var rs = new ResizeSettings();
            if (maxHeight > 0)
            {
                rs.MaxHeight = maxHeight;
                rs.MaxWidth = maxWidth;
            }            
            return ImageBuilder.Current.Build(imageStream, rs);
        }
        public static byte[] ToByteArray(this Bitmap bmp)
        {
            if (bmp != null)
            {
                using (var ms = new MemoryStream())
                {
                    bmp.Save(ms, ImageFormat.Jpeg);
                    return ms.ToArray();
                }                
            }
            return default(byte[]);
        }
        public static List<string> GetAlphabet()
        {            
            var firstLetter = char.Parse(Resources.Global.Strings.AlphabetFirstLetter);
            var lastLetter = char.Parse(Resources.Global.Strings.AlphabetLastLetter);
            var alphabet = new List<string>(lastLetter - firstLetter + 1);

            for (var c = firstLetter; c <= lastLetter; c++)
            {
                alphabet.Add(c.ToString(CultureInfo.CurrentCulture));
            }
            return alphabet;
        }
        public static decimal ToDecimal(this string value)
        {
            decimal result;
            decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
            return result;
        }
        public static int ToInteger(this string value) 
        {
            int result;
            int.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
            return result;
        }
        public static string ToShortDateString(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.ToShortDateString();
            }
            return string.Empty;
        }
        public static string ToYearString(this DateTime? dateTime) 
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.Year.ToString();
            }
            return "-";
        }
        public static DateTime ToDateTime(this string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                DateTime dateTime;
                DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
                return dateTime;
            }
            return default(DateTime);
        }
        public static void SendEmail(string from, string to, string subject, string body)
        {
            try
            {
                var smtpServer = ConfigurationManager.AppSettings["August2008:SmtpServer"];
                var username = ConfigurationManager.AppSettings["August2008:SmtpUsername"];
                var password = ConfigurationManager.AppSettings["August2008:SmtpPassword"];
                var smtp = new SmtpClient(smtpServer);
                smtp.Credentials = new NetworkCredential(username, password);
                smtp.Send(new MailMessage(from, to, subject, body));
            }
            catch (Exception)
            {
            }
        }
        public static bool IsCurrentRoute(this RequestContext context, string areaName, string controllerName, params string[] actionNames)
        {
            var routeData = context.RouteData;
            var routeArea = routeData.DataTokens["area"] as string;
            var current = false;

            if (((string.IsNullOrEmpty(routeArea) && string.IsNullOrEmpty(areaName)) || (routeArea == areaName))
                  && ((string.IsNullOrEmpty(controllerName)) || (routeData.GetRequiredString("controller") == controllerName))
                  && ((actionNames == null) || actionNames.Contains(routeData.GetRequiredString("action"))))
            {
                current = true;
            }
            return current;
        }
        public static RouteValueDictionary ToRouteValues(this NameValueCollection source)
        {
            var routeValues = new RouteValueDictionary();
            if (source != null)
            {                
                foreach (string name in source.AllKeys)
                {
                    routeValues.Add(name, source[name]);
                }
            }
            return routeValues;
        }
    }
}