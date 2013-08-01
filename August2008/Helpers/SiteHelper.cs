﻿using System;
using System.Collections.Generic;
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
            var maxHeight = 125;
            var maxWidth = 125;

            switch (size)
            {
                case PhotoSize.Medium:
                    maxHeight = 225;
                    maxWidth = 225;
                    break;
            }
            var rs = new ResizeSettings
                {
                    Cache = ServerCacheMode.Always,
                    MaxHeight = maxHeight,
                    MaxWidth = maxWidth,
                };

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
        public static string ToShortDateString(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.ToShortDateString();
            }
            return string.Empty;
        }
        public static void SendEmail(string from, string to, string subject, string body)
        {
            var smtpServer = ConfigurationManager.AppSettings["August2008:SmtpServer"]; 
            var username = ConfigurationManager.AppSettings["August2008:SmtpUsername"]; 
            var password = ConfigurationManager.AppSettings["August2008:SmtpPassword"]; 
            var smtp = new SmtpClient(smtpServer);
            smtp.Credentials = new NetworkCredential(username, password);
            smtp.Send(new MailMessage(from, to, subject, body));
        }
    }
}