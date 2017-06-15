using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.Thrift
{
    public class ThriftServerConfigConverter : CustomCreationConverter<ServerConfig>
    {
        public override ServerConfig Create(Type objectType)
        {
            return new ThriftServerConfig();
        }
    }
}