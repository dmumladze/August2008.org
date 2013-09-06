using System;
using System.Globalization;
using AutoMapper;

namespace August2008.Helpers
{
    public class PayPalDateTimeConverter : ITypeConverter<string, DateTime>
    {
        static string[] formats = new string[] { "HH:mm:ss MMM dd, yyyy PDT", "HH:mm:ss MMM dd, yyyy PST" };

        public DateTime Convert(ResolutionContext context)
        {
            if (!context.IsSourceValueNull)
            {
                var input = context.SourceValue.ToString();
                DateTime output;                
                if (!DateTime.TryParseExact(input, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out output))
                {
                    DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out output);
                }
                return output;
            }
            return default(DateTime);
        }
    }
}