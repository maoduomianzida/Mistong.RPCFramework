using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Thrift;

namespace Mistong.RPCFramework.Thrift
{
    public class ThriftDynamicProxy : IDynamicProxyBuilder
    {
        private string _dynamicAsseblyName;
        private AssemblyBuilder _assemblyBuilder;
        private ModuleBuilder _moduleBuilder;
        private ConcurrentDictionary<Type, Type> _dynamicProxyCache;
        private MethodInfo _processException;

        public ThriftDynamicProxy(string dynamicAsseblyName)
        {
            if (string.IsNullOrWhiteSpace(dynamicAsseblyName))
            {
                throw new ArgumentException(nameof(dynamicAsseblyName) + "参数不能为null或者空字符串");
            }
            _dynamicAsseblyName = dynamicAsseblyName;
            _dynamicProxyCache = new ConcurrentDictionary<Type, Type>();
            _processException = typeof(ThriftDynamicProxy).GetMethod("ProcessException", BindingFlags.Public | BindingFlags.Static);
            BuildDynamicAssembly();
        }

        protected void BuildDynamicAssembly()
        {
            AssemblyName assemblyName = new AssemblyName(_dynamicAsseblyName);
            _assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            _moduleBuilder = _assemblyBuilder.DefineDynamicModule(_dynamicAsseblyName);
        }

        public virtual Type CreateProxy(Type interfaceType)
        {
            if (interfaceType == null) throw new ArgumentNullException(nameof(interfaceType));

            return _dynamicProxyCache.GetOrAdd(interfaceType, CreateProxyCore);
        }

        private IEnumerable<MethodInfo> GetAllMethods(Type interfaceType)
        {
            foreach (MethodInfo method in interfaceType.GetMethods())
            {
                yield return method;
            }
            foreach(Type tmpType in interfaceType.GetInterfaces())
            {
                foreach (MethodInfo method in tmpType.GetMethods())
                {
                    yield return method;
                }
            }
        }

        protected virtual void BuildMethods(TypeBuilder typeBuilder, FieldBuilder fieldBuilder, Type interfaceType)
        {
            foreach (MethodInfo method in GetAllMethods(interfaceType))
            {
                BuildMethod(typeBuilder, method, fieldBuilder);
            }
        }

        protected virtual Type CreateProxyCore(Type interfaceType)
        {
            TypeBuilder typeBuilder = _moduleBuilder.DefineType(_dynamicAsseblyName + "." + interfaceType.Name + "Implement", TypeAttributes.Public, typeof(Object), new Type[] { interfaceType });
            FieldBuilder fieldBuilder = typeBuilder.DefineField("_realImplement", interfaceType, FieldAttributes.Private);
            BuildConstructor(typeBuilder, fieldBuilder);
            BuildMethods(typeBuilder,fieldBuilder,interfaceType);

            return typeBuilder.CreateType();
        }

        protected virtual void BuildConstructor(TypeBuilder type, FieldBuilder field)
        {
            MethodBuilder method = type.DefineMethod(".ctor", MethodAttributes.Public | MethodAttributes.HideBySig);
            method.SetReturnType(typeof(void));
            method.SetParameters(field.FieldType);
            ILGenerator il = method.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, typeof(object).GetConstructor(new Type[] { }));
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, field);
            il.Emit(OpCodes.Ret);
        }


        public static T ProcessException<T>(TApplicationException err)
        {

        }

        protected virtual void BuildMethod(TypeBuilder typeBuilder, MethodInfo methodInfo, FieldBuilder field)
        {
            MethodBuilder method = typeBuilder.DefineMethod(methodInfo.Name,
                MethodAttributes.Public |
                MethodAttributes.Virtual |
                MethodAttributes.NewSlot |
                MethodAttributes.HideBySig |
                MethodAttributes.Final);
            method.SetReturnType(methodInfo.ReturnType);
            ParameterInfo[] paramsArr = methodInfo.GetParameters();
            method.SetParameters((from param in paramsArr select param.ParameterType).ToArray());
            ILGenerator il = method.GetILGenerator();
            bool haveReturnType = methodInfo.ReturnType != typeof(void);
            if (haveReturnType) il.DeclareLocal(methodInfo.ReturnType);
            il.BeginExceptionBlock();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, field);
            for (int i = 0; i < paramsArr.Length; i++)
            {
                il.Emit(OpCodes.Ldarg_S, (i + 1));
            }
            il.Emit(OpCodes.Callvirt, methodInfo);
            if (haveReturnType) il.Emit(OpCodes.Stloc_0);
            il.BeginCatchBlock(typeof(TApplicationException));
            il.Emit(OpCodes.Pop);
            if (haveReturnType)
            {
                il.Emit(OpCodes.Call, _processException.MakeGenericMethod(methodInfo.ReturnType));
                il.Emit(OpCodes.Stloc_0);
            }
            il.EndExceptionBlock();
            if (haveReturnType) il.Emit(OpCodes.Ldloc_0);

            il.Emit(OpCodes.Ret);
        }
    }
}