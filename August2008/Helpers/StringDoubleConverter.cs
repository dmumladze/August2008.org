using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

namespace August2008.Helpers
{
    public class StringDoubleConverter : ITypeConverter<string, double>
    {
        public double Convert(ResolutionContext context)
        {
            if (!context.IsSourceValueNull)
            {
                return context.SourceValue.ToString().ToDouble();
            }
            return 0D;
        }
    }
}