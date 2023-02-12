using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///线程交叉访问助手
///需要放在物体上，不能用单例创建
///</summary>
namespace Common
{
    public class ThreadCrossHelper : MonoSingleton<ThreadCrossHelper>
    {
        class DelayItem
        {
            //存贮要执行的方法
            public Action Action { get; set; }
            //存贮要延迟的时间
            public DateTime Time { get; set; }
        }

        private List<DelayItem> ActionList;
        protected override void Init()
        {
            base.Init();
            ActionList = new List<DelayItem>();
        }

        private void Update()
        {
            //避免在增加的时候删除输出
            lock (ActionList)
            {
                for (int i = ActionList.Count - 1; i >= 0; i--)
                {
                    if (ActionList[i].Time <= DateTime.Now)
                    {
                        ActionList[i].Action();
                        ActionList.RemoveAt(i);
                    }
                }
            }

        }

        /// <summary>
        /// 需要在主线程中执行的方法
        /// </summary>
        /// <param name="action">方法</param>
        /// <param name="delay">方法调用延迟时间</param>
        public void ExecuteOnMainThread(Action action, float delay = 0)
        {
            lock (ActionList)
            {
                var item = new DelayItem()
                {
                    Action = action,
                    Time = DateTime.Now.AddSeconds(delay)
                };
                ActionList.Add(item);
            }

        }
    }
}
