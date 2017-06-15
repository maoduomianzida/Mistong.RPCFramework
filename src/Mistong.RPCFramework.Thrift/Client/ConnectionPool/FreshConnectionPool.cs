using Mistong.RPCFramework.Thrift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Thrift.Transport;

namespace Mistong.RPCFramework
{
    public class FreshConnectionPool : IThriftConnectionPool
    {
        public TTransport GetTransport(Service service)
        {
            return new TSocket(new TcpClient(service.Address, service.Port));
        }
    }
}