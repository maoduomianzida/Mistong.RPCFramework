using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thrift.Transport;

namespace Mistong.RPCFramework.Thrift
{
    internal class DecorateTransport : TTransport
    {
        private TTransport _transport;
        private TransportPoolItem _item;
        private ThriftService _service;
        private ThriftConnectionStore _connectionStore;

        public DecorateTransport(TTransport transport, ThriftConnectionStore store, ThriftService service,TransportPoolItem item)
        {
            Contract.Assert(transport != null && item != null && service != null && store != null);
            _transport = transport;
            _item = item;
            _service = service;
            _connectionStore = store;
        }

        public override bool IsOpen
        {
            get { return _transport.IsOpen; }
        }

        public override void Close()
        {
            _transport.Close();
        }

        public override void Open()
        {
            _transport.Open();
        }

        public override int Read(byte[] buf, int off, int len)
        {
            return _transport.Read(buf,off,len);
        }

        public override void Write(byte[] buf, int off, int len)
        {
            _transport.Write(buf, off, len);
        }

        protected override void Dispose(bool disposing)
        {
            _connectionStore.ReleaseTransport(_service,_item);
        }
    }
}