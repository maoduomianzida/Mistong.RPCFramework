﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Thrift;
using System.Threading;

namespace Mistong.RPCFramework.Thrift
{
    public class ThriftDynamicProxy : IDynamicProxyBuilder
    {
        private string _dynamicAssemblyName;
        private AssemblyBuilder _assemblyBuilder;
        private ModuleBuilder _moduleBuilder;
        private ConcurrentDictionary<Type, Lazy<Type>> _dynamicProxyCache;
        private MethodInfo _handException;
        private ConstructorInfo _exceptionContextCtor;

        public ThriftDynamicProxy(string dynamicAssemblyName)
        {
            if (string.IsNullOrWhiteSpace(dynamicAssemblyName))
            {
                throw new ArgumentException(nameof(dynamicAssemblyName) + "参数不能为null或者空字符串");
            }
            _dynamicAssemblyName = dynamicAssemblyName;
            _dynamicProxyCache = new ConcurrentDictionary<Type, Lazy<Type>>();
            _handException = typeof(IServiceContainerExtension).GetMethod("HandException",new Type[] { typeof(IServiceContainer),typeof(ExceptionContext) });
            _exceptionContextCtor = typeof(ExceptionContext).GetConstructor(new Type[] { typeof(ActionDescriptor), typeof(Exception) });
            BuildDynamicAssembly();
        }

        protected void BuildDynamicAssembly()
        {
            AssemblyName assemblyName = new AssemblyName(_dynamicAssemblyName);
            _assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            _moduleBuilder = _assemblyBuilder.DefineDynamicModule(_dynamicAssemblyName);
        }

        public virtual Type CreateProxy(Type interfaceType)
        {
            if (interfaceType == null) throw new ArgumentNullException(nameof(interfaceType));

            return _dynamicProxyCache.GetOrAdd(interfaceType,
                      type => new Lazy<Type>(() => CreateProxyCore(type), LazyThreadSafetyMode.PublicationOnly)).Value;
        }

        private IEnumerable<MethodInfo> GetAllMethods(Type interfaceType)
        {
            foreach (MethodInfo method in interfaceType.GetMethods())
            {
                yield return method;
            }
            foreach (Type tmpType in interfaceType.GetInterfaces().Except(new Type[] { typeof(IDisposable) }))
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

        protected virtual void ImplementIDisposable(TypeBuilder typeBuilder, FieldBuilder fieldBuidler)
        {
            MethodInfo disposeMethod = typeof(IDisposable).GetMethod("Dispose");
            MethodBuilder methodBuilder = typeBuilder.DefineMethod(disposeMethod.Name, MethodAttributes.Public |
                MethodAttributes.Virtual |
                MethodAttributes.NewSlot |
                MethodAttributes.HideBySig |
                MethodAttributes.Final);
            methodBuilder.SetReturnType(typeof(void));
            methodBuilder.SetParameters(new Type[] { });
            ILGenerator il = methodBuilder.GetILGenerator();
            Type disposeType = typeof(IDisposable);
            il.DeclareLocal(disposeType);
            Label falseLabel = il.DefineLabel();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, fieldBuidler);
            il.Emit(OpCodes.Isinst, disposeType);
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Cgt_Un);
            il.Emit(OpCodes.Brfalse_S, falseLabel);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Callvirt, disposeType.GetMethod("Dispose"));
            il.MarkLabel(falseLabel);
            il.Emit(OpCodes.Ret);
        }

        protected virtual Type CreateProxyCore(Type interfaceType)
        {
            bool containsDisposable = !typeof(IDisposable).IsAssignableFrom(interfaceType);
            TypeBuilder typeBuilder = _moduleBuilder.DefineType(_dynamicAssemblyName + "." + interfaceType.Name + "Implement", TypeAttributes.Public, typeof(Object), new Type[] { interfaceType, typeof(IDisposable) });
            FieldBuilder fieldBuilder = typeBuilder.DefineField("_realImplement", interfaceType, FieldAttributes.Private);
            BuildConstructor(typeBuilder, fieldBuilder);
            BuildMethods(typeBuilder, fieldBuilder, interfaceType);
            ImplementIDisposable(typeBuilder, fieldBuilder);

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

        private void BuildActionDescriptor(ILGenerator il,MethodInfo methodInfo)
        {
            ParameterInfo[] paramArr = methodInfo.GetParameters();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Callvirt, typeof(object).GetMethod("GetType"));
            il.Emit(OpCodes.Ldstr, methodInfo.Name);
            il.Emit(OpCodes.Ldc_I4, methodInfo.GetParameters().Length);
            il.Emit(OpCodes.Newarr,typeof(Type));
            for(int i = 0; i < paramArr.Length; i++)
            {
                il.Emit(OpCodes.Dup);
                il.Emit(OpCodes.Ldc_I4,i);
                il.Emit(OpCodes.Ldtoken,paramArr[i].ParameterType);
                il.Emit(OpCodes.Call,typeof(Type).GetMethod("GetTypeFromHandle",new Type[] { typeof(RuntimeTypeHandle) }));
                il.Emit(OpCodes.Stelem_Ref);
            }
            il.Emit(OpCodes.Callvirt, typeof(Type).GetMethod("GetMethod", new Type[] { typeof(string),typeof(Type[]) }));
            il.Emit(OpCodes.Call,typeof(ActionDescriptorCreator).GetMethod("GetActionDescriptor",new Type[] { typeof(MethodInfo)}));
            il.Emit(OpCodes.Stloc_2);
        }

        private void BuildActionParams(ILGenerator il, MethodInfo methodInfo)
        {
            ParameterInfo[] paramArr = methodInfo.GetParameters();
            Type keyType = typeof(string);
            Type valueType = typeof(object);
            Type dicType = typeof(Dictionary<,>).MakeGenericType(keyType, valueType);
            il.Emit(OpCodes.Newobj,dicType.GetConstructor(new Type[] { }));
            for(int i = 0; i < paramArr.Length ;i++)
            {
                ParameterInfo parameter = paramArr[i];
                il.Emit(OpCodes.Dup);
                il.Emit(OpCodes.Ldstr,parameter.Name);
                il.Emit(OpCodes.Ldarg_S, i + 1);
                if (parameter.ParameterType.IsValueType)
                {
                    il.Emit(OpCodes.Box,parameter.ParameterType);
                }
                il.Emit(OpCodes.Callvirt,dicType.GetMethod("Add",new Type[] { keyType, valueType }));
            }
        }

        private void BuildActionContext(ILGenerator il,MethodInfo methodInfo)
        {
            il.Emit(OpCodes.Ldloc_2);
            il.Emit(OpCodes.Ldarg_0);
            BuildActionParams(il,methodInfo);
            il.Emit(OpCodes.Newobj,typeof(ActionContext).GetConstructor(
                new Type[] {
                    typeof(ActionDescriptor),
                    typeof(object),
                    typeof(IDictionary<,>).MakeGenericType(typeof(string),typeof(object))
                }));
        }

        private void BuildActionResult(ILGenerator il,MethodInfo methodInfo)
        {
            il.Emit(OpCodes.Ldloc_2);
            if (methodInfo.ReturnType != typeof(void))
            {
                il.Emit(OpCodes.Ldloc_3);
                if (methodInfo.ReturnType.IsValueType)
                {
                    il.Emit(OpCodes.Box,methodInfo.ReturnType);
                }
            }
            else
            {
                il.Emit(OpCodes.Ldnull);
            }
            il.Emit(OpCodes.Newobj,typeof(ActionResult).GetConstructor(new Type[] { typeof(ActionDescriptor),typeof(object)}));
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
            il.DeclareLocal(typeof(Exception));
            il.DeclareLocal(typeof(IServiceContainer));
            il.DeclareLocal(typeof(ActionDescriptor));
            if (haveReturnType) il.DeclareLocal(methodInfo.ReturnType);
            BuildActionDescriptor(il, methodInfo);
            il.Emit(OpCodes.Call, typeof(GlobalSetting).GetProperty("Container", BindingFlags.Public | BindingFlags.Static).GetMethod);
            il.Emit(OpCodes.Stloc_1);
            il.Emit(OpCodes.Ldloc_1);
            BuildActionContext(il, methodInfo);
            il.Emit(OpCodes.Call, typeof(IServiceContainerExtension).GetMethod("ActionExecuteBefore",new Type[] { typeof(IServiceContainer),typeof(ActionContext) }));
            il.BeginExceptionBlock();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, field);
            for (int i = 0; i < paramsArr.Length; i++)
            {
                il.Emit(OpCodes.Ldarg_S, (i + 1));
            }
            il.Emit(OpCodes.Callvirt, methodInfo);
            if (haveReturnType) il.Emit(OpCodes.Stloc_3);
            il.BeginCatchBlock(typeof(Exception));
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Ldloc_1);
            il.Emit(OpCodes.Ldloc_2);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Newobj, _exceptionContextCtor);
            il.Emit(OpCodes.Call, _handException.MakeGenericMethod(haveReturnType ? methodInfo.ReturnType : typeof(object)));
            if (haveReturnType)
            {
                il.Emit(OpCodes.Stloc_3);
            }
            il.EndExceptionBlock();
            il.Emit(OpCodes.Ldloc_1);
            BuildActionResult(il,methodInfo);
            il.Emit(OpCodes.Call, typeof(IServiceContainerExtension).GetMethod("ActionExecuteAfter",new Type[] { typeof(IServiceContainer),typeof(ActionResult) }));
            if (haveReturnType) il.Emit(OpCodes.Ldloc_3);

            il.Emit(OpCodes.Ret);
        }
    }
}