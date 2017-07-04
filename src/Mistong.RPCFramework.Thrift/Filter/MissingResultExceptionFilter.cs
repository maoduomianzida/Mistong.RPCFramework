using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Thrift;
using System.Reflection;
using static Thrift.TApplicationException;
using System.Linq.Expressions;

namespace Mistong.RPCFramework.Thrift
{
    public class MissingResultExceptionFilter : IExceptionFilter
    {
        private Func<TApplicationException, ExceptionType> _exceptionTypeGetter;
        public Func<TApplicationException, ExceptionType> ExceptionTypeGetter
        {
            get
            {
                if (_exceptionTypeGetter == null)
                {
                    lock (this)
                    {
                        if (_exceptionTypeGetter == null)
                        {
                            _exceptionTypeGetter = CreateGetExceptionTypeFunc();
                        }
                    }
                }

                return _exceptionTypeGetter;
            }
        }

        protected Func<TApplicationException,ExceptionType> CreateGetExceptionTypeFunc()
        {
            FieldInfo fieldInfo = typeof(TApplicationException).GetField("type",BindingFlags.Instance | BindingFlags.GetField | BindingFlags.NonPublic);
            ParameterExpression parameterExpression = Expression.Parameter(typeof(TApplicationException), "applicationException");
            Expression returnExpression = Expression.Field(parameterExpression, fieldInfo);

            return Expression.Lambda<Func<TApplicationException, ExceptionType>>(returnExpression, parameterExpression).Compile();
        }

        public T HandException<T>(ExceptionContext context)
        {
            if (context.Exception is TApplicationException applicationException && ExceptionTypeGetter(applicationException) == ExceptionType.MissingResult)
            {
                context.HandException = true;
                return TypeHelper.GetDefalutValue<T>();
            }
            ExceptionDispatchInfo dispatch = ExceptionDispatchInfo.Capture(context.Exception);
            dispatch.Throw();

            return default(T);
        }
    }
}