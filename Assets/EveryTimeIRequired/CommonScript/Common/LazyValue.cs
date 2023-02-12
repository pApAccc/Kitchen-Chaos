using System;

/// <summary>
/// 解决竞争的懒加载
/// 防止在Start中调用另一个在Start中初始化的值，导致寻找到null，0等不正确的值
/// 在Awake中进行懒加载，保证如果出现竞争，Start中也有值
/// 为了避免不必要的问题，建议在Awake中进行懒加载，Start中调用ForceInit()
/// <summary>
namespace Common
{
    public class LazyValue<T>
    {
        private T _value;
        private bool _initialized = false;

        public Func<T> _initializer;

        /// <summary>
        /// 接受泛型返回值的回调构造函数
        /// </summary>
        /// <param name="initializer">返回值为T的无参函数</param>
        public LazyValue(Func<T> initializer)
        {
            _initializer = initializer;
        }

        public T value
        {
            get
            {
                //使用get方法进行赋值，如果没有初始化，则会调用回调函数进行初始化
                ForceInit();
                return _value;
            }
            set
            {
                //赋值，同时保证不再调用回调
                _initialized = true;
                _value = value;
            }
        }

        /// <summary>
        /// 强制进行赋值
        /// </summary>
        public void ForceInit()
        {
            if (!_initialized)
            {
                _value = _initializer();
                _initialized = true;
            }
        }
    }
}