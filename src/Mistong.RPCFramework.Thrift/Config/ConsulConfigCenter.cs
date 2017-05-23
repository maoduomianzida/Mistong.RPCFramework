using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.Thrift
{
    public class ConsulConfigCenter : ConfigCenter
    {
        public static readonly string ConfigType = "consul";

        public override string Type { get { return ConfigType; } }
    }
}