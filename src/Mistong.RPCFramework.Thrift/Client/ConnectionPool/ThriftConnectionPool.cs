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
        private ThriftConnectionStore _connectionStore;

        public ThriftConnectionPool(int connectionLimit)
        {
            if (connectionLimit == 0) throw new ArgumentException("连接池的最大连接数不能为0");

            _connectionStore = new ThriftConnectionStore(connectionLimit);
        }

        public TTransport GetTransport(Service service)
        {
            ThriftService thriftService = service as ThriftService;
            if(thriftService == null)
            {
                throw new NullReferenceException("thriftService");
            }

            return _connectionStore.GetOrAdd(thriftService, tmp => new TSocket(tmp.Address, tmp.Port,5 * 1000));
        }
    }
}