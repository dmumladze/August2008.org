using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using August2008.Common.Interfaces;

namespace August2008.Common
{
    public static class Extensions
    {
        public static string ToDbXml(this IEnumerable<IPostedFile> postedFiles)
        {
            if (postedFiles != null)
            {
                var sb = new StringBuilder("<Photos>");
                foreach (var file in postedFiles)
                {
                    sb.Append(file.ToDbXml());
                }
                sb.Append("</Photos>");
                return sb.ToString();
            }
            return string.Empty;
        }
        public static string ToDbXml(this IPostedFile file)
        {
            return string.Format("<Photo PhotoUrl=\"{0}\" ContentType=\"{1}\" {2}/>", 
                Path.GetFileName(file.FileName), 
                file.ContentType, 
                file.Attributes.ToXmlAttributes("FileName"));
        }   
        public static string ToXmlAttributes(this IDictionary<string, string> dictionary, params string[] ignore)
        {
            if (dictionary != null)
            {
                var sb = new StringBuilder();
                foreach (var item in dictionary.Where(item => !item.Key.Equals(ignore)))
                {
                    sb.AppendFormat("{0}=\"{1}\"", item.Key, item.Value);
                }
                return sb.ToString();
            }
            return string.Empty;
        }
        public static string ToDbXml(this IDictionary<string, string> dictionary, params string[] ignore) 
        {
            if (dictionary != null)
            {
                var sb = new StringBuilder("<Dictionary>");
                foreach (var item in dictionary.Where(item => !item.Key.Equals(ignore)))
                {
                    sb.AppendFormat("<Item Key=\"{0}\" Value=\"{1}\" />", item.Key, item.Value);
                }
                sb.Append("</Dictionary>");
                return sb.ToString();
            }
            return string.Empty;
        }
    }
}
