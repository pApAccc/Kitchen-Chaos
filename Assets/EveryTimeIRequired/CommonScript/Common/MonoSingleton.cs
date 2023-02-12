using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

///<summary>
///单例设计模式
///</summary>
namespace Common
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                    if (instance == null)
                    {
                        new GameObject("Singleton of " + typeof(T).Name).AddComponent<T>();
                    }
                    else
                    {
                        instance.Init();
                    }
                }
                return instance;
            }
        }

        protected void Awake()
        {
            //防止游戏重启后,因调用DontDestroyOnLoad出现两个单例
            int countOfInstance = FindObjectsOfType<T>().Length;
            if (countOfInstance > 1)
            {
                Destroy(gameObject);
            }

            //调用get方法
            if (Instance == null)
            {

            }
        }

        protected virtual void Init()
        {

        }
    }
}


