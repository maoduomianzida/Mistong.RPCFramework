using Mistong.RPCFramework.Thrift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thrift.Transport;

namespace Mistong.RPCFramework.UnitTest
{
    public class SingletonLockConnectionPool : IThriftConnectionPool
    {
        private TSocket _socket;

        public SingletonLockConnectionPool()
        {
            _socket = new TSocket(new System.Net.Sockets.TcpClient("172.16.211.146", 1000));
            if (!_socket.IsOpen)
            {
                _socket.Open();
            }
        }

        public TTransport GetTransport(Service service)
        {
            return _socket;
        }
    }
}