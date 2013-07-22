using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

namespace August2008.Helpers
{
    public class StringDecimalConverter : ITypeConverter<string, decimal>
    {
        public decimal Convert(ResolutionContext context)
        {
            if (!context.IsSourceValueNull)
            {
                return context.SourceValue.ToString().ToDecimal();
            }
            return 0M;
        }
    }
}