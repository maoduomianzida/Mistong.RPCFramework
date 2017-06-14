using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.Thrift
{
    public class FilterCollection : List<FilterInfo>
    {
        public IEnumerable<IActionFilter> ActionFilters { get; private set; }
        public IEnumerable<IExceptionFilter> ExceptionFilters { get; private set; }
        private FilterComparer _comparer;
        private int _buildStatus;

        public FilterCollection()
        {
            _comparer = new FilterComparer();
            ActionFilters = Enumerable.Empty<IActionFilter>();
            ExceptionFilters = Enumerable.Empty<IExceptionFilter>();
        }

        public void Build()
        {
            if(Interlocked.CompareExchange(ref _buildStatus,1,0) == 0)
            {
                Sort(_comparer);
                var collection = from obj in this select obj.Instance;
                List<IActionFilter> actionFilters = new List<IActionFilter>();
                List<IExceptionFilter> exceptionFilters = new List<IExceptionFilter>();
                foreach (object instance in collection)
                {
                    if(instance is IActionFilter)
                    {
                        actionFilters.Add(instance as IActionFilter);
                    }
                    else if(instance is IExceptionFilter)
                    {
                        exceptionFilters.Add(instance as IExceptionFilter);
                    }
                }
                ActionFilters = actionFilters;
                ExceptionFilters = exceptionFilters;
            }
        }

        protected virtual ICollection<FilterInfo> SortFilter(ICollection<FilterInfo> filters)
        {
            List<FilterInfo> newFilters = filters as List<FilterInfo> ?? new List<FilterInfo>(filters);
            newFilters.Sort(_comparer);

            return newFilters;
        }
    }
}