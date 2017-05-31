using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thrift.Transport;

namespace Mistong.RPCFramework.Thrift
{
    internal class TransportPoolItem
    {
        public TTransport Transport { get; set; }

        public bool IsFree { get; set; }
    }
}