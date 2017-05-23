using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.Thrift
{
    public class TypeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(Type) == objectType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string serviceTypeStr = reader.Value as string;

            return Type.GetType(serviceTypeStr);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Type type = value as Type;
            if(type != null)
            {
                writer.WriteValue(type.FullName);
            }
        }
    }
}