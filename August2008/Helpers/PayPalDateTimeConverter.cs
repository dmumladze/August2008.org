using System;
using AutoMapper;

namespace August2008.Helpers
{
    public class PayPalDateTimeConverter : ITypeConverter<string, DateTime>
    {
        public DateTime Convert(ResolutionContext context)
        {
            if (!context.IsSourceValueNull)
            {
                return context.SourceValue.ToString().ToDateTime();
            }
            return default(DateTime);
        }
    }
}