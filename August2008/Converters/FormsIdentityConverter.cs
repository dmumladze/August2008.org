using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using August2008.Models;
using Newtonsoft.Json;

namespace August2008.Converters
{
    public class FormsIdentityConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}