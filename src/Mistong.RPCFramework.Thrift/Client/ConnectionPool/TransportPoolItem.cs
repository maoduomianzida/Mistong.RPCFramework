using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thrift.Transport;

namespace Mistong.RPCFramework.Thrift
{
    public class TransportPoolItem
    {
        public TTransport Transport { get; private set; }

        public bool IsFree { get; set; }
    }
}