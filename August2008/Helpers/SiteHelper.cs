using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
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
        public static FileContentResult GetHeroPhoto(string name, PhotoSize size)
        {
            var path = Path.Combine(Settings.Default.HeroPhotoDirectory, name);
            var bmp = ResizeImage(new FileStream(HttpContext.Current.Server.MapPath(path), FileMode.Open), size);
            return new FileContentResult(bmp.ToByteArray(), MediaTypeNames.Application.Octet);
        }
        public static Bitmap ResizeImage(Stream streamImage, PhotoSize size)
        {
            var maxHeight = 125;
            var maxWidth = 125;

            switch (size)
            {
                case PhotoSize.Medium:
                    maxHeight = 225;
                    maxHeight = 225;
                    break;
            }
            var rs = new ResizeSettings
                {
                    Cache = ServerCacheMode.Always,
                    MaxHeight = maxHeight,
                    MaxWidth = maxWidth,
                };
            return ImageBuilder.Current.Build(streamImage, rs);
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
    }
}