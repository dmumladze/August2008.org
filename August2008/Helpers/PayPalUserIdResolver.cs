using System;
using August2008.Model;
using AutoMapper;
using Newtonsoft.Json;

namespace August2008.Helpers
{
    public class PayPalUserIdResolver : ValueResolver<PayPalVariables, int>
    {
        protected override int ResolveCore(PayPalVariables source)
        {
            var item = JsonConvert.DeserializeObject<PayPalCustom>(source.custom);
            return item.UserId;
        }
    }
}