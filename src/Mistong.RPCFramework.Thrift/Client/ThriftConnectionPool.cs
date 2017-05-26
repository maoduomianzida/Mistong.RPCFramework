using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thrift.Transport;
using System.Net.Sockets;

namespace Mistong.RPCFramework.Thrift
{
    public class ThriftConnectionPool : IThriftConnectionPool
    {
        public TTransport GetTransport(Service service)
        {
            return new TSocket(new TcpClient(service.Address,service.Port));
        }
    }
}
