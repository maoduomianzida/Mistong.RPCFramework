using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.Thrift
{
    public class ThriftClientConfigConverter : CustomCreationConverter<ClientConfig>
    {
        public override ClientConfig Create(Type objectType)
        {
            return new ThriftClientConfig();
        }
    }
}
