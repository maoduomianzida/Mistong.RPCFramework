using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Thrift;

namespace Mistong.RPCFramework.Thrift
{
    internal static class ThriftServiceHelper
    {
        private static string[] _thriftClassSuffixs = new string[] { "+Iface", "+ISync", "+IAsync" };

        /// <summary>
        /// 创建thrift Processor实现类
        /// </summary>
        /// <param name="processorType">thrift Processor类型</param>
        /// <param name="thriftService">thrift服务实例</param>
        /// <returns></returns>
        public static TProcessor CreateProcessor(Type processorType,object thriftService)
        {
            return Activator.CreateInstance(processorType, thriftService) as TProcessor;
        }

        /// <summary>
        /// 获取thrift服务的Processor 类型
        /// </summary>
        /// <param name="implementType">thrift接口实现类</param>
        /// <returns></returns>
        public static Type GetProcessorType(Type implementType)
        {
            Type interfaceType = ExtractThriftInterface(implementType);
            if(interfaceType != null)
            {
                Type serviceType = interfaceType.DeclaringType;
                string processorName = serviceType.FullName + "+Processor";
                string assemblyName = serviceType.Assembly.FullName;

                return Type.GetType(processorName + "," + assemblyName);
            }

            return null;
        }

        /// <summary>
        /// 获取thrift服务的接口
        /// </summary>
        /// <param name="thriftType">thrift接口实现类</param>
        /// <returns></returns>
        public static Type ExtractThriftInterface(Type thriftType)
        {
            if (IsThriftInterface(thriftType))
            {
                return thriftType;
            }
            foreach (Type type in thriftType.GetInterfaces())
            {
                Type result = ExtractThriftInterface(type);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// 判断类型是否是thrift接口
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsThriftInterface(Type type)
        {
            string typeName = type.Name;
            string suffixs = _thriftClassSuffixs.FirstOrDefault(tmp => type.FullName.EndsWith(tmp));

            return suffixs != null;
        }
    }
}
