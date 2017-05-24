using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework
{
    internal static class TypeHelper
    {
        /// <summary>
        /// 从程序集中获取指定类型的实现类
        /// </summary>
        /// <param name="type">寻找的类型</param>
        /// <param name="findPredicate">类型的过滤条件</param>
        /// <param name="predicate">程序集过滤条件</param>
        /// <param name="assemblies">寻找的程序集</param>
        /// <returns></returns>
        public static Type[] GetExtendTypes(Type type,Func<Type,bool> findPredicate = null,Func<Assembly,bool> predicate = null, params Assembly[] assemblies)
        {
            if(assemblies == null || assemblies.Length == 0)
            {
                assemblies = AppDomain.CurrentDomain.GetAssemblies();
                if(predicate != null)
                {
                    assemblies = assemblies.Where(predicate).ToArray();
                }
            }
            IEnumerable<Type> types = Enumerable.Empty<Type>();
            foreach(Assembly assembly in assemblies)
            {
                IEnumerable<Type> tmps = assembly.GetTypes().Where(t => type.IsAssignableFrom(t));
                if(findPredicate != null)
                {
                    tmps = tmps.Where(findPredicate);
                }
                types = types.Union(tmps);
            }

            return types.ToArray();
        }
    }
}