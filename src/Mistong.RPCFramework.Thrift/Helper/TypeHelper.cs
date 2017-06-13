using System;
using System.Collections.Generic;

namespace Mistong.RPCFramework.Thrift
{
    public static class TypeHelper
    {
        public static bool IsImplementType(Type type, Type interfaceType, out Type[] genericTypeArr)
        {
            Type realType = type;
            if (type.IsGenericType)
            {
                realType = type.GetGenericTypeDefinition();
                genericTypeArr = type.GetGenericArguments();
            }
            else
            {
                genericTypeArr = new Type[] { };
            }
            if (interfaceType.IsAssignableFrom(realType)) return true;
            foreach (Type tmp in type.GetInterfaces())
            {
                if (IsImplementType(tmp, interfaceType)) return true;
            }

            return false;
        }

        public static bool IsImplementType(Type type, Type interfaceType)
        {
            Type realType = type;
            if (type.IsGenericType)
            {
                realType = type.GetGenericTypeDefinition();
            }
            if (interfaceType.IsAssignableFrom(realType)) return true;
            foreach (Type tmp in type.GetInterfaces())
            {
                if (IsImplementType(tmp, interfaceType)) return true;
            }

            return false;
        }

        public static T GetDefalutValue<T>()
        {
            Type type = typeof(T);
            if (!type.IsValueType && type != typeof(string) && IsImplementType(type, typeof(IEnumerable<>)))
            {
                if (type.IsAbstract || type.IsInterface)
                {
                    Type[] paramArr;
                    if (IsImplementType(type, typeof(IDictionary<,>), out paramArr))
                    {
                        return (T)Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(paramArr[0], paramArr[1]), 0);
                    }
                    if (IsImplementType(type, typeof(IEnumerable<>), out paramArr))
                    {
                        return (T)Activator.CreateInstance(typeof(List<>).MakeGenericType(paramArr[0]), 0);
                    }
                }
                else if (type.IsArray)
                {
                    return (T)Activator.CreateInstance(type, 0);
                }
                else if (type.GetConstructor(new Type[] { }) != null)
                {
                    return (T)Activator.CreateInstance(type);
                }
            }

            return default(T);
        }
    }
}