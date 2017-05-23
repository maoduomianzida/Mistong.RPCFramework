using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Mistong.RPCFramework.Thrift
{
    public class ServiceCreationConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(Service).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            JObject obj = serializer.Deserialize<JObject>(reader);
            string type = obj["type"].ToObject<string>();
            if (type == "thrift")
            {
                return obj.ToObject(typeof(ThriftService));
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException("无需使用ServiceCreationConverter转换json");
        }
    }
}