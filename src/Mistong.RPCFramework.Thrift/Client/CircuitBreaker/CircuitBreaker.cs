using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.CircuitBreaker
{
    public class CircuitBreaker
    {
        private CircuitBreakerState _openState;
        private CircuitBreakerState _closeState;
        private CircuitBreakerState _halfOpenState;
        internal CircuitBreakerState _currentState;
        private object _lockObject;
        private CircuitBreakerSetting _setting;

        public CircuitBreaker(CircuitBreakerSetting setting)
        {
            CheckSetting(setting);
            _setting = setting;
            _closeState = new CloseState(this, new Tuple<TimeSpan, int>(_setting.AllowFailInterval, _setting.AllowFailTimes));
            _openState = new OpenState(this, _setting.HalfOpenDuration);
            _halfOpenState = new HalfOpenState(this, _setting.HalfOpenRequestLimit);
            _lockObject = new object();
            MoveToCloseState();
        }

        private void CheckSetting(CircuitBreakerSetting setting)
        {
            if (setting == null)
                throw new ArgumentNullException(nameof(setting));
            if (setting.ProtectAction == null)
                throw new NullReferenceException("ProtectAction不能为null");
            if (setting.AllowFailInterval <= TimeSpan.Zero)
                throw new ArgumentException("允许错误次数的时间间隔不能小于0");
            if (setting.AllowFailTimes <= 0)
                throw new ArgumentException("在间隔时间内允许错误的次数不能小于0");
            if (setting.HalfOpenDuration <= TimeSpan.Zero)
                throw new ArgumentException("打开状态下切换到半开状态的时间间隔不能小于0");
            if (setting.HalfOpenRequestLimit <= 0)
                throw new ArgumentException("半开状态下允许的请求次数不能小于0");
        }

        public void Execute()
        {
            try
            {
                lock (_lockObject)
                {
                    _currentState.ProcessBefore();
                }
                _setting.ProtectAction();
                lock (_lockObject)
                {
                    _currentState.ProcessSuccess();
                }
            }
            catch (RemoteResourceException err)
            {
                lock (_lockObject)
                {
                    _currentState.ProcessFail();
                }
                if (_setting.ExceptionProcess == null)
                {
                    throw err;
                }
                else
                {
                    _setting.ExceptionProcess(err);
                }
            }
        }

        public void MoveToCloseState()
        {
            _currentState?.Clear();
            _currentState = _closeState;
            _currentState.Initialize();
        }

        public void MoveToOpenState()
        {
            _currentState?.Clear();
            _currentState = _openState;
            _currentState.Initialize();
        }

        public void MoveToHalfOpenState()
        {
            _currentState?.Clear();
            _currentState = _halfOpenState;
            _currentState.Initialize();
        }
    }
}
