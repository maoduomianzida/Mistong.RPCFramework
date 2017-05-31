using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thrift.Transport;

namespace Mistong.RPCFramework.Thrift
{
    internal class ThriftConnectionStore
    {
        private IDictionary<ThriftService, TransportPoolItemCollection> _connectionPool;
        private readonly int _connectionLimit;

        public ThriftConnectionStore(int connectionLimit)
        {
            Contract.Assert(connectionLimit > 0);
            _connectionPool = new Dictionary<ThriftService, TransportPoolItemCollection>(new ThriftServiceEqualityComparer());
            _connectionLimit = connectionLimit;
        }

        public TTransport GetOrAdd(ThriftService service,Func<ThriftService, TTransport> createAction)
        {
            Contract.Assert(service != null && createAction != null);
            TransportPoolItem item = null;
            lock (this)
            {
                if(_connectionPool.ContainsKey(service))
                {
                    item  = _connectionPool[service].GetUsableTransport(() => createAction(service));
                }
                else
                {
                    TransportPoolItemCollection collection = new TransportPoolItemCollection(_connectionLimit);
                    TTransport transport = createAction(service);
                    if(transport != null)
                    {
                        item = new TransportPoolItem { Transport = transport, IsFree = false };
                        collection.Add(item);
                    }
                    _connectionPool.Add(service,collection);
                }
            }
            if(item != null)
            {
                return new DecorateTransport(item.Transport,this,service,item);
            }

            return null;
        }

        public void ReleaseTransport(ThriftService service, TransportPoolItem item)
        {
            if (_connectionPool.ContainsKey(service))
            {
                _connectionPool[service].SetFree(item);
            }
        }
    }
}