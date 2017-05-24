using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thrift.Transport;

namespace Mistong.RPCFramework.Thrift
{
    public class ThriftService : Service
    {
        public override string Type { get { return "thrift"; } }

        public Type ServiceType { get; set; }
    }
}