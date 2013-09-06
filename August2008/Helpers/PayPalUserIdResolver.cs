using System;
using August2008.Model;
using AutoMapper;
using Newtonsoft.Json;

namespace August2008.Helpers
{
    public class PayPalUserIdResolver : ValueResolver<PayPalTransaction, int>
    {
        protected override int ResolveCore(PayPalTransaction source)
        {
            var item = JsonConvert.DeserializeObject<PayPalCustom>(source.custom);
            return item.UserId;
        }
    }
}