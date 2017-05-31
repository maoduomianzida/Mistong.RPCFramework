using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thrift.Transport;
using System.Net.Sockets;
using System.Threading;

namespace Mistong.RPCFramework.Thrift
{
    public class ThriftConnectionPool : IThriftConnectionPool
    {
        private ThriftConnectionStore _connectionStore;

        internal ThriftConnectionStore ConnectionStore
        {
            get { return _connectionStore; }
        }

        public ThriftConnectionPool(int connectionLimit, int waitFreeMillisecond, int waitFreeTimes)
        {
            if (connectionLimit <= 0) throw new ArgumentException("连接池的最大连接数小于等于0");
            if (waitFreeTimes < 0) throw new ArgumentException("等待其它线程释放连接的超时时间（毫秒）不能小于0毫秒");
            if (waitFreeTimes <= 0) throw new ArgumentException("尝试等待其它线程释放连接的次数不能小于等于0次");

            _connectionStore = new ThriftConnectionStore(connectionLimit, waitFreeMillisecond, waitFreeTimes);
        }

        public TTransport GetTransport(Service service)
        {
            ThriftService thriftService = service as ThriftService;
            if (thriftService == null)
            {
                throw new NullReferenceException("thriftService");
            }

            return _connectionStore.GetOrAdd(thriftService, tmp =>
            {
                TSocket socket = new TSocket(new TcpClient(tmp.Address, tmp.Port));
                if(!socket.IsOpen)
                {
                    socket.Open();
                }

                return socket;
            });
        }
    }
}