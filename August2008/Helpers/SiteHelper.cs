using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using August2008.Models;
using August2008.Properties;
using August2008.Resources.Shared;
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
        public static List<char> GetAlphabet()
        {            
            var firstLetter = char.Parse(Global.AlphabetFirstLetter);
            var lastLetter = char.Parse(Global.AlphabetLastLetter);
            var alphabet = new List<char>(lastLetter - firstLetter + 1);

            for (var c = firstLetter; c <= lastLetter; c++)
            {
                alphabet.Add(c);
            }
            return alphabet;
        }
        public static decimal ToDecimal(this string value)
        {
            decimal result;
            decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
            return result;
        }
    }
}