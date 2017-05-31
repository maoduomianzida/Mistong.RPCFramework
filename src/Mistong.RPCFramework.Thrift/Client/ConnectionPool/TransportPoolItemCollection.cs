using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Thrift.Transport;

namespace Mistong.RPCFramework.Thrift
{
    internal class TransportPoolItemCollection
    {
        private ICollection<TransportPoolItem> _collection;
        private int _maxLength;
        private AutoResetEvent _waitHandler;

        public TransportPoolItemCollection(int maxLength)
        {
            Contract.Assert(maxLength > 0);
            _collection = new Collection<TransportPoolItem>();
            _maxLength = maxLength;
            _waitHandler = new AutoResetEvent(false);
        }

        public int Count
        {
            get { return _collection.Count; }
        }

        public bool Add(TransportPoolItem item)
        {
            Contract.Assert(item != null);
            if (!_collection.Contains(item) && CanAdd())
            {
                _collection.Add(item);

                return true;
            }

            return false;
        }

        public void Clear()
        {
            _collection.Clear();
        }

        public bool CanAdd()
        {
            return _maxLength > _collection.Count;
        }

        public bool Contains(TransportPoolItem item)
        {
            return _collection.Contains(item);
        }

        public TransportPoolItem GetUsableTransport(Func<TTransport> createAction)
        {
            Contract.Assert(createAction != null);
            TransportPoolItem item = _collection.SingleOrDefault(tmp => tmp.IsFree);
            if (item == null)
            {
                if(CanAdd())
                {
                    TTransport transport = createAction();
                    if (transport != null)
                    {
                        TransportPoolItem newItem = new TransportPoolItem { Transport = transport };
                        if (Add(newItem))
                        {
                            item = newItem;
                        }
                    }
                }
                else
                {
                    _waitHandler.WaitOne();
                    item = _collection.SingleOrDefault(tmp => tmp.IsFree);
                }
            }
            if (item != null)
            {
                item.IsFree = false;
            }

            return item;
        }

        public void SetFree(TransportPoolItem item)
        {
            Contract.Assert(item != null);
            item.IsFree = true;
            _waitHandler.Set();
        }
    }
}