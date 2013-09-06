using System;
using August2008.Models;
using AutoMapper;
using Newtonsoft.Json;

namespace August2008.Helpers
{
    public class PayPalUserIdResolver : ValueResolver<PayPalModel, int>
    {
        protected override int ResolveCore(PayPalModel source)
        {
            var item = JsonConvert.DeserializeObject<PayPalItemNumber>(source.item_number);
            int userId;
            int.TryParse(item.UserId);
            return userId;
        }
    }
}