using System;
using AutoMapper;

namespace August2008.Helpers
{
    public class StringIntegerConverter : ITypeConverter<string, int>
    {
        public int Convert(ResolutionContext context)
        {
            if (!context.IsSourceValueNull)
            {
                return context.SourceValue.ToString().ToInteger();
            }
            return 0;
        }
    }
}