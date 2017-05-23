using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.Thrift
{
    public class ConfigCenterCreationConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(ConfigCenter).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            JObject obj = serializer.Deserialize<JObject>(reader);
            string type = obj["type"].ToObject<string>();
            if (type == ConsulConfigCenter.ConfigType)
            {
                return obj.ToObject(typeof(ConsulConfigCenter));
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException("无需使用ConfigCenterCreationConverter转换json");
        }
    }
}