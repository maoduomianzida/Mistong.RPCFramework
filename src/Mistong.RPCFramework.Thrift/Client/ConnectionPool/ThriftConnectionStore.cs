using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Thrift.Transport;

namespace Mistong.RPCFramework.Thrift
{
    internal class ThriftConnectionStore
    {
        private IDictionary<ThriftService, TransportPoolItemCollection> _connectionPool;
        private readonly int _connectionLimit;
        private int _waitFreeMillisecond;
        private TimeSpan _transportOverdueInterval;

        internal IDictionary<ThriftService, TransportPoolItemCollection> ConnectionPool
        {
            get
            {
                return _connectionPool;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionLimit">同一个地址端口下的最大thrift连接数</param>
        /// <param name="waitFreeMillisecond">达到最大连接数后，等待其它连接释放的超时时间（毫秒）</param>
        /// <param name="waitFreeTimes">达到最大连接数后，尝试等待其它线程释放连接的次数</param>
        public ThriftConnectionStore(int connectionLimit, int waitFreeMillisecond,TimeSpan transportOverdueInterval)
        {
            Contract.Assert(connectionLimit > 0);
            _connectionPool = new Dictionary<ThriftService, TransportPoolItemCollection>(new ThriftServiceEqualityComparer());
            _connectionLimit = connectionLimit;
            _waitFreeMillisecond = waitFreeMillisecond;
            _transportOverdueInterval = transportOverdueInterval;
        }

        public TTransport GetOrAdd(ThriftService service, Func<ThriftService, TTransport> createAction)
        {
            Contract.Assert(service != null && createAction != null);
            TransportPoolItem item = null;
            lock (this)
            {
                if (_connectionPool.ContainsKey(service))
                {
                    while (true)
                    {
                        item = _connectionPool[service].GetUsableTransport(() => createAction(service));
                        if (item == null)
                        {
                            Monitor.Wait(this, _waitFreeMillisecond);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else
                {
                    TransportPoolItemCollection collection = new TransportPoolItemCollection(_connectionLimit, _transportOverdueInterval);
                    TTransport transport = createAction(service);
                    if (transport != null)
                    {
                        item = new TransportPoolItem { Transport = transport, IsFree = false, LastUseTime = DateTime.Now };
                        collection.Add(item);
                    }
                    _connectionPool.Add(service, collection);
                }
            }
            if (item != null)
            {
                return new DecorateTransport(item.Transport, this, service, item);
            }

            return null;
        }

        public void ReleaseTransport(ThriftService service, TransportPoolItem item)
        {
            lock (this)
            {
                if (_connectionPool.ContainsKey(service))
                {
                    _connectionPool[service].SetFree(item);
                    Monitor.Pulse(this);
                }
            }
        }
    }
}